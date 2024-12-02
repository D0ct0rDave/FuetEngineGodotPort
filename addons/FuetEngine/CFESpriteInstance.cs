using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;

namespace FuetEngine
{
    public class CFESpriteInstance : Node2D
    {	
		/// To speedup things
        private bool m_bOneActionOneFrame = false;

		/// Speed multiplier for this instance.
		private float m_rSpeedMult = 1.0f;

        /// Current status of the sprite, i.e. animation and other things.
        /// Current sprite action being played.
        private int m_uiSpriteAction = 0;
        private CFESpriteAction m_oCurAction = null;
        private CFESpriteFrame m_oCurFrame = null;
		/// Moment in the sequence related to the current action.
        private float m_rActionTime = 0.0f;
        /// Current frame in the action.
        private int m_uiCurrentActionFrame = 0;
        /// Current frame in the action.
        private Color m_oColor = new Color(1,1,1,1);
        [Export]
        public CFESprite m_poSprite = new CFESprite();

        public bool bInitialized()
        {
            return(m_poSprite != null);
        }

        public void Init(CFESprite _sprite)
        {
            // This prevents the created components to be saved when the scene is saved.
            // hideFlags = HideFlags.HideAndDontSave;
            m_poSprite = _sprite;
            
            AddChild(m_poSprite);
            m_poSprite.Owner = this;

            if (m_poSprite != null)
            {
                m_uiSpriteAction = 0;
                m_oCurAction = m_poSprite.poGetAction(0);
                if (m_oCurAction == null)
                {
                    // delete sprite
                    m_poSprite = null;
                    return;
                }

                m_uiCurrentActionFrame = 0;
                m_oCurFrame = m_oCurAction.m_oSeq[0];
                if (m_oCurFrame == null)
                {
                    // delete sprite
                    m_poSprite = null;
                    return;
                }

                // TODO: SetupScale(_oGameObject);

                // 
                m_bOneActionOneFrame = (m_poSprite.m_oActions.Count == 1) && (m_oCurAction.m_oSeq.Count == 1);
            }
        }

		// Update is called once per frame
		public override void _Process(float _deltaT)
		{
			if (! m_bOneActionOneFrame)
			{
                if (m_poSprite == null)
                    return;

                // TODO: CFESpriteInstUpdater.Update(this, _deltaT);

				m_oCurAction = m_poSprite.poGetAction(m_uiSpriteAction);
                if (m_oCurAction == null)
                    return;

				m_oCurFrame = m_oCurAction.m_oSeq[m_uiCurrentActionFrame];
                if (m_oCurFrame == null)
                    return;

                // TODO:  if (transform.localScale != Vector3.one)
                    // TODO: SetupScale(gameObject);

                // Graphics.DrawTexture(rect, m_mr.sprite.texture, m_mr.sprite.rect, 0, 0, 0, 0);
            }
        }

        public void SetAction(int _iAction)
        {
            if (_iAction == -1) return;
            if (m_poSprite == null) return;

            CFESpriteAction oAction = m_poSprite.poGetAction(_iAction);
            if (oAction == null) return;
                
            m_uiSpriteAction = _iAction;
            m_oCurAction     = oAction;

            m_uiCurrentActionFrame = 0;
            m_oCurFrame = m_oCurAction.m_oSeq[0];
            if (m_oCurFrame != null)
            {
            }
            
            // TODO: CFESpriteInstUpdater.SetCurrentActionTime(this, 0.0f);
            // TODO: SetupScale(gameObject);

            // 
            m_bOneActionOneFrame = (m_poSprite.m_oActions.Count == 1) && (m_oCurAction.m_oSeq.Count == 1);
        }
        
        public void SetAction(string _sAction)
        {
            if (m_poSprite == null) return;
            SetAction(m_poSprite.iGetActionIdx(_sAction));
        }
    }
}
