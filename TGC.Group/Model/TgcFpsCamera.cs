using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Utils;
using TGC.Examples.Collision.SphereCollision;
using TGC.Core.Collision;

namespace TGC.Examples.Camara
{
    /// <summary>
    ///     Camara en primera persona que utiliza matrices de rotacion, solo almacena las rotaciones en updown y costados.
    ///     Ref: http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series4/Mouse_camera.php
    ///     Autor: Rodrigo Garcia.
    /// </summary>
    public class TgcFpsCamera : TgcCamera
    {
        Vector3 newPosition = new Vector3(0, 0, 0);
        private readonly Point mouseCenter; //Centro de mause 2D para ocultarlo.

        //Se mantiene la matriz rotacion para no hacer este calculo cada vez.
        private Matrix cameraRotation;

        //Direction view se calcula a partir de donde se quiere ver con la camara inicialmente. por defecto se ve en -Z.
        private Vector3 directionView;        

        //No hace falta la base ya que siempre es la misma, la base se arma segun las rotaciones de esto costados y updown.
        private float leftrightRot;
        private float updownRot;

        private bool lockCam;
        private Vector3 positionEye;

        private bool collitionActive = true;

        //Manager de colisiones
        private TGC.Examples.Collision.SphereCollision.SphereCollisionManager collisionManagerCamara;

        //Esfera para detectar colisiones 
        public Core.BoundingVolumes.TgcBoundingSphere sphereCamara { get; set; }

        private Core.Geometry.TgcBox cajaLoca = new Core.Geometry.TgcBox();

        public Vector3 getNewPosition()
        {
            return newPosition;
        }

        public TgcFpsCamera(TgcD3dInput input)
        {
            Input = input;
            positionEye = new Vector3();
            mouseCenter = new Point(
                D3DDevice.Instance.Device.Viewport.Width / 2,
                D3DDevice.Instance.Device.Viewport.Height / 2);
            RotationSpeed = 0.1f;
            MovementSpeed = 500f;
            JumpSpeed = 500f;
            directionView = new Vector3(0, 0, -1);
            leftrightRot = FastMath.PI_HALF;
            updownRot = -FastMath.PI / 10.0f;
            cameraRotation = Matrix.RotationX(updownRot) * Matrix.RotationY(leftrightRot);
        }

        public TgcFpsCamera(Vector3 positionEye, TgcD3dInput input) : this(input)
        {
            this.positionEye = positionEye;
        }

        public TgcFpsCamera(Vector3 positionEye, float moveSpeed, float jumpSpeed, TgcD3dInput input)
            : this(positionEye, input)
        {
            float x = positionEye.X;
            float y = positionEye.Y;
            float z = positionEye.Z;
            MovementSpeed = moveSpeed;
            JumpSpeed = jumpSpeed;
            cajaLoca.setPositionSize(new Vector3(x, y - 35f, z), new Vector3(5,5,5));
            Core.SceneLoader.TgcMesh laCajaLoca = cajaLoca.toMesh("laCajaLoca");
            sphereCamara = Core.BoundingVolumes.TgcBoundingSphere.computeFromMesh(laCajaLoca);
            sphereCamara.setValues(new Vector3(x, y - 35f, z),15f);
            lockCam = true;
            collisionManagerCamara = new TGC.Examples.Collision.SphereCollision.SphereCollisionManager();
            collisionManagerCamara.toggleGravity();
        }

        public TgcFpsCamera(Vector3 positionEye, float moveSpeed, float jumpSpeed, float rotationSpeed,
            TgcD3dInput input)
            : this(positionEye, moveSpeed, jumpSpeed, input)
        {
            RotationSpeed = rotationSpeed;
        }

        private TgcD3dInput Input { get; }

        public bool LockCam
        {
            get { return lockCam; }
            set
            {
                if (!lockCam && value)
                {
                    Cursor.Position = mouseCenter;

                    Cursor.Hide();
                }
                if (lockCam && !value)
                    Cursor.Show();
                lockCam = value;
            }
        }

        public float MovementSpeed { get; set; }

        public float RotationSpeed { get; set; }

        public float JumpSpeed { get; set; }

