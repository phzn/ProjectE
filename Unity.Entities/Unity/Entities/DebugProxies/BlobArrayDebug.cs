using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.DebugProxies
{
	// Token: 0x020004C4 RID: 1220
	[DebuggerDisplay("{Description,nq}")]
	internal struct BlobArrayDebug<T> where T : struct
	{
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06001D52 RID: 7506 RVA: 0x0006643C File Offset: 0x0006463C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public string Description
		{
			get
			{
				return string.Concat(new string[]
				{
					"BlobArray<",
					typeof(T).Name,
					"> (",
					this.Length.ToString(),
					")"
				});
			}
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x0006648F File Offset: 0x0006468F
		public override string ToString()
		{
			return this.Description;
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06001D54 RID: 7508 RVA: 0x00066497 File Offset: 0x00064697
		public int Length
		{
			get
			{
				return UnsafeUtility.AsRef<BlobArray<T>>(this.m_BasePtr).Length;
			}
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x000664A9 File Offset: 0x000646A9
		public BlobArrayDebug(IntPtr basePtr)
		{
			this.m_BasePtr = basePtr.ToPointer();
			this.m_Entries = null;
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x000664C0 File Offset: 0x000646C0
		private unsafe void Init()
		{
			if (this.m_Entries != null)
			{
				return;
			}
			ref BlobArray<T> ptr = ref UnsafeUtility.AsRef<BlobArray<T>>(this.m_BasePtr);
			int offsetPtr = ptr.m_OffsetPtr;
			byte* ptr2 = (byte*)this.m_BasePtr + offsetPtr;
			object[] array = new object[ptr.Length];
			int length = ptr.Length;
			int num = UnsafeUtility.SizeOf<T>();
			for (int i = 0; i < length; i++)
			{
				byte* ptr3 = ptr2 + num * i;
				array[i] = BlobProxy.UnpackValue((void*)ptr3, typeof(T));
			}
			this.m_Entries = array;
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06001D57 RID: 7511 RVA: 0x0006653F File Offset: 0x0006473F
		public object[] Entries
		{
			get
			{
				this.Init();
				return this.m_Entries;
			}
		}

		// Token: 0x04000A8A RID: 2698
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private unsafe readonly void* m_BasePtr;

		// Token: 0x04000A8B RID: 2699
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object[] m_Entries;
	}
}
