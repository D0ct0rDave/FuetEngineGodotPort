//-----------------------------------------------------------------------------
namespace FuetEngine
{
	public static class CFEMenuDefinitions
	{
		public const string ENTER_PAGE_EVENT_NAME = "OnEnterPage";
		public const string EXIT_PAGE_EVENT_NAME = "OnExitPage";
		public const string IDLE_PAGE_EVENT_NAME = "OnIdlePage";

		public const string RELEASE_BUTTON_EVENT_NAME = "OnRelease";
		public const string PRESS_BUTTON_EVENT_NAME = "OnPress";
		public const string ENTER_BUTTON_EVENT_NAME = "OnEnter";
		public const string SELECT_BUTTON_EVENT_NAME = "OnSelect";
		public const string UNSELECT_BUTTON_EVENT_NAME = "OnUnselect";

#if DISABLE_MENU_FOCUS_EVENTS
		public const string FOCUS_BUTTON_EVENT_NAME = "___OnFocus___";
		public const string LOSEFOCUS_BUTTON_EVENT_NAME = "___OnLoseFocus___";
#else
		public const string FOCUS_BUTTON_EVENT_NAME = "OnFocus";
		public const string LOSEFOCUS_BUTTON_EVENT_NAME = "OnLoseFocus";
#endif

		public const string IDLE_BUTTON_EVENT_NAME = "OnIdle";
		public const string ENABLE_BUTTON_EVENT_NAME = "OnEnable";
		public const string DISABLE_BUTTON_EVENT_NAME = "OnDisable";
		public const string CHECK_BUTTON_EVENT_NAME = "OnCheck";
		public const string UNCHECK_BUTTON_EVENT_NAME = "OnUnCheck";
	}
}

// -----------------------------------------------------------------------------
