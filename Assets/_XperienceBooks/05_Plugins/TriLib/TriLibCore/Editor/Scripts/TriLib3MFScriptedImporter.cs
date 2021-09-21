using UnityEditor.Experimental.AssetImporters;

namespace TriLibCore.Editor
{
#if !TRILIB_DISABLE_EDITOR_3MF_IMPORT
    [ScriptedImporter(2, new[] { "3mf" })]
#endif
    public class TriLib3MFScriptedImporter : TriLibScriptedImporter
    {

    }
}