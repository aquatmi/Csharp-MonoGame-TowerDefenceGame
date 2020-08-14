using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Dungeon_Crawler.Components;

namespace Dungeon_Crawler.Managers
{
    public class UIManager
    {
        GameManager gameManager;
        
        Color highlightColour = Color.Lime;

        List<Texture2D> buttonTextures = new List<Texture2D>();
        //0 = BG, 1 = Pause, 2 = Play, 3 = Fast Forward
        //4 = BG, 5 = First, 6 = Last, 7 = Strong, 8 = Near
        //9 = BG, 10 = Laser, 11 = Burst, 12 = Rail

        List<Rectangle> speedButtons = new List<Rectangle>();
        List<Rectangle> targetButtons = new List<Rectangle>();
        List<Rectangle> towerButtons = new List<Rectangle>();

        SpriteFont font;

        public int speedMode = 1;
        public int targetMode = 0;
        float timeTilWaveStart;
        public UIManager(GameManager _gameManager, SpriteFont _font, List<Texture2D> _UITextures)
        {
            gameManager = _gameManager;
            font = _font;

            buttonTextures = _UITextures;

            speedButtons.Add(new Rectangle(20, 650, 70, 50));   //pause
            speedButtons.Add(new Rectangle(100, 650, 70, 50));  //play
            speedButtons.Add(new Rectangle(180, 650, 70, 50));  //fast forward

            targetButtons.Add(new Rectangle(20, 590, 50, 50));  //first
            targetButtons.Add(new Rectangle(80, 590, 50, 50));  //last
            targetButtons.Add(new Rectangle(140, 590, 50, 50)); //strong
            targetButtons.Add(new Rectangle(200, 590, 50, 50)); //near

            towerButtons.Add(new Rectangle(20, 140, 230, 140)); //laser
            towerButtons.Add(new Rectangle(20, 290, 230, 140)); //burst
            towerButtons.Add(new Rectangle(20, 440, 230, 140)); //rail
        }

