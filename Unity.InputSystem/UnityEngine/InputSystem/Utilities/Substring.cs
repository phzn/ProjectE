using System;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200014C RID: 332
	internal struct Substring : IComparable<Substring>, IEquatable<Substring>
	{
		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001227 RID: 4647 RVA: 0x0005653D File Offset: 0x0005473D
		public bool isEmpty
		{
			get
			{
				return this.m_Length == 0;
			}
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00056548 File Offset: 0x00054748
		public Substring(string str)
		{
			this.m_String = str;
			this.m_Index = 0;
			if (str != null)
			{
				this.m_Length = str.Length;
				return;
			}
			this.m_Length = 0;
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0005656F File Offset: 0x0005476F
		public Substring(string str, int index, int length)
		{
			this.m_String = str;
			this.m_Index = index;
			this.m_Length = length;
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00056586 File Offset: 0x00054786
		public Substring(string str, int index)
		{
			this.m_String = str;
			this.m_Index = index;
			this.m_Length = str.Length - index;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x000565A4 File Offset: 0x000547A4
		public override bool Equals(object obj)
		{
			if (obj is Substring)
			{
				Substring substring = (Substring)obj;
				return this.Equals(substring);
			}
			string text = obj as string;
			return text != null && this.Equals(text);
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x000565DC File Offset: 0x000547DC
		public bool Equals(string other)
		{
			if (string.IsNullOrEmpty(other))
			{
				return this.m_Length == 0;
			}
			if (other.Length != this.m_Length)
			{
				return false;
			}
			for (int i = 0; i < this.m_Length; i++)
			{
				if (other[i] != this.m_String[this.m_Index + i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0005663B File Offset: 0x0005483B
		public bool Equals(Substring other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00056647 File Offset: 0x00054847
		public bool Equals(InternedString other)
		{
			return this.length == other.length && string.Compare(this.m_String, this.m_Index, other.ToString(), 0, this.length, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00056683 File Offset: 0x00054883
		public int CompareTo(Substring other)
		{
			return Substring.Compare(this, other, StringComparison.CurrentCulture);
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00056694 File Offset: 0x00054894
		public static int Compare(Substring left, Substring right, StringComparison comparison)
		{
			if (left.m_Length == right.m_Length)
			{
				return string.Compare(left.m_String, left.m_Index, right.m_String, right.m_Index, left.m_Length, comparison);
			}
			if (left.m_Length < right.m_Length)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x000566E8 File Offset: 0x000548E8
		public bool StartsWith(string str)
		{
			if (str.Length > this.length)
			{
				return false;
			}
			for (int i = 0; i < str.Length; i++)
			{
				if (this.m_String[this.m_Index + i] != str[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00056735 File Offset: 0x00054935
		public string Substr(int index = 0, int length = -1)
		{
			if (length < 0)
			{
				length = this.length - index;
			}
			return this.m_String.Substring(this.m_Index + index, length);
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00056759 File Offset: 0x00054959
		public override string ToString()
		{
			if (this.m_String == null)
			{
				return string.Empty;
			}
			return this.m_String.Substring(this.m_Index, this.m_Length);
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00056780 File Offset: 0x00054980
		public override int GetHashCode()
		{
			if (this.m_String == null)
			{
				return 0;
			}
			if (this.m_Index == 0 && this.m_Length == this.m_String.Length)
			{
				return this.m_String.GetHashCode();
			}
			return this.ToString().GetHashCode();
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x000567CF File Offset: 0x000549CF
		public static bool operator ==(Substring a, Substring b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x000567D9 File Offset: 0x000549D9
		public static bool operator !=(Substring a, Substring b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x000567E6 File Offset: 0x000549E6
		public static bool operator ==(Substring a, InternedString b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x000567F0 File Offset: 0x000549F0
		public static bool operator !=(Substring a, InternedString b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x000567FD File Offset: 0x000549FD
		public static bool operator ==(InternedString a, Substring b)
		{
			return b.Equals(a);
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00056807 File Offset: 0x00054A07
		public static bool operator !=(InternedString a, Substring b)
		{
			return !b.Equals(a);
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00056814 File Offset: 0x00054A14
		public static implicit operator Substring(string s)
		{
			return new Substring(s);
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x0600123C RID: 4668 RVA: 0x0005681C File Offset: 0x00054A1C
		public int length
		{
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x0600123D RID: 4669 RVA: 0x00056824 File Offset: 0x00054A24
		public int index
		{
			get
			{
				return this.m_Index;
			}
		}

		// Token: 0x170004E8 RID: 1256
		public char this[int index]
		{
			get
			{
				if (index < 0 || index >= this.m_Length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.m_String[this.m_Index + index];
			}
		}

		// Token: 0x0400073C RID: 1852
		private readonly string m_String;

		// Token: 0x0400073D RID: 1853
		private readonly int m_Index;

		// Token: 0x0400073E RID: 1854
		private readonly int m_Length;
	}
}
