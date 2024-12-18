using System;
using Godot;

namespace FuetEngine
{
	public class CFEMenuCheckBox : CFEMenuButton
	{
		private bool m_bChecked = false;
		// --------------------------------------------------------------------
		public CFEMenuCheckBox(CFEMenuPage _oOwner)
			: base(_oOwner)
		{
		}
		// --------------------------------------------------------------------
		public override void ProcessEvent(EFEMenuButtonEvent _eEventType)
		{
			base.ProcessEvent(_eEventType);

			if (_eEventType == CFEMenuButton.EFEMenuButtonEvent.MBE_INPUT_PRESS)
			{
				Check(!bIsChecked());
			}
		}
		// --------------------------------------------------------------------
		public virtual void Check(bool _bCheck = true)
		{
			if (!bIsEnabled()) return;

			m_bChecked = _bCheck;

			if (!bIsVisible()) return;

			SetInitialState(true);

			if (m_bChecked)
			{
				m_oHUDManager.iPlay(CFEMenuDefinitions.CHECK_BUTTON_EVENT_NAME, m_oObj);
				SetInitialState(false);
			}
			else
			{
				m_oHUDManager.iPlay(CFEMenuDefinitions.UNCHECK_BUTTON_EVENT_NAME, m_oObj);
			}
		}
		// --------------------------------------------------------------------
		/// Unchecks the button.
		public void UnCheck()
		{
			Check(false);
		}
		// --------------------------------------------------------------------
		/// Tells whether the check box is checked or not.
		public bool bIsChecked()
		{
			return m_bChecked;
		}
		// --------------------------------------------------------------------
	}
}
//-----------------------------------------------------------------------------
