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

        public Inventory inventory = new Inventory(20,20);

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

        public static Player getPlayer() {
            return Player.player;
        }

        public override void Init() {
            base.Init();
            alternateMovement = false;
            direction = new Vector2(0, 0);
            reach = 4;
            movementDistance = (1f / 4f);
            speed = 2.5f;
            elapsed = 0f;
            light = new Light(position, 4, .25f, Color.White);
            Light.addLight(light);
        }

        public override void Update() {
            base.Update();

            light.position = position;
            if(elapsed < (1f / (speed / movementDistance))) {
                elapsed += Time.deltaTime;
            }
            if(elapsed > (1f / (speed / movementDistance))) {
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
                    position.X += movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.A)) {
                    //left
                    position.X += -movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.W)) {
                    //up
                    position.Y += -movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.S)) {
                    //down
                    position.Y += movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }


            } else {
                alternateMovement = true;
                if(Input.keyboardstate.IsKeyDown(Keys.W)) {
                    //up
                    position.Y += -movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.S)) {
                    //down
                    position.Y += movementDistance;

                    elapsed -= (1f / (speed / movementDistance));

                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.A)) {
                    //left
                    position.X += -movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }

                if(Input.keyboardstate.IsKeyDown(Keys.D)) {
                    //right
                    position.X += movementDistance;

                    elapsed -= (1f / (speed / movementDistance));
                    return;
                }


            }

        }
    }
}
