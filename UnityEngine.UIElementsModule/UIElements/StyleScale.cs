using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000309 RID: 777
	public struct StyleScale : IStyleValue<Scale>, IEquatable<StyleScale>
	{
		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x00069FD0 File Offset: 0x000681D0
		// (set) Token: 0x06001ACE RID: 6862 RVA: 0x0006A035 File Offset: 0x00068235
		public Scale value
		{
			get
			{
				StyleKeyword keyword = this.m_Keyword;
				if (!true)
				{
				}
				Scale scale;
				switch (keyword)
				{
				case StyleKeyword.Undefined:
					scale = this.m_Value;
					goto IL_004F;
				case StyleKeyword.Null:
					scale = Scale.None();
					goto IL_004F;
				case StyleKeyword.None:
					scale = Scale.None();
					goto IL_004F;
				case StyleKeyword.Initial:
					scale = Scale.Initial();
					goto IL_004F;
				}
				throw new NotImplementedException();
				IL_004F:
				if (!true)
				{
				}
				return scale;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x0006A048 File Offset: 0x00068248
		// (set) Token: 0x06001AD0 RID: 6864 RVA: 0x0006A060 File Offset: 0x00068260
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

		// Token: 0x06001AD1 RID: 6865 RVA: 0x0006A06A File Offset: 0x0006826A
		public StyleScale(Scale v)
		{
			this = new StyleScale(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x0006A078 File Offset: 0x00068278
		public StyleScale(StyleKeyword keyword)
		{
			this = new StyleScale(default(Scale), keyword);
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x0006A097 File Offset: 0x00068297
		public StyleScale(Vector2 scale)
		{
			this = new StyleScale(new Scale(scale));
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x0006A0A7 File Offset: 0x000682A7
		internal StyleScale(Scale v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x0006A0B8 File Offset: 0x000682B8
		public static implicit operator StyleScale(Vector2 scale)
		{
			return new Scale(scale);
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x0006A0D8 File Offset: 0x000682D8
		public static bool operator ==(StyleScale lhs, StyleScale rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x0006A10C File Offset: 0x0006830C
		public static bool operator !=(StyleScale lhs, StyleScale rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x0006A128 File Offset: 0x00068328
		public static implicit operator StyleScale(StyleKeyword keyword)
		{
			return new StyleScale(keyword);
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x0006A140 File Offset: 0x00068340
		public static implicit operator StyleScale(Scale v)
		{
			return new StyleScale(v);
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x0006A158 File Offset: 0x00068358
		public bool Equals(StyleScale other)
		{
			return other == this;
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x0006A178 File Offset: 0x00068378
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleScale)
			{
				StyleScale styleScale = (StyleScale)obj;
				flag = this.Equals(styleScale);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x0006A1A4 File Offset: 0x000683A4
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x0006A1D8 File Offset: 0x000683D8
		public override string ToString()
		{
			return this.DebugString<Scale>();
		}

		// Token: 0x04000AF8 RID: 2808
		private Scale m_Value;

		// Token: 0x04000AF9 RID: 2809
		private StyleKeyword m_Keyword;
	}
}
