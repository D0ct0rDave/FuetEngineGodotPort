using Godot;
using System.Diagnostics;
using CFEVect2 = Godot.Vector2;

namespace FuetEngine
{
    [Tool]
	public static class Support
	{
		public const string SPRITE_SCRIPT_FILE = "res://Scripts/FuetEngine/Sprite/CFESprite.cs";
		public const string SPRITE_INSTANCE_SCRIPT_FILE = "res://Scripts/FuetEngine/Sprite/CFESpriteInstance.cs";
		public const string SPRITE_FRAME_SCRIPT_FILE = "res://Scripts/FuetEngine/Sprite/CFESpriteFrame.cs";
		public const string SPRITE_ACTION_SCRIPT_FILE = "res://Scripts/FuetEngine/Sprite/CFESpriteAction.cs";
		public const string HUD_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUD.cs";
		public const string HUD_ELEMENT_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDElement.cs";
		public const string HUD_OBJECT_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDObject.cs";
		public const string HUD_GROUP_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDGroup.cs";
		public const string HUD_ICON_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDIcon.cs";
		//---------------------------------------------------------------------
		private static B _CreateObject<T,B>(Script _script) where T : Node, new() where B : Node
		{
			_script.SetupLocalToScene();

			T node = new T();
			var godotObjectId = node.GetInstanceId();
			node.SetScript(_script);
			B finalNode = GD.InstanceFromId(godotObjectId) as B;

			if (finalNode == null)
			{
				GD.Print("Cannot convert back node to class of type " + typeof(B) + ".\n" + 
				"If this class is instanced inside the editor don't forget to mark it as [Tool] !!!");

				Debugger.Break(); // Set breakpoint here
			}

			return finalNode;
		}
		//---------------------------------------------------------------------
		public static T CreateObject<T>(Script _script) where T : Node, new()
		{
			return _CreateObject<T,T>(_script);
		}
		//---------------------------------------------------------------------
		public static T CreateObject<T>(string _scriptFilename) where T : Node, new()
		{
			GD.Print("CreateObject("+_scriptFilename+")");			
			return _CreateObject<T,T>(ResourceLoader.Load(_scriptFilename) as Script);
		}
		//---------------------------------------------------------------------
		public static B CreateObject<B,T>(Script _script) where T : Node, new() where B : Node
		{
			return _CreateObject<T,B>(_script);
		}
		//---------------------------------------------------------------------
		public static B CreateObject<T,B>(string _scriptFilename) where T : Node, new() where B : Node
		{
			GD.Print("CreateObject("+_scriptFilename+")");
			return _CreateObject<T,B>(ResourceLoader.Load(_scriptFilename) as Script);
		}
		//---------------------------------------------------------------------
		public static void SetObjectEnabled(Node _node, bool _enabled)
		{
			_node.SetPhysicsProcess(_enabled);
			_node.SetPhysicsProcessInternal(_enabled);
			_node.SetProcess(_enabled);
			_node.SetProcessUnhandledInput(_enabled);
			_node.SetProcessUnhandledKeyInput(_enabled);
		}
		public static void SetObjectEnabled(Node2D _node, bool _enabled)
		{
			_node.Visible = _enabled;
			SetObjectEnabled(_node as Node, _enabled);
		}
	}
	// ----------------------------------------------------------------------------
	/// Rendering Blending modes
	public enum EFEBlendMode
	{
		BM_NONE,
		
		BM_COPY,
		BM_ALPHA,
		BM_ADD,
		BM_ALPHAADD,
		BM_MULT,
		
		BM_FOG,
		BM_FOGADD,
		
		BM_MAGICMARKER,
		BM_LIGHTMARKER,
		BM_LIGHTSABER,
		BM_REVEAL,
		BM_LUMISHADE_REVEAL,
		
		BM_NUM_MODES,
		BM_DEFAULT = BM_ALPHA
	};

	public class CFERect
	{
		/// <summary>
		///  Default constructor for the class
		/// </summary>
		public CFERect() { }

		public CFERect(float _fIX,float _fIY, float _fFX, float _fFY)
		{
			m_oIni.x = _fIX;
			m_oIni.y = _fFX;
			m_oEnd.x = _fFX;
			m_oEnd.x = _fFY;
		}

		public CFEVect2 m_oIni = new CFEVect2();
		public CFEVect2 m_oEnd = new CFEVect2();
	};

	public class CFENamedObject
	{
		/// Default constructor of this object.
		public CFENamedObject(string _sName)
		{
			SetName(_sName);
		}
		
		/// Sets the name for this object.
		public void SetName(string _sName)
		{
			m_sName = _sName;
		}
		
		/// Retrieves the name of this object.
		public string sGetName()
		{
			return (m_sName);
		}

		protected string m_sName = "";
	};
};