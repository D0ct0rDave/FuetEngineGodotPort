using System;
public partial class StatedObject<T>
{
    /*
	T can be anything! int, string, enum

	BEST USAGE:
	private StatedObject<T> _myState = new StatedObject<T>();
    public StatedObject<T> MyState
    {
        get => _myState // GetState() // (?);
        set => _myState.ChangeState(value);
    }
	*/
	
	// ---------------------------------------------------------------
	// BEGIN: Functions to override
	// ---------------------------------------------------------------

	/// Specific code to perform when the object enters to a given state.
	public delegate void OnEnterStateCallback(T _state);
	public OnEnterStateCallback OnEnterState;
	
	public delegate void OnExitStateCallback(T _state, T _newState);
	public OnExitStateCallback OnExitState;

	// ---------------------------------------------------------------
	// END: Functions to override
	// ---------------------------------------------------------------
	/// Retrieve the previous state of the object
	public T GetPrevState()
	{
		return(m_prevState);
	}
	
	/// Retrieve the state of the object
	public T GetState()
	{
		return(m_state);
	}
	
	/// Force the object to a given state: Should not be called directly unless strictly necessary.
	public void SetState(T _state)
	{			
		m_prevState	= m_state;
		m_state		= _state;
	}

	///  Changes the object to a given state, forcing the performing Exit and Enter events.
	public void ForceState(T _state)
	{
		if (OnExitState != null)
		{
			OnExitState(m_state, _state);
		}

		m_stateTime = 0.0f;
		if (OnEnterState != null)
		{
			OnEnterState(_state);
		}

		SetState(_state);
	}
	
	/// Changes the object to a given state, performing Exit and Enter events.
	public void ChangeState(T _state)
	{
		if ((! m_allowsReentrance) && _state.Equals(m_state)) return;
		ForceState(_state);
	}

	/// Enables or disables whether the object allows reentering to the current state.
	public void SetReentranceAllowed(bool _allowsReentrance)
	{
		m_allowsReentrance = _allowsReentrance;
	}
	
	/// Retrieves if the object allows reentering to the current state.
	public bool AllowsReentrance()
	{
		return(m_allowsReentrance);
	}
	
	/// Sets up the initial time of the state.
	public void SetStateTime(double _stateTime)
	{
		m_stateTime = _stateTime; 
	}
	
	/// Retrieves the current state time.
	public double GetStateTime()
	{
		return(m_stateTime);
	}
	
	/// Updates the time of the current state. Returns true if the state time has not yet reached 0.
	public bool UpdateStateTime(double _deltaT)
	{
		m_stateTime -= _deltaT;
		return(m_stateTime > 0.0f);
	}
	
	// equality operators
    public static bool operator ==(StatedObject<T> stateObj, T state)
    {
        return stateObj.GetState().Equals(state);
    }

    public static bool operator !=(StatedObject<T> stateObj, T state)
    {
        return !stateObj.GetState().Equals(state);
    }

    // Override Equals anf GetHashCode to avoid warnings
    public override bool Equals(object obj)
    {
        if (obj is StatedObject<T> other)
        {
            return Equals(m_state, other.m_state);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return m_state.GetHashCode();
    }
	
	// ---------------------------------------------------------------
	// Current object state
	private T m_state;
	
	// Previous object state
	private T m_prevState;
	
	// Allow state reentrance?
	private bool m_allowsReentrance = false;
	
	/// The time of the current state.
	private double m_stateTime = 0.0;
	// ---------------------------------------------------------------
}
