using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using First.MainGame.Units;
using First.MainGame;
using Microsoft.Xna.Framework;

namespace First.MainGame {
    public class Unit : GameObject {

        public Tile parent;
        public UnitType type;

        //Defaults
        //Properties
        public float durability = 1f;
        public bool isSolid = false;
        public bool isOpaque = true;
        public bool isBreakable = false;
        public bool held = false;
        //Trackers
        private float breaktime = 0f;

        public Unit(Tile parent, UnitType type, Sprite sprite) : base(parent.position, new Sprite(), parent.layer) {
            this.parent = parent;
            this.sprite = sprite;
            this.type = type;
        }

        public Unit(Tile parent, UnitType type) : base(parent.position, new Sprite(), parent.layer) {
            this.parent = parent;
            type = UnitType.Error;

        }

        public Unit(Tile parent) : base(parent.position, new Sprite(), parent.layer) {
            this.parent = parent;
            type = UnitType.Error;

        }


        public virtual void Drop() {
            new GroundItem(this);
            parent.top = new Air(parent);
        }

        public override void Update() {
            base.Update();
        }

        public void Hold() {
            if(isBreakable) {
                Break();
            }
        }

        public void UnHold() {
            if(isBreakable) {
                breaktime = 0f;
            }

        }

        public void Break() {
            if(isBreakable) {
                breaktime += Time.deltaTime;
                if(breaktime > durability) {
                    Drop();
                }
            }
        }


        public virtual void Interact(MouseButton mb) {
            Console.WriteLine("Clicked on " + position + " : " + this);
        }



        public void Render(SpriteBatch sb, int layer) {
            sprite.Draw(sb, parent.position, (int) parent.layer + layer);
        }

    }
}
