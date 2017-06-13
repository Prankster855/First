using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using First.MainGame.GameObjects;

namespace First.MainGame {
    public class GroundItem : GameObject {
        Item item;
        Vector2 velocity;
        float elapsed;
        float speed = .75f;

        private static float killtime = 300f;

        public GroundItem(Item item) : base(item.position + new Vector2(.5f, .5f), new Sprite(Sprite.SpriteDictionary ["Error"]), Layer.Item) {
            this.item = item;
            sprite = item.sprite;
        }
        public GroundItem(Unit unit) : base(unit.parent.position + new Vector2(.5f, .5f), new Sprite(Sprite.SpriteDictionary ["Error"]), Layer.Item) {
            item = new Item(unit.type);
            sprite = item.sprite;
        }

        public override void Init() {
            base.Init();
            Random rnd = new Random((int) DateTime.Now.Ticks);
            int a = rnd.Next(0, 360);
            float rot = (float) (a / (Math.PI));
            velocity = new Vector2((float) Math.Cos(rot), (float) Math.Sin(rot)) * 1.5f;
        }

        public void PickUp() {
            Player.player.inventory.addItem(item);
            Delete();
        }

        public override void Update() {
            base.Update();

            Player p = Player.getPlayer();
            float dis = Vector2.Distance(position, p.position);

            if(dis <= 2.25f) {
                position += (p.position - position) * Time.deltaTime * speed;
            }
            if(dis <= .75f) {
                PickUp();
            }

            elapsed += Time.deltaTime;

            velocity *= (float) Math.Pow(Math.E, Math.Log(.15) * Time.deltaTime);

            position += velocity * Time.deltaTime;

            if(elapsed > GroundItem.killtime) {
                Delete();
            }
        }

        public void addToInventory() {
            Player.player.inventory.addItem(item);
            Delete();
        }

        public override void Render(SpriteBatch sb) {
            sprite.Draw(sb, position, (int) layer, .5f);
        }




    }
}
