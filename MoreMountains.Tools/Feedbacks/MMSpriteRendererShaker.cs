using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000254 RID: 596
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Renderer/MMSpriteRendererShaker")]
	[RequireComponent(typeof(SpriteRenderer))]
	public class MMSpriteRendererShaker : MMShaker
	{
		// Token: 0x06001083 RID: 4227 RVA: 0x00049CD0 File Offset: 0x00047ED0
		protected override void Initialization()
		{
			base.Initialization();
			if (this.BoundSpriteRenderer == null)
			{
				this.BoundSpriteRenderer = base.gameObject.GetComponent<SpriteRenderer>();
			}
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00049CF7 File Offset: 0x00047EF7
		protected virtual void Reset()
		{
			this.ShakeDuration = 1f;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00049D04 File Offset: 0x00047F04
		protected override void Shake()
		{
			if (this.ModifyColor)
			{
				this._remappedTimeSinceStart = MMFeedbacksHelpers.Remap(Time.time - this._shakeStartedTimestamp, 0f, this.ShakeDuration, 0f, 1f);
				this.BoundSpriteRenderer.color = this.ColorOverTime.Evaluate(this._remappedTimeSinceStart);
			}
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00049D61 File Offset: 0x00047F61
		protected override void GrabInitialValues()
		{
			this._initialColor = this.BoundSpriteRenderer.color;
			this._originalFlipX = this.BoundSpriteRenderer.flipX;
			this._originalFlipY = this.BoundSpriteRenderer.flipY;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00049D96 File Offset: 0x00047F96
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this.BoundSpriteRenderer.color = this._initialColor;
			this.BoundSpriteRenderer.flipX = this._originalFlipX;
			this.BoundSpriteRenderer.flipY = this._originalFlipY;
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00049DD1 File Offset: 0x00047FD1
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ModifyColor = this._originalModifyColor;
			this.ShakeDuration = this._originalShakeDuration;
			this.ColorOverTime = this._originalColorOverTime;
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x00049DFD File Offset: 0x00047FFD
		public override void StartListening()
		{
			base.StartListening();
			MMSpriteRendererShakeEvent.Register(new MMSpriteRendererShakeEvent.Delegate(this.OnMMSpriteRendererShakeEvent));
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00049E17 File Offset: 0x00048017
		public override void StopListening()
		{
			base.StopListening();
			MMSpriteRendererShakeEvent.Unregister(new MMSpriteRendererShakeEvent.Delegate(this.OnMMSpriteRendererShakeEvent));
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00049E34 File Offset: 0x00048034
		public virtual void OnMMSpriteRendererShakeEvent(float shakeDuration, bool modifyColor, Gradient colorOverTime, bool flipX, bool flipY, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3))
		{
			if (!this.CheckEventAllowed(channelData, useRange, eventRange, eventOriginPosition) || (!this.Interruptible && this.Shaking))
			{
				return;
			}
			this._resetShakerValuesAfterShake = resetShakerValuesAfterShake;
			this._resetTargetValuesAfterShake = resetTargetValuesAfterShake;
			if (resetShakerValuesAfterShake)
			{
				this._originalModifyColor = this.ModifyColor;
				this._originalShakeDuration = this.ShakeDuration;
				this._originalColorOverTime = this.ColorOverTime;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.ModifyColor = modifyColor;
				this.ShakeDuration = shakeDuration;
				this.ColorOverTime = colorOverTime;
				this.FlipX = flipX;
				this.FlipY = flipY;
			}
			if (this.FlipX)
			{
				this.BoundSpriteRenderer.flipX = !this.BoundSpriteRenderer.flipX;
			}
			if (this.FlipY)
			{
				this.BoundSpriteRenderer.flipY = !this.BoundSpriteRenderer.flipY;
			}
			this.Play();
		}

		// Token: 0x04001359 RID: 4953
		[MMInspectorGroup("SpriteRenderer", true, 39, false)]
		[Tooltip("the SpriteRenderer to affect when playing the feedback")]
		public SpriteRenderer BoundSpriteRenderer;

		// Token: 0x0400135A RID: 4954
		[Tooltip("whether or not that SpriteRenderer should be turned off on start")]
		public bool StartsOff = true;

		// Token: 0x0400135B RID: 4955
		[MMInspectorGroup("Color", true, 40, false)]
		[Tooltip("whether or not this shaker should modify color")]
		public bool ModifyColor = true;

		// Token: 0x0400135C RID: 4956
		[Tooltip("the colors to apply to the SpriteRenderer over time")]
		public Gradient ColorOverTime;

		// Token: 0x0400135D RID: 4957
		[MMInspectorGroup("Flip", true, 41, false)]
		[Tooltip("whether or not to flip the sprite on X")]
		public bool FlipX;

		// Token: 0x0400135E RID: 4958
		[Tooltip("whether or not to flip the sprite on Y")]
		public bool FlipY;

		// Token: 0x0400135F RID: 4959
		protected Color _initialColor;

		// Token: 0x04001360 RID: 4960
		protected bool _originalModifyColor;

		// Token: 0x04001361 RID: 4961
		protected float _originalShakeDuration;

		// Token: 0x04001362 RID: 4962
		protected Gradient _originalColorOverTime;

		// Token: 0x04001363 RID: 4963
		protected bool _originalFlipX;

		// Token: 0x04001364 RID: 4964
		protected bool _originalFlipY;
	}
}
