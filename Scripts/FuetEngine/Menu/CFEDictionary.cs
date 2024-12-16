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
	//-------------------------------------------------------------------------
	class CFEDictionary
	{

		// Database to store key/value pairs in the form of a config file.
		private CFEConfigFile m_oDict;

		//---------------------------------------------------------------------
		/// Default constructor to build a dictionary around the given file data.
		CFEDictionary(CFEString _sDictionary)
		{
			CFEString sDict = _sDictionary + ".dic";
			m_oDict = new CFEConfigFile(sDict);
			if (!m_oDict.bInitialized())
			{
				delete m_oDict;
				m_oDict = null;
			}
		}
		//-------------------------------------------------------------------------
		/// Retrieves the value of the given variable, or it's default given value if the variable is not found.
		CFEString sGetString(CFEString _sVariable, CFEString _sDefaultValue)
		{
			if (m_oDict != null)
			{
				return( m_oDict.sGetString(CFEString("Dictionary.") + _sVariable,_sDefaultValue) );
			}
			else
			{
				return(_sDefaultValue);
			}
		}
		//-------------------------------------------------------------------------
		bool bExists(CFEString _sVariable)
		{
			if (m_oDict != null)
			{
				return( m_oDict->bExists( CFEString("Dictionary.") + _sVariable) );
			}
			else
			{
				return(false);
			}
		}
		//-------------------------------------------------------------------------
		bool bInitialized()
		{
			return(m_oDict != null);
		}
	}
	// ----------------------------------------------------------------------------
}
