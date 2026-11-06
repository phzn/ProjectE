using System;
using System.Runtime.CompilerServices;
using Pathfinding.Graphs.Grid;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Jobs
{
	// Token: 0x02000177 RID: 375
	internal static class NativeArrayExtensions
	{
		// Token: 0x06000BB5 RID: 2997 RVA: 0x00040FC4 File Offset: 0x0003F1C4
		public static JobMemSet<T> MemSet<[IsUnmanaged] T>(this NativeArray<T> self, T value) where T : struct, ValueType
		{
			return new JobMemSet<T>
			{
				data = self,
				value = value
			};
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00040FEC File Offset: 0x0003F1EC
		public static JobAND BitwiseAndWith(this NativeArray<bool> self, NativeArray<bool> other)
		{
			return new JobAND
			{
				result = self,
				data = other
			};
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00041014 File Offset: 0x0003F214
		public static JobCopy<T> CopyToJob<T>(this NativeArray<T> from, NativeArray<T> to) where T : struct
		{
			return new JobCopy<T>
			{
				from = from,
				to = to
			};
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0004103C File Offset: 0x0003F23C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SliceActionJob<T> WithSlice<T>(this T action, Slice3D slice) where T : struct, GridIterationUtilities.ISliceAction
		{
			return new SliceActionJob<T>
			{
				action = action,
				slice = slice
			};
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00041064 File Offset: 0x0003F264
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IndexActionJob<T> WithLength<T>(this T action, int length) where T : struct, GridIterationUtilities.ISliceAction
		{
			return new IndexActionJob<T>
			{
				action = action,
				length = length
			};
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0004108C File Offset: 0x0003F28C
		public static JobRotate3DArray<T> Rotate3D<[IsUnmanaged] T>(this NativeArray<T> arr, int3 size, int dx, int dz) where T : struct, ValueType
		{
			return new JobRotate3DArray<T>
			{
				arr = arr,
				size = size,
				dx = dx,
				dz = dz
			};
		}
	}
}
