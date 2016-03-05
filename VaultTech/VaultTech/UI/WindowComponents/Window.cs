/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 7/June/2014
 * Date Moddified :- 12/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VaultTech.UI.WindowComponents
{
    public class Window : Component
    {
        #region Private Variables
        
        bool MoveWindow;
        Vector2 PreviousWindowPosition, PreviousWindowTitleBarPosition;
        #endregion
        #region Public properties
        public new Vector2 Position
        {
            get { return base.Position; }
            set { base.Position = value; }
        }

        public string StyleFilePath, WindowNodeNameInXml, WindowTitleBarNodeNameInXml;
        public WindowTitleBar windowTitleBar;
        public bool Focused;
        public int WindowIndex;
        #endregion

        public Window()
        {
            windowTitleBar = new WindowTitleBar();
        }

        public virtual new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);

            windowTitleBar.Initialize(Graphics);
        }

        public virtual new void LoadContent(string StyleFilePath, string WindowNodeNameInXml, string WindowTitleBarNodeNameInXml)
        {
            this.StyleFilePath = StyleFilePath;
            this.WindowNodeNameInXml = WindowNodeNameInXml;
            this.WindowTitleBarNodeNameInXml = WindowTitleBarNodeNameInXml;

            base.LoadContent(StyleFilePath, WindowNodeNameInXml);

            windowTitleBar.LoadContent(StyleFilePath, WindowNodeNameInXml + "/" + WindowTitleBarNodeNameInXml);
        }

        public virtual new void UpdateOnce(SpriteBatch spriteBatch)
        {
            windowTitleBar.Size = new Vector2(Size.X, windowTitleBar.Size.Y);
            windowTitleBar.Position = new Vector2(Position.X, Position.Y);
            windowTitleBar.UpdateOnce(spriteBatch);

            base.UpdateOnce(spriteBatch);
        }

        void CalculateWindowMovment()
        {
            if (windowTitleBar.Pressed)
            {
                PreviousWindowTitleBarPosition = new Vector2(MouseCursor.Position.X - windowTitleBar.Position.X, MouseCursor.Position.Y - windowTitleBar.Position.Y);
                PreviousWindowPosition = new Vector2(MouseCursor.Position.X - base.Position.X, MouseCursor.Position.Y - base.Position.Y);

                MoveWindow = true;
                Focused = true;
                windowTitleBar.Pressed = false;
            }

            if (windowTitleBar.Released)
            {
                MoveWindow = false;
                windowTitleBar.Released = false;
            }

            if (!windowTitleBar.Hovered && MouseCursor.CurrentMouseState.LeftButton == ButtonState.Pressed && MouseCursor.LastMouseState.LeftButton != ButtonState.Pressed)
                Focused = false;

            if (MoveWindow)
            {
                windowTitleBar.Position = new Vector2(MouseCursor.Position.X - PreviousWindowTitleBarPosition.X, MouseCursor.Position.Y - PreviousWindowTitleBarPosition.Y);
                base.Position = new Vector2(MouseCursor.Position.X - PreviousWindowPosition.X, MouseCursor.Position.Y - PreviousWindowPosition.Y); ;
            }
        }

        public virtual new void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CalculateWindowMovment();

            windowTitleBar.Update(gameTime);
        }

        public virtual new void Draw()
        {
            base.Draw();

            windowTitleBar.Draw();
        }
    }
}