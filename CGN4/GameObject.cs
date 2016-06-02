using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Drawing;

namespace CGN4 {
    public abstract class GameObject {
        
        public Vector3d Position = new Vector3d();
        public double Rotation = 0;
        public Vector3d RotationAxis = new Vector3d();
        public Vector3d Scale = new Vector3d(1,1,1);
        public abstract void Update();
        public abstract void Draw();


    }
}
