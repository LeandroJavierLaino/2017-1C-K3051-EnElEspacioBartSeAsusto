using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;
using TGC.Core.BoundingVolumes;
using Microsoft.DirectX;
using TGC.Collision;
using TGC.Core.Collision;
namespace TGC.Group.Model
{
    class Monstruo
    {
        private const float velocidad = 90f;


		//se usa para rotar el mesh lentamente hasta un objetivo determinado sin pasarse
		private float Approach(float val, float tar, float amo) {
			//distancia entre el valor y el target
			float dist = tar - val;
			//si el amount para aproximar es positivo (acercamiento) y es mayor que la distancia que se quiere aproximar se reemplaza por la distancia 
			if (amo > 0f && FastMath.Abs(dist) < amo)
			{
				amo = dist;
			}
			else if (dist < 0f) amo *= -1f;
			
			return val + amo;
		}

		private float AngleDistance(float a, float b) {
			//float c = b - a + FastMath.PI;
			//float mod = c - FastMath.Floor(c / FastMath.PI_HALF) * FastMath.PI - FastMath.PI_HALF;
			//return mod;
			return FastMath.Atan2(FastMath.Sin(b - a), FastMath.Cos(b - a));
		}

		private float rotation_speed;

		//Current target position
		private Vector3 target;

		//booleano que indica si el monstruo actualmente esta persiguiendo el jugador o esta de patrulla
		bool chasingPlayer;
        //Manager de colisiones
        private MonsterCollider collisionManager;

        //Esfera para detectar colisiones del monstruo
        private Core.BoundingVolumes.TgcBoundingSphere sphere { get; set; }

        //Si esta variable es falsa el monstruo no tiene comportamiento y no se dibuja
        private bool activo = false;
        public bool Activo { get { return activo; } set { activo = value; } }

        //Si esta variable es falsa el monstruo no colisiona con objetos
        private bool colisiones = true;
        public bool Colisiones { get { return colisiones; } set { colisiones = value; } }

        //Mesh del monstruo
        public TgcMesh mesh { get; set; }
        
        //Posicion
        public Vector3 Position { get { return mesh.Position; } set { mesh.Position = value; this.sphere.setCenter(value); } }

		public Vector2 LookAt { get { return new Vector2(FastMath.Sin(mesh.Rotation.Y - FastMath.PI), FastMath.Cos(mesh.Rotation.Y - FastMath.PI)); } }

        //Nodos de recorrido
        private List<Vector3> recorrido;
		private int nextNode;

		//rango de distancia en el que detecta al jugador independientemente de su posicion relativa al rango de vision
		private float rangoInmediatoSq;

		/*
		 * valor real entre -1 y 1 que representa el rango de visión del monstruo
		 * 1 significa un radio de 0 grados (sólo ve lo que tiene justo en frente, perfectamente alineado con el ángulo en que está encarado el monstruo).
		 * 0 significa un radio de 180 grados
		 * -1 significa un radio de 360 grados (ve en toda dirección)
		 */
		private float rangoVision;

        //Si la camara colisiona con un trigger el monstruo aparece en el spawnpoint de igual indice
        /*
        private List<Core.BoundingVolumes.TgcBoundingSphere> triggers = new List<TgcBoundingSphere>();
        private List<Vector3> spawnPoints = new List<Vector3>();
        private int lastTrigger = -1;
        void checkTriggers(Vector3 cameraPos) {

            //chequeamos cada trigger
            for (int i = 0; i < triggers.Count; i++) {
                //si el trigger contiene la posicion de la camara
                if (i != lastTrigger && Core.Collision.TgcCollisionUtils.testPointSphere(triggers[i], cameraPos)){
                    //se mueve al monstruo a la zona de spawn
                    this.mesh.Position = spawnPoints[i];
                    //Se actualiza el ultimo trigger activado
                    if(lastTrigger >= 0)triggers[lastTrigger].setRenderColor(System.Drawing.Color.Yellow);
                    lastTrigger = i;
                    triggers[i].setRenderColor(System.Drawing.Color.Red);
                    //Se activa al monstruo
                    activo = true;
                    break;
                }
            
            }
        }
        */
        public void move_ignore_collisions(Vector3 Movement) {
            Position = Position + Movement;
        }
        

        public void move(Vector3 Movement, List<TgcBoundingAxisAlignBox> obstacles) {
            if (this.colisiones) {
                Movement = collisionManager.moveCharacter(this.sphere, Movement, obstacles);
            }
            move_ignore_collisions(Movement);
        }

