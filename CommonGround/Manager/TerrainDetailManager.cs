using ColossalFramework;
using KianCommons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CommonGround.GUI;
using CommonGround.Data;
using UnityEngine;

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
            var terrainManager = Singleton<TerrainManager>.instance;
            if (gameAreaManager.m_maxAreaCount < 81) gameAreaManager.m_maxAreaCount = 81;
            Log.Debug("Common ground: Applying terrain detail; preset: " + ModSettings.settings.preset.ToString());
            switch (ModSettings.settings.preset) {
                case AreaPreset.Custom: {
                        for (int x = 0; x < 9; x++) {
                            for (int z = 0; z < 9; z++) {
                                if (ModSettings.settings.custom[x, z]) {
                                    Log.Debug("detailing patch (" + x + "|" + z + ")");
                                    terrainManager.SetDetailedPatch(x, z);
                                }
                            }
                        }
                        break;
                    }
                case AreaPreset.StartTile: {
                        gameAreaManager.GetStartTile(out int x, out int z);
                        x += 2;
                        z += 2;
                        Log.Debug("detailing patch (" + x + "|" + z + ")");
                        terrainManager.SetDetailedPatch(x, z);
                        break;
                    }
                case AreaPreset.AdjacentToPurchased: {
                        for (int x = 0; x < 9; x++) {
                            for (int z = 0; z < 9; z++) {
                                for (int x2 = Mathf.Max(x - 1, 2); x2 <= x + 1 && x2 < 7; x2++) {
                                    for (int z2 = Mathf.Max(z - 1, 2); z2 <= z + 1 && z2 < 7; z2++) {
                                        if (gameAreaManager.IsUnlocked(x2-2, z2-2)) {//offset 81tiles to 25tiles
                                            Log.Debug("detailing patch (" + x + "|" + z + ")");
                                            terrainManager.SetDetailedPatch(x, z);
                                            //continue to x,z loops
                                            x2 = int.MaxValue - 1;//avoid overflow
                                            z2 = int.MaxValue - 1;
                                        }
                                    }
                                }
                            }
                        }
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
                            terrainManager.SetDetailedPatch(x, z);
                        }
                    }
                    break;
            }
        }
    }
}
