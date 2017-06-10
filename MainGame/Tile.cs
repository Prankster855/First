using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using First.MainGame.Units;

namespace First.MainGame {
    public class Tile : GameObject {

        public Unit bottom;
        public Unit top;

        public Tile(Vector2 position) : base(position, new Sprite(), Layer.Ground) {
            bottom = new Air(this);
            top = new Air(this);
        }

        public override void Update() {
            base.Update();
            bottom.Update();
            top.Update();
        }

        public void Interact(MouseButton mb) {
            if(top is Air) {
                bottom.Interact(mb);
            } else {
                top.Interact(mb);
            }
        }

        public void Hold() {
            if(top is Air) {
                bottom.Hold();
            } else {
                top.Hold();
            }
        }

        public void UnHold() {
            if(top is Air) {
                bottom.UnHold();
            } else {
                top.UnHold();
            }
        }


        public override void Render(SpriteBatch sb) {
            if(top.isOpaque) {
                top.Render(sb, 1);
            } else {
                top.Render(sb, 1);
                bottom.Render(sb, 0);
            }

        }

    }
}
