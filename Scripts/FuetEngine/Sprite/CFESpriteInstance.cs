using Godot;
using CFEVect2 = Godot.Vector2;

namespace FuetEngine
{
	[Tool]
	
	public class CFESpriteInstance : Node2D
	{
		// --------------------------------------------------------------------
		/// Speed multiplier for this instance.
		public float m_rSpeedMult = 1.0f;
		// ------------------------------------
		/// Current status of the sprite, i.e. animation and other things.
		// ------------------------------------
		/// Moment in the sequence related to the current action.
		public float m_rActionTime = 0.0f;
		/// Current frame in the action.
		public int m_uiCurrentActionFrame = 0;
		public CFESpriteAction m_curAction = null;		
		// ------------------------------------
		/// To speedup things
		private bool m_actionHasOneFrame = false;		
		private Vector2[] m_oVX = new Vector2[4];
		private Vector2[] m_oUV = new Vector2[4];
		private Color[] m_oVC = new Color[4];

		[Export]
		private CFESprite m_sprite = null;
		[Export]
		public int Action {	set	{ SetAction(value);	} get{return m_iCurrentAction;} }
		private  int m_iCurrentAction = -1;
		// --------------------------------------------------------------------
		// Make sure you provide a parameterless constructor.
		public CFESpriteInstance()
		{
		}
		// --------------------------------------------------------------------
		public bool bInitialized()
		{
			return m_sprite != null;
		}
		// --------------------------------------------------------------------
		public void Init(CFESprite _sprite)
		{
			m_sprite = _sprite;
			Init();
		}
		// --------------------------------------------------------------------
		public void Init()
		{		
			// Connect("draw", OnDrawSignal);

			if (m_sprite != null)
			{
				SetAction(0);
			}
		}
		// --------------------------------------------------------------------
		public void SetAction(int _iAction)
		{
			m_iCurrentAction = _iAction;

			if (_iAction == -1) return;
			if (m_sprite == null) return;

			CFESpriteAction action = m_sprite.GetAction(_iAction);
			if (action == null) return;
				
			m_curAction = action;
			m_uiCurrentActionFrame = 0;

			CFESpriteInstUpdater.SetCurrentActionTime(this, 0.0f);
			m_actionHasOneFrame = (m_curAction.GetNumberOfFrames() == 1);
			
			SetFrameToSprite(m_curAction.GetFrame(0));
		}
		// --------------------------------------------------------------------        
		public void SetAction(string _sAction)
		{
			if (m_sprite == null) return;
			SetAction(m_sprite.iGetActionIdx(_sAction));
		}
		// --------------------------------------------------------------------
		private void SetFrameToSprite(CFESpriteFrame _frame)
		{
			GenerateSpriteGeometry(_frame);
		}
		// --------------------------------------------------------------------
		public override void _Ready()
		{
			Init();
		}
		// --------------------------------------------------------------------
		public override void _Process(float _deltaT)
		{			
			if ((m_sprite == null) || m_actionHasOneFrame || (m_curAction == null))
			{
				Update();
			}
			else
			{
				CFESpriteFrame oldFrame = m_curAction.GetFrame(m_uiCurrentActionFrame);
				CFESpriteInstUpdater.Update(this, _deltaT);
				CFESpriteFrame currentFrame = m_curAction.GetFrame(m_uiCurrentActionFrame);

				if ((currentFrame != null) && (oldFrame != currentFrame))
				{
					SetFrameToSprite(currentFrame);
				}
			
				Update();
			}
		}
		// ----------------------------------------------------------------------------		
		private void OnDrawSignal()
		{
			Render();
		}
		// ----------------------------------------------------------------------------		
		public override void _Draw()
		{
			Render();
		}
		// ----------------------------------------------------------------------------
		public void GenerateSpriteGeometry(CFESpriteFrame _oFrame)
		{
			m_oVC[0] = m_oVC[1] = m_oVC[2] = m_oVC[3] = new Color(1.0f,1.0f,1.0f,1.0f);

			CFEVect2 oPivot = (new CFEVect2(0.5f, 0.5f) - _oFrame.m_oPivot);
			
			m_oVX[0]   = new CFEVect2(-0.5f,-0.5f);
			m_oVX[2]   = new CFEVect2( 0.5f, 0.5f);
			m_oVX[1].x = m_oVX[2].x;
			m_oVX[1].y = m_oVX[0].y;
			m_oVX[3].x = m_oVX[0].x;
			m_oVX[3].y = m_oVX[2].y;

			for (int i=0; i<4; i++)
			{
				m_oVX[i] += oPivot;

				m_oVX[i].x *= _oFrame.m_oSize.x;
				m_oVX[i].y *= _oFrame.m_oSize.y;
			}

			// Normal path / Scale Equally XU / YV ...
			if ((_oFrame.m_bUWorldCoords  == false) && (_oFrame.m_bVWorldCoords  == false))
			{
				CFEVect2 oIUV = _oFrame.m_oUVIni;
				CFEVect2 oFUV = _oFrame.m_oUVEnd;

				if (_oFrame.m_bScaleXUEqually)
				{
					oIUV.x *= GlobalScale.x;
					oFUV.x *= GlobalScale.x;
				}
				if (_oFrame.m_bScaleYVEqually)
				{
					oIUV.y *= GlobalScale.y;
					oFUV.y *= GlobalScale.y;
				}

				m_oUV[0]   = oIUV;
				m_oUV[2]   = oFUV;
				m_oUV[1].x = m_oUV[2].x;
				m_oUV[1].y = m_oUV[0].y;
				m_oUV[3].x = m_oUV[0].x;
				m_oUV[3].y = m_oUV[2].y;
			}

			// UVProject on only one axis
			else /*  if ((_rAngle==_0r) || (_poFrame->m_bUWorldCoords != _poFrame->m_bVWorldCoords)) */ 
			{
				/*
				CFEVect2 oPivot = (CFEVect2(_05r,_05r) - _poFrame->m_oPivot);

				CFEVect2 oIUV = CFEVect2(-_05r,-_05r);
				oIUV += oPivot;
				oIUV *= _oScale;
				oIUV *= _poFrame->m_oSize;
				oIUV += _oPos;						// these are world coords (without transform)
				oIUV *= _poFrame->m_oUV.m_oEnd;

				CFEVect2 oFUV = CFEVect2(_05r,_05r);
				oFUV += oPivot;
				oFUV *= _oScale;
				oFUV *= _poFrame->m_oSize;
				oFUV += _oPos;						// these are world coords (without transform)
				oFUV *= _poFrame->m_oUV.m_oEnd;
				*/
				
				/*
				V   = (_05r,_05r);
				PIV' = V - PIV
				
				I = ((-V + PIV')*SCL*SZ + POS)*ITD 
				F = (( V + PIV')*SCL*SZ + POS)*ITD
				
				I = ((-V + V - PIV)*SCL*SZ + POS)*ITD =>
				F = (( V + V - PIV)*SCL*SZ + POS)*ITD =>

				I = ((-PIV)*SCL*SZ + POS)*ITD = (-PIV*SCL*SZ + POS)*ITD = oIUV
				F = (( 2V - PIV)*SCL*SZ + POS)*ITD =((ONE - PIV)*SCL*SZ + POS)*ITD => (SCL*SZ -PIV*SCL*SZ + POS)*ITD =  (SCL*SZ*ITD) + (-PIV*SCL*SZ + POS)*ITD = (SCL*SZ*ITD) + oIUV
				*/

				CFEVect2 oMult = _oFrame.m_oSize * GlobalScale;

				CFEVect2 oIUV = _oFrame.m_oPivot;
				oIUV *= (-1.0f);
				oIUV *= oMult;
				oIUV += GlobalPosition;
				oIUV *= _oFrame.m_o1OverDims;

				CFEVect2 oFUV = oIUV;
				oMult *= _oFrame.m_o1OverDims;
				oFUV += oMult;
				
				if (_oFrame.m_bUWorldCoords == false)
				{
					oIUV.x = _oFrame.m_oUVIni.x;
					oFUV.x = _oFrame.m_oUVEnd.x;
					
					if (_oFrame.m_bScaleXUEqually)
					{
						oIUV.x *= GlobalScale.x;
						oFUV.x *= GlobalScale.x;
					}
				}
				else if (_oFrame.m_bVWorldCoords == false)
				{
					oIUV.y = _oFrame.m_oUVIni.y;
					oFUV.y = _oFrame.m_oUVEnd.y;

					if (_oFrame.m_bScaleYVEqually)
					{
						oIUV.y *= GlobalScale.y;
						oFUV.y *= GlobalScale.y;
					}
				}

				m_oUV[0]   = oIUV;
				m_oUV[2]   = oFUV;
				m_oUV[1].x = m_oUV[2].x;
				m_oUV[1].y = m_oUV[0].y;
				m_oUV[3].x = m_oUV[0].x;
				m_oUV[3].y = m_oUV[2].y;
			}
		}
		// --------------------------------------------------------------------
		private void Render()
		{
			if (m_curAction == null)
			{
				return;
			}

			// Material BlendMode
			// Special cases: (_poInstance->m_rActionTime  <= _0r) is a specific case of the one treated on SafeRenderFrame
			if ((m_curAction.m_eBlendMode == EFEBlendMode.BM_NONE) || (m_curAction.m_eBlendMode == EFEBlendMode.BM_COPY)|| m_actionHasOneFrame || (m_curAction.m_rActionTime <= 0.0f))
			{
				// _poRenderer->SetDepth(_rDepth);
				RenderSpriteFrame(m_curAction.GetFrame(m_uiCurrentActionFrame), 1.0f);
				return;
			}

			// Render the frame
			switch (m_curAction.m_ePlayMode)
			{
				// ------------------------------------------------
				case ESFSPlayMode.SFSPM_NONE:
				case ESFSPlayMode.SFSPM_ONESHOT:
				{
					SafeRenderFrame(m_uiCurrentActionFrame, m_uiCurrentActionFrame);
				}
				break;
				// ------------------------------------------------
				case ESFSPlayMode.SFSPM_LOOP:
				{
					SafeRenderFrame(m_uiCurrentActionFrame, m_uiCurrentActionFrame);
				}
				break;
				// ------------------------------------------------
				case ESFSPlayMode.SFSPM_PINGPONGSTOP:
				{
					// #pragma message("TO BE IMPLEMENTED")
				}
				break;
				case ESFSPlayMode.SFSPM_PINGPONG:
				{
					// #pragma message("TO BE IMPLEMENTED")
				}
				break;
			}
		}
		// --------------------------------------------------------------------
		private void SafeRenderFrame(int _iFrame, float _rActionTime)
		{
			uint uiLoops = (uint)(_rActionTime / m_curAction.m_rActionTime);
			float rTime = _rActionTime - ((float)(uiLoops)*m_curAction.m_rActionTime);

			CFESpriteFrame oSF = m_curAction.GetFrame(_iFrame);
			float rRelTime  = rTime - oSF.m_rStartTime;
			int iNextFrame = m_curAction.uiNextFrame(_iFrame);

			// Sets the rendering depth.
			// _poRenderer->SetDepth(_rDepth);

			if (rRelTime <= oSF.m_rDelay)
			{
				// Render only the current frame.
				RenderSpriteFrame(oSF, 1.0f);
			}
			else
			{
				// Blend factor between frames.	
				float rFact = (rRelTime - oSF.m_rDelay) / oSF.m_rBlendTime;
				RenderSpriteFrames(m_curAction.GetFrame(_iFrame), m_curAction.GetFrame(iNextFrame), rFact);
			}
		}
		// --------------------------------------------------------------------
		private void RenderSpriteFrames(CFESpriteFrame _oSrc, CFESpriteFrame _oDst, float _rFactor)
		{
			float rSFactor = Mathf.Clamp(2.0f*(1.0f-_rFactor), 0.0f ,1.0f);
			float rDFactor = Mathf.Clamp(2.0f*(_rFactor),0.0f, 1.0f);

			RenderSpriteFrame(_oSrc, rSFactor);
			RenderSpriteFrame(_oDst, rDFactor);
		}
		// --------------------------------------------------------------------
		private void RenderSpriteFrame(CFESpriteFrame _oFrame, float _alpha)
		{
			DrawPrimitive( m_oVX, m_oVC, m_oUV, _oFrame.m_hMaterial);
		}
		// --------------------------------------------------------------------
	}
}
