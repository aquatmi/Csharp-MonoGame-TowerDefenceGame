using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using TDGame.Components;
using TDGame.Tower;
using TDGame.Enemy;
using Microsoft.Xna.Framework.Audio;

namespace TDGame.Managers
{
    public class TowerManager
    {
        GameManager gameManager;
        //Variables

        List<BaseTower> towerList;
        List<Rectangle> towerRects;

        Texture2D beamTexture;
        Texture2D laserTowerTexture;
        Texture2D burstTowerTexture;
        Texture2D railTowerTexture;

        public bool buildMode;
        public string towerType;

        int laserTowerCost = 100;
        int burstTowerCost = 250;
        int railTowerCost = 250;

        List<SoundEffect> towerSounds = new List<SoundEffect>();

        public TowerManager(GameManager _manager, List<Texture2D> _towerTextures, List<SoundEffect> _towerSounds)
        {
            gameManager = _manager;
            towerList = new List<BaseTower>();
            towerRects = new List<Rectangle>();

            towerSounds = _towerSounds;
            towerSounds.RemoveAt(0);

            beamTexture = _towerTextures[0];
            laserTowerTexture = _towerTextures[1];
            burstTowerTexture = _towerTextures[2];
            railTowerTexture = _towerTextures[3];
        }

        //Get/Sets

        //Functions
        public void Update(GameTime gameTime)
        {
            foreach (BaseTower tower in towerList)
            {
                tower.Update(gameTime);
                if (!tower.shooting)
                    checkRange(tower);
                if (tower.canFire(gameTime, gameManager.uiManager.speedMode) && tower.hasTarget())
                {
                    if (tower is BurstTower)
                        tower.Fire(gameManager.uiManager.speedMode);
                    else
                        tower.Fire();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BaseTower t in towerList)
            {
                t.Draw(spriteBatch);
                if (t.shooting)
                {
                    DrawBeam(spriteBatch, t);
                    //t.shooting = false;
                }
            }
        }

        void DrawBeam(SpriteBatch spriteBatch, BaseTower tower)
        {
            if (tower.hasTarget())
            {
                if (tower.targetType == "MULTI")
                {
                    foreach (BaseEnemy e in tower.groundEnemies)
                    {
                        //System.Console.WriteLine("Draw line");
                        spriteBatch.DrawLine(beamTexture, tower.Origin, e.getOrigin, tower.beamColour);
                    }
                }
                else if (tower.targetType == "SINGLE")
                {
                    //System.Console.WriteLine("Draw line");
                    if (tower is LaserTower)
                        spriteBatch.DrawLine(beamTexture, tower.Origin, tower.target.getOrigin, tower.beamColour);
                    else if (tower is RailTower)
                        spriteBatch.DrawLine(beamTexture, tower.Origin, tower.target.getOrigin, tower.beamColour);
                }
            }
        }
        public void addTower(Rectangle rectangle)
        {
            switch (towerType)
            {
                case "LASER":
                    if (gameManager.gameMoney >= laserTowerCost)
                    {
                        if (gameManager.isSoundEnabled)
                            towerList.Add(new LaserTower(laserTowerTexture, towerSounds[0], rectangle));
                        else
                            towerList.Add(new LaserTower(laserTowerTexture, null, rectangle));
                        gameManager.mapManager.addTower();
                        gameManager.gameMoney -= laserTowerCost;
                    }
                    break;
                case "BURST":
                    if (gameManager.gameMoney >= burstTowerCost)
                    {
                        if (gameManager.isSoundEnabled)
                            towerList.Add(new BurstTower(burstTowerTexture, towerSounds[1], rectangle));
                        else
                            towerList.Add(new BurstTower(burstTowerTexture, null, rectangle));
                        gameManager.mapManager.addTower();
                        gameManager.gameMoney -= burstTowerCost;
                    }
                    break;
                case "RAIL":
                    if (gameManager.gameMoney >= railTowerCost)
                    {
                        if (gameManager.isSoundEnabled)
                            towerList.Add(new RailTower(railTowerTexture, towerSounds[2], rectangle));
                        else
                            towerList.Add(new RailTower(railTowerTexture, null, rectangle));
                        gameManager.mapManager.addTower();
                        gameManager.gameMoney -= railTowerCost;
                    }
                    break;
                default:
                    System.Console.WriteLine("Invalid Tower");
                    break;
            }
            buildMode = false;
            towerType = null;
        }

        public void checkRange(BaseTower t)
        {
            string priority = gameManager.targetPriority;
            switch (t.targetType)
            {
                case "MULTI":       //Attacks all enemys in range
                    t.targetList.Clear();
                    t.targetList = findEnemiesInRange(t);
                    break;
                case "SINGLE":
                    t.target = findTarget(t);
                    break;

            }

            float getDistance(Vector2 _towerPosition, Vector2 _enemyPosition)
            {
                float distance = Vector2.Distance(_towerPosition, _enemyPosition);
                return distance;
            }

            BaseEnemy findTarget(BaseTower tower)
            {
                List<BaseEnemy> allEnemiesInRange = findEnemiesInRange(tower);
                List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
                foreach (BaseEnemy e in allEnemiesInRange)
                {
                    if (e.isAir && !tower.AA)
                        continue;
                    else
                        enemiesInRange.Add(e);
                }
                //System.Console.WriteLine(enemiesInRange.Count);
                BaseEnemy currentTarget = null;

                switch (priority)
                {
                    case "NEAR":
                        float testdistance = float.MaxValue;
                        foreach (BaseEnemy e in enemiesInRange)
                        {
                            if (getDistance(tower.Origin, e.getOrigin) < testdistance)
                            {
                                testdistance = getDistance(tower.Origin, e.getOrigin);
                                currentTarget = e;
                            }
                        }
                        return currentTarget;
                    case "STRONG":
                        float testhealth = 0;
                        foreach (BaseEnemy e in enemiesInRange)
                        {
                            if (e.health > testhealth)
                            {
                                testhealth = e.health;
                                currentTarget = e;
                            }
                        }
                        return currentTarget;
                    case "FIRST":
                        float testScoreFirst = 0;
                        foreach (BaseEnemy e in enemiesInRange)
                        {
                            if (e.score > testScoreFirst)
                            {
                                testScoreFirst = e.score;
                                currentTarget = e;
                            }
                        }
                        return currentTarget;
                    case "LAST":
                        float testScoreLast = float.MaxValue;
                        foreach (BaseEnemy e in enemiesInRange)
                        {
                            if (e.score < testScoreLast)
                            {
                                testScoreLast = e.score;
                                currentTarget = e;
                            }
                        }
                        return currentTarget;
                }
                return null;
            }

            List<BaseEnemy> findEnemiesInRange(BaseTower tower)
            {
                List<BaseEnemy> allEnemies = gameManager.enemyManager.getEnemies;
                List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();

                foreach (BaseEnemy e in allEnemies)
                {
                    if (getDistance(tower.Origin, e.getOrigin) <= tower.getRange)
                    {
                        enemiesInRange.Add(e);
                    }
                }
                //System.Console.WriteLine(enemiesInRange.Count);
                return enemiesInRange;
            }
        }
        



    }
}

