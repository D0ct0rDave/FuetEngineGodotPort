using System;
using Godot;
using System.Collections.Generic;
using CFEVect2 = Godot.Vector2;

namespace FuetEngine
{
    // ------------------------------------------------------------------------
    public enum ESFSPlayMode
    {
        SFSPM_NONE,

        SFSPM_ONESHOT,
        SFSPM_PINGPONGSTOP,

        SFSPM_LOOP,
        SFSPM_PINGPONG,

        SFSPM_NUM,
    };
    [Tool]
    // ------------------------------------------------------------------------
    public class CFESpriteAction : Node
    {
        /// Total time of one loop of the animation.
        [Export]
        public float m_rActionTime = 0.0f;
        /// The blending mode to use for this sprite action
        [Export]
        public EFEBlendMode m_eBlendMode = EFEBlendMode.BM_ALPHA;
        /// Play mode
        [Export]
        public ESFSPlayMode m_ePlayMode = ESFSPlayMode.SFSPM_NONE;
        /// Random start time: range of random time to start the action.
        [Export]
        public float m_rRandStartTime = 0.0f;
        /// Sequence of frames composing the sprite
        // --------------------------------------------------------------------
        // Make sure you provide a parameterless constructor.
        public CFESpriteAction()
        {
        }
        /// Default constructor for the class
        public CFESpriteAction(string _sName) { SetName(_sName); }
        /// Retrieves the action play mode.
        public ESFSPlayMode eGetPlayMode() { return (m_ePlayMode); }
        /// Retrieves the maximum time of the animation without looping or -1 if infinite (when looping)
        public float rGetMaxActionTime() { return (m_rActionTime); }
        /// Retrieves the maximum time of the animation taking into account the length of looping cycles.
        public float rGetActionTime() { return (m_rActionTime); }
        // --------------------------------------------------------------------
		/// Sets the name for this object.
        public new void SetName(string _name)
        {
            Name = _name;
        }
        
    	/// Retrieves the name of this object.
		public string sGetName()
		{
			return Name;
		}
        // --------------------------------------------------------------------
        /// Returns the sprite frame number corresponding to the given time, searching from SeekFrame.
        public int uiGetFrame(float _rTime, int _uiSeekFrame)
        {
            int uiMaxFrames = GetChildCount();
            switch (m_ePlayMode)
            {
                // .................................................
                case ESFSPlayMode.SFSPM_NONE:
                case ESFSPlayMode.SFSPM_ONESHOT:
                {
                    if (_rTime >= m_rActionTime)
                    {
                        return uiMaxFrames - 1;
                    }
                    else
                    {
                        return uiSafeGetFrame(_rTime, _uiSeekFrame);
                    }
                }
                // break;

                // .................................................
                case ESFSPlayMode.SFSPM_LOOP:
                {
                    if (m_rActionTime == 0.0f) return (0);
                    uint uiLoops = (uint)(_rTime / m_rActionTime);
                    float rTime = _rTime - ((float)uiLoops * m_rActionTime);

                    return uiSafeGetFrame(rTime, _uiSeekFrame);
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
        // --------------------------------------------------------------------
        /// Returns the following frame to the given one taking into account the playing mode.
        public int uiNextFrame(int _uiFrame)
        {
            int uiMaxFrames = GetChildCount();
            switch (m_ePlayMode)
            {
                // .................................................
                case ESFSPlayMode.SFSPM_NONE:
                case ESFSPlayMode.SFSPM_ONESHOT:
                {
                    if (_uiFrame < (uiMaxFrames - 1))
                        return _uiFrame + 1;
                    else
                        return uiMaxFrames - 1;
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
        // --------------------------------------------------------------------
        // Utility functions
        // --------------------------------------------------------------------
        // PRE: 
        //	0 <= _rTime <= m_rActionTime
        //	0 <= _uiSeekFrame < uiMaxFrames
        // --------------------------------------------------------------------
        protected int uiSafeGetFrame(float _rTime, int _uiSeekFrame)
        {
            int uiStartFrame;
            int uiEndFrame;

            int uiMaxFrames = GetChildCount();

            // Seek frame is occurs after current time
            CFESpriteFrame seekFrame = GetFrame(_uiSeekFrame);
            if (seekFrame == null)
            {
                return 0; 
            }

            if (seekFrame.m_rStartTime > _rTime)
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
                CFESpriteFrame frame = GetFrame(i + 1);
                if (frame == null)
                {
                    return 0;
                }
                
                if (frame.m_rStartTime > _rTime)
                    return(i);
            }

            return uiMaxFrames - 1;
        }
        // --------------------------------------------------------------------
        public CFESpriteFrame GetFrame(int i)
        {
            return GetChild(i) as CFESpriteFrame;
        }
        // --------------------------------------------------------------------
    };
}
