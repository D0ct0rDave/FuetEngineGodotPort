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
			// Godot ResourceLoader expects the filename to have the extension so it
			// can detect the appropriate ResourceLoader to load the resource.
			string canonicalFilename = CFEStringUtils.sGetPath(filename) + "/" + CFEStringUtils.sGetFilename(filename) + ".spr";

			Resource resource = ResourceLoader.Load(canonicalFilename);
			CFESprite spriteResource = resource as CFESprite;
			
			if (resource != null)
			{
				GD.Print("Resource loaded successfully: " + canonicalFilename);
			}
			else
			{
				GD.Print("Cannot load resource: " + canonicalFilename);
			}
			
			if (spriteResource == null)
			{
				GD.Print("Cannot convert resource to Sprite: " + canonicalFilename);
			}

			return spriteResource;
			
			/*
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
			*/
		}

		protected override void InvalidateResource(CFESprite _oRes)
		{
			_oRes.Dispose();
		}
	}
}
// ----------------------------------------------------------------------------
