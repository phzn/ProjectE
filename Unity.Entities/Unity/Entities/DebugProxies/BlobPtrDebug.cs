using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.DebugProxies
{
	// Token: 0x020004C6 RID: 1222
	[DebuggerTypeProxy(typeof(BlobPtrProxy<>))]
	[DebuggerDisplay("{Description,nq}")]
	internal struct BlobPtrDebug<T> where T : struct
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001D5D RID: 7517 RVA: 0x00066624 File Offset: 0x00064824
		public string Description
		{
			get
			{
				return "BlobPtr<" + typeof(T).Name + ">";
			}
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x00066644 File Offset: 0x00064844
		public override string ToString()
		{
			return this.Description;
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x0006664C File Offset: 0x0006484C
		public BlobPtrDebug(IntPtr basePtr)
		{
			this.m_BasePtr = basePtr.ToPointer();
			this.m_Value = null;
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x00066664 File Offset: 0x00064864
		private unsafe void Init()
		{
			if (this.m_Value != null)
			{
				return;
			}
			byte* ptr = (byte*)this.m_BasePtr + UnsafeUtility.AsRef<BlobPtr<T>>(this.m_BasePtr).m_OffsetPtr;
			this.m_Value = BlobProxy.UnpackValue((void*)ptr, typeof(T));
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001D61 RID: 7521 RVA: 0x000666A8 File Offset: 0x000648A8
		public object Value
		{
			get
			{
				this.Init();
				return this.m_Value;
			}
		}

		// Token: 0x04000A8E RID: 2702
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private unsafe readonly void* m_BasePtr;

		// Token: 0x04000A8F RID: 2703
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object m_Value;
	}
}
