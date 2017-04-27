using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Camara;
using TGC.Core.Input;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Utils;
using System.Collections.Generic;
using TGC.Core.UserControls.Modifier;
using TGC.Core.Shaders;
using TGC.Core.Collision;
using TGC.Examples.Collision.SphereCollision;
using TGC.Core.BoundingVolumes;

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar el modelo que instancia GameForm <see cref="Form.GameForm.InitGraphics()" />
    ///     line 97.
    /// </summary>
    public class GameModel : TgcExample
    {
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        /// <param name="mediaDir">Ruta donde esta la carpeta con los assets</param>
        /// <param name="shadersDir">Ruta donde esta la carpeta con los shaders</param>
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }

        private readonly List<TGC.Core.BoundingVolumes.TgcBoundingAxisAlignBox> objetosColisionables = new List<TGC.Core.BoundingVolumes.TgcBoundingAxisAlignBox>();

        //Caja que se muestra en el ejemplo.
        //usar TgcBox como ejemplo para cargar cualquier caja que queramos.
        private TgcBox Box { get; set; }

        //Escena
        private TgcScene TgcScene { get; set; }

        private TgcMesh PuertaModelo { get; set; }
        private TgcMesh MonstruoModelo { get; set; }

        private TgcMesh Puerta1 { get; set; }
        private TgcMesh Puerta2 { get; set; }
        private TgcMesh Puerta3 { get; set; }
        private TgcMesh Puerta4 { get; set; }
        private TgcMesh Puerta5 { get; set; }
        private TgcMesh Puerta6 { get; set; }
        private TgcMesh Puerta7 { get; set; }
        private TgcMesh Puerta8 { get; set; }
        private TgcMesh Puerta9 { get; set; }
        private TgcMesh Puerta10 { get; set; }
        private TgcMesh Puerta11 { get; set; }
        private TgcMesh Puerta12 { get; set; }
        private TgcMesh Puerta13 { get; set; }
        private TgcMesh Puerta14 { get; set; }
        private TgcMesh Puerta15 { get; set; }
        private TgcMesh Puerta16 { get; set; }
        private TgcMesh Puerta17 { get; set; }
        private TgcMesh Puerta18 { get; set; }
        private TgcMesh Puerta19 { get; set; }
        private TgcMesh Puerta20 { get; set; }
        private TgcMesh Puerta21 { get; set; }
        private TgcMesh Puerta22 { get; set; }
        private TgcMesh Puerta23 { get; set; }
        private TgcMesh Puerta24 { get; set; }
        private TgcMesh Puerta25 { get; set; }
        private TgcMesh Puerta26 { get; set; }
        private TgcMesh Puerta27 { get; set; }
        private TgcMesh Puerta28 { get; set; }

        private TgcMesh Monstruo { get; set; }
        private SphereCollisionManager collisionManager;

        //Variables del Monstruo
        private Core.BoundingVolumes.TgcBoundingSphere monstruoSphere { get; set; }
        private bool monstruoActivo = false;
        //si esta variable es true choca con las paredes
        private bool monstruoSolido = true;
        private const float monstruoVelocidad = 90f;

        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }
        
        private Microsoft.DirectX.Direct3D.Effect Shader { get; set; }
        private TgcBox lightMesh;
        private TgcBox playerPos;

        private Linterna glowstick;
        private Linterna lighter;
        private Linterna flashlight;

        private float timer = 0;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            //FPS Camara Modo Dios 
            //TODO: diseñar camara con colisiones y física.
            Camara = new Examples.Camara.TgcFpsCamera(new Vector3(463, 51, 83), 125f, 100f, Input);
            var d3dDevice = D3DDevice.Instance.Device;

            //Version para cargar escena desde carpeta descomprimida
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene = loader.loadSceneFromFile(this.MediaDir + "FullLevel-TgcScene.xml", this.MediaDir + "\\");

            //Device de DirectX para crear primitivas.
            //
            PuertaModelo = loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0];

            MonstruoModelo = loader.loadSceneFromFile(this.MediaDir + "\\Monstruo-TgcScene.xml").Meshes[0];

            //=========================================================================================
            // Ojo con el add de meshes a la escena ya que los agrega y no son elementos independientes.
            // Hay que ver como reaccionan, para la entrega habria comentar las puertas, para que se
            // vea como el enemigo persigue al jugador
            //=========================================================================================

            Puerta1 = PuertaModelo.createMeshInstance("Puerta1");
            Puerta1.AutoTransformEnable = true;
            Puerta1.move(89f, 31.5f, 275f);
            TgcScene.Meshes.Add(Puerta1);

            Puerta2 = PuertaModelo.createMeshInstance("Puerta2");
            Puerta2.AutoTransformEnable = true;
            Puerta2.move(439f, 32f, 203f);
            TgcScene.Meshes.Add(Puerta2);

            Puerta3 = PuertaModelo.createMeshInstance("Puerta3");
            Puerta3.AutoTransformEnable = true;
            Puerta3.move(201f, 32f, 1570f);
            Puerta3.rotateY(FastMath.PI_HALF);
            Puerta3.createBoundingBox();
            TgcScene.Meshes.Add(Puerta3);

            Puerta4 = PuertaModelo.createMeshInstance("Puerta4");
            Puerta4.AutoTransformEnable = true;
            Puerta4.move(452f, 32f, 1221f);
            Puerta4.rotateY(FastMath.PI_HALF);
            Puerta4.createBoundingBox();
            TgcScene.Meshes.Add(Puerta4);

            Puerta5 = PuertaModelo.createMeshInstance("Puerta5");
            Puerta5.AutoTransformEnable = true;
            Puerta5.move(459f, 32f, 1675f);
            TgcScene.Meshes.Add(Puerta5);

            Puerta6 = PuertaModelo.createMeshInstance("Puerta6");
            Puerta6.AutoTransformEnable = true;
            Puerta6.move(734f, 32f, 1570f);
            Puerta6.rotateY(FastMath.PI_HALF);
            Puerta6.createBoundingBox();
            TgcScene.Meshes.Add(Puerta6);

            Puerta7 = PuertaModelo.createMeshInstance("Puerta7");
            Puerta7.AutoTransformEnable = true;
            Puerta7.move(915f, 32f, 751f);
            Puerta7.rotateY(FastMath.PI_HALF);
            Puerta7.createBoundingBox();
            TgcScene.Meshes.Add(Puerta7);

            Puerta8 = PuertaModelo.createMeshInstance("Puerta8");
            Puerta8.AutoTransformEnable = true;
            Puerta8.move(695f, 32f, 600f);
            Puerta8.rotateY(FastMath.PI_HALF);
            Puerta8.createBoundingBox();
            TgcScene.Meshes.Add(Puerta8);

            Puerta9 = PuertaModelo.createMeshInstance("Puerta9");
            Puerta9.AutoTransformEnable = true;
            Puerta9.move(469f, 32f, 921f);
            TgcScene.Meshes.Add(Puerta9);

            Puerta10 = PuertaModelo.createMeshInstance("Puerta10");
            Puerta10.AutoTransformEnable = true;
            Puerta10.move(695f, 32f, 912f);
            Puerta10.rotateY(FastMath.PI_HALF);
            Puerta10.createBoundingBox();
            TgcScene.Meshes.Add(Puerta10);

            Puerta11 = PuertaModelo.createMeshInstance("Puerta11");
            Puerta11.AutoTransformEnable = true;
            Puerta11.move(399f, 32f, 724f);
            TgcScene.Meshes.Add(Puerta11);

            Puerta12 = PuertaModelo.createMeshInstance("Puerta12");
            Puerta12.AutoTransformEnable = true;
            Puerta12.move(454f, 32f, 331f);
            Puerta12.rotateY(FastMath.PI_HALF);
            Puerta12.createBoundingBox();
            TgcScene.Meshes.Add(Puerta12);

            Puerta13 = PuertaModelo.createMeshInstance("Puerta13");
            Puerta13.AutoTransformEnable = true;
            Puerta13.move(399f, 32f, 1292f);
            TgcScene.Meshes.Add(Puerta13);

            Puerta14 = PuertaModelo.createMeshInstance("Puerta14");
            Puerta14.AutoTransformEnable = true;
            Puerta14.move(89f, 32f, 922f);
            TgcScene.Meshes.Add(Puerta14);

            Puerta15 = PuertaModelo.createMeshInstance("Puerta1");
            Puerta15.AutoTransformEnable = true;
            Puerta15.move(89f, 142f, 275f);
            TgcScene.Meshes.Add(Puerta15);

            Puerta16 = PuertaModelo.createMeshInstance("Puerta2");
            Puerta16.AutoTransformEnable = true;
            Puerta16.move(439f, 142f, 203f);
            TgcScene.Meshes.Add(Puerta16);

            Puerta17 = PuertaModelo.createMeshInstance("Puerta3");
            Puerta17.AutoTransformEnable = true;
            Puerta17.move(201f, 142f, 1570f);
            Puerta17.rotateY(FastMath.PI_HALF);
            Puerta17.createBoundingBox();
            TgcScene.Meshes.Add(Puerta17);

            Puerta18 = PuertaModelo.createMeshInstance("Puerta4");
            Puerta18.AutoTransformEnable = true;
            Puerta18.move(452f, 142f, 1221f);
            Puerta18.rotateY(FastMath.PI_HALF);
            Puerta18.createBoundingBox();
            TgcScene.Meshes.Add(Puerta18);

            Puerta19 = PuertaModelo.createMeshInstance("Puerta5");
            Puerta19.AutoTransformEnable = true;
            Puerta19.move(459f, 142f, 1675f);
            TgcScene.Meshes.Add(Puerta19);

            Puerta20 = PuertaModelo.createMeshInstance("Puerta6");
            Puerta20.AutoTransformEnable = true;
            Puerta20.move(734f, 142f, 1570f);
            Puerta20.rotateY(FastMath.PI_HALF);
            Puerta20.createBoundingBox();
            TgcScene.Meshes.Add(Puerta20);

            Puerta21 = PuertaModelo.createMeshInstance("Puerta7");
            Puerta21.AutoTransformEnable = true;
            Puerta21.move(915f, 142f, 751f);
            Puerta21.rotateY(FastMath.PI_HALF);
            Puerta21.createBoundingBox();
            TgcScene.Meshes.Add(Puerta21);

            Puerta22 = PuertaModelo.createMeshInstance("Puerta8");
            Puerta22.AutoTransformEnable = true;
            Puerta22.move(695f, 142f, 600f);
            Puerta22.rotateY(FastMath.PI_HALF);
            Puerta22.createBoundingBox();
            TgcScene.Meshes.Add(Puerta22);

            Puerta23 = PuertaModelo.createMeshInstance("Puerta9");
            Puerta23.AutoTransformEnable = true;
            Puerta23.move(469f, 142f, 921f);
            Puerta23.createBoundingBox();
            TgcScene.Meshes.Add(Puerta23);

            Puerta24 = PuertaModelo.createMeshInstance("Puerta10");
            Puerta24.AutoTransformEnable = true;
            Puerta24.move(695f, 142f, 912f);
            Puerta24.rotateY(FastMath.PI_HALF);
            Puerta24.createBoundingBox();
            TgcScene.Meshes.Add(Puerta24);

            Puerta25 = PuertaModelo.createMeshInstance("Puerta11");
            Puerta25.AutoTransformEnable = true;
            Puerta25.move(399f, 142f, 724f);
            TgcScene.Meshes.Add(Puerta25);

            Puerta26 = PuertaModelo.createMeshInstance("Puerta12");
            Puerta26.AutoTransformEnable = true;
            Puerta26.move(454f, 142f, 331f);
            Puerta26.rotateY(FastMath.PI_HALF);
            Puerta26.createBoundingBox();
            TgcScene.Meshes.Add(Puerta26);

            Puerta27 = PuertaModelo.createMeshInstance("Puerta13");
            Puerta27.AutoTransformEnable = true;
            Puerta27.move(399f, 142f, 1292f);
            TgcScene.Meshes.Add(Puerta27);

            Puerta28 = PuertaModelo.createMeshInstance("Puerta14");
            Puerta28.AutoTransformEnable = true;
            Puerta28.move(89f, 142f, 922f);
            TgcScene.Meshes.Add(Puerta28);

            Monstruo = MonstruoModelo.createMeshInstance("Monstruo");
            Monstruo.move(463, 30, 83);
            monstruoSphere = Core.BoundingVolumes.TgcBoundingSphere.computeFromMesh(Monstruo);
            monstruoSphere.setCenter(Monstruo.Position);
            objetosColisionables.Clear();
            foreach (var mesh in TgcScene.Meshes)
            {
                objetosColisionables.Add(mesh.BoundingBox);
            }
            TgcScene.Meshes.Add(Monstruo);
            //Textura de la carperta Media. Game.Default es un archivo de configuracion (Game.settings) util para poner cosas.
            //Pueden abrir el Game.settings que se ubica dentro de nuestro proyecto para configurar.
            var pathTexturaCaja = MediaDir + Game.Default.TexturaCaja;

            //Cargamos una textura, tener en cuenta que cargar una textura significa crear una copia en memoria.
            //Es importante cargar texturas en Init, si se hace en el render loop podemos tener grandes problemas si instanciamos muchas.
            var texture = TgcTexture.createTexture(pathTexturaCaja);

            //Creamos una caja 3D ubicada de dimensiones (5, 10, 5) y la textura como color.
            var size = new Vector3(10, 10, 10);
            //Construimos una caja según los parámetros, por defecto la misma se crea con centro en el origen y se recomienda así para facilitar las transformaciones.
            Box = TgcBox.fromSize(size, texture);
            //Posición donde quiero que este la caja, es común que se utilicen estructuras internas para las transformaciones.
            //Entonces actualizamos la posición lógica, luego podemos utilizar esto en render para posicionar donde corresponda con transformaciones.
            Box.Position = new Vector3(-25, 0, 0);

            //Luz?? Sep, si uno hace en el render un lightMesh.render(), te va a mostrar donde se posiciona la luz en el espacio.

            lightMesh = TgcBox.fromSize(new Vector3(5, 5, 5));

            //Pongo al mesh en posicion, activo e AutoTransform
            lightMesh.AutoTransformEnable = true;
            lightMesh.Position = new Vector3(463, 51, 83);
            lightMesh.Color = Color.GreenYellow;

            collisionManager = new SphereCollisionManager();

            playerPos = TgcBox.fromSize(new Vector3(5, 5, 5));
            lightMesh.Position = new Vector3(463, 51, 83);

            glowstick = new Linterna();
            glowstick.setSelect(true);
            glowstick.setEnergia(3);//en el caso del glowstick la energia representa el numero de glowsticks que tiene el jugador

            lighter = new Linterna();
            lighter.setSelect(false);
            lighter.setEnergia(100);

            flashlight = new Linterna();
            flashlight.setSelect(false);
            flashlight.setEnergia(100);
        }
        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();

            //Switch entre glowstick(F), encendedor(G) y linterna(H)
  
            if (Input.keyPressed(Key.F))
            {
                lightMesh.Color = Color.GreenYellow;
                this.glowstick.setSelect(true);
                this.lighter.setSelect(false);
                this.flashlight.setSelect(false);
                timer = 0;
            }
            if (Input.keyPressed(Key.G))
            {
                this.glowstick.setSelect(false);
                this.lighter.setSelect(true);
                this.flashlight.setSelect(false);
                lightMesh.Color = Color.Yellow;
                timer = 0;
            }
            if (Input.keyPressed(Key.H))
            {
                this.glowstick.setSelect(false);
                this.lighter.setSelect(false);
                this.flashlight.setSelect(true);
                lightMesh.Color = Color.WhiteSmoke;
                timer = 0;
            }
            //Logica de seleccion de luces
            //para el glowstick cada 60 seg deberiamos perder 1 barra.
            if (glowstick.getSelect())
            {
                if (System.Math.Truncate(timer)/60 == 1 && glowstick.getEnergia()>0)
                {
                    glowstick.perderEnergia(1);
                    timer = 0;
                }
            }
            if (lighter.getSelect())
            {
                if (System.Math.Truncate(timer) % 1 == 0 && lighter.getEnergia()>0)
                {
                    lighter.perderEnergia(0.083f);
                    timer = 0;
                }
            }
            if (flashlight.getSelect())
            {
                if (System.Math.Truncate(timer) % 1 == 0 &&flashlight.getEnergia()>0)
                {
                    flashlight.perderEnergia(0.041f);
                    timer = 0;
                }
            }


            //Para activar o desactivar al monstruo
            if (Input.keyPressed(Key.M)) {
                monstruoActivo = !monstruoActivo;
            }

            //Para activar o desactivar colisiones del monstruo
            if (Input.keyPressed(Key.N))
            {
                monstruoSolido = !monstruoSolido;
            }
            
            //Logica del monstruo
            if (monstruoActivo) {
                var targetDistance = Camara.Position - Monstruo.Position;

                targetDistance.Y = 0f;//El monstruo solo se mueve en el plano XZ

                var targetDirection = Vector3.Normalize(targetDistance);
                var monstruoMovement = targetDirection * monstruoVelocidad * ElapsedTime;

                var targetAngleH = FastMath.Atan2(targetDirection.X, targetDirection.Z);
                var targetAngleV = FastMath.Asin(targetDirection.Y);
                var originalRot = Monstruo.Rotation;
                
                var originalPos = Monstruo.Position;

                Monstruo.Rotation = new Vector3(targetAngleV, targetAngleH + FastMath.PI, 0);
                if (monstruoSolido)
                {
                    monstruoMovement = collisionManager.moveCharacter(monstruoSphere, monstruoMovement, objetosColisionables);
                }
                else monstruoSphere.moveCenter(monstruoMovement);
                Monstruo.move(monstruoMovement);
                /*
                Monstruo.move(monstruoMovement);
                //Chequear si el objeto principal en su nueva posición choca con alguno de los objetos de la escena.
                //Si es así, entonces volvemos a la posición original.
                //Cada TgcMesh tiene un objeto llamado BoundingBox. El BoundingBox es una caja 3D que representa al objeto
                //de forma simplificada (sin tener en cuenta toda la complejidad interna del modelo).
                //Este BoundingBox se utiliza para chequear si dos objetos colisionan entre sí.
                //El framework posee la clase TgcCollisionUtils con muchos algoritmos de colisión de distintos tipos de objetos.
                //Por ejemplo chequear si dos cajas colisionan entre sí, o dos esferas, o esfera con caja, etc.
                var collisionFound = false;

                //chequeo de Colision mocho del monstruo
                foreach (var mesh in this.TgcScene.Meshes)
                {
                    if (mesh.Equals(Monstruo)) continue;
                    //Los dos BoundingBox que vamos a testear
                    var mainMeshBoundingBox = Monstruo.BoundingBox;
                    var sceneMeshBoundingBox = mesh.BoundingBox;

                    //Ejecutar algoritmo de detección de colisiones
                    var collisionResult = TgcCollisionUtils.classifyBoxBox(mainMeshBoundingBox, sceneMeshBoundingBox);

                    //Hubo colisión con un objeto. Guardar resultado y abortar loop.
                    if (collisionResult != TgcCollisionUtils.BoxBoxResult.Afuera)
                    {
                        collisionFound = true;
                        break;
                    }
                }

                //Si hubo alguna colisión, entonces restaurar la posición original del mesh
                if (collisionFound)
                {
                    Monstruo.Position = originalPos;
                    Monstruo.Rotation = originalRot;
                }
                */



            } 
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            PreRender();
            timer += ElapsedTime;

            if (this.glowstick.getSelect() || this.lighter.getSelect())
            {
                Shader = TgcShaders.Instance.TgcMeshPointLightShader;
            }
            if (this.flashlight.getSelect())
            {
                Shader = TgcShaders.Instance.TgcMeshSpotLightShader;
            }

            if (glowstick.getEnergia() == 0 && System.Math.Truncate(lighter.getEnergia()) == 0 && System.Math.Truncate(flashlight.getEnergia()) == 0)
            {
                //TODO: Poner un Shader que distorsione todo D:
            }

            playerPos.Position = Camara.Position;
            //boundingSphereCamara.moveCenter(new Vector3(playerPos.Position.X + 0, playerPos.Position.Y - 15, playerPos.Position.Z + 0));
            //sphereCamara.Position = new Vector3(Camara.Position.X + 0,Camara.Position.Y-15,Camara.Position.Z + 0);
            
            foreach (var mesh in TgcScene.Meshes)
            {
                mesh.Effect = Shader;
                mesh.Technique = TgcShaders.Instance.getTgcMeshTechnique(mesh.RenderType);
            }
            //Renderizar meshes
            foreach (var mesh in TgcScene.Meshes)
            {
                //se actualiza el transform del mesh
                mesh.UpdateMeshTransform();

                //Logica de luces dependiendo de la seleccion y la energia de las mismas
                if (glowstick.getSelect() && glowstick.getEnergia() != 0)
                {
                    lightMesh.Position = Camara.Position;
                    //Cargar variables shader de la luz
                    mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("lightIntensity", 20f);
                    mesh.Effect.SetValue("lightAttenuation", 2f);

                    //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                    mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularExp", 20f);
                }
                if (glowstick.getSelect() && glowstick.getEnergia() == 0)
                {
                    lightMesh.Position = Camara.Position;
                    //Cargar variables shader de la luz
                    mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("lightIntensity", 5f);
                    mesh.Effect.SetValue("lightAttenuation", 2f);

                    //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                    mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(Color.Gainsboro));
                    mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularExp", 20f);
                }
                if (lighter.getSelect() && lighter.getEnergia() > 20)
                {
                    lightMesh.Position = Camara.Position;
                    //Cargar variables shader de la luz
                    mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("lightIntensity", 35f);
                    mesh.Effect.SetValue("lightAttenuation", 1f);

                    //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                    mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularExp", 20f);
                }
                if (lighter.getSelect() && lighter.getEnergia() <= 20 && lighter.getEnergia() > 0)
                {
                    if (System.Math.Truncate(lighter.getEnergia()) % 2 == 0)
                    {
                        lightMesh.Color = Color.Orange;
                        lightMesh.Position = Camara.Position;
                        //Cargar variables shader de la luz
                        mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                        mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                        mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                        mesh.Effect.SetValue("lightIntensity", 25f);
                        mesh.Effect.SetValue("lightAttenuation", 1f);

                        //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                        mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                        mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(lightMesh.Color));
                        mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                        mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.Black));
                        mesh.Effect.SetValue("materialSpecularExp", 20f);
                    }
                    if (System.Math.Truncate(lighter.getEnergia()) % 2 == 1)
                    {
                        lightMesh.Color = Color.OrangeRed;
                        lightMesh.Position = Camara.Position;
                        //Cargar variables shader de la luz
                        mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                        mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                        mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                        mesh.Effect.SetValue("lightIntensity", 45f);
                        mesh.Effect.SetValue("lightAttenuation", 1f);

                        //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                        mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                        mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(lightMesh.Color));
                        mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                        mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.Black));
                        mesh.Effect.SetValue("materialSpecularExp", 20f);
                    }
                }
                if (lighter.getSelect() && lighter.getEnergia() == 0)
                {
                    lightMesh.Position = Camara.Position;
                    //Cargar variables shader de la luz
                    mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("lightIntensity", 2f);
                    mesh.Effect.SetValue("lightAttenuation", 1f);

                    //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                    mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularExp", 20f);
                }
                if (flashlight.getSelect() && flashlight.getEnergia() > 10)
                {
                    float x;
                    float y;
                    float z;
                    //Si no colisiona contra algo es esto
                    // lamda * director + coordenada en eje
                    x = (float)80.5 * (Camara.LookAt - Camara.Position).X + Camara.LookAt.X;
                    y = (float)80.5 * (Camara.LookAt - Camara.Position).Y + Camara.LookAt.Y;
                    z = (float)80.5 * (Camara.LookAt - Camara.Position).Z + Camara.LookAt.Z;
                    lightMesh.Position = new Vector3(x, y, z);
                    float a;
                    float b;
                    float c;
                    a = (float)3000.01 * (Camara.LookAt - Camara.Position).X + Camara.Position.X;
                    b = (float)3000.01 * (Camara.LookAt - Camara.Position).Y + Camara.Position.Y;
                    c = (float)3000.01 * (Camara.LookAt - Camara.Position).Z + Camara.Position.Z;
                    var direccion = new Vector3(a, b, c);
                    direccion.Normalize();
                    var posLuz = lightMesh.Position;

                    mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(posLuz));
                    mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("spotLightDir", TgcParserUtils.vector3ToFloat3Array(direccion));
                    mesh.Effect.SetValue("lightIntensity", 350f);
                    mesh.Effect.SetValue("lightAttenuation", 5f);
                    mesh.Effect.SetValue("spotLightAngleCos", 0.65f);
                    mesh.Effect.SetValue("spotLightExponent", 10f);

                    //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                    mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(Color.White));
                    mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularExp", 29f);
                }
                if (flashlight.getSelect() && flashlight.getEnergia() <= 10 && flashlight.getEnergia() > 0)
                {
                    float x;
                    float y;
                    float z;
                    //Si no colisiona contra algo es esto
                    // lamda * director + coordenada en eje
                    x = (float)80.5 * (Camara.LookAt - Camara.Position).X + Camara.LookAt.X;
                    y = (float)80.5 * (Camara.LookAt - Camara.Position).Y + Camara.LookAt.Y;
                    z = (float)80.5 * (Camara.LookAt - Camara.Position).Z + Camara.LookAt.Z;
                    lightMesh.Position = new Vector3(x, y, z);
                    float a;
                    float b;
                    float c;
                    a = (float)3000.01 * (Camara.LookAt - Camara.Position).X + Camara.Position.X;
                    b = (float)3000.01 * (Camara.LookAt - Camara.Position).Y + Camara.Position.Y;
                    c = (float)3000.01 * (Camara.LookAt - Camara.Position).Z + Camara.Position.Z;
                    var direccion = new Vector3(a, b, c);
                    direccion.Normalize();
                    var posLuz = lightMesh.Position;

                    mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(posLuz));
                    mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("spotLightDir", TgcParserUtils.vector3ToFloat3Array(direccion));
                    mesh.Effect.SetValue("lightIntensity", 150f);
                    mesh.Effect.SetValue("lightAttenuation", 5f);
                    mesh.Effect.SetValue("spotLightAngleCos", 0.65f);
                    mesh.Effect.SetValue("spotLightExponent", 10f);

                    //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                    mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(Color.White));
                    mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularExp", 29f);
                }
                if (flashlight.getSelect() && System.Math.Truncate(flashlight.getEnergia()) == 0)
                {
                    float x;
                    float y;
                    float z;
                    //Si no colisiona contra algo es esto
                    // lamda * director + coordenada en eje
                    x = (float)80.5 * (Camara.LookAt - Camara.Position).X + Camara.LookAt.X;
                    y = (float)80.5 * (Camara.LookAt - Camara.Position).Y + Camara.LookAt.Y;
                    z = (float)80.5 * (Camara.LookAt - Camara.Position).Z + Camara.LookAt.Z;
                    lightMesh.Position = new Vector3(x, y, z);
                    float a;
                    float b;
                    float c;
                    a = (float)3000.01 * (Camara.LookAt - Camara.Position).X + Camara.Position.X;
                    b = (float)3000.01 * (Camara.LookAt - Camara.Position).Y + Camara.Position.Y;
                    c = (float)3000.01 * (Camara.LookAt - Camara.Position).Z + Camara.Position.Z;
                    var direccion = new Vector3(a, b, c);
                    direccion.Normalize();
                    var posLuz = lightMesh.Position;

                    mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(posLuz));
                    mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("spotLightDir", TgcParserUtils.vector3ToFloat3Array(direccion));
                    mesh.Effect.SetValue("lightIntensity", 50f);
                    mesh.Effect.SetValue("lightAttenuation", 5f);
                    mesh.Effect.SetValue("spotLightAngleCos", 0.65f);
                    mesh.Effect.SetValue("spotLightExponent", 10f);

                    //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                    mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(Color.White));
                    mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularExp", 29f);
                }
                if (glowstick.getEnergia() == 0 && System.Math.Truncate(lighter.getEnergia()) == 0 && System.Math.Truncate(flashlight.getEnergia()) == 0)
                {
                    //TODO: poner las propiedades del shader que genera distorsiones D:
                }

                //Renderizar modelo
                mesh.render();
                mesh.BoundingBox.render();

            }
            monstruoSphere.render();
            //Dibuja un texto por pantalla
            DrawText.drawText("Use W,A,S,D para desplazarte, Espacio para subir, Control para bajar, Shift para ir mas rapido y el mouse para mover la camara: \n "
                + "Position : " + TgcParserUtils.printVector3(Camara.Position) + "\n"
                + " LookAt : " + TgcParserUtils.printVector3(Camara.LookAt) + "\n"
                + " Light Position : " + TgcParserUtils.printVector3(lightMesh.Position) + "\n"
                + " Monster Position : " + TgcParserUtils.printVector3(Monstruo.Position) + "\n"
                + " Glowstick Stock : " + glowstick.getEnergia() + "\n"
                + " Lighter Energy : " + lighter.getEnergia() + "\n"
                + " Flashlight Energy : " + flashlight.getEnergia() + "\n"
                + " Time: " + timer + "\n"
                + " M para Monstruo D:" + "\n"
                + " N para activar/desactivar colisiones del Monstruo"
            , 0, 30, Color.OrangeRed);

            //Siempre antes de renderizar el modelo necesitamos actualizar la matriz de transformacion.
            //Debemos recordar el orden en cual debemos multiplicar las matrices, en caso de tener modelos jerárquicos, tenemos control total.
            //Box.Transform = Matrix.Scaling(Box.Scale) * Matrix.RotationYawPitchRoll(Box.Rotation.Y, Box.Rotation.X, Box.Rotation.Z) * Matrix.Translation(Box.Position);
            //A modo ejemplo realizamos toda las multiplicaciones, pero aquí solo nos hacia falta la traslación.
            //Finalmente invocamos al render de la caja
            //Box.render();

            //Cuando tenemos modelos mesh podemos utilizar un método que hace la matriz de transformación estándar.
            //Es útil cuando tenemos transformaciones simples, pero OJO cuando tenemos transformaciones jerárquicas o complicadas.
            //Mesh.UpdateMeshTransform();
            //Render de una escena
            //TgcScene.renderAll();

            Puerta1.render();
            Puerta2.render();
            Puerta3.render();
            Puerta4.render();
            Puerta5.render();
            Puerta6.render();
            Puerta7.render();
            Puerta8.render();
            Puerta9.render();
            Puerta10.render();
            Puerta11.render();
            Puerta12.render();
            Puerta13.render();
            Puerta14.render();
            Puerta15.render();
            Puerta16.render();
            Puerta17.render();
            Puerta18.render();
            Puerta19.render();
            Puerta20.render();
            Puerta21.render();
            Puerta22.render();
            Puerta23.render();
            Puerta24.render();
            Puerta25.render();
            Puerta26.render();
            Puerta27.render();
            Puerta28.render();
            //lightMesh.render();
            
            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            PostRender();
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            //Dispose de una escena.
            //PuertaModelo.dispose();
            Puerta1.dispose();
            Puerta2.dispose();
            Puerta3.dispose();
            Puerta4.dispose();
            Puerta5.dispose();
            Puerta6.dispose();
            Puerta7.dispose();
            Puerta8.dispose();
            Puerta9.dispose();
            Puerta10.dispose();
            Puerta11.dispose();
            Puerta12.dispose();
            Puerta13.dispose();
            Puerta14.dispose();
            Monstruo.dispose();
            TgcScene.disposeAll();
            Shader.Dispose();
        }
    }
}