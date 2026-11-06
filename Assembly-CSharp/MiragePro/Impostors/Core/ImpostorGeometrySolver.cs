using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiragePro.Impostors.Core
{
	// Token: 0x02000275 RID: 629
	public class ImpostorGeometrySolver
	{
		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x0006D11C File Offset: 0x0006B31C
		// (set) Token: 0x0600106B RID: 4203 RVA: 0x0006D124 File Offset: 0x0006B324
		public ImpostorPreset Preset { get; private set; }

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x0006D12D File Offset: 0x0006B32D
		public int POVNumber
		{
			get
			{
				if (this.Dirty)
				{
					this.Update();
				}
				return this.povNb;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x0006D143 File Offset: 0x0006B343
		public bool Dirty
		{
			get
			{
				return this.lastPresets != this.Preset;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x0006D156 File Offset: 0x0006B356
		public int Subdivisions
		{
			get
			{
				return Mathf.CeilToInt(Mathf.Sqrt((float)this.POVNumber));
			}
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x0006D169 File Offset: 0x0006B369
		public ImpostorGeometrySolver(ref ImpostorPreset settings)
		{
			this.Preset = settings;
			this.Update();
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0006D180 File Offset: 0x0006B380
		private void Update()
		{
			if (this.Preset.FibonacciSphere)
			{
				this.latitudeFibonacciClusters = new int[1 + this.Preset.latitudeSamples * 2];
				for (int i = 0; i <= this.Preset.latitudeSamples; i++)
				{
					this.latitudeFibonacciClusters[this.Preset.latitudeSamples + i] = Mathf.RoundToInt(Mathf.Cos((float)i * 3.1415927f / (2f * (float)(this.Preset.latitudeSamples + 1))) * (float)this.Preset.longitudeSamples);
					this.latitudeFibonacciClusters[this.Preset.latitudeSamples - i] = Mathf.RoundToInt(Mathf.Cos((float)i * 3.1415927f / (2f * (float)(this.Preset.latitudeSamples + 1))) * (float)this.Preset.longitudeSamples);
				}
				this.povNb = 0;
				foreach (int num in this.latitudeFibonacciClusters)
				{
					this.povNb += num;
				}
			}
			else
			{
				this.povNb = this.Preset.longitudeSamples * (1 + 2 * this.Preset.latitudeSamples);
			}
			this.lastPresets = ImpostorPreset.Clone(this.Preset);
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x0006D2C8 File Offset: 0x0006B4C8
		public List<Vector3> GetNormalizedPOVs()
		{
			if (this.Dirty)
			{
				this.Update();
			}
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < this.POVNumber; i++)
			{
				list.Add(this.GetNormalizedCameraPosition(i));
			}
			return list;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0006D308 File Offset: 0x0006B508
		public List<Vector3> GetPOVs(Mesh mesh)
		{
			if (this.Dirty)
			{
				this.Update();
			}
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < this.POVNumber; i++)
			{
				list.Add(this.GetCameraPosition(mesh, i));
			}
			return list;
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x0006D34C File Offset: 0x0006B54C
		public List<Vector3> GetPOVs(float distance)
		{
			if (this.Dirty)
			{
				this.Update();
			}
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < this.POVNumber; i++)
			{
				list.Add(this.GetNormalizedCameraPosition(i) * distance);
			}
			return list;
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0006D394 File Offset: 0x0006B594
		public Vector3 GetCameraPosition(Mesh mesh, int iteration)
		{
			return this.GetNormalizedCameraPosition(iteration) * (mesh.bounds.size.magnitude + 0.01f);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0006D3CC File Offset: 0x0006B5CC
		public Vector3 GetNormalizedCameraPosition(int iteration)
		{
			int num = iteration % (this.Subdivisions * this.Subdivisions);
			if (this.Preset.FibonacciSphere)
			{
				int num2 = 0;
				float num3 = 0f;
				float num4 = 0f;
				for (int i = 0; i < this.latitudeFibonacciClusters.Length; i++)
				{
					num2 += this.latitudeFibonacciClusters[i];
					if (num2 > num)
					{
						num3 = (float)(i - this.Preset.latitudeSamples) * this.Preset.latitudeAngularStep * 0.017453292f;
						num4 = (float)(num - num2 + this.latitudeFibonacciClusters[i]) / (float)this.latitudeFibonacciClusters[i] * 3.1415927f * 2f;
						break;
					}
				}
				return new Vector3(Mathf.Cos(num4) * Mathf.Cos(num3), Mathf.Sin(num3), Mathf.Sin(num4) * Mathf.Cos(num3));
			}
			float num5 = ((float)(num % this.Preset.longitudeSamples) * this.Preset.longitudeAngularStep + this.Preset.longitudeOffset) * 0.017453292f;
			float num6 = (float)(Mathf.FloorToInt((float)num / (float)this.Preset.longitudeSamples) - this.Preset.latitudeSamples + this.Preset.latitudeOffset) * this.Preset.latitudeAngularStep * 0.017453292f;
			return new Vector3(Mathf.Cos(num5) * Mathf.Cos(num6), Mathf.Sin(num6), Mathf.Sin(num5) * Mathf.Cos(num6));
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0006D538 File Offset: 0x0006B738
		public Vector3 GetBoundsAwareCenter(MeshFilter meshFilter)
		{
			Bounds bounds = meshFilter.sharedMesh.bounds;
			Vector3 vector = meshFilter.transform.TransformVector(bounds.center);
			float magnitude = bounds.center.magnitude;
			vector = vector.normalized * magnitude;
			Vector3 vector2 = Vector3.ProjectOnPlane(vector, Vector3.up);
			return new Vector3(-vector2.x, meshFilter.transform.position.y - meshFilter.GetComponent<MeshRenderer>().bounds.center.y * meshFilter.transform.localScale.y, -vector2.z);
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0006D5E0 File Offset: 0x0006B7E0
		public float GetOrthgraphicCameraSize(Mesh mesh)
		{
			return mesh.bounds.extents.magnitude;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0006D604 File Offset: 0x0006B804
		public float GetOrthgraphicCameraSize(List<MeshFilter> meshFilters)
		{
			return this.GetBounds(meshFilters).extents.magnitude;
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0006D628 File Offset: 0x0006B828
		public Vector3 GetPivotExcentricity(MeshFilter meshFilter)
		{
			Bounds bounds = meshFilter.sharedMesh.bounds;
			Vector3 vector = meshFilter.transform.TransformVector(bounds.center);
			float magnitude = bounds.center.magnitude;
			vector = vector.normalized * magnitude;
			return Vector3.ProjectOnPlane(vector, Vector3.up);
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0006D680 File Offset: 0x0006B880
		public Bounds GetBounds(List<MeshFilter> meshFilters)
		{
			if (meshFilters.Count == 0 || meshFilters[0] == null)
			{
				return default(Bounds);
			}
			Bounds bounds = meshFilters[0].GetComponent<MeshRenderer>().bounds;
			Vector3 vector = bounds.min;
			Vector3 vector2 = bounds.max;
			foreach (MeshFilter meshFilter in meshFilters)
			{
				if (!(meshFilter == null))
				{
					Bounds bounds2 = meshFilter.GetComponent<MeshRenderer>().bounds;
					vector = Vector3.Min(vector, bounds2.min);
					vector2 = Vector3.Max(vector2, bounds2.max);
				}
			}
			Bounds bounds3 = default(Bounds);
			bounds3.SetMinMax(vector, vector2);
			return bounds3;
		}

		// Token: 0x040011F3 RID: 4595
		private int povNb;

		// Token: 0x040011F4 RID: 4596
		private ImpostorPreset lastPresets;

		// Token: 0x040011F5 RID: 4597
		private int[] latitudeFibonacciClusters;
	}
}
