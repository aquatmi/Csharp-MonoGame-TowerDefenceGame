using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TDGame.Components
{
    public class MenuComponent
    {
        #region Fields
        SpriteFont spriteFont; // Menu Font
        readonly List<string> menuItems = new List<string>(); // What the menu shows
        int selectedIndex = -1; // Index of the menu item selected
        bool mouseOver; // Is the mouse over menu item
        bool soundPlayed = false;

        int width;  // Where the menu
        int height; // is rendered

        Color normalColour = Color.Black; // Colours of the buttons normally
        Color altColour = Color.White;    // And when mouseOver = true
        SoundEffect click;
        Rectangle currentButton;

        Texture2D texture; // Background image for buttons
        Vector2 position;  // Position of menu
        #endregion

        #region Properties
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = (int)MathHelper.Clamp( value, 0, menuItems.Count - 1); }
        }
        public Color NormalColor
        {
            get { return normalColour; }
            set { normalColour = value; }
        }
        public Color AltColor
        {
            get { return altColour; }
            set { altColour = value; }
        }
        public bool MouseOver { get { return mouseOver; } }
        #endregion

        #region Constructors
        public MenuComponent(SpriteFont spriteFont, Texture2D texture)
        {
            this.mouseOver = false;
            this.spriteFont = spriteFont;
            this.texture = texture;
        }
        public MenuComponent(SpriteFont spriteFont, Texture2D texture, string[] menuItems, SoundEffect click)
            :this(spriteFont, texture)
        {
            this.click = click;
            selectedIndex = 0;
            foreach(string s in menuItems)
            {
                this.menuItems.Add(s);
            }
            MeasureMenu();
        }
        #endregion

        #region Methods
        public void SetMenuItems(string[] items) // Change the items that are in the menu to a new set
        {
            menuItems.Clear();
            menuItems.AddRange(items);
            MeasureMenu();

            selectedIndex = 0;
        }
        private void MeasureMenu() // Calculate the width and height of the menu
        {
            width = texture.Width;
            height = 0;

            foreach(string s in menuItems)
            {
                Vector2 size = spriteFont.MeasureString(s);

                if (size.X > width)
                    width = (int)size.X;

                height += texture.Height + 50;
            }

            height -= 50;
        }
        public void Update(GameTime gameTime)                    // Check if the mouse is over a button
        {                                                        // And if necessary highlight that button
            Vector2 menuPosition = position;
            Point p = XInput.MouseState.Position;

            Rectangle buttonRect;
            mouseOver = false;
            selectedIndex = -1;

            for (int i = 0; i <menuItems.Count; i++)
            {
                buttonRect = new Rectangle((int)menuPosition.X, (int)menuPosition.Y, texture.Width, texture.Height);

                if (buttonRect.Contains(p))
                {
                    selectedIndex = i;
                    mouseOver = true;
                    if (!soundPlayed)
                    {
                        click.Play(0.5f, 0f, 0f);
                        soundPlayed = true;
                        currentButton = buttonRect;
                    }
                }
                if (!currentButton.Contains(p))
                {
                    soundPlayed = false;
                }
                menuPosition.Y += texture.Height + 50;
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) // Renders menu items
        {
            Vector2 menuPosition = position;
            Color myColor;

            for (int i = 0; i <menuItems.Count; i++)
            {
                if (i == SelectedIndex)
                    myColor = altColour;
                else
                    myColor = normalColour;

                spriteBatch.Draw(texture, menuPosition, Color.White);

                Vector2 textSize = spriteFont.MeasureString(menuItems[i]);

                Vector2 textPosition = menuPosition + new Vector2((int)(texture.Width - textSize.X) / 2, (int)(texture.Height - textSize.Y) / 2);

                spriteBatch.DrawString(spriteFont, menuItems[i], textPosition, myColor);

                menuPosition.Y += texture.Height + 50;
            }
        }
        #endregion
    }
}
