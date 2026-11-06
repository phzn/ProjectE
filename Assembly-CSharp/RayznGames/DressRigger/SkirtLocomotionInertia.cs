using System;
using Invector.vCharacterController;
using MagicaCloth2;
using UnityEngine;

namespace RayznGames.DressRigger
{
	// Token: 0x0200078E RID: 1934
	[DisallowMultipleComponent]
	[AddComponentMenu("Dress Rigger/Skirt Locomotion Inertia")]
	public sealed class SkirtLocomotionInertia : MonoBehaviour
	{
		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x060032C5 RID: 12997 RVA: 0x0012034E File Offset: 0x0011E54E
		// (set) Token: 0x060032C6 RID: 12998 RVA: 0x00120356 File Offset: 0x0011E556
		public float RunningWorldInertia
		{
			get
			{
				return this.runningWorldInertia;
			}
			set
			{
				this.runningWorldInertia = Mathf.Clamp01(value);
			}
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x00120364 File Offset: 0x0011E564
		private void OnEnable()
		{
			this.ResolveCloth();
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x0012036C File Offset: 0x0011E56C
		private void LateUpdate()
		{
			this.ResolveCloth();
			if (this.cloth == null || !this.hasOriginalInertia)
			{
				return;
			}
			if (this.motor == null || !base.transform.IsChildOf(this.motor.transform))
			{
				this.motor = base.GetComponentInParent<vThirdPersonMotor>(true);
			}
			if (this.cloth.isActiveAndEnabled && this.motor != null && this.motor.isActiveAndEnabled && (this.motor.isRunning || this.motor.isSprinting) && this.motor.inputMagnitude > 0.1f && !this.motor.customAction && !this.motor.ragdolled && !this.motor.isCrouching && !this.motor.isDead)
			{
				this.overridingInertia = true;
				this.SetWorldInertia(Mathf.Min(this.originalWorldInertia, this.runningWorldInertia));
				return;
			}
			this.RestoreInertia();
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x0012047C File Offset: 0x0011E67C
		private void ResolveCloth()
		{
			if (this.cloth == null)
			{
				this.cloth = base.GetComponent<MagicaCloth>();
			}
			if (this.cloth == null || this.hasOriginalInertia)
			{
				return;
			}
			this.originalWorldInertia = this.cloth.SerializeData.inertiaConstraint.worldInertia;
			this.hasOriginalInertia = true;
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x001204DC File Offset: 0x0011E6DC
		private void SetWorldInertia(float value)
		{
			if (this.cloth == null || Mathf.Approximately(this.cloth.SerializeData.inertiaConstraint.worldInertia, value))
			{
				return;
			}
			this.cloth.SerializeData.inertiaConstraint.worldInertia = value;
			this.cloth.SetParameterChange();
		}

		// Token: 0x060032CB RID: 13003 RVA: 0x00120536 File Offset: 0x0011E736
		private void RestoreInertia()
		{
			if (!this.overridingInertia)
			{
				return;
			}
			this.SetWorldInertia(this.originalWorldInertia);
			this.overridingInertia = false;
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x00120554 File Offset: 0x0011E754
		private void OnDisable()
		{
			this.RestoreInertia();
			this.motor = null;
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x00120563 File Offset: 0x0011E763
		private void OnDestroy()
		{
			this.RestoreInertia();
		}

		// Token: 0x0400336D RID: 13165
		[SerializeField]
		[Range(0f, 1f)]
		[Tooltip("Maximum world movement inertia during running. Zero follows root travel fully; local cloth motion and wind remain active.")]
		private float runningWorldInertia = 0.3f;

		// Token: 0x0400336E RID: 13166
		private MagicaCloth cloth;

		// Token: 0x0400336F RID: 13167
		private vThirdPersonMotor motor;

		// Token: 0x04003370 RID: 13168
		private float originalWorldInertia;

		// Token: 0x04003371 RID: 13169
		private bool hasOriginalInertia;

		// Token: 0x04003372 RID: 13170
		private bool overridingInertia;
	}
}
