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
        public Dictionary <Vector2, Tile> map;
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
            map = World.map;
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
            World.map.Clear();
            foreach(Vector2 v in ss.map.Keys) {
                Tile t = new Tile(v);
                t.bottom = Unit.getUnit(t, ss.map [v].bottom.type);
                t.top = Unit.getUnit(t, ss.map [v].top.type);
                World.map.Add(v, t);
            }
        }
        void LoadLights(SaveState ss) {
            Light.lights = ss.lights;
        }

        void LoadPlayer(SaveState ss) {
            Player.player.Init();
            Player.player.position = ss.player.position;
            Player.player.inventory = ss.player.inventory;
        }
        #endregion
    }
}
