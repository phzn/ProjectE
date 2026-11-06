using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Feel
{
	// Token: 0x0200038D RID: 909
	public class FeelDemosInstructions : MonoBehaviour
	{
		// Token: 0x060018D2 RID: 6354 RVA: 0x0006981C File Offset: 0x00067A1C
		protected virtual void Awake()
		{
			this.TargetText.text = this.DesktopText;
			this._canvasGroup = base.gameObject.GetComponent<CanvasGroup>();
			base.StartCoroutine(this.DisappearCo());
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x0006984D File Offset: 0x00067A4D
		protected virtual IEnumerator DisappearCo()
		{
			yield return MMCoroutine.WaitFor(this.DisappearDelay);
			base.StartCoroutine(MMFade.FadeCanvasGroup(this._canvasGroup, this.DisappearDuration, 0f, true));
			yield return MMCoroutine.WaitFor(this.DisappearDuration + 0.1f);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04001CF4 RID: 7412
		[Header("Bindings")]
		public Text TargetText;

		// Token: 0x04001CF5 RID: 7413
		public float DisappearDelay = 3f;

		// Token: 0x04001CF6 RID: 7414
		public float DisappearDuration = 0.3f;

		// Token: 0x04001CF7 RID: 7415
		[Header("Texts")]
		public string DesktopText = "Press space to...";

		// Token: 0x04001CF8 RID: 7416
		public string MobileText = "Tap anywhere to...";

		// Token: 0x04001CF9 RID: 7417
		protected CanvasGroup _canvasGroup;
	}
}
