using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CGN4
{
    public class GUI
    {
        private readonly Font TextFont = new Font(FontFamily.GenericSansSerif, 20);
        private readonly Dictionary<string, GUIText> texts;
        private readonly List<GUIShape> shapes;
        private Bitmap TextBitmap;
        private int textureId;
        private Size clientSize;
        private Rectangle clientRectangle;

        public void Resize(Rectangle clientRectangle, Size clientSize)
        {
            foreach (var text in texts.Values.Where(p => p.MustResize))
            {
                var newX = text.Position.X + ((clientSize.Width - this.clientSize.Width) / 2);
                var newY = text.Position.Y + ((clientSize.Height - this.clientSize.Height) / 2);
                text.Position = new PointF(newX, newY);
            }

            this.clientRectangle = clientRectangle;
            this.clientSize = clientSize;
            TextBitmap = new Bitmap(clientRectangle.Width, clientRectangle.Height);
            CreateTexture();
        }

        private void CreateTexture()
        {
            int newTextureId;
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Replace);
            Bitmap bitmap = TextBitmap;
            GL.GenTextures(1, out newTextureId);
            GL.BindTexture(TextureTarget.Texture2D, newTextureId);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.Finish();
            bitmap.UnlockBits(data);
            textureId = newTextureId;
        }

        public GUI(Rectangle clientRectangle, Size clientSize)
        {
            texts = new Dictionary<string, GUIText>();
            shapes = new List<GUIShape>();
            Resize(clientRectangle, clientSize);
        }

        public void ClearGUI()
        {
            texts.Clear();
            shapes.Clear();
        }

        public void AddText(GameState gameState, string key, string s, PointF pos, Brush col, bool mustResize = false)
        {
            texts.Add(key, new GUIText
            {
                GameState = gameState,
                Color = col,
                Line = s,
                Position = pos,
                MustResize = mustResize
            });
        }

        public void UpdateText(string key, string s)
        {
            if (texts.ContainsKey(key))
            {
                texts[key].Line = s;
            }
        }

        public void AddShape(GUIShape shape)
        {
            shapes.Add(shape);
        }

        public void Update(GameState gameState)
        {
            //Faz update nas texturas de texto
            if (texts.Count > 0)
            {
                using (Graphics gfx = Graphics.FromImage(TextBitmap))
                {
                    gfx.Clear(Color.Black);
                    gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    for (int i = 0; i < texts.Count; i++)
                    {
                        var text = texts.Values.ToArray()[i];
                        if (text.GameState == gameState)
                        {
                            gfx.DrawString(text.Line, TextFont, text.Color, text.Position);
                        }
                    }
                }

                BitmapData data = TextBitmap.LockBits(new Rectangle(0, 0, TextBitmap.Width, TextBitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, TextBitmap.Width, TextBitmap.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                TextBitmap.UnlockBits(data);
            }
        }

        public void Draw(GameState gameState)
        {
            DrawShapes(gameState);
            DrawText();
        }

        private void DrawText()
        {
            GL.PushMatrix();
            GL.Disable(EnableCap.DepthTest);
            GL.LoadIdentity();
            GL.Viewport(new Rectangle(clientRectangle.Left, clientRectangle.Top, clientRectangle.Width, clientRectangle.Height));
            Matrix4 ortho_projection = Matrix4.CreateOrthographicOffCenter(0, clientSize.Width, clientSize.Height, 0, -35, 1);
            GL.MatrixMode(MatrixMode.Projection);

            GL.PushMatrix();
            GL.LoadMatrix(ref ortho_projection);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.DstColor);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0); GL.Vertex2(TextBitmap.Width, 0);
            GL.TexCoord2(1, 1); GL.Vertex2(TextBitmap.Width, TextBitmap.Height);
            GL.TexCoord2(0, 1); GL.Vertex2(0, TextBitmap.Height);
            GL.End();
            GL.PopMatrix();

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            GL.PopMatrix();
        }

        private void DrawShapes(GameState gameState)
        {
            GL.PushMatrix();
            GL.LoadIdentity();
            foreach (var shape in shapes.Where(p => p.GameState == gameState))
            {
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(shape.Color);
                GL.LineWidth(4);
                GL.Vertex2(shape.X, shape.Y);
                GL.Vertex2(shape.X + 2f, shape.Y);
                GL.Vertex2(shape.X + 2f, shape.Y - 2f);
                GL.Vertex2(shape.X, shape.Y - 2f);
                GL.End();
            }

            GL.PopMatrix();
        }

        public void Dispose()
        {
            if (textureId > 0)
            {
                GL.DeleteTexture(textureId);
            }
        }
    }

    public class GUIShape
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public GameState GameState { get; set; }
        public PrimitiveType Primitive { get; set; }
        public Color Color { get; set; }
    }

    public class GUIText
    {
        public bool MustResize { get; set; }
        public GameState GameState { get; set; }
        public PointF Position { get; set; }
        public string Line { get; set; }
        public Brush Color { get; set; }
    }
}