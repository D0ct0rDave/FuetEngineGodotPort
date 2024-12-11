#if TOOLS
using Godot;
using System.Collections.Generic;

[Tool]
public partial class FuetEnginePluginRegistererPlugin : EditorPlugin
{
	private List<EditorImportPlugin> m_importPlugins = new List<EditorImportPlugin>();
	public override void _EnterTree()
	{
		/*
		{
			EditorImportPlugin importPlugin = new CFEConfigFileImportPlugin();
			AddImportPlugin(importPlugin);
			m_importPlugins.Add(importPlugin);
		}
		*/

		{
			EditorImportPlugin importPlugin = new CFESpriteFileImportPlugin();
			AddImportPlugin(importPlugin);
			m_importPlugins.Add(importPlugin);
		}
		{
			EditorImportPlugin importPlugin = new CFEHUDFileImportPlugin();
			AddImportPlugin(importPlugin);
			m_importPlugins.Add(importPlugin);
		}

		foreach(EditorImportPlugin plugin in m_importPlugins)
		{
			GD.Print(plugin.GetVisibleName() + " installed.");
		}
	}

	public override void _ExitTree()
	{
		foreach(EditorImportPlugin plugin in m_importPlugins)
    	{
			RemoveImportPlugin(plugin);
		}

		m_importPlugins.Clear();
	}
}
#endif
