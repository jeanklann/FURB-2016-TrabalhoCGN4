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
        public static VBO vbo;
        Vector3d DesiredPos;
        double angle2 = 0;
        Vector3d rotationAxis2 = new Vector3d(1, 0, 0);
        double angle3 = 0;
        Vector3d rotationAxis3 = new Vector3d(0, 0, 1);
        double angle4 = 0;
        Vector3d rotationAxis4 = new Vector3d(0, 1, 0);
        public Nave(){
            Scale = new Vector3d(0.1, 0.1, 0.1);
            RotationAxis = new Vector3d(0, 1, 0);
            Rotation = -90;
            vbo.Load();
        }


        public override void Draw() {
            GL.PushMatrix();
            GL.Translate(Position);
            GL.Rotate(Rotation, RotationAxis);
            GL.Rotate(angle2, rotationAxis2);
            GL.Rotate(angle3, rotationAxis3);
            GL.Rotate(angle4, rotationAxis4);
            GL.Scale(Scale);
            vbo.Draw();
            GL.PopMatrix();
        }

        public override void Update() {
            if(DesiredPos == Vector3d.Zero) {
                DesiredPos = Position;
            }
            var state = OpenTK.Input.Keyboard.GetState();
            if(state[Key.Escape]) {
                Console.WriteLine("Saindo do jogo...");
                Environment.Exit(0);
            }
            Vector3d LastPos = Position;
            foreach(var item in Game.MainGame.predios.predios) {
                if(BBox.DetectCollision(Position, item.BBox)){
                    Game.MainGame.gameState = GameState.Defeat;
                }
            }


            if(state[Key.Up]) {
                DesiredPos.Y += Game.MainGame.RenderTime * 2f;
            }
            if(state[Key.Down]) {
                DesiredPos.Y -= Game.MainGame.RenderTime * 2f;
            }
            if(state[Key.Left]) {
                DesiredPos.X += Game.MainGame.RenderTime * 2f;
            }
            if(state[Key.Right]) {
                DesiredPos.X -= Game.MainGame.RenderTime * 2f;
            }

            if(DesiredPos.X > 1f) DesiredPos.X = 1f;
            if(DesiredPos.X < -1f) DesiredPos.X = -1f;
            if(DesiredPos.Y > 1f) DesiredPos.Y = 1f;
            if(DesiredPos.Y < -1f) DesiredPos.Y = -1f;

            double distance1 = DesiredPos.X - Position.X;
            angle2 = -distance1*100;

            double distance2 = DesiredPos.Y - Position.Y;
            angle3 = -distance2*200;

            double distance3 = DesiredPos.X - Position.X;
            angle4 = -distance3*100;

            Position = Vector3d.Lerp(LastPos, DesiredPos, 0.2);

           
        }
        public bool DetectedCollision() {
            return false;
        }
    }
}
