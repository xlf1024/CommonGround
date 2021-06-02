using ColossalFramework.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CommonGround.Data {

    public class CommonGroundConfig {
        public const string FILE_NAME = "CommonGroundConfig.xml";

        [DefaultValue(AreaPreset.Square25)]
        public AreaPreset preset = AreaPreset.Square25;
        [XmlIgnore]
        public bool[,] custom = new bool[9, 9];
        [XmlArray("custom")]
        [DefaultValue(null)]
        public bool[][] SerializableCustom {
            get {
                return custom?.ToJagged();
            }
            set{    
                custom = value?.ToRectangular();
            }
        }


        // From LoadOrderMod
        public void Serialize() {
            XmlSerializer ser = new XmlSerializer(typeof(CommonGroundConfig));
            using (FileStream fs = new FileStream(Path.Combine(DataLocation.localApplicationData, FILE_NAME), FileMode.Create, FileAccess.Write)) {
                ser.Serialize(fs, this);
            }
        }

        public static CommonGroundConfig Deserialize() {
            try {
                XmlSerializer ser = new XmlSerializer(typeof(CommonGroundConfig));
                using (FileStream fs = new FileStream(Path.Combine(DataLocation.localApplicationData, FILE_NAME), FileMode.Open, FileAccess.Read)) {
                    var config = ser.Deserialize(fs) as CommonGroundConfig;
                    if (config.custom is null) {
                        config.custom = new bool[9, 9];
                    }
                    return config;
                }
            }
            catch {
                return null;
            }
        }
    }
    public static class ArrayExtension {
        public static T[][] ToJagged<T>(this T[,] rectangular) {
            var rowCount = rectangular.GetLength(0);
            var columnCount = rectangular.GetLength(1);
            var jagged = new T[rowCount][];
            for (int i = 0; i < rowCount; i++) {
                jagged[i] = new T[columnCount];
                for (int j = 0; j < columnCount; j++) {
                    jagged[i][j] = rectangular[i, j];
                }
            }
            return jagged;
        }
        public static T[,] ToRectangular<T>(this T[][] jagged) {
            var rowCount = jagged.Length;
            var columnCount = 0;
            for (int i = 0; i < rowCount; i++) {
                if (jagged[i].Length > columnCount) columnCount = jagged[i].Length;
            }
            var rectangular = new T[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++) {
                for (int j = 0; j < jagged[i].Length; j++) {
                    rectangular[i, j] = jagged[i][j];
                }
            }
            return rectangular;
        }
    }
}
