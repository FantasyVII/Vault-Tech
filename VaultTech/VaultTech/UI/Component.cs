/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 22/April/2014
 * Date Moddified :-04/May/2015
 * </Copyright>
 */

using System;
using System.IO;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using VaultTech.Graphics.FontRenderer;
using VaultTech.Contents;

namespace VaultTech.UI
{
    public class Component
    {
        protected GraphicsDeviceManager Graphics;
        protected SpriteBatch spriteBatch;
        SpriteBatch CreateTextureSpriteBatch;

        internal ComponentTexture CurrentComponentTexture, NormalComponentTexture, HoveredComponentTexture, PressedComponentTexture;
        internal bool UpdateTexturePositionOnce;

        internal BorderThickness borderThickness;
        internal Vector2 CornerSize;

        public Vector2 Position { get; set; }
        public Rectangle Rectangle { get; set; }

        Vector2 DefaultSize;

        public Vector2 Size
        {
            get
            {
                if (CurrentComponentTexture != null)
                {
                    if (CurrentComponentTexture.RendererMode == ComponentTexture.RenderMode.NoneScalable)
                        return new Vector2(CurrentComponentTexture.OriginalTexture.Width, CurrentComponentTexture.OriginalTexture.Height);

                    else if (CurrentComponentTexture.RendererMode == ComponentTexture.RenderMode.Scalable)
                        return CurrentComponentTexture.Size;

                    else if (CurrentComponentTexture.RendererMode == ComponentTexture.RenderMode.ScalableRealTime)
                        return DefaultSize;
                }
                return DefaultSize;
            }

            set { DefaultSize = value; }
        }

        internal FontRenderer fontRenderer;
        internal bool ResizeComponentSizeToTextSize;
        public string Text { get; set; }
        public enum Alignment { Default, Left, Center, Right };
        public Alignment TextAlignment { get; set; }
        internal bool DisableVerticalAlighment;
        public enum Renderer { Dynamic, Static };
        public Renderer TextRenderer { get; set; }
        internal Vector2 TextPosition, TextOffsetPosition;
        internal int TotalInvisibleLettersLength;
        public Color TextColor;
        internal string OldText;
        internal bool DidTextChanged = true;

        public bool Hovered { get; set; }
        public bool Pressed { get; set; }
        public bool Released { get; set; }

        internal bool DisableMouseInteraction;

        RasterizerState rasterizerState;
        Rectangle CurrentScissorRectangle;
        Rectangle ScissorRectangle { get; set; }

        internal Component()
        {
            UpdateTexturePositionOnce = false;

            fontRenderer = new FontRenderer();
            ResizeComponentSizeToTextSize = false;
            TotalInvisibleLettersLength = 0;

            DisableVerticalAlighment = false;
            rasterizerState = new RasterizerState() { ScissorTestEnable = true };
        }

        internal virtual void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
            CreateTextureSpriteBatch = new SpriteBatch(Graphics.GraphicsDevice);

            fontRenderer.Initialize(Graphics);
        }

