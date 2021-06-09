namespace CommonGround.Patches {
    using HarmonyLib;
    using JetBrains.Annotations;
    using KianCommons;
    using KianCommons.Patches;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    [UsedImplicitly]
    [HarmonyPatch(typeof(TerrainManager), "SampleDetailSurface", new Type[] { typeof(float), typeof(float) })]
    /*
     * SampleDetailSurface sometimes gives GetSurfaceCell a z-value that already has 4320 multiplied as if for linear indexing, which GetSurfaceCell takes care of itself.
     * This transpiler removes that multiplication.
     * */
    class SampleDetailSurfacePatch {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codesEnumerable) {
            var codes = codesEnumerable.ToList();
            CodeInstruction numLoadInstruction = null;
            for(int i = 0; i<codes.Count; i++) {
                var code = codes[i];
                if(code.Is(OpCodes.Ldc_I4, 4320)) {
                    //find the local variable that stores 4320
                    Log.Debug("found load constant 4320");
                    yield return code;
                    i++;
                    code = codes[i];
                    if (code.IsStloc()) {
                        numLoadInstruction = TranspilerUtils.BuildLdLocFromStLoc(code);
                        Log.Debug("found local variable for 4320");
                    }
                }else if(numLoadInstruction is not null && code.IsSameInstruction(numLoadInstruction)) {
                    //if that variable is loaded and then immediately multiplied, skip those two instructions
                    if(codes[i+1].opcode == OpCodes.Mul) {
                        i += 2;
                        code = codes[i];
                        Log.Debug("skipping * 4320");
                    }
                }
                yield return code;
            }
        }
    }
}