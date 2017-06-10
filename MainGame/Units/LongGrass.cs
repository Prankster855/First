using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First.MainGame.Units {
    public class LongGrass : Unit {
        public LongGrass(Tile parent) : base(parent, UnitType.LongGrass) {
            sprite = new Sprite(Sprite.SpriteDictionary ["LongGrass"]);
            isBreakable = true;
            durability = .25f;
            isOpaque = false;
        }


    }
}
