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
	public class CFEHUDLabel : CFEHUDObject 
	{
		// Make sure you provide a parameterless constructor.
		public CFEHUDLabel(){}
		public CFEHUDLabel(string _sName) : base(_sName) {}
	};
}
//-----------------------------------------------------------------------------
