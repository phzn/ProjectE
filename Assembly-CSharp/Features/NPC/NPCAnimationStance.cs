using System;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ED0 RID: 3792
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator))]
	public sealed class NPCAnimationStance : MonoBehaviour
	{
		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06007191 RID: 29073 RVA: 0x002E24BC File Offset: 0x002E06BC
		public float Stance
		{
			get
			{
				return this.stance;
			}
		}

		// Token: 0x06007192 RID: 29074 RVA: 0x002E24C4 File Offset: 0x002E06C4
		public void SetStance(float value)
		{
			this.stance = Mathf.Clamp01(value);
			this.Apply();
		}

		// Token: 0x06007193 RID: 29075 RVA: 0x002E24D8 File Offset: 0x002E06D8
		private void OnEnable()
		{
			this.animator = base.GetComponent<Animator>();
			this.cachedController = null;
			this.Apply();
		}

		// Token: 0x06007194 RID: 29076 RVA: 0x002E24F3 File Offset: 0x002E06F3
		private void Update()
		{
			this.Apply();
		}

		// Token: 0x06007195 RID: 29077 RVA: 0x002E24FC File Offset: 0x002E06FC
		private void Apply()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponent<Animator>();
			}
			if (!this.animator.isActiveAndEnabled || this.animator.runtimeAnimatorController == null)
			{
				return;
			}
			if (this.cachedController != this.animator.runtimeAnimatorController)
			{
				this.cachedController = this.animator.runtimeAnimatorController;
				this.hasStance = false;
				foreach (AnimatorControllerParameter animatorControllerParameter in this.animator.parameters)
				{
					if (animatorControllerParameter.nameHash == NPCAnimationStance.StanceId && animatorControllerParameter.type == AnimatorControllerParameterType.Float)
					{
						this.hasStance = true;
						break;
					}
				}
			}
			if (this.hasStance && !Mathf.Approximately(this.animator.GetFloat(NPCAnimationStance.StanceId), this.stance))
			{
				this.animator.SetFloat(NPCAnimationStance.StanceId, this.stance);
			}
		}

		// Token: 0x0400748F RID: 29839
		private static readonly int StanceId = Animator.StringToHash("Stance");

		// Token: 0x04007490 RID: 29840
		[SerializeField]
		[Range(0f, 1f)]
		private float stance;

		// Token: 0x04007491 RID: 29841
		private Animator animator;

		// Token: 0x04007492 RID: 29842
		private RuntimeAnimatorController cachedController;

		// Token: 0x04007493 RID: 29843
		private bool hasStance;
	}
}
