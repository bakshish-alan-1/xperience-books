using UnityEditor.Experimental.AssetImporters;

namespace TriLibCore.Editor
{
#if !TRILIB_DISABLE_EDITOR_PLY_IMPORT
    [ScriptedImporter(2, new[] { "ply" })]
#endif
    public class TriLibPLYScriptedImporter : TriLibScriptedImporter
    {

    }
}