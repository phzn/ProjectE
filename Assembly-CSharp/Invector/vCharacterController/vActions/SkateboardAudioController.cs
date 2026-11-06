using System;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x020006FF RID: 1791
	[RequireComponent(typeof(SkateboardInteractable))]
	public class SkateboardAudioController : MonoBehaviour
	{
		// Token: 0x06002DAC RID: 11692 RVA: 0x000FF9F4 File Offset: 0x000FDBF4
		private void Awake()
		{
			this.skateboard = base.GetComponent<SkateboardInteractable>();
			this.source = base.gameObject.AddComponent<AudioSource>();
			this.source.clip = this.skateClip;
			this.source.loop = true;
			this.source.playOnAwake = false;
			this.source.volume = 0f;
			this.source.spatialBlend = this.spatialBlend;
			this.source.dopplerLevel = 0f;
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000FFA78 File Offset: 0x000FDC78
		private void Update()
		{
			if (this.source == null || this.source.clip == null)
			{
				return;
			}
			float num = Mathf.Abs(this.skateboard.CurrentSpeed);
			bool flag = this.skateboard.IsBeingWorn && num > this.silenceSpeed;
			float num2 = Mathf.Clamp01(num / Mathf.Max(0.01f, this.fullSpeed));
			float num3 = (flag ? Mathf.Lerp(this.volumeAtLowSpeed, this.volumeAtFullSpeed, num2) : 0f);
			float num4 = this.volumeAtFullSpeed / Mathf.Max(0.05f, this.fadeTime) * Time.deltaTime;
			this.source.volume = Mathf.MoveTowards(this.source.volume, num3, num4);
			if (this.source.volume > 0f && !this.source.isPlaying)
			{
				this.source.Play();
				return;
			}
			if (Mathf.Approximately(this.source.volume, 0f) && num3 <= 0f && this.source.isPlaying)
			{
				this.source.Stop();
			}
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000FFBA2 File Offset: 0x000FDDA2
		private void OnDisable()
		{
			if (this.source != null)
			{
				this.source.volume = 0f;
				this.source.Stop();
			}
		}

		// Token: 0x04002DAA RID: 11690
		[Header("Clip")]
		[Tooltip("Loop played while the board is rolling (CielSkateAudio)")]
		[SerializeField]
		private AudioClip skateClip;

		// Token: 0x04002DAB RID: 11691
		[Header("Volume (kept low: background texture)")]
		[Tooltip("Loop volume when barely moving (the quiet start).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float volumeAtLowSpeed = 0.04f;

		// Token: 0x04002DAC RID: 11692
		[Tooltip("Loop volume at full speed. Keep low so it never gets in the way.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float volumeAtFullSpeed = 0.15f;

		// Token: 0x04002DAD RID: 11693
		[Tooltip("Speed (m/s) considered 'full speed' for the volume ramp (sprint/balance cruise).")]
		[SerializeField]
		private float fullSpeed = 7.5f;

		// Token: 0x04002DAE RID: 11694
		[Tooltip("Speed (m/s) under which the board counts as stopped and the loop fades out.")]
		[SerializeField]
		private float silenceSpeed = 0.2f;

		// Token: 0x04002DAF RID: 11695
		[Header("Fading")]
		[Tooltip("Seconds for the loop to fade in from silence / out to silence.")]
		[SerializeField]
		private float fadeTime = 1.2f;

		// Token: 0x04002DB0 RID: 11696
		[Header("Source Settings")]
		[Tooltip("0 = 2D (always audible to the player), 1 = fully 3D positional.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float spatialBlend;

		// Token: 0x04002DB1 RID: 11697
		private SkateboardInteractable skateboard;

		// Token: 0x04002DB2 RID: 11698
		private AudioSource source;
	}
}
