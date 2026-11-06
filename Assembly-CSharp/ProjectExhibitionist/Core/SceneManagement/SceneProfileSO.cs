using System;
using UnityEngine;

namespace ProjectExhibitionist.Core.SceneManagement
{
	// Token: 0x02000A81 RID: 2689
	[CreateAssetMenu(fileName = "SceneProfile_New", menuName = "ProjectExhibitionist/Scene Profile", order = 0)]
	public class SceneProfileSO : ScriptableObject
	{
		// Token: 0x04004DF4 RID: 19956
		[Header("Identificazione")]
		[Tooltip("Nome leggibile del profilo (solo per debug/UI).")]
		public string displayName = "New Scene Profile";

		// Token: 0x04004DF5 RID: 19957
		[Header("Ciclo di gioco")]
		[Tooltip("True per le scene che fanno parte di una run (Game/POI). False per hub come CielHome: il Rank system viene smontato e la run successiva riparte pulita.")]
		public bool participatesInRun = true;

		// Token: 0x04004DF6 RID: 19958
		[Header("Mappa")]
		[Tooltip("Genera la mappa procedurale a chunk (ChunkManager). OFF per scene con mappa statica già presente (POI).")]
		public bool proceduralMap = true;

		// Token: 0x04004DF7 RID: 19959
		[Header("Detection / Encounter")]
		[Tooltip("Abilita lo stack di detection/encounter che ora vive nel _Core: SensoryManager, ExhibitionEventDirector, HUDManager, TimedEffectHUDService. OFF per hub/menu (CielHome, MainMenu, Prison) dove non c'è gameplay di esibizione.")]
		public bool detection = true;

		// Token: 0x04004DF8 RID: 19960
		[Header("Player Emotions")]
		[Tooltip("Abilita l'EmotionManager del Player. È un sistema del Player, indipendente dalla detection: può restare attivo anche in hub/menu (es. CielHome) dove non c'è gameplay di esibizione ma il bilancio emotivo serve comunque.")]
		public bool playerEmotions = true;

		// Token: 0x04004DF9 RID: 19961
		[Header("NPC")]
		[Tooltip("Abilita spawn e gestione NPC (NPCZoneManager).")]
		public bool npcs = true;

		// Token: 0x04004DFA RID: 19962
		[Tooltip("Spawn procedurale delle troupe Reporter.")]
		public bool reporters = true;

		// Token: 0x04004DFB RID: 19963
		[Tooltip("Spawn di poliziotti a seguito della Suspicion.")]
		public bool police = true;

		// Token: 0x04004DFC RID: 19964
		[Tooltip("Raggio libero attorno all'ingresso dei POI statici. Nessun NPC viene mai creato dentro questa fascia; durante il primo popolamento lo stesso raggio segue anche il Player. Evita che il caricamento termini con Ciel gia' circondata. Zero mantiene il comportamento globale standard.")]
		[Min(0f)]
		public float initialNpcSafeRadius = 20f;

		// Token: 0x04004DFD RID: 19965
		[Tooltip("Durata della fascia di sicurezza dopo l'inizializzazione della scena. Impedisce al rebalance immediatamente successivo al loading di riempire il vuoto vicino al Player.")]
		[Min(0f)]
		public float initialNpcSafeDuration = 12f;

		// Token: 0x04004DFE RID: 19966
		[Header("Ambiente")]
		[Tooltip("Abilita lo skybox/meteo globale (AzureCoreSystem) del core. OFF per ambienti interni che usano la propria illuminazione.")]
		public bool skybox = true;

		// Token: 0x04004DFF RID: 19967
		[Tooltip("Abilita le luci stradali dinamiche (StreetLightManager). Tipicamente legate alla mappa procedurale.")]
		public bool streetLights = true;

		// Token: 0x04004E00 RID: 19968
		[Tooltip("Abilita l'audio ambientale di strada del core. OFF per scene con il proprio audio ambientale.")]
		public bool ambientAudio = true;

		// Token: 0x04004E01 RID: 19969
		[Header("HUD")]
		[Tooltip("Mostra la minimappa (Compass Navigator Pro). OFF nei POI con scena dedicata: il refresh della minimappa ri-renderizza la scena dall'alto (TopDownCamera) a ogni movimento del player — in ambienti densi come il Mall dimezza gli FPS. La compass bar non è toccata da questo flag.")]
		public bool minimap = true;

		// Token: 0x04004E02 RID: 19970
		[Header("Equipaggiamento player")]
		[Tooltip("Rende disponibili i veicoli del player (monociclo, bici, ...). Meccanica futura: spawn/loot. Per ora tipicamente OFF.")]
		public bool vehicles;

		// Token: 0x04004E03 RID: 19971
		[Header("NPC — override per-scena")]
		[Tooltip("Se valorizzato, sovrascrive il prefisso usato da NPCRuntimeSpawner per filtrare i preset NPC in questa scena (es. 'Pool_' per popolare la piscina SOLO con i Pool_*). Vuoto = usa il prefisso di default dello spawner ('NPC_').")]
		public string npcPrefixOverride = "";
	}
}
