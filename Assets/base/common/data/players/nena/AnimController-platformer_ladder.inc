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

const uint NUM_TRANSITIONS = 22;
//-----------------------------------------------------------------------------
static TTransitionData gsoTransitions[] =
{
    {"",0,0,0.0f},
    // 1
    {
        "IN_LADDER",
        0 | ACV_IN_LADDER,
        0 | ACV_IN_LADDER,
        0.000
    },
    // 2
    {
        "IN_LADDER_TOP&~IN_LADDER_BOTTOM",
        0 | ACV_IN_LADDER_TOP,
        0 | ACV_IN_LADDER_BOTTOM | ACV_IN_LADDER_TOP,
        0.000
    },
    // 3
    {
        "~IN_LADDER_TOP",
        0,
        0 | ACV_IN_LADDER_TOP,
        0.000
    },
    // 4
    {
        "IN_LADDER_TOP&IN_LADDER_BOTTOM",
        0 | ACV_IN_LADDER_BOTTOM | ACV_IN_LADDER_TOP,
        0 | ACV_IN_LADDER_BOTTOM | ACV_IN_LADDER_TOP,
        0.000
    },
    // 5
    {
        "~IN_LADDER",
        0,
        0 | ACV_IN_LADDER,
        0.000
    },
    // 6
    {
        "IN_LADDER_TOP&~IN_LADDER_BOTTOM",
        0 | ACV_IN_LADDER_TOP,
        0 | ACV_IN_LADDER_BOTTOM | ACV_IN_LADDER_TOP,
        0.000
    },
    // 7
    {
        "~WALK&~ATTACK",
        0,
        0 | ACV_ATTACK | ACV_WALK,
        0.000
    },
    // 8
    {
        "ATTACK",
        0 | ACV_ATTACK,
        0 | ACV_ATTACK,
        0.000
    },
    // 9
    {
        "~IN_LADDER",
        0,
        0 | ACV_IN_LADDER,
        0.000
    },
    // 10
    {
        "~IN_LADDER",
        0,
        0 | ACV_IN_LADDER,
        0.000
    },
    // 11
    {
        "~PLAYING_ANIM&~IN_LADDER_TOP",
        0,
        0 | ACV_IN_LADDER_TOP | ACV_PLAYING_ANIM,
        0.000
    },
    // 12
    {
        "IN_LADDER&~IN_LADDER_TOP",
        0 | ACV_IN_LADDER,
        0 | ACV_IN_LADDER_TOP | ACV_IN_LADDER,
        0.000
    },
    // 13
    {
        "~IN_LADDER",
        0,
        0 | ACV_IN_LADDER,
        0.000
    },
    // 14
    {
        "WALK&~ATTACK",
        0 | ACV_WALK,
        0 | ACV_ATTACK | ACV_WALK,
        0.000
    },
    // 15
    {
        "ATTACK",
        0 | ACV_ATTACK,
        0 | ACV_ATTACK,
        0.000
    },
    // 16
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.250
    },
    // 17
    {
        "IN_LADDER_BOTTOM&~IN_LADDER_TOP",
        0 | ACV_IN_LADDER_BOTTOM,
        0 | ACV_IN_LADDER_TOP | ACV_IN_LADDER_BOTTOM,
        0.000
    },
    // 18
    {
        "ATTACK",
        0 | ACV_ATTACK,
        0 | ACV_ATTACK,
        0.000
    },
    // 19
    {
        "~WALK&~ATTACK",
        0,
        0 | ACV_ATTACK | ACV_WALK,
        0.000
    },
    // 20
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.000
    },
    // 21
    {
        "ATTACK",
        0 | ACV_ATTACK,
        0 | ACV_ATTACK,
        0.000
    },
    // 22
    {
        "WALK&~ATTACK",
        0 | ACV_WALK,
        0 | ACV_ATTACK | ACV_WALK,
        0.000
    },
};
//-----------------------------------------------------------------------------
enum EAnimControllerState
{
    ACS_NONE,

    ACS_ANY,
    ACS_LADDER_START,
    ACS_LADDER_LOOP,
    ACS_LADDER_END,
    ACS_LADDER_TOP,
    ACS_LADDER_LOOP_EXIT,
    ACS_LADDER_SIDE,
    ACS_LADDER_ATTACK,
    ACS_LADDER_LOOP2,
    ACS_LADDER_ATTACK2,
    ACS_LADDER_SIDE2,

    ACS_NUM_STATES,
    ACS_MAX_STATES = 0xffffffff
};

//-----------------------------------------------------------------------------
static CFEString gssAnimations[ACS_NUM_STATES] =
{
    "",
    "",
    "",
    "ladder_loop",
    "",
    "ladder_climb_down",
    "ladder_climb_up",
    "ladder_side",
    "ladder_attack",
    "ladder_loop",
    "ladder_attack",
    "ladder_side",
};

//-----------------------------------------------------------------------------
static int gsiStateTable[ACS_NUM_STATES*ACS_NUM_STATES] =
{
    -1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,-1,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,-1,-1,3,-1,2,-1,-1,-1,4,-1,-1,
    -1,-1,-1,-1,5,-1,6,7,8,-1,-1,-1,
    -1,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,10,-1,11,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,-1,-1,12,13,-1,-1,-1,-1,-1,-1,-1,
    -1,-1,-1,14,-1,-1,-1,-1,15,-1,-1,-1,
    -1,-1,-1,-1,-1,-1,-1,16,-1,-1,-1,-1,
    -1,-1,-1,17,-1,-1,-1,-1,-1,-1,18,19,
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,20,
    -1,-1,-1,-1,-1,-1,-1,-1,-1,22,21,-1,
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