        /// <summary>
        ///     Cuando se elimina esto hay que desbloquear la camera.
        /// </summary>
        ~TgcFpsCamera()
        {
            LockCam = false;
        }

        public void UpdateCamera(float elapsedTime, List<Core.BoundingVolumes.TgcBoundingAxisAlignBox> obstaculos, float vidaPorcentaje,float staminaPorcentaje)
        {
            //Para el menu deberia ser Cursor.Show(); sino no ves donde haces click :P
            Cursor.Hide();
            var moveVector = new Vector3(0, 0, 0);
            Vector3 targetDistance = new Vector3(0, 0, 0);
            sphereCamara.setCenter(new Vector3(Position.X, Position.Y -30f, Position.Z));

            #region Movimientos
            //Forward
            if (Input.keyDown(Key.W) && vidaPorcentaje > 0 )
            {
                targetDistance += (new Vector3(LookAt.X, 0, LookAt.Z) - new Vector3(Position.X, 0, Position.Z)) * MovementSpeed;
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    moveVector += new Vector3(0, 0, -newPosition.Length());
                }
                else
                {
                    moveVector += new Vector3(0, 0, -1) * MovementSpeed;
                }
            }

            //Backward
            if (Input.keyDown(Key.S) && vidaPorcentaje > 0)
            {
                targetDistance += (new Vector3(LookAt.X, 0, LookAt.Z) - new Vector3(Position.X, 0, Position.Z)) * MovementSpeed;
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, -targetDistance, obstaculos);
                    moveVector += new Vector3(0,0, newPosition.Length());
                }
                else
                {
                    moveVector += new Vector3(0, 0, 1) * MovementSpeed;
                }
            }

            //Strafe right
            if (Input.keyDown(Key.D) && vidaPorcentaje > 0)
            {
                //Hay que ver si se puede obtener un vector ortogonal al targetDistance de W o S
                targetDistance += new Vector3(-1, 0, 0) * MovementSpeed;
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    moveVector += newPosition;
                }
                else
                {
                    moveVector += new Vector3(-1, 0, 0) * MovementSpeed;
                }
            }

            //Strafe left
            if (Input.keyDown(Key.A) && vidaPorcentaje > 0)
            {
                targetDistance += new Vector3(1, 0, 0) * MovementSpeed;
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    moveVector += newPosition;
                }
                else
                {
                    moveVector += new Vector3(1, 0, 0) * MovementSpeed;
                }
            }
            #endregion
            
            #region Modificadores de movimiento
            //Jump
            if (Input.keyDown(Key.Space))
            {
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    moveVector += new Vector3(0, 1, 0) * JumpSpeed;
                }
                else
                {
                    moveVector += new Vector3(0, 1, 0) * JumpSpeed;
                }               
            }

            //Crouch
            if (Input.keyDown(Key.LeftControl))
            {
                moveVector += new Vector3(0, -1, 0) * JumpSpeed;
            }

            if (Input.keyPressed(Key.L) || Input.keyPressed(Key.Escape))
            {
                collitionActive = !collitionActive;
                //LockCam = !lockCam;
            }

            if (Input.keyDown(Key.LeftShift) && staminaPorcentaje > 0)
            {
                MovementSpeed = 200f;
            }
            else
            {
                MovementSpeed = 100f;
            }
