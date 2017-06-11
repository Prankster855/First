using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace First.MainGame.GameObjects {
    public class Player : GameObject {

        static Player player;

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
        public float speed;
        float movementDistance;
        bool alternateMovement;
        public int reach;

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
        }

        public override void Update() {
            base.Update();
            elapsed += Time.deltaTime;
            if(elapsed > (1f / (speed / movementDistance))) {
                Camera.zoomtarget = 2f;
                Movement();
            } else {
                Camera.zoomtarget = 1.75f;
            }
        }

        void Movement() {
            if(alternateMovement) {
                alternateMovement = false;
                if(Input.keyboardstate.IsKeyDown(Keys.D)) {
                    //right
                    position.X += movementDistance;

                    elapsed = 0f;
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.A)) {
                    //left
                    position.X += -movementDistance;

                    elapsed = 0f;
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.W)) {
                    //up
                    position.Y += -movementDistance;

                    elapsed = 0f;
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.S)) {
                    //down
                    position.Y += movementDistance;

                    elapsed = 0f;
                    return;
                }


            } else {
                alternateMovement = true;
                if(Input.keyboardstate.IsKeyDown(Keys.W)) {
                    //up
                    position.Y += -movementDistance;

                    elapsed = 0f;
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.S)) {
                    //down
                    position.Y += movementDistance;

                    elapsed = 0f;
                    return;
                }
                if(Input.keyboardstate.IsKeyDown(Keys.A)) {
                    //left
                    position.X += -movementDistance;

                    elapsed = 0f;
                    return;
                }

                if(Input.keyboardstate.IsKeyDown(Keys.D)) {
                    //right
                    position.X += movementDistance;

                    elapsed = 0f;
                    return;
                }


            }

        }
    }
}
