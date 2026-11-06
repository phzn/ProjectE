using System;
using Features.Detection;
using Features.Inventory.Placement;
using Features.PlayerEmotes;
using Features.Rank;
using UnityEngine;

namespace Features.Inventory.Actions
{
	// Token: 0x02001163 RID: 4451
	[CreateAssetMenu(fileName = "DildoAction_New", menuName = "Features/Inventory/Dildo Action Definition")]
	public class DildoActionDefinition : ScriptableObject
	{
		// Token: 0x1700156D RID: 5485
		// (get) Token: 0x06008793 RID: 34707 RVA: 0x00377970 File Offset: 0x00375B70
		public string ActionId
		{
			get
			{
				return this.actionId;
			}
		}

		// Token: 0x1700156E RID: 5486
		// (get) Token: 0x06008794 RID: 34708 RVA: 0x00377978 File Offset: 0x00375B78
		public string DisplayName
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x1700156F RID: 5487
		// (get) Token: 0x06008795 RID: 34709 RVA: 0x00377980 File Offset: 0x00375B80
		public string KeyDisplayLabel
		{
			get
			{
				return this.keyDisplayLabel;
			}
		}

		// Token: 0x17001570 RID: 5488
		// (get) Token: 0x06008796 RID: 34710 RVA: 0x00377988 File Offset: 0x00375B88
		public bool RequiresUncoveredMouth
		{
			get
			{
				return this.requiresUncoveredMouth;
			}
		}

