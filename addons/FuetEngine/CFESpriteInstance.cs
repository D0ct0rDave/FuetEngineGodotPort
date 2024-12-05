using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;

namespace FuetEngine
{
    [Tool]
    public class CFESpriteInstance : Node2D
    {
        // ------------------------------------------------------------------------
		/// To speedup things
        private bool m_actionHasOneFrame = false;

		/// Speed multiplier for this instance.
		public float m_rSpeedMult = 1.0f;

        /// Current status of the sprite, i.e. animation and other things.
        /// Current sprite action being played.
        public int m_uiSpriteAction = 0;
        private CFESpriteAction m_curAction = null;
		/// Moment in the sequence related to the current action.
        public float m_rActionTime = 0.0f;
        /// Current frame in the action.
        public int m_uiCurrentActionFrame = 0;
        /// Current frame in the action.
        private Color m_oColor = new Color(1,1,1,1);
        public CFESprite m_sprite = null;
        private Sprite m_mainSprite = null;
        private Sprite m_secondarySprite = null;
        // ------------------------------------------------------------------------
        public bool bInitialized()
        {
            return(m_sprite != null);
        }
        // ------------------------------------------------------------------------
        public void Init(CFESprite _sprite)
        {
            // This prevents the created components to be saved when the scene is saved.
            // hideFlags = HideFlags.HideAndDontSave;
            m_sprite = _sprite;

            if (m_sprite != null)
            {
                AddChild(m_sprite);
                SetAction(0);
                FuetEngine.Support.SetObjectEnabled(m_sprite, false);
            }
        }
        // ------------------------------------------------------------------------
        public void SetAction(int _iAction)
        {
            if (_iAction == -1) return;
            if (m_sprite == null) return;

            CFESpriteAction action = m_sprite.GetAction(_iAction);
            if (action == null) return;
                
            m_uiSpriteAction = _iAction;
            m_curAction     = action;

            m_uiCurrentActionFrame = 0;

            CFESpriteInstUpdater.SetCurrentActionTime(this, 0.0f);
            m_actionHasOneFrame = (m_curAction.GetChildCount() == 1);

            if (m_actionHasOneFrame)
            {
                if (m_mainSprite != null)
                {
                    SetFrameToSprite(m_curAction.GetFrame(0), ref m_mainSprite);
                }

                FuetEngine.Support.SetObjectEnabled(m_sprite, false);
            }
        }
        // ------------------------------------------------------------------------        
        public void SetAction(string _sAction)
        {
            if (m_sprite == null) return;
            SetAction(m_sprite.iGetActionIdx(_sAction));
        }
        // ------------------------------------------------------------------------
        private void SetFrameToSprite(CFESpriteFrame _frame, ref Sprite _sprite)
        {
            _sprite.Scale = _frame.m_oScale;
            _sprite.Texture = _frame.m_hMaterial;
            _sprite.Offset = new Vector2(_frame.m_oPivot.x * _frame.m_oSize.x,
                                         _frame.m_oPivot.y * _frame.m_oSize.y);
        }
        // ------------------------------------------------------------------------
        public override void _Ready()
        {
            m_mainSprite = GetChild(1) as Sprite;
            m_secondarySprite = GetChild(2) as Sprite;

            Init(GetChild(0) as CFESprite);

        }
        // ------------------------------------------------------------------------
		// Update is called once per frame
		public override void _Process(float _deltaT)
		{
            if ((m_sprite == null) || m_actionHasOneFrame)
            {
                return;
            }

            CFESpriteFrame oldFrame = m_curAction.GetFrame(m_uiCurrentActionFrame);
            CFESpriteInstUpdater.Update(this, _deltaT);
            CFESpriteFrame currentFrame = m_curAction.GetFrame(m_uiCurrentActionFrame);

            if (currentFrame == null)
                return;
                
            if (m_mainSprite != null)
            {
                SetFrameToSprite(currentFrame, ref m_mainSprite);
            }
        }
        // ------------------------------------------------------------------------
    }
}
