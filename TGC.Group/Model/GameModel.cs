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
using TGC.Group.Model;
using TGC.Examples.Engine2D.Spaceship.Core;
using TGC.Core.Text;
using TGC.Core.PortalRendering;

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

        #region HUD
        //HUD
        
        //Dibujador o dibujante??? 
        private Drawer2D drawer2D;
        
        //Vida
        private CustomSprite vida;
        private float vidaPorcentaje = 100.1f;
        
        //Punto de Mira
        private CustomSprite centerPoint;

        //Stamina
        private CustomSprite stamina;
        private float staminaPorcentaje = 100f;

        //Glowsticks
        private CustomSprite glowstickHUD1;
        private CustomSprite glowstickHUD2;
        private CustomSprite glowstickHUD3;

        //Lighter
        private CustomSprite lighterHUD;

        //Flashlight
        private CustomSprite flashlightHUD;
        private CustomSprite flashlightLiveHUD;
        #endregion
        private TgcText2D textoDeLaMuerte;

        private TgcMesh PuertaModelo { get; set; }
        private TgcMesh MonstruoModelo { get; set; }

        private Puerta puerta1;
        private Puerta puerta2;
        private Puerta puerta3;
        private Puerta puerta4;
        private Puerta puerta5;
        private Puerta puerta6;

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

        private Boton botonEscapePod1;
        private Boton botonEscapePod2;
        private Boton botonOxigeno;
        private Boton botonElectricidad;
        private Boton botonElectricidad2;
        private Boton botonCombustible;

        private Monstruo monstruo { get; set; }
        private SphereCollisionManager collisionManager;
        private TgcBoundingSphere esferaDeLinterna;

        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }
        
        private Microsoft.DirectX.Direct3D.Effect Shader { get; set; }
        private TgcBox lightMesh;
        private TgcBox playerPos;

        private Linterna glowstick;
        private Linterna lighter;
        private Linterna flashlight;

        private Botiquin botiquin1;
        private Botiquin botiquin2;
        private Botiquin botiquin3;
        private Botiquin botiquin4;
        private Botiquin botiquin5;
        private Botiquin botiquin6;

        //private List<Botiquin> botiquines;

        private float timer;

        #region Init
        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            Camara = new Examples.Camara.TgcFpsCamera(new Vector3(463, 55.2f, 83), 125f, 100f, Input);
            var d3dDevice = D3DDevice.Instance.Device; 

            #region HUD init
            drawer2D = new Drawer2D();
            vida = new CustomSprite();
            vida.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\vidaRed.jpg", D3DDevice.Instance.Device);

            stamina = new CustomSprite();
            stamina.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\staminaYellow.jpg", D3DDevice.Instance.Device);

            centerPoint = new CustomSprite();
            centerPoint.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\green.bmp", D3DDevice.Instance.Device);

            glowstickHUD1 = new CustomSprite();
            glowstickHUD1.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\foam-stick-green.png", D3DDevice.Instance.Device);

            glowstickHUD2 = new CustomSprite();
            glowstickHUD2.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\foam-stick-green.png", D3DDevice.Instance.Device);

            glowstickHUD3 = new CustomSprite();
            glowstickHUD3.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\foam-stick-green.png", D3DDevice.Instance.Device);

            lighterHUD = new CustomSprite();
            lighterHUD.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\Zippo.png", D3DDevice.Instance.Device);

            flashlightHUD = new CustomSprite();
            flashlightHUD.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\linternaHUD.png", D3DDevice.Instance.Device);

            flashlightLiveHUD = new CustomSprite();
            flashlightLiveHUD.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\white.bmp", D3DDevice.Instance.Device);
            #endregion

            //Carga de nivel
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene = loader.loadSceneFromFile(this.MediaDir + "FullLevel-TgcScene.xml", this.MediaDir + "\\");
            //TgcScene.PortalRendering.createDebugPortals(Color.Purple);
            //TgcScene.PortalRendering.Portals.Add(new Core.PortalRendering.TgcPortalRenderingPortal("puerta"));

            //Computamos las normales
            foreach (var mesh in TgcScene.Meshes)
            {
                int[] adjacencia = {1,1,1,1,1,1,1,1,1,1,1,1};
                
                //TODO hay que reordenar normales
                mesh.D3dMesh.ComputeNormals();
            }

            //Carga de puerta y de enemigo
            PuertaModelo = loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0];

            MonstruoModelo = loader.loadSceneFromFile(this.MediaDir + "\\Monstruo-TgcScene.xml").Meshes[0];

            #region PuertasInit
            puerta1 = new Puerta();
            puerta1.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta1.changePosition(new Vector3(89f, 31.5f, 275f));
            //puerta1.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta1.getMesh());
           
            //TgcPortalRenderingPortal puerta1Portal = new TgcPortalRenderingPortal("puerta1Portal", puerta1.getMesh().BoundingBox);
            //TgcScene.PortalRendering.Portals.Add(puerta1Portal);

            puerta2 = new Puerta();
            puerta2.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta2.changePosition(new Vector3(439f, 32f, 203f));
            //puerta2.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta2.getMesh());
            //TgcScene.PortalRendering.Portals.Add(new Core.PortalRendering.TgcPortalRenderingPortal("puerta2", puerta2.getMesh().BoundingBox));
     
            puerta3 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta3.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta3.getMesh().move(new Vector3(201f, 32f, 1570f));
            puerta3.getMesh().rotateY(FastMath.PI_HALF);
            puerta3.getMesh().UpdateMeshTransform();
            TgcScene.Meshes.Add(puerta3.getMesh());

            /*
            Puerta4 = PuertaModelo.createMeshInstance("Puerta4");
            Puerta4.AutoTransformEnable = true;
            Puerta4.move(452f, 32f, 1221f);
            Puerta4.rotateY(FastMath.PI_HALF);
            Puerta4.createBoundingBox();
            TgcScene.Meshes.Add(Puerta4);
            
            /*
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
            */
            #endregion

            #region TriggerMonstruoInit
            //Se declaran y definen las zonas que al ser ingresadas activan al monstruo
            var monsterTriggers = new List<TgcBoundingSphere>();
            var monsterSpawnPoints = new List<Vector3>();
            
            monsterTriggers.Add(new TgcBoundingSphere(new Vector3(442f, 40f, 251f), 50f));
            monsterSpawnPoints.Add(new Vector3(942f, 30f, 250f));

            monsterTriggers.Add(new TgcBoundingSphere(new Vector3(420f, 40f, 691f), 50f));
            monsterSpawnPoints.Add(new Vector3(70f, 30f, 690f));

            monsterTriggers.Add(new TgcBoundingSphere(new Vector3(644f, 40f, 412f), 50f));
            monsterSpawnPoints.Add(new Vector3(944f, 30f, 406f));

            monsterTriggers.Add(new TgcBoundingSphere(new Vector3(923f, 40f, 731f), 50f));
            monsterSpawnPoints.Add(new Vector3(570f, 30f, 743f));

            monsterTriggers.Add(new TgcBoundingSphere(new Vector3(770f, 40f, 1722f), 50f));
            monsterSpawnPoints.Add(new Vector3(766f, 30f, 1335f));

            //2do piso igual al primero
            monsterTriggers.Add(new TgcBoundingSphere(new Vector3(442f, 40f + 110f+ 110f, 251f), 50f));
            monsterSpawnPoints.Add(new Vector3(942f, 30f + 110f, 250f));

            monsterTriggers.Add(new TgcBoundingSphere(new Vector3(420f, 40f + 110f, 691f), 50f));
            monsterSpawnPoints.Add(new Vector3(70f, 30f + 110f, 690f));

            monsterTriggers.Add(new TgcBoundingSphere(new Vector3(644f, 40f + 110f, 412f), 50f));
            monsterSpawnPoints.Add(new Vector3(944f, 30f + 110f, 406f));

            monsterTriggers.Add(new TgcBoundingSphere(new Vector3(923f, 40f + 110f, 731f), 50f));
            monsterSpawnPoints.Add(new Vector3(570f, 30f + 110f, 743f));

            monsterTriggers.Add(new TgcBoundingSphere(new Vector3(770f, 40f + 110f, 1722f), 50f));
            monsterSpawnPoints.Add(new Vector3(766f, 30f + 110f, 1335f));
