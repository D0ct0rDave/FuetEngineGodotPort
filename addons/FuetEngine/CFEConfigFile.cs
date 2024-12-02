using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuetEngine
{
    public class CFEConfigFile
    {
        /// Default constructor for this class.
        public CFEConfigFile()
        {
        }

        /// Creates the object and opens the configuration file ready for reading.
        public CFEConfigFile(string _sConfigFile)
        {
            m_bInitialized = bLoad(_sConfigFile);
        }

        /// Loads the configuration file ready for reading.
        public bool bLoad(string _sConfigFile)
        {
            m_oReader = new CLibConfigReader();
            m_bInitialized = m_oReader.bRead(_sConfigFile);
            return(m_bInitialized);
        }

        /// Retrieves whether the configuration file was correctly opened or not.
        public bool bInitialized()
        {
            return( m_bInitialized );
        }

        /// Retrieves a string variable value from the configuration file
        public string sGetString(string _sVar,string _sDefaultValue)
        {
            if (m_bInitialized)
            {
                if (m_oReader.bExists(_sVar))
                {
                    CVariable oVar = m_oReader.oGetVariable(_sVar);
                    return (oVar.sGetValue());
                }
                else
                    return (_sDefaultValue);
            }
            else
                return (_sDefaultValue);
        }


        /// Retrieves a float variable value from the configuration file
        public float rGetReal(string _sVar,float _rDefaultValue)
        {
            if (m_bInitialized)
            {
                if (m_oReader.bExists(_sVar))
                {
                    CVariable oVar = m_oReader.oGetVariable(_sVar);
                    return (oVar.rGetValue());
                }
                else
                    return (_rDefaultValue);
            }
            else
                return (_rDefaultValue);
        }

        /// Retrieves a integer variable value from the configuration file
        public int iGetInteger(string _sVar,int _iDefaultValue)
        {
            if (m_bInitialized)
            {
                if (m_oReader.bExists(_sVar))
                {
                    CVariable oVar = m_oReader.oGetVariable(_sVar);
                    return (oVar.iGetValue());
                }
                else
                    return (_iDefaultValue);
            }
            else
                return (_iDefaultValue);
        }


        /// Retrieves a bool variable value from the configuration file
        public bool bGetBool(string _sVar,bool _bDefaultValue)
        {
            if (m_bInitialized)
            {
                if (m_oReader.bExists(_sVar))
                {
                    CVariable oVar = m_oReader.oGetVariable(_sVar);
                    return (oVar.bGetValue());
                }
                else
                    return (_bDefaultValue);
            }
            else
                return (_bDefaultValue);
        }


        /// Retrieves whether the given variable exists or not.
        public bool bExists(string _sVar)
        {
            if (m_bInitialized)
                return (m_oReader.bExists(_sVar));
            else
                return (false);
        }

        ///
        protected CLibConfigReader m_oReader;
        protected bool m_bInitialized = false;
    };
}
