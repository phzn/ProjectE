using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CompassNavigatorPro
{
	// Token: 0x020002AB RID: 683
	[ExecuteAlways]
	public class ScanEffect : MonoBehaviour
	{
		// Token: 0x0600135E RID: 4958 RVA: 0x0007D4FC File Offset: 0x0007B6FC
		private void OnEnable()
		{
			Renderer component = base.GetComponent<Renderer>();
			this.mat = Object.Instantiate<Material>(component.sharedMaterial);
			component.sharedMaterial = this.mat;
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x0007D52D File Offset: 0x0007B72D
		private void OnDestroy()
		{
			ScanEffect.RemoveInstanceFromPool(this);
			if (this.mat != null)
			{
				Object.DestroyImmediate(this.mat);
			}
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x0007D54E File Offset: 0x0007B74E
		public void Reset()
		{
			this.OnScanHit.RemoveAllListeners();
			this.OnScanEnd.RemoveAllListeners();
			this.pois.Clear();
			base.transform.localScale = Vector3.zero;
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0007D58D File Offset: 0x0007B78D
		public void ScanStart()
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.ScanExecute());
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0007D5A2 File Offset: 0x0007B7A2
		private IEnumerator ScanExecute()
		{
			float start = Time.time;
			if (this.audioClipStart != null)
			{
				AudioSource.PlayClipAtPoint(this.audioClipStart, this.center);
			}
			base.transform.position = this.center;
			this.mat.SetColor(ShaderParams.Color, this.color);
			this.mat.SetColor(ShaderParams.ColorBlend, this.colorBlend);
			this.mat.SetVector(ShaderParams.Center, this.center);
			this.mat.SetFloat(ShaderParams.Width, this.width);
			this.mat.SetFloat(ShaderParams.MaxRange, this.maxRange - this.width);
			this.mat.SetFloat(ShaderParams.Alpha, 1f);
			if (this.grid)
			{
				this.mat.EnableKeyword("SCAN_GRID");
				this.mat.SetColor(ShaderParams.GridColor, this.gridColor);
				this.mat.SetVector(ShaderParams.GridData, new Vector4(this.gridSize, this.gridDepthWidthControl, this.gridDepthStart, 0f));
			}
			else
			{
				this.mat.DisableKeyword("SCAN_GRID");
			}
			this.mat.SetInt(ShaderParams.Filled, this.filled ? 1 : 0);
			this.mat.SetInt(ShaderParams.ScanLines, this.scanLines ? 1 : 0);
			this.mat.SetInt(ShaderParams.Noise, this.noise ? 1 : 0);
			if (this.edgeOutline)
			{
				this.mat.EnableKeyword("SCAN_OUTLINE");
				this.mat.SetColor(ShaderParams.EdgeColor, this.edgeColor);
				this.mat.SetVector(ShaderParams.EdgeData, new Vector4(this.edgeDepthThreshold, this.edgeColorThreshold, this.edgeStrength, this.edgeWidth));
			}
			else
			{
				this.mat.DisableKeyword("SCAN_OUTLINE");
			}
			int poiCount = this.pois.Count;
			int currentPOIIndex = 0;
			bool soundPlayed = false;
			float t;
			do
			{
				float num = Time.time - start;
				t = num / this.duration;
				float num2 = this.maxRange * t;
				base.transform.localScale = new Vector3(num2, num2, num2);
				while (currentPOIIndex < poiCount)
				{
					CompassProPOI compassProPOI = this.pois[currentPOIIndex];
					if (compassProPOI == null || ((1 << compassProPOI.gameObject.layer) & this.scanLayerMask) == 0)
					{
						int num3 = currentPOIIndex;
						currentPOIIndex = num3 + 1;
					}
					else
					{
						if (Vector3.Distance(this.center, compassProPOI.transform.position) > num2)
						{
							break;
						}
						if (!soundPlayed)
						{
							soundPlayed = true;
							if (compassProPOI.scanHitAudioClip != null)
							{
								AudioSource.PlayClipAtPoint(compassProPOI.scanHitAudioClip, compassProPOI.transform.position);
							}
							else if (this.audioClipHit != null)
							{
								AudioSource.PlayClipAtPoint(this.audioClipHit, compassProPOI.transform.position);
							}
						}
						this.OnScanHit.Invoke(this, compassProPOI, base.transform);
						int num3 = currentPOIIndex;
						currentPOIIndex = num3 + 1;
					}
				}
				if (this.fadeOutDuration > 0f)
				{
					float num4 = (num - this.fadeOutStart) / this.fadeOutDuration;
					num4 = Mathf.Clamp01(num4);
					this.mat.SetFloat(ShaderParams.Alpha, 1f - num4);
					if (num4 >= 1f)
					{
						break;
					}
				}
				yield return null;
			}
			while (t < 1f);
			if (this.fadeOutDuration > 0f)
			{
				while (!(base.gameObject == null))
				{
					float num5 = Time.time - start;
					t = (num5 - this.fadeOutStart) / this.fadeOutDuration;
					t = Mathf.Clamp01(t);
					this.mat.SetFloat(ShaderParams.Alpha, 1f - t);
					yield return null;
					if (t >= 1f)
					{
						break;
					}
				}
			}
			this.ScanEnd();
			yield break;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0007D5B1 File Offset: 0x0007B7B1
		private void ScanEnd()
		{
			if (base.gameObject == null)
			{
				return;
			}
			this.OnScanHit.RemoveAllListeners();
			base.gameObject.SetActive(false);
			this.OnScanEnd.Invoke(this);
			this.OnScanEnd.RemoveAllListeners();
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x0007D5F0 File Offset: 0x0007B7F0
		private static GameObject GetRootContainer()
		{
			if (ScanEffect.rootContainer == null)
			{
				ScanEffect.rootContainer = GameObject.Find("Scan_Pool");
				if (ScanEffect.rootContainer == null)
				{
					ScanEffect.rootContainer = new GameObject("Scan_Pool");
					Object.DontDestroyOnLoad(ScanEffect.rootContainer);
				}
			}
			return ScanEffect.rootContainer;
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0007D644 File Offset: 0x0007B844
		public static ScanEffect GetInstanceFromPool()
		{
			int count = ScanEffect.pool.Count;
			ScanEffect scanEffect;
			for (int i = 0; i < count; i++)
			{
				scanEffect = ScanEffect.pool[i];
				if (scanEffect != null && !scanEffect.isActiveAndEnabled)
				{
					scanEffect.Reset();
					return scanEffect;
				}
			}
			if (ScanEffect.prefab == null)
			{
				ScanEffect.prefab = Resources.Load<GameObject>("CNPro/Prefabs/ScanSphere");
				if (ScanEffect.prefab == null)
				{
					Debug.LogError("ScanSphere prefab not found!");
					return null;
				}
			}
			GameObject gameObject = Object.Instantiate<GameObject>(ScanEffect.prefab);
			gameObject.name = "Sonar FX";
			gameObject.transform.SetParent(ScanEffect.GetRootContainer().transform);
			scanEffect = gameObject.GetComponent<ScanEffect>();
			ScanEffect.pool.Add(scanEffect);
			return scanEffect;
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x0007D6FE File Offset: 0x0007B8FE
		public static void RemoveInstanceFromPool(ScanEffect sonar)
		{
			if (ScanEffect.pool.Contains(sonar))
			{
				ScanEffect.pool.Remove(sonar);
			}
		}

		// Token: 0x040014D2 RID: 5330
		public Vector3 center;

		// Token: 0x040014D3 RID: 5331
		public Color color = new Color(0f, 0.5f, 1f, 0.2f);

		// Token: 0x040014D4 RID: 5332
		public LayerMask scanLayerMask = -1;

		// Token: 0x040014D5 RID: 5333
		public float width = 100f;

		// Token: 0x040014D6 RID: 5334
		public float maxRange = 800f;

		// Token: 0x040014D7 RID: 5335
		public float duration = 2f;

		// Token: 0x040014D8 RID: 5336
		public float fadeOutStart = 2f;

		// Token: 0x040014D9 RID: 5337
		public float fadeOutDuration = 1f;

		// Token: 0x040014DA RID: 5338
		public bool grid = true;

		// Token: 0x040014DB RID: 5339
		public Color gridColor = new Color(1f, 1f, 0f);

		// Token: 0x040014DC RID: 5340
		public float gridSize = 0.1f;

		// Token: 0x040014DD RID: 5341
		public float gridDepthWidthControl = 150f;

		// Token: 0x040014DE RID: 5342
		public float gridDepthStart = 100f;

		// Token: 0x040014DF RID: 5343
		public bool filled;

		// Token: 0x040014E0 RID: 5344
		public bool scanLines = true;

		// Token: 0x040014E1 RID: 5345
		public bool noise = true;

		// Token: 0x040014E2 RID: 5346
		public bool edgeOutline = true;

		// Token: 0x040014E3 RID: 5347
		public Color edgeColor = Color.white;

		// Token: 0x040014E4 RID: 5348
		public float edgeDepthThreshold = 0.01f;

		// Token: 0x040014E5 RID: 5349
		public float edgeColorThreshold = 0.1f;

		// Token: 0x040014E6 RID: 5350
		public float edgeStrength = 2f;

		// Token: 0x040014E7 RID: 5351
		public float edgeWidth = 1f;

		// Token: 0x040014E8 RID: 5352
		[ColorUsage(true, false)]
		public Color colorBlend = new Color(0f, 0f, 0f, 0.75f);

		// Token: 0x040014E9 RID: 5353
		public AudioClip audioClipStart;

		// Token: 0x040014EA RID: 5354
		public AudioClip audioClipHit;

		// Token: 0x040014EB RID: 5355
		private Material mat;

		// Token: 0x040014EC RID: 5356
		public UnityEvent<ScanEffect, CompassProPOI, Transform> OnScanHit;

		// Token: 0x040014ED RID: 5357
		public UnityEvent<ScanEffect> OnScanEnd;

		// Token: 0x040014EE RID: 5358
		public readonly List<CompassProPOI> pois = new List<CompassProPOI>();

		// Token: 0x040014EF RID: 5359
		private const string POOL_CONTAINER_NAME = "Scan_Pool";

		// Token: 0x040014F0 RID: 5360
		private static List<ScanEffect> pool = new List<ScanEffect>();

		// Token: 0x040014F1 RID: 5361
		private static GameObject prefab;

		// Token: 0x040014F2 RID: 5362
		private static GameObject rootContainer;
	}
}
