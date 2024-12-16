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
	public static class CFEHUDLoader
	{
		// --------------------------------------------------------------------
		private static string	m_sWorkingDir;
		private static int	m_uiFileVersion;

		// Accumulated depth
		private static float m_rDepth = 0.0f;


		// Current depth factor
		private static float m_rDepthFact = 1.0f;

		// creation counter to make object render properly.
		private static int m_iObjectIdx = 0;
		// --------------------------------------------------------------------
		private static void Reset()
		{
			m_rDepthFact = 1.0f;
			m_iObjectIdx = 0;
		}
		// --------------------------------------------------------------------
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
		// --------------------------------------------------------------------
		/// Loads a HUD element from disk
		public static CFEHUDElement oLoadElement(CFEString _sFilename)
		{
			string sFilename = _sFilename + ".hel";
			m_sWorkingDir = CFEStringUtils.sGetPath(_sFilename);
			
			CFEConfigFile oConfig = new CFEConfigFile(sFilename);
			if (! oConfig.bInitialized() ) return(null);

			return oLoadElement("HUDElement", oConfig);
		}
		// --------------------------------------------------------------------
		/// Loads a HUD element actions from disk
		public static void LoadElementActions(CFEString _sFilename, ref CFEHUDElement _oElem)
		{
			CFEString sFilename = _sFilename + ".hea";
			m_sWorkingDir = CFEStringUtils.sGetPath(_sFilename);

			CFEConfigFile oConfig = new CFEConfigFile(sFilename);
			if (! oConfig.bInitialized() ) return;

			LoadElementActions("HUDElementActions",oConfig, ref _oElem);
		}
		// --------------------------------------------------------------------
		/// Loads a HUD object from disk
		public static CFEHUDObject oLoadObject(string _sFilename)
		{
			string sFilename = _sFilename + ".hob";
			m_sWorkingDir = CFEStringUtils.sGetPath(_sFilename);
			
			CFEConfigFile oConfig = new CFEConfigFile(sFilename);
			if (! oConfig.bInitialized() ) return(null);

			return oLoadObject("HUDObject", oConfig);
		}
		// --------------------------------------------------------------------	
		private static CFEHUDElement oLoadElement(CFEString _sPrefix, CFEConfigFile _oConfigFile)
		{
			// Element name
			CFEString sElemName = _oConfigFile.sGetString(_sPrefix + ".Name","nonamed-element");

			CFEHUDElement oElem = new CFEHUDElement(); // Support.CreateObject<CFEHUDElement>(FuetEngine.Support.HUD_ELEMENT_SCRIPT_FILE);
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
			LoadElementActions(_sPrefix, _oConfigFile, ref oElem);
			
			return oElem;
		}
		// --------------------------------------------------------------------
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
		// --------------------------------------------------------------------
		private static CFEHUDObject oLoadObject(string _sPrefix, CFEConfigFile _oConfigFile)
		{
			// Creation factor allows objects to be rendered properly when same depth (prevent z fighting)
			// This makes the rendering depend on creation order.
			m_iObjectIdx--;

			string sElemType = _oConfigFile.sGetString(_sPrefix + ".Type","none");
			string sElemName = _oConfigFile.sGetString(_sPrefix + ".Name","nonamed");

			if (sElemType == "Label")
			{
				CFEHUDLabel oLabel = new CFEHUDLabel(); // Support.CreateObject<CFEHUDLabel>(FuetEngine.Support.HUD_LABEL_SCRIPT_FILE);
				LoadCommonObjectProperties(_sPrefix,_oConfigFile, oLabel);
				
				string sText = _oConfigFile.sGetString(_sPrefix + ".Text","Label");
				oLabel.SetText(sText);
				
				string sFont = _oConfigFile.sGetString(_sPrefix + ".Font","");
				if (sFont != "")
				{
					string sFontFile = m_sWorkingDir + "/" + sFont;
					CFEFont font = CFEFontMgr.Instance.Load(sFontFile);
					oLabel.SetFont(font);
				}
				
				string sHAlign = _oConfigFile.sGetString(_sPrefix + ".HAlignment","left");
				oLabel.SetHAlignment( FuetEngine.Support.eGetHAlignFromString(sHAlign) );
				
				string sVAlign = _oConfigFile.sGetString(_sPrefix + ".VAlignment","center");
				oLabel.SetVAlignment( FuetEngine.Support.eGetVAlignFromString(sVAlign) );
				
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
				CFEHUDIcon oIcon = new CFEHUDIcon(); // (FuetEngine.Support.HUD_ICON_SCRIPT_FILE);
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
				CFEHUDRect oRect = new CFEHUDRect(); // (FuetEngine.Support.HUD_RECT_SCRIPT_FILE);
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
				CFEHUDShape oShape = new CFEHUDShape(); // (FuetEngine.Support.HUD_SHAPE_SCRIPT_FILE);
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
				CFEHUDPSys oPSys = new CFEHUDPSys(); // (FuetEngine.Support.HUD_PSYS_SCRIPT_FILE);
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
				 
				CFEHUDGroup oGroup = new CFEHUDGroup(); // (FuetEngine.Support.HUD_GROUP_SCRIPT_FILE);

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
		//-----------------------------------------------------------------------------
		private static void LoadElementActions(CFEString _sPrefix, CFEConfigFile _oConfigFile, ref CFEHUDElement _oElem)
		{
			int iNumActions = _oConfigFile.iGetInteger(_sPrefix + ".NumElemActions", 0);
			
			CFEHUDElementAction	resetAction = CreateResetAnimation(_sPrefix, _oConfigFile,ref _oElem);
			_oElem.iAddAction(resetAction);

			for (int i=0; i<iNumActions; i++)
			{
				CFEString sPrefix = _sPrefix + ".Action" + i.ToString();
				CFEHUDElementAction oObj = oLoadAction(sPrefix, _oConfigFile, ref _oElem);

				_oElem.iAddAction(oObj);
			}
		}
		//-----------------------------------------------------------------------------
		private static CFEHUDElementAction oLoadAction(CFEString _sPrefix, CFEConfigFile _oConfigFile, ref CFEHUDElement _oElem)
		{
			CFEString sName = _oConfigFile.sGetString(_sPrefix + ".Name","noname action elem");

			CFEHUDElementAction elementAction = new CFEHUDElementAction();
			elementAction.ResourceName = sName;
			elementAction.ResourceLocalToScene = true;

			int iNumObjActions = _oConfigFile.iGetInteger(_sPrefix + ".NumObjActions", 0);
			for (int i=0; i<iNumObjActions; i++)
			{
				CFEString sPrefix = _sPrefix +".ObjAction" + i.ToString();
				LoadObjAction(sPrefix, _oConfigFile, ref _oElem, ref elementAction);
			}

			elementAction.Length = CFEHUDActionTime.rGetMaxActionTime(elementAction); 			
			return elementAction;
		}
		
		//-----------------------------------------------------------------------------
		public static void LoadObjAction(CFEString _sPrefix, CFEConfigFile _oConfigFile, ref CFEHUDElement _oElem, ref Animation objAction)
		{
			CFEString sHUDObject = _oConfigFile.sGetString(_sPrefix + ".HUDObject", "");    
			if (sHUDObject == "")
			{
				return;
			}

			// find hud object
			CFEHUDElemLocator elemLocator = new CFEHUDElemLocator();
			CFEHUDObject oHUDObject = elemLocator.oLocateHUDObject(_oElem, sHUDObject);
			NodePath nodePath = _oElem.GetPathTo(oHUDObject);
			string sHUDObjectPath = nodePath;

			if (oHUDObject == null)
			{
				GD.Print("Unable to find HUDObject: " + sHUDObject);
				return;
			}

			/// To keep compatibility loading previous HUD files.
			if (m_uiFileVersion > 1)
			{
				Support.ReadKFBFunc<float>(_sPrefix + ".XFunc", "Value", sHUDObjectPath + ":position:x", 0.0f, oHUDObject.m_oIniPos.x, Support.InitialValueOperation.Sum, _oConfigFile, ref objAction);
				Support.ReadKFBFunc<float>(_sPrefix + ".YFunc", "Value", sHUDObjectPath + ":position:y", 0.0f, oHUDObject.m_oIniPos.y, Support.InitialValueOperation.Sum, _oConfigFile, ref objAction);
			}
			else
			{
				Support.ReadKFBFunc(_sPrefix + ".PosFunc", "", sHUDObjectPath + ":position", CFEVect2.Zero, oHUDObject.m_oIniPos, Support.InitialValueOperation.Sum, _oConfigFile, ref objAction);				
			}

			// Scale
			Support.ReadKFBFunc(_sPrefix + ".ScaleFunc", "", sHUDObjectPath + ":scale", CFEVect2.One, oHUDObject.m_oIniScale, Support.InitialValueOperation.Mult, _oConfigFile, ref objAction);	

			// Angle
			Support.ReadKFBFunc<float>(_sPrefix + ".AngleFunc", "Value", sHUDObjectPath + ":rotation_degrees", 0.0f, oHUDObject.m_rIniAngle, Support.InitialValueOperation.Sum, _oConfigFile, ref objAction);

			// Depth

			// Color
			Support.ReadKFBFunc(_sPrefix + ".ColorFunc", "", sHUDObjectPath + ":modulate", new Color(1,1,1,1), oHUDObject.m_oIniColor, Support.InitialValueOperation.Mult, _oConfigFile, ref objAction);

			// Visibility
			Support.ReadKFBFunc<bool>(_sPrefix + ".VisFunc", "Value", sHUDObjectPath + ":visible", true, oHUDObject.m_bIniVis, Support.InitialValueOperation.None, _oConfigFile, ref objAction);

			// Actions
			Support.ReadKFBFunc<int>(_sPrefix + ".ActionFunc", "Value", sHUDObjectPath + ":CFESpriteInstance:Action", -1, oHUDObject.m_iIniAction, Support.InitialValueOperation.None, _oConfigFile, ref objAction);

			/*
			// Events
			sVar = _sPrefix + ".EventFunc";
			if (_oConfigFile.bExists(sVar + ".WrapMode"))
				poObjAction->m_oEventFunc.SetWrapMode( CFEKFBFuncUtils::eGetWrapMode(_oConfigFile.sGetString(sVar + ".WrapMode",DEFAULT_WRAP_MODE)) );
			else
				poObjAction->m_oEventFunc.SetWrapMode( KFBFWM_FINALVALUE );

			uiKeyFrames = _oConfigFile.iGetInteger(sVar + ".NumKeyFrames",1);
			if (uiKeyFrames == 0) uiKeyFrames = 1;	// will use default values
			
			sVar += ".KeyFrame";
			for (i=0;i<uiKeyFrames;i++)
			{
				CFEString sIVar = sVar + CFEString((int)i);
				FEReal rTime = _oConfigFile.rGetReal(sIVar + ".Time",_0r);

				CFEEvent oEvent;
				oEvent.m_sEventName  = _oConfigFile.sGetString(sIVar + ".EventName", CFEString::sNULL());
				oEvent.m_sEventValue = _oConfigFile.sGetString(sIVar + ".EventValue",CFEString::sNULL());
				// oEvent.SetEventTime(rTime);

				poObjAction->m_oEventFunc.InsertKeyFrame(oEvent,rTime,KFLF_NONE);
			}
			*/
		}
		// --------------------------------------------------------------------
		private static CFEHUDElementAction CreateResetAnimation(CFEString _sPrefix, CFEConfigFile _oConfigFile, ref CFEHUDElement _oElem)
		{
			GD.Print("Create Reset action for element "+_oElem.Name);

			CFEHUDElementAction elementAction = new CFEHUDElementAction();
			elementAction.ResourceName = "Reset";
			elementAction.ResourceLocalToScene = true;
		
			int iNumElemActions = _oConfigFile.iGetInteger(_sPrefix + ".NumElemActions", 0);
			for (int j=0; j<iNumElemActions; j++)
			{
				CFEString sElemActionPrefix = _sPrefix + ".Action" + j.ToString();

				int iNumObjActions = _oConfigFile.iGetInteger(sElemActionPrefix + ".NumObjActions", 0);
				for (int i=0; i<iNumObjActions; i++)
				{
					CFEString sObjActionPrefix = sElemActionPrefix +".ObjAction" + i.ToString();
					CreateObjResetAnimation(sObjActionPrefix, _oConfigFile, ref _oElem, ref elementAction);
				}
			}

			elementAction.Length = 0.01f;
			return elementAction;
		}
		// --------------------------------------------------------------------
		private static void CreateObjResetAnimation(CFEString _sPrefix, CFEConfigFile _oConfigFile, ref CFEHUDElement _oElem, ref CFEHUDElementAction _elementAction)
		{
			CFEString sHUDObject = _oConfigFile.sGetString(_sPrefix + ".HUDObject", "");    
			if (sHUDObject == "")
			{
				return;
			}

			// find hud object
			CFEHUDElemLocator elemLocator = new CFEHUDElemLocator();
			CFEHUDObject oHUDObject = elemLocator.oLocateHUDObject(_oElem, sHUDObject);
			NodePath nodePath = _oElem.GetPathTo(oHUDObject);
			string sHUDObjectPath = nodePath;

			if (oHUDObject == null)
			{
				GD.Print("Unable to find HUDObject: " + sHUDObject);
				return;
			}
			
			if (m_uiFileVersion > 1)
			{
				Support.CreateResetAnimation<float>(_sPrefix + ".XFunc", sHUDObjectPath + ":position:x", oHUDObject.m_oIniPos.x, _oConfigFile, ref _elementAction);
				Support.CreateResetAnimation<float>(_sPrefix + ".YFunc", sHUDObjectPath + ":position:y", oHUDObject.m_oIniPos.y, _oConfigFile, ref _elementAction);
			}
			else
			{
				Support.CreateResetAnimation<CFEVect2>(_sPrefix + ".PosFunc", sHUDObjectPath + ":position", oHUDObject.m_oIniPos, _oConfigFile, ref _elementAction);				
			}

			// Scale
			Support.CreateResetAnimation<CFEVect2>(_sPrefix + ".ScaleFunc", sHUDObjectPath + ":scale", oHUDObject.m_oIniScale, _oConfigFile, ref _elementAction);				

			// Angle
			Support.CreateResetAnimation<float>(_sPrefix + ".AngleFunc", sHUDObjectPath + ":rotation_degrees", oHUDObject.m_rIniAngle, _oConfigFile, ref _elementAction);

			// Depth

			// Color
			Support.CreateResetAnimation<CFEColor>(_sPrefix + ".ColorFunc", sHUDObjectPath + ":modulate", oHUDObject.m_oIniColor, _oConfigFile, ref _elementAction);				

			// Visibility
			Support.CreateResetAnimation<bool>(_sPrefix + ".VisFunc", sHUDObjectPath + ":visible", oHUDObject.m_bIniVis, _oConfigFile, ref _elementAction);

			// Actions
			Support.CreateResetAnimation<int>(_sPrefix + ".ActionFunc", sHUDObjectPath + ":Action", oHUDObject.m_iIniAction, _oConfigFile, ref _elementAction);
		}
		// --------------------------------------------------------------------
		private static float rGetObjectDepth(float _rObjDepth)
		{
			return m_rDepth + (m_rDepthFact * _rObjDepth);
		}
		// --------------------------------------------------------------------
		private static int iGetRenderIndex(CFEHUDObject _oObj)
		{
			float rDepth = rGetObjectDepth(_oObj.m_rIniDepth);
			
			int iDepthFact = (int)(rDepth * 10000.0f);
			return iDepthFact + m_iObjectIdx;
		}
		// --------------------------------------------------------------------
	}
};
