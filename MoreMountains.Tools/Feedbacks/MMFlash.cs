using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200024A RID: 586
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(CanvasGroup))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Various/MMFlash")]
	public class MMFlash : MMMonoBehaviour
	{
		// Token: 0x06001019 RID: 4121 RVA: 0x000475C3 File Offset: 0x000457C3
		public virtual float GetTime()
		{
			if (this._timescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x000475D8 File Offset: 0x000457D8
		public virtual float GetDeltaTime()
		{
			if (this._timescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x000475ED File Offset: 0x000457ED
		protected virtual void Start()
		{
			this._image = base.GetComponent<Image>();
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			this._initialColor = this._image.color;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00047618 File Offset: 0x00045818
		protected virtual void Update()
		{
			if (this._flashing)
			{
				this._image.enabled = true;
				this._currentTween = this.FlashInTween;
				if (this.GetTime() - this._flashStartedTimestamp > this._duration / 2f)
				{
					this._direction = -1;
					this._currentTween = this.FlashOutTween;
				}
				if (this._direction == 1)
				{
					this._delta += this.GetDeltaTime() / (this._duration / 2f);
				}
				else
				{
					this._delta -= this.GetDeltaTime() / (this._duration / 2f);
				}
				if (this.GetTime() - this._flashStartedTimestamp > this._duration)
				{
					this._flashing = false;
				}
				float num = MMMaths.Remap(this._delta, 0f, this._duration / 2f, 0f, 1f);
				float num2 = this._currentTween.Evaluate(num);
				this._canvasGroup.alpha = Mathf.Lerp(0f, this._targetAlpha, num2);
				return;
			}
			this._image.enabled = false;
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0004773C File Offset: 0x0004593C
		public virtual void DebugTest()
		{
			MMFlashEvent.Trigger(this.DebugSettings.FlashColor, this.DebugSettings.FlashDuration, this.DebugSettings.FlashAlpha, this.DebugSettings.FlashID, new MMChannelData(this.DebugSettings.ChannelMode, this.DebugSettings.Channel, this.DebugSettings.MMChannelDefinition), TimescaleModes.Unscaled, false);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x000477A2 File Offset: 0x000459A2
		public virtual void OnMMFlashEvent(Color flashColor, float duration, float alpha, int flashID, MMChannelData channelData, TimescaleModes timescaleMode, bool stop = false)
		{
			if (flashID != this.FlashID)
			{
				return;
			}
			if (stop)
			{
				this._flashing = false;
				return;
			}
			if (!MMChannel.Match(channelData, this.ChannelMode, this.Channel, this.MMChannelDefinition))
			{
				return;
			}
			this.Flash(flashColor, duration, alpha, timescaleMode);
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x000477E4 File Offset: 0x000459E4
		public virtual void Flash(Color flashColor, float duration, float alpha, TimescaleModes timescaleMode)
		{
			if (this._flashing && this.Interruptable)
			{
				this._flashing = false;
			}
			if (!this._flashing)
			{
				this._flashing = true;
				this._direction = 1;
				this._canvasGroup.alpha = 0f;
				this._targetAlpha = alpha;
				this._delta = 0f;
				this._image.color = flashColor;
				this._duration = duration;
				this._timescaleMode = timescaleMode;
				this._flashStartedTimestamp = this.GetTime();
			}
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00047867 File Offset: 0x00045A67
		protected virtual void OnEnable()
		{
			MMFlashEvent.Register(new MMFlashEvent.Delegate(this.OnMMFlashEvent));
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0004787B File Offset: 0x00045A7B
		protected virtual void OnDisable()
		{
			MMFlashEvent.Unregister(new MMFlashEvent.Delegate(this.OnMMFlashEvent));
		}

		// Token: 0x04001288 RID: 4744
		[MMInspectorGroup("Flash", true, 121, false)]
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x04001289 RID: 4745
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMFEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x0400128A RID: 4746
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMFEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x0400128B RID: 4747
		[Tooltip("the ID of this MMFlash object. When triggering a MMFlashEvent you can specify an ID, and only MMFlash objects with this ID will answer the call and flash, allowing you to have more than one flash object in a scene")]
		public int FlashID;

		// Token: 0x0400128C RID: 4748
		[Tooltip("if this is true, the MMFlash will stop before playing on every new event received")]
		public bool Interruptable;

		// Token: 0x0400128D RID: 4749
		[MMInspectorGroup("Interpolation", true, 122, false)]
		[Tooltip("the animation curve to use when flashing in")]
		public MMTweenType FlashInTween = new MMTweenType(MMTween.MMTweenCurve.LinearTween);

		// Token: 0x0400128E RID: 4750
		[Tooltip("the animation curve to use when flashing out")]
		public MMTweenType FlashOutTween = new MMTweenType(MMTween.MMTweenCurve.LinearTween);

		// Token: 0x0400128F RID: 4751
		[MMInspectorGroup("Debug", true, 123, false)]
		[Tooltip("the set of test settings to use when pressing the DebugTest button")]
		public MMFlashDebugSettings DebugSettings;

		// Token: 0x04001290 RID: 4752
		[Tooltip("a test button that calls the DebugTest method")]
		[MMFInspectorButton("DebugTest")]
		public bool DebugTestButton;

		// Token: 0x04001291 RID: 4753
		protected Image _image;

		// Token: 0x04001292 RID: 4754
		protected CanvasGroup _canvasGroup;

		// Token: 0x04001293 RID: 4755
		protected bool _flashing;

		// Token: 0x04001294 RID: 4756
		protected float _targetAlpha;

		// Token: 0x04001295 RID: 4757
		protected Color _initialColor;

		// Token: 0x04001296 RID: 4758
		protected float _delta;

		// Token: 0x04001297 RID: 4759
		protected float _flashStartedTimestamp;

		// Token: 0x04001298 RID: 4760
		protected int _direction = 1;

		// Token: 0x04001299 RID: 4761
		protected float _duration;

		// Token: 0x0400129A RID: 4762
		protected TimescaleModes _timescaleMode;

		// Token: 0x0400129B RID: 4763
		protected MMTweenType _currentTween;
	}
}
