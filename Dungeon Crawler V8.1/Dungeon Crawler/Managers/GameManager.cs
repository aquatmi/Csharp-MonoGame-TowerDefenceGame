using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using Dungeon_Crawler.StateManager;
using Dungeon_Crawler.Components;
using Dungeon_Crawler.GameStates;
using Dungeon_Crawler.Enemy;
using Dungeon_Crawler.Tower;
using Dungeon_Crawler.Managers;

namespace Dungeon_Crawler.Managers
{
    public class GameManager
    {
        #region variables
        public MapManager mapManager;
        public WaveManager waveManager;
        public TowerManager towerManager;
        public EnemyManager enemyManager;
        public UIManager uiManager;

        public string targetPriority;

        public int gameHealth = 25;
        public int gameMoney = 50;
        public bool isSoundEnabled;

        List<BaseEnemy> enemies = new List<BaseEnemy>();
        List<BaseTower> towers = new List<BaseTower>();
        List<BaseEnemy> doneList = new List<BaseEnemy>();

        List<SoundEffect> gameSounds = new List<SoundEffect>();

        List<BaseEnemy> target = new List<BaseEnemy>();

        List<Texture2D> UITextures = new List<Texture2D>();

        Game1 GameRef;
        #endregion
        public GameManager(SpriteFont _gameFont, List<Texture2D> _UITextures, List<Texture2D> _mapTextures, List<Texture2D> _towerTextures, List<Texture2D> _enemyTextures, List<SoundEffect> _gameSounds, Game1 _GameRef, string[] _waveLines)
        {
            gameSounds = _gameSounds;
            isSoundEnabled = _GameRef.StartSettingsState.getSoundEnabled;
            SoundEffect boom = _gameSounds[4];
            GameRef = _GameRef;
            uiManager = new UIManager(this, _gameFont, _UITextures);

            mapManager = new MapManager(this, _mapTextures);
            mapManager.Run();

            waveManager = new WaveManager(this, _waveLines);
            
            towerManager = new TowerManager(this, _towerTextures, gameSounds);
            enemyManager = new EnemyManager(this, _enemyTextures, boom);

            targetPriority = "FIRST";
        }

        #region Get Sets
        
        #endregion
        
        public void Update(GameTime gameTime)
        {
            uiManager.KeyBoardInput();

            //if tower is being built, check mouse input
            if (XInput.CheckMouseReleased(MouseButtons.Left))
            {
                Point mousePoint = XInput.MouseState.Position;
                uiManager.MouseInput(MouseButtons.Left, mousePoint);
            }
            uiManager.Update(gameTime);
            towerManager.Update(gameTime);
            enemyManager.Update(gameTime);

            if (!waveManager.waitingToStartWave(gameTime)) //if waiting time between waves is over
            {
                if (!waveManager.AreEnemiesLeft())         //and there arent any more enemies left to spawn this wave
                    waveManager.StartWave();               //start next wave
                else
                {
                    waveManager.Spawn(gameTime);
                }
            }
            
            if(gameHealth <= 0)
            {
                GameOver('l');
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //mapManager.Draw(spriteBatch);
            enemyManager.Draw(spriteBatch);
            towerManager.Draw(spriteBatch);
            uiManager.Draw(spriteBatch);
        }

        public void GameOver(char victory)
        {
            switch (victory)
            {
                case 'v':       //Victory
                    //Change state to victoy screen state with button to go back to main menu
                    break;
                case 'l':       //Loss
                    //change state to loss screen state with button to go back to main menu
                    break;
            }
        }


    }
}
