using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Drawing;

namespace CGN4 {
    public class Predio : GameObject {
        public Predio() {
            Scale = new Vector3d(0.25, 1, 0.25);
        }
        public static void Load() {
            Lists.Predio = GL.GenLists(1);
            GL.NewList(Lists.Predio, ListMode.Compile);
            GL.Begin(PrimitiveType.Quads);


            //Draw
            GL.Color3(Color.White);
            GL.Vertex3(1, 1, -1);
            GL.Vertex3(-1, 1, -1);
            GL.Vertex3(-1, 1, 1);
            GL.Vertex3(1, 1, 1);

            GL.Color3(Color.Bisque);
            GL.Vertex3(1, -1, 1);
            GL.Vertex3(-1, -1, 1);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(1, -1, -1);

            GL.Color3(Color.Blue);
            GL.Vertex3(1, 1, 1);
            GL.Vertex3(-1, 1, 1);
            GL.Vertex3(-1, -1, 1);
            GL.Vertex3(1, -1, 1);

            GL.Color3(Color.DarkOliveGreen);
            GL.Vertex3(1, -1, -1);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(-1, 1, -1);
            GL.Vertex3(1, 1, -1);

            GL.Color3(Color.Fuchsia);
            GL.Vertex3(-1, 1, 1);
            GL.Vertex3(-1, 1, -1);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(-1, -1, 1);

            GL.Color3(Color.ForestGreen);
            GL.Vertex3(1, 1, -1);
            GL.Vertex3(1, 1, 1);
            GL.Vertex3(1, -1, 1);
            GL.Vertex3(1, -1, -1);
            //EndDraw


            GL.End();
            GL.EndList();
        }
        public override void Draw() {
            GL.PushMatrix();
            GL.Translate(Position);
            GL.Rotate(Rotation, RotationAxis);
            GL.Scale(Scale);
            GL.CallList(Lists.Predio);
            GL.PopMatrix();
        }
            

        public override void Update() {
            throw new NotImplementedException();
        }
    }
}
