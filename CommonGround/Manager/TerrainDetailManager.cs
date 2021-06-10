using ColossalFramework;
using KianCommons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CommonGround.GUI;
using CommonGround.Data;

namespace CommonGround.Manager {
    public static class TerrainDetailManager {
        public static void ApplyTerrainDetail() {
            var loadingManager = Singleton<LoadingManager>.instance;
            if (!loadingManager.m_loadingComplete || loadingManager.m_currentlyLoading) return;
            var simulationManager = Singleton<SimulationManager>.instance;
            if (Thread.CurrentThread != simulationManager.m_simulationThread) {
                simulationManager.AddAction(ApplyTerrainDetail);
                return;
            }
            var gameAreaManager = Singleton<GameAreaManager>.instance;
            if (gameAreaManager.m_maxAreaCount < 81) gameAreaManager.m_maxAreaCount = 81;
            Log.Debug("Common ground: Applying terrain detail; preset: " + ModSettings.settings.preset.ToString());
            switch (ModSettings.settings.preset) {
                case AreaPreset.Custom: {
                        for (int x = 0; x < 9; x++) {
                            for (int z = 0; z < 9; z++) {
                                if (ModSettings.settings.custom[x, z]) {
                                    Log.Debug("detailing patch (" + x + "|" + z + ")");
                                    Singleton<TerrainManager>.instance.SetDetailedPatch(x, z);
                                }
                            }
                        }
                        break;
                    }
                case AreaPreset.StartTile: {
                        Singleton<GameAreaManager>.instance.GetStartTile(out int x, out int z);
                        Log.Debug("detailing patch (" + x + "|" + z + ")");
                        Singleton<TerrainManager>.instance.SetDetailedPatch(x, z);
                        break;
                    }
                default:
                    int xMin;
                    int xMax;
                    int zMin;
                    int zMax;
                    switch (ModSettings.settings.preset) {
                        case AreaPreset.CenterTile:
                            xMin = 4;
                            xMax = 4;
                            zMin = 4;
                            zMax = 4;
                            break;
                        case AreaPreset.Square25:
                            xMin = 2;
                            xMax = 6;
                            zMin = 2;
                            zMax = 6;
                            break;
                        case AreaPreset.Square49:
                            xMin = 1;
                            xMax = 7;
                            zMin = 1;
                            zMax = 7;
                            break;
                        case AreaPreset.Square81:
                            xMin = 0;
                            xMax = 8;
                            zMin = 0;
                            zMax = 8;
                            break;
                        default:
                            xMin = 4;
                            xMax = 4;
                            zMin = 4;
                            zMax = 4;
                            break;
                    }
                    for (int x = xMin; x <= xMax; x++) {
                        for (int z = zMin; z <= zMax; z++) {
                            Log.Debug("detailing patch (" + x + "|" + z + ")");
                            Singleton<TerrainManager>.instance.SetDetailedPatch(x, z);
                        }
                    }
                    break;
            }
        }
    }
}