        public void Update(GameTime gameTime)
        {
            switch (targetMode)
            {
                case 0:
                    gameManager.targetPriority = "FIRST";
                    break;
                case 1:
                    gameManager.targetPriority = "LAST";
                    break;
                case 2:
                    gameManager.targetPriority = "STRONG";
                    break;
                case 3:
                    gameManager.targetPriority = "NEAR";
                    break;
            }

            if (gameManager.waveManager.waitingToStartWave(gameTime))
            {
                timeTilWaveStart = gameManager.waveManager.stallTime;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //HEALTH
            spriteBatch.DrawString(font, "HEALTH: ", new Vector2(25, 20), Color.Black);
            spriteBatch.DrawString(font, gameManager.gameHealth.ToString(), new Vector2(200, 20), Color.Black);

            //MONEY
            spriteBatch.DrawString(font, "MONEY: ", new Vector2(25, 55), Color.Black);
            if (gameManager.gameMoney.ToString().Length == 3)
                spriteBatch.DrawString(font, gameManager.gameMoney.ToString(), new Vector2(181, 55), Color.Black);
            else
                spriteBatch.DrawString(font, gameManager.gameMoney.ToString(), new Vector2(166, 55), Color.Black);

            //WAVE
            spriteBatch.DrawString(font, "WAVE: ", new Vector2(25, 90), Color.Black);
            if (timeTilWaveStart > 0.5)
                spriteBatch.DrawString(font, ((int)timeTilWaveStart).ToString(), new Vector2(150, 90), Color.Black);
            else if (gameManager.waveManager.CurrentWave == null)
                spriteBatch.DrawString(font, "0 / 50", new Vector2(150, 90), Color.Black);
            else
                spriteBatch.DrawString(font, gameManager.waveManager.CurrentWave.waveNumber + " / 50", new Vector2(150, 90), Color.Black);


            //SPEED OPTIONS
            if (speedMode == 0)
                spriteBatch.Draw(buttonTextures[0], speedButtons[0], highlightColour);
            spriteBatch.Draw(buttonTextures[1], speedButtons[0], Color.White);

            if (speedMode == 1)
                spriteBatch.Draw(buttonTextures[0], speedButtons[1], highlightColour);
            spriteBatch.Draw(buttonTextures[2], speedButtons[1], Color.White);

            if (speedMode == 2)
                spriteBatch.Draw(buttonTextures[0], speedButtons[2], highlightColour);
            spriteBatch.Draw(buttonTextures[3], speedButtons[2], Color.White);

            //TARGET OPTIONS
            if (targetMode == 0)
                spriteBatch.Draw(buttonTextures[4], targetButtons[0], highlightColour);
            spriteBatch.Draw(buttonTextures[5], targetButtons[0], Color.White);

            if (targetMode == 1)
                spriteBatch.Draw(buttonTextures[4], targetButtons[1], highlightColour);
            spriteBatch.Draw(buttonTextures[6], targetButtons[1], Color.White);

            if (targetMode == 2)
                spriteBatch.Draw(buttonTextures[4], targetButtons[2], highlightColour);
            spriteBatch.Draw(buttonTextures[7], targetButtons[2], Color.White);

            if (targetMode == 3)
                spriteBatch.Draw(buttonTextures[4], targetButtons[3], highlightColour);
            spriteBatch.Draw(buttonTextures[8], targetButtons[3], Color.White);

            //TOWER BUTTONS
            if (gameManager.towerManager.towerType == "LASER")
                spriteBatch.Draw(buttonTextures[9], towerButtons[0], highlightColour);
            spriteBatch.Draw(buttonTextures[10], towerButtons[0], Color.White);

            if (gameManager.towerManager.towerType == "BURST")
                spriteBatch.Draw(buttonTextures[9], towerButtons[1], highlightColour);
            spriteBatch.Draw(buttonTextures[11], towerButtons[1], Color.White);

            if (gameManager.towerManager.towerType == "RAIL")
                spriteBatch.Draw(buttonTextures[9], towerButtons[2], highlightColour);
            spriteBatch.Draw(buttonTextures[12], towerButtons[2], Color.White);
        }

        void createTower(Point mousePoint)
        {
            if (gameManager.mapManager.checkTowers(mousePoint))
            {
                gameManager.towerManager.addTower(gameManager.mapManager.tempTower);
            }
        }

        void updateSpeed(Point mousePoint)
        {
            for (int i = 0; i < speedButtons.Count; i++)
            {
                if (speedButtons[i].Contains(mousePoint))
                {
                    speedMode = i;
                }
            }
        }
        void updateTarget(Point mousePoint)
        {
            for (int i = 0; i < targetButtons.Count; i++)
            {
                if (targetButtons[i].Contains(mousePoint))
                {
                    targetMode = i;
                }
            }
        }

        void updateTower(Point mousePoint)
        {
            int tempTower = -1;
            for (int i = 0; i < towerButtons.Count; i++)
            {
                if (towerButtons[i].Contains(mousePoint))
                {
                    tempTower = i;
                }
            }
            if (tempTower == 0) //LASER
            {
                if (!gameManager.towerManager.buildMode)
                {
                    gameManager.towerManager.buildMode = true;
                    gameManager.towerManager.towerType = "LASER";
                }
                else if (gameManager.towerManager.towerType == "LASER")
                {
                    gameManager.towerManager.buildMode = false;
                    gameManager.towerManager.towerType = null;
                }
                else
                    gameManager.towerManager.towerType = "LASER";
            }
            else if (tempTower == 1) // BURST
            {
                if (!gameManager.towerManager.buildMode)
                {
                    gameManager.towerManager.buildMode = true;
                    gameManager.towerManager.towerType = "BURST";
                }
                else if (gameManager.towerManager.towerType == "BURST")
                {
                    gameManager.towerManager.buildMode = false;
                    gameManager.towerManager.towerType = null;
                }
                else
                    gameManager.towerManager.towerType = "BURST";
            }
            else if (tempTower == 2) //RAIL
            {
                if (!gameManager.towerManager.buildMode)
                {
                    gameManager.towerManager.buildMode = true;
                    gameManager.towerManager.towerType = "RAIL";
                }
                else if (gameManager.towerManager.towerType == "RAIL")
                {
                    gameManager.towerManager.buildMode = false;
                    gameManager.towerManager.towerType = null;
                }
                else
                    gameManager.towerManager.towerType = "RAIL";
            }
        }

        public void KeyBoardInput()
        {
            //BUILD MODE
            if (!gameManager.towerManager.buildMode)
            {
                if (XInput.CheckKeyReleased(Keys.L))
                {
                    gameManager.towerManager.buildMode = true;
                    gameManager.towerManager.towerType = "LASER";
                }
                else if (XInput.CheckKeyReleased(Keys.B))
                {
                    gameManager.towerManager.buildMode = true;
                    gameManager.towerManager.towerType = "BURST";
                }
                else if (XInput.CheckKeyReleased(Keys.R))
                {
                    gameManager.towerManager.buildMode = true;
                    gameManager.towerManager.towerType = "RAIL";
                }
            }
            else
            {
                if (gameManager.towerManager.towerType == "LASER")
                {
                    if (XInput.CheckKeyReleased(Keys.L))
                    {
                        gameManager.towerManager.buildMode = false;
                        gameManager.towerManager.towerType = null;
                    }
                    else if (XInput.CheckKeyReleased(Keys.B))
                        gameManager.towerManager.towerType = "BURST";
                    else if (XInput.CheckKeyReleased(Keys.R))
                        gameManager.towerManager.towerType = "RAIL";
                }
                else if (gameManager.towerManager.towerType == "BURST")
                {
                    if (XInput.CheckKeyReleased(Keys.B))
                    {
                        gameManager.towerManager.buildMode = false;
                        gameManager.towerManager.towerType = null;
                    }
                    else if (XInput.CheckKeyReleased(Keys.L))
                        gameManager.towerManager.towerType = "LASER";
                    else if (XInput.CheckKeyReleased(Keys.R))
                        gameManager.towerManager.towerType = "RAIL";
                }
                else if (gameManager.towerManager.towerType == "RAIL")
                {
                    if (XInput.CheckKeyReleased(Keys.R))
                    {
                        gameManager.towerManager.buildMode = false;
                        gameManager.towerManager.towerType = null;
                    }
                    else if (XInput.CheckKeyReleased(Keys.L))
                        gameManager.towerManager.towerType = "LASER";
                    else if (XInput.CheckKeyReleased(Keys.B))
                        gameManager.towerManager.towerType = "BURST";
                }
            }

            //TARGET MODE
            if (XInput.CheckKeyReleased(Keys.F))
            {
                gameManager.targetPriority = "FIRST";
                targetMode = 0;
            }
            else if (XInput.CheckKeyReleased(Keys.A))
            {
                gameManager.targetPriority = "LAST";
                targetMode = 1;
            }
            else if (XInput.CheckKeyReleased(Keys.S))
            {
                gameManager.targetPriority = "STRONG";
                targetMode = 2;
            }
            else if (XInput.CheckKeyReleased(Keys.N))
            {
                gameManager.targetPriority = "NEAR";
                targetMode = 3;
            }

            //SPEED MODE
            if (XInput.CheckKeyReleased(Keys.Space))
            {
                if (speedMode == 0)
                    speedMode = 1;
                else
                    speedMode = 0;
            }

            //CHEAT CODES
            if (XInput.CheckKeyHeld(Keys.LeftShift) && XInput.CheckKeyHeld(Keys.LeftControl) && XInput.CheckKeyHeld(Keys.H))
            {
                gameManager.gameHealth = 100000000;
            }
            if (XInput.CheckKeyHeld(Keys.LeftShift) && XInput.CheckKeyHeld(Keys.LeftControl) && XInput.CheckKeyHeld(Keys.M))
            {
                gameManager.gameMoney = 100000000;
            }
            if (XInput.CheckKeyHeld(Keys.LeftShift) && XInput.CheckKeyHeld(Keys.LeftControl) && XInput.CheckKeyHeld(Keys.S))
            {
                speedMode = 10;
            }

        }
        public void MouseInput(MouseButtons button, Point mousePos)
        {
            if (gameManager.towerManager.buildMode)
                createTower(mousePos);

            updateSpeed(mousePos);
            updateTarget(mousePos);
            updateTower(mousePos);

        }





    }
}
