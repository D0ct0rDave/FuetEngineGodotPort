using Godot;

namespace FuetEngine
{
    public class CFESpriteInstUpdater
	{
		private static RandomNumberGenerator m_rng = null;
		// --------------------------------------------------------------------
		public static void SetCurrentActionTime(CFESpriteInstance _poSI, float _rActionTime)
		{
			CFESpriteAction action = _poSI.m_curAction;
			float rNewTime = _rActionTime;

			// To prevent overflows
			switch (action.eGetPlayMode())
			{
				case ESFSPlayMode.SFSPM_ONESHOT:
				{
					rNewTime = Mathf.Min(rNewTime, action.rGetMaxActionTime());
				}
				break;
				
				case ESFSPlayMode.SFSPM_LOOP:
				{
					if (action.m_rRandStartTime > 0.0f)
					{
						rNewTime = rNewTime % action.rGetMaxActionTime();

						//loop detected
						if (rNewTime < _rActionTime)
						{
							if (m_rng == null)
							{
								m_rng = new RandomNumberGenerator();
								m_rng.Randomize();
							}
							
							rNewTime = rNewTime + m_rng.Randf() * action.m_rRandStartTime;
						}
					}
					else
					{
						rNewTime = rNewTime % action.rGetMaxActionTime();
					}	
				}
				break;
				
				case ESFSPlayMode.SFSPM_PINGPONGSTOP:
				{
					rNewTime = Mathf.Min(rNewTime, 2.0f * action.rGetMaxActionTime () + 0.01f);
				}
				break;
				
				case ESFSPlayMode.SFSPM_PINGPONG:
				{
					rNewTime = rNewTime %(2.0f*action.rGetMaxActionTime());
				}
				break;
			}

			_poSI.m_rActionTime = rNewTime;
			_poSI.m_uiCurrentActionFrame = action.uiGetFrame(rNewTime, _poSI.m_uiCurrentActionFrame);
		}
		// --------------------------------------------------------------------
		// Update is called once per frame
		public static void Update(CFESpriteInstance _poSI, float _rDeltaT) 
		{
			SetCurrentActionTime(_poSI,_poSI.m_rActionTime+_rDeltaT*_poSI.m_rSpeedMult);
		}
		// --------------------------------------------------------------------
	}
}
