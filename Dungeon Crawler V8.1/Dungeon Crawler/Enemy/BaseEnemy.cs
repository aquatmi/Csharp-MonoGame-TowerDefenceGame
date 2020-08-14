using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler.Enemy
{
    public class BaseEnemy
    {
        #region Variables
        protected Texture2D sprite;

        public int nodeRef;
        bool alive, finished;

        protected Vector2 position;
        public Vector2 velocity;
        protected Vector2 origin;
        float rotation;

        //Stats
        public float health, speed, score;
        public bool isAir = false;
        #endregion

        #region Constructor
        public BaseEnemy(Vector2 _startNode)
        {
            nodeRef = 0;
            alive = true;
            finished = false;
            score = 0;
        }
        #endregion

        #region Get/Sets
        public bool isAlive { get { return alive; } }
        public bool setAlive { set { alive = value; } }
        public bool isFinished { get { return finished; } }
        public bool setFinished { set { finished = value; } }
        public Texture2D getSprite { get { return sprite; } }
        public Vector2 getPosition { get { return position; } }
        public Vector2 setPosition { set { position = value; } }
        public float getRotation { get { return rotation; } }
        public Vector2 getOrigin { get { return origin; } }
        #endregion

        #region Functions
        public void Update(GameTime gameTime, int speedVar)
        {
            if (health < 0)
                alive = false;
            moveMe(gameTime, speedVar);
        }

        public void takeDamage(float damage)
        {
            health -= damage;
        }
        public void takeDamage(float damage, int multiplier)
        {
            health -= (damage * multiplier);
        }

        public void moveMe(GameTime gameTime, int speedVar)
        {
            // Move the sprite by speed, scaled by elapsed time.
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * speedVar;
            origin += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * speedVar;

            // Score is used to calculate who is in the lead
            score += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            score += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void Align()
        {
            if (velocity.X > 1) // Right
                rotation = 0;
            else if (velocity.Y > 1) // Down
                rotation = (float)(Math.PI / 2);
            else if (velocity.X < -1) // Left
                rotation = (float)(Math.PI);
            else if (velocity.Y < -1) // Up
                rotation = (float)((3 * Math.PI) / 2);
        }
        #endregion
    }
}
