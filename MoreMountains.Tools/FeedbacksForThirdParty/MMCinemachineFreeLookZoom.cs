using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002E9 RID: 745
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Cinemachine/MMCinemachineFreeLookZoom")]
	[RequireComponent(typeof(CinemachineFreeLook))]
	public class MMCinemachineFreeLookZoom : MonoBehaviour
	{
		// Token: 0x060014E7 RID: 5351 RVA: 0x00059554 File Offset: 0x00057754
		public virtual float GetTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x00059569 File Offset: 0x00057769
		public virtual float GetDeltaTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060014E9 RID: 5353 RVA: 0x0005957E File Offset: 0x0005777E
		// (set) Token: 0x060014EA RID: 5354 RVA: 0x00059586 File Offset: 0x00057786
		public virtual TimescaleModes TimescaleMode { get; set; }

		// Token: 0x060014EB RID: 5355 RVA: 0x0005958F File Offset: 0x0005778F
		protected virtual void Awake()
		{
			this._freeLookCamera = base.gameObject.GetComponent<CinemachineFreeLook>();
			this._initialFieldOfView = this._freeLookCamera.m_Lens.FieldOfView;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x000595B8 File Offset: 0x000577B8
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
				this._freeLookCamera.m_Lens.FieldOfView = Mathf.LerpUnclamped(this._startFieldOfView, this._targetFieldOfView, this.ZoomTween.Evaluate(num));
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

		// Token: 0x060014ED RID: 5357 RVA: 0x000596B4 File Offset: 0x000578B4
		public virtual void Zoom(MMCameraZoomModes mode, float newFieldOfView, float transitionDuration, float duration, bool relative = false, MMTweenType tweenType = null)
		{
			if (this._zooming)
			{
				return;
			}
			this._zooming = true;
			this._elapsedTime = 0f;
			this._mode = mode;
			this._startFieldOfView = this._freeLookCamera.m_Lens.FieldOfView;
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

		// Token: 0x060014EE RID: 5358 RVA: 0x0005977E File Offset: 0x0005797E
		protected virtual void TestZoom()
		{
			this.Zoom(this.TestMode, this.TestFieldOfView, this.TestTransitionDuration, this.TestDuration, false, null);
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x000597A0 File Offset: 0x000579A0
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
				this._freeLookCamera.m_Lens.FieldOfView = this._initialFieldOfView;
				return;
			}
			this.Zoom(mode, newFieldOfView, transitionDuration, duration, relative, tweenType);
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x000597FF File Offset: 0x000579FF
		protected virtual void OnEnable()
		{
			MMCameraZoomEvent.Register(new MMCameraZoomEvent.Delegate(this.OnCameraZoomEvent));
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x00059813 File Offset: 0x00057A13
		protected virtual void OnDisable()
		{
			MMCameraZoomEvent.Unregister(new MMCameraZoomEvent.Delegate(this.OnCameraZoomEvent));
		}

		// Token: 0x040017E6 RID: 6118
		[Header("Channel")]
		[MMFInspectorGroup("Shaker Settings", true, 3, false, false)]
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x040017E7 RID: 6119
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMFEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x040017E8 RID: 6120
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMFEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x040017E9 RID: 6121
		[Header("Transition Speed")]
		[Tooltip("the animation curve to apply to the zoom transition")]
		public MMTweenType ZoomTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x040017EA RID: 6122
		[Header("Test Zoom")]
		[Tooltip("the mode to apply the zoom in when using the test button in the inspector")]
		public MMCameraZoomModes TestMode;

		// Token: 0x040017EB RID: 6123
		[Tooltip("the target field of view to apply the zoom in when using the test button in the inspector")]
		public float TestFieldOfView = 30f;

		// Token: 0x040017EC RID: 6124
		[Tooltip("the transition duration to apply the zoom in when using the test button in the inspector")]
		public float TestTransitionDuration = 0.1f;

		// Token: 0x040017ED RID: 6125
		[Tooltip("the duration to apply the zoom in when using the test button in the inspector")]
		public float TestDuration = 0.05f;

		// Token: 0x040017EE RID: 6126
		[MMFInspectorButton("TestZoom")]
		public bool TestZoomButton;

		// Token: 0x040017F0 RID: 6128
		protected CinemachineFreeLook _freeLookCamera;

		// Token: 0x040017F1 RID: 6129
		protected float _initialFieldOfView;

		// Token: 0x040017F2 RID: 6130
		protected MMCameraZoomModes _mode;

		// Token: 0x040017F3 RID: 6131
		protected bool _zooming;

		// Token: 0x040017F4 RID: 6132
		protected float _startFieldOfView;

		// Token: 0x040017F5 RID: 6133
		protected float _transitionDuration;

		// Token: 0x040017F6 RID: 6134
		protected float _duration;

		// Token: 0x040017F7 RID: 6135
		protected float _targetFieldOfView;

		// Token: 0x040017F8 RID: 6136
		protected float _delta;

		// Token: 0x040017F9 RID: 6137
		protected int _direction = 1;

		// Token: 0x040017FA RID: 6138
		protected float _reachedDestinationTimestamp;

		// Token: 0x040017FB RID: 6139
		protected bool _destinationReached;

		// Token: 0x040017FC RID: 6140
		protected float _elapsedTime;

		// Token: 0x040017FD RID: 6141
		protected float _zoomStartedAt;
	}
}
