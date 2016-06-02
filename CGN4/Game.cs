using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGN4 {
    class Game:GameWindow {
        public Nave nave;
        public CriadorDePredios predios;
        public Matrix4d ProjectionMatrix;
        public static Game MainGame;
        public int score = 0;
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);

            Nave.Load();
            nave = new Nave();
            nave.Position = new Vector3d(0, 0, -2);

            
            Predio.Load();
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
