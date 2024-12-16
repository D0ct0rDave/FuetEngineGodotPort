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
	public class CFEHUDElemLocator : CFEHUDVisitor
	{
		// --------------------------------------------------------------------
		// private static string	m_sWorkingDir;
		// private static int		m_uiFileVersion;
		private static string m_sName;
		private object m_oHUDElem;

		// --------------------------------------------------------------------
		enum SearchType
		{
			ST_ELEMACTION = 0,
			ST_OBJACTION = 1,
			ST_HUDELEM = 2,
			ST_HUDOBJ = 3
		}
		private SearchType m_searchType = SearchType.ST_ELEMACTION;
		//-----------------------------------------------------------------------------
		public CFEHUDElement oLocateHUDElement(CFEHUD _oHUDModel, CFEString _sName)
		{
			if ((_oHUDModel == null) || (_sName == ""))
			{
				return null;
			}

			m_sName = _sName;
			m_oHUDElem = null;
			m_searchType = SearchType.ST_HUDELEM;

			_oHUDModel.Accept(this);

			return ((CFEHUDElement)m_oHUDElem);
		}
		//-----------------------------------------------------------------------------
		/// Retrieves the object that matches with the specified name.
		public CFEHUDObject oLocateHUDObject(CFEHUDElement _oElem, CFEString _sName)
		{
			if ((_oElem == null) || (_sName == "")) return null;

			m_sName = _sName;
			m_oHUDElem = null;
			m_searchType = SearchType.ST_HUDOBJ;

			_oElem.Accept(this);

			return m_oHUDElem as CFEHUDObject;
		}
		// --------------------------------------------------------------------
		public override void Visit(CFEHUDElement _oObj)
		{
			switch (m_searchType)
			{
				case SearchType.ST_HUDELEM:
				{
					if (_oObj.Name == m_sName)
					{
						m_oHUDElem = _oObj;
					}
				}
				break;

				case SearchType.ST_HUDOBJ:
				{
					for (int i = 0; i < _oObj.iNumLayers(); i++)
					{
						_oObj.oGetLayer(i).Accept(this);
						if (m_oHUDElem != null) return;
					}
				}
				break;

				case SearchType.ST_OBJACTION:
				case SearchType.ST_ELEMACTION:
				{
					for (int i = 0; i < _oObj.iNumActions(); i++)
					{
						/*
						TODO:
						_oObj.oGetAction(i).Accept(this);
						if (m_oHUDElem != null) return;
						*/
					}
				}
				break;
			}
		}
		// --------------------------------------------------------------------
		public override void Visit(CFEHUDObject _oObj)
		{
		}
		// --------------------------------------------------------------------
		public override void Visit(CFEHUDGroup _oObj)
		{
			if ((_oObj.Name == m_sName) && (m_searchType == SearchType.ST_HUDOBJ))
			{
				m_oHUDElem = _oObj;
				return;
			}

			for (int i = 0; i < _oObj.iNumObjs(); i++)
			{
				if (_oObj.oGetObject(i) != null)
				{
					_oObj.oGetObject(i).Accept(this);

					if (m_oHUDElem != null)
					{
						return;
					}
				}
			}
		}
		// --------------------------------------------------------------------
		public override void Visit(CFEHUDLabel _oObj)
		{
			if ((_oObj.Name == m_sName) && (m_searchType == SearchType.ST_HUDOBJ))
			{
				m_oHUDElem = _oObj;
			}
		}
		// --------------------------------------------------------------------
		public override void Visit(CFEHUDIcon _oObj)
		{
			if ((_oObj.Name == m_sName) && (m_searchType == SearchType.ST_HUDOBJ))
			{
				m_oHUDElem = _oObj;
			}
		}
		// --------------------------------------------------------------------
		public override void Visit(CFEHUDRect _oObj)
		{
			if ((_oObj.Name == m_sName) && (m_searchType == SearchType.ST_HUDOBJ))
			{
				m_oHUDElem = _oObj;
			}
		}
		// --------------------------------------------------------------------
		public override void Visit(CFEHUDShape _oObj)
		{
			if ((_oObj.Name == m_sName) && (m_searchType == SearchType.ST_HUDOBJ))
			{
				m_oHUDElem = _oObj;
			}
		}
		// --------------------------------------------------------------------
		public override void Visit(CFEHUDPSys _oObj)
		{
			if ((_oObj.Name == m_sName) && (m_searchType == SearchType.ST_HUDOBJ))
			{
				m_oHUDElem = _oObj;
			}
		}
	}
}
//-----------------------------------------------------------------------------
