using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using First.MainGame.Units;
using Microsoft.Xna.Framework;

namespace First.MainGame {
    public class World {

        public static int TileSize = 32;

        static Vector2 offset = Vector2.Zero;

        static private int daylength = 5;

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

        static bool night = false;

        static public void Update() {

            if(!night) {
                Light.globalLight += Time.deltaTime / daylength;
                if(Light.globalLight >= Light.maxlight) {
                    night = true;
                }
            } else {
                Light.globalLight -= Time.deltaTime / daylength;
                if(Light.globalLight <= Light.minlight) {
                    night = false;
                }
            }



            //Find visible tiles in screenspace
            processVisibleTiles();

            //update visible tiles
            foreach(Tile t in visible) {
                t.Update();
            }
            //update lights
            foreach(Light l in Light.lights) {
                l.Update();
            }

            //process visible lights
            Light.processVisibleLights();

            //process lightmap
            Light.processLightMap();

            //handle mouse input
            processMouseInput();

        }

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
            for(var x = (int) ((Camera.Pos.X / World.TileSize) - Math.Round(GraphicalSettings.screensize.X / 2 / World.TileSize)) - 2; x < (int) ((Camera.Pos.X / World.TileSize) + Math.Round(GraphicalSettings.screensize.X / 2 / World.TileSize)) + 2; x++) {
                for(var y = (int) ((Camera.Pos.Y / World.TileSize) - Math.Round(GraphicalSettings.screensize.Y / 2 / World.TileSize)) - 2; y < (int) ((Camera.Pos.Y / World.TileSize) + Math.Round(GraphicalSettings.screensize.Y / 2 / World.TileSize)) + 2; y++) {
                    Vector2 f = new Vector2(x, y);
                    if(!map.ContainsKey(f)) {
                        createTile(f);
                    }
                    visible.Add(map [f]);
                }

            }
        }

        static private void processMouseInput() {
            Vector2 b = Handler.mouseToWorld(Input.mousestate) + new Vector2(-.5f, -.5f);
            Vector2 a = new Vector2((float) Math.Round(b.X), (float) Math.Round(b.Y));
            oldHeld = held;
            held = new Vector2(.1f, .1f);

            if(map.ContainsKey(a)) {
                if(Input.MouseButtons.Contains(MouseButton.Left) && Input.oldMouseButtons.Contains(MouseButton.Left)) {
                    if(Selection.Allowed) {
                        map [a].Hold();
                        held = a;
                    }
                } else if(Input.pressedOnce(MouseButton.Left)) {
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
            Light.Render(sb, true);
        }

    }
}
