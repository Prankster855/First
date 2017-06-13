using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using First.MainGame.Units;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.IO;
using First.MainGame.GameObjects;
using System.Diagnostics;

namespace First.MainGame {
    public class World {
        [JsonIgnore]
        public const int TileSize = 32;
        [JsonIgnore]
        public const int TickRate = 30;
        [JsonIgnore]
        private int daylength = 60;
        public float time = 0;
        private float nexttick = 0;
        [JsonIgnore]
        public int tick = 0;

        public float globallight;
        public bool night = false;

        public Dictionary <Vector2, Tile> map = new Dictionary<Vector2, Tile>();

        [JsonIgnore]
        public List<Tile> visible = new List<Tile>();

        [JsonIgnore]
        public const float layerincrement = 1f/2048f;

        #region Random
        [JsonIgnore]
        public int seed=0;
        private Vector2 oldHeld;
        private Vector2 held = Vector2.Zero;
        #endregion

        private Vector2 lastplayerpos;
        bool didProcessLightMap;
        public void Update() {
            time += Time.deltaTime;
            nexttick += Time.deltaTime;

            //TODO: check if player moves a certain distance then apply
            if(lastplayerpos != Player.player.position) {
                processVisibleTiles();
                //process visible lights
                Light.processVisibleLights();
                Light.processLightMap();
                didProcessLightMap = true;
            }


            //tick
            while(nexttick > 1f / TickRate) {
                nexttick -= 1f / TickRate;
                tick++;
                doDayLightCycle();
                //process lightmap
                if(!didProcessLightMap && tick % 2 == 0) {
                    Light.processLightMap();
                }
                tickTiles();
            }

            processMouseInput();

            lastplayerpos = Player.player.position;
            didProcessLightMap = false;

        }
        private void doDayLightCycle() {
            //float g = time % daylength;
            //TODO: fix eternal darkness thing
            if(!night) {
                Light.globalLight += (1f / TickRate) / daylength;
                if(Light.globalLight >= Light.maxlight) {
                    night = true;
                    Light.globalLight = Light.maxlight;
                }
            } else {
                Light.globalLight -= (1f / TickRate) / daylength;
                if(Light.globalLight <= Light.minlight) {
                    night = false;
                    Light.globalLight = Light.minlight;
                }
            }
        }


        private void tickTiles() {
            int size = 16;
            for(var x = (int) ((Camera.Pos.X / World.TileSize) - Math.Round(GraphicalSettings.screensize.X / 2 / World.TileSize)) - size;
                x < (int) ((Camera.Pos.X / World.TileSize) + Math.Round(GraphicalSettings.screensize.X / 2 / World.TileSize)) + size; x++) {
                for(var y = (int) ((Camera.Pos.Y / World.TileSize) - Math.Round(GraphicalSettings.screensize.Y / 2 / World.TileSize)) - size;
                    y < (int) ((Camera.Pos.Y / World.TileSize) + Math.Round(GraphicalSettings.screensize.Y / 2 / World.TileSize)) + size; y++) {
                    Vector2 f = new Vector2(x, y);
                    if(!map.ContainsKey(f)) {
                        createTile(f);
                    }
                    map [f].Tick();
                }

            }
        }

        private void createTile(Vector2 position) {
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

        public void processVisibleTiles() {
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

        private void processMouseInput() {
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

        public void Render(SpriteBatch sb) {

            foreach(Tile t in visible) {
                t.Render(sb);
            }

            Light.Render(sb, true);

        }
    }
}
