using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace First.MainGame {
    public class Light : GameObject {

        #region GameObject

        public Color color;
        public int radius;
        public float intensity;

        public Light(Vector2 position, int? radius, float? intensity, Color? color)
            : base(position, new Sprite(Sprite.SpriteDictionary ["Black"]), Layer.Light) {
            this.color = color ?? Color.Red;
            this.radius = radius ?? 6;
            this.intensity = intensity ?? 1f;
        }
        #endregion



        public static float globalLight = minlight;
        public static float minlight = .1f;
        public static float maxlight = .9f;
        public static List<Light> lights = new List<Light>();
        public static Dictionary<Vector2, Light> visibleLights = new Dictionary<Vector2, Light>();
        public static Dictionary<Vector2, float> LightMap = new Dictionary<Vector2, float>();

        #region Add/Remove
        static public void addLight(Light light) {
            lights.Add(light);
        }

        static public void removeLight(Light light) {
            lights.Remove(light);
        }
        #endregion

        public static void processVisibleLights() {

            Dictionary<Vector2, Light> temp = new Dictionary<Vector2, Light>();

            foreach(Light l in lights) {
                temp.Add(l.position, l);
            }

            visibleLights.Clear();
            int extend = 20;
            for(var x = (int) ((Camera.Pos.X / World.TileSize) - Math.Round(GraphicalSettings.screensize.X / 2 / World.TileSize)) - extend; x < (int) ((Camera.Pos.X / World.TileSize) + Math.Round(GraphicalSettings.screensize.X / 2 / World.TileSize)) + extend; x++) {
                for(var y = (int) ((Camera.Pos.Y / World.TileSize) - Math.Round(GraphicalSettings.screensize.Y / 2 / World.TileSize)) - extend; y < (int) ((Camera.Pos.Y / World.TileSize) + Math.Round(GraphicalSettings.screensize.Y / 2 / World.TileSize)) + extend; y++) {

                    Vector2 v = new Vector2(x, y);
                    if(temp.ContainsKey(v)) {
                        visibleLights.Add(v, temp [v]);
                    }

                }

            }
        }

        public static void processLightMap() {
            LightMap.Clear();
            foreach(Tile t in World.visible) {
                addToLightMap(t.position, globalLight);

                foreach(Light l in visibleLights.Values) {

                    float dis = Vector2.Distance(t.position, l.position);
                    if(dis <= l.radius) {
                        addToLightMap(t.position, dis / l.radius);
                    }
                }
            }
        }

        private static void addToLightMap(Vector2 v, float f) {
            if(LightMap.ContainsKey(v)) {
                LightMap [v] = f * LightMap [v];
            } else {
                LightMap.Add(v, f);
            }
        }

        private static float getLightMap(Vector2 v) {
            if(LightMap.ContainsKey(v)) {
                float a = LightMap [v];
                if(a > globalLight) {
                    return globalLight;
                } else {
                    return a;
                }
            }
            return globalLight;

        }

        public static void Render(SpriteBatch sb, bool b) {
            foreach(Tile t in World.visible) {
                Vector2 v = t.position;

                sb.Draw(Sprite.SpriteDictionary ["Black"], v * World.TileSize,
                        null, new Color(255, 255, 255, (int) (getLightMap(v) * 255)), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float) Layer.Light / 2048);
            }

        }



    }
}
