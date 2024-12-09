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

			if (ResourceLoader.Exists(filename + ".tres"))
			{
				Resource resource = ResourceLoader.Load(filename + ".tres");
				spriteResource = resource as CFESprite;
			}
			else
			{
				spriteResource = CFESpriteLoader.oLoad(filename, false);
				if (spriteResource != null)
				{
					Godot.Error saveError = ResourceSaver.Save(filename + ".tres", spriteResource);
				}
				else
				{
					GD.Print("Null Sprite: " + filename);
				}
			}

			spriteResource.ResourceLocalToScene = false;
			spriteResource.ResourcePath = filename + "." + "tres";
			return spriteResource;
		}

		protected override void InvalidateResource(CFESprite _oRes)
		{
			_oRes.Dispose();
		}
	}
}
// ----------------------------------------------------------------------------
