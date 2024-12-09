using System;
using System.Collections.Generic;
using Godot;

namespace FuetEngine
{
    // ------------------------------------------------------------------------
    [Tool]
    public class CFESprite : Resource
    {
        /// CFESpriteSequence list that builds the different animation one sprite may contain
        /// Reference to the sprite material used by this sprite ...
        [Export]
        public List<CFESpriteAction> m_oActions = new List<CFESpriteAction>();
        [Export]
        public string Name {get;set;}

        public CFESprite() { SetName("CFESprite"); }

        /// Retrieves an action by the given index.
        public CFESpriteAction GetAction(int _uiActionIdx)
        {
            if ((_uiActionIdx<0) || (_uiActionIdx >= m_oActions.Count)) return(null);
            return (m_oActions[_uiActionIdx]);
        }

        /// Retrieves an action by its name or NULL if the action is not found.
        public CFESpriteAction GetAction(string _sActionName)
        {
            int iIdx = iGetActionIdx(_sActionName);
            if (iIdx == -1) return (null);

            return( GetAction(iIdx) );
        }

        /// Retrieves an action index by its name or -1 if the action is not found.
        public int iGetActionIdx(string _sActionName)
        {
            for (int i = 0; i < m_oActions.Count; i++)
                if (m_oActions[i].sGetName() == _sActionName)
                    return (i);

            return (-1);
        }

        /// Retrieves an action by the given index.
        public int iAddAction(CFESpriteAction _spriteAction)
        {
            m_oActions.Add(_spriteAction);
            // return AddChild(_spriteAction);

            return m_oActions.Count-1;
        }
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
    };
}
