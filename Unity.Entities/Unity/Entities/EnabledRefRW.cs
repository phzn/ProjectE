using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x0200020E RID: 526
	[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
	public readonly struct EnabledRefRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IQueryTypeParameter where T : struct, ValueType, IEnableableComponent
	{
		// Token: 0x06000C59 RID: 3161 RVA: 0x00038982 File Offset: 0x00036B82
		public static implicit operator EnabledRefRO<T>(EnabledRefRW<T> componentEnabledRefRW)
		{
			return new EnabledRefRO<T>(componentEnabledRefRW.m_Ptr);
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0003898F File Offset: 0x00036B8F
		public unsafe EnabledRefRW(SafeBitRef ptr, int* ptrChunkDisabledCount)
		{
			this.m_Ptr = ptr;
			this.m_PtrChunkDisabledCount = ptrChunkDisabledCount;
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x0003899F File Offset: 0x00036B9F
		public static EnabledRefRW<T> Null
		{
			get
			{
				return new EnabledRefRW<T>(SafeBitRef.Null, null);
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x000389AD File Offset: 0x00036BAD
		public bool IsValid
		{
			get
			{
				return this.m_Ptr.IsValid;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x000389BA File Offset: 0x00036BBA
		public bool ValueRO
		{
			get
			{
				return this.m_Ptr.GetBit();
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x000389BA File Offset: 0x00036BBA
		// (set) Token: 0x06000C5F RID: 3167 RVA: 0x000389C8 File Offset: 0x00036BC8
		public unsafe bool ValueRW
		{
			get
			{
				return this.m_Ptr.GetBit();
			}
			set
			{
				bool bit = this.m_Ptr.GetBit();
				this.m_Ptr.SetBit(value);
				if (Hint.Likely(bit != value))
				{
					int num = math.select(1, -1, value);
					Interlocked.Add(UnsafeUtility.AsRef<int>((void*)this.m_PtrChunkDisabledCount), num);
				}
			}
		}

		// Token: 0x04000563 RID: 1379
		private readonly SafeBitRef m_Ptr;

		// Token: 0x04000564 RID: 1380
		private unsafe readonly int* m_PtrChunkDisabledCount;
	}
}
