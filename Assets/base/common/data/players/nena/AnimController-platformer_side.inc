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

const uint NUM_TRANSITIONS = 36;
//-----------------------------------------------------------------------------
static TTransitionData gsoTransitions[] =
{
    {"",0,0,0.0f},
    // 1
    {
        "WALK",
        0 | ACV_WALK,
        0 | ACV_WALK,
        0.250
    },
    // 2
    {
        "CROUCH",
        0 | ACV_CROUCH,
        0 | ACV_CROUCH,
        0.250
    },
    // 3
    {
        "JUMP_UP",
        0 | ACV_JUMP_UP,
        0 | ACV_JUMP_UP,
        0.000
    },
    // 4
    {
        "IN_LADDER",
        0 | ACV_IN_LADDER,
        0 | ACV_IN_LADDER,
        0.000
    },
    // 5
    {
        "PAIN",
        0 | ACV_PAIN,
        0 | ACV_PAIN,
        0.000
    },
    // 6
    {
        "IN_CHECKPOINT",
        0 | ACV_IN_CHECKPOINT,
        0 | ACV_IN_CHECKPOINT,
        0.500
    },
    // 7
    {
        "JUMP_DOWN",
        0 | ACV_JUMP_DOWN,
        0 | ACV_JUMP_DOWN,
        0.250
    },
    // 8
    {
        "PAIN",
        0 | ACV_PAIN,
        0 | ACV_PAIN,
        0.000
    },
    // 9
    {
        "IN_LADDER",
        0 | ACV_IN_LADDER,
        0 | ACV_IN_LADDER,
        0.000
    },
    // 10
    {
        "PAIN",
        0 | ACV_PAIN,
        0 | ACV_PAIN,
        0.000
    },
    // 11
    {
        "~JUMP_DOWN",
        0,
        0 | ACV_JUMP_DOWN,
        0.125
    },
    // 12
    {
        "IN_LADDER",
        0 | ACV_IN_LADDER,
        0 | ACV_IN_LADDER,
        0.000
    },
    // 13
    {
        "JUMP_UP",
        0 | ACV_JUMP_UP,
        0 | ACV_JUMP_UP,
        0.125
    },
    // 14
    {
        "~WALK",
        0,
        0 | ACV_WALK,
        0.250
    },
    // 15
    {
        "PAIN",
        0 | ACV_PAIN,
        0 | ACV_PAIN,
        0.000
    },
    // 16
    {
        "FALL",
        0 | ACV_FALL,
        0 | ACV_FALL,
        0.250
    },
    // 17
    {
        "CROUCH",
        0 | ACV_CROUCH,
        0 | ACV_CROUCH,
        0.250
    },
    // 18
    {
        "IN_LADDER",
        0 | ACV_IN_LADDER,
        0 | ACV_IN_LADDER,
        0.000
    },
    // 19
    {
        "WALK",
        0 | ACV_WALK,
        0 | ACV_WALK,
        0.500
    },
    // 20
    {
        "~CROUCH",
        0,
        0 | ACV_CROUCH,
        0.250
    },
    // 21
    {
        "PAIN",
        0 | ACV_PAIN,
        0 | ACV_PAIN,
        0.000
    },
    // 22
    {
        "~CROUCH",
        0,
        0 | ACV_CROUCH,
        0.250
    },
    // 23
    {
        "~WALK",
        0,
        0 | ACV_WALK,
        0.500
    },
    // 24
    {
        "PAIN",
        0 | ACV_PAIN,
        0 | ACV_PAIN,
        0.000
    },
    // 25
    {
        "FALL",
        0 | ACV_FALL,
        0 | ACV_FALL,
        0.250
    },
    // 26
    {
        "PAIN_LANDED",
        0 | ACV_PAIN_LANDED,
        0 | ACV_PAIN_LANDED,
        0.250
    },
    // 27
    {
        "DIE",
        0 | ACV_DIE,
        0 | ACV_DIE,
        0.750
    },
    // 28
    {
        "~PAIN_LANDED",
        0,
        0 | ACV_PAIN_LANDED,
        0.250
    },
    // 29
    {
        "~FALL",
        0,
        0 | ACV_FALL,
        0.000
    },
    // 30
    {
        "~PLAYING_ANIM&~WALK&CROUCH",
        0 | ACV_CROUCH,
        0 | ACV_CROUCH | ACV_WALK | ACV_PLAYING_ANIM,
        0.250
    },
    // 31
    {
        "~PLAYING_ANIM&WALK&CROUCH",
        0 | ACV_CROUCH | ACV_WALK,
        0 | ACV_CROUCH | ACV_WALK | ACV_PLAYING_ANIM,
        0.250
    },
    // 32
    {
        "~PLAYING_ANIM&~WALK&~CROUCH",
        0,
        0 | ACV_CROUCH | ACV_WALK | ACV_PLAYING_ANIM,
        0.250
    },
    // 33
    {
        "~PLAYING_ANIM&WALK&~CROUCH",
        0 | ACV_WALK,
        0 | ACV_CROUCH | ACV_WALK | ACV_PLAYING_ANIM,
        0.250
    },
    // 34
    {
        "~IN_LADDER&FALL",
        0 | ACV_FALL,
        0 | ACV_FALL | ACV_IN_LADDER,
        0.000
    },
    // 35
    {
        "~IN_LADDER&~FALL",
        0,
        0 | ACV_FALL | ACV_IN_LADDER,
        0.000
    },
    // 36
    {
        "~IN_CHECKPOINT",
        0,
        0 | ACV_IN_CHECKPOINT,
        0.250
    },
};
//-----------------------------------------------------------------------------
enum EAnimControllerState
{
    ACS_NONE,

