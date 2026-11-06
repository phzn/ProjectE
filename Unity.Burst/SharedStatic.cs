using System;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Burst
{
	// Token: 0x0200002F RID: 47
	public readonly struct SharedStatic<T> where T : struct
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00005ABD File Offset: 0x00003CBD
		private unsafe SharedStatic(void* buffer)
		{
			this._buffer = buffer;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005AC6 File Offset: 0x00003CC6
		public ref T Data
		{
			get
			{
				return Unsafe.AsRef<T>(this._buffer);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00005AD3 File Offset: 0x00003CD3
		public unsafe void* UnsafeDataPointer
		{
			get
			{
				return this._buffer;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005ADB File Offset: 0x00003CDB
		public static SharedStatic<T> GetOrCreate<TContext>(uint alignment = 0U)
		{
			return SharedStatic<T>.GetOrCreateUnsafe(alignment, BurstRuntime.GetHashCode64<TContext>(), 0L);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005AEA File Offset: 0x00003CEA
		public static SharedStatic<T> GetOrCreate<TContext, TSubContext>(uint alignment = 0U)
		{
			return SharedStatic<T>.GetOrCreateUnsafe(alignment, BurstRuntime.GetHashCode64<TContext>(), BurstRuntime.GetHashCode64<TSubContext>());
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005AFC File Offset: 0x00003CFC
		public static SharedStatic<T> GetOrCreateUnsafe(uint alignment, long hashCode, long subHashCode)
		{
			return new SharedStatic<T>(SharedStatic.GetOrCreateSharedStaticInternal(hashCode, subHashCode, (uint)UnsafeUtility.SizeOf<T>(), (alignment == 0U) ? 16U : alignment));
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005B17 File Offset: 0x00003D17
		public static SharedStatic<T> GetOrCreatePartiallyUnsafeWithHashCode<TSubContext>(uint alignment, long hashCode)
		{
			return new SharedStatic<T>(SharedStatic.GetOrCreateSharedStaticInternal(hashCode, BurstRuntime.GetHashCode64<TSubContext>(), (uint)UnsafeUtility.SizeOf<T>(), (alignment == 0U) ? 16U : alignment));
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005B36 File Offset: 0x00003D36
		public static SharedStatic<T> GetOrCreatePartiallyUnsafeWithSubHashCode<TContext>(uint alignment, long subHashCode)
		{
			return new SharedStatic<T>(SharedStatic.GetOrCreateSharedStaticInternal(BurstRuntime.GetHashCode64<TContext>(), subHashCode, (uint)UnsafeUtility.SizeOf<T>(), (alignment == 0U) ? 16U : alignment));
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005B55 File Offset: 0x00003D55
		public static SharedStatic<T> GetOrCreate(Type contextType, uint alignment = 0U)
		{
			return SharedStatic<T>.GetOrCreateUnsafe(alignment, BurstRuntime.GetHashCode64(contextType), 0L);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005B65 File Offset: 0x00003D65
		public static SharedStatic<T> GetOrCreate(Type contextType, Type subContextType, uint alignment = 0U)
		{
			return SharedStatic<T>.GetOrCreateUnsafe(alignment, BurstRuntime.GetHashCode64(contextType), BurstRuntime.GetHashCode64(subContextType));
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005B79 File Offset: 0x00003D79
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private static void CheckIf_T_IsUnmanagedOrThrow()
		{
			if (!UnsafeUtility.IsUnmanaged<T>())
			{
				throw new InvalidOperationException(string.Format("The type {0} used in SharedStatic<{1}> must be unmanaged (contain no managed types).", typeof(T), typeof(T)));
			}
		}

		// Token: 0x0400017E RID: 382
		private unsafe readonly void* _buffer;

		// Token: 0x0400017F RID: 383
		private const uint DefaultAlignment = 16U;
	}
}
