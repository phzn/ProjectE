using System;
using System.Collections.Generic;
using Features.Inventory.Wearable;
using Features.PlayerEmotions;
using UnityEngine;

namespace Features.Rank
{
	// Token: 0x02000DA4 RID: 3492
	[CreateAssetMenu(fileName = "Rank_New", menuName = "Rank/Rank Definition")]
	public class RankDefinitionSO : ScriptableObject
	{
		// Token: 0x04006940 RID: 26944
		[Header("Identita'")]
		[Tooltip("Id stabile del rank (es. 'shy', 'curious'). Usato per log/debug, non per il save.")]
		public string rankId;

		// Token: 0x04006941 RID: 26945
		[Tooltip("Nome mostrato in HUD/recap.")]
		public string displayName;

		// Token: 0x04006942 RID: 26946
		public Color badgeColor = Color.white;

		// Token: 0x04006943 RID: 26947
		[Tooltip("Opzionale: icona badge per HUD/recap.")]
		public Sprite badgeIcon;

		// Token: 0x04006944 RID: 26948
		[Header("Presentazione (CielOS Rank app)")]
		[Tooltip("Riga flavour mostrata sotto il nome del rank nella Rank app. Solo testo player-facing.")]
		[TextArea]
		public string tagline;

		// Token: 0x04006945 RID: 26949
		[Tooltip("Bullet list player-facing di cosa il rank permette (outfit, azioni, emote, encounter). Mostrata nella card 'What you can do' della Rank app. NIENTE dettagli implementativi.")]
		[TextArea]
		public string[] perkLines;

		// Token: 0x04006946 RID: 26950
		[Header("Progressione (D5)")]
		[Tooltip("Livello minimo per entrare in questo rank (condizione a di D5). 0/10/20/30/40/50.")]
		public int minLevel;

		// Token: 0x04006947 RID: 26951
		[Tooltip("Picco-distanza (m) richiesto in UNA run per la promozione A questo rank (condizione b di D5).")]
		public float promotionRunDistance;

		// Token: 0x04006948 RID: 26952
		[Header("Loss (D4/D9)")]
		[Tooltip("Encounter NPC persi tollerati in una run prima del game over (PerpRun). 1/1/1/2/4.")]
		public int maxLossesPerRun = 1;

		// Token: 0x04006949 RID: 26953
		[Header("Notte (D3)")]
		[Tooltip("true = Azure resta di notte per tutta la run Classic (SHY fino a HORNY). False da HORNY SLUT; Free usa le proprie impostazioni.")]
		public bool nightOnly = true;

		// Token: 0x0400694A RID: 26954
		[Header("Outfit di partenza (D7)")]
		[Tooltip("Capi indossati allo start run. SHY: ClassicPanties+Top2; CURIOUS/AROUSED/HORNY SLUT: ExposeBra+ExposePanties; HORNY: Towel.")]
		public List<WearableItemSO> startOutfitItems = new List<WearableItemSO>();

		// Token: 0x0400694B RID: 26955
		[Tooltip("true = l'outfit rank sostituisce la customizzazione del player. false riservato a modalita' future (sandbox).")]
		public bool startOutfitReplacesCustom = true;

		// Token: 0x0400694C RID: 26956
		[Header("Permessi azioni (tabella §1)")]
		[Tooltip("Aperture bra/underwear (da CURIOUS).")]
		public bool allowLingerieOpenAnimations;

		// Token: 0x0400694D RID: 26957
		[Tooltip("Slip/drop del top/bra (da AROUSED).")]
		public bool allowBraRemoval;

		// Token: 0x0400694E RID: 26958
		[Tooltip("Flash arms/full (da HORNY).")]
		public bool allowFlash;

		// Token: 0x0400694F RID: 26959
		[Tooltip("Drop di tutto (HORNY towel; HORNY SLUT tutto).")]
		public bool allowFullUndress;

		// Token: 0x04006950 RID: 26960
		[Tooltip("Emote wheel (da HORNY).")]
		public bool allowEmoteWheel;

		// Token: 0x04006951 RID: 26961
		[Tooltip("Q/Z emote pages available at this rank. 0 = locked, -1 = all pages.")]
		public int unlockedEmoteWheelPages;

		// Token: 0x04006952 RID: 26962
		[Tooltip("Emote di masturbation dentro la wheel (da HORNY).")]
		public bool allowMasturbationEmotes;

		// Token: 0x04006953 RID: 26963
		[Tooltip("Stance switch col Tab (D8): true SOLO su Certified Exhibitionist e Admin.")]
		public bool allowStanceSwitch;

		// Token: 0x04006954 RID: 26964
		[Tooltip("Cover pose attivabili (Y/Q). false SOLO a SHY; true da CURIOUS in su (2.2-G).")]
		public bool allowCoverPose = true;

		// Token: 0x04006955 RID: 26965
		[Header("Facce")]
		[Tooltip("Stance di default all'inizio della run. Embarassed (SHY/CURIOUS) / Confident (da AROUSED).")]
		public EmotionStance defaultStance = EmotionStance.Embarassed;

