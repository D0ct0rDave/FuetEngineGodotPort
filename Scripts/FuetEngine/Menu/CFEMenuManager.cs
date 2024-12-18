using System;
using Godot;

//-----------------------------------------------------------------------------
using CFEString = System.String;
//-----------------------------------------------------------------------------

namespace FuetEngine
{
	public class CFEMenuManager : Node2D
	{
		// --------------------------------------------------------------------
		[Export]
		public string MenuFilename { set; get; }
		// --------------------------------------------------------------------
		private CFEConfigFile m_menuConfig = null;
		private CFEMenuPageFactory m_pageFactory = null;
		private CFEMenuInputMgr m_inputManager = null;
		private CFEMenuPage m_currentPage = null;
		// --------------------------------------------------------------------
		private CFEString m_baseDir = "";
		private CFEString m_defaultPage = "";
		private CFEString m_startPage = "";
		private CFEString m_nextPage = "";
		private CFEString m_currentPageName = "";
		private CFEString m_previousPage = "";
		// --------------------------------------------------------------------
		public enum MenuManagerState
		{
			None,
			ShowPage,
			Finish,
			NumStates
		}
		private StatedObject<MenuManagerState> m_state = new StatedObject<MenuManagerState>();
		// --------------------------------------------------------------------
		public CFEMenuManager()
		{
			m_state.OnEnterState = OnEnterState;
			m_state.OnExitState = OnExitState;
		}
		// --------------------------------------------------------------------
		public override void _Ready()
		{
			m_pageFactory = new CFEMenuPageFactory();
			m_inputManager = new CFEMenuInputMgr();

			AddChild(m_inputManager);

			m_inputManager.EnableCursorInput(true);
			m_inputManager.EnableButtonInput(true);

			if (MenuFilename != "")
			{
				Init(MenuFilename, m_pageFactory, m_inputManager);
			}
			else
			{
				GD.Print("No filename provided for menu manager");
			}
		}
		// --------------------------------------------------------------------
		public override void _Process(float _deltaT)
		{
			Update(_deltaT);
		}
		// --------------------------------------------------------------------
		public void Init(CFEString _menuFilename, CFEMenuPageFactory _pageFactory, CFEMenuInputMgr _inputManager)
		{
			Init(_menuFilename, "", _pageFactory, _inputManager);
		}
		// --------------------------------------------------------------------
		public void Init(CFEString _menuFilename, CFEString _initialPage, CFEMenuPageFactory _pageFactory, CFEMenuInputMgr _inputManager)
		{
			m_pageFactory = _pageFactory;
			m_inputManager = _inputManager;

			m_menuConfig = new CFEConfigFile($"{_menuFilename}.mnu");
			if (m_menuConfig.bInitialized())
			{
				m_defaultPage = m_menuConfig.sGetString("Menu.DefaultPage", "");
				m_startPage = m_menuConfig.sGetString("Menu.StartPage", "");
				m_baseDir = "res://Assets/base/common/" + m_menuConfig.sGetString("Menu.BaseDir", "");

				m_state.SetReentranceAllowed(true);

				m_nextPage = string.IsNullOrEmpty(_initialPage) ? m_startPage : _initialPage;
				m_previousPage = m_nextPage;

				m_state.ChangeState(MenuManagerState.ShowPage);
			}
		}
		// --------------------------------------------------------------------
		public void Finish()
		{
			if (m_currentPage != null)
			{
				m_currentPage.Finish();
			}

			m_currentPage = null;
			m_menuConfig = null;
		}
		// --------------------------------------------------------------------
		public bool IsFinished()
		{
			return GetState() == MenuManagerState.Finish;
		}
		// --------------------------------------------------------------------
		public virtual void Update(float _deltaT)
		{
			if (GetState() == MenuManagerState.ShowPage)
			{
				if (m_currentPage != null)
				{
					m_currentPage.Update(_deltaT);
					if (m_currentPage.bFinished())
					{
						var nextPage = m_currentPage.sGetPageToGo();
						switch (nextPage)
						{
							case "GotoNext":
							{
								m_nextPage = m_currentPage.sGetNextPage();
							}
							break;
							case "GotoBack":
								m_nextPage = m_currentPage.sGetPrevPage();
								break;
							case "GotoStart":
								m_nextPage = m_startPage;
								break;
							case "GotoDefault":
								m_nextPage = m_defaultPage;
								break;
							default:
								m_nextPage = nextPage;
								break;
						}

						if (m_nextPage == "GotoEnd")
						{
							m_state.ChangeState(MenuManagerState.Finish);
						}
						else if (m_nextPage != m_currentPageName)
						{
							m_state.ChangeState(MenuManagerState.ShowPage);
						}
					}
				}
			}
		}
		// --------------------------------------------------------------------
		private void OnEnterState(MenuManagerState _state)
		{
			if (_state == MenuManagerState.ShowPage)
			{
				while (GetChildCount() > 0)
				{
					RemoveChild(GetChild(0));
				}

				m_currentPage = m_pageFactory.Create(m_nextPage);
				if (m_currentPage != null)
				{
					m_currentPage.SetNextPage(m_menuConfig.sGetString($"Menu.{m_nextPage}.NextPage", ""));
					m_currentPage.SetPrevPage(m_menuConfig.sGetString($"Menu.{m_nextPage}.PrevPage", m_currentPageName));
					m_currentPage.Init($"{m_baseDir}/{m_nextPage}", m_inputManager, m_menuConfig);
					AddChild(m_currentPage.oGetHUD());

					m_currentPageName = m_nextPage;
					m_nextPage = "";
				}
				else
				{
					m_currentPageName = "";
					m_nextPage = "";
				}
			}
		}
		// --------------------------------------------------------------------
		private void OnExitState(MenuManagerState _state, MenuManagerState _newState)
		{
			if (_state == MenuManagerState.ShowPage)
			{
				m_previousPage = m_currentPageName;
				if (m_currentPage != null)
				{
					m_currentPage.Finish();
				}

				m_currentPage = null;
			}
		}
		// --------------------------------------------------------------------
		public void ChangeToPage(CFEString _nextPage)
		{
			m_nextPage = _nextPage;
			m_state.ChangeState(MenuManagerState.ShowPage);
		}
		// --------------------------------------------------------------------
		public MenuManagerState GetState()
		{
			return m_state.GetState();
		}
		// --------------------------------------------------------------------
	}
}
