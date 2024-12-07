//------------------------------------------------------------------------------
// This implementation was done before knowing the object type. 
// As this implementation works, we'll be using it as it is.
//------------------------------------------------------------------------------
using System.Globalization;

namespace FuetEngine
{
    public class CVariable
	{
		public enum EPropertyType
		{
			PT_NONE,
			PT_BOOL,
			PT_INTEGER,
			PT_STRING,
			PT_REAL,
		};

		public CVariable ()
		{
			m_sName = "";
			m_eType = EPropertyType.PT_NONE;
		}

		// copy constructor?
		public CVariable(CVariable right)
		{
			m_sName  = right.m_sName; 
			m_eType  = right.m_eType;
			m_oValue = right.m_oValue;
		}

		/// Constructor for the class.
		public CVariable(string _sVarName, string _sValue)
		{
			m_sName = _sVarName;
			m_oValue.m_sValue = _sValue;

			NumberFormatInfo oInfo = new NumberFormatInfo();
			oInfo.NumberDecimalSeparator = ".";

				/// Guess the type of the variable
			// float
			if (_sValue.Contains(".") && float.TryParse(_sValue, NumberStyles.Float | NumberStyles.AllowThousands,oInfo, out m_oValue.m_fValue))
			{
				// m_oValue.m_fValue = Convert.ToSingle(m_sValue);
				m_eType = EPropertyType.PT_REAL;
			}
			// integer
			else if (int.TryParse(_sValue,out m_oValue.m_iValue))
			{
				m_eType = EPropertyType.PT_INTEGER;
			}
			// boolean true
			else if (_sValue == "true")
			{
				m_oValue.m_bValue = true;
				m_eType = EPropertyType.PT_BOOL;
			}
			// boolean false
			else if (_sValue == "false")
			{
				m_oValue.m_bValue = false;
				m_eType = EPropertyType.PT_BOOL;
			}
			else
			{
				// string
				m_eType = EPropertyType.PT_STRING;
			}
		}
			
		// ----------------------------
		/// Retrieves the name of the property.
		public string sGetName()
		{
			return(m_sName);
		}
		// ----------------------------
		public EPropertyType eGetType()
		{
			return(m_eType);
		}
		// ----------------------------
		public bool bGetValue()
		{
			// boolean true
			return(m_oValue.m_bValue);
		}
		// ----------------------------		
		public int iGetValue()
		{
			return(m_oValue.m_iValue);
		}
		// ----------------------------
		public float rGetValue()
		{
			return(m_oValue.m_fValue);
		}
		// ----------------------------
		public string sGetValue()
		{
			return(m_oValue.m_sValue);
		}
		// ----------------------------
				
		protected string		m_sName;    // variable name
		protected EPropertyType	m_eType;	// the type of this variable.

		// In C++ this is a Union
		protected class CUValues
		{
			public CUValues()
			{
				m_sValue = "";
				m_iValue = 0; 
				m_fValue = 0.0f; 
				m_bValue = false;
			}

			public string	m_sValue;
			public int		m_iValue;
			public float	m_fValue;
			public bool		m_bValue;

			// bool, int, float, string
		};

		protected CUValues m_oValue = new CUValues();
	}
}
