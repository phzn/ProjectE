using System;

namespace Unity.Entities.Serialization
{
	// Token: 0x020003A0 RID: 928
	internal struct UnsafeUntypedWeakReferenceId
	{
		// Token: 0x06001898 RID: 6296 RVA: 0x0005EA50 File Offset: 0x0005CC50
		public UnsafeUntypedWeakReferenceId(UntypedWeakReferenceId weakAssetRef)
		{
			this.GlobalId = weakAssetRef.GlobalId;
			this.GenerationType = weakAssetRef.GenerationType;
		}

		// Token: 0x04000977 RID: 2423
		public RuntimeGlobalObjectId GlobalId;

		// Token: 0x04000978 RID: 2424
		public WeakReferenceGenerationType GenerationType;
	}
}
