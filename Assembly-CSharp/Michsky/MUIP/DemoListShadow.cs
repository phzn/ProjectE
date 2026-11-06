using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x020001ED RID: 493
	public class DemoListShadow : MonoBehaviour, IBeginDragHandler, IEventSystemHandler
	{
		// Token: 0x06000E33 RID: 3635 RVA: 0x00060D5C File Offset: 0x0005EF5C
		private void Awake()
		{
			this.CheckForValue(0f);
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00060D6C File Offset: 0x0005EF6C
		public void CheckForValue(float value)
		{
			if ((double)value > 0.05)
			{
				base.StopCoroutine("LeftCGFadeOut");
				base.StartCoroutine("LeftCGFadeIn");
			}
			else
			{
				base.StopCoroutine("LeftCGFadeIn");
				base.StartCoroutine("LeftCGFadeOut");
			}
			if ((double)value < 0.95)
			{
				base.StopCoroutine("RightCGFadeOut");
				base.StartCoroutine("RightCGFadeIn");
				return;
			}
			base.StopCoroutine("RightCGFadeIn");
			base.StartCoroutine("RightCGFadeOut");
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00060DF2 File Offset: 0x0005EFF2
		public void ScrollUp()
		{
			base.StopCoroutine("ScrollDownHelper");
			base.StartCoroutine("ScrollUpHelper");
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00060E0B File Offset: 0x0005F00B
		public void ScrollDown()
		{
			base.StopCoroutine("ScrollUpHelper");
			base.StartCoroutine("ScrollDownHelper");
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00060E24 File Offset: 0x0005F024
		public void OnBeginDrag(PointerEventData data)
		{
			base.StopCoroutine("ScrollUpHelper");
			base.StopCoroutine("ScrollDownHelper");
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00060E3C File Offset: 0x0005F03C
		private IEnumerator ScrollUpHelper()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.scrollTime)
			{
				this.listScrollbar.value = Mathf.Lerp(this.listScrollbar.value, 0f, elapsedTime / this.scrollTime);
				elapsedTime += Time.unscaledDeltaTime;
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00060E4B File Offset: 0x0005F04B
		private IEnumerator ScrollDownHelper()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.scrollTime)
			{
				this.listScrollbar.value = Mathf.Lerp(this.listScrollbar.value, 1f, elapsedTime / this.scrollTime);
				elapsedTime += Time.unscaledDeltaTime;
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00060E5A File Offset: 0x0005F05A
		private IEnumerator LeftCGFadeIn()
		{
			this.leftCG.interactable = true;
			this.leftCG.blocksRaycasts = true;
			while (this.leftCG.alpha < 0.99f)
			{
				this.leftCG.alpha += Time.unscaledDeltaTime * this.transitionSpeed;
				yield return new WaitForEndOfFrame();
			}
			this.leftCG.alpha = 1f;
			yield break;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00060E69 File Offset: 0x0005F069
		private IEnumerator LeftCGFadeOut()
		{
			this.leftCG.interactable = false;
			this.leftCG.blocksRaycasts = false;
			while (this.leftCG.alpha > 0.01f)
			{
				this.leftCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				yield return new WaitForEndOfFrame();
			}
			this.leftCG.alpha = 0f;
			yield break;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00060E78 File Offset: 0x0005F078
		private IEnumerator RightCGFadeIn()
		{
			this.rightCG.interactable = true;
			this.rightCG.blocksRaycasts = true;
			while (this.rightCG.alpha < 0.99f)
			{
				this.rightCG.alpha += Time.unscaledDeltaTime * this.transitionSpeed;
				yield return new WaitForEndOfFrame();
			}
			this.rightCG.alpha = 1f;
			yield break;
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00060E87 File Offset: 0x0005F087
		private IEnumerator RightCGFadeOut()
		{
			this.rightCG.interactable = false;
			this.rightCG.blocksRaycasts = false;
			while (this.rightCG.alpha > 0.01f)
			{
				this.rightCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				yield return new WaitForEndOfFrame();
			}
			this.rightCG.alpha = 0f;
			yield break;
		}

		// Token: 0x04000EB5 RID: 3765
		[Header("Resources")]
		[SerializeField]
		private Scrollbar listScrollbar;

		// Token: 0x04000EB6 RID: 3766
		[SerializeField]
		private CanvasGroup leftCG;

		// Token: 0x04000EB7 RID: 3767
		[SerializeField]
		private CanvasGroup rightCG;

		// Token: 0x04000EB8 RID: 3768
		[Header("Settings")]
		[SerializeField]
		private float scrollTime = 5f;

		// Token: 0x04000EB9 RID: 3769
		[SerializeField]
		private float transitionSpeed = 4f;
	}
}
