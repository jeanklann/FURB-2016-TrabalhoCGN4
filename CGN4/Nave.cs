using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Drawing;
using OpenTK.Input;

namespace CGN4 {
    public class Nave : GameObject {

        public static void Load() {
            Lists.Nave = GL.GenLists(1);
            GL.NewList(Lists.Nave, ListMode.Compile);
            GL.Begin(PrimitiveType.Triangles);

            //Draw
            GL.Color3(Color.MidnightBlue);
            GL.Vertex3(-0.2f, 0.2f, 0);
            GL.Color3(Color.SpringGreen);
            GL.Vertex3(0.0f, -0.2f, 0);
            GL.Color3(Color.Ivory);
            GL.Vertex3(0.2f, 0.2f, 0);
            //EndDraw

            GL.End();
            GL.EndList();
        }
        public override void Draw() {
            GL.PushMatrix();
            GL.Translate(Position);
            GL.Rotate(Rotation, RotationAxis);
            GL.Scale(Scale);
            GL.CallList(Lists.Nave);
            GL.PopMatrix();
        }

        public override void Update() {
            var state = OpenTK.Input.Keyboard.GetState();
            if(state[Key.Escape]) {
                Console.WriteLine("Saindo do jogo...");
                Environment.Exit(0);
            }
            if(state[Key.Up]) {
                Position.Y -= Game.MainGame.RenderTime * 2f;
            }
            if(state[Key.Down]) {
                Position.Y += Game.MainGame.RenderTime * 2f;
            }
            if(state[Key.Left]) {
                Position.X += Game.MainGame.RenderTime * 2f;
            }
            if(state[Key.Right]) {
                Position.X -= Game.MainGame.RenderTime * 2f;
            }
            if(Position.X > 1f) Position.X = 1f;
            if(Position.X < -1f) Position.X = -1f;
            if(Position.Y > 1f) Position.Y = 1f;
            if(Position.Y < -1f) Position.Y = -1f;
        }
        public bool DetectedCollision() {
            return false;
        }
    }
}
