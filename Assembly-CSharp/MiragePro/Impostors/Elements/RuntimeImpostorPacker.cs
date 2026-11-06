using System;
using System.Collections.Generic;
using MiragePro.Impostors.Core;
using UnityEngine;

namespace MiragePro.Impostors.Elements
{
	// Token: 0x02000273 RID: 627
	public class RuntimeImpostorPacker
	{
		// Token: 0x06001056 RID: 4182 RVA: 0x0006CBF8 File Offset: 0x0006ADF8
		public RuntimeImpostorPacker()
		{
			this.litImpostorShader = Shader.Find("Shader Graphs/MirageProImpostor");
			if (this.litImpostorShader == null || !this.litImpostorShader.isSupported)
			{
				Debug.LogError("[Mirage Pro] No supported impostor shader found. Please reimport the Mirage package.");
				return;
			}
			this.unlitImpostorShader = Shader.Find("Shader Graphs/MirageProImpostorUnlit");
			if (this.unlitImpostorShader == null || !this.unlitImpostorShader.isSupported)
			{
				Debug.LogError("[Mirage Pro] No supported impostor shader found. Please reimport the Mirage package.");
				return;
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0006CC78 File Offset: 0x0006AE78
		public GameObject PackRuntimeImpostor(GameObject sourceGo, List<MeshFilter> sourceFilters, Texture2D colorMap, Texture2D normalMap, Texture2D maskMap, float orthographicSize, int subdivisions, Vector3 pivotExcentricity, ImpostorPreset settings, ImpostorLODGroupPreset lodGroupSettings)
		{
			Material material = new Material((settings.lightingMethod != LightingMethod.UseSunSource) ? this.litImpostorShader : this.unlitImpostorShader);
			material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
			material.EnableKeyword("_ALPHATEST_ON");
			material.SetTexture("_MainTex", colorMap);
			if (settings.lightingMethod != LightingMethod.UseSunSource)
			{
				material.SetTexture("_NormalMap", normalMap);
				material.SetTexture("_MaskMap", maskMap);
			}
			material.SetFloat("_GridSize", (float)(subdivisions * subdivisions));
			material.SetInt("_LongitudeSamples", settings.longitudeSamples);
			material.SetFloat("_LongitudeOffset", settings.longitudeOffset);
			material.SetFloat("_LongitudeStep", settings.longitudeAngularStep);
			material.SetInt("_LatitudeSamples", settings.latitudeSamples);
			material.SetInt("_LatitudeOffset", settings.latitudeOffset);
			material.SetFloat("_LatitudeStep", settings.latitudeAngularStep);
			material.SetFloat("_SmartSphere", settings.FibonacciSphere ? 1f : 0f);
			Mesh mesh = ImpostorMeshUtility.BuildQuad(orthographicSize * 2f);
			mesh.name = "ImpostorQuad(Runtime)";
			GameObject gameObject = new GameObject("ImpostorInstance_Runtime");
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.SetParent(sourceGo.transform);
			gameObject.transform.position = pivotExcentricity;
			gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
			gameObject.AddComponent<MeshRenderer>().sharedMaterial = material;
			this.SetupRuntimeLODGroup(sourceGo, sourceFilters, gameObject, lodGroupSettings);
			return gameObject;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0006CDFC File Offset: 0x0006AFFC
		private void SetupRuntimeLODGroup(GameObject sourceGo, List<MeshFilter> sourceFilters, GameObject impostor, ImpostorLODGroupPreset lodSettings)
		{
			if (!lodSettings.setupLOD)
			{
				return;
			}
			bool flag;
			LODGroup lodgroup = MirageRuntimeUtilities.GetSingleLODGroup(sourceFilters, out flag);
			if (!flag)
			{
				Debug.LogError("[Mirage Runtime] LODGroup conflict: cannot set LODs.");
				return;
			}
			if (lodgroup == null)
			{
				lodgroup = sourceGo.AddComponent<LODGroup>();
				lodgroup.fadeMode = LODFadeMode.CrossFade;
				LOD[] array = new LOD[2];
				Renderer[] array2 = new Renderer[sourceFilters.Count];
				for (int i = 0; i < sourceFilters.Count; i++)
				{
					array2[i] = sourceFilters[i].GetComponent<Renderer>();
				}
				array[0].renderers = array2;
				array[0].screenRelativeTransitionHeight = lodSettings.lodPerformance;
				array[0].fadeTransitionWidth = 0.1f;
				array[1].renderers = new Renderer[] { impostor.GetComponent<MeshRenderer>() };
				array[1].screenRelativeTransitionHeight = lodSettings.lodSizeCulling;
				array[1].fadeTransitionWidth = 0.1f;
				lodgroup.SetLODs(array);
				lodgroup.RecalculateBounds();
				ImpostorReference impostorReference = sourceGo.GetComponent<ImpostorReference>();
				if (impostorReference == null)
				{
					impostorReference = sourceGo.AddComponent<ImpostorReference>();
				}
				impostorReference.impostorObject = impostor;
				return;
			}
			MirageRuntimeUtilities.CleanLODGroup(lodgroup);
			LOD[] lods = lodgroup.GetLODs();
			ImpostorReference impostorReference2 = lodgroup.GetComponent<ImpostorReference>();
			if (!(impostorReference2 != null) || !(impostorReference2.impostorObject != null))
			{
				LOD[] array3 = new LOD[lods.Length + 1];
				lods.CopyTo(array3, 0);
				array3[lods.Length - 1].screenRelativeTransitionHeight = lodSettings.lodPerformance;
				array3[lods.Length].renderers = new Renderer[] { impostor.GetComponent<MeshRenderer>() };
				array3[lods.Length].screenRelativeTransitionHeight = lodSettings.lodSizeCulling;
				lodgroup.SetLODs(array3);
				lodgroup.RecalculateBounds();
				if (impostorReference2 == null)
				{
					impostorReference2 = lodgroup.gameObject.AddComponent<ImpostorReference>();
				}
				impostorReference2.impostorObject = impostor;
				return;
			}
			GameObject impostorObject = impostorReference2.impostorObject;
			Renderer component = impostorObject.GetComponent<Renderer>();
			LOD[] array4 = lods;
			int num = -1;
			for (int j = 0; j < array4.Length; j++)
			{
				Renderer[] renderers = array4[j].renderers;
				for (int k = 0; k < renderers.Length; k++)
				{
					if (renderers[k] == component)
					{
						num = j;
					}
				}
			}
			if (num < 0)
			{
				Debug.LogError("[Mirage Runtime] Previous impostor not found in LODs.");
				return;
			}
			array4[num].renderers = new Renderer[] { impostor.GetComponent<MeshRenderer>() };
			array4[num].screenRelativeTransitionHeight = lodSettings.lodSizeCulling;
			Object.Destroy(impostorObject);
			lodgroup.SetLODs(array4);
			impostorReference2.impostorObject = impostor;
		}

		// Token: 0x040011EB RID: 4587
		private Shader litImpostorShader;

		// Token: 0x040011EC RID: 4588
		private Shader unlitImpostorShader;
	}
}
