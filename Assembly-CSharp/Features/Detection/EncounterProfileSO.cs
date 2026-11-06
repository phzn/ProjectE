using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.Detection
{
	// Token: 0x0200128D RID: 4749
	[CreateAssetMenu(fileName = "EncounterProfile_", menuName = "Detection/Encounter Profile")]
	public class EncounterProfileSO : ScriptableObject
	{
		// Token: 0x04009846 RID: 38982
		[Header("Identita' attore")]
		[Tooltip("Tipo dell'attore/initiator di questa scena. Usato per personalizzare la reazione vocale di Ciel (CielDetectionVoiceSet sceglie i versi in base a chi cattura/interviene). Gli encounter senza profilo (NPC di strada via TriggerDetection) valgono come Generic.")]
		public DetectionActorType actorType;

		// Token: 0x04009847 RID: 38983
		[Header("Posizionamento attori aggiuntivi")]
		[Tooltip("Slot di posizione/comportamento per ciascun additionalActor di EncounterRequest.\nactorSlots[0] → additionalActors[0] (es. cameraman)\nactorSlots[1] → additionalActors[1] (es. tecnico)")]
		public List<EncounterProfileSO.ActorPositionSlot> actorSlots = new List<EncounterProfileSO.ActorPositionSlot>();

		// Token: 0x04009848 RID: 38984
		[Header("Reazione")]
		[Tooltip("Profilo di reazione che sovrascrive quello dell'NPC initiator durante la Phase 9 (NPC Reacts).\nNull = usa il NPCReactionProfileSO del NPCSenses (comportamento standard).")]
		public NPCReactionProfileSO reactionProfileOverride;

		// Token: 0x04009849 RID: 38985
		[Tooltip("Se true, il Director salta interamente la Phase 9 (NPC reaction animation + attesa).\nCaso d'uso: cattura Polizia → niente emote di reazione iniziale, si passa direttamente all'idle/dialogo.")]
		public bool skipNPCReaction;

		// Token: 0x0400984A RID: 38986
		[Header("Dialogo")]
		[Tooltip("Se true, il primo dialogo NPC parte in parallelo alla reaction (Phase 9) invece che dopo.\nDefault false = dialogo parte dopo la fine della reaction (comportamento standard).")]
		public bool startDialogueWithReaction;

		// Token: 0x0400984B RID: 38987
		[Tooltip("Profilo di dialogo che sovrascrive quello basato sulla personalità del NPC.\nNull = usa il NPCDialogueProfileSO del NPCDialogueHolder (comportamento standard).")]
		public NPCDialogueProfileSO dialogueOverride;

		// Token: 0x0400984C RID: 38988
		[Tooltip("Pack di dialogo multi-speaker (es. troupe reporter).\nSe impostato, ha priorità su dialogueOverride per le fasi configurate.\nNull = usa il flow standard NPCDialogueProfileSO.")]
		public DialoguePackSO multiActorDialogue;

		// Token: 0x0400984D RID: 38989
		[Header("Camera")]
		[Tooltip("Preset camera per la fase di reazione NPC.\nNull = usa NpcPhaseCameraPresets del director (default).")]
		public DetectionCameraPresetCollectionSO npcCameraPresetsOverride;

		// Token: 0x0400984E RID: 38990
		[Header("Overlay UI")]
		[Tooltip("Prefab UI istanziato per tutta la durata della scena (es. cornice TV news, lower third).\nNull = nessun overlay.")]
		public GameObject uiOverlayPrefab;

		// Token: 0x0400984F RID: 38991
		[Header("Post Process")]
		[Tooltip("Volume Profile URP applicato globalmente durante la scena (es. LUT CRT/TV, grain).\nNull = nessun override post-process.")]
		public VolumeProfile postProcessOverride;

		// Token: 0x04009850 RID: 38992
		[Range(0f, 1f)]
		[Tooltip("Peso del volume post-process sovrapposto al profilo di gioco.")]
		public float postProcessWeight = 1f;

		// Token: 0x04009851 RID: 38993
		[Header("Choice override (cattura Polizia / encounters speciali)")]
		[Tooltip("Se true, il Director salta la Phase 13 (selezione categoria Shame/Expose) e apre direttamente l'EmoteWheel sulla categoria 'forcedEmoteCategory'. Caso d'uso: cattura Polizia → niente choice player, mostra subito le emote 'Police'.")]
		public bool skipCategoryChoice;

		// Token: 0x04009852 RID: 38994
		[Tooltip("Categoria forzata quando skipCategoryChoice e' true. L'EmoteWheel apre la lista corrispondente da EmoteCollectionSO (Police = PoliceEmotes).")]
		public ChoiceCategory forcedEmoteCategory = ChoiceCategory.Police;

		// Token: 0x04009853 RID: 38995
		[Tooltip("Se true, dopo la scelta dell'emote 'forcedEmotionImpactCategory' (sotto) viene usato AL POSTO della Category dell'emote per decidere quale impact emotion applicare (Shame/Expose/Flee). Default false: l'emote.Category guida l'impact (comportamento standard).\nCaso d'uso: cattura Polizia → le emote stanno in categoria 'Police' (per separare il wheel UI), ma il loro effetto sull'emotion balance del player deve essere quello Shame (Ciel viene catturata, prova vergogna). Senza override cadrebbero in 'Flee'.")]
		public bool overrideEmotionImpactCategory;

		// Token: 0x04009854 RID: 38996
		[Tooltip("Categoria forzata per il calcolo dell'emotion impact quando 'overrideEmotionImpactCategory' e' true. Esempio: 'Shame' per la cattura Polizia. Ignorato se override e' false.")]
		public ChoiceCategory forcedEmotionImpactCategory;

		// Token: 0x04009855 RID: 38997
		[Header("Emotion override (cattura Polizia)")]
		[Tooltip("Se true, all'inizio del flusso di detection (prima ancora della scelta emote) il Director forza EmotionBalance a 'forcedStartBalance' (Shame pieno → EmbSupreme) e poi CONGELA l'EmotionManager (SetFrozen) cosi' lo stato non cambia piu' per il resto dell'encounter. Faccia + layer Embarrassed diventano EmbSupreme coerentemente. Caso d'uso: cattura Polizia (anche da poliziotto singolo, senza taser) → Ciel in EmbSupreme bloccata fino al GameOver.")]
		public bool forceShameSupremeAndFreeze;

		// Token: 0x04009856 RID: 38998
		[Tooltip("Valore di EmotionBalance forzato all'inizio quando 'forceShameSupremeAndFreeze' e' true. -100 = Shame massimo (EmbSupreme).")]
		[Range(-100f, 100f)]
		public float forcedStartBalance = -100f;

		// Token: 0x04009857 RID: 38999
		[Header("Newspaper")]
		[Tooltip("Se true, durante l'esecuzione dell'emote il Director cattura una foto della scena (Camera.main → RenderTexture, niente UI) per il giornale post-GameOver. Caso d'uso: cattura Polizia. Richiede un NewspaperPhotoService in scena.")]
		public bool captureNewspaperPhoto;

		// Token: 0x04009858 RID: 39000
		[Header("Comportamento fine scena")]
		[Tooltip("Cosa succede agli attori dopo la chiusura della scena.")]
		public EncounterExitBehavior exitBehavior;

		// Token: 0x04009859 RID: 39001
		[Tooltip("Se true, il Director esegue 'onSceneEnded' DURANTE il fade-to-black (prima del fade-in), cosi' quando lo schermo torna visibile il successor state e' gia' attivo. Caso d'uso: cattura Polizia → BeginPerpWalk parte sotto fade nero, quando il fade-in completa il player e' gia' in stato perp walk con camera / animation handcuffs gia' settate. Senza questo flag c'e' un frame visibile con la camera 'reset' del Director prima che PerpWalk si attivi.")]
		public bool invokeOnSceneEndedDuringFade;

		// Token: 0x0200128E RID: 4750
		[Serializable]
		public class ActorPositionSlot
		{
			// Token: 0x0400985A RID: 39002
			[Tooltip("Offset posizione rispetto all'initiator, in spazio locale dell'initiator.\nX = destra, Y = su, Z = avanti.\nEs. cameraman: (1.8, 0, 0.4) → leggermente a destra e avanti.")]
			public Vector3 positionOffset = new Vector3(1.8f, 0f, 0.4f);

			// Token: 0x0400985B RID: 39003
			[Tooltip("Se true, l'attore viene ruotato per guardare il player.\nSe false, mantiene la propria rotazione corrente.")]
			public bool lookAtPlayer = true;

			// Token: 0x0400985C RID: 39004
			[Tooltip("Valore UpperBodyAction da impostare sull'attore durante la scena.\n0 = nessun override (usa lo stato corrente).")]
			public int upperBodyActionIndex;
		}
	}
}
