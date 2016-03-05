/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 30/April/2014
 * Date Moddified :- 11/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.Contents;

namespace VaultTech.Graphics.FontRenderer
{
    class Bitmap
    {
        internal int ID;
        internal Texture2D Texture;

        internal Bitmap(int ID, Texture2D Texture)
        {
            this.ID = ID;
            this.Texture = Texture;
        }
    }

    /// <summary>
    /// This class is responsible for rendering text on the screen.
    /// </summary>
    public class FontRenderer
    {
        #region MonoGame variables
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;
        #endregion

        List<CharacterData> characterData;
        public List<Character> Characters;

        public string FontName;
        public int FontSize;
        public int NewLineHeight;

        string OriginalText;

        public FontRenderer()
        {
            Characters = new List<Character>();
            characterData = new List<CharacterData>();
        }

        /// <summary>
        /// Initialize FontRenderer GraphicsDeviceManager.
        /// </summary>
        /// <param name="Graphics">MonoGame GraphicsDeviceManager</param>
        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        void LoadFontFile(Stream XMLStream, string FontPath, bool LoadFromContentArchive)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XMLStream);
            FileManager.Dispose();

            FontName = xmlDoc.SelectSingleNode("font/info").Attributes.GetNamedItem("face").Value;
            FontSize = int.Parse(xmlDoc.SelectSingleNode("font/info").Attributes.GetNamedItem("size").Value);
            NewLineHeight = int.Parse(xmlDoc.SelectSingleNode("font/common").Attributes.GetNamedItem("lineHeight").Value);

            #region Load Bitmap textures
            List<Bitmap> Bitmaps = new List<Bitmap>();

            foreach (XmlNode node in xmlDoc.SelectNodes("font/pages/page"))
            {
                MemoryStream TextureStream;
                try
                {
                    string BitmapPath = node.Attributes.GetNamedItem("file").Value;
                    TextureStream = FileManager.GetFileMemoryStreamFromArchive(FontPath, BitmapPath);

                    if (TextureStream == null)
                        throw new Exception("Could not find file \"" + FileManager.ContentFolder + FontPath + "/" + BitmapPath + "\"");
                }
                catch (Exception ex) { throw ex; }

                Bitmaps.Add(new Bitmap(int.Parse(node.Attributes.GetNamedItem("id").Value), Texture2D.FromStream(Graphics.GraphicsDevice, TextureStream)));
                TextureStream.Dispose();
                FileManager.Dispose();
            }
            #endregion

            #region Load characters data
            foreach (XmlNode node in xmlDoc.SelectNodes("font/chars/char"))
            {
                int ASCII_ID = int.Parse(node.Attributes.GetNamedItem("id").Value);
                Rectangle SourceRectangle = new Rectangle(int.Parse(node.Attributes.GetNamedItem("x").Value), int.Parse(node.Attributes.GetNamedItem("y").Value),
                                                          int.Parse(node.Attributes.GetNamedItem("width").Value), int.Parse(node.Attributes.GetNamedItem("height").Value));
                Vector2 OffsetPosition = new Vector2(int.Parse(node.Attributes.GetNamedItem("xoffset").Value), int.Parse(node.Attributes.GetNamedItem("yoffset").Value));
                int Spacing = int.Parse(node.Attributes.GetNamedItem("xadvance").Value);
                int TextureID = int.Parse(node.Attributes.GetNamedItem("page").Value);
                Texture2D BitmapTexture = null;

                for (int i = 0; i < Bitmaps.Count; i++)
                    if (TextureID == Bitmaps[i].ID)
                        BitmapTexture = Bitmaps[i].Texture;

                characterData.Add(new CharacterData(ASCII_ID, BitmapTexture, SourceRectangle, OffsetPosition, Spacing));
            }