#endregion

            //Solo rotar si se esta aprentando el boton izq del mouse
            if (lockCam )
            {
                leftrightRot -= -Input.XposRelative * RotationSpeed;
                updownRot -= Input.YposRelative * RotationSpeed;
                //Se actualiza matrix de rotacion, para no hacer este calculo cada vez y solo cuando en verdad es necesario.
                cameraRotation = Matrix.RotationY(leftrightRot);
            }

            if (lockCam)
                Cursor.Position = mouseCenter;

            //Calculamos la nueva posicion del ojo segun la rotacion actual de la camara.
            var cameraRotatedPositionEye = Vector3.TransformNormal(moveVector * elapsedTime, cameraRotation);
            positionEye += cameraRotatedPositionEye;

            cameraRotation = Matrix.RotationX(updownRot) * Matrix.RotationY(leftrightRot);
            cameraRotatedPositionEye = Vector3.TransformNormal(moveVector * elapsedTime, cameraRotation);

            //Calculamos el target de la camara, segun su direccion inicial y las rotaciones en screen space x,y.

            var cameraRotatedTarget = Vector3.TransformNormal( directionView, cameraRotation);
            var cameraFinalTarget = positionEye + cameraRotatedTarget;
           
            var cameraOriginalUpVector = DEFAULT_UP_VECTOR;
            var cameraRotatedUpVector = Vector3.TransformNormal(cameraOriginalUpVector, cameraRotation);

            base.SetCamera(positionEye, cameraFinalTarget, cameraRotatedUpVector);
            
        }

        /// <summary>
        ///     se hace override para actualizar las posiones internas, estas seran utilizadas en el proximo update.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="directionView"> debe ser normalizado.</param>
        public override void SetCamera(Vector3 position, Vector3 directionView)
        {
            positionEye = position;
            this.directionView = directionView;
        }
        public void render(float elapsedTime, List<Core.BoundingVolumes.TgcBoundingAxisAlignBox> obstaculos)
        {
            //sphereCamara.setCenter( bounce(sphereCamara.Position, obstaculos));
            
            sphereCamara.render();
        }

        public Vector3 bounce(Vector3 unaPosicion, List<Core.BoundingVolumes.TgcBoundingAxisAlignBox> obstaculos)
        {
            foreach (var mesh in obstaculos)
            {
                //valida si hay colision entre la camara y algun mesh del escenario    
                if (Core.Collision.TgcCollisionUtils.testSphereAABB(sphereCamara, mesh))
                {
                    Vector3 puntoDeColision = Core.Collision.TgcCollisionUtils.closestPointAABB(sphereCamara.Center, mesh);
                    
                    if (sphereCamara.Radius > distancia(puntoDeColision))
                    {
                        //en X
                        if (puntoDeColision.X > sphereCamara.Position.X)
                        {
                            unaPosicion.X -= desplazar(sphereCamara.Radius, puntoDeColision);
                        }

                        if (puntoDeColision.X < sphereCamara.Position.X)
                        {
                            unaPosicion.X += desplazar(sphereCamara.Radius, puntoDeColision);
                        }

                        //en Z

                        if (puntoDeColision.Z > sphereCamara.Position.Z)
                        {
                            unaPosicion.Z -= desplazar(sphereCamara.Radius, puntoDeColision);
                        }

                        if (puntoDeColision.Z < sphereCamara.Position.Z)
                        {
                            unaPosicion.Z += desplazar(sphereCamara.Radius, puntoDeColision);
                        }
                    }
                    else
                    {
                        //en X
                        if (puntoDeColision.X == sphereCamara.Position.X + sphereCamara.Radius)
                        {
                            //en Z

                            if (puntoDeColision.Z > sphereCamara.Position.Z)
                            {
                                unaPosicion.Z -= desplazar(sphereCamara.Radius, puntoDeColision);
                            }

                            if (puntoDeColision.Z < sphereCamara.Position.Z)
                            {
                                unaPosicion.Z += desplazar(sphereCamara.Radius, puntoDeColision);
                            }
                        }

                        //en Z
                        if (puntoDeColision.Z == sphereCamara.Position.Z + sphereCamara.Radius)
                        {
                            //en X
                            if (puntoDeColision.X > sphereCamara.Position.X)
                            {
                                unaPosicion.X -= desplazar(sphereCamara.Radius, puntoDeColision);
                            }

                            if (puntoDeColision.X < sphereCamara.Position.X)
                            {
                                unaPosicion.X += desplazar(sphereCamara.Radius, puntoDeColision);
                            }
                        }
                    }

                    break;
                }
            }
            return unaPosicion;
        }

        public float distancia(Vector3 puntoDeColision)
        {
            return (FastMath.Sqrt(FastMath.Pow2(puntoDeColision.X - sphereCamara.Position.X) + FastMath.Pow2(puntoDeColision.Y - sphereCamara.Position.Y) + FastMath.Pow2(puntoDeColision.Z - sphereCamara.Position.Z)));
        }

        public float desplazar(float radio, Vector3 puntoDeColision)
        {
            return radio - distancia(puntoDeColision);
        }
    }
}