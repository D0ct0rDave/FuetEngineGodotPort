#if TOOLS
using Godot;
using FuetEngine;

[Tool]
public partial class CFEHUDFileImportPlugin : EditorImportPlugin
{
	// ------------------------------------------------------------------------	
	public override string GetImporterName()
	{
		return "CFEHUDFileImportPlugin";
	}
	// ------------------------------------------------------------------------	
	public override string GetVisibleName()
	{
		return "CFEHUD file importer";
	}
	// ------------------------------------------------------------------------	
	public override string GetResourceType()
	{
		return "PackedScene";
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
		// Extensiones de archivo que el importador puede manejar
		Godot.Collections.Array array = new Godot.Collections.Array();
		array.Add( "hud");
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
		return "tscn";
	}
	// ------------------------------------------------------------------------	
	private void SetHierarchyOwner(ref Node _root, Node _owner)
	{
		if (_root != _owner)
		{
			_root.Owner = _owner;
		}

		for (int i=0; i<_root.GetChildCount(); i++)
		{
			Node child = _root.GetChild(i);
			SetHierarchyOwner(ref child, _owner);
		}
	}
	// ------------------------------------------------------------------------	
	private Resource LoadResource(string _sFilename)
	{
		string filenameWithoutExtension = _sFilename.Substr(0,_sFilename.Length - 4);

		CFEHUD oHUD = CFEHUDLoader.oLoad(filenameWithoutExtension);
		Node childNode = oHUD as Node;
		SetHierarchyOwner(ref childNode, childNode);

		var packedScene = new PackedScene();
		Godot.Error error = packedScene.Pack(childNode);

		return (error != Error.Ok)? null : packedScene;
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
		
		{
			string filenameWithoutExtension = _sourceFile.Substr(0,_sourceFile.Length - 4);			
			SaveResource(resource, filenameWithoutExtension  + "." + GetSaveExtension());
		}

		return SaveResource(resource, _savePath + "." + GetSaveExtension());
	}
	// ------------------------------------------------------------------------	
}
#endif
