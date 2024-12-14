using System;
using System.Diagnostics;
using Godot;
//-----------------------------------------------------------------------------
using CFEString = System.String;
using FEReal = System.Single;
using CFEVect2 = Godot.Vector2;
using CFEColor = Godot.Color;
using CFEFont = Godot.Theme;
using System.Drawing.Printing;
using System.Collections.Generic;
//-----------------------------------------------------------------------------
namespace FuetEngine
{
	// ------------------------------------------------------------------------
	// Keyframe based functions : Interpolation between keys method
	public enum EFEKFLerpFunc
	{
		KFLF_NONE,

		KFLF_CONSTANT,	/// To retrieve constant values until the next frame.
		KFLF_LERP,		/// To perform linear interpolation between this frame and the next.
		KFLF_SIN,		/// To perform sinusoidal interpolation between this frame and the next.
		KFLF_EXP,		/// To perform logarithmic interpolation between this frame and the next.
		KFLF_RAND,		/// To retrieve random values between the interval limits.
		KFLF_SINSIN,	/// To perform sinusoidal interpolation between this frame and the next.
		KFLF_EXPLOG,	/// To perform logarithmic interpolation between this frame and the next.
		KFLF_ISIN,      /// To perform inverse sinusoidal interpolation between this frame and the next.

		KFLF_NUM,
		KFLF_MAX
	}
	// ------------------------------------------------------------------------
	// Keyframe based functions : wrapping method

	public enum EFEKFBFuncWrapMode
	{
		KFBFWM_NONE,

		KFBFWM_LOOP,			/// To starts from the begining.
		KFBFWM_PINGPONG,		/// To performs a pingpong over the function.
		KFBFWM_FINALVALUE,		/// To repeat the last value.
		KFBFWM_INITIALVALUE,	/// To repeat the initial value.

		KFBFWM_NUM,
		KFBFWM_MAX
	}
	// ------------------------------------------------------------------------
	// Text rendering enums
	/// Horizontal alignment modes to draw text
	public enum EFETextHAlignmentMode
	{
		THAM_NONE,
		THAM_LEFT,
		THAM_CENTER,
		THAM_RIGHT,
		THAM_NUM_MODES,
		THAM_MAX,
		THAM_DEFAULT = THAM_LEFT
	}
	// ------------------------------------------------------------------------
	/// Vertical alignment modes to draw text
	public enum EFETextVAlignmentMode
	{
		TVAM_NONE,
		TVAM_TOP,
		TVAM_CENTER,
		TVAM_BOTTOM,
		TVAM_NUM_MODES,
		TVAM_MAX,
		TVAM_DEFAULT = TVAM_TOP
	}
	// ------------------------------------------------------------------------
	/// Rendering Blending modes
	public enum EFEBlendMode
	{
		BM_NONE,
		
		BM_COPY,
		BM_ALPHA,
		BM_ADD,
		BM_ALPHAADD,
		BM_MULT,
		
		BM_FOG,
		BM_FOGADD,
		
		BM_MAGICMARKER,
		BM_LIGHTMARKER,
		BM_LIGHTSABER,
		BM_REVEAL,
		BM_LUMISHADE_REVEAL,
		
		BM_NUM_MODES,
		BM_DEFAULT = BM_ALPHA
	}
	// ------------------------------------------------------------------------
    [Tool]
	public static class Support
	{
		public const string SPRITE_SCRIPT_FILE = "res://Scripts/FuetEngine/Sprite/CFESprite.cs";
		public const string SPRITE_INSTANCE_SCRIPT_FILE = "res://Scripts/FuetEngine/Sprite/CFESpriteInstance.cs";
		public const string SPRITE_FRAME_SCRIPT_FILE = "res://Scripts/FuetEngine/Sprite/CFESpriteFrame.cs";
		public const string SPRITE_ACTION_SCRIPT_FILE = "res://Scripts/FuetEngine/Sprite/CFESpriteAction.cs";
		public const string HUD_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUD.cs";
		public const string HUD_ELEMENT_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDElement.cs";
		public const string HUD_OBJECT_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDObject.cs";
		public const string HUD_GROUP_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDGroup.cs";
		public const string HUD_ICON_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDIcon.cs";
		public const string HUD_LABEL_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDLabel.cs";
		public const string HUD_SHAPE_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDShape.cs";
		public const string HUD_PSYS_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDPSys.cs";
		public const string HUD_RECT_SCRIPT_FILE = "res://Scripts/FuetEngine/HUD/CFEHUDRect.cs";
		
