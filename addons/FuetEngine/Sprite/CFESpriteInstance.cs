using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Godot;

namespace FuetEngine
{
    public class CFESpriteInstance : Node2D
    {
        // --------------------------------------------------------------------
		/// Speed multiplier for this instance.
		public float m_rSpeedMult = 1.0f;
        // ------------------------------------
        /// Current status of the sprite, i.e. animation and other things.
        // ------------------------------------
		/// Moment in the sequence related to the current action.
        public float m_rActionTime = 0.0f;
        /// Current frame in the action.
        public int m_uiCurrentActionFrame = 0;
        public CFESpriteAction m_curAction = null;
        // ------------------------------------
		/// To speedup things
        private bool m_actionHasOneFrame = false;
        private CFESprite m_sprite = null;
        private Sprite m_mainSprite = null;
        private Sprite m_secondarySprite = null;
        // --------------------------------------------------------------------
        public bool bInitialized()
        {
            return(m_sprite != null);
        }
        // --------------------------------------------------------------------
        public void Init()
        {
            m_mainSprite = GetChild(0) as Sprite;
            m_secondarySprite = GetChild(1) as Sprite;
            m_sprite = GetChild(2) as CFESprite;

            if (m_sprite != null)
            {
                AddChild(m_sprite);
                SetAction(0);
                FuetEngine.Support.SetObjectEnabled(m_sprite, false);
            }
            
            // No blending between frames for now.
            if (m_secondarySprite != null)
            {
                FuetEngine.Support.SetObjectEnabled(m_secondarySprite, false);
            }
        }
        // --------------------------------------------------------------------
        public void SetAction(int _iAction)
        {
            if (_iAction == -1) return;
            if (m_sprite == null) return;

            CFESpriteAction action = m_sprite.GetAction(_iAction);
            if (action == null) return;
                
            m_curAction = action;
            m_uiCurrentActionFrame = 0;

            CFESpriteInstUpdater.SetCurrentActionTime(this, 0.0f);
            m_actionHasOneFrame = (m_curAction.GetChildCount() == 1);
            
            SetFrameToSprite(m_curAction.GetFrame(0), ref m_mainSprite);

            if (m_actionHasOneFrame)
            {
                if (m_secondarySprite != null)
                {
                    FuetEngine.Support.SetObjectEnabled(m_secondarySprite, false);
                }
            }
        }
        // --------------------------------------------------------------------        
        public void SetAction(string _sAction)
        {
            if (m_sprite == null) return;
            SetAction(m_sprite.iGetActionIdx(_sAction));
        }
        // --------------------------------------------------------------------
        private void SetFrameToSprite(CFESpriteFrame _frame, ref Sprite _sprite)
        {
            _sprite.Scale = _frame.m_oScale;
            _sprite.Texture = _frame.m_hMaterial;

            _sprite.Offset = new Vector2((0.5f-_frame.m_oPivot.x) * _frame.m_oSize.x,
                                         (0.5f-_frame.m_oPivot.y) * _frame.m_oSize.y);
        }
        // --------------------------------------------------------------------
        public override void _Ready()
        {
            Init();
        }
        // --------------------------------------------------------------------
		public override void _Process(float _deltaT)
		{
            if ((m_sprite == null) || m_actionHasOneFrame || (m_curAction == null))
            {
                return;
            }

            CFESpriteFrame oldFrame = m_curAction.GetFrame(m_uiCurrentActionFrame);
            CFESpriteInstUpdater.Update(this, _deltaT);
            CFESpriteFrame currentFrame = m_curAction.GetFrame(m_uiCurrentActionFrame);

            if (currentFrame == null)
                return;
                
            SetFrameToSprite(currentFrame, ref m_mainSprite);
        }
        /*
        // --------------------------------------------------------------------
        private void RenderSpriteFrame(CFESpriteFrame _oDst, Vector2 _oPos, Vector2 _oScale, float _rAngle, Color _oColor, float _alpha)
        {
            
        }
        // --------------------------------------------------------------------
        private void RenderSpriteFrames(CFESpriteFrame _oSrc, CFESpriteFrame _oDst, float _rFactor, Vector2 _oPos, Vector2 _oScale, float _rAngle, Color _oColor)
        {
            float rSFactor = Mathf.Clamp(2.0f*(1.0f-_rFactor), 0.0f ,1.0f);
            float rDFactor = Mathf.Clamp(2.0f*(_rFactor),0.0f, 1.0f);

            RenderSpriteFrame(_oSrc, _oPos, _oScale, _rAngle, _oColor, rSFactor);
            RenderSpriteFrame(_oDst, _oPos, _oScale, _rAngle, _oColor, rDFactor);
        }
        // --------------------------------------------------------------------
        private void SafeRenderFrame(int _iFrame, float _rActionTime, Vector2 _oPos, float _rDepth, Vector2 _oScale, float _rAngle, Color _oColor)
        {
            uint uiLoops = (uint)(_rActionTime / m_curAction.m_rActionTime);
            float rTime = _rActionTime - ((float)(uiLoops)*m_curAction.m_rActionTime);

            CFESpriteFrame poSF = m_curAction.GetFrame(_iFrame);
            float rRelTime  = rTime - poSF.m_rStartTime;
            int iNextFrame = m_curAction.uiNextFrame(_iFrame);

            // Sets the rendering depth.
            // _poRenderer->SetDepth(_rDepth);

            if (rRelTime <= poSF.m_rDelay)
            {
                // Render only the current frame.
                RenderSpriteFrame(poSF, _oPos, _oScale, _rAngle, _oColor, 1.0f);
            }
            else
            {
                // Blend factor between frames.	
                float rFact = (rRelTime - poSF.m_rDelay) / poSF.m_rBlendTime;
                RenderSpriteFrames(m_curAction.GetFrame(_iFrame),m_curAction.GetFrame(iNextFrame), rFact, _oPos, _oScale, _rAngle, _oColor);
            }
        }
        // --------------------------------------------------------------------
        private void Render(Vector2 _oPos, float _rDepth, Vector2 _oScale, float _rAngle, Color _oColor)
        {
            // Special cases: (_poInstance->m_rActionTime  <= _0r) is a specific case of the one treated on SafeRenderFrame
            if ((m_curAction.m_eBlendMode == EFEBlendMode.BM_NONE) || (m_curAction.m_eBlendMode == EFEBlendMode.BM_COPY)|| m_actionHasOneFrame || (m_curAction.m_rActionTime <= 0.0f))
            {
                // _poRenderer->SetDepth(_rDepth);
                RenderSpriteFrame(m_curAction.GetFrame(m_uiCurrentActionFrame), _oPos, _oScale, _rAngle, _oColor, 1.0f);
                return;
            }

            // Render the frame
            switch (m_curAction.m_ePlayMode)
            {
                // ------------------------------------------------
                case ESFSPlayMode.SFSPM_NONE:
                case ESFSPlayMode.SFSPM_ONESHOT:
                {
                    SafeRenderFrame(m_uiCurrentActionFrame, m_rActionTime, _oPos, _rDepth, _oScale, _rAngle, _oColor);
                }
                break;
                // ------------------------------------------------
                case ESFSPlayMode.SFSPM_LOOP:
                {
                    SafeRenderFrame(m_uiCurrentActionFrame, m_rActionTime, _oPos, _rDepth, _oScale, _rAngle, _oColor);
                }
                break;
                // ------------------------------------------------
                case ESFSPlayMode.SFSPM_PINGPONGSTOP:
                {
                    // #pragma message("TO BE IMPLEMENTED")
                }
                break;
                case ESFSPlayMode.SFSPM_PINGPONG:
                {
                    // #pragma message("TO BE IMPLEMENTED")
                }
                break;
            }
        }
        */
        // --------------------------------------------------------------------
    }
}