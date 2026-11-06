using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200016C RID: 364
	public struct MatchMakingKeyValuePair_t
	{
		// Token: 0x0600088E RID: 2190 RVA: 0x0000C50C File Offset: 0x0000A70C
		private MatchMakingKeyValuePair_t(string strKey, string strValue)
		{
			this.m_szKey = strKey;
			this.m_szValue = strValue;
		}

		// Token: 0x04000AD8 RID: 2776
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szKey;

		// Token: 0x04000AD9 RID: 2777
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szValue;
	}
}
