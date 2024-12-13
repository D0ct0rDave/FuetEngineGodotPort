using Godot;
//-----------------------------------------------------------------------------
using CFEString = System.String;
using FEReal = System.Single;
using CFEVect2 = Godot.Vector2;
using CFEColor = Godot.Color;
//-----------------------------------------------------------------------------
namespace FuetEngine
{
	public static class CFEHUDLoader
	{
		private static string	m_sWorkingDir;
		private static int	m_uiFileVersion;

		// Accumulated depth
		private static float m_rDepth = 0.0f;

		// Current depth factor
		private static float m_rDepthFact = 1.0f;

		// creation counter to make object render properly.
		private static int m_iObjectIdx = 0;
		  // ------------------------------------------------------------------------------
		private static void Reset()
		{
			m_rDepthFact = 1.0f;
			m_iObjectIdx = 0;
		}
		// ------------------------------------------------------------------------------
		/// Loads a HUD from a given file    
		public static CFEHUD oLoad(CFEString _sFilename)
		{
			string sFilename = _sFilename + ".hud";
			m_sWorkingDir = CFEStringUtils.sGetPath(_sFilename);
			
			CFEConfigFile oConfig = new CFEConfigFile(sFilename);
			if (! oConfig.bInitialized() ) return null;

			/// Initialize the HUDLoader internal variables
			Reset();

			CFEHUD oHUD = Support.CreateObject<Node2D, CFEHUD>(FuetEngine.Support.HUD_SCRIPT_FILE);

			// FileVersion
			m_uiFileVersion = oConfig.iGetInteger("HUD.FileVersion",1);
			
			// Number of elements.
			int uiNumElements = oConfig.iGetInteger("HUD.NumElements",0);
			
			// Read elements.
			for (int j=0; j<uiNumElements; j++)
			{
				string sVar = "HUD.Element" + j;
				CFEHUDElement oElem = oLoadElement(sVar, oConfig);
				
				// Add the new element.
				oHUD.iAddElement(oElem);
			}
			
			return oHUD;
		}
		// ------------------------------------------------------------------------------
		/// Loads a HUD element from disk
		public static CFEHUDElement oLoadElement(CFEString _sFilename)
		{
			string sFilename = _sFilename + ".hel";
			m_sWorkingDir = CFEStringUtils.sGetPath(_sFilename);
			
			CFEConfigFile oConfig = new CFEConfigFile(sFilename);
			if (! oConfig.bInitialized() ) return(null);

			return oLoadElement("HUDElement",oConfig);
		}
		// ------------------------------------------------------------------------------
		/// Loads a HUD element actions from disk
		public static void LoadElementActions(CFEString _sFilename,CFEHUDElement _poElem)
		{
		}
		// ------------------------------------------------------------------------------
		/// Loads a HUD object from disk
		public static CFEHUDObject oLoadObject(string _sFilename)
		{
			string sFilename = _sFilename + ".hob";
			m_sWorkingDir = CFEStringUtils.sGetPath(_sFilename);
			
			CFEConfigFile oConfig = new CFEConfigFile(sFilename);
			if (! oConfig.bInitialized() ) return(null);

			return oLoadObject("HUDObject", oConfig);
		}
		// ------------------------------------------------------------------------------	
		private static CFEHUDElement oLoadElement(CFEString _sPrefix, CFEConfigFile _oConfigFile)
		{
			// Element name
			CFEString sElemName = _oConfigFile.sGetString(_sPrefix + ".Name","nonamed-element");

			CFEHUDElement oElem = Support.CreateObject<CFEHUDElement>(FuetEngine.Support.HUD_ELEMENT_SCRIPT_FILE);
			oElem.Name = sElemName;

			// Number of layers
			int uiNumLayers = _oConfigFile.iGetInteger(_sPrefix + ".NumLayers",0);
			
			int i;
			for (i=0;i<uiNumLayers;i++)
			{
				string sPrefix = _sPrefix + ".Layer" + i;
				CFEHUDObject oObj = oLoadObject(sPrefix, _oConfigFile);
				
				oElem.iAddLayer(oObj);
			}
			
			// Number of actions
			LoadElementActions(_sPrefix, _oConfigFile, oElem);
			
			return oElem;
		}
		// ------------------------------------------------------------------------------
		private static void LoadElementActions(CFEString _sPrefix, CFEConfigFile _oConfigFile, CFEHUDElement _poElem)
		{
		}
		// ------------------------------------------------------------------------------
		private static CFEHUDElementAction oLoadAction(CFEString _sPrefix, CFEConfigFile _oConfigFile, CFEHUDElement _poElem)
		{
			return null;
		}
		// ------------------------------------------------------------------------------
		private static CFEHUDObjectAction oLoadObjAction(CFEString _sPrefix, CFEConfigFile _oConfigFile, CFEHUDElement _poElem)
		{
			return null;
		}
		// ------------------------------------------------------------------------------
		private static void LoadCommonObjectProperties(CFEString _sPrefix, CFEConfigFile _oConfigFile, CFEHUDObject _oObj)
		{
			// Name
			string sElemName = _oConfigFile.sGetString(_sPrefix + ".Name","nonamed");
			_oObj.Name = sElemName;

			// position & depth
			float rX = _oConfigFile.rGetReal(_sPrefix + ".Position.x",0.0f);
			float rY = _oConfigFile.rGetReal(_sPrefix + ".Position.y",0.0f);
			float rD = _oConfigFile.rGetReal(_sPrefix + ".Depth",0.0f);
			
			_oObj.SetIniPos( new CFEVect2(rX,rY) );
			_oObj.SetIniDepth(rD);

			// scale
			float rSX = _oConfigFile.rGetReal(_sPrefix + ".Scale.x",1.0f);
			float rSY = _oConfigFile.rGetReal(_sPrefix + ".Scale.y",1.0f);
			_oObj.SetIniScale( new CFEVect2(rSX,rSY) );

			// angle
			float rA = _oConfigFile.rGetReal(_sPrefix + ".Angle",0.0f);
			_oObj.SetIniAngle( rA );

			// color
			float rCR = _oConfigFile.rGetReal(_sPrefix + ".Color.r",1.0f);
			float rCG = _oConfigFile.rGetReal(_sPrefix + ".Color.g",1.0f);
			float rCB = _oConfigFile.rGetReal(_sPrefix + ".Color.b",1.0f);
			float rCA = _oConfigFile.rGetReal(_sPrefix + ".Color.a",1.0f);
			_oObj.SetIniColor(new CFEColor(rCR,rCG,rCB,rCA) );

			// visibility
			bool bVisible = _oConfigFile.bGetBool(_sPrefix + ".Visible",true);
			_oObj.ShowObj( bVisible );

			// action
			int iAction = _oConfigFile.iGetInteger(_sPrefix + ".Action",-1);
			_oObj.SetIniAction( iAction );

			// TAG
			string sTAG = _oConfigFile.sGetString(_sPrefix + ".TAG","");
			_oObj.SetTAG( sTAG );

			// Setup proper depth and color (acummulated while traversing the hierarchy)
			_oObj.ZIndex = iGetRenderIndex(_oObj);
			_oObj.ZAsRelative = false;
		}
		// ------------------------------------------------------------------------------
		private static CFEHUDObject oLoadObject(string _sPrefix, CFEConfigFile _oConfigFile)
		{
			// Creation factor allows objects to be rendered properly when same depth (prevent z fighting)
			// This makes the rendering depend on creation order.
			m_iObjectIdx--;

			string sElemType = _oConfigFile.sGetString(_sPrefix + ".Type","none");
			string sElemName = _oConfigFile.sGetString(_sPrefix + ".Name","nonamed");

			if (sElemType == "Label")
			{
				CFEHUDLabel oLabel = Support.CreateObject<CFEHUDLabel>(FuetEngine.Support.HUD_LABEL_SCRIPT_FILE);
				LoadCommonObjectProperties(_sPrefix,_oConfigFile, oLabel);
				
				string sText = _oConfigFile.sGetString(_sPrefix + ".Text","Label");
				oLabel.SetText(sText);
				
				string sFont = _oConfigFile.sGetString(_sPrefix + ".Font","");
				if (sFont != "")
				{
					string sFontFile = m_sWorkingDir + "/" + sFont;
					// CFEFont poFont = CFEFontMgr::I()->poLoad( sFontFile );
					oLabel.SetFont(sFontFile);
				}
				
				string sHAlign = _oConfigFile.sGetString(_sPrefix + ".HAlignment","left");
				oLabel.SetHAlignment( eGetHAlignFromString(sHAlign) );
				
				string sVAlign = _oConfigFile.sGetString(_sPrefix + ".VAlignment","center");
				oLabel.SetVAlignment( eGetVAlignFromString(sVAlign) );
				
				float rTracking, rInterlining, rPointSize,rAdjustmentWidth;
				rTracking	 = _oConfigFile.rGetReal(_sPrefix + ".Tracking",0.0f);
				rInterlining = _oConfigFile.rGetReal(_sPrefix + ".Interlining",0.0f);
				rPointSize   = _oConfigFile.rGetReal(_sPrefix + ".PointSize",1.0f);
				rAdjustmentWidth = _oConfigFile.rGetReal(_sPrefix + ".AdjustmentWidth",-1.0f);
				
				oLabel.SetTracking(rTracking);
				oLabel.SetInterlining(rInterlining);
				oLabel.SetPointSize(rPointSize);
				oLabel.SetAdjustmentWidth(rAdjustmentWidth);

				return oLabel;
			}
			
			else if (sElemType == "Icon")
			{
				CFEHUDIcon oIcon = Support.CreateObject<CFEHUDIcon>(FuetEngine.Support.HUD_ICON_SCRIPT_FILE);
				LoadCommonObjectProperties(_sPrefix, _oConfigFile, oIcon);

				string sSprite = _oConfigFile.sGetString(_sPrefix + ".Sprite","");
				if (sSprite != "")
				{
					string sSpriteFile = m_sWorkingDir + "/" + sSprite;
					
					CFESprite spriteResource = CFESpriteMgr.Instance.Load(sSpriteFile);
					if (spriteResource != null)
					{
						CFESpriteInstance spriteInstance = Support.CreateObject<CFESpriteInstance>(FuetEngine.Support.SPRITE_INSTANCE_SCRIPT_FILE);

						spriteInstance.Name = "CFESpriteInstance";
						spriteInstance.Init(spriteResource);

						spriteInstance.SetAction(oIcon.iGetIniAction());
						// spriteInstance.m_oColor            = oRenderColor(m_oModColor, poIcon);
						
						oIcon.SetIcon(spriteInstance);
					}
					else
					{
						GD.Print("CFEHUDIcon cannot load sprite resource " + sSpriteFile);
					}
				}

				return oIcon;
			}
			
			else if (sElemType == "Rect")
			{
				CFEHUDRect oRect = Support.CreateObject<CFEHUDRect>(FuetEngine.Support.HUD_RECT_SCRIPT_FILE);
				LoadCommonObjectProperties(_sPrefix, _oConfigFile, oRect);
				
				// Load width / height / pivot
				float rWidth  = _oConfigFile.rGetReal(_sPrefix + ".Width", 0.0f);
				float rHeight = _oConfigFile.rGetReal(_sPrefix + ".Height",0.0f);
				float rPivotX = _oConfigFile.rGetReal(_sPrefix + ".PivotX",0.5f);
				float rPivotY = _oConfigFile.rGetReal(_sPrefix + ".PivotY",0.5f);
				
				oRect.SetWidth(rWidth);
				oRect.SetHeight(rHeight);
				oRect.SetPivot( new CFEVect2(rPivotX, rPivotY) );
				
				// Load corner colors
				for (int i=0; i<4; i++)
				{
					string sCorner = ".Corner" + i;
					string sVar    = _sPrefix + sCorner;

					CFEColor oColor;
					oColor.r = _oConfigFile.rGetReal(sVar + ".r",1.0f);
					oColor.g = _oConfigFile.rGetReal(sVar + ".g",1.0f);
					oColor.b = _oConfigFile.rGetReal(sVar + ".b",1.0f);
					oColor.a = _oConfigFile.rGetReal(sVar + ".a",1.0f);
					
					oRect.SetCornerColor(i, oColor);
				}

				return oRect;
			}
			
			else if (sElemType == "Shape")
			{
				CFEHUDShape oShape = Support.CreateObject<CFEHUDShape>(FuetEngine.Support.HUD_SHAPE_SCRIPT_FILE);
				LoadCommonObjectProperties(_sPrefix, _oConfigFile, oShape);
				
				string sMesh = _oConfigFile.sGetString(_sPrefix + ".Mesh","");
				if (sMesh != "")
				{
					string sMeshFile = m_sWorkingDir + "/" + sMesh;
	
					/*
					FEHandler hMeshInst = CFEMeshInstMgr::I()->hGetInstance(sMeshFile);
					if (hMeshInst != NULL)
					{
						poShape->SetMesh(hMeshInst);
						
						CFEMeshInstMgr::I()->SetAction(hMeshInst,poShape->iGetAction());
						CFEMeshInstMgr::I()->Enable(hMeshInst);
						CFEMeshInstMgr::I()->ManageRender(hMeshInst,false);
					}
					*/
				}
				
				return oShape;
			}
			
			else if (sElemType == "PSys")
			{
				CFEHUDPSys oPSys = Support.CreateObject<CFEHUDPSys>(FuetEngine.Support.HUD_PSYS_SCRIPT_FILE);
				LoadCommonObjectProperties(_sPrefix, _oConfigFile, oPSys);
			
				string sPSys = _oConfigFile.sGetString(_sPrefix + ".PSys","");
				if (sPSys != "")
				{
					string sPSysFile = m_sWorkingDir + "/" + sPSys;
					/*
					FEHandler hPSysInst = CFEParticleSysInstMgr::I()->hGetInstance(sPSysFile);
					
					if (hPSysInst != NULL)
					{
						poPSys->SetPSys(hPSysInst);
						
						CFEParticleSysInstMgr::I()->Enable(hPSysInst);
						CFEParticleSysInstMgr::I()->ManageRender(hPSysInst,false);
					}
					*/
				}
				return oPSys;
			}
			
			else if (sElemType == "Group")
			{
				int iOldObjIdx = m_iObjectIdx;
				m_iObjectIdx = 0;
					
				int uiNumObjects = _oConfigFile.iGetInteger(_sPrefix + ".NumObjects",0);
				
				CFEHUDGroup oGroup = Support.CreateObject<CFEHUDGroup>(FuetEngine.Support.HUD_GROUP_SCRIPT_FILE);

				LoadCommonObjectProperties(_sPrefix, _oConfigFile, oGroup);

				float rDepthFact = _oConfigFile.rGetReal(_sPrefix + ".DepthFact",0.1f);
				oGroup.SetDepthFact(rDepthFact);

				// Push current depth
				float rOldDepthFact = m_rDepthFact;
				float rOldDepth     = m_rDepth;

					m_rDepth    = m_rDepth + m_rDepthFact * oGroup.m_rIniDepth;
					m_rDepthFact *= rDepthFact;                                 // order important!!!!

					for (int i=0;i<uiNumObjects;i++)
					{
						string sObject = _sPrefix + ".Object" + i;
						CFEHUDObject oObj = oLoadObject(sObject, _oConfigFile);

						oGroup.iAddObject(oObj);
					}

				// Pop depth
				m_rDepth     = rOldDepth;
				m_rDepthFact = rOldDepthFact;
				
				m_iObjectIdx = iOldObjIdx;

				return oGroup;
			}
			
			return null;
		}

