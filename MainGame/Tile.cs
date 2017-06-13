using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using First.MainGame.Units;
using Newtonsoft.Json;

namespace First.MainGame {
    public class Tile {
        [JsonProperty(PropertyName = "b")]
        public Unit bottom;
        [JsonProperty(PropertyName = "t")]
        public Unit top;

        [JsonIgnore]
        public Layer layer = Layer.Ground;
        [JsonIgnore]
        public Vector2 position;

        public Tile(Vector2 position) {
            bottom = new Air(this);
            top = new Air(this);
            this.position = position;
        }

        public void Tick() {
            top.Tick();
            bottom.Tick();
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

        public void Render(SpriteBatch sb) {
            if(top.isOpaque) {
                top.Render(sb, 1);
            } else {
                top.Render(sb, 1);
                bottom.Render(sb, 0);
            }

        }

    }
}
