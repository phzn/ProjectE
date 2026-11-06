using System;
using Dinopunch.LivingMud;
using Features.MainMenu;
using UnityEngine;

namespace Features.Audio
{
	// Token: 0x0200152F RID: 5423
	[RequireComponent(typeof(Animator))]
	public class MudFootstepAudioController : MonoBehaviour
	{
		// Token: 0x0600A7F1 RID: 42993 RVA: 0x004621C4 File Offset: 0x004603C4
		internal bool OwnsAudioSource(AudioSource source)
		{
			return source != null && source == this.audioSource;
		}

		// Token: 0x0600A7F2 RID: 42994 RVA: 0x004621E0 File Offset: 0x004603E0
		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();
			this.audioSource = base.gameObject.AddComponent<AudioSource>();
			this.audioSource.playOnAwake = false;
			this.audioSource.spatialBlend = (this.is3DSound ? 1f : 0f);
			this.audioSource.minDistance = this.minDistance;
			this.audioSource.maxDistance = this.maxDistance;
			this.audioSource.rolloffMode = AudioRolloffMode.Linear;
			GameAudioRouter.Route(this.audioSource, GameAudioRouter.Channel.Sfx);
		}

		// Token: 0x0600A7F3 RID: 42995 RVA: 0x0046226F File Offset: 0x0046046F
		private void Start()
		{
			if (this.mudController == null)
			{
				this.mudController = base.GetComponent<FootMudController>();
			}
		}

		// Token: 0x0600A7F4 RID: 42996 RVA: 0x0046228C File Offset: 0x0046048C
		public void PlayMudFootstep(AnimationEvent evt)
		{
			if (this.audioSource == null)
			{
				return;
			}
			if (evt != null && evt.animatorClipInfo.weight < 0.5f)
			{
				return;
			}
			this.PlayInternal();
		}

		// Token: 0x0600A7F5 RID: 42997 RVA: 0x004622C7 File Offset: 0x004604C7
		public void PlayMudFootstepManual()
		{
			if (this.audioSource == null)
			{
				return;
			}
			this.PlayInternal();
		}

		// Token: 0x0600A7F6 RID: 42998 RVA: 0x004622E0 File Offset: 0x004604E0
		private void PlayInternal()
		{
			float currentSpeed = this.GetCurrentSpeed();
			if (currentSpeed < this.minSpeedForSound)
			{
				return;
			}
			AudioClip audioClip = this.SelectClip();
			if (audioClip == null)
			{
				return;
			}
			float num = this.baseVolume;
			if (this.scaleVolumeWithSpeed)
			{
				num = Mathf.Lerp(this.baseVolume * 0.5f, this.baseVolume, Mathf.Clamp01(currentSpeed / 2f));
			}
			this.audioSource.pitch = 1f + Random.Range(-this.pitchVariation, this.pitchVariation);
			this.audioSource.PlayOneShot(audioClip, num);
			if (this.debugMode)
			{
				string currentSurfaceLabel = this.GetCurrentSurfaceLabel();
				Debug.Log(string.Format("[MudFootstep] {0} | Clip: {1} | Vol: {2:F2} | Speed: {3:F2}", new object[] { currentSurfaceLabel, audioClip.name, num, currentSpeed }));
			}
		}

		// Token: 0x0600A7F7 RID: 42999 RVA: 0x004623B8 File Offset: 0x004605B8
		private AudioClip SelectClip()
		{
			if (this.mudController != null && this.mudController.IsInMud && this.mudController.CurrentSurface != null)
			{
				MudType currentMudType = this.mudController.CurrentMudType;
				if ((currentMudType & MudType.Watery) != (MudType)0 && this.waterClips != null && this.waterClips.Length != 0)
				{
					return this.GetRandomClip(this.waterClips, ref this.lastWaterIndex);
				}
				if ((currentMudType & (MudType.Sloppy | MudType.Dry)) != (MudType)0 && this.mudClips != null && this.mudClips.Length != 0)
				{
					return this.GetRandomClip(this.mudClips, ref this.lastMudIndex);
				}
			}
			if (this.normalClips != null && this.normalClips.Length != 0)
			{
				return this.GetRandomClip(this.normalClips, ref this.lastNormalIndex);
			}
			return null;
		}

