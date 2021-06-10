using ColossalFramework;
using ICities;
using KianCommons.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonGround.Manager;
using ColossalFramework.UI;
using KianCommons.UI;
using KianCommons.UI.Table;
using KianCommons;
using UnityEngine;
using CommonGround.Data;

namespace CommonGround.GUI {
    class ModSettings {
        public const string FILE_NAME = nameof(CommonGround);
        public static CommonGroundConfig settings;
        private static UIDropDown presetDropDown;
        private static UIPanel customGroupPanel;
        private static UICheckBoxExt[,] customCheckBoxes = new UICheckBoxExt[9, 9];
        private static UIComponent settingsWindow = null;
        static ModSettings() {
            settings = CommonGroundConfig.Deserialize() ?? new CommonGroundConfig();
        }
        internal static void OnSettingsUI(UIHelperBase helper) {
            presetDropDown = helper.AddDropdown("always detailed area", Enum.GetNames(typeof(AreaPreset)), (int)settings.preset, (_) => { OnSettingsChanged(); }) as UIDropDown;

            var customGroup = helper.AddGroup("custom");
            customGroupPanel = (customGroup as UIHelper).self as UIPanel;
            var table = customGroupPanel.AddUIComponent<UITable>();
            table.Expand(9, 9);
            for (int x = 0; x < 9; x++) {
                for (int z = 0; z < 9; z++) {
                    customCheckBoxes[x, z] = table.GetCell(9-1-z, x).AddUIComponent<UICheckBoxExt>();
                    //customCheckBoxes[x, y].RemoveUIComponent(customCheckBoxes[x, y].label);
                    UnityEngine.Object.Destroy(customCheckBoxes[x, z].label);
                    customCheckBoxes[x, z].isChecked = settings.custom[x, z];
                    customCheckBoxes[x, z].eventCheckChanged += (_, _) => OnSettingsChanged();
                    customCheckBoxes[x, z].width = 20f;
                }
            }
            customGroup.AddButton("clear", () => {
                foreach(var customCheckBox in customCheckBoxes) {
                    customCheckBox.isChecked = false;
                }
                OnSettingsChanged();
            });
            if (settings.preset != AreaPreset.Custom) customGroupPanel.parent.Hide();

            var text = ((helper as UIHelper).self as UIComponent).AddUIComponent<UILabel>();
            text.text = "Note: while the game is loaded, only increasing the detailed area will have an effect.";

            //find settings window
            settingsWindow = (helper as UIHelper).self as UIComponent;
            while (settingsWindow.parent is not null && !settingsWindow.name.EndsWith("OptionsPanel")) {
                settingsWindow = settingsWindow.parent;
            }

            settingsWindow.eventVisibilityChanged += OnSettingsWindowVisibilityChanged;
        }

        internal static void OnSettingsWindowVisibilityChanged(UIComponent _, bool visible){
            // only apply any changes as soon as the settings are closed
            if (!visible) {
                Log.Debug("CommonGround: settings window closed");
                TerrainDetailManager.ApplyTerrainDetail();
            }
        }
        internal static void OnSettingsChanged() {
            Log.Debug("CommonGround: OnSettingsChanged");
            settings.preset = (AreaPreset)presetDropDown.selectedIndex;
            if (settings.preset == AreaPreset.Custom) {
                customGroupPanel.parent.Show();
            } else {
                customGroupPanel.parent.Hide();
            }
            for (int x = 0; x < 9; x++) {
                for (int z = 0; z < 9; z++) {
                    settings.custom[x, z] = customCheckBoxes[x, z].isChecked;
                }
            }
            settings.Serialize();
        }

        internal static void RemoveEventListeners() {
            settingsWindow.eventVisibilityChanged -= OnSettingsWindowVisibilityChanged;
        }
    }
}
