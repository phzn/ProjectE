using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002EE RID: 750
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Cinemachine/MMCinemachineZoom")]
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class MMCinemachineZoom : MonoBehaviour
	{
		// Token: 0x06001513 RID: 5395 RVA: 0x00059DD8 File Offset: 0x00057FD8
		public virtual float GetTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x00059DED File Offset: 0x00057FED
		public virtual float GetDeltaTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06001515 RID: 5397 RVA: 0x00059E02 File Offset: 0x00058002
		// (set) Token: 0x06001516 RID: 5398 RVA: 0x00059E0A File Offset: 0x0005800A
		public virtual TimescaleModes TimescaleMode { get; set; }

		// Token: 0x06001517 RID: 5399 RVA: 0x00059E13 File Offset: 0x00058013
		protected virtual void Awake()
		{
			this._virtualCamera = base.gameObject.GetComponent<CinemachineVirtualCamera>();
			this._initialFieldOfView = this._virtualCamera.m_Lens.FieldOfView;
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00059E3C File Offset: 0x0005803C
		protected virtual void Update()
		{
			if (!this._zooming)
			{
				return;
			}
			this._elapsedTime = this.GetTime() - this._zoomStartedAt;
			if (this._elapsedTime <= this._transitionDuration)
			{
				float num = MMMaths.Remap(this._elapsedTime, 0f, this._transitionDuration, 0f, 1f);
				this._virtualCamera.m_Lens.FieldOfView = Mathf.LerpUnclamped(this._startFieldOfView, this._targetFieldOfView, this.ZoomTween.Evaluate(num));
				return;
			}
			if (!this._destinationReached)
			{
				this._reachedDestinationTimestamp = this.GetTime();
				this._destinationReached = true;
			}
			if (this._mode == MMCameraZoomModes.For && this._direction == 1)
			{
				if (this.GetTime() - this._reachedDestinationTimestamp > this._duration)
				{
					this._direction = -1;
					this._zoomStartedAt = this.GetTime();
					this._startFieldOfView = this._targetFieldOfView;
					this._targetFieldOfView = this._initialFieldOfView;
					return;
				}
			}
			else
			{
				this._zooming = false;
			}
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00059F38 File Offset: 0x00058138
		public virtual void Zoom(MMCameraZoomModes mode, float newFieldOfView, float transitionDuration, float duration, bool useUnscaledTime, bool relative = false, MMTweenType tweenType = null)
		{
			if (this._zooming)
			{
				return;
			}
			this._zooming = true;
			this._elapsedTime = 0f;
			this._mode = mode;
			this.TimescaleMode = (useUnscaledTime ? TimescaleModes.Unscaled : TimescaleModes.Scaled);
			this._startFieldOfView = this._virtualCamera.m_Lens.FieldOfView;
			this._transitionDuration = transitionDuration;
			this._duration = duration;
			this._transitionDuration = transitionDuration;
			this._direction = 1;
			this._destinationReached = false;
			this._zoomStartedAt = this.GetTime();
			if (tweenType != null)
			{
				this.ZoomTween = tweenType;
			}
			switch (mode)
			{
			case MMCameraZoomModes.For:
				this._targetFieldOfView = newFieldOfView;
				break;
			case MMCameraZoomModes.Set:
				this._targetFieldOfView = newFieldOfView;
				break;
			case MMCameraZoomModes.Reset:
				this._targetFieldOfView = this._initialFieldOfView;
				break;
			}
			if (relative)
			{
				this._targetFieldOfView += this._initialFieldOfView;
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0005A010 File Offset: 0x00058210
		protected virtual void TestZoom()
		{
			this.Zoom(this.TestMode, this.TestFieldOfView, this.TestTransitionDuration, this.TestDuration, false, false, null);
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x0005A034 File Offset: 0x00058234
		public virtual void OnCameraZoomEvent(MMCameraZoomModes mode, float newFieldOfView, float transitionDuration, float duration, MMChannelData channelData, bool useUnscaledTime, bool stop = false, bool relative = false, bool restore = false, MMTweenType tweenType = null)
		{
			if (!MMChannel.Match(channelData, this.ChannelMode, this.Channel, this.MMChannelDefinition))
			{
				return;
			}
			if (stop)
			{
				this._zooming = false;
				return;
			}
			if (restore)
			{
				this._virtualCamera.m_Lens.FieldOfView = this._initialFieldOfView;
				return;
			}
			this.Zoom(mode, newFieldOfView, transitionDuration, duration, useUnscaledTime, relative, tweenType);
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0005A095 File Offset: 0x00058295
		protected virtual void OnEnable()
		{
			MMCameraZoomEvent.Register(new MMCameraZoomEvent.Delegate(this.OnCameraZoomEvent));
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0005A0A9 File Offset: 0x000582A9
		protected virtual void OnDisable()
		{
			MMCameraZoomEvent.Unregister(new MMCameraZoomEvent.Delegate(this.OnCameraZoomEvent));
		}

		// Token: 0x04001814 RID: 6164
		[Header("Channel")]
		[MMFInspectorGroup("Shaker Settings", true, 3, false, false)]
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x04001815 RID: 6165
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMFEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x04001816 RID: 6166
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMFEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x04001817 RID: 6167
		[Header("Transition Speed")]
		[Tooltip("the animation curve to apply to the zoom transition")]
		public MMTweenType ZoomTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x04001818 RID: 6168
		[Header("Test Zoom")]
		[Tooltip("the mode to apply the zoom in when using the test button in the inspector")]
		public MMCameraZoomModes TestMode;

		// Token: 0x04001819 RID: 6169
		[Tooltip("the target field of view to apply the zoom in when using the test button in the inspector")]
		public float TestFieldOfView = 30f;

		// Token: 0x0400181A RID: 6170
		[Tooltip("the transition duration to apply the zoom in when using the test button in the inspector")]
		public float TestTransitionDuration = 0.1f;

		// Token: 0x0400181B RID: 6171
		[Tooltip("the duration to apply the zoom in when using the test button in the inspector")]
		public float TestDuration = 0.05f;

		// Token: 0x0400181C RID: 6172
		[MMFInspectorButton("TestZoom")]
		public bool TestZoomButton;

		// Token: 0x0400181E RID: 6174
		protected CinemachineVirtualCamera _virtualCamera;

		// Token: 0x0400181F RID: 6175
		protected float _initialFieldOfView;

		// Token: 0x04001820 RID: 6176
		protected MMCameraZoomModes _mode;

		// Token: 0x04001821 RID: 6177
		protected bool _zooming;

		// Token: 0x04001822 RID: 6178
		protected float _startFieldOfView;

		// Token: 0x04001823 RID: 6179
		protected float _transitionDuration;

		// Token: 0x04001824 RID: 6180
		protected float _duration;

		// Token: 0x04001825 RID: 6181
		protected float _targetFieldOfView;

		// Token: 0x04001826 RID: 6182
		protected float _elapsedTime;

		// Token: 0x04001827 RID: 6183
		protected int _direction = 1;

		// Token: 0x04001828 RID: 6184
		protected float _reachedDestinationTimestamp;

		// Token: 0x04001829 RID: 6185
		protected bool _destinationReached;

		// Token: 0x0400182A RID: 6186
		protected float _zoomStartedAt;
	}
}
