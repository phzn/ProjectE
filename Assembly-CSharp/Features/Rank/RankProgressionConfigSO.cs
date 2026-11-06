using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Rank
{
	// Token: 0x02000DAD RID: 3501
	[CreateAssetMenu(fileName = "RankConfig", menuName = "Rank/Rank Progression Config")]
	public class RankProgressionConfigSO : ScriptableObject
	{
		// Token: 0x06006712 RID: 26386 RVA: 0x00293D08 File Offset: 0x00291F08
		public float GetLevelCost(int fromLevel)
		{
			int num = Mathf.Clamp(fromLevel, 0, Mathf.Max(0, this.levelCostGrowthCap));
			return Mathf.Max(1f, this.baseLevelCost) + Mathf.Max(0f, this.levelCostIncrease) * (float)num;
		}

		// Token: 0x040069A9 RID: 27049
		[Header("Rank (ordine = progressione)")]
		[Tooltip("I 6 rank in ordine: Shy, Curious, Aroused, Horny, Horny Slut, Certified Exhibitionist (D10).")]
		public List<RankDefinitionSO> ranksInOrder = new List<RankDefinitionSO>();

		// Token: 0x040069AA RID: 27050
		[Header("Admin rank (debug)")]
		[Tooltip("Rank di debug per i video (AdminRank): NON fa parte della progressione. Quando attivo (Tools/Rank/Set Rank/Admin) CurrentRank ritorna questo asset senza toccare rankIndex; disattivandolo Ciel torna al rank di prima. Invisibile nella Rank app (itera solo ranksInOrder).")]
		public RankDefinitionSO adminRank;

		// Token: 0x040069AB RID: 27051
		[Header("Pesi XP (D1)")]
		[Tooltip("XP per metro del PICCO-distanza della run.")]
		public float xpPerMeterPeak = 1f;

		// Token: 0x040069AC RID: 27052
		[Tooltip("XP per ogni volta che il player e' stato visto.")]
		public float xpPerTimeSeen = 5f;

		// Token: 0x040069AD RID: 27053
		[Tooltip("XP per ogni emote VALIDA (fatta con almeno un NPC entro emoteValidityRadius).")]
		public float xpPerValidEmote = 20f;

		// Token: 0x040069AE RID: 27054
		[Tooltip("Conversione dei punti foto in XP prima del moltiplicatore distanza. 0.01 = 1000 score -> 10 XP.")]
		public float xpPerScorePoint = 0.01f;

		// Token: 0x040069AF RID: 27055
		[Tooltip("Ogni quanti metri di picco aumenta di 1 il moltiplicatore XP dello score.")]
		public float scoreDistanceStepMeters = 1000f;

		// Token: 0x040069B0 RID: 27056
		[Tooltip("Tetto del moltiplicatore distanza applicato alla componente score.")]
		[Min(1f)]
		public int maxScoreDistanceMultiplier = 6;

		// Token: 0x040069B1 RID: 27057
		[Tooltip("Raggio (m): un'emote conta come valida solo se c'e' almeno 1 NPC entro questa distanza quando parte.")]
		public float emoteValidityRadius = 15f;

		// Token: 0x040069B2 RID: 27058
		[Header("Costo livelli")]
		[Tooltip("XP richiesti per ogni livello, indipendentemente da livello e rank.")]
		[Min(1f)]
		public float baseLevelCost = 200f;

		// Token: 0x040069B3 RID: 27059
		[Tooltip("XP aggiuntivi per ogni livello: costo = base + incremento * livello corrente.")]
		[Min(0f)]
		public float levelCostIncrease;

		// Token: 0x040069B4 RID: 27060
		[Tooltip("Livello oltre cui il costo smette di crescere, mantenendo accessibile la progressione post-game.")]
		[Min(0f)]
		public int levelCostGrowthCap = 50;

		// Token: 0x040069B5 RID: 27061
		[Header("Notte (D3)")]
		[Tooltip("Ora notturna (0-24) mantenuta per tutta la run Classic quando il rank e' nightOnly.")]
		public float forcedNightHour = 22f;

		// Token: 0x040069B6 RID: 27062
		[Tooltip("Ora da cui applicare il minimo di leggibilita' notturna (intervallo che attraversa mezzanotte).")]
		[Range(0f, 24f)]
		public float nightVisibilityStartHour = 18.5f;

		// Token: 0x040069B7 RID: 27063
		[Tooltip("Ora a cui terminare il minimo di leggibilita' notturna.")]
		[Range(0f, 24f)]
		public float nightVisibilityEndHour = 6f;

		// Token: 0x040069B8 RID: 27064
		[Tooltip("Colore legacy mantenuto per compatibilita'. La resa notturna conserva ora i colori ambientali Azure.")]
		public Color nightAmbientFillColor = new Color(0.06f, 0.08f, 0.13f, 1f);

		// Token: 0x040069B9 RID: 27065
		[Tooltip("Intensita' minima del fill ambientale durante la notte. Non aggiunge luci o render pass.")]
		[Range(0f, 2f)]
		public float minimumNightAmbientIntensity = 0.55f;

		// Token: 0x040069BA RID: 27066
		[Tooltip("Minimo della luce direzionale Azure (luna) durante la notte. Riusa la main light esistente.")]
		[Range(0f, 2f)]
		public float minimumNightDirectionalIntensity = 0.25f;

		// Token: 0x040069BB RID: 27067
		[Header("Slip per-pezzo (Fase 2, step 4)")]
		[Tooltip("Substring (case-insensitive) che identificano un pezzo 'top/bra' fra i wornName del ClothSlipController. AROUSED puo' slippare SOLO questi pezzi. I wornName reali sono i nomi dei GameObject dei mesh indossati: VERIFICARE a runtime e allineare qui (es. 'bra', 'top', 'ExposeBra').")]
		public string[] topPieceNameMarkers = new string[] { "bra", "top" };

		// Token: 0x040069BC RID: 27068
		[Header("Escalation loss (D9)")]
		[Tooltip("Quanto suspicion aggiungere a ogni loss NPC quando il rank ha escalatePoliceOnLoss (HORNY SLUT+).")]
		public float suspicionPerLoss = 1f;

		// Token: 0x040069BD RID: 27069
		[Header("Feedback azione bloccata (2.2-D)")]
		[Tooltip("Messaggio centrale mostrato quando il rank nega un'azione. Override per-chiamata possibile.")]
		public string blockedActionMessage = "Action locked";

		// Token: 0x040069BE RID: 27070
		[Tooltip("Messaggio quando una masturbation e' negata perche' l'inguine e' fisicamente coperto (H3).")]
		public string masturbationBlockedMessage = "Take your clothes off first";

		// Token: 0x040069BF RID: 27071
		[Header("Outfit rank (2.4-A)")]
		[Tooltip("Prefissi partId degli accessori ESTETICI (es. 'Accessory.') che l'outfit rank NON deve spogliare (Headpin/Glasses restano addosso). I capi funzionali 'Outfit.*' (mask/shoes) vengono comunque strippati.")]
		public string[] aestheticSlotPrefixes = new string[] { "Accessory." };

		// Token: 0x040069C0 RID: 27072
		[Header("Timer scelta encounter (2.4-B)")]
		[Tooltip("Durata (s) della fase di scelta encounter quando il Rank system e' attivo. Allo scadere: auto-SHAME. Default 15.")]
		public float choiceTimerSeconds = 15f;

		// Token: 0x040069C1 RID: 27073
		[Header("Aperture lingerie = Confident (3.2-C)")]
		[Tooltip("Balance spinto durante un'apertura lingerie (Confident BASE, non supreme). Default 80.")]
		public float lingerieConfidentBalance = 80f;

		// Token: 0x040069C2 RID: 27074
		[Tooltip("Cooldown (s) dopo che entrambe le aperture si chiudono, prima di tornare Embarassed. Riaprire annulla il timer. Default 3.")]
		public float lingerieConfidentCooldownSeconds = 3f;
	}
}
