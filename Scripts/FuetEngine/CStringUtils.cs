using System;
using System.IO;
using System.Collections.Generic;

namespace FuetEngine
{
	public class CFEStringUtils
	{
		// ----------------------------------------------------------------------------
		static public string sGetPath(string _sFilename)
		{
			Uri uri = new Uri(_sFilename);
			string baseUrl = uri.Scheme + "://" + uri.Host + uri.AbsolutePath.Replace(Path.GetFileName(uri.LocalPath), string.Empty);
			return baseUrl;
		}
		// ----------------------------------------------------------------------------
		static public string sGetFilename(string _sFilename)
		{
			return Path.GetFileNameWithoutExtension(_sFilename);
		}
		// ----------------------------------------------------------------------------
		public static string sGetCanonicalPath(string _sPath)
		{
			if (_sPath == "") return (_sPath);

			// ----------------------------------------------
			// Are there really any relative directories?
			// ----------------------------------------------
			int iPos = _sPath.LastIndexOf("./");
			if (iPos <= 0) return (_sPath);

			// ----------------------------------------------
			int iIdx = 0;
			char[] szPath = _sPath.ToCharArray();
			List<string> m_sDirs = new List<string>();
			string sDir = "";

			while (iIdx != _sPath.Length)
			{
				if (szPath[iIdx] == '/')
				{
					m_sDirs.Add(sDir);
					sDir = "";
				}
				else
				{
					sDir += szPath[iIdx];
				}

				iIdx++;
			}

			int i;
			for (i = 0; i < m_sDirs.Count;)
			{
				if (m_sDirs[i] == "..")
				{
					if ((i > 0) && (m_sDirs[i - 1] != ".."))
					{
						m_sDirs.RemoveAt(i);
						m_sDirs.RemoveAt(i - 1);
						i--;
					}
					else
					{
						// can't do anything
						i++;
					}
				}
				else if (m_sDirs[i] == ".")
				{
					m_sDirs.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}

			string sRes = "";
			for (i = 0; i < m_sDirs.Count; i++)
			{
				sRes += m_sDirs[i] + '/';
			}

			sRes += sDir;
			return (sRes);
		}
	}
};