using System;

namespace Unity.Entities
{
	// Token: 0x0200027E RID: 638
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	public class UpdateAfterAttribute : Attribute, ISystemOrderAttribute
	{
		// Token: 0x06001111 RID: 4369 RVA: 0x000486F1 File Offset: 0x000468F1
		public UpdateAfterAttribute(Type systemType)
		{
			if (systemType == null)
			{
				throw new ArgumentNullException("systemType");
			}
			this.SystemType = systemType;
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x00048714 File Offset: 0x00046914
		public Type SystemType { get; }
	}
}
