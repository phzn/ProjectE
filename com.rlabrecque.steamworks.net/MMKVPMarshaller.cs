using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200017A RID: 378
	public class MMKVPMarshaller
	{
		// Token: 0x060008CE RID: 2254 RVA: 0x0000D1D0 File Offset: 0x0000B3D0
		public MMKVPMarshaller(MatchMakingKeyValuePair_t[] filters)
		{
			if (filters == null)
			{
				return;
			}
			int num = Marshal.SizeOf<MatchMakingKeyValuePair_t>();
			this.m_pNativeArray = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>() * filters.Length);
			this.m_pArrayEntries = Marshal.AllocHGlobal(num * filters.Length);
			for (int i = 0; i < filters.Length; i++)
			{
				Marshal.StructureToPtr<MatchMakingKeyValuePair_t>(filters[i], new IntPtr(this.m_pArrayEntries.ToInt64() + (long)(i * num)), false);
			}
			Marshal.WriteIntPtr(this.m_pNativeArray, this.m_pArrayEntries);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0000D254 File Offset: 0x0000B454
		~MMKVPMarshaller()
		{
			if (this.m_pArrayEntries != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pArrayEntries);
			}
			if (this.m_pNativeArray != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pNativeArray);
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
		public static implicit operator IntPtr(MMKVPMarshaller that)
		{
			return that.m_pNativeArray;
		}

		// Token: 0x04000B10 RID: 2832
		private IntPtr m_pNativeArray;

		// Token: 0x04000B11 RID: 2833
		private IntPtr m_pArrayEntries;
	}
}
