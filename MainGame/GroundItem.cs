using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace First.MainGame {
    public class GroundItem : GameObject {
        Unit unit;
        Vector2 velocity;
        float elapsed;

        private static float killtime = 300f;

        public GroundItem(Unit unit) : base(unit.position + new Vector2(.5f, .5f), new Sprite(Sprite.SpriteDictionary ["Error"]), Layer.Item) {
            this.unit = unit;
            sprite = unit.sprite;
            Handler.addGameObject(this);
        }

        public override void Init() {
            base.Init();
            Random rnd = new Random((int) DateTime.Now.Ticks);
            int a = rnd.Next(0, 360);
            float rot = (float) (a / (Math.PI));
            velocity = new Vector2((float) Math.Cos(rot), (float) Math.Sin(rot)) * 1.5f;
        }

        public override void Update() {
            base.Update();

            elapsed += Time.deltaTime;

            velocity *= (float) Math.Pow(Math.E, Math.Log(.15) * Time.deltaTime);

            position += velocity * Time.deltaTime;

            if(elapsed > GroundItem.killtime) {
                Delete();
            }
        }

        public override void Render(SpriteBatch sb) {
            sprite.Draw(sb, position, (int) layer, .5f);
        }




    }
}
