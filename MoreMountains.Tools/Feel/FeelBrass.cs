using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000375 RID: 885
	[AddComponentMenu("")]
	public class FeelBrass : MonoBehaviour
	{
		// Token: 0x0600184F RID: 6223 RVA: 0x0006731C File Offset: 0x0006551C
		protected virtual void Update()
		{
			this.HandleInput();
			this.ControlLightIntensity();
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0006732A File Offset: 0x0006552A
		protected virtual void HandleInput()
		{
			if (FeelDemosInputHelper.CheckMainActionInputPressedThisFrame())
			{
				this.SpecialDanceMove();
			}
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x00067339 File Offset: 0x00065539
		protected virtual void ControlLightIntensity()
		{
			this.TargetLight.intensity = this.TargetAnalyzer.NormalizedBufferedAmplitude * 5f;
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x00067357 File Offset: 0x00065557
		protected virtual void SpecialDanceMove()
		{
			if (Time.time - this._lastMoveStartedAt >= this.CooldownDuration)
			{
				MMFeedbacks specialDanceMoveFeedbacks = this.SpecialDanceMoveFeedbacks;
				if (specialDanceMoveFeedbacks != null)
				{
					specialDanceMoveFeedbacks.PlayFeedbacks();
				}
				this._lastMoveStartedAt = Time.time;
			}
		}

		// Token: 0x04001C32 RID: 7218
		[Header("Bindings")]
		public MMAudioAnalyzer TargetAnalyzer;

		// Token: 0x04001C33 RID: 7219
		public Light TargetLight;

		// Token: 0x04001C34 RID: 7220
		[Header("Cooldown")]
		[Tooltip("a duration, in seconds, between two special dance moves, during which moves are prevented")]
		public float CooldownDuration = 0.1f;

		// Token: 0x04001C35 RID: 7221
		[Header("Feedbacks")]
		[Tooltip("a feedback to play when doing a special dance move")]
		public MMFeedbacks SpecialDanceMoveFeedbacks;

		// Token: 0x04001C36 RID: 7222
		protected float _lastMoveStartedAt = -100f;
	}
}
