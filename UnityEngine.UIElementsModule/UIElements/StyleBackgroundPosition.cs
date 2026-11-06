using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002FA RID: 762
	public struct StyleBackgroundPosition : IStyleValue<BackgroundPosition>, IEquatable<StyleBackgroundPosition>
	{
		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x060019EA RID: 6634 RVA: 0x000683F4 File Offset: 0x000665F4
		// (set) Token: 0x060019EB RID: 6635 RVA: 0x0006841F File Offset: 0x0006661F
		public BackgroundPosition value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(BackgroundPosition);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x060019EC RID: 6636 RVA: 0x00068430 File Offset: 0x00066630
		// (set) Token: 0x060019ED RID: 6637 RVA: 0x00068448 File Offset: 0x00066648
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

		// Token: 0x060019EE RID: 6638 RVA: 0x00068452 File Offset: 0x00066652
		public StyleBackgroundPosition(BackgroundPosition v)
		{
			this = new StyleBackgroundPosition(v, StyleKeyword.Undefined);
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x00068460 File Offset: 0x00066660
		public StyleBackgroundPosition(StyleKeyword keyword)
		{
			this = new StyleBackgroundPosition(default(BackgroundPosition), keyword);
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x0006847F File Offset: 0x0006667F
		internal StyleBackgroundPosition(BackgroundPosition v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x00068490 File Offset: 0x00066690
		public static bool operator ==(StyleBackgroundPosition lhs, StyleBackgroundPosition rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x000684C4 File Offset: 0x000666C4
		public static bool operator !=(StyleBackgroundPosition lhs, StyleBackgroundPosition rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x000684E0 File Offset: 0x000666E0
		public static implicit operator StyleBackgroundPosition(StyleKeyword keyword)
		{
			return new StyleBackgroundPosition(keyword);
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x000684F8 File Offset: 0x000666F8
		public static implicit operator StyleBackgroundPosition(BackgroundPosition v)
		{
			return new StyleBackgroundPosition(v);
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x00068510 File Offset: 0x00066710
		public bool Equals(StyleBackgroundPosition other)
		{
			return other == this;
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x00068530 File Offset: 0x00066730
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleBackgroundPosition)
			{
				StyleBackgroundPosition styleBackgroundPosition = (StyleBackgroundPosition)obj;
				flag = this.Equals(styleBackgroundPosition);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0006855C File Offset: 0x0006675C
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x00068590 File Offset: 0x00066790
		public override string ToString()
		{
			return this.DebugString<BackgroundPosition>();
		}

		// Token: 0x04000AD9 RID: 2777
		private BackgroundPosition m_Value;

		// Token: 0x04000ADA RID: 2778
		private StyleKeyword m_Keyword;
	}
}
