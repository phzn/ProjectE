using System;
using System.Runtime.InteropServices;

namespace Unity.Serialization.Json
{
	// Token: 0x0200006B RID: 107
	internal struct ManagedCharBuffer
	{
		// Token: 0x06000283 RID: 643 RVA: 0x0000B0EC File Offset: 0x000092EC
		public unsafe char* GetUnsafePtr()
		{
			return this.m_PinnedPtr;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000B0F4 File Offset: 0x000092F4
		public char[] GetManagedArray()
		{
			return this.m_ManagedBuffer;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000B0FC File Offset: 0x000092FC
		public unsafe ManagedCharBuffer(int length)
		{
			this.m_ManagedBuffer = new char[length];
			this.m_GCHandle = GCHandle.Alloc(this.m_ManagedBuffer, GCHandleType.Pinned);
			this.m_PinnedPtr = (char*)this.m_GCHandle.AddrOfPinnedObject().ToPointer();
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000B140 File Offset: 0x00009340
		public void Dispose()
		{
			this.m_GCHandle.Free();
		}

		// Token: 0x04000149 RID: 329
		private readonly char[] m_ManagedBuffer;

		// Token: 0x0400014A RID: 330
		private readonly GCHandle m_GCHandle;

		// Token: 0x0400014B RID: 331
		private unsafe readonly char* m_PinnedPtr;
	}
}
