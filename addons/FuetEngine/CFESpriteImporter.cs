#if TOOLS
using Godot;
using System;
using System.Collections.Generic;
using FuetEngine;
using Sprite2D = Godot.Sprite;

public partial class FESpriteImporter
{
	// ------------------------------------------------------------------------
	public static Node2D Import(string _sFilename)
	{
		string filenameWithoutExtension = _sFilename.Substr(0,_sFilename.Length - 4); 
		CFESprite sprite = CFESpriteLoader.poLoad(filenameWithoutExtension, false);

		if ((sprite == null) || (sprite.m_oActions == null) || (sprite.m_oActions.Count == 0))
		{
			return null;
		}
		
		if ((sprite.m_oActions.Count > 1) || (sprite.m_oActions[0].m_oSeq.Count > 1))
		{
			return BuildAnimatedSprite(sprite);
		}
		else
		{
			return BuildSimpleSprite(sprite);
		}
	}
	// ------------------------------------------------------------------------
	private static bool AllActionsAndFramesHaveSamePivot(CFESprite _sprite)
	{
		Vector2 pivotToCheck = new Vector2();
		bool checkPivot = false;
		
		foreach(CFESpriteAction action in _sprite.m_oActions)
		{
			foreach(CFESpriteFrame frame in action.m_oSeq)
			{
				if (checkPivot)
				{
					pivotToCheck = frame.m_oPivot;
				}
				else
				{
					if (frame.m_oPivot == pivotToCheck)
					{
						return false;
					}
				}
			}
		}

		return true;
	}
	// ------------------------------------------------------------------------
	private static bool AllFramesHaveSameSpeed(CFESpriteAction _spriteAction)
	{
		float frameTimeToCheck = 0.0f;
		bool checkFrameTime = false;
		
		foreach(CFESpriteFrame frame in _spriteAction.m_oSeq)
		{
			if (checkFrameTime)
			{
				frameTimeToCheck = frame.m_rFrameTime;
			}
			else
			{
				if (frame.m_rFrameTime != frameTimeToCheck)
				{
					return false;
				}
			}
		}

		return true;
	}
	// ------------------------------------------------------------------------
	private static Rect2 GetRectFromSpriteFrame(CFESpriteFrame _spriteFrame, float _texWidth, float _texHeight)
	{
		Rect2 rect = new Rect2();
		rect.Position = new Vector2(_spriteFrame.m_oUV.m_oIni.x*_texWidth, 
									_spriteFrame.m_oUV.m_oIni.y*_texHeight);

		rect.Size = new Vector2((_spriteFrame.m_oUV.m_oEnd.x - _spriteFrame.m_oUV.m_oIni.x)*_texWidth,
							    (_spriteFrame.m_oUV.m_oEnd.y - _spriteFrame.m_oUV.m_oIni.y)*_texHeight);

		return rect;
	}
	// ------------------------------------------------------------------------
	private static void AddAction(ref SpriteFrames _spriteFrames, CFESpriteAction _spriteAction)
	{
		_spriteFrames.AddAnimation(_spriteAction.sGetName());
		
		// speed
		if (AllFramesHaveSameSpeed(_spriteAction) && (_spriteAction.m_oSeq.Count > 0))
		{
			float fps = 5.0f;
			if (_spriteAction.m_oSeq[0].m_rFrameTime > 0.0f)
			{
				fps = 1.0f / _spriteAction.m_oSeq[0].m_rFrameTime;
			}

			_spriteFrames.SetAnimationSpeed(_spriteAction.sGetName(), fps);
		}

		// warp mode
		if (_spriteAction.eGetPlayMode() == FuetEngine.ESFSPlayMode.SFSPM_LOOP)
		{
			_spriteFrames.SetAnimationLoop(_spriteAction.sGetName(), true);
		}

		// set frames
		foreach(CFESpriteFrame frame in _spriteAction.m_oSeq)
		{
			AddActionFrame(ref _spriteFrames, _spriteAction.sGetName(), frame);
		}
	}
	// ------------------------------------------------------------------------
	private static void AddActionFrame(ref SpriteFrames _spriteFrames, string _animation, CFESpriteFrame _spriteFrame)
	{
		string textureFilename = _spriteFrame.m_hMaterial + ".png";
		Texture texture = ResourceLoader.Load<Texture>(textureFilename);
		if (texture == null)
		{
			return;
		}
		
		// texture.ResourceLocalToScene = true;

		AtlasTexture atlasTexture = new AtlasTexture();
		atlasTexture.Atlas = texture;
		atlasTexture.ResourceLocalToScene = true;

		atlasTexture.Region = GetRectFromSpriteFrame(_spriteFrame, texture.GetWidth(), texture.GetHeight());

		_spriteFrames.AddFrame(_animation, atlasTexture);
	}
	// ------------------------------------------------------------------------
	private static Node2D BuildAnimatedSprite(CFESprite _sprite)
	{
		Node2D node = new Node2D();
		node.Name = _sprite.sGetName();

		SpriteFrames spriteFrames = new SpriteFrames();
		spriteFrames.ResourceLocalToScene = true;
		spriteFrames.RemoveAnimation("default");

		foreach(CFESpriteAction action in _sprite.m_oActions)
		{
			AddAction(ref spriteFrames, action);
		}

		AnimatedSprite animatedSprite = new AnimatedSprite();
		animatedSprite.Frames = spriteFrames;
		animatedSprite.Name = _sprite.sGetName();
		animatedSprite.Owner = node;
		animatedSprite.Animation = _sprite.m_oActions[0].sGetName();
		
		if (AllActionsAndFramesHaveSamePivot(_sprite))
		{
			// spriteNode.Offset = new Vector2(spriteFrame.m_oPivot.x*texWidth, spriteFrame.m_oPivot.y*texHeight);
		}

		node.AddChild(animatedSprite);
		animatedSprite.Owner = node; // always after addchild

		return node;
	}
	// ------------------------------------------------------------------------	
	private static Node2D BuildSimpleSprite(CFESprite _sprite)
	{
		if ((_sprite == null) || (_sprite.m_oActions.Count > 0) || (_sprite.m_oActions[0].m_oSeq.Count > 0))
		{
			return null;
		}
		
		CFESpriteAction spriteAction = _sprite.m_oActions[0];
		CFESpriteFrame spriteFrame = spriteAction.m_oSeq[0];

		string textureFilename = spriteFrame.m_hMaterial + ".png";
		Texture texture = ResourceLoader.Load<Texture>(textureFilename);
		if (texture == null)
		{
			return null;
		}

		Node2D node = new Node2D();
		node.Name = _sprite.sGetName();

		Sprite2D spriteNode = new Sprite2D();
		spriteNode.Texture = texture;
		spriteNode.RegionEnabled = true;
		
		float texWidth = texture.GetWidth();
		float texHeight = texture.GetHeight();

		Rect2 rect = GetRectFromSpriteFrame(spriteFrame, texWidth, texHeight);
		spriteNode.RegionRect = rect;
		spriteNode.Offset = new Vector2(spriteFrame.m_oPivot.x*texWidth, spriteFrame.m_oPivot.y*texHeight);
		spriteNode.Scale = new Vector2(spriteFrame.m_oSize.x / rect.Size.x, spriteFrame.m_oSize.y / rect.Size.y);

		node.AddChild(spriteNode);
		spriteNode.Owner = node;

		return node;
	}
	// ------------------------------------------------------------------------
}
#endif
