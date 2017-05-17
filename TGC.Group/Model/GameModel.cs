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

        //Estados del juego
        private GameState StateJuego;
        private GameState StateMenu;
        private GameState StatePause;

        private GameState CurrentState;

        //Caja que se muestra en el ejemplo.
        //usar TgcBox como ejemplo para cargar cualquier caja que queramos.
        private TgcBox Box { get; set; }

        //Escena
        private TgcScene TgcScene { get; set; }
        Celda celdaEscapePod1;
        List<Celda> celdasEscena = new List<Celda>();
        List<Portal> portalesEscena = new List<Portal>();

        //Estado

        #region Menu
        //Menu
        //Bitmaps de botones
        private CustomBitmap boton_normal;
        private CustomBitmap boton_mouseover;


        //Redundancia FTW
        Menu.Menu menu;

        private TgcText2D textoPausa;
        #endregion



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
        private CustomSprite lighterLiveHUD;

        //Flashlight
        private CustomSprite flashlightHUD;
        private CustomSprite flashlightLiveHUD;

        //Hands xD no hubo tiempo para generar manos y elementos en 3D
        private CustomSprite glowstickHand;
        private CustomSprite lighterHand;
        private CustomSprite flashlightHand;

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
        private Puerta puerta7;
        private Puerta puerta8;
        private Puerta puerta9;
        private Puerta puerta10;
        private Puerta puerta11;
        private Puerta puerta12;
        private Puerta puerta13;
        private Puerta puerta14;
        private Puerta puerta15;
        private Puerta puerta16;
        private Puerta puerta17;
        private Puerta puerta18;
        private Puerta puerta19;
        private Puerta puerta20;
        private Puerta puerta21;
        private Puerta puerta22;
        private Puerta puerta23;
        private Puerta puerta24;
        private Puerta puerta25;
        private Puerta puerta26;
        private Puerta puerta27;
        private Puerta puerta28;

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

            #region Init Menu
            menu = new Menu.Menu(Input, "En el espacio Bart se asustó");
            boton_mouseover = new CustomBitmap(MediaDir + "\\Textures\\botonMouseover.png", D3DDevice.Instance.Device);
            boton_normal = new CustomBitmap(MediaDir + "\\Textures\\botonNormal.png", D3DDevice.Instance.Device);

            var botonjugar = new Menu.Button("Jugar",Input, boton_normal, boton_mouseover,
                () => { this.CurrentState = StateJuego; }
            );
            botonjugar.Position = new Vector2(100, 200);
            menu.pushButton(botonjugar);
            #endregion



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

            lighterLiveHUD = new CustomSprite();
            lighterLiveHUD.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\white.bmp", D3DDevice.Instance.Device);
            lighterLiveHUD.Color = Color.Yellow;

            flashlightHUD = new CustomSprite();
            flashlightHUD.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\linternaHUD.png", D3DDevice.Instance.Device);

            flashlightLiveHUD = new CustomSprite();
            flashlightLiveHUD.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\white.bmp", D3DDevice.Instance.Device);

            glowstickHand = new CustomSprite();
            glowstickHand.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\handAndGlowstick.png", D3DDevice.Instance.Device);

            lighterHand = new CustomSprite();
            lighterHand.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\handAndZippo.png", D3DDevice.Instance.Device);

            flashlightHand = new CustomSprite();
            flashlightHand .Bitmap = new CustomBitmap(MediaDir + "\\Textures\\handAndFlashlight.png", D3DDevice.Instance.Device);

            #endregion

            //Seteamos las acciones que se realizan dependiendo del estado del juego
            #region AccionesJuego
            StateJuego = new GameState();
            StateJuego.Update = UpdateGame;
            StateJuego.Render = RenderGame;
            #endregion

            #region AccionesMenu
            StateMenu = new GameState();
            StateMenu.Update = UpdateMenu;
            StateMenu.Render = RenderMenu;
            #endregion
            #region AccionesPausa
            StatePause = new GameState();
            StatePause.Update = UpdatePause;
            StatePause.Render = RenderPause;
            #endregion

            CurrentState = StateMenu;

            //Carga de nivel
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene = loader.loadSceneFromFile(this.MediaDir + "FullLevel-TgcScene.xml", this.MediaDir + "\\");

            celdaEscapePod1 = new Celda();
            celdaEscapePod1.establecerCelda(new Vector3(210,100,170), new Vector3(460,50,105));//Al fin posicion y dimension OK aunque podria llegar hasta el portal =/
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[0]);

            celdasEscena.Add(celdaEscapePod1);

            /*
            //Computamos las normales
            foreach (var mesh in TgcScene.Meshes)
            {
                int[] adjacencia = {1,1,1,1,1,1,1,1,1,1,1,1};
                
                //TODO hay que reordenar normales
                mesh.D3dMesh.ComputeNormals();
            }
            */

            //Carga de puerta y de enemigo
            PuertaModelo = loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0];

            MonstruoModelo = loader.loadSceneFromFile(this.MediaDir + "\\Monstruo-TgcScene.xml").Meshes[0];

            #region PuertasInit
            puerta1 = new Puerta();
            puerta1.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta1.changePosition(new Vector3(89f, 32f, 275f));
            puerta1.setPosicionOriginal(new Vector3(89f, 32f, 275f));
            TgcScene.Meshes.Add(puerta1.getMesh());
           
            //TgcPortalRenderingPortal puerta1Portal = new TgcPortalRenderingPortal("puerta1Portal", puerta1.getMesh().BoundingBox);
            //TgcScene.PortalRendering.Portals.Add(puerta1Portal);

            puerta2 = new Puerta();
            puerta2.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta2.changePosition(new Vector3(439f, 32f, 203f));
            puerta2.setPosicionOriginal(new Vector3(439f, 32f, 203f));
            TgcScene.Meshes.Add(puerta2.getMesh());
            //TgcScene.PortalRendering.Portals.Add(new Core.PortalRendering.TgcPortalRenderingPortal("puerta2", puerta2.getMesh().BoundingBox));
     
            puerta3 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta3.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta3.getMesh().move(new Vector3(201f, 32f, 1570f));
            puerta3.getMesh().rotateY(FastMath.PI_HALF);
            puerta3.getMesh().UpdateMeshTransform();
            TgcScene.Meshes.Add(puerta3.getMesh());

            puerta4 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta4.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta4.getMesh().move(new Vector3(452f, 32f, 1221f));
            puerta4.getMesh().rotateY(FastMath.PI_HALF);
            puerta4.getMesh().UpdateMeshTransform();
            TgcScene.Meshes.Add(puerta4.getMesh());

            puerta5 = new Puerta();
            puerta5.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta5.changePosition(new Vector3(459f, 32f, 1675f));
            TgcScene.Meshes.Add(puerta5.getMesh());

            puerta6 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta6.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta6.getMesh().move(new Vector3(734f, 32f, 1570f));
            puerta6.getMesh().rotateY(FastMath.PI_HALF);
            puerta6.getMesh().UpdateMeshTransform();
            TgcScene.Meshes.Add(puerta6.getMesh());

            puerta7 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta7.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta7.getMesh().move(new Vector3(915f, 32f, 751f));
            puerta7.getMesh().rotateY(FastMath.PI_HALF);
            puerta7.getMesh().UpdateMeshTransform();
            puerta7.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta7.getMesh());

            puerta8 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta8.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta8.getMesh().move(new Vector3(915f, 32f, 751f));
            puerta8.getMesh().rotateY(FastMath.PI_HALF);
            puerta8.getMesh().UpdateMeshTransform();
            puerta8.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta8.getMesh());

            puerta9 = new Puerta();
            puerta9.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta9.changePosition(new Vector3(469f, 32f, 921f));
            TgcScene.Meshes.Add(puerta9.getMesh());

            puerta10 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta10.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta10.getMesh().move(new Vector3(915f, 32f, 751f));
            puerta10.getMesh().rotateY(FastMath.PI_HALF);
            puerta10.getMesh().UpdateMeshTransform();
            puerta10.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta10.getMesh());

            puerta11 = new Puerta();
            puerta11.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta11.changePosition(new Vector3(399f, 32f, 724f));
            TgcScene.Meshes.Add(puerta11.getMesh());

            puerta12 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta12.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta12.getMesh().move(new Vector3(454f, 32f, 331f));
            puerta12.getMesh().rotateY(FastMath.PI_HALF);
            puerta12.getMesh().UpdateMeshTransform();
            puerta12.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta12.getMesh());

            puerta13 = new Puerta();
            puerta13.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta13.changePosition(new Vector3(399f, 32f, 1292f));
            TgcScene.Meshes.Add(puerta13.getMesh());

            puerta14 = new Puerta();
            puerta14.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta14.changePosition(new Vector3(89f, 32f, 922f));
            TgcScene.Meshes.Add(puerta14.getMesh());

            puerta15 = new Puerta();
            puerta15.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta15.changePosition(new Vector3(89f, 142f, 275f));
            TgcScene.Meshes.Add(puerta15.getMesh());

            puerta16 = new Puerta();
            puerta16.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta16.changePosition(new Vector3(439f, 142f, 203f));
            TgcScene.Meshes.Add(puerta16.getMesh());

            puerta17 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta17.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta17.getMesh().move(new Vector3(201f, 142f, 1570f));
            puerta17.getMesh().rotateY(FastMath.PI_HALF);
            puerta17.getMesh().UpdateMeshTransform();
            puerta17.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta17.getMesh());

            puerta18 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta18.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta18.getMesh().move(new Vector3(452f, 142f, 1221f));
            puerta18.getMesh().rotateY(FastMath.PI_HALF);
            puerta18.getMesh().UpdateMeshTransform();
            puerta18.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta18.getMesh());

            puerta19 = new Puerta();
            puerta19.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta19.changePosition(new Vector3(459f, 142f, 1675f));
            TgcScene.Meshes.Add(puerta19.getMesh());

            puerta20 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta20.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta20.getMesh().move(new Vector3(734f, 142f, 1570f));
            puerta20.getMesh().rotateY(FastMath.PI_HALF);
            puerta20.getMesh().UpdateMeshTransform();
            puerta20.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta20.getMesh());

            puerta21 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta21.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta21.getMesh().move(new Vector3(915f, 142f, 751f));
            puerta21.getMesh().rotateY(FastMath.PI_HALF);
            puerta21.getMesh().UpdateMeshTransform();
            puerta21.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta21.getMesh());

            puerta22 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta22.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta22.getMesh().move(new Vector3(695f, 142f, 600f));
            puerta22.getMesh().rotateY(FastMath.PI_HALF);
            puerta22.getMesh().UpdateMeshTransform();
            puerta22.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta22.getMesh());

            puerta23 = new Puerta();
            puerta23.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta23.changePosition(new Vector3(469f, 142f, 921f));
            TgcScene.Meshes.Add(puerta23.getMesh());

            puerta24 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta24.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta24.getMesh().move(new Vector3(695f, 142f, 912f));
            puerta24.getMesh().rotateY(FastMath.PI_HALF);
            puerta24.getMesh().UpdateMeshTransform();
            puerta24.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta24.getMesh());

            puerta25 = new Puerta();
            puerta25.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta25.changePosition(new Vector3(399f, 142f, 724f));
            TgcScene.Meshes.Add(puerta25.getMesh());

            puerta26 = new Puerta();//estas puertas necesitan un modelo rotado sino no funcionan bien las coliciones
            puerta26.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta26.getMesh().move(new Vector3(454f, 142f, 331f));
            puerta26.getMesh().rotateY(FastMath.PI_HALF);
            puerta26.getMesh().UpdateMeshTransform();
            puerta26.getMesh().createBoundingBox();
            TgcScene.Meshes.Add(puerta26.getMesh());

            puerta27 = new Puerta();
            puerta27.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta27.changePosition(new Vector3(399f, 142f, 1292f));
            TgcScene.Meshes.Add(puerta27.getMesh());

            puerta28 = new Puerta();
            puerta28.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta28.changePosition(new Vector3(89f, 142f, 922f));
            TgcScene.Meshes.Add(puerta28.getMesh());

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
            textoDeLaMuerte.Position = new Point(D3DDevice.Instance.Width / 12, D3DDevice.Instance.Height / 2);
            textoDeLaMuerte.changeFont(new System.Drawing.Font("TimesNewRoman", 55));
            #endregion
            #region Texto de Pausa
            textoPausa = new TgcText2D();
            textoPausa.Text = "PAUSA";
            textoPausa.Color = Color.Gray;
            textoPausa.Position = new Point(D3DDevice.Instance.Width / 12, D3DDevice.Instance.Height / 2);
            textoPausa.changeFont(new System.Drawing.Font("TimesNewRoman", 55));
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
            CurrentState.Update();
        }
        #endregion

        public void UpdateGame() {
            #region Update del HUD
            vida.Position = new Vector2(20f, D3DDevice.Instance.Height - 40f);
            vida.Scaling = new Vector2(8f, 0.5f);
            stamina.Position = new Vector2(20f, D3DDevice.Instance.Height - 80f);
            stamina.Scaling = new Vector2(8f, 0.5f);
            centerPoint.Position = new Vector2(D3DDevice.Instance.Width / 2, D3DDevice.Instance.Height / 2);
            centerPoint.Scaling = new Vector2(0.25f, 0.25f);
            glowstickHUD1.Position = new Vector2(20f, 20f);
            glowstickHUD1.Scaling = new Vector2(0.125f, 0.125f);
            glowstickHUD2.Position = new Vector2(20f, glowstickHUD1.Position.Y + 60f);
            glowstickHUD2.Scaling = new Vector2(0.125f, 0.125f);
            glowstickHUD3.Position = new Vector2(20f, glowstickHUD2.Position.Y + 60f);
            glowstickHUD3.Scaling = new Vector2(0.125f, 0.125f);
            lighterHUD.Position = new Vector2(20f, 20f);
            lighterHUD.Scaling = new Vector2(0.0625f, 0.0625f);
            lighterLiveHUD.Position = new Vector2(10f, 35f);
            lighterLiveHUD.Scaling = new Vector2(2.0f, 15f);
            flashlightHUD.Position = new Vector2(20f, 20f);
            flashlightHUD.Scaling = new Vector2(1.5f, 1.5f);
            flashlightLiveHUD.Position = new Vector2(25f, 35f);
            flashlightLiveHUD.Scaling = new Vector2(8f, 2.5f);
            glowstickHand.Position = new Vector2(D3DDevice.Instance.Width / 2 - 100f, D3DDevice.Instance.Height - 400f);
            glowstickHand.Scaling = new Vector2(0.167f, 0.125f);
            lighterHand.Position = new Vector2(D3DDevice.Instance.Width / 2 - 100f, D3DDevice.Instance.Height - 400f);
            lighterHand.Scaling = new Vector2(0.167f, 0.125f);
            flashlightHand.Position = new Vector2(D3DDevice.Instance.Width / 2 - 100f, D3DDevice.Instance.Height - 400f);
            flashlightHand.Scaling = new Vector2(0.167f, 0.125f);
            #endregion

            #region Logica Luces
            //Switch entre glowstick(F), encendedor(G) y linterna(H)
            if (Input.keyPressed(Key.F) && vidaPorcentaje > 0)
            {
                lightMesh.Color = Color.GreenYellow;
                this.glowstick.setSelect(true);
                this.lighter.setSelect(false);
                this.flashlight.setSelect(false);
                timer = 0;
            }
            if (Input.keyPressed(Key.G) && vidaPorcentaje > 0)
            {
                this.glowstick.setSelect(false);
                this.lighter.setSelect(true);
                this.flashlight.setSelect(false);
                lightMesh.Color = Color.Yellow;
                timer = 0;
            }
            if (Input.keyPressed(Key.H) && vidaPorcentaje > 0)
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
                if (System.Math.Truncate(timer) / 60 == 1 && glowstick.getEnergia() > 0)
                {
                    glowstick.perderEnergia(1);
                    timer = 0;
                }
            }
            if (lighter.getSelect())
            {
                if (System.Math.Truncate(timer) % 1 == 0 && lighter.getEnergia() > 0)
                {
                    lighter.perderEnergia(0.083f);
                    lighterLiveHUD.Scaling = new Vector2(2, (lighter.getEnergia() / 100) * 15);
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
                if (System.Math.Truncate(timer) % 1 == 0 && flashlight.getEnergia() > 0)
                {
                    flashlight.perderEnergia(0.041f);
                    flashlightLiveHUD.Scaling = new Vector2((flashlight.getEnergia() / 100) * 8, 2.5f);
                    timer = 0;
                }
            }
            if (!flashlight.getSelect() && flashlight.getEnergia() < 100)
            {
                flashlight.ganarEnergia(0.020f);
            }

            #endregion

            #region Accion con botones

            if (Input.keyPressed(Key.E) && distance(Camara.Position, botonElectricidad.meshBoton.Position) < 55)
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
                puerta4.abrirPuerta(Camara.Position);
                puerta5.abrirPuerta(Camara.Position);
                puerta6.abrirPuerta(Camara.Position);
                //La misma logica para todasssssss las puertas U__________U
            }

            //Esta forma complica que se cierren las puertas
            /*
            if (Input.keyPressed(Key.E)&& !closed)
            {
                puerta1.cerrarPuerta(true);
                puerta2.cerrarPuerta(true);
                puerta3.cerrarPuerta(true);
                puerta4.cerrarPuerta(true);
                puerta5.cerrarPuerta(true);
                puerta6.cerrarPuerta(true);
            }
            */
            #endregion

            #region Accion Botiquines
            if (vidaPorcentaje < 100 && Input.keyDown(Key.E) && distance(Camara.Position, botiquin1.Position) < 80)
            {
                botiquin1.consumir(Camara.Position);
                if (vidaPorcentaje < 80)
                {
                    vidaPorcentaje += 20f;
                }
                else
                {
                    vidaPorcentaje = 100f;
                }
            }
            if (vidaPorcentaje < 100 && Input.keyPressed(Key.E) && distance(Camara.Position, botiquin2.Position) < 80)
            {
                botiquin2.consumir(Camara.Position);
                if (vidaPorcentaje < 80)
                {
                    vidaPorcentaje += 20f;
                }
                else
                {
                    vidaPorcentaje = 100f;
                }
            }
            if (vidaPorcentaje < 100 && Input.keyPressed(Key.E) && distance(Camara.Position, botiquin3.Position) < 80)
            {
                botiquin3.consumir(Camara.Position);
                if (vidaPorcentaje < 80)
                {
                    vidaPorcentaje += 20f;
                }
                else
                {
                    vidaPorcentaje = 100f;
                }
            }
            if (vidaPorcentaje < 100 && Input.keyPressed(Key.E) && distance(Camara.Position, botiquin4.Position) < 80)
            {
                botiquin4.consumir(Camara.Position);
                if (vidaPorcentaje < 80)
                {
                    vidaPorcentaje += 20f;
                }
                else
                {
                    vidaPorcentaje = 100f;
                }
            }
            if (vidaPorcentaje < 100 && Input.keyPressed(Key.E) && distance(Camara.Position, botiquin5.Position) < 80)
            {
                botiquin5.consumir(Camara.Position);
                if (vidaPorcentaje < 80)
                {
                    vidaPorcentaje += 20f;
                }
                else
                {
                    vidaPorcentaje = 100f;
                }
            }
            if (vidaPorcentaje < 100 && Input.keyPressed(Key.E) && distance(Camara.Position, botiquin6.Position) < 80)
            {
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
                vidaPorcentaje -= 0.2f;
            }

            vida.Scaling = new Vector2((vidaPorcentaje / 100) * 8, 0.5f);

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
            //Pausa
            if (Input.keyPressed(Key.Return))
            {
                CurrentState = StatePause;
            }

            #endregion


            #region Logica Monstruo
            //Para activar o desactivar al monstruo
            if (Input.keyPressed(Key.M))
            {
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
            camarita.UpdateCamera(ElapsedTime, objetosColisionables, vidaPorcentaje, staminaPorcentaje);

        }

        public void UpdateMenu() { menu.Update(ElapsedTime); }

        public void UpdatePause()
        {
            if (Input.keyPressed(Key.Return)) CurrentState=StateJuego;
        }
        #region Render
        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            PreRender();
            CurrentState.Render();
            PostRender();
        }
        #endregion

        public void RenderGame() {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            timer += ElapsedTime;

            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(vida);
            drawer2D.DrawSprite(stamina);
            drawer2D.DrawSprite(centerPoint);
            if (glowstick.getEnergia() >= 1 && glowstick.getSelect())
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
            if (glowstick.getSelect())
            {
                drawer2D.DrawSprite(glowstickHand);
            }
            if (lighter.getEnergia() > 0 && lighter.getSelect())
            {
                drawer2D.DrawSprite(lighterHUD);
            }
            if (lighter.getSelect())
            {
                drawer2D.DrawSprite(lighterHand);
            }
            if (lighter.getSelect() && lighter.getEnergia() > 0)
            {
                drawer2D.DrawSprite(lighterLiveHUD);
            }
            if (flashlight.getEnergia() > 0 && flashlight.getSelect())
            {
                drawer2D.DrawSprite(flashlightHUD);
                drawer2D.DrawSprite(flashlightLiveHUD);
            }
            if (flashlight.getSelect())
            {
                drawer2D.DrawSprite(flashlightHand);
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

                    //celdaEscapePod1.render(Camara.Position);
                    /**/
                    if (mesh.Position.Y < Camara.Position.Y + 60f)
                    {
                        mesh.render();
                    }
                    else
                    {
                        if (mesh.Position.Y >= Camara.Position.Y - 50f)
                        {
                            mesh.render();
                        }
                    }/**/

                }
                //mesh.BoundingBox.render();

            }

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

            //lightMesh.render();
            //lightMesh.BoundingBox.render();
            botonEscapePod1.meshBoton.render();
            botonEscapePod2.meshBoton.render();
            botonElectricidad.meshBoton.render();
            botonElectricidad2.meshBoton.render();
            botonOxigeno.meshBoton.render();
            botonCombustible.meshBoton.render();
            TGC.Examples.Camara.TgcFpsCamera camarita = (TGC.Examples.Camara.TgcFpsCamera)Camara;
            camarita.render(ElapsedTime, objetosColisionables);

            
        }
        public void RenderPause() {
            RenderGame();
            textoPausa.render();
        }
        public void RenderMenu() {
            menu.Render(ElapsedTime, this.drawer2D);
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            //Dispose de una escena.          
            glowstickHUD1.Dispose();
            glowstickHUD2.Dispose();
            glowstickHUD3.Dispose();
            flashlightHUD.Dispose();
            monstruo.mesh.dispose();
            TgcScene.disposeAll();
            if(Shader!=null) Shader.Dispose();
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