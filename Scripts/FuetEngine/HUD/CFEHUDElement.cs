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
	public partial class CFEHUDElement : Node2D
	{
		/// Default constructor of this element
		public CFEHUDElement(CFEString _sName)
		{
			Name = _sName;

			m_oLayers = new Node2D();
			m_oLayers.Name = "Layers";
			AddChild(m_oLayers);

			m_oActions = new Node();
			m_oActions.Name = "Action";
			AddChild(m_oActions);
		}

		/// Inserts an action at the given position
		public void InsertAction(uint _uiIdx, CFEHUDElementAction _oAction)
		{
			GD.Print("TODO: Implement CFEHUDElement.InsertAction properly");
			/*
			m_oActions.Insert(m_oActions.begin() + (long)_uiIdx, _oAction);
			*/
		}

		/// Adds a new action into the HUD Element.
		public int iAddAction(CFEHUDElementAction _oAction)
		{
			m_oActions.AddChild(_oAction);
			return m_oActions.GetChildCount()-1;
		}

		/// Retrieves the action identified by the given index.
		public CFEHUDElementAction oGetAction(int _iAction)
		{
			return m_oActions.GetChild<CFEHUDElementAction>(_iAction);
		}

		/// Deletes a action in the HUD element.
		public void DeleteAction(int _iAction)
		{
			m_oActions.RemoveChild(m_oActions.GetChild(_iAction));
		}

		/// Swap the contents of layer A and B.
		public void SwapActions(uint _uiActionA, uint _uiActionB)
		{
			GD.Print("TODO: Implement CFEHUDElement.SwapActions properly");
			// std::swap(m_oActions[_uiActionA],m_oActions[_uiActionB]);
		}

		/// Retrieves the number of actions in the current HUD element.
		public int iNumActions()
		{
			return m_oActions.GetChildCount();
		}

		/// Adds a new layer into the HUD Element.
		public int iAddLayer(CFEHUDObject _oLayer)
		{
			m_oLayers.AddChild(_oLayer);
			return m_oLayers.GetChildCount();
		}

		/// Retrieves the layer identified by the given index.
		public CFEHUDObject oGetLayer(int _iLayer)
		{
			return m_oLayers.GetChild<CFEHUDObject>(_iLayer);
		}

		/// Deletes a layer in the HUD element.
		public void DeleteLayer(int _iLayer)
		{
			m_oLayers.RemoveChild(m_oLayers.GetChild(_iLayer));
		}

		/// Swap the contents of layer A and B.
		public void SwapLayers(uint _uiLayerA,uint _uiLayerB)
		{
			GD.Print("TODO: Implement CFEHUDElement.SwapLayers properly");
			// std::swap(m_oLayers[_uiLayerA],m_oLayers[_uiLayerB]);
		}

		/// Retrieves the number of layers in the HUD element.
		public int iNumLayers()
		{
			return m_oLayers.GetChildCount();
		}

		/// Perform processing over the object
		public virtual void Accept(CFEHUDVisitor _oVisitor)
		{
			_oVisitor.Visit(this);
		}

		protected Node2D m_oLayers;
		protected Node m_oActions;
	}
}
//-----------------------------------------------------------------------------
