using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;
using TGC.Core.BoundingVolumes;
using Microsoft.DirectX;
using TGC.Examples.Collision.SphereCollision;
namespace TGC.Group.Model
{
    class Monstruo
    {
        private const float velocidad = 90f;

        //Manager de colisiones
        private SphereCollisionManager collisionManager;

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


        //Si la camara colisiona con un trigger el monstruo aparece en el spawnpoint de igual indice
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

        public void move_ignore_collisions(Vector3 Movement) {
            Position = Position + Movement;
        }
        

        public void move(Vector3 Movement, List<TgcBoundingAxisAlignBox> obstacles) {
            if (this.colisiones) {
                Movement = collisionManager.moveCharacter(this.sphere, Movement, obstacles);
            }
            move_ignore_collisions(Movement);
        }

        public void render() {
            if (activo)
            {
                mesh.UpdateMeshTransform();
                mesh.render();
                sphere.render();
                mesh.BoundingBox.render();
                foreach (var trigger in triggers) { trigger.render(); }
            }
        }

        //La lista de triggers y la de spawnPoints deben ser del mismo tamaño
        public void init(TgcMesh mesh,Vector3 startPos, List<Core.BoundingVolumes.TgcBoundingSphere> triggers, List<Vector3> spawnPoints) {
            this.mesh = mesh;
            sphere = Core.BoundingVolumes.TgcBoundingSphere.computeFromMesh(mesh);

            Position = startPos;
            this.triggers = triggers;
            this.spawnPoints = spawnPoints;
            collisionManager = new SphereCollisionManager();

        }

        public void update(Vector3 targetPos, List<Core.BoundingVolumes.TgcBoundingAxisAlignBox> obstaculos, float ElapsedTime) {



            checkTriggers(targetPos);

            if (!activo) return;
            var targetDistance = targetPos - Position;
            
            //El monstruo solo se mueve en el plano XZ
            targetDistance.Y = 0f;

            //Normalizar distancia para obtener versor direccion
            var targetDirection = Vector3.Normalize(targetDistance);
            
            //Obtener movimiento
            var movement = targetDirection * velocidad * ElapsedTime;
            
            //Se obtiene el angulo de rotacion horizontal
            var targetAngleH = FastMath.Atan2(targetDirection.X, targetDirection.Z);

            //Se obtiene el angulo de rotacion vertical a partir de la altura del versor director(ya no es relevante)
            //var targetAngleV = FastMath.Asin(targetDirection.Y);
            
            var originalRot = mesh.Rotation;

            var originalPos = Position;

            //Rotamos el mesh
            mesh.Rotation = new Vector3(0, targetAngleH + FastMath.PI, 0);

            move(movement, obstaculos);

        }
    }
}
