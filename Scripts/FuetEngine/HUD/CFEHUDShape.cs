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
	public class CFEHUDShape : CFEHUDObject
	{
		// Make sure you provide a parameterless constructor.
		public CFEHUDShape(){}
		public CFEHUDShape(string _sName) : base(_sName) {}
	};
}
//-----------------------------------------------------------------------------
