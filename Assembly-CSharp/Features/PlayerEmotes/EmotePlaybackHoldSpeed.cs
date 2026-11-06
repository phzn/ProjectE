using System;
using UnityEngine;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E23 RID: 3619
	public sealed class EmotePlaybackHoldSpeed
	{
		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x06006BFF RID: 27647 RVA: 0x002B381B File Offset: 0x002B1A1B
		public bool IsManaging
		{
			get
			{
				return this._animator != null;
			}
		}

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x06006C00 RID: 27648 RVA: 0x002B3826 File Offset: 0x002B1A26
		public Animator TargetAnimator
		{
			get
			{
				return this._animator;
			}
		}

		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x06006C01 RID: 27649 RVA: 0x002B382E File Offset: 0x002B1A2E
		public float SavedSpeed
		{
			get
			{
				return this._saved;
			}
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x06006C02 RID: 27650 RVA: 0x002B3836 File Offset: 0x002B1A36
		public float WrittenSpeed
		{
			get
			{
				return this._written;
			}
		}

		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x06006C03 RID: 27651 RVA: 0x002B383E File Offset: 0x002B1A3E
		public float FrozenFraction
		{
			get
			{
				if (!this.IsManaging || this._saved <= 0f)
				{
					return 0f;
				}
				return Mathf.Clamp01(1f - this._written / this._saved);
			}
		}

		// Token: 0x06006C04 RID: 27652 RVA: 0x002B3874 File Offset: 0x002B1A74
		public bool Begin(Animator animator)
		{
			if (this.IsManaging)
			{
				if (animator == this._animator && animator != null && animator.enabled && EmotePlaybackHoldSpeed.Same(animator.speed, this._written))
				{
					return true;
				}
				this.Step(false, 0f, 0f, 0f, true, true);
			}
			this._animator = null;
			if (animator == null || !animator.enabled || animator.speed <= 0f)
			{
				return false;
			}
			this._animator = animator;
			this._saved = (this._written = animator.speed);
			return true;
		}

		// Token: 0x06006C05 RID: 27653 RVA: 0x002B3914 File Offset: 0x002B1B14
		public void Step(bool held, float deltaTime, float freezeSeconds, float resumeSeconds, bool snap, bool final)
		{
			if (!this.IsManaging)
			{
				return;
			}
			Animator animator = this._animator;
			if (animator == null)
			{
				this._animator = null;
				return;
			}
			if (final)
			{
				snap = true;
			}
			if (!animator.enabled && !final)
			{
				return;
			}
			if (animator.enabled && !EmotePlaybackHoldSpeed.Same(animator.speed, this._written))
			{
				this._animator = null;
				return;
			}
			float num = (held ? 0f : this._saved);
			float num2 = num;
			if (!snap)
			{
				float num3 = ((num < this._written) ? freezeSeconds : resumeSeconds);
				if (deltaTime <= 0f || float.IsInfinity(deltaTime))
				{
					deltaTime = 0f;
				}
				if (num3 > 0f)
				{
					num2 = Mathf.MoveTowards(this._written, num, this._saved / num3 * deltaTime);
				}
			}
			if (!held && EmotePlaybackHoldSpeed.Same(num2, this._saved))
			{
				num2 = this._saved;
			}
			if (num2 != this._written || !EmotePlaybackHoldSpeed.Same(animator.speed, num2))
			{
				animator.speed = num2;
				this._written = num2;
			}
			if (!held && num2 == this._saved)
			{
				this._animator = null;
			}
		}

		// Token: 0x06006C06 RID: 27654 RVA: 0x002B3A25 File Offset: 0x002B1C25
		private static bool Same(float a, float b)
		{
			return Mathf.Abs(a - b) <= 0.0001f;
		}

		// Token: 0x04006E88 RID: 28296
		private const float SpeedEpsilon = 0.0001f;

		// Token: 0x04006E89 RID: 28297
		private Animator _animator;

		// Token: 0x04006E8A RID: 28298
		private float _saved = 1f;

		// Token: 0x04006E8B RID: 28299
		private float _written = 1f;
	}
}
