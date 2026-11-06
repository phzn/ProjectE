using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x02000568 RID: 1384
	[vClassHeader("Item Collection HUD", true, "icon_v2", false, "", helpBoxText = "Contains all behaviour to show messages sended")]
	public class vItemCollectionTextHUD : MonoBehaviour
	{
		// Token: 0x0600210F RID: 8463 RVA: 0x000C16A7 File Offset: 0x000BF8A7
		public void Show(string message, float timeToStay = 1f, float timeToFadeOut = 1f)
		{
			this.Message.text = message;
			base.StartCoroutine(this.Timer(timeToStay, timeToFadeOut));
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000C16C4 File Offset: 0x000BF8C4
		private IEnumerator Timer(float timeToStay = 1f, float timeToFadeOut = 1f)
		{
			this.Message.CrossFadeAlpha(1f, 0.5f, false);
			yield return new WaitForSeconds(timeToStay);
			this.Message.CrossFadeAlpha(0f, timeToFadeOut, false);
			yield return new WaitForSeconds(timeToFadeOut + 0.1f);
			Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000C16E1 File Offset: 0x000BF8E1
		private void Awake()
		{
			this.Clear();
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000C16E9 File Offset: 0x000BF8E9
		public void Clear()
		{
			this.Message.text = "";
			this.Message.CrossFadeAlpha(0f, 0f, false);
		}

		// Token: 0x040021B3 RID: 8627
		public Text Message;
	}
}
