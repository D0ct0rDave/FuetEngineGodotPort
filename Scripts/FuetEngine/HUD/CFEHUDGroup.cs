//-----------------------------------------------------------------------------
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
    public partial class CFEHUDGroup : CFEHUDObject
	{
		[Export]
		public FEReal m_rDepthFactor = 0.1f;

		// Make sure you provide a parameterless constructor.
		public CFEHUDGroup(){}
		/// Default constructor of the class.
		public CFEHUDGroup(CFEString _sName) : 
			base(_sName)
		{
		}

		/// Default constructor of the class.
		public CFEHUDGroup(CFEString _sName, int _uiReserveSize) : 
			base(_sName)
		{
		}

		/// Adds a new object to the group.
		public int iAddObject(CFEHUDObject _oObject)
		{
			AddChild(_oObject);
			return GetChildCount() - 1;
		}

		/// Inserts a new object into the group right after the given position.
		public void InsertObject(CFEHUDObject _oObject,uint _uiIdx)
		{
			GD.Print("TODO: Implement CFEHUDGroup.InsertObject properly");
			/*
			// m_oObjs.insert(m_oObjs.begin() + (long)_uiIdx, _poObject);
			*/
		}

		/// Sets the a given object.
		public void SetObject(uint _uiIdx,CFEHUDObject _oObject)
		{
			GD.Print("TODO: Implement CFEHUDGroup.SetObject properly");
			/*
			m_oObjs[_uiIdx] = _oObject;
			*/
		}

		/// Retrieves the index of the given object (if exists)
		public int iGetObjectIndex(CFEHUDObject _oObject)
		{
			for (int i=0; i<GetChildCount(); i++)
			{
				if (GetChild(i) == _oObject)
				{
					return i;
				}
			}
			return -1;
		}

		/// Swaps the two given elements
		public void Swap(uint _uiIdxA,uint _uiIdxB)
		{
			GD.Print("TODO: Implement CFEHUDGroup.Swap properly");
			/*
			CFEHUDObject* poAux = m_oObjs[_uiIdxA];
			m_oObjs[_uiIdxA] = m_oObjs[_uiIdxB];
			m_oObjs[_uiIdxB] = poAux;
			*/
		}

		/// Deletes an element identified by the given index of this group.
		public void DeleteObject(int _iIdx)
		{
			RemoveChild(GetChild(_iIdx));
		}

		/// Retrieves an element identified by the given index of this group.
		public CFEHUDObject oGetObject(int _iIdx)
		{
			return GetChild<CFEHUDObject>(_iIdx);
		}

		/// Retrieves the number of subojects belonging to this group.
		public int iNumObjs()
		{
			return GetChildCount();
		}
		
		/// Sets the depth factor used with the subobjects of this group.
		public void SetDepthFact(FEReal _rDepthFactor)
		{
			m_rDepthFactor = _rDepthFactor;
		}
		
		/// Retrieves the depth factor for this group.
		public FEReal rGetDepthFact()
		{
			return m_rDepthFactor;
		}

		/// Perform processing over the object
		public override void Accept(CFEHUDVisitor _oVisitor)
		{
			_oVisitor.Visit(this);
		}

	}
}
//-----------------------------------------------------------------------------
