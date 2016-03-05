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

using VaultTech.UI.MenuComponents;
using VaultTech.UI.PanelComponents;

namespace VaultTech.UI.DropDownMenuComponents
{
    public class DropDownMenuPanel : Panel
    {
        public Menu menu;

        public DropDownMenuPanel()
        {
            menu = new Menu();
            base.HasHorizontalScrollbar = true;
            base.HasVerticalScrollbar = true;
        }

        public new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
            menu.Initialize(Graphics);
        }

        public void LoadContent(string StyleFilePath, string PanelNodeNameInXml, string MenuNodeNameInXml, string MenuItemNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, PanelNodeNameInXml);
            menu.LoadContent(StyleFilePath, MenuNodeNameInXml, MenuItemNodeNameInXml);
        }

        public new void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);
            menu.UpdateOnce(spriteBatch);
        }

        public new void Update(GameTime gameTime)
        {
            menu.Position = base.Position;
            menu.Size = base.Size;

            base.Update(gameTime);
            menu.Update(gameTime);
        }

        public new void Draw()
        {
            base.Draw();
            menu.Draw();
        }
    }
}