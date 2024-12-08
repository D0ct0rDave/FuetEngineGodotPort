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
			CFESprite sprite = CFESpriteLoader.oLoad(filename, false);
			if (sprite == null)
			{
				GD.Print("Null Sprite: " + filename);
			}
			return sprite;
		}

		protected override void InvalidateResource(CFESprite _oRes)
		{
			_oRes.Dispose();
		}
	}
}
// ----------------------------------------------------------------------------
