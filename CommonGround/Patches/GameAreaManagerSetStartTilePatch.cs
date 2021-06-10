namespace CommonGround.Patches {
    using HarmonyLib;
    using JetBrains.Annotations;
    using KianCommons;
    using System;

    [UsedImplicitly]
    [HarmonyPatch(typeof(GameAreaManager), "SetStartTile")]
    class SetStartTilePatch {

        [HarmonyPatch(new Type[] { typeof(int) })]
        [HarmonyPostfix]
        static void Postfix1Arg() {
            Log.Debug("SetStartTile(int) postfix");
            if (GUI.ModSettings.settings.preset == Data.AreaPreset.StartTile) Manager.TerrainDetailManager.ApplyTerrainDetail();
        }


        [HarmonyPatch(new Type[] { typeof(int), typeof(int) })]
        [HarmonyPostfix]
        static void Postfix2Arg() {
            Log.Debug("SetStartTile(int,int) postfix");
            if (GUI.ModSettings.settings.preset == Data.AreaPreset.StartTile) Manager.TerrainDetailManager.ApplyTerrainDetail();
        }
    }
}