    ACS_IDLE,
    ACS_JUMP_UP,
    ACS_JUMP_DOWN,
    ACS_WALK,
    ACS_CROUCH,
    ACS_WALK_CROUCH,
    ACS_PAIN,
    ACS_PAIN_LAND,
    ACS_FALL,
    ACS_FALL_LAND,
    ACS_IN_LADDER,
    ACS_DYING,
    ACS_IN_CHECKPOINT,

    ACS_NUM_STATES,
    ACS_MAX_STATES = 0xffffffff
};

//-----------------------------------------------------------------------------
static CFEString gssAnimations[ACS_NUM_STATES] =
{
    "",
    "idle",
    "jump_up",
    "jump_down",
    "walk",
    "crouch",
    "walk_crouch",
    "pain_jump",
    "pain_land",
    "fall",
    "fall_land",
    "idle",
    "dead",
    "check_point",
};

//-----------------------------------------------------------------------------
static int gsiStateTable[ACS_NUM_STATES*ACS_NUM_STATES] =
{
    -1,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,-1,3,-1,1,2,-1,5,-1,-1,-1,4,-1,6,
    -1,-1,-1,7,-1,-1,-1,8,-1,-1,-1,9,-1,-1,
    -1,-1,-1,-1,-1,-1,-1,10,-1,-1,11,12,-1,-1,
    -1,14,13,-1,-1,-1,17,15,-1,16,-1,18,-1,-1,
    -1,20,-1,-1,-1,-1,19,21,-1,-1,-1,-1,-1,-1,
    -1,-1,-1,-1,22,23,-1,24,-1,25,-1,-1,-1,-1,
    -1,-1,-1,-1,-1,-1,-1,-1,26,-1,-1,-1,27,-1,
    -1,28,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,29,-1,-1,-1,
    -1,32,-1,-1,33,30,31,-1,-1,-1,-1,-1,-1,-1,
    -1,35,-1,-1,-1,-1,-1,-1,-1,34,-1,-1,-1,-1,
    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
    -1,36,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,
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
