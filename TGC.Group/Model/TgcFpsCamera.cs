﻿using System.Drawing;
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
            Vector3 lastPos = new Vector3( x, y, z);
            Vector3 lastLookAt = LookAt;
            Vector3 lastUpVector = UpVector;
            Vector3 lastPositionSphere = new Vector3 ((float)System.Math.Truncate(sphereCamara.Position.X),(float)System.Math.Truncate(sphereCamara.Position.Y), (float)System.Math.Truncate(sphereCamara.Position.Z));
            Vector3 targetDistance = new Vector3(0, 0, 0);
            Vector3 newPosition = new Vector3(0, 0, 0);

            #region Movimientos
            //Forward
            if (Input.keyDown(Key.W))
            {
                targetDistance += new Vector3(0, 0, -1) * MovementSpeed;
                targetDistance.Normalize();  
                sphereCamara.setCenter(new Vector3(x, y - 35f, z));
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    //newPosition = bounce(newPosition, targetDistance,obstaculos, lastPos);
                    moveVector += newPosition * MovementSpeed;// gravedad(newPosition, 0, 0, 0, elapsedTime) * MovementSpeed;
                }
                else
                {
                    moveVector += new Vector3(0, 0, -1) * MovementSpeed;
                }
            }

            //Backward
            if (Input.keyDown(Key.S))
            {
                targetDistance += new Vector3(0, 0, 1) * MovementSpeed;
                targetDistance.Normalize();
                sphereCamara.setCenter(new Vector3(x, y - 35f, z));
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    //newPosition = bounce(newPosition, targetDistance, obstaculos, lastPos);
                    moveVector += gravedad(newPosition, 0, 0, 0, elapsedTime) * MovementSpeed;
                }
                else
                {
                    moveVector += new Vector3(0, 0, 1) * MovementSpeed;
                }
            }

            //Strafe right
            if (Input.keyDown(Key.D))
            {
                targetDistance += new Vector3(-1, 0, 0) * MovementSpeed;
                targetDistance.Normalize();
                sphereCamara.setCenter(new Vector3(x, y - 35f, z));
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    //newPosition = bounce(newPosition, targetDistance, obstaculos, lastPos);
                    moveVector += gravedad(newPosition, 0, 0, 0, elapsedTime) * MovementSpeed;
                }
                else
                {
                    moveVector += new Vector3(-1, 0, 0) * MovementSpeed;
                }
            }

            //Strafe left
            if (Input.keyDown(Key.A))
            {
                targetDistance += new Vector3(1, 0, 0) * MovementSpeed;
                targetDistance.Normalize();
                sphereCamara.setCenter(new Vector3(x, y - 35f, z));
                if (collitionActive)
                {
                    newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                    //newPosition = bounce(newPosition, targetDistance, obstaculos, lastPos);
                    moveVector += gravedad(newPosition, 0, 0, 0, elapsedTime) * MovementSpeed;
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
                newPosition = collisionManagerCamara.moveCharacter(sphereCamara, targetDistance, obstaculos);
                moveVector += new Vector3(0, 1, 0) * JumpSpeed + gravedad(newPosition,0,0,0,elapsedTime) * MovementSpeed;
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

            if (Input.keyDown(Key.LeftShift))
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

        public Vector3 bounce(Vector3 unaPosicion, Vector3 targetDistance, List<Core.BoundingVolumes.TgcBoundingAxisAlignBox> obstaculos, Vector3 lastPos)
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

        public Vector3 gravedad(Vector3 unaPosicion, float velocidadX, float velocidadY, float velocidadZ, float elasedTime)
        {
            unaPosicion += new Vector3(velocidadX*elasedTime,velocidadY*elasedTime + gravity * FastMath.Pow2(elasedTime),velocidadZ*elasedTime);

            return unaPosicion;
        }
    }
}