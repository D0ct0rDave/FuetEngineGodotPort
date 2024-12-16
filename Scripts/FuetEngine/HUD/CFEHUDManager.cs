using System;
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
	class CFEHUDManager
	{
		//---------------------------------------------------------------------
		class TElemAction
		{

	        CFEString				m_sActionName;

			FEBool					m_bAutoHideAfterPlay;
	        FEReal					m_rTime;

	        CFEHUDElementAction*	m_poAction;
			CFEHUDObject*			m_poObject;

		};
	
		private List<TElemAction>	m_poHUDActions;
        private List<TElemAction>	m_poEnabledActions;
		CFEDictionary				m_poDic;

		#ifdef DUAL_SCREEN
		CFEArray<FEHandler>			m_hHUD;
		#else
		FEHandler					m_hHUD;
		#endif

		CFEArray<CFEHUDObject*> m_poObjs;
		FEReal						m_rLastDeltaT;

		//---------------------------------------------------------------------
		CFEHUDManager() :
			m_poDic(NULL)

			#ifndef DUAL_SCREEN
			,m_hHUD(NULL)
			#endif

			,m_rLastDeltaT(_0r)
		{
			
		}
		//---------------------------------------------------------------------
		~CFEHUDManager()
		{
			m_poObjs.clear();
			Reset();
		
		//---------------------------------------------------------------------
		void CFEHUDManager::ProcessHUD(CFEHUD* _poHUD)
		{
			// process objects.
			for (uint e=0;e<_poHUD->uiNumElements();e++)
			{
				CFEHUDElement* poElem = _poHUD->poGetElement(e);

				if (poElem != NULL)
					AddElementActions(poElem);

				// Add the element to the list.
				m_poObjs.push_back(poElem->poGetLayer(0));
			}
		}
		// -----------------------------------------------------------------------------
		void CFEHUDManager::AddElementActions(CFEHUDElement* _poElem)
		{
			/// Add all the element actions to the table.
			for (uint a=0;a<_poElem->uiNumActions();a++)
			{
				CFEHUDElementAction* poAction = _poElem->poGetAction(a);

				TElemAction* poEA	= new TElemAction;
				poEA->m_poAction	= poAction;
				poEA->m_poObject	= _poElem->poGetLayer(0);
				poEA->m_bAutoHideAfterPlay = false;
				poEA->m_rTime		= _0r;
				poEA->m_sActionName = poAction->sGetName();

				// add the action to the list.
				m_poHUDActions.push_back(poEA);
			}
		}
		// -----------------------------------------------------------------------------
		void CFEHUDManager::SubstNode(const CFEString& _sParentNode,const CFEString& _sNodeToSubst,CFEHUDObject* _poNewNode)
		{
			#ifdef DUAL_SCREEN
			for (uint i=0;i<m_hHUD.size();i++)
			{
				FEHandler hHUD = m_hHUD[i];		
				
			#else
			{
				FEHandler hHUD = m_hHUD;
			#endif
				
				if (hHUD != NULL)
				{
					/// Retrieves the element that matches with the specified name.
					CFEHUDElement* poElem = NULL;
					CFEHUDGroup* poGroup  = NULL;

					CFEHUDElemLocator oElemLocator;
					poElem = oElemLocator.poLocateHUDElement((CFEHUD*)hHUD,_sParentNode);

					if (poElem != NULL)
					{
						poGroup = (CFEHUDGroup*)oElemLocator.poLocateHUDObject(poElem,_sParentNode);

						if ((poElem!=NULL) && (poGroup!=NULL))
						{
							CFEHUDObject* poOldObj = NULL;
					
							// Locate object.
							for (uint j=0;j<poGroup->uiNumObjs();j++)
							{
								if (poGroup->poGetObject(j)->sGetName() |= _sNodeToSubst)
								{
									poOldObj = poGroup->poGetObject(j);

									_poNewNode->SetIniPos(poOldObj->oGetIniPos());
									_poNewNode->SetIniScale(poOldObj->oGetIniScale());
									_poNewNode->SetIniAngle(poOldObj->rGetIniAngle());
									_poNewNode->SetIniColor(poOldObj->oGetIniColor());

									// Substitue node
									poGroup->SetObject(j,_poNewNode);

									delete poOldObj;
									break;
								}
							}

							// Setup actions using the old object.
							for (uint j=0;j<poElem->uiNumActions();j++)
							{
								CFEHUDElementAction* poAction = poElem->poGetAction(j);

								for (uint k=0;k<poAction->uiNumActions();k++)
								{
									if (poAction->poGetAction(k)->poGetHUDObject() == poOldObj)
										poAction->poGetAction(k)->SetHUDObject( _poNewNode );
								}
							}
						}
					}
				} // hHUD != null

			}
		}
		// -----------------------------------------------------------------------------
		void CFEHUDManager::SubstSprite(FEHandler _hSpriteInstance,const CFEString& _sParentNode,const CFEString& _sNode)
		{
			CFEHUDIcon* poNewIconNode = new CFEHUDIcon(_sNode);
			poNewIconNode->SetIcon(_hSpriteInstance);
			SubstNode(_sParentNode,_sNode,poNewIconNode);
		}
		// -----------------------------------------------------------------------------
		/// Initializes the HUD manager.
		void CFEHUDManager::Init(FEHandler _hHUD,CFEDictionary* _poDic,FEBool _bMultiScreen)
		{
			Reset();

			m_poDic = _poDic;
			CFEHUD* poHUD = CFEHUDInstMgr::poGetHUD(_hHUD);

			if (m_poDic != NULL)
				CFEHUDTranslator::Translate(poHUD, m_poDic);

			ProcessHUD(poHUD);

		#ifdef DUAL_SCREEN

			if (! _bMultiScreen)
			{
				m_hHUD.push_back(_hHUD);
				return;
			}

			// Split HUD Elements between screens.
			m_hHUD.push_back(new CFEHUD);
			m_hHUD.push_back(new CFEHUD);

			while (poHUD->uiNumElements()>0)
			{
				CFEHUDElement* poElem = poHUD->poGetElement(0);

				CFEHUDRectGen oGen((FEPointer)poElem);
				oGen.Visit(poElem);
				CFERect oRect = oGen.oGetRect();

				// % del objeto que est� en hud y en el otro
				FEReal rHUD0Fact = _0r;
				FEReal rHUD1Fact = _0r;

				// compute rHUD0Fact
				if (oRect.m_oEnd.y <= DESIGN_SCREEN_HEIGHT)
					rHUD0Fact = _1r;
				else
				{
					if (oRect.m_oIni.y >= DESIGN_SCREEN_HEIGHT)
						rHUD0Fact = _0r;
					else
						// trozo de objeto que est� en la pantalla 1
						rHUD0Fact = (DESIGN_SCREEN_HEIGHT - oRect.m_oIni.y) / oRect.rHeight();
				}

				// compute rHUD1Fact
				if (oRect.m_oIni.y >= DESIGN_SCREEN_HEIGHT)
					rHUD1Fact = _1r;
				else
				{
					if (oRect.m_oEnd.y <= DESIGN_SCREEN_HEIGHT)
						rHUD1Fact = _0r;
					else
						// trozo de objeto que est� en la pantalla 2
						rHUD1Fact = (oRect.m_oEnd.y - DESIGN_SCREEN_HEIGHT) / oRect.rHeight();
				}

				// En teor�a no puede ir un mismo elemento a los 2 huds si no peta a la hora de destruir el HUD.
				// DMC 25-01-2016: Tratamos esta posibilidad en el Finish...
				if (rHUD0Fact > 0.1f)
					((CFEHUD*)m_hHUD.at(0))->uiAddElement(poElem);

				if (rHUD1Fact > 0.1f)
					((CFEHUD*)m_hHUD.at(1))->uiAddElement(poElem);

				/*
				if (rHUD0Fact>rHUD1Fact)
				((CFEHUD*)m_hHUD.at(0))->uiAddElement(poElem);
				else
				((CFEHUD*)m_hHUD.at(1))->uiAddElement(poElem);
				*/

				poHUD->DeleteElement(0);
			}

			delete poHUD;

		#else
			m_hHUD = _hHUD;
		#endif
		}
		// -----------------------------------------------------------------------------
		void CFEHUDManager::Init(const CFEString& _sFilename,FEBool _bMultiScreen)
		{
			/// load HUD
			FEHandler hHUD = CFEHUDInstMgr::hGetInstance(_sFilename);
			if (hHUD == NULL) return;

			// translate / localize
			CFEDictionary* poDic = new CFEDictionary(_sFilename);
			if (! poDic->bInitialized())
			{
				delete poDic;
				poDic = NULL;
			}

			Init(hHUD, poDic,_bMultiScreen);
		}
		// -----------------------------------------------------------------------------
		void CFEHUDManager::Finish()
		{
			Reset();
		}
		// -----------------------------------------------------------------------------
		void CFEHUDManager::Reset()
		{
			// Delete HUDs
			#ifdef DUAL_SCREEN
			
				// DMC 25-01-2016: Process HUDs to find duplicated elements and delete them to avoid 
				// the destruction of the same element 2 times...
				if (m_hHUD.size()==2)
				{
					CFEHUD* poHUD0 = CFEHUDInstMgr::poGetHUD(m_hHUD[0]);
					CFEHUD* poHUD1 = CFEHUDInstMgr::poGetHUD(m_hHUD[1]);

					if ((poHUD0 != NULL) && (poHUD1!=NULL))
					{
						for (uint e0=0;e0<poHUD0->uiNumElements();e0++)
						{
							for (uint e1=0;e1<poHUD1->uiNumElements();)
							{
								// if the element in HUD1 is the same as the current in HUD0
								if (poHUD0->poGetElement(e0) == poHUD1->poGetElement(e1))
								{
									// Delete this elemento from HUD1
									poHUD1->DeleteElement(e1);
									break;
								}
								else
									e1++;
							}
						}
					}
				}

				/// Free HUD resources
				for (uint i=0;i<m_hHUD.size();i++)
				{
					CFEHUD* poHUD = CFEHUDInstMgr::poGetHUD(m_hHUD[i]);
					if (poHUD != NULL)
						delete poHUD;
				}
				m_hHUD.clear();
			#else
				CFEHUD* poHUD = CFEHUDInstMgr::poGetHUD(m_hHUD);
				if (poHUD != NULL)
					delete poHUD;
				m_hHUD = NULL;
			#endif

			// Delete dictionary
			if (m_poDic != NULL)
			{
				delete m_poDic;
				m_poDic = NULL;
			}

			// Delete HUD element actions
			for (uint i=0;i<m_poHUDActions.size();i++)
				delete m_poHUDActions[i];

			m_poHUDActions.clear();
			m_poEnabledActions.clear();
		}
		// -----------------------------------------------------------------------------
		CFEHUDElement* CFEHUDManager::poGetElement(const CFEString& _sName)
		{
			CFEHUDElemLocator oElemLocator;

			#ifdef DUAL_SCREEN
			for (uint i=0;i<m_hHUD.size();i++)
			{
				FEHandler hHUD = m_hHUD[i];
			#else
			{
				FEHandler hHUD = m_hHUD;
			#endif
				if (hHUD != NULL)
				{
					CFEHUDElement* poElem = oElemLocator.poLocateHUDElement((CFEHUD*)hHUD,_sName);
					if (poElem != NULL) return(poElem);
				}
			}

			return(NULL);
		}
		// -----------------------------------------------------------------------------
		CFEHUDObject* CFEHUDManager::poGetObject(const CFEString& _sName)
		{
			#ifdef DUAL_SCREEN
			for (uint i=0;i<m_hHUD.size();i++)
			{
				FEHandler hHUD = m_hHUD[i];
			#else
			{
				FEHandler hHUD = m_hHUD;
			#endif
				if (hHUD != NULL)
				{
					CFEHUDObject* poObj = CFEHUDInstMgr::poGetObject(hHUD,_sName);
					if (poObj!=NULL) return(poObj);
				}
			}

			return(NULL);
		}
		// -----------------------------------------------------------------------------
		CFEHUDElementAction* CFEHUDManager::poGetAction(const CFEString& _sName)
		{
			#ifdef DUAL_SCREEN
			for (uint i=0;i<m_hHUD.size();i++)
			{
				FEHandler hHUD = m_hHUD[i];
			#else
			{
				FEHandler hHUD = m_hHUD;
			#endif
				if (hHUD != NULL)
				{
					CFEHUDElementAction* poAction = CFEHUDInstMgr::poGetElementAction(hHUD,_sName);
					if (poAction != NULL) return(poAction);
				}
			}

			return(NULL);
		}
		// -----------------------------------------------------------------------------
		int CFEHUDManager::iGetActionIdx(const CFEString& _sAction,CFEHUDObject* _poObj)
		{
			if (_poObj == NULL)
			{
				for (uint i=0;i<m_poHUDActions.size();i++)
					if (m_poHUDActions[i]->m_sActionName |= _sAction)
						return(i);
			}
			else
			{
				for (uint i=0;i<m_poHUDActions.size();i++)
					if ((m_poHUDActions[i]->m_sActionName |= _sAction) && (m_poHUDActions[i]->m_poObject == _poObj))
						return(i);
			}

			return(-1);
		}
		// -----------------------------------------------------------------------------
		int CFEHUDManager::iPlay(const CFEString& _sAction,CFEHUDObject* _poObj,FEBool _bAutoShowBeforePlay,FEBool _bAutoHideAfterPlay)
		{
			if (_poObj == NULL)
			{
				// play on all the objects which have this action
				int iFoundIdx = -1;

				for (uint i=0;i<m_poHUDActions.size();i++)
				{
					if (m_poHUDActions[i]->m_sActionName |= _sAction)
					{
						// Stop all the animations being played by this object.
						StopObjectActions(m_poHUDActions[i]->m_poObject);

						#ifdef ENABLE_MENU_EVENT_LOGGING
						CFESystem::Print("Playing %s action on %s object\n",_sAction.szString(),m_poHUDActions[i].m_poEA[0]->m_poObject->sGetName().szString());
						#endif

						Play(i,_bAutoShowBeforePlay,_bAutoHideAfterPlay);

						if (iFoundIdx == -1)
							iFoundIdx = i;
					}
				}

				return(iFoundIdx);
			}
			else
			{
				// Stop all the animations being played by this object.
				StopObjectActions(_poObj);

				// Now start the new anim.
				int iAction = iGetActionIdx(_sAction,_poObj);

				if (iAction != -1)
					Play(iAction,_bAutoShowBeforePlay,_bAutoHideAfterPlay);

				return(iAction);
			}
		}
		// -----------------------------------------------------------------------------
		void CFEHUDManager::Play(uint _uiActionIdx,FEBool _bAutoShowBeforePlay,FEBool _bAutoHideAfterPlay)
		{
			if (_uiActionIdx >= m_poHUDActions.size()) return;

			TElemAction* poEA = m_poHUDActions[_uiActionIdx];
			if ((poEA->m_poObject == NULL) || (poEA->m_poAction == NULL)) return;

			// One object cannot be playing several actions.
			Stop(_uiActionIdx);

			// lo activamos o no lo activamos ?!?!?!
			if (_bAutoShowBeforePlay)
				poEA->m_poObject->ShowObj();

			poEA->m_bAutoHideAfterPlay = _bAutoHideAfterPlay;
			poEA->m_rTime			   = _0r;

			// Objects that doesn't have an associated object action should set their action default values.
			CFEHUDUpdater::SetActionDefaultValues(poEA->m_poObject);
			CFEHUDUpdater::RestartActions(poEA->m_poAction);
			CFEHUDUpdater::Process(poEA->m_poAction,_0r);

			// Add to the list of enabled actions
			m_poEnabledActions.push_back(poEA);
		}
		// -----------------------------------------------------------------------------
		// Helper to perform the same process on both stop functions.
		void CFEHUDManager::StopEnabledAction(uint _uiEnabledAction)
		{
			// Update action to show last second state.
			TElemAction* poEA = m_poEnabledActions[_uiEnabledAction];
			FEReal rLastTick = poEA->m_poAction->rGetActionTime();

			if (rLastTick > _0r) // only for non - loopeable anims ...
				CFEHUDInstMgr::SetActionTime(poEA->m_poAction,poEA->m_poAction->rGetActionTime());

			if (poEA->m_bAutoHideAfterPlay)
				poEA->m_poObject->HideObj();

			m_poEnabledActions.Delete(_uiEnabledAction);
		}
		// -----------------------------------------------------------------------------
		void CFEHUDManager::Stop(uint _uiActionIdx)
		{
			if (_uiActionIdx>=m_poHUDActions.size()) return;
			
			TElemAction* poEA = m_poHUDActions[_uiActionIdx];
			if ((poEA->m_poObject == NULL) || (poEA->m_poAction == NULL)) return;

			/// Look inside the enabled actions list.
			for (uint a=0;a<m_poEnabledActions.size();a++)
			{
				/// If the current enabled action is the element action we're looking for
				if (poEA == m_poEnabledActions[a])
				{
					StopEnabledAction(a);
					return;
				}
			}
		}
		// -----------------------------------------------------------------------------
		void CFEHUDManager::Stop(const CFEString& _sAction,CFEHUDObject* _poObj)
		{
			if (_poObj == NULL)
			{
				/// Look inside the enabled actions list.
				for (uint a=0;a<m_poEnabledActions.size();)
				{
					/// If the current enabled action is the element action we're looking for
					if (m_poEnabledActions[a]->m_sActionName |= _sAction)
						StopEnabledAction(a);
					else
						a++;
				}
			}	
			else
			{
				int iActionIdx = iGetActionIdx(_sAction,_poObj);
				if (iActionIdx != -1) Stop(iActionIdx);
			}
		}
		// -----------------------------------------------------------------------------
		FEBool CFEHUDManager::bPlaying(uint _uiActionIdx)
		{
			if (_uiActionIdx>=m_poHUDActions.size()) return(false);

			/// Look inside the enabled actions list.
			for (uint a=0;a<m_poEnabledActions.size();a++)
			{
				/// If the current enabled action is the element action we're looking for
				if (m_poHUDActions[_uiActionIdx] == m_poEnabledActions[a])
					return(true);
			}

			return (false);
		}
		// -----------------------------------------------------------------------------
		FEBool CFEHUDManager::bPlaying(const CFEString& _sAction,CFEHUDObject* _poObj)
		{
			if (_poObj == NULL)
			{
				// look for all the objects which have this action
				for (uint i=0;i<m_poEnabledActions.size();i++)
				{
					TElemAction* poEA = m_poEnabledActions[i];
					if (poEA->m_poAction->sGetName() |= _sAction)
						return(true);
				}

				return(false);
			}	
			else
			{
				int iActionIdx = iGetActionIdx(_sAction,_poObj);
				if (iActionIdx != -1) return(bPlaying(iActionIdx));

				return(false);
			}
		}
		// -----------------------------------------------------------------------------
		void CFEHUDManager::Update(FEReal _rDeltaT)
		{
			for (uint i=0;i<m_poEnabledActions.size();)
			{
				TElemAction* poEA  = m_poEnabledActions[i];
				FEReal rActionTime = poEA->m_poAction->rGetActionTime();

				if ((poEA->m_rTime < rActionTime) || (rActionTime<_0r))
				{
					// To prevent overflows
					if (rActionTime<_0r)
					{
						// poEA->m_rTime = CFEMath::rMod(poEA->m_rTime + _rDeltaT,poEA->m_poAction->rGetMaxActionTime());
						poEA->m_rTime += _rDeltaT;
					}
					else
						poEA->m_rTime = CFEMath::rMin(poEA->m_rTime + _rDeltaT,poEA->m_poAction->rGetMaxActionTime());

					CFEHUDInstMgr::SetActionTime(poEA->m_poAction,poEA->m_rTime);
					i++;
				}
				else
				{
					// Update action to show last second state.
					CFEHUDInstMgr::SetActionTime(poEA->m_poAction,rActionTime);

					if (poEA->m_bAutoHideAfterPlay && (poEA->m_poObject!=NULL))
						poEA->m_poObject->HideObj();
			
					m_poEnabledActions.Delete(i);
				}
			}

			#ifdef DUAL_SCREEN
			for (uint i=0;i<m_hHUD.size();i++)
			{
				FEHandler hHUD = m_hHUD[i];		
			#else
			{
				FEHandler hHUD = m_hHUD;
			#endif
				if (hHUD != NULL)
				{			
					CFEHUDInstMgr::Update(hHUD,_rDeltaT);
				}
			}

			m_rLastDeltaT = _rDeltaT;
		}
		// ---------------------------------------------------------------------------
		void CFEHUDManager::Render(CFERenderer* _poRenderer)
		{
			FEHandler hHUD;

			#ifdef DUAL_SCREEN
				if (m_hHUD.size() == 0) return;
				if (m_hHUD.size() == 1)
					hHUD = m_hHUD[0];
				else
				{
					CFEVect2 oVT = _poRenderer->oGetViewTranslation();
					hHUD = (oVT.y < DESIGN_SCREEN_HEIGHT)?m_hHUD[0]:m_hHUD[1];
				}
			#else
				hHUD = m_hHUD;
			#endif

			if (hHUD == NULL) return;

			CFEHUDInstMgr::Update(m_rLastDeltaT);
			CFEHUDInstMgr::Render(hHUD,_poRenderer);
			m_rLastDeltaT = _0r;
		}
		// -----------------------------------------------------------------------------
		CFEString CFEHUDManager::sGetString(const CFEString& _sVariable,const CFEString& _sDefaultValue)
		{
			if ( (m_poDic != NULL) && (m_poDic->bExists(_sVariable)) )
				return(m_poDic->sGetString(_sVariable,_sDefaultValue));
			else
				return(_sDefaultValue);
		}
		// -----------------------------------------------------------------------------
		void CFEHUDManager::StopObjectActions(CFEHUDObject* _poObj)
		{
			// Stop all the actions of being played by this object.
			for (uint i=0;i<m_poHUDActions.size();i++)
			{
				if ((_poObj==NULL) || (m_poHUDActions[i]->m_poObject == _poObj))
					Stop(i);
			}
		}
	}
}
// -----------------------------------------------------------------------------
