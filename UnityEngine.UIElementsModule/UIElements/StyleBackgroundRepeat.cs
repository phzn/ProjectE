using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002FB RID: 763
	public struct StyleBackgroundRepeat : IStyleValue<BackgroundRepeat>, IEquatable<StyleBackgroundRepeat>
	{
		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x000685B4 File Offset: 0x000667B4
		// (set) Token: 0x060019FA RID: 6650 RVA: 0x000685DF File Offset: 0x000667DF
		public BackgroundRepeat value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(BackgroundRepeat);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060019FB RID: 6651 RVA: 0x000685F0 File Offset: 0x000667F0
		// (set) Token: 0x060019FC RID: 6652 RVA: 0x00068608 File Offset: 0x00066808
		public StyleKeyword keyword
		{
			get
			{
				return this.m_Keyword;
			}
			set
			{
				this.m_Keyword = value;
			}
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00068612 File Offset: 0x00066812
		public StyleBackgroundRepeat(BackgroundRepeat v)
		{
			this = new StyleBackgroundRepeat(v, StyleKeyword.Undefined);
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x00068620 File Offset: 0x00066820
		public StyleBackgroundRepeat(StyleKeyword keyword)
		{
			this = new StyleBackgroundRepeat(default(BackgroundRepeat), keyword);
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0006863F File Offset: 0x0006683F
		internal StyleBackgroundRepeat(BackgroundRepeat v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x00068650 File Offset: 0x00066850
		public static bool operator ==(StyleBackgroundRepeat lhs, StyleBackgroundRepeat rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x00068684 File Offset: 0x00066884
		public static bool operator !=(StyleBackgroundRepeat lhs, StyleBackgroundRepeat rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x000686A0 File Offset: 0x000668A0
		public static implicit operator StyleBackgroundRepeat(StyleKeyword keyword)
		{
			return new StyleBackgroundRepeat(keyword);
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x000686B8 File Offset: 0x000668B8
		public static implicit operator StyleBackgroundRepeat(BackgroundRepeat v)
		{
			return new StyleBackgroundRepeat(v);
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x000686D0 File Offset: 0x000668D0
		public bool Equals(StyleBackgroundRepeat other)
		{
			return other == this;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x000686F0 File Offset: 0x000668F0
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleBackgroundRepeat)
			{
				StyleBackgroundRepeat styleBackgroundRepeat = (StyleBackgroundRepeat)obj;
				flag = this.Equals(styleBackgroundRepeat);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0006871C File Offset: 0x0006691C
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00068750 File Offset: 0x00066950
		public override string ToString()
		{
			return this.DebugString<BackgroundRepeat>();
		}

		// Token: 0x04000ADB RID: 2779
		private BackgroundRepeat m_Value;

		// Token: 0x04000ADC RID: 2780
		private StyleKeyword m_Keyword;
	}
}
