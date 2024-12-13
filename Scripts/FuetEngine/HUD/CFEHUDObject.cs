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
	public class CFEHUDObject : Node2D
	{
		// Do we want to keep these classes or just want to build a hierarchy of Godot Objects?
		// I we are not supposed to modify these values, we should keep these properties private 
		// and non exportable.
		[Export]
		public CFEVect2 m_oIniPos { set{ m_iniPos = value; GlobalPosition = value; } get{ return m_iniPos; }}
		private CFEVect2 m_iniPos;
		[Export]
		public CFEVect2	m_oIniScale { set{ m_iniScale = value; GlobalScale = value; } get{ return m_iniScale; }}
		private CFEVect2 m_iniScale;
		[Export]
		public FEReal m_rIniAngle { set{ m_iniAngle = value; GlobalRotation = value; } get{ return m_iniAngle; }}
		public FEReal m_iniAngle;
		[Export]
		public FEReal m_rIniDepth { set{ m_iniDepth = value; ZIndex = -(int)(m_iniDepth*255.0f); } get{ return m_iniDepth; }}
		private FEReal m_iniDepth;
		[Export]
		public CFEColor	m_oIniColor { set{ m_iniColor = value; Modulate = value; } get{ return m_iniColor; }}
		private  CFEColor m_iniColor;
		[Export]
		public int m_iIniAction;
		[Export]
		public bool	m_bIniVis { set{ m_iniVis = value; Visible = value; } get{ return m_iniVis; }}
		private  bool m_iniVis;
		[Export]
		public CFEString	m_sTAG;

		/// Values set up by actions
		public CFEVect2		m_oCurPos;
		public CFEVect2		m_oCurScale;
		public FEReal		m_rCurAngle;
		public FEReal		m_rCurDepth;
		public CFEColor		m_oCurColor;
		public int			m_iCurAction;
		public bool			m_bCurVis;		
		// --------------------------------------------------------------------
		// Make sure you provide a parameterless constructor.
		public CFEHUDObject()
		{
			Name = "CFEHUDObject";
			m_rIniDepth = 0.0f;
			m_oIniColor	= new CFEColor(1.0f,1.0f,1.0f,1.0f);
			m_iIniAction= -1;
			m_bIniVis   = true;

			m_sTAG		= "";

			m_oCurPos	= CFEVect2.Zero;
			m_oCurScale	= CFEVect2.One;
			m_rCurAngle = 0.0f;
			m_rCurDepth = 0.0f;
			m_oCurColor = new CFEColor(1.0f,1.0f,1.0f,1.0f);
			m_iCurAction= -1;
			m_bCurVis = true;
		}
		// --------------------------------------------------------------------
		/// Sets the initial position for the HUD Object.
		public void SetIniPos(CFEVect2 _oPos)
		{
			GlobalPosition = _oPos;
		}
		// --------------------------------------------------------------------
		/// Retrieves the initial position of the HUD Object.
		public CFEVect2 oGetIniPos()
		{
			return GlobalPosition;
		}
		// --------------------------------------------------------------------
		/// Sets the initial scale for the HUD Object.
		public void SetIniScale(CFEVect2 _oScale)
		{
			GlobalScale  = _oScale;
		}
		// --------------------------------------------------------------------
		/// Retrieves the initial scale of the HUD Object.
		public CFEVect2 oGetIniScale()
		{
			return GlobalScale;
		}
		// --------------------------------------------------------------------
		/// Sets the initial angle for the HUD Object.
		public void SetIniAngle(FEReal _rAngle)
		{
			GlobalRotation = _rAngle;
		}
		// --------------------------------------------------------------------
		/// Retrieves the initial angle of the HUD Object.
		public FEReal rGetIniAngle()
		{
			return GlobalRotation;
		}
		// --------------------------------------------------------------------
		/// Sets the initial depth for the HUD Object.
		public void SetIniDepth(FEReal _rDepth)
		{
			// m_rIniDepth = _rDepth;
		}
		// --------------------------------------------------------------------
		/// Retrieves the initial depth of the HUD Object.
		public FEReal rGetIniDepth()
		{
			return m_rIniDepth;
		}
		// --------------------------------------------------------------------
		/// Sets the color for the HUD Object.
		public void SetIniColor(CFEColor _oColor)
		{
			m_oIniColor = _oColor;
		}
		// --------------------------------------------------------------------
		/// Retrieves the color of the HUD Object.
		public CFEColor oGetIniColor()
		{
			return m_oIniColor;
		}
		// --------------------------------------------------------------------
		/// Sets the initial visibility of the object
		public void SetIniVis(bool _bVisible)
		{
			m_bIniVis = _bVisible;
		}
		// --------------------------------------------------------------------
		/// Retrieves the initial visibility of the object
		public bool bGetIniVis()
		{
			return m_bIniVis;
		}
		// --------------------------------------------------------------------
		/// Makes this HUD Object visible.
		public void ShowObj(bool _bShowObj = true)
		{
			SetIniVis(_bShowObj);
		}
		// --------------------------------------------------------------------
		/// Makes this HUD Object invisible.
		public void HideObj()
		{
			ShowObj(false);
		}
		// --------------------------------------------------------------------
		/// Retrieves whether this object is visible or not.
		public bool bIsObjVisible()
		{
			return bGetIniVis();
		}
		// --------------------------------------------------------------------
		/// Sets the initial action for this object. (only sprites, meshes, (and skel anims)).
		public void SetIniAction(int _iAction)
		{
			m_iIniAction = _iAction;
		}
		// --------------------------------------------------------------------
		/// Retrieves the initial action of this object. (only sprites, meshes, (and skel anims)).
		public int iGetIniAction()
		{
			return m_iIniAction;
		}
		// --------------------------------------------------------------------
		/// Sets the current position for the HUD Object.
		public void SetCurPos(CFEVect2 _oPos)
		{
			m_oCurPos = _oPos;
		}
		// --------------------------------------------------------------------
		/// Retrieves the current position of the HUD Object.
		public CFEVect2 oGetCurPos()
		{
			return m_oCurPos;
		}
		// --------------------------------------------------------------------
		/// Sets the current scale for the HUD Object.
		public void SetCurScale(CFEVect2 _oScale)
		{
			m_oCurScale = _oScale;
		}
		// --------------------------------------------------------------------
		/// Retrieves the current scale of the HUD Object.
		public CFEVect2 oGetCurScale()
		{
			return m_oCurScale;
		}
		// --------------------------------------------------------------------
		/// Sets the current angle for the HUD Object.
		public void SetCurAngle(FEReal _rAngle)
		{
			m_rCurAngle = _rAngle;
		}
		// --------------------------------------------------------------------
		/// Retrieves the current angle of the HUD Object.
		public FEReal rGetCurAngle()
		{
			return(m_rCurAngle);
		}
		// --------------------------------------------------------------------
		/// Sets the current depth for the HUD Object.
		public void SetCurDepth(FEReal _rDepth)
		{
			m_rCurDepth = _rDepth;
		}
		// --------------------------------------------------------------------
		/// Retrieves the current depth of the HUD Object.
		public FEReal rGetCurDepth()
		{
			return(m_rCurDepth);
		}
		// --------------------------------------------------------------------
		/// Sets the color for the HUD Object.
		public void SetCurColor(CFEColor _oColor)
		{
			m_oCurColor = _oColor;
		}
		// --------------------------------------------------------------------
		/// Retrieves the color of the HUD Object.
		public CFEColor oGetCurColor()
		{
			return m_oCurColor;
		}
		// --------------------------------------------------------------------
		/// Sets the action visibility
		public void SetCurVis(bool _bCurVis)
		{
			m_bCurVis = _bCurVis;
		}
		// --------------------------------------------------------------------
		/// Retrieves whether this object is not visible or should take into account object visibiliy.
		public bool bGetCurVis()
		{
			return m_bCurVis;
		}
		// --------------------------------------------------------------------
		/// Sets the current action for this object. (only sprites, meshes, (and skel anims)).
		public virtual void SetCurAction(int _iAction)
		{
			m_iCurAction = _iAction;
		}
		// --------------------------------------------------------------------
		/// Retrieves the current action of this object. (only sprites, meshes, (and skel anims)).
		public int iGetCurAction()
		{
			return m_iCurAction;
		}

		// -----------------------------------------------
		/// Retrieves the initial position of the HUD Object.
		public CFEVect2 oGetPos()
		{
			return oGetCurPos() + oGetIniPos();
		}
		// --------------------------------------------------------------------
		/// Retrieves the initial scale of the HUD Object.
		public CFEVect2 oGetScale()
		{
			return oGetCurScale() + oGetIniScale();
		}
		// --------------------------------------------------------------------
		/// Retrieves the initial angle of the HUD Object.
		public FEReal rGetAngle()
		{
			return rGetCurAngle() + rGetIniAngle();
		}
		// --------------------------------------------------------------------
		/// Retrieves the current depth of the object
		public FEReal rGetDepth()
		{
			return rGetCurDepth() + rGetIniDepth();
		}
		// --------------------------------------------------------------------
		/// Retrieves the color of the HUD Object.
		public CFEColor oGetColor()
		{
			return oGetCurColor() * oGetIniColor();
		}
		// --------------------------------------------------------------------
		/// Retrieves the object action.
		public int iGetAction()
		{
			if (m_iCurAction == -1)
				return(m_iIniAction);
			else
				return(m_iCurAction);
		}
		// --------------------------------------------------------------------
		/// Retrieves whether this object is visible or not.
		public bool bIsVisible()
		{
			return m_bIniVis && m_bCurVis;
		}
		// --------------------------------------------------------------------
		/// Sets the TAG string for this object
		public void SetTAG(CFEString _sTAG)
		{
			m_sTAG = _sTAG;
		}
		// --------------------------------------------------------------------
		/// Retrieves the TAG string of this object
		public CFEString sGetTAG()
		{return m_sTAG;
		}
		// --------------------------------------------------------------------
		/// Perform processing over the object
		public virtual void Accept(CFEHUDVisitor _oVisitor)
		{
			_oVisitor.Visit(this);
		}
	}
}
//-----------------------------------------------------------------------------
