/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 28/April/2014
 * Date Moddified :- 9/March/2015
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

using VaultTech.Contents;

namespace VaultTech.UI.TextBoxComponents
{
    public class TextBox : Component
    {
        #region Private Variables
        Texture2D Cursor;
        Color CursorColor;

        TextBoxHelper textBoxHelper;

        Vector2 ConstantPosition;
        #endregion
        #region Public properties
        public bool IsPasswordProtected
        {
            get { return textBoxHelper.IsPasswordProtected; }
            set { textBoxHelper.IsPasswordProtected = value; }
        }

        public bool IsMultiLine
        {
            get { return textBoxHelper.IsMultiLine; }
            set { textBoxHelper.IsMultiLine = value; }
        }

        public Vector2 OffsetPosition { get; set; }

        public new Vector2 TextOffsetPosition
        {
            get { return base.TextOffsetPosition; }
            set { base.TextOffsetPosition = value; }
        }

        public new Vector2 Position
        {
            get { return base.Position; }
            set
            {
                base.Position = value;
                //textBoxHelper.TextPosition = base.Position + base.TextOffsetPosition;
                textBoxHelper.TextBoxPosition = base.Position;
            }
        }

        public new Vector2 Size
        {
            get { return base.Size; }
            set
            {
                base.Size = value;
                textBoxHelper.TextBoxSize = value;
            }
        }

        public bool EnterKeyPressed
        {
            get { return textBoxHelper.EnterKeyPressed; }
            set { textBoxHelper.EnterKeyPressed = value; }
        }
        #endregion

        public TextBox()
        {
            textBoxHelper = new TextBoxHelper(base.fontRenderer);
        }

        internal override void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
        }

        void LoadCursorTexture(string StyleFilePath, string TextBoxNodeNameInXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(FileManager.ContentFolder + StyleFilePath);

            if (xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures") != null)
            {
                Cursor = StreamTexture.LoadTextureFromStream(Graphics, xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures/CursorTexture").Attributes.GetNamedItem("Path").Value);
                CursorColor = new Color(int.Parse(xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures/CursorTextureColor").Attributes.GetNamedItem("Red").Value),
                                                int.Parse(xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures/CursorTextureColor").Attributes.GetNamedItem("Green").Value),
                                                int.Parse(xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures/CursorTextureColor").Attributes.GetNamedItem("Blue").Value),
                                                int.Parse(xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures/CursorTextureColor").Attributes.GetNamedItem("Alpha").Value));
            }
        }

        internal override void LoadContent(string StyleFilePath, string TextBoxNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, "UIStyle/TextBoxesStyle");
            LoadCursorTexture(StyleFilePath, "UIStyle/TextBoxesStyle");
        }

        internal override void UpdateOnce(SpriteBatch spriteBatch)
        {
            ConstantPosition = base.Position;
            base.UpdateOnce(spriteBatch);

            textBoxHelper.UpdateOnce();
        }

        internal override void Update(GameTime gameTime)
        {
            if (base.Pressed)
                textBoxHelper.Update(gameTime);

            if (!base.Hovered && MouseCursor.CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                base.Pressed = false;
                textBoxHelper.DrawCursor = false;
            }

            base.Update(gameTime);
        }

        internal override void Draw()
        {
            base.Draw();

            if (textBoxHelper.DrawCursor)
                spriteBatch.Draw(Cursor, textBoxHelper.CursorRectangle, Color.White);
        }

        internal override void DrawText()
        {
            Text = textBoxHelper.Text;
            base.TextOffsetPosition = textBoxHelper.TextPosition;
            TextColor = Color.White;
            base.DrawText();
            
        }
    }
}


/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using VaultTech.Contents;

namespace VaultTech.UI.TextBoxComponents
{
    public class TextBox : Component
    {
        #region Private Variables
        Texture2D Cursor;
        Color CursorColor;

        TextBoxHelper textBoxHelper;

        Vector2 ConstantPosition;
        #endregion
        #region Public properties
        public bool IsPasswordProtected
        {
            get { return textBoxHelper.IsPasswordProtected; }
            set { textBoxHelper.IsPasswordProtected = value; }
        }

        public bool IsMultiLine
        {
            get { return textBoxHelper.IsMultiLine; }
            set { textBoxHelper.IsMultiLine = value; }
        }

        public Vector2 OffsetPosition { get; set; }

        public new Vector2 TextOffsetPosition
        {
            get { return base.TextOffsetPosition; }
            set { base.TextOffsetPosition = value; }
        }

        public new Vector2 Position
        {
            get { return base.Position; }
            set
            {
                base.Position = value;
                textBoxHelper.TextPosition = base.Position + base.TextOffsetPosition;
                textBoxHelper.TextBoxPosition = base.Position;
            }
        }

        public new Vector2 Size
        {
            get { return base.Size; }
            set
            {
                base.Size = value;
                textBoxHelper.TextBoxSize = value;
            }
        }

        public new string Text
        {
            get { return textBoxHelper.Text; }
            set { textBoxHelper.Text = value; }
        }

        public bool EnterKeyPressed
        {
            get { return textBoxHelper.EnterKeyPressed; }
            set { textBoxHelper.EnterKeyPressed = value; }
        }
        #endregion

        public TextBox()
        {
            textBoxHelper = new TextBoxHelper(base.fontRenderer);
        }

        public new void Initialize(GraphicsDeviceManager Graphics)
        {
            base.Initialize(Graphics);
        }

        void LoadCursorTexture(string StyleFilePath, string TextBoxNodeNameInXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(FileManager.ContentFolder + StyleFilePath);

            if (xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures") != null)
            {
                Cursor = StreamTexture.LoadTextureFromStream(Graphics, xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures/CursorTexture").Attributes.GetNamedItem("Path").Value);
                CursorColor = new Color(int.Parse(xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures/CursorTextureColor").Attributes.GetNamedItem("Red").Value),
                                                int.Parse(xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures/CursorTextureColor").Attributes.GetNamedItem("Green").Value),
                                                int.Parse(xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures/CursorTextureColor").Attributes.GetNamedItem("Blue").Value),
                                                int.Parse(xmlDoc.SelectSingleNode(TextBoxNodeNameInXml + "/Textures/CursorTextureColor").Attributes.GetNamedItem("Alpha").Value));
            }
        }

        public new void LoadContent(string StyleFilePath, string TextBoxNodeNameInXml)
        {
            base.LoadContent(StyleFilePath, TextBoxNodeNameInXml);
            LoadCursorTexture(StyleFilePath, TextBoxNodeNameInXml);
        }

        public new void UpdateOnce(SpriteBatch spriteBatch)
        {
            ConstantPosition = base.Position;
            base.UpdateOnce(spriteBatch);

            textBoxHelper.UpdateOnce(spriteBatch);
        }

        public new void Update(GameTime gameTime)
        {
            if (base.Pressed)
                textBoxHelper.Update(gameTime);

            if (!base.Hovered && MouseCursor.CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                base.Pressed = false;
                textBoxHelper.DrawCursor = false;
            }

            base.Update(gameTime);
        }

        public new void Draw()
        {
            base.Draw();

            if (textBoxHelper.DrawCursor)
                spriteBatch.Draw(Cursor, textBoxHelper.CursorRectangle, Color.White);
        }

        public void DrawDynamicText(Color TextColor)
        {
            textBoxHelper.DrawString(TextColor);
        }
    }
    
}*/
