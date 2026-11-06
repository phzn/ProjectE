using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002FE RID: 766
	public struct StyleCursor : IStyleValue<Cursor>, IEquatable<StyleCursor>
	{
		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001A28 RID: 6696 RVA: 0x00068B20 File Offset: 0x00066D20
		// (set) Token: 0x06001A29 RID: 6697 RVA: 0x00068B4B File Offset: 0x00066D4B
		public Cursor value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(Cursor);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001A2A RID: 6698 RVA: 0x00068B5C File Offset: 0x00066D5C
		// (set) Token: 0x06001A2B RID: 6699 RVA: 0x00068B74 File Offset: 0x00066D74
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

		// Token: 0x06001A2C RID: 6700 RVA: 0x00068B7E File Offset: 0x00066D7E
		public StyleCursor(Cursor v)
		{
			this = new StyleCursor(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00068B8C File Offset: 0x00066D8C
		public StyleCursor(StyleKeyword keyword)
		{
			this = new StyleCursor(default(Cursor), keyword);
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x00068BAB File Offset: 0x00066DAB
		internal StyleCursor(Cursor v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x00068BBC File Offset: 0x00066DBC
		public static bool operator ==(StyleCursor lhs, StyleCursor rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x00068BF0 File Offset: 0x00066DF0
		public static bool operator !=(StyleCursor lhs, StyleCursor rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x00068C0C File Offset: 0x00066E0C
		public static implicit operator StyleCursor(StyleKeyword keyword)
		{
			return new StyleCursor(keyword);
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x00068C24 File Offset: 0x00066E24
		public static implicit operator StyleCursor(Cursor v)
		{
			return new StyleCursor(v);
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x00068C3C File Offset: 0x00066E3C
		public bool Equals(StyleCursor other)
		{
			return other == this;
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x00068C5C File Offset: 0x00066E5C
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleCursor)
			{
				StyleCursor styleCursor = (StyleCursor)obj;
				flag = this.Equals(styleCursor);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x00068C88 File Offset: 0x00066E88
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x00068CBC File Offset: 0x00066EBC
		public override string ToString()
		{
			return this.DebugString<Cursor>();
		}

		// Token: 0x04000AE1 RID: 2785
		private Cursor m_Value;

		// Token: 0x04000AE2 RID: 2786
		private StyleKeyword m_Keyword;
	}
}
