using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000233 RID: 563
	[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
	public readonly struct RefRO<T> : IQueryTypeParameter where T : struct, IComponentData
	{
		// Token: 0x06000DE1 RID: 3553 RVA: 0x00042A2A File Offset: 0x00040C2A
		public unsafe RefRO(NativeArray<T> componentDataArray, int index)
		{
			this._Data = (byte*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(componentDataArray);
			this._Data += UnsafeUtility.SizeOf<T>() * index;
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void OutOfBoundsArrayConstructor(int index, int length)
		{
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00042A4C File Offset: 0x00040C4C
		public static RefRO<T> Optional(NativeArray<T> componentDataArray, int index)
		{
			if (componentDataArray.Length != 0)
			{
				return new RefRO<T>(componentDataArray, index);
			}
			return default(RefRO<T>);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00042A73 File Offset: 0x00040C73
		internal unsafe RefRO(void* ptr)
		{
			this._Data = (byte*)ptr;
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x00042A7C File Offset: 0x00040C7C
		public bool IsValid
		{
			get
			{
				return this._Data != null;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x00042A8B File Offset: 0x00040C8B
		public unsafe readonly ref T ValueRO
		{
			get
			{
				return UnsafeUtility.AsRef<T>((void*)this._Data);
			}
		}

		// Token: 0x040005EA RID: 1514
		private unsafe readonly byte* _Data;
	}
}
