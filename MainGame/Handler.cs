using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First.MainGame {
    public class Handler {

        private static List<GameObject> gameObjects = new List<GameObject>();
        public static KeyboardState keyboardstate = new KeyboardState();
        public static KeyboardState oldKeyboardstate = new KeyboardState();
        public static MouseState mousestate = new MouseState();
        public static MouseState oldMousestate = new MouseState();
        public static List<MouseButton> oldMouseButtons = new List<MouseButton>();
        public static List<MouseButton> MouseButtons = new List<MouseButton>();
        private static List<GameObject> removeQueue = new List<GameObject>();


        static public bool pressedOnce(Keys a) {
            if(!oldKeyboardstate.IsKeyDown(a) && keyboardstate.IsKeyDown(a)) {
                return true;
            }
            return false;
        }
        static public bool pressedOnce(MouseButton button) {
            if(button == MouseButton.Left) {
                if(oldMousestate.LeftButton != ButtonState.Pressed && mousestate.LeftButton == ButtonState.Pressed) {
                    return true;
                }
            }
            if(button == MouseButton.Right) {
                if(oldMousestate.RightButton != ButtonState.Pressed && mousestate.RightButton == ButtonState.Pressed) {
                    return true;
                }
            }
            return false;
        }

        static public void processMouseButtons() {
            oldMouseButtons = MouseButtons;
            MouseButtons = new List<MouseButton>();
            if(mousestate.LeftButton == ButtonState.Pressed) {
                MouseButtons.Add(MouseButton.Left);
            }
            if(mousestate.RightButton == ButtonState.Pressed) {
                MouseButtons.Add(MouseButton.Right);
            }

        }

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
            oldKeyboardstate = keyboardstate;
            keyboardstate = Keyboard.GetState();
            oldMousestate = mousestate;
            mousestate = Mouse.GetState();
            processMouseButtons();
            World.Update();
            foreach(var go in gameObjects) {
                go.Update();
            }
            removequeue();
            Camera.Update();
        }

        static public void Render(SpriteBatch sb) {
            foreach(var go in gameObjects) {
                Vector2 pos = go.position;
                if(pos.X > (Camera.Pos.X / World.TileSize) - (Game1.screensize.X / World.TileSize / 2) &&
                    pos.X < (Camera.Pos.X / World.TileSize) + (Game1.screensize.X / World.TileSize / 2) &&
                    pos.Y > (Camera.Pos.Y / World.TileSize) - (Game1.screensize.Y / World.TileSize / 2) &&
                    pos.Y < (Camera.Pos.Y / World.TileSize) + (Game1.screensize.Y / World.TileSize / 2)
                    ) {
                    go.Render(sb);
                }
            }
            World.Render(sb);
        }




    }
}
