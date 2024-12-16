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
	public class CFEMenuPage
	{

	}

	//-----------------------------------------------------------------------------
	/*
	class CFEHUDObjCloner : public CFEHUDVisitor
	{
		public:	
			
		CFEHUDObject			m_oSrc;
		bool					m_bToEnable;
		CFEHUDElemLocator	m_oElemLocator;

		public:	
		
			static void Clone(CFEHUDObject _poDst,CFEHUDObject _oSrc,bool _bToEnable)
			{
				CFEHUDObjCloner oObj;
				
				oObj.m_oSrc		= _oSrc;
				oObj.m_bToEnable = _bToEnable;

				_oDst.Accept(oObj);
			}

			/// 
			virtual void Visit(CFEHUD _oObj) {};

			/// 
			virtual void Visit(CFEHUDElement _oObj){};

			/// 
			virtual void Visit(CFEHUDObject _oObj)
			{
				CFEHUDObject oSrc = m_oElemLocator.oLocateHUDObject( m_oSrc, _oObj.sGetName() );

				if (oSrc != NULL)
					CopyProperties(_oObj, oSrc, m_bToEnable);
			}

			/// 
			virtual void Visit(CFEHUDGroup _oObj)
			{
				for (uint i=0;i<_oObj.uiNumObjs();i++)
					_oObj.poGetObject(i).Accept(this);

				// Recorrido en post orden
				CFEHUDObject oSrc = m_oElemLocator.poLocateHUDObject( m_oSrc, _oObj.sGetName() );

				if (oSrc != NULL)
					CopyProperties(_oObj, oSrc, m_bToEnable);
			};

			/// 
			virtual void Visit(CFEHUDLabel _oObj){ Visit((CFEHUDObject)_oObj); };
			/// 
			virtual void Visit(CFEHUDRect _oObj){ Visit((CFEHUDObject)_oObj); };
			/// 
			virtual void Visit(CFEHUDShape _oObj){ Visit((CFEHUDObject)_oObj); };
			/// 
			virtual void Visit(CFEHUDPSys _oObj){ Visit((CFEHUDObject)_oObj); };
			/// 
			virtual void Visit(CFEHUDElementAction _oObj){ Visit((CFEHUDObject)_oObj); };
			/// 
			virtual void Visit(CFEHUDObjectAction _oObj){ Visit((CFEHUDObject)_oObj); };

			/// Retrieves the type of visitor (useful when needed to extend the basic class hierarchy).
			virtual const CFEString sGetType(){return("CFEHUDCloner");};
	}
	*/
	public class CFEMenuButton
	{

		const string ENTER_PAGE_EVENT_NAME = "";
		const string PRESS_BUTTON_EVENT_NAME = "";
		const string MBS_ENTERING_PAGE = "";

		// ------------------------------------------------------------------------
		public enum EFEMenuButtonState
		{
			MBS_NONE = 0,

			/// Stable states
			MBS_IDLE = 1,       /// Button ready doing nothing.
			MBS_DISABLED = 2,       /// Button unable to interact with
			MBS_SELECTED = 3,       /// Button selected and ready
			MBS_PRESSED = 4,        /// Button pressed
			MBS_FOCUSED = 5,        /// Button with cursor over but not selected.

									/// Transitional states
			MBS_TRANSITION = 6,     /// Button is currently performing a(n animated) transition between 2 states
			MBS_ENTERING_PAGE = 7,      /// Entire page is entering to IDLE state
			MBS_EXITING_PAGE = 8,       /// Entire page is going to exit,

										/// Rare Stable State
			MBS_EXIT_DONE = 9,      /// Button has performed the exiting page animation.

			MBS_NUM_BUTTON_STATES
		}
		// ------------------------------------------------------------------------
		public enum EFEMenuButtonEvent
		{
			MBE_NONE,

			MBE_ENTER_PAGE,         /// When page enters		
			//	MBE_END_ENTERING_PAGE,	/// When page ends entering state

			MBE_ENABLE,             /// When button should be (re)enabled
			MBE_DISABLE,            /// When button should be disabled.

			MBE_SELECT,             /// Button is selected
			MBE_UNSELECT,           /// When button is unselected

			MBE_FOCUS,              /// When cursor enters
			MBE_UNFOCUS,            /// When cursor exits

			MBE_INPUT_PRESS,        /// When Button is pressed (both with button or cursor)
			MBE_INPUT_RELEASE,      /// When Button is released (both with button or cursor)

			MBE_EXIT_PAGE,          /// When page exits

			MBE_NUM_BUTTON_EVENTS
		}
		//-----------------------------------------------------------------------------
		public enum EFEButtonNeigh
		{
			BN_TOP = 0,
			BN_LEFT = 1,
			BN_RIGHT = 2,
			BN_BOTTOM = 3

		}

		private StatedObject<EFEMenuButtonState> m_state = new StatedObject<EFEMenuButtonState>();

		// ------------------------------------------------------------------------
		/// Sets the initial state of the button depending on the parameter and the current state

		/// The HUD element representing this button.
		CFEHUDObject m_oObj = null;

		/// The HUD element representing this button.
		CFEHUDObject m_oOriObj = null;

		/// The HUDmanager who will process the visual aspect of the button states.
		CFEHUDManager m_oHUDManager = null;

		/// Rect of this button.
		CFERect m_oRect = null;

		/// Is enabled the button
		// bool					m_bEnabled;

		/// Next state to change to.
		EFEMenuButtonState m_nextState;

		/// Current action being played.
		CFEString m_sCurrentAction;
		int m_iCurrentAction = -1;

		/// The owner page of this button.
		private CFEMenuPage m_oOwner = null;
		private CFEMenuButton[] m_oNeighs = null;

		// ------------------------------------------------------------------------

		private void CopyProperties(CFEHUDObject _oDst, CFEHUDObject _oSrc, bool _bToEnable)
		{
			if (_bToEnable)
			{
				/// Common properties for object
				_oDst.SetIniPos(_oSrc.oGetIniPos());
				_oDst.SetIniScale(_oSrc.oGetIniScale());
				_oDst.SetIniAngle(_oSrc.rGetIniAngle());
				_oDst.SetIniDepth(_oSrc.rGetIniDepth());
				_oDst.SetIniColor(_oSrc.oGetIniColor());
				_oDst.SetIniAction(_oSrc.iGetIniAction());
				_oDst.ShowObj(_oSrc.bGetIniVis());

				_oDst.SetCurPos(_oSrc.oGetCurPos());
				_oDst.SetCurScale(_oSrc.oGetCurScale());
				_oDst.SetCurAngle(_oSrc.rGetCurAngle());
				_oDst.SetCurDepth(_oSrc.rGetCurDepth());
				_oDst.SetCurColor(_oSrc.oGetCurColor());
				_oDst.SetCurVis(_oSrc.bGetCurVis());
				_oDst.SetCurAction(_oSrc.iGetCurAction());
			}
			else
			{
				/// Common properties for object
				_oDst.SetIniPos(_oSrc.oGetPos());
				_oDst.SetIniScale(_oSrc.oGetScale());
				_oDst.SetIniAngle(_oSrc.rGetAngle());
				_oDst.SetIniDepth(_oSrc.rGetDepth());
				_oDst.SetIniColor(_oSrc.oGetColor());
				_oDst.SetIniAction(_oSrc.iGetAction());
				_oDst.ShowObj(_oSrc.bIsVisible());

				_oDst.SetCurPos(CFEVect2.Zero);
				_oDst.SetCurScale(CFEVect2.One);
				_oDst.SetCurAngle(0.0f);
				_oDst.SetCurDepth(0.0f);
				_oDst.SetCurColor(new CFEColor(1, 1, 1, 1));
				_oDst.SetCurAction(-1);
				_oDst.SetCurVis(true);
			}
		}
		// ------------------------------------------------------------------------
		public CFEMenuButton(CFEMenuPage _oOwner)
		{
			m_oNeighs = new CFEMenuButton[4];
			m_oNeighs[0] = m_oNeighs[1] = m_oNeighs[2] = m_oNeighs[3] = null;
			m_oOwner = _oOwner;
		}
		//-----------------------------------------------------------------------------
		public void Init(CFEHUDObject _oObj, CFEHUDManager _oHUDManager)
		{
			// m_oOriObj		= CFEHUDInstancer::poCreateInstance(_poObj);
			m_oObj = _oObj;
			m_oHUDManager = _oHUDManager;
			m_oRect = null; // TODO: CFEHUDRectGen::oGetRect(NULL, _poObj);

			m_state.ChangeState(EFEMenuButtonState.MBS_NONE);
		}
		//-----------------------------------------------------------------------------
		private void Finish()
		{
		}
		//-----------------------------------------------------------------------------
		public void Update(FEReal _rDeltaT)
		{
			switch (m_state.GetState())
			{
				case EFEMenuButtonState.MBS_NONE:
				{
					m_state.ChangeState(EFEMenuButtonState.MBS_IDLE);
				}
				break;

				case EFEMenuButtonState.MBS_TRANSITION:
				{
					if ((m_iCurrentAction == -1) || (!m_oHUDManager.bPlaying(m_iCurrentAction)))
					{
						m_state.ChangeState(m_nextState);
					}
				}
				break;

				case EFEMenuButtonState.MBS_ENTERING_PAGE:
				{
					if (!m_oHUDManager.bPlaying(ENTER_PAGE_EVENT_NAME, m_oObj))
					{
						m_state.ChangeState(m_nextState);
					}
				}
				break;

				// ----------------------------
				case EFEMenuButtonState.MBS_EXITING_PAGE:
				{
					if ((m_iCurrentAction == -1) || (!m_oHUDManager.bPlaying(m_iCurrentAction)))
					{
						m_state.ChangeState(EFEMenuButtonState.MBS_EXIT_DONE);
					}
				}
				break;

				// ----------------------------
				case EFEMenuButtonState.MBS_PRESSED:
				{
					if (!m_oHUDManager.bPlaying(PRESS_BUTTON_EVENT_NAME, m_oObj))
					{
						m_state.ChangeState(m_nextState);
					}
				}
				break;
			}
		}
		//-----------------------------------------------------------------------------
		public void Disable()
		{
			// TODO: Enable(false);
		}
		//-----------------------------------------------------------------------------
		// Event managed function: This way we can reuse the base function in derived
		// classes.
		//-----------------------------------------------------------------------------
		public void ProcessEvent(EFEMenuButtonEvent _eEventType)
		{
			switch (_eEventType)
			{
				/// When page enters
				case EFEMenuButtonEvent.MBE_ENTER_PAGE:
				{
					m_nextState = m_state.GetState();
					m_state.ChangeState(EFEMenuButtonEvent.MBS_ENTERING_PAGE);
				}
				break;

				/// When button should be (re)enabled
				case EFEMenuButtonEvent.MBE_ENABLE:
				{
					bool bCurVis = m_oObj.bGetCurVis();
					bool bIniVis = m_oObj.bGetIniVis();

					if (m_state.GetState() != EFEMenuButtonEvent.MBS_DISABLED) return;
					SetInitialState(true);

					m_oObj.SetCurVis(bCurVis);
					m_oObj.SetIniVis(bIniVis);

					// Perform action inmediatly.
					m_state.ChangeState(MBS_IDLE);
				}
				break;

				/// When button should be disabled.
				case EFEMenuButtonEvent.MBE_DISABLE:
				{
					if (m_state.GetState() == EFEMenuButtonEvent.MBS_DISABLED) return;

					// Perform action inmediatly.
					m_state.ChangeState(EFEMenuButtonEvent.MBS_DISABLED);

					// ChangeState(MBS_TRANSITION);
					// SetNextState(MBS_DISABLED);
				}
				break;

				/// Button is selected
				case EFEMenuButtonEvent.MBE_SELECT:
				{
					if (m_state.GetState() == EFEMenuButtonEvent.MBS_DISABLED) return;
					m_state.ChangeState(EFEMenuButtonEvent.MBS_TRANSITION);
					SetNextState(EFEMenuButtonEvent.MBS_SELECTED);
				}
				break;

				/// When button is unselected
				case EFEMenuButtonEvent.MBE_UNSELECT:
				{
					if (
						((m_state.GetState() == EFEMenuButtonEvent.MBS_TRANSITION) && (uiGetNextState() == EFEMenuButtonEvent.MBS_SELECTED))
						||
						(m_state.GetState() == EFEMenuButtonEvent.MBS_SELECTED)
						||
						(m_state.GetState() == EFEMenuButtonEvent.MBS_PRESSED)
						)
					{
						// Not possible!! ....
						// ChangeState(MBS_TRANSITION);
						// SetNextState(MBS_IDLE);

						// ...perform action inmediatly. Because idle and select states can have infinite (looped) animations.
						m_state.ChangeState(EFEMenuButtonEvent.MBS_IDLE);
					}
				}
				break;

				/// When cursor enters
				case EFEMenuButtonEvent.MBE_FOCUS:
				{
					if (m_state.GetState() != EFEMenuButtonEvent.MBS_IDLE) return;

					// Perform action inmediatly. Because idle and select states can have infinite (looped) animations.
					m_state.ChangeState(EFEMenuButtonEvent.MBS_FOCUSED);
				}
				break;

				/// When cursor exits
				case EFEMenuButtonEvent.MBE_UNFOCUS:
				{
					if (m_state.GetState() != EFEMenuButtonEvent.MBS_FOCUSED) return;

					if (m_iCurrentAction != -1) m_oHUDManager.Stop(m_iCurrentAction);
					m_iCurrentAction = m_oHUDManager.iPlay(LOSEFOCUS_BUTTON_EVENT_NAME, m_oObj, this->bIsVisible());

					m_state.ChangeState(EFEMenuButtonEvent.MBS_TRANSITION);
					SetNextState(EFEMenuButtonEvent.MBS_IDLE);
				}
				break;

				/// When Button is pressed
				case EFEMenuButtonEvent.MBE_INPUT_PRESS:
				{
					if (m_state.GetState() == EFEMenuButtonEvent.MBS_DISABLED) return;
					if (m_state.GetState() == EFEMenuButtonEvent.MBS_PRESSED) return;
					// Perform action inmediatly
					m_state.ChangeState(EFEMenuButtonEvent.MBS_PRESSED);
				}
				break;

				/// When Button is released
				case EFEMenuButtonEvent.MBE_INPUT_RELEASE:
				{
					if (m_state.GetState() == EFEMenuButtonEvent.MBS_DISABLED) return;

					// Perform action inmediatly
					if (m_iCurrentAction != -1) m_oHUDManager.Stop(m_iCurrentAction);
					m_iCurrentAction = m_oHUDManager.iPlay(RELEASE_BUTTON_EVENT_NAME, m_oObj, this->bIsVisible());

					m_state.ChangeState(EFEMenuButtonEvent.MBS_TRANSITION);
					SetNextState(EFEMenuButtonEvent.MBS_SELECTED);
				}
				break;

				/// When page exits
				case EFEMenuButtonEvent.MBE_EXIT_PAGE:
				{
					// Perform action inmediatly
					// if ((m_iCurrentAction==-1) || (! m_oHUDManager.bPlaying((uint)m_iCurrentAction)))						
					m_state.ChangeState(EFEMenuButtonEvent.MBS_EXITING_PAGE);
				}
				break;
			}
		}

		protected virtual void OnEnterState(EFEMenuButtonEvent _state)
		{
			switch (_state)
			{
				case EFEMenuButtonEvent.MBS_ENTERING_PAGE:
				{
					// Animation is played by the page
					// m_iCurrentAction = m_oHUDManager.iPlay(ENTER_PAGE_EVENT_NAME,m_oObj);
				}
				break;

				case EFEMenuButtonEvent.MBS_IDLE:
				{
					if (m_iCurrentAction != -1) m_oHUDManager.Stop(m_iCurrentAction);
					m_iCurrentAction = m_oHUDManager.iPlay(IDLE_BUTTON_EVENT_NAME, m_oObj, this->bIsVisible());
				}
				break;

				case EFEMenuButtonEvent.BS_DISABLED:
				{
					bool bCurVis = m_oObj.bGetCurVis();
					bool bIniVis = m_oObj.bGetIniVis();

					if (m_iCurrentAction != -1) m_oHUDManager.Stop(m_iCurrentAction);
					SetInitialState(true);
					m_iCurrentAction = m_oHUDManager.iPlay(DISABLE_BUTTON_EVENT_NAME, m_oObj, this->bIsVisible());
					SetInitialState(false);
					m_oHUDManager.Stop(m_iCurrentAction);
					CFEHUDUpdater.SetActionDefaultValues(m_oObj);
					m_iCurrentAction = -1;

					m_oObj.SetCurVis(bCurVis);
					m_oObj.SetIniVis(bIniVis);
				}
				break;

				case EFEMenuButtonEvent.MBS_PRESSED:
				{
					if (m_iCurrentAction != -1) m_oHUDManager.Stop(m_iCurrentAction);
					m_iCurrentAction = m_oHUDManager.iPlay(PRESS_BUTTON_EVENT_NAME, m_oObj, this->bIsVisible());
					SetNextState(EFEMenuButtonEvent.MBS_SELECTED);
				}
				break;

				case EFEMenuButtonEvent.MBS_SELECTED:
				{
					if (m_iCurrentAction != -1) m_oHUDManager.Stop(m_iCurrentAction);
					m_iCurrentAction = m_oHUDManager.iPlay(SELECT_BUTTON_EVENT_NAME, m_oObj, this->bIsVisible());
				}
				break;

				case MBS_FOCUSED:
				{
					if (m_iCurrentAction != -1) m_oHUDManager.Stop(m_iCurrentAction);
					m_iCurrentAction = m_oHUDManager.iPlay(FOCUS_BUTTON_EVENT_NAME, m_oObj, this->bIsVisible());
				}
				break;

				/// Transitional states
				case EFEMenuButtonEvent.MBS_EXITING_PAGE:
				{
					if ((m_state.GetState() != EFEMenuButtonEvent.MBS_PRESSED) && (m_iCurrentAction != -1))
						m_oHUDManager.Stop(m_iCurrentAction);

					SetNextState(EFEMenuButtonEvent.MBS_EXIT_DONE);

					// Done by the page
					// m_iCurrentAction = m_oHUDManager.iPlay(EXIT_PAGE_EVENT_NAME,m_oObj);
				}
				break;

				case EFEMenuButtonEvent.MBS_EXIT_DONE:
				{
				}
				break;
			}
		}
		//-----------------------------------------------------------------------------
		protected virtual void OnExitState(EFEMenuButtonEvent _previousState, EFEMenuButtonEvent _newState)
		{

		}
		//-----------------------------------------------------------------------------
		CFEString sGetName()
		{
			return (m_oObj.Name);
		}
		//-----------------------------------------------------------------------------
		void SetPos(CFEVect2 _oPos)
		{
			poGetHUDObject().SetIniPos(_oPos);
			// as position has been modified programatically, we need to recompute the initially calculated rect
			// TODO: m_oRect = CFEHUDRectGen.oGetRect(null, m_oObj);
		}
		//-----------------------------------------------------------------------------
		void SetInitialState(bool _bRestore)
		{
			/*
			if (_bRestore)
				CFEHUDObjCloner::Clone(m_oObj,m_oOriObj,true);
			else
				CFEHUDObjCloner::Clone(m_oObj,m_oObj,false);
			*/
		}
		//-----------------------------------------------------------------------------
		void Show(bool _bShow)
		{
			m_oObj.ShowObj(_bShow);

			if (
				(!_bShow)

				&& (
					((m_state.GetState() == EFEMenuButtonState.MBS_TRANSITION) && (m_state.GetNextState() == EFEMenuButtonState.MBS_SELECTED))
					||
					(m_state.GetState() == EFEMenuButtonState.MBS_SELECTED)
					||
					(m_state.GetState() == EFEMenuButtonState.MBS_PRESSED)
				)

				&& (m_oOwner != null)
			)
			{
				if (m_oOwner->sGetSelectedButton() == sGetName())
					m_oOwner->SelectButton("");
			}
		}
	}
	//-----------------------------------------------------------------------------
}

