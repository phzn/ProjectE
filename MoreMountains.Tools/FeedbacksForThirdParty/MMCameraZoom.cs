using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002E0 RID: 736
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Camera/MMCameraZoom")]
	public class MMCameraZoom : MonoBehaviour
	{
		// Token: 0x06001497 RID: 5271 RVA: 0x00058394 File Offset: 0x00056594
		public virtual float GetTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x000583A9 File Offset: 0x000565A9
		public virtual float GetDeltaTime()
		{
			if (this.TimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x000583BE File Offset: 0x000565BE
		// (set) Token: 0x0600149A RID: 5274 RVA: 0x000583C6 File Offset: 0x000565C6
		public virtual TimescaleModes TimescaleMode { get; set; }

		// Token: 0x0600149B RID: 5275 RVA: 0x000583CF File Offset: 0x000565CF
		protected virtual void Awake()
		{
			this._camera = base.gameObject.GetComponent<Camera>();
			this._initialFieldOfView = this._camera.fieldOfView;
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x000583F4 File Offset: 0x000565F4
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
				this._camera.fieldOfView = Mathf.LerpUnclamped(this._startFieldOfView, this._targetFieldOfView, this.ZoomTween.Evaluate(num));
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

		// Token: 0x0600149D RID: 5277 RVA: 0x000584EC File Offset: 0x000566EC
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
			this._startFieldOfView = this._camera.fieldOfView;
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

		// Token: 0x0600149E RID: 5278 RVA: 0x000585BF File Offset: 0x000567BF
		protected virtual void TestZoom()
		{
			this.Zoom(this.TestMode, this.TestFieldOfView, this.TestTransitionDuration, this.TestDuration, false, false, this.ZoomTween);
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x000585E8 File Offset: 0x000567E8
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
				this._camera.fieldOfView = this._initialFieldOfView;
				return;
			}
			this.Zoom(mode, newFieldOfView, transitionDuration, duration, useUnscaledTime, relative, tweenType);
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x00058644 File Offset: 0x00056844
		protected virtual void OnEnable()
		{
			MMCameraZoomEvent.Register(new MMCameraZoomEvent.Delegate(this.OnCameraZoomEvent));
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x00058658 File Offset: 0x00056858
		protected virtual void OnDisable()
		{
			MMCameraZoomEvent.Unregister(new MMCameraZoomEvent.Delegate(this.OnCameraZoomEvent));
		}

		// Token: 0x0400178B RID: 6027
		[Header("Channel")]
		[MMFInspectorGroup("Shaker Settings", true, 3, false, false)]
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x0400178C RID: 6028
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMFEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x0400178D RID: 6029
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMFEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x0400178E RID: 6030
		[Header("Transition Speed")]
		[Tooltip("the animation curve to apply to the zoom transition")]
		public MMTweenType ZoomTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x0400178F RID: 6031
		[Header("Test Zoom")]
		[Tooltip("the mode to apply the zoom in when using the test button in the inspector")]
		public MMCameraZoomModes TestMode;

		// Token: 0x04001790 RID: 6032
		[Tooltip("the target field of view to apply the zoom in when using the test button in the inspector")]
		public float TestFieldOfView = 30f;

		// Token: 0x04001791 RID: 6033
		[Tooltip("the transition duration to apply the zoom in when using the test button in the inspector")]
		public float TestTransitionDuration = 0.1f;

		// Token: 0x04001792 RID: 6034
		[Tooltip("the duration to apply the zoom in when using the test button in the inspector")]
		public float TestDuration = 0.05f;

		// Token: 0x04001793 RID: 6035
		[MMFInspectorButton("TestZoom")]
		public bool TestZoomButton;

		// Token: 0x04001795 RID: 6037
		protected Camera _camera;

		// Token: 0x04001796 RID: 6038
		protected float _initialFieldOfView;

		// Token: 0x04001797 RID: 6039
		protected MMCameraZoomModes _mode;

		// Token: 0x04001798 RID: 6040
		protected bool _zooming;

		// Token: 0x04001799 RID: 6041
		protected float _startFieldOfView;

		// Token: 0x0400179A RID: 6042
		protected float _transitionDuration;

		// Token: 0x0400179B RID: 6043
		protected float _duration;

		// Token: 0x0400179C RID: 6044
		protected float _targetFieldOfView;

		// Token: 0x0400179D RID: 6045
		protected int _direction = 1;

		// Token: 0x0400179E RID: 6046
		protected float _reachedDestinationTimestamp;

		// Token: 0x0400179F RID: 6047
		protected bool _destinationReached;

		// Token: 0x040017A0 RID: 6048
		protected float _elapsedTime;

		// Token: 0x040017A1 RID: 6049
		protected float _zoomStartedAt;
	}
}
