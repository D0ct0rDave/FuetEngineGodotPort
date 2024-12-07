using System.IO;
using CFEVect2 = Godot.Vector2;
using Godot;
using System;

namespace FuetEngine
{
	using CFEString = System.String;
	using FEReal = System.Single;

	[Tool]
	public static class Support
	{
		public static Node CreateObject(Script _script)
		{
			_script.SetupLocalToScene();

			Node node = new Node();
			var godotObjectId = node.GetInstanceId();
			node.SetScript(_script);
			return GD.InstanceFromId(godotObjectId) as Node;
		}
        public static Node CreateObject(string _scriptFilename)
        {
            return CreateObject(ResourceLoader.Load(_scriptFilename) as Script);
        }

        public static T CreateObject<T>(Script _script) where T : Node, new()
        {
			_script.SetupLocalToScene();

			T node = new T();
			var godotObjectId = node.GetInstanceId();
			node.SetScript(_script);
			node = GD.InstanceFromId(godotObjectId) as T;

			if (node == null)
			{
				GD.Print("Cannot convert back node to class " + typeof(T) + ".\n" + 
				"If this class is instanced inside the editor don't forget to mark it as [Tool] !!!");
			}

			return node;
		}

		public static T CreateObject<T>(string _scriptFilename) where T : Node, new()
        {
            return CreateObject<T>(ResourceLoader.Load(_scriptFilename) as Script);
        }

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