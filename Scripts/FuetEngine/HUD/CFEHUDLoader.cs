using Godot;
//-----------------------------------------------------------------------------
using CFEString = System.String;
using FEReal = System.Single;
using CFEVect2 = Godot.Vector2;
using CFEColor = Godot.Color;
//-----------------------------------------------------------------------------
namespace FuetEngine
{
    public class CFEHUDLoader
    {
        // ------------------------------------------------------------------------------
        static void Reset()
        {
            m_oModColor = new CFEColor(1.0f, 1.0f, 1.0f, 1.0f);

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
		protected static CFEHUDElement oLoadElement(CFEString _sPrefix, CFEConfigFile _oConfigFile)
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
		protected static void LoadElementActions(CFEString _sPrefix, CFEConfigFile _oConfigFile, CFEHUDElement _poElem)
		{
		}
		// ------------------------------------------------------------------------------
		protected static CFEHUDElementAction oLoadAction(CFEString _sPrefix, CFEConfigFile _oConfigFile, CFEHUDElement _poElem)
		{
			return null;
		}
		// ------------------------------------------------------------------------------
		protected static CFEHUDObjectAction oLoadObjAction(CFEString _sPrefix, CFEConfigFile _oConfigFile, CFEHUDElement _poElem)
		{
			return null;
		}
        // ------------------------------------------------------------------------------
        protected static void LoadCommonObjectProperties(CFEString _sPrefix, CFEConfigFile _oConfigFile, CFEHUDObject _oObj)
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
        }
		// ------------------------------------------------------------------------------
		protected static CFEHUDObject oLoadObject(string _sPrefix, CFEConfigFile _oConfigFile)
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
				// poLabel->SetText( sText );
				
				string sFont = _oConfigFile.sGetString(_sPrefix + ".Font","");
				if (sFont != "")
				{
					string sFontFile = m_sWorkingDir + "/" + sFont;
					// CFEFont poFont = CFEFontMgr::I()->poLoad( sFontFile );
					// poLabel->SetFont( poFont );
				}
				
				string sHAlign = _oConfigFile.sGetString(_sPrefix + ".HAlignment","left");
				// poLabel->SetHAlignment( eGetHAlignFromString(sHAlign) );
				
				string sVAlign = _oConfigFile.sGetString(_sPrefix + ".VAlignment","center");
				// poLabel->SetVAlignment( eGetVAlignFromString(sVAlign) );
				
				float rTracking, rInterlining, rPointSize,rAdjustmentWidth;
				rTracking	 = _oConfigFile.rGetReal(_sPrefix + ".Tracking",0.0f);
				rInterlining = _oConfigFile.rGetReal(_sPrefix + ".Interlining",0.0f);
				rPointSize   = _oConfigFile.rGetReal(_sPrefix + ".PointSize",1.0f);
				rAdjustmentWidth = _oConfigFile.rGetReal(_sPrefix + ".AdjustmentWidth",-1.0f);
				
				// poLabel->SetTracking( rTracking );
				// poLabel->SetInterlining( rInterlining );
				// poLabel->SetPointSize( rPointSize );
				// poLabel->SetAdjustmentWidth( rAdjustmentWidth );
				
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
					CFESpriteInstance spriteInstance = Support.CreateObject<Node2D,CFESpriteInstance>(FuetEngine.Support.SPRITE_INSTANCE_SCRIPT_FILE);

					spriteInstance.Name = "CFESpriteInstance";
					spriteInstance.Init(spriteResource);

					spriteInstance.SetAction(oIcon.iGetIniAction());
                    // spriteInstance.m_oColor            = oRenderColor(m_oModColor, poIcon);
					
					oIcon.SetIcon(spriteInstance);
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
                //poRect->SetWidth(rWidth);
                //poRect->SetHeight(rHeight);
                //poRect->SetPivot( CFEVect2(rPivotX,rPivotY) );
				
