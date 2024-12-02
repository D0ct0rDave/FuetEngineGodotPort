using System.IO;
using CFEVect2 = Godot.Vector2;

namespace FuetEngine
{
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

    [System.Serializable]
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