/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 22/April/2014
 * Date Moddified :- 8/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.Contents;

namespace VaultTech.UI
{
    public class CheckBox : Component
    {
        #region Private Variables
        Texture2D CheckedTexture;
        Color CheckedTextureColor;
        Rectangle CheckedTextureRectangle;
        #endregion
        #region Public properties
        public bool Checked { get; set; }

        #endregion

        public new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
        }

        void LoadCheckedTexture(string StyleFilePath, string CheckBoxNodeNameInXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(FileManager.ContentFolder + StyleFilePath);

            if (xmlDoc.SelectSingleNode(CheckBoxNodeNameInXml + "/Textures") != null)
            {
                CheckedTexture = StreamTexture.LoadTextureFromStream(Graphics, xmlDoc.SelectSingleNode(CheckBoxNodeNameInXml + "/Textures/CheckedTexture").Attributes.GetNamedItem("Path").Value);
                CheckedTextureColor = new Color(int.Parse(xmlDoc.SelectSingleNode(CheckBoxNodeNameInXml + "/Textures/CheckedTextureColor").Attributes.GetNamedItem("Red").Value),
                                               int.Parse(xmlDoc.SelectSingleNode(CheckBoxNodeNameInXml + "/Textures/CheckedTextureColor").Attributes.GetNamedItem("Green").Value),
                                               int.Parse(xmlDoc.SelectSingleNode(CheckBoxNodeNameInXml + "/Textures/CheckedTextureColor").Attributes.GetNamedItem("Blue").Value),
                                               int.Parse(xmlDoc.SelectSingleNode(CheckBoxNodeNameInXml + "/Textures/CheckedTextureColor").Attributes.GetNamedItem("Alpha").Value));
            }
        }

        public new void LoadContent(string StyleFilePath, string CheckBoxNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, CheckBoxNodeNameInXml);
            LoadCheckedTexture(StyleFilePath, CheckBoxNodeNameInXml);
        }

        public new void LoadContentFromArchive(string StyleFilePath, string CheckBoxNodeNameInXml)
        {
            throw new NotImplementedException();
        }

        public new void UpdateOnce(SpriteBatch spriteBatch)
        {
            base.UpdateOnce(spriteBatch);
        }

        public new void Update(GameTime gameTime)
        {
            CheckedTextureRectangle = new Rectangle((int)(base.Position.X + base.borderThickness.Left), (int)(base.Position.Y + base.borderThickness.Top),
                                                    (int)(base.Size.X - base.borderThickness.Left * 2), (int)(base.Size.Y - base.borderThickness.Top * 2));

            if (base.Hovered)
                base.CurrentComponentTexture = base.HoveredComponentTexture;
            else
            {
                base.Pressed = false;
                base.Released = false;
            }

            if (Checked && base.Hovered && base.Pressed && base.Released)
            {
                Checked = false;
                base.Pressed = false;
                base.Released = false;
            }

            if (base.Hovered && base.Pressed && base.Released)
            {
                Checked = true;
                base.Pressed = false;
                base.Released = false;
            }

            base.Released = false;

            base.Update(gameTime);
        }

        public new void Draw()
        {
            base.Draw();

            if (Checked)
                spriteBatch.Draw(CheckedTexture, CheckedTextureRectangle, CheckedTextureColor);
        }
    }
}