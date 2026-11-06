using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000185 RID: 389
	[AddComponentMenu("More Mountains/Tools/Animation/MMStopMotionAnimation")]
	public class MMStopMotionAnimation : MonoBehaviour
	{
		// Token: 0x06000910 RID: 2320 RVA: 0x0002A5C5 File Offset: 0x000287C5
		protected virtual void Awake()
		{
			this._animator = base.gameObject.GetComponent<Animator>();
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0002A5D8 File Offset: 0x000287D8
		protected virtual void Update()
		{
			this.StopMotion();
			if (Time.time - this._lastPollAt > this.PollFrequency)
			{
				this.Poll();
			}
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0002A5FC File Offset: 0x000287FC
		protected virtual void StopMotion()
		{
			if (!this.StopMotionEnabled)
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			MMStopMotionAnimation.FramerateModes framerateMode = this.FramerateMode;
			if (framerateMode != MMStopMotionAnimation.FramerateModes.Manual)
			{
				if (framerateMode == MMStopMotionAnimation.FramerateModes.Automatic)
				{
					num = 1f / this.FramesPerSecond;
					num2 = 1f / (this.FramesPerSecond - 1f) * 2f * this._currentClipFPS;
				}
			}
			else
			{
				num = this.ManualTimeBetweenFrames;
				num2 = this.ManualAnimatorSpeed;
			}
			this.timet += Time.deltaTime;
			if (this.timet > num)
			{
				this.timet -= num;
				this._animator.speed = num2;
				return;
			}
			this._animator.speed = 0f;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0002A6B4 File Offset: 0x000288B4
		protected virtual void Poll()
		{
			this._currentClip = this._animator.GetCurrentAnimatorClipInfo(this.AnimationLayerID)[0].clip;
			this._currentClipLength = this._currentClip.length;
			this._currentClipFPS = this._currentClip.frameRate;
			this._lastPollAt = Time.time;
		}

		// Token: 0x04000AB5 RID: 2741
		[Header("General Settings")]
		public bool StopMotionEnabled = true;

		// Token: 0x04000AB6 RID: 2742
		public int AnimationLayerID;

		// Token: 0x04000AB7 RID: 2743
		[Header("Framerate")]
		public MMStopMotionAnimation.FramerateModes FramerateMode = MMStopMotionAnimation.FramerateModes.Automatic;

		// Token: 0x04000AB8 RID: 2744
		[MMEnumCondition("FramerateMode", new int[] { 1 })]
		public float FramesPerSecond = 4f;

		// Token: 0x04000AB9 RID: 2745
		[MMEnumCondition("FramerateMode", new int[] { 1 })]
		public float PollFrequency = 1f;

		// Token: 0x04000ABA RID: 2746
		[MMEnumCondition("FramerateMode", new int[] { 0 })]
		public float ManualTimeBetweenFrames = 0.125f;

		// Token: 0x04000ABB RID: 2747
		[MMEnumCondition("FramerateMode", new int[] { 0 })]
		public float ManualAnimatorSpeed = 2f;

		// Token: 0x04000ABC RID: 2748
		public float timet;

		// Token: 0x04000ABD RID: 2749
		protected float _currentClipFPS;

		// Token: 0x04000ABE RID: 2750
		protected float _currentClipLength;

		// Token: 0x04000ABF RID: 2751
		protected float _lastPollAt = -10f;

		// Token: 0x04000AC0 RID: 2752
		protected Animator _animator;

		// Token: 0x04000AC1 RID: 2753
		protected AnimationClip _currentClip;

		// Token: 0x02000474 RID: 1140
		public enum FramerateModes
		{
			// Token: 0x040020D1 RID: 8401
			Manual,
			// Token: 0x040020D2 RID: 8402
			Automatic
		}
	}
}
