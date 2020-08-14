using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Dungeon_Crawler.StateManager;
using Dungeon_Crawler.Components;
using Microsoft.Xna.Framework.Audio;

namespace Dungeon_Crawler.GameStates
{
    public interface IChooseMapState : IGameState
    {
        string Difficulty { get; }
    }

    public class ChooseMapState : BaseGameState, IChooseMapState
    {
        #region Field Region
        Texture2D background;
        SpriteFont spriteFont;
        MenuComponent menuComponent;
        string difficulty;
        #endregion

        #region Property Region
        public string Difficulty
        {
            get { return difficulty; }
        }
        #endregion

        #region Constructor Region
        public ChooseMapState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IChooseMapState), this);
        }
        #endregion

        #region Method Region
        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteFont = Game.Content.Load<SpriteFont>(@"Font\Interface");
            background = Game.Content.Load<Texture2D>(@"GameScreen\Menu");

            Texture2D texture = Game.Content.Load<Texture2D>(@"Misc\MenuButton");
            SoundEffect click = Game.Content.Load<SoundEffect>(@"Sound\UI\Click");

            string[] menuItems = { "EASY", "MEDIUM", "HARD", "BACK"};

            menuComponent = new MenuComponent(spriteFont, texture, menuItems, click);

            Vector2 position = new Vector2();

            position.X = 1150 - menuComponent.Width;
            position.Y = 140;

            menuComponent.Position = position;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            menuComponent.Update(gameTime);

            if (XInput.CheckKeyReleased(Keys.Space) || XInput.CheckKeyReleased(Keys.Enter) ||
                (menuComponent.MouseOver && XInput.CheckMouseReleased(MouseButtons.Left)))
            {
                if (menuComponent.SelectedIndex == 0)
                {
                    difficulty = "EASY";
                    manager.ChangeState((PlayState)GameRef.StartPlayState);
                }
                else if (menuComponent.SelectedIndex == 1)
                {
                    difficulty = "MEDIUM";
                    manager.ChangeState((PlayState)GameRef.StartPlayState);
                }
                else if (menuComponent.SelectedIndex == 2)
                {
                    difficulty = "HARD";
                    manager.ChangeState((PlayState)GameRef.StartPlayState);
                }
                else if (menuComponent.SelectedIndex == 3)
                {
                    manager.ChangeState((MainMenuState)GameRef.StartMenuState);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();
            GameRef.SpriteBatch.Draw(background, Vector2.Zero, Color.White);
            GameRef.SpriteBatch.End();

            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin();
            menuComponent.Draw(gameTime, GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
        }

        #endregion
    }
}