		// --------------------------------------------------------------------
		private static B _CreateObject<T,B>(Script _script) where T : Godot.Object, new() where B : Godot.Object
		{
			_script.SetupLocalToScene();

			T node = new T();
			var godotObjectId = node.GetInstanceId();
			node.SetScript(_script);
			B finalNode = GD.InstanceFromId(godotObjectId) as B;

			if (finalNode == null)
			{
				GD.Print("Cannot convert back node to class of type " + typeof(B) + ".\n" + 
				"If this class is instanced inside the editor don't forget to mark it as [Tool] !!!");

				Debugger.Break(); // Set breakpoint here
			}

			return finalNode;
		}
		// --------------------------------------------------------------------
		public static T CreateObject<T>(Script _script) where T : Godot.Object, new()
		{
			return _CreateObject<T,T>(_script);
		}
		// --------------------------------------------------------------------
		public static T CreateObject<T>(string _scriptFilename) where T : Node, new()
		{
			// GD.Print("CreateObject("+_scriptFilename+")");			
			return _CreateObject<T,T>(ResourceLoader.Load(_scriptFilename) as Script);
		}
		// --------------------------------------------------------------------
		public static B CreateObject<B,T>(Script _script) where T : Godot.Object, new() where B : Godot.Object
		{
			return _CreateObject<T,B>(_script);
		}
		// --------------------------------------------------------------------
		public static B CreateObject<T,B>(string _scriptFilename) where T : Godot.Object, new() where B : Godot.Object
		{
			// GD.Print("CreateObject("+_scriptFilename+")");
			return _CreateObject<T,B>(ResourceLoader.Load(_scriptFilename) as Script);
		}
		// --------------------------------------------------------------------
		public static void SetObjectEnabled(Node _node, bool _enabled)
		{
			_node.SetPhysicsProcess(_enabled);
			_node.SetPhysicsProcessInternal(_enabled);
			_node.SetProcess(_enabled);
			_node.SetProcessUnhandledInput(_enabled);
			_node.SetProcessUnhandledKeyInput(_enabled);
		}
		// --------------------------------------------------------------------
		public static void SetObjectEnabled(Node2D _node, bool _enabled)
		{
			_node.Visible = _enabled;
			SetObjectEnabled(_node as Node, _enabled);
		}
		// --------------------------------------------------------------------
		public static EFETextHAlignmentMode eGetHAlignFromString(CFEString _sAlign)
		{
			CFEString sAlign = _sAlign.ToLower();

			if (sAlign == "left")
				return(EFETextHAlignmentMode.THAM_LEFT);

		else if (sAlign == "center")
			return(EFETextHAlignmentMode.THAM_CENTER);

		else if (sAlign == "right")
			return(EFETextHAlignmentMode.THAM_RIGHT);

			return(EFETextHAlignmentMode.THAM_LEFT);
		}
		// --------------------------------------------------------------------
		public static EFETextVAlignmentMode eGetVAlignFromString(CFEString _sAlign)
		{
			CFEString sAlign = _sAlign.ToLower();
			
			if (sAlign == "top")
				return(EFETextVAlignmentMode.TVAM_TOP);

		else if (sAlign == "center")
			return(EFETextVAlignmentMode.TVAM_CENTER);

		else if (sAlign == "bottom")
			return(EFETextVAlignmentMode.TVAM_BOTTOM);

			return(EFETextVAlignmentMode.TVAM_CENTER);
		}
		// --------------------------------------------------------------------
		public static EFEKFBFuncWrapMode eGetWrapModeFromString(CFEString _sWrapMode)
		{
			CFEString sWrapMode = _sWrapMode.ToLower();
				if (sWrapMode == "Loop")
					return EFEKFBFuncWrapMode.KFBFWM_LOOP;

			else if (sWrapMode == "PingPong")
					return EFEKFBFuncWrapMode.KFBFWM_PINGPONG;

			else if (sWrapMode == "InitialValue")
					return EFEKFBFuncWrapMode.KFBFWM_INITIALVALUE;

			else if (sWrapMode == "FinalValue")
					return EFEKFBFuncWrapMode.KFBFWM_FINALVALUE;

				return EFEKFBFuncWrapMode.KFBFWM_FINALVALUE;
		}
		// --------------------------------------------------------------------
		public static EFEKFLerpFunc eGetLerpFuncFromString(CFEString _sLerpFunc)
		{
			CFEString sLerpFunc = _sLerpFunc.ToLower();

				if (sLerpFunc == "const")
					return EFEKFLerpFunc.KFLF_CONSTANT;

			else if (sLerpFunc == "linear")
					return EFEKFLerpFunc.KFLF_LERP;

			else if (sLerpFunc == "sin")
					return EFEKFLerpFunc.KFLF_SIN;

			else if (sLerpFunc == "invsin")
					return EFEKFLerpFunc.KFLF_ISIN;

			else if (sLerpFunc == "exp")
					return EFEKFLerpFunc.KFLF_EXP;

			else if (sLerpFunc == "random")
					return EFEKFLerpFunc.KFLF_RAND;

			else if (sLerpFunc == "sinsin")
					return EFEKFLerpFunc.KFLF_SINSIN;

			else if (sLerpFunc == "explog")
					return EFEKFLerpFunc.KFLF_EXPLOG;

				return EFEKFLerpFunc.KFLF_LERP;
		}
		// --------------------------------------------------------------------
		public static Godot.Label.AlignEnum GetGodotLabelAlignment(EFETextHAlignmentMode _hAlignmentMode)
		{
			switch (_hAlignmentMode)
			{
				case EFETextHAlignmentMode.THAM_LEFT:	return Label.AlignEnum.Left;
				case EFETextHAlignmentMode.THAM_CENTER:	return Label.AlignEnum.Center;
				case EFETextHAlignmentMode.THAM_RIGHT:	return Label.AlignEnum.Right;
				
				default:
				return Label.AlignEnum.Left;
				// break;
			}
		}
		//---------------------------------------------------------------------
		public static Godot.Label.VAlign GetGodotLabelVAlignment(EFETextVAlignmentMode _vAlignmentMode)
		{
			switch (_vAlignmentMode)
			{
				case EFETextVAlignmentMode.TVAM_TOP:	return Label.VAlign.Top;
				case EFETextVAlignmentMode.TVAM_CENTER:	return Label.VAlign.Center;
				case EFETextVAlignmentMode.TVAM_BOTTOM:	return Label.VAlign.Bottom;
				
				default:
				return Label.VAlign.Top;
				// break;
			}
		}
		// --------------------------------------------------------------------
		public static Godot.Animation.InterpolationType GetGodotInterpolationType(EFEKFLerpFunc _lerpFunc)
		{
			switch (_lerpFunc)
			{
				case EFEKFLerpFunc.KFLF_CONSTANT:	return Animation.InterpolationType.Nearest;
				case EFEKFLerpFunc.KFLF_LERP:		return Animation.InterpolationType.Linear;
				case EFEKFLerpFunc.KFLF_EXP:		return Animation.InterpolationType.Cubic;
				
				default:
				return Animation.InterpolationType.Cubic;
				// break;
			}
		}

