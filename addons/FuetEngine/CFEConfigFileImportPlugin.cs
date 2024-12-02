#if TOOLS
using System;
using Godot;
using FuetEngine;

[Tool]
public partial class CFEConfigFileImportPlugin : EditorImportPlugin
{
	public override string GetImporterName()
	{
		return "CFEConfigFileImporter";
	}

	public override string GetVisibleName()
	{
		return "CFEConfigFile";
	}
    public override string GetResourceType()
    {
        return "PackedScene";
    }
    
	public override Godot.Collections.Array GetImportOptions(int i)
    {
        var options = new Godot.Collections.Array();

        // Agregar opciones de importación
        // options.Add("miopcion1", new OptionProperty(true, PropertyHintEnum.NONE, "Mi Opción 1"));
        // options.Add("miopcion2", new OptionProperty(1.0f, PropertyHintEnum.RANGE, "Mi Opción 2", 0.0f, 10.0f));

        return options;
    }

    public override Godot.Collections.Array GetRecognizedExtensions()
    {
        // Extensiones de archivo que el importador puede manejar
		Godot.Collections.Array array = new Godot.Collections.Array();
		array.Add( "cfg");
		array.Add( "spr");
	    return array;
    }
	public override int GetPresetCount()
	{
		return 0;
	}
	public override string GetSaveExtension()
	{
		return "config.tscn";
	}

	public override int Import(string _sourceFile, string _savePath, Godot.Collections.Dictionary _options, Godot.Collections.Array _r_platform_variants, Godot.Collections.Array _r_gen_files)
	{
		Node2D node = ConvertConfigToNode(_sourceFile);
		if (node != null)
		{
			GD.Print("ChildCount: " + node.GetChildCount().ToString());

			var packedScene = new PackedScene();
			Godot.Error error = packedScene.Pack(node);
			if(error == Error.Ok)
			{
				GD.Print(_savePath);
				Godot.Error saveError = ResourceSaver.Save(_savePath, packedScene);
				if (saveError != Error.Ok)
				{
					return 0;
				}
			}
		}

		return 0;
	}
	private Node2D ConvertConfigToNode(string _sFilename)
	{
		string extension = _sFilename.Substring(_sFilename.Length-3);
		if (extension == "spr")
		{
			return FESpriteImporter.Import(_sFilename);
		}
		else if (extension == "hud")
		{
			return null;
		}

		return null;
	}
}
#endif
