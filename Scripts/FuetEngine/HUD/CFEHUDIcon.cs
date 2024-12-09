using Godot;
//-----------------------------------------------------------------------------
namespace FuetEngine
{
    [Tool]
	public class CFEHUDIcon : CFEHUDObject 
	{
		// Make sure you provide a parameterless constructor.
		public CFEHUDIcon()
		{
			Name = "CFEHUDIcon";
		}

		public void SetIcon(CFESpriteInstance _spriteInstance)
		{
			while (GetChildCount()>0)
			{
				Node node = GetChild(0);
				
				RemoveChild(node);
				node.QueueFree();
			}
			AddChild(_spriteInstance);
		}

		/// Sets the image for this icon Object.
		public CFESpriteInstance GetIcon()
		{
			return GetChild(0) as CFESpriteInstance;
		}

		/// Perform processing over the object
		public override void Accept(CFEHUDVisitor _oVisitor)
		{
			_oVisitor.Visit(this);
		}
	};
}
//-----------------------------------------------------------------------------
