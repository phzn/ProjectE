using System;
using UnityEngine;
using UnityEngine.UI;

namespace Invector.Throw
{
	// Token: 0x02000504 RID: 1284
	public class vThrowUI : MonoBehaviour
	{
		// Token: 0x06001F3F RID: 7999 RVA: 0x000BA2A4 File Offset: 0x000B84A4
		internal virtual void UpdateCount(vThrowManagerBase throwManager, bool showMaxAmount = true)
		{
			if (this.currentThrowCount)
			{
				this.currentThrowCount.text = throwManager.CurrentThrowAmount.ToString();
			}
			if (this.maxThrowCount)
			{
				this.maxThrowCount.text = (showMaxAmount ? throwManager.MaxThrowObjects.ToString() : "");
			}
			if (this.display)
			{
				this.display.sprite = throwManager.CurrentThrowableSprite;
			}
		}

		// Token: 0x04001FD9 RID: 8153
		public Text maxThrowCount;

		// Token: 0x04001FDA RID: 8154
		public Text currentThrowCount;

		// Token: 0x04001FDB RID: 8155
		public Image display;
	}
}
