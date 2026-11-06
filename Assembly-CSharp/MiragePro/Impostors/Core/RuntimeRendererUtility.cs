using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiragePro.Impostors.Core
{
	// Token: 0x0200027D RID: 637
	public class RuntimeRendererUtility
	{
		// Token: 0x060010A3 RID: 4259 RVA: 0x0006F1F4 File Offset: 0x0006D3F4
		public RuntimeRendererUtility()
		{
			this.size = 1024 * Vector2Int.one;
			this.renderLayer = PlayerPrefs.GetInt("Mirage_RuntimeBakingRenderLayer", 30);
			GameObject gameObject = new GameObject("RuntimeIsoCam");
			this.camera = gameObject.AddComponent<Camera>();
			this.camera.cullingMask = 1 << this.renderLayer;
			this.camera.orthographic = true;
			this.camera.orthographicSize = 1f;
			this.camera.clearFlags = CameraClearFlags.Color;
			this.camera.backgroundColor = Color.black;
			this.camera.nearClipPlane = -50f;
			this.camera.farClipPlane = 50f;
			this.camera.allowHDR = false;
			this.camera.allowMSAA = false;
			this.DisableSceneLightsForRenderLayer(this.renderLayer);
			this.L0 = new GameObject("L0");
			this.lights[0] = this.L0.AddComponent<Light>();
			this.lights[0].renderingLayerMask = 1 << this.renderLayer;
			this.L1 = new GameObject("L1");
			this.lights[1] = this.L1.AddComponent<Light>();
			this.lights[1].transform.rotation = Quaternion.Euler(340f, 218f, 177f);
			this.lights[1].color = new Color(0.4f, 0.4f, 0.45f, 0f) * 0.7f;
			this.lights[1].renderingLayerMask = 1 << this.renderLayer;
			this.rt = new RenderTexture(this.size.x, this.size.y, 24, RenderTextureFormat.ARGB32);
			this.rt.Create();
			this.camera.targetTexture = this.rt;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0006F403 File Offset: 0x0006D603
		public void AddSingleGO(GameObject go)
		{
			this.additionalGO.Add(go);
			go.layer = this.renderLayer;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0006F420 File Offset: 0x0006D620
		private void DisableSceneLightsForRenderLayer(int renderLayer)
		{
			Light[] array = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].cullingMask &= ~(1 << renderLayer);
			}
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0006F458 File Offset: 0x0006D658
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, MaterialPropertyBlock customProperties, Transform probeAnchor, bool useLightProbe)
		{
			Graphics.DrawMesh(mesh, matrix, material, this.renderLayer, this.camera, submeshIndex);
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0006F470 File Offset: 0x0006D670
		public void BeginStaticPreview(Rect rect)
		{
			this.size = new Vector2Int((int)rect.size.x, (int)rect.size.y);
			this.tex = new Texture2D(this.size.x, this.size.y, TextureFormat.RGBA32, false);
			if (this.rt != null)
			{
				this.camera.targetTexture = null;
				this.rt.Release();
				Object.DestroyImmediate(this.rt);
			}
			this.rt = new RenderTexture(this.size.x, this.size.y, 24, RenderTextureFormat.ARGB32);
			this.rt.Create();
			this.camera.targetTexture = this.rt;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0006F538 File Offset: 0x0006D738
		public void Render(bool _ = true)
		{
			this.camera.Render();
			RenderTexture.active = this.rt;
			this.tex.ReadPixels(new Rect(0f, 0f, (float)this.size.x, (float)this.size.y), 0, 0);
			this.tex.Apply();
			RenderTexture.active = null;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0006F5A0 File Offset: 0x0006D7A0
		public Texture2D EndStaticPreview()
		{
			return this.tex;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0006F5A8 File Offset: 0x0006D7A8
		public void Cleanup()
		{
			if (this.rt != null)
			{
				this.camera.targetTexture = null;
				this.rt.Release();
				Object.DestroyImmediate(this.rt);
			}
			if (this.camera != null)
			{
				Object.DestroyImmediate(this.camera.gameObject);
			}
			foreach (Light light in this.lights)
			{
				if (light != null)
				{
					Object.DestroyImmediate(light.gameObject);
				}
			}
			foreach (GameObject gameObject in this.additionalGO)
			{
				if (gameObject != null)
				{
					Object.DestroyImmediate(gameObject);
				}
			}
		}

		// Token: 0x04001215 RID: 4629
		public Camera camera;

		// Token: 0x04001216 RID: 4630
		public Light[] lights = new Light[2];

		// Token: 0x04001217 RID: 4631
		public Color ambientColor;

		// Token: 0x04001218 RID: 4632
		private GameObject L0;

		// Token: 0x04001219 RID: 4633
		private GameObject L1;

		// Token: 0x0400121A RID: 4634
		private int renderLayer = 30;

		// Token: 0x0400121B RID: 4635
		private RenderTexture rt;

		// Token: 0x0400121C RID: 4636
		private Vector2Int size;

		// Token: 0x0400121D RID: 4637
		private Texture2D tex;

		// Token: 0x0400121E RID: 4638
		private List<GameObject> additionalGO = new List<GameObject>();
	}
}
