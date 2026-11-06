using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x0200020D RID: 525
	[GenerateTestsForBurstCompatibility(RequiredUnityDefine = "ENABLE_UNITY_COLLECTIONS_CHECKS", GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
	public readonly struct EnabledRefRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T> : IQueryTypeParameter where T : struct, ValueType, IEnableableComponent
	{
		// Token: 0x06000C55 RID: 3157 RVA: 0x00038953 File Offset: 0x00036B53
		public EnabledRefRO(SafeBitRef ptr)
		{
			this.m_Ptr = ptr;
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x0003895C File Offset: 0x00036B5C
		public static EnabledRefRO<T> Null
		{
			get
			{
				return new EnabledRefRO<T>(SafeBitRef.Null);
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x00038968 File Offset: 0x00036B68
		public bool IsValid
		{
			get
			{
				return this.m_Ptr.IsValid;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00038975 File Offset: 0x00036B75
		public bool ValueRO
		{
			get
			{
				return this.m_Ptr.GetBit();
			}
		}

		// Token: 0x04000562 RID: 1378
		private readonly SafeBitRef m_Ptr;
	}
}
