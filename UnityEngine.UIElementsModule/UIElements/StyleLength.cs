using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000306 RID: 774
	public struct StyleLength : IStyleValue<Length>, IEquatable<StyleLength>
	{
		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001A9E RID: 6814 RVA: 0x000698C0 File Offset: 0x00067AC0
		// (set) Token: 0x06001A9F RID: 6815 RVA: 0x00069908 File Offset: 0x00067B08
		public Length value
		{
			get
			{
				bool flag = this.m_Keyword == StyleKeyword.Auto || this.m_Keyword == StyleKeyword.None || this.m_Keyword == StyleKeyword.Undefined;
				Length length;
				if (flag)
				{
					length = this.m_Value;
				}
				else
				{
					length = default(Length);
				}
				return length;
			}
			set
			{
				bool flag = value.IsAuto();
				if (flag)
				{
					this.m_Keyword = StyleKeyword.Auto;
				}
				else
				{
					bool flag2 = value.IsNone();
					if (flag2)
					{
						this.m_Keyword = StyleKeyword.None;
					}
					else
					{
						this.m_Keyword = StyleKeyword.Undefined;
					}
				}
				this.m_Value = value;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x0006994C File Offset: 0x00067B4C
		// (set) Token: 0x06001AA1 RID: 6817 RVA: 0x00069964 File Offset: 0x00067B64
		public StyleKeyword keyword
		{
			get
			{
				return this.m_Keyword;
			}
			set
			{
				this.m_Keyword = value;
				bool flag = this.m_Keyword == StyleKeyword.Auto;
				if (flag)
				{
					this.m_Value = Length.Auto();
				}
				else
				{
					bool flag2 = this.m_Keyword == StyleKeyword.None;
					if (flag2)
					{
						this.m_Value = Length.None();
					}
					else
					{
						bool flag3 = this.m_Keyword > StyleKeyword.Undefined;
						if (flag3)
						{
							this.m_Value = default(Length);
						}
					}
				}
			}
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x000699CC File Offset: 0x00067BCC
		public StyleLength(float v)
		{
			this = new StyleLength(new Length(v, LengthUnit.Pixel), StyleKeyword.Undefined);
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x000699DE File Offset: 0x00067BDE
		public StyleLength(Length v)
		{
			this = new StyleLength(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x000699EC File Offset: 0x00067BEC
		public StyleLength(StyleKeyword keyword)
		{
			this = new StyleLength(default(Length), keyword);
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00069A0C File Offset: 0x00067C0C
		internal StyleLength(Length v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
			bool flag = v.IsAuto();
			if (flag)
			{
				this.m_Keyword = StyleKeyword.Auto;
			}
			else
			{
				bool flag2 = v.IsNone();
				if (flag2)
				{
					this.m_Keyword = StyleKeyword.None;
				}
			}
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x00069A50 File Offset: 0x00067C50
		public static bool operator ==(StyleLength lhs, StyleLength rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00069A84 File Offset: 0x00067C84
		public static bool operator !=(StyleLength lhs, StyleLength rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x00069AA0 File Offset: 0x00067CA0
		public static implicit operator StyleLength(StyleKeyword keyword)
		{
			return new StyleLength(keyword);
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x00069AB8 File Offset: 0x00067CB8
		public static implicit operator StyleLength(float v)
		{
			return new StyleLength(v);
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x00069AD0 File Offset: 0x00067CD0
		public static implicit operator StyleLength(Length v)
		{
			return new StyleLength(v);
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x00069AE8 File Offset: 0x00067CE8
		public bool Equals(StyleLength other)
		{
			return other == this;
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x00069B08 File Offset: 0x00067D08
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleLength)
			{
				StyleLength styleLength = (StyleLength)obj;
				flag = this.Equals(styleLength);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00069B34 File Offset: 0x00067D34
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x00069B68 File Offset: 0x00067D68
		public override string ToString()
		{
			return this.DebugString<Length>();
		}

		// Token: 0x04000AF2 RID: 2802
		private Length m_Value;

		// Token: 0x04000AF3 RID: 2803
		private StyleKeyword m_Keyword;
	}
}
