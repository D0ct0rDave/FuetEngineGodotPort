using System;
using Godot;

namespace FuetEngine
{
    [Tool]
    // ------------------------------------------------------------------------
    public class CFESprite : Node
    {
        // Make sure you provide a parameterless constructor.
        public CFESprite() {}

        /// Retrieves an action by the given index.
        public CFESpriteAction GetAction(int _uiActionIdx)
        {
            if ((_uiActionIdx<0) || (_uiActionIdx >= GetChildCount())) return(null);
            return GetChild(_uiActionIdx) as CFESpriteAction;
        }

        /// Retrieves an action by its name or NULL if the action is not found.
        public CFESpriteAction GetAction(string _sActionName)
        {
            int iIdx = iGetActionIdx(_sActionName);
            if (iIdx == -1) return (null);

            return(GetAction(iIdx));
        }

        /// Retrieves an action index by its name or -1 if the action is not found.
        public int iGetActionIdx(string _sActionName)
        {
            for (int i = 0; i < GetChildCount(); i++)
                if (GetChild(i).Name == _sActionName)
                    return (i);

            return (-1);
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

        public static explicit operator CFESprite(Reference v)
        {
            throw new NotImplementedException();
        }
        // --------------------------------------------------------------------
    };
}