		//---------------------------------------------------------------------
		static float rGetObjectDepth(float _rObjDepth)
		{
			return m_rDepth + (m_rDepthFact * _rObjDepth);
		}
		//---------------------------------------------------------------------
		private static int iGetRenderIndex(CFEHUDObject _oObj)
		{
			float rDepth = rGetObjectDepth(_oObj.m_rIniDepth);
			
			int iDepthFact = (int)(rDepth * 10000.0f);
			return iDepthFact + m_iObjectIdx;
		}
		//-----------------------------------------------------------------------------
		private static EFETextHAlignmentMode eGetHAlignFromString(CFEString _sAlign)
		{
			if (_sAlign == "left")
				return(EFETextHAlignmentMode.THAM_LEFT);

		else if (_sAlign == "center")
			return(EFETextHAlignmentMode.THAM_CENTER);

		else if (_sAlign == "right")
			return(EFETextHAlignmentMode.THAM_RIGHT);

			return(EFETextHAlignmentMode.THAM_LEFT);
		}
		//-----------------------------------------------------------------------------
		private static EFETextVAlignmentMode eGetVAlignFromString(CFEString _sAlign)
		{
			if (_sAlign == "top")
				return(EFETextVAlignmentMode.TVAM_TOP);

		else if (_sAlign == "center")
			return(EFETextVAlignmentMode.TVAM_CENTER);

		else if (_sAlign == "bottom")
			return(EFETextVAlignmentMode.TVAM_BOTTOM);

			return(EFETextVAlignmentMode.TVAM_CENTER);
		}
		//---------------------------------------------------------------------
	}
};