using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System;
using ObjLoader.Loader.Loaders;
using System.IO;
using System.Threading;

namespace CGN4
{
    class Game : GameWindow
    {
        public Nave nave;
        public CriadorDePredios predios;
        public GUI GUI;
        public Matrix4d ProjectionMatrix;
        public static Game MainGame;
        public int score = 0;

        public GameState gameState = GameState.Stopped;

        float[] MaterialSpecular = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] MaterialShiness = { 50.0f };
        float[] LightPosition = { 1.0f, 1.0f, 1.0f, 0.0f };
        float[] LightAmbient = { 0.5f, 0.5f, 0.5f, 1.0f };

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.Black);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, MaterialSpecular);
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, MaterialShiness);
            GL.Light(LightName.Light0, LightParameter.Position, LightPosition);
            GL.Light(LightName.Light0, LightParameter.Ambient, LightAmbient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, MaterialSpecular);
            //GL.Enable(EnableCap.CullFace);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);

            InitGame();
        }

        private void InitGame()
        {
            GUI = new GUI(ClientRectangle, ClientSize);

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
            fileStream.Close();
            nave = new Nave();
            nave.Position = new Vector3d(0, 0, -2);

            predios = new CriadorDePredios(nave);
            MainGame = this;

            #region Define GUI

            GUI.AddText(GameState.Running, "ScoreR", "Score: 0", new PointF(0, 0), new SolidBrush(Color.White));

            GUI.AddText(GameState.Paused, "Texto jogo pausado", "Jogo pausado", new PointF(110, 100), new SolidBrush(Color.White), true);
            GUI.AddText(GameState.Paused, "ScoreP", "Score: 0", new PointF(140, 150), new SolidBrush(Color.White), true);
            GUI.AddText(GameState.Paused, "Texto continuar", "Pressione P para continuar.", new PointF(30, 200), new SolidBrush(Color.White), true);
            GUI.AddShape(new GUIShape
            {
                GameState = GameState.Paused,
                X = -1f,
                Y = 1,
                Color = Color.CadetBlue,
                Primitive = PrimitiveType.QuadStrip
            });

            GUI.AddText(GameState.Defeat, "Texto derrota", "Você morreu", new PointF(110, 100), new SolidBrush(Color.White), true);
            GUI.AddText(GameState.Defeat, "ScoreD", "Score: " + score, new PointF(140, 150), new SolidBrush(Color.White), true);
            GUI.AddText(GameState.Defeat, "Texto recomeçar", "Pressione R para recomeçar.", new PointF(30, 200), new SolidBrush(Color.White), true);
            GUI.AddShape(new GUIShape
            {
                GameState = GameState.Defeat,
                X = -1f,
                Y = 1,
                Color = Color.Salmon,
                Primitive = PrimitiveType.QuadStrip
            });

            #endregion Define GUI

            gameState = GameState.Running;
        }

        private void RestartGame()
        {
            score = 0;
            nave = new Nave();
            nave.Position = new Vector3d(0, 0, -2);
            predios = new CriadorDePredios(nave);
            gameState = GameState.Running;
        }

        private void PauseGame()
        {
            if (gameState == GameState.Running)
            {
                gameState = GameState.Paused;
            }
            else
            {
                if (gameState == GameState.Paused)
                {
                    gameState = GameState.Running;
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(new Rectangle(ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width, ClientRectangle.Height));
            ProjectionMatrix = Matrix4d.Perspective(30, ClientSize.Width / ClientSize.Height, 1, 100);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref ProjectionMatrix);
            GUI.Resize(ClientRectangle, ClientSize);
            Console.WriteLine("Janela e GUI redimensionada");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            
            GUI.Update(gameState);
            GUI.UpdateText("ScoreR", "Score: " + score);
            GUI.UpdateText("ScoreP", "Score: " + score);
            GUI.UpdateText("ScoreD", "Score: " + score);
            if (gameState == GameState.Running)
            {
                predios.Update();
                nave.Update();
            }
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GUI.Draw(gameState);
            GL.PushMatrix();

            if (gameState == GameState.Running)
            {
                #region Camera

                GL.Scale(new Vector3d(1, -1, 1));

                #endregion

                #region Draw

                nave.Draw();
                predios.Draw();

                #endregion
            }

            GL.PopMatrix();
            GL.Finish();
            SwapBuffers();
        }
        public Game()
        {
            KeyDown += (object sender, KeyboardKeyEventArgs e) =>
            {
                switch (e.Key)
                {
                    case Key.D:
                        gameState = GameState.Defeat;
                        break;
                    case Key.R:
                        RestartGame();
                        break;
                    case Key.P:
                        PauseGame();
                        break;
					case Key.S:
						score += 100;
						break;
                }
            };
        }

    }
}
