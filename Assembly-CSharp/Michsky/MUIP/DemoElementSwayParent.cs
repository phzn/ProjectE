using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Michsky.MUIP
{
	// Token: 0x020001EB RID: 491
	public class DemoElementSwayParent : MonoBehaviour
	{
		// Token: 0x06000E27 RID: 3623 RVA: 0x00060AD4 File Offset: 0x0005ECD4
		private void Awake()
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				this.elements.Add(transform.GetComponent<DemoElementSway>());
			}
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00060B38 File Offset: 0x0005ED38
		public void DissolveAll(DemoElementSway currentSway)
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				if (this.elements[i] == currentSway)
				{
					this.elements[i].Active();
				}
				else
				{
					this.elements[i].Dissolve();
				}
			}
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00060B94 File Offset: 0x0005ED94
		public void HighlightAll()
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				this.elements[i].Highlight();
			}
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00060BC8 File Offset: 0x0005EDC8
		public void SetWindowManagerButton(int index)
		{
			if (this.elements.Count == 0)
			{
				base.StartCoroutine("SWMHelper", index);
				return;
			}
			for (int i = 0; i < this.elements.Count; i++)
			{
				if (i == index)
				{
					this.elements[i].WindowManagerSelect();
				}
				else if (this.elements[i].wmSelected)
				{
					this.elements[i].WindowManagerDeselect();
				}
			}
			if (this.titleAnimator == null)
			{
				return;
			}
			this.elementTitleHelper.text = this.elements[this.prevIndex].gameObject.name;
			this.elementTitle.text = this.elements[index].gameObject.name;
			this.titleAnimator.Play("Idle");
			this.titleAnimator.Play("Transition");
			this.prevIndex = index;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00060CC3 File Offset: 0x0005EEC3
		private IEnumerator SWMHelper(int index)
		{
			yield return new WaitForSeconds(0.1f);
			this.SetWindowManagerButton(index);
			yield break;
		}

		// Token: 0x04000EAC RID: 3756
		[SerializeField]
		private Animator titleAnimator;

		// Token: 0x04000EAD RID: 3757
		[SerializeField]
		private TextMeshProUGUI elementTitle;

		// Token: 0x04000EAE RID: 3758
		[SerializeField]
		private TextMeshProUGUI elementTitleHelper;

		// Token: 0x04000EAF RID: 3759
		private List<DemoElementSway> elements = new List<DemoElementSway>();

		// Token: 0x04000EB0 RID: 3760
		private int prevIndex;
	}
}
