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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VaultTech.UI.WindowComponents
{
    public class WindowTitleBar : Component
    {
        #region Private Variables
        Rectangle MoveingAreaRec;
        #endregion
        #region Public properties
        public Button CloseButton;

        public new Vector2 Position
        {
            get { return base.Position; }
            set { base.Position = value; }
        }

        public new Vector2 Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        public new bool Hovered, Pressed, Released;
        #endregion

        public WindowTitleBar()
        {
            CloseButton = new Button();
        }

        public new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);

            CloseButton.Initialize(Graphics);
        }

        public new void LoadContent(string StyleFilePath, string ComponentNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, ComponentNodeNameInXml);

            CloseButton.LoadContent(StyleFilePath, ComponentNodeNameInXml + "/CloseButton");
        }

        public new void UpdateOnce(SpriteBatch spriteBatch)
        {
            CloseButton.UpdateOnce(spriteBatch);

            base.UpdateOnce(spriteBatch);
        }

        void SetPositions()
        {
            CloseButton.Position = new Vector2(base.Position.X + base.Size.X - base.borderThickness.Right - CloseButton.Size.X, base.Position.Y + base.borderThickness.Top);
            
            MoveingAreaRec = new Rectangle((int)(base.Position.X + base.borderThickness.Left), (int)(base.Position.Y + base.borderThickness.Left),
                                            (int)(base.Size.X - CloseButton.Size.X - base.borderThickness.Right), (int)(base.Size.Y - base.borderThickness.Top - base.borderThickness.Bottom));
        }

        void WindowTitaleBarMouseStatus()
        {
            if (MoveingAreaRec.Intersects(MouseCursor.rectangle))
                Hovered = true;
            else
                Hovered = false;

            if (Hovered && MouseCursor.CurrentMouseState.LeftButton == ButtonState.Pressed && MouseCursor.LastMouseState.LeftButton != ButtonState.Pressed)
                Pressed = true;

            if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed && MouseCursor.CurrentMouseState.LeftButton == ButtonState.Released)
                Released = true;
        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CloseButton.Update(gameTime);

            SetPositions();
            WindowTitaleBarMouseStatus();
        }

        public new void Draw() 
        {
            base.Draw();

            CloseButton.Draw();
        }
    }
}