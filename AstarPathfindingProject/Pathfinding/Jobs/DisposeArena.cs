using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Pathfinding.Jobs
{
	// Token: 0x02000188 RID: 392
	public class DisposeArena
	{
		// Token: 0x06000BD3 RID: 3027 RVA: 0x000416CD File Offset: 0x0003F8CD
		public unsafe void Add<[IsUnmanaged] T>(NativeArray<T> data) where T : struct, ValueType
		{
			if (this.buffer == null)
			{
				this.buffer = ListPool<NativeArray<byte>>.Claim();
			}
			this.buffer.Add(*UnsafeUtility.As<NativeArray<T>, NativeArray<byte>>(ref data));
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000416F9 File Offset: 0x0003F8F9
		public unsafe void Add<[IsUnmanaged] T>(NativeList<T> data) where T : struct, ValueType
		{
			if (this.buffer2 == null)
			{
				this.buffer2 = ListPool<NativeList<byte>>.Claim();
			}
			this.buffer2.Add(*UnsafeUtility.As<NativeList<T>, NativeList<byte>>(ref data));
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00041725 File Offset: 0x0003F925
		public unsafe void Add<[IsUnmanaged] T>(NativeQueue<T> data) where T : struct, ValueType
		{
			if (this.buffer3 == null)
			{
				this.buffer3 = ListPool<NativeQueue<byte>>.Claim();
			}
			this.buffer3.Add(*UnsafeUtility.As<NativeQueue<T>, NativeQueue<byte>>(ref data));
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00041754 File Offset: 0x0003F954
		public unsafe void Add<[IsUnmanaged] T>(UnsafeList<T> data) where T : struct, ValueType
		{
			if (this.ptrBuffer == null)
			{
				this.ptrBuffer = ListPool<ValueTuple<IntPtr, AllocatorManager.AllocatorHandle, int, int>>.Claim();
			}
			this.ptrBuffer.Add(new ValueTuple<IntPtr, AllocatorManager.AllocatorHandle, int, int>((IntPtr)((void*)data.Ptr), data.Allocator, UnsafeUtility.SizeOf<T>() * data.Capacity, UnsafeUtility.AlignOf<T>()));
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000417A7 File Offset: 0x0003F9A7
		public unsafe void Add<[IsUnmanaged] T>(UnsafeSpan<T> data) where T : struct, ValueType
		{
			if (this.ptrBufferTracked == null)
			{
				this.ptrBufferTracked = ListPool<ValueTuple<IntPtr, Allocator>>.Claim();
			}
			this.ptrBufferTracked.Add(new ValueTuple<IntPtr, Allocator>((IntPtr)((void*)data.ptr), data.Allocator));
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x000417E0 File Offset: 0x0003F9E0
		public unsafe void Remove<[IsUnmanaged] T>(NativeArray<T> data) where T : struct, ValueType
		{
			if (this.buffer == null)
			{
				return;
			}
			void* unsafeBufferPointerWithoutChecks = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(data);
			for (int i = 0; i < this.buffer.Count; i++)
			{
				if (NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(this.buffer[i]) == unsafeBufferPointerWithoutChecks)
				{
					this.buffer.RemoveAtSwapBack<NativeArray<byte>>(i);
					return;
				}
			}
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00041834 File Offset: 0x0003FA34
		public void Add<T>(T data) where T : IArenaDisposable
		{
			data.DisposeWith(this);
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00041844 File Offset: 0x0003FA44
		public void Add(GCHandle handle)
		{
			if (this.gcHandles == null)
			{
				this.gcHandles = ListPool<GCHandle>.Claim();
			}
			this.gcHandles.Add(handle);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00041868 File Offset: 0x0003FA68
		public unsafe void DisposeAll()
		{
			if (this.buffer != null)
			{
				for (int i = 0; i < this.buffer.Count; i++)
				{
					this.buffer[i].Dispose();
				}
				ListPool<NativeArray<byte>>.Release(ref this.buffer);
			}
			if (this.buffer2 != null)
			{
				for (int j = 0; j < this.buffer2.Count; j++)
				{
					this.buffer2[j].Dispose();
				}
				ListPool<NativeList<byte>>.Release(ref this.buffer2);
			}
			if (this.buffer3 != null)
			{
				for (int k = 0; k < this.buffer3.Count; k++)
				{
					this.buffer3[k].Dispose();
				}
				ListPool<NativeQueue<byte>>.Release(ref this.buffer3);
			}
			if (this.gcHandles != null)
			{
				for (int l = 0; l < this.gcHandles.Count; l++)
				{
					this.gcHandles[l].Free();
				}
				ListPool<GCHandle>.Release(ref this.gcHandles);
			}
			if (this.ptrBuffer != null)
			{
				for (int m = 0; m < this.ptrBuffer.Count; m++)
				{
					AllocatorManager.Free(this.ptrBuffer[m].Item2, (void*)this.ptrBuffer[m].Item1, this.ptrBuffer[m].Item3, this.ptrBuffer[m].Item4, 1);
				}
				ListPool<ValueTuple<IntPtr, AllocatorManager.AllocatorHandle, int, int>>.Release(ref this.ptrBuffer);
			}
			if (this.ptrBufferTracked != null)
			{
				for (int n = 0; n < this.ptrBufferTracked.Count; n++)
				{
					UnsafeUtility.FreeTracked((void*)this.ptrBufferTracked[n].Item1, this.ptrBufferTracked[n].Item2);
				}
				ListPool<ValueTuple<IntPtr, Allocator>>.Release(ref this.ptrBufferTracked);
			}
		}

		// Token: 0x040007BF RID: 1983
		private List<NativeArray<byte>> buffer;

		// Token: 0x040007C0 RID: 1984
		private List<NativeList<byte>> buffer2;

		// Token: 0x040007C1 RID: 1985
		private List<NativeQueue<byte>> buffer3;

		// Token: 0x040007C2 RID: 1986
		private List<ValueTuple<IntPtr, AllocatorManager.AllocatorHandle, int, int>> ptrBuffer;

		// Token: 0x040007C3 RID: 1987
		private List<ValueTuple<IntPtr, Allocator>> ptrBufferTracked;

		// Token: 0x040007C4 RID: 1988
		private List<GCHandle> gcHandles;
	}
}
