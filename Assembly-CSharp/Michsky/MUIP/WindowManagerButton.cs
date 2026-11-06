using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.MUIP
{
	// Token: 0x0200026E RID: 622
	[RequireComponent(typeof(Animator))]
	public class WindowManagerButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x0600104A RID: 4170 RVA: 0x0006C813 File Offset: 0x0006AA13
		private void Awake()
		{
			if (this.buttonAnimator == null)
			{
				this.buttonAnimator = base.gameObject.GetComponent<Animator>();
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				this.enableMobileMode = true;
			}
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0006C84C File Offset: 0x0006AA4C
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.enableMobileMode)
			{
				return;
			}
			if (!this.buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hover to Pressed") && !this.buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal to Pressed"))
			{
				this.buttonAnimator.Play("Normal to Hover");
			}
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0006C8A8 File Offset: 0x0006AAA8
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.enableMobileMode)
			{
				return;
			}
			if (!this.buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hover to Pressed") && !this.buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal to Pressed"))
			{
				this.buttonAnimator.Play("Hover to Normal");
			}
		}

		// Token: 0x040011DF RID: 4575
		public bool enableMobileMode;

		// Token: 0x040011E0 RID: 4576
		[HideInInspector]
		public Animator buttonAnimator;
	}
}
