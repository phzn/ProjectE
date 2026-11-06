using System;
using System.Collections.Generic;
using MiragePro.Impostors.Core;
using UnityEngine;

namespace MiragePro.Impostors.Elements
{
	// Token: 0x02000271 RID: 625
	public class MirageRuntimeImpostor : MonoBehaviour
	{
		// Token: 0x06001051 RID: 4177 RVA: 0x0006C957 File Offset: 0x0006AB57
		private void Reset()
		{
			this.preset = ScriptableObject.CreateInstance<ImpostorPreset>();
			this.lodSettings = ScriptableObject.CreateInstance<ImpostorLODGroupPreset>();
			this.solver = new ImpostorGeometrySolver(ref this.preset);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0006C980 File Offset: 0x0006AB80
		private void Awake()
		{
			if (this.preset == null)
			{
				this.preset = ScriptableObject.CreateInstance<ImpostorPreset>();
			}
			if (this.lodSettings == null)
			{
				this.lodSettings = ScriptableObject.CreateInstance<ImpostorLODGroupPreset>();
			}
			if (this.solver == null)
			{
				this.solver = new ImpostorGeometrySolver(ref this.preset);
			}
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0006C9D8 File Offset: 0x0006ABD8
		private void Start()
		{
			if (this.triggerMode == MirageRuntimeImpostor.BakeTriggerMode.OnStart)
			{
				if (this.targets.Count == 0)
				{
					Debug.LogWarning("[Mirage Runtime] No mesh filters found under " + base.name);
					return;
				}
				this.BakeRuntimeImpostor();
			}
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0006CA0C File Offset: 0x0006AC0C
		public void BakeRuntimeImpostor()
		{
			if (this.preset == null || this.lodSettings == null || this.solver == null)
			{
				return;
			}
			Vector3 center = this.solver.GetBounds(this.targets).center;
			float orthgraphicCameraSize = this.solver.GetOrthgraphicCameraSize(this.targets);
			int num = Mathf.RoundToInt(Mathf.Pow(2f, (float)(7 + this.preset.resIndex)));
			List<Vector3> povs = this.solver.GetPOVs(orthgraphicCameraSize + 0.01f);
			List<Mesh> list = new List<Mesh>();
			List<Material[]> list2 = new List<Material[]>();
			List<Matrix4x4> list3 = new List<Matrix4x4>();
			foreach (MeshFilter meshFilter in this.targets)
			{
				if (!(meshFilter == null))
				{
					list.Add(meshFilter.sharedMesh);
					list2.Add(meshFilter.GetComponent<MeshRenderer>().sharedMaterials);
					Matrix4x4 matrix4x = Matrix4x4.Translate(-center);
					list3.Add(matrix4x * meshFilter.GetComponent<MeshRenderer>().localToWorldMatrix);
				}
			}
			IBakingEngine bakingEngine = new RuntimeBakingEngine(povs, list, list2, list3, num, orthgraphicCameraSize, this.preset.lightingMethod);
			Texture2D texture2D = bakingEngine.ComputeColorMaps();
			bakingEngine.ApplyPostProcessing(ref texture2D, texture2D);
			Texture2D texture2D2 = null;
			Texture2D texture2D3 = null;
			if (this.preset.lightingMethod != LightingMethod.UseSunSource)
			{
				texture2D2 = bakingEngine.ComputeNormalMaps();
				bakingEngine.ApplyPostProcessing(ref texture2D2, texture2D);
				texture2D3 = bakingEngine.ComputeMaskMaps();
			}
			bakingEngine.Cleanup();
			new RuntimeImpostorPacker().PackRuntimeImpostor(base.gameObject, this.targets, texture2D, texture2D2, texture2D3, orthgraphicCameraSize, this.solver.Subdivisions, center, this.preset, this.lodSettings);
		}

		// Token: 0x040011E2 RID: 4578
		public bool overrideTargets;

		// Token: 0x040011E3 RID: 4579
		public List<MeshFilter> targets = new List<MeshFilter>();

		// Token: 0x040011E4 RID: 4580
		[Header("Settings")]
		public ImpostorPreset preset;

		// Token: 0x040011E5 RID: 4581
		public ImpostorLODGroupPreset lodSettings;

		// Token: 0x040011E6 RID: 4582
		public MirageRuntimeImpostor.BakeTriggerMode triggerMode;

		// Token: 0x040011E7 RID: 4583
		private ImpostorGeometrySolver solver;

		// Token: 0x02000272 RID: 626
		public enum BakeTriggerMode
		{
			// Token: 0x040011E9 RID: 4585
			OnStart,
			// Token: 0x040011EA RID: 4586
			ViaScript
		}
	}
}
