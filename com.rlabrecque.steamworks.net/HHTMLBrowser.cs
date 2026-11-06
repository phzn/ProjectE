using System;

namespace Steamworks
{
	// Token: 0x02000191 RID: 401
	[Serializable]
	public struct HHTMLBrowser : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser>
	{
		// Token: 0x060009B1 RID: 2481 RVA: 0x0000F5B3 File Offset: 0x0000D7B3
		public HHTMLBrowser(uint value)
		{
			this.m_HHTMLBrowser = value;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0000F5BC File Offset: 0x0000D7BC
		public override string ToString()
		{
			return this.m_HHTMLBrowser.ToString();
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0000F5C9 File Offset: 0x0000D7C9
		public override bool Equals(object other)
		{
			return other is HHTMLBrowser && this == (HHTMLBrowser)other;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0000F5E6 File Offset: 0x0000D7E6
		public override int GetHashCode()
		{
			return this.m_HHTMLBrowser.GetHashCode();
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0000F5F3 File Offset: 0x0000D7F3
		public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y)
		{
			return x.m_HHTMLBrowser == y.m_HHTMLBrowser;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0000F603 File Offset: 0x0000D803
		public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y)
		{
			return !(x == y);
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0000F60F File Offset: 0x0000D80F
		public static explicit operator HHTMLBrowser(uint value)
		{
			return new HHTMLBrowser(value);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0000F617 File Offset: 0x0000D817
		public static explicit operator uint(HHTMLBrowser that)
		{
			return that.m_HHTMLBrowser;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0000F61F File Offset: 0x0000D81F
		public bool Equals(HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser == other.m_HHTMLBrowser;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0000F62F File Offset: 0x0000D82F
		public int CompareTo(HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);
		}

		// Token: 0x04000B83 RID: 2947
		public static readonly HHTMLBrowser Invalid = new HHTMLBrowser(0U);

		// Token: 0x04000B84 RID: 2948
		public uint m_HHTMLBrowser;
	}
}
