using System;
using Features.Rank;
using Features.RunModes;
using UnityEngine;

namespace Features.Vehicles.Spawning
{
	// Token: 0x02000B8C RID: 2956
	public class VehicleSpawnPoint : MonoBehaviour
	{
		// Token: 0x0600548B RID: 21643 RVA: 0x0020D741 File Offset: 0x0020B941
		internal Object ReadUniversityGameplayReference(UniversityGameplayField field)
		{
			if (field == UniversityGameplayField.VehicleSpawnPoint_spawnProfile)
			{
				return this.spawnProfile;
			}
			throw new InvalidOperationException("Unsupported VehicleSpawnPoint gameplay binding: " + field.ToString());
		}

		// Token: 0x0600548C RID: 21644 RVA: 0x0020D76A File Offset: 0x0020B96A
		internal void WriteUniversityGameplayReference(UniversityGameplayField field, Object asset)
		{
			if (field == UniversityGameplayField.VehicleSpawnPoint_spawnProfile)
			{
				this.spawnProfile = (VehicleSpawnProfile)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported VehicleSpawnPoint gameplay binding: " + field.ToString());
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x0600548D RID: 21645 RVA: 0x0020D799 File Offset: 0x0020B999
		public bool GuaranteedSpawn
		{
			get
			{
				return this.guaranteedSpawn;
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x0600548E RID: 21646 RVA: 0x0020D7A1 File Offset: 0x0020B9A1
		public VehicleSpawnProfile SpawnProfile
		{
			get
			{
				return this.spawnProfile;
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x0600548F RID: 21647 RVA: 0x0020D7A9 File Offset: 0x0020B9A9
		public Vector3 SpawnPosition
		{
			get
			{
				if (!(this.spawnTransform != null))
				{
					return base.transform.position;
				}
				return this.spawnTransform.position;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06005490 RID: 21648 RVA: 0x0020D7D0 File Offset: 0x0020B9D0
		public Quaternion SpawnRotation
		{
			get
			{
				if (!this.usePointRotation)
				{
					return Quaternion.identity;
				}
				if (!(this.spawnTransform != null))
				{
					return base.transform.rotation;
				}
				return this.spawnTransform.rotation;
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06005491 RID: 21649 RVA: 0x0020D808 File Offset: 0x0020BA08
		public Vector3 ClearanceSize
		{
			get
			{
				return new Vector3(Mathf.Max(0.1f, this.clearanceSize.x), Mathf.Max(0.1f, this.clearanceSize.y), Mathf.Max(0.1f, this.clearanceSize.z));
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06005492 RID: 21650 RVA: 0x0020D859 File Offset: 0x0020BA59
		public Vector3 ClearanceCenter
		{
			get
			{
				return this.SpawnPosition + Vector3.up * (0.08f + this.ClearanceSize.y * 0.5f);
			}
		}

		// Token: 0x06005493 RID: 21651 RVA: 0x0020D888 File Offset: 0x0020BA88
		public bool TryPickVehiclePrefab(out GameObject prefab)
		{
			prefab = null;
			if (this.spawnProfile == null)
			{
				return false;
			}
			float num;
			if (!Application.isPlaying && !RunModeService.IsLongWayHome)
			{
				num = 1f;
			}
			else
			{
				RunModeKind currentMode = RunModeService.CurrentMode;
				RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
				num = GuidedRankGameplayPolicy.RideableSpawnMultiplier(currentMode, (currentRank != null) ? currentRank.rankId : null, RankProgressionService.IsAdminRankActive);
			}
			float num2 = num;
			if (num2 <= 0f || (num2 < 1f && Random.value >= num2))
			{
				return false;
			}
			float vehicleSpawnMultiplier = RunModeService.VehicleSpawnMultiplier;
			if (!this.guaranteedSpawn)
			{
				return this.spawnProfile.TryPickVehiclePrefab(vehicleSpawnMultiplier, out prefab);
			}
			return this.spawnProfile.TryPickGuaranteedVehiclePrefab(vehicleSpawnMultiplier, out prefab);
		}

		// Token: 0x06005494 RID: 21652 RVA: 0x0020D924 File Offset: 0x0020BB24
		private void OnDrawGizmos()
		{
			if (!this.drawGizmos)
			{
				return;
			}
			Vector3 spawnPosition = this.SpawnPosition;
			Gizmos.color = this.gizmoColor;
			Gizmos.DrawWireSphere(spawnPosition, 0.35f);
			Vector3 vector = this.SpawnRotation * Vector3.forward;
			Gizmos.DrawLine(spawnPosition, spawnPosition + vector * 1.2f);
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = Matrix4x4.TRS(this.ClearanceCenter, this.SpawnRotation, Vector3.one);
			Gizmos.color = new Color(this.gizmoColor.r, this.gizmoColor.g, this.gizmoColor.b, this.gizmoColor.a * 0.45f);
			Gizmos.DrawWireCube(Vector3.zero, this.ClearanceSize);
			Gizmos.matrix = matrix;
		}

		// Token: 0x040054CE RID: 21710
		public const float ClearanceBottomOffset = 0.08f;

		// Token: 0x040054CF RID: 21711
		[Header("Spawn")]
		[SerializeField]
		private VehicleSpawnProfile spawnProfile;

		// Token: 0x040054D0 RID: 21712
		[SerializeField]
		private Transform spawnTransform;

		// Token: 0x040054D1 RID: 21713
		[Tooltip("Se attivo, questo singolo punto ignora soltanto il peso 'nessun veicolo' del profilo e sceglie sempre uno dei prefab validi. I controlli di terreno e occupazione restano attivi per evitare spawn pericolosi.")]
		[SerializeField]
		private bool guaranteedSpawn;

		// Token: 0x040054D2 RID: 21714
		[SerializeField]
		private bool usePointRotation = true;

		// Token: 0x040054D3 RID: 21715
		[Tooltip("Volume libero richiesto per lo spawn (X larghezza, Y altezza, Z lunghezza). Ruota insieme alla freccia del punto, quindi vicino a un muro orienta Z lungo il muro.")]
		[SerializeField]
		private Vector3 clearanceSize = new Vector3(1.5f, 1.8f, 2.5f);

		// Token: 0x040054D4 RID: 21716
		[Header("Debug")]
		[SerializeField]
		private bool drawGizmos = true;

		// Token: 0x040054D5 RID: 21717
		[SerializeField]
		private Color gizmoColor = new Color(1f, 0.45f, 0f, 0.8f);
	}
}
