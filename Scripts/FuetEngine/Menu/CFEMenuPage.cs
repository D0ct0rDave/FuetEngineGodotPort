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
	public class CFEMenuPage : CFENamedObject
	{
		// --------------------------------------------------------------------
		private const CFEString BACKGROUND_GROUP_NAME = "Background";
		private const CFEString FOREGROUND_GROUP_NAME = "Foreground";
		private const CFEString SKIPABLE_GROUP_NAME = "Skip_";
		private const CFEString BUTTON_GROUP_NAME = "BT_";
		private const CFEString CHECKBOX_GROUP_NAME = "CB_";
		private const CFEString OPTION_GROUP_NAME = "OPT_";
		private const CFEString NEW_OPTION_GROUP_NAME = "NEW_OPT_GROUP";
		// The buttons in the page.
		private List<CFEMenuButton> m_oButtons = new List<CFEMenuButton>();

		// 
		private CFEHUDManager m_oHUDManager;

		/// The amount of idle time to consider the page as inactive.
		private FEReal m_rIdleTime;

		/// The time to wait until the page launches and OnIdlePage event
		private FEReal m_rTimeToIdle;

		/// The next logical page to go when this page has finished.
		private CFEString m_sNextPage;

		/// The next logical page to go when this page has finished.
		private CFEString m_sPrevPage;

		/// The next page to go when this page has finished.
		private CFEString m_sPageToGo;

		/// The button to be selected upon entering the page.
		private CFEString m_sOnEnterSelButton;

		/// Input manager to process input over the menus.
		private CFEMenuInputMgr m_oIM;

		/// The menu configuration file.
		private CFEConfigFile m_oMenuCfg;

		/// 
		private int m_iSelectedButton = -1;
		private int m_iFocusedButton = -1;

		/// The page currently active.
		// static CFEMenuPage*			m_oThisPage;
		private class CFEMenuOptionGroup
		{
			public List<CFEMenuOptionButton> m_oSiblings = new List<CFEMenuOptionButton>();
		};

		// Must be pointer! In case of arrayh resize, some buttons of previous parsed groups
		// can point the siblings field to invalid data (due to array resize (realloc))
		private List<CFEMenuOptionGroup> m_oGroups = new List<CFEMenuOptionGroup>();
		// --------------------------------------------------------------------
		public enum MenuPageState
		{
			MPS_NONE,

			MPS_ENTERING_PAGE,
			MPS_IDLE,
			MPS_EXIT_PAGE,
			MPS_EXITING_PAGE,

			MPS_FINISH,

			MPS_NUMSTATES
		};
		private StatedObject<MenuPageState> m_state = new StatedObject<MenuPageState>();
		// --------------------------------------------------------------------
		public CFEMenuPage(CFEString _sName)
			: base(_sName)
		{
			m_state.OnEnterState = OnEnterState;
			m_state.OnExitState = OnExitState;
		}
		// --------------------------------------------------------------------
		public void Init(CFEString _sFilename, CFEMenuInputMgr _oIM, CFEConfigFile _oMenuCfg)
		{
			m_oIM = _oIM;
			m_oMenuCfg = _oMenuCfg;

			m_oHUDManager = new CFEHUDManager();
			m_oHUDManager.Init(_sFilename);

			// hay que recorrer la página para obtener los botones.		
			for (int i = 0; i < m_oHUDManager.oGetPageObjs().Count; i++)
			{
				CFEHUDObject oObj = m_oHUDManager.oGetPageObjs()[i];
				if (oObj.sGetName().StartsWith(BUTTON_GROUP_NAME))
				{
					CFEMenuButton oButton = new CFEMenuButton(this);
					oButton.Init(m_oHUDManager.oGetPageObjs()[i], m_oHUDManager);

					m_oButtons.Add(oButton);
				}
				else if (oObj.sGetName().StartsWith(CHECKBOX_GROUP_NAME))
				{
					CFEMenuCheckBox oButton = new CFEMenuCheckBox(this);
					oButton.Init(m_oHUDManager.oGetPageObjs()[i], m_oHUDManager);

					m_oButtons.Add(oButton);
				}
				else if (oObj.sGetName().StartsWith(OPTION_GROUP_NAME))
				{
					if (m_oGroups.Count > 0)
					{
						int iGroupIdx = m_oGroups.Count - 1;

						CFEMenuOptionButton oButton = new CFEMenuOptionButton(this, m_oGroups[iGroupIdx].m_oSiblings);
						oButton.Init(oObj, m_oHUDManager);

						m_oGroups[iGroupIdx].m_oSiblings.Add(oButton);
						m_oButtons.Add(oButton);
					}
				}
				else if (oObj.sGetName().StartsWith(NEW_OPTION_GROUP_NAME))
				{
					// add new group
					CFEMenuOptionGroup oGroup = new CFEMenuOptionGroup();
					m_oGroups.Add(oGroup);
				}
			}

			// hide unwanted buttons
			for (int i = 0; i < m_oButtons.Count; i++)
			{
				string sVar = $"Menu.{sGetName()}.{m_oButtons[i].sGetName()}.Hide";

				// hide the button?
				bool bHide = m_oMenuCfg.bGetBool(sVar, false);
				if (bHide == true)
				{
					m_oButtons[i].Hide();
				}
			}

			BuildNeighbours();
		}
		// --------------------------------------------------------------------
		public void Finish()
		{
			m_oHUDManager.Finish();
			m_oHUDManager = null;

			//delete the buttons of the page...
			for (int i = 0; i < m_oButtons.Count; i++)
			{
				m_oButtons[i].Finish();
			}
			m_oButtons.Clear();

			//delete the groups of the page...
			m_oGroups.Clear();
		}
		// --------------------------------------------------------------------
		/// Builds the neighbours for the buttons in the page.
		public void BuildNeighbours()
		{
			// look for the button
			for (int i = 0; i < m_oButtons.Count; i++)
			{
				if (m_oButtons[i] == null) continue;
				if (m_oButtons[i].oGetHUDObject() == null) continue;

				CFEMenuButton[] oNeighs = new CFEMenuButton[4]; // { null, null, null, null };
				CFEVect2 oPos = m_oButtons[i].oGetHUDObject().oGetIniPos();

				int uiL = 0x7fffffff;
				int uiR = 0x7fffffff;
				int uiT = 0x7fffffff;
				int uiB = 0x7fffffff;

				// for this button get the next button to the left
				for (int j = 0; j < m_oButtons.Count; j++)
				{
					if (m_oButtons[j] == null) continue;
					if (m_oButtons[j].oGetHUDObject() == null) continue;

					if (m_oButtons[j] != m_oButtons[i])
					{
						CFEVect2 oNeighPos = m_oButtons[j].oGetHUDObject().oGetIniPos();
						CFEVect2 oDst = oPos - oNeighPos;

						int iY = (int)oDst.y;
						iY *= iY;
						int iX = (int)oDst.x;
						iX *= iX;

						// LEFT neighbour
						if (oNeighPos.x < oPos.x)
						{
							int uiD = 10000 * iY + iX;
							if (uiD < uiL)
							{
								uiL = uiD;
								oNeighs[CFEMenuButton.EFEButtonNeigh_Left] = m_oButtons[j];
							}
						}

						// RIGHT neighbour
						if (oNeighPos.x > oPos.x)
						{
							int uiD = 10000 * iY + iX;
							if (uiD < uiR)
							{
								uiR = uiD;
								oNeighs[CFEMenuButton.EFEButtonNeigh_Right] = m_oButtons[j];
							}
						}

						// TOP neighbour
						if (oNeighPos.y < oPos.y)
						{
							int uiD = iY + 10000 * iX;
							if (uiD < uiT)
							{
								uiT = uiD;
								oNeighs[CFEMenuButton.EFEButtonNeigh_Top] = m_oButtons[j];
							}
						}

						// BOTTOM neighbour
						if (oNeighPos.y > oPos.y)
						{
							int uiD = iY + 10000 * iX;
							if (uiD < uiB)
							{
								uiB = uiD;
								oNeighs[CFEMenuButton.EFEButtonNeigh_Bottom] = m_oButtons[j];
							}
						}
					}
				}

				// setup computed neighbours
				m_oButtons[i].SetNeighbour(oNeighs[CFEMenuButton.EFEButtonNeigh_Top], CFEMenuButton.EFEButtonNeigh_Top);
				m_oButtons[i].SetNeighbour(oNeighs[CFEMenuButton.EFEButtonNeigh_Bottom], CFEMenuButton.EFEButtonNeigh_Bottom);
				m_oButtons[i].SetNeighbour(oNeighs[CFEMenuButton.EFEButtonNeigh_Left], CFEMenuButton.EFEButtonNeigh_Left);
				m_oButtons[i].SetNeighbour(oNeighs[CFEMenuButton.EFEButtonNeigh_Right], CFEMenuButton.EFEButtonNeigh_Right);

				// read from config...	
				CFEString sPrefix = $"Menu.{sGetName()}.{m_oButtons[i].sGetName()}.Neighs.";

				CFEMenuButton oButton = null;
				if (m_oMenuCfg.bExists(sPrefix + "Top"))
				{
					oButton = oGetButton(m_oMenuCfg.sGetString(sPrefix + "Top", ""));
					m_oButtons[i].SetNeighbour(oButton, CFEMenuButton.EFEButtonNeigh_Top);
				}
				if (m_oMenuCfg.bExists(sPrefix + "Bottom"))
				{
					oButton = oGetButton(m_oMenuCfg.sGetString(sPrefix + "Bottom", ""));
					m_oButtons[i].SetNeighbour(oButton, CFEMenuButton.EFEButtonNeigh_Bottom);
				}
				if (m_oMenuCfg.bExists(sPrefix + "Left"))
				{
					oButton = oGetButton(m_oMenuCfg.sGetString(sPrefix + "Left", ""));
					m_oButtons[i].SetNeighbour(oButton, CFEMenuButton.EFEButtonNeigh_Left);
				}

				if (m_oMenuCfg.bExists(sPrefix + "Right"))
				{
					oButton = oGetButton(m_oMenuCfg.sGetString(sPrefix + "Right", ""));
					m_oButtons[i].SetNeighbour(oButton, CFEMenuButton.EFEButtonNeigh_Right);
				}
			}

			if (m_oButtons.Count == 1)
				SelectButton(0);
		}
		// --------------------------------------------------------------------
		/// Retrieves the next selectable neighbour button using the given direction.
		public int iGetNextNeighbour(CFEMenuButton _oButton, int _iNeigh)
		{
			// to prevent unwanted cycles
			const int MAX_PASSES = 100;

			// look for the prev focusable button focus
			CFEMenuButton oNeigh = _oButton.oGetNeighbour(_iNeigh);

			int iPasses = 0;
			while ((oNeigh != null) && (iPasses < MAX_PASSES))
			{
				if (oNeigh.bIsEnabled() && oNeigh.bIsVisible())
				{
					break;
				}
				else
					oNeigh = oNeigh.oGetNeighbour(_iNeigh);

				iPasses++;
			}

			if ((iPasses < MAX_PASSES) && (oNeigh != null))
			{
				// look for the button
				for (int i = 0; i < m_oButtons.Count; i++)
				{
					if (m_oButtons[i] == oNeigh)
						return (i);
				}
			}

			return (-1);
		}
		// --------------------------------------------------------------------
		public void Update(FEReal _rDeltaT)
		{
			if (GetState() != MenuPageState.MPS_NONE)
			{
				for (int i = 0; i < m_oButtons.Count; i++)
					m_oButtons[i].Update(_rDeltaT);
			}

			switch (GetState())
			{
				//
				case MenuPageState.MPS_NONE:
				{
					ChangeState(MenuPageState.MPS_ENTERING_PAGE);
				}
				break;

				case MenuPageState.MPS_ENTERING_PAGE:
				{
					if (!m_oHUDManager.bPlaying(CFEMenuDefinitions.ENTER_PAGE_EVENT_NAME, null))
					{
						ChangeState(MenuPageState.MPS_IDLE);
					}
				}
				break;

				case MenuPageState.MPS_IDLE:
				{
					if (m_oIM != null)
					{
						// ---------------------------------------------------------
						// First check input buttons
						// ---------------------------------------------------------
						// Select focusable button?
						if (m_oIM.bDown(CFEMenuInputMgr.EFEInputButton.IB_UP) && (m_iSelectedButton >= 0))
						{
							// look for the prev focusable button focus
							int iNextSelectableButton = iGetNextNeighbour(m_oButtons[m_iSelectedButton], CFEMenuButton.EFEButtonNeigh_Top);
							if (iNextSelectableButton != -1)
								SelectButton(iNextSelectableButton);

							m_rTimeToIdle = m_rIdleTime;
						}

						else if (m_oIM.bDown(CFEMenuInputMgr.EFEInputButton.IB_LEFT) && (m_iSelectedButton >= 0))
						{
							int iNextSelectableButton = iGetNextNeighbour(m_oButtons[m_iSelectedButton], CFEMenuButton.EFEButtonNeigh_Left);
							if (iNextSelectableButton != -1)
								SelectButton(iNextSelectableButton);

							m_rTimeToIdle = m_rIdleTime;
						}

						else if (m_oIM.bDown(CFEMenuInputMgr.EFEInputButton.IB_RIGHT) && (m_iSelectedButton >= 0))
						{
							int iNextSelectableButton = iGetNextNeighbour(m_oButtons[m_iSelectedButton], CFEMenuButton.EFEButtonNeigh_Right);
							if (iNextSelectableButton != -1)
								SelectButton(iNextSelectableButton);

							m_rTimeToIdle = m_rIdleTime;
						}

						else if (m_oIM.bDown(CFEMenuInputMgr.EFEInputButton.IB_DOWN) && (m_iSelectedButton >= 0))
						{
							int iNextSelectableButton = iGetNextNeighbour(m_oButtons[m_iSelectedButton], CFEMenuButton.EFEButtonNeigh_Bottom);
							if (iNextSelectableButton != -1)
								SelectButton(iNextSelectableButton);

							m_rTimeToIdle = m_rIdleTime;
						}

						// Button pressed ?
						else if (m_oIM.bDown(CFEMenuInputMgr.EFEInputButton.IB_B))
						{
							CFEMenuButton oButton = oGetButton("BT_BACK");
							if ((oButton != null) && oButton.bIsEnabled() && oButton.bIsVisible())
							{
								if (m_iSelectedButton >= 0)
								{
									m_oButtons[m_iSelectedButton].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_UNSELECT);
									OnButtonExit(m_oButtons[m_iSelectedButton]);
								}

								if (oButton.GetState() != CFEMenuButton.EFEMenuButtonState.MBS_PRESSED)
								{
									oButton.ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_INPUT_PRESS);
									OnButtonPress(oButton);
								}

								// new selected button
								for (int i = 0; i < m_oButtons.Count; i++)
								{
									if (m_oButtons[i] == oButton)
									{
										m_iSelectedButton = i;
										break;
									}
								}
							}

							m_rTimeToIdle = m_rIdleTime;
						}

						// Button pressed ?
						else if (m_oIM.bDown(CFEMenuInputMgr.EFEInputButton.IB_A) && (m_iSelectedButton >= 0))
						{
							if (m_oButtons[m_iSelectedButton].GetState() != CFEMenuButton.EFEMenuButtonState.MBS_PRESSED)
							{
								m_oButtons[m_iSelectedButton].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_INPUT_PRESS);
								OnButtonPress(m_oButtons[m_iSelectedButton]);
							}

							m_rTimeToIdle = m_rIdleTime;
						}

						// Button released ?
						else if (m_oIM.bUp(CFEMenuInputMgr.EFEInputButton.IB_A) && (m_iSelectedButton >= 0))
						{
							m_oButtons[m_iSelectedButton].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_INPUT_RELEASE);
							OnButtonRelease(m_oButtons[m_iSelectedButton]);
						}
						else
						{
							// ---------------------------------------------------------
							// Then check cursor
							// ---------------------------------------------------------
							bool bNoFocusedButton = true;
							for (int i = 0; i < m_oButtons.Count; i++)
							{
								if ((!m_oButtons[i].bIsEnabled()) || (!m_oButtons[i].bIsVisible())) continue;

								CFEMenuInputMgr.EFECursorResult eCR = m_oIM.eTestCursor(m_oButtons[i].oGetRect());

								if (eCR == CFEMenuInputMgr.EFECursorResult.CR_CURSOR_OVER_PRESS)
								{
									bNoFocusedButton = false;
									m_iFocusedButton = i;

									// Process focus events
									if (i != m_iSelectedButton)
									{
										if (m_iSelectedButton >= 0)
										{
											m_oButtons[m_iSelectedButton].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_UNSELECT);
											OnButtonExit(m_oButtons[m_iSelectedButton]);
										}

										m_iSelectedButton = i;

										// IS THIS SAFE TO DO ???? (Performs also the onfocus enter state / event
										// SelectButton(i);
									}

									if (m_iSelectedButton >= 0) // is this (m_iSelectedButton<0) possible ?
									{
										if (m_oButtons[m_iSelectedButton].GetState() != CFEMenuButton.EFEMenuButtonState.MBS_PRESSED)
										{
											m_oButtons[m_iSelectedButton].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_INPUT_PRESS);
											OnButtonPress(m_oButtons[m_iSelectedButton]);
										}
									}

									m_rTimeToIdle = m_rIdleTime;

									// No more checks :)
									break;
								}
								else if (eCR == CFEMenuInputMgr.EFECursorResult.CR_CURSOR_OVER)
								{
									bNoFocusedButton = false;

									// Process focus events
									if (i != m_iFocusedButton)
									{
										if (m_iFocusedButton >= 0)
										{
											m_oButtons[m_iFocusedButton].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_UNFOCUS);
										}

										m_iFocusedButton = i;

										m_oButtons[m_iFocusedButton].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_FOCUS);
									}

									m_rTimeToIdle = m_rIdleTime;

									// No more checks :)
									break;
								}
								/*
								else
								{
									if ((m_iSelectedButton>=0) && (m_oButtons[i].GetState() == MBS_PRESSED))
										m_oButtons[i].ProcessEvent(MBE_RELEASE);
								}
								*/
							} // end for

							if ((bNoFocusedButton) && (m_iFocusedButton != -1))
							{
								m_oButtons[m_iFocusedButton].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_UNFOCUS);
								m_iFocusedButton = -1;
							}
						} // end if
					} // end if m_oIM != null
					else
					{
						/*
						if (m_iFocusedButton!=-1)
						{
							m_oButtons[m_iFocusedButton].ProcessEvent(MBE_UNFOCUS);
							m_iFocusedButton = -1;
						}
						*/
					}

					if ((m_rIdleTime != -1) && (m_rTimeToIdle > 0.0f))
					{
						m_rTimeToIdle -= _rDeltaT;

						if (m_rTimeToIdle <= 0.0f)
						{
							m_rTimeToIdle = 0.0f;
							OnIdlePage();
						}
					}
				}
				break;

				case MenuPageState.MPS_EXIT_PAGE:
				{
					// wait until all buttons have finished their animations...
					bool bExit = true;
					for (int i = 0; i < m_oButtons.Count; i++)
					{
						if (m_oButtons[i].GetState() != CFEMenuButton.EFEMenuButtonState.MBS_EXIT_DONE)
						{
							bExit = false;
							break;
						}
					}

					if (bExit)
					{
						ChangeState(MenuPageState.MPS_EXITING_PAGE);
					}
				}
				break;

				case MenuPageState.MPS_EXITING_PAGE:
				{
					if (!m_oHUDManager.bPlaying(CFEMenuDefinitions.EXIT_PAGE_EVENT_NAME, null))
					{
						OnExitPageDone();
						ChangeState(MenuPageState.MPS_FINISH);
					}
				}
				break;
			}

			if (m_oHUDManager != null)
				m_oHUDManager.Update(_rDeltaT);
		}
		// --------------------------------------------------------------------
		public void SelectButton(CFEString _sButtonToSelect)
		{
			if (_sButtonToSelect == "")
			{
				SelectButton(-1);
			}
			else
			{
				for (int i = 0; i < m_oButtons.Count; i++)
				{
					if (m_oButtons[i].sGetName().ToLower() == _sButtonToSelect.ToLower())
					{
						SelectButton(i);
						return;
					}
				}
			}
		}
		// --------------------------------------------------------------------
		// Forces the focus to be on the button identified by the given index.
		public void SelectButton(int _iButtonToSelect)
		{
			if (_iButtonToSelect == -1)
			{
				if (m_iSelectedButton >= 0)
				{
					m_oButtons[m_iSelectedButton].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_UNSELECT);
					OnButtonExit(m_oButtons[m_iSelectedButton]);
				}

				m_iSelectedButton = -1;
				return;
			}

			CFEMenuButton oButton = m_oButtons[_iButtonToSelect];
			if ((!oButton.bIsEnabled()) || (!oButton.bIsVisible())) return;

			if (GetState() != MenuPageState.MPS_IDLE)
			{
				m_iSelectedButton = _iButtonToSelect;
				oButton.ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_SELECT);
			}
			else
			{
				if (m_iSelectedButton >= 0)
				{
					m_oButtons[m_iSelectedButton].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_UNSELECT);
					OnButtonExit(m_oButtons[m_iSelectedButton]);
				}

				m_iSelectedButton = _iButtonToSelect;

				oButton.ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_SELECT);
				OnButtonEnter(oButton);
			}
		}
		// --------------------------------------------------------------------
		/// Specific code to perform when the object enters to a given MenuPageState.
		private void OnEnterState(MenuPageState _eState)
		{
			switch (_eState)
			{
				case MenuPageState.MPS_ENTERING_PAGE:
				{
					m_oHUDManager.iPlay(CFEMenuDefinitions.ENTER_PAGE_EVENT_NAME, null, false);

					// do enter page for the page itself.
					OnEnterPage();

					// Do enter page for every registered button.
					for (int i = 0; i < m_oButtons.Count; i++)
						m_oButtons[i].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_ENTER_PAGE);
				}
				break;

				case MenuPageState.MPS_IDLE:
				{
					m_oHUDManager.iPlay(CFEMenuDefinitions.IDLE_PAGE_EVENT_NAME, null, false);

					// Select the first selectable button.
					if (sGetOnEnterSelectButton() != "")
					{
						for (int i = 0; i < m_oButtons.Count; i++)
						{
							if ((m_oButtons[i].bIsEnabled() && m_oButtons[i].bIsVisible())
							 && (sGetOnEnterSelectButton() == m_oButtons[i].sGetName()))
							{
								SelectButton(i);
								break;
							}
						}
					}

					OnEnterPageDone();
				}
				break;

				case MenuPageState.MPS_EXIT_PAGE:
				{
					// Do exit page for every registered button.
					for (int i = 0; i < m_oButtons.Count; i++)
						m_oButtons[i].ProcessEvent(CFEMenuButton.EFEMenuButtonEvent.MBE_EXIT_PAGE);
				}
				break;

				case MenuPageState.MPS_EXITING_PAGE:
				{
					m_oHUDManager.iPlay(CFEMenuDefinitions.EXIT_PAGE_EVENT_NAME, null, false);

					// do exit page for the page itself.
					OnExitPage();
				}
				break;

				case MenuPageState.MPS_FINISH:
				{

				}
				break;
			}
		}
		// --------------------------------------------------------------------
		/// Specific code to perform when the object exits from a given MenuPageState.
		private void OnExitState(MenuPageState _eState, MenuPageState _eNewState)
		{
		}
		// --------------------------------------------------------------------
		public bool bFinished()
		{
			return (GetState() == MenuPageState.MPS_FINISH);
		}
		// --------------------------------------------------------------------
		public bool bExiting()
		{
			return ((GetState() == MenuPageState.MPS_EXIT_PAGE) || (GetState() == MenuPageState.MPS_EXITING_PAGE));
		}
		// --------------------------------------------------------------------
		public bool bEntering()
		{
			return (GetState() == MenuPageState.MPS_ENTERING_PAGE);
		}
		// --------------------------------------------------------------------
		public bool bIdle()
		{
			return (GetState() == MenuPageState.MPS_IDLE);
		}
		// --------------------------------------------------------------------
		public CFEMenuButton oGetButton(CFEString _sButtonName)
		{
			for (int i = 0; i < m_oButtons.Count; i++)
			{
				if (m_oButtons[i].sGetName().ToLower() == _sButtonName.ToLower())
					return m_oButtons[i];
			}
			return null;
		}
		// --------------------------------------------------------------------
		/// Called when a button in the page is pressed.
		public void OnButtonPress(CFEMenuButton _poEmisor)
		{
			// avoid stressing the button
			if (m_oMenuCfg == null) return;

			CFEString sVar = $"Menu.{sGetName()}.{_poEmisor.sGetName()}.OnPress";

			CFEString sPageToGo = m_oMenuCfg.sGetString(sVar, "");
			if (sPageToGo != "")
			{
				SetPageToGo(sPageToGo);
				ChangeState(MenuPageState.MPS_EXIT_PAGE);
			}
		}
		// --------------------------------------------------------------------
		public void EnableButton(CFEString _sButtonName)
		{
			CFEMenuButton oButton = oGetButton(_sButtonName);

			if ((oButton != null) && (oButton.GetState() == CFEMenuButton.EFEMenuButtonState.MBS_DISABLED))
				oButton.Enable();
		}
		// --------------------------------------------------------------------
		public void DisableButton(CFEString _sButtonName)
		{
			CFEMenuButton oButton = oGetButton(_sButtonName);

			if ((oButton != null) && (oButton.GetState() != CFEMenuButton.EFEMenuButtonState.MBS_DISABLED))
				oButton.Disable();
		}
		// --------------------------------------------------------------------
		public CFEString sGetSelectedButton()
		{
			if (m_iSelectedButton == -1)
				return "";
			else
				return m_oButtons[m_iSelectedButton].sGetName();
		}
		// --------------------------------------------------------------------
		/// Sets the next logical page
		public void SetNextPage(CFEString _sNextPage)
		{
			m_sNextPage = _sNextPage;
		}

		/// Get the next page to go.
		public CFEString sGetNextPage()
		{
			return m_sNextPage;
		}

		/// Sets the previous logical page.
		public void SetPrevPage(CFEString _sPrevPage)
		{
			m_sPrevPage = _sPrevPage;
		}

		/// Get the next page to go.
		public CFEString sGetPrevPage()
		{
			return m_sPrevPage;
		}

		/// GotoNext, GotoBack, GotoStart, GotoDefault, GotoIdle have special meanings.
		public void SetPageToGo(CFEString _sPageToGo)
		{
			m_sPageToGo = _sPageToGo;
		}

		/// Retrieves the next page to go. GotoEnd, GotoNext, GotoBack, GotoStart, GotoDefault, GotoIdle have special meanings.
		public CFEString sGetPageToGo()
		{
			return m_sPageToGo;
		}

		/// Sets the button to be selected upon entering the page.
		public void SetOnEnterSelectButton(CFEString _sOnEnterSelButton)
		{
			m_sOnEnterSelButton = _sOnEnterSelButton;
		}

		/// Retrieves the button which should be selected upon entering the page.
		public CFEString sGetOnEnterSelectButton()
		{
			return m_sOnEnterSelButton;
		}

		/// Sets the idle time of the page. -1 for no idle time.
		public void SetIdleTime(FEReal _rIdleTime)
		{
			m_rIdleTime = _rIdleTime;
			m_rTimeToIdle = m_rIdleTime;
		}

		/// Returns the idle time of the page.
		public FEReal rGetIdleTime()
		{
			return m_rIdleTime;
		}

		/// Retrieve the current HUD Manager
		public CFEHUDManager oGetHUDManager()
		{
			return m_oHUDManager;
		}

		/// Retrieve the input manager used by this object
		public CFEMenuInputMgr oGetInputManager()
		{
			return m_oIM;
		}

		/// Sets the input manager for this page.
		public void SetInputManager(CFEMenuInputMgr _oIM)
		{
			m_oIM = _oIM;
		}
		/// Retrieve menu configuration file.
		CFEConfigFile oGetMenuCfg()
		{
			return m_oMenuCfg;
		}
		//-----------------------------------------------------------------------------
		public CFEHUD oGetHUD()
		{
			return m_oHUDManager.oGetHUD();
		}
		// --------------------------------------------------------------------
		public MenuPageState GetState()
		{
			return m_state.GetState();
		}
		private void ChangeState(MenuPageState _stateToChange)
		{
			m_state.ChangeState(_stateToChange);
		}
		// --------------------------------------------------------------------
		/// Called when the page starts entering.
		public virtual void OnEnterPage() { }

		/// Called when the page enter is done.
		public virtual void OnEnterPageDone() { }

		/// Called when the page is exited.
		public virtual void OnExitPage() { }

		/// Called when the page exit is done. Called just before the change to the page finish MenuPageState.
		public virtual void OnExitPageDone() { }

		/// Called when a button in the page is released.
		public virtual void OnButtonRelease(CFEMenuButton _oButton) { }

		/// Called when a button in the page is focussed.
		public virtual void OnButtonEnter(CFEMenuButton _oButton) { }

		/// Called when a button in the page loses the focus
		public virtual void OnButtonExit(CFEMenuButton _oButton) { }

		/// Called after the page is inactive for a given amount of idle time.
		public virtual void OnIdlePage() { }

		/// Called when the user press the go back button.
		public virtual void OnGotoPrevPage() { }
	}
}
//-----------------------------------------------------------------------------

