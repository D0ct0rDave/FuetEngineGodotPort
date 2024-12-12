using Godot;
//-----------------------------------------------------------------------------
using CFEString = System.String;
using FEReal = System.Single;
using CFEVect2 = Godot.Vector2;
using CFEColor = Godot.Color;
//-----------------------------------------------------------------------------
namespace FuetEngine
{
    [Tool]
	public class CFEHUDRect : CFEHUDObject 
	{
		//-----------------------------------------------------------------------------	
		[Export]
		public float              		m_rWidth;
		[Export]
	    public float					m_rHeight;
		[Export]
	    public CFEColor[] m_oCorners	= new CFEColor[4];
		[Export]
		public CFEVect2            		m_oPivot;
		//-----------------------------------------------------------------------------	
		private Vector2[] m_oVX = new Vector2[4];
		private Vector2[] m_oUV = new Vector2[4];
		//-----------------------------------------------------------------------------	
		/// Sets the pivot of the rect
		public void SetPivot(CFEVect2 _oPivot)
		{
			m_oPivot = _oPivot;
		}

		/// Retrieves the pivot of the rect
		public CFEVect2 oGetPivot()
		{
			return m_oPivot;
		}
		
		/// Retrieves the width of the rect
		public void SetWidth(FEReal _rWidth)
		{
			m_rWidth = _rWidth;
		}

		/// Retrieves the width of the rect
		public FEReal rGetWidth()        		    
		{
			return m_rWidth;
		}

		/// Retrieves the height of the rect
		public void SetHeight(FEReal _rHeight)
		{
			m_rHeight = _rHeight;
		}

		/// Retrieves the height of the rect
		public FEReal rGetHeight()
		{
			return m_rHeight;
		}
		
		/// Sets one of ther corners colors
		public void SetCornerColor(int _iCorner, CFEColor _oColor)
		{
			m_oCorners[_iCorner] = _oColor;
		}

		/// Retrieves one of ther corners colors
		public CFEColor oGetCornerColor(int _iCorner)
		{
			return m_oCorners[_iCorner];
		}
		//-----------------------------------------------------------------------------
		public void GenerateSpriteGeometry()
		{
			FEReal rXOfs = -m_oPivot.x * m_rWidth;
			FEReal rYOfs = -m_oPivot.y * m_rHeight;

			m_oVX[0].x = rXOfs;
			m_oVX[0].y = rYOfs;

			m_oVX[1].x = rXOfs + m_rWidth;
			m_oVX[1].y = rYOfs;

			m_oVX[2].x = rXOfs + m_rWidth;
			m_oVX[2].y = rYOfs + m_rHeight;

			m_oVX[3].x = rXOfs;
			m_oVX[3].y = rYOfs + m_rHeight;		
		}

		// --------------------------------------------------------------------
		public CFEHUDRect()
		{
			Name = "CFEHUDRect";
		}
		// --------------------------------------------------------------------
		public override void _Ready()
		{
			GenerateSpriteGeometry();
		}
		// ----------------------------------------------------------------------------
		public override void _Process(float _deltaT)
		{			
			Update();
		}
		// ----------------------------------------------------------------------------
		private void OnDrawSignal()
		{
			Render();
		}
		// ----------------------------------------------------------------------------		
		public override void _Draw()
		{
			Render();
		}
		// ----------------------------------------------------------------------------		
		private void Render()
		{
			DrawPrimitive( m_oVX, m_oCorners, m_oUV);
		}
		//-----------------------------------------------------------------------------
	};
}
//-----------------------------------------------------------------------------
