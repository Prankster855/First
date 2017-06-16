using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace First.MainGame {
    public class Handler {

        public static World world = new World();

        public static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> removeQueue = new List<GameObject>();
        public static SaveState savestate = new SaveState();

        static public Vector2 mouseToWorld(MouseState ms) {
            return Vector2.Transform(new Vector2(ms.X, ms.Y), Matrix.Invert(Camera.matrix)) / World.TileSize;
        }

        static public void addGameObject(GameObject go) {
            gameObjects.Add(go);
            //Console.WriteLine("Added gameobject at position " + go.position.X + ", " + go.position.Y);
        }
        static public void removeGameObject(GameObject go) {
            removeQueue.Add(go);
        }
        static private void removequeue() {
            foreach(var go in removeQueue) {
                gameObjects.Remove(go);
            }
        }

        static public void Update() {

            Input.Update();
            world.Update();
            Camera.Update();
            foreach(var go in gameObjects) {
                go.Update();
            }
            removequeue();
        }

        static public void Render(SpriteBatch sb) {
            foreach(var go in gameObjects) {
                Vector2 pos = go.position;
                if(pos.X > (Camera.Pos.X / World.TileSize) - (GraphicalSettings.screensize.X / World.TileSize / 2) &&
                    pos.X < (Camera.Pos.X / World.TileSize) + (GraphicalSettings.screensize.X / World.TileSize / 2) &&
                    pos.Y > (Camera.Pos.Y / World.TileSize) - (GraphicalSettings.screensize.Y / World.TileSize / 2) &&
                    pos.Y < (Camera.Pos.Y / World.TileSize) + (GraphicalSettings.screensize.Y / World.TileSize / 2)) {
                    go.Render(sb);
                }
            }
            Handler.world.Render(sb);
            //Console.WriteLine(Sprite.drawcalls);
        }


    }
}
