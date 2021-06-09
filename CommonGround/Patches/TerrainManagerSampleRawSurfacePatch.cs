namespace CommonGround.Patches {
    using HarmonyLib;
    using JetBrains.Annotations;
    using KianCommons;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    [UsedImplicitly]
    [HarmonyPatch(typeof(TerrainManager), "SampleRawSurface", new Type[] { typeof(float), typeof(float) })]
    /*
     * SampleRawSurface sometimes forgets to multiply the z coordinate by 1080 when indexing m_rawSurface
     * This transpiler add that multiplication.
     * 
     * Assumption: the integer z coordinate is stored in local variable 1
     * */
    class SampleRawSurfacePatch {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codesEnumerable) {
            var codes = codesEnumerable.ToList();
            CodeInstruction numLoadInstruction = null;
            for(int i = 0; i<codes.Count; i++) {
                var code = codes[i];
                if (code.LoadsField(typeof(TerrainManager).GetField("m_rawSurface",ReflectionHelpers.ALL))) {
                    yield return code;
                    i++;
                    code = codes[i];
                    if(code.opcode == OpCodes.Ldloc_1) {
                        yield return code;
                        yield return new CodeInstruction(OpCodes.Ldc_I4, 1080);
                        yield return new CodeInstruction(OpCodes.Mul);
                        Log.Debug("inserted * 1080");
                        i++;
                        code = codes[i];
                    }
                }
                yield return code;
            }
        }
    }
}