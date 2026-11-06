using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x0200056A RID: 1386
	[vClassHeader("vItemDisplay", true, "icon_v2", false, "")]
	public class vItemDisplay : vMonoBehaviour
	{
		// Token: 0x0600211A RID: 8474 RVA: 0x000C17E0 File Offset: 0x000BF9E0
		public virtual void DisplayItem(vItemManager.CollectedItemInfo info)
		{
			if (this.useColorByType)
			{
				vItemDisplay.ColorByType colorByType = Array.Find<vItemDisplay.ColorByType>(this.colorByTypes, (vItemDisplay.ColorByType c) => c.types.Contains(info.item.type));
				if (colorByType != null)
				{
					this.bgColor.color = colorByType.color;
				}
			}
			this.icon.sprite = info.item.icon;
			this.itemName.text = this.FormatText(this.nameFormat, info.item.name);
			this.type.text = this.FormatText(this.typeFormat, info.item.type.ToString());
			if (info.amount > 1 || !this.displayAmountOnlyGreaterOne)
			{
				this.amount.text = this.FormatText(this.amountFormat, info.amount.ToString());
				return;
			}
			this.amount.text = "";
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000C18F0 File Offset: 0x000BFAF0
		public virtual string FormatText(string format, string value)
		{
			if (string.IsNullOrEmpty(format))
			{
				return value;
			}
			return string.Format(format, value);
		}

		// Token: 0x040021B9 RID: 8633
		[vEditorToolbar("Settings", false, "", false, false)]
		public Image icon;

		// Token: 0x040021BA RID: 8634
		public Text itemName;

		// Token: 0x040021BB RID: 8635
		public Text type;

		// Token: 0x040021BC RID: 8636
		public Text amount;

		// Token: 0x040021BD RID: 8637
		[vEditorToolbar("ColorByType", false, "", false, false)]
		public bool useColorByType;

		// Token: 0x040021BE RID: 8638
		public MaskableGraphic bgColor;

		// Token: 0x040021BF RID: 8639
		public vItemDisplay.ColorByType[] colorByTypes;

		// Token: 0x040021C0 RID: 8640
		[vHelpBox("Use {0} inside string format to inset the value", vHelpBoxAttribute.MessageType.None)]
		public string nameFormat = "Name: {0}";

		// Token: 0x040021C1 RID: 8641
		public string typeFormat = "Type: {0}";

		// Token: 0x040021C2 RID: 8642
		public string amountFormat = "Amount: {0}";

		// Token: 0x040021C3 RID: 8643
		public bool displayAmountOnlyGreaterOne = true;

		// Token: 0x0200056B RID: 1387
		[Serializable]
		public class ColorByType
		{
			// Token: 0x0600211D RID: 8477 RVA: 0x000C1933 File Offset: 0x000BFB33
			public ColorByType()
			{
				Color white = Color.white;
			}

			// Token: 0x040021C4 RID: 8644
			public List<vItemType> types;

			// Token: 0x040021C5 RID: 8645
			public Color color = Color.white;
		}
	}
}
