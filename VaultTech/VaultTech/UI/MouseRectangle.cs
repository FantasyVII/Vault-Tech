/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 25/February/2015
 * Date Moddified :- 27/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VaultTech.UI
{
    public class MouseRectangle : Component
    {
        bool ChangeRectanglePosition = true;
        Vector2 LastPositionPressed;

        public new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
        }

        public new void LoadContent(string StyleFilePath, string ComponentNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, ComponentNodeNameInXml);
        }

        public new void LoadContentFromArchive(string StyleFilePath, string ComponentNodeNameInXml)
        {
            base.LoadContentFromArchive(StyleFilePath, ComponentNodeNameInXml);
        }

        public new void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);
        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed)
            {
                if (ChangeRectanglePosition)
                {
                    base.Position = MouseCursor.Position;
                    LastPositionPressed = MouseCursor.Position;
                    ChangeRectanglePosition = false;
                }
            }

            if (MouseCursor.Position.X > LastPositionPressed.X && MouseCursor.Position.Y > LastPositionPressed.Y)
            {
                if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    base.Position = new Vector2(LastPositionPressed.X, base.Position.Y);
                    base.Size = new Vector2(MouseCursor.Position.X - LastPositionPressed.X, MouseCursor.Position.Y - LastPositionPressed.Y);
                }
            }

            if (MouseCursor.Position.X < LastPositionPressed.X && MouseCursor.Position.Y > LastPositionPressed.Y)
            {
                if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    base.Position = new Vector2(MouseCursor.Position.X, base.Position.Y);
                    base.Size = new Vector2(LastPositionPressed.X - MouseCursor.Position.X, MouseCursor.Position.Y - LastPositionPressed.Y);
                }
            }

            if (MouseCursor.Position.X > LastPositionPressed.X && MouseCursor.Position.Y < LastPositionPressed.Y)
            {
                if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    base.Position = new Vector2(base.Position.X, MouseCursor.Position.Y);
                    base.Size = new Vector2(MouseCursor.Position.X - LastPositionPressed.X, LastPositionPressed.Y - MouseCursor.Position.Y);
                }
            }

            if (MouseCursor.Position.X < LastPositionPressed.X && MouseCursor.Position.Y < LastPositionPressed.Y)
            {
                if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    base.Position = new Vector2(MouseCursor.Position.X, MouseCursor.Position.Y);
                    base.Size = new Vector2(LastPositionPressed.X - MouseCursor.Position.X, LastPositionPressed.Y - MouseCursor.Position.Y);
                }
            }

            if (MouseCursor.LastMouseState.LeftButton == ButtonState.Released)
            {
                ChangeRectanglePosition = true;
                base.Position = Vector2.Zero;
                base.Size = Vector2.Zero;
            }
        }

        public new void Draw()
        {
            base.Draw();
        }
    }
}