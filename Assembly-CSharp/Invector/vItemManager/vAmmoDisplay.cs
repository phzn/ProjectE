using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x020005F3 RID: 1523
	public class vAmmoDisplay : MonoBehaviour
	{
		// Token: 0x0600234B RID: 9035 RVA: 0x000C92F8 File Offset: 0x000C74F8
		private void Start()
		{
			if (this.display == null)
			{
				Object.Destroy(base.gameObject);
			}
			this.display.text = "";
			if (this.secundaryDisplay)
			{
				this.secundaryDisplay.text = "";
			}
			this.currentAmmoId = -1;
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x000C9354 File Offset: 0x000C7554
		public void Show()
		{
			if (this.display)
			{
				this.display.gameObject.SetActive(true);
			}
			if (this.secundaryDisplay)
			{
				this.secundaryDisplay.gameObject.SetActive(true);
			}
			this.onShow.Invoke();
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x000C93A8 File Offset: 0x000C75A8
		public void Hide()
		{
			if (this.display)
			{
				this.display.gameObject.SetActive(false);
			}
			if (this.secundaryDisplay)
			{
				this.secundaryDisplay.gameObject.SetActive(true);
			}
			this.onHide.Invoke();
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x000C93FC File Offset: 0x000C75FC
		public void UpdateDisplay(string text1, string text2 = "", int id = 0)
		{
			if (this.display && !text1.Equals("") && !this.display.gameObject.activeSelf)
			{
				this.display.gameObject.SetActive(true);
			}
			if (this.secundaryDisplay && !text2.Equals("") && !this.secundaryDisplay.gameObject.activeSelf)
			{
				this.secundaryDisplay.gameObject.SetActive(true);
			}
			if (this.currentAmmoId != id)
			{
				this.onChangeAmmo.Invoke(id);
				this.currentAmmoId = id;
			}
			if (this.display)
			{
				this.display.text = text1;
			}
			if (this.secundaryDisplay)
			{
				this.secundaryDisplay.text = text2;
			}
		}

		// Token: 0x04002361 RID: 9057
		public int displayID = 1;

		// Token: 0x04002362 RID: 9058
		[SerializeField]
		[vHelpBox("Ammo loaded in the Clip", vHelpBoxAttribute.MessageType.None)]
		protected Text display;

		// Token: 0x04002363 RID: 9059
		[SerializeField]
		[vHelpBox("Ammo left in the Inventory", vHelpBoxAttribute.MessageType.None)]
		protected Text secundaryDisplay;

		// Token: 0x04002364 RID: 9060
		public UnityEvent onShow;

		// Token: 0x04002365 RID: 9061
		public UnityEvent onHide;

		// Token: 0x04002366 RID: 9062
		[vHelpBox("Event based in the current AmmoID", vHelpBoxAttribute.MessageType.None)]
		public vAmmoDisplay.OnChangeAmmoEvent onChangeAmmo;

		// Token: 0x04002367 RID: 9063
		private int currentAmmoId;

		// Token: 0x020005F4 RID: 1524
		[Serializable]
		public class OnChangeAmmoEvent : UnityEvent<int>
		{
		}
	}
}
