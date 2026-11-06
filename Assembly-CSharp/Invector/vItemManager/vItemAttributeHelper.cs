using System;
using System.Collections.Generic;

namespace Invector.vItemManager
{
	// Token: 0x02000562 RID: 1378
	public static class vItemAttributeHelper
	{
		// Token: 0x060020FE RID: 8446 RVA: 0x000C149F File Offset: 0x000BF69F
		public static void CopyTo(this vItemAttribute itemAttribute, vItemAttribute to)
		{
			to.isBool = itemAttribute.isBool;
			to.name = itemAttribute.name;
			to.value = itemAttribute.value;
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x000C14C8 File Offset: 0x000BF6C8
		public static bool Contains(this List<vItemAttribute> attributes, vItemAttributes name)
		{
			return attributes.Find((vItemAttribute at) => at.name == name) != null;
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x000C14F8 File Offset: 0x000BF6F8
		public static vItemAttribute GetAttributeByType(this List<vItemAttribute> attributes, vItemAttributes name)
		{
			return attributes.Find((vItemAttribute at) => at.name == name);
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000C1524 File Offset: 0x000BF724
		public static bool Equals(this vItemAttribute attributeA, vItemAttribute attributeB)
		{
			return attributeA.name == attributeB.name;
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000C1534 File Offset: 0x000BF734
		public static List<vItemAttribute> CopyAsNew(this List<vItemAttribute> copy)
		{
			List<vItemAttribute> list = new List<vItemAttribute>();
			if (copy != null)
			{
				for (int i = 0; i < copy.Count; i++)
				{
					vItemAttribute vItemAttribute = new vItemAttribute(copy[i].name, copy[i].value);
					list.Add(vItemAttribute);
				}
			}
			return list;
		}
	}
}
