using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First.MainGame {
    public class Inventory {

        int width = 1;
        int height = 1;

        ItemStack[,] contents;

        public Inventory(int width, int height) {
            this.width = width;
            this.height = height;
            contents = new ItemStack [width, height];

            for(int x = 0; x < width; x++) {
                for(int y = 0; y < height; y++) {
                    contents [x, y] = new ItemStack(ItemType.Air);
                }
            }
        }

        public void addItem(Item item) {
            //TODO: add item management
        }



    }
}
