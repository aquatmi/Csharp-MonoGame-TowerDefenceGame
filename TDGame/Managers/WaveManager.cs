using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using TDGame.Components;

namespace TDGame.Managers
{
    public class WaveManager
    {
        GameManager gameManager;

        string[] waveLines = new string[25];
        List<Wave> Waves = new List<Wave>();
        public Wave CurrentWave = null;
        int waveNumber = 0;

        const int WAVEAMOUNT = 25;
        const int STALLTIME = 10;
        public float stallTime = 0;

        float spawnTimer;

        int amountLeftToSpawn;

        public WaveManager(GameManager _gameManager, string[] _waveLines)
        {
            gameManager = _gameManager;
            waveLines = _waveLines;
            CreateWaves();
            foreach (Wave w in Waves)
                System.Console.WriteLine(w.healthModifier);
        }
        
        void CreateWaves()
        {
            //									|-----        AMOUNT OF SPAWNED	       -----|
            //SPAWN RATE	:	HEALTH MODIFIER	:	NORMAL	:	FAST	:	STRONG	:	AIR	:
            //float         :   float           :   int     :   int     :   int     :   int :
            int waveNumber = 1;

            foreach (string line in waveLines)
            {
                Wave tempWave = new Wave();

                tempWave.waveNumber = waveNumber;
                string[] temp = line.Split(',');
                if (Waves.Count == 0)
                {
                    tempWave.spawnRate = 1.5f;
                    tempWave.healthModifier = 0.8f;
                }
                else
                {
                    if (Waves[waveNumber - 2].spawnRate == 0.2)
                        tempWave.spawnRate = 0.2f;
                    else
                        tempWave.spawnRate = Waves[waveNumber - 2].spawnRate - 0.1f;

                    if (Waves[waveNumber - 2].healthModifier == 2)
                        tempWave.healthModifier = 2f;
                    else
                        tempWave.healthModifier = Waves[waveNumber - 2].healthModifier + ((float)Math.Log(waveNumber,3) / 5);
                }
                tempWave.spawnAmountN = int.Parse(temp[0]);
                tempWave.spawnAmountF = int.Parse(temp[1]);
                tempWave.spawnAmountS = int.Parse(temp[2]);
                tempWave.spawnAmountA = int.Parse(temp[3]);

                waveNumber++;

                Waves.Add(tempWave);
            }
        }

        public bool waitingToStartWave(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            stallTime -= elapsed * gameManager.uiManager.speedMode;
            if (stallTime < 0)
            { 
                return false;
            }
            else
                return true;
        }

        public void StartWave()
        {
            stallTime = STALLTIME;
            if (waveNumber != 25)
            {
                CurrentWave = Waves[waveNumber];
                waveNumber++;
                amountLeftToSpawn = CurrentWave.spawnAmountA + CurrentWave.spawnAmountF + CurrentWave.spawnAmountN + CurrentWave.spawnAmountS;
            }
            else
            {
                gameManager.GameOver('v');
            }
        }

        public void Spawn(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            spawnTimer -= elapsed * gameManager.uiManager.speedMode;
            if (spawnTimer < 0 && amountLeftToSpawn > 0)
            {
                //Timer expired, execute action
                Random rand = new Random();
                switch (rand.Next(0, 4))
                {
                    case 0:
                        //spawn normal
                        if (CurrentWave.spawnAmountN > 0)
                        {
                            gameManager.enemyManager.CreateEnemy('n', CurrentWave.healthModifier);
                            spawnTimer = CurrentWave.spawnRate;   //Reset Timer
                            CurrentWave.spawnAmountN--;
                            amountLeftToSpawn--;
                        }
                        break;
                    case 1:
                        //spawn fast
                        if (CurrentWave.spawnAmountF > 0)
                        {
                            gameManager.enemyManager.CreateEnemy('f', CurrentWave.healthModifier);
                            spawnTimer = CurrentWave.spawnRate;   //Reset Timer
                            CurrentWave.spawnAmountF--;
                            amountLeftToSpawn--;
                        }
                        break;
                    case 2:
                        //spawn slow
                        if (CurrentWave.spawnAmountS > 0)
                        {
                            gameManager.enemyManager.CreateEnemy('s', CurrentWave.healthModifier);
                            spawnTimer = CurrentWave.spawnRate;   //Reset Timer
                            CurrentWave.spawnAmountS--;
                            amountLeftToSpawn--;
                        }
                        break;
                    case 3:
                        //spawn air
                        if (CurrentWave.spawnAmountA > 0)
                        {
                            gameManager.enemyManager.CreateEnemy('a', CurrentWave.healthModifier);
                            spawnTimer = CurrentWave.spawnRate;   //Reset Timer
                            CurrentWave.spawnAmountA--;
                            amountLeftToSpawn--;
                        }
                        break;
                }
            }
        }

        public bool AreEnemiesLeft() //Finds if there ARE enemies left to spawn
        {
            if (amountLeftToSpawn != 0) //enemies remain
                return true;
            else
            {
                if (gameManager.gameMoney + 50 < int.MaxValue)
                    gameManager.gameMoney += 50;
                return false;
            }
        }
    }
}

