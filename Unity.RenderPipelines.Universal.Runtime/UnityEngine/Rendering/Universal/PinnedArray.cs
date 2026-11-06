using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200008A RID: 138
	internal struct PinnedArray<T> : IDisposable where T : struct
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0001F1C7 File Offset: 0x0001D3C7
		public int length
		{
			get
			{
				if (this.managedArray == null)
				{
					return 0;
				}
				return this.managedArray.Length;
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001F1DB File Offset: 0x0001D3DB
		public unsafe PinnedArray(int length)
		{
			this.managedArray = new T[length];
			this.handle = GCHandle.Alloc(this.managedArray, GCHandleType.Pinned);
			this.nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.handle.AddrOfPinnedObject(), length, Allocator.None);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001F218 File Offset: 0x0001D418
		public void Dispose()
		{
			if (this.managedArray == null)
			{
				return;
			}
			this.handle.Free();
			this = default(PinnedArray<T>);
		}

		// Token: 0x040003CA RID: 970
		public T[] managedArray;

		// Token: 0x040003CB RID: 971
		public GCHandle handle;

		// Token: 0x040003CC RID: 972
		public NativeArray<T> nativeArray;
	}
}
