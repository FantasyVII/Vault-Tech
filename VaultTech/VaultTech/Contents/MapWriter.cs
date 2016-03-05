/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 13/January/2015
 * Date Moddified :- 18/January/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using VaultTech.Graphics.MapContent;

namespace VaultTech.Contents
{
    public class MapWriter
    {
        Map map;

        XmlTextWriter xmlWriter;

        public void WriteFile(Map map, string FileName)
        {
            this.map = map;

            xmlWriter = new XmlTextWriter(FileName + ".xml", Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;

            xmlWriter.WriteStartDocument();

            xmlWriter.WriteStartElement("MapData");
            {
                WriteMapData();
                WriteLayers();
                WriteTextures();
            }

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

        }

        void WriteMapData()
        {
            xmlWriter.WriteStartElement("Map");
            {
                xmlWriter.WriteAttributeString("Name", "4 way hero");

                xmlWriter.WriteAttributeString("Width", map.ArraySize.X.ToString());
                xmlWriter.WriteAttributeString("Hight", map.ArraySize.Y.ToString());

                xmlWriter.WriteAttributeString("TileWidth", map.tileBank.TileSize.X.ToString());
                xmlWriter.WriteAttributeString("TileHight", map.tileBank.TileSize.Y.ToString());
            }
            xmlWriter.WriteEndElement();
        }

        void WriteLayers()
        {
            xmlWriter.WriteStartElement("Layers");
            {
                WriteCollisionLayer();
                WriteLayer();
            }
            xmlWriter.WriteEndElement();
        }

        void WriteCollisionLayer()
        {
            int[,] CollisionArray = map.ConvertCollisionListTo2DArray();

            xmlWriter.WriteStartElement("CollisionLayer");
            {
                for (int y = 0; y < map.ArraySize.Y; y++)
                {
                    xmlWriter.WriteStartElement("Data");
                    {
                        for (int x = 0; x < map.ArraySize.X; x++)
                        {
                            if (x < map.ArraySize.X - 1)
                                xmlWriter.WriteString(CollisionArray[x, y] + ",");
                            else
                                xmlWriter.WriteString(CollisionArray[x, y].ToString());
                        }
                    }
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
        }

        void WriteLayer()
        {
            xmlWriter.WriteStartElement("Layer");
            {
                for (int y = 0; y < map.ArraySize.Y; y++)
                {
                    xmlWriter.WriteStartElement("Data");
                    {
                        for (int x = 0; x < map.ArraySize.X; x++)
                        {
                            if (x < map.ArraySize.X - 1)
                                xmlWriter.WriteString(map.Layers[0].Array[x, y] + ",");
                            else
                                xmlWriter.WriteString(map.Layers[0].Array[x, y].ToString());
                        }
                    }
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
        }

        void WriteTextures()
        {
            xmlWriter.WriteStartElement("TileSheets");
            {
                WriteTileSheets();
            }
            xmlWriter.WriteEndElement();
        }

        void WriteTileSheets()
        {
            for (int i = 0; i < map.tileBank.TileSheets.Count; i++)
            {
                xmlWriter.WriteStartElement("TileSheet");
                {
                    xmlWriter.WriteAttributeString("Path", map.tileBank.TileSheets[i].Path);
                }
                xmlWriter.WriteEndElement();
            }
        }
    }
}