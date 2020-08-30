using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TDGame.Tower
{
    class RailTower : BaseTower
    {
        public RailTower(Texture2D _sprite, SoundEffect _sound, Rectangle _rectangle)
            : base(_rectangle)
        {
            beamColour = new Color(251, 144, 0, 255);

            sprite = _sprite;
            sound = _sound;
            inTile = _rectangle;
            correctRect();
            Origin = new Vector2(myTile.X + (sprite.Width / 2), myTile.Y + (sprite.Height / 2));

            range = 200;
            fireRate = 1f;
            damage = 8;

            targetType = "SINGLE";
            AA = true;
        }
    }
}
