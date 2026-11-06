using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x020000C8 RID: 200
	[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[]
	{
		typeof(int),
		typeof(NativeSortExtension.DefaultComparer<int>)
	})]
	public struct SortJobDefer<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U> where T : struct, ValueType where U : IComparer<T>
	{
		// Token: 0x06000945 RID: 2373 RVA: 0x0001BFD8 File Offset: 0x0001A1D8
		public JobHandle Schedule(JobHandle inputDeps = default(JobHandle))
		{
			SortJobDefer<T, U>.SegmentSort segmentSort = new SortJobDefer<T, U>.SegmentSort
			{
				DataRO = this.Data,
				Data = this.Data.m_ListData,
				Comp = this.Comp,
				SegmentWidth = 1024
			};
			JobHandle jobHandle = (ref segmentSort).ScheduleByRef<SortJobDefer<T, U>.SegmentSort, T>(this.Data, 1024, inputDeps);
			return new SortJobDefer<T, U>.SegmentSortMerge
			{
				Data = this.Data,
				Comp = this.Comp,
				SegmentWidth = 1024
			}.Schedule(jobHandle);
		}

		// Token: 0x040003FD RID: 1021
		public NativeList<T> Data;

		// Token: 0x040003FE RID: 1022
		public U Comp;

		// Token: 0x020000C9 RID: 201
		[BurstCompile]
		public struct SegmentSort : IJobParallelForDefer
		{
			// Token: 0x06000946 RID: 2374 RVA: 0x0001C070 File Offset: 0x0001A270
			public unsafe void Execute(int index)
			{
				int num = index * this.SegmentWidth;
				int num2 = ((this.Data->Length - num < this.SegmentWidth) ? (this.Data->Length - num) : this.SegmentWidth);
				NativeSortExtension.Sort<T, U>(this.Data->Ptr + (IntPtr)num * (IntPtr)sizeof(T) / (IntPtr)sizeof(T), num2, this.Comp);
			}

			// Token: 0x040003FF RID: 1023
			[ReadOnly]
			internal NativeList<T> DataRO;

			// Token: 0x04000400 RID: 1024
			[NativeDisableUnsafePtrRestriction]
			internal unsafe UnsafeList<T>* Data;

			// Token: 0x04000401 RID: 1025
			internal U Comp;

			// Token: 0x04000402 RID: 1026
			internal int SegmentWidth;
		}

		// Token: 0x020000CA RID: 202
		[BurstCompile]
		public struct SegmentSortMerge : IJob
		{
			// Token: 0x06000947 RID: 2375 RVA: 0x0001C0D4 File Offset: 0x0001A2D4
			public unsafe void Execute()
			{
				int length = this.Data.Length;
				T* unsafePtr = this.Data.GetUnsafePtr<T>();
				int num = (length + (this.SegmentWidth - 1)) / this.SegmentWidth;
				int* ptr;
				checked
				{
					ptr = stackalloc int[unchecked((UIntPtr)num) * 4];
				}
				T* ptr2 = (T*)Memory.Unmanaged.Allocate((long)(UnsafeUtility.SizeOf<T>() * length), 16, Allocator.Temp);
				for (int i = 0; i < length; i++)
				{
					int num2 = -1;
					T t = default(T);
					for (int j = 0; j < num; j++)
					{
						int num3 = j * this.SegmentWidth;
						int num4 = ptr[j];
						int num5 = ((length - num3 < this.SegmentWidth) ? (length - num3) : this.SegmentWidth);
						if (num4 != num5)
						{
							T t2 = unsafePtr[(IntPtr)(num3 + num4) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
							if (num2 == -1 || this.Comp.Compare(t2, t) <= 0)
							{
								t = t2;
								num2 = j;
							}
						}
					}
					ptr[num2]++;
					ptr2[(IntPtr)i * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)] = t;
				}
				UnsafeUtility.MemCpy((void*)unsafePtr, (void*)ptr2, (long)(UnsafeUtility.SizeOf<T>() * length));
			}

			// Token: 0x04000403 RID: 1027
			[NativeDisableUnsafePtrRestriction]
			internal NativeList<T> Data;

			// Token: 0x04000404 RID: 1028
			internal U Comp;

			// Token: 0x04000405 RID: 1029
			internal int SegmentWidth;
		}
	}
}
