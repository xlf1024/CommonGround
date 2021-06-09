namespace CommonGround.Patches {
    using HarmonyLib;
    using JetBrains.Annotations;
    using KianCommons;
    using System;

    [UsedImplicitly]
    [HarmonyPatch(typeof(GameAreaManager), "SetStartTile")]
    class SetStartTilePatch {

        [HarmonyPatch(new Type[] { typeof(int) })]
        static void Postfix1Arg() {
            if (GUI.ModSettings.settings.preset == Data.AreaPreset.StartTile) Manager.TerrainDetailManager.ApplyTerrainDetail(); ;
        }


        [HarmonyPatch(new Type[] { typeof(int), typeof(int) })]
        static void Postfix2Arg() {
            if (GUI.ModSettings.settings.preset == Data.AreaPreset.StartTile) Manager.TerrainDetailManager.ApplyTerrainDetail(); ;
        }
    }
}