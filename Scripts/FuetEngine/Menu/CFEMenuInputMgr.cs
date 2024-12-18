
using System;
using Godot;
// --------------------------------------------------------------------
namespace FuetEngine
{
	// --------------------------------------------------------------------
	public class CFEMenuInputMgr : Node
	{
		// --------------------------------------------------------------------
		public enum EFECursorResult
		{
			CR_NONE,
			CR_CURSOR_OVER,             // cursor over the surface
			CR_CURSOR_OVER_PRESS,       // cursor over the surface and pressing
			CR_NUM_CURSOR_RESULTS
		}
		// --------------------------------------------------------------------
		// These buttons should be mapped into the project properties
		// -------------------------------------------------------------------- 
		public enum EFEInputButton
		{
			IB_UP,          // digital up.
			IB_DOWN,        // digital down.
			IB_LEFT,        // digital left.
			IB_RIGHT,       // digital right.

			IB_MIG,         // Menu in game button.

			IB_A,           // Generic buttons.
			IB_B,
			IB_C,
			IB_D,
			IB_E,
			IB_F,
			IB_G,
			IB_H,
			IB_I,
			IB_J,
			IB_K,

			IB_NUM_BUTTONS
		}
		enum EFEInputPressureButton
		{
			IPB_A,
			IPB_B,
			IPB_C,

			IPB_NUMIPBS,
		}
		// --------------------------------------------------------------------
		private bool m_bEnableCI;
		private bool m_bEnableBI;
		private Vector2 m_oCursorPos = new Vector2(0.0f, 0.0f);
		// --------------------------------------------------------------------
		public CFEMenuInputMgr()
		{
		}
		// --------------------------------------------------------------------
		public override void _Input(InputEvent _event)
		{
			InputEventMouseMotion mouseMotionEvent = _event as InputEventMouseMotion;
			if (mouseMotionEvent != null)
			{
				m_oCursorPos = mouseMotionEvent.Relative; //Absolute; // 
			}

			// Receives mouse button input
			InputEventMouseButton mouseButtonEvent = _event as InputEventMouseButton;
		}
		// --------------------------------------------------------------------
		public EFECursorResult eTestCursor(CFERect _oRect)
		{
			if (m_bEnableCI == false) return EFECursorResult.CR_NONE;

			EFECursorResult eRes = EFECursorResult.CR_NONE;
			if (CFEMath.bInside(_oRect, m_oCursorPos))
			{
				if (!Input.IsActionPressed(EFEInputPressureButton.IPB_A.ToString())
				  && !Input.IsActionPressed(EFEInputPressureButton.IPB_B.ToString()))
				{
					eRes = EFECursorResult.CR_CURSOR_OVER;
					// continue test to see if someone else is pressing the button.
				}
				else
				{
					eRes = EFECursorResult.CR_CURSOR_OVER_PRESS;

					// return asap
					return eRes;
				}
			}

			return eRes;
		}
		// --------------------------------------------------------------------
		public bool bDown(EFEInputButton _eButton)
		{
			if (m_bEnableBI == false) return (false);
			return Input.IsActionPressed(_eButton.ToString());
		}
		// --------------------------------------------------------------------
		public bool bUp(EFEInputButton _eButton)
		{
			if (m_bEnableBI == false) return (false);
			return !Input.IsActionPressed(_eButton.ToString());
		}
		// --------------------------------------------------------------------
		/// Enables/Disables input from the cursor
		public void EnableCursorInput(bool _bEnable)
		{
			m_bEnableCI = _bEnable;
		}

		/// Retrieves wether input from the cursor is enabled or not
		public bool bIsCursorInputEnabled()
		{
			return m_bEnableCI;
		}

		/// Enables/Disables input from the buttons
		public void EnableButtonInput(bool _bEnable)
		{
			m_bEnableBI = _bEnable;
		}

		/// Retrieves wether input from the buttons are enabled or not
		public bool bIsButtonInputEnabled()
		{
			return m_bEnableBI;
		}
	}
	// ------------------------------------------------------------------------
}
