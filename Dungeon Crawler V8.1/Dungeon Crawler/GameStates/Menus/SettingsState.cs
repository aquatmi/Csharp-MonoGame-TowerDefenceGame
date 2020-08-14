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
    public interface ISettingsState : IGameState
    {
        bool getSoundEnabled { get; }
    }

    public class SettingsState : BaseGameState, ISettingsState
    {
        #region Field Region
        Texture2D background;
        Texture2D mute;
        Texture2D unmute;
        SpriteFont spriteFont;
        MenuComponent menuComponent;
        public bool isSoundEnabled = true;
        public bool getSoundEnabled  { get { return isSoundEnabled; } }

#endregion

#region Property Region
#endregion

#region Constructor Region
public SettingsState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(ISettingsState), this);
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
            mute = Game.Content.Load<Texture2D>(@"Misc\Mute");
            unmute = Game.Content.Load<Texture2D>(@"Misc\Unmute");

            Texture2D texture = Game.Content.Load<Texture2D>(@"Misc\MenuButton");

            SoundEffect click = Game.Content.Load<SoundEffect>(@"Sound\UI\Click");

            string[] menuItems = { "VOLUME", "BACK" };

            menuComponent = new MenuComponent(spriteFont, texture, menuItems, click);

            Vector2 position = new Vector2();

            position.X = 1150 - menuComponent.Width;
            position.Y = 250;

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
                    if (isSoundEnabled)
                        isSoundEnabled = false;
                    else if (!isSoundEnabled)
                        isSoundEnabled = true;
                }
                else if (menuComponent.SelectedIndex == 1)
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
            if (isSoundEnabled)
                GameRef.SpriteBatch.Draw(unmute, new Vector2(970,270), Color.White);
            else
                GameRef.SpriteBatch.Draw(mute, new Vector2(970, 270), Color.White);
            GameRef.SpriteBatch.End();
        }

        #endregion
    }
}
