using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000234 RID: 564
	[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
	public readonly struct RefRW<T> : IQueryTypeParameter where T : struct, IComponentData
	{
		// Token: 0x06000DE7 RID: 3559 RVA: 0x00042A98 File Offset: 0x00040C98
		public unsafe static explicit operator RefRO<T>(RefRW<T> refRW)
		{
			return new RefRO<T>((void*)refRW._Data);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void OutOfBoundsArrayConstructor(int index, int length)
		{
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00042AA5 File Offset: 0x00040CA5
		public unsafe RefRW(byte* ptr)
		{
			this._Data = ptr;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00042AAE File Offset: 0x00040CAE
		public unsafe RefRW(NativeArray<T> componentDataNativeArray, int index)
		{
			this._Data = (byte*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(componentDataNativeArray);
			this._Data += UnsafeUtility.SizeOf<T>() * index;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00042AD0 File Offset: 0x00040CD0
		public static RefRW<T> Optional(NativeArray<T> componentDataNativeArray, int index)
		{
			if (componentDataNativeArray.Length != 0)
			{
				return new RefRW<T>(componentDataNativeArray, index);
			}
			return default(RefRW<T>);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00042AA5 File Offset: 0x00040CA5
		internal unsafe RefRW(void* ptr)
		{
			this._Data = (byte*)ptr;
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x00042AF7 File Offset: 0x00040CF7
		public bool IsValid
		{
			get
			{
				return this._Data != null;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x00042B06 File Offset: 0x00040D06
		public unsafe ref T ValueRW
		{
			get
			{
				return UnsafeUtility.AsRef<T>((void*)this._Data);
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x00042B06 File Offset: 0x00040D06
		public unsafe readonly ref T ValueRO
		{
			get
			{
				return UnsafeUtility.AsRef<T>((void*)this._Data);
			}
		}

		// Token: 0x040005EB RID: 1515
		private unsafe readonly byte* _Data;
	}
}
