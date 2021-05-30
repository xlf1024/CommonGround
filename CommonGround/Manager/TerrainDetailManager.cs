using ColossalFramework;
using KianCommons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CommonGround.GUI;

namespace CommonGround.Manager {
    public static class TerrainDetailManager {
        public static void ApplyTerrainDetail() {
            var simulationManager = Singleton<SimulationManager>.instance;
            if (Thread.CurrentThread != simulationManager.m_simulationThread) {
                simulationManager.AddAction(ApplyTerrainDetail);
                return;
            }
            var gameAreaManager = Singleton<GameAreaManager>.instance;
            if (gameAreaManager.m_maxAreaCount < 81) gameAreaManager.m_maxAreaCount = 81;
            Log.Debug("increasing terrain detail");
            for (int z = ModSettings.TerrainDetailMinZ.value; z <= ModSettings.TerrainDetailMaxZ.value; z++) {
                for (int x = ModSettings.TerrainDetailMinX.value; x <= ModSettings.TerrainDetailMaxX.value; x++) {
                    Log.Debug("detailing patch (" + x + "|" + z + ")");
                    Singleton<TerrainManager>.instance.SetDetailedPatch(x, z);
                }
            }
        }
    }
}
