using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200030A RID: 778
	public struct StyleTranslate : IStyleValue<Translate>, IEquatable<StyleTranslate>
	{
		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001ADE RID: 6878 RVA: 0x0006A1FC File Offset: 0x000683FC
		// (set) Token: 0x06001ADF RID: 6879 RVA: 0x0006A261 File Offset: 0x00068461
		public Translate value
		{
			get
			{
				StyleKeyword keyword = this.m_Keyword;
				if (!true)
				{
				}
				Translate translate;
				switch (keyword)
				{
				case StyleKeyword.Undefined:
					translate = this.m_Value;
					goto IL_004F;
				case StyleKeyword.Null:
					translate = Translate.None();
					goto IL_004F;
				case StyleKeyword.None:
					translate = Translate.None();
					goto IL_004F;
				case StyleKeyword.Initial:
					translate = Translate.None();
					goto IL_004F;
				}
				throw new NotImplementedException();
				IL_004F:
				if (!true)
				{
				}
				return translate;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06001AE0 RID: 6880 RVA: 0x0006A274 File Offset: 0x00068474
		// (set) Token: 0x06001AE1 RID: 6881 RVA: 0x0006A28C File Offset: 0x0006848C
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

		// Token: 0x06001AE2 RID: 6882 RVA: 0x0006A296 File Offset: 0x00068496
		public StyleTranslate(Translate v)
		{
			this = new StyleTranslate(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x0006A2A4 File Offset: 0x000684A4
		public StyleTranslate(StyleKeyword keyword)
		{
			this = new StyleTranslate(default(Translate), keyword);
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x0006A2C3 File Offset: 0x000684C3
		internal StyleTranslate(Translate v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x0006A2D4 File Offset: 0x000684D4
		public static bool operator ==(StyleTranslate lhs, StyleTranslate rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x0006A308 File Offset: 0x00068508
		public static bool operator !=(StyleTranslate lhs, StyleTranslate rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x0006A324 File Offset: 0x00068524
		public static implicit operator StyleTranslate(StyleKeyword keyword)
		{
			return new StyleTranslate(keyword);
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x0006A33C File Offset: 0x0006853C
		public static implicit operator StyleTranslate(Translate v)
		{
			return new StyleTranslate(v);
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x0006A354 File Offset: 0x00068554
		public bool Equals(StyleTranslate other)
		{
			return other == this;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x0006A374 File Offset: 0x00068574
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleTranslate)
			{
				StyleTranslate styleTranslate = (StyleTranslate)obj;
				flag = this.Equals(styleTranslate);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0006A3A0 File Offset: 0x000685A0
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x0006A3D4 File Offset: 0x000685D4
		public override string ToString()
		{
			return this.DebugString<Translate>();
		}

		// Token: 0x04000AFA RID: 2810
		private Translate m_Value;

		// Token: 0x04000AFB RID: 2811
		private StyleKeyword m_Keyword;
	}
}
