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

        private float gravity; 

        //Manager de colisiones
        private SphereCollisionManager collisionManagerCamara;

        //Esfera para detectar colisiones 
        public Core.BoundingVolumes.TgcBoundingSphere sphereCamara { get; set; }

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
            sphereCamara = new Core.BoundingVolumes.TgcBoundingSphere(new Vector3(x,y - 35f,z),15f);
            lockCam = true;
            collisionManagerCamara = new Collision.SphereCollision.SphereCollisionManager();
            collisionManagerCamara.SlideFactor = 15f;
            gravity = -10f;
            
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

        public void UpdateCamera(float elapsedTime, List<Core.BoundingVolumes.TgcBoundingAxisAlignBox> obstaculos)
        {
            var moveVector = new Vector3(0, 0, 0);

            float x = Position.X;
            float y = Position.Y;
            float z = Position.Z;
            Vector3 lastPos = new Vector3((float)System.Math.Truncate( x), (float)System.Math.Truncate(y), (float)System.Math.Truncate(z));
            Vector3 lastLookAt = LookAt;
            Vector3 lastUpVector = UpVector;
            Vector3 lastPositionSphere = new Vector3 ((float)System.Math.Truncate(sphereCamara.Position.X),(float)System.Math.Truncate(sphereCamara.Position.Y), (float)System.Math.Truncate(sphereCamara.Position.Z));
            //sphereCamara.setValues(new Vector3(x, y - 35f, z), 15f);

            //Forward
            if (Input.keyDown(Key.W))
            {
                Vector3 targetDistance = new Vector3(0,0,0);
                Vector3 newPosition = new Vector3(0, 0, 0);
                targetDistance += new Vector3(0, 0, -1) * MovementSpeed;
                targetDistance.Normalize();  
                sphereCamara.setCenter(new Vector3(x, y - 35f, z));
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    newPosition = bounce(newPosition,obstaculos);
                    moveVector += newPosition * MovementSpeed;
                    //moveVector += new Vector3(0, 0, -1) * MovementSpeed;
                }
                else
                {
                    moveVector += new Vector3(0, 0, -1) * MovementSpeed;
                }
            }

            //Backward
            if (Input.keyDown(Key.S))
            {
                Vector3 targetDistance = new Vector3(0, 0, 0);
                Vector3 newPosition = new Vector3(0, 0, 0);
                targetDistance += new Vector3(0, 0, 1) * MovementSpeed;
                targetDistance.Normalize();
                sphereCamara.setCenter(new Vector3(x, y - 35f, z));
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    moveVector += newPosition * MovementSpeed;
                    //moveVector += new Vector3(0, 0, 1) * MovementSpeed;
                }
                else
                {
                    moveVector += new Vector3(0, 0, 1) * MovementSpeed;
                }
            }

            //Strafe right
            if (Input.keyDown(Key.D))
            {
                Vector3 targetDistance = new Vector3(0, 0, 0);
                Vector3 newPosition = new Vector3(0, 0, 0);
                targetDistance += new Vector3(-1, 0, 0) * MovementSpeed;
                targetDistance.Normalize();
                sphereCamara.setCenter(new Vector3(x, y - 35f, z));
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    moveVector += newPosition * MovementSpeed;
                    //moveVector += new Vector3(-1, 0, 0) * MovementSpeed;
                }
                else
                {
                    moveVector += new Vector3(-1, 0, 0) * MovementSpeed;
                }
            }

            //Strafe left
            if (Input.keyDown(Key.A))
            {
                Vector3 targetDistance = new Vector3(0, 0, 0);
                Vector3 newPosition = new Vector3(0, 0, 0);
                targetDistance += new Vector3(1, 0, 0) * MovementSpeed;
                targetDistance.Normalize();
                sphereCamara.setCenter(new Vector3(x, y - 35f, z));
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    moveVector += newPosition * MovementSpeed;
                    //moveVector += new Vector3(1, 0, 0) * MovementSpeed;
                }
                else
                {
                    moveVector += new Vector3(1, 0, 0) * MovementSpeed;
                }
            }

            //Jump
            if (Input.keyDown(Key.Space))
            {
                moveVector += (new Vector3(0, 1, 0) * JumpSpeed) + new Vector3(0, gravity, 0) * elapsedTime * elapsedTime;
            }/*
            else
            {
                //La condicion real seria si no esta colisionando con el suelo hacer esto
                if (System.Math.Truncate( Position.Y + 50) >= 0)
                {
                    moveVector += new Vector3(0, gravity, 0) * elapsedTime * elapsedTime;
                }
            }*/

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

            if (Input.keyDown(Key.LeftShift))
            {
                MovementSpeed = 200f;
            }
            else
            {
                MovementSpeed = 100f;
            }

            //Solo rotar si se esta aprentando el boton izq del mouse
            if (lockCam )
            {
                leftrightRot -= -Input.XposRelative * RotationSpeed;
                updownRot -= Input.YposRelative * RotationSpeed;
                //Se actualiza matrix de rotacion, para no hacer este calculo cada vez y solo cuando en verdad es necesario.
                cameraRotation = Matrix.RotationX(updownRot) * Matrix.RotationY(leftrightRot);
            }

            if (lockCam)
                Cursor.Position = mouseCenter;

            //Calculamos la nueva posicion del ojo segun la rotacion actual de la camara.
            var cameraRotatedPositionEye = Vector3.TransformNormal(moveVector * elapsedTime, cameraRotation);
            positionEye += cameraRotatedPositionEye;

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
        public void render()
        {
            sphereCamara.render();
        }

        public Vector3 bounce(Vector3 unaPosicion, List<Core.BoundingVolumes.TgcBoundingAxisAlignBox> obstaculos)
        {
            foreach (var mesh in obstaculos)
            {
                //valida si hay colision entre la camara y algun mesh del escenario    
                if (Core.Collision.TgcCollisionUtils.testSphereAABB(sphereCamara, mesh))
                {
                    //isColliding = !isColliding;

                    //TODO armar toda la jerarquia de colisiones.
                    Vector3 puntoDeColision = Core.Collision.TgcCollisionUtils.closestPointAABB(sphereCamara.Center, mesh);
                    //en X

                    if (puntoDeColision.X > sphereCamara.Position.X)
                    {
                        unaPosicion.X -= (float)(sphereCamara.Radius - System.Math.Truncate((FastMath.Sqrt(FastMath.Pow2(puntoDeColision.X - sphereCamara.Position.X) + FastMath.Pow2(puntoDeColision.Y - sphereCamara.Position.Y) + FastMath.Pow2(puntoDeColision.Z - sphereCamara.Position.Z)))));
                    }

                    if (puntoDeColision.X < sphereCamara.Position.X)
                    {
                        unaPosicion.X += (float)(sphereCamara.Radius - System.Math.Truncate((FastMath.Sqrt(FastMath.Pow2(puntoDeColision.X - sphereCamara.Position.X) + FastMath.Pow2(puntoDeColision.Y - sphereCamara.Position.Y) + FastMath.Pow2(puntoDeColision.Z - sphereCamara.Position.Z)))));
                    }

                    //en Y
                    if (puntoDeColision.Y > sphereCamara.Position.Y)
                    {
                        unaPosicion.Y -= (float)(sphereCamara.Radius - System.Math.Truncate((FastMath.Sqrt(FastMath.Pow2(puntoDeColision.X - sphereCamara.Position.X) + FastMath.Pow2(puntoDeColision.Y - sphereCamara.Position.Y) + FastMath.Pow2(puntoDeColision.Z - sphereCamara.Position.Z)))));
                    }

                    if (puntoDeColision.Y < sphereCamara.Position.Y)
                    {
                        unaPosicion.Y += (float)(sphereCamara.Radius - System.Math.Truncate((FastMath.Sqrt(FastMath.Pow2(puntoDeColision.X - sphereCamara.Position.X) + FastMath.Pow2(puntoDeColision.Y - sphereCamara.Position.Y) + FastMath.Pow2(puntoDeColision.Z - sphereCamara.Position.Z)))));
                    }

                    //en Z

                    if (puntoDeColision.Z > sphereCamara.Position.Z)
                    {
                        unaPosicion.Z -= (float)(sphereCamara.Radius - System.Math.Truncate((FastMath.Sqrt(FastMath.Pow2(puntoDeColision.X - sphereCamara.Position.X) + FastMath.Pow2(puntoDeColision.Y - sphereCamara.Position.Y) + FastMath.Pow2(puntoDeColision.Z - sphereCamara.Position.Z)))));

                    }

                    if (puntoDeColision.Z < sphereCamara.Position.Z)
                    {
                        unaPosicion.Z += (float)(sphereCamara.Radius - System.Math.Truncate((FastMath.Sqrt(FastMath.Pow2(puntoDeColision.X - sphereCamara.Position.X) + FastMath.Pow2(puntoDeColision.Y - sphereCamara.Position.Y) + FastMath.Pow2(puntoDeColision.Z - sphereCamara.Position.Z)))));

                    }

                    break;
                }
            }
            return unaPosicion;
        }
    }
}