        void LoadStyleFile(Stream ContentStream, string XmlPath, string ComponentNodeNameInXml, bool LoadFromArchive)
        {
            XmlDocument xmlDoc = new XmlDocument();

            if (LoadFromArchive)
            {
                xmlDoc.Load(ContentStream);
                ContentStream.Dispose();
                FileManager.Dispose();
            }
            else
                xmlDoc.Load(FileManager.ContentFolder + XmlPath);

            if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures") != null)
            {
                if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/NormalTexture") != null)
                {
                    NormalComponentTexture = new ComponentTexture();

                    if (LoadFromArchive)
                    {
                        MemoryStream TextureStream;
                        try
                        {
                            string TexturePath = xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/NormalTexture").Attributes.GetNamedItem("Path").Value;
                            TextureStream = FileManager.GetFileMemoryStreamFromArchive(TexturePath);
                            NormalComponentTexture.OriginalTexture = Texture2D.FromStream(Graphics.GraphicsDevice, TextureStream);

                            if (ContentStream == null)
                                throw new Exception("Could not find file \"" + FileManager.ContentFolder + FileManager.SourceArchiveFileName + "/" + XmlPath + "\"");
                            else if (TextureStream == null)
                                throw new Exception("Could not find file \"" + FileManager.ContentFolder + FileManager.SourceArchiveFileName + "/" + TexturePath + "\"");
                        }
                        catch (Exception ex) { throw ex; }

                        TextureStream.Dispose();
                        FileManager.Dispose();
                    }
                    else
                        NormalComponentTexture.OriginalTexture = StreamTexture.LoadTextureFromStream(Graphics, xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/NormalTexture").Attributes.GetNamedItem("Path").Value);

                    NormalComponentTexture.Color = new Color(int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/NormalTextureColor").Attributes.GetNamedItem("Red").Value),
                                                    int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/NormalTextureColor").Attributes.GetNamedItem("Green").Value),
                                                    int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/NormalTextureColor").Attributes.GetNamedItem("Blue").Value),
                                                    int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/NormalTextureColor").Attributes.GetNamedItem("Alpha").Value));

                    NormalComponentTexture.Initialize(Graphics);
                }

                if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/HoveredTexture") != null)
                {
                    HoveredComponentTexture = new ComponentTexture();

                    if (LoadFromArchive)
                    {
                        MemoryStream TextureStream;
                        try
                        {
                            string TexturePath = xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/HoveredTexture").Attributes.GetNamedItem("Path").Value;
                            TextureStream = FileManager.GetFileMemoryStreamFromArchive(TexturePath);
                            HoveredComponentTexture.OriginalTexture = Texture2D.FromStream(Graphics.GraphicsDevice, TextureStream);

                            if (TextureStream == null)
                                throw new Exception("Could not find file \"" + FileManager.ContentFolder + FileManager.SourceArchiveFileName + "/" + TexturePath + "\"");
                        }
                        catch (Exception ex) { throw ex; }

                        TextureStream.Dispose();
                        FileManager.Dispose();
                    }
                    else
                        HoveredComponentTexture.OriginalTexture = StreamTexture.LoadTextureFromStream(Graphics, xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/HoveredTexture").Attributes.GetNamedItem("Path").Value);

                    HoveredComponentTexture.Color = new Color(int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/HoveredTextureColor").Attributes.GetNamedItem("Red").Value),
                                                    int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/HoveredTextureColor").Attributes.GetNamedItem("Green").Value),
                                                    int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/HoveredTextureColor").Attributes.GetNamedItem("Blue").Value),
                                                    int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/HoveredTextureColor").Attributes.GetNamedItem("Alpha").Value));

                    HoveredComponentTexture.Initialize(Graphics);
                }

                if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/PressedTexture") != null)
                {
                    PressedComponentTexture = new ComponentTexture();

                    if (LoadFromArchive)
                    {
                        MemoryStream TextureStream;
                        try
                        {
                            string TexturePath = xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/PressedTexture").Attributes.GetNamedItem("Path").Value;
                            TextureStream = FileManager.GetFileMemoryStreamFromArchive(TexturePath);
                            PressedComponentTexture.OriginalTexture = Texture2D.FromStream(Graphics.GraphicsDevice, TextureStream);

                            if (TextureStream == null)
                                throw new Exception("Could not find file \"" + FileManager.ContentFolder + FileManager.SourceArchiveFileName + "/" + TexturePath + "\"");
                        }
                        catch (Exception ex) { throw ex; }

                        TextureStream.Dispose();
                        FileManager.Dispose();
                    }
                    else
                        PressedComponentTexture.OriginalTexture = StreamTexture.LoadTextureFromStream(Graphics, xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/PressedTexture").Attributes.GetNamedItem("Path").Value);

                    PressedComponentTexture.Color = new Color(int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/PressedTextureColor").Attributes.GetNamedItem("Red").Value),
                                                     int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/PressedTextureColor").Attributes.GetNamedItem("Green").Value),
                                                     int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/PressedTextureColor").Attributes.GetNamedItem("Blue").Value),
                                                     int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Textures/PressedTextureColor").Attributes.GetNamedItem("Alpha").Value));

                    PressedComponentTexture.Initialize(Graphics);
                }
            }

            if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Rendering") != null)
            {
                if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Rendering").Attributes.GetNamedItem("Mode").Value == "None Scalable")
                {
                    if (NormalComponentTexture != null)
                        NormalComponentTexture.RendererMode = ComponentTexture.RenderMode.NoneScalable;

                    if (HoveredComponentTexture != null)
                        HoveredComponentTexture.RendererMode = ComponentTexture.RenderMode.NoneScalable;

                    if (PressedComponentTexture != null)
                        PressedComponentTexture.RendererMode = ComponentTexture.RenderMode.NoneScalable;
                }
                else if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Rendering").Attributes.GetNamedItem("Mode").Value == "Scalable")
                {
                    if (NormalComponentTexture != null)
                        NormalComponentTexture.RendererMode = ComponentTexture.RenderMode.Scalable;

                    if (HoveredComponentTexture != null)
                        HoveredComponentTexture.RendererMode = ComponentTexture.RenderMode.Scalable;

                    if (PressedComponentTexture != null)
                        PressedComponentTexture.RendererMode = ComponentTexture.RenderMode.Scalable;
                }
                else if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Rendering").Attributes.GetNamedItem("Mode").Value == "Scalable RealTime")
                {
                    if (NormalComponentTexture != null)
                        NormalComponentTexture.RendererMode = ComponentTexture.RenderMode.ScalableRealTime;

                    if (HoveredComponentTexture != null)
                        HoveredComponentTexture.RendererMode = ComponentTexture.RenderMode.ScalableRealTime;

                    if (PressedComponentTexture != null)
                        PressedComponentTexture.RendererMode = ComponentTexture.RenderMode.ScalableRealTime;
                }
            }

            if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Dimensions") != null)
            {
                borderThickness = new BorderThickness(int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Dimensions/BorderThickness").Attributes.GetNamedItem("Top").Value),
                                                        int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Dimensions/BorderThickness").Attributes.GetNamedItem("Right").Value),
                                                        int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Dimensions/BorderThickness").Attributes.GetNamedItem("Bottom").Value),
                                                        int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Dimensions/BorderThickness").Attributes.GetNamedItem("Left").Value));

                CornerSize = new Vector2(int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Dimensions/CornerSize").Attributes.GetNamedItem("X").Value),
                                          int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Dimensions/CornerSize").Attributes.GetNamedItem("Y").Value));


                if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Dimensions/DefaultSize") != null)
                {
                    if (Size.X <= 0 || Size.Y <= 0)
                        DefaultSize = new Vector2(int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Dimensions/DefaultSize").Attributes.GetNamedItem("X").Value),
                                                    int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Dimensions/DefaultSize").Attributes.GetNamedItem("Y").Value));
                }
            }

            if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text") != null)
            {
                string alignment = "";

                // if (LoadFromArchive)
                //fontRenderer.LoadContentFromArchive(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text/Font").Attributes.GetNamedItem("Path").Value);
                //else
                fontRenderer.LoadContent(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text/Font").Attributes.GetNamedItem("Path").Value);

                if (xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text/DefaultText") != null)
                {
                    if (String.IsNullOrEmpty(Text))
                        Text = xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text/DefaultText").Attributes.GetNamedItem("Value").Value;
                }

                if (TextAlignment == Alignment.Default)
                {
                    alignment = xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text/DefaultAlignment").Attributes.GetNamedItem("Value").Value;

                    if (alignment.ToLowerInvariant() == "left")
                        TextAlignment = Alignment.Left;

                    if (alignment.ToLowerInvariant() == "center")
                        TextAlignment = Alignment.Center;

                    if (alignment.ToLowerInvariant() == "right")
                        TextAlignment = Alignment.Right;
                }

                TextOffsetPosition = new Vector2(int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text/OffsetPosition").Attributes.GetNamedItem("X").Value),
                                                  int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text/OffsetPosition").Attributes.GetNamedItem("Y").Value));

                if (TextColor == null)
                    TextColor = new Color(int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text/DefaultTextColor").Attributes.GetNamedItem("Red").Value),
                                            int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text/DefaultTextColor").Attributes.GetNamedItem("Green").Value),
                                            int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text/DefaultTextColor").Attributes.GetNamedItem("Blue").Value),
                                            int.Parse(xmlDoc.SelectSingleNode(ComponentNodeNameInXml + "/Text/DefaultTextColor").Attributes.GetNamedItem("Alpha").Value));
            }
        }

        internal virtual void LoadContent(string StyleFilePath, string ComponentNodeNameInXml)
        {
            LoadStyleFile(null, StyleFilePath, ComponentNodeNameInXml, false);
        }

        internal virtual void LoadContentFromArchive(string StyleFilePath, string ComponentNodeNameInXml)
        {
            Stream ContentStream;
            try
            {
                ContentStream = FileManager.GetFileStreamFromArchive(StyleFilePath);

                if (ContentStream == null)
                    throw new Exception("Could not find file \"" + FileManager.ContentFolder + FileManager.SourceArchiveFileName + "/" + StyleFilePath + "\"");
            }
            catch (Exception ex) { throw ex; }

            LoadStyleFile(ContentStream, StyleFilePath, ComponentNodeNameInXml, true);
            FileManager.Dispose();
        }

        internal virtual void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            if (Size.X <= 0 || Size.Y <= 0)
                throw new Exception("Components size cannot be less or equal than zero");

            fontRenderer.UpdateOnce(spriteBatch);

            if (NormalComponentTexture != null)
                NormalComponentTexture.CreateTexture(spriteBatch, Vector2.Zero, Size, CornerSize, borderThickness);

            if (HoveredComponentTexture != null)
                HoveredComponentTexture.CreateTexture(spriteBatch, Vector2.Zero, Size, CornerSize, borderThickness);

            if (PressedComponentTexture != null)
                PressedComponentTexture.CreateTexture(spriteBatch, Vector2.Zero, Size, CornerSize, borderThickness);

            CurrentComponentTexture = NormalComponentTexture;

            if (CurrentComponentTexture.RendererMode == ComponentTexture.RenderMode.Scalable)
            {
                Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

                if (NormalComponentTexture != null)
                    NormalComponentTexture.Size = this.Size;

                if (HoveredComponentTexture != null)
                    HoveredComponentTexture.Size = this.Size;

                if (PressedComponentTexture != null)
                    PressedComponentTexture.Size = this.Size;
            }
        }

        internal virtual void Update(GameTime gameTime)
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

            if (CurrentComponentTexture.RendererMode == ComponentTexture.RenderMode.ScalableRealTime)
            {
                if (NormalComponentTexture != null)
                    NormalComponentTexture.Size = this.Size;

                if (HoveredComponentTexture != null)
                    HoveredComponentTexture.Size = this.Size;

                if (PressedComponentTexture != null)
                    PressedComponentTexture.Size = this.Size;
            }

            if (NormalComponentTexture != null)
                NormalComponentTexture.Position = this.Position;

            if (HoveredComponentTexture != null)
                HoveredComponentTexture.Position = this.Position;

            if (PressedComponentTexture != null)
                PressedComponentTexture.Position = this.Position;

            if (!DisableMouseInteraction)
            {
                if (Rectangle.Intersects(MouseCursor.rectangle))
                {
                    Hovered = true;

                    if (HoveredComponentTexture != null)
                        CurrentComponentTexture = HoveredComponentTexture;
                }
                else
                {
                    Hovered = false;

                    if (NormalComponentTexture != null)
                        CurrentComponentTexture = NormalComponentTexture;
                }

                if (Hovered && MouseCursor.CurrentMouseState.LeftButton == ButtonState.Pressed && MouseCursor.LastMouseState.LeftButton != ButtonState.Pressed)
                    Pressed = true;

                if (Pressed)
                    if (PressedComponentTexture != null)
                        CurrentComponentTexture = PressedComponentTexture;

                if (MouseCursor.LastMouseState.LeftButton == ButtonState.Pressed && MouseCursor.CurrentMouseState.LeftButton == ButtonState.Released)
                    Released = true;
            }
            CurrentComponentTexture.Update();

            ScissorRectangle = new Rectangle((int)Position.X + 1, (int)Position.Y + 1, (int)Size.X - 1, (int)Size.Y - 1);
        }

        internal virtual void Draw()
        {
            CurrentComponentTexture.Draw();
        }

        internal virtual void DrawText()
        {
            if (TextRenderer == Renderer.Dynamic)
                DrawDynamicText();
            else
                DrawStaticText();




            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, rasterizerState);
            CurrentScissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
            spriteBatch.GraphicsDevice.ScissorRectangle = ScissorRectangle;

            fontRenderer.Draw(Text, TextPosition, TextColor);

            spriteBatch.GraphicsDevice.ScissorRectangle = CurrentScissorRectangle;
            spriteBatch.End();
        }
    
        void SetTextAlignment()
        {
            if (TextAlignment == Alignment.Left)
                if (DisableVerticalAlighment)
                    TextPosition = new Vector2(Position.X + borderThickness.Left, Position.Y) + TextOffsetPosition;
                else
                    TextPosition = new Vector2(Position.X + borderThickness.Left, Position.Y + (Size.Y / 2) - (fontRenderer.FontSize / 2)) + TextOffsetPosition;

            if (TextAlignment == Alignment.Center)
                if (DisableVerticalAlighment)
                    TextPosition = new Vector2(Position.X + (Size.X / 2) - (fontRenderer.FontSize / 2) - (borderThickness.Left / 2), Position.Y) + TextOffsetPosition;
                else
                    TextPosition = new Vector2(Position.X + (Size.X / 2) - (fontRenderer.FontSize / 2) - (borderThickness.Left / 2), Position.Y + (Size.Y / 2) - (fontRenderer.FontSize / 2)) + TextOffsetPosition;

            if (TextAlignment == Alignment.Right)
                if (DisableVerticalAlighment)
                    TextPosition = new Vector2(Position.X + Size.X - (fontRenderer.FontSize) - borderThickness.Left, Position.Y) + TextOffsetPosition;
                else
                    TextPosition = new Vector2(Position.X + Size.X - (fontRenderer.FontSize) - borderThickness.Left, Position.Y + (Size.Y / 2) - (fontRenderer.FontSize / 2)) + TextOffsetPosition;
        }
        /*
        void ResizeComponentToTextSize()
        {
            if (ResizeComponentSizeToTextSize)
            {
                if (spriteFont.MeasureString(Text).X > Size.X + borderThickness.Left)
                {
                    Size.X = spriteFont.MeasureString(Text).X + borderThickness.Left + borderThickness.Left;
                    rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
                }
            }
        }*/
        /*
        void ClampTextToComponentSize()
        {
            if (!ResizeComponentSizeToTextSize)
            {
                if (fontRenderer.MeasureString(Text).X - (TotalInvisibleLettersLength * 11) >= Size.X - borderThickness.Left - 11)
                    TotalInvisibleLettersLength += 1;

                if (fontRenderer.MeasureString(Text).X - (TotalInvisibleLettersLength * 11) <= Size.X - borderThickness.Left - 11 && TotalInvisibleLettersLength > 0)
                    TotalInvisibleLettersLength -= 1;
            }
        }*/

        void DrawStaticText()
        {
            this.Text = Text;

            //ResizeComponentToTextSize();
            //ClampTextToComponentSize();
            SetTextAlignment();

            if (DidTextChanged)
            {
                OldText = Text;
                //fontRenderer.ConvertStaticTextToTexture = true;
                DidTextChanged = false;
            }

            if (Text != OldText)
                DidTextChanged = true;

            //fontRenderer.DrawStaticText(Text, TextPosition, TextColor);
        }

        void DrawDynamicText()
        {
            //ResizeComponentToTextSize();
            //ClampTextToComponentSize();
            SetTextAlignment();
        }
    }
}