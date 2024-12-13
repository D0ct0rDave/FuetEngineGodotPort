// ----------------------------------------------------------------------------
using System.Data;
using Godot;
using CFEString = System.String;
using CFEFont = Godot.Theme;

namespace FuetEngine
{
	[Tool]
	public class CFEFontMgr : ResourceManager<CFEFont, CFEFontMgr>
	{
		protected override CFEFont LoadResource(string filename)
		{
			CFEFont themeResource = null;

			// Godot ResourceLoader expects the filename to have the extension so it
			// can detect the appropriate ResourceLoader to load the resource.
			string canonicalFilename = CFEStringUtils.sGetPath(filename) + "/" + CFEStringUtils.sGetFilename(filename);
			string canonicalFilenameWithExt =  canonicalFilename + ".theme.tres";

			if (ResourceLoader.Exists(canonicalFilenameWithExt))
			{
				Resource resource = ResourceLoader.Load(canonicalFilenameWithExt);
				
				themeResource = resource as CFEFont;				
				if (themeResource == null)
				{
					GD.Print("Cannot convert resource to CFEFont: " + canonicalFilenameWithExt);	
				}
			}
			else
			{
				GD.Print("Cannot find CFEFont: " + canonicalFilenameWithExt);
			}

			return themeResource;				
		}

		protected override void InvalidateResource(Theme _oRes)
		{
			_oRes.Dispose();
		}
	}
}
// ----------------------------------------------------------------------------
