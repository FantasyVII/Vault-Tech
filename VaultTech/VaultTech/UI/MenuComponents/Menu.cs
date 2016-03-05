/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 10/August/2014
 * Date Moddified :- 8/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.UI.MenuComponents
{
    public class Menu : Component
    {
        public List<MenuItem> Item;
        
        public float OffsetSpaceBetweenItemsY;

        public Menu()
        {
            Item = new List<MenuItem>();
        }

        public new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);

            for (int i = 0; i < Item.Count; i++)
                Item[i].Initialize(Graphics);
        }

        public void LoadContent(string StyleFilePath, string MenuNodeNameInXml, string MenuItemNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, MenuNodeNameInXml);

            for (int i = 0; i < Item.Count; i++)
                Item[i].LoadContent(StyleFilePath, MenuNodeNameInXml + "/" + MenuItemNodeNameInXml);
        }

        public new void UpdateOnce(SpriteBatch spriteBatch, Vector2 Position, Vector2 SingleItemSize, float OffsetSpaceBetweenItemsY)
        {
            this.OffsetSpaceBetweenItemsY = OffsetSpaceBetweenItemsY;

            for (int i = 0; i < Item.Count; i++)
                Item[i].UpdateOnce(spriteBatch);

            base.Position = Position;
            base.Size = new Vector2(SingleItemSize.X, (SingleItemSize.Y * Item.Count) + (OffsetSpaceBetweenItemsY * Item.Count));
            
            base.UpdateOnce(spriteBatch);
        }

        public new void Update(GameTime gameTime)
        {
            for (int i = 0; i < Item.Count; i++)
            {
                Item[i].Size = new Vector2(base.Size.X, Item[i].Size.Y);

                if (Item[i].Alignment == Alignment.Right)
                    Item[i].Position = new Vector2((base.Position.X + base.Size.X) - Item[i].Size.X, (base.Position.Y + Item[i].Size.Y * i) + (OffsetSpaceBetweenItemsY * i));

                if (Item[i].Alignment == Alignment.Center)
                    Item[i].Position = new Vector2(base.Position.X + (base.Size.X / 2) - (Item[i].Size.X / 2), base.Position.Y + (Item[i].Size.Y * i) + (OffsetSpaceBetweenItemsY * i));

                if (Item[i].Alignment == Alignment.Left)
                    Item[i].Position = new Vector2(base.Position.X, base.Position.Y + (Item[i].Size.Y * i) + (OffsetSpaceBetweenItemsY * i));

                Item[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        public new void Draw()
        {
            base.Draw();

            for (int i = 0; i < Item.Count; i++)
                Item[i].Draw();
        }

        public new void DrawStaticText()
        {
           /* for (int i = 0; i < Item.Count; i++)
                Item[i].DrawStaticText();*/
        }
    }
}