using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TDGame.StateManager;
using TDGame.Components;
using TDGame.Managers;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace TDGame.GameStates
{
    public interface IPlayState : IGameState
    {
    }

    public class PlayState : BaseGameState, IPlayState
    {
        #region Field Region
        Texture2D background;
        SpriteFont interfaceFont;
        SpriteFont gameFont;
        List<Texture2D> UITextures = new List<Texture2D>();
        List<Texture2D> mapTextures = new List<Texture2D>();
        List<Texture2D> towerTextures = new List<Texture2D>();
        List<SoundEffect> gameSounds = new List<SoundEffect>();
        List<Texture2D> enemyTextures = new List<Texture2D>();
        string[] waveLines = new string[25];
        bool isSoundEnabled;

        GameManager gameManager;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region
        public PlayState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IPlayState), this);
        }
        #endregion

        #region Method Region
        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            interfaceFont = Game.Content.Load<SpriteFont>(@"Font\Interface");
            gameFont = Game.Content.Load<SpriteFont>(@"Font\Game");

            background = Game.Content.Load<Texture2D>(@"GameScreen\Background");

            UITextures.Add(content.Load<Texture2D>(@"Misc\SpeedButtonBG"));
            UITextures.Add(content.Load<Texture2D>(@"Misc\PauseButton"));
            UITextures.Add(content.Load<Texture2D>(@"Misc\PlayButton"));
            UITextures.Add(content.Load<Texture2D>(@"Misc\FastButton"));

            UITextures.Add(content.Load<Texture2D>(@"Misc\TargetButtonBG"));
            UITextures.Add(content.Load<Texture2D>(@"Misc\FirstButton"));
            UITextures.Add(content.Load<Texture2D>(@"Misc\LastButton"));
            UITextures.Add(content.Load<Texture2D>(@"Misc\StrongButton"));
            UITextures.Add(content.Load<Texture2D>(@"Misc\NearButton"));

            UITextures.Add(content.Load<Texture2D>(@"Misc\TowerButtonBG"));
            UITextures.Add(content.Load<Texture2D>(@"Misc\LaserButton"));
            UITextures.Add(content.Load<Texture2D>(@"Misc\BurstButton"));
            UITextures.Add(content.Load<Texture2D>(@"Misc\RailButton"));

            Console.WriteLine("This works fine up to here");

            switch (GameRef.StartChooseMapState.Difficulty)
            {
                case "EASY":
                    mapTextures.Add(content.Load<Texture2D>(@"Map\Easy"));      //Easy map
                    ReadLines("EASY");
                    break;
                case "MEDIUM":
                    mapTextures.Add(content.Load<Texture2D>(@"Map\Medium"));  //Medium map
                    ReadLines("MEDIUM");
                    break;
                case "HARD":
                    mapTextures.Add(content.Load<Texture2D>(@"Map\Hard"));    //Hard map
                    ReadLines("HARD");
                    break;
            }
            mapTextures.Add(content.Load<Texture2D>(@"Tile\Path"));     //Path Texture
            mapTextures.Add(content.Load<Texture2D>(@"Tile\Terrain"));  //Terrain Texture
            
            towerTextures.Add(content.Load<Texture2D>(@"Tower\Beam"));  //Laser Beam Texture
            towerTextures.Add(content.Load<Texture2D>(@"Tower\Laser")); //Laser Tower Texture
            towerTextures.Add(content.Load<Texture2D>(@"Tower\Burst")); //Burst Tower Texture
            towerTextures.Add(content.Load<Texture2D>(@"Tower\Rail"));  //Rail Tower Texture
            
            enemyTextures.Add(content.Load<Texture2D>(@"Enemy\Fast"));  //Fast Enemy Texture
            enemyTextures.Add(content.Load<Texture2D>(@"Enemy\Normal"));//Normal Enemy Texture
            enemyTextures.Add(content.Load<Texture2D>(@"Enemy\Slow"));  //Slow Enemy Texture
            enemyTextures.Add(content.Load<Texture2D>(@"Enemy\Air"));   //Air Enemy Texture

            gameSounds.Add(content.Load<SoundEffect>(@"Sound\UI\Click"));
            gameSounds.Add(content.Load<SoundEffect>(@"Sound\Tower\Laser"));
            gameSounds.Add(content.Load<SoundEffect>(@"Sound\Tower\Burst"));
            gameSounds.Add(content.Load<SoundEffect>(@"Sound\Tower\Rail"));
            gameSounds.Add(content.Load<SoundEffect>(@"Sound\UI\Boom"));

            gameManager = new GameManager(gameFont, UITextures, mapTextures, towerTextures, enemyTextures, gameSounds, GameRef, waveLines);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            gameManager.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.GraphicsDevice.Clear(Color.Black);
            GameRef.SpriteBatch.Begin();
            GameRef.SpriteBatch.Draw(background, Vector2.Zero, Color.White);
            gameManager.Draw(GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();

            base.Draw(gameTime);

        }

        void ReadLines(string level)
        {
            switch (level)
            {
                case "EASY":
                    using (var reader = new StreamReader(@"Waves\Easy.txt"))
                    {
                        for (int i = 0; i < 25; i++){
                            waveLines[i] = reader.ReadLine();
                        }
                    }
                    break;
                case "MEDIUM":  
                    using (var reader = new StreamReader(@"Waves\Medium.txt"))
                    {
                        for (int i = 0; i < 25; i++)
                        {
                            waveLines[i] = reader.ReadLine();
                        }
                    }
                    break;
                case "HARD":
                    using (var reader = new StreamReader(@"Waves\Hard.txt"))
                    {
                        for (int i = 0; i < 25; i++)
                        {
                            waveLines[i] = reader.ReadLine();
                        }
                    }
                    break;
            }
        }
        #endregion
    }
}
