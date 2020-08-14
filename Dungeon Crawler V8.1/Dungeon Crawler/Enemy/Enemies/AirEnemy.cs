using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler.Enemy
{
    class AirEnemy : BaseEnemy
    {
        public AirEnemy(Texture2D _sprite, Vector2 _startNode, float healthModifier)
            : base(_startNode)
        {
            position = _startNode - new Vector2(_sprite.Width / 2, _sprite.Height / 2);
            origin = _startNode;
            sprite = _sprite;
            health = 20 * healthModifier;
            speed = 1;
            isAir = true;
        }
    }
}
