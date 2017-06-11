using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using First.MainGame.GameObjects;

namespace First.MainGame {
    public class Camera {

        static public Matrix             _transform; // Matrix Transform
        static public Vector2          _pos= Vector2.Zero; // Camera Position
        static protected float         _rotation = 0.0f; // Camera Rotation
        static public GameObject target = Player.getPlayer();
        static public Matrix matrix;

        static public float _zoom = 2f; // Camera Zoom
        static public float zoomtarget = 2f;
        static public float zoomspeed = .7f;


        static public void Update() {
            _zoom -= (_zoom - zoomtarget) * Time.deltaTime * zoomspeed;
            _pos += ((Player.getPlayer().position + new Vector2(.5f)) - (_pos / World.TileSize)) * Time.deltaTime * ((6 / 2) * World.TileSize * _zoom);
        }

        // Get set position
        static public Vector2 Pos {
            get { return _pos; }
            set { _pos = value; }
        }

        static public Matrix get_transformation(GraphicsDevice graphicsDevice) {
            _transform =
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(_rotation) *
                                         Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(GraphicalSettings.screensize.X * 0.5f, GraphicalSettings.screensize.Y * 0.5f, 0));
            return _transform;
        }
    }
}