            FileManager.Dispose();
            #endregion
        }

        public void LoadContent(string FontPath)
        {
            #region Get .fnt file stream from .font archive file.
            FontPath = FontPath.Replace('\\', '/');
            string XMLFontPath = FontPath;

            if (XMLFontPath.Contains('/'))
                XMLFontPath = XMLFontPath.Substring(XMLFontPath.LastIndexOf('/') + 1, XMLFontPath.Length - XMLFontPath.LastIndexOf('/') - 1).Replace(".font", ".fnt");
            else
                XMLFontPath = FontPath.Replace(".font", ".fnt");

            Stream FontStream;
            try
            {
                FontStream = FileManager.GetFileStreamFromArchive(FontPath, XMLFontPath);

                if (FontStream == null)
                    throw new Exception("Could not find file \"" + FileManager.ContentFolder + FontPath + "/" + XMLFontPath + "\"");
            }
            catch (Exception ex) { throw ex; }
            #endregion

            LoadFontFile(FontStream, FontPath, false);
        }

        /// <summary>
        /// Initialize FontRenderer SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">MonoGame SpriteBatch.</param>
        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Measure the string size.
        /// </summary>
        /// <param name="Text">The string or text to measure.</param>
        /// <returns>Returns the string height and width in a Vector2 class type.</returns>
        public Vector2 MeasureString(string Text)
        {
            Vector2 TextSize = Vector2.Zero;

            for (int i = 0; i < Text.Length; i++)
            {
                for (int j = 0; j < characterData.Count; j++)
                {
                    if (Text[i] == characterData[j].Character)
                    {
                        TextSize.X += characterData[j].Spacing;
                        TextSize.Y = NewLineHeight;
                    }
                }
            }

            return TextSize;
        }

        /// <summary>
        /// Takes a string and finds the appropriate texture for it and stores it in a Characters list to be then drawn on the screen.
        /// </summary>
        /// <param name="Text">String of text.</param>
        /// <param name="Position">Text position on the screen.</param>
        /// <param name="color">Text color.</param>
        void ConvertStringToBitmapTextures(string Text, Color color)
        {
            if (Text != OriginalText)
            {
                OriginalText = Text;
                Characters = new List<Character>();
                Vector2 CharPosition = Vector2.Zero;

                for (int i = 0; i < Text.Length; i++)
                {
                    if (i == Text.IndexOf("\n", i))
                    {
                        CharPosition.X = 0;
                        CharPosition.Y += NewLineHeight;
                    }

                    for (int j = 0; j < characterData.Count; j++)
                    {
                        if (Text[i] == characterData[j].Character)
                        {
                            Characters.Add(new Character(characterData[j], Vector2.Zero, color));
                            Characters[Characters.Count - 1].PositionInString = new Vector2(CharPosition.X + characterData[j].OffsetPosition.X, CharPosition.Y + characterData[j].OffsetPosition.Y);
                            CharPosition.X += characterData[j].Spacing;

                            if (i == Text.Length - 1)
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws a list of character Textures that are stored in the Characters list.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Position"></param>
        /// <param name="color"></param>
        public void Draw(string Text, Vector2 Position, Color color)
        {
            ConvertStringToBitmapTextures(Text, color);

            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].Position = Position + Characters[i].PositionInString;
                spriteBatch.Draw(Characters[i].characterData.Texture, Characters[i].Position, Characters[i].characterData.SourceRectangle, Characters[i].color);
            }
        }
    }
}

/*
/// <summary>
/// Draws a specific part of the text.
/// </summary>
/// <param name="Text">The text that would be drawn</param>
/// <param name="Position">Text Position</param>
/// <param name="SourceRectangle"></param>
/// <param name="color">Text color</param>
public void Draw(string Text, Vector2 Position, Rectangle SourceRectangle, Color color)
{
    ConvertStringToBitmapTextures(Text, Position, color);

    for (int i = 0; i < Characters.Count; i++)
        if (Characters[i].Position.X + Characters[i].Size.X <= SourceRectangle.X + SourceRectangle.Width && Characters[i].Position.X >= SourceRectangle.X)
            spriteBatch.Draw(Characters[i].characterData.Texture, Characters[i].Position, Characters[i].characterData.SourceRectangle, Characters[i].color);
}*/

/*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.Contents;

namespace VaultTech.Graphics.FontRenderer
{
    /// <summary>
    /// This class is responsible for rendering text on the screen.
    /// </summary>
    public class FontRenderer
    {
        #region MonoGame variables
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;
        #endregion
        #region Private variables
        Texture2D BitmapTexture;
        Texture2D StaticText;

        List<CharacterMap> characterMap;
        List<Character> Characters = new List<Character>();
        List<int> IndexesOfNewLine = new List<int>();

        string OriginalString;
        #endregion
        #region Public variables
        /// <summary>
        /// If this boolean is true it will reconvert the text to a texture.
        /// if this boolean is false it will only convert the text to a texture once.
        /// </summary>
        public bool ConvertStaticTextToTexture = true;

        /// <summary>
        /// Get the new line character height in pixels.
        /// </summary>
        public int NewLineHeight { get; private set; }

        /// <summary>
        /// Font Size.
        /// </summary>
        public int FontSize;
        #endregion

        /// <summary>
        /// FontRenderer constructor
        /// </summary>
        public FontRenderer()
        {
            characterMap = new List<CharacterMap>();
        }

        /// <summary>
        /// Initialize FontRenderer GraphicsDeviceManager.
        /// </summary>
        /// <param name="Graphics">MonoGame GraphicsDeviceManager</param>
        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        void LoadFontXmlStream(Stream XMLStream, string FontPath, bool LoadFromContentArchive)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XMLStream);

            NewLineHeight = int.Parse(xmlDoc.SelectSingleNode("font/common").Attributes.GetNamedItem("lineHeight").Value);
            FontSize = NewLineHeight;

            string TextureFontPath = FontPath;

            if (TextureFontPath.Contains('/'))
                TextureFontPath = TextureFontPath.Substring(TextureFontPath.LastIndexOf('/') + 1, TextureFontPath.Length - TextureFontPath.LastIndexOf('/') - 1).Replace(".font", ".png");
            else
                TextureFontPath = FontPath.Replace(".font", ".png");

            FileManager.Dispose();

            if (LoadFromContentArchive)
            {
                Stream ContentStream;
                MemoryStream TextureStream;

                try
                {
                    ContentStream = FileManager.GetFileStreamFromArchive(FontPath);
                    TextureStream = FileManager.GetFileMemoryStreamFromArchive(ContentStream, TextureFontPath);

                    if (ContentStream == null)
                        throw new Exception("Could not find file \"" + FileManager.ContentFolder + FileManager.SourceArchiveFileName + "/" + FontPath + "\"");
                    else if (TextureStream == null)
                        throw new Exception("Could not find file \"" + FileManager.ContentFolder + FileManager.SourceArchiveFileName + "/" + FontPath + "/" + TextureFontPath + "\"");
                }
                catch (Exception ex) { throw ex; }

                BitmapTexture = Texture2D.FromStream(Graphics.GraphicsDevice, TextureStream);

                ContentStream.Dispose();
                TextureStream.Dispose();
            }
            else
            {
                MemoryStream TextureStream;
                try
                {
                    TextureStream = FileManager.GetFileMemoryStreamFromArchive(FontPath, TextureFontPath);

                    if (TextureStream == null)
                        throw new Exception("Could not find file \"" + FileManager.ContentFolder + FontPath + "/" + TextureFontPath + "\"");
                }
                catch (Exception ex) { throw ex; }

                BitmapTexture = Texture2D.FromStream(Graphics.GraphicsDevice, TextureStream);

                TextureStream.Dispose();
            }

            foreach (XmlNode node in xmlDoc.SelectNodes("font/chars/char"))
            {
                int ASCII_ID = int.Parse(node.Attributes.GetNamedItem("id").Value);
                Vector2 Position = new Vector2(int.Parse(node.Attributes.GetNamedItem("x").Value), int.Parse(node.Attributes.GetNamedItem("y").Value));
                Vector2 OffsetPosition = new Vector2(int.Parse(node.Attributes.GetNamedItem("xoffset").Value), int.Parse(node.Attributes.GetNamedItem("yoffset").Value));
                int Spacing = int.Parse(node.Attributes.GetNamedItem("xadvance").Value);
                Vector2 Size = new Vector2(int.Parse(node.Attributes.GetNamedItem("width").Value), int.Parse(node.Attributes.GetNamedItem("height").Value));

                Texture2D CharacterTexture = CropTexture.Crop(Graphics, BitmapTexture, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y));

                characterMap.Add(new CharacterMap(ASCII_ID, CharacterTexture, OffsetPosition, Spacing, Size));
            }

            FileManager.Dispose();
        }

        /// <summary>
        /// This method will load font from specified folder.
        /// </summary>
        /// <param name="FontPath">Font path</param>
        public void LoadContent(string FontPath)
        {
            FontPath = FontPath.Replace('\\', '/');
            string XMLFontPath = FontPath;

            if (XMLFontPath.Contains('/'))
                XMLFontPath = XMLFontPath.Substring(XMLFontPath.LastIndexOf('/') + 1, XMLFontPath.Length - XMLFontPath.LastIndexOf('/') - 1).Replace(".font", ".fnt");
            else
                XMLFontPath = FontPath.Replace(".font", ".fnt");

            Stream FontStream;

            try
            {
                FontStream = FileManager.GetFileStreamFromArchive(FontPath, XMLFontPath);

                if (FontStream == null)
                    throw new Exception("Could not find file \"" + FileManager.ContentFolder + FontPath + "/" + XMLFontPath + "\"");
            }
            catch (Exception ex) { throw ex; }

            LoadFontXmlStream(FontStream, FontPath, false);

            FontStream.Dispose();
        }

        /// <summary>
        /// This method will load the font from a compressed file.
        /// </summary>
        /// <param name="FontPathInArchive">Font path</param>
        public void LoadContentFromArchive(string FontPathInArchive)
        {
            FontPathInArchive = FontPathInArchive.Replace('\\', '/');
            string XMLFontPath = FontPathInArchive;

            if (XMLFontPath.Contains('/'))
                XMLFontPath = XMLFontPath.Substring(XMLFontPath.LastIndexOf('/') + 1, XMLFontPath.Length - XMLFontPath.LastIndexOf('/') - 1).Replace(".font", ".fnt");
            else
                XMLFontPath = FontPathInArchive.Replace(".font", ".fnt");

            Stream ContentStream, FontStream;

            try
            {
                ContentStream = FileManager.GetFileStreamFromArchive(FontPathInArchive);
                FontStream = FileManager.GetFileStreamFromArchive(ContentStream, XMLFontPath);

                if (ContentStream == null)
                    throw new Exception("Could not find file \"" + FileManager.ContentFolder + FileManager.SourceArchiveFileName + "/" + FontPathInArchive + "\"");
                else if (FontStream == null)
                    throw new Exception("Could not find file \"" + FileManager.ContentFolder + FileManager.SourceArchiveFileName + "/" + FontPathInArchive + "/" + XMLFontPath + "\"");
            }
            catch (Exception ex) { throw ex; }

            LoadFontXmlStream(FontStream, FontPathInArchive, true);

            ContentStream.Dispose();
            FontStream.Dispose();
        }

        /// <summary>
        /// Initialize FontRenderer SpriteBatch.
        /// </summary>
        /// <param name="spriteBatch">MonoGame SpriteBatch.</param>
        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Measure the string size.
        /// </summary>
        /// <param name="Text">The string or text to measure.</param>
        /// <returns>Returns the string height and width in a Vector2 class type.</returns>
        public Vector2 MeasureString(string Text)
        {
            Vector2 TextSize = Vector2.Zero;

            for (int i = 0; i < Text.Length; i++)
            {
                for (int j = 0; j < characterMap.Count; j++)
                {
                    if (Text[i] == characterMap[j].Character)
                    {
                        TextSize.X += characterMap[j].Spacing;
                        TextSize.Y = NewLineHeight;
                    }
                }
            }

            return TextSize;
        }

        /// <summary>
        /// Measure the string size between two specific indexes.
        /// </summary>
        /// <param name="Text">The string or text to measure.</param>
        /// <param name="StartingIndex">The starting index of the string to start the measurement at.</param>
        /// <param name="Length">The ending index of the string.</param>
        /// <returns>Returns the string height and width in a Vector2 class type.</returns>
        public Vector2 MeasureString(string Text, int StartingIndex, int Length)
        {
            Vector2 TextSize = Vector2.Zero;

            for (int i = StartingIndex; i < Length; i++)
            {
                for (int j = 0; j < characterMap.Count; j++)
                {
                    if (Text[i] == characterMap[j].Character)
                    {
                        TextSize.X += characterMap[j].Spacing;
                        TextSize.Y = NewLineHeight;
                    }
                }
            }

            return TextSize;
        }

        /// <summary>
        /// Check if a string contains a new line character.
        /// </summary>
        /// <param name="Text"></param>
        void DoesStringContainsNewLine(string Text)
        {
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text.Contains("\n"))
                    if (i == Text.IndexOf("\n", i))
                        if (!IndexesOfNewLine.Contains(Text.IndexOf("\n", i) + 1))
                            IndexesOfNewLine.Add(Text.IndexOf("\n", i) + 1);
            }
        }

        /// <summary>
        /// Find all the character bitmaps in the Texture and add them in the character class.
        /// </summary>
        /// <param name="Text">String text.</param>
        /// <param name="color">Text Color</param>
        void FindCharacter(string Text, Color color)
        {
            if (Text != OriginalString)
            {
                OriginalString = Text;
                Characters = new List<Character>();
                Vector2 Position = Vector2.Zero;
                float TextStartingXPosition = Position.X;
                DoesStringContainsNewLine(Text);

                for (int i = 0; i < Text.Length; i++)
                {
                    //If the string contains a new line then add a new line.
                    for (int k = 0; k < IndexesOfNewLine.Count; k++)
                    {
                        if (i == IndexesOfNewLine[k])
                        {
                            Position.X = TextStartingXPosition;
                            Position.Y += NewLineHeight;
                        }
                    }

                    for (int j = 0; j < characterMap.Count; j++)
                    {
                        if (Text[i] == characterMap[j].Character)
                        {
                            Characters.Add(new Character(characterMap[j].ASCII_ID, characterMap[j].Character, characterMap[j].Texture, new Vector2(Position.X + characterMap[j].OffsetPosition.X, Position.Y + characterMap[j].OffsetPosition.Y), characterMap[j].Size, color));
                            Position.X += characterMap[j].Spacing;

                            if (i + 1 == Text.Length)
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws every character in the string.
        /// </summary>
        /// <param name="Text">The text that would be drawn</param>
        /// <param name="Position">Text position</param>
        /// <param name="color">Text color</param>
        void DrawText(string Text, Vector2 Position, Color color)
        {
            FindCharacter(Text, color);

            for (int i = 0; i < Characters.Count; i++)
                spriteBatch.Draw(Characters[i].Texture, Characters[i].Position + Position, Characters[i].Color);
        }

        /// <summary>
        /// Draws a specific part of the text.
        /// </summary>
        /// <param name="Text">The text that would be drawn</param>
        /// <param name="Position">Text Position</param>
        /// <param name="SourceRectangle"></param>
        /// <param name="color">Text color</param>
        void DrawText(string Text, Vector2 Position, Rectangle SourceRectangle, Color color)
        {
            FindCharacter(Text, color);

            for (int i = 0; i < Characters.Count; i++)
                if (Characters[i].Position.X + Position.X + Characters[i].Size.X < SourceRectangle.X + SourceRectangle.Width && Characters[i].Position.X + Position.X > SourceRectangle.X)
                    spriteBatch.Draw(Characters[i].Texture, Characters[i].Position + Position, Characters[i].Color);
        }

        /// <summary>
        /// Draws a static text that does not change in real time. This is the fastest and best method to draw a large amount of text on the screen.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Position">Text position</param>
        /// <param name="color">Text color</param>
        public void DrawStaticText(string Text, Vector2 Position, Color color)
        {
            if (ConvertStaticTextToTexture)
            {
                Vector2 TextSize = MeasureString(Text);

                CombineTextures.Begin(Graphics, new Vector2(TextSize.X, ((IndexesOfNewLine.Count + 1) * NewLineHeight)));
                spriteBatch.Begin();
                DrawText(Text, Vector2.Zero, Color.White);
                spriteBatch.End();
                CombineTextures.End();

                StaticText = CombineTextures.GetFinalTexture();

                ConvertStaticTextToTexture = false;
            }
            spriteBatch.Begin();
            spriteBatch.Draw(StaticText, Position, color);
            spriteBatch.End();
        }

        /// <summary>
        /// Draws a dynamic text that changes in real time. This method drops performance dramatically when drawing a large amount of text on the screen.
        /// Use this method only when wanting to draw small amount of text on the screen.
        /// </summary>
        /// <param name="Text">Text string</param>
        /// <param name="Position">Text position</param>
        /// <param name="color">Text color</param>
        public void DrawDynamicText(string Text, Vector2 Position, Color color)
        {
            DrawText(Text, Position, color);
        }

        /// <summary>
        /// Draws a specific rectangle or part of a dynamic text that changes in real time. This method drops performance dramatically when drawing a large amount of text on the screen.
        /// Use this method only when wanting to draw small amount of text on the screen.
        /// </summary>
        /// <param name="Text">Text string</param>
        /// <param name="Position">Text position</param>
        /// <param name="SourceRectangle">Set part of the text that want to be drawn</param>
        /// <param name="color">Text color</param>
        public void DrawDynamicText(string Text, Vector2 Position, Rectangle SourceRectangle, Color color)
        {
            DrawText(Text, Position, SourceRectangle, color);
        }
    }
}*/