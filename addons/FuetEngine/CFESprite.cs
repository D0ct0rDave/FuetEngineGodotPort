using System;
using System.Collections.Generic;
using CFEVect2 = Godot.Vector2;

namespace FuetEngine
{
    // ----------------------------------------------------------------------------
    public class CFESpriteFrame : Godot.Resource
    {
        /// Should we use the same scale of X for the U tex coord?
        public bool m_bScaleXUEqually;

        /// Should we use the same scale of Y for the V tex coord?
        public bool m_bScaleYVEqually;

        /// Should this frame use projected world coordinates instead of UV coords? (U component)
        public bool m_bUWorldCoords;

        /// Should this frame use projected world coordinates instead of UV coords? (V component)
        public bool m_bVWorldCoords;

        /// Rectangle inside the sprite material defining this sprite frame.
        public CFERect m_oUV = new CFERect();

        /// Size in pixels (of virtual screen dimensions) of the frame.
        public CFEVect2 m_oSize = new CFEVect2();

        /// Time this frame should be rendered without blending.
        public float m_rDelay;

        /// The blending time between this and the next frame.
        public float m_rBlendTime;

        /// Total time between this and the next frame.
        public float m_rFrameTime;

        /// The moment in whole action this frame should be played.
        public float m_rStartTime;

        /// Pivot point of the sprite frame.
        // pivots go from 0,0 to 1,1 relative to the frame
        public CFEVect2 m_oPivot = new CFEVect2();

        /// Inverse of the material dimensions
        public CFEVect2 m_o1OverDims = new CFEVect2();

        /// The material used by this sprite frame.
        // public Material m_hMaterial = null;
        public string m_hMaterial = null;
        
        /*
        /// The Unity m_sprite.
        public Mesh     m_sprite = null;

        public void GenerateSpriteGeometry(CFEVect2 _oPos, CFEVect2 _oScale)
        {
            if (m_sprite == null) m_sprite = new Mesh();

            // Retrieve vertex arrays
            Vector3[] oVX = m_sprite.vertices;
            Vector2[] oUV = m_sprite.uv;
            int[] iIdx = m_sprite.triangles;

            if ((oVX == null)  || (oVX.Length<4))  oVX = new Vector3[4];
            if ((oUV == null)  || (oUV.Length<4))  oUV = new Vector2[4];
            if ((iIdx == null) || (iIdx.Length<4)) iIdx = new int[6];

            // Setup VX
            CFEVect2 oPivot = new Vector2(0.5f, 0.5f);
            oPivot -= m_oPivot;

            oVX[0] = new Vector3(-0.5f, -0.5f, 0.0f);
            oVX[2] = new Vector3(0.5f, 0.5f, 0.0f);
            oVX[1].x = oVX[2].x;
            oVX[1].y = oVX[0].y;
            oVX[1].z = 0.0f;
            oVX[3].x = oVX[0].x;
            oVX[3].y = oVX[2].y;
            oVX[3].z = 0.0f;

            for (uint i = 0; i < 4; i++)
            {
                // 1
                oVX[i].x += oPivot.x;
                oVX[i].y += oPivot.y;

                // 2,3 
                oVX[i].x *= m_oSize.x * CFESystem.UNITS_PER_PIXEL_FACT;
                oVX[i].y *= m_oSize.y * CFESystem.UNITS_PER_PIXEL_FACT;
                oVX[i].y *= -1.0f;

                // 4
                // handle angle

                // 5 
                // _oVX[i].x = _oPos.x;
                // _oVX[i].y = _oPos.y;
            }

            // Setup indices
            iIdx[0] = 0;
            iIdx[1] = 1;
            iIdx[2] = 2;

            iIdx[3] = 2;
            iIdx[4] = 3;
            iIdx[5] = 0;

            // .........................................................
            // Setup UV
            // .........................................................
            
            // Normal path / Scale Equally XU / YV ...
            if ((m_bUWorldCoords  == false) && (m_bVWorldCoords  == false))
            {
                CFEVect2 oIUV = m_oUV.m_oIni;
                CFEVect2 oFUV = m_oUV.m_oEnd;

                if (m_bScaleXUEqually)
                {
                    oIUV.x *= _oScale.x;
                    oFUV.x *= _oScale.x;
                }
                if (m_bScaleYVEqually)
                {
                    oIUV.y *= _oScale.y;
                    oFUV.y *= _oScale.y;
                }

                // Setup UV
                oUV[0] = new Vector2(oIUV.x, 1.0f - oIUV.y);
                oUV[1] = new Vector2(oFUV.x, 1.0f - oIUV.y);
                oUV[2] = new Vector2(oFUV.x, 1.0f - oFUV.y);
                oUV[3] = new Vector2(oIUV.x, 1.0f - oFUV.y);
            }

	        // UVProject on only one axis
	        else
	        {
                CFEVect2 oMult;
                oMult.x = m_oSize.x * _oScale.x;
                oMult.y = m_oSize.y * _oScale.y;

                Vector2 oIUV = m_oPivot;
		        oIUV *= -1.0f;
		        oIUV.x *= oMult.x;
                oIUV.y *= oMult.y;
                oIUV += _oPos;
		        oIUV.x *= m_o1OverDims.x;
                oIUV.y *= m_o1OverDims.y;

                CFEVect2 oFUV = oIUV;
		        oMult.x *= m_o1OverDims.x;
                oMult.y *= m_o1OverDims.y;
                oFUV += oMult;
		
		        if (m_bUWorldCoords == false)
		        {
			        oIUV.x = m_oUV.m_oIni.x;
			        oFUV.x = m_oUV.m_oEnd.x;
			
			        if (m_bScaleXUEqually)
			        {
				        oIUV.x *= _oScale.x;
				        oFUV.x *= _oScale.x;
			        }
		        }
		        else if (m_bVWorldCoords == false)
		        {
			        oIUV.y = m_oUV.m_oIni.y;
			        oFUV.y = m_oUV.m_oEnd.y;

			        if (m_bScaleYVEqually)
			        {
				        oIUV.y *= _oScale.y;
				        oFUV.y *= _oScale.y;
			        }
		        }

                // Setup UV
                oUV[0] = new Vector2(oIUV.x, 1.0f - oIUV.y);
                oUV[1] = new Vector2(oFUV.x, 1.0f - oIUV.y);
                oUV[2] = new Vector2(oFUV.x, 1.0f - oFUV.y);
                oUV[3] = new Vector2(oIUV.x, 1.0f - oFUV.y);
            }

            // update mesh
            m_sprite.vertices   = oVX;
            m_sprite.uv         = oUV;
            m_sprite.triangles  = iIdx;
            m_sprite.RecalculateBounds();
            // m_sprite.UploadMeshData(true);
        }
        */
    };
    // ----------------------------------------------------------------------------
    public enum ESFSPlayMode
    {
        SFSPM_NONE,

