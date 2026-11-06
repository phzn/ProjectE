using System;
using System.Collections.Generic;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001284 RID: 4740
	[CreateAssetMenu(fileName = "New Emote Definition", menuName = "Detection/Emote Wheel/Emote Definition")]
	public class EmoteDefinitionSO : ScriptableObject
	{
		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x0600930C RID: 37644 RVA: 0x003C6097 File Offset: 0x003C4297
		public bool IsBlockedByClothing
		{
			get
			{
				return (this.RequiresUncoveredBottom || this.IsMasturbationEmote) && PlayerExposureState.IsBottomPhysicallyCovered;
			}
		}

		// Token: 0x0600930D RID: 37645 RVA: 0x003C60B0 File Offset: 0x003C42B0
		public vTriggerGenericAction ResolveTriggerPrefab()
		{
			if (this.TriggerPrefab == null)
			{
				return null;
			}
			vTriggerGenericAction vTriggerGenericAction = this.TriggerPrefab.GetComponent<vTriggerGenericAction>();
			if (vTriggerGenericAction == null)
			{
				vTriggerGenericAction = this.TriggerPrefab.GetComponentInChildren<vTriggerGenericAction>(true);
			}
			if (vTriggerGenericAction == null)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[EmoteDefinitionSO] '",
					base.name,
					"' trigger prefab '",
					this.TriggerPrefab.name,
					"' has no vTriggerGenericAction component."
				}));
			}
			return vTriggerGenericAction;
		}

		// Token: 0x040097C2 RID: 38850
		[Header("Identity")]
		[Tooltip("Unique identifier for this emote")]
		public string EmoteId;

		// Token: 0x040097C3 RID: 38851
		[Tooltip("Display name shown in the wheel (use TMP rich text if needed)")]
		public string DisplayName;

		// Token: 0x040097C4 RID: 38852
		[Tooltip("Optional icon displayed alongside the name")]
		public Sprite Icon;

		// Token: 0x040097C5 RID: 38853
		[Header("Action")]
		[Tooltip("Prefab used for this emote (must contain vTriggerGenericAction on root or children)")]
		public GameObject TriggerPrefab;

		// Token: 0x040097C6 RID: 38854
		[Tooltip("Optional gameplay action used by the detection wheel instead of playing an emote.")]
		public EmoteSpecialAction SpecialAction;

		// Token: 0x040097C7 RID: 38855
		[Tooltip("How long the emote animation lasts before returning to gameplay")]
		public float Duration = 3f;

		// Token: 0x040097C8 RID: 38856
		[Header("Playback Mode")]
		[Tooltip("If true, the emote loops until the player ends it via the Director's global SkipInput. If false, plays once but holds the final pose until skip.")]
		public bool IsLoopEmote;

		// Token: 0x040097C9 RID: 38857
		[Header("Gameplay Emote Settings")]
		[Tooltip("Whether the player can move during this emote")]
		public EmoteMovementMode MovementMode;

		// Token: 0x040097CA RID: 38858
		[Tooltip("How the emote ends: timed (auto after Duration) or manual (player presses end key)")]
		public EmoteEndBehavior EndBehavior;

		// Token: 0x040097CB RID: 38859
		[Tooltip("Se true, mentre questa emote e' in esecuzione Ciel usa il respiro 'lewd' in loop al posto del respiro ambient normale (stesso effetto del flash). Usato dalle emote dello stand interattivo. Il respiro normale viene silenziato per la durata dell'emote.")]
		public bool UseLewdBreathing;

		// Token: 0x040097CC RID: 38860
		[Header("POI Restriction")]
		[Tooltip("Optional PoiEmoteProfileSO.PoiId required to use this emote. Leave empty to allow it everywhere.")]
		public string RequiredPoiId = string.Empty;

		// Token: 0x040097CD RID: 38861
		[Header("Emote Locomotion")]
		[Tooltip("If true the emote clip drives character motion via animator root motion (clip must have baked root motion). Applies to BOTH MovementMode values: • Moving: locomotion clips move the player at their baked speed; if false, player walks/runs at MovingNoRootMotionSpeed via Invector's standard non-root-motion locomotion. • Static: input stays locked, but the emote clip can still translate/rotate the character via root motion (useful for one-shot animations that displace the player).")]
		public bool MovingUseRootMotion = true;

		// Token: 0x040097CE RID: 38862
		[Tooltip("Moving emotes only, used when MovingUseRootMotion = false. Movement speed (m/s) applied via Invector's free-locomotion speed override while the emote is playing.")]
		[Min(0f)]
		public float MovingNoRootMotionSpeed = 1.5f;

		// Token: 0x040097CF RID: 38863
		[Header("Props & Effects")]
		[Tooltip("Props/effects to spawn and sync with this emote")]
		public List<EmotePropBinding> PropBindings = new List<EmotePropBinding>();

		// Token: 0x040097D0 RID: 38864
		[Header("Audio")]
		[Tooltip("Optional audio clip to play when this emote starts")]
		public AudioClip EmoteAudioClip;

		// Token: 0x040097D1 RID: 38865
		[Tooltip("Volume for the emote audio (0-1)")]
		[Range(0f, 1f)]
		public float AudioVolume = 1f;

		// Token: 0x040097D2 RID: 38866
		[Header("Detection Camera Presets")]
		[Tooltip("Optional camera presets used during the emote playback phase of detection. If null, the default emote camera from ExhibitionEventDirector is used.")]
		public DetectionCameraPresetCollectionSO DetectionCameraPresets;

		// Token: 0x040097D3 RID: 38867
		[Header("Category")]
		[Tooltip("Which choice path this emote belongs to (used by Detection system)")]
		public ChoiceCategory Category;

		// Token: 0x040097D4 RID: 38868
		[Header("Unlock")]
		[Tooltip("If true, the emote is available from the start")]
		public bool UnlockedByDefault = true;

		// Token: 0x040097D5 RID: 38869
		[Tooltip("Optional unlock condition key (for save system integration)")]
		public string UnlockKey;

		// Token: 0x040097D6 RID: 38870
		[Header("Futanari Gating")]
		[Tooltip("Disponibilita' dell'emote in funzione dello stato Futanari Mode del player. Always = sempre, FutanariOnly = solo se ON, NoFutanari = solo se OFF.")]
		public FutanariRequirement FutanariRequirement;

		// Token: 0x040097D7 RID: 38871
		[Tooltip("Se true, la fisica MagicaCloth della Futanari Mode resta attiva mentre l'emote suona. Se false, viene messa in pausa per evitare wobble indesiderati su animazioni precise (es. JerkOff). Ignorato se Futanari Mode e' OFF.")]
		public bool UseMagicaClothDuringEmote = true;

		// Token: 0x040097D8 RID: 38872
		[Header("Clothing requirements")]
		[Tooltip("Requires the groin to be physically uncovered before selection or playback. Panties, trousers, shorts and other covering garments block this emote.")]
		public bool RequiresUncoveredBottom;

		// Token: 0x040097D9 RID: 38873
		[Header("Masturbation Mechanic")]
		[Tooltip("Se true, l'emote partecipa al sistema di masturbazione: fa salire la barra dell'orgasmo, abilita il controllo player della clip-speed via Ctrl+MouseWheel, e a barra piena triggera la transizione all'animator state di ending (param OrgasmEnding).")]
		public bool IsMasturbationEmote;

		// Token: 0x040097DA RID: 38874
		[Tooltip("Quanto (0..1 della barra) l'emote aggiunge al meter ogni secondo a clip-speed 1.0x. Il progress reale e' moltiplicato per la clip-speed corrente.")]
		[Min(0f)]
		public float OrgasmProgressPerSecondAtBaseSpeed = 0.1f;

		// Token: 0x040097DB RID: 38875
		[Tooltip("Velocita' minima della clip selezionabile dal player (moltiplicatore Animator.SetFloat).")]
		[Min(0.05f)]
		public float MinClipSpeed = 0.5f;

		// Token: 0x040097DC RID: 38876
		[Tooltip("Velocita' massima della clip selezionabile dal player.")]
		[Min(0.05f)]
		public float MaxClipSpeed = 2f;

		// Token: 0x040097DD RID: 38877
		[Tooltip("Velocita' iniziale applicata quando l'emote parte. Sempre reset a questo valore.")]
		[Min(0.05f)]
		public float DefaultClipSpeed = 1f;

		// Token: 0x040097DE RID: 38878
		[Tooltip("Icona alternativa (es. con cuoricino badge overlay) usata nella rebind wheel per distinguere visivamente le emote di masturbazione. Se null, ripiega su Icon.")]
		public Sprite IconWithBadge;
	}
}
