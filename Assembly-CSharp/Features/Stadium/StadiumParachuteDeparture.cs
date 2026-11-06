using System;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C47 RID: 3143
	public sealed class StadiumParachuteDeparture
	{
		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06005AC2 RID: 23234 RVA: 0x00238D8A File Offset: 0x00236F8A
		// (set) Token: 0x06005AC3 RID: 23235 RVA: 0x00238D92 File Offset: 0x00236F92
		public StadiumParachuteLaunchPoint Point { get; private set; }

		// Token: 0x06005AC4 RID: 23236 RVA: 0x00238D9B File Offset: 0x00236F9B
		public void Reset()
		{
			this.Point = null;
			this._hasSample = false;
			this._crossedOpening = false;
		}

		// Token: 0x06005AC5 RID: 23237 RVA: 0x00238DB4 File Offset: 0x00236FB4
		public void Observe(StadiumParachuteZone zone, Vector3 position, bool grounded, bool allowed, float time)
		{
			if (!allowed || zone == null || !zone.Contains(position))
			{
				this.Reset();
				return;
			}
			if (this._hasSample && (position - this._lastPosition).sqrMagnitude > 9f)
			{
				this.Reset();
				return;
			}
			Vector3 lastPosition = this._lastPosition;
			bool hasSample = this._hasSample;
			this._lastPosition = position;
			this._hasSample = true;
			if (this.Point != null && (!this.Point.isActiveAndEnabled || !this.Point.transform.IsChildOf(zone.transform)))
			{
				this.Reset();
				return;
			}
			if (this.Point != null && !this._crossedOpening && hasSample && (this.Point.gate == null || this.Point.gate.IsPassable) && this.Point.CrossesOpening(lastPosition, position))
			{
				this._crossedOpening = true;
			}
			if (this.Point != null && this._crossedOpening)
			{
				if (!this.Point.ContainsDeparture(position))
				{
					this.Reset();
					return;
				}
				if (grounded && this.Point.ContainsApproach(position))
				{
					this._lastGroundedTime = time;
					this._lastGroundedHeight = position.y;
				}
				if (time - this._lastGroundedTime > 2f)
				{
					this.Reset();
				}
				return;
			}
			else
			{
				if (grounded)
				{
					StadiumParachuteLaunchPoint stadiumParachuteLaunchPoint = zone.FindApproach(position);
					this.Point = ((stadiumParachuteLaunchPoint != null && (stadiumParachuteLaunchPoint.gate == null || stadiumParachuteLaunchPoint.gate.IsPassable)) ? stadiumParachuteLaunchPoint : null);
					this._crossedOpening = this.Point != null && this.Point.ContainsDeparture(position);
					this._lastGroundedTime = time;
					this._lastGroundedHeight = position.y;
					return;
				}
				if (this.Point == null || time - this._lastGroundedTime > 2f || !this.Point.ContainsApproach(position))
				{
					this.Reset();
				}
				return;
			}
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x00238FC4 File Offset: 0x002371C4
		public bool CanDepart(Vector3 position, float time)
		{
			return this.Point != null && this._crossedOpening && time - this._lastGroundedTime <= 2f && this.Point.ContainsDeparture(position) && this._lastGroundedHeight - position.y >= 2f;
		}

		// Token: 0x04005BB8 RID: 23480
		private float _lastGroundedTime;

		// Token: 0x04005BB9 RID: 23481
		private float _lastGroundedHeight;

		// Token: 0x04005BBA RID: 23482
		private Vector3 _lastPosition;

		// Token: 0x04005BBB RID: 23483
		private bool _hasSample;

		// Token: 0x04005BBC RID: 23484
		private bool _crossedOpening;

		// Token: 0x04005BBD RID: 23485
		public const float Lifetime = 2f;

		// Token: 0x04005BBE RID: 23486
		public const float OpeningDrop = 2f;
	}
}
