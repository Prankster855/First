using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using First.MainGame;
using Newtonsoft.Json;

namespace First.MainGame {
    public class Light {

        //TODO: color

        #region GameObject
        [JsonProperty(PropertyName = "p")]
        public Vector2 position;
        [JsonProperty(PropertyName = "r")]
        public int radius;
        [JsonProperty(PropertyName = "i")]
        public float intensity;
        [JsonProperty(PropertyName = "c")]
        public Color color;
        private Sprite sprite;

        public Light(Vector2 position, int radius, float intensity, Color color) {
            this.position = position;
            this.color = color;
            this.radius = radius;
            this.intensity = 1 - intensity;
            sprite = new Sprite(Sprite.SpriteDictionary ["Black"]);
        }

        public void Remove() {
            Light.lights.Remove(this);
        }
        #endregion

        public static float globalLight = maxlight;
        public static float minlight = .2f;
        public static float maxlight = .6f;
        public static List<Light> lights = new List<Light>();
        public static Dictionary<Vector2, List<Light>> visibleLights = new Dictionary<Vector2, List<Light>>();
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

            Dictionary<Vector2, List<Light>> temp = new Dictionary<Vector2, List<Light>>();

            foreach(Light l in lights) {

                Vector2 a = new Vector2((float) Math.Round(l.position.X), (float) Math.Round(l.position.Y));
                if(temp.ContainsKey(a)) {
                    temp [a].Add(l);
                } else {
                    temp.Add(a, new List<Light>());
                    temp [a].Add(l);
                }
            }

            visibleLights.Clear();
            int extend = 20;
            for(var x = (int) ((Camera.Pos.X / World.TileSize) - Math.Round(GraphicalSettings.screensize.X / 2 / World.TileSize)) - extend; x < (int) ((Camera.Pos.X / World.TileSize) + Math.Round(GraphicalSettings.screensize.X / 2 / World.TileSize)) + extend; x++) {
                for(var y = (int) ((Camera.Pos.Y / World.TileSize) - Math.Round(GraphicalSettings.screensize.Y / 2 / World.TileSize)) - extend; y < (int) ((Camera.Pos.Y / World.TileSize) + Math.Round(GraphicalSettings.screensize.Y / 2 / World.TileSize)) + extend; y++) {

                    Vector2 v = new Vector2(x, y);
                    if(temp.ContainsKey(v)) {
                        foreach(Light l in temp [v]) {
                            addToVisible(v, l);
                        }
                    }

                }

            }
        }

        public static void addToVisible(Vector2 v, Light l) {
            if(visibleLights.ContainsKey(v)) {
                visibleLights [v].Add(l);
            } else {
                visibleLights.Add(v, new List<Light>());
                visibleLights [v].Add(l);
            }
        }

        public static void processLightMap() {
            LightMap.Clear();
            foreach(Tile t in World.visible) {
                addToLightMap(t.position, globalLight);

                foreach(List<Light> ll in visibleLights.Values) {
                    foreach(Light l in ll) {
                        float dis = Vector2.Distance(t.position, l.position);
                        if(dis <= l.radius) {
                            addToLightMap(t.position, (dis / l.radius) * l.intensity);
                        }
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
