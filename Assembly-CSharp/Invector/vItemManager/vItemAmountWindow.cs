using System;
using UnityEngine;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x02000560 RID: 1376
	public class vItemAmountWindow : vMasterWindow
	{
		// Token: 0x060020F8 RID: 8440 RVA: 0x000C125C File Offset: 0x000BF45C
		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.itemWindowDisplay && this.itemWindowDisplay.currentSelectedSlot.item)
			{
				this.singleAmountControl.SetActive(this.itemWindowDisplay.currentSelectedSlot.item.amount <= 1);
				this.multAmountControl.SetActive(this.itemWindowDisplay.currentSelectedSlot.item.amount > 1);
				if (this.amountDisplay)
				{
					this.amountDisplay.text = 1.ToString("00");
				}
				this.itemWindowDisplay.amount = 1;
			}
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x000C1314 File Offset: 0x000BF514
		public virtual void ChangeAmount(int value)
		{
			if (this.itemWindowDisplay && this.itemWindowDisplay.currentSelectedSlot.item)
			{
				this.itemWindowDisplay.amount += value;
				this.itemWindowDisplay.amount = Mathf.Clamp(this.itemWindowDisplay.amount, 1, this.itemWindowDisplay.currentSelectedSlot.item.amount);
				if (this.amountDisplay)
				{
					this.amountDisplay.text = this.itemWindowDisplay.amount.ToString("00");
				}
			}
		}

		// Token: 0x0400219D RID: 8605
		public vItemWindowDisplay itemWindowDisplay;

		// Token: 0x0400219E RID: 8606
		public GameObject singleAmountControl;

		// Token: 0x0400219F RID: 8607
		public GameObject multAmountControl;

		// Token: 0x040021A0 RID: 8608
		public Text amountDisplay;
	}
}
