using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace First.MainGame.GameObjects {
    public class Light : GameObject {

        public Color color;
        public int radius;
        public float intensity;

        public Light(Vector2 position, int? radius, float? intensity, Color? color)
            : base(position, new Sprite(Sprite.SpriteDictionary ["Black"]), Layer.Light) {
            this.color = color ?? Color.Red;
            this.radius = radius ?? 6;
            this.intensity = intensity ?? 1f;
        }



    }
}
