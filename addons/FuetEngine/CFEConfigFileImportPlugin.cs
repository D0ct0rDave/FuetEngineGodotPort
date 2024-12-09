#if TOOLS
using Godot;
using FuetEngine;

[Tool]
public partial class CFEConfigFileImportPlugin : EditorImportPlugin
{
	// ------------------------------------------------------------------------	
	public override string GetImporterName()
	{
		return "CFEConfigFileImporter";
	}
	// ------------------------------------------------------------------------	
	public override string GetVisibleName()
	{
		return "CFEConfigFile";
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

		// Agregar opciones de importación
		// options.Add("miopcion1", new OptionProperty(true, PropertyHintEnum.NONE, "Mi Opción 1"));
		// options.Add("miopcion2", new OptionProperty(1.0f, PropertyHintEnum.RANGE, "Mi Opción 2", 0.0f, 10.0f));

		return options;
	}
	// ------------------------------------------------------------------------	
	public override Godot.Collections.Array GetRecognizedExtensions()
	{
		// Extensiones de archivo que el importador puede manejar
		Godot.Collections.Array array = new Godot.Collections.Array();
		array.Add( "cfg");
		array.Add( "spr");
		// array.Add( "hud");
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
	public override int Import(string _sourceFile, string _savePath, Godot.Collections.Dictionary _options, Godot.Collections.Array _r_platform_variants, Godot.Collections.Array _r_gen_files)
	{
		Node node = ConvertConfigToNode(_sourceFile);
		if (node != null)
		{
			var packedScene = new PackedScene();
			Godot.Error error = packedScene.Pack(node);

			if(error == Error.Ok)
			{
				Godot.Error saveError = ResourceSaver.Save(_savePath + "." + GetSaveExtension(), packedScene/* , Godot.ResourceSaver.SaverFlags.BundleResources | Godot.ResourceSaver.SaverFlags.RelativePaths*/ );
				if (saveError != Error.Ok)
				{
					return 1;
				}
				return 0;
			}
		}
		return 1;
	}
	// ------------------------------------------------------------------------	
	private Node ConvertConfigToNode(string _sFilename)
	{
		string extension = _sFilename.Substring(_sFilename.Length-3);
		string filenameWithoutExtension = _sFilename.Substr(0,_sFilename.Length - 4); 

		if (extension == "spr")
		{
			CFESprite spriteResource = CFESpriteMgr.Instance.Load(filenameWithoutExtension);

			CFESpriteInstance spriteInstance = Support.CreateObject<Node2D>(FuetEngine.Support.SPRITE_INSTANCE_SCRIPT_FILE) as CFESpriteInstance;

			spriteInstance.Name = "CFESpriteInstance";
			spriteInstance.Init(spriteResource);

			Node childNode = spriteInstance as Node;
			SetHierarchyOwner(ref childNode, childNode);

			return spriteInstance;
		}
		else if (extension == "hud")
		{
			CFEHUD oHUD = CFEHUDLoader.oLoad(filenameWithoutExtension);

			Node childNode = oHUD as Node;
			SetHierarchyOwner(ref childNode, childNode);

			return oHUD;
		}

		return null;
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
}
#endif
