using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CFEVect2 = Godot.Vector2;

namespace FuetEngine
{
    public class CFESpriteLoader
    {
        // ----------------------------------------------------------------------------
        /*
        public static CFESprite poBuildBasicSprite(string _hMat,string _sSpriteName)
        {
            CFESprite poSprite = new CFESprite();
            poSprite.SetName(_sSpriteName);

            CFESpriteAction oAction = new CFESpriteAction("");
            oAction.m_ePlayMode     = ESFSPlayMode.SFSPM_ONESHOT;
	        oAction.m_rActionTime   = 0.0f;
	        oAction.m_rRandStartTime = 0.0f;
            oAction.m_eBlendMode    = EFEBlendMode.BM_ALPHA;

            CFESpriteFrame oFrame   = new CFESpriteFrame();
            oFrame.m_oPivot     = new CFEVect2(0.0f, 0.0f);
            oFrame.m_oUV.m_oIni = new CFEVect2(0.0f, 0.0f);
            oFrame.m_oUV.m_oEnd = new CFEVect2(1.0f, 1.0f);
            oFrame.m_rBlendTime = 0.0f;
            oFrame.m_rDelay     = 0.0f;
            oFrame.m_rStartTime = 0.0f;
            oFrame.m_rFrameTime = 0.0f;
	        oFrame.m_oPivot.x	= 0.5f;
	        oFrame.m_oPivot.y	= 0.5f;
	        oFrame.m_bScaleXUEqually = false;
	        oFrame.m_bScaleYVEqually = false;
	        oFrame.m_bUWorldCoords = false;
	        oFrame.m_bVWorldCoords = false;

			int uiTX = 0; 
            int uiTY = 0; 
            if ((_hMat == null) && (_hMat.mainTexture == null))
            {
                uiTX = _hMat.mainTexture.width;
                uiTY = _hMat.mainTexture.height;
                // CFEMaterialMgr::I()->bGetMaterialProperty(_hMat, CFEString("DiffuseMap.Width") ,(FEPointer)&uiTX);
                // CFEMaterialMgr::I()->bGetMaterialProperty(_hMat, CFEString("DiffuseMap.Height"),(FEPointer)&uiTY);
            }

            oFrame.m_oSize.x    = uiTX;
            oFrame.m_oSize.y    = uiTY;
            oFrame.m_hMaterial  = _hMat;
            // oFrame.GenerateSpriteGeometry(CFEVect2.zero,CFEVect2.one);

            oAction.m_oSeq.Add(oFrame);
            poSprite.m_oActions.Add(oAction);

            return( poSprite );
        }
        */