        SFSPM_ONESHOT,
        SFSPM_PINGPONGSTOP,

        SFSPM_LOOP,
        SFSPM_PINGPONG,

        SFSPM_NUM,
    };

    public class CFESpriteAction : Godot.Resource
    {
        /// Default constructor for the class
        public CFESpriteAction(string _sName) { SetName(_sName); }

        // ----------------------------------------------------------------------------------------------------
        /// Returns the sprite frame number corresponding to the given time, searching from SeekFrame.
        public int uiGetFrame(float _rTime, int _uiSeekFrame)
        {
            int uiMaxFrames = m_oSeq.Count;
            switch (m_ePlayMode)
            {
                // .................................................
                case ESFSPlayMode.SFSPM_NONE:
                case ESFSPlayMode.SFSPM_ONESHOT:
                {
                    if (_rTime >= m_rActionTime)
                    {
                        return (uiMaxFrames - 1);
                    }
                    else
                    {
                        return (uiSafeGetFrame(this, _rTime, _uiSeekFrame));
                    }
                }
                // break;

                // .................................................
                case ESFSPlayMode.SFSPM_LOOP:
                {
                    if (m_rActionTime == 0.0f) return (0);
                    uint uiLoops = (uint)(_rTime / m_rActionTime);
                    float rTime = _rTime - ((float)uiLoops * m_rActionTime);

                    return (uiSafeGetFrame(this, rTime, _uiSeekFrame));
                }
                // break; // avoid statement unreachable warning

                // .................................................
                case ESFSPlayMode.SFSPM_PINGPONGSTOP:
                {
                    return (0);
                }

                case ESFSPlayMode.SFSPM_PINGPONG:
                {
                    return (0);
                }
                // break;
            };

            return (0);
        }
        
