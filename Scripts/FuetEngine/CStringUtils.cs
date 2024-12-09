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
			List<string> pathElements = ParsePathElements(_sFilename);
			// remove possible filename
			if (pathElements.Count > 1)
			{
				pathElements.RemoveAt(pathElements.Count-1);
				CompactPathElements(ref pathElements);
				string canonicalString = BuildPathFromPathElements(pathElements);
				return canonicalString;
			}
			else if (pathElements.Count == 1)
			{
				return pathElements[0];
			}
			else
			{
				return "";
			}

			// Uri uri = new Uri(_sFilename);
			// string baseUrl = uri.Scheme + "://" + uri.Host + uri.AbsolutePath.Replace(Path.GetFileName(uri.LocalPath), string.Empty);
			// return baseUrl;
		}
		// ----------------------------------------------------------------------------
		static public string sGetFilename(string _sFilename)
		{
			return Path.GetFileNameWithoutExtension(_sFilename);
		}
		// ----------------------------------------------------------------------------
		static public List<string> ParsePathElements(string _sPath)
		{
			List<string> sElements = new List<string>();
			if (_sPath == "") return (sElements);

			string sProcessedPath = _sPath.Replace("\\", "/");

			// encode possible uri scheme token
			string schemeToken = "|#@$|";

			//adding an extra "/" helps identifying the scheme as an element
			sProcessedPath = sProcessedPath.Replace("//", schemeToken + "/"); 

			// ----------------------------------------------
			// Are there really any relative directories?
			// ----------------------------------------------
			int iIdx = 0;
			char[] szPath = sProcessedPath.ToCharArray();
			string sDir = "";

			while (iIdx != sProcessedPath.Length)
			{
				if (szPath[iIdx] == '/')
				{
					sElements.Add(sDir);
					sDir = "";
				}
				else
				{
					sDir += szPath[iIdx];
				}

				iIdx++;
			}
			if (sDir != "")
			{
				// Last element
				sElements.Add(sDir);
			}

			// decode possible uri scheme token
			if (sElements.Count>0)
			{
				sElements[0] = sElements[0].Replace(schemeToken, "//");
			}
			
			return (sElements);
		}
		// ----------------------------------------------------------------------------
		static public void CompactPathElements(ref List<string> _pathElements)
		{
			int i;
			for (i = 0; i < _pathElements.Count;)
			{
				if (_pathElements[i] == "..")
				{
					if ((i > 0) && (_pathElements[i - 1] != ".."))
					{
						_pathElements.RemoveAt(i);
						_pathElements.RemoveAt(i - 1);
						i--;
					}
					else
					{
						// can't do anything
						i++;
					}
				}
				else if (_pathElements[i] == ".")
				{
					_pathElements.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
		// ----------------------------------------------------------------------------
		public static string BuildPathFromPathElements(List<string> _pathElements)
		{
			string sRes = "";
			for (int i = 0; i < _pathElements.Count; i++)
			{				
				sRes += _pathElements[i];
				if ( !_pathElements[i].Contains("//") && (i != (_pathElements.Count-1)) )
				{
					sRes += '/';
				}
			}
			return sRes;
		}
		// ----------------------------------------------------------------------------
		public static string sGetCanonicalPath(string _sPath)
		{
			if (_sPath == "") return _sPath;

			string sProcessedPath = _sPath.Replace("\\", "/");
			
			// ----------------------------------------------
			// Are there really any relative directories?
			// ----------------------------------------------
			int iPos = sProcessedPath.LastIndexOf("./");
			if (iPos <= 0)
			{
				return sProcessedPath;			
			}
			else
			{
				List<string> pathElements = ParsePathElements(sProcessedPath);
				CompactPathElements(ref pathElements);
				string sRes = BuildPathFromPathElements(pathElements);

				return sRes;
			}
		}
	}
};