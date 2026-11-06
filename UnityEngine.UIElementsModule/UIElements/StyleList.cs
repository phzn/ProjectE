using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x02000307 RID: 775
	public struct StyleList<T> : IStyleValue<List<T>>, IEquatable<StyleList<T>>
	{
		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001AAF RID: 6831 RVA: 0x00069B8C File Offset: 0x00067D8C
		// (set) Token: 0x06001AB0 RID: 6832 RVA: 0x00069BAF File Offset: 0x00067DAF
		public List<T> value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : null;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x00069BC0 File Offset: 0x00067DC0
		// (set) Token: 0x06001AB2 RID: 6834 RVA: 0x00069BD8 File Offset: 0x00067DD8
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

		// Token: 0x06001AB3 RID: 6835 RVA: 0x00069BE2 File Offset: 0x00067DE2
		public StyleList(List<T> v)
		{
			this = new StyleList<T>(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x00069BEE File Offset: 0x00067DEE
		public StyleList(StyleKeyword keyword)
		{
			this = new StyleList<T>(null, keyword);
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x00069BFA File Offset: 0x00067DFA
		internal StyleList(List<T> v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x00069C0C File Offset: 0x00067E0C
		public static bool operator ==(StyleList<T> lhs, StyleList<T> rhs)
		{
			bool flag = lhs.m_Keyword != rhs.m_Keyword;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				List<T> value = lhs.m_Value;
				List<T> value2 = rhs.m_Value;
				bool flag3 = value == value2;
				if (flag3)
				{
					flag2 = true;
				}
				else
				{
					bool flag4 = value == null || value2 == null;
					flag2 = !flag4 && value.Count == value2.Count && value.SequenceEqual<T>(value2);
				}
			}
			return flag2;
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x00069C80 File Offset: 0x00067E80
		public static bool operator !=(StyleList<T> lhs, StyleList<T> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x00069C9C File Offset: 0x00067E9C
		public static implicit operator StyleList<T>(StyleKeyword keyword)
		{
			return new StyleList<T>(keyword);
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x00069CB4 File Offset: 0x00067EB4
		public static implicit operator StyleList<T>(List<T> v)
		{
			return new StyleList<T>(v);
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x00069CCC File Offset: 0x00067ECC
		public bool Equals(StyleList<T> other)
		{
			return other == this;
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x00069CEC File Offset: 0x00067EEC
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleList<T>)
			{
				StyleList<T> styleList = (StyleList<T>)obj;
				flag = this.Equals(styleList);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x00069D18 File Offset: 0x00067F18
		public override int GetHashCode()
		{
			int num = 0;
			bool flag = this.m_Value != null && this.m_Value.Count > 0;
			if (flag)
			{
				num = EqualityComparer<T>.Default.GetHashCode(this.m_Value[0]);
				for (int i = 1; i < this.m_Value.Count; i++)
				{
					num = (num * 397) ^ EqualityComparer<T>.Default.GetHashCode(this.m_Value[i]);
				}
			}
			return (num * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x00069DB0 File Offset: 0x00067FB0
		public override string ToString()
		{
			return this.DebugString<List<T>>();
		}

		// Token: 0x04000AF4 RID: 2804
		private StyleKeyword m_Keyword;

		// Token: 0x04000AF5 RID: 2805
		private List<T> m_Value;
	}
}
