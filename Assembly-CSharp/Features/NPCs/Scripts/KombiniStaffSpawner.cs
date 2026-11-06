using System;
using System.Collections.Generic;
using System.Linq;
using Features.NPC;
using UnityEngine;

namespace Features.NPCs.Scripts
{
	// Token: 0x02000EBF RID: 3775
	public class KombiniStaffSpawner : MonoBehaviour
	{
		// Token: 0x06007112 RID: 28946 RVA: 0x002DEA5B File Offset: 0x002DCC5B
		private void Start()
		{
			if (this.spawnOnStart)
			{
				this.SpawnStaff();
			}
		}

		// Token: 0x06007113 RID: 28947 RVA: 0x002DEA6C File Offset: 0x002DCC6C
		public void SpawnStaff()
		{
			foreach (StaffRoleConfig staffRoleConfig in this.staffConfigs)
			{
				if (staffRoleConfig.GetRandomPrefab() == null)
				{
					Debug.LogWarning("[KombiniStaffSpawner] Nessun prefab disponibile per il ruolo " + staffRoleConfig.roleName);
				}
				else
				{
					int num = staffRoleConfig.minCount;
					for (int i = 0; i < staffRoleConfig.maxCount - staffRoleConfig.minCount; i++)
					{
						if (Random.value <= staffRoleConfig.spawnProbability)
						{
							num++;
						}
					}
					List<Transform> spawnPointsForRole = this.GetSpawnPointsForRole(staffRoleConfig);
					for (int j = 0; j < num; j++)
					{
						Transform transform = spawnPointsForRole[j % spawnPointsForRole.Count];
						Vector3 position = transform.position;
						Quaternion rotation = transform.rotation;
						GameObject randomPrefab = staffRoleConfig.GetRandomPrefab();
						Transform transform2 = (this.parentNPCsToSpawner ? base.transform : null);
						GameObject gameObject = Object.Instantiate<GameObject>(randomPrefab, position, rotation, transform2);
						gameObject.name = string.Format("{0}_{1}_{2}", staffRoleConfig.role, j + 1, randomPrefab.name);
						this.AssignWorkplaceContext(gameObject);
						this.EnsureWanderController(gameObject);
						if (num > spawnPointsForRole.Count)
						{
							float num2 = 0.5f;
							gameObject.transform.position += new Vector3(Random.Range(-num2, num2), 0f, Random.Range(-num2, num2));
						}
					}
				}
			}
			Debug.Log("[KombiniStaffSpawner] Staff spawning completato.");
		}

		// Token: 0x06007114 RID: 28948 RVA: 0x002DEC18 File Offset: 0x002DCE18
		private void EnsureWanderController(GameObject npc)
		{
			if (npc.GetComponent<NPCWanderController>() == null)
			{
				npc.AddComponent<NPCWanderController>();
				Debug.Log("[KombiniStaffSpawner] Added NPCWanderController to " + npc.name);
			}
		}

		// Token: 0x06007115 RID: 28949 RVA: 0x002DEC44 File Offset: 0x002DCE44
		private void AssignWorkplaceContext(GameObject npc)
		{
			NPCContext npccontext = npc.GetComponent<NPCContext>();
			if (npccontext == null)
			{
				npccontext = npc.AddComponent<NPCContext>();
			}
			Transform transform = this.GetWorkplaceRoot();
			npccontext.SetWorkplace(transform);
		}

		// Token: 0x06007116 RID: 28950 RVA: 0x002DEC76 File Offset: 0x002DCE76
		private Transform GetWorkplaceRoot()
		{
			if (this.workplaceRoot != null)
			{
				return this.workplaceRoot;
			}
			return base.transform;
		}

		// Token: 0x06007117 RID: 28951 RVA: 0x002DEC94 File Offset: 0x002DCE94
		private List<Transform> GetSpawnPointsForRole(StaffRoleConfig config)
		{
			if (config.spawnPoints != null && config.spawnPoints.Count > 0)
			{
				List<Transform> list = config.spawnPoints.Where<Transform>((Transform p) => p != null).ToList<Transform>();
				if (list.Count > 0)
				{
					return list;
				}
			}
			List<Transform> list2 = new List<Transform>();
			if (this.fallbackSpawnPoint != null)
			{
				list2.Add(this.fallbackSpawnPoint);
			}
			else
			{
				list2.Add(base.transform);
			}
			return list2;
		}

		// Token: 0x040073D8 RID: 29656
		[Header("Spawn Settings")]
		public List<StaffRoleConfig> staffConfigs;

		// Token: 0x040073D9 RID: 29657
		[Header("Fallback Spawn Point")]
		[Tooltip("Punto di spawn di fallback se un ruolo non ha spawn points specifici. Se null, usa la posizione di questo oggetto.")]
		public Transform fallbackSpawnPoint;

		// Token: 0x040073DA RID: 29658
		[Tooltip("Se true, spawna all'avvio.")]
		public bool spawnOnStart = true;

		// Token: 0x040073DB RID: 29659
		[Header("Workplace Context")]
		[Tooltip("La radice del punto di interesse (Kombini). Se null, usa questo transform (lo spawner stesso).")]
		public Transform workplaceRoot;

		// Token: 0x040073DC RID: 29660
		[Tooltip("Se true, gli NPC spawnati diventano figli di questo GameObject.")]
		public bool parentNPCsToSpawner = true;
	}
}
