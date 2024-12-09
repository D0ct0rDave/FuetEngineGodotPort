using Godot;
namespace FuetEngine
{
	//-----------------------------------------------------------------------------
    [Tool]
	public class CFEHUD : Node2D
	{
		// Make sure you provide a parameterless constructor.
		public CFEHUD(){}

		/// Adds a new element into the HUD.
		public int iAddElement(CFEHUDElement _oElement)
		{
			AddChild(_oElement);
			return GetChildCount()-1;
		}

		/// Retrieves the element identified by the given index.
		public CFEHUDElement oGetElement(int _iElement)
		{
			return GetChild<CFEHUDElement>(_iElement);
		}

		/// Retrieves the number of element in the HUD.
		public int iNumElements()
		{
			return GetChildCount();
		}

		/// Takes out an element from the HUD.
		public void DeleteElement(int _iElement)
		{
			RemoveChild(GetChild(_iElement));
		}

		/// Perform processing over the object
		public virtual void Accept(CFEHUDVisitor _oVisitor)
		{
			_oVisitor.Visit(this);
		}
	};
	//-----------------------------------------------------------------------------
}