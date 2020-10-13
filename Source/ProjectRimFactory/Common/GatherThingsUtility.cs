﻿using System;
using System.Collections.Generic;
using System.Linq;
using ProjectRimFactory.Common;
using Verse;
using RimWorld;
namespace ProjectRimFactory {
    public static class GatherThingsUtility {
        /// <summary>
        /// A list of cells that might be appropriate for a PRF building to gather input items from.
        ///   If the PRF building has a CompPowerWorkSetting comp, it uses that, otherwise
        ///   it defaults to all adjacent cells around the building.
        /// </summary>
        /// <param name="building">A building. Probably a PRF building. Probably one that makes things.</param>
        public static IEnumerable<IntVec3> InputCells(this Building building) {
            return building.GetComp<CompPowerWorkSetting>()?.GetRangeCells() ?? GenAdj.CellsAdjacent8Way(building);
        }
        /// <summary>
        /// A list of items a PRF building might want to use as input resources, either on the ground,
        ///   in storage, or on coveyor belts.
        /// </summary>
        /// <returns>The items in cell <paramref name="c"/> for use.</returns>
        /// <param name="c">Cell</param>
        /// <param name="map">Map</param>
        public static IEnumerable<Thing> AllThingsInCellForUse(this IntVec3 c, Map map) {
            foreach (var t in map.thingGrid.ThingsListAt(c)) {
                if (t is Building && t is IThingHolder holder) {
                    if (holder.GetDirectlyHeldThings() is ThingOwner<Thing> owner) {
                        foreach (var moreT in owner.InnerListForReading) yield return moreT;
                    }
                } else if (t.def.category == ThingCategory.Item) {
                    yield return t;
                }
            }
        }
    }
}