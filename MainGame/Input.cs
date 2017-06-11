using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using First.MainGame;

namespace First.MainGame {
    public class Input {
        public static KeyboardState keyboardstate = new KeyboardState();
        public static KeyboardState oldKeyboardstate = new KeyboardState();
        public static MouseState mousestate = new MouseState();
        public static MouseState oldMousestate = new MouseState();
        public static List<MouseButton> oldMouseButtons = new List<MouseButton>();
        public static List<MouseButton> MouseButtons = new List<MouseButton>();

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

        public static void Update() {
            oldKeyboardstate = keyboardstate;
            keyboardstate = Keyboard.GetState();
            oldMousestate = mousestate;
            mousestate = Mouse.GetState();
            processMouseButtons();
        }


    }
}
