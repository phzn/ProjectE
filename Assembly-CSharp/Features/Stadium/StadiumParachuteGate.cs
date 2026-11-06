using System;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C45 RID: 3141
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(130)]
	public sealed class StadiumParachuteGate : MonoBehaviour
	{
		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06005AB4 RID: 23220 RVA: 0x0023895D File Offset: 0x00236B5D
		public bool IsPassable
		{
			get
			{
				return base.isActiveAndEnabled && this.barrier != null && !this.barrier.enabled;
			}
		}

		// Token: 0x06005AB5 RID: 23221 RVA: 0x00238985 File Offset: 0x00236B85
		public void Configure(StadiumParachuteZone owner, StadiumParachuteLaunchPoint point, Transform pivot, BoxCollider blocker)
		{
			this.zone = owner;
			this.launchPoint = point;
			this.hinge = pivot;
			this.barrier = blocker;
			this.CloseImmediate();
		}

		// Token: 0x06005AB6 RID: 23222 RVA: 0x002389AC File Offset: 0x00236BAC
		private void Update()
		{
			StadiumParachuteController stadiumParachuteController = ((this.zone != null) ? this.zone.BoundPlayer : null);
			bool flag = stadiumParachuteController != null && this.launchPoint != null && this.launchPoint.ContainsApproach(stadiumParachuteController.transform.position);
			if (!flag)
			{
				this._departed = false;
			}
			bool flag2 = flag && !this._departed && stadiumParachuteController.CanApproachLaunchPoint;
			this.Step(flag2, Time.deltaTime);
		}

		// Token: 0x06005AB7 RID: 23223 RVA: 0x00238A30 File Offset: 0x00236C30
		internal void Step(bool allowed, float deltaTime)
		{
			this._opening = Mathf.MoveTowards(this._opening, allowed ? 1f : 0f, Mathf.Max(0f, deltaTime) / (allowed ? this.openingSeconds : this.closingSeconds));
			if (this.hinge != null)
			{
				this.hinge.localRotation = Quaternion.Euler(0f, -100f * Mathf.SmoothStep(0f, 1f, this._opening), 0f);
			}
			if (this.barrier != null)
			{
				this.barrier.enabled = !allowed || this._opening < 0.99f;
			}
		}

		// Token: 0x06005AB8 RID: 23224 RVA: 0x00238AE9 File Offset: 0x00236CE9
		public void NotifyDeparture()
		{
			this._departed = true;
			this.Step(false, 0f);
		}

		// Token: 0x06005AB9 RID: 23225 RVA: 0x00238AFE File Offset: 0x00236CFE
		private void CloseImmediate()
		{
			this._opening = 0f;
			this._departed = false;
			this.Step(false, 0f);
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x00238B1E File Offset: 0x00236D1E
		private void OnEnable()
		{
			this.CloseImmediate();
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x00238B1E File Offset: 0x00236D1E
		private void OnDisable()
		{
			this.CloseImmediate();
		}

		// Token: 0x04005BAA RID: 23466
		[SerializeField]
		private StadiumParachuteZone zone;

		// Token: 0x04005BAB RID: 23467
		[SerializeField]
		private StadiumParachuteLaunchPoint launchPoint;

		// Token: 0x04005BAC RID: 23468
		[SerializeField]
		private Transform hinge;

		// Token: 0x04005BAD RID: 23469
		[SerializeField]
		private BoxCollider barrier;

		// Token: 0x04005BAE RID: 23470
		[SerializeField]
		[Min(0.1f)]
		private float openingSeconds = 0.5f;

		// Token: 0x04005BAF RID: 23471
		[SerializeField]
		[Min(0.1f)]
		private float closingSeconds = 0.65f;

		// Token: 0x04005BB0 RID: 23472
		private float _opening;

		// Token: 0x04005BB1 RID: 23473
		private bool _departed;
	}
}
