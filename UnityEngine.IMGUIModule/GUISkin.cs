using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000027 RID: 39
	[AssetFileNameExtension("guiskin", new string[] { })]
	[RequiredByNativeCode]
	[ExecuteInEditMode]
	[Serializable]
	public sealed class GUISkin : ScriptableObject
	{
		// Token: 0x0600029C RID: 668 RVA: 0x0000AB42 File Offset: 0x00008D42
		public GUISkin()
		{
			this.m_CustomStyles = new GUIStyle[1];
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000AB6A File Offset: 0x00008D6A
		internal void OnEnable()
		{
			this.Apply();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000AB74 File Offset: 0x00008D74
		internal static void CleanupRoots()
		{
			GUISkin.current = null;
			GUISkin.ms_Error = null;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000AB84 File Offset: 0x00008D84
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x0000AB9C File Offset: 0x00008D9C
		public Font font
		{
			get
			{
				return this.m_Font;
			}
			set
			{
				this.m_Font = value;
				bool flag = GUISkin.current == this;
				if (flag)
				{
					GUIStyle.SetDefaultFont(this.m_Font);
				}
				this.Apply();
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000ABD4 File Offset: 0x00008DD4
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x0000ABEC File Offset: 0x00008DEC
		public GUIStyle box
		{
			get
			{
				return this.m_box;
			}
			set
			{
				this.m_box = value;
				this.Apply();
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000AC00 File Offset: 0x00008E00
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x0000AC18 File Offset: 0x00008E18
		public GUIStyle label
		{
			get
			{
				return this.m_label;
			}
			set
			{
				this.m_label = value;
				this.Apply();
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000AC2C File Offset: 0x00008E2C
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0000AC44 File Offset: 0x00008E44
		public GUIStyle textField
		{
			get
			{
				return this.m_textField;
			}
			set
			{
				this.m_textField = value;
				this.Apply();
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000AC58 File Offset: 0x00008E58
		// (set) Token: 0x060002A8 RID: 680 RVA: 0x0000AC70 File Offset: 0x00008E70
		public GUIStyle textArea
		{
			get
			{
				return this.m_textArea;
			}
			set
			{
				this.m_textArea = value;
				this.Apply();
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000AC84 File Offset: 0x00008E84
		// (set) Token: 0x060002AA RID: 682 RVA: 0x0000AC9C File Offset: 0x00008E9C
		public GUIStyle button
		{
			get
			{
				return this.m_button;
			}
			set
			{
				this.m_button = value;
				this.Apply();
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000ACB0 File Offset: 0x00008EB0
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000ACC8 File Offset: 0x00008EC8
		public GUIStyle toggle
		{
			get
			{
				return this.m_toggle;
			}
			set
			{
				this.m_toggle = value;
				this.Apply();
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000ACDC File Offset: 0x00008EDC
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0000ACF4 File Offset: 0x00008EF4
		public GUIStyle window
		{
			get
			{
				return this.m_window;
			}
			set
			{
				this.m_window = value;
				this.Apply();
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000AD08 File Offset: 0x00008F08
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000AD20 File Offset: 0x00008F20
		public GUIStyle horizontalSlider
		{
			get
			{
				return this.m_horizontalSlider;
			}
			set
			{
				this.m_horizontalSlider = value;
				this.Apply();
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000AD34 File Offset: 0x00008F34
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000AD4C File Offset: 0x00008F4C
		public GUIStyle horizontalSliderThumb
		{
			get
			{
				return this.m_horizontalSliderThumb;
			}
			set
			{
				this.m_horizontalSliderThumb = value;
				this.Apply();
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000AD60 File Offset: 0x00008F60
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000AD78 File Offset: 0x00008F78
		internal GUIStyle horizontalSliderThumbExtent
		{
			get
			{
				return this.m_horizontalSliderThumbExtent;
			}
			set
			{
				this.m_horizontalSliderThumbExtent = value;
				this.Apply();
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000AD8C File Offset: 0x00008F8C
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0000ADA4 File Offset: 0x00008FA4
		internal GUIStyle sliderMixed
		{
			get
			{
				return this.m_SliderMixed;
			}
			set
			{
				this.m_SliderMixed = value;
				this.Apply();
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000ADB8 File Offset: 0x00008FB8
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000ADD0 File Offset: 0x00008FD0
		public GUIStyle verticalSlider
		{
			get
			{
				return this.m_verticalSlider;
			}
			set
			{
				this.m_verticalSlider = value;
				this.Apply();
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000ADE4 File Offset: 0x00008FE4
		// (set) Token: 0x060002BA RID: 698 RVA: 0x0000ADFC File Offset: 0x00008FFC
		public GUIStyle verticalSliderThumb
		{
			get
			{
				return this.m_verticalSliderThumb;
			}
			set
			{
				this.m_verticalSliderThumb = value;
				this.Apply();
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000AE10 File Offset: 0x00009010
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0000AE28 File Offset: 0x00009028
		internal GUIStyle verticalSliderThumbExtent
		{
			get
			{
				return this.m_verticalSliderThumbExtent;
			}
			set
			{
				this.m_verticalSliderThumbExtent = value;
				this.Apply();
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000AE3C File Offset: 0x0000903C
		// (set) Token: 0x060002BE RID: 702 RVA: 0x0000AE54 File Offset: 0x00009054
		public GUIStyle horizontalScrollbar
		{
			get
			{
				return this.m_horizontalScrollbar;
			}
			set
			{
				this.m_horizontalScrollbar = value;
				this.Apply();
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000AE68 File Offset: 0x00009068
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x0000AE80 File Offset: 0x00009080
		public GUIStyle horizontalScrollbarThumb
		{
			get
			{
				return this.m_horizontalScrollbarThumb;
			}
			set
			{
				this.m_horizontalScrollbarThumb = value;
				this.Apply();
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000AE94 File Offset: 0x00009094
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000AEAC File Offset: 0x000090AC
		public GUIStyle horizontalScrollbarLeftButton
		{
			get
			{
				return this.m_horizontalScrollbarLeftButton;
			}
			set
			{
				this.m_horizontalScrollbarLeftButton = value;
				this.Apply();
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000AEC0 File Offset: 0x000090C0
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x0000AED8 File Offset: 0x000090D8
		public GUIStyle horizontalScrollbarRightButton
		{
			get
			{
				return this.m_horizontalScrollbarRightButton;
			}
			set
			{
				this.m_horizontalScrollbarRightButton = value;
				this.Apply();
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000AEEC File Offset: 0x000090EC
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x0000AF04 File Offset: 0x00009104
		public GUIStyle verticalScrollbar
		{
			get
			{
				return this.m_verticalScrollbar;
			}
			set
			{
				this.m_verticalScrollbar = value;
				this.Apply();
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000AF18 File Offset: 0x00009118
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x0000AF30 File Offset: 0x00009130
		public GUIStyle verticalScrollbarThumb
		{
			get
			{
				return this.m_verticalScrollbarThumb;
			}
			set
			{
				this.m_verticalScrollbarThumb = value;
				this.Apply();
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000AF44 File Offset: 0x00009144
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000AF5C File Offset: 0x0000915C
		public GUIStyle verticalScrollbarUpButton
		{
			get
			{
				return this.m_verticalScrollbarUpButton;
			}
			set
			{
				this.m_verticalScrollbarUpButton = value;
				this.Apply();
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000AF70 File Offset: 0x00009170
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000AF88 File Offset: 0x00009188
		public GUIStyle verticalScrollbarDownButton
		{
			get
			{
				return this.m_verticalScrollbarDownButton;
			}
			set
			{
				this.m_verticalScrollbarDownButton = value;
				this.Apply();
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000AF9C File Offset: 0x0000919C
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000AFB4 File Offset: 0x000091B4
		public GUIStyle scrollView
		{
			get
			{
				return this.m_ScrollView;
			}
			set
			{
				this.m_ScrollView = value;
				this.Apply();
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000AFC8 File Offset: 0x000091C8
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0000AFE0 File Offset: 0x000091E0
		public GUIStyle[] customStyles
		{
			get
			{
				return this.m_CustomStyles;
			}
			set
			{
				this.m_CustomStyles = value;
				this.Apply();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000AFF4 File Offset: 0x000091F4
		public GUISettings settings
		{
			get
			{
				return this.m_Settings;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000B00C File Offset: 0x0000920C
		internal static GUIStyle error
		{
			get
			{
				bool flag = GUISkin.ms_Error == null;
				if (flag)
				{
					GUISkin.ms_Error = new GUIStyle();
					GUISkin.ms_Error.name = "StyleNotFoundError";
				}
				return GUISkin.ms_Error;
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000B04C File Offset: 0x0000924C
		internal void Apply()
		{
			bool flag = this.m_CustomStyles == null;
			if (flag)
			{
				Debug.Log("custom styles is null");
			}
			this.BuildStyleCache();
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000B07C File Offset: 0x0000927C
		private void BuildStyleCache()
		{
			bool flag = this.m_box == null;
			if (flag)
			{
				this.m_box = new GUIStyle();
			}
			bool flag2 = this.m_button == null;
			if (flag2)
			{
				this.m_button = new GUIStyle();
			}
			bool flag3 = this.m_toggle == null;
			if (flag3)
			{
				this.m_toggle = new GUIStyle();
			}
			bool flag4 = this.m_label == null;
			if (flag4)
			{
				this.m_label = new GUIStyle();
			}
			bool flag5 = this.m_window == null;
			if (flag5)
			{
				this.m_window = new GUIStyle();
			}
			bool flag6 = this.m_textField == null;
			if (flag6)
			{
				this.m_textField = new GUIStyle();
			}
			bool flag7 = this.m_textArea == null;
			if (flag7)
			{
				this.m_textArea = new GUIStyle();
			}
			bool flag8 = this.m_horizontalSlider == null;
			if (flag8)
			{
				this.m_horizontalSlider = new GUIStyle();
			}
			bool flag9 = this.m_horizontalSliderThumb == null;
			if (flag9)
			{
				this.m_horizontalSliderThumb = new GUIStyle();
			}
			bool flag10 = this.m_verticalSlider == null;
			if (flag10)
			{
				this.m_verticalSlider = new GUIStyle();
			}
			bool flag11 = this.m_verticalSliderThumb == null;
			if (flag11)
			{
				this.m_verticalSliderThumb = new GUIStyle();
			}
			bool flag12 = this.m_horizontalScrollbar == null;
			if (flag12)
			{
				this.m_horizontalScrollbar = new GUIStyle();
			}
			bool flag13 = this.m_horizontalScrollbarThumb == null;
			if (flag13)
			{
				this.m_horizontalScrollbarThumb = new GUIStyle();
			}
			bool flag14 = this.m_horizontalScrollbarLeftButton == null;
			if (flag14)
			{
				this.m_horizontalScrollbarLeftButton = new GUIStyle();
			}
			bool flag15 = this.m_horizontalScrollbarRightButton == null;
			if (flag15)
			{
				this.m_horizontalScrollbarRightButton = new GUIStyle();
			}
			bool flag16 = this.m_verticalScrollbar == null;
			if (flag16)
			{
				this.m_verticalScrollbar = new GUIStyle();
			}
			bool flag17 = this.m_verticalScrollbarThumb == null;
			if (flag17)
			{
				this.m_verticalScrollbarThumb = new GUIStyle();
			}
			bool flag18 = this.m_verticalScrollbarUpButton == null;
			if (flag18)
			{
				this.m_verticalScrollbarUpButton = new GUIStyle();
			}
			bool flag19 = this.m_verticalScrollbarDownButton == null;
			if (flag19)
			{
				this.m_verticalScrollbarDownButton = new GUIStyle();
			}
			bool flag20 = this.m_ScrollView == null;
			if (flag20)
			{
				this.m_ScrollView = new GUIStyle();
			}
			this.m_Styles = new Dictionary<string, GUIStyle>(StringComparer.OrdinalIgnoreCase);
			this.m_Styles["box"] = this.m_box;
			this.m_box.name = "box";
			this.m_Styles["button"] = this.m_button;
			this.m_button.name = "button";
			this.m_Styles["toggle"] = this.m_toggle;
			this.m_toggle.name = "toggle";
			this.m_Styles["label"] = this.m_label;
			this.m_label.name = "label";
			this.m_Styles["window"] = this.m_window;
			this.m_window.name = "window";
			this.m_Styles["textfield"] = this.m_textField;
			this.m_textField.name = "textfield";
			this.m_Styles["textarea"] = this.m_textArea;
			this.m_textArea.name = "textarea";
			this.m_Styles["horizontalslider"] = this.m_horizontalSlider;
			this.m_horizontalSlider.name = "horizontalslider";
			this.m_Styles["horizontalsliderthumb"] = this.m_horizontalSliderThumb;
			this.m_horizontalSliderThumb.name = "horizontalsliderthumb";
			this.m_Styles["verticalslider"] = this.m_verticalSlider;
			this.m_verticalSlider.name = "verticalslider";
			this.m_Styles["verticalsliderthumb"] = this.m_verticalSliderThumb;
			this.m_verticalSliderThumb.name = "verticalsliderthumb";
			this.m_Styles["horizontalscrollbar"] = this.m_horizontalScrollbar;
			this.m_horizontalScrollbar.name = "horizontalscrollbar";
			this.m_Styles["horizontalscrollbarthumb"] = this.m_horizontalScrollbarThumb;
			this.m_horizontalScrollbarThumb.name = "horizontalscrollbarthumb";
			this.m_Styles["horizontalscrollbarleftbutton"] = this.m_horizontalScrollbarLeftButton;
			this.m_horizontalScrollbarLeftButton.name = "horizontalscrollbarleftbutton";
			this.m_Styles["horizontalscrollbarrightbutton"] = this.m_horizontalScrollbarRightButton;
			this.m_horizontalScrollbarRightButton.name = "horizontalscrollbarrightbutton";
			this.m_Styles["verticalscrollbar"] = this.m_verticalScrollbar;
			this.m_verticalScrollbar.name = "verticalscrollbar";
			this.m_Styles["verticalscrollbarthumb"] = this.m_verticalScrollbarThumb;
			this.m_verticalScrollbarThumb.name = "verticalscrollbarthumb";
			this.m_Styles["verticalscrollbarupbutton"] = this.m_verticalScrollbarUpButton;
			this.m_verticalScrollbarUpButton.name = "verticalscrollbarupbutton";
			this.m_Styles["verticalscrollbardownbutton"] = this.m_verticalScrollbarDownButton;
			this.m_verticalScrollbarDownButton.name = "verticalscrollbardownbutton";
			this.m_Styles["scrollview"] = this.m_ScrollView;
			this.m_ScrollView.name = "scrollview";
			bool flag21 = this.m_CustomStyles != null;
			if (flag21)
			{
				for (int i = 0; i < this.m_CustomStyles.Length; i++)
				{
					bool flag22 = this.m_CustomStyles[i] == null;
					if (!flag22)
					{
						this.m_Styles[this.m_CustomStyles[i].name] = this.m_CustomStyles[i];
					}
				}
			}
			bool flag23 = !this.m_Styles.TryGetValue("HorizontalSliderThumbExtent", out this.m_horizontalSliderThumbExtent);
			if (flag23)
			{
				this.m_horizontalSliderThumbExtent = new GUIStyle();
				this.m_horizontalSliderThumbExtent.name = "horizontalsliderthumbextent";
				this.m_Styles["HorizontalSliderThumbExtent"] = this.m_horizontalSliderThumbExtent;
			}
			bool flag24 = !this.m_Styles.TryGetValue("SliderMixed", out this.m_SliderMixed);
			if (flag24)
			{
				this.m_SliderMixed = new GUIStyle();
				this.m_SliderMixed.name = "SliderMixed";
				this.m_Styles["SliderMixed"] = this.m_SliderMixed;
			}
			bool flag25 = !this.m_Styles.TryGetValue("VerticalSliderThumbExtent", out this.m_verticalSliderThumbExtent);
			if (flag25)
			{
				this.m_verticalSliderThumbExtent = new GUIStyle();
				this.m_Styles["VerticalSliderThumbExtent"] = this.m_verticalSliderThumbExtent;
				this.m_verticalSliderThumbExtent.name = "verticalsliderthumbextent";
			}
			GUISkin.error.stretchHeight = true;
			GUISkin.error.normal.textColor = Color.red;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000B740 File Offset: 0x00009940
		public GUIStyle GetStyle(string styleName)
		{
			GUIStyle guistyle = this.FindStyle(styleName);
			bool flag = guistyle != null;
			GUIStyle guistyle2;
			if (flag)
			{
				guistyle2 = guistyle;
			}
			else
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Unable to find style '",
					styleName,
					"' in skin '",
					base.name,
					"' ",
					(Event.current != null) ? Event.current.type.ToString() : "<called outside OnGUI>"
				}));
				guistyle2 = GUISkin.error;
			}
			return guistyle2;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000B7CC File Offset: 0x000099CC
		public GUIStyle FindStyle(string styleName)
		{
			bool flag = this.m_Styles == null;
			if (flag)
			{
				this.BuildStyleCache();
			}
			GUIStyle guistyle;
			bool flag2 = this.m_Styles.TryGetValue(styleName, out guistyle);
			GUIStyle guistyle2;
			if (flag2)
			{
				guistyle2 = guistyle;
			}
			else
			{
				guistyle2 = null;
			}
			return guistyle2;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000B80C File Offset: 0x00009A0C
		internal void MakeCurrent()
		{
			GUISkin.current = this;
			GUIStyle.SetDefaultFont(this.font);
			bool flag = GUISkin.m_SkinChanged != null;
			if (flag)
			{
				GUISkin.m_SkinChanged();
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000B844 File Offset: 0x00009A44
		public IEnumerator GetEnumerator()
		{
			bool flag = this.m_Styles == null;
			if (flag)
			{
				this.BuildStyleCache();
			}
			return this.m_Styles.Values.GetEnumerator();
		}

		// Token: 0x040000AB RID: 171
		[SerializeField]
		private Font m_Font;

		// Token: 0x040000AC RID: 172
		[SerializeField]
		private GUIStyle m_box;

		// Token: 0x040000AD RID: 173
		[SerializeField]
		private GUIStyle m_button;

		// Token: 0x040000AE RID: 174
		[SerializeField]
		private GUIStyle m_toggle;

		// Token: 0x040000AF RID: 175
		[SerializeField]
		private GUIStyle m_label;

		// Token: 0x040000B0 RID: 176
		[SerializeField]
		private GUIStyle m_textField;

		// Token: 0x040000B1 RID: 177
		[SerializeField]
		private GUIStyle m_textArea;

		// Token: 0x040000B2 RID: 178
		[SerializeField]
		private GUIStyle m_window;

		// Token: 0x040000B3 RID: 179
		[SerializeField]
		private GUIStyle m_horizontalSlider;

		// Token: 0x040000B4 RID: 180
		[SerializeField]
		private GUIStyle m_horizontalSliderThumb;

		// Token: 0x040000B5 RID: 181
		[NonSerialized]
		private GUIStyle m_horizontalSliderThumbExtent;

		// Token: 0x040000B6 RID: 182
		[SerializeField]
		private GUIStyle m_verticalSlider;

		// Token: 0x040000B7 RID: 183
		[SerializeField]
		private GUIStyle m_verticalSliderThumb;

		// Token: 0x040000B8 RID: 184
		[NonSerialized]
		private GUIStyle m_verticalSliderThumbExtent;

		// Token: 0x040000B9 RID: 185
		[NonSerialized]
		private GUIStyle m_SliderMixed;

		// Token: 0x040000BA RID: 186
		[SerializeField]
		private GUIStyle m_horizontalScrollbar;

		// Token: 0x040000BB RID: 187
		[SerializeField]
		private GUIStyle m_horizontalScrollbarThumb;

		// Token: 0x040000BC RID: 188
		[SerializeField]
		private GUIStyle m_horizontalScrollbarLeftButton;

		// Token: 0x040000BD RID: 189
		[SerializeField]
		private GUIStyle m_horizontalScrollbarRightButton;

		// Token: 0x040000BE RID: 190
		[SerializeField]
		private GUIStyle m_verticalScrollbar;

		// Token: 0x040000BF RID: 191
		[SerializeField]
		private GUIStyle m_verticalScrollbarThumb;

		// Token: 0x040000C0 RID: 192
		[SerializeField]
		private GUIStyle m_verticalScrollbarUpButton;

		// Token: 0x040000C1 RID: 193
		[SerializeField]
		private GUIStyle m_verticalScrollbarDownButton;

		// Token: 0x040000C2 RID: 194
		[SerializeField]
		private GUIStyle m_ScrollView;

		// Token: 0x040000C3 RID: 195
		[SerializeField]
		internal GUIStyle[] m_CustomStyles;

		// Token: 0x040000C4 RID: 196
		[SerializeField]
		private GUISettings m_Settings = new GUISettings();

		// Token: 0x040000C5 RID: 197
		internal static GUIStyle ms_Error;

		// Token: 0x040000C6 RID: 198
		private Dictionary<string, GUIStyle> m_Styles = null;

		// Token: 0x040000C7 RID: 199
		internal static GUISkin.SkinChangedDelegate m_SkinChanged;

		// Token: 0x040000C8 RID: 200
		internal static GUISkin current;

		// Token: 0x02000028 RID: 40
		// (Invoke) Token: 0x060002DA RID: 730
		internal delegate void SkinChangedDelegate();
	}
}
