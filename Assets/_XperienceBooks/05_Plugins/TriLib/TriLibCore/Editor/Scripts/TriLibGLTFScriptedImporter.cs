using UnityEditor.Experimental.AssetImporters;

namespace TriLibCore.Editor
{
#if !TRILIB_DISABLE_EDITOR_GLTF_IMPORT
    [ScriptedImporter(2, new[] { "gltf", "glb"})]
#endif
    public class TriLibGLTFScriptedImporter : TriLibScriptedImporter
    {

    }
}