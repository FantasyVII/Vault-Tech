/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 16/March/2014
 * Date Moddified :- 31/July/2015
 * </Copyright>
 */

using System;
using System.Text;
using System.Xml;
using System.IO;

using Microsoft.Xna.Framework;

namespace VaultTech
{
    public class Initialize
    {
        #region Engine variables
        Game game;
        GraphicsDeviceManager Graphics;

        public static Vector2 ScreenResolution;
        public Vector2 WindowPosition;
        public float FrameRate;
        public bool MultiSampling, Vsync, IsFrameRateCaped, IsWindowCentered;
        public string DisplayMode;
        #endregion

        string XMLFilePath, XMLFileName;

        public Initialize(Game game, GraphicsDeviceManager Graphics)
        {
            this.game = game;
            this.Graphics = Graphics;
        }

        /// <summary>
        /// Create an XML file of the engine settings. 
        /// This is done only if the XML file does not exist. 
        /// </summary>
        void CreateEngineSettingsXMLFile()
        {
            if (!Directory.Exists(@"Content\Engine\Config\"))
                Directory.CreateDirectory(@"Content\Engine\Config\");

            XmlTextWriter xmlWriter = new XmlTextWriter(@"Content\Engine\Config\EngineConfig.xml", Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;

            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("Engine");
            {
                xmlWriter.WriteStartElement("Video");
                {
                    xmlWriter.WriteStartElement("Resolution");
                    {
                        xmlWriter.WriteAttributeString("Width", Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width.ToString());
                        xmlWriter.WriteAttributeString("Hight", Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height.ToString());
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("RefreshRate");
                    {
                        xmlWriter.WriteAttributeString("Value", Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.RefreshRate.ToString());
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("DisplayMode");
                    {
                        xmlWriter.WriteAttributeString("Value", "Window");
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("WindowPosition");
                    {
                        xmlWriter.WriteAttributeString("Center", "true");
                        xmlWriter.WriteAttributeString("X", "0");
                        xmlWriter.WriteAttributeString("Y", "0");
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("MultiSampling");
                    {
                        xmlWriter.WriteAttributeString("Value", "true");
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("Vsync");
                    {
                        xmlWriter.WriteAttributeString("Value", "true");
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("FrameRate");
                    {
                        xmlWriter.WriteAttributeString("IsCaped", "false");
                        xmlWriter.WriteAttributeString("CapLimit", "60");
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Audio");
                {
                    xmlWriter.WriteStartElement("Master");
                    {
                        xmlWriter.WriteAttributeString("Value", "50");
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("Sound");
                    {
                        xmlWriter.WriteAttributeString("Value", "50");
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("Music");
                    {
                        xmlWriter.WriteAttributeString("Value", "50");
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        /// <summary>
        /// Load the engine settings from the XML file. 
        /// </summary>
        void LoadEngineSettingsXMLFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XMLFilePath + XMLFileName);

            if (xmlDoc.SelectSingleNode("Engine") != null)
            {
                if (xmlDoc.SelectSingleNode("Engine/Video") != null)
                {
                    ScreenResolution = new Vector2(int.Parse(xmlDoc.SelectSingleNode("Engine/Video/Resolution").Attributes.GetNamedItem("Width").Value),
                                                    int.Parse(xmlDoc.SelectSingleNode("Engine/Video/Resolution").Attributes.GetNamedItem("Hight").Value));

                    DisplayMode = xmlDoc.SelectSingleNode("Engine/Video/DisplayMode").Attributes.GetNamedItem("Value").Value;
                    IsWindowCentered = bool.Parse(xmlDoc.SelectSingleNode("Engine/Video/WindowPosition").Attributes.GetNamedItem("Center").Value);
                    WindowPosition = new Vector2(int.Parse(xmlDoc.SelectSingleNode("Engine/Video/WindowPosition").Attributes.GetNamedItem("X").Value),
                                                 int.Parse(xmlDoc.SelectSingleNode("Engine/Video/WindowPosition").Attributes.GetNamedItem("Y").Value));
                    MultiSampling = bool.Parse(xmlDoc.SelectSingleNode("Engine/Video/MultiSampling").Attributes.GetNamedItem("Value").Value);
                    Vsync = bool.Parse(xmlDoc.SelectSingleNode("Engine/Video/Vsync").Attributes.GetNamedItem("Value").Value);
                    IsFrameRateCaped = bool.Parse(xmlDoc.SelectSingleNode("Engine/Video/FrameRate").Attributes.GetNamedItem("IsCaped").Value);
                    FrameRate = float.Parse(xmlDoc.SelectSingleNode("Engine/Video/FrameRate").Attributes.GetNamedItem("CapLimit").Value);

                    if (FrameRate < 25)
                        FrameRate = 25;
                }
            }
        }

        public void ApplyChanges()
        {
            XMLFilePath = @"Content\Engine\Config\";
            XMLFileName = @"EngineConfig.xml";

            if (!File.Exists(XMLFilePath + XMLFileName))
            {
                CreateEngineSettingsXMLFile();
                LoadEngineSettingsXMLFile();
            }
            else
                LoadEngineSettingsXMLFile();

            Graphics.PreferredBackBufferWidth = (int)ScreenResolution.X;
            Graphics.PreferredBackBufferHeight = (int)ScreenResolution.Y;
            Graphics.PreferMultiSampling = MultiSampling;

            if (DisplayMode == "FullScreen")
            {
                Graphics.IsFullScreen = true;
            }
            else if (DisplayMode == "Window")
            {
                Graphics.IsFullScreen = false;
            }
            else if (DisplayMode == "Borderless Window")
            {
                Graphics.IsFullScreen = false;
                game.Window.IsBorderless = true;
            }

            Graphics.SynchronizeWithVerticalRetrace = Vsync;
            Graphics.ApplyChanges();

            if (IsWindowCentered)
                game.Window.Position = new Point((Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 2) - (Graphics.PreferredBackBufferWidth / 2),
                                            (int)(Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 2) - (Graphics.PreferredBackBufferHeight / 2));
            else
                game.Window.Position = new Point((int)WindowPosition.X, (int)WindowPosition.Y);

            game.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / FrameRate);
            game.IsFixedTimeStep = IsFrameRateCaped;


        }
    }
}



/*                        string tt = "";
                        List<DisplayMode> dmList = new List<DisplayMode>();
                        foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                        {
                            tt += " " + dm.AspectRatio.ToString();
                            dmList.Add(dm);
                        }                        
*/
