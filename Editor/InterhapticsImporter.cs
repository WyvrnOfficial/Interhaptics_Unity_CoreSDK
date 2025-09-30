/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif


namespace Interhaptics.Editor
{

    [ScriptedImporter(1, "haps")]
    internal class InterhapticsImporter : ScriptedImporter
    {

        public override void OnImportAsset(AssetImportContext ctx)
        {
            HapticMaterial hapticMaterial = HapticMaterial.CreateInstanceFromString(System.IO.File.ReadAllText(ctx.assetPath));
            ctx.AddObjectToAsset("main", hapticMaterial);
            ctx.SetMainObject(hapticMaterial);
        }

    }

}
