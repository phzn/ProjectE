using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000244 RID: 580
	[RequireComponent(typeof(MMWiggle))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Camera/MMCameraShaker")]
	public class MMCameraShaker : MonoBehaviour
	{
		// Token: 0x06000FFD RID: 4093 RVA: 0x00047065 File Offset: 0x00045265
		protected virtual void Awake()
		{
			this._wiggle = base.GetComponent<MMWiggle>();
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00047074 File Offset: 0x00045274
		public virtual void ShakeCamera(float duration, float amplitude, float frequency, float amplitudeX, float amplitudeY, float amplitudeZ, bool useUnscaledTime)
		{
			if (Time.unscaledTime - this._shakeStartedTimestamp < this.CooldownBetweenShakes)
			{
				return;
			}
			if (amplitudeX != 0f || amplitudeY != 0f || amplitudeZ != 0f)
			{
				this._wiggle.PositionWiggleProperties.AmplitudeMin.x = -amplitudeX;
				this._wiggle.PositionWiggleProperties.AmplitudeMin.y = -amplitudeY;
				this._wiggle.PositionWiggleProperties.AmplitudeMin.z = -amplitudeZ;
				this._wiggle.PositionWiggleProperties.AmplitudeMax.x = amplitudeX;
				this._wiggle.PositionWiggleProperties.AmplitudeMax.y = amplitudeY;
				this._wiggle.PositionWiggleProperties.AmplitudeMax.z = amplitudeZ;
			}
			else
			{
				this._wiggle.PositionWiggleProperties.AmplitudeMin = Vector3.one * -amplitude;
				this._wiggle.PositionWiggleProperties.AmplitudeMax = Vector3.one * amplitude;
			}
			this._shakeStartedTimestamp = Time.unscaledTime;
			this._wiggle.PositionWiggleProperties.UseUnscaledTime = useUnscaledTime;
			this._wiggle.PositionWiggleProperties.FrequencyMin = frequency;
			this._wiggle.PositionWiggleProperties.FrequencyMax = frequency;
			this._wiggle.PositionWiggleProperties.NoiseFrequencyMin = frequency * Vector3.one;
			this._wiggle.PositionWiggleProperties.NoiseFrequencyMax = frequency * Vector3.one;
			this._wiggle.WigglePosition(duration);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x000471FB File Offset: 0x000453FB
		public virtual void OnCameraShakeEvent(float duration, float amplitude, float frequency, float amplitudeX, float amplitudeY, float amplitudeZ, bool infinite, MMChannelData channelData, bool useUnscaledTime)
		{
			if (!MMChannel.Match(channelData, this.ChannelMode, this.Channel, this.MMChannelDefinition))
			{
				return;
			}
			this.ShakeCamera(duration, amplitude, frequency, amplitudeX, amplitudeY, amplitudeZ, useUnscaledTime);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0004722A File Offset: 0x0004542A
		protected virtual void OnEnable()
		{
			MMCameraShakeEvent.Register(new MMCameraShakeEvent.Delegate(this.OnCameraShakeEvent));
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0004723E File Offset: 0x0004543E
		protected virtual void OnDisable()
		{
			MMCameraShakeEvent.Unregister(new MMCameraShakeEvent.Delegate(this.OnCameraShakeEvent));
		}

		// Token: 0x04001278 RID: 4728
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x04001279 RID: 4729
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x0400127A RID: 4730
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x0400127B RID: 4731
		[Tooltip("a cooldown, in seconds, after a shake, during which no other shake can start")]
		public float CooldownBetweenShakes;

		// Token: 0x0400127C RID: 4732
		protected MMWiggle _wiggle;

		// Token: 0x0400127D RID: 4733
		protected float _shakeStartedTimestamp = float.MinValue;
	}
}
