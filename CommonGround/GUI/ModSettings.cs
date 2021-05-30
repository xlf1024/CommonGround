using ColossalFramework;
using ICities;
using KianCommons.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonGround.Manager;

namespace CommonGround.GUI {
    class ModSettings {
        public const string FILE_NAME = nameof(CommonGround);
        public static SavedBool SavedBool(string key, bool def) => new SavedBool(key, FILE_NAME, def, true);
        public static SavedInt SavedInt(string key, int def) => new SavedInt(key, FILE_NAME, def, true);


        public static readonly SavedInt TerrainDetailMinX = SavedInt(nameof(TerrainDetailMinX), 4);
        public static readonly SavedInt TerrainDetailMaxX = SavedInt(nameof(TerrainDetailMaxX), 4);
        public static readonly SavedInt TerrainDetailMinZ = SavedInt(nameof(TerrainDetailMinZ), 4);
        public static readonly SavedInt TerrainDetailMaxZ = SavedInt(nameof(TerrainDetailMaxZ), 4);
        internal static void OnSettingsUI(UIHelperBase helper) {
            helper.AddSavedClampedIntTextfield("minX", TerrainDetailMinX, 0, 8, TerrainDetailManager.ApplyTerrainDetail);
            helper.AddSavedClampedIntTextfield("maxX", TerrainDetailMaxX, 0, 8, TerrainDetailManager.ApplyTerrainDetail);
            helper.AddSavedClampedIntTextfield("minZ", TerrainDetailMinZ, 0, 8, TerrainDetailManager.ApplyTerrainDetail);
            helper.AddSavedClampedIntTextfield("maxZ", TerrainDetailMaxZ, 0, 8, TerrainDetailManager.ApplyTerrainDetail);
        }

    }
}
