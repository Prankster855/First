using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace First.MainGame {
    public class GameObject {

        public Vector2 position;
        [JsonIgnore]
        public Sprite sprite;
        [JsonIgnore]
        public Layer layer;

        public GameObject(Vector2 position, Sprite sprite, Layer layer) {
            this.position = position;
            this.sprite = sprite;
            this.layer = layer;

            Init();
        }

        public GameObject(Vector2 position) {
            this.position = position;
            Init();
        }

        public void Delete() {
            Handler.removeGameObject(this);
        }

        public virtual void Init() {

        }

        public virtual void Update() {

        }

        public virtual void Render(SpriteBatch sb) {
            sprite.Draw(sb, position, (int) layer);
        }

    }

}
