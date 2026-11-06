using System;
using Features.Inventory.Wearable;
using Features.NippleRotor;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x02001225 RID: 4645
	[CreateAssetMenu(menuName = "Features/Egg Rotor/Config", fileName = "EggRotorConfig")]
	public sealed class EggRotorConfigSO : ScriptableObject
	{
		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x06008FD0 RID: 36816 RVA: 0x003AD609 File Offset: 0x003AB809
		public float ApproachSeconds
		{
			get
			{
				return Mathf.Max(0f, this.insertSeconds) * Mathf.Clamp(this.insertApproachFraction, 0.05f, 0.9f);
			}
		}

		// Token: 0x170016FC RID: 5884
		// (get) Token: 0x06008FD1 RID: 36817 RVA: 0x003AD631 File Offset: 0x003AB831
		public float SlideInSeconds
		{
			get
			{
				return Mathf.Max(0f, this.insertSeconds) * (1f - Mathf.Clamp(this.insertApproachFraction, 0.05f, 0.9f));
			}
		}

		// Token: 0x06008FD2 RID: 36818 RVA: 0x003AD65F File Offset: 0x003AB85F
		public static float[] CreateDefaultInsideNoiseRadii()
		{
			return new float[] { 3f, 6f, 10f };
		}

		// Token: 0x06008FD3 RID: 36819 RVA: 0x003AD674 File Offset: 0x003AB874
		public static NippleRotorCordSettings CreateAttachedCord()
		{
			return new NippleRotorCordSettings
			{
				mainLength = 0.14f,
				mainNodes = 16,
				maxStretch = 1.6f,
				radius = 0.0012f,
				torsoRadius = 0f,
				breastRadius = 0f,
				thighRadius = 0.055f,
				gravity = 1f,
				damping = 2.5f,
				motionInfluence = 0.15f,
				shapeSpring = 4f
			};
		}

		// Token: 0x06008FD4 RID: 36820 RVA: 0x003AD6FC File Offset: 0x003AB8FC
		public static NippleRotorCordSettings CreateSwingCord()
		{
			return new NippleRotorCordSettings
			{
				mainLength = 0.18f,
				mainNodes = 16,
				maxStretch = 1.6f,
				radius = 0.0012f,
				torsoRadius = 0f,
				breastRadius = 0f,
				thighRadius = 0.055f,
				gravity = 1f,
				damping = 1.2f,
				motionInfluence = 0.35f,
				shapeSpring = 0.5f
			};
		}

		// Token: 0x06008FD5 RID: 36821 RVA: 0x003AD784 File Offset: 0x003AB984
		public NippleRotorLevel GetLevel(int level)
		{
			if (level < 1 || this.levels == null || level > this.levels.Length)
			{
				return default(NippleRotorLevel);
			}
			return this.levels[level - 1];
		}

		// Token: 0x06008FD6 RID: 36822 RVA: 0x003AD7C0 File Offset: 0x003AB9C0
		public static float NoiseRadiusFor(EggRotorConfigSO config, int level, EggRotorPlacement placement)
		{
			if (config == null || level <= 0)
			{
				return 0f;
			}
			level = EggRotorIds.ClampLevel(level);
			if (placement != EggRotorPlacement.Inside)
			{
				return Mathf.Max(0f, config.GetLevel(level).noiseRadius);
			}
			float[] array = config.insideNoiseRadii;
			int num = level - 1;
			if (array == null || num >= array.Length)
			{
				return 0f;
			}
			return Mathf.Max(0f, array[num]);
		}

		// Token: 0x06008FD7 RID: 36823 RVA: 0x003AD829 File Offset: 0x003ABA29
		public bool MigrateLegacyValues()
		{
			return this.rig != null && this.rig.MigrateLegacyInsidePose();
		}

		// Token: 0x06008FD8 RID: 36824 RVA: 0x003AD840 File Offset: 0x003ABA40
		private void OnEnable()
		{
			this.MigrateLegacyValues();
		}

		// Token: 0x06008FD9 RID: 36825 RVA: 0x003AD84C File Offset: 0x003ABA4C
		private void OnValidate()
		{
			this.outsideLoops = EggRotorConfigSO.EnsureLength(this.outsideLoops);
			this.insideLoops = EggRotorConfigSO.EnsureLength(this.insideLoops);
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
			if (this.insideNoiseRadii == null || this.insideNoiseRadii.Length < 3)
			{
				float[] array2 = EggRotorConfigSO.CreateDefaultInsideNoiseRadii();
				int num2 = 0;
				while (this.insideNoiseRadii != null && num2 < this.insideNoiseRadii.Length)
				{
					array2[num2] = this.insideNoiseRadii[num2];
					num2++;
				}
				this.insideNoiseRadii = array2;
			}
			this.audioMinDistance = Mathf.Max(0.01f, this.audioMinDistance);
			this.audioMaxDistance = Mathf.Max(this.audioMinDistance + 0.01f, this.audioMaxDistance);
			this.audioFadeSeconds = Mathf.Max(0f, this.audioFadeSeconds);
			this.npcRefreshHz = Mathf.Max(0f, this.npcRefreshHz);
			this.remoteBuzzScale = Mathf.Max(0f, this.remoteBuzzScale);
			this.insertSeconds = Mathf.Max(0f, this.insertSeconds);
			this.removeSeconds = Mathf.Max(0f, this.removeSeconds);
			this.placeSeconds = Mathf.Max(0f, this.placeSeconds);
			this.glansSeconds = Mathf.Max(0f, this.glansSeconds);
			this.insertionBlendShapeFadeSeconds = Mathf.Max(0f, this.insertionBlendShapeFadeSeconds);
			if (this.rig == null)
			{
				this.rig = new EggRotorRigSettings();
			}
			this.rig.MigrateLegacyInsidePose();
			this.rig.eggScale = Mathf.Max(0.0001f, this.rig.eggScale);
			this.rig.remoteScale = Mathf.Max(0.0001f, this.rig.remoteScale);
			if (this.attachedCord == null)
			{
				this.attachedCord = EggRotorConfigSO.CreateAttachedCord();
			}
			if (this.swingCord == null)
			{
				this.swingCord = EggRotorConfigSO.CreateSwingCord();
			}
			if (this.climax == null)
			{
				this.climax = new EggRotorClimaxSettings();
			}
			this.climax.Validate();
		}

		// Token: 0x06008FDA RID: 36826 RVA: 0x003ADA98 File Offset: 0x003ABC98
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

		// Token: 0x040093C4 RID: 37828
		private const int VibrationLevels = 3;

		// Token: 0x040093C5 RID: 37829
		[Header("Assets")]
		public WearableItemSO item;

		// Token: 0x040093C6 RID: 37830
		[Tooltip("Remote and band in the band frame (origin at the ring centre); \"Buzz\" with \"CordSocket\" (+Z along the cable).")]
		public GameObject remotePrefab;

		// Token: 0x040093C7 RID: 37831
		[Tooltip("The egg, root = cord socket frame; \"Buzz\" (NippleRotorBuzz) with \"Egg\", \"Tape\" and \"CordSocket\".")]
		public GameObject eggPrefab;

		// Token: 0x040093C8 RID: 37832
		[Tooltip("The Pura² pack's cable material.")]
		public Material cordMaterial;

		// Token: 0x040093C9 RID: 37833
		[Header("Audio")]
		[Tooltip("[0]=Low [1]=Medium [2]=High. Outside and Swing.")]
		public AudioClip[] outsideLoops = new AudioClip[3];

		// Token: 0x040093CA RID: 37834
		[Tooltip("Muffled versions: the egg is inside, or a garment covers the crotch.")]
		public AudioClip[] insideLoops = new AudioClip[3];

		// Token: 0x040093CB RID: 37835
		public AudioClip switchClip;

		// Token: 0x040093CC RID: 37836
		[Header("Levels (Low, Medium, High)")]
		[Tooltip("noiseRadius is the NPC hearing radius Outside and on the Swing.")]
		public NippleRotorLevel[] levels = NippleRotorConfigSO.CreateDefaultLevels();

		// Token: 0x040093CD RID: 37837
		[Tooltip("NPC hearing radius per level while the egg is inside (Low, Medium, High).")]
		public float[] insideNoiseRadii = EggRotorConfigSO.CreateDefaultInsideNoiseRadii();

		// Token: 0x040093CE RID: 37838
		[Header("Audio Playback")]
		[Range(0f, 1f)]
		public float audioSpatialBlend = 0.35f;

		// Token: 0x040093CF RID: 37839
		public float audioMinDistance = 2f;

		// Token: 0x040093D0 RID: 37840
		public float audioMaxDistance = 20f;

		// Token: 0x040093D1 RID: 37841
		public float audioFadeSeconds = 0.2f;

		// Token: 0x040093D2 RID: 37842
		[Range(0f, 1f)]
		public float switchVolume = 0.55f;

		// Token: 0x040093D3 RID: 37843
		[Tooltip("Level volume multiplier for the muffled Inside loops (they are louder than the Outside ones). Same mix as the nipple rotor under clothes.")]
		[Range(0f, 1f)]
		public float insideLoopsVolumeScale = 0.6f;

		// Token: 0x040093D4 RID: 37844
		[Header("Feedback")]
		[Tooltip("AnimeBodyPhysics.TryShake strength01 when the level goes up (0 = off).")]
		[Range(0f, 1f)]
		public float levelChangeShake = 0.2f;

		// Token: 0x040093D5 RID: 37845
		[Tooltip("LastKnownPlayerPosition / Searching target refresh rate while vibrating.")]
		public float npcRefreshHz = 4f;

		// Token: 0x040093D6 RID: 37846
		[Tooltip("Remote jitter relative to the egg (0 = the box on the thigh stays still).")]
		public float remoteBuzzScale;

		// Token: 0x040093D7 RID: 37847
		[Header("Emotes")]
		[Tooltip("Shown when a masturbation emote is started while the egg is worn: they are disabled then, in every placement (their hand and hip animation fights the egg). English gameplay_prompts source text; it has no translation entry, so every language shows it in English. The toys stay available.")]
		public string masturbationEmotesBlockedMessage = "Remove the Egg Rotor first";

		// Token: 0x040093D8 RID: 37848
		[Header("Placement")]
		[Tooltip("Placement used the first time the rotor is worn; later wears keep the last choice.")]
		public EggRotorPlacement defaultPlacement = EggRotorPlacement.Outside;

		// Token: 0x040093D9 RID: 37849
		[Tooltip("Seconds to insert the egg: the move onto the entry plus the slide in.")]
		public float insertSeconds = 1.3f;

		// Token: 0x040093DA RID: 37850
		[Tooltip("Share of insertSeconds spent moving onto the entry before the slide.")]
		[Range(0.05f, 0.9f)]
		public float insertApproachFraction = 0.3f;

		// Token: 0x040093DB RID: 37851
		[Tooltip("Seconds to slide the egg out from the Inside pose back to the entry.")]
		public float removeSeconds = 0.55f;

		// Token: 0x040093DC RID: 37852
		[Tooltip("Seconds to move the egg onto its taped Outside pose.")]
		public float placeSeconds = 0.25f;

		// Token: 0x040093DD RID: 37853
		[Tooltip("Seconds to move the egg onto the glans (Futanari Mode).")]
		public float glansSeconds = 0.35f;

		// Token: 0x040093DE RID: 37854
		[Tooltip("Body blend shape that opens the vagina around the egg: it rises with the slide and is held while the egg is inside (BlendShapeManager, Animation priority). Kupa_V_LR is the shape the vaginal dildo actions key on Ciel's Body_sotai_NSFW; her body has no Vagina_Open01/02 (the dildo clips key those only on the source pack's Girl_Body).")]
		public string insertionBlendShape = "Kupa_V_LR";

		// Token: 0x040093DF RID: 37855
		[Tooltip("Weight held while the egg is inside (0 = off): the labia close a little around the tail, which still shows from below. The vaginal dildo actions hold 60-80, toys 100.")]
		[Range(0f, 100f)]
		public float insertionBlendShapeWeight = 45f;

		// Token: 0x040093E0 RID: 37856
		[Tooltip("Widest opening during the slide, while the egg's widest part passes the opening (at or above the held weight).")]
		[Range(0f, 100f)]
		public float insertionPeakBlendShapeWeight = 80f;

		// Token: 0x040093E1 RID: 37857
		[Tooltip("Seconds for the full opening to close when it is released without the slide (a garment covers the crotch) or to follow a weight edit. The slides drive it directly; 0 snaps.")]
		public float insertionBlendShapeFadeSeconds = 0.25f;

		// Token: 0x040093E2 RID: 37858
		[Header("Rig")]
		public EggRotorRigSettings rig = new EggRotorRigSettings();

		// Token: 0x040093E3 RID: 37859
		[Tooltip("Single strand remote -> egg socket (the egg's tail, outside the body in Inside and Outside). mainLength is the strand length. Keep mainLength and mainNodes equal to swingCord's: the drop onto the Swing then releases the cable in place.")]
		public NippleRotorCordSettings attachedCord = EggRotorConfigSO.CreateAttachedCord();

		// Token: 0x040093E4 RID: 37860
		[Tooltip("Single strand from the remote with a free end that carries the egg (Swing): the same cable, only the physics differ.")]
		public NippleRotorCordSettings swingCord = EggRotorConfigSO.CreateSwingCord();

		// Token: 0x040093E5 RID: 37861
		[Header("Climax (orgasm meter)")]
		[Tooltip("The egg fills the orgasm meter; at 100 % Ciel stops, trembles and squirts (EggRotorClimax).")]
		public EggRotorClimaxSettings climax = new EggRotorClimaxSettings();

		// Token: 0x040093E6 RID: 37862
		public const string DefaultMasturbationEmotesBlockedMessage = "Remove the Egg Rotor first";

		// Token: 0x040093E7 RID: 37863
		public const float DefaultOpeningWeight = 45f;

		// Token: 0x040093E8 RID: 37864
		public const float DefaultPeakOpeningWeight = 80f;

		// Token: 0x040093E9 RID: 37865
		public const float DefaultCableLength = 0.18f;

		// Token: 0x040093EA RID: 37866
		public const float AttachedCableLength = 0.14f;

		// Token: 0x040093EB RID: 37867
		public const int DefaultCableNodes = 16;
	}
}
