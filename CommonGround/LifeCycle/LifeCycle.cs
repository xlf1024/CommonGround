namespace CommonGround.LifeCycle {
    using System;
    using CitiesHarmony.API;
    using ICities;
    using KianCommons;
    using System.Diagnostics;
    using UnityEngine.SceneManagement;
    using CommonGround.Manager;

    public static class LifeCycle {
        public static string HARMONY_ID = "xlf1024.CommonGround";
        public static bool bHotReload = false;

        public static SimulationManager.UpdateMode UpdateMode => SimulationManager.instance.m_metaData.m_updateMode;
        public static LoadMode Mode => (LoadMode)UpdateMode;
        public static string Scene => SceneManager.GetActiveScene().name;

        public static bool Loaded;

        public static void Enable() {

            HarmonyHelper.EnsureHarmonyInstalled();
            HarmonyHelper.DoOnHarmonyReady(() => HarmonyUtil.InstallHarmony(HARMONY_ID));
            if (HelpersExtensions.InGameOrEditor)
                HotReload();

        }

        public static void Disable() {
            Unload();
        }

        public static void HotReload() {
            bHotReload = true;
            TerrainDetailManager.ApplyTerrainDetail();
        }

        public static void OnLevelLoaded(LoadMode mode) {
            TerrainDetailManager.ApplyTerrainDetail();
        }

        public static void Unload() {
            Log.Info("LifeCycle.Unload() called");
            HarmonyUtil.UninstallHarmony(HARMONY_ID);
            Loaded = false;
        }
    }
}
