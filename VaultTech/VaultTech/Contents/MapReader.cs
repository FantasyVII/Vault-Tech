/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 16/March/2014
 * Date Moddified :- 15/March/2015
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

using VaultTech.Physics;
using VaultTech.Graphics.MapContent;

namespace VaultTech.Contents
{
    class MapReader
    {
        GraphicsDeviceManager Graphics;

        Map map;

        public MapReader(Map map)
        {
            this.map = map;
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        /// <summary>
        /// Load from the Content folder.
        /// </summary>
        /// <param name="MapFilePath"></param>
        public void LoadContent(string MapFilePath)
        {
            MapFilePath = FileManager.ContentFolder + MapFilePath;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(MapFilePath);

            LoadMapData(xmlDoc);
            LoadCollitionLayer(xmlDoc);
            LoadLayers(xmlDoc);
            LoadTileSheets(xmlDoc, false);
        }

        /// <summary>
        /// Load from compressed file. Content.data
        /// </summary>
        /// <param name="MapFilePath"></param>
        public void LoadContentFromArchive(string MapFilePath)
        {
            Stream MapFileStream;
            try
            {
                MapFileStream = FileManager.GetFileStreamFromArchive(MapFilePath);

                if (MapFileStream == null)
                    throw new Exception("Could not find file \"" + FileManager.ContentFolder + FileManager.SourceArchiveFileName + "/" + MapFilePath + "\"");
            }
            catch (Exception ex) { throw ex; }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(MapFileStream);

            MapFileStream.Dispose();
            FileManager.Dispose();

            LoadMapData(xmlDoc);
            LoadCollitionLayer(xmlDoc);
            LoadLayers(xmlDoc);
            LoadTileSheets(xmlDoc, true);
        }

        void LoadMapData(XmlDocument xmlDoc)
        {
            map.Name = xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("Name").Value.ToString();

            map.tileBank.TileSize.X = int.Parse(xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("TileWidth").Value);
            map.tileBank.TileSize.Y = int.Parse(xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("TileHight").Value);

            map.ArraySize.X = int.Parse(xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("Width").Value);
            map.ArraySize.Y = int.Parse(xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("Height").Value);

            if (xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("RenderMode") != null)
            {
                if (xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("RenderMode").Value == "SingleTile")
                    map.renderMode = Map.RenderMode.SingleTile;
                else if (xmlDoc.SelectSingleNode("MapData/Map").Attributes.GetNamedItem("RenderMode").Value == "LargeTexture")
                    map.renderMode = Map.RenderMode.LargeTexture;
            }

            map.Size = map.ArraySize * map.tileBank.TileSize;
        }

        void LoadCollitionLayer(XmlDocument xmlDoc)
        {
            if (xmlDoc.SelectSingleNode("MapData/Layers/CollisionLayer") != null)
            {
                int[,] TempArray = new int[(int)map.ArraySize.X, (int)map.ArraySize.Y];
                int y = 0;

                foreach (XmlNode node in xmlDoc.SelectNodes("MapData/Layers/CollisionLayer").Item(0).ChildNodes)
                {
                    string[] temp = node.InnerText.Split(',');

                    for (int x = 0; x < temp.Length; x++)
                    {
                        TempArray[x, y] = int.Parse(temp[x]);

                        if (TempArray[x, y] == 1)
                            map.CollisionObjects.Add(new PhysicsObject(new Vector2(x, y) * map.tileBank.TileSize, map.tileBank.TileSize));
                    }
                    y++;
                }
            }
        }

        void LoadLayers(XmlDocument xmlDoc)
        {
            int LayerIndex = 0;

            foreach (XmlNode node in xmlDoc.SelectNodes("MapData/Layers/Layer"))
            {
                map.Layers.Add(new Layer(map));

                int y = 0;

                foreach (XmlNode InnerNode in node.ChildNodes)
                {
                    string[] temp = InnerNode.InnerText.Split(',');

                    for (int x = 0; x < temp.Length; x++)
                        map.Layers[LayerIndex].Array[x, y] = int.Parse(temp[x]);

                    y++;
                }
                LayerIndex++;
            }
        }

        /// <summary>
        /// Load map textures from XML file like tile path, size, ID and location.
        /// </summary>
        void LoadTileSheets(XmlDocument xmlDoc, bool LoadFromArchive)
        {
            foreach (XmlNode node in xmlDoc.SelectNodes("MapData/TileSheets").Item(0).ChildNodes)
            {
                if (LoadFromArchive)
                {
                    string TileSheetPath = node.Attributes.GetNamedItem("Path").Value;

                    MemoryStream TileSheetStream;
                    try
                    {
                        TileSheetStream = FileManager.GetFileMemoryStreamFromArchive(TileSheetPath);

                        if (TileSheetStream == null)
                            throw new Exception("Could not find file \"" + FileManager.ContentFolder + FileManager.SourceArchiveFileName + "/" + TileSheetPath + "\"");
                    }
                    catch (Exception ex) { throw ex; }

                    map.tileBank.TileSheets.Add(new TileSheet(Texture2D.FromStream(Graphics.GraphicsDevice, TileSheetStream), TileSheetPath));

                    TileSheetStream.Dispose();
                    FileManager.Dispose();
                }
                else
                {
                    string TileSheetPath = node.Attributes.GetNamedItem("Path").Value;

                    try { map.tileBank.TileSheets.Add(new TileSheet(StreamTexture.LoadTextureFromStream(Graphics, TileSheetPath), TileSheetPath)); }
                    catch (Exception ex) { throw ex; }
                }
            }
        }
    }
}