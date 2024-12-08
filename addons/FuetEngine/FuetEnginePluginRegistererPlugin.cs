#if TOOLS
using Godot;
using System.Collections.Generic;

[Tool]
public partial class FuetEnginePluginRegistererPlugin : EditorPlugin
{
	private List<EditorImportPlugin> m_importPlugins = new List<EditorImportPlugin>();
	public override void _EnterTree()
	{
		EditorImportPlugin importPlugin = new CFEConfigFileImportPlugin();
		AddImportPlugin(importPlugin);
		// AddCustomType("MiPluginDeImportacion", typeof(MiPluginDeImportacion));
		
		m_importPlugins.Add(importPlugin);
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
