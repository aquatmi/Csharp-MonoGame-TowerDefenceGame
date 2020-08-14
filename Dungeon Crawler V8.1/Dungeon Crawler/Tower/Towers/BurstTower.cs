using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dungeon_Crawler.Enemy;
using Microsoft.Xna.Framework.Audio;

namespace Dungeon_Crawler.Tower
{
    public class BurstTower : BaseTower
    {
        public BurstTower(Texture2D _sprite, SoundEffect _sound, Rectangle _rectangle)
            : base(_rectangle)
        {
            groundEnemies = new List<BaseEnemy>();

            beamColour = new Color(125, 0, 250, 255);

            sprite = _sprite;
            sound = _sound;
            inTile = _rectangle;
            correctRect();
            Origin = new Vector2(myTile.X + (sprite.Width / 2), myTile.Y + (sprite.Height / 2));

            range = 80;
            fireRate = 0.1f;
            damage = 0.025f;

            targetType = "MULTI";
            targetList = new List<BaseEnemy>();
        }

        public override bool hasTarget()
        {
            if (targetList.Count == 0)
            {
                //System.Console.WriteLine("Has no Target");
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void Fire(int multiplier)
        {
            groundEnemies.Clear();
            foreach (BaseEnemy e in targetList)
            {
                if (e.isAir)
                    continue;
                else
                    groundEnemies.Add(e);
            }

            foreach (BaseEnemy target in groundEnemies)
                target.takeDamage(damage, multiplier);
            shooting = true;
        }
    }
}
