using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200030B RID: 779
	public struct StyleTextShadow : IStyleValue<TextShadow>, IEquatable<StyleTextShadow>
	{
		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x0006A3F8 File Offset: 0x000685F8
		// (set) Token: 0x06001AEE RID: 6894 RVA: 0x0006A423 File Offset: 0x00068623
		public TextShadow value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(TextShadow);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001AEF RID: 6895 RVA: 0x0006A434 File Offset: 0x00068634
		// (set) Token: 0x06001AF0 RID: 6896 RVA: 0x0006A44C File Offset: 0x0006864C
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

		// Token: 0x06001AF1 RID: 6897 RVA: 0x0006A456 File Offset: 0x00068656
		public StyleTextShadow(TextShadow v)
		{
			this = new StyleTextShadow(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x0006A464 File Offset: 0x00068664
		public StyleTextShadow(StyleKeyword keyword)
		{
			this = new StyleTextShadow(default(TextShadow), keyword);
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x0006A483 File Offset: 0x00068683
		internal StyleTextShadow(TextShadow v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x0006A494 File Offset: 0x00068694
		public static bool operator ==(StyleTextShadow lhs, StyleTextShadow rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x0006A4C8 File Offset: 0x000686C8
		public static bool operator !=(StyleTextShadow lhs, StyleTextShadow rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0006A4E4 File Offset: 0x000686E4
		public static implicit operator StyleTextShadow(StyleKeyword keyword)
		{
			return new StyleTextShadow(keyword);
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x0006A4FC File Offset: 0x000686FC
		public static implicit operator StyleTextShadow(TextShadow v)
		{
			return new StyleTextShadow(v);
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x0006A514 File Offset: 0x00068714
		public bool Equals(StyleTextShadow other)
		{
			return other == this;
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x0006A534 File Offset: 0x00068734
		public override bool Equals(object obj)
		{
			bool flag = !(obj is StyleTextShadow);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				StyleTextShadow styleTextShadow = (StyleTextShadow)obj;
				flag2 = styleTextShadow == this;
			}
			return flag2;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x0006A570 File Offset: 0x00068770
		public override int GetHashCode()
		{
			int num = 917506989;
			num = num * -1521134295 + this.m_Keyword.GetHashCode();
			return num * -1521134295 + this.m_Value.GetHashCode();
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x0006A5C0 File Offset: 0x000687C0
		public override string ToString()
		{
			return this.DebugString<TextShadow>();
		}

		// Token: 0x04000AFC RID: 2812
		private StyleKeyword m_Keyword;

		// Token: 0x04000AFD RID: 2813
		private TextShadow m_Value;
	}
}
