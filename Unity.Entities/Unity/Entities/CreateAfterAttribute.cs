using System;

namespace Unity.Entities
{
	// Token: 0x02000280 RID: 640
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	public class CreateAfterAttribute : Attribute, ISystemOrderAttribute
	{
		// Token: 0x06001115 RID: 4373 RVA: 0x00048747 File Offset: 0x00046947
		public CreateAfterAttribute(Type systemType)
		{
			if (systemType == null)
			{
				throw new ArgumentNullException("systemType");
			}
			this.SystemType = systemType;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x0004876A File Offset: 0x0004696A
		public Type SystemType { get; }
	}
}
