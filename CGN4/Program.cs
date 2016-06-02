using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGN4 {
    class Program {
        static void Main(string[] args) {
            Game game = new Game();
            game.ClientSize = new System.Drawing.Size(400, 400);
            game.Run(60);
        }
    }
}
