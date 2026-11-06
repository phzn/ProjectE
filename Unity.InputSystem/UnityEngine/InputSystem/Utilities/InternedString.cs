using System;
using System.Globalization;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000136 RID: 310
	public struct InternedString : IEquatable<InternedString>, IComparable<InternedString>
	{
		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x00052582 File Offset: 0x00050782
		public int length
		{
			get
			{
				string stringLowerCase = this.m_StringLowerCase;
				if (stringLowerCase == null)
				{
					return 0;
				}
				return stringLowerCase.Length;
			}
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00052595 File Offset: 0x00050795
		public InternedString(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				this.m_StringOriginalCase = null;
				this.m_StringLowerCase = null;
				return;
			}
			this.m_StringOriginalCase = string.Intern(text);
			this.m_StringLowerCase = string.Intern(text.ToLower(CultureInfo.InvariantCulture));
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x000525D0 File Offset: 0x000507D0
		public bool IsEmpty()
		{
			return this.m_StringLowerCase == null;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x000525DB File Offset: 0x000507DB
		public string ToLower()
		{
			return this.m_StringLowerCase;
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x000525E4 File Offset: 0x000507E4
		public override bool Equals(object obj)
		{
			if (obj is InternedString)
			{
				InternedString internedString = (InternedString)obj;
				return this.Equals(internedString);
			}
			string text = obj as string;
			if (text == null)
			{
				return false;
			}
			if (this.m_StringLowerCase == null)
			{
				return string.IsNullOrEmpty(text);
			}
			return string.Equals(this.m_StringLowerCase, text.ToLower(CultureInfo.InvariantCulture));
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00052639 File Offset: 0x00050839
		public bool Equals(InternedString other)
		{
			return this.m_StringLowerCase == other.m_StringLowerCase;
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00052649 File Offset: 0x00050849
		public int CompareTo(InternedString other)
		{
			return string.Compare(this.m_StringLowerCase, other.m_StringLowerCase, StringComparison.InvariantCultureIgnoreCase);
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0005265D File Offset: 0x0005085D
		public override int GetHashCode()
		{
			if (this.m_StringLowerCase == null)
			{
				return 0;
			}
			return this.m_StringLowerCase.GetHashCode();
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00052674 File Offset: 0x00050874
		public override string ToString()
		{
			return this.m_StringOriginalCase ?? string.Empty;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00052685 File Offset: 0x00050885
		public static bool operator ==(InternedString a, InternedString b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0005268F File Offset: 0x0005088F
		public static bool operator !=(InternedString a, InternedString b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x0005269C File Offset: 0x0005089C
		public static bool operator ==(InternedString a, string b)
		{
			return string.Compare(a.m_StringLowerCase, b, StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x000526AE File Offset: 0x000508AE
		public static bool operator !=(InternedString a, string b)
		{
			return string.Compare(a.m_StringLowerCase, b, StringComparison.InvariantCultureIgnoreCase) != 0;
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x000526C0 File Offset: 0x000508C0
		public static bool operator ==(string a, InternedString b)
		{
			return string.Compare(a, b.m_StringLowerCase, StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x000526D2 File Offset: 0x000508D2
		public static bool operator !=(string a, InternedString b)
		{
			return string.Compare(a, b.m_StringLowerCase, StringComparison.InvariantCultureIgnoreCase) != 0;
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x000526E4 File Offset: 0x000508E4
		public static bool operator <(InternedString left, InternedString right)
		{
			return string.Compare(left.m_StringLowerCase, right.m_StringLowerCase, StringComparison.InvariantCultureIgnoreCase) < 0;
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x000526FB File Offset: 0x000508FB
		public static bool operator >(InternedString left, InternedString right)
		{
			return string.Compare(left.m_StringLowerCase, right.m_StringLowerCase, StringComparison.InvariantCultureIgnoreCase) > 0;
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00052712 File Offset: 0x00050912
		public static implicit operator string(InternedString str)
		{
			return str.ToString();
		}

		// Token: 0x0400070E RID: 1806
		private readonly string m_StringOriginalCase;

		// Token: 0x0400070F RID: 1807
		private readonly string m_StringLowerCase;
	}
}
