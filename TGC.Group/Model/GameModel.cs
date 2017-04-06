using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
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

        //Caja que se muestra en el ejemplo.
        //usar TgcBox como ejemplo para cargar cualquier caja que queramos.
        private TgcBox Box { get; set; }

        //Escena
        private TgcScene TgcScene { get; set; }

        private TgcMesh PuertaModelo { get; set; }

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
        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }

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
            Camara = new Examples.Camara.TgcFpsCamera(new Vector3(0f, 0f, 0f), 100f, 100f, Input);
            var d3dDevice = D3DDevice.Instance.Device;

            //Version para cargar escena desde carpeta descomprimida
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene = loader.loadSceneFromFile(this.MediaDir + "FullLevel-TgcScene.xml", this.MediaDir + "\\");
            
            //Device de DirectX para crear primitivas.
            //
            PuertaModelo = loader.loadSceneFromFile(this.MediaDir + "\\PUERTA2-TgcScene.xml").Meshes[0];

            
            Puerta1 = PuertaModelo.createMeshInstance("Puerta1");
            Puerta1.AutoTransformEnable = true;
            Puerta1.move(89f, 31.5f, 275f);

            Puerta2 = PuertaModelo.createMeshInstance("Puerta2");
            Puerta2.AutoTransformEnable = true;
            Puerta2.move(439f, 32f, 203f);

            Puerta3 = PuertaModelo.createMeshInstance("Puerta3");
            Puerta3.AutoTransformEnable = true;
            Puerta3.move(201f, 32f, 1570f);
            Puerta3.rotateY(FastMath.PI_HALF);

            Puerta4 = PuertaModelo.createMeshInstance("Puerta4");
            Puerta4.AutoTransformEnable = true;
            Puerta4.move(452f, 32f, 1221f);
            Puerta4.rotateY(FastMath.PI_HALF);

            Puerta5 = PuertaModelo.createMeshInstance("Puerta5");
            Puerta5.AutoTransformEnable = true;
            Puerta5.move(459f, 32f, 1675f);

            Puerta6 = PuertaModelo.createMeshInstance("Puerta6");
            Puerta6.AutoTransformEnable = true;
            Puerta6.move(734f, 32f, 1570f);
            Puerta6.rotateY(FastMath.PI_HALF);

            Puerta7 = PuertaModelo.createMeshInstance("Puerta7");
            Puerta7.AutoTransformEnable = true;
            Puerta7.move(915f, 32f, 751f);
            Puerta7.rotateY(FastMath.PI_HALF);

            Puerta8 = PuertaModelo.createMeshInstance("Puerta8");
            Puerta8.AutoTransformEnable = true;
            Puerta8.move(695f, 32f, 600f);
            Puerta8.rotateY(FastMath.PI_HALF);

            Puerta9 = PuertaModelo.createMeshInstance("Puerta9");
            Puerta9.AutoTransformEnable = true;
            Puerta9.move(469f, 32f, 921f);

            Puerta10 = PuertaModelo.createMeshInstance("Puerta10");
            Puerta10.AutoTransformEnable = true;
            Puerta10.move(695f, 32f, 912f);
            Puerta10.rotateY(FastMath.PI_HALF);

            Puerta11 = PuertaModelo.createMeshInstance("Puerta11");
            Puerta11.AutoTransformEnable = true;
            Puerta11.move(399f, 32f, 724f);

            Puerta12 = PuertaModelo.createMeshInstance("Puerta12");
            Puerta12.AutoTransformEnable = true;
            Puerta12.move(454f, 32f, 331f);
            Puerta12.rotateY(FastMath.PI_HALF);

            Puerta13 = PuertaModelo.createMeshInstance("Puerta13");
            Puerta13.AutoTransformEnable = true;
            Puerta13.move(399f, 32f, 1292f);

            Puerta14 = PuertaModelo.createMeshInstance("Puerta14");
            Puerta14.AutoTransformEnable = true;
            Puerta14.move(89f, 32f, 922f);

            Puerta15 = PuertaModelo.createMeshInstance("Puerta1");
            Puerta15.AutoTransformEnable = true;
            Puerta15.move(89f, 142f, 275f);

            Puerta16 = PuertaModelo.createMeshInstance("Puerta2");
            Puerta16.AutoTransformEnable = true;
            Puerta16.move(439f, 142f, 203f);

            Puerta17 = PuertaModelo.createMeshInstance("Puerta3");
            Puerta17.AutoTransformEnable = true;
            Puerta17.move(201f, 142f, 1570f);
            Puerta17.rotateY(FastMath.PI_HALF);

            Puerta18 = PuertaModelo.createMeshInstance("Puerta4");
            Puerta18.AutoTransformEnable = true;
            Puerta18.move(452f, 142f, 1221f);
            Puerta18.rotateY(FastMath.PI_HALF);

            Puerta19 = PuertaModelo.createMeshInstance("Puerta5");
            Puerta19.AutoTransformEnable = true;
            Puerta19.move(459f, 142f, 1675f);

            Puerta20 = PuertaModelo.createMeshInstance("Puerta6");
            Puerta20.AutoTransformEnable = true;
            Puerta20.move(734f, 142f, 1570f);
            Puerta20.rotateY(FastMath.PI_HALF);

            Puerta21 = PuertaModelo.createMeshInstance("Puerta7");
            Puerta21.AutoTransformEnable = true;
            Puerta21.move(915f, 142f, 751f);
            Puerta21.rotateY(FastMath.PI_HALF);

            Puerta22 = PuertaModelo.createMeshInstance("Puerta8");
            Puerta22.AutoTransformEnable = true;
            Puerta22.move(695f, 142f, 600f);
            Puerta22.rotateY(FastMath.PI_HALF);

            Puerta23 = PuertaModelo.createMeshInstance("Puerta9");
            Puerta23.AutoTransformEnable = true;
            Puerta23.move(469f, 142f, 921f);

            Puerta24 = PuertaModelo.createMeshInstance("Puerta10");
            Puerta24.AutoTransformEnable = true;
            Puerta24.move(695f, 142f, 912f);
            Puerta24.rotateY(FastMath.PI_HALF);

            Puerta25 = PuertaModelo.createMeshInstance("Puerta11");
            Puerta25.AutoTransformEnable = true;
            Puerta25.move(399f, 142f, 724f);

            Puerta26 = PuertaModelo.createMeshInstance("Puerta12");
            Puerta26.AutoTransformEnable = true;
            Puerta26.move(454f, 142f, 331f);
            Puerta26.rotateY(FastMath.PI_HALF);

            Puerta27 = PuertaModelo.createMeshInstance("Puerta13");
            Puerta27.AutoTransformEnable = true;
            Puerta27.move(399f, 142f, 1292f);

            Puerta28 = PuertaModelo.createMeshInstance("Puerta14");
            Puerta28.AutoTransformEnable = true;
            Puerta28.move(89f, 142f, 922f);
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

        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();

            //Capturar Input teclado
            if (Input.keyPressed(Key.F))
            {
                BoundingBox = !BoundingBox;
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

            //Dibuja un texto por pantalla
            DrawText.drawText("Use W,A,S,D para desplazarte, Espacio para subir verticalmente, Control para bajar verticalmente y el mouse para mover la camara: " + TgcParserUtils.printVector3(Camara.Position) + TgcParserUtils.printVector3(Camara.LookAt) + "\n", 0, 30, Color.OrangeRed);

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
            TgcScene.renderAll();

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
            TgcScene.disposeAll();
        }
    }
}