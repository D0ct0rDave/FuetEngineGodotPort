#if TOOLS
using Godot;
using FuetEngine;

[Tool]
public partial class CFESpriteFileImportPlugin : EditorImportPlugin
{
	// ------------------------------------------------------------------------	
	public override string GetImporterName()
	{
		return "CFESpriteFileImportPlugin";
	}
	// ------------------------------------------------------------------------	
	public override string GetVisibleName()
	{
		return "CFESprite file importer";
	}
	// ------------------------------------------------------------------------	
	public override string GetResourceType()
	{
		return "Resource";
	}
	// ------------------------------------------------------------------------	
	public override Godot.Collections.Array GetImportOptions(int i)
	{
		var options = new Godot.Collections.Array();
		return options;
	}
	// ------------------------------------------------------------------------	
	public override Godot.Collections.Array GetRecognizedExtensions()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		array.Add( "spr");
		return array;
	}
	// ------------------------------------------------------------------------	
	public override int GetPresetCount()
	{
		return 0;
	}
	// ------------------------------------------------------------------------	
	public override string GetSaveExtension()
	{
		return "tres";
	}
	// ------------------------------------------------------------------------	
	private Resource LoadResource(string _sFilename)
	{
		string filenameWithoutExtension = _sFilename.Substr(0,_sFilename.Length - 4);
		GD.Print("CFESpriteFileImportPlugin::LoadResource " + filenameWithoutExtension);
		
		return CFESpriteLoader.oLoad(filenameWithoutExtension);
	}
	// ------------------------------------------------------------------------	
	private int SaveResource(Resource _resource, string _savePath)
	{
		Godot.Error saveError = ResourceSaver.Save(_savePath, _resource/* , Godot.ResourceSaver.SaverFlags.BundleResources | Godot.ResourceSaver.SaverFlags.RelativePaths*/);
		return (saveError != Error.Ok) ? 1 : 0;
	}
	// ------------------------------------------------------------------------	
	public override int Import(string _sourceFile, string _savePath, Godot.Collections.Dictionary _options, Godot.Collections.Array _r_platform_variants, Godot.Collections.Array _r_gen_files)
	{
		Resource resource = LoadResource(_sourceFile);
		if (resource != null)
		{
			return SaveResource(resource, _savePath + "." + GetSaveExtension());
		}

		return 1;
	}
	// ------------------------------------------------------------------------
}
#endif
