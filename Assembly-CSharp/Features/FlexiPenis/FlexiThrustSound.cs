using System;
using UnityEngine;

namespace Features.FlexiPenis
{
	// Token: 0x020011FB RID: 4603
	public class FlexiThrustSound : MonoBehaviour
	{
		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x06008DF3 RID: 36339 RVA: 0x003A02C4 File Offset: 0x0039E4C4
		// (set) Token: 0x06008DF4 RID: 36340 RVA: 0x003A02CC File Offset: 0x0039E4CC
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
			}
		}

		// Token: 0x06008DF5 RID: 36341 RVA: 0x003A02D5 File Offset: 0x0039E4D5
		private void Awake()
		{
			this._source = base.gameObject.AddComponent<AudioSource>();
			this._source.playOnAwake = false;
			this._source.loop = false;
			this._source.spatialBlend = this.spatialBlend;
		}

		// Token: 0x06008DF6 RID: 36342 RVA: 0x003A0314 File Offset: 0x0039E514
		public void AE_FlexiThrust()
		{
			if (!this.active)
			{
				return;
			}
			if (this._source == null || this.thrustClips == null || this.thrustClips.Length == 0)
			{
				return;
			}
			if (Time.time - this._lastPlayTime < this.minInterval)
			{
				return;
			}
			AudioClip audioClip = this.thrustClips[this._next % this.thrustClips.Length];
			this._next++;
			if (audioClip == null)
			{
				return;
			}
			this._lastPlayTime = Time.time;
			this._source.pitch = Random.Range(this.pitchRange.x, this.pitchRange.y);
			float num = Mathf.Clamp01(this.volume - Random.Range(0f, this.volumeJitter));
			this._source.PlayOneShot(audioClip, num);
		}

		// Token: 0x04009126 RID: 37158
		[Header("Thrust SFX (the two dildo 'ciaf' sounds)")]
		[Tooltip("The two impact sounds played on each thrust, alternated. Assign Ciaf 1 / Ciaf 2 from Assets/Features/Dildo/Sounds.")]
		[SerializeField]
		private AudioClip[] thrustClips;

		// Token: 0x04009127 RID: 37159
		[Header("Playback")]
		[Range(0f, 1f)]
		[SerializeField]
		private float volume = 0.8f;

		// Token: 0x04009128 RID: 37160
		[Tooltip("Random pitch range applied per hit so repeats don't sound identical.")]
		[SerializeField]
		private Vector2 pitchRange = new Vector2(0.95f, 1.06f);

		// Token: 0x04009129 RID: 37161
		[Tooltip("Random volume jitter (0 = none). Subtracted from volume per hit.")]
		[Range(0f, 0.5f)]
		[SerializeField]
		private float volumeJitter = 0.1f;

		// Token: 0x0400912A RID: 37162
		[Tooltip("2D (0) vs 3D (1) sound. Kept 2D by default like the vibrator/hitachi loop.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float spatialBlend;

		// Token: 0x0400912B RID: 37163
		[Tooltip("Minimum seconds between two hits — guards against double-fires if events overlap.")]
		[SerializeField]
		private float minInterval = 0.05f;

		// Token: 0x0400912C RID: 37164
		[Header("Gate")]
		[Tooltip("When false, incoming AE_FlexiThrust events are ignored. FlexiWallHeldActions raises this only while the action is playing so idle/hold poses stay silent even if a clip leaks an event.")]
		[SerializeField]
		private bool active;

		// Token: 0x0400912D RID: 37165
		private AudioSource _source;

		// Token: 0x0400912E RID: 37166
		private int _next;

		// Token: 0x0400912F RID: 37167
		private float _lastPlayTime = -999f;
	}
}
