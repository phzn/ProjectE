using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.MUIP
{
	// Token: 0x020001E5 RID: 485
	public class DemoElementSway : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x06000DF6 RID: 3574 RVA: 0x00060228 File Offset: 0x0005E428
		private void Awake()
		{
			if (this.swayParent == null)
			{
				DemoElementSwayParent component = base.transform.parent.GetComponent<DemoElementSwayParent>();
				if (component == null)
				{
					base.transform.parent.gameObject.AddComponent<DemoElementSwayParent>();
				}
				this.swayParent = component;
			}
			this.defaultPos = this.swayObject.anchoredPosition;
			this.normalCG.alpha = 1f;
			this.highlightedCG.alpha = 0f;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x000602AC File Offset: 0x0005E4AC
		private void Update()
		{
			if (this.allowSway)
			{
				this.cursorPos = Input.mousePosition;
			}
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				this.ProcessOverlay();
				return;
			}
			if (this.mainCanvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				this.ProcessSSC();
				return;
			}
			if (this.mainCanvas.renderMode == RenderMode.WorldSpace)
			{
				this.ProcessWorldSpace();
			}
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0006030C File Offset: 0x0005E50C
		private void ProcessOverlay()
		{
			if (this.allowSway)
			{
				this.swayObject.position = Vector2.Lerp(this.swayObject.position, this.cursorPos, Time.deltaTime * this.smoothness);
				return;
			}
			this.swayObject.localPosition = Vector2.Lerp(this.swayObject.localPosition, this.defaultPos, Time.deltaTime * this.smoothness);
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00060398 File Offset: 0x0005E598
		private void ProcessSSC()
		{
			if (this.allowSway)
			{
				this.swayObject.position = Vector2.Lerp(this.swayObject.position, Camera.main.ScreenToWorldPoint(this.cursorPos), Time.deltaTime * this.smoothness);
				return;
			}
			this.swayObject.localPosition = Vector2.Lerp(this.swayObject.localPosition, this.defaultPos, Time.deltaTime * this.smoothness);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0006042C File Offset: 0x0005E62C
		private void ProcessWorldSpace()
		{
			if (this.allowSway)
			{
				Vector3 vector = new Vector3(this.cursorPos.x, this.cursorPos.y, this.mainCanvas.transform.position.z / 6f);
				this.swayObject.position = Vector3.Lerp(this.swayObject.position, Camera.main.ScreenToWorldPoint(vector), Time.deltaTime * this.smoothness);
				return;
			}
			this.swayObject.localPosition = Vector3.Lerp(this.swayObject.localPosition, this.defaultPos, Time.deltaTime * this.smoothness);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x000604DE File Offset: 0x0005E6DE
		public void Dissolve()
		{
			if (this.wmSelected)
			{
				return;
			}
			base.StopCoroutine("DissolveHelper");
			base.StopCoroutine("HighlightHelper");
			base.StopCoroutine("ActiveHelper");
			base.StartCoroutine("DissolveHelper");
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00060516 File Offset: 0x0005E716
		public void Highlight()
		{
			if (this.wmSelected)
			{
				return;
			}
			base.StopCoroutine("DissolveHelper");
			base.StopCoroutine("HighlightHelper");
			base.StopCoroutine("ActiveHelper");
			base.StartCoroutine("HighlightHelper");
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0006054E File Offset: 0x0005E74E
		public void Active()
		{
			if (this.wmSelected)
			{
				return;
			}
			base.StopCoroutine("DissolveHelper");
			base.StopCoroutine("HighlightHelper");
			base.StopCoroutine("HighlightHelper");
			base.StartCoroutine("ActiveHelper");
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00060586 File Offset: 0x0005E786
		public void WindowManagerSelect()
		{
			this.wmSelected = true;
			base.StopCoroutine("ActiveHelper");
			base.StopCoroutine("HighlightHelper");
			base.StartCoroutine("WMSelectHelper");
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x000605B1 File Offset: 0x0005E7B1
		public void WindowManagerDeselect()
		{
			this.wmSelected = false;
			base.StartCoroutine("WMDeselectHelper");
			base.StartCoroutine("DissolveHelper");
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x000605D2 File Offset: 0x0005E7D2
		public void OnPointerEnter(PointerEventData data)
		{
			this.allowSway = true;
			this.swayParent.DissolveAll(this);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x000605E7 File Offset: 0x0005E7E7
		public void OnPointerExit(PointerEventData data)
		{
			this.allowSway = false;
			this.swayParent.HighlightAll();
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x000605FB File Offset: 0x0005E7FB
		public void OnPointerClick(PointerEventData data)
		{
			this.onClick.Invoke();
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00060608 File Offset: 0x0005E808
		private IEnumerator DissolveHelper()
		{
			while (this.normalCG.alpha > this.dissolveAlpha)
			{
				this.normalCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				this.highlightedCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				yield return null;
			}
			this.highlightedCG.alpha = 0f;
			this.normalCG.alpha = this.dissolveAlpha;
			this.highlightedCG.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00060617 File Offset: 0x0005E817
		private IEnumerator HighlightHelper()
		{
			while (this.normalCG.alpha < 1f)
			{
				this.normalCG.alpha += Time.unscaledDeltaTime * this.transitionSpeed;
				this.highlightedCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				yield return null;
			}
			this.normalCG.alpha = 1f;
			this.highlightedCG.alpha = 0f;
			this.highlightedCG.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00060626 File Offset: 0x0005E826
		private IEnumerator ActiveHelper()
		{
			this.highlightedCG.gameObject.SetActive(true);
			while (this.highlightedCG.alpha < 1f)
			{
				this.normalCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				this.highlightedCG.alpha += Time.unscaledDeltaTime * this.transitionSpeed;
				yield return null;
			}
			this.highlightedCG.alpha = 1f;
			this.normalCG.alpha = 0f;
			yield break;
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00060635 File Offset: 0x0005E835
		private IEnumerator WMSelectHelper()
		{
			this.selectedCG.gameObject.SetActive(true);
			while (this.selectedCG.alpha < 1f)
			{
				this.normalCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				this.highlightedCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				this.selectedCG.alpha += Time.unscaledDeltaTime * this.transitionSpeed;
				yield return null;
			}
			this.highlightedCG.alpha = 0f;
			this.normalCG.alpha = 0f;
			this.selectedCG.alpha = 1f;
			yield break;
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00060644 File Offset: 0x0005E844
		private IEnumerator WMDeselectHelper()
		{
			while (this.selectedCG.alpha > 0f)
			{
				this.selectedCG.alpha -= Time.unscaledDeltaTime * this.transitionSpeed;
				yield return null;
			}
			this.selectedCG.alpha = 0f;
			this.selectedCG.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04000E8F RID: 3727
		[Header("Resources")]
		[SerializeField]
		private DemoElementSwayParent swayParent;

		// Token: 0x04000E90 RID: 3728
		[SerializeField]
		private Canvas mainCanvas;

		// Token: 0x04000E91 RID: 3729
		[SerializeField]
		private RectTransform swayObject;

		// Token: 0x04000E92 RID: 3730
		[SerializeField]
		private CanvasGroup normalCG;

		// Token: 0x04000E93 RID: 3731
		[SerializeField]
		private CanvasGroup highlightedCG;

		// Token: 0x04000E94 RID: 3732
		[SerializeField]
		private CanvasGroup selectedCG;

		// Token: 0x04000E95 RID: 3733
		[Header("Settings")]
		[SerializeField]
		private float smoothness = 10f;

		// Token: 0x04000E96 RID: 3734
		[SerializeField]
		private float transitionSpeed = 8f;

		// Token: 0x04000E97 RID: 3735
		[SerializeField]
		[Range(0f, 1f)]
		private float dissolveAlpha = 0.5f;

		// Token: 0x04000E98 RID: 3736
		[Header("Events")]
		[SerializeField]
		private UnityEvent onClick;

		// Token: 0x04000E99 RID: 3737
		private bool allowSway;

		// Token: 0x04000E9A RID: 3738
		[HideInInspector]
		public bool wmSelected;

		// Token: 0x04000E9B RID: 3739
		private Vector3 cursorPos;

		// Token: 0x04000E9C RID: 3740
		private Vector2 defaultPos;
	}
}