		// Token: 0x04006956 RID: 26966
		[Tooltip("Come reagisce la faccia al superamento del 50% detection.")]
		public DetectionFaceReaction detectionReaction;

		// Token: 0x04006957 RID: 26967
		[Tooltip("Durata (s) della reazione per EmbSupremeTimed/HornySupreme (D6: range 10-15).")]
		public float detectionReactionSeconds = 12f;

		// Token: 0x04006958 RID: 26968
		[Tooltip("Curious: continuous seconds below 25% attention before detection releases EMB.")]
		[Min(0f)]
		public float detectionEscapeRecoverySeconds = 3f;

		// Token: 0x04006959 RID: 26969
		[Tooltip("HORNY+: durante i flash la faccia va CONFIDENT SUPREME.")]
		public bool confidentSupremeDuringFlash;

		// Token: 0x0400695A RID: 26970
		[Tooltip("Seconds to keep the flash expression after closing. Held-open flashes have no time limit.")]
		[Min(0f)]
		public float flashSupremeCooldownSeconds = 3f;

		// Token: 0x0400695B RID: 26971
		[Tooltip("HORNY: EMB fisso finche' il towel resta a terra (droppato).")]
		public bool embWhileTowelDropped;

		// Token: 0x0400695C RID: 26972
		[Tooltip("CURIOUS+: faccia Confident durante le aperture lingerie (gia' fatto da ExposeLingerieFaceController, qui e' il flag di rank).")]
		public bool confidentDuringLingerieOpen;

		// Token: 0x0400695D RID: 26973
		[Tooltip("AROUSED (3.4-C): togliere il bra/top (nessun capo INDOSSATO che copre fisicamente il top) → lock supreme positivo (+100, faccia 3). L'apertura NON basta: serve lo slip/drop vero.")]
		public bool supremeWhileTopBare;

		// Token: 0x0400695E RID: 26974
		[Tooltip("HORNY SLUT+ (3.4-F-bis): SOLO con ENTRAMBI i pezzi tolti (niente top E niente bottom fisicamente coperti) → lock supreme positivo (+100). Un pezzo solo = nessun lock.")]
		public bool supremeWhileFullyBare;

		// Token: 0x0400695F RID: 26975
		[Tooltip("3.4-H: reaction Photo/Video stance-aware — il pre-detection (+2/s) segue la stance (EMB → sale verso EmbSupreme). true SOLO su AdminRank e Certified Exhibitionist.")]
		public bool preDetectionFollowsStance;

		// Token: 0x04006960 RID: 26976
		[Tooltip("Debug (AdminRank): spegne il decay del balance per tutta la run — l'intensita'/lo stato restano dove il designer li mette.")]
		public bool disableEmotionDecay;

		// Token: 0x04006961 RID: 26977
		[Header("HUD (3.2-A)")]
		[Tooltip("Mostra l'EmotionBadge in HUD. Si sblocca con Tab a Certified Exhibitionist.")]
		public bool showEmotionBadge;

		// Token: 0x04006962 RID: 26978
		[Header("Scelte encounter")]
		public EncounterChoiceMode choiceMode;

		// Token: 0x04006963 RID: 26979
		[HideInInspector]
		public bool allowEncounterEscape;

		// Token: 0x04006964 RID: 26980
		[Header("Escalation loss (D9)")]
		[Tooltip("HORNY SLUT+: ogni encounter perso alza la pressione polizia/reporter via SuspicionManager. false sotto HORNY SLUT.")]
		public bool escalatePoliceOnLoss;

		// Token: 0x04006965 RID: 26981
		[Header("Presenze in strada (2.3-A)")]
		[Tooltip("Polizia in giro. false su SHY/CURIOUS/AROUSED, true da HORNY. Il rank puo' solo SPEGNERE cio' che il SceneProfile ha abilitato.")]
		public bool enablePolice;

		// Token: 0x04006966 RID: 26982
		[Tooltip("Legacy serialized value, no longer used for reporter spawning. SceneProfile enables reporters; suspicion level 1 requests one troupe and level 3 requests two.")]
		public bool enableReporters;

		// Token: 0x04006967 RID: 26983
		[Header("Telefono (2.3-D)")]
		[Tooltip("Apertura smartphone (tasto P). Disponibile fin da SHY; le singole app hanno il proprio gate.")]
		public bool allowSmartphone = true;

		// Token: 0x04006968 RID: 26984
		[Tooltip("CielGram is available from SHY. Face blur is mandatory until Certified Exhibitionist.")]
		public bool allowCielGram = true;

		// Token: 0x04006969 RID: 26985
		[Header("Sblocchi (solo rank finale = true)")]
		public bool unlocksMoodSetting;

		// Token: 0x0400696A RID: 26986
		public bool unlocksRankMode;

		// Token: 0x0400696B RID: 26987
		public bool unlocksSandbox;

		// Token: 0x0400696C RID: 26988
		public bool unlocksGodmode;

		// Token: 0x0400696D RID: 26989
		public bool unlocksWindowFlashing;
	}
}
