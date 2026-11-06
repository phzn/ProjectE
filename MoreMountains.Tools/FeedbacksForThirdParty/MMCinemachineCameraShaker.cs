using System;
using System.Collections;
using Cinemachine;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002E6 RID: 742
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Cinemachine/MMCinemachineCameraShaker")]
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class MMCinemachineCameraShaker : MonoBehaviour
	{
		// Token: 0x060014C2 RID: 5314 RVA: 0x00058C27 File Offset: 0x00056E27
		public virtual float GetTime()
		{
			if (this._timescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00058C3C File Offset: 0x00056E3C
		public virtual float GetDeltaTime()
		{
			if (this._timescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00058C51 File Offset: 0x00056E51
		protected virtual void Awake()
		{
			this._virtualCamera = base.gameObject.GetComponent<CinemachineVirtualCamera>();
			this._perlin = this._virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00058C78 File Offset: 0x00056E78
		protected virtual void Start()
		{
			if (this._perlin != null)
			{
				this.IdleAmplitude = this._perlin.m_AmplitudeGain;
				this.IdleFrequency = this._perlin.m_FrequencyGain;
			}
			this._targetAmplitude = this.IdleAmplitude;
			this._targetFrequency = this.IdleFrequency;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x00058CD0 File Offset: 0x00056ED0
		protected virtual void Update()
		{
			if (this._perlin != null)
			{
				this._perlin.m_AmplitudeGain = this._targetAmplitude;
				this._perlin.m_FrequencyGain = Mathf.Lerp(this._perlin.m_FrequencyGain, this._targetFrequency, this.GetDeltaTime() * this.LerpSpeed);
			}
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00058D2A File Offset: 0x00056F2A
		public virtual void ShakeCamera(float duration, bool infinite, bool useUnscaledTime = false)
		{
			base.StartCoroutine(this.ShakeCameraCo(duration, this.DefaultShakeAmplitude, this.DefaultShakeFrequency, infinite, useUnscaledTime));
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x00058D48 File Offset: 0x00056F48
		public virtual void ShakeCamera(float duration, float amplitude, float frequency, bool infinite, bool useUnscaledTime = false)
		{
			if (this._shakeCoroutine != null)
			{
				base.StopCoroutine(this._shakeCoroutine);
			}
			this._shakeCoroutine = base.StartCoroutine(this.ShakeCameraCo(duration, amplitude, frequency, infinite, useUnscaledTime));
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00058D77 File Offset: 0x00056F77
		protected virtual IEnumerator ShakeCameraCo(float duration, float amplitude, float frequency, bool infinite, bool useUnscaledTime)
		{
			this._targetAmplitude = amplitude;
			this._targetFrequency = frequency;
			this._timescaleMode = (useUnscaledTime ? TimescaleModes.Unscaled : TimescaleModes.Scaled);
			if (!infinite)
			{
				yield return new WaitForSeconds(duration);
				this.CameraReset();
			}
			yield break;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00058DAB File Offset: 0x00056FAB
		public virtual void CameraReset()
		{
			this._targetAmplitude = this.IdleAmplitude;
			this._targetFrequency = this.IdleFrequency;
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00058DC5 File Offset: 0x00056FC5
		public virtual void OnCameraShakeEvent(float duration, float amplitude, float frequency, float amplitudeX, float amplitudeY, float amplitudeZ, bool infinite, MMChannelData channelData, bool useUnscaledTime)
		{
			if (!MMChannel.Match(channelData, this.ChannelMode, this.Channel, this.MMChannelDefinition))
			{
				return;
			}
			this.ShakeCamera(duration, amplitude, frequency, infinite, useUnscaledTime);
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00058DF0 File Offset: 0x00056FF0
		public virtual void OnCameraShakeStopEvent(MMChannelData channelData)
		{
			if (!MMChannel.Match(channelData, this.ChannelMode, this.Channel, this.MMChannelDefinition))
			{
				return;
			}
			if (this._shakeCoroutine != null)
			{
				base.StopCoroutine(this._shakeCoroutine);
			}
			this.CameraReset();
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00058E27 File Offset: 0x00057027
		protected virtual void OnEnable()
		{
			MMCameraShakeEvent.Register(new MMCameraShakeEvent.Delegate(this.OnCameraShakeEvent));
			MMCameraShakeStopEvent.Register(new MMCameraShakeStopEvent.Delegate(this.OnCameraShakeStopEvent));
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00058E4D File Offset: 0x0005704D
		protected virtual void OnDisable()
		{
			MMCameraShakeEvent.Unregister(new MMCameraShakeEvent.Delegate(this.OnCameraShakeEvent));
			MMCameraShakeStopEvent.Unregister(new MMCameraShakeStopEvent.Delegate(this.OnCameraShakeStopEvent));
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x00058E74 File Offset: 0x00057074
		protected virtual void TestShake()
		{
			MMCameraShakeEvent.Trigger(this.TestDuration, this.TestAmplitude, this.TestFrequency, 0f, 0f, 0f, false, new MMChannelData(this.ChannelMode, this.Channel, this.MMChannelDefinition), false);
		}

		// Token: 0x040017B5 RID: 6069
		[Header("Settings")]
		[Tooltip("whether to listen on a channel defined by an int or by a MMChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x040017B6 RID: 6070
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[MMFEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x040017B7 RID: 6071
		[Tooltip("the MMChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMFEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x040017B8 RID: 6072
		[Tooltip("The default amplitude that will be applied to your shakes if you don't specify one")]
		public float DefaultShakeAmplitude = 0.5f;

		// Token: 0x040017B9 RID: 6073
		[Tooltip("The default frequency that will be applied to your shakes if you don't specify one")]
		public float DefaultShakeFrequency = 10f;

		// Token: 0x040017BA RID: 6074
		[Tooltip("the amplitude of the camera's noise when it's idle")]
		[MMFReadOnly]
		public float IdleAmplitude;

		// Token: 0x040017BB RID: 6075
		[Tooltip("the frequency of the camera's noise when it's idle")]
		[MMFReadOnly]
		public float IdleFrequency = 1f;

		// Token: 0x040017BC RID: 6076
		[Tooltip("the speed at which to interpolate the shake")]
		public float LerpSpeed = 5f;

		// Token: 0x040017BD RID: 6077
		[Header("Test")]
		[Tooltip("a duration (in seconds) to apply when testing this shake via the TestShake button")]
		public float TestDuration = 0.3f;

		// Token: 0x040017BE RID: 6078
		[Tooltip("the amplitude to apply when testing this shake via the TestShake button")]
		public float TestAmplitude = 2f;

		// Token: 0x040017BF RID: 6079
		[Tooltip("the frequency to apply when testing this shake via the TestShake button")]
		public float TestFrequency = 20f;

		// Token: 0x040017C0 RID: 6080
		[MMFInspectorButton("TestShake")]
		public bool TestShakeButton;

		// Token: 0x040017C1 RID: 6081
		protected TimescaleModes _timescaleMode;

		// Token: 0x040017C2 RID: 6082
		protected Vector3 _initialPosition;

		// Token: 0x040017C3 RID: 6083
		protected Quaternion _initialRotation;

		// Token: 0x040017C4 RID: 6084
		protected CinemachineBasicMultiChannelPerlin _perlin;

		// Token: 0x040017C5 RID: 6085
		protected CinemachineVirtualCamera _virtualCamera;

		// Token: 0x040017C6 RID: 6086
		protected float _targetAmplitude;

		// Token: 0x040017C7 RID: 6087
		protected float _targetFrequency;

		// Token: 0x040017C8 RID: 6088
		private Coroutine _shakeCoroutine;
	}
}
