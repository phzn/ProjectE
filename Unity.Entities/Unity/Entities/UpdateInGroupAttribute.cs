using System;

namespace Unity.Entities
{
	// Token: 0x02000281 RID: 641
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class UpdateInGroupAttribute : Attribute
	{
		// Token: 0x06001117 RID: 4375 RVA: 0x00048772 File Offset: 0x00046972
		public UpdateInGroupAttribute(Type groupType)
		{
			if (groupType == null)
			{
				throw new ArgumentNullException("groupType");
			}
			this.GroupType = groupType;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06001118 RID: 4376 RVA: 0x00048795 File Offset: 0x00046995
		public Type GroupType { get; }

		// Token: 0x0400068F RID: 1679
		public bool OrderFirst;

		// Token: 0x04000690 RID: 1680
		public bool OrderLast;
	}
}
