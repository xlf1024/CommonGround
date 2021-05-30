namespace CommonGround.LifeCycle
{
    using System;
    using JetBrains.Annotations;
    using ICities;
    using CitiesHarmony.API;
    using KianCommons;
    using System.Diagnostics;

    public class CommonGroundMod : IUserMod
    {
        public static Version ModVersion => typeof(CommonGroundMod).Assembly.GetName().Version;
        public static string VersionString => ModVersion.ToString(2);
        public string Name => "Common Ground " + VersionString;
        public string Description => "make terrain in map editor and in unpurchased tiles behave like in purchased tiles in-game";

        [UsedImplicitly]
        public void OnEnabled()
        {
            LifeCycle.Enable();
        }

        [UsedImplicitly]
        public void OnDisabled()
        {
            LifeCycle.Disable();
        }

        [UsedImplicitly]
        public void OnSettingsUI(UIHelperBase helper) {
            GUI.ModSettings.OnSettingsUI(helper);
        }

    }
}
