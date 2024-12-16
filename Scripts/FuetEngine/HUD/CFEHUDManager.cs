using System;
using System.Collections.Generic;
using Godot;

//-----------------------------------------------------------------------------
using CFEString = System.String;
using FEReal = System.Single;
using CFEVect2 = Godot.Vector2;
using CFEColor = Godot.Color;
using CFEFont = Godot.Theme;
using CFEHUDElementAction = Godot.Animation;
//-----------------------------------------------------------------------------
namespace FuetEngine
{
	public class CFEHUDManager
	{
		//---------------------------------------------------------------------
		private class TElemAction
		{
			public CFEString m_sActionName;
			public bool m_bAutoHideAfterPlay;
			public FEReal m_rTime;
			public CFEHUDElementAction m_oAction;
			public CFEHUDObject m_oObject;
		};

		private List<TElemAction> m_oHUDActions = new List<TElemAction>();
		private List<TElemAction> m_oEnabledActions = new List<TElemAction>();
		CFEDictionary m_oDic = null;
		CFEHUD m_hud = null;

		private List<CFEHUDObject> m_oObjs = new List<CFEHUDObject>();
		FEReal m_rLastDeltaT = 0.0f;

		//---------------------------------------------------------------------
		public CFEHUDManager()
		{
		}
		//---------------------------------------------------------------------
		private void ProcessHUD(CFEHUD _oHUD)
		{
			// process objects.
			for (int e = 0; e < _oHUD.iNumElements(); e++)
			{
				CFEHUDElement oElem = _oHUD.oGetElement(e);
				if (oElem != null)
				{
					AddElementActions(oElem);
				}

				// Add the element to the list.
				m_oObjs.Add(oElem.oGetLayer(0));
			}
		}
		// -----------------------------------------------------------------------------
		private void AddElementActions(CFEHUDElement _oElem)
		{
			/// Add all the element actions to the table.
			for (int a = 0; a < _oElem.iNumActions(); a++)
			{
				CFEHUDElementAction oAction = _oElem.oGetAction(a);

				TElemAction elemAction = new TElemAction();
				elemAction.m_oAction = oAction;
				elemAction.m_oObject = _oElem.oGetLayer(0);
				elemAction.m_bAutoHideAfterPlay = false;
				elemAction.m_rTime = 0.0f;
				elemAction.m_sActionName = oAction.GetName();

				// add the action to the list.
				m_oHUDActions.Add(elemAction);
			}
		}
		// -----------------------------------------------------------------------------
		/*
		void SubstNode(CFEString _sParentNode, CFEString _sNodeToSubst, CFEHUDObject _oNewNode)
		{
			{
				CFEHUD hud = m_hud;
				if (hud != null)
				{
					/// Retrieves the element that matches with the specified name.
					CFEHUDElement oElem = null;
					CFEHUDGroup oGroup = null;

					CFEHUDElemLocator oElemLocator;
					oElem = oElemLocator.oLocateHUDElement(hud, _sParentNode);

					if (oElem != null)
					{
						oGroup = (CFEHUDGroup)oElemLocator.oLocateHUDObject(oElem, _sParentNode);

						if ((oElem != null) && (oGroup != null))
						{
							CFEHUDObject oOldObj = null;

							// Locate object.
							for (uint j = 0; j < oGroup.iNumObjs(); j++)
							{
								if (oGroup.oGetObject(j).Name.ToLower == _sNodeToSubst.ToLower)
								{
									oOldObj = oGroup.oGetObject(j);

									_oNewNode.SetIniPos(oOldObj.oGetIniPos());
									_oNewNode.SetIniScale(oOldObj.oGetIniScale());
									_oNewNode.SetIniAngle(oOldObj.rGetIniAngle());
									_oNewNode.SetIniColor(oOldObj.oGetIniColor());

									// Substitute node
									oGroup.SetObject(j, _oNewNode);

									oOldObj.Dispose();
									break;
								}
							}

							// Setup actions using the old object.
							for (int j = 0; j < oElem.iNumActions(); j++)
							{
								CFEHUDElementAction oAction = oElem.oGetAction(j);

								for (int k = 0; k < oAction.iNumActions(); k++)
								{
									if (oAction.oGetAction(k).oGetHUDObject() == oOldObj)
									{
										oAction.oGetAction(k).SetHUDObject(_oNewNode);
									}
								}
							}
						}
					}
				} // hHUD != null

			}
		}
		// -----------------------------------------------------------------------------
		void SubstSprite(FEHandler _hSpriteInstance, CFEString _sParentNode, CFEString _sNode)
		{
			CFEHUDIcon oNewIconNode = new CFEHUDIcon(_sNode);
			oNewIconNode.SetIcon(_hSpriteInstance);
			SubstNode(_sParentNode, _sNode, oNewIconNode);
		}
		*/
		// -----------------------------------------------------------------------------
		/// Initializes the HUD manager.
		private void Init(CFEHUD _hHUD, CFEDictionary _oDic)
		{
			Reset();

			m_oDic = _oDic;
			m_hud = _hHUD;

			if (m_oDic != null)
			{
				// TODO: CFEHUDTranslator.Translate(m_hud, m_oDic);
			}

			ProcessHUD(m_hud);
		}
		// -----------------------------------------------------------------------------
		public void Init(CFEString _sFilename)
		{
			/// load HUD
			CFEHUD hHUD = CFEHUDLoader.oLoad(_sFilename);
			if (hHUD == null) return;

			// translate / localize
			CFEDictionary oDic = new CFEDictionary(_sFilename);
			if (!oDic.bInitialized())
			{
				oDic = null;
			}

			Init(hHUD, oDic);
		}
		// -----------------------------------------------------------------------------
		public void Finish()
		{
			Reset();
		}
		// -----------------------------------------------------------------------------
		public void Reset()
		{
			// Delete HUDs
			if (m_hud != null)
			{
				m_hud.Dispose();
				m_hud = null;
			}

			// Delete dictionary
			if (m_oDic != null)
			{
				m_oDic = null;
			}

			// Delete HUD element actions
			for (int i = 0; i < m_oHUDActions.Count; i++)
			{
				m_oHUDActions[i] = null;
			}

			m_oHUDActions.Clear();
			m_oEnabledActions.Clear();
		}
		// -----------------------------------------------------------------------------
		private CFEHUDElement oGetElement(CFEString _sName)
		{
			if (m_hud != null)
			{
				CFEHUDElemLocator oElemLocator = new CFEHUDElemLocator();
				return oElemLocator.oLocateHUDElement(m_hud, _sName);
			}

			return null;
		}
		// -----------------------------------------------------------------------------
		private CFEHUDObject oGetObject(CFEString _sName)
		{
			if (m_hud != null)
			{
				return null; // CFEHUDInstMgr::poGetObject(hHUD, _sName);
			}

			return null;
		}
		// -----------------------------------------------------------------------------
		private CFEHUDElementAction oGetAction(CFEString _sName)
		{
			if (m_hud != null)
			{
				return null; // CFEHUDInstMgr::poGetElementAction(hHUD, _sName);
			}

			return null;
		}
		// -----------------------------------------------------------------------------
		private int iGetActionIdx(CFEString _sAction, CFEHUDObject _oObj)
		{
			if (_oObj == null)
			{
				for (int i = 0; i < m_oHUDActions.Count; i++)
				{

					if (m_oHUDActions[i].m_sActionName.ToLower() == _sAction.ToLower())
					{
						return i;
					}
				}
			}
			else
			{
				for (int i = 0; i < m_oHUDActions.Count; i++)
				{

					if ((m_oHUDActions[i].m_sActionName.ToLower() == _sAction) && (m_oHUDActions[i].m_oObject == _oObj))
					{
						return i;
					}
				}
			}

			return -1;
		}
		// -----------------------------------------------------------------------------
		public int iPlay(CFEString _sAction, CFEHUDObject _oObj, bool _bAutoShowBeforePlay, bool _bAutoHideAfterPlay)
		{
			if (_oObj == null)
			{
				// play on all the objects which have this action
				int iFoundIdx = -1;
				for (int i = 0; i < m_oHUDActions.Count; i++)
				{
					if (m_oHUDActions[i].m_sActionName.ToLower() == _sAction.ToLower())
					{
						// Stop all the animations being played by this object.
						StopObjectActions(m_oHUDActions[i].m_oObject);

#if ENABLE_MENU_EVENT_LOGGING
						CFESystem::Print("Playing %s action on %s object\n", _sAction.szString(), m_poHUDActions[i].m_poEA[0]->m_poObject->sGetName().szString());
#endif

						Play(i, _bAutoShowBeforePlay, _bAutoHideAfterPlay);

						if (iFoundIdx == -1)
						{
							iFoundIdx = i;
						}
					}
				}

				return iFoundIdx;
			}
			else
			{
				// Stop all the animations being played by this object.
				StopObjectActions(_oObj);

				// Now start the new anim.
				int iAction = iGetActionIdx(_sAction, _oObj);
				if (iAction != -1)
				{
					Play(iAction, _bAutoShowBeforePlay, _bAutoHideAfterPlay);
				}

				return iAction;
			}
		}
		// -----------------------------------------------------------------------------
		private void Play(int _iActionIdx, bool _bAutoShowBeforePlay, bool _bAutoHideAfterPlay)
		{
			if (_iActionIdx >= m_oHUDActions.Count) return;

			TElemAction oEA = m_oHUDActions[_iActionIdx];
			if ((oEA.m_oObject == null) || (oEA.m_oAction == null)) return;

			// One object cannot be playing several actions.
			Stop(_iActionIdx);

			// lo activamos o no lo activamos ?!?!?!
			if (_bAutoShowBeforePlay)
				oEA.m_oObject.ShowObj();

			oEA.m_bAutoHideAfterPlay = _bAutoHideAfterPlay;
			oEA.m_rTime = 0.0f;

			// Objects that doesn't have an associated object action should set their action default values.
			// TODO: CFEHUDUpdater::SetActionDefaultValues(poEA->m_poObject);
			// TODO: CFEHUDUpdater::RestartActions(poEA->m_poAction);
			// TODO: CFEHUDUpdater::Process(poEA->m_poAction, _0r);

			// Add to the list of enabled actions
			m_oEnabledActions.Add(oEA);
		}
		// -----------------------------------------------------------------------------
		// Helper to perform the same process on both stop functions.
		private void StopEnabledAction(int _iEnabledAction)
		{
			// Update action to show last second state.
			TElemAction oEA = m_oEnabledActions[_iEnabledAction];
			FEReal rLastTick = oEA.m_oAction.Length;

			if (rLastTick > 0.0f) // only for non - loopeable anims ...
			{
				// TODO: CFEHUDInstMgr::SetActionTime(poEA->m_poAction, poEA->m_poAction->rGetActionTime());
			}

			if (oEA.m_bAutoHideAfterPlay)
			{
				oEA.m_oObject.HideObj();
			}

			m_oEnabledActions.RemoveAt(_iEnabledAction);
		}
		// -----------------------------------------------------------------------------
		private void Stop(int _iActionIdx)
		{
			if (_iActionIdx >= m_oHUDActions.Count) return;

			TElemAction oEA = m_oHUDActions[_iActionIdx];
			if ((oEA.m_oObject == null) || (oEA.m_oAction == null)) return;

			/// Look inside the enabled actions list.
			for (int a = 0; a < m_oEnabledActions.Count; a++)
			{
				/// If the current enabled action is the element action we're looking for
				if (oEA == m_oEnabledActions[a])
				{
					StopEnabledAction(a);
					return;
				}
			}
		}
		// -----------------------------------------------------------------------------
		private void Stop(CFEString _sAction, CFEHUDObject _oObj)
		{
			if (_oObj == null)
			{
				/// Look inside the enabled actions list.
				for (int a = 0; a < m_oEnabledActions.Count;)
				{
					/// If the current enabled action is the element action we're looking for
					if (m_oEnabledActions[a].m_sActionName.ToLower() == _sAction.ToLower())
						StopEnabledAction(a);
					else
						a++;
				}
			}
			else
			{
				int iActionIdx = iGetActionIdx(_sAction, _oObj);
				if (iActionIdx != -1) Stop(iActionIdx);
			}
		}
		// -----------------------------------------------------------------------------
		public bool bPlaying(int _iActionIdx)
		{
			if (_iActionIdx >= m_oHUDActions.Count) return false;

			/// Look inside the enabled actions list.
			for (int a = 0; a < m_oEnabledActions.Count; a++)
			{
				/// If the current enabled action is the element action we're looking for
				if (m_oHUDActions[_iActionIdx] == m_oEnabledActions[a])
					return (true);
			}

			return (false);
		}
		// -----------------------------------------------------------------------------
		public bool bPlaying(CFEString _sAction, CFEHUDObject _oObj)
		{
			if (_oObj == null)
			{
				// look for all the objects which have this action
				for (int i = 0; i < m_oEnabledActions.Count; i++)
				{
					TElemAction oEA = m_oEnabledActions[i];
					if (oEA.m_oAction.GetName().ToLower() == _sAction.ToLower())
						return true;
				}

				return false;
			}
			else
			{
				int iActionIdx = iGetActionIdx(_sAction, _oObj);
				if (iActionIdx != -1) return (bPlaying(iActionIdx));

				return false;
			}
		}
		// -----------------------------------------------------------------------------
		public void Update(FEReal _rDeltaT)
		{
			for (int i = 0; i < m_oEnabledActions.Count;)
			{
				TElemAction oEA = m_oEnabledActions[i];
				FEReal rActionTime = oEA.m_oAction.Length;

				if ((oEA.m_rTime < rActionTime) || (rActionTime < 0.0f))
				{
					// To prevent overflows
					if (rActionTime < 0.0f)
					{
						// poEA->m_rTime = CFEMath::rMod(poEA->m_rTime + _rDeltaT,poEA->m_poAction->rGetMaxActionTime());
						oEA.m_rTime += _rDeltaT;
					}
					else
					{
						oEA.m_rTime = Mathf.Min(oEA.m_rTime + _rDeltaT, oEA.m_oAction.Length);
					}

					// TODO: CFEHUDInstMgr::SetActionTime(poEA->m_poAction, poEA->m_rTime);
					i++;
				}
				else
				{
					// Update action to show last second state.
					// TODO: CFEHUDInstMgr::SetActionTime(poEA->m_poAction, rActionTime);

					if (oEA.m_bAutoHideAfterPlay && (oEA.m_oObject != null))
						oEA.m_oObject.HideObj();

					m_oEnabledActions.RemoveAt(i);
				}
			}

			// TODO: CFEHUDInstMgr::Update(hHUD, _rDeltaT);
			m_rLastDeltaT = _rDeltaT;
		}
		// -----------------------------------------------------------------------------
		public CFEString sGetString(CFEString _sVariable, CFEString _sDefaultValue)
		{
			if ((m_oDic != null) && (m_oDic.bExists(_sVariable)))
				return m_oDic.sGetString(_sVariable, _sDefaultValue);
			else
				return _sDefaultValue;
		}
		// -----------------------------------------------------------------------------
		public void StopObjectActions(CFEHUDObject _oObj)
		{
			// Stop all the actions of being played by this object.
			for (int i = 0; i < m_oHUDActions.Count; i++)
			{
				if ((_oObj == null) || (m_oHUDActions[i].m_oObject == _oObj))
					Stop(i);
			}
		}
	}
}
// -----------------------------------------------------------------------------
