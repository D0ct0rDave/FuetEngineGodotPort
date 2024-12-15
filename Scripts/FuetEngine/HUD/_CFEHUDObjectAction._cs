using Godot;
// ----------------------------------------------------------------------------

namespace FuetEngine
{
    [Tool] 
	public partial class CFEHUDObjectAction : Node
	{
		// Make sure you provide a parameterless constructor.
		public CFEHUDObjectAction()
		{
			Name = "CFEHUDObjectAction";
		}
		/*
		/// Retrieves the configuration of the HUDObject at a given moment in time.
		const CFEHUDObject& oGetValue(FEReal _rTime)
		{
			// Computes all the values for all the properties of the HUD object.		
			m_poHUDObject->SetCurVis( m_bVisFunc.oGetValue(_rTime) );

			/// this properties should only be setup if the object is visible.
			if (m_poHUDObject->bIsVisible())
			{
				m_poHUDObject->SetCurAngle( m_rAngleFunc.oGetValue(_rTime) );
				m_poHUDObject->SetCurPos( CFEVect2(m_rXFunc.oGetValue(_rTime),m_rYFunc.oGetValue(_rTime)) );
				m_poHUDObject->SetCurScale( m_oScaleFunc.oGetValue(_rTime) );
				m_poHUDObject->SetCurColor( m_oColorFunc.oGetValue(_rTime) );
				m_poHUDObject->SetCurDepth( m_rDepthFunc.oGetValue(_rTime) );
				m_poHUDObject->SetCurAction( m_iActionFunc.oGetValue(_rTime) );

				// if the object is not visible it shoudn't trigger an event.
				m_oEventFunc.Check(_rTime);
			}

			return( *m_poHUDObject );
		}
		*/

		/// Sets the HUD object associated to this layer
		public void SetHUDObject(CFEHUDObject _oHUDObject)
		{
			// m_oEventFunc.SetObject(_oHUDObject);
			m_oHUDObject = _oHUDObject;
		}

		/// Retrieves the HUD object associated to this layer
		public CFEHUDObject oGetHUDObject()
		{
			return m_oHUDObject;
		}

		/// Perform processing over the object
		public virtual void Accept(CFEHUDVisitor _oVisitor)
		{
			_oVisitor.Visit(this);
		}
	/*
	public:

		CFEKFBFunc<FEReal>		m_rXFunc;
		CFEKFBFunc<FEReal>		m_rYFunc;
		CFEKFBFunc<CFEVect2>	m_oScaleFunc;
		CFEKFBFunc<CFEColor>	m_oColorFunc;
		CFEKFBFunc<FEReal>		m_rAngleFunc;
		CFEKFBFunc<FEReal>		m_rDepthFunc;
		CFEKFBFunc<int>			m_iActionFunc;
		CFEKFBFunc<FEBool>		m_bVisFunc;
		CFEHUDEventFunc			m_oEventFunc;

	protected:
	*/

		CFEHUDObject			m_oHUDObject = null;
	}
}
//-----------------------------------------------------------------------------
