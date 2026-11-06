using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000006 RID: 6
	public struct AnimationJobCache : IDisposable
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020D0 File Offset: 0x000002D0
		internal AnimationJobCache(float[] data)
		{
			this.m_Data = new NativeArray<float>(data, Allocator.Persistent);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020DF File Offset: 0x000002DF
		public void Dispose()
		{
			this.m_Data.Dispose();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020EC File Offset: 0x000002EC
		public float GetRaw(CacheIndex index, int offset = 0)
		{
			return this.m_Data[index.idx + offset];
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002101 File Offset: 0x00000301
		public void SetRaw(float val, CacheIndex index, int offset = 0)
		{
			this.m_Data[index.idx + offset] = val;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002118 File Offset: 0x00000318
		public unsafe T Get<[IsUnmanaged] T>(CacheIndex index, int offset = 0) where T : struct, ValueType
		{
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = num / UnsafeUtility.SizeOf<float>();
			T t = default(T);
			UnsafeUtility.MemCpy((void*)(&t), (void*)((byte*)((byte*)this.m_Data.GetUnsafeReadOnlyPtr<float>() + (IntPtr)index.idx * 4) + (IntPtr)(offset * num2) * 4), (long)num);
			return t;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002164 File Offset: 0x00000364
		public unsafe void Set<[IsUnmanaged] T>(T val, CacheIndex index, int offset = 0) where T : struct, ValueType
		{
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = num / UnsafeUtility.SizeOf<float>();
			UnsafeUtility.MemCpy((void*)((byte*)((byte*)this.m_Data.GetUnsafePtr<float>() + (IntPtr)index.idx * 4) + (IntPtr)(offset * num2) * 4), (void*)(&val), (long)num);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021A8 File Offset: 0x000003A8
		public unsafe void SetArray<[IsUnmanaged] T>(T[] v, CacheIndex index, int offset = 0) where T : struct, ValueType
		{
			int num = UnsafeUtility.SizeOf<T>();
			int num2 = num / UnsafeUtility.SizeOf<float>();
			fixed (T[] array = v)
			{
				void* ptr;
				if (v == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = (void*)(&array[0]);
				}
				UnsafeUtility.MemCpy((void*)((byte*)((byte*)this.m_Data.GetUnsafePtr<float>() + (IntPtr)index.idx * 4) + (IntPtr)(offset * num2) * 4), ptr, (long)(num * v.Length));
			}
		}

		// Token: 0x04000002 RID: 2
		private NativeArray<float> m_Data;
	}
}
