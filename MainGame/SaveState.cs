using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using First.MainGame;
using First.MainGame.GameObjects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace First.MainGame {
    public class SaveState {

        public Player player;
        public List<Light> lights;
        public World world;
        string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/saves/save.json";

        public void Save() {
            SaveWorld();
            SavePlayer();
            SaveLights();

            JsonSerializer serializer = new JsonSerializer();
            FileInfo a = new FileInfo(path);
            a.Directory.Create();
            using(StreamWriter sw = new StreamWriter(path))
            using(JsonWriter writer = new JsonTextWriter(sw)) {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, this);
            }

            Light.addLight(Player.player.light);
        }

        #region SAVE
        void SaveWorld() {
            Handler.world.globallight = Light.globalLight;
            world = Handler.world;
            Console.WriteLine("SAVE WORLD");
        }

        void SaveLights() {
            Light.removeLight(Player.player.light);
            lights = Light.lights;
        }

        void SavePlayer() {
            player = Player.player;
        }
        #endregion

        public void Load() {
            SaveState ss = JsonConvert.DeserializeObject<SaveState>(File.ReadAllText(path));
            LoadWorld(ss);
            LoadLights(ss);
            LoadPlayer(ss);
            Camera.Pos = (Player.player.position + new Vector2(.5f)) / World.TileSize;
        }

        #region LOAD
        void LoadWorld(SaveState ss) {
            foreach(GameObject gi in Handler.gameObjects) {
                if(gi is GroundItem) {
                    gi.Delete();
                }
            }
            Handler.world.map.Clear();
            foreach(Vector2 v in ss.world.map.Keys) {
                Tile t = new Tile(v);
                t.bottom = Unit.getUnit(t, ss.world.map [v].bottom.type);
                t.top = Unit.getUnit(t, ss.world.map [v].top.type);
                Handler.world.map.Add(v, t);
            }
            Handler.world.night = ss.world.night;
            Handler.world.time = ss.world.time;

            Console.WriteLine("LOAD WORLD");
        }
        void LoadLights(SaveState ss) {
            Light.lights = ss.lights;
            Light.globalLight = ss.world.globallight;
        }

        void LoadPlayer(SaveState ss) {
            Player.player.Init();
            Player.player.position = ss.player.position;
            Player.player.inventory = ss.player.inventory;
        }
        #endregion
    }
}
