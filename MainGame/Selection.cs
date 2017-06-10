using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using First.MainGame.GameObjects;

namespace First.MainGame {
    public class Selection : GameObject {
        public static Selection selection;
        public Sprite Cursor = new Sprite(Sprite.SpriteDictionary["CursorBlue"]);
        public Sprite avaliableCursor = new Sprite(Sprite.SpriteDictionary["CursorBlue"]);
        public Sprite nullCursor = new Sprite(Sprite.SpriteDictionary["CursorRed"]);
        public Sprite CanSelect = new Sprite(Sprite.SpriteDictionary["Selectable"]);
        public Sprite CantSelect = new Sprite(Sprite.SpriteDictionary["NotSelectable"]);
        public static bool Allowed;

        public Selection() : base(Vector2.Zero, new Sprite(), Layer.Selection) {
            Selection.selection = this;
        }

        public override void Update() {
            Vector2 a = Handler.mouseToWorld(Handler.mousestate);
            position = new Vector2((float) Math.Round(a.X - .5), (float) Math.Round(a.Y - .5));
            Player p = Player.getPlayer();
            if(Vector2.Distance(Handler.mouseToWorld(Handler.mousestate), p.position) > p.reach) {
                Cursor = nullCursor;
                Allowed = false;
            } else {
                Cursor = avaliableCursor;
                Allowed = true;
            }



        }

        public override void Render(SpriteBatch sb) {
            Cursor.Draw(sb, Handler.mouseToWorld(Handler.mousestate), (int) Layer.Mouse, 1 / Camera._zoom);
            Vector2 a = Handler.mouseToWorld(Handler.mousestate) - new Vector2(.5f, .5f);
            Vector2 b = new Vector2((float) Math.Round(a.X), (float) Math.Round(a.Y));
            if(Allowed) {
                CanSelect.Draw(sb, b, (int) Layer.Selection);
            } else {
                CantSelect.Draw(sb, b, (int) Layer.Selection);
            }
        }

    }
}