		// ------------------------------------------------------------------------
		public static float GetGodotTransition(EFEKFLerpFunc _lerpFunc)
		{
			switch (_lerpFunc)
			{
				case EFEKFLerpFunc.KFLF_CONSTANT:	return -64.0f;
				case EFEKFLerpFunc.KFLF_LERP:		return 1.0f;
				case EFEKFLerpFunc.KFLF_EXP:		return 4.0f;
				case EFEKFLerpFunc.KFLF_EXPLOG:		return -4.0f;
				case EFEKFLerpFunc.KFLF_SIN:		return 0.5f;
			
				default:
				return 1.0f;
				// break;
			}
		}

		// --------------------------------------------------------------------
		
		
		/*
		public static Godot.Animation.LoopWrap GetGodotInterpolationType(EFEKFBFuncWrapMode _lerpFunc)
		{
			switch (_lerpFunc)
			{
				case EFEKFLerpFunc.KFLF_CONSTANT:	return Animation.InterpolationType.Nearest;
				case EFEKFLerpFunc.KFLF_LERP:		return Animation.InterpolationType.Linear;
				case EFEKFLerpFunc.KFLF_EXP:		return Animation.InterpolationType.Cubic;
				
				default:
				return Animation.InterpolationType.Cubic;
				// break;
			}
		}

		*/
		// --------------------------------------------------------------------
		/*
		public static void MethodToSpecialize<T>(T _value)
		{
			// GD.Print(nameof(T) );
		}
		
		public static void MethodToSpecialize<bool>(T _value)
		{
			GD.Print("Bool");
		}

*/
		// --------------------------------------------------------------------
		public static void ReadKFBFunc(string _sSrcFuncName, string _variableName, string _sDstActionTrackName, Color _defaultValue, CFEConfigFile _oConfigFile, ref Animation _objAction)
		{
			const CFEString DEFAULT_WRAP_MODE = "finalvalue";
			const CFEString DEFAULT_LERP_FUNC = "linear";
	
			EFEKFBFuncWrapMode eFuncWrapMode = Support.eGetWrapModeFromString(_oConfigFile.sGetString(_sSrcFuncName + ".WrapMode", DEFAULT_WRAP_MODE));

			int iKeyFrames = _oConfigFile.iGetInteger(_sSrcFuncName + ".NumKeyFrames", -1);
			// TODO: if (iKeyFrames == 0) iKeyFrames = 1;	// will use default values

			if (iKeyFrames != -1)
			{
				int trackIndex = _objAction.AddTrack(Animation.TrackType.Value);
				_objAction.TrackSetPath(trackIndex, _sDstActionTrackName);

				string sVar = _sSrcFuncName + ".KeyFrame";
				for (int i=0; i<iKeyFrames; i++)
				{
					CFEString sIVar = sVar + i.ToString();

					float r = _oConfigFile.rGetReal(sIVar + ".r",  _defaultValue.r);
					float g = _oConfigFile.rGetReal(sIVar + ".g",  _defaultValue.g);
					float b = _oConfigFile.rGetReal(sIVar + ".b",  _defaultValue.b);
					float a = _oConfigFile.rGetReal(sIVar + ".a",  _defaultValue.a);
					Godot.Color color = new Godot.Color(r, g, b, a);

					FEReal rTime = _oConfigFile.rGetReal(sIVar + ".Time", 0.0f);
					EFEKFLerpFunc lerpMode = Support.eGetLerpFuncFromString(_oConfigFile.sGetString(sIVar + ".LerpFunc", DEFAULT_LERP_FUNC));

					_objAction.TrackInsertKey(trackIndex, rTime, color);
				}

				_objAction.TrackSetInterpolationType(trackIndex, Animation.InterpolationType.Linear);
				_objAction.TrackSetInterpolationLoopWrap(trackIndex, (eFuncWrapMode  == EFEKFBFuncWrapMode.KFBFWM_NONE)? false : true);
			}
		}
		// --------------------------------------------------------------------
		/*
		public static void ReadKFBFunc(string _sPrefix, string _variableName, string _sDstActionTrackName, float _defaultValue, CFEConfigFile _oConfigFile, ref Animation _objAction)
		{
			const CFEString DEFAULT_WRAP_MODE = "finalvalue";
			const CFEString DEFAULT_LERP_FUNC = "linear";

			int trackIndex = -1;
			for (int j=0;j<2;j++)
			{
				string srcFuncName = (j==0) ? _sPrefix + ".XFunc" : _sPrefix + ".YFunc";
				string variableName = (j==0) ? ".x" : ".y";

				EFEKFBFuncWrapMode sFuncWrapMode = Support.eGetWrapModeFromString(_oConfigFile.sGetString(srcFuncName + ".WrapMode", DEFAULT_WRAP_MODE));
				int iKeyFrames = _oConfigFile.iGetInteger(srcFuncName + ".NumKeyFrames", -1);
				// TODO: if (iKeyFrames == 0) iKeyFrames = 1;	// will use default values

				if (iKeyFrames != -1)
				{
					if (trackIndex == -1)
					{
						trackIndex = _objAction.AddTrack(Animation.TrackType.Value);
						_objAction.TrackSetPath(trackIndex, _sDstActionTrackName);
					}

					string sVar = srcFuncName + ".KeyFrame";
					for (int i=0; i<iKeyFrames; i++)
					{
						CFEString sIVar = sVar + i.ToString();
						float value = _oConfigFile.rGetReal(sIVar + variableName,  _defaultValue);

						FEReal rTime = _oConfigFile.rGetReal(sIVar + ".Time", 0.0f);
						EFEKFLerpFunc lerpMode = Support.eGetLerpFuncFromString(_oConfigFile.sGetString(sIVar + ".LerpFunc", DEFAULT_LERP_FUNC));
					
						CFEVect2 position = CFEVect2.Zero;
						int keyIndex = _objAction.TrackFindKey(trackIndex, rTime, true);
						if (keyIndex != -1)
						{
							object o = _objAction.TrackGetKeyValue(trackIndex, keyIndex);
						}

						if (j == 0)
						{
							position.x = value;
						} 
						else
						{
							position.y = value;
						}

						_objAction.TrackInsertKey(trackIndex, rTime, position, GetGodotTransition(lerpMode));
					}

					_objAction.TrackSetInterpolationType(trackIndex, Animation.InterpolationType.Linear);
				}
			}
		}
		*/
		// --------------------------------------------------------------------
		public static void ReadKFBFunc(string _sSrcFuncName, string _variableName, string _sDstActionTrackName, CFEVect2 _defaultValue, CFEConfigFile _oConfigFile, ref Animation _objAction)
		{
			const CFEString DEFAULT_WRAP_MODE = "finalvalue";
			const CFEString DEFAULT_LERP_FUNC = "linear";
	
			EFEKFBFuncWrapMode eFuncWrapMode = Support.eGetWrapModeFromString(_oConfigFile.sGetString(_sSrcFuncName + ".WrapMode", DEFAULT_WRAP_MODE));

			int iKeyFrames = _oConfigFile.iGetInteger(_sSrcFuncName + ".NumKeyFrames", -1);
			// TODO: if (iKeyFrames == 0) iKeyFrames = 1;	// will use default values

			if (iKeyFrames != -1)
			{
				int trackIndex = _objAction.AddTrack(Animation.TrackType.Value);
				_objAction.TrackSetPath(trackIndex, _sDstActionTrackName);

				string sVar = _sSrcFuncName + ".KeyFrame";
				for (int i=0; i<iKeyFrames; i++)
				{
					CFEString sIVar = sVar + i.ToString();

					float x = _oConfigFile.rGetReal(sIVar + ".x",  _defaultValue.x);
					float y = _oConfigFile.rGetReal(sIVar + ".y",  _defaultValue.y);
					Vector2 vector = new Vector2(x, y);

					FEReal rTime = _oConfigFile.rGetReal(sIVar + ".Time", 0.0f);
					EFEKFLerpFunc lerpMode = Support.eGetLerpFuncFromString(_oConfigFile.sGetString(sIVar + ".LerpFunc", DEFAULT_LERP_FUNC));

					_objAction.TrackInsertKey(trackIndex, rTime, vector, GetGodotTransition(lerpMode));
				}
				
				_objAction.TrackSetInterpolationType(trackIndex, Animation.InterpolationType.Linear);
				_objAction.TrackSetInterpolationLoopWrap(trackIndex, (eFuncWrapMode  == EFEKFBFuncWrapMode.KFBFWM_NONE)? false : true);
			}
		}
		// --------------------------------------------------------------------
		public static void ReadKFBFunc<T>(string _sSrcFuncName, string _variableName, string _sDstActionTrackName, T _defaultValue, CFEConfigFile _oConfigFile, ref Animation _objAction)
		{
			const CFEString DEFAULT_WRAP_MODE = "finalvalue";
			const CFEString DEFAULT_LERP_FUNC = "linear";
	
			EFEKFBFuncWrapMode eFuncWrapMode = Support.eGetWrapModeFromString(_oConfigFile.sGetString(_sSrcFuncName + ".WrapMode", DEFAULT_WRAP_MODE));

			int iKeyFrames = _oConfigFile.iGetInteger(_sSrcFuncName + ".NumKeyFrames", -1);
			// TODO: if (iKeyFrames == 0) iKeyFrames = 1;	// will use default values

			if (iKeyFrames != -1)
			{
				int trackIndex = _objAction.AddTrack(Animation.TrackType.Value);
				_objAction.TrackSetPath(trackIndex, _sDstActionTrackName);

				string sVar = _sSrcFuncName + ".KeyFrame";
				for (int i=0; i<iKeyFrames; i++)
				{
					CFEString sIVar = sVar + i.ToString();

					object value = _defaultValue;
					if (typeof(T) == typeof(FEReal))
					{
						value = _oConfigFile.rGetReal(sIVar + "." + _variableName, Convert.ToSingle(_defaultValue));
					}
					else if (typeof(T) == typeof(int))
					{
						value = _oConfigFile.iGetInteger(sIVar + "." + _variableName, Convert.ToInt32(_defaultValue));
					} 
					else if (typeof(T) == typeof(bool))
					{
						value = _oConfigFile.bGetBool(sIVar + "." + _variableName, Convert.ToBoolean(_defaultValue));
					}
					else
					{
						throw new InvalidOperationException($"Unsupported type: {typeof(T)}");
					}

					FEReal rTime = _oConfigFile.rGetReal(sIVar + ".Time", 0.0f);
					EFEKFLerpFunc lerpMode = Support.eGetLerpFuncFromString(_oConfigFile.sGetString(sIVar + ".LerpFunc", DEFAULT_LERP_FUNC));

					_objAction.TrackInsertKey(trackIndex, rTime, value, GetGodotTransition(lerpMode));

				}
				
				_objAction.TrackSetInterpolationType(trackIndex, Animation.InterpolationType.Linear);
				_objAction.TrackSetInterpolationLoopWrap(trackIndex, (eFuncWrapMode  == EFEKFBFuncWrapMode.KFBFWM_NONE)? false : true);
			}
		}
		// --------------------------------------------------------------------
	}
	// --------------------------------------------------------------------
	public class CFERect
	{
		/// <summary>
		///  Default constructor for the class
		/// </summary>
		public CFERect() { }

		public CFERect(float _fIX,float _fIY, float _fFX, float _fFY)
		{
			m_oIni.x = _fIX;
			m_oIni.y = _fFX;
			m_oEnd.x = _fFX;
			m_oEnd.x = _fFY;
		}

		public CFEVect2 m_oIni = new CFEVect2();
		public CFEVect2 m_oEnd = new CFEVect2();
	};
	// --------------------------------------------------------------------
	public class CFENamedObject
	{
		/// Default constructor of this object.
		public CFENamedObject(string _sName)
		{
			SetName(_sName);
		}
		
		/// Sets the name for this object.
		public void SetName(string _sName)
		{
			m_sName = _sName;
		}
		
		/// Retrieves the name of this object.
		public string sGetName()
		{
			return (m_sName);
		}

		protected string m_sName = "";
	};
	// --------------------------------------------------------------------
};