using System;

namespace Unity.Entities
{
	// Token: 0x0200027F RID: 639
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	public class CreateBeforeAttribute : Attribute, ISystemOrderAttribute
	{
		// Token: 0x06001113 RID: 4371 RVA: 0x0004871C File Offset: 0x0004691C
		public CreateBeforeAttribute(Type systemType)
		{
			if (systemType == null)
			{
				throw new ArgumentNullException("systemType");
			}
			this.SystemType = systemType;
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06001114 RID: 4372 RVA: 0x0004873F File Offset: 0x0004693F
		public Type SystemType { get; }
	}
}
