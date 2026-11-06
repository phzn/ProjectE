using System;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x0200028A RID: 650
	[Serializable]
	internal struct UntypedUnityObjectRef : IEquatable<UntypedUnityObjectRef>
	{
		// Token: 0x0600114C RID: 4428 RVA: 0x00049307 File Offset: 0x00047507
		public bool Equals(UntypedUnityObjectRef other)
		{
			return this.instanceId == other.instanceId;
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00049318 File Offset: 0x00047518
		public override bool Equals(object obj)
		{
			if (obj is UntypedUnityObjectRef)
			{
				UntypedUnityObjectRef untypedUnityObjectRef = (UntypedUnityObjectRef)obj;
				return this.Equals(untypedUnityObjectRef);
			}
			return false;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0004933D File Offset: 0x0004753D
		public override int GetHashCode()
		{
			return this.instanceId;
		}

		// Token: 0x0400069D RID: 1693
		[SerializeField]
		internal int instanceId;
	}
}
