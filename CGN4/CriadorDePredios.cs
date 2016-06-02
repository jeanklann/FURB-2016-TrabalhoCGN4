using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGN4 {
    public class CriadorDePredios {
        public Nave nave;
        public List<Predio> predios = new List<Predio>();
        int quant = 15;
        int offset = 3;
        Random random = new Random();
        public CriadorDePredios(Nave nave) {
            this.nave = nave;
            for(int i = 0; i < quant; i++) {
                Predio predio = new Predio();
                RandomPredio(predio);
                predio.Position.Z = - i * offset;
                predios.Add(predio);
            }
        }

        public void Update() {
            foreach(Predio predio in predios) {
                predio.Position.Z += Game.MainGame.RenderTime * 5;
                if(predio.Position.Z > 0) {
                    Game.MainGame.score++;
                    Console.WriteLine("Score: "+Game.MainGame.score);
                    RandomPredio(predio);
                    predio.Position.Z = -quant * offset;
                }
            }
        }
        public void RandomPredio(Predio predio) {
            predio.Position.X = (random.NextDouble() - 0.5) * 2 * 2;
        }

        public void Draw() {
            foreach(Predio predio in predios) {
                predio.Draw();
            }
        }
    }
}
