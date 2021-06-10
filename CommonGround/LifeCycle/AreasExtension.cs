using CommonGround.Data;
using CommonGround.GUI;
using CommonGround.Manager;
using ICities;
using KianCommons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonGround.LifeCycle {
    public class AreasExtension : AreasExtensionBase{
        public override void OnUnlockArea(int x, int z) {
            Log.Debug("OnUnlockArea");
            if(ModSettings.settings.preset == AreaPreset.AdjacentToPurchased) {
                TerrainDetailManager.ApplyTerrainDetail();
            }
        }
    }
}
