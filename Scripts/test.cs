using Godot;
using FuetEngine;

public class test : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export]
	public string m_resource;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		/*
		CFEConfigFileImportPlugin plugin = new CFEConfigFileImportPlugin();
		Node node = plugin.LoadConfigAsNode("res://Assets/base/common/data/menu/page_profile.hud");
		AddChild(node);
		*/

		/*
        if (!Engine.EditorHint)
        {
            AddChild(new FuetEnginePluginRegistererPlugin());
        }
        */

		/*
		CFEConfigFileImportPlugin plugin = new  CFEConfigFileImportPlugin();
        Node node = plugin.LoadConfigAsNode("res://Assets/data/menu/page_ini.hud");
        AddChild(node);
		*/

		/*
		CFESpriteInstance spriteInstance = Support.CreateObject<CFESpriteInstance>(FuetEngine.Support.SPRITE_INSTANCE_SCRIPT_FILE);
		spriteInstance.Name = "CFESpriteInstance";
		spriteInstance.Init(sprite);
        
        AddChild(spriteInstance);
        */

		/*
        CFEConfigFileImportPlugin plugin = new  CFEConfigFileImportPlugin();
        Node node = plugin.LoadConfigAsNode("res://Assets/base/common/data/menu/page_delete_profile.hud");
        AddChild(node);

        Node thisNode = this;
        plugin.SetHierarchyOwner(ref thisNode, thisNode);

		var packedScene = new PackedScene();
		Godot.Error error = packedScene.Pack(this);
        if (error == Error.Ok)
        {
            Godot.Error saveError = ResourceSaver.Save("res://Assets/test.tscn", packedScene);
        }
        */

		// plugin.Import("res://Assets/data/intro/sprites/logo.spr", "res://Assets/data/intro/sprites/logo", null, null, null);
		// plugin.Import("res://Assets/base/common/data/HUD/sprites/HUD_bocata.spr", "res://Assets/base/common/data/HUD/sprites/HUD_bocata", null, null, null);

		// plugin.Import("res://Assets/Sprites/fire_static.spr", "res://Assets/Sprites/fire_static", null, null, null);
		// plugin.Import("res://Assets/data/Intro/intro_page1.hud", "res://Assets/data/Intro/intro_page1", null, null, null);
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      d
	//  }
}
