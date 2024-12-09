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
        "PAIN",
        0 | ACV_PAIN,
        0 | ACV_PAIN,
        0.000
    },
    // 2
    {
        "DIE",
        0 | ACV_DIE,
        0 | ACV_DIE,
        0.500
    },
    // 3
    {
        "DIE",
        0 | ACV_DIE,
        0 | ACV_DIE,
        0.500
    },
    // 4
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.250
    },
};
//-----------------------------------------------------------------------------
enum EAnimControllerState
{
    ACS_NONE,

    ACS_IDLE,
    ACS_PAIN,
    ACS_DYING,

    ACS_NUM_STATES,
    ACS_MAX_STATES = 0xffffffff
};

//-----------------------------------------------------------------------------
static CFEString gssAnimations[ACS_NUM_STATES] =
{
    "",
    "flying",
    "pain",
    "dead",
};

//-----------------------------------------------------------------------------
static int gsiStateTable[ACS_NUM_STATES*ACS_NUM_STATES] =
{
    -1,0,-1,-1,
    -1,-1,1,2,
    -1,4,-1,3,
    -1,-1,-1,-1,
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

			// si el valor actual de los flags son el mismo que el requerido para la transición
			// uiVal será igual a 0.
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
