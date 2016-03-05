/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 16/March/2014
 * Date Moddified :- 5/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.Graphics.MapContent
{
    /// <summary>
    /// This class is responsible for taking the tile sheet and giving an ID number for each tile in the tile sheet.
    /// </summary>
    public class TileBank
    {
        /// <summary>
        /// Single tile size in the tile sheet.
        /// </summary>
        public Vector2 TileSize;
        /// <summary>
        /// List of all the tile sheets.
        /// </summary>
        internal List<TileSheet> TileSheets = new List<TileSheet>();
        /// <summary>
        /// List of all the tiles in the tile sheets.
        /// </summary>
        internal List<Tile> Tiles = new List<Tile>();

        internal void LoadContent()
        {
            AddTiles();
        }

        /// <summary>
        /// This function will add a new tile to the Tiles list with a unique ID number. 
        /// It also adds the position and size of the tile from the tile sheet.
        /// </summary>
        void AddTiles()
        {
            int TileID = -1;

            for (int i = 0; i < TileSheets.Count; i++)
            {
                for (int y = 0; y < TileSheets[i].Texture.Height / TileSize.X; y++)
                {
                    for (int x = 0; x < TileSheets[i].Texture.Width / TileSize.Y; x++)
                    {
                        TileID++;

                        Rectangle SourceRectangle = new Rectangle((int)(x * TileSize.X), (int)(y * TileSize.Y), (int)TileSize.X, (int)TileSize.Y);

                        Tiles.Add(new Tile(TileID, SourceRectangle, i));
                    }
                }
            }
        }
    }
}