        /// Loads an sprite from a given file
        public static CFESprite poLoad(string _sFilename,bool _bLoadMaterials)
        {
            if (_sFilename == "") return (null);

            string sFilename   = _sFilename + ".spr";
            string sWorkingDir = CFEStringUtils.sGetPath(_sFilename);

            CFEConfigFile oConfig = new CFEConfigFile(sFilename);
            if (!oConfig.bInitialized())
            {
                if (_bLoadMaterials == false) return (null);

                // We haven't found the sprite definition file. Let's see i we can find
                // a material and then build a basic sprite with it.

                /// Retrieves the filename portion of a full qualified filename.
                string sMaterial = sWorkingDir + CFEStringUtils.sGetFilename(_sFilename);
                /*
                TODO: Material hMat = (_bLoadMaterials) ? CFEMaterialMgr.poLoad(sMaterial) : null;

                if (hMat != null)
                    return ( poBuildBasicSprite(hMat, sMaterial) );
                else
                */
                    return (null);
            }

            CFESprite poSprite = new CFESprite();

            // Retrieve sprite name
            poSprite.SetName(oConfig.sGetString("Sprite.Name", "nonamed"));

            // Number of actions
            int uiNumActions = oConfig.iGetInteger("Sprite.NumActions", 0);

            for (int a = 0; a < uiNumActions; a++)
            {
                CFESpriteAction oAction = new CFESpriteAction("");
                oAction.m_rActionTime = 0.0f;

                string sAction = "Sprite.Action" + a;
                string sVar;

                // Action name
                sVar = sAction + ".Name";
                oAction.SetName(oConfig.sGetString(sVar, "nonamed"));

                // Action blend mode
                sVar = sAction + ".BlendMode";
                oAction.m_eBlendMode = eGetBlendMode(oConfig.sGetString(sVar, "ALPHA"));

                // Action play mode
                sVar = sAction + ".PlayMode";
                oAction.m_ePlayMode = eGetPlayMode(oConfig.sGetString(sVar, "ONESHOT"));

                sVar = sAction + ".RandomStartTime";
                oAction.m_rRandStartTime = oConfig.rGetReal(sVar, 0.0f);

                sVar = sAction + ".Frames";

                string sFVar = sVar + ".Type";

                string sType = oConfig.sGetString(sFVar, "Regular");

                if (sType == "Regular")
                {
                    sFVar = sVar + ".Regular";
                    string sFRVar;

                    sFRVar = sFVar + ".XFrames";
                    int uiXFrames = oConfig.iGetInteger(sFRVar, 1);

                    sFRVar = sFVar + ".YFrames";
                    int uiYFrames = oConfig.iGetInteger(sFRVar, 1);

                    sFRVar = sFVar + ".Material";
                    string sMaterial = sWorkingDir + oConfig.sGetString(sFRVar, "");
					// TODO: Material hMat = (_bLoadMaterials) ? CFEMaterialMgr.poLoad(sMaterial) : null;
                    string hMat = sMaterial;
                        
                    // Filter
                    sFRVar = sFVar + ".Filter";
                    string sFilter = oConfig.sGetString(sFRVar, "linear");
                    // TODO: CFEMaterialMgr.bSetMaterialProperty(hMat, "DiffuseMap.Filter", sFilter);

                    sFRVar = sFVar + ".FPS";
                    float rFPS = oConfig.rGetReal(sFRVar, 1.0f);

                    sFRVar = sFVar + ".Pivot.x";
                    float rPivotX = oConfig.rGetReal(sFRVar, 0.5f);

                    sFRVar = sFVar + ".Pivot.y";
                    float rPivotY = oConfig.rGetReal(sFRVar, 0.5f);

                    sFRVar = sFVar + ".Size.Width";
                    float rFWidth = oConfig.rGetReal(sFRVar, 1.0f);

                    sFRVar = sFVar + ".Size.Height";
                    float rFHeight = oConfig.rGetReal(sFRVar, 1.0f);

                    float rXStep = 1.0f / (float)uiXFrames;
                    float rYStep = 1.0f / (float)uiYFrames;

                    for (uint j = 0; j < uiYFrames; j++)
                    {
                        for (uint i = 0; i < uiXFrames; i++)
                        {
                            // Create the frame sequence
                            CFESpriteFrame oFrame = new CFESpriteFrame();

                            oFrame.m_oPivot     = new CFEVect2(rPivotX, rPivotY);
                            oFrame.m_oUV.m_oIni = new CFEVect2((float)i * rXStep, (float)j * rYStep);
                            oFrame.m_oUV.m_oEnd = new CFEVect2((float)(i + 1) * rXStep, (float)(j + 1) * rYStep);
                            oFrame.m_hMaterial  = hMat;
                            oFrame.m_o1OverDims = new CFEVect2(0, 0);
                            oFrame.m_oSize.x    = rFWidth;
                            oFrame.m_oSize.y    = rFHeight;
                            oFrame.m_rDelay     = 0.0f;
                            oFrame.m_rBlendTime = 1.0f / rFPS;
                            oFrame.m_rFrameTime = oFrame.m_rDelay + oFrame.m_rBlendTime;
                            oFrame.m_rStartTime = oAction.m_rActionTime;
                            oFrame.m_bScaleXUEqually = false;
                            oFrame.m_bScaleYVEqually = false;
                            oFrame.m_bUWorldCoords = false;
                            oFrame.m_bVWorldCoords = false;
							// TODO: oFrame.GenerateSpriteGeometry(CFEVect2.zero,CFEVect2.one);

							oAction.m_rActionTime += oFrame.m_rFrameTime;
                            oAction.m_oSeq.Add( oFrame );							
                        }
                    }
                }

                else if (sType == "Sequence")
                {
                    sFVar = sVar + ".Sequence";
                    string sFRVar;

                    // Number of frames in the sequence
                    sFRVar = sFVar + ".Frames";
                    int uiFrames = oConfig.iGetInteger(sFRVar, 1);

                    // Material filename root
                    sFRVar = sFVar + ".Material";
                    string sMaterial = sWorkingDir + oConfig.sGetString(sFRVar, "");

                    // Pivot
                    sFRVar = sFVar + ".Pivot.x";
                    float rPivotX = oConfig.rGetReal(sFRVar, 0.5f);

                    sFRVar = sFVar + ".Pivot.y";
                    float rPivotY = oConfig.rGetReal(sFRVar, 0.5f);

                    for (uint i = 0; i < uiFrames; i++)
                    {
                        // Create the frame sequence
                        CFESpriteFrame oFrame = new CFESpriteFrame();

                        string sFrameTex = sMaterial + i;

                        oFrame.m_oPivot     = new CFEVect2(rPivotX, rPivotY);
                        oFrame.m_oUV.m_oIni = new CFEVect2(0, 0);
                        oFrame.m_oUV.m_oEnd = new CFEVect2(1, 1);
						oFrame.m_hMaterial  = sMaterial; // TODO: (_bLoadMaterials) ? CFEMaterialMgr.poLoad(sMaterial) : null;
                        oFrame.m_o1OverDims = new CFEVect2(0, 0);
                        oFrame.m_rStartTime = oAction.m_rActionTime;
                        oFrame.m_bScaleXUEqually = false;
                        oFrame.m_bScaleYVEqually = false;
                        oFrame.m_bUWorldCoords = false;
                        oFrame.m_bVWorldCoords = false;
                        // TODO: oFrame.GenerateSpriteGeometry(CFEVect2.zero,CFEVect2.one);

                        oAction.m_rActionTime += oFrame.m_rFrameTime;
                        oAction.m_oSeq.Add(oFrame);
                    }
                }

                else if (sType == "Complete")
                {
                    // Read every frame one by one
                    sFVar = sVar + ".Complete";
                    string sFCVar;

                    // Number of frames in the sequence
                    sFCVar = sFVar + ".NumFrames";
                    int uiFrames = oConfig.iGetInteger(sFCVar, 1);

                    // Create the frame sequence
                    for (uint i = 0; i < uiFrames; i++)
                    {
                        CFESpriteFrame oFrame = new CFESpriteFrame();

                        sFCVar = sFVar + ".Frame" + i;
                        string sFrameVar;

                        // Material filename
                        sFrameVar = sFCVar + ".Material";
                        string sMaterial = sWorkingDir + oConfig.sGetString(sFrameVar, "");
						// TODO: Material hMat = (_bLoadMaterials) ? CFEMaterialMgr.poLoad(sMaterial) : null;
                        string hMat = sMaterial;

                        // Wrap mode
                        sFrameVar = sFCVar + ".SWrapMode";
                        string sSWrapMode = oConfig.sGetString(sFrameVar, "clamp");
						// TODO: CFEMaterialMgr.bSetMaterialProperty(hMat, "DiffuseMap.SWrapMode", sSWrapMode);

                        sFrameVar = sFCVar + ".TWrapMode";
                        string sTWrapMode = oConfig.sGetString(sFrameVar, "clamp");
						// TODO: CFEMaterialMgr.bSetMaterialProperty(hMat, "DiffuseMap.TWrapMode", sTWrapMode);

                        // Filter
                        sFrameVar = sFCVar + ".Filter";
                        string sFilter = oConfig.sGetString(sFrameVar, "linear");
						// TODO: CFEMaterialMgr.bSetMaterialProperty(hMat, "DiffuseMap.Filter", sFilter);

                        // Read delay time
                        sFrameVar = sFCVar + ".DelayTime";
                        float rDelayTime = oConfig.rGetReal(sFrameVar, 0.0f);

                        // Read blend time
                        sFrameVar = sFCVar + ".BlendTime";
                        float rBlendTime = oConfig.rGetReal(sFrameVar, 0.0f);

                        // Pivot
                        sFrameVar = sFCVar + ".Pivot.x";
                        float rPivotX = oConfig.rGetReal(sFrameVar, 0.5f);

                        sFrameVar = sFCVar + ".Pivot.y";
                        float rPivotY = oConfig.rGetReal(sFrameVar, 0.5f);

                        // Size
                        sFrameVar = sFCVar + ".Size.Width";
                        float rFWidth = oConfig.rGetReal(sFrameVar, 1.0f);

                        sFrameVar = sFCVar + ".Size.Height";
                        float rFHeight = oConfig.rGetReal(sFrameVar, 1.0f);

                        // Tex coords
                        sFrameVar = sFCVar + ".TexCoords.IU";
                        float rIU = oConfig.rGetReal(sFrameVar, 0.0f);

                        sFrameVar = sFCVar + ".TexCoords.IV";
                        float rIV = oConfig.rGetReal(sFrameVar, 0.0f);

                        sFrameVar = sFCVar + ".TexCoords.FU";
                        float rFU = oConfig.rGetReal(sFrameVar, 1.0f);

                        sFrameVar = sFCVar + ".TexCoords.FV";
                        float rFV = oConfig.rGetReal(sFrameVar, 1.0f);

                        sFrameVar = sFCVar + ".TexCoords.UWorldCoords";
                        bool bUWorldCoords = oConfig.bGetBool(sFrameVar, false);

                        sFrameVar = sFCVar + ".TexCoords.VWorldCoords";
                        bool bVWorldCoords = oConfig.bGetBool(sFrameVar, false);

                        sFrameVar = sFCVar + ".TexCoords.ScaleXUEqually";
                        bool bScaleXUEqually = oConfig.bGetBool(sFrameVar, false);

                        sFrameVar = sFCVar + ".TexCoords.ScaleYVEqually";
                        bool bScaleYVEqually = oConfig.bGetBool(sFrameVar, false);

                        // TODO: 
                        float r1OverW = 0.0f;
                        float r1OverH = 0.0f;

                        /*
                        if (bUWorldCoords && (hMat != null) && (hMat.mainTexture != null))
                        {
                            int uiTX = hMat.mainTexture.width;
                            // CFEMaterialMgr::I()->bGetMaterialProperty(hMat, string("DiffuseMap.Width"), (FEPointer) & uiTX);
                            if (uiTX == 0) uiTX = 1;
                            r1OverW = 1.0f / (float)uiTX;
                        }

						if (bVWorldCoords && (hMat != null)&& (hMat.mainTexture != null))
                        {
                            int uiTY = hMat.mainTexture.height;
                            // CFEMaterialMgr::I()->bGetMaterialProperty(hMat, string("DiffuseMap.Height"), (FEPointer) & uiTY);
                            if (uiTY == 0) uiTY = 1;
                            r1OverH = 1.0f / (float)uiTY;
                        }
                        */

                        oFrame.m_hMaterial  = hMat;
                        oFrame.m_o1OverDims = new CFEVect2(r1OverW, r1OverH);
                        oFrame.m_oPivot     = new CFEVect2(rPivotX, rPivotY);
                        oFrame.m_oUV.m_oIni = new CFEVect2(rIU, rIV);
                        oFrame.m_oUV.m_oEnd = new CFEVect2(rFU, rFV);
                        oFrame.m_oSize      = new CFEVect2(rFWidth, rFHeight);
                        oFrame.m_rDelay     = rDelayTime;
                        oFrame.m_rBlendTime = rBlendTime;
                        oFrame.m_rFrameTime = rBlendTime + rDelayTime;
                        oFrame.m_rStartTime = oAction.m_rActionTime;
                        oFrame.m_bScaleXUEqually = bScaleXUEqually;
                        oFrame.m_bScaleYVEqually = bScaleYVEqually;
                        oFrame.m_bUWorldCoords = bUWorldCoords;
                        oFrame.m_bVWorldCoords = bVWorldCoords;
						// oFrame.GenerateSprteGeometry(CFEVect2.zero,CFEVect2.one);

                        oAction.m_rActionTime += oFrame.m_rFrameTime;
                        oAction.m_oSeq.Add(oFrame);
                    }
                }

                poSprite.m_oActions.Add(oAction);
            }

            return (poSprite);
        }

