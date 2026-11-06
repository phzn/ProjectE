using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200020B RID: 523
	public readonly struct SafeBitRef
	{
		// Token: 0x06000C46 RID: 3142 RVA: 0x000387B0 File Offset: 0x000369B0
		public unsafe SafeBitRef(ulong* ptr, int offsetInBits)
		{
			this.m_Ptr = ptr;
			this.m_OffsetInBits = offsetInBits;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x000387C0 File Offset: 0x000369C0
		public static SafeBitRef Null
		{
			get
			{
				return new SafeBitRef(null, 0);
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x000387CA File Offset: 0x000369CA
		public SafeBitRef Offset(int offsetInBits)
		{
			return new SafeBitRef(this.m_Ptr, this.m_OffsetInBits + offsetInBits);
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x000387DF File Offset: 0x000369DF
		public bool IsValid
		{
			get
			{
				return this.m_Ptr != null;
			}
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x000387EE File Offset: 0x000369EE
		public bool GetBit()
		{
			return Bitwise.IsSet(this.m_Ptr, this.m_OffsetInBits);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00038801 File Offset: 0x00036A01
		public void SetBit(bool value)
		{
			UnsafeBitArray.Set(this.m_Ptr, this.m_OffsetInBits, value);
		}

		// Token: 0x0400055E RID: 1374
		private unsafe readonly ulong* m_Ptr;

		// Token: 0x0400055F RID: 1375
		private readonly int m_OffsetInBits;
	}
}
