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
                for (int y = 0; y < 9; y++) {
                    customCheckBoxes[x, y] = table.GetCell(y, x).AddUIComponent<UICheckBoxExt>();
                    //customCheckBoxes[x, y].RemoveUIComponent(customCheckBoxes[x, y].label);
                    UnityEngine.Object.Destroy(customCheckBoxes[x, y].label);
                    customCheckBoxes[x, y].isChecked = settings.custom[x, y];
                    customCheckBoxes[x, y].eventCheckChanged += (_, _) => OnSettingsChanged();
                    customCheckBoxes[x, y].width = 20f;
                }
            }
            if (settings.preset != AreaPreset.Custom) customGroupPanel.parent.Hide();

            var text = ((helper as UIHelper).self as UIComponent).AddUIComponent<UILabel>();
            text.text = "Note: while the game is loaded, only increasing the detailed area will have an effect.";

        }
        internal static void OnSettingsChanged() {
            settings.preset = (AreaPreset)presetDropDown.selectedIndex;
            if (settings.preset == AreaPreset.Custom) {
                customGroupPanel.parent.Show();
            } else {
                customGroupPanel.parent.Hide();
            }
            for (int x = 0; x < 9; x++) {
                for (int y = 0; y < 9; y++) {
                    settings.custom[x, y] = customCheckBoxes[x, y].isChecked;
                }
            }
            settings.Serialize();
            if (HelpersExtensions.InGameOrEditor) {
                TerrainDetailManager.ApplyTerrainDetail();
            }
        }
    }
}
