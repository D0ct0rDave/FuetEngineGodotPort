using System;
using Godot;
using System.Collections.Generic;
using CFEVect2 = Godot.Vector2;

namespace FuetEngine
{
    // ----------------------------------------------------------------------------
    public class CFESprite : Node
    {
        // --------------------------------------------------------------------
        // Make sure you provide a parameterless constructor.
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
        // --------------------------------------------------------------------
		/// Sets the name for this object.
        public new void SetName(string _name)
        {
            m_name = _name;
        }
        
    	/// Retrieves the name of this object.
		public string sGetName()
		{
			return m_name;
		}

        private string m_name;
        // --------------------------------------------------------------------
        [Export]        
        public List<CFESpriteAction> m_oActions = new List<CFESpriteAction>();
        // --------------------------------------------------------------------
    };
}
