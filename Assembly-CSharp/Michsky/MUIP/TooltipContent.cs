using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000246 RID: 582
	[AddComponentMenu("Modern UI Pack/Tooltip/Tooltip Content")]
	public class TooltipContent : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06000FBE RID: 4030 RVA: 0x000688A8 File Offset: 0x00066AA8
		private void Start()
		{
			if (this.tooltipRect == null || this.descriptionText == null)
			{
				try
				{
					this.tooltipRect = GameObject.Find("Tooltip Rect");
					this.descriptionText = this.tooltipRect.transform.GetComponentInChildren<TextMeshProUGUI>();
				}
				catch
				{
					Debug.LogError("<b>[Tooltip Content]</b> Tooltip Rect is missing.", this);
					return;
				}
			}
			if (this.tooltipRect != null)
			{
				this.tpManager = this.tooltipRect.GetComponentInParent<TooltipManager>();
				this.tooltipAnimator = this.tooltipRect.GetComponentInParent<Animator>();
			}
			if (this.tpManager.contentLE == null)
			{
				this.tpManager.contentLE = this.descriptionText.GetComponent<LayoutElement>();
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00068974 File Offset: 0x00066B74
		public void ProcessEnter()
		{
			if (this.tooltipRect == null)
			{
				return;
			}
			this.descriptionText.text = this.description;
			this.tpManager.allowUpdate = true;
			this.tpManager.currentTooltip = this;
			this.CheckForContentWidth();
			base.StopCoroutine("DisableAnimator");
			this.tooltipAnimator.gameObject.SetActive(false);
			this.tooltipAnimator.gameObject.SetActive(true);
			if (this.delay == 0f)
			{
				this.tooltipAnimator.Play("In");
			}
			else
			{
				base.StartCoroutine("ShowTooltip");
			}
			if (this.forceToUpdate)
			{
				base.StartCoroutine("UpdateLayoutPosition");
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00068A2C File Offset: 0x00066C2C
		public void ProcessExit()
		{
			if (this.tooltipRect == null)
			{
				return;
			}
			if (this.delay != 0f)
			{
				base.StopCoroutine("ShowTooltip");
				if (this.tooltipAnimator.GetCurrentAnimatorStateInfo(0).IsName("In"))
				{
					this.tooltipAnimator.Play("Out");
				}
			}
			else
			{
				this.tooltipAnimator.Play("Out");
			}
			this.tpManager.allowUpdate = false;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00068AA9 File Offset: 0x00066CA9
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.ProcessEnter();
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00068AB1 File Offset: 0x00066CB1
		public void OnPointerExit(PointerEventData eventData)
		{
			this.ProcessExit();
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00068AB9 File Offset: 0x00066CB9
		public void OnMouseEnter()
		{
			if (this.useIn3D)
			{
				this.ProcessEnter();
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00068AC9 File Offset: 0x00066CC9
		public void OnMouseExit()
		{
			if (this.useIn3D)
			{
				this.ProcessExit();
			}
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00068AD9 File Offset: 0x00066CD9
		public void CheckForContentWidth()
		{
			this.LayoutElementCreator();
			base.StartCoroutine("CalculateContentWidth");
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00068AF0 File Offset: 0x00066CF0
		private void LayoutElementCreator()
		{
			if (this.tpManager.contentLE == null)
			{
				this.descriptionText.gameObject.AddComponent<LayoutElement>();
				this.tpManager.contentLE = this.descriptionText.GetComponent<LayoutElement>();
			}
			this.tpManager.contentLE.preferredWidth = this.tpManager.preferredWidth;
			this.tpManager.contentLE.enabled = false;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00068B63 File Offset: 0x00066D63
		private IEnumerator CalculateContentWidth()
		{
			yield return new WaitForSecondsRealtime(0.05f);
			if (this.descriptionText.GetComponent<RectTransform>().sizeDelta.x >= this.tpManager.preferredWidth + 1f)
			{
				this.tpManager.contentLE.enabled = true;
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.tpManager.contentLE.gameObject.GetComponent<RectTransform>());
			this.tpManager.contentLE.preferredWidth = this.tpManager.preferredWidth;
			yield break;
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00068B72 File Offset: 0x00066D72
		private IEnumerator ShowTooltip()
		{
			yield return new WaitForSecondsRealtime(this.delay);
			this.tooltipAnimator.Play("In");
			base.StopCoroutine("ShowTooltip");
			yield break;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00068B81 File Offset: 0x00066D81
		private IEnumerator UpdateLayoutPosition()
		{
			yield return new WaitForSecondsRealtime(0.05f);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.tooltipAnimator.gameObject.GetComponent<RectTransform>());
			yield break;
		}

		// Token: 0x040010C0 RID: 4288
		[Header("Content")]
		[TextArea]
		public string description;

		// Token: 0x040010C1 RID: 4289
		public float delay;

		// Token: 0x040010C2 RID: 4290
		[Header("Resources")]
		public GameObject tooltipRect;

		// Token: 0x040010C3 RID: 4291
		public TextMeshProUGUI descriptionText;

		// Token: 0x040010C4 RID: 4292
		[Header("Settings")]
		public bool forceToUpdate;

		// Token: 0x040010C5 RID: 4293
		public bool useIn3D;

		// Token: 0x040010C6 RID: 4294
		private TooltipManager tpManager;

		// Token: 0x040010C7 RID: 4295
		[HideInInspector]
		public Animator tooltipAnimator;
	}
}
