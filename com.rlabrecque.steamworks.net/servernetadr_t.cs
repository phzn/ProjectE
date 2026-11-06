using System;

namespace Steamworks
{
	// Token: 0x02000188 RID: 392
	[Serializable]
	public struct servernetadr_t
	{
		// Token: 0x0600093F RID: 2367 RVA: 0x0000EC03 File Offset: 0x0000CE03
		public void Init(uint ip, ushort usQueryPort, ushort usConnectionPort)
		{
			this.m_unIP = ip;
			this.m_usQueryPort = usQueryPort;
			this.m_usConnectionPort = usConnectionPort;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0000EC1A File Offset: 0x0000CE1A
		public ushort GetQueryPort()
		{
			return this.m_usQueryPort;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0000EC22 File Offset: 0x0000CE22
		public void SetQueryPort(ushort usPort)
		{
			this.m_usQueryPort = usPort;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0000EC2B File Offset: 0x0000CE2B
		public ushort GetConnectionPort()
		{
			return this.m_usConnectionPort;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0000EC33 File Offset: 0x0000CE33
		public void SetConnectionPort(ushort usPort)
		{
			this.m_usConnectionPort = usPort;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0000EC3C File Offset: 0x0000CE3C
		public uint GetIP()
		{
			return this.m_unIP;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0000EC44 File Offset: 0x0000CE44
		public void SetIP(uint unIP)
		{
			this.m_unIP = unIP;
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0000EC4D File Offset: 0x0000CE4D
		public string GetConnectionAddressString()
		{
			return servernetadr_t.ToString(this.m_unIP, this.m_usConnectionPort);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0000EC60 File Offset: 0x0000CE60
		public string GetQueryAddressString()
		{
			return servernetadr_t.ToString(this.m_unIP, this.m_usQueryPort);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0000EC74 File Offset: 0x0000CE74
		public static string ToString(uint unIP, ushort usPort)
		{
			return string.Format("{0}.{1}.{2}.{3}:{4}", new object[]
			{
				(ulong)(unIP >> 24) & 255UL,
				(ulong)(unIP >> 16) & 255UL,
				(ulong)(unIP >> 8) & 255UL,
				(ulong)unIP & 255UL,
				usPort
			});
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0000ECE6 File Offset: 0x0000CEE6
		public static bool operator <(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP < y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort < y.m_usQueryPort);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0000ED16 File Offset: 0x0000CF16
		public static bool operator >(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP > y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort > y.m_usQueryPort);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0000ED46 File Offset: 0x0000CF46
		public override bool Equals(object other)
		{
			return other is servernetadr_t && this == (servernetadr_t)other;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0000ED63 File Offset: 0x0000CF63
		public override int GetHashCode()
		{
			return this.m_unIP.GetHashCode() + this.m_usQueryPort.GetHashCode() + this.m_usConnectionPort.GetHashCode();
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0000ED88 File Offset: 0x0000CF88
		public static bool operator ==(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP == y.m_unIP && x.m_usQueryPort == y.m_usQueryPort && x.m_usConnectionPort == y.m_usConnectionPort;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0000EDB6 File Offset: 0x0000CFB6
		public static bool operator !=(servernetadr_t x, servernetadr_t y)
		{
			return !(x == y);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0000EDC2 File Offset: 0x0000CFC2
		public bool Equals(servernetadr_t other)
		{
			return this.m_unIP == other.m_unIP && this.m_usQueryPort == other.m_usQueryPort && this.m_usConnectionPort == other.m_usConnectionPort;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0000EDF0 File Offset: 0x0000CFF0
		public int CompareTo(servernetadr_t other)
		{
			return this.m_unIP.CompareTo(other.m_unIP) + this.m_usQueryPort.CompareTo(other.m_usQueryPort) + this.m_usConnectionPort.CompareTo(other.m_usConnectionPort);
		}

		// Token: 0x04000B69 RID: 2921
		private ushort m_usConnectionPort;

		// Token: 0x04000B6A RID: 2922
		private ushort m_usQueryPort;

		// Token: 0x04000B6B RID: 2923
		private uint m_unIP;
	}
}
