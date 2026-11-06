using System;
using System.Collections;
using Features.Localization;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003E8 RID: 1000
	public class HackStringEffect : MonoBehaviour
	{
		// Token: 0x0600197E RID: 6526 RVA: 0x000A1330 File Offset: 0x0009F530
		public void StartUnlock()
		{
			base.StartCoroutine(this.StartEffect());
			this.UnlockButton.SetActive(false);
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x000A134B File Offset: 0x0009F54B
		public void OnHackFinished()
		{
			Debug.Log("Hacking Finished!");
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x000A1357 File Offset: 0x0009F557
		private void OnEnable()
		{
			this.isReset = false;
			this.textComponent.color = Color.red;
			LocalizedGameText.BindSource(this.textComponent, "phone_story", "ACCESS DENIED", null);
			this.UnlockButton.SetActive(true);
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x000A1393 File Offset: 0x0009F593
		private IEnumerator StartEffect()
		{
			LocalizedGameText.BindFontOnly(this.textComponent);
			this.isReset = true;
			float iteration = 0f;
			while (this.isReset)
			{
				string source = GameLocalization.GetSource("phone_story", this.targetText);
				if (iteration >= (float)source.Length)
				{
					LocalizedGameText.BindSource(this.textComponent, "phone_story", this.targetText, null);
					this.textComponent.color = Color.green;
					this.isReset = false;
					this.OnHackFinished();
					yield break;
				}
				string text = "";
				for (int i = 0; i < source.Length; i++)
				{
					if ((float)i < iteration)
					{
						text += source[i].ToString();
					}
					else
					{
						text += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[Random.Range(0, 26)].ToString();
					}
				}
				this.textComponent.text = text;
				iteration += 0.33333334f;
				yield return new WaitForSeconds(this.speed);
			}
			yield break;
		}

		// Token: 0x04001A9B RID: 6811
		private const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		// Token: 0x04001A9C RID: 6812
		private string targetText = "ACCESS GRANTED";

		// Token: 0x04001A9D RID: 6813
		public float speed = 0.1f;

		// Token: 0x04001A9E RID: 6814
		public TMP_Text textComponent;

		// Token: 0x04001A9F RID: 6815
		private bool isReset;

		// Token: 0x04001AA0 RID: 6816
		public GameObject UnlockButton;
	}
}
