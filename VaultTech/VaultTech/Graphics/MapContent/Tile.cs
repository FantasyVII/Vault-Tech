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

namespace VaultTech.Graphics.MapContent
{
    /// <summary>
    /// This class is responsible for storing tile data.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Tile unique ID number.
        /// </summary>
        public int ID;
        /// <summary>
        /// Tile position and size in respect to tile sheet.
        /// </summary>
        public Rectangle SourceRectangle;
        /// <summary>
        /// Tile sheet ID number that this tile is using.
        /// </summary>
        public int TileSheetID;

        /// <summary>
        /// Constructor responsible for storing tile data.
        /// </summary>
        /// <param name="ID">Tile unique ID number</param>
        /// <param name="SourceRectangle">Tile position and size in respect to tile sheet.</param>
        /// <param name="TileSheetID">Tile sheet ID number that this tile is using.</param>
        public Tile(int ID, Rectangle SourceRectangle, int TileSheetID)
        {
            this.ID = ID;
            this.SourceRectangle = SourceRectangle;
            this.TileSheetID = TileSheetID;
        }
    }
}