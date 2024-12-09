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
		public class CFEHUDPSys : CFEHUDObject 
	{
		// Make sure you provide a parameterless constructor.
		public CFEHUDPSys(){}
		public CFEHUDPSys(string _sName) : base(_sName) {}
	};
}
//-----------------------------------------------------------------------------