		// Token: 0x06008797 RID: 34711 RVA: 0x00377990 File Offset: 0x00375B90
		public bool TryAllowClothing()
		{
			if (!this.requiresUncoveredMouth)
			{
				return UncoveredBottomRequirement.TryAllow();
			}
			if (!PlayerExposureState.IsMouthPhysicallyBlocked)
			{
				return true;
			}
			BlockedActionFeedback.Show("Remove the item covering your mouth first");
			return false;
		}

		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x06008798 RID: 34712 RVA: 0x003779B4 File Offset: 0x00375BB4
		public PlacementSurfaceType RequiredPlacementType
		{
			get
			{
				return this.requiredPlacementType;
			}
		}

		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x06008799 RID: 34713 RVA: 0x003779BC File Offset: 0x00375BBC
		public PlayerActionEntry PlayerActionEntry
		{
			get
			{
				return this.playerActionEntry;
			}
		}

		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x0600879A RID: 34714 RVA: 0x003779C4 File Offset: 0x00375BC4
		public bool IsLoop
		{
			get
			{
				return this.isLoop;
			}
		}

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x0600879B RID: 34715 RVA: 0x003779CC File Offset: 0x00375BCC
		public DetectionCameraPresetCollectionSO CameraPresets
		{
			get
			{
				return this.cameraPresets;
			}
		}

		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x0600879C RID: 34716 RVA: 0x003779D4 File Offset: 0x00375BD4
		public Vector3 PlayerPositionOffset
		{
			get
			{
				return this.playerPositionOffset;
			}
		}

		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x0600879D RID: 34717 RVA: 0x003779DC File Offset: 0x00375BDC
		public Vector3 PlayerRotationOffset
		{
			get
			{
				return this.playerRotationOffset;
			}
		}

		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x0600879E RID: 34718 RVA: 0x003779E4 File Offset: 0x00375BE4
		public Vector3 DildoPositionOffset
		{
			get
			{
				return this.dildoPositionOffset;
			}
		}

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x0600879F RID: 34719 RVA: 0x003779EC File Offset: 0x00375BEC
		public float AlignmentDuration
		{
			get
			{
				return this.alignmentDuration;
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x060087A0 RID: 34720 RVA: 0x003779F4 File Offset: 0x00375BF4
		public bool DebugLogs
		{
			get
			{
				return this.debugLogs;
			}
		}

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x060087A1 RID: 34721 RVA: 0x003779FC File Offset: 0x00375BFC
		public string dildoAnimationParameter_prop
		{
			get
			{
				return this.dildoAnimationParameter;
			}
		}

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x060087A2 RID: 34722 RVA: 0x003779FC File Offset: 0x00375BFC
		public string DildoAnimationParameter
		{
			get
			{
				return this.dildoAnimationParameter;
			}
		}

		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x060087A3 RID: 34723 RVA: 0x00377A04 File Offset: 0x00375C04
		public bool UseTriggerForDildo
		{
			get
			{
				return this.useTriggerForDildo;
			}
		}

		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x060087A4 RID: 34724 RVA: 0x00377A0C File Offset: 0x00375C0C
		public int DildoActionId
		{
			get
			{
				return this.dildoActionId;
			}
		}

		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x060087A5 RID: 34725 RVA: 0x00377A14 File Offset: 0x00375C14
		public float Excitement
		{
			get
			{
				return this.excitement;
			}
		}

		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x060087A6 RID: 34726 RVA: 0x00377A1C File Offset: 0x00375C1C
		public bool DisableFaceLayer
		{
			get
			{
				return this.disableFaceLayer;
			}
		}

		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x060087A7 RID: 34727 RVA: 0x00377A24 File Offset: 0x00375C24
		public int FaceExpressionIndex
		{
			get
			{
				return this.faceExpressionIndex;
			}
		}

		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x060087A8 RID: 34728 RVA: 0x00377A2C File Offset: 0x00375C2C
		public AudioClip[] InteractionSoundClips
		{
			get
			{
				return this.interactionSoundClips;
			}
		}

		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x060087A9 RID: 34729 RVA: 0x00377A34 File Offset: 0x00375C34
		public float InteractionVolume
		{
			get
			{
				return this.interactionVolume;
			}
		}

		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x060087AA RID: 34730 RVA: 0x00377A3C File Offset: 0x00375C3C
		public bool UsesMasturbationVoiceAndMeter
		{
			get
			{
				return this.enableMasturbationVoiceAndMeter;
			}
		}

		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x060087AB RID: 34731 RVA: 0x00377A44 File Offset: 0x00375C44
		public EmoteDefinitionSO MasturbationSettings
		{
			get
			{
				return this.masturbationSettings;
			}
		}

		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x060087AC RID: 34732 RVA: 0x00377A4C File Offset: 0x00375C4C
		public string OrgasmAnimationState
		{
			get
			{
				return this.orgasmAnimationState;
			}
		}

		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x060087AD RID: 34733 RVA: 0x00377A54 File Offset: 0x00375C54
		public AnimationClip OrgasmAnimationClip
		{
			get
			{
				return this.orgasmAnimationClip;
			}
		}

		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x060087AE RID: 34734 RVA: 0x00377A5C File Offset: 0x00375C5C
		public float OrgasmEffectDelay
		{
			get
			{
				return this.orgasmEffectDelay;
			}
		}

		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x060087AF RID: 34735 RVA: 0x00377A64 File Offset: 0x00375C64
		public float MinPlaybackSpeed
		{
			get
			{
				if (!(this.masturbationSettings != null))
				{
					return 0.6f;
				}
				return this.masturbationSettings.MinClipSpeed;
			}
		}

		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x060087B0 RID: 34736 RVA: 0x00377A85 File Offset: 0x00375C85
		public float MaxPlaybackSpeed
		{
			get
			{
				if (!(this.masturbationSettings != null))
				{
					return 2f;
				}
				return this.masturbationSettings.MaxClipSpeed;
			}
		}

		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x060087B1 RID: 34737 RVA: 0x00377AA6 File Offset: 0x00375CA6
		public float DefaultPlaybackSpeed
		{
			get
			{
				if (!(this.masturbationSettings != null))
				{
					return 1f;
				}
				return this.masturbationSettings.DefaultClipSpeed;
			}
		}

		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x060087B2 RID: 34738 RVA: 0x00377AC7 File Offset: 0x00375CC7
		public AudioClip[] PleasureSoundClips
		{
			get
			{
				return this.pleasureSoundClips;
			}
		}

		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x060087B3 RID: 34739 RVA: 0x00377ACF File Offset: 0x00375CCF
		public float PleasureVolume
		{
			get
			{
				return this.pleasureVolume;
			}
		}

		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x060087B4 RID: 34740 RVA: 0x00377AD7 File Offset: 0x00375CD7
		public float PleasureSoundDelay
		{
			get
			{
				return this.pleasureSoundDelay;
			}
		}

		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x060087B5 RID: 34741 RVA: 0x00377ADF File Offset: 0x00375CDF
		public float PleasureLoopDelay
		{
			get
			{
				return this.pleasureLoopDelay;
			}
		}

		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x060087B6 RID: 34742 RVA: 0x00377AE7 File Offset: 0x00375CE7
		public float OrgasmProgressPerSecond
		{
			get
			{
				if (!(this.masturbationSettings != null))
				{
					return 0.02f;
				}
				return Mathf.Max(0f, this.masturbationSettings.OrgasmProgressPerSecondAtBaseSpeed);
			}
		}

		// Token: 0x060087B7 RID: 34743 RVA: 0x00377B12 File Offset: 0x00375D12
		public bool IsCompatibleWith(PlacementSurfaceType surfaceType)
		{
			return this.requiredPlacementType == surfaceType;
		}

		// Token: 0x060087B8 RID: 34744 RVA: 0x00377B20 File Offset: 0x00375D20
		private void OnValidate()
		{
			this.alignmentDuration = Mathf.Clamp(this.alignmentDuration, 0.05f, 1f);
			if (string.IsNullOrEmpty(this.actionId) && !string.IsNullOrEmpty(base.name))
			{
				this.actionId = base.name;
			}
		}

		// Token: 0x04008A2A RID: 35370
		[Header("Identity")]
		[Tooltip("Unique identifier used to look up and dispatch the action")]
		[SerializeField]
		private string actionId;

		// Token: 0x04008A2B RID: 35371
		[Tooltip("Label shown in the interaction HUD")]
		[SerializeField]
		private string displayName;

		// Token: 0x04008A2C RID: 35372
		[Tooltip("Key label shown in the HUD action list (e.g. '1', '2', 'F')")]
		[SerializeField]
		private string keyDisplayLabel = "F";

		// Token: 0x04008A2D RID: 35373
		[Header("Physical clothing requirement")]
		[Tooltip("Only a physical mouth obstruction blocks this action. Other actions require an uncovered lower body.")]
		[SerializeField]
		private bool requiresUncoveredMouth;

		// Token: 0x04008A2E RID: 35374
		[Header("Placement Filter")]
		[Tooltip("Which placement surface type allows this action. Set to Floor or Wall to restrict.")]
		[SerializeField]
		private PlacementSurfaceType requiredPlacementType = PlacementSurfaceType.Floor;

		// Token: 0x04008A2F RID: 35375
		[Header("Invector Animation")]
		[Tooltip("PlayerActionEntry referencing the vTriggerGenericAction prefab for this animation")]
		[SerializeField]
		private PlayerActionEntry playerActionEntry;

		// Token: 0x04008A30 RID: 35376
		[Tooltip("If true, the animation loops and requires manual exit")]
		[SerializeField]
		private bool isLoop;

		// Token: 0x04008A31 RID: 35377
		[Header("Camera Configuration")]
		[Tooltip("Optional camera presets to use during this action. If null, uses director defaults.")]
		[SerializeField]
		private DetectionCameraPresetCollectionSO cameraPresets;

		// Token: 0x04008A32 RID: 35378
		[Header("Player Alignment")]
		[Tooltip("Position offset from the placed dildo's Transform where the player should stand. Local space of the dildo.")]
		[SerializeField]
		private Vector3 playerPositionOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x04008A33 RID: 35379
		[Tooltip("Euler angle offset applied to the dildo's rotation for the player's facing direction")]
		[SerializeField]
		private Vector3 playerRotationOffset = new Vector3(0f, 180f, 0f);

		// Token: 0x04008A34 RID: 35380
		[Header("Dildo Offset (Optional)")]
		[Tooltip("Offset applied to the dildo's position relative to its placement point during the action.")]
		[SerializeField]
		private Vector3 dildoPositionOffset = Vector3.zero;

		// Token: 0x04008A35 RID: 35381
		[Tooltip("Duration of the smooth alignment tween before the animation starts (seconds)")]
		[SerializeField]
		[Range(0.05f, 1f)]
		private float alignmentDuration = 0.3f;

		// Token: 0x04008A36 RID: 35382
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008A37 RID: 35383
		[Header("Dildo Animation (Optional)")]
		[Tooltip("The name of the animation state or trigger to set on the Dildo's Animator (if it has one).")]
		[SerializeField]
		private string dildoAnimationParameter = "";

		// Token: 0x04008A38 RID: 35384
		[Tooltip("If true, sets a trigger. If false, sets an Integer 'ActionID'.")]
		[SerializeField]
		private bool useTriggerForDildo = true;

		// Token: 0x04008A39 RID: 35385
		[Tooltip("The Integer value to set if useTriggerForDildo is false.")]
		[SerializeField]
		private int dildoActionId;

		// Token: 0x04008A3A RID: 35386
		[Tooltip("The 'Excitement' value to set on the Dildo Animator (0 = Base, 1 = _ORG)")]
		[SerializeField]
		[Range(0f, 1f)]
		private float excitement;

		// Token: 0x04008A3B RID: 35387
		[Header("Face Layer Control")]
		[Tooltip("If true, disables the 'Face' animator layer during this action. Set to false to keep facial animations active.")]
		[SerializeField]
		private bool disableFaceLayer;

		// Token: 0x04008A3C RID: 35388
		[Tooltip("Face expression index to set on the player animator during this action. Only used when disableFaceLayer is false. Set to -1 to disable.")]
		[SerializeField]
		private int faceExpressionIndex = -1;

		// Token: 0x04008A3D RID: 35389
		[Header("Audio - Interaction Sounds (Animation Events)")]
		[Tooltip("Audio clips played via AnimationEvents during the animation (e.g., impact, movement sounds). Add AE_DildoInteraction() events to your animation clips.")]
		[SerializeField]
		private AudioClip[] interactionSoundClips;

		// Token: 0x04008A3E RID: 35390
		[Tooltip("Base volume for interaction sounds")]
		[SerializeField]
		[Range(0f, 1f)]
		private float interactionVolume = 0.7f;

		// Token: 0x04008A3F RID: 35391
		[Header("Audio - Pleasure Sounds (Code Triggered)")]
		[Tooltip("Enables masturbation voices, ending vocals and shared meter progress for this action. Disable for actions that should play only their contact sounds.")]
		[SerializeField]
		private bool enableMasturbationVoiceAndMeter = true;

		// Token: 0x04008A40 RID: 35392
		[Tooltip("Shared settings for meter progress, playback speed limits and completion effects.")]
		[SerializeField]
		private EmoteDefinitionSO masturbationSettings;

		// Token: 0x04008A41 RID: 35393
		[Header("Action completion")]
		[Tooltip("Existing actor state that plays this action's authored completion clip.")]
		[SerializeField]
		private string orgasmAnimationState;

		// Token: 0x04008A42 RID: 35394
		[SerializeField]
		private AnimationClip orgasmAnimationClip;

		// Token: 0x04008A43 RID: 35395
		[Tooltip("Seconds into the completion clip at which the existing emote effect is activated.")]
		[SerializeField]
		[Min(0f)]
		private float orgasmEffectDelay = 0.5f;

		// Token: 0x04008A44 RID: 35396
		[Tooltip("Audio clips for player pleasure sounds (moans, etc.). Played automatically during action.")]
		[SerializeField]
		private AudioClip[] pleasureSoundClips;

		// Token: 0x04008A45 RID: 35397
		[Tooltip("Base volume for pleasure sounds (0-1)")]
		[SerializeField]
		[Range(0f, 1f)]
		private float pleasureVolume = 0.6f;

		// Token: 0x04008A46 RID: 35398
		[Tooltip("Delay before playing first pleasure sound after action starts (seconds)")]
		[SerializeField]
		[Range(0f, 5f)]
		private float pleasureSoundDelay = 0.2f;

		// Token: 0x04008A47 RID: 35399
		[Tooltip("Fixed delay between pleasure sound loops (seconds). Set to 0 to disable looping.")]
		[SerializeField]
		[Range(0f, 30f)]
		private float pleasureLoopDelay = 5f;
	}
}
