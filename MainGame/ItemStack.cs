using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using First.MainGame;
using Newtonsoft.Json;

namespace First.MainGame {
    public class ItemStack {
        [JsonProperty(PropertyName = "c")]
        public int count;
        [JsonProperty(PropertyName = "t")]
        public ItemType type;

        public ItemStack(ItemType type) {
            this.type = type;
        }

        public void addItem(ItemType type) {
            if(this.type != type) {
                this.type = type;
                count++;
                return;
            }
            count++;
        }


    }
}
