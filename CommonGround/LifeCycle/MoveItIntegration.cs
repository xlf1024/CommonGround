using KianCommons;
using MoveItIntegration;
using System;
using System.Collections.Generic;
using KianCommons.Serialization;

namespace CommonGround.LifeCycle {

    public class MoveItIntegrationFactory : IMoveItIntegrationFactory {
        public MoveItIntegrationBase GetInstance() => new MoveItIntegration();
    }

    public class MoveItIntegration : MoveItIntegrationBase { 

        public override string ID => "CS.xlf1024.CommonGround";

        public override Version DataVersion => new Version(0, 0, 1);

        public override object Decode64(string base64Data, Version dataVersion) {
            Log.Debug($"MoveItIntegration.Decode64({base64Data},{dataVersion}) was called");
            if (base64Data == null || base64Data.Length == 0) return null;
            byte [] data = Convert.FromBase64String(base64Data);
            return SerializationUtil.Deserialize(data, dataVersion).LogRet("MoveItIntegration.Decode64 ->");
        }

        public override string Encode64(object record) {
            Log.Debug($"MoveItIntegration.Encode64({record}) was called");
            var data = SerializationUtil.Serialize(record);
            if (data == null || data.Length == 0) return null;
            return Convert.ToBase64String(data).LogRet("MoveItIntegration.Encode64 ->");
        }

        public override object Copy(InstanceID sourceInstanceID) {
            Log.Debug($"MoveItIntegration.Copy({sourceInstanceID.ToSTR()}) called");
            switch (sourceInstanceID.Type) {
                default:
                    Log.Debug("Unsupported integration");
                    return null;
            }
        }

        public override void Paste(InstanceID targetrInstanceID, object record, Dictionary<InstanceID, InstanceID> map) {
            string strRecord = record == null ? "null" : record.ToString();
            string strInstanceID = targetrInstanceID.ToSTR();
            Log.Debug($"MoveItIntegration.Paste({strInstanceID}, record:{strRecord}, map) was called");
            switch (targetrInstanceID.Type) {
                default:
                    Log.Debug("Unsupported integration");
                    break;
            }
        }


        public static void Paste(object record, Dictionary<InstanceID, InstanceID> map) { 
        }

    }
}