        public void Render() {
            //if (activo)
            //{
                mesh.UpdateMeshTransform();
                mesh.render();
                //sphere.render();
                //mesh.BoundingBox.render();
            //    foreach (var trigger in triggers) { trigger.render(); }
            //}
        }

        
        //La lista de triggers y la de spawnPoints deben ser del mismo tamaño
        public void Init(TgcMesh mesh,/*Vector3 startPos, List<Core.BoundingVolumes.TgcBoundingSphere> triggers, List<Vector3> spawnPoints,*/ List<Vector3> recorrido) {
            this.mesh = mesh;
			this.rangoInmediatoSq = 1000.0f;
            sphere = Core.BoundingVolumes.TgcBoundingSphere.computeFromMesh(mesh);
			sphere.setValues(sphere.Center, 20);
			Position = recorrido[0];
            //this.triggers = triggers;
            //this.spawnPoints = spawnPoints;
            this.recorrido = recorrido;
			nextNode = 1;
			chasingPlayer = false;
			rotation_speed = 0.1f;

			//1    -> 0
			//0.5  -> 60
			//0    -> 180
			//-0.5 -> 300
			//-1   -> 360
			rangoVision = 0.2f;

			target = recorrido[nextNode];
			collisionManager = new MonsterCollider();

        }
        public bool MoveTowards(Vector3 targetPos, List<Core.BoundingVolumes.TgcBoundingAxisAlignBox> obstaculos, float ElapsedTime) {
            var targetDistance = targetPos - Position;
			bool ret = false;
            //El monstruo solo se mueve en el plano XZ
            targetDistance.Y = 0f;

            //Normalizar distancia para obtener versor direccion
            var targetDirection = Vector3.Normalize(targetDistance);

            //Obtener movimiento
            var movement = targetDirection * velocidad * ElapsedTime;

            //Si el movimiento es mayor que la distancia al objetivo lo reemplazamos por la misma para no pasarnos
            if(movement.LengthSq() >= targetDistance.LengthSq())
            {
                movement = targetDistance;
				ret = true;
            }

            //Se obtiene el angulo de rotacion horizontal
            var targetAngleH = FastMath.Atan2(targetDirection.X, targetDirection.Z);

            //Se obtiene el angulo de rotacion vertical a partir de la altura del versor director(ya no es relevante)
            //var targetAngleV = FastMath.Asin(targetDirection.Y);

            var originalRot = mesh.Rotation;

            var originalPos = Position;
			if (chasingPlayer)
			{
				//Rotamos el mesh, se suma PI para que de la cara y no la espalda
				mesh.Rotation = new Vector3(0, targetAngleH + FastMath.PI, 0);
				move(movement, obstaculos);
			}
			else {
				//si tamos tranca rotamos lentamente a la dirección a la que queremos ir

				//sacamos la distancia entre angulos
				float distancia = AngleDistance(mesh.Rotation.Y,targetAngleH + FastMath.PI);
				mesh.Rotation = new Vector3(0f, Approach(mesh.Rotation.Y, mesh.Rotation.Y+distancia, rotation_speed),0f);

				//si ya estamos como queremos nos movemos
				if (FastMath.EpsilonEquals(AngleDistance(mesh.Rotation.Y, targetAngleH + FastMath.PI),0f)) {
					move(movement, obstaculos);
				}
			}
			return ret;
        }
        public void Update(Vector3 targetPos, List<Core.BoundingVolumes.TgcBoundingAxisAlignBox> obstaculos, float ElapsedTime) {
			if (!activo) return;
			var Distance = targetPos - Position;

			//get LookAt Vector del Monstruo

			var LookAt = this.LookAt;
			var DistanceDir2D = new Vector2(Distance.X, Distance.Z);
			DistanceDir2D.Normalize();
			//chequear si el jugador está en el rango de visión
			bool target_visible = Distance.LengthSq() < rangoInmediatoSq;

			if (!target_visible && Vector2.Dot(LookAt, DistanceDir2D) >= rangoVision) {
				target_visible = true;
				//chequear si nada se interpone entre el jugador y el monstruo
				Core.Geometry.TgcRay ray = new Core.Geometry.TgcRay(Position, Distance);
				foreach (var mesh in obstaculos) {
					if (TgcCollisionUtils.intersectRayAABB(ray, mesh, out Vector3 interseccion)) {
						if ((interseccion - Position).LengthSq() < Distance.LengthSq()) {
							//si hay algun objeto en el medio el target no es visible y dejamos de queryar
							target_visible = false;
							break;
						};
					};
				}
			}
			//SI LO VEO LO HAGO PELOTAAA
			if (target_visible) { target = targetPos; chasingPlayer = true; }
			
			if (MoveTowards(target, obstaculos, ElapsedTime) && !chasingPlayer)
			{
				//si llegue al proximo punto cambio de punto
				++nextNode;
				nextNode %= recorrido.Count();
				target = recorrido[nextNode];
				
			}
			//si estoy persiguiendo al jugador pero ya no lo veo y estoy cerca del ultimo punto donde lo vi
			if (chasingPlayer && !target_visible && Core.Collision.TgcCollisionUtils.testPointSphere(new TgcBoundingSphere(sphere.Center,sphere.Radius + 20), target))
			{
				//dejo de perseguir y regreso a mi recorrido
				chasingPlayer = false;
				Position = recorrido[nextNode];
				target = recorrido[nextNode];
			}


			//rotar para apuntar al proximo punto

			//checkTriggers(targetPos);

		}
    }
}
