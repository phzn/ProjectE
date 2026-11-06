using System;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.UIElements
{
	// Token: 0x02000301 RID: 769
	public struct StyleEnum<T> : IStyleValue<T>, IEquatable<StyleEnum<T>> where T : struct, IConvertible
	{
		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001A4C RID: 6732 RVA: 0x00068FD4 File Offset: 0x000671D4
		// (set) Token: 0x06001A4D RID: 6733 RVA: 0x00068FFF File Offset: 0x000671FF
		public T value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(T);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x00069010 File Offset: 0x00067210
		// (set) Token: 0x06001A4F RID: 6735 RVA: 0x00069028 File Offset: 0x00067228
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

		// Token: 0x06001A50 RID: 6736 RVA: 0x00069032 File Offset: 0x00067232
		public StyleEnum(T v)
		{
			this = new StyleEnum<T>(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x00069040 File Offset: 0x00067240
		public StyleEnum(StyleKeyword keyword)
		{
			this = new StyleEnum<T>(default(T), keyword);
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x0006905F File Offset: 0x0006725F
		internal StyleEnum(T v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x00069070 File Offset: 0x00067270
		public static bool operator ==(StyleEnum<T> lhs, StyleEnum<T> rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && UnsafeUtility.EnumEquals<T>(lhs.m_Value, rhs.m_Value);
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x000690A4 File Offset: 0x000672A4
		public static bool operator !=(StyleEnum<T> lhs, StyleEnum<T> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x000690C0 File Offset: 0x000672C0
		public static implicit operator StyleEnum<T>(StyleKeyword keyword)
		{
			return new StyleEnum<T>(keyword);
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x000690D8 File Offset: 0x000672D8
		public static implicit operator StyleEnum<T>(T v)
		{
			return new StyleEnum<T>(v);
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x000690F0 File Offset: 0x000672F0
		public bool Equals(StyleEnum<T> other)
		{
			return other == this;
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x00069110 File Offset: 0x00067310
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleEnum<T>)
			{
				StyleEnum<T> styleEnum = (StyleEnum<T>)obj;
				flag = this.Equals(styleEnum);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x0006913C File Offset: 0x0006733C
		public override int GetHashCode()
		{
			return (UnsafeUtility.EnumToInt<T>(this.m_Value) * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00069168 File Offset: 0x00067368
		public override string ToString()
		{
			return this.DebugString<T>();
		}

		// Token: 0x04000AE8 RID: 2792
		private T m_Value;

		// Token: 0x04000AE9 RID: 2793
		private StyleKeyword m_Keyword;
	}
}
