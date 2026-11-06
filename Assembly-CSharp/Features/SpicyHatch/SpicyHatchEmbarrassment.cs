using System;
using Features.Detection;
using Features.PlayerEmotes;
using RayznGames.VehicleSystem;
using UnityEngine;

namespace Features.SpicyHatch
{
	// Token: 0x02000C52 RID: 3154
	public class SpicyHatchEmbarrassment : MonoBehaviour
	{
		// Token: 0x06005B1E RID: 23326 RVA: 0x0023AD0C File Offset: 0x00238F0C
		public static void NotifySkirtLifted(float duration)
		{
			if (duration <= 0f)
			{
				return;
			}
			if (SpicyHatchEmbarrassment._instance == null)
			{
				SpicyHatchEmbarrassment._instance = new GameObject("SpicyHatchEmbarrassment").AddComponent<SpicyHatchEmbarrassment>();
			}
			SpicyHatchEmbarrassment._instance._until = Time.time + duration;
			SpicyHatchEmbarrassment._instance.enabled = true;
		}

		// Token: 0x06005B1F RID: 23327 RVA: 0x0023AD60 File Offset: 0x00238F60
		private bool EmotePlaying()
		{
			if (this._emotes == null && !this._emotesSearched)
			{
				this._emotes = Object.FindObjectOfType<GameplayEmoteController>(true);
				this._emotesSearched = true;
			}
			return this._emotes != null && this._emotes.IsPlaying;
		}

		// Token: 0x06005B20 RID: 23328 RVA: 0x0023ADB4 File Offset: 0x00238FB4
		private bool RagdollRecovering()
		{
			if (this._ragdollRecovery == null && !this._ragdollSearched)
			{
				this._ragdollRecovery = Object.FindObjectOfType<CielRagdollRecovery>(true);
				this._ragdollSearched = true;
			}
			return this._ragdollRecovery != null && this._ragdollRecovery.IsRecovering;
		}

		// Token: 0x06005B21 RID: 23329 RVA: 0x0023AE08 File Offset: 0x00239008
		private void Update()
		{
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance == null)
			{
				return;
			}
			bool flag = Time.time < this._until;
			if (flag)
			{
				if (!this._faceHeld || !instance.IsFacialExpressionOverriddenForAction)
				{
					this._animator = instance.PlayerAnimator;
					instance.DisableFacialExpressionForAction();
					instance.SetExpression(4);
					if (this._animator != null && !string.IsNullOrEmpty(instance.EmotionBlendParamName))
					{
						this._animator.SetFloat(instance.EmotionBlendParamName, -1f);
					}
					this._faceHeld = true;
				}
			}
			else if (this._faceHeld)
			{
				this._faceHeld = false;
				if (this._animator != null && !string.IsNullOrEmpty(instance.EmotionBlendParamName))
				{
					this._animator.SetFloat(instance.EmotionBlendParamName, instance.EmotionBalance / 100f);
				}
				instance.RestoreFacialExpression();
			}
			bool flag2 = flag && !this.EmotePlaying() && !this.RagdollRecovering();
			if (flag2 && !this._bodyHeld)
			{
				this.AcquireBody(instance);
			}
			if (this._bodyHeld)
			{
				if (flag2)
				{
					this.DriveEmbLayer(1f, 0.6f);
				}
				else if (this.DriveEmbLayer(0f, 1f))
				{
					this.ReleaseBody(instance);
				}
			}
			if (!flag && !this._faceHeld && !this._bodyHeld)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06005B22 RID: 23330 RVA: 0x0023AF64 File Offset: 0x00239164
		private void AcquireBody(PlayerEmotionController emo)
		{
			this._animator = emo.PlayerAnimator;
			if (this._animator != null && this._embLayerIndex < 0)
			{
				this._embLayerIndex = this._animator.GetLayerIndex(emo.EmbarrassedLayerName);
			}
			emo.DisableEmotionLayersForAction();
			this._bodyHeld = true;
		}

		// Token: 0x06005B23 RID: 23331 RVA: 0x0023AFB8 File Offset: 0x002391B8
		private void ReleaseBody(PlayerEmotionController emo)
		{
			this._bodyHeld = false;
			emo.RestoreEmotionLayers();
		}

		// Token: 0x06005B24 RID: 23332 RVA: 0x0023AFC8 File Offset: 0x002391C8
		private bool DriveEmbLayer(float target, float blendSeconds)
		{
			if (this._animator == null || this._embLayerIndex < 0)
			{
				return true;
			}
			float num = ((blendSeconds > 0.01f) ? (Time.deltaTime / blendSeconds) : 1f);
			float num2 = Mathf.MoveTowards(this._animator.GetLayerWeight(this._embLayerIndex), target, num);
			this._animator.SetLayerWeight(this._embLayerIndex, num2);
			return Mathf.Approximately(num2, target);
		}

		// Token: 0x06005B25 RID: 23333 RVA: 0x0023B038 File Offset: 0x00239238
		private void OnDisable()
		{
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance == null)
			{
				this._faceHeld = false;
				this._bodyHeld = false;
				return;
			}
			if (this._bodyHeld)
			{
				if (this._animator != null && this._embLayerIndex >= 0)
				{
					this._animator.SetLayerWeight(this._embLayerIndex, 0f);
				}
				this.ReleaseBody(instance);
			}
			if (this._faceHeld)
			{
				this._faceHeld = false;
				instance.RestoreFacialExpression();
			}
		}

		// Token: 0x06005B26 RID: 23334 RVA: 0x0023B0B4 File Offset: 0x002392B4
		private void OnDestroy()
		{
			if (SpicyHatchEmbarrassment._instance == this)
			{
				SpicyHatchEmbarrassment._instance = null;
			}
		}

		// Token: 0x04005C4A RID: 23626
		private const int EmbSupremeExpression = 4;

		// Token: 0x04005C4B RID: 23627
		private const float BlendInSeconds = 0.6f;

		// Token: 0x04005C4C RID: 23628
		private const float BlendOutSeconds = 1f;

		// Token: 0x04005C4D RID: 23629
		private static SpicyHatchEmbarrassment _instance;

		// Token: 0x04005C4E RID: 23630
		private float _until;

		// Token: 0x04005C4F RID: 23631
		private bool _faceHeld;

		// Token: 0x04005C50 RID: 23632
		private bool _bodyHeld;

		// Token: 0x04005C51 RID: 23633
		private Animator _animator;

		// Token: 0x04005C52 RID: 23634
		private int _embLayerIndex = -1;

		// Token: 0x04005C53 RID: 23635
		private GameplayEmoteController _emotes;

		// Token: 0x04005C54 RID: 23636
		private bool _emotesSearched;

		// Token: 0x04005C55 RID: 23637
		private CielRagdollRecovery _ragdollRecovery;

		// Token: 0x04005C56 RID: 23638
		private bool _ragdollSearched;
	}
}
