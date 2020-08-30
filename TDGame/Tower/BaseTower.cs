using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TDGame.Enemy;
using Microsoft.Xna.Framework.Audio;

namespace TDGame.Tower
{
    public class BaseTower
    {
        //Tower Details
        protected Texture2D sprite;
        public Vector2 Origin;
        protected Rectangle inTile;
        protected Rectangle myTile;
        public Color beamColour;
        public SoundEffect sound;

        public bool AA = false;

        protected float damage;
        protected float fireRate;
        protected int range;
        private float timeToFire = 0;
        public bool shooting = false;
        const float BEAMLIFE = 0.2f;
        float beamLifeTime = BEAMLIFE;

        public string targetType;
        public BaseEnemy target;
        public List<BaseEnemy> targetList;
        public List<BaseEnemy> groundEnemies;

        public BaseTower(Rectangle _rectangle)
        {
        }
        
        public float getDamage { get { return damage; } }
        public int getRange { get { return range; } }
        public float getFireRate { get { return fireRate; } }

        public virtual bool hasTarget()
        {
            if (target == null)
                return false;
            else
                return true;
        }

        public virtual void Fire()
        {
            target.takeDamage(damage);
            shooting = true;
            timeToFire = 0;
        }
        public virtual void Fire(int multiplier)
        {
            //just here to be overriden
        }

        public bool canFire(GameTime gameTime, int speedVar)
        {
            timeToFire += (float)gameTime.ElapsedGameTime.TotalSeconds * speedVar;
            return timeToFire >= fireRate;
        }

        protected void correctRect()
        {
            myTile = new Rectangle(inTile.X + 5, inTile.Y + 5, 40, 40);
        }

        public void Update(GameTime gameTime)
        {
            if (shooting)
            {
                //if (sound != null)
                //    sound.Play(0.02f,0,0);
                beamLifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (beamLifeTime <= 0)
            {
                shooting = false;
                beamLifeTime = BEAMLIFE;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, myTile , Color.White);
        }
    }
}