				/*
                // Load corner colors
                Texture2D tex = new Texture2D(2, 2);
                for (int i=0;i<4;i++)
				{
					string sCorner = ".Corner" + i;
					string sVar    = _sPrefix + sCorner;

					Color oColor;
					oColor.r = _oConfigFile.rGetReal(sVar + ".r",1.0f);
					oColor.g = _oConfigFile.rGetReal(sVar + ".g",1.0f);
					oColor.b = _oConfigFile.rGetReal(sVar + ".b",1.0f);
					oColor.a = _oConfigFile.rGetReal(sVar + ".a",1.0f);
                    
                    int x = i % 2;
                    int y = i/2;
                    tex.SetPixel(x, y, oColor);

                    // poRect->SetCornerColor(i,oColor);
                }
                tex.Apply();

                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 2, 2), new Vector2(rPivotX, rPivotY), 100.0f, 0, SpriteMeshType.FullRect);
                SpriteRenderer oSR = oGameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
                oSR.sprite = sprite;
                oSR.color  = oRenderColor(m_oModColor, poRect);
                oSR.sortingOrder = iGetRenderIndex(poRect);

                // *0.5f because the rect is 2x2 not 1x1
                oGameObject.transform.localScale = new Vector3(oGameObject.transform.localScale.x * rWidth * 0.5f, -1.0f * oGameObject.transform.localScale.y * rHeight * 0.5f, 1.0f);
				*/

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

                // Push current color & depth
                CFEColor oOldModColor = m_oModColor;  // NOTE: We can do that because Color is a struct, not a class
                float rOldDepthFact = m_rDepthFact;
                float rOldDepth     = m_rDepth;

                    m_oModColor = m_oModColor * oGroup.oGetColor();
                    // m_rDepth    = m_rDepth + m_rDepthFact * oGroup.m_rDepth;
                    m_rDepthFact *= rDepthFact;                                 // order important!!!!

                    for (int i=0;i<uiNumObjects;i++)
                    {
					    string sObject = _sPrefix + ".Object" + i;
					    CFEHUDObject oObj = oLoadObject(sObject,_oConfigFile);

                        oGroup.iAddObject(oObj);
				    }

                // Pop color & depth
                m_oModColor  = oOldModColor;
                m_rDepth     = rOldDepth;
                m_rDepthFact = rOldDepthFact;
                
                m_iObjectIdx = iOldObjIdx;

                return oGroup;
			}
			
			return null;
		}

		protected static string	m_sWorkingDir;
		protected static int	m_uiFileVersion;

        // Accumulated modulation color to render hud objects.
        protected static CFEColor m_oModColor = new CFEColor(1.0f, 1.0f, 1.0f, 1.0f);

        // Accumulated depth
        protected static float m_rDepth = 0.0f;

        // Current depth factor
        protected static float m_rDepthFact = 1.0f;

        // creation counter to make object render properly.
        protected static int m_iObjectIdx = 0;

        //-----------------------------------------------------------------------------
        /// Utiliy functions
        //-----------------------------------------------------------------------------
        static CFEColor oRenderColor(CFEColor _oModColor, CFEHUDObject _poObj)
        {
	        return( _oModColor* _poObj.oGetColor() );
        }
        //---------------------------------------------------------------------
		static CFEColor oGetRenderColor(CFEHUDObject _poObj)
        {
            return oRenderColor(m_oModColor, _poObj);
        }
		//---------------------------------------------------------------------
        static float rGetObjectDepth(float _rObjDepth)
        {
            return m_rDepth + (m_rDepthFact * _rObjDepth);
        }
        //---------------------------------------------------------------------
        static float rGetRenderDepth(float _rObjDepth)
        {
            return rGetObjectDepth(_rObjDepth);
		}
        //---------------------------------------------------------------------
		static int iGetRenderIndex(CFEHUDObject _oObj)
        {
            float rDepth = 0.0f; // rGetObjectDepth(_oObj.m_rDepth);
            
            int iDepthFact = (int)(rDepth * 10000.0f);
            return iDepthFact + m_iObjectIdx;
        }
        //---------------------------------------------------------------------
    }
};