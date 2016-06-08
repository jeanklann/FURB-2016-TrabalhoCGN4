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
        public GameObject GameObject;
        public static bool DetectCollision(Vector3d Position, BBox other) {
            if(
                (Position.X > other.Min.X+other.GameObject.Position.X && Position.X < other.Max.X+other.GameObject.Position.X) &&
                (Position.Y > other.Min.Y+other.GameObject.Position.Y && Position.Y < other.Max.Y+other.GameObject.Position.Y) &&
                (Position.Z > other.Min.Z+other.GameObject.Position.Z && Position.Z < other.Max.Z+other.GameObject.Position.Z)
                ) {
                return true;
            }
            return false;
        }
    }
}
