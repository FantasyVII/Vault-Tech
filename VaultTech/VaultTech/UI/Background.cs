/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 22/April/2014
 * Date Moddified :- 18/January/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.UI
{
    public class Background
    {
        GraphicsDeviceManager Graphics;

        Texture2D Texture;
        Rectangle rectangle;
        Color color;

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        void LoadXmlFile(ContentManager Content, string StyleFilePath, string ComponentNodeNameInXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(StyleFilePath);

            if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml) != null)
            {
                Texture = Content.Load<Texture2D>(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Texture").Attributes.GetNamedItem("Path").Value);
                color = new Color(int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/TextureColor").Attributes.GetNamedItem("Red").Value),
                                  int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/TextureColor").Attributes.GetNamedItem("Green").Value),
                                  int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/TextureColor").Attributes.GetNamedItem("Blue").Value),
                                  int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/TextureColor").Attributes.GetNamedItem("Alpha").Value));
            }
        }

        public void LoadContent(ContentManager Content, string StyleFilePath, string ComponentNodeNameInXml)
        {
            LoadXmlFile(Content, StyleFilePath, ComponentNodeNameInXml);
        }

        public void UpdateOnce()
        {
            rectangle = new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, rectangle, color);
        }
    }
}