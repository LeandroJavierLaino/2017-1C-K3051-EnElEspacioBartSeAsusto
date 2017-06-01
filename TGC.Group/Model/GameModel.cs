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
using TGC.Core.Sound;

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

		//Coleccion de fuentes privadas (se cargan desde archivos)
		private System.Drawing.Text.PrivateFontCollection Fonts;

        //Estados del juego
        private GameState StateGame;
        private GameState StateMenu;
        private GameState StatePause;
		private GameState StateHowToPlay;

        private GameState CurrentState;

        //Caja que se muestra en el ejemplo.
        //usar TgcBox como ejemplo para cargar cualquier caja que queramos.
        private TgcBox Box { get; set; }

        //Escena
        private TgcScene TgcScene { get; set; }
        
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
		Menu.Menu menuHowToPlay;

		private TgcText2D textoPausa;
		private TgcText2D textoHowToPlay;
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
        private List<Boton> botones = new List<Boton>();

        private List<TgcMesh> puertas = new List<TgcMesh>();

        private Monstruo monstruo { get; set; }
        private SphereCollisionManager collisionManager;
        private TgcBoundingSphere esferaDeLinterna;

        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }
        
        private Microsoft.DirectX.Direct3D.Effect Shader { get; set; }
        private Microsoft.DirectX.Direct3D.Effect ShaderBoton { get; set; }
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
        private List<TgcMesh> botiquines = new List<TgcMesh>();
        //Sound and Music
        private TgcMp3Player mp3Player;
        private TgcStaticSound soundBoton;
        private TgcStaticSound soundPuerta;
        private TgcStaticSound soundHeartBeat;
        private TgcStaticSound soundAmbience;
        private TgcStaticSound soundWalk;
        private TgcStaticSound soundLoseLife;
        private Tgc3dSound sound3dMotor;
        private Tgc3dSound sound3DMonster;
        private float tiempoPaso = 0;
        private float tiempoGolpe = 0;

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
            mp3Player = new TgcMp3Player();
            soundBoton = new TgcStaticSound();
            soundBoton.dispose();
            soundPuerta = new TgcStaticSound();
            soundPuerta.dispose();
            soundHeartBeat = new TgcStaticSound();
            soundHeartBeat.dispose();
            soundAmbience = new TgcStaticSound();
            soundAmbience.dispose();
            soundWalk = new TgcStaticSound();
            soundWalk.dispose();
            soundLoseLife = new TgcStaticSound();
            soundLoseLife.dispose();
            //Convertir de Stereo a Mono, no funciona
            //sound3dMotor = new Tgc3dSound(MediaDir + "Sounds\\lab_loop1.wav", new Vector3(575,50,705),DirectSound.DsDevice);
            //sound3dMotor.MinDistance = 130f;
            sound3DMonster = new Tgc3dSound(MediaDir + "Sounds\\alert1.wav", new Vector3(), DirectSound.DsDevice);
            sound3DMonster.MinDistance = 130f;
            #region Fonts
            Fonts = new System.Drawing.Text.PrivateFontCollection();
            Fonts.AddFontFile(MediaDir + "\\Fonts\\coldnightforalligators.ttf");
            //Fonts.AddFontFile(MediaDir + "\\Fonts\\murderous desire DEMO.otf");//esta fuente no funca en la maquina del laburo pero es la que va xD
            #endregion

            Camara = new Examples.Camara.TgcFpsCamera(new Vector3(463, 55.2f, 83), 125f, 100f, Input);
            var d3dDevice = D3DDevice.Instance.Device;

            soundBoton.loadSound(MediaDir + "Sounds\\button9.wav", DirectSound.DsDevice);
            soundPuerta.loadSound(MediaDir + "Sounds\\doormove3.wav", DirectSound.DsDevice);
            soundHeartBeat.loadSound(MediaDir + "Sounds\\heartbeat1.wav", DirectSound.DsDevice);
            soundAmbience.loadSound(MediaDir + "Sounds\\ambience_base.wav", DirectSound.DsDevice);
            
            soundLoseLife.loadSound(MediaDir + "Sounds\\pl_pain5.wav", DirectSound.DsDevice);

            #region Init Menu
            {
                menu = new Menu.Menu(Input, "En el espacio Bart se asustó", Fonts.Families[0]);
                boton_mouseover = new CustomBitmap(MediaDir + "\\Textures\\botonMouseover.png", D3DDevice.Instance.Device);
                boton_normal = new CustomBitmap(MediaDir + "\\Textures\\botonNormal.png", D3DDevice.Instance.Device);

                var botonJugar = new Menu.Button("Jugar", Input, boton_normal, boton_mouseover,
                    () => { this.CurrentState = StateGame; }
                , Fonts.Families[0]);
                botonJugar.Position = new Vector2(100, 200);
                menu.pushButton(botonJugar);

                var botonComoJugar = new Menu.Button("Como Jugar", Input, boton_normal, boton_mouseover,
                    () => { this.CurrentState = StateHowToPlay; }
                , Fonts.Families[0]);
                botonComoJugar.Position = new Vector2(100, 275);
                menu.pushButton(botonComoJugar);
            }

            {
                menuHowToPlay = new Menu.Menu(Input, "Cómo Jugar", Fonts.Families[0]);
                boton_mouseover = new CustomBitmap(MediaDir + "\\Textures\\botonMouseover.png", D3DDevice.Instance.Device);
                boton_normal = new CustomBitmap(MediaDir + "\\Textures\\botonNormal.png", D3DDevice.Instance.Device);

                var botonjugar = new Menu.Button("Jugar", Input, boton_normal, boton_mouseover,
                    () => { this.CurrentState = StateGame; }
                , Fonts.Families[0]);

                botonjugar.Position = new Vector2(100, D3DDevice.Instance.Height / 4);

                menuHowToPlay.pushButton(botonjugar);
            }
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
            flashlightHand.Bitmap = new CustomBitmap(MediaDir + "\\Textures\\handAndFlashlight.png", D3DDevice.Instance.Device);

            #endregion

            //Seteamos las acciones que se realizan dependiendo del estado del juego
            #region AccionesJuego
            StateGame = new GameState();
            StateGame.Update = UpdateGame;
            StateGame.Render = RenderGame;
            #endregion

            #region AccionesMenu
            StateMenu = new GameState();
            StateMenu.Update = UpdateMenu;
            StateMenu.Render = RenderMenu;
            #endregion

            #region AccionesHowToPlay
            StateHowToPlay = new GameState();
            StateHowToPlay.Update = UpdateHowToPlay;
            StateHowToPlay.Render = RenderHowToPlay;
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

            #region Portal Rendering

            float centro1Floor = 50;
            float altura = 100;
            float centro2Floor = 160;
            float alturaEscalera = 210;
            float centroEscalera = 105;

            #region Celda Escape Pod 1er Piso
            //Creamos y seteamos la celda
            Celda celdaEscapePod1 = new Celda();
            celdaEscapePod1.establecerCelda(new Vector3(210, altura, 180), new Vector3(460, centro1Floor, 105));//Al fin posicion y dimension OK 
            //Meshes asociados a la celda
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[211]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[212]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[213]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[214]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[215]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[216]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[217]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[531]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[210]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[209]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[208]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[207]);
            celdaEscapePod1.agregarMesh(TgcScene.Meshes[578]);
            #endregion

            #region Celda 1 Primer Piso
            //Creamos y seteamos la celda
            Celda celdaOne1Floor = new Celda();
            celdaOne1Floor.establecerCelda(new Vector3(970, altura, 350), new Vector3(490, centro1Floor, 352));
            //Meshes de la celda
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[0]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[1]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[2]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[3]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[4]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[6]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[17]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[18]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[19]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[20]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[21]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[22]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[23]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[24]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[25]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[26]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[37]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[38]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[39]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[40]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[41]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[42]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[43]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[44]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[45]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[46]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[47]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[48]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[49]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[50]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[51]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[52]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[53]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[54]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[55]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[56]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[57]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[58]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[59]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[60]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[61]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[62]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[63]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[64]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[65]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[66]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[67]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[68]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[69]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[70]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[71]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[72]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[73]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[74]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[75]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[76]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[77]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[78]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[79]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[218]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[219]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[220]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[221]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[222]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[223]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[224]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[225]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[226]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[227]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[316]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[501]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[502]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[503]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[504]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[505]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[506]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[507]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[532]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[539]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[540]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[541]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[542]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[543]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[544]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[545]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[546]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[547]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[548]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[549]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[550]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[551]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[552]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[553]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[554]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[555]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[558]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[559]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[560]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[561]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[587]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[588]);
            celdaOne1Floor.agregarMesh(TgcScene.Meshes[623]);
            #endregion

            #region Portal entre la Celda 1 del 1er Piso y el Escape Pod del 1er Piso
            Portal portalEscapeOne = new Portal();
            portalEscapeOne.establecerPortal(new Vector3(60, altura, 10), new Vector3(462,centro1Floor,202), celdaEscapePod1, celdaOne1Floor);
            celdaEscapePod1.agregarPortal(portalEscapeOne);
            celdaOne1Floor.agregarPortal(portalEscapeOne);
            portalesEscena.Add(portalEscapeOne);
            celdasEscena.Add(celdaEscapePod1);
            celdasEscena.Add(celdaOne1Floor);
            #endregion

            Celda celdaTwo1Floor = new Celda();
            celdaTwo1Floor.establecerCelda(new Vector3(550,altura,450),new Vector3(694,centro1Floor,769));
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[0]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[1]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[2]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[4]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[5]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[7]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[9]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[16]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[8]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[107]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[119]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[122]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[120]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[105]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[627]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[110]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[628]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[106]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[109]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[523]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[509]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[104]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[108]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[638]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[633]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[629]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[632]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[630]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[631]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[111]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[113]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[112]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[114]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[115]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[117]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[116]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[118]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[121]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[94]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[96]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[95]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[99]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[634]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[508]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[98]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[624]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[626]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[97]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[528]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[524]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[527]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[525]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[526]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[529]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[638]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[535]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[100]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[102]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[101]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[103]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[15]);
            celdaTwo1Floor.agregarMesh(TgcScene.Meshes[13]);
            //Meshes 
            celdasEscena.Add(celdaTwo1Floor);

            Portal portalOneTwo1Floor = new Portal();
            portalOneTwo1Floor.establecerPortal(new Vector3(60, altura, 10), new Vector3(946, centro1Floor, 535), celdaOne1Floor, celdaTwo1Floor);
            portalesEscena.Add(portalOneTwo1Floor);
            celdaTwo1Floor.agregarPortal(portalOneTwo1Floor);
            celdaOne1Floor.agregarPortal(portalOneTwo1Floor);

            Celda celdaThree1Floor = new Celda();
            celdaThree1Floor.establecerCelda(new Vector3(440,altura,450),new Vector3(0,centro1Floor,0));
            //Meshes
            celdasEscena.Add(celdaThree1Floor);

            Portal portalOneThree1Floor = new Portal();
            portalOneThree1Floor.establecerPortal(new Vector3(60, altura, 10), new Vector3(0, centro1Floor, 0), celdaOne1Floor, celdaThree1Floor);
            portalesEscena.Add(portalOneThree1Floor);

            Portal portalTwoThree1Floor = new Portal();
            portalTwoThree1Floor.establecerPortal(new Vector3(10, altura, 60), new Vector3(0, centro1Floor, 0), celdaOne1Floor, celdaThree1Floor);
            portalesEscena.Add(portalTwoThree1Floor);

            Celda celdaFour1Floor = new Celda();
            celdaFour1Floor.establecerCelda(new Vector3(420,altura,310),new Vector3(0,centro1Floor,0));
            //Meshes
            celdasEscena.Add(celdaFour1Floor);

            Celda celdaFive1Floor = new Celda();
            celdaFive1Floor.establecerCelda(new Vector3(660,altura,440),new Vector3(0,centro1Floor,0));
            //Meshes
            celdasEscena.Add(celdaFive1Floor);

            Celda celdaEscalera7 = new Celda();
            celdaEscalera7.establecerCelda(new Vector3(180,alturaEscalera,270),new Vector3(0,centroEscalera,0));
            //Meshes
            celdasEscena.Add(celdaEscalera7);

            Portal portal1FloorEscalera7 = new Portal();
            portal1FloorEscalera7.establecerPortal(new Vector3(60, altura, 10), new Vector3(0, centro1Floor, 0), celdaFive1Floor, celdaEscalera7);
            portalesEscena.Add(portal1FloorEscalera7);

            #region Celda Escalera 8
            //Creamos y seteamos la Celda
            Celda celdaEscalera8 = new Celda();
            celdaEscalera8.establecerCelda(new Vector3(270,alturaEscalera,180),new Vector3(1135,centroEscalera,275));
            //Meshes asociados a la celda
            celdaEscalera8.agregarMesh(TgcScene.Meshes[228]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[229]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[230]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[231]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[232]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[233]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[234]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[235]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[236]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[474]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[475]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[476]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[477]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[478]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[479]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[480]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[481]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[482]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[579]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[596]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[597]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[598]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[599]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[600]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[601]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[602]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[603]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[604]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[605]);
            celdaEscalera8.agregarMesh(TgcScene.Meshes[537]);
            celdasEscena.Add(celdaEscalera8);
            #endregion

            #region Portal 1er Piso y Escalera 8
            Portal portal1FloorEscalera8 = new Portal();
            portal1FloorEscalera8.establecerPortal(new Vector3(10, altura, 60), new Vector3(980, centro1Floor, 238), celdaOne1Floor, celdaEscalera8);
            portalesEscena.Add(portal1FloorEscalera8);
            celdaOne1Floor.agregarPortal(portal1FloorEscalera8);
            celdaEscalera8.agregarPortal(portal1FloorEscalera8);
            #endregion

            Celda celdaEscapePod2 = new Celda();
            celdaEscapePod2.establecerCelda(new Vector3(210, altura, 180), new Vector3(460, centro2Floor, 105));//Al fin posicion y dimension OK 
            celdaEscapePod2.agregarMesh(TgcScene.Meshes[0]);
            //Meshes
            celdasEscena.Add(celdaEscapePod2);

            Celda celdaOne2Floor = new Celda();
            celdaOne2Floor.establecerCelda(new Vector3(970, altura, 330), new Vector3(0, centro2Floor, 0));//falta centro XZ
            //Meshes 
            celdasEscena.Add(celdaOne2Floor);

            Portal portalEscapeOne2 = new Portal();
            portalEscapeOne2.establecerPortal(new Vector3(60,altura,10), new Vector3(0,centro2Floor,0), celdaEscapePod1, celdaOne1Floor);
            portalesEscena.Add(portalEscapeOne2);

            Celda celdaTwo2Floor = new Celda();
            celdaTwo2Floor.establecerCelda(new Vector3(550, altura, 450), new Vector3(0, centro2Floor, 0));
            //Meshes 
            celdasEscena.Add(celdaTwo2Floor);

            Celda celdaThree2Floor = new Celda();
            celdaThree2Floor.establecerCelda(new Vector3(440, altura, 450), new Vector3(0, centro2Floor, 0));
            //Meshes
            celdasEscena.Add(celdaThree2Floor);

            Celda celdaFour2Floor = new Celda();
            celdaFour2Floor.establecerCelda(new Vector3(420, altura, 310), new Vector3(0, centro2Floor, 0));
            //Meshes
            celdasEscena.Add(celdaFour2Floor);

            Celda celdaFive2Floor = new Celda();
            celdaFive2Floor.establecerCelda(new Vector3(660, altura, 440), new Vector3(0, centro2Floor, 0));
            //Meshes
            celdasEscena.Add(celdaFive2Floor);
            #endregion

            //se recalculan las normales
            foreach (var mesh in TgcScene.Meshes)
            {
                int[] adj = new int[mesh.D3dMesh.NumberFaces * 3];
                mesh.D3dMesh.GenerateAdjacency(0, adj);
                mesh.D3dMesh.ComputeNormals(adj);
            }

            //Carga de puerta y de enemigo

            MonstruoModelo = loader.loadSceneFromFile(this.MediaDir + "\\Monstruo-TgcScene.xml").Meshes[0];

            #region PuertasInit
            puerta1 = new Puerta();
            puerta1.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta1.changePosition(new Vector3(89f, 32f, 275f));
            puertas.Add(puerta1.getMesh());

            puerta2 = new Puerta();
            puerta2.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta2.changePosition(new Vector3(439f, 32f, 203f));
            puertas.Add(puerta2.getMesh());

            puerta3 = new Puerta();
            puerta3.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta3.getMesh().move(new Vector3(201f-15f, 32f, 1570f-22f));
            puerta3.getMesh().UpdateMeshTransform();
            puertas.Add(puerta3.getMesh());

            puerta4 = new Puerta();
            puerta4.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta4.getMesh().move(new Vector3(452f-15f, 32f, 1221f-22f));
            puerta4.getMesh().UpdateMeshTransform();
            puertas.Add(puerta4.getMesh());

            puerta5 = new Puerta();
            puerta5.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta5.changePosition(new Vector3(459f, 32f, 1675f));
            puertas.Add(puerta5.getMesh());

            puerta6 = new Puerta();
            puerta6.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta6.getMesh().move(new Vector3(734f-15f, 32f, 1570f-22f));
            puerta6.getMesh().UpdateMeshTransform();
            puertas.Add(puerta6.getMesh());

            puerta7 = new Puerta();
            puerta7.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta7.getMesh().move(new Vector3(915f-20f, 32f, 751f-22f));
            puerta7.getMesh().UpdateMeshTransform();
            puertas.Add(puerta7.getMesh());
            
            puerta8 = new Puerta();
            puerta8.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta8.getMesh().move(new Vector3(695f-20f, 32f, 578f));
            puerta8.getMesh().UpdateMeshTransform();
            puertas.Add(puerta8.getMesh());
            
            puerta9 = new Puerta();
            puerta9.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta9.changePosition(new Vector3(469f, 32f, 921f));
            puertas.Add(puerta9.getMesh());
            
            puerta10 = new Puerta();
            puerta10.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta10.getMesh().move(new Vector3(695f-20f, 32f, 886f));
            puerta10.getMesh().UpdateMeshTransform();
            puertas.Add(puerta10.getMesh());
            
            puerta11 = new Puerta();
            puerta11.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta11.changePosition(new Vector3(399f, 32f, 724f));
            puertas.Add(puerta11.getMesh());

            puerta12 = new Puerta();
            puerta12.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta12.getMesh().move(new Vector3(454f-15f, 32f, 331f-22f));
            puerta12.getMesh().UpdateMeshTransform();
            puertas.Add(puerta12.getMesh());

            puerta13 = new Puerta();
            puerta13.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta13.changePosition(new Vector3(399f, 32f, 1292f));
            puertas.Add(puerta13.getMesh());

            puerta14 = new Puerta();
            puerta14.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta14.changePosition(new Vector3(89f, 32f, 922f));
            puertas.Add(puerta14.getMesh());

            puerta15 = new Puerta();
            puerta15.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta15.changePosition(new Vector3(89f, 142f, 275f));
            puertas.Add(puerta15.getMesh());

            puerta16 = new Puerta();
            puerta16.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta16.changePosition(new Vector3(439f, 142f, 203f));
            puertas.Add(puerta16.getMesh());

            puerta17 = new Puerta();
            puerta17.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta17.getMesh().move(new Vector3(201f-15f, 142f, 1570f-22f));
            puerta17.getMesh().UpdateMeshTransform();
            puertas.Add(puerta17.getMesh());

            puerta18 = new Puerta();
            puerta18.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta18.getMesh().move(new Vector3(452f-15f, 142f, 1221f-22f));
            puerta18.getMesh().UpdateMeshTransform();
            puertas.Add(puerta18.getMesh());

            puerta19 = new Puerta();
            puerta19.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta19.changePosition(new Vector3(459f, 142f, 1675f));
            puertas.Add(puerta19.getMesh());

            puerta20 = new Puerta();
            puerta20.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta20.getMesh().move(new Vector3(734f-15f, 142f, 1570f-22f));
            puerta20.getMesh().UpdateMeshTransform();
            puertas.Add(puerta20.getMesh());

            puerta21 = new Puerta();
            puerta21.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta21.getMesh().move(new Vector3(915f-15f, 142f, 751f-22f));
            puerta21.getMesh().UpdateMeshTransform();
            puertas.Add(puerta21.getMesh());

            puerta22 = new Puerta();
            puerta22.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta22.getMesh().move(new Vector3(695f-15f, 142f, 600f-22f));
            puerta22.getMesh().UpdateMeshTransform();
            puertas.Add(puerta22.getMesh());

            puerta23 = new Puerta();
            puerta23.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta23.changePosition(new Vector3(469f, 142f, 921f));
            puertas.Add(puerta23.getMesh());

            puerta24 = new Puerta();
            puerta24.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta24.getMesh().move(new Vector3(695f-15f, 142f, 912f-22f));
            puerta24.getMesh().UpdateMeshTransform();
            puertas.Add(puerta24.getMesh());

            puerta25 = new Puerta();
            puerta25.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta25.changePosition(new Vector3(399f, 142f, 724f));
            puertas.Add(puerta25.getMesh());

            puerta26 = new Puerta();
            puerta26.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2a90-TgcScene.xml").Meshes[0]);
            puerta26.getMesh().move(new Vector3(454f-15f, 142f, 331f-22f));
            puerta26.getMesh().UpdateMeshTransform();
            puertas.Add(puerta26.getMesh());

            puerta27 = new Puerta();
            puerta27.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta27.changePosition(new Vector3(399f, 142f, 1292f));
            puertas.Add(puerta27.getMesh());

            puerta28 = new Puerta();
            puerta28.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0]);
            puerta28.changePosition(new Vector3(89f, 142f, 922f));
            puertas.Add(puerta28.getMesh());

            #endregion
            /*
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
            */
            #region Recorrido Monstruo
            var recorrido = new List<Vector3> ();
            recorrido.Add(new Vector3(944, 30, 242 ));
            recorrido.Add(new Vector3(944, 30, 954 ));
            recorrido.Add(new Vector3(618, 30, 954 ));
            recorrido.Add(new Vector3(618, 30, 1331));
            recorrido.Add(new Vector3(771, 30, 1331));
            recorrido.Add(new Vector3(771, 30, 1708));
            recorrido.Add(new Vector3(171, 30, 1708));
            recorrido.Add(new Vector3(171, 30, 1330));
            recorrido.Add(new Vector3(261, 30, 1330));
            recorrido.Add(new Vector3(258, 30, 962 ));
            recorrido.Add(new Vector3(41 , 30, 962 ));
            recorrido.Add(new Vector3(41 , 30, 691 ));
            recorrido.Add(new Vector3(424, 30, 691 ));
            recorrido.Add(new Vector3(424, 30, 471 ));
            recorrido.Add(new Vector3(39 , 30, 471 ));
            recorrido.Add(new Vector3(39 , 30, 242 ));
            #endregion
            monstruo = new Monstruo();
            monstruo.Init(MonstruoModelo.createMeshInstance("Monstruo"),/*new Vector3(0, 0, 0), monsterTriggers, monsterSpawnPoints,*/ recorrido);
            
            objetosColisionables.Clear();

            foreach (var mesh in TgcScene.Meshes)
            {
                objetosColisionables.Add(mesh.BoundingBox);
            }
            
            foreach (var mesh in puertas)
            {
                objetosColisionables.Add(mesh.BoundingBox);
            }
            TgcScene.Meshes.Add(monstruo.mesh);

            #region Botiquines Init
            
            //Falta mesh y ubicaciones de cada botiquin
            botiquin1 = new Botiquin();
            botiquin1.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin1.changePosicion(new Vector3(463,0,490));
            botiquines.Add(botiquin1.meshBotiquin);


            botiquin2 = new Botiquin();
            botiquin2 = new Botiquin();
            botiquin2.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin2.changePosicion(new Vector3(463, 110, 490));
            botiquines.Add(botiquin2.meshBotiquin);

            botiquin3 = new Botiquin();
            botiquin3 = new Botiquin();
            botiquin3.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin3.changePosicion(new Vector3(400, 0, 890));
            botiquines.Add(botiquin3.meshBotiquin);

            botiquin4 = new Botiquin();
            botiquin4 = new Botiquin();
            botiquin4.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin4.changePosicion(new Vector3(400, 110, 890));
            botiquines.Add(botiquin4.meshBotiquin);

            botiquin5 = new Botiquin();
            botiquin5 = new Botiquin();
            botiquin5.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin5.changePosicion(new Vector3(225, 0, 1600));
            botiquines.Add(botiquin5.meshBotiquin);

            botiquin6 = new Botiquin();
            botiquin6 = new Botiquin();
            botiquin6.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\botiquin-TgcScene.xml").Meshes[0]);
            botiquin6.changePosicion(new Vector3(225, 0, 1600));
            botiquines.Add(botiquin6.meshBotiquin);

            #endregion

            #region Texto de la Muerte
            textoDeLaMuerte = new TgcText2D();
            textoDeLaMuerte.Text = "YOU DIED";
            textoDeLaMuerte.Color = Color.Red;
            textoDeLaMuerte.Position = new Point(D3DDevice.Instance.Width / 12, D3DDevice.Instance.Height / 2);
            textoDeLaMuerte.changeFont(new System.Drawing.Font("TimesNewRoman", 55));
			#endregion

			#region Texto de Pausa
			textoPausa = new TgcText2D()
			{
				Text = "PAUSA",
				Color = Color.Gray,
				Position = new Point(D3DDevice.Instance.Width / 12, D3DDevice.Instance.Height / 2)
			};
			textoPausa.changeFont(new System.Drawing.Font("TimesNewRoman", 55));
			#endregion
			#region Texto de ComoJugar
			textoHowToPlay = new TgcText2D()
			{
				Text = "Hay un monstruo en el edificio! No sabemos qué es y no importa cómo llegó. La prioridad es salir con vida. Arreglándotelas con escasa iluminación, deberás pulsar los varios botones rojos que se encuentran para poder escapar. Pero ojo, que no te agarre el cuco porque sos boleta!!! Te movés con WASD, con el mouse manejás la cámara.Con E abrís puertas y apretás botones.Con FGH aternás entre tus diferentes recursos de iluminación.Con Shift corrés.",
				Color = Color.White,
				Position = new Point(D3DDevice.Instance.Width / 12, D3DDevice.Instance.Height / 2),
				Size = new Size((int)(D3DDevice.Instance.Width * (5f / 6f)), D3DDevice.Instance.Height),
				Align = TgcText2D.TextAlign.LEFT
			};
			textoHowToPlay.changeFont(new System.Drawing.Font("TimesNewRoman", 25));
			#endregion
			#region BotonesInit
			botonEscapePod1 = new Boton();           
            botonEscapePod1.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonEscapePod1.meshBoton.Position = new Vector3(440, 25, 30);
            botonEscapePod1.changeColor(Color.Red);
            botones.Add(botonEscapePod1);

            botonEscapePod2 = new Boton();
            botonEscapePod2.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonEscapePod2.meshBoton.Position = new Vector3(440, 135, 30);
            botonEscapePod2.changeColor(Color.Red);
            botones.Add(botonEscapePod2);

            botonOxigeno = new Boton();
            botonOxigeno.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonOxigeno.meshBoton.Position = new Vector3(305, 135, 730);
            botonOxigeno.changeColor(Color.Red);
            botones.Add(botonOxigeno);

            botonElectricidad = new Boton();
            botonElectricidad.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonElectricidad.meshBoton.Position = new Vector3(490, 25, 1520);
            botonElectricidad.changeColor(Color.Red);
            botones.Add(botonElectricidad);

            botonElectricidad2 = new Boton();
            botonElectricidad2.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonElectricidad2.meshBoton.Position = new Vector3(490, 135, 1520);
            botonElectricidad2.changeColor(Color.Red);
            botones.Add(botonElectricidad2);

            botonCombustible = new Boton();
            botonCombustible.setMesh(loader.loadSceneFromFile(this.MediaDir + "\\boton-TgcScene.xml").Meshes[0]);
            botonCombustible.meshBoton.Position = new Vector3(550, 25, 280);
            botonCombustible.changeColor(Color.Red);
            botones.Add(botonCombustible);
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

            //Musica ahora reproduce musica pero tira un error de que no se cargo bien
            
            mp3Player.FileName = this.MediaDir + "Music\\hl1_song19.mp3";

            timer = 0;
			UpdateGame();
			
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

			((TGC.Examples.Camara.TgcFpsCamera)this.Camara).LockCam = true;
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
                //lightMesh.Position = chocaLuz(lightMesh, Camara.Position, new Vector3(x, y, z), objetosColisionables);
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
                soundBoton.play(false);
            }

            if (Input.keyPressed(Key.E) && distance(Camara.Position, botonElectricidad2.meshBoton.Position) < 55)
            {
                botonElectricidad2.changeColor(Color.Green);
                botonElectricidad2.isGreen = true;
                soundBoton.play(false);
            }

            if (Input.keyPressed(Key.E) && distance(Camara.Position, botonOxigeno.meshBoton.Position) < 55)
            {
                botonOxigeno.changeColor(Color.Green);
                botonOxigeno.isGreen = true;
                soundBoton.play(false);
            }

            if (Input.keyPressed(Key.E) && distance(Camara.Position, botonCombustible.meshBoton.Position) < 55)
            {
                botonCombustible.changeColor(Color.Green);
                botonCombustible.isGreen = true;
                soundBoton.play(false);
            }

            //Tiene que estar en verde todos los demas botones
            if (Input.keyPressed(Key.E) && (distance(Camara.Position, botonEscapePod1.meshBoton.Position) < 55 || distance(Camara.Position, botonEscapePod2.meshBoton.Position) < 55) && botonCombustible.isGreen && botonElectricidad.isGreen && botonElectricidad2.isGreen && botonOxigeno.isGreen)
            {
                botonEscapePod1.changeColor(Color.Green);
                botonEscapePod2.changeColor(Color.Green);
                soundBoton.play(false);
                //aaaaand We WON!!!
            }

            #endregion

            #region Accion Puertas

            if (Input.keyPressed(Key.E))
            {
                //Abre y cierra las puertas estando cerca del umbral de la puerta
                puerta1.abrirPuerta(Camara.Position);
                puerta2.abrirPuerta(Camara.Position);
                puerta3.abrirPuerta(Camara.Position);
                puerta4.abrirPuerta(Camara.Position);
                puerta5.abrirPuerta(Camara.Position);
                puerta6.abrirPuerta(Camara.Position);
                puerta7.abrirPuerta(Camara.Position);
                puerta8.abrirPuerta(Camara.Position);
                puerta9.abrirPuerta(Camara.Position);
                puerta10.abrirPuerta(Camara.Position);
                puerta11.abrirPuerta(Camara.Position);
                puerta12.abrirPuerta(Camara.Position);
                puerta13.abrirPuerta(Camara.Position);
                puerta14.abrirPuerta(Camara.Position);
                puerta15.abrirPuerta(Camara.Position);
                puerta16.abrirPuerta(Camara.Position);
                puerta17.abrirPuerta(Camara.Position);
                puerta18.abrirPuerta(Camara.Position);
                puerta19.abrirPuerta(Camara.Position);
                puerta20.abrirPuerta(Camara.Position);
                puerta21.abrirPuerta(Camara.Position);
                puerta22.abrirPuerta(Camara.Position);
                puerta23.abrirPuerta(Camara.Position);
                puerta24.abrirPuerta(Camara.Position);
                puerta25.abrirPuerta(Camara.Position);
                puerta26.abrirPuerta(Camara.Position);
                puerta27.abrirPuerta(Camara.Position);
                puerta28.abrirPuerta(Camara.Position);
                soundPuerta.play(false);
            }

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

            var camarita = (TGC.Examples.Camara.TgcFpsCamera)Camara;
            camarita.UpdateCamera(ElapsedTime, objetosColisionables, vidaPorcentaje, staminaPorcentaje);

            #region Logica Personaje

            //Vida
            tiempoGolpe += ElapsedTime;
            if (distance(monstruo.Position, Camara.Position) < 50f && vidaPorcentaje > 0)
            {
                vidaPorcentaje -= 0.2f;
                if(tiempoGolpe >= 0.75f)
                {
                    soundLoseLife.play(false);
                    tiempoGolpe = 0;
                }
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
            if (vidaPorcentaje <= 15)
            {
                soundHeartBeat.play(true);
            }

            tiempoPaso += ElapsedTime;

            if (camarita.seMueve() && tiempoPaso >= 0.5f)
            {
                soundWalk.dispose();
                soundWalk.loadSound(MediaDir + "Sounds\\tile3.wav", DirectSound.DsDevice);
                soundWalk.play(false);
                tiempoPaso = 0;
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
            monstruo.Update(Camara.Position, objetosColisionables, ElapsedTime);
            #endregion

            sound3DMonster.Position = monstruo.Position;
            sound3DMonster.play(true);
            //sound3dMotor.play(true);

        }

        public void UpdateMenu() {

			((TGC.Examples.Camara.TgcFpsCamera)this.Camara).LockCam = false;
			menu.Update(ElapsedTime);
			
		}

        public void UpdatePause()
        {

			((TGC.Examples.Camara.TgcFpsCamera)this.Camara).LockCam = false;
			if (Input.keyPressed(Key.Return)) CurrentState=StateGame;
        }

		public void UpdateHowToPlay() {

			((TGC.Examples.Camara.TgcFpsCamera)this.Camara).LockCam = false;
			menuHowToPlay.Update(ElapsedTime);
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

		public void RenderCamaraMenu()
		{ //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
			timer += ElapsedTime;
            //mp3Player.FileName = this.MediaDir + "Music\\hl1_song19.mp3";
            mp3Player.FileName = this.MediaDir + "Music\\Mastermind.mp3";
            if (mp3Player.getStatus() == TgcMp3Player.States.Open)
            {
                mp3Player.play(true);
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

            List<TgcMesh> candidatos = new List<TgcMesh>();

            foreach(var mesh in TgcScene.Meshes)
            {
                var r = TgcCollisionUtils.classifyFrustumAABB(Frustum, mesh.BoundingBox);
                if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
                {
                    candidatos.Add(mesh);
                }
            }

			//Renderizar meshes
			foreach (var mesh in candidatos)
			{
                aplicaLuces(mesh);
			}

            foreach(var mesh in candidatos)
            {
                mesh.render();
            }
		}

		public void RenderGame() {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            timer += ElapsedTime;

            soundAmbience.play(true);
            
            //Si el estado del reproductor de musica es que el archivo esta abierto entonces lo reproducimos
            //Primero cerramos el archivo de audio del menu y elegimos uno nuevo.
            if (mp3Player.getStatus() == TgcMp3Player.States.Playing)
            {
                mp3Player.stop();
            }

            

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
            if (lighter.getSelect())
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

            //Aplicamos un Shader especifico a cada tipo de luz
            if (this.glowstick.getSelect() || this.lighter.getSelect() && vidaPorcentaje > 0)
            {
                Shader = TgcShaders.Instance.TgcMeshPointLightShader;
            }
            if (this.flashlight.getSelect() && vidaPorcentaje > 0)
            {
                Shader = TgcShaders.Instance.TgcMeshSpotLightShader;
            }

            //Si nos quedamos sin energia en alguno de nuestros elementos de iluminacion se va a aplicar un shader de distorsion 
            if ((glowstick.getSelect() && glowstick.getEnergia() <= 0) || (lighter.getSelect() && lighter.getEnergia() <= 0) || (flashlight.getSelect() && flashlight.getEnergia() <= 0))
            {
                Shader = TgcShaders.loadEffect(ShadersDir + "DistortionShader.fx");
                Shader.SetValue("time", ElapsedTime);
            }
            
            //Si morimos aplicamos un Shader que pone todo en escala de grices
            //Y mostramos un mensaje de muerte
            if(vidaPorcentaje <= 0)
            {
                textoDeLaMuerte.render();
                Shader = TgcShaders.loadEffect(ShadersDir + "GrayShader.fx");
            }
            
            playerPos.Position = Camara.Position;

            //Agrega efecto del Shader a los elementos del escenario
            foreach (var mesh in TgcScene.Meshes)
            {
                mesh.Effect = Shader;
                mesh.Technique = TgcShaders.Instance.getTgcMeshTechnique(mesh.RenderType);
            }

            //Renderizar meshes
            foreach (var mesh in TgcScene.Meshes)
            {
                aplicaLuces(mesh); 
            }

            //Agrega efecto del Shader a las puertas
            foreach (var mesh in puertas)
            {
                mesh.Effect = Shader;
                mesh.Technique = TgcShaders.Instance.getTgcMeshTechnique(mesh.RenderType);
            }

            //Renderizamos las puertas
            foreach (var mesh in puertas)
            {
                aplicaLuces(mesh);
                mesh.render();
            }

            //Aplicamos el efecto del Shader a los botiquines
            foreach(var botiquin in botiquines)
            {
                botiquin.Effect = Shader;
                botiquin.Technique = TgcShaders.Instance.getTgcMeshTechnique(botiquin.RenderType);
            }

            //Renderizamos los botiquines
            foreach (var botiquin in botiquines)
            {
                botiquin.render();
            }

            #region Render y shader de botones            
            //Shader para los botones
            var shaderBoton = TgcShaders.loadEffect(ShadersDir + "RedAndGreenShader.fx"); ;

            if (botonEscapePod2.getColor() == Color.Green)
            {
                shaderBoton.SetValue("isGreen", true);
                botonEscapePod2.applyEffect(shaderBoton);
                botonEscapePod2.meshBoton.render();
            }
            else
            {
                if (botonEscapePod2.getColor() == Color.Red)
                {
                    shaderBoton.SetValue("isGreen", false);
                    botonEscapePod2.applyEffect(shaderBoton);
                    botonEscapePod2.meshBoton.render();
                }
            }

            if (botonEscapePod1.getColor() == Color.Green)
            {
                shaderBoton.SetValue("isGreen", true);
                botonEscapePod1.applyEffect(shaderBoton);
                botonEscapePod1.meshBoton.render();
            }
            else
            {
                if (botonEscapePod1.getColor() == Color.Red)
                {
                    shaderBoton.SetValue("isGreen", false);
                    botonEscapePod1.applyEffect(shaderBoton);
                    botonEscapePod1.meshBoton.render();
                }
            }

            if (botonCombustible.getColor() == Color.Green)
            {
                shaderBoton.SetValue("isGreen", true);
                botonCombustible.applyEffect(shaderBoton);
                botonCombustible.meshBoton.render();
            }
            else
            {
                if (botonCombustible.getColor() == Color.Red)
                {
                    shaderBoton.SetValue("isGreen", false);
                    botonCombustible.applyEffect(shaderBoton);
                    botonCombustible.meshBoton.render();
                }
            }

            if (botonOxigeno.getColor() == Color.Green)
            {
                shaderBoton.SetValue("isGreen", true);
                botonOxigeno.applyEffect(shaderBoton);
                botonOxigeno.meshBoton.render();
            }
            else
            {
                if (botonOxigeno.getColor() == Color.Red)
                {
                    shaderBoton.SetValue("isGreen", false);
                    botonOxigeno.applyEffect(shaderBoton);
                    botonOxigeno.meshBoton.render();
                }
            }

            if (botonElectricidad2.getColor() == Color.Green)
            {
                shaderBoton.SetValue("isGreen", true);
                botonElectricidad2.applyEffect(shaderBoton);
                botonElectricidad2.meshBoton.render();
            }
            else
            {
                if (botonElectricidad2.getColor() == Color.Red)
                {
                    shaderBoton.SetValue("isGreen", false);
                    botonElectricidad2.applyEffect(shaderBoton);
                    botonElectricidad2.meshBoton.render();
                }
            }

            if (botonElectricidad.getColor() == Color.Green)
            {
               shaderBoton.SetValue("isGreen", true);
                botonElectricidad.applyEffect(shaderBoton);
                botonElectricidad.meshBoton.render();
            }
            else
            {
                if (botonElectricidad.getColor() == Color.Red)
                {
                    shaderBoton.SetValue("isGreen", false);
                    botonElectricidad.applyEffect(shaderBoton);
                    botonElectricidad.meshBoton.render();
                }
            } 
          
            #endregion

            //Aplicamos Shader y renderizamos al mostro D:
            monstruo.mesh.Effect = Shader;
            monstruo.mesh.Technique = TgcShaders.Instance.getTgcMeshTechnique(monstruo.mesh.RenderType);

            monstruo.Render();

            //Dibuja un texto por pantalla
            TGC.Examples.Camara.TgcFpsCamera camaraPrint = (TGC.Examples.Camara.TgcFpsCamera)Camara;
            DrawText.drawText("Use W,A,S,D para desplazarte, Espacio para subir, Control para bajar, Shift para ir mas rapido y el mouse para mover la camara: \n "
                + "Position : " + TgcParserUtils.printVector3(Camara.Position) + "\n"
                + " LookAt : " + TgcParserUtils.printVector3(Camara.LookAt) + "\n"
                + " Light Position : " + TgcParserUtils.printVector3(lightMesh.Position) + "\n"
                + " Monster Position : " + TgcParserUtils.printVector3(monstruo.Position) + "\n"
                + " Camera Bounding Sphere : " + TgcParserUtils.printVector3(camaraPrint.sphereCamara.Position) + "\n"
                + " M para Monstruo D:" + "\n"
                + " N para activar/desactivar colisiones del Monstruo \n"
                + " L activa colisiones de la camara"
            , 0, 30, Color.OrangeRed);
            /*
            //render por "Portal" Rendering
            List<TgcMesh> candidatos = new List<TgcMesh>();
            foreach (var celda in celdasEscena)
            {
                candidatos.AddRange(celda.render(Camara.Position, Frustum));
            }

            foreach (var candidato in candidatos)
            {
                var r = TgcCollisionUtils.classifyFrustumAABB(Frustum, candidato.BoundingBox);
                if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
                {
                    candidato.render();
                }
            }*/


            //Render con Frustum Culling

            List<TgcMesh> candidatos = new List<TgcMesh>();

            foreach (var mesh in TgcScene.Meshes)
            {
                //Renderizar modelo con FrustumCulling
                var r = TgcCollisionUtils.classifyFrustumAABB(Frustum, mesh.BoundingBox);
                if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
                {
                    candidatos.Add(mesh);
                }
             }

            foreach(var candidato in candidatos)
            {
                candidato.render();
            }
             
            //lightMesh.render();
           
            TGC.Examples.Camara.TgcFpsCamera camarita = (TGC.Examples.Camara.TgcFpsCamera)Camara;
            camarita.render(ElapsedTime, objetosColisionables);
           
        }
        public void RenderPause() {
            RenderGame();
            textoPausa.render();
        }
		public void RenderHowToPlay() {

			RenderCamaraMenu();
			menuHowToPlay.Render(ElapsedTime, this.drawer2D);
			textoHowToPlay.render();

		}
        public void RenderMenu() {
			RenderCamaraMenu();
            menu.Render(ElapsedTime, this.drawer2D);
		
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            //Dispose de todos los elementos que cargamos en el juego
            soundWalk.dispose();
            soundHeartBeat.dispose();
            soundAmbience.dispose();
            soundBoton.dispose();
            soundPuerta.dispose();
            soundLoseLife.dispose();
            mp3Player.stop();         
            glowstickHUD1.Dispose();
            glowstickHUD2.Dispose();
            glowstickHUD3.Dispose();
            flashlightHUD.Dispose();
            monstruo.mesh.dispose();
            TgcScene.disposeAll();
            foreach (var puerta in puertas)
            {
                puerta.dispose();
            }
            foreach(var boton in botones)
            {
                boton.meshBoton.dispose();
            }
            foreach(var botiquin in botiquines)
            {
                botiquin.dispose();
            }
            if (Shader!=null) Shader.Dispose();
            textoDeLaMuerte.Dispose();
            vida.Dispose();
            stamina.Dispose();
            centerPoint.Dispose();
        }

        public float distance(Vector3 a, Vector3 b)
        {
            return (FastMath.Sqrt(FastMath.Pow2(a.X - b.X) + FastMath.Pow2(a.Y - b.Y) + FastMath.Pow2(a.Z - b.Z)));
        }

        public Vector3 chocaLuz(TgcBox cajaDeLuz, Vector3 centroCamara, Vector3 targetPosition , List<TgcBoundingAxisAlignBox> colisionables)
        {
            Vector3 retorno = cajaDeLuz.Position;
            TgcBox clon = cajaDeLuz;
            clon.Position = targetPosition;
            retorno = clon.Position;

            List<TgcBoundingAxisAlignBox> candidates = new List<TgcBoundingAxisAlignBox>();

            foreach (var colisionable in colisionables)
            {
                if (Core.Collision.TgcCollisionUtils.testAABBAABB(clon.BoundingBox, colisionable))
                {
                    candidates.Add(colisionable);
                }
            }

            foreach (var colisionable in candidates)
            {
                var caras = colisionable.computeFaces();

                foreach(var cara in caras)
                {
                    var pNormal = TgcCollisionUtils.getPlaneNormal(cara.Plane);

                    var movementRay = new TgcRay(retorno, targetPosition);
                    float brutePlaneDist;
                    Vector3 brutePlaneIntersectionPoint;

                    if (!TgcCollisionUtils.intersectRayPlane(movementRay, cara.Plane, out brutePlaneDist,out brutePlaneIntersectionPoint))
                    {
                        continue;
                    }

                    var radioCaja = cajaDeLuz.Size.X / 2; //"radio" de una caja =P
                    var movementRadiusLengthSq = Vector3.Multiply(targetPosition, radioCaja).LengthSq();

                    if (brutePlaneDist * brutePlaneDist > movementRadiusLengthSq)
                    {
                        continue;
                    }
                    
                    
                }
              
                break;
            }

            return retorno;
        }

        public void aplicaLuces(TgcMesh mesh)
        {
            //se actualiza el transform del mesh
            mesh.UpdateMeshTransform();
            if (vidaPorcentaje > 0)
            {
                //Logica de luces dependiendo de la seleccion, la energia de las mismas y que el player este vivo =P(Manco como te puede matar ese bicho)
                if (glowstick.getSelect() && glowstick.getEnergia() > 0)
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
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("materialSpecularExp", 2000f);
                }
                if (glowstick.getSelect() && glowstick.getEnergia() <= 0)
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
                    mesh.Effect.SetValue("materialSpecularExp", 2000f);
                }
                if (lighter.getSelect() && lighter.getEnergia() > 20)
                {
                    lightMesh.Position = Camara.Position;
                    //Cargar variables shader de la luz
                    mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("lightIntensity", 3500f *FastMath.Sqrt( ElapsedTime));
                    mesh.Effect.SetValue("lightAttenuation", 20f);

                    //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                    mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("materialSpecularExp", 200f);
                }
                if (lighter.getSelect() && lighter.getEnergia() <= 20 && lighter.getEnergia() > 0)
                {
                    lightMesh.Position = Camara.Position;
                    //Cargar variables shader de la luz
                    mesh.Effect.SetValue("lightColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("lightPosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("eyePosition", TgcParserUtils.vector3ToFloat4Array(Camara.Position));
                    mesh.Effect.SetValue("lightIntensity", 250f * FastMath.Sqrt(ElapsedTime));
                    mesh.Effect.SetValue("lightAttenuation", 1f);

                    //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                    mesh.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialAmbientColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor(Color.Black));
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(lightMesh.Color));
                    mesh.Effect.SetValue("materialSpecularExp", 200f);
                }
                if (flashlight.getSelect() && flashlight.getEnergia() > 10)
                {
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
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.White));
                    mesh.Effect.SetValue("materialSpecularExp", 290f);
                }
                if (flashlight.getSelect() && flashlight.getEnergia() <= 10 && flashlight.getEnergia() > 0)
                {
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
                    mesh.Effect.SetValue("materialSpecularColor", ColorValue.FromColor(Color.White));
                    mesh.Effect.SetValue("materialSpecularExp", 290f);
                }
                if (glowstick.getEnergia() == 0 && System.Math.Truncate(lighter.getEnergia()) == 0 && System.Math.Truncate(flashlight.getEnergia()) == 0)
                {
                    //TODO: poner las propiedades del shader que genera distorsiones D:
                }
            }
            
        }
    }
}