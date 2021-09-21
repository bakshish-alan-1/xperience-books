using UnityEditor.Experimental.AssetImporters;

namespace TriLibCore.Editor
{
#if !TRILIB_DISABLE_EDITOR_STL_IMPORT
    [ScriptedImporter(2, new[] { "stl" })]
#endif
    public class TriLibSTLScriptedImporter : TriLibScriptedImporter
    {

    }
}