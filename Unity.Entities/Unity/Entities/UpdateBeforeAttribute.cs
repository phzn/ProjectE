using System;

namespace Unity.Entities
{
	// Token: 0x0200027D RID: 637
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	public class UpdateBeforeAttribute : Attribute, ISystemOrderAttribute
	{
		// Token: 0x0600110F RID: 4367 RVA: 0x000486C6 File Offset: 0x000468C6
		public UpdateBeforeAttribute(Type systemType)
		{
			if (systemType == null)
			{
				throw new ArgumentNullException("systemType");
			}
			this.SystemType = systemType;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x000486E9 File Offset: 0x000468E9
		public Type SystemType { get; }
	}
}
