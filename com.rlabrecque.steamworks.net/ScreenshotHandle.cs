using System;

namespace Steamworks
{
	// Token: 0x020001B6 RID: 438
	[Serializable]
	public struct ScreenshotHandle : IEquatable<ScreenshotHandle>, IComparable<ScreenshotHandle>
	{
		// Token: 0x06000AF8 RID: 2808 RVA: 0x000107C5 File Offset: 0x0000E9C5
		public ScreenshotHandle(uint value)
		{
			this.m_ScreenshotHandle = value;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x000107CE File Offset: 0x0000E9CE
		public override string ToString()
		{
			return this.m_ScreenshotHandle.ToString();
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x000107DB File Offset: 0x0000E9DB
		public override bool Equals(object other)
		{
			return other is ScreenshotHandle && this == (ScreenshotHandle)other;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x000107F8 File Offset: 0x0000E9F8
		public override int GetHashCode()
		{
			return this.m_ScreenshotHandle.GetHashCode();
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00010805 File Offset: 0x0000EA05
		public static bool operator ==(ScreenshotHandle x, ScreenshotHandle y)
		{
			return x.m_ScreenshotHandle == y.m_ScreenshotHandle;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00010815 File Offset: 0x0000EA15
		public static bool operator !=(ScreenshotHandle x, ScreenshotHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00010821 File Offset: 0x0000EA21
		public static explicit operator ScreenshotHandle(uint value)
		{
			return new ScreenshotHandle(value);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00010829 File Offset: 0x0000EA29
		public static explicit operator uint(ScreenshotHandle that)
		{
			return that.m_ScreenshotHandle;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00010831 File Offset: 0x0000EA31
		public bool Equals(ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle == other.m_ScreenshotHandle;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00010841 File Offset: 0x0000EA41
		public int CompareTo(ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);
		}

		// Token: 0x04000BED RID: 3053
		public static readonly ScreenshotHandle Invalid = new ScreenshotHandle(0U);

		// Token: 0x04000BEE RID: 3054
		public uint m_ScreenshotHandle;
	}
}
