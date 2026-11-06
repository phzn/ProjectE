using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000214 RID: 532
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("Modern UI Pack/Layout/Layout Group Fix")]
	public class LayoutGroupFix : MonoBehaviour
	{
		// Token: 0x06000EE7 RID: 3815 RVA: 0x0006451D File Offset: 0x0006271D
		private void OnEnable()
		{
			if (!this.fixWithDelay && this.fixOnEnable)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
				return;
			}
			if (this.fixWithDelay)
			{
				base.StartCoroutine(this.FixDelay());
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00064550 File Offset: 0x00062750
		public void FixLayout()
		{
			if (!this.fixWithDelay)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
				return;
			}
			base.StartCoroutine(this.FixDelay());
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00064573 File Offset: 0x00062773
		private IEnumerator FixDelay()
		{
			yield return new WaitForSecondsRealtime(this.fixDelay);
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.GetComponent<RectTransform>());
			yield break;
		}

		// Token: 0x04000F9D RID: 3997
		[SerializeField]
		private bool fixOnEnable = true;

		// Token: 0x04000F9E RID: 3998
		[SerializeField]
		private bool fixWithDelay = true;

		// Token: 0x04000F9F RID: 3999
		private float fixDelay = 0.025f;
	}
}
