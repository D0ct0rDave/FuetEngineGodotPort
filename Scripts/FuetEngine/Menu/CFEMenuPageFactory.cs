using System;
using Godot;
// ----------------------------------------------------------------------------
using CFEString = System.String;
// ----------------------------------------------------------------------------
namespace FuetEngine
{
	public class CFEMenuPageFactory
	{
		public CFEMenuPage Create(CFEString _sMenuPage)
		{
			/*
			if (_sMenuPage == "page_unlocked_pres")
				return(new CMYMP_UnlockedPres(_sMenuPage));

			else if (_sMenuPage == "page_mp_optionsel")
				return(new CMYMP_MP_OptionSel(_sMenuPage));

			else
			*/
			{
				GD.Print($"Falling back to default Behavior for {_sMenuPage} menu page!!!");
				return new CFEMenuPage(_sMenuPage);
			}
		}
	}
}
