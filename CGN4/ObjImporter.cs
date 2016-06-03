using System;
using OpenTK;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGN4 {
    public static class ObjImporter {
        public static Mesh Import(string path) {
            if(!File.Exists(path))
                return null;

            if(new FileInfo(path).Extension != ".obj")
                return null;

            string name;

            List<Vector3d> positions = new List<Vector3d>();
            List<Vector3d> normals = new List<Vector3d>();
            List<Vector2d> uvs = new List<Vector2d>();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();

            string line;

            // open the filr
            using(StreamReader stream = File.OpenText(path)) {
                while(!stream.EndOfStream) {
                    // trim white space and scip empty lines
                    line = stream.ReadLine().Trim();
                    if(string.IsNullOrWhiteSpace(line))
                        continue;

                    // replace . with , necessary for float.Parse()
                    line = line.Replace('.', ',');

                    // split line into segments
                    string[] segments = line.Split(' ');

                    switch(segments[0]) {
                        case "o": // Name
                            name = segments[1];
                            break;

                        case "v": // Vertex Position
                            float x = float.Parse(segments[1]);
                            float y = float.Parse(segments[2]);
                            float z = float.Parse(segments[3]);
                            positions.Add(new Vector3d(x, y, z));
                            break;

                        case "vt": // Vertex TexCoord
                            float u = float.Parse(segments[1]);
                            float v = float.Parse(segments[2]);
                            uvs.Add(new Vector2d(u, v));
                            break;

                        case "vn": // Vertex Normal
                            float nx = float.Parse(segments[1]);
                            float ny = float.Parse(segments[2]);
                            float nz = float.Parse(segments[3]);
                            normals.Add(new Vector3d(nx, ny, nz));
                            break;

                        case "f": // faces only supports triangles!
                            if(segments.Length != 4)
                                break;

                            Vertex vert = new Vertex();
                            for(int i = 1; i < segments.Length; i++) {
                                string[] split = segments[i].Split('/');

                                for(int s = 0; s < split.Length; s++) {
                                    if(string.IsNullOrWhiteSpace(split[s]))
                                        continue;

                                    int index = int.Parse(split[s]) - 1;
                                    if(s == 0)
                                        vert.Position = positions[index];
                                    else if(s == 1)
                                        vert.Uv = uvs[index];
                                    else if(s == 2)
                                        vert.Normal = normals[index];
                                }
                                indices.Add((uint)indices.Count);
                                vertices.Add(vert);

                            }
                            break;
                    }
                }
            }

            return new Mesh(vertices.ToArray(), indices.ToArray());
        }
    }
    public class Mesh {
        public Vertex[] vertices;
        public uint[] indices;
        public Mesh(Vertex[] vertices, uint[] indices) {
            this.vertices = vertices;
            this.indices = indices;
        }
    }
    public struct Vertex {
        public Vector2d Uv;
        public Vector3d Normal;
        public Vector3d Position;
    }
}
