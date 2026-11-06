using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001C8 RID: 456
	public class MMShaker : MMMonoBehaviour
	{
		// Token: 0x06000B61 RID: 2913 RVA: 0x000326D9 File Offset: 0x000308D9
		public virtual float GetTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x000326EE File Offset: 0x000308EE
		public virtual float GetDeltaTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x00032703 File Offset: 0x00030903
		public virtual MMChannelData ChannelData
		{
			get
			{
				return new MMChannelData(this.ChannelMode, this.Channel, this.MMChannelDefinition);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0003271C File Offset: 0x0003091C
		public virtual bool ListeningToEvents
		{
			get
			{
				return this._listeningToEvents;
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00032724 File Offset: 0x00030924
		protected virtual void Awake()
		{
			this.Initialization();
			if (!this._listeningToEvents)
			{
				this.StartListening();
			}
			this.Shaking = this.PlayOnAwake;
			base.enabled = this.PlayOnAwake;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00032752 File Offset: 0x00030952
		protected virtual void Initialization()
		{
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00032754 File Offset: 0x00030954
		public virtual void ForceInitialization()
		{
			this.Initialization();
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0003275C File Offset: 0x0003095C
		public virtual void StartShaking()
		{
			this._journey = (this.ForwardDirection ? 0f : this.ShakeDuration);
			if (this.GetTime() - this._shakeStartedTimestamp < this.CooldownBetweenShakes)
			{
				return;
			}
			if (this.Shaking)
			{
				return;
			}
			base.enabled = true;
			this._shakeStartedTimestamp = this.GetTime();
			this.Shaking = true;
			this.GrabInitialValues();
			this.ShakeStarts();
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x000327C9 File Offset: 0x000309C9
		protected virtual void ShakeStarts()
		{
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x000327CB File Offset: 0x000309CB
		protected virtual void GrabInitialValues()
		{
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x000327D0 File Offset: 0x000309D0
		protected virtual void Update()
		{
			if (this.Shaking || this.PermanentShake)
			{
				this.Shake();
				this._journey += (this.ForwardDirection ? this.GetDeltaTime() : (-this.GetDeltaTime()));
			}
			if (this.Shaking && !this.PermanentShake && (this._journey < 0f || this._journey > this.ShakeDuration))
			{
				this.Shaking = false;
				this.ShakeComplete();
			}
			if (this.PermanentShake)
			{
				if (this._journey < 0f)
				{
					this._journey = this.ShakeDuration;
				}
				if (this._journey > this.ShakeDuration)
				{
					this._journey = 0f;
				}
			}
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00032889 File Offset: 0x00030A89
		protected virtual void Shake()
		{
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0003288C File Offset: 0x00030A8C
		protected virtual float ShakeFloat(AnimationCurve curve, float remapMin, float remapMax, bool relativeIntensity, float initialValue)
		{
			float num = MMFeedbacksHelpers.Remap(this._journey, 0f, this.ShakeDuration, 0f, 1f);
			float num2 = MMFeedbacksHelpers.Remap(curve.Evaluate(num), 0f, 1f, remapMin, remapMax);
			if (relativeIntensity)
			{
				num2 += initialValue;
			}
			return num2;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x000328E4 File Offset: 0x00030AE4
		protected virtual Color ShakeGradient(Gradient gradient)
		{
			float num = MMFeedbacksHelpers.Remap(this._journey, 0f, this.ShakeDuration, 0f, 1f);
			return gradient.Evaluate(num);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00032919 File Offset: 0x00030B19
		protected virtual void ResetTargetValues()
		{
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0003291B File Offset: 0x00030B1B
		protected virtual void ResetShakerValues()
		{
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00032920 File Offset: 0x00030B20
		protected virtual void ShakeComplete()
		{
			this._journey = (this.ForwardDirection ? this.ShakeDuration : 0f);
			this.Shake();
			if (this._resetTargetValuesAfterShake || this.AlwaysResetTargetValuesAfterShake)
			{
				this.ResetTargetValues();
			}
			if (this._resetShakerValuesAfterShake)
			{
				this.ResetShakerValues();
			}
			base.enabled = false;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00032979 File Offset: 0x00030B79
		protected virtual void OnEnable()
		{
			this.StartShaking();
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00032981 File Offset: 0x00030B81
		protected virtual void OnDestroy()
		{
			this.StopListening();
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00032989 File Offset: 0x00030B89
		protected virtual void OnDisable()
		{
			if (this.Shaking)
			{
				this.ShakeComplete();
			}
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00032999 File Offset: 0x00030B99
		public virtual void Play()
		{
			if (this.GetTime() - this._shakeStartedTimestamp < this.CooldownBetweenShakes)
			{
				return;
			}
			base.enabled = true;
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x000329B8 File Offset: 0x00030BB8
		public virtual void Stop()
		{
			this.Shaking = false;
			this.ShakeComplete();
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x000329C7 File Offset: 0x00030BC7
		public virtual void StartListening()
		{
			this._listeningToEvents = true;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000329D0 File Offset: 0x00030BD0
		public virtual void StopListening()
		{
			this._listeningToEvents = false;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x000329DC File Offset: 0x00030BDC
		protected virtual bool CheckEventAllowed(MMChannelData channelData, bool useRange = false, float range = 0f, Vector3 eventOriginPosition = default(Vector3))
		{
			return MMChannel.Match(channelData, this.ChannelMode, this.Channel, this.MMChannelDefinition) && base.gameObject.activeInHierarchy && (!useRange || Vector3.Distance(base.transform.position, eventOriginPosition) <= range);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00032A30 File Offset: 0x00030C30
		public virtual float ComputeRangeIntensity(bool useRange, float rangeDistance, bool useRangeFalloff, AnimationCurve rangeFalloff, Vector2 remapRangeFalloff, Vector3 rangePosition)
		{
			if (!useRange)
			{
				return 1f;
			}
			float num = Vector3.Distance(rangePosition, base.transform.position);
			if (num > rangeDistance)
			{
				return 0f;
			}
			if (!useRangeFalloff)
			{
				return 1f;
			}
			float num2 = MMMaths.Remap(num, 0f, rangeDistance, 0f, 1f);
			return MMMaths.Remap(rangeFalloff.Evaluate(num2), 0f, 1f, remapRangeFalloff.x, remapRangeFalloff.y);
		}

		// Token: 0x04000D12 RID: 3346
		[MMInspectorGroup("Shaker Settings", true, 3, false)]
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x04000D13 RID: 3347
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x04000D14 RID: 3348
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x04000D15 RID: 3349
		[Tooltip("the duration of the shake, in seconds")]
		public float ShakeDuration = 0.2f;

		// Token: 0x04000D16 RID: 3350
		[Tooltip("if this is true this shaker will play on awake")]
		public bool PlayOnAwake;

		// Token: 0x04000D17 RID: 3351
		[Tooltip("if this is true, the shaker will shake permanently as long as its game object is active")]
		public bool PermanentShake;

		// Token: 0x04000D18 RID: 3352
		[Tooltip("if this is true, a new shake can happen while shaking")]
		public bool Interruptible = true;

		// Token: 0x04000D19 RID: 3353
		[Tooltip("if this is true, this shaker will always reset target values, regardless of how it was called")]
		public bool AlwaysResetTargetValuesAfterShake;

		// Token: 0x04000D1A RID: 3354
		[Tooltip("if this is true, this shaker will ignore any value passed in an event that triggered it, and will instead use the values set on its inspector")]
		public bool OnlyUseShakerValues;

		// Token: 0x04000D1B RID: 3355
		[Tooltip("a cooldown, in seconds, after a shake, during which no other shake can start")]
		public float CooldownBetweenShakes;

		// Token: 0x04000D1C RID: 3356
		[Tooltip("whether or not this shaker is shaking right now")]
		[MMFReadOnly]
		public bool Shaking;

		// Token: 0x04000D1D RID: 3357
		[HideInInspector]
		public bool ForwardDirection = true;

		// Token: 0x04000D1E RID: 3358
		[HideInInspector]
		public TimescaleModes TimescaleMode;

		// Token: 0x04000D1F RID: 3359
		[HideInInspector]
		internal bool _listeningToEvents;

		// Token: 0x04000D20 RID: 3360
		protected float _shakeStartedTimestamp = float.MinValue;

		// Token: 0x04000D21 RID: 3361
		protected float _remappedTimeSinceStart;

		// Token: 0x04000D22 RID: 3362
		protected bool _resetShakerValuesAfterShake;

		// Token: 0x04000D23 RID: 3363
		protected bool _resetTargetValuesAfterShake;

		// Token: 0x04000D24 RID: 3364
		protected float _journey;
	}
}
