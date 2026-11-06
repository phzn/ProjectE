using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200000A RID: 10
	public sealed class AnimatorNormalUpdateModeLease : IDisposable
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00003ADC File Offset: 0x00001CDC
		private AnimatorNormalUpdateModeLease(Animator animator, AnimatorNormalUpdateModeLease.SharedState state)
		{
			this._animator = animator;
			this._state = state;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003AF4 File Offset: 0x00001CF4
		public static AnimatorNormalUpdateModeLease Acquire(Animator animator)
		{
			if (animator == null)
			{
				return null;
			}
			AnimatorNormalUpdateModeLease.SharedState value = AnimatorNormalUpdateModeLease.States.GetValue(animator, (Animator _) => new AnimatorNormalUpdateModeLease.SharedState());
			if (value.Owners == 0)
			{
				value.OriginalMode = animator.updateMode;
			}
			value.Owners++;
			animator.updateMode = AnimatorUpdateMode.Normal;
			return new AnimatorNormalUpdateModeLease(animator, value);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003B68 File Offset: 0x00001D68
		public void Dispose()
		{
			if (this._state == null)
			{
				return;
			}
			AnimatorNormalUpdateModeLease.SharedState state = this._state;
			Animator animator = this._animator;
			this._state = null;
			this._animator = null;
			state.Owners--;
			if (state.Owners == 0 && animator != null)
			{
				animator.updateMode = state.OriginalMode;
			}
		}

		// Token: 0x0400004C RID: 76
		private static readonly ConditionalWeakTable<Animator, AnimatorNormalUpdateModeLease.SharedState> States = new ConditionalWeakTable<Animator, AnimatorNormalUpdateModeLease.SharedState>();

		// Token: 0x0400004D RID: 77
		private Animator _animator;

		// Token: 0x0400004E RID: 78
		private AnimatorNormalUpdateModeLease.SharedState _state;

		// Token: 0x02000023 RID: 35
		private sealed class SharedState
		{
			// Token: 0x0400015C RID: 348
			public int Owners;

			// Token: 0x0400015D RID: 349
			public AnimatorUpdateMode OriginalMode;
		}
	}
}
