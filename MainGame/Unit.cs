using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using First.MainGame.Units;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace First.MainGame {
    public class Unit {

        [JsonProperty(PropertyName = "t")]
        public ItemType type;
        [JsonIgnore]
        public Tile parent;
        [JsonIgnore]
        public float durability = 1f;
        [JsonIgnore]
        public bool isSolid = false;
        [JsonIgnore]
        public bool isOpaque = true;
        [JsonIgnore]
        public bool isBreakable = false;
        [JsonIgnore]
        public bool held = false;
        [JsonIgnore]
        private float breaktime = 0f;
        [JsonIgnore]
        public Sprite sprite;

        [JsonConstructor]
        public Unit(Tile parent, ItemType type) {
            this.parent = parent;
            this.type = type;
            sprite = Item.ItemDictionary [type];
        }

        public Unit(Tile parent) {
            this.parent = parent;
            type = ItemType.Error;

        }

        public void Tick() {

        }

        public void Update() {

        }

        public void Render(SpriteBatch sb, int layer) {
            sprite.Draw(sb, parent.position, (int) parent.layer + layer);
        }

        public virtual void Interact(MouseButton mb) {
            Console.WriteLine("Clicked on " + parent.position + " : " + this);
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

        public virtual void Drop() {
            Handler.addGameObject(new GroundItem(this));
            parent.top = new Air(parent);
        }



        public static Unit getUnit(Tile parent, ItemType it) {
            switch(it) {
                case ItemType.Air:
                return new Air(parent);

                case ItemType.Error:
                Console.WriteLine("ERROR");
                return new Air(parent);

                case ItemType.Grass:
                return new Grass(parent);

                case ItemType.LongGrass:
                return new LongGrass(parent);

                default:
                Console.WriteLine("ERROR IN GETTING UNIT");
                break;
            }
            Console.WriteLine("ERROR IN GETTING UNIT");
            return new Unit(parent);
        }

    }
}
