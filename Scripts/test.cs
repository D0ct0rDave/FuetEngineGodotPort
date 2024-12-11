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
        if (!Engine.EditorHint)
        {
            AddChild(new FuetEnginePluginRegistererPlugin());
        }
        */

        /*
        Resource resource = ResourceLoader.Load("res://Assets/data/intro/sprites/logo.spr");
        CFESprite sprite = resource as CFESprite;

		CFESpriteInstance spriteInstance = Support.CreateObject<CFESpriteInstance>(FuetEngine.Support.SPRITE_INSTANCE_SCRIPT_FILE);
		spriteInstance.Name = "CFESpriteInstance";
		spriteInstance.Init(sprite);
        
        AddChild(spriteInstance);
        */

/*
        PackedScene packedScene = ResourceLoader.Load("res://Assets/data/Intro/intro_page1.hud") as PackedScene;
        Node node = packedScene.Instance();
        AddChild(node);
*/
        CFEConfigFileImportPlugin plugin = new  CFEConfigFileImportPlugin();
        plugin.Import("res://Assets/data/Intro/intro_page1.hud", "res://Assets/data/Intro/intro_page1", null, null, null);

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
