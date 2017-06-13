using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using First.MainGame;
using Microsoft.Xna.Framework;

namespace First.MainGame {
    public class Item : GameObject {

        public static Dictionary<ItemType,Sprite> ItemDictionary = new Dictionary<ItemType, Sprite>();

        public static void AddItem(ItemType it, Sprite sprite) {
            ItemDictionary.Add(it, sprite);
        }

        public ItemType type;

        public Item(ItemType type) : base(Vector2.Zero) {
            this.type = type;
            sprite = Item.ItemDictionary [type];
        }

    }
}
