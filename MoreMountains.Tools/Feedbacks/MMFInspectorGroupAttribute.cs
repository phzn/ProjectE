using System;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001B9 RID: 441
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
	public class MMFInspectorGroupAttribute : PropertyAttribute
	{
		// Token: 0x06000A8F RID: 2703 RVA: 0x0002F938 File Offset: 0x0002DB38
		public MMFInspectorGroupAttribute(string groupName, bool groupAllFieldsUntilNextGroupAttribute = false, int groupColorIndex = 24, bool requiresSetup = false, bool closedByDefault = false)
		{
			if (groupColorIndex > 139)
			{
				groupColorIndex = 139;
			}
			this.GroupName = groupName;
			this.GroupAllFieldsUntilNextGroupAttribute = groupAllFieldsUntilNextGroupAttribute;
			this.GroupColorIndex = groupColorIndex;
			this.RequiresSetup = requiresSetup;
			this.ClosedByDefault = closedByDefault;
		}

		// Token: 0x04000C8F RID: 3215
		public string GroupName;

		// Token: 0x04000C90 RID: 3216
		public bool GroupAllFieldsUntilNextGroupAttribute;

		// Token: 0x04000C91 RID: 3217
		public int GroupColorIndex;

		// Token: 0x04000C92 RID: 3218
		public bool RequiresSetup;

		// Token: 0x04000C93 RID: 3219
		public bool ClosedByDefault;
	}
}
