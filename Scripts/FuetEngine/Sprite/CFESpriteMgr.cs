// ----------------------------------------------------------------------------
using System.Data;
using Godot;
using CFEString = System.String;

namespace FuetEngine
{
	[Tool]
	public class CFESpriteMgr : ResourceManager<CFESprite, CFESpriteMgr>
	{
		protected override CFESprite LoadResource(string filename)
		{
			CFESprite spriteResource;
			string canonicalFilename = CFEStringUtils.sGetCanonicalPath(filename);
			string godotResourceFilename = canonicalFilename + ".tres";

			if (ResourceLoader.Exists(godotResourceFilename))
			{
				Resource resource = ResourceLoader.Load(godotResourceFilename);
				spriteResource = resource as CFESprite;
			}
			else
			{
				spriteResource = CFESpriteLoader.oLoad(canonicalFilename, false);
				if (spriteResource != null)
				{
					Godot.Error saveError = ResourceSaver.Save(godotResourceFilename, spriteResource);
				}
				else
				{
					GD.Print("Null Sprite: " + canonicalFilename);
				}
			}
			
			if (spriteResource != null)
			{
				spriteResource.ResourceLocalToScene = false;
				spriteResource.ResourcePath = godotResourceFilename;
			}

			return spriteResource;
		}

		protected override void InvalidateResource(CFESprite _oRes)
		{
			_oRes.Dispose();
		}
	}
}
// ----------------------------------------------------------------------------
