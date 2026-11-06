using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000A9 RID: 169
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
	public class MMInspectorGroupAttribute : PropertyAttribute
	{
		// Token: 0x06000375 RID: 885 RVA: 0x00011845 File Offset: 0x0000FA45
		public MMInspectorGroupAttribute(string groupName, bool groupAllFieldsUntilNextGroupAttribute = false, int groupColorIndex = 24, bool closedByDefault = false)
		{
			if (groupColorIndex > 139)
			{
				groupColorIndex = 139;
			}
			this.GroupName = groupName;
			this.GroupAllFieldsUntilNextGroupAttribute = groupAllFieldsUntilNextGroupAttribute;
			this.GroupColorIndex = groupColorIndex;
			this.ClosedByDefault = closedByDefault;
		}

		// Token: 0x040004DD RID: 1245
		public string GroupName;

		// Token: 0x040004DE RID: 1246
		public bool GroupAllFieldsUntilNextGroupAttribute;

		// Token: 0x040004DF RID: 1247
		public int GroupColorIndex;

		// Token: 0x040004E0 RID: 1248
		public bool ClosedByDefault;
	}
}
