using System;
using System.Collections.Generic;

namespace Invector.vItemManager
{
	// Token: 0x020005B4 RID: 1460
	[Serializable]
	public class ItemReference
	{
		// Token: 0x06002245 RID: 8773 RVA: 0x000C5A6C File Offset: 0x000C3C6C
		public ItemReference(int id)
		{
			this.id = id;
			this.addToEquipArea = true;
			this.autoEquip = false;
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x000C5A90 File Offset: 0x000C3C90
		public ItemReference Clone()
		{
			return new ItemReference(this.id)
			{
				name = this.name,
				amount = this.amount,
				autoEquip = this.autoEquip,
				addToEquipArea = this.addToEquipArea,
				indexArea = this.indexArea,
				changeAttributes = this.changeAttributes,
				attributes = this.attributes
			};
		}

		// Token: 0x0400228A RID: 8842
		public int id;

		// Token: 0x0400228B RID: 8843
		public string name;

		// Token: 0x0400228C RID: 8844
		public int amount;

		// Token: 0x0400228D RID: 8845
		public List<vItemAttribute> attributes;

		// Token: 0x0400228E RID: 8846
		public bool changeAttributes;

		// Token: 0x0400228F RID: 8847
		public bool autoEquip;

		// Token: 0x04002290 RID: 8848
		public bool addToEquipArea = true;

		// Token: 0x04002291 RID: 8849
		public int indexArea;
	}
}
