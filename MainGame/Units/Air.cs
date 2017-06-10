using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First.MainGame.Units {
    public class Air : Unit {
        public Air(Tile parent) : base(parent, UnitType.Air) {
            sprite = new Sprite(Sprite.SpriteDictionary ["Blank"]);
            isOpaque = false;
        }

        public override void Interact(MouseButton mb) {
            base.Interact(mb);
        }
    }
}
