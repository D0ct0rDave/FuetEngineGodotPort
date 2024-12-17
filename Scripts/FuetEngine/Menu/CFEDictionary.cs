using System;
using Godot;
//-----------------------------------------------------------------------------
using CFEString = System.String;
//-----------------------------------------------------------------------------
namespace FuetEngine
{
	//-------------------------------------------------------------------------
	public class CFEDictionary
	{

		// Database to store key/value pairs in the form of a config file.
		private CFEConfigFile m_oDict;

		//---------------------------------------------------------------------
		/// Default constructor to build a dictionary around the given file data.
		public CFEDictionary(CFEString _sDictionary)
		{
			CFEString sDict = _sDictionary + ".dic";
			m_oDict = new CFEConfigFile(sDict);
			if (!m_oDict.bInitialized())
			{
				m_oDict = null;
			}
		}
		//-------------------------------------------------------------------------
		/// Retrieves the value of the given variable, or it's default given value if the variable is not found.
		public CFEString sGetString(CFEString _sVariable, CFEString _sDefaultValue)
		{
			if (m_oDict != null)
			{
				return m_oDict.sGetString("Dictionary." + _sVariable, _sDefaultValue);
			}
			else
			{
				return (_sDefaultValue);
			}
		}
		//-------------------------------------------------------------------------
		public bool bExists(CFEString _sVariable)
		{
			if (m_oDict != null)
			{
				return m_oDict.bExists("Dictionary." + _sVariable);
			}
			else
			{
				return false;
			}
		}
		//-------------------------------------------------------------------------
		public bool bInitialized()
		{
			return m_oDict != null;
		}
	}
	// ----------------------------------------------------------------------------
}
