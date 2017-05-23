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

        //Nodos de recorrido
        private List<Vector3> recorrido;
		private int nextNode;

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
            if (activo)
            {
                mesh.UpdateMeshTransform();
                mesh.render();
                //sphere.render();
                //mesh.BoundingBox.render();
            //    foreach (var trigger in triggers) { trigger.render(); }
            }
        }

        
        //La lista de triggers y la de spawnPoints deben ser del mismo tamaño
        public void Init(TgcMesh mesh,/*Vector3 startPos, List<Core.BoundingVolumes.TgcBoundingSphere> triggers, List<Vector3> spawnPoints,*/ List<Vector3> recorrido) {
            this.mesh = mesh;
            sphere = Core.BoundingVolumes.TgcBoundingSphere.computeFromMesh(mesh);
			sphere.setValues(sphere.Center, 20);
			Position = recorrido[0];
            //this.triggers = triggers;
            //this.spawnPoints = spawnPoints;
            this.recorrido = recorrido;
			nextNode = 1;
			chasingPlayer = false;

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

            //Si el movimiento es mayor que la distancia al objetivo lo reemplazamos por la misma
            if(movement.LengthSq() > targetDistance.LengthSq())
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
            
            //Rotamos el mesh
            mesh.Rotation = new Vector3(0, targetAngleH + FastMath.PI, 0);

			move(movement, obstaculos);
			return ret;
        }
        public void Update(Vector3 targetPos, List<Core.BoundingVolumes.TgcBoundingAxisAlignBox> obstaculos, float ElapsedTime) {
			if (!activo) return;
			//chequear si veo al jugador
			
			var Distance = targetPos - Position;
			Core.Geometry.TgcRay ray = new Core.Geometry.TgcRay(Position, Distance);
			bool target_visible = true;
			foreach (var mesh in obstaculos) {

				if (TgcCollisionUtils.intersectRayAABB(ray, mesh, out Vector3 interseccion)) {
					if ((interseccion-Position).LengthSq() < Distance.LengthSq()) {
						target_visible = false;
					};
				};
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
			if (chasingPlayer == true && !target_visible && Core.Collision.TgcCollisionUtils.testPointSphere(new TgcBoundingSphere(sphere.Center,sphere.Radius+20), target))
			{
				//dejo de perseguir y regreso a mi recorrido
				chasingPlayer = false;
				Position = recorrido[nextNode];
			}


			//rotar para apuntar al proximo punto

			//checkTriggers(targetPos);

		}
    }
}
