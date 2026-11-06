using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000305 RID: 773
	public struct StyleInt : IStyleValue<int>, IEquatable<StyleInt>
	{
		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001A8F RID: 6799 RVA: 0x00069728 File Offset: 0x00067928
		// (set) Token: 0x06001A90 RID: 6800 RVA: 0x0006974B File Offset: 0x0006794B
		public int value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : 0;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x0006975C File Offset: 0x0006795C
		// (set) Token: 0x06001A92 RID: 6802 RVA: 0x00069774 File Offset: 0x00067974
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

		// Token: 0x06001A93 RID: 6803 RVA: 0x0006977E File Offset: 0x0006797E
		public StyleInt(int v)
		{
			this = new StyleInt(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x0006978A File Offset: 0x0006798A
		public StyleInt(StyleKeyword keyword)
		{
			this = new StyleInt(0, keyword);
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x00069796 File Offset: 0x00067996
		internal StyleInt(int v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x000697A8 File Offset: 0x000679A8
		public static bool operator ==(StyleInt lhs, StyleInt rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x000697DC File Offset: 0x000679DC
		public static bool operator !=(StyleInt lhs, StyleInt rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x000697F8 File Offset: 0x000679F8
		public static implicit operator StyleInt(StyleKeyword keyword)
		{
			return new StyleInt(keyword);
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00069810 File Offset: 0x00067A10
		public static implicit operator StyleInt(int v)
		{
			return new StyleInt(v);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x00069828 File Offset: 0x00067A28
		public bool Equals(StyleInt other)
		{
			return other == this;
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00069848 File Offset: 0x00067A48
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleInt)
			{
				StyleInt styleInt = (StyleInt)obj;
				flag = this.Equals(styleInt);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00069874 File Offset: 0x00067A74
		public override int GetHashCode()
		{
			return (this.m_Value * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x0006989C File Offset: 0x00067A9C
		public override string ToString()
		{
			return this.DebugString<int>();
		}

		// Token: 0x04000AF0 RID: 2800
		private int m_Value;

		// Token: 0x04000AF1 RID: 2801
		private StyleKeyword m_Keyword;
	}
}
