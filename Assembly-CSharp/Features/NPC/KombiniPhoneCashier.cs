using System;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EC9 RID: 3785
	[DisallowMultipleComponent]
	public sealed class KombiniPhoneCashier : MonoBehaviour
	{
		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06007144 RID: 28996 RVA: 0x002DFC6B File Offset: 0x002DDE6B
		public Animator PoseAnimator
		{
			get
			{
				return this.animator;
			}
		}

		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06007145 RID: 28997 RVA: 0x002DFC73 File Offset: 0x002DDE73
		public GameObject Smartphone
		{
			get
			{
				return this.smartphone;
			}
		}

		// Token: 0x06007146 RID: 28998 RVA: 0x002DFC7B File Offset: 0x002DDE7B
		public void Configure(Animator poseAnimator, GameObject phone)
		{
			this.animator = poseAnimator;
			this.smartphone = phone;
		}

		// Token: 0x06007147 RID: 28999 RVA: 0x002DFC8B File Offset: 0x002DDE8B
		private void OnEnable()
		{
			if (this.smartphone != null)
			{
				this.smartphone.SetActive(true);
			}
		}

		// Token: 0x0400741F RID: 29727
		[SerializeField]
		private Animator animator;

		// Token: 0x04007420 RID: 29728
		[SerializeField]
		private GameObject smartphone;
	}
}
