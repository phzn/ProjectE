using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000302 RID: 770
	public struct StyleFloat : IStyleValue<float>, IEquatable<StyleFloat>
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x0006918C File Offset: 0x0006738C
		// (set) Token: 0x06001A5C RID: 6748 RVA: 0x000691B3 File Offset: 0x000673B3
		public float value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : 0f;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001A5D RID: 6749 RVA: 0x000691C4 File Offset: 0x000673C4
		// (set) Token: 0x06001A5E RID: 6750 RVA: 0x000691DC File Offset: 0x000673DC
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

		// Token: 0x06001A5F RID: 6751 RVA: 0x000691E6 File Offset: 0x000673E6
		public StyleFloat(float v)
		{
			this = new StyleFloat(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x000691F2 File Offset: 0x000673F2
		public StyleFloat(StyleKeyword keyword)
		{
			this = new StyleFloat(0f, keyword);
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x00069202 File Offset: 0x00067402
		internal StyleFloat(float v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x00069214 File Offset: 0x00067414
		public static bool operator ==(StyleFloat lhs, StyleFloat rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x00069248 File Offset: 0x00067448
		public static bool operator !=(StyleFloat lhs, StyleFloat rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x00069264 File Offset: 0x00067464
		public static implicit operator StyleFloat(StyleKeyword keyword)
		{
			return new StyleFloat(keyword);
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x0006927C File Offset: 0x0006747C
		public static implicit operator StyleFloat(float v)
		{
			return new StyleFloat(v);
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x00069294 File Offset: 0x00067494
		public bool Equals(StyleFloat other)
		{
			return other == this;
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x000692B4 File Offset: 0x000674B4
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleFloat)
			{
				StyleFloat styleFloat = (StyleFloat)obj;
				flag = this.Equals(styleFloat);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x000692E0 File Offset: 0x000674E0
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x0006930C File Offset: 0x0006750C
		public override string ToString()
		{
			return this.DebugString<float>();
		}

		// Token: 0x04000AEA RID: 2794
		private float m_Value;

		// Token: 0x04000AEB RID: 2795
		private StyleKeyword m_Keyword;
	}
}
