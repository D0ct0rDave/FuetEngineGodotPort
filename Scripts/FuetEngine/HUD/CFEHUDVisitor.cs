//-----------------------------------------------------------------------------
using Godot;
//-----------------------------------------------------------------------------
using CFEString = System.String;
using FEReal = System.Single;
using CFEVect2 = Godot.Vector2;
using CFEColor = Godot.Color;   
//-----------------------------------------------------------------------------
namespace FuetEngine
{
    [Tool]
    public class CFEHUDVisitor
    {
        /// 
        public virtual void Visit(CFEHUD _oObj)
        {
            for(int i=0; i<_oObj.iNumElements(); i++)
		    {
                _oObj.oGetElement(i).Accept(this);
            }
        }

        /// 
        public virtual void Visit(CFEHUDElement _oObj)
        {
            for (int i=0; i<_oObj.iNumLayers(); i++)
                if (_oObj.oGetLayer(i) != null)
                    _oObj.oGetLayer(i).Accept(this);

            /*
            for (int i=0; i<_oObj.iNumActions(); i++)
                if (_oObj.oGetAction(i) != null)
                    _oObj.oGetAction(i).Accept(this);
            */
        }

        /// 
        /*
        public virtual void Visit(CFEHUDElementAction _oObj)
        {
            for (int i=0; i<_oObj.iNumActions(); i++)
                if (_oObj.oGetAction(i) != null)
                    _oObj.oGetAction(i).Accept(this);
        }
        */

        /// 
        public virtual void Visit(CFEHUDGroup _oObj)
        {
            Visit((CFEHUDObject)_oObj);

            for (int i=0; i<_oObj.iNumObjs(); i++)
                if (_oObj.oGetObject(i) != null)
                    _oObj.oGetObject(i).Accept(this);
        }

        /// 
        public virtual void Visit(CFEHUDLabel _oObj)
        {
            Visit((CFEHUDObject)_oObj);
        }
        
        /// 
        public virtual void Visit(CFEHUDIcon _oObj)
        {
            Visit((CFEHUDObject)_oObj);
        }

        /// 
        public virtual void Visit(CFEHUDRect _oObj)
        {
            Visit((CFEHUDObject)_oObj);
        }
        
        /// 
        public virtual void Visit(CFEHUDShape _oObj)
        {
            Visit((CFEHUDObject)_oObj);
        }
        
        /// 
        public virtual void Visit(CFEHUDPSys _oObj)
        {
            Visit((CFEHUDObject)_oObj);
        }

        /// 
        // virtual void Visit(CFEHUDObject* _poObj)
        public virtual void Visit(CFEHUDObject _oObj)
        {
            // default behavior
            // DO NOTHING
        }
        ///
        // virtual void Visit(CFEHUDObjectAction* _poObj)
        /*
        public virtual void Visit(CFEHUDObjectAction _oAction)
        {
            // default behavior
            // DO NOTHING        
        }
        */

        /// Retrieves the type of visitor (useful when needed to extend the basic class hierarchy).
        public virtual CFEString sGetType() { return "BaseVisitor"; }
    }
}
//-----------------------------------------------------------------------------
