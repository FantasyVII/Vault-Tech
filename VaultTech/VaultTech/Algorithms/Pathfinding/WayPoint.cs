/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 17/March/2014
 * Date Moddified :- 24/January/2016
 * </Copyright>
 */

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using VaultTech.Physics;

namespace VaultTech.Algorithms.Pathfinding
{
    public class Waypoint
    {
        public int WayPointIndex;
        public bool ReachedDestination;

        public void MoveTo(GameTime gameTime, PhysicsObject physicsObject, Vector2 Destination)
        {
            float Distance = Vector2.Distance(physicsObject.Position, Destination);
            Vector2 dir = Destination - physicsObject.Position;

            if (Distance > Math.Ceiling(physicsObject.Speed))
            {
                dir.Normalize();
                physicsObject.Position += dir * (float)(physicsObject.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }

        public void MoveTo(GameTime gameTime, PhysicsObject physicsObject, List<Vector2> DestinationWaypoint)
        {
            if (DestinationWaypoint.Count > 0)
            {
                if (!ReachedDestination)
                {
                    float Distance = Vector2.Distance(physicsObject.Position, DestinationWaypoint[WayPointIndex]);
                    Vector2 Direction = DestinationWaypoint[WayPointIndex] - physicsObject.Position;
                    Direction.Normalize();

                    if (Distance > Direction.Length())
                        physicsObject.Position += Direction * (float)(physicsObject.Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                    else
                    {
                        if (WayPointIndex >= DestinationWaypoint.Count - 1)
                        {
                            physicsObject.Position += Direction;
                            ReachedDestination = true;
                        }
                        else
                            WayPointIndex++;
                    }
                }
            }
        }
    }
}