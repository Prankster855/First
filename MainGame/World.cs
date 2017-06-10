using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using First.MainGame.Units;
using Microsoft.Xna.Framework;
using First.MainGame.GameObjects;

namespace First.MainGame {
    public class World {

        public static int TileSize = 32;

        static Vector2 offset = Vector2.Zero;

        //tiles
        public static Dictionary <Vector2, Tile> map = new Dictionary<Vector2, Tile>();
        public static List<Tile> visible = new List<Tile>();

        //rendering
        public const float layerincrement = 1f/2048f;

        #region Random
        public static int seed=0;
        private static Vector2 oldHeld;
        private static Vector2 held = Vector2.Zero;
        #endregion

        static public void Update() {

            //Find visible tiles in screenspace
            processVisibleTiles();

            //update visible tiles
            foreach(Tile t in visible) {
                t.Update();
            }
            //update lights
            foreach(Light l in lights) {
                l.Update();
            }

            //process visible lights
            processVisibleLights();

            //process lightmap
            processLightMap();

            //handle mouse input
            processMouseInput();

        }

        #region Lights

        //lights
        public static float globalLight = .1f;
        public static List<Light> lights = new List<Light>();
        public static Dictionary<Vector2, Light> visibleLights = new Dictionary<Vector2, Light>();
        public static Dictionary<Vector2, Color> lightMap = new Dictionary<Vector2, Color>();

        static public void addLight(Light light) {
            lights.Add(light);
        }

        static public void removeLight(Light light) {
            lights.Remove(light);
        }

        static private void processVisibleLights() {

            Dictionary<Vector2, Light> temp = new Dictionary<Vector2, Light>();

            foreach(Light l in lights) {
                temp.Add(l.position, l);
            }

            visibleLights.Clear();
            int extend = 20;
            for(var x = (int) ((Camera.Pos.X / World.TileSize) - Math.Round(Game1.screensize.X / 2 / World.TileSize)) - extend; x < (int) ((Camera.Pos.X / World.TileSize) + Math.Round(Game1.screensize.X / 2 / World.TileSize)) + extend; x++) {
                for(var y = (int) ((Camera.Pos.Y / World.TileSize) - Math.Round(Game1.screensize.Y / 2 / World.TileSize)) - extend; y < (int) ((Camera.Pos.Y / World.TileSize) + Math.Round(Game1.screensize.Y / 2 / World.TileSize)) + extend; y++) {

                    Vector2 v = new Vector2(x, y);
                    //v is every visible position
                    if(temp.ContainsKey(v)) {
                        visibleLights.Add(v, temp [v]);
                    }

                }

            }
        }

        private static void processLightMap() {
            lightMap.Clear();
            foreach(Tile t in visible) {
                //for every tile in visible, 
                foreach(Light l in visibleLights.Values) {
                    //get the light values from all intersecting lights
                    if(Vector2.Distance(t.position, l.position) <= l.radius) {
                        addColortoLightmap(t.position, l.color);
                    } else {
                        //addColortoLightmap(t.position, new Color(10, 10, 10, 255));
                    }
                }

            }

        }

        public static void addColortoLightmap(Vector2 position, Color color) {
            if(lightMap.ContainsKey(position)) {
                if(lightMap [position] != color) {
                    Color a = lightMap [position];
                    Color c = new Color(color.R - ((color.R - a.R) / 2),
                        color.G - ((color.G - a.G) / 2),
                        color.B - ((color.B - a.B) / 2), 255);
                    lightMap [position] = c;
                }

            } else {
                lightMap.Add(position, color);
            }
        }

        public static Color getLightMap(Vector2 position) {

            position = new Vector2((float) Math.Round(position.X), (float) Math.Round(position.Y));

            if(lightMap.ContainsKey(position)) {
                return lightMap [position];
            }
            int a = (int) (255 * World.globalLight);
            Color c = new Color(a, a, a, 255);

            return c;
        }

        static public Color getLightValue(Vector2 position) {
            if(lightMap.ContainsKey(position)) {
                return lightMap [position];
            } else {
                return Color.Blue;
            }
        }

        #endregion

        static private void createTile(Vector2 position) {
            seed++;
            int baseseed = (int) DateTime.Now.Ticks;
            Random rnd = new Random(baseseed + seed);

            Tile a = new Tile(position);
            if(rnd.Next(1, 20) == 2) {
                a.bottom = new Grass(a);
                a.top = new LongGrass(a);
            } else {
                a.bottom = new Grass(a);
            }

            map.Add(position, a);

        }

        static private void processVisibleTiles() {
            visible.Clear();
            for(var x = (int) ((Camera.Pos.X / World.TileSize) - Math.Round(Game1.screensize.X / 2 / World.TileSize)) - 2; x < (int) ((Camera.Pos.X / World.TileSize) + Math.Round(Game1.screensize.X / 2 / World.TileSize)) + 2; x++) {
                for(var y = (int) ((Camera.Pos.Y / World.TileSize) - Math.Round(Game1.screensize.Y / 2 / World.TileSize)) - 2; y < (int) ((Camera.Pos.Y / World.TileSize) + Math.Round(Game1.screensize.Y / 2 / World.TileSize)) + 2; y++) {
                    Vector2 f = new Vector2(x, y);
                    if(!map.ContainsKey(f)) {
                        createTile(f);
                    }
                    visible.Add(map [f]);
                }

            }
        }

        static private void processMouseInput() {
            Vector2 b = Handler.mouseToWorld(Handler.mousestate) + new Vector2(-.5f, -.5f);
            Vector2 a = new Vector2((float) Math.Round(b.X), (float) Math.Round(b.Y));
            oldHeld = held;
            held = new Vector2(.1f, .1f);

            if(map.ContainsKey(a)) {
                if(Handler.MouseButtons.Contains(MouseButton.Left) && Handler.oldMouseButtons.Contains(MouseButton.Left)) {
                    if(Selection.Allowed) {
                        map [a].Hold();
                        held = a;
                    }
                } else if(Handler.pressedOnce(MouseButton.Left)) {
                    if(Selection.Allowed) {
                        map [a].Interact(MouseButton.Left);

                    }
                }

            }

            if(oldHeld != held) {
                if(map.ContainsKey(oldHeld)) {
                    map [oldHeld].UnHold();
                }

            }
        }

        static public void Render(SpriteBatch sb) {
            foreach(Tile t in visible) {
                t.Render(sb);
            }
        }

    }
}
