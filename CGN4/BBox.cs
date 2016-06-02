using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace CGN4 {
    public class BBox {
        public Vector3d Max = new Vector3d();
        public Vector3d Min = new Vector3d();
        public bool DetectCollision(BBox other) {
            if(
                (Min.X > other.Min.X && Min.X < other.Max.X) ||
                (Min.Y > other.Min.Y && Min.Y < other.Max.Y) ||
                (Min.Z > other.Min.Z && Min.Z < other.Max.Z) ||
                (Max.X > other.Min.X && Min.X < other.Max.X) ||
                (Max.Y > other.Min.Y && Min.Y < other.Max.Y) ||
                (Max.Z > other.Min.Z && Min.Z < other.Max.Z)
                ) {
                return true;
            }
            return false;
        }
    }
}
