using System;
using Godot;
//-----------------------------------------------------------------------------
using CFEString = System.String;
using FEReal = System.Single;
using CFEVect2 = Godot.Vector2;
using CFEColor = Godot.Color;
using CFEFont = Godot.Theme;
using CFEHUDElementAction = Godot.Animation; 
//-----------------------------------------------------------------------------
/*
#define GENERAL_ACTION_TIME_CHECK()\
		if ((eWrapMode != KFBFWM_LOOP) && (eWrapMode != KFBFWM_PINGPONG))\
		{\
			 if ((rTime>=_0r)&& (rActionTime < rTime))\
				rActionTime = rTime;\
		}\
		else\
		{\
			return(-_1r);\
		}
*/

public static class CFEHUDActionTime
{
	public static float rGetActionTime(CFEHUDElementAction _oAction)
	{
		/*
		if (_poAction->uiNumActions() == 0) return(_0r);
		FEReal rActionTime = -_1r;

		for (uint i=0;i<_poAction->uiNumActions();i++)
		{
			CFEHUDObjectAction* poObjAction = _poAction->poGetAction(i);
			EFEKFBFuncWrapMode eWrapMode;
			FEReal rTime;

			eWrapMode = poObjAction->m_rXFunc.eGetWrapMode();
			rTime = poObjAction->m_rXFunc.rGetMaxLimit();
			GENERAL_ACTION_TIME_CHECK();

			eWrapMode = poObjAction->m_rYFunc.eGetWrapMode();
			rTime = poObjAction->m_rYFunc.rGetMaxLimit();
			GENERAL_ACTION_TIME_CHECK();

			eWrapMode = poObjAction->m_oScaleFunc.eGetWrapMode();
			rTime = poObjAction->m_oScaleFunc.rGetMaxLimit();
			GENERAL_ACTION_TIME_CHECK();

			eWrapMode = poObjAction->m_rAngleFunc.eGetWrapMode();
			rTime = poObjAction->m_rAngleFunc.rGetMaxLimit();
			GENERAL_ACTION_TIME_CHECK();

			eWrapMode = poObjAction->m_rDepthFunc.eGetWrapMode();
			rTime = poObjAction->m_rDepthFunc.rGetMaxLimit();
			GENERAL_ACTION_TIME_CHECK();

			eWrapMode = poObjAction->m_oColorFunc.eGetWrapMode();
			rTime = poObjAction->m_oColorFunc.rGetMaxLimit();
			GENERAL_ACTION_TIME_CHECK();

			eWrapMode = poObjAction->m_iActionFunc.eGetWrapMode();
			rTime = poObjAction->m_iActionFunc.rGetMaxLimit();
			GENERAL_ACTION_TIME_CHECK();

			eWrapMode = poObjAction->m_bVisFunc.eGetWrapMode();
			rTime = poObjAction->m_bVisFunc.rGetMaxLimit();
			GENERAL_ACTION_TIME_CHECK();

			eWrapMode = poObjAction->m_oEventFunc.eGetWrapMode();
			rTime = poObjAction->m_oEventFunc.rGetMaxLimit();
			GENERAL_ACTION_TIME_CHECK();
		}

		return(rActionTime);
		*/

		return 0.0f;
	}
	//-----------------------------------------------------------------------------
	public static FEReal rGetMaxActionTime(CFEHUDElementAction _oAction)
	{
		if (_oAction.GetTrackCount() == 0)
		{
			return 0.0f;
		}

		FEReal rMaxActionTime = -1.0f;
		for (int i=0; i<_oAction.GetTrackCount(); i++)
		{
			int keyCount = _oAction.TrackGetKeyCount(i);
			if (keyCount > 0)
			{
				FEReal keyTime = _oAction.TrackGetKeyTime(i, keyCount-1);
				if ((keyTime >= 0.0f) && (keyTime > rMaxActionTime))
				{
					rMaxActionTime = keyTime;
				}
			}
		}

		return rMaxActionTime;
	}
//-----------------------------------------------------------------------------
}
