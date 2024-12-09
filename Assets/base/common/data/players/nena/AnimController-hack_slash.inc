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

const uint NUM_TRANSITIONS = 18;
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
        "RND_1&RND_2",
        0 | ACV_RND_2 | ACV_RND_1,
        0 | ACV_RND_2 | ACV_RND_1,
        0.000
    },
    // 4
    {
        "RND_1&~RND_2",
        0 | ACV_RND_1,
        0 | ACV_RND_2 | ACV_RND_1,
        0.000
    },
    // 5
    {
        "~RND_1&~RND_2",
        0,
        0 | ACV_RND_2 | ACV_RND_1,
        0.000
    },
    // 6
    {
        "~RND_1&RND_2",
        0 | ACV_RND_2,
        0 | ACV_RND_2 | ACV_RND_1,
        0.000
    },
    // 7
    {
        "RND_1&RND_2",
        0 | ACV_RND_2 | ACV_RND_1,
        0 | ACV_RND_2 | ACV_RND_1,
        0.000
    },
    // 8
    {
        "~RND_1&~RND_2",
        0,
        0 | ACV_RND_2 | ACV_RND_1,
        0.000
    },
    // 9
    {
        "RND_1&~RND_2",
        0 | ACV_RND_1,
        0 | ACV_RND_2 | ACV_RND_1,
        0.000
    },
    // 10
    {
        "~RND_1&RND_2",
        0 | ACV_RND_2,
        0 | ACV_RND_2 | ACV_RND_1,
        0.000
    },
    // 11
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.000
    },
    // 12
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.000
    },
    // 13
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.000
    },
    // 14
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.000
    },
    // 15
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.000
    },
    // 16
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.000
    },
    // 17
    {
        "~PLAYING_ANIM",
        0,
        0 | ACV_PLAYING_ANIM,
        0.000
    },
    // 18
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
    ACS_ATTACK1,
    ACS_ATTACK3,
    ACS_ATTACK_CROUCH1,
    ACS_ATTACK_CROUCH2,
    ACS_ATTACK2,
    ACS_ATTACK4,
    ACS_ATTACK_CROUCH4,
    ACS_ATTACK_CROUCH3,

    ACS_NUM_STATES,
    ACS_MAX_STATES = 0xffffffff
};

//-----------------------------------------------------------------------------
static CFEString gssAnimations[ACS_NUM_STATES] =
{
    "",
    "",
    "",
    "",
    "attack",
    "attack2",
    "attack_crouch",
    "attack_crouch1",
    "attack1",
    "attack3",
    "attack_crouch",
    "attack_crouch2",
};

//-----------------------------------------------------------------------------
static int gsiStateTable[ACS_NUM_STATES*ACS_NUM_STATES] =
{
    -1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,-1,2,1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,-1,-1,-1,3,6,-1,-1,4,5,-1,-1,
    -1,-1,-1,-1,-1,-1,7,9,-1,-1,8,10,
    -1,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
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
