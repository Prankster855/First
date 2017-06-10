using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using First.MainGame;

namespace First.MainGame {
    public class Sprite {

        public static Dictionary<String, Texture2D> SpriteDictionary;

        public static void AddSprite(String name, Texture2D sprite) {
            SpriteDictionary.Add(name, sprite);
        }

        public static Texture2D GetSprite(String name) {
            return SpriteDictionary [name];
        }

        Texture2D sprite;
        public int width;
        public int height;


        public Sprite(Texture2D sprite) {
            this.sprite = sprite;
            width = sprite.Width;
            height = sprite.Height;
        }
        public Sprite() {
            sprite = Sprite.SpriteDictionary ["Blank"];
            width = sprite.Width;
            height = sprite.Height;
        }

        public void Draw(SpriteBatch sb, Vector2 position, int Layer) {
            Draw(sb, position, Layer, 1f);
        }

        public void Draw(SpriteBatch sb, Vector2 position, int layer, float scale) {
            Vector2 a = Camera.Pos / World.TileSize;

            Color c;


            if(layer < (int) Layer.Light) {
                if(layer == (int) Layer.Selection) {
                    c = Color.White;
                } else {
                    c = World.getLightMap(position);
                }
            } else {
                c = Color.White;
            }

            if(position.X > a.X - (Game1.screensize.X / World.TileSize * 2) &&
                position.X < a.X + (Game1.screensize.X / World.TileSize * 2) &&
                position.Y > a.Y - (Game1.screensize.Y / World.TileSize * 2) &&
                position.Y < a.Y + (Game1.screensize.Y / World.TileSize * 2)) {
                sb.Draw(sprite, new Vector2(
                                (position.X * World.TileSize),
                                (position.Y * World.TileSize)),
                            null, c, 0f, Vector2.Zero, scale, SpriteEffects.None, (float) layer / 2048);
            }



        }



    }
}
