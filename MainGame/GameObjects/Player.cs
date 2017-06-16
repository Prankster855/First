using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace First.MainGame.GameObjects {
    public class Player : GameObject {

        public static Player player;

        public Player(Vector2 position, Sprite sprite)
        : base(position, sprite, Layer.Player) {
            if(Player.player == null) {
                Player.player = this;
            }
        }

        public Inventory inventory;

        //TODO: Collisions?
        Vector2 direction;
        float elapsed;
        [JsonIgnore]
        public float speed;
        float movementDistance;
        bool alternateMovement;
        [JsonIgnore]
        public int reach;
        [JsonIgnore]
        public Light light;
        Vector2 target;

        public static Player getPlayer() {
            return Player.player;
        }

        public override void Init() {
            base.Init();
            inventory = new Inventory(20, 20);
            alternateMovement = false;
            direction = new Vector2(0, 0);
            reach = 4;
            movementDistance = (1f / 4f);
            speed = 2.5f;
            elapsed = 0f;
            light = new Light(position, 4, .25f, Color.White);
            Light.addLight(light);
            target = position;
        }

        public override void Update() {
            base.Update();

            position += (target - position) * 50 * Time.deltaTime;

            light.position = position;

            float a = (1f / (speed / movementDistance));

            if(elapsed < a) {
                elapsed += Time.deltaTime;
            }
            if(elapsed > a) {
                Camera.zoomtarget = 2f;
                Movement();
            } else {
                Camera.zoomtarget = 1.75f;
            }
        }

        void Movement() {
            if(Input.keyboardstate.IsKeyDown(Keys.R)) {
                Handler.savestate.Save();
            }
            if(Input.keyboardstate.IsKeyDown(Keys.T)) {
                Handler.savestate.Load();
            }

            if(alternateMovement) {
                alternateMovement = false;
                if(Input.keyboardstate.IsKeyDown(Keys.D)) {
                    //right
                    target.X += movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.A)) {
                    //left
                    target.X += -movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.W)) {
                    //up
                    target.Y += -movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.S)) {
                    //down
                    target.Y += movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }


            } else {
                alternateMovement = true;
                if(Input.keyboardstate.IsKeyDown(Keys.W)) {
                    //up
                    target.Y += -movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.S)) {
                    //down
                    target.Y += movementDistance;

                    elapsed -= (1f / (speed / movementDistance));

                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.A)) {
                    //left
                    target.X += -movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }

                if(Input.keyboardstate.IsKeyDown(Keys.D)) {
                    //right
                    target.X += movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }


            }

        }
    }
}
