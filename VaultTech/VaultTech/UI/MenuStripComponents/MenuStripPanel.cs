/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 15/June/2014
 * Date Moddified :- 9/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.UI.PanelComponents;

namespace VaultTech.UI.MenuStripComponents
{
    public class MenuStripPanel : Panel
    {
        public List<MenuStripItem> Items;

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

        public MenuStripPanel()
        {
            Items = new List<MenuStripItem>();

            base.HasHorizontalScrollbar = false;
            base.HasVerticalScrollbar = false;
        }

        public new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);

            for (int i = 0; i < Items.Count; i++)
                Items[i].Initialize(Graphics);
        }

        public new void LoadContent(string StyleFilePath, string PanelNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, PanelNodeNameInXml);

            for (int i = 0; i < Items.Count; i++)
                Items[i].LoadContent(StyleFilePath, "UIStyle/MenuStripStyle/MenuStripItemStyle");
        }

        public new void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);

            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Size = new Vector2(base.Size.X, 25);
                Items[i].Position = new Vector2(base.Position.X, base.Position.Y + (i * Items[i].Size.Y));
                Items[i].UpdateOnce(spriteBatch);
            }
        }

        public new void Update(GameTime gameTime)
        {
            for (int i = 0; i < Items.Count; i++)
                Items[i].Update(gameTime);

            if (Items.Count > 0)
                base.Size = new Vector2(base.Size.X, Items[0].Size.Y * Items.Count);

            base.Update(gameTime);
        }

        public new void Draw()
        {
            base.Draw();

            for (int i = 0; i < Items.Count; i++)
                Items[i].Draw();
        }

        public void DrawStaticText()
        {
            for (int i = 0; i < Items.Count; i++)
                Items[i].DrawStaticText();
        }
    }
}