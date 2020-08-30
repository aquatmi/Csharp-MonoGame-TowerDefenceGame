using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using TDGame.Enemy;
using TDGame.Managers;
using Microsoft.Xna.Framework.Audio;

namespace TDGame.Managers
{
    public class EnemyManager
    {
        Texture2D fastEnemyTexture;
        Texture2D normalEnemyTexture;
        Texture2D slowEnemyTexture;
        Texture2D airEnemyTexture;

        Vector2 startNode;

        SoundEffect boom;

        List<Vector2> track = new List<Vector2>();
        List<BaseEnemy> enemyList = new List<BaseEnemy>();
        GameManager manager;

        List<BaseEnemy> doneList = new List<BaseEnemy>();

        public EnemyManager(GameManager _manager, List<Texture2D> _enemyTextures, SoundEffect _boom)
        {
            manager = _manager;
            track = _manager.mapManager.trackList;

            fastEnemyTexture = _enemyTextures[0];
            normalEnemyTexture = _enemyTextures[1];
            slowEnemyTexture = _enemyTextures[2];
            airEnemyTexture = _enemyTextures[3];

            startNode = manager.mapManager.getStartNode();

            boom = _boom;
        }

        public List<BaseEnemy> getEnemies { get { return enemyList; } }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (enemyList != null)
            {
                foreach (BaseEnemy e in enemyList)
                    spriteBatch.Draw(e.getSprite, e.getPosition, Color.White);
            }
        }


        public void Update(GameTime gameTime)
        {
            foreach (BaseEnemy e in enemyList)
            {
                calcSpeed(e);
                e.Align();
                e.Update(gameTime, manager.uiManager.speedMode);

                if (!e.isAlive)
                    Kill(e);
                else if (e.isFinished)
                    Finish(e);
            }

            foreach (BaseEnemy e in doneList)
            {
                enemyList.Remove(e);
            }
            doneList.Clear();
        }

        void calcSpeed(BaseEnemy e)
        {
            if (e.nodeRef + 1 == track.Count) //check to see if track is empty            
            {
                e.setFinished = true;
            }
            else if (isAtNode(e)) //not empty
            {
                e.score += 1200;
                float xDist = -(e.getOrigin.X - track[e.nodeRef + 1].X);
                float yDist = -(e.getOrigin.Y - track[e.nodeRef + 1].Y);

                e.velocity.X = xDist * e.speed;
                e.velocity.Y = yDist * e.speed;

                e.nodeRef++;
            }
        }
        void Kill(BaseEnemy e)
        {
            doneList.Add(e);
            if (manager.gameMoney + 2 < int.MaxValue)
                manager.gameMoney += 5;
            boom.Play(0.02f, 0, 0);
        }
        void Finish(BaseEnemy e)
        {
            doneList.Add(e);
            manager.gameHealth--;
            //Health Loss Sound
        }

        public void CreateEnemy(char type, float healthModifier)
        {
            switch (type)
            {
                case 'n':
                    enemyList.Add(new NormalEnemy(normalEnemyTexture, startNode, healthModifier));
                    break;
                case 'f':
                    enemyList.Add(new FastEnemy(fastEnemyTexture, startNode, healthModifier));
                    break;
                case 's':
                    enemyList.Add(new SlowEnemy(slowEnemyTexture, startNode, healthModifier));
                    break;
                case 'a':
                    enemyList.Add(new AirEnemy(airEnemyTexture, startNode, healthModifier));
                    break;
            }
            
        }

        bool isAtNode(BaseEnemy e)
        {
            Vector2 NodeRef = track[e.nodeRef];
            if ((e.getOrigin.X > NodeRef.X - 3 && e.getOrigin.X < NodeRef.X + 3) && (e.getOrigin.Y > NodeRef.Y - 3 && e.getOrigin.Y < NodeRef.Y + 3))
            {
                return true;
            }
            else
                return false;
        }
    }
}

