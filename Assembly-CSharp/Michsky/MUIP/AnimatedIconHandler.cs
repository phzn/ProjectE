using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.MUIP
{
	// Token: 0x0200020D RID: 525
	[RequireComponent(typeof(Animator))]
	public class AnimatedIconHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x06000EC3 RID: 3779 RVA: 0x00063D20 File Offset: 0x00061F20
		private void Start()
		{
			if (this.iconAnimator == null)
			{
				this.iconAnimator = base.gameObject.GetComponent<Animator>();
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00063D41 File Offset: 0x00061F41
		public void PlayIn()
		{
			this.iconAnimator.Play("In");
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00063D53 File Offset: 0x00061F53
		public void PlayOut()
		{
			this.iconAnimator.Play("Out");
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00063D65 File Offset: 0x00061F65
		public void ClickEvent()
		{
			if (this.isClicked)
			{
				this.PlayOut();
				this.isClicked = false;
				return;
			}
			this.PlayIn();
			this.isClicked = true;
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00063D8A File Offset: 0x00061F8A
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.playType == AnimatedIconHandler.PlayType.Click)
			{
				this.ClickEvent();
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00063D9A File Offset: 0x00061F9A
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.playType == AnimatedIconHandler.PlayType.Hover)
			{
				this.iconAnimator.Play("In");
			}
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00063DB5 File Offset: 0x00061FB5
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.playType == AnimatedIconHandler.PlayType.Hover)
			{
				this.iconAnimator.Play("Out");
			}
		}

		// Token: 0x04000F73 RID: 3955
		[Header("Settings")]
		public AnimatedIconHandler.PlayType playType;

		// Token: 0x04000F74 RID: 3956
		public Animator iconAnimator;

		// Token: 0x04000F75 RID: 3957
		private bool isClicked;

		// Token: 0x0200020E RID: 526
		public enum PlayType
		{
			// Token: 0x04000F77 RID: 3959
			Click,
			// Token: 0x04000F78 RID: 3960
			Hover,
			// Token: 0x04000F79 RID: 3961
			None
		}
	}
}
