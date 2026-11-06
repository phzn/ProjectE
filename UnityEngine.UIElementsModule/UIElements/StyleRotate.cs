using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000308 RID: 776
	public struct StyleRotate : IStyleValue<Rotate>, IEquatable<StyleRotate>
	{
		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x00069DD4 File Offset: 0x00067FD4
		// (set) Token: 0x06001ABF RID: 6847 RVA: 0x00069E39 File Offset: 0x00068039
		public Rotate value
		{
			get
			{
				StyleKeyword keyword = this.m_Keyword;
				if (!true)
				{
				}
				Rotate rotate;
				switch (keyword)
				{
				case StyleKeyword.Undefined:
					rotate = this.m_Value;
					goto IL_004F;
				case StyleKeyword.Null:
					rotate = Rotate.None();
					goto IL_004F;
				case StyleKeyword.None:
					rotate = Rotate.None();
					goto IL_004F;
				case StyleKeyword.Initial:
					rotate = Rotate.Initial();
					goto IL_004F;
				}
				throw new NotImplementedException();
				IL_004F:
				if (!true)
				{
				}
				return rotate;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x00069E4C File Offset: 0x0006804C
		// (set) Token: 0x06001AC1 RID: 6849 RVA: 0x00069E64 File Offset: 0x00068064
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

		// Token: 0x06001AC2 RID: 6850 RVA: 0x00069E6E File Offset: 0x0006806E
		public StyleRotate(Rotate v)
		{
			this = new StyleRotate(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x00069E7C File Offset: 0x0006807C
		public StyleRotate(StyleKeyword keyword)
		{
			this = new StyleRotate(default(Rotate), keyword);
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x00069E9B File Offset: 0x0006809B
		internal StyleRotate(Rotate v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x00069EAC File Offset: 0x000680AC
		public static bool operator ==(StyleRotate lhs, StyleRotate rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x00069EE0 File Offset: 0x000680E0
		public static bool operator !=(StyleRotate lhs, StyleRotate rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x00069EFC File Offset: 0x000680FC
		public static implicit operator StyleRotate(StyleKeyword keyword)
		{
			return new StyleRotate(keyword);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x00069F14 File Offset: 0x00068114
		public static implicit operator StyleRotate(Rotate v)
		{
			return new StyleRotate(v);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x00069F2C File Offset: 0x0006812C
		public bool Equals(StyleRotate other)
		{
			return other == this;
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x00069F4C File Offset: 0x0006814C
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleRotate)
			{
				StyleRotate styleRotate = (StyleRotate)obj;
				flag = this.Equals(styleRotate);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x00069F78 File Offset: 0x00068178
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00069FAC File Offset: 0x000681AC
		public override string ToString()
		{
			return this.DebugString<Rotate>();
		}

		// Token: 0x04000AF6 RID: 2806
		private Rotate m_Value;

		// Token: 0x04000AF7 RID: 2807
		private StyleKeyword m_Keyword;
	}
}
