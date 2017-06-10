using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using First.MainGame;

namespace First.MainGame.Units {
    public class Grass : Unit {
        public Grass(Tile parent) : base(parent, UnitType.Grass) {
            sprite = new Sprite(Sprite.SpriteDictionary ["Grass"]);
        }

    }
}
