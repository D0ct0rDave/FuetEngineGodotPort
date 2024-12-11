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
			CFESprite spriteResource = null;

			// Godot ResourceLoader expects the filename to have the extension so it
			// can detect the appropriate ResourceLoader to load the resource.
			string canonicalFilename = CFEStringUtils.sGetPath(filename) + "/" + CFEStringUtils.sGetFilename(filename);
			string canonicalFilenameWithExt =  canonicalFilename + ".spr";

			if (ResourceLoader.Exists(canonicalFilenameWithExt))
			{
				Resource resource = ResourceLoader.Load(canonicalFilenameWithExt);

				spriteResource = resource as CFESprite;				
				if (spriteResource == null)
				{
					GD.Print("Cannot convert resource to Sprite: " + canonicalFilenameWithExt);	
				}
			}
			else
			{
				spriteResource = CFESpriteLoader.oLoad(canonicalFilename);
				if (spriteResource == null)
				{
					GD.Print("Cannot load Sprite: " + canonicalFilename);	
				}
				else
				{
					spriteResource.ResourceLocalToScene = true;
				}
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
