//Auto-generated: Do not modify this file!

using System.Collections;
using System.Collections.Generic;
using TriLibCore.Gltf.Reader;
using TriLibCore.Obj.Reader;
using TriLibCore.Ply.Reader;
using TriLibCore.Stl.Reader;
using TriLibCore.ThreeMf.Reader;
using TriLibCore.Fbx.Reader;

namespace TriLibCore
{
    public class Readers
    {
        public static IList<string> Extensions
        {
            get
            {
                var extensions = new List<string>();
				extensions.AddRange(GltfReader.GetExtensions());
				extensions.AddRange(ObjReader.GetExtensions());
				extensions.AddRange(PlyReader.GetExtensions());
				extensions.AddRange(StlReader.GetExtensions());
				extensions.AddRange(ThreeMfReader.GetExtensions());
				extensions.AddRange(FbxReader.GetExtensions());
                return extensions;
            }
        }
        public static ReaderBase FindReaderForExtension(string extension)
        {
			
			if (((IList) GltfReader.GetExtensions()).Contains(extension))
			{
				return new GltfReader();
			}
			if (((IList) ObjReader.GetExtensions()).Contains(extension))
			{
				return new ObjReader();
			}
			if (((IList) PlyReader.GetExtensions()).Contains(extension))
			{
				return new PlyReader();
			}
			if (((IList) StlReader.GetExtensions()).Contains(extension))
			{
				return new StlReader();
			}
			if (((IList) ThreeMfReader.GetExtensions()).Contains(extension))
			{
				return new ThreeMfReader();
			}
			if (((IList) FbxReader.GetExtensions()).Contains(extension))
			{
				return new FbxReader();
			}
            return null;
        }
    }
}