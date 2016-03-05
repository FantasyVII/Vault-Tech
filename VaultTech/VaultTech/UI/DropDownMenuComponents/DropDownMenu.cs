/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 11/June/2014
 * Date Moddified :- 9/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VaultTech.UI.DropDownMenuComponents
{
    public class DropDownMenu : Component
    {
        public DropDownMenuPanel DropMenuPanel;
        Button DropDownButton;

        bool Show;
        public int ItemSelected;

        public DropDownMenu()
        {
            DropMenuPanel = new DropDownMenuPanel();
            DropDownButton = new Button();
        }

        public new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
            DropMenuPanel.Initialize(Graphics);
            DropDownButton.Initialize(Graphics);
        }

        public void LoadContent(string StyleFilePath, string DropMenuNodeNameInXml, string PanelNodeNameInXml, string MenuNodeNameInXml, string MenuItemNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, DropMenuNodeNameInXml);
            DropMenuPanel.LoadContent(StyleFilePath, PanelNodeNameInXml, MenuNodeNameInXml, MenuItemNodeNameInXml);
            DropDownButton.LoadContent(StyleFilePath, DropMenuNodeNameInXml + "/ButtonsStyle");
        }

        public new void UpdateOnce(SpriteBatch spriteBatch, Vector2 Position, Vector2 Size)
        {
            DropDownButton.Size = new Vector2(19, base.Size.Y - base.borderThickness.Top - base.borderThickness.Bottom);
            DropDownButton.Position = new Vector2(base.Position.X + base.Size.X - DropDownButton.Size.X - base.borderThickness.Right, base.Position.Y + base.borderThickness.Top);
            DropMenuPanel.UpdateOnce(spriteBatch);
            DropDownButton.UpdateOnce(spriteBatch);
            base.UpdateOnce(spriteBatch);
        }

        public new void Update(GameTime gameTime)
        {
            DropMenuPanel.Position = new Vector2(base.Position.X, base.Position.Y + base.Size.Y);
            DropMenuPanel.Size = new Vector2(base.Size.X, 100);
            DropDownButton.Update(gameTime);
            if (base.Pressed)
            {

                DropDownButton.Pressed = true;
                DropDownButton.CurrentComponentTexture = DropDownButton.PressedComponentTexture;
                Show = true;
                //base.Pressed = false;
            }

            if (MouseCursor.CurrentMouseState.LeftButton == ButtonState.Pressed && !base.Hovered && !DropMenuPanel.menu.Hovered)
            {
                Show = false;
                DropDownButton.Pressed = false;
                base.Pressed = false;
            }


            if (Show)
            {
                DropMenuPanel.Update(gameTime);

                for (int i = 0; i < DropMenuPanel.menu.Item.Count; i++)
                {
                    if (DropMenuPanel.menu.Item[i].Pressed)
                    {
                        Show = false;
                        DropMenuPanel.menu.Item[i].Pressed = false;
                        ItemSelected = i;
                    }
                }
            }


            base.Update(gameTime);
        }

        public new void Draw()
        {
            base.Draw();

            DropDownButton.Draw();

            if (Show)
                DropMenuPanel.Draw();
        }
    }
}