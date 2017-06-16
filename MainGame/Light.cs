using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using First.MainGame;
using Newtonsoft.Json;

namespace First.MainGame {
    public class Light {

        //TODO: color the lights!!
        //TODO: custom resample size?
        //TODO: refactor lightmap?

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

        public static float globalLight = .25f;
        public static float minlight = .25f;
        public static float maxlight = .55f;
        public static List<Light> lights = new List<Light>();
        public static Dictionary<Vector2, List<Light>> visibleLights = new Dictionary<Vector2, List<Light>>();
        public static Dictionary<Vector2, float> LightMap = new Dictionary<Vector2, float>();
        public static List<Vector2> resample = new List<Vector2>();

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
            int extend = 5;
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
            resample.Clear();
            foreach(Tile t in Handler.world.visible) {
                resample.Add(new Vector2(t.position.X + 0f, t.position.Y + 0f));
                resample.Add(new Vector2(t.position.X + .5f, t.position.Y + 0f));
                resample.Add(new Vector2(t.position.X + 0f, t.position.Y + .5f));
                resample.Add(new Vector2(t.position.X + .5f, t.position.Y + .5f));
            }
            //instead of going through every position of resample, find what SHOULD be by the lights
            foreach(Vector2 v in resample) {
                addToLightMap(v, globalLight);
            }

            foreach(List<Light> ll in visibleLights.Values) {
                List<Vector2> a = new List<Vector2>();
                Light temp = new Light(Vector2.Zero, 0, 0, Color.White);

                foreach(Light l in ll) {
                    temp.position = l.position;
                    if(l.radius > temp.radius) {
                        temp.radius = l.radius;
                    }
                }

                temp.position.X = (float) Math.Round(temp.position.X);
                temp.position.Y = (float) Math.Round(temp.position.Y);

                for(float x = temp.position.X - temp.radius - 1; x < temp.position.X + temp.radius + 1; x += .5f) {
                    for(float y = temp.position.Y - temp.radius - 1; y < temp.position.Y + temp.radius + 1; y += .5f) {
                        a.Add(new Vector2(x, y));
                    }

                }

                foreach(Light l in ll) {

                    foreach(Vector2 v in a) {

                        float dis = ((v.X - l.position.X) * (v.X - l.position.X)) + ((v.Y - l.position.Y) * (v.Y - l.position.Y)) + .01f;
                        float r = l.radius * l.radius;
                        if(dis <= r) {
                            addToLightMap(v, (dis / r) * l.intensity);
                        }
                    }
                }

            }
            /*
            foreach(Vector2 v in resample) {

                addToLightMap(v, globalLight);

                foreach(List<Light> ll in visibleLights.Values) {
                    foreach(Light l in ll) {
                        if(v.X > l.position.X - l.radius && v.X < l.position.X + l.radius &&
                           v.Y > l.position.Y - l.radius && v.Y < l.position.Y + l.radius) {
                            float dis = ((v.X - l.position.X) * (v.X - l.position.X)) + ((v.Y - l.position.Y) * (v.Y - l.position.Y));
                            float r = l.radius * l.radius;
                            if(dis <= r) {
                                addToLightMap(v, (dis / r) * l.intensity);
                            }
                        }
                    }
                }

            }
            */
        }

        static void addToLightMap(Vector2 v, float f) {
            if(LightMap.ContainsKey(v)) {
                LightMap [v] *= f;
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

        //TODO: if a tile isnt affected, make transparent to x, and have a black background within game

        public static void Render(SpriteBatch sb, bool b) {
            foreach(Vector2 v in resample) {
                Sprite.drawcalls++;
                sb.Draw(Sprite.SpriteDictionary ["Black"], v * World.TileSize,
                        null, new Color(255, 255, 255,
                        (int) (getLightMap(v) * 255)), 0f, Vector2.Zero, 1f, SpriteEffects.None, (float) Layer.Light / 2048);
            }

        }



    }
}
