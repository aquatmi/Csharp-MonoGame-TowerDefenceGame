using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TDGame.Components;
using TDGame.StateManager;
using TDGame.GameStates;


namespace TDGame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameStateManager gameStateManager;
        ITitleIntroState titleIntroState;
        IMainMenuState startMenuState;
        ISettingsState startSettingsState;
        IChooseMapState startChooseMapState;
        IPlayState startPlayState;

        static Rectangle screenRectangle;

        public SpriteBatch SpriteBatch { get { return spriteBatch; } }
        public static Rectangle ScreenRectangle { get { return screenRectangle; } }
        public GameStateManager GameStateManager { get { return gameStateManager; } }
        public ITitleIntroState TitleIntroState { get { return titleIntroState; } }
        public IMainMenuState StartMenuState { get { return startMenuState; } }
        public ISettingsState StartSettingsState { get { return startSettingsState; } }
        public IChooseMapState StartChooseMapState { get { return startChooseMapState; } }
        public IPlayState StartPlayState { get { return startPlayState; } }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            screenRectangle = new Rectangle(0, 0, 1280, 720);
            graphics.PreferredBackBufferWidth = ScreenRectangle.Width;
            graphics.PreferredBackBufferHeight = ScreenRectangle.Height;
            gameStateManager = new GameStateManager(this);
            Components.Add(gameStateManager);
            this.IsMouseVisible = true;

            titleIntroState = new TitleIntroState(this);
            startMenuState = new MainMenuState(this);
            startSettingsState = new SettingsState(this);
            startChooseMapState = new ChooseMapState(this);
            startPlayState = new PlayState(this);

            gameStateManager.ChangeState((TitleIntroState)titleIntroState);
        }
        protected override void Initialize()
        {
            Components.Add(new XInput(this));
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