#endregion

            monstruo = new Monstruo();
            monstruo.init(MonstruoModelo.createMeshInstance("Monstruo"),new Vector3(0, 0, 0), monsterTriggers, monsterSpawnPoints);
            
            objetosColisionables.Clear();

            foreach (var mesh in TgcScene.Meshes)
            {
                objetosColisionables.Add(mesh.BoundingBox);
            }
            TgcScene.Meshes.Add(monstruo.mesh);

            #region Botiquines Init
            
            //Falta mesh y ubicaciones de cada botiquin
            botiquin1 = new Botiquin();
            botiquin1.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin1.changePosicion(new Vector3(463,0,490));
            TgcScene.Meshes.Add(botiquin1.meshBotiquin);


            botiquin2 = new Botiquin();
            botiquin2 = new Botiquin();
            botiquin2.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin2.changePosicion(new Vector3(463, 110, 490));
            TgcScene.Meshes.Add(botiquin2.meshBotiquin);

            botiquin3 = new Botiquin();
            botiquin3 = new Botiquin();
            botiquin3.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin3.changePosicion(new Vector3(400, 0, 890));
            TgcScene.Meshes.Add(botiquin3.meshBotiquin);

            botiquin4 = new Botiquin();
            botiquin4 = new Botiquin();
            botiquin4.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin4.changePosicion(new Vector3(400, 110, 890));
            TgcScene.Meshes.Add(botiquin4.meshBotiquin);

            botiquin5 = new Botiquin();
            botiquin5 = new Botiquin();
            botiquin5.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin5.changePosicion(new Vector3(225, 0, 1600));
            TgcScene.Meshes.Add(botiquin5.meshBotiquin);

            botiquin6 = new Botiquin();
            botiquin6 = new Botiquin();
            botiquin6.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin6.changePosicion(new Vector3(225, 0, 1600));
            TgcScene.Meshes.Add(botiquin6.meshBotiquin);

            #endregion

            #region Texto de la Muerte
            textoDeLaMuerte = new TgcText2D();
            textoDeLaMuerte.Text = "YOU DIED";
            textoDeLaMuerte.Color = Color.Red;
            textoDeLaMuerte.Position = new Point(D3DDevice.Instance.Width/12,D3DDevice.Instance.Height/2);
            textoDeLaMuerte.changeFont(new System.Drawing.Font("TimesNewRoman", 55 ));
            #endregion

            #region BotonesInit
            botonEscapePod1 = new Boton();           
            botonEscapePod1.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonEscapePod1.meshBoton.Position = new Vector3(440, 25, 30);
            botonEscapePod1.changeColor(Color.Red);
            //TgcScene.Meshes.Add(botonEscapePod1.meshBoton);

            botonEscapePod2 = new Boton();
            botonEscapePod2.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonEscapePod2.meshBoton.Position = new Vector3(440, 135, 30);
            botonEscapePod2.changeColor(Color.Red);
            //TgcScene.Meshes.Add(botonEscapePod2.meshBoton);

            botonOxigeno = new Boton();
            botonOxigeno.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonOxigeno.meshBoton.Position = new Vector3(305, 135, 730);
            botonOxigeno.changeColor(Color.Red);
            //TgcScene.Meshes.Add(botonOxigeno.meshBoton);

            botonElectricidad = new Boton();
            botonElectricidad.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonElectricidad.meshBoton.Position = new Vector3(490, 25, 1520);
            botonElectricidad.changeColor(Color.Red);
            //TgcScene.Meshes.Add(botonElectricidad.meshBoton);

            botonElectricidad2 = new Boton();
            botonElectricidad2.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonElectricidad2.meshBoton.Position = new Vector3(490, 135, 1520);
            botonElectricidad2.changeColor(Color.Red);
            //TgcScene.Meshes.Add(botonElectricidad2.meshBoton);

            botonCombustible = new Boton();
            botonCombustible.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonCombustible.meshBoton.Position = new Vector3(550, 25, 280);
            botonCombustible.changeColor(Color.Red);
            //TgcScene.Meshes.Add(botonCombustible.meshBoton);
            #endregion

            #region LucesInit
            //Luz?? Sep, si uno hace en el render un lightMesh.render(), te va a mostrar donde se posiciona la luz en el espacio.
            lightMesh = TgcBox.fromSize(new Vector3(5, 5, 5));

            //Pongo al mesh en posicion, activo e AutoTransform
            lightMesh.AutoTransformEnable = true;
            lightMesh.Position = new Vector3(463, 51, 83);
            lightMesh.Color = Color.GreenYellow;

            collisionManager = new SphereCollisionManager();

            playerPos = TgcBox.fromSize(new Vector3(5, 5, 5));

            glowstick = new Linterna();
            glowstick.setSelect(true);
            glowstick.setEnergia(3);//en el caso del glowstick la energia representa el numero de glowsticks que tiene el jugador

            lighter = new Linterna();
            lighter.setSelect(false);
            lighter.setEnergia(100);

            flashlight = new Linterna();
            flashlight.setSelect(false);
            flashlight.setEnergia(100);

            esferaDeLinterna = new TgcBoundingSphere();
            esferaDeLinterna.setValues(lightMesh.Position,10f);
            #endregion

            timer = 0;
        }
        #endregion

        #region Update
        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();
            #region Update del HUD
            vida.Position = new Vector2(20f, D3DDevice.Instance.Height-40f);
            vida.Scaling = new Vector2(8f,0.5f);
            stamina.Position = new Vector2(20f, D3DDevice.Instance.Height - 80f);
            stamina.Scaling = new Vector2(8f, 0.5f);
            centerPoint.Position = new Vector2(D3DDevice.Instance.Width / 2, D3DDevice.Instance.Height / 2);
            centerPoint.Scaling = new Vector2(0.25f,0.25f);
            glowstickHUD1.Position = new Vector2(20f,20f);
            glowstickHUD1.Scaling = new Vector2(0.125f,0.125f);
            glowstickHUD2.Position = new Vector2(20f, glowstickHUD1.Position.Y + 60f);
            glowstickHUD2.Scaling = new Vector2(0.125f, 0.125f);
            glowstickHUD3.Position = new Vector2(20f, glowstickHUD2.Position.Y + 60f);
            glowstickHUD3.Scaling = new Vector2(0.125f, 0.125f);
            lighterHUD.Position = new Vector2(20f, 20f);
            lighterHUD.Scaling = new Vector2(0.0625f, 0.0625f);
            flashlightHUD.Position = new Vector2(20f, 20f);
            flashlightHUD.Scaling = new Vector2(1.5f,1.5f);
            flashlightLiveHUD.Position = new Vector2(25f,35f);
            flashlightLiveHUD.Scaling = new Vector2(8f, 2.5f);

            #endregion

            #region Logica Luces
            //Switch entre glowstick(F), encendedor(G) y linterna(H)
            if (Input.keyPressed(Key.F) && vidaPorcentaje > 0 )
            {
                lightMesh.Color = Color.GreenYellow;
                this.glowstick.setSelect(true);
                this.lighter.setSelect(false);
                this.flashlight.setSelect(false);
                timer = 0;
            }
            if (Input.keyPressed(Key.G) && vidaPorcentaje > 0 )
            {
                this.glowstick.setSelect(false);
                this.lighter.setSelect(true);
                this.flashlight.setSelect(false);
                lightMesh.Color = Color.Yellow;
                timer = 0;
            }
            if (Input.keyPressed(Key.H) && vidaPorcentaje>0 )
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
                float x;
                float y;
                float z;
                //Si no colisiona contra algo es esto
                // lamda * director + coordenada en eje
                x = (float)14 * (Camara.LookAt - Camara.Position).X + Camara.LookAt.X;
                y = (float)14 * (Camara.LookAt - Camara.Position).Y + Camara.LookAt.Y;
                z = (float)14 * (Camara.LookAt - Camara.Position).Z + Camara.LookAt.Z;
                lightMesh.Position = new Vector3(x, y, z);
                lightMesh.Position = chocaLuz(lightMesh, Camara.Position, lightMesh.BoundingBox, objetosColisionables);
                if (System.Math.Truncate(timer) % 1 == 0 &&flashlight.getEnergia()>0)
                {
                    flashlight.perderEnergia(0.041f);
                    flashlightLiveHUD.Scaling = new Vector2((flashlight.getEnergia() / 100) * 8, 2.5f);
                    timer = 0;
                }
            }
            if (!flashlight.getSelect())
            {
                flashlight.ganarEnergia(0.020f);
            }

            #endregion

            #region Accion con botones

            if (Input.keyPressed(Key.E) && distance(Camara.Position,botonElectricidad.meshBoton.Position) < 55)
            {
                botonElectricidad.changeColor(Color.Green);
                botonElectricidad.isGreen = true;
            }

            if (Input.keyPressed(Key.E) && distance(Camara.Position, botonElectricidad2.meshBoton.Position) < 55)
            {
                botonElectricidad2.changeColor(Color.Green);
                botonElectricidad2.isGreen = true;
            }

            if (Input.keyPressed(Key.E) && distance(Camara.Position, botonOxigeno.meshBoton.Position) < 55)
            {
                botonOxigeno.changeColor(Color.Green);
                botonOxigeno.isGreen = true;
            }

            if (Input.keyPressed(Key.E) && distance(Camara.Position, botonCombustible.meshBoton.Position) < 55)
            {
                botonCombustible.changeColor(Color.Green);
                botonCombustible.isGreen = true;
            }

            //Tiene que estar en verde todos los demas botones
            if (Input.keyPressed(Key.E) && (distance(Camara.Position, botonEscapePod1.meshBoton.Position) < 55 || distance(Camara.Position, botonEscapePod2.meshBoton.Position) < 55) && botonCombustible.isGreen && botonElectricidad.isGreen && botonElectricidad2.isGreen && botonOxigeno.isGreen)
            {
                botonEscapePod1.changeColor(Color.Green);
                botonEscapePod2.changeColor(Color.Green);
                //aaaaand We WON!!!
            }

            #endregion

            #region Accion Puertas
            if (Input.keyPressed(Key.E))
            {
                puerta1.abrirPuerta(Camara.Position);
                puerta2.abrirPuerta(Camara.Position);
                puerta3.abrirPuerta(Camara.Position);
                //La misma logica para todasssssss las puertas U__________U
            }

            #endregion

            #region Accion Botiquines
            if(vidaPorcentaje < 100 && Input.keyPressed(Key.E))
            { 
                botiquin1.consumir(Camara.Position);
                botiquin2.consumir(Camara.Position);
                botiquin3.consumir(Camara.Position);
                botiquin4.consumir(Camara.Position);
                botiquin5.consumir(Camara.Position);
                botiquin6.consumir(Camara.Position);
                if (vidaPorcentaje < 80)
                {
                    vidaPorcentaje += 20f;
                }
                else
                {
                    vidaPorcentaje = 100f; 
                }
            }

            #endregion

            #region Logica Personaje

            //Vida
            if (distance(monstruo.Position, Camara.Position) < 50f && vidaPorcentaje > 0)
            {
                vidaPorcentaje -= 0.1f; 
            }

            vida.Scaling = new Vector2((vidaPorcentaje/100) * 8, 0.5f);

            //Stamina
            if (Input.keyDown(Key.LeftShift) && staminaPorcentaje > 0)
            {
                staminaPorcentaje -= 0.4f;
            }
            else
            {
                if (staminaPorcentaje < 100)
                {
                    staminaPorcentaje += 0.1f;
                }
            }
            stamina.Scaling = new Vector2((staminaPorcentaje / 100) * 8, 0.5f);
            #endregion  

            #region Logica Monstruo
            //Para activar o desactivar al monstruo
            if (Input.keyPressed(Key.M)) {
                monstruo.Activo = !monstruo.Activo;
            }

            //Para activar o desactivar colisiones del monstruo
            if (Input.keyPressed(Key.N))
            {
                monstruo.Colisiones = !monstruo.Colisiones;
            }
            
            //Logica del monstruo
            monstruo.update(Camara.Position, objetosColisionables, ElapsedTime);
