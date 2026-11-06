using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Vehicles.Spawning
{
	// Token: 0x02000B8E RID: 2958
	[CreateAssetMenu(fileName = "VehicleSpawnProfile", menuName = "Features/Vehicles/Spawn Profile")]
	public class VehicleSpawnProfile : ScriptableObject
	{
		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06005496 RID: 21654 RVA: 0x0020DA4A File Offset: 0x0020BC4A
		public float NoSpawnWeight
		{
			get
			{
				return this.noSpawnWeight;
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06005497 RID: 21655 RVA: 0x0020DA52 File Offset: 0x0020BC52
		public IReadOnlyList<VehicleSpawnCandidate> Candidates
		{
			get
			{
				return this.candidates;
			}
		}

		// Token: 0x06005498 RID: 21656 RVA: 0x0020DA5A File Offset: 0x0020BC5A
		public bool TryPickVehiclePrefab(out GameObject prefab)
		{
			return this.TryPickVehiclePrefab(1f, out prefab);
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x0020DA68 File Offset: 0x0020BC68
		public bool TryPickVehiclePrefab(float probabilityMultiplier, out GameObject prefab)
		{
			return this.TryPickVehiclePrefab(this.noSpawnWeight, probabilityMultiplier, out prefab);
		}

		// Token: 0x0600549A RID: 21658 RVA: 0x0020DA78 File Offset: 0x0020BC78
		public bool TryPickGuaranteedVehiclePrefab(out GameObject prefab)
		{
			return this.TryPickGuaranteedVehiclePrefab(1f, out prefab);
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x0020DA86 File Offset: 0x0020BC86
		public bool TryPickGuaranteedVehiclePrefab(float probabilityMultiplier, out GameObject prefab)
		{
			return this.TryPickVehiclePrefab(0f, probabilityMultiplier, out prefab);
		}

		// Token: 0x0600549C RID: 21660 RVA: 0x0020DA98 File Offset: 0x0020BC98
		private bool TryPickVehiclePrefab(float emptyWeightOverride, float probabilityMultiplier, out GameObject prefab)
		{
			prefab = null;
			if (float.IsNaN(probabilityMultiplier) || float.IsInfinity(probabilityMultiplier))
			{
				probabilityMultiplier = 1f;
			}
			if (probabilityMultiplier <= 0f)
			{
				return false;
			}
			float num = Mathf.Max(0f, emptyWeightOverride);
			float num2 = num;
			for (int i = 0; i < this.candidates.Count; i++)
			{
				if (!(this.candidates[i].prefab == null))
				{
					num2 += Mathf.Max(0f, this.candidates[i].weight);
				}
			}
			if (num2 <= 0f)
			{
				return false;
			}
			if (probabilityMultiplier != 1f)
			{
				float num3 = num2 - num;
				if (num3 <= 0f)
				{
					return false;
				}
				float num4 = Mathf.Clamp01(num3 / num2 * probabilityMultiplier);
				num = num3 * (1f - num4) / num4;
				num2 = num + num3;
			}
			float num5 = Random.value * num2;
			if (num5 < num)
			{
				return false;
			}
			float num6 = num;
			for (int j = 0; j < this.candidates.Count; j++)
			{
				VehicleSpawnCandidate vehicleSpawnCandidate = this.candidates[j];
				if (!(vehicleSpawnCandidate.prefab == null))
				{
					float num7 = Mathf.Max(0f, vehicleSpawnCandidate.weight);
					if (num7 > 0f)
					{
						num6 += num7;
						if (num5 <= num6)
						{
							prefab = vehicleSpawnCandidate.prefab;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600549D RID: 21661 RVA: 0x0020DBE8 File Offset: 0x0020BDE8
		private void OnValidate()
		{
			this.noSpawnWeight = Mathf.Max(0f, this.noSpawnWeight);
			for (int i = 0; i < this.candidates.Count; i++)
			{
				VehicleSpawnCandidate vehicleSpawnCandidate = this.candidates[i];
				vehicleSpawnCandidate.weight = Mathf.Max(0f, vehicleSpawnCandidate.weight);
				this.candidates[i] = vehicleSpawnCandidate;
			}
		}

		// Token: 0x040054D8 RID: 21720
		[Header("Spawn Weights")]
		[Tooltip("Peso della scelta 'nessun veicolo'. Se alto, aumentano i punti senza spawn.")]
		[Min(0f)]
		[SerializeField]
		private float noSpawnWeight = 1f;

		// Token: 0x040054D9 RID: 21721
		[Tooltip("Lista prefab veicoli con peso di selezione")]
		[SerializeField]
		private List<VehicleSpawnCandidate> candidates = new List<VehicleSpawnCandidate>();
	}
}
