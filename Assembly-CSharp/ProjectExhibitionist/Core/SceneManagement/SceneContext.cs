using System;
using UnityEngine;

namespace ProjectExhibitionist.Core.SceneManagement
{
	// Token: 0x02000A80 RID: 2688
	[DefaultExecutionOrder(-1100)]
	[DisallowMultipleComponent]
	public class SceneContext : MonoBehaviour
	{
		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06004C7E RID: 19582 RVA: 0x001DBA38 File Offset: 0x001D9C38
		// (set) Token: 0x06004C7F RID: 19583 RVA: 0x001DBA3F File Offset: 0x001D9C3F
		public static SceneContext Current { get; private set; }

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06004C80 RID: 19584 RVA: 0x001DBA47 File Offset: 0x001D9C47
		public Transform PlayerSpawnPoint
		{
			get
			{
				return this.playerSpawnPoint;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06004C81 RID: 19585 RVA: 0x001DBA4F File Offset: 0x001D9C4F
		public SceneProfileSO Profile
		{
			get
			{
				return this.profile;
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004C82 RID: 19586 RVA: 0x001DBA57 File Offset: 0x001D9C57
		public Transform NpcSpawnContainer
		{
			get
			{
				return this.npcSpawnContainer;
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06004C83 RID: 19587 RVA: 0x001DBA5F File Offset: 0x001D9C5F
		public Transform EnvironmentRootOverride
		{
			get
			{
				return this.environmentRootOverride;
			}
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x001DBA68 File Offset: 0x001D9C68
		private void Awake()
		{
			if (SceneContext.Current != null && SceneContext.Current != this)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[SceneContext] Esiste già un SceneContext attivo ('",
					SceneContext.Current.name,
					"'). Questa istanza ('",
					base.name,
					"') lo sostituisce. Una scena dovrebbe averne una sola."
				}));
			}
			SceneContext.Current = this;
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[SceneContext] Attivo per scena '",
					base.gameObject.scene.name,
					"'. Spawn=",
					(this.playerSpawnPoint != null) ? this.playerSpawnPoint.name : "<none>",
					", Profile=",
					(this.profile != null) ? this.profile.name : "<none>"
				}));
			}
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x001DBB62 File Offset: 0x001D9D62
		private void OnDestroy()
		{
			if (SceneContext.Current == this)
			{
				SceneContext.Current = null;
			}
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x001DBB78 File Offset: 0x001D9D78
		public bool TryGetPlayerSpawn(out Vector3 position, out Quaternion rotation)
		{
			if (this.playerSpawnPoint != null)
			{
				position = this.playerSpawnPoint.position;
				rotation = Quaternion.Euler(0f, this.playerSpawnPoint.eulerAngles.y, 0f);
				return true;
			}
			position = Vector3.zero;
			rotation = Quaternion.identity;
			return false;
		}

		// Token: 0x04004DEF RID: 19951
		[Header("Player")]
		[Tooltip("Punto in cui il player persistente (Ciel, dal _Core) viene riposizionato all'ingresso in questa scena. Se null, il player resta dov'è.")]
		[SerializeField]
		private Transform playerSpawnPoint;

		// Token: 0x04004DF0 RID: 19952
		[Header("Configurazione scena")]
		[Tooltip("Profilo che dice al core quali sistemi attivare/disattivare in questa scena (mappa procedurale, NPC, polizia, skybox, ...). Se null, il core usa i suoi default.")]
		[SerializeField]
		private SceneProfileSO profile;

		// Token: 0x04004DF1 RID: 19953
		[Header("NPC (scene statiche / POI)")]
		[Tooltip("Container che fa da 'chunk statico' per lo spawn NPC: i suoi NPCSpawnPoint (anche annidati) verranno raccolti dal NPCZoneManager del _Core. Tipicamente il GameObject 'NPCSpawnPoints' della scena. Lascia null se la scena usa la mappa procedurale (gli spawn point vengono dai chunk generati).")]
		[SerializeField]
		private Transform npcSpawnContainer;

		// Token: 0x04004DF2 RID: 19954
		[Header("Override ambiente (opzionali)")]
		[Tooltip("Se assegnato, questo è lo skybox/Azure specifico di questa scena. Lascia null per usare quello globale del _Core.")]
		[SerializeField]
		private Transform environmentRootOverride;

		// Token: 0x04004DF3 RID: 19955
		[Header("Debug")]
		[SerializeField]
		private bool enableDebugLogs = true;
	}
}
