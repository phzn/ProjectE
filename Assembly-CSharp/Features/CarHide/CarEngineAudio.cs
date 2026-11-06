using System;
using UnityEngine;
using VehicleSpawnPlugin;

namespace Features.CarHide
{
	// Token: 0x020014C6 RID: 5318
	[DisallowMultipleComponent]
	public class CarEngineAudio : MonoBehaviour
	{
		// Token: 0x0600A376 RID: 41846 RVA: 0x0043E804 File Offset: 0x0043CA04
		private void Awake()
		{
			this._vehicle = base.GetComponent<VehicleAI>();
			if (this.source == null)
			{
				this.source = base.GetComponent<AudioSource>();
			}
			this._detune = Random.Range(-0.04f, 0.04f);
			this._lastPos = base.transform.position;
			if (this.source != null)
			{
				this.source.loop = false;
				this._twin = base.gameObject.AddComponent<AudioSource>();
				CarEngineAudio.CopySourceSettings(this.source, this._twin);
				this._front = this.source;
				this._back = this._twin;
			}
		}

		// Token: 0x0600A377 RID: 41847 RVA: 0x0043E8B4 File Offset: 0x0043CAB4
		private static void CopySourceSettings(AudioSource from, AudioSource to)
		{
			to.clip = from.clip;
			to.loop = false;
			to.playOnAwake = false;
			to.outputAudioMixerGroup = from.outputAudioMixerGroup;
			to.spatialBlend = from.spatialBlend;
			to.rolloffMode = from.rolloffMode;
			to.minDistance = from.minDistance;
			to.maxDistance = from.maxDistance;
			to.dopplerLevel = from.dopplerLevel;
			to.volume = 0f;
		}

		// Token: 0x0600A378 RID: 41848 RVA: 0x0043E930 File Offset: 0x0043CB30
		private void Update()
		{
			if (this.source == null || this._twin == null)
			{
				return;
			}
			if (!(this._vehicle != null) || !this._vehicle.enabled || !(this.source.clip != null))
			{
				if (this._front.isPlaying)
				{
					this._front.Stop();
				}
				if (this._back.isPlaying)
				{
					this._back.Stop();
				}
				this._fading = false;
				return;
			}
			if (!this._front.isPlaying && !this._fading)
			{
				this._front.time = this.loopTrimStart;
				this._front.Play();
			}
			float num = Time.time - this._lastSampleTime;
			if (num >= 0.5f)
			{
				this._speed = (base.transform.position - this._lastPos).magnitude / num;
				this._lastPos = base.transform.position;
				this._lastSampleTime = Time.time;
			}
			float num2 = Mathf.Clamp01(this._speed / Mathf.Max(this.fullVolumeSpeed, 0.1f));
			float num3 = Mathf.Lerp(this.idleVolume, this.drivingVolume, num2);
			float num4 = this.idlePitch + this.drivingPitchBoost * num2 + this._detune;
			this._front.pitch = num4;
			this._back.pitch = num4;
			float num5 = Mathf.Max(this.crossfadeSeconds, 0.05f);
			float num6 = this.source.clip.length - this.loopTrimEnd - num5;
			if (!this._fading && this._front.time >= num6)
			{
				this._back.time = this.loopTrimStart;
				this._back.Play();
				this._fading = true;
			}
			if (this._fading)
			{
				float num7 = Mathf.Clamp01((this._front.time - num6) / num5);
				if (!this._front.isPlaying)
				{
					num7 = 1f;
				}
				this._front.volume = num3 * (1f - num7);
				this._back.volume = num3 * num7;
				if (num7 >= 1f)
				{
					this._front.Stop();
					AudioSource front = this._front;
					this._front = this._back;
					this._back = front;
					this._fading = false;
					return;
				}
			}
			else
			{
				this._front.volume = num3;
				this._back.volume = 0f;
			}
		}

		// Token: 0x0400A72E RID: 42798
		[Header("Source (configured by the rollout tool)")]
		[Tooltip("3D source with the CarAudio clip. Auto-resolved from this GameObject; a twin source is cloned from it at runtime for the crossfaded loop.")]
		[SerializeField]
		private AudioSource source;

		// Token: 0x0400A72F RID: 42799
		[Header("Volume")]
		[Tooltip("Rumble volume while the car idles (stopped, engine on).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float idleVolume = 0.08f;

		// Token: 0x0400A730 RID: 42800
		[Tooltip("Rumble volume at/above full cruise speed. Kept LOW on purpose.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float drivingVolume = 0.22f;

		// Token: 0x0400A731 RID: 42801
		[Tooltip("Speed (m/s) at which drivingVolume is reached.")]
		[SerializeField]
		private float fullVolumeSpeed = 8f;

		// Token: 0x0400A732 RID: 42802
		[Header("Seamless loop")]
		[Tooltip("Seconds skipped at the clip HEAD (MP3 encoder padding).")]
		[SerializeField]
		private float loopTrimStart = 0.15f;

		// Token: 0x0400A733 RID: 42803
		[Tooltip("Seconds skipped at the clip TAIL (MP3 encoder padding).")]
		[SerializeField]
		private float loopTrimEnd = 0.2f;

		// Token: 0x0400A734 RID: 42804
		[Tooltip("Overlap between the outgoing and incoming source at every wrap.")]
		[SerializeField]
		private float crossfadeSeconds = 0.5f;

		// Token: 0x0400A735 RID: 42805
		[Header("Pitch")]
		[Tooltip("Pitch while idling; rises slightly with speed. Each car also gets a tiny random detune so a queue of cars doesn't phase into one droning note.")]
		[SerializeField]
		private float idlePitch = 0.9f;

		// Token: 0x0400A736 RID: 42806
		[SerializeField]
		private float drivingPitchBoost = 0.25f;

		// Token: 0x0400A737 RID: 42807
		private const float SpeedSampleWindow = 0.5f;

		// Token: 0x0400A738 RID: 42808
		private VehicleAI _vehicle;

		// Token: 0x0400A739 RID: 42809
		private Vector3 _lastPos;

		// Token: 0x0400A73A RID: 42810
		private float _lastSampleTime;

		// Token: 0x0400A73B RID: 42811
		private float _speed;

		// Token: 0x0400A73C RID: 42812
		private float _detune;

		// Token: 0x0400A73D RID: 42813
		private AudioSource _twin;

		// Token: 0x0400A73E RID: 42814
		private AudioSource _front;

		// Token: 0x0400A73F RID: 42815
		private AudioSource _back;

		// Token: 0x0400A740 RID: 42816
		private bool _fading;
	}
}
