using Godot;
//-----------------------------------------------------------------------------
using CFEString = System.String;
using FEReal = System.Single;
using CFEVect2 = Godot.Vector2;
using CFEColor = Godot.Color;
//-----------------------------------------------------------------------------
namespace FuetEngine
{
    [Tool]
	public class CFEHUDLabel : CFEHUDObject 
	{
		//---------------------------------------------------------------------
		/*
		[Export]
		public 
		*/
		private CFEString		        m_sFont;/*
		[Export]
		public 
		*/
		private CFEString 				m_sText; // { set{ SetText(value); } get{ return sGetText(); }}
		/*
		[Export]
		public 
		*/
		private EFETextHAlignmentMode   m_eHAlignment;
		/*
		[Export]
		public 
		*/
		private EFETextVAlignmentMode   m_eVAlignment;
		/*
		[Export]
		public 
		*/
		private FEReal					m_rPointSize;
		/*
		[Export]
		public 
		*/
		private FEReal					m_rTracking;
		/*
		[Export]
		public 
		*/
		private FEReal					m_rInterlining;
		/*
		[Export]
		public 
		*/
		private FEReal					m_rAdjustmentWidth;
		/*
		[Export]
		public 
		*/
		private CFEString		        m_sAdjustedText;		
		private Label 					m_label;
		//---------------------------------------------------------------------		
		// Make sure you provide a parameterless constructor.
		public CFEHUDLabel()
		{			
			Name = "CFEHUDLabel";
		}
		//---------------------------------------------------------------------
		/// Sets the font for this label.
		public void SetFont(CFEString _sFont)
		{
			CheckLabel();
			m_sFont = _sFont;

			if (m_rAdjustmentWidth != -1.0f)
				AdjustText();
		}
		//---------------------------------------------------------------------
		/// Retrieves the font's label.
		public CFEString GetFont()
		{
			return m_sFont;
		}
		//---------------------------------------------------------------------
		/// Sets the text to be displayed by this label.
		public void SetText(CFEString _sText)
		{
			CheckLabel();
			
			m_sText = _sText;
			m_label.Text = _sText;

			if (m_rAdjustmentWidth != -1.0f)
				AdjustText();
		}
		//---------------------------------------------------------------------
		/// Retrieves the text of this label.
		public CFEString sGetText()
		{
			return m_sText;
		}
		//---------------------------------------------------------------------
		/// Retrieves the printable text string of this label.
		public CFEString sGetPrintableText()
		{
			if (m_rAdjustmentWidth == -1.0f)
				return m_sText;
			else
				return m_sAdjustedText;
		}
		//---------------------------------------------------------------------
		/// Sets the horizontal alignment for the label.
		public void SetHAlignment(EFETextHAlignmentMode _eHAlignment)
		{
			CheckLabel();
			m_eHAlignment = _eHAlignment;
			m_label.Align = GetLabelAlignment(_eHAlignment);
		}
		//---------------------------------------------------------------------
		/// Retrieves the horizontal alignment of the label.
		public EFETextHAlignmentMode eGetHAlignment()
		{
			CheckLabel();
			return m_eHAlignment;
		}
		
		//---------------------------------------------------------------------
		/// Sets the vertical alignment for the label.
		public void SetVAlignment(EFETextVAlignmentMode _eVAlignment)
		{
			CheckLabel();
			m_eVAlignment = _eVAlignment;
			m_label.Valign = GetLabelVAlignment(_eVAlignment);
		}
		//---------------------------------------------------------------------
		/// Retrieves the vertical alignment of the label.
		public EFETextVAlignmentMode eGetVAlignment()
		{
			CheckLabel();
			return m_eVAlignment;
		}
		//---------------------------------------------------------------------
		/// Sets the point size for this label. PointSize scales uniformly the text being displayed.
		public void SetPointSize(FEReal _rPointSize)
		{
			CheckLabel();
			m_rPointSize = _rPointSize;
			if (m_rAdjustmentWidth != -1.0f)
				AdjustText();
		}
		//---------------------------------------------------------------------
		/// Retrieves the point size for this label.
		public FEReal rGetPointSize()
		{
			return m_rPointSize;
		}

		//---------------------------------------------------------------------
		/// Sets the tracking used by this label.
		public void SetTracking(FEReal _rTracking)
		{
			CheckLabel();
			m_rTracking = _rTracking;
			if (m_rAdjustmentWidth != -1.0f)
				AdjustText();					
		}
		//---------------------------------------------------------------------
		/// Retrieves the tracking used by this label.
		public FEReal rGetTracking()
		{
			return m_rTracking;
		}
		//---------------------------------------------------------------------
		/// Sets the interlining space used by this label.
		public void SetInterlining(FEReal _rInterlining)
		{
			CheckLabel();
			m_rInterlining = _rInterlining;
		}
		//---------------------------------------------------------------------
		/// Retrieves the interlining space used by this label.
		public FEReal rGetInterlining()
		{
			return m_rInterlining;
		}
		//---------------------------------------------------------------------
		/// Sets the adjustment width this label will accomodate. -1 if no adjustment width.
		public void SetAdjustmentWidth(FEReal _rAdjustmentWidth)
		{
			CheckLabel();
			m_rAdjustmentWidth = _rAdjustmentWidth;
			if (m_rAdjustmentWidth != -1.0f)
				AdjustText();
		}

		//---------------------------------------------------------------------
		/// Retrieves the adjustment width this label will accomodate. -1 if no adjustment width
		public FEReal rGetAdjustmentWidth()
		{
			return m_rAdjustmentWidth;
		}

		//---------------------------------------------------------------------
		/// Perform processing over the object
		public override void Accept(CFEHUDVisitor _oVisitor)
		{
			_oVisitor.Visit(this);
		}

		//---------------------------------------------------------------------
		private void AdjustText()
		{
		}
		//---------------------------------------------------------------------
		private void CheckLabel()
		{
			if (GetChildCount() ==0)
			{				
				m_label = new Label();
				m_label.Name = "Label";
				AddChild(m_label);
			}
		}
		//---------------------------------------------------------------------
		private Label.AlignEnum GetLabelAlignment(EFETextHAlignmentMode _hAlignmentMode)
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
		private Label.VAlign GetLabelVAlignment(EFETextVAlignmentMode _vAlignmentMode)
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
		
	};	
}
//-----------------------------------------------------------------------------
