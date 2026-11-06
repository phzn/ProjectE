using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Collections
{
	// Token: 0x020000C5 RID: 197
	[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "UNITY_2020_2_OR_NEWER", GenericTypeArguments = new Type[]
	{
		typeof(int),
		typeof(NativeSortExtension.DefaultComparer<int>)
	})]
	public struct SortJob<[global::System.Runtime.CompilerServices.IsUnmanaged] T, U> where T : struct, ValueType where U : IComparer<T>
	{
		// Token: 0x06000942 RID: 2370 RVA: 0x0001BD88 File Offset: 0x00019F88
		public JobHandle Schedule(JobHandle inputDeps = default(JobHandle))
		{
			if (this.Length == 0)
			{
				return inputDeps;
			}
			int num = (this.Length + 1023) / 1024;
			int threadIndexCount = JobsUtility.ThreadIndexCount;
			int num2 = math.max(1, threadIndexCount);
			int num3 = num / num2;
			JobHandle jobHandle = new SortJob<T, U>.SegmentSort
			{
				Data = this.Data,
				Comp = this.Comp,
				Length = this.Length,
				SegmentWidth = 1024
			}.Schedule(num, num3, inputDeps);
			return new SortJob<T, U>.SegmentSortMerge
			{
				Data = this.Data,
				Comp = this.Comp,
				Length = this.Length,
				SegmentWidth = 1024
			}.Schedule(jobHandle);
		}

		// Token: 0x040003F2 RID: 1010
		public unsafe T* Data;

		// Token: 0x040003F3 RID: 1011
		public U Comp;

		// Token: 0x040003F4 RID: 1012
		public int Length;

		// Token: 0x020000C6 RID: 198
		[BurstCompile]
		public struct SegmentSort : IJobParallelFor
		{
			// Token: 0x06000943 RID: 2371 RVA: 0x0001BE50 File Offset: 0x0001A050
			public void Execute(int index)
			{
				int num = index * this.SegmentWidth;
				int num2 = ((this.Length - num < this.SegmentWidth) ? (this.Length - num) : this.SegmentWidth);
				NativeSortExtension.Sort<T, U>(this.Data + (IntPtr)num * (IntPtr)sizeof(T) / (IntPtr)sizeof(T), num2, this.Comp);
			}

			// Token: 0x040003F5 RID: 1013
			[NativeDisableUnsafePtrRestriction]
			internal unsafe T* Data;

			// Token: 0x040003F6 RID: 1014
			internal U Comp;

			// Token: 0x040003F7 RID: 1015
			internal int Length;

			// Token: 0x040003F8 RID: 1016
			internal int SegmentWidth;
		}

		// Token: 0x020000C7 RID: 199
		[BurstCompile]
		public struct SegmentSortMerge : IJob
		{
			// Token: 0x06000944 RID: 2372 RVA: 0x0001BEA4 File Offset: 0x0001A0A4
			public unsafe void Execute()
			{
				int num = (this.Length + (this.SegmentWidth - 1)) / this.SegmentWidth;
				int* ptr;
				checked
				{
					ptr = stackalloc int[unchecked((UIntPtr)num) * 4];
				}
				T* ptr2 = (T*)Memory.Unmanaged.Allocate((long)(UnsafeUtility.SizeOf<T>() * this.Length), 16, Allocator.Temp);
				for (int i = 0; i < this.Length; i++)
				{
					int num2 = -1;
					T t = default(T);
					for (int j = 0; j < num; j++)
					{
						int num3 = j * this.SegmentWidth;
						int num4 = ptr[j];
						int num5 = ((this.Length - num3 < this.SegmentWidth) ? (this.Length - num3) : this.SegmentWidth);
						if (num4 != num5)
						{
							T t2 = this.Data[(IntPtr)(num3 + num4) * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)];
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
				UnsafeUtility.MemCpy((void*)this.Data, (void*)ptr2, (long)(UnsafeUtility.SizeOf<T>() * this.Length));
			}

			// Token: 0x040003F9 RID: 1017
			[NativeDisableUnsafePtrRestriction]
			internal unsafe T* Data;

			// Token: 0x040003FA RID: 1018
			internal U Comp;

			// Token: 0x040003FB RID: 1019
			internal int Length;

			// Token: 0x040003FC RID: 1020
			internal int SegmentWidth;
		}
	}
}
