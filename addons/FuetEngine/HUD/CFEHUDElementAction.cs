using System;
using System.Collections.Generic;
using Godot;
//-----------------------------------------------------------------------------
using CFEString = System.String;
using FEReal = System.Single;
using CFEVect2 = Godot.Vector2;
using CFEColor = Godot.Color;
// ----------------------------------------------------------------------------
namespace FuetEngine
{
	[Tool]
	public partial class CFEHUDElementAction : Node
	{
		public FEReal m_rActionTime = 0.0f;
		public FEReal m_rMaxActionTime = 0.0f;

		public CFEHUDElementAction(CFEString _sName)
		{
			Name = _sName;
		}
			
		/// Adds a new action into the HUD Element.
		public int iAddAction(CFEHUDObjectAction _oAction)
		{
			AddChild(_oAction);
			return GetChildCount() - 1;
		}
			
		/// Inserts a new action into the given position.
		public void InsertAction(CFEHUDObjectAction _oAction, int _iIdx)
		{
			GD.Print("TODO: Implement CFEHUDElementAction.InsertAction properly");
			/*
			if (_iIdx == 0)
			{
				Node firstNode = GetChild(0);
				AddChildBelowNode(_oAction, firstNode);
				RemoveChild(firstNode);
				AddChildBelowNode(firstNode, _oAction);
			}
			else
			{
				AddChildBelowNode(_oAction, GetChild(_iIdx-1));
			}
			*/
		}		

		/// Retrieves the action identified by the given index.
		public CFEHUDObjectAction oGetAction(int _iAction)
		{
			return GetChild<CFEHUDObjectAction>(_iAction);
		}

		/// Deletes a action in the HUD element.
		public void DeleteAction(int _iAction)
		{
			RemoveChild(GetChild(_iAction));
		}
		
		/// Swaps the two given actions
		public void Swap(uint _uiIdxA,uint _uiIdxB)
		{
			GD.Print("TODO: Implement CFEHUDElementAction.Swap properly");
			/*
			Node oAux = GetChild((int)_uiIdxA);
			ReplaceBy((int)_uiIdxA, GetChild((int)_uiIdxB));
			ReplaceBy((int)_uiIdxB, oAux);
			*/
		}

		/// Retrieves the number of actions in the current HUD element.
		public int iNumActions()
		{
			return GetChildCount();
		}

		/// Perform processing over the object
		public virtual void Accept(CFEHUDVisitor _oVisitor)
		{
			_oVisitor.Visit(this);
		}
		
		/// Sets the maximum time of the animation without looping or -1 if infinite (when looping).
		public void SetActionTime(float _rActionTime)
		{
			m_rActionTime = _rActionTime;
		}

		/// Retrieves the maximum time of the animation without looping or -1 if infinite (when looping)
		public float rGetActionTime()
		{
			return(m_rActionTime);
		}

		/// Sets the maximum time of the animation taking into account the length of looping cycles.
		public void SetMaxActionTime(float _rMaxActionTime)
		{
			m_rMaxActionTime = _rMaxActionTime;
		}

		/// Retrieves the maximum time of the animation taking into account the length of looping cycles.
		public float rGetMaxActionTime()
		{
			return m_rMaxActionTime;
		}
	}
}
//-----------------------------------------------------------------------------