#endregion

            var camarita = (TGC.Examples.Camara.TgcFpsCamera)Camara;
            camarita.UpdateCamera(ElapsedTime, objetosColisionables,vidaPorcentaje,staminaPorcentaje);
        }
        #endregion

        #region Render
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

            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(vida);
            drawer2D.DrawSprite(stamina);
            drawer2D.DrawSprite(centerPoint);
            if (glowstick.getEnergia() >= 1 && glowstick.getSelect()==true)
            {
                drawer2D.DrawSprite(glowstickHUD1);
                if (glowstick.getEnergia() >= 2)
                {
                    drawer2D.DrawSprite(glowstickHUD2);
                }
                if (glowstick.getEnergia() == 3)
                {
                    drawer2D.DrawSprite(glowstickHUD3);
                }
            }
            if (lighter.getEnergia() > 0 && lighter.getSelect() == true)
            {
                drawer2D.DrawSprite(lighterHUD);
            }
            if (flashlight.getEnergia() > 0 && flashlight.getSelect() == true)
            {
                drawer2D.DrawSprite(flashlightHUD);
                drawer2D.DrawSprite(flashlightLiveHUD);
            }
            
            drawer2D.EndDrawSprite();
            
            if (vidaPorcentaje <= 0)
            {
                textoDeLaMuerte.render();
            }

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
            
            foreach (var mesh in TgcScene.Meshes)
            {
                mesh.Effect = Shader;
                mesh.Technique = TgcShaders.Instance.getTgcMeshTechnique(mesh.RenderType);
            }

            //TgcScene.PortalRendering.updateVisibility(Camara.Position, Frustum);
            
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
                if (glowstick.getSelect() &&  glowstick.getEnergia() == 0)
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
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("materialSpecularExp", 20f);
                }
                if (lighter.getSelect() && lighter.getEnergia() <= 20 && lighter.getEnergia() > 0)
                {
                    if (System.Math.Truncate(lighter.getEnergia()) % 2 == 0)
                    {
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
                        lightMesh.Position = Camara.Position;
                        //Cargar variables shader de la luz
                        mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                        mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                        mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                        mesh.Effect.SetValue("lightIntensity", 15f);
                        mesh.Effect.SetValue("lightAttenuation", 1f);

                        //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                        mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                        mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(lightMesh.Color));
                        mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                        mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.Black));
                        mesh.Effect.SetValue("materialSpecularExp", 20f);
                    }
                }
                if (lighter.getSelect() && System.Math.Truncate(lighter.getEnergia()) == 0)
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
                    x = (float)50 * (Camara.LookAt - Camara.Position).X + Camara.LookAt.X;
                    y = (float)50 * (Camara.LookAt - Camara.Position).Y + Camara.LookAt.Y;
                    z = (float)50 * (Camara.LookAt - Camara.Position).Z + Camara.LookAt.Z;
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
                    
                    //lightMesh.Position = chocaLuz(lightMesh, Camara.Position, lightMesh.BoundingBox, objetosColisionables);
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
                    x = (float)14 * (Camara.LookAt - Camara.Position).X + Camara.LookAt.X;
                    y = (float)14 * (Camara.LookAt - Camara.Position).Y + Camara.LookAt.Y;
                    z = (float)14 * (Camara.LookAt - Camara.Position).Z + Camara.LookAt.Z;
                    lightMesh.Position = new Vector3(x, y, z);
                    //lightMesh.Position = chocaLuz(lightMesh, Camara.Position, lightMesh.BoundingBox, objetosColisionables);
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

                //Renderizar modelo con FrustumCulling
                var r = TgcCollisionUtils.classifyFrustumAABB(Frustum, mesh.BoundingBox);
                if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
                {
                    mesh.render();
                }
                mesh.BoundingBox.render();

            }

            //Renderiza botiquines
            //botiquin1.meshBotiquin.render();

            monstruo.render();
            
            //Dibuja un texto por pantalla
            TGC.Examples.Camara.TgcFpsCamera camaraPrint = (TGC.Examples.Camara.TgcFpsCamera)Camara;
            DrawText.drawText("Use W,A,S,D para desplazarte, Espacio para subir, Control para bajar, Shift para ir mas rapido y el mouse para mover la camara: \n "
                + "Position : " + TgcParserUtils.printVector3(Camara.Position) + "\n"
                + " LookAt : " + TgcParserUtils.printVector3(Camara.LookAt) + "\n"
                + " Light Position : " + TgcParserUtils.printVector3(lightMesh.Position) + "\n"
                + " Monster Position : " + TgcParserUtils.printVector3(monstruo.Position) + "\n"
                + " Camera Bounding Sphere : " + TgcParserUtils.printVector3(camaraPrint.sphereCamara.Position) + "\n"
                + " Glowstick Stock : " + glowstick.getEnergia() + "\n"
                + " Lighter Energy : " + lighter.getEnergia() + "\n"
                + " Flashlight Energy : " + flashlight.getEnergia() + "\n"
                + " M para Monstruo D:" + "\n"
                + " N para activar/desactivar colisiones del Monstruo \n"
                + " L activa colisiones de la camara"
            , 0, 30, Color.OrangeRed);

            /*
            Puerta1.render();
            /*
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
            */
            //lightMesh.render();
            //lightMesh.BoundingBox.render();
            botonEscapePod1.meshBoton.render();
            botonEscapePod2.meshBoton.render();
            botonElectricidad.meshBoton.render();
            botonElectricidad2.meshBoton.render();
            botonOxigeno.meshBoton.render();
            botonCombustible.meshBoton.render();
            TGC.Examples.Camara.TgcFpsCamera camarita = (TGC.Examples.Camara.TgcFpsCamera)Camara;
            camarita.render(ElapsedTime,objetosColisionables);
            
            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            PostRender();
        }
        #endregion

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            //Dispose de una escena.
            //PuertaModelo.dispose();
            /*
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
            */
            
            glowstickHUD1.Dispose();
            glowstickHUD2.Dispose();
            glowstickHUD3.Dispose();
            flashlightHUD.Dispose();
            monstruo.mesh.dispose();
            TgcScene.disposeAll();
            Shader.Dispose();
            textoDeLaMuerte.Dispose();
            vida.Dispose();
            stamina.Dispose();
            centerPoint.Dispose();
        }

        public float distance(Vector3 a, Vector3 b)
        {
            return (FastMath.Sqrt(FastMath.Pow2(a.X - b.X) + FastMath.Pow2(a.Y - b.Y) + FastMath.Pow2(a.Z - b.Z)));
        }

        public Vector3 chocaLuz(TgcBox cajaDeLuz, Vector3 centroCamara, TgcBoundingAxisAlignBox luz , List<TgcBoundingAxisAlignBox> colisionables)
        {
            Vector3 retorno = cajaDeLuz.Position;
            foreach(var colisionable in colisionables)
            {
                if (Core.Collision.TgcCollisionUtils.testAABBAABB(luz, colisionable))
                {
                    if(Core.Collision.TgcCollisionUtils.classifyBoxBox(colisionable, luz)==Core.Collision.TgcCollisionUtils.BoxBoxResult.Atravesando)
                    {

                        Vector3 puntodecolision = Core.Collision.TgcCollisionUtils.closestPointAABB(cajaDeLuz.Position, colisionable);

                        retorno = centroCamara * distance(centroCamara, puntodecolision);
                    }
                    /*
                    if (cajaDeLuz.Position.X + cajaDeLuz.Size.X > retorno.X)
                    {
                        retorno.X = cajaDeLuz.Position.X + cajaDeLuz.Size.X - retorno.X;
                    }
                    if (cajaDeLuz.Position.X + cajaDeLuz.Size.X < retorno.X)
                    {
                        retorno.X = cajaDeLuz.Position.X + cajaDeLuz.Size.X + retorno.X;
                    }

                    if (cajaDeLuz.Position.Y + cajaDeLuz.Size.Y > retorno.Y)
                    {
                        retorno.Y = cajaDeLuz.Position.Y + cajaDeLuz.Size.Y - retorno.Y;
                    }
                    if (cajaDeLuz.Position.Y + cajaDeLuz.Size.Y < retorno.Y)
                    {
                        retorno.Y = cajaDeLuz.Position.Y + cajaDeLuz.Size.Y + retorno.Y;
                    }

                    if (cajaDeLuz.Position.Z + cajaDeLuz.Size.Z > retorno.Z)
                    {
                        retorno.Z = cajaDeLuz.Position.Z + cajaDeLuz.Size.Z - retorno.Z;
                    }
                    if (cajaDeLuz.Position.Z + cajaDeLuz.Size.Z < retorno.Z)
                    {
                        retorno.Z = cajaDeLuz.Position.Z + cajaDeLuz.Size.Z + retorno.Z;
                    }*/
                    //break;
                }
                
            }
                
            return retorno;
        }
    }
}