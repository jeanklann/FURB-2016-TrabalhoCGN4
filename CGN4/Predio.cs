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
        //public static VBO vbo = new VBO(ObjImporter.Import("C:\\Users\\jean.klann.SISTEMAFIESC\\Desktop\\predio.obj"));
        public static VBO vbo;
        public BBox BBox;
        public Predio() {
            Scale = new Vector3d(0.5, 0.5, 0.5);
            RotationAxis = new Vector3d(0, 1, 0);
            Rotation = -90;
            //vbo.NewBuilding();
            vbo.Load();
            BBox = new BBox();
            BBox.GameObject = this;
            BBox.Max = new Vector3d(0.8, 50, 0.5);
            BBox.Min = new Vector3d(-0.8, -50, -0.5);
        }
        public override void Draw() {
            GL.PushMatrix();
            GL.Translate(Position);
            GL.Rotate(Rotation, RotationAxis);
            GL.Scale(Scale);
            //GL.CallList(Lists.Predio);
            vbo.Draw();
            GL.PopMatrix();
        }
            

        public override void Update() {
            throw new NotImplementedException();
        }
    }
}
