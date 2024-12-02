using Godot;
using System;



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
        CFEConfigFileImportPlugin plugin = new  CFEConfigFileImportPlugin();
        plugin.Import("res://Assets/Sprites/fire_static.spr", "res://Assets/Sprites/fire_static.tscn", null, null, null);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
