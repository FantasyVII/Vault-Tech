/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 14/June/2014
 * Date Moddified :- 9/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VaultTech.UI.MenuStripComponents
{
    public class MenuStrip
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        public List<MenuStripItem> MenuStripItems;

        Texture2D Background;
        Color BackgroundColor;

        Vector2 Size;
        Rectangle rectangle;

        bool MenuStripPressed;

        public MenuStrip()
        {
            MenuStripItems = new List<MenuStripItem>();
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;

            for (int i = 0; i < MenuStripItems.Count; i++)
                MenuStripItems[i].Initialize(Graphics);
        }

        void LoadBackgroundTexture(string StyleFilePath, string MenuStripNodeNameInXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(StyleFilePath);

            if (xmlDoc.SelectSingleNode(MenuStripNodeNameInXml + "/Textures") != null)
            {
                //Background = Content.Load<Texture2D>(xmlDoc.SelectSingleNode(MenuStripNodeNameInXml + "/Textures/BackgroundTexture").Attributes.GetNamedItem("Path").Value);
                BackgroundColor = new Color(int.Parse(xmlDoc.SelectSingleNode(MenuStripNodeNameInXml + "/Textures/BackgroundTextureColor").Attributes.GetNamedItem("Red").Value),
                                                int.Parse(xmlDoc.SelectSingleNode(MenuStripNodeNameInXml + "/Textures/BackgroundTextureColor").Attributes.GetNamedItem("Green").Value),
                                                int.Parse(xmlDoc.SelectSingleNode(MenuStripNodeNameInXml + "/Textures/BackgroundTextureColor").Attributes.GetNamedItem("Blue").Value),
                                                int.Parse(xmlDoc.SelectSingleNode(MenuStripNodeNameInXml + "/Textures/BackgroundTextureColor").Attributes.GetNamedItem("Alpha").Value));
            }
        }

        public void LoadContent(string StyleFilePath, string MenuStripNodeNameInXml)
        {
            LoadBackgroundTexture(StyleFilePath, MenuStripNodeNameInXml);

            for (int i = 0; i < MenuStripItems.Count; i++)
                MenuStripItems[i].LoadContent(StyleFilePath, MenuStripNodeNameInXml + "/MenuStripItemStyle");
        }

        public void UpdateOnce(SpriteBatch spriteBatch, Vector2 Position, Vector2 Size)
        {
            this.spriteBatch = spriteBatch;

            Size = new Vector2(Graphics.PreferredBackBufferWidth, 25);
            rectangle = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);

            MenuStripItems[0].Position = new Vector2(0, 0);

            for (int i = 0; i < MenuStripItems.Count; i++)
            {
                if (i > 0)
                    MenuStripItems[i].Position = new Vector2(MenuStripItems[i - 1].Position.X + MenuStripItems[i - 1].Size.X, 0);

                MenuStripItems[i].Size = new Vector2(100, 25);
                MenuStripItems[i].UpdateOnce(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (MouseCursor.CurrentMouseState.LeftButton == ButtonState.Pressed && MouseCursor.LastMouseState.LeftButton != ButtonState.Pressed && MenuStripPressed)
            {
                for (int i = 0; i < MenuStripItems.Count; i++)
                {
                    MenuStripItems[i].Show = false;
                    MenuStripItems[i].Pressed = false;
                }

                MenuStripPressed = false;
            }

            for (int i = 0; i < MenuStripItems.Count; i++)
            {
                MenuStripItems[i].Update(gameTime);

                if (MenuStripItems[i].Pressed)
                    MenuStripPressed = true;

                if (MenuStripPressed)
                {
                    if (MenuStripItems[i].Hovered)
                    {
                        for (int j = 0; j < MenuStripItems.Count; j++)
                        {
                            MenuStripItems[j].Show = false;
                            MenuStripItems[j].Pressed = false;
                        }

                        MenuStripItems[i].Pressed = true;
                        MenuStripItems[i].Show = true;
                    }
                }
            }
        }

        public void Draw()
        {
            spriteBatch.Draw(Background, rectangle, BackgroundColor);

            for (int i = 0; i < MenuStripItems.Count; i++)
                MenuStripItems[i].Draw();
        }

        public void DrawStaticText()
        {
            for (int i = 0; i < MenuStripItems.Count; i++)
                MenuStripItems[i].DrawStaticText();
        }
    }
}