        // ----------------------------------------------------------------------------
        // ----------------------------------------------------------------------------
        // Utility functions
        // ----------------------------------------------------------------------------
        // ----------------------------------------------------------------------------
        static protected ESFSPlayMode eGetPlayMode(string _sString)
        {
            if (_sString == "ONESHOT")
                return(ESFSPlayMode.SFSPM_ONESHOT);

            else if (_sString == "PINGPONGSTOP")
                return(ESFSPlayMode.SFSPM_PINGPONGSTOP);

            else if (_sString == "LOOP")
                return(ESFSPlayMode.SFSPM_LOOP);

            else if (_sString == "PINGPONG")
                return(ESFSPlayMode.SFSPM_PINGPONG);

            return(ESFSPlayMode.SFSPM_NONE);
        }
        // ----------------------------------------------------------------------------
        static protected EFEBlendMode eGetBlendMode(string _sString)
        {
            if (_sString == "ALPHA")
                return (EFEBlendMode.BM_ALPHA);

            else if (_sString == "ALPHAADD")
                return (EFEBlendMode.BM_ALPHAADD);

            else if (_sString == "ADD")
                return (EFEBlendMode.BM_ADD);

            else if (_sString == "MULT")
                return (EFEBlendMode.BM_MULT);

            else if (_sString == "COPY")
                return (EFEBlendMode.BM_COPY);

            else if (_sString == "FOG")
                return (EFEBlendMode.BM_FOG);

            else if (_sString == "FOGADD")
                return (EFEBlendMode.BM_FOGADD);

            else if (_sString == "MAGICMARKER")
                return (EFEBlendMode.BM_MAGICMARKER);

            else if (_sString == "LIGHTMARKER")
                return (EFEBlendMode.BM_LIGHTMARKER);

            else if (_sString == "LIGHTSABER")
                return (EFEBlendMode.BM_LIGHTSABER);

            else if (_sString == "REVEAL")
                return (EFEBlendMode.BM_REVEAL);

            else if (_sString == "LUMISHADE_REVEAL")
                return (EFEBlendMode.BM_LUMISHADE_REVEAL);

            return (EFEBlendMode.BM_NONE);
        }
        // ----------------------------------------------------------------------------
        static protected uint uiGetWrapMode(string _sString)
        {
            if (_sString == "REPEAT")
                return (1);

            else if (_sString == "MIRROR")
                return (2);

            return (0);
        }
        // ----------------------------------------------------------------------------
        static protected uint uiGetFilter(string _sString)
        {
            if (_sString == "NEAREST")
                return (0);

            else if (_sString == "LINEAR")
                return (1);

            return (1);
        }
        // ----------------------------------------------------------------------------
    }
}
