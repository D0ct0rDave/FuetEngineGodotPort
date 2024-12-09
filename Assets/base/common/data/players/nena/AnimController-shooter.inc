//-----------------------------------------------------------------------------
// Automatically generated header file . Please modify carefully.
//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
#include <FuetEngine.h>
//-----------------------------------------------------------------------------
typedef struct
{
    char* m_szExpression;
	uint  m_uiFlagsValues;
	uint  m_uiFlagsMask;
	float m_fBlendTime;
}TTransitionData;

const uint NUM_TRANSITIONS = 4;
//-----------------------------------------------------------------------------
static TTransitionData gsoTransitions[] =
{
    {"",0,0,0.0f},
    // 1
    {
        "ATTACK&CROUCH",
        0 | ACV_CROUCH | ACV_ATTACK,
        0 | ACV_CROUCH | ACV_ATTACK,
        0.000
    },
    // 2
    {
        "ATTACK&~CROUCH",
        0 | ACV_ATTACK,
        0 | ACV_CROUCH | ACV_ATTACK,
        0.000
    },
    // 3
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.000
    },
    // 4
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.000
    },
};
//-----------------------------------------------------------------------------
enum EAnimControllerState
{
    ACS_NONE,

    ACS_ANY,
    ACS_ATTACK,
    ACS_ATTACK_CROUCH,

    ACS_NUM_STATES,
    ACS_MAX_STATES = 0xffffffff
};

//-----------------------------------------------------------------------------
static CFEString gssAnimations[ACS_NUM_STATES] =
{
    "",
    "",
    "shot",
    "crouch_shot",
};

//-----------------------------------------------------------------------------
static int gsiStateTable[ACS_NUM_STATES*ACS_NUM_STATES] =
{
    -1,0,-1,-1,
    -1,-1,2,1,
    -1,3,-1,-1,
    -1,4,-1,-1,
};
//-----------------------------------------------------------------------------
static EAnimControllerState eGetNewState(EAnimControllerState _eCurState,uint _uiCurFlags,int* _piLastTransition)
{
	int* piStateTable = &gsiStateTable[_eCurState*ACS_NUM_STATES + 0];

	for (uint s=0;s<ACS_NUM_STATES;s++)
	{
		if (piStateTable[s] > -1)
		{
			// Current state has a possible transition to the state "s"
			int iTransitionIdx = piStateTable[s];
			TTransitionData* pTrans = &gsoTransitions[iTransitionIdx];

			// si el valor actual de los flags son el mismo que el requerido para la transici�n
			// uiVal ser� igual a 0.
			if ( ((_uiCurFlags & pTrans->m_uiFlagsMask) ^ pTrans->m_uiFlagsValues) == 0)
			{
				if (_piLastTransition != NULL)
					*_piLastTransition = iTransitionIdx;

				// we have a new state "s"
				return((EAnimControllerState)s);
			}
		}
	}

	return(_eCurState);
}
//-----------------------------------------------------------------------------
