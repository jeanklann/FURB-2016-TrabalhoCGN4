using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjLoader.Loader.Loaders;
using System.IO;

namespace CGN4 {
    class Game:GameWindow {
        public Nave nave;
        public CriadorDePredios predios;
        public Matrix4d ProjectionMatrix;
        public static Game MainGame;
        public int score = 0;

        float[] mat_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] mat_shininess = { 50.0f };
        float[] light_position = { 1.0f, 1.0f, 1.0f, 0.0f };
        float[] light_ambient = { 0.5f, 0.5f, 0.5f, 1.0f };

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            GL.ClearColor(Color.Black);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, mat_specular);
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, mat_shininess);
            GL.Light(LightName.Light0, LightParameter.Position, light_position);
            GL.Light(LightName.Light0, LightParameter.Ambient, light_ambient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, mat_specular);
            //GL.Enable(EnableCap.CullFace);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);

            var objLoaderFactory = new ObjLoaderFactory();
            var objLoader = objLoaderFactory.Create();
            var fileStream = new FileStream("predio.obj", FileMode.Open);
            var result = objLoader.Load(fileStream);
            Predio.vbo = new VBO(result);
            fileStream.Close();

            objLoaderFactory = new ObjLoaderFactory();
            objLoader = objLoaderFactory.Create();
            fileStream = new FileStream("nave.obj", FileMode.Open);
            result = objLoader.Load(fileStream);
            Nave.vbo = new VBO(result);


            nave = new Nave();
            nave.Position = new Vector3d(0, 0, -2);

            
            //Predio.Load();
            predios = new CriadorDePredios(nave);
            MainGame = this;
        }
        protected override void OnResize(EventArgs e) {
            GL.Viewport(new Rectangle(ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width, ClientRectangle.Height));
            ProjectionMatrix = Matrix4d.Perspective(30, ClientSize.Width / ClientSize.Height, 1, 100);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref ProjectionMatrix);
            Console.WriteLine("Janela redimensionada");
        }
        protected override void OnUpdateFrame(FrameEventArgs e) {
            predios.Update();
            nave.Update();
        }
        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.PushMatrix();
            #region Camera
            GL.Scale(new Vector3d(1,-1,1));
            #endregion

            #region Draw
            
            nave.Draw();
            predios.Draw();
            #endregion

            GL.PopMatrix();
            GL.Finish();
            SwapBuffers();
        }
        public Game() {
            KeyDown += (object sender, KeyboardKeyEventArgs e) => {
                switch(e.Key) {
                    case Key.R:
                        
                        break;
                    

                }
            };
        }

    }
}
