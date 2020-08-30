using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDGame.Enemy
{
    class FastEnemy : BaseEnemy
    {
        public FastEnemy(Texture2D _sprite, Vector2 _startNode, float healthModifier)
            : base(_startNode)
        {
            position = _startNode - new Vector2(_sprite.Width / 2, _sprite.Height / 2);
            origin = _startNode;
            sprite = _sprite;
            health = 10 * healthModifier;
            speed = 2;
        }
    }
}
