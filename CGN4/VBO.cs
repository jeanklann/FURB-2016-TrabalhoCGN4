using ObjLoader.Loader.Loaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGN4 {
    public struct ColorStruct {
        byte R, G, B, A;
        public ColorStruct(Color color) {
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
        }
    }
    public class VBO {
        private static Random Random = new Random();
        public Vector3d[] VertexArray;
        public Vector3d[] NormalsArray;
        public Vector2d[] TextureArray;
        public uint[] IndexArray;
        public ColorStruct[] ColorArray;
        public uint VertexArrayID;
        public uint NormalsArrayID;
        public uint TextureArrayID;
        public uint ColorArrayID;
        public uint IndexArrayID;
        public uint TextureID;
        public PrimitiveType PrimitiveType = PrimitiveType.Quads;

        public readonly int VertexGap = Vector3d.SizeInBytes;
        public readonly int NormalsGap = Vector3d.SizeInBytes;
        public readonly int TextureGap = Vector2d.SizeInBytes;
        public readonly int ColorGap = 4;
        public readonly int IndexGap = sizeof(uint);

        private bool useNormals = false;
        private bool useTexture = false;
        private bool useIndex = false;
        private bool useColor = false;

        private uint size = 0;



        public VBO(uint Size, bool UseNormals, bool UseTexture, bool UseIndex, bool UseColor) {
            useNormals = UseNormals;
            useTexture = UseTexture;
            useIndex = UseIndex;
            useColor = UseColor;

            VertexArray = new Vector3d[Size];
            if(UseNormals)
                NormalsArray = new Vector3d[Size];
            if(UseTexture)
                TextureArray = new Vector2d[Size];
            if(UseIndex)
                IndexArray = new uint[Size];
            if(UseColor)
                ColorArray = new ColorStruct[Size];
        }
        public VBO(LoadResult result) {
            useTexture = false;
            useIndex = false;
            useColor = true;
            useNormals = true;
            PrimitiveType = PrimitiveType.Quads;


            List<Vector3d> VertexArrayList = new List<Vector3d>();
            List<Vector3d> NormalArrayList = new List<Vector3d>();
            List<ColorStruct> ColorArrayList = new List<ColorStruct>();
            List<uint> IndexArrayList = new List<uint>();

            foreach(var item in result.Vertices) {
                //VertexArrayList.Add(new Vector3d(item.X, item.Y, item.Z));
            }


            foreach(var item in result.Groups) {
                foreach(var item2 in item.Faces) {
                    for(int i = 0; i < item2.Count; i++) {
                        /*
                        //VertexArrayList.Add(item2[i].
                        NormalArrayList.Add(new Vector3d(result.Normals[i].X, result.Normals[i].Y, result.Normals[i].Z));
                        ColorArrayList.Add(new ColorStruct(Color.FromArgb((int)(item.Material.DiffuseColor.X * 255), (int)(item.Material.DiffuseColor.Y * 255), (int)(item.Material.DiffuseColor.Z * 255))));
                        IndexArrayList.Add((uint)item2[i].VertexIndex);*/
                        //*
                        VertexArrayList.Add(new Vector3d(result.Vertices[item2[i].VertexIndex - 1].X, result.Vertices[item2[i].VertexIndex - 1].Y, result.Vertices[item2[i].VertexIndex - 1].Z));
                        NormalArrayList.Add(new Vector3d(result.Normals[item2[i].NormalIndex - 1].X, result.Normals[item2[i].NormalIndex - 1].Y, result.Normals[item2[i].NormalIndex - 1].Z));
                        ColorArrayList.Add(new ColorStruct(Color.FromArgb((int)(item.Material.DiffuseColor.X * 255), (int)(item.Material.DiffuseColor.Y * 255), (int)(item.Material.DiffuseColor.Z * 255))));
                        //IndexArrayList.Add((uint) VertexArrayList.IndexOf(VertexArrayList[VertexArrayList.Count-1]));
                        //IndexArrayList.Add((uint)item2[i].VertexIndex);
                        //*/

                    }
                    
                }
            }

            VertexArray = VertexArrayList.ToArray();
            ColorArray = ColorArrayList.ToArray();
            NormalsArray = NormalArrayList.ToArray();
            IndexArray = IndexArrayList.ToArray();


        }


        public void Load() {
            GL.GenBuffers(1, out VertexArrayID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexArrayID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexArray.Length * VertexGap), VertexArray, BufferUsageHint.StreamDraw);
            GL.VertexPointer(3, VertexPointerType.Double, VertexGap, IntPtr.Zero);

            if(useNormals) {
                GL.GenBuffers(1, out NormalsArrayID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, NormalsArrayID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(NormalsArray.Length * NormalsGap), NormalsArray, BufferUsageHint.StreamDraw);
                GL.NormalPointer(NormalPointerType.Double, NormalsGap, IntPtr.Zero);
            }
            if(useTexture) {
                GL.GenBuffers(1, out TextureArrayID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, TextureArrayID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(TextureArray.Length * TextureGap), TextureArray, BufferUsageHint.StreamDraw);
                GL.TexCoordPointer(2, TexCoordPointerType.Double, TextureGap, IntPtr.Zero);
            }
            if(useColor) {
                GL.GenBuffers(1, out ColorArrayID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, ColorArrayID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(ColorArray.Length * ColorGap), ColorArray, BufferUsageHint.StreamDraw);
                GL.ColorPointer(4, ColorPointerType.UnsignedByte, ColorGap, IntPtr.Zero);
            }
            if(useIndex) {
                GL.GenBuffers(1, out IndexArrayID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, IndexArrayID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(IndexArray.Length * IndexGap), IndexArray, BufferUsageHint.StreamDraw);
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Unload() {
            GL.DeleteBuffers(1, ref VertexArrayID);
            if(useNormals)
                GL.DeleteBuffers(1, ref NormalsArrayID);
            if(useTexture)
                GL.DeleteBuffers(1, ref TextureArrayID);
            if(useIndex)
                GL.DeleteBuffers(1, ref IndexArrayID);
            if(useColor)
                GL.DeleteBuffers(1, ref ColorArrayID);
        }

        public void Update() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexArrayID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexArray.Length * VertexGap), VertexArray, BufferUsageHint.StreamDraw);

            if(useNormals) {
                GL.BindBuffer(BufferTarget.ArrayBuffer, NormalsArrayID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(NormalsArray.Length * NormalsGap), NormalsArray, BufferUsageHint.StreamDraw);
            }
            if(useTexture) {
                GL.BindBuffer(BufferTarget.ArrayBuffer, TextureArrayID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(TextureArray.Length * TextureGap), TextureArray, BufferUsageHint.StreamDraw);
            }
            if(useColor) {
                GL.BindBuffer(BufferTarget.ArrayBuffer, ColorArrayID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(ColorArray.Length * ColorGap), ColorArray, BufferUsageHint.StreamDraw);
            }
            if(useIndex) {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexArrayID);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(IndexArray.Length * IndexGap), IndexArray, BufferUsageHint.StreamDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Draw() {
            EnableClientStates();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexArrayID);
            GL.VertexPointer(3, VertexPointerType.Double, VertexGap, IntPtr.Zero);



            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorArrayID);
            GL.ColorPointer(4, ColorPointerType.UnsignedByte, ColorGap, IntPtr.Zero);

            GL.BindBuffer(BufferTarget.ArrayBuffer, NormalsArrayID);
            GL.NormalPointer(NormalPointerType.Double, NormalsGap, IntPtr.Zero);

            GL.BindBuffer(BufferTarget.ArrayBuffer, IndexArrayID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(IndexArray.Length * IndexGap), IndexArray, BufferUsageHint.StreamDraw);
            if(useIndex) {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexArrayID);
                GL.DrawElements(PrimitiveType, IndexArray.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            } else {
                GL.DrawArrays(PrimitiveType, 0, VertexArray.Length);
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            DisableClientStates();
        }

        private void EnableClientStates() {
            if(useTexture) {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, TextureID);
                GL.EnableClientState(ArrayCap.TextureCoordArray);
            }
            if(useNormals) {
                GL.EnableClientState(ArrayCap.NormalArray);
            }
            if(useColor) {
                GL.EnableClientState(ArrayCap.ColorArray);
            }
            if(useIndex) {
                GL.EnableClientState(ArrayCap.IndexArray);
            } else {
                GL.EnableClientState(ArrayCap.VertexArray);
            }
        }
        private void DisableClientStates() {
            if(useTexture) {
                GL.Disable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            }
            if(useNormals) {
                GL.DisableClientState(ArrayCap.NormalArray);
            }
            if(useColor) {
                GL.DisableClientState(ArrayCap.ColorArray);
            }
            if(useIndex) {
                GL.DisableClientState(ArrayCap.IndexArray);
            } else {
                GL.DisableClientState(ArrayCap.VertexArray);
            }
        }

    }
}