		// Token: 0x0600A7F8 RID: 43000 RVA: 0x00462478 File Offset: 0x00460678
		private AudioClip GetRandomClip(AudioClip[] clips, ref int lastIndex)
		{
			if (clips.Length == 1)
			{
				return clips[0];
			}
			int num;
			do
			{
				num = Random.Range(0, clips.Length);
			}
			while (num == lastIndex && clips.Length > 1);
			lastIndex = num;
			return clips[num];
		}

		// Token: 0x0600A7F9 RID: 43001 RVA: 0x004624AC File Offset: 0x004606AC
		private float GetCurrentSpeed()
		{
			if (this.animator == null)
			{
				return 1f;
			}
			if (this.animator.HasParameter("InputMagnitude"))
			{
				return Mathf.Abs(this.animator.GetFloat("InputMagnitude"));
			}
			if (this.animator.HasParameter("VerticalSpeed"))
			{
				return Mathf.Abs(this.animator.GetFloat("VerticalSpeed"));
			}
			if (this.animator.HasParameter("Speed"))
			{
				return Mathf.Abs(this.animator.GetFloat("Speed"));
			}
			return 1f;
		}

		// Token: 0x0600A7FA RID: 43002 RVA: 0x0046254C File Offset: 0x0046074C
		private string GetCurrentSurfaceLabel()
		{
			if (this.mudController == null || !this.mudController.IsInMud)
			{
				return "Normal";
			}
			MudType currentMudType = this.mudController.CurrentMudType;
			if ((currentMudType & MudType.Watery) != (MudType)0)
			{
				return "Water";
			}
			if ((currentMudType & MudType.Sloppy) != (MudType)0)
			{
				return "Sloppy";
			}
			if ((currentMudType & MudType.Dry) != (MudType)0)
			{
				return "Dry";
			}
			return "Normal";
		}

		// Token: 0x0400AC9B RID: 44187
		[Header("Surface Audio Clips")]
		[Tooltip("Footstep clips for normal ground (no mud)")]
		[SerializeField]
		private AudioClip[] normalClips;

		// Token: 0x0400AC9C RID: 44188
		[Tooltip("Footstep clips for Sloppy/Dry mud surfaces")]
		[SerializeField]
		private AudioClip[] mudClips;

		// Token: 0x0400AC9D RID: 44189
		[Tooltip("Footstep clips for Watery surfaces")]
		[SerializeField]
		private AudioClip[] waterClips;

		// Token: 0x0400AC9E RID: 44190
		[Header("Volume")]
		[SerializeField]
		[Range(0f, 1f)]
		private float baseVolume = 0.5f;

		// Token: 0x0400AC9F RID: 44191
		[SerializeField]
		[Range(0f, 0.2f)]
		private float pitchVariation = 0.1f;

		// Token: 0x0400ACA0 RID: 44192
		[Tooltip("Scale volume with character speed")]
		[SerializeField]
		private bool scaleVolumeWithSpeed = true;

		// Token: 0x0400ACA1 RID: 44193
		[Tooltip("Minimum InputMagnitude to play a footstep sound")]
		[SerializeField]
		private float minSpeedForSound = 0.1f;

		// Token: 0x0400ACA2 RID: 44194
		[Header("3D Sound")]
		[SerializeField]
		private bool is3DSound = true;

		// Token: 0x0400ACA3 RID: 44195
		[SerializeField]
		private float minDistance = 1f;

		// Token: 0x0400ACA4 RID: 44196
		[SerializeField]
		private float maxDistance = 15f;

		// Token: 0x0400ACA5 RID: 44197
		[Header("References")]
		[Tooltip("FootMudController on the MC. Auto-detected if null.")]
		[SerializeField]
		private FootMudController mudController;

		// Token: 0x0400ACA6 RID: 44198
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x0400ACA7 RID: 44199
		private AudioSource audioSource;

		// Token: 0x0400ACA8 RID: 44200
		private Animator animator;

		// Token: 0x0400ACA9 RID: 44201
		private int lastNormalIndex = -1;

		// Token: 0x0400ACAA RID: 44202
		private int lastMudIndex = -1;

		// Token: 0x0400ACAB RID: 44203
		private int lastWaterIndex = -1;
	}
}
