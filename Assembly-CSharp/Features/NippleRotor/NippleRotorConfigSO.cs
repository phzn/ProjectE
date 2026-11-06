using System;
using Features.Inventory.Wearable;
using Features.NipplePiercings;
using UnityEngine;

namespace Features.NippleRotor
{
	// Token: 0x02000F54 RID: 3924
	[CreateAssetMenu(menuName = "Features/Nipple Rotor/Config", fileName = "NippleRotorConfig")]
	public sealed class NippleRotorConfigSO : ScriptableObject
	{
		// Token: 0x060075A5 RID: 30117 RVA: 0x002FA5B0 File Offset: 0x002F87B0
		public static NippleRotorLevel[] CreateDefaultLevels()
		{
			return new NippleRotorLevel[]
			{
				new NippleRotorLevel(4f, 0.00035f, 9f, 1.5f, 0.21f, 1f),
				new NippleRotorLevel(8f, 0.0007f, 13f, 2.5f, 0.33f, 1f),
				new NippleRotorLevel(14f, 0.0012f, 18f, 4f, 0.45f, 1f)
			};
		}

		// Token: 0x060075A6 RID: 30118 RVA: 0x002FA644 File Offset: 0x002F8844
		public NippleRotorLevel GetLevel(int level)
		{
			if (level < 1 || this.levels == null || level > this.levels.Length)
			{
				return default(NippleRotorLevel);
			}
			return this.levels[level - 1];
		}

		// Token: 0x060075A7 RID: 30119 RVA: 0x002FA680 File Offset: 0x002F8880
		public static float NoiseRadiusFor(NippleRotorConfigSO config, int level)
		{
			if (config == null || level <= 0)
			{
				return 0f;
			}
			return Mathf.Max(0f, config.GetLevel(NippleRotorIds.ClampLevel(level)).noiseRadius);
		}

		// Token: 0x060075A8 RID: 30120 RVA: 0x002FA6B0 File Offset: 0x002F88B0
		private void OnValidate()
		{
			this.outsideLoops = NippleRotorConfigSO.EnsureLength(this.outsideLoops);
			this.insideLoops = NippleRotorConfigSO.EnsureLength(this.insideLoops);
			if (this.levels == null || this.levels.Length < 3)
			{
				NippleRotorLevel[] array = NippleRotorConfigSO.CreateDefaultLevels();
				int num = 0;
				while (this.levels != null && num < this.levels.Length)
				{
					array[num] = this.levels[num];
					num++;
				}
				this.levels = array;
			}
			this.audioMinDistance = Mathf.Max(0.01f, this.audioMinDistance);
			this.audioMaxDistance = Mathf.Max(this.audioMinDistance + 0.01f, this.audioMaxDistance);
			this.audioFadeSeconds = Mathf.Max(0f, this.audioFadeSeconds);
			this.npcRefreshHz = Mathf.Max(0f, this.npcRefreshHz);
			this.remoteBuzzScale = Mathf.Max(0f, this.remoteBuzzScale);
			if (this.cord == null)
			{
				this.cord = new NippleRotorCordSettings();
			}
			if (this.grip == null)
			{
				this.grip = new NippleRotorGripSettings();
			}
		}

		// Token: 0x060075A9 RID: 30121 RVA: 0x002FA7C8 File Offset: 0x002F89C8
		private static AudioClip[] EnsureLength(AudioClip[] clips)
		{
			if (clips != null && clips.Length >= 3)
			{
				return clips;
			}
			AudioClip[] array = new AudioClip[3];
			int num = 0;
			while (clips != null && num < clips.Length)
			{
				array[num] = clips[num];
				num++;
			}
			return array;
		}

		// Token: 0x04007883 RID: 30851
		private const int VibrationLevels = 3;

		// Token: 0x04007884 RID: 30852
		[Header("Assets")]
		public WearableItemSO item;

		// Token: 0x04007885 RID: 30853
		[Tooltip("Root: NipplePiercingAttachment (+Erection, +Visibility, +StreamerCensorTarget).")]
		public GameObject cupsPrefab;

		// Token: 0x04007886 RID: 30854
		[Tooltip("Pivot = grip point; children \"Buzz\" (NippleRotorBuzz) and \"CordSocket\".")]
		public GameObject remotePrefab;

		// Token: 0x04007887 RID: 30855
		public Material cordMaterial;

		// Token: 0x04007888 RID: 30856
		[Tooltip("Ciel's breast skin samples the cable collides with (Tools/Nipple Rotor/Bake Breast Surface). Empty = the approximate breast spheres.")]
		public NipplePiercingSurfaceBinding breastSurface;

		// Token: 0x04007889 RID: 30857
		[Header("Audio")]
		[Tooltip("[0]=Low [1]=Medium [2]=High.")]
		public AudioClip[] outsideLoops = new AudioClip[3];

		// Token: 0x0400788A RID: 30858
		[Tooltip("Muffled versions, used while the cups are covered by a top.")]
		public AudioClip[] insideLoops = new AudioClip[3];

		// Token: 0x0400788B RID: 30859
		public AudioClip switchClip;

		// Token: 0x0400788C RID: 30860
		[Header("Levels (Low, Medium, High)")]
		public NippleRotorLevel[] levels = NippleRotorConfigSO.CreateDefaultLevels();

		// Token: 0x0400788D RID: 30861
		[Header("Audio Playback")]
		[Range(0f, 1f)]
		public float audioSpatialBlend = 0.35f;

		// Token: 0x0400788E RID: 30862
		public float audioMinDistance = 2f;

		// Token: 0x0400788F RID: 30863
		public float audioMaxDistance = 20f;

		// Token: 0x04007890 RID: 30864
		public float audioFadeSeconds = 0.2f;

		// Token: 0x04007891 RID: 30865
		[Range(0f, 1f)]
		public float switchVolume = 0.55f;

		// Token: 0x04007892 RID: 30866
		[Tooltip("Level volume multiplier while a garment covers the cups (the muffled Inside loops are louder).")]
		[Range(0f, 1f)]
		public float coveredVolumeScale = 0.6f;

		// Token: 0x04007893 RID: 30867
		[Header("Feedback")]
		[Tooltip("AnimeBodyPhysics.TryShake strength01 when the level goes up (0 = off).")]
		[Range(0f, 1f)]
		public float levelChangeShake = 0.35f;

		// Token: 0x04007894 RID: 30868
		[Tooltip("LastKnownPlayerPosition / Searching target refresh rate while vibrating.")]
		public float npcRefreshHz = 4f;

		// Token: 0x04007895 RID: 30869
		[Tooltip("Remote jitter relative to the cups.")]
		public float remoteBuzzScale = 0.35f;

		// Token: 0x04007896 RID: 30870
		[Header("Rig")]
		public NippleRotorCordSettings cord = new NippleRotorCordSettings();

		// Token: 0x04007897 RID: 30871
		public NippleRotorGripSettings grip = new NippleRotorGripSettings();
	}
}
