namespace CommonGround.Patches {
    using HarmonyLib;
    using JetBrains.Annotations;
    using KianCommons;

    [UsedImplicitly]
    [HarmonyPatch(typeof(TerrainManager), "SetDetailedPatch")]
    /*
     * SetDetailedPatch doesn't check whether the TerrainPatch was already detailed. This Prefix adds that check.
     * */
    class SetDetailedPatchPatch {
        static bool Prefix(int x, int z, ref bool __result, TerrainManager __instance) {
            int linearCoordinate = z * 9 + x;
            if(__instance.m_patches[linearCoordinate].m_simDetailIndex != 0) {
                //TerrainPatch is already detailed
                Log.Debug("patch (" + x + "|" + z + ") was already detailed.");
                __result = true;
                return false;
            }
            return true;
        }
    }
}