        // ----------------------------------------------------------------------------------------------------
        /// Returns the following frame to the given one taking into account the playing mode.
        public int uiNextFrame(int _uiFrame)
        {
            int uiMaxFrames = m_oSeq.Count;

            switch (m_ePlayMode)
            {
                // .................................................
                case ESFSPlayMode.SFSPM_NONE:
                {
                    if (_uiFrame < (uiMaxFrames - 1))
                        return (_uiFrame + 1);
                    else
                        return (uiMaxFrames - 1);
                }

                case ESFSPlayMode.SFSPM_ONESHOT:
                {
                    if (_uiFrame < (uiMaxFrames - 1))
                        return (_uiFrame + 1);
                    else
                        return (uiMaxFrames - 1);
                }
                // break; // avoid statement unreachable warning

                // .................................................
                case ESFSPlayMode.SFSPM_LOOP:
                {
                    return ((_uiFrame + 1) % uiMaxFrames);
                }
                // break; // avoid statement unreachable warning

                // .................................................
                case ESFSPlayMode.SFSPM_PINGPONGSTOP:
                {
                    return (0);
                }

                case ESFSPlayMode.SFSPM_PINGPONG:
                {
                    return (0);
                }
            };

            return (0);
        }
        // ----------------------------------------------------------------------------------------------------
        /// Retrieves the action play mode.
        public ESFSPlayMode eGetPlayMode() { return (m_ePlayMode); }

        /// Retrieves the maximum time of the animation without looping or -1 if infinite (when looping)
        public float rGetMaxActionTime() { return (m_rActionTime); }

        /// Retrieves the maximum time of the animation taking into account the length of looping cycles.
        public float rGetActionTime() { return (m_rActionTime); }

        /// Total time of one loop of the animation.
        public float m_rActionTime = 0.0f;

        /// The blending mode to use for this sprite action
        public EFEBlendMode m_eBlendMode = EFEBlendMode.BM_ALPHA;

        /// Play mode
        public ESFSPlayMode m_ePlayMode = ESFSPlayMode.SFSPM_NONE;

        /// Random start time: range of random time to start the action.
        public float m_rRandStartTime = 0.0f;

        /// Sequence of frames composing the sprite
        public List<CFESpriteFrame> m_oSeq = new List<CFESpriteFrame>();


        // --------------------------------------------------------------------
		/// Default constructor of this object. <summary>
        // --------------------------------------------------------------------

		/// Sets the name for this object.
        public new void SetName(string _name)
        {
            ResourceName = _name;
        }
        
    	/// Retrieves the name of this object.
		public string sGetName()
		{
			return ResourceName;
		}

        // --------------------------------------------------------------------
        // Utility functions
        // --------------------------------------------------------------------
        // PRE: 
        //	0 <= _rTime <= m_rActionTime
        //	0 <= _uiSeekFrame < uiMaxFrames
        // --------------------------------------------------------------------
        protected int uiSafeGetFrame(CFESpriteAction _poAction, float _rTime, int _uiSeekFrame)
        {
            int uiStartFrame;
            int uiEndFrame;

            int uiMaxFrames = _poAction.m_oSeq.Count;

            // Seek frame is occurs after current time
            if (_poAction.m_oSeq[_uiSeekFrame].m_rStartTime > _rTime)
            {
                // Search from the begining
                uiStartFrame = 0;
                uiEndFrame = _uiSeekFrame - 1;
            }
            else
            {
                // Search from
                uiStartFrame = _uiSeekFrame;
                uiEndFrame = uiMaxFrames - 1;
            }

            for (int i = uiStartFrame; i < uiEndFrame; i++)
            {
                if (_poAction.m_oSeq[i + 1].m_rStartTime > _rTime)
                    return(i);
            }

            return (uiMaxFrames - 1);
        }
    };

    // ----------------------------------------------------------------------------
    public class CFESprite : Godot.Resource
    {
        public CFESprite() { SetName(""); }

        /// Retrieves an action by the given index.
        public CFESpriteAction poGetAction(int _uiActionIdx)
        {
            if ((_uiActionIdx<0) || (_uiActionIdx >= m_oActions.Count)) return(null);
            return (m_oActions[_uiActionIdx]);
        }

        /// Retrieves an action by its name or NULL if the action is not found.
        public CFESpriteAction poGetAction(string _sActionName)
        {
            int iIdx = iGetActionIdx(_sActionName);
            if (iIdx == -1) return (null);

            return( poGetAction(iIdx) );
        }

        /// Retrieves an action index by its name or -1 if the action is not found.
        public int iGetActionIdx(string _sActionName)
        {
            for (int i = 0; i < m_oActions.Count; i++)
                if (m_oActions[i].sGetName() == _sActionName)
                    return (i);

            return (-1);
        }

        /// CFESpriteSequence list that builds the different animation one sprite may contain
        /// Reference to the sprite material used by this sprite ...
        public List<CFESpriteAction> m_oActions = new List<CFESpriteAction>();

        // --------------------------------------------------------------------
	    /// Sets the name for this object.
        public new void SetName(string _name)
        {
            ResourceName = _name;
        }

    	/// Retrieves the name of this object.
		public string sGetName()
		{
			return ResourceName;
		}
        // --------------------------------------------------------------------
    };
}
