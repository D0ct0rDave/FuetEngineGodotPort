using System;
using System.Collections.Generic;
using Godot;

namespace FuetEngine
{
	public class CFEMenuOptionButton : CFEMenuCheckBox
	{
		/// All this button siblings.
		private List<CFEMenuOptionButton> m_oSiblings = new List<CFEMenuOptionButton>();

		// --------------------------------------------------------------------
		public CFEMenuOptionButton(CFEMenuPage _oOwner, List<CFEMenuOptionButton> _oSiblings)
			: base(_oOwner)
		{
			m_oSiblings = _oSiblings;
		}
		// --------------------------------------------------------------------
		public void UncheckRest()
		{
			for (int i = 0; i < m_oSiblings.Count; i++)
			{
				if ((m_oSiblings[i] != this) && m_oSiblings[i].bIsChecked() && m_oSiblings[i].bIsEnabled())
				{
					m_oSiblings[i].UnCheck();
				}
			}
		}
		//-----------------------------------------------------------------------------
		/// Checks or unchecks the button.
		public override void Check(bool _bCheck = true)
		{
			base.Check(_bCheck);
			UncheckRest();
		}
		//-----------------------------------------------------------------------------
		protected override void ProcessEvent(EFEMenuButtonEvent _eEventType)
		{
			if (_eEventType == CFEMenuButton.EFEMenuButtonEvent.MBE_INPUT_PRESS)
			{
				// simluate always input press event
				base.ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_INPUT_PRESS);
				Check();
			}

			/// To disociate SELECTION with Checking comment the following elseif block
			/*
			else if (_eEventType == MBE_SELECT)
			{
				if (uiGetState() != MBS_NONE)
				{
					// CFEMenuCheckBox::ProcessEvent(MBE_INPUT_PRESS);	
					// m_poOwner->OnButtonPress(this);

					Check();
					UncheckRest();	

					CFEMenuCheckBox::ProcessEvent(MBE_SELECT);	
					m_poOwner->OnButtonEnter(this);
				}
				else
				{
					CFEMenuCheckBox::ProcessEvent(MBE_SELECT);	
				}
			}
			*/
			else
			{
				base.ProcessEvent(_eEventType);
			}
		}
		// --------------------------------------------------------------------
	}
}
