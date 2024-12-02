#if TOOLS
using Godot;
using System;
using System.Net;
using FuetEngine;

public partial class FESpriteImporter
{
	public static Node2D Import(string _sFilename)
	{
		string filenameWithoutExtension = _sFilename.Substr(0,_sFilename.Length - 4); 
		CFESprite sprite = CFESpriteLoader.poLoad(filenameWithoutExtension, false);

		// Node2D node = new Node2D();
		// node.Name = sprite.sGetName();

		SpriteFrames spriteFrames = new SpriteFrames();
		spriteFrames.ResourceLocalToScene = true;
		
		foreach(CFESpriteAction action in sprite.m_oActions)
		{
			AddAction(ref spriteFrames, action);
		}

		AnimatedSprite animatedSprite = new AnimatedSprite();
		animatedSprite.Frames = spriteFrames;
		animatedSprite.Name = sprite.sGetName();
		return animatedSprite;

		// node.AddChild(animatedSprite);
		// return node;
	}

	private static void AddAction(ref SpriteFrames _spriteFrames, CFESpriteAction _spriteAction)
	{
		foreach(CFESpriteFrame frame in _spriteAction.m_oSeq)
		{
			AddActionFrame(ref _spriteFrames, _spriteAction.sGetName(), frame);
		}
	}

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

		float width = texture.GetWidth();
		float height = texture.GetHeight();

		Rect2 rect = new Rect2();
		rect.Position = new Vector2(_spriteFrame.m_oUV.m_oIni.x*width, 
									_spriteFrame.m_oUV.m_oIni.y*height);

		rect.Size = new Vector2((_spriteFrame.m_oUV.m_oEnd.x - _spriteFrame.m_oUV.m_oIni.x)*width,
							    (_spriteFrame.m_oUV.m_oEnd.y - _spriteFrame.m_oUV.m_oIni.y)*height);

		atlasTexture.Region = rect;

		_spriteFrames.AddFrame(_animation, atlasTexture);
	}
}
#endif
