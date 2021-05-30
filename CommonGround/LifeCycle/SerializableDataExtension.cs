namespace CommonGround.LifeCycle
{
    using JetBrains.Annotations;
    using ICities;
    using KianCommons;
    using System;
    using CommonGround.GUI;
    using KianCommons.Serialization;

    [Serializable]
    public class CommonGroundState {
        public static CommonGroundState Instance;

        public string Version = typeof(CommonGroundState).VersionOf().ToString(3);
        public byte[] Data;
        public GameConfigT GameConfig;

        public static byte[] Serialize() {
            //Manager.ValidateAndHeal(false);
            Instance = new CommonGroundState {
                //Data = Manager.Serialize(),
                GameConfig = Settings.GameConfig,
            };

            return SerializationUtil.Serialize(Instance);
        }

        public static void Deserialize(byte[] data) {
            if (data == null) {
                Log.Debug($"CommonGroundState.Deserialize(data=null)");
                Instance = new CommonGroundState();
            } else {
                Log.Debug($"´CommonGroundState.Deserialize(data): data.Length={data?.Length}");
                Instance = SerializationUtil.Deserialize(data, default) as CommonGroundState;
                switch (Instance?.Version) { }
            }
            Settings.GameConfig = Instance.GameConfig;
            Settings.UpdateGameSettings();
            var version = new Version(Instance.Version);
            //Manager.Deserialize(Instance.Data, version);
        }

    }

    [UsedImplicitly]
    public class SerializableDataExtension
        : SerializableDataExtensionBase
    {
        private const string DATA_ID0 = "RoadTransitionManager_V1.0";
        private const string DATA_ID1 = "CommonGround_V1.0";
        private const string DATA_ID = "CommonGround_V2.0";

        public static int LoadingVersion;
        public override void OnLoadData()
        {
            byte[] data = serializableDataManager.LoadData(DATA_ID);
            if (data != null) {
                LoadingVersion = 2;
                CommonGroundState.Deserialize(data);
            } else {
                // convert to new version
                LoadingVersion = 1;
                data = serializableDataManager.LoadData(DATA_ID1)
                    ?? serializableDataManager.LoadData(DATA_ID0);
                //Manager.Deserialize(data, new Version(1, 0));
            }
        }

        public override void OnSaveData()
        {
            byte[] data = CommonGroundState.Serialize();
            serializableDataManager.SaveData(DATA_ID, data);
        }
    }
}
