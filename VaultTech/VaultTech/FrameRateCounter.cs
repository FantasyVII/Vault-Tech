/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 4/April/2014
 * Date Moddified :- 07/May/2015
 * </Copyright>
 */

using System;

using Microsoft.Xna.Framework;

namespace VaultTech
{
    /// <summary>
    /// FrameRateCounter class is responsible for measuring the engine frame rate. 
    /// </summary>
    public class FrameRateCounter
    {
        #region FrameRateCounter class variables
        /// <summary>
        /// Get the frame rate.
        /// </summary>
        public int FrameRate { get; private set; }

        int frameCounter;
        TimeSpan elapsedTime;
        #endregion

        /// <summary>
        /// Measure and update the frame rate.
        /// </summary>
        /// <param name="gameTime">MonoGame GameTime.</param>
        public void CalculateDrawMethodFrameRate(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                FrameRate = frameCounter;
                frameCounter = 0;
            }

            frameCounter++;
        }
    }
}