using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x0200008A RID: 138
public sealed class ParkPerformanceProbe : MonoBehaviour
{
	// Token: 0x060005FA RID: 1530 RVA: 0x0002AFFC File Offset: 0x000291FC
	public void Configure(HeavyZoneStreamingProxy targetProxy, Vector3 target)
	{
		this.proxy = targetProxy;
		this.viewTarget = target;
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0002B00C File Offset: 0x0002920C
	private IEnumerator Start()
	{
		if (Environment.GetCommandLineArgs().Contains("--verify-park-terrain"))
		{
			Application.runInBackground = true;
		}
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = -1;
		base.transform.LookAt(this.viewTarget);
		int num;
		for (int i = 0; i < 120; i = num + 1)
		{
			yield return null;
			num = i;
		}
		long usedBeforeLoad = Profiler.GetTotalAllocatedMemoryLong();
		long reservedBeforeLoad = Profiler.GetTotalReservedMemoryLong();
		float loadStarted = Time.realtimeSinceStartup;
		if (this.proxy != null)
		{
			this.proxy.gameObject.SetActive(true);
		}
		float deadline = Time.realtimeSinceStartup + 90f;
		while (this.proxy != null && !this.proxy.IsReady && !this.proxy.LoadFailed && Time.realtimeSinceStartup < deadline)
		{
			yield return null;
		}
		if (this.proxy == null || this.proxy.LoadFailed || !this.proxy.IsReady)
		{
			Debug.LogError("[ParkBenchmark] FAIL: streamed Park did not load within 90 seconds.");
			ParkPerformanceProbe.FinishTerrainVerificationFailure("Payload did not load within 90 seconds.");
			yield break;
		}
		float assetReadySeconds = Time.realtimeSinceStartup - loadStarted;
		while (this.proxy != null && !this.proxy.IsOperationalReady && !this.proxy.LoadFailed && Time.realtimeSinceStartup < deadline)
		{
			yield return null;
		}
		if (this.proxy == null || this.proxy.LoadFailed || !this.proxy.IsOperationalReady)
		{
			Debug.LogError("[ParkBenchmark] FAIL: streamed Park was asset-ready but not operationally ready within 90 seconds.");
			ParkPerformanceProbe.FinishTerrainVerificationFailure("Payload did not become operational within 90 seconds.");
			yield break;
		}
		float operationalReadySeconds = Time.realtimeSinceStartup - loadStarted;
		float integrationSeconds = Mathf.Max(0f, operationalReadySeconds - assetReadySeconds);
		if (Environment.GetCommandLineArgs().Contains("--verify-park-terrain"))
		{
			yield return this.VerifyTerrainRendering();
			yield break;
		}
		GameObject payload = this.proxy.PayloadInstance;
		IEnumerable<MeshFilter> componentsInChildren = payload.GetComponentsInChildren<MeshFilter>(true);
		SkinnedMeshRenderer[] componentsInChildren2 = payload.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		Renderer[] renderers = payload.GetComponentsInChildren<Renderer>(true);
		MeshCollider[] componentsInChildren3 = payload.GetComponentsInChildren<MeshCollider>(true);
		int num2 = componentsInChildren.Count<MeshFilter>((MeshFilter filter) => filter.sharedMesh == null) + componentsInChildren2.Count<SkinnedMeshRenderer>((SkinnedMeshRenderer renderer) => renderer.sharedMesh == null) + componentsInChildren3.Count<MeshCollider>((MeshCollider collider) => collider.sharedMesh == null) + renderers.Sum<Renderer>((Renderer renderer) => renderer.sharedMaterials.Count<Material>((Material material) => material == null || material.shader == null));
		if (num2 != 0)
		{
			Debug.LogError("[ParkBenchmark] FAIL: runtime payload has " + num2.ToString() + " missing mesh/material/shader references.");
			yield break;
		}
		for (int i = 0; i < this.warmupFrames; i = num + 1)
		{
			yield return null;
			num = i;
		}
		this._frameMilliseconds.Clear();
		for (int i = 0; i < this.sampleFrames; i = num + 1)
		{
			yield return null;
			this._frameMilliseconds.Add(Time.unscaledDeltaTime * 1000f);
			num = i;
		}
		this._frameMilliseconds.Sort();
		float num3 = this._frameMilliseconds.Average();
		float num4 = this._frameMilliseconds[Mathf.Clamp(Mathf.CeilToInt((float)this._frameMilliseconds.Count * 0.95f) - 1, 0, this._frameMilliseconds.Count - 1)];
		float num5 = ((num3 > 0f) ? (1000f / num3) : 0f);
		long num6 = Profiler.GetTotalAllocatedMemoryLong() / 1048576L;
		long num7 = Profiler.GetTotalReservedMemoryLong() / 1048576L;
		long num8 = (Profiler.GetTotalAllocatedMemoryLong() - usedBeforeLoad) / 1048576L;
		long num9 = (Profiler.GetTotalReservedMemoryLong() - reservedBeforeLoad) / 1048576L;
		string text = ((num5 >= 30f && num4 <= 50f) ? "PASS" : "REVIEW");
		Debug.Log(string.Concat(new string[]
		{
			string.Format("[ParkBenchmark] {0} | fps.avg={1:F1} frame.avg={2:F2}ms ", text, num5, num3),
			string.Format("frame.p95={0:F2}ms objects={1} ", num4, payload.GetComponentsInChildren<Transform>(true).Length),
			string.Format("renderers={0} allocated={1}MB reserved={2}MB ", renderers.Length, num6, num7),
			string.Format("assetReady.time={0:F2}s operationalReady.time={1:F2}s ", assetReadySeconds, operationalReadySeconds),
			string.Format("integration.time={0:F2}s loadDelta.allocated={1}MB ", integrationSeconds, num8),
			string.Format("loadDelta.reserved={0}MB", num9)
		}));
		yield break;
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0002B01B File Offset: 0x0002921B
	private IEnumerator VerifyTerrainRendering()
	{
		Application.runInBackground = true;
		Application.targetFrameRate = 60;
		string text = Environment.GetCommandLineArgs().FirstOrDefault<string>((string arg) => arg.StartsWith("--verification-output="));
		string directory = ((text != null) ? text.Substring("--verification-output=".Length) : Path.Combine(Application.persistentDataPath, "ParkTerrainVerification"));
		Directory.CreateDirectory(directory);
		List<string> rows = new List<string> { "tile,changedPixels,totalPixels,shaderSupported,active,enabled,instanced,nonInstancedChanged" };
		Terrain[] terrains = this.proxy.PayloadInstance.GetComponentsInChildren<Terrain>(true);
		Renderer[] renderers = this.proxy.PayloadInstance.GetComponentsInChildren<Renderer>(true);
		bool[] enabledRenderers = renderers.Select<Renderer, bool>((Renderer renderer) => renderer.enabled).ToArray<bool>();
		bool[] heightmaps = terrains.Select<Terrain, bool>((Terrain terrain) => terrain.drawHeightmap).ToArray<bool>();
		bool[] foliage = terrains.Select<Terrain, bool>((Terrain terrain) => terrain.drawTreesAndFoliage).ToArray<bool>();
		Camera camera = base.GetComponent<Camera>();
		RenderTexture target = new RenderTexture(128, 128, 24, RenderTextureFormat.ARGB32);
		target.Create();
		Texture2D sample = new Texture2D(128, 128, TextureFormat.RGB24, false);
		UniversalRenderPipeline.SingleCameraRequest renderRequest = new UniversalRenderPipeline.SingleCameraRequest
		{
			destination = target
		};
		bool cameraWasEnabled = camera.enabled;
		float cameraHeight = (Environment.GetCommandLineArgs().Contains("--verify-terrain-basemap") ? 125f : 50f);
		int passed = 0;
		try
		{
			Renderer[] array = renderers;
			int j;
			for (j = 0; j < array.Length; j++)
			{
				array[j].enabled = false;
			}
			foreach (Terrain terrain2 in terrains)
			{
				terrain2.drawHeightmap = false;
				terrain2.drawTreesAndFoliage = false;
			}
			camera.targetTexture = target;
			camera.enabled = false;
			camera.clearFlags = CameraClearFlags.Color;
			camera.backgroundColor = Color.magenta;
			camera.orthographic = true;
			camera.nearClipPlane = 0.1f;
			camera.farClipPlane = 200f;
			camera.cullingMask = -1;
			camera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
			UniversalAdditionalCameraData component = camera.GetComponent<UniversalAdditionalCameraData>();
			if (component != null)
			{
				component.renderPostProcessing = false;
				component.volumeLayerMask = 0;
			}
			for (int i = 0; i < terrains.Length; i = j + 1)
			{
				Terrain terrain3 = terrains[i];
				TerrainData terrainData = terrain3.terrainData;
				if (terrainData == null)
				{
					rows.Add(terrain3.name + ",0,16384,false");
				}
				else
				{
					Vector3 vector = terrain3.transform.position + new Vector3(terrainData.size.x * 0.5f, terrainData.GetInterpolatedHeight(0.5f, 0.5f) + cameraHeight, terrainData.size.z * 0.5f);
					camera.transform.position = vector;
					camera.orthographicSize = Mathf.Min(terrainData.size.x, terrainData.size.z) * 0.35f;
					yield return null;
					RenderPipeline.SubmitRenderRequest<UniversalRenderPipeline.SingleCameraRequest>(camera, renderRequest);
					Color32[] before = ParkPerformanceProbe.ReadPixels(target, sample);
					if ((float)before.Count<Color32>((Color32 pixel) => pixel.r > 200 && pixel.g < 30 && pixel.b > 200) < (float)before.Length * 0.9f)
					{
						ParkPerformanceProbe.FinishTerrainVerificationFailure("Camera did not render its clear frame.");
						yield break;
					}
					terrain3.drawHeightmap = true;
					yield return null;
					RenderPipeline.SubmitRenderRequest<UniversalRenderPipeline.SingleCameraRequest>(camera, renderRequest);
					Color32[] array3 = ParkPerformanceProbe.ReadPixels(target, sample);
					int num = 0;
					for (int k = 0; k < before.Length; k++)
					{
						if (Mathf.Abs((int)(before[k].r - array3[k].r)) + Mathf.Abs((int)(before[k].g - array3[k].g)) + Mathf.Abs((int)(before[k].b - array3[k].b)) > 30)
						{
							num++;
						}
					}
					bool flag = terrain3.materialTemplate != null && terrain3.materialTemplate.shader.isSupported;
					int num2 = -1;
					if (num == 0 && terrain3.drawInstanced)
					{
						terrain3.drawInstanced = false;
						RenderPipeline.SubmitRenderRequest<UniversalRenderPipeline.SingleCameraRequest>(camera, renderRequest);
						Color32[] array4 = ParkPerformanceProbe.ReadPixels(target, sample);
						num2 = 0;
						for (int l = 0; l < before.Length; l++)
						{
							if (Mathf.Abs((int)(before[l].r - array4[l].r)) + Mathf.Abs((int)(before[l].g - array4[l].g)) + Mathf.Abs((int)(before[l].b - array4[l].b)) > 30)
							{
								num2++;
							}
						}
						terrain3.drawInstanced = true;
					}
					if (num > before.Length / 2 && flag)
					{
						j = passed;
						passed = j + 1;
					}
					rows.Add(string.Concat(new string[]
					{
						terrain3.name,
						",",
						num.ToString(),
						",",
						before.Length.ToString(),
						",",
						flag.ToString(),
						",",
						terrain3.gameObject.activeInHierarchy.ToString(),
						",",
						terrain3.enabled.ToString(),
						",",
						terrain3.drawInstanced.ToString(),
						",",
						num2.ToString()
					}));
					File.WriteAllBytes(Path.Combine(directory, "terrain-" + i.ToString("00") + ".png"), sample.EncodeToPNG());
					terrain3.drawHeightmap = false;
					terrain3 = null;
					before = null;
				}
				j = i;
			}
		}
		finally
		{
			for (int m = 0; m < terrains.Length; m++)
			{
				terrains[m].drawHeightmap = heightmaps[m];
				terrains[m].drawTreesAndFoliage = foliage[m];
			}
			for (int n = 0; n < renderers.Length; n++)
			{
				renderers[n].enabled = enabledRenderers[n];
			}
			camera.targetTexture = null;
			camera.enabled = cameraWasEnabled;
			target.Release();
			Object.Destroy(target);
			Object.Destroy(sample);
			File.WriteAllLines(Path.Combine(directory, "terrain-render.csv"), rows);
		}
		bool flag2 = terrains.Length == 25 && passed == terrains.Length;
		string text2 = string.Concat(new string[]
		{
			"[ParkTerrainBuild] ",
			flag2 ? "PASS" : "FAIL",
			" rendered=",
			passed.ToString(),
			"/",
			terrains.Length.ToString()
		});
		File.WriteAllText(Path.Combine(directory, "result.txt"), text2);
		Debug.Log(text2);
		if (!Application.isEditor)
		{
			Application.Quit(flag2 ? 0 : 2);
		}
		yield break;
		yield break;
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x0002B02C File Offset: 0x0002922C
	private static void FinishTerrainVerificationFailure(string reason)
	{
		if (!Environment.GetCommandLineArgs().Contains("--verify-park-terrain"))
		{
			return;
		}
		string text = Environment.GetCommandLineArgs().FirstOrDefault<string>((string arg) => arg.StartsWith("--verification-output="));
		string text2 = ((text != null) ? text.Substring("--verification-output=".Length) : Path.Combine(Application.persistentDataPath, "ParkTerrainVerification"));
		Directory.CreateDirectory(text2);
		File.WriteAllText(Path.Combine(text2, "result.txt"), "[ParkTerrainBuild] FAIL: " + reason);
		if (!Application.isEditor)
		{
			Application.Quit(2);
		}
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0002B0C8 File Offset: 0x000292C8
	private static Color32[] ReadPixels(RenderTexture target, Texture2D texture)
	{
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = target;
		texture.ReadPixels(new Rect(0f, 0f, (float)target.width, (float)target.height), 0, 0);
		texture.Apply();
		RenderTexture.active = active;
		return texture.GetPixels32();
	}

	// Token: 0x0400048F RID: 1167
	[SerializeField]
	private HeavyZoneStreamingProxy proxy;

	// Token: 0x04000490 RID: 1168
	[SerializeField]
	private Vector3 viewTarget;

	// Token: 0x04000491 RID: 1169
	[SerializeField]
	[Min(30f)]
	private int warmupFrames = 180;

	// Token: 0x04000492 RID: 1170
	[SerializeField]
	[Min(60f)]
	private int sampleFrames = 300;

	// Token: 0x04000493 RID: 1171
	private readonly List<float> _frameMilliseconds = new List<float>(300);
}
