using System;
using System.Collections.Generic;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Features.SecurityCamera
{
	// Token: 0x02000C5C RID: 3164
	[DisallowMultipleComponent]
	public class CctvOverlayHud : MonoBehaviour
	{
		// Token: 0x06005B88 RID: 23432 RVA: 0x0023DFD4 File Offset: 0x0023C1D4
		private void OnEnable()
		{
			TMP_Text tmp_Text = this.recIndicator as TMP_Text;
			if (tmp_Text != null)
			{
				LocalizedGameText.BindSource(tmp_Text, "prison_ui", "REC", null).FitTranslationToRectangle(false);
			}
			else
			{
				Text text = this.recIndicator as Text;
				if (text != null)
				{
					LocalizedLegacyGameText.BindSource(text, "prison_ui", "REC").FitTranslationToRectangle();
				}
			}
			if (this.timestampText != null)
			{
				LocalizedGameText.BindFontOnly(this.timestampText);
			}
			this._t = 0f;
			if (this.recIndicator != null)
			{
				CctvOverlayHud.SetAlpha(this.recIndicator, 1f);
			}
		}

		// Token: 0x06005B89 RID: 23433 RVA: 0x0023E070 File Offset: 0x0023C270
		private void Update()
		{
			this._t += Time.unscaledDeltaTime;
			if (this.recIndicator != null)
			{
				float num = ((this._t * this.recBlinkHz % 1f < 0.5f) ? 1f : 0.15f);
				CctvOverlayHud.SetAlpha(this.recIndicator, num);
			}
			if (this.timestampText != null)
			{
				this.timestampText.text = DateTime.Now.ToString(this.timestampFormat);
			}
			if (this.scanlines != null)
			{
				Rect uvRect = this.scanlines.uvRect;
				uvRect.y = (uvRect.y - this.scanlineScrollSpeed * Time.unscaledDeltaTime) % 1f;
				this.scanlines.uvRect = uvRect;
			}
		}

		// Token: 0x06005B8A RID: 23434 RVA: 0x0023E144 File Offset: 0x0023C344
		private static void SetAlpha(Graphic g, float a)
		{
			Color color = g.color;
			color.a = a;
			g.color = color;
		}

		// Token: 0x06005B8B RID: 23435 RVA: 0x0023E168 File Offset: 0x0023C368
		public void RenderCaptureOverlay(RenderTexture destination, Camera sourceCamera)
		{
			Canvas component = base.GetComponent<Canvas>();
			RectTransform rectTransform = base.transform as RectTransform;
			if (destination == null || component == null || rectTransform == null || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			Color color = ((this.recIndicator != null) ? this.recIndicator.color : Color.clear);
			string text = ((this.timestampText != null) ? this.timestampText.text : null);
			RenderTexture active = RenderTexture.active;
			List<Mesh> list = new List<Mesh>();
			Dictionary<Material, Material> dictionary = new Dictionary<Material, Material>();
			CommandBuffer commandBuffer = new CommandBuffer
			{
				name = "CCTV still overlay"
			};
			try
			{
				if (this.recIndicator != null)
				{
					CctvOverlayHud.SetAlpha(this.recIndicator, 1f);
				}
				if (this.timestampText != null)
				{
					this.timestampText.text = DateTime.Now.ToString(this.timestampFormat);
				}
				Canvas.ForceUpdateCanvases();
				TMP_Text[] componentsInChildren = base.GetComponentsInChildren<TMP_Text>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].ForceMeshUpdate(false, false);
				}
				Rect rect = rectTransform.rect;
				if (rect.width > 0f && rect.height > 0f)
				{
					commandBuffer.SetRenderTarget(destination);
					commandBuffer.SetViewport(new Rect(0f, 0f, (float)destination.width, (float)destination.height));
					commandBuffer.DisableScissorRect();
					commandBuffer.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.Ortho(rect.xMin, rect.xMax, rect.yMin, rect.yMax, -100f, 100f));
					MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
					foreach (Graphic graphic in base.GetComponentsInChildren<Graphic>())
					{
						if (graphic.isActiveAndEnabled && !graphic.canvasRenderer.cull)
						{
							Mesh mesh = CctvOverlayHud.CopyCaptureMesh(graphic.canvasRenderer);
							if (!(mesh == null))
							{
								list.Add(mesh);
								if (mesh.vertexCount != 0)
								{
									materialPropertyBlock.Clear();
									Material materialForRendering = graphic.materialForRendering;
									materialPropertyBlock.SetTexture("_MainTex", (graphic is TMP_Text || graphic is TMP_SubMeshUI) ? materialForRendering.mainTexture : graphic.mainTexture);
									Material material;
									if (!dictionary.TryGetValue(materialForRendering, out material))
									{
										material = new Material(materialForRendering)
										{
											hideFlags = HideFlags.HideAndDontSave
										};
										material.SetInt("unity_GUIZTestMode", 8);
										dictionary.Add(materialForRendering, material);
									}
									commandBuffer.DrawMesh(mesh, rectTransform.worldToLocalMatrix * graphic.transform.localToWorldMatrix, material, 0, 0, materialPropertyBlock);
								}
							}
						}
					}
					if (sourceCamera != null)
					{
						commandBuffer.SetViewProjectionMatrices(sourceCamera.worldToCameraMatrix, sourceCamera.projectionMatrix);
						commandBuffer.SetViewport(sourceCamera.pixelRect);
					}
					Graphics.ExecuteCommandBuffer(commandBuffer);
				}
			}
			finally
			{
				if (this.recIndicator != null)
				{
					this.recIndicator.color = color;
				}
				if (this.timestampText != null)
				{
					this.timestampText.text = text;
				}
				commandBuffer.Release();
				foreach (Mesh mesh2 in list)
				{
					if (Application.isPlaying)
					{
						Object.Destroy(mesh2);
					}
					else
					{
						Object.DestroyImmediate(mesh2);
					}
				}
				foreach (Material material2 in dictionary.Values)
				{
					if (Application.isPlaying)
					{
						Object.Destroy(material2);
					}
					else
					{
						Object.DestroyImmediate(material2);
					}
				}
				RenderTexture.active = active;
			}
		}

		// Token: 0x06005B8C RID: 23436 RVA: 0x0023E58C File Offset: 0x0023C78C
		private static Mesh CopyCaptureMesh(CanvasRenderer renderer)
		{
			Mesh mesh = renderer.GetMesh();
			if (mesh == null)
			{
				return null;
			}
			Mesh mesh2;
			try
			{
				if (mesh.vertexCount == 0)
				{
					mesh2 = null;
				}
				else
				{
					Mesh mesh3 = new Mesh
					{
						name = "CCTV capture geometry",
						hideFlags = HideFlags.HideAndDontSave,
						indexFormat = mesh.indexFormat
					};
					mesh3.vertices = mesh.vertices;
					mesh3.colors32 = mesh.colors32;
					mesh3.normals = mesh.normals;
					mesh3.tangents = mesh.tangents;
					List<Vector4> list = new List<Vector4>();
					for (int i = 0; i < 4; i++)
					{
						mesh.GetUVs(i, list);
						if (list.Count > 0)
						{
							mesh3.SetUVs(i, list);
						}
					}
					mesh3.subMeshCount = mesh.subMeshCount;
					for (int j = 0; j < mesh.subMeshCount; j++)
					{
						mesh3.SetIndices(mesh.GetIndices(j), mesh.GetTopology(j), j);
					}
					mesh2 = mesh3;
				}
			}
			finally
			{
				if (Application.isPlaying)
				{
					Object.Destroy(mesh);
				}
				else
				{
					Object.DestroyImmediate(mesh);
				}
			}
			return mesh2;
		}

		// Token: 0x04005CB4 RID: 23732
		[Header("REC blink")]
		[Tooltip("Graphic (Image o testo) del 'REC' / pallino che deve lampeggiare.")]
		[SerializeField]
		private Graphic recIndicator;

		// Token: 0x04005CB5 RID: 23733
		[Tooltip("Frequenza del lampeggio in Hz (cicli al secondo).")]
		[SerializeField]
		private float recBlinkHz = 1f;

		// Token: 0x04005CB6 RID: 23734
		[Header("Timestamp")]
		[Tooltip("Testo TMP del timestamp. Avanza in tempo reale mentre l'overlay e' attivo.")]
		[SerializeField]
		private TMP_Text timestampText;

		// Token: 0x04005CB7 RID: 23735
		[Tooltip("Formato del timestamp (DateTime.ToString).")]
		[SerializeField]
		private string timestampFormat = "yyyy-MM-dd  HH:mm:ss";

		// Token: 0x04005CB8 RID: 23736
		[Header("Scanlines")]
		[Tooltip("RawImage con la texture a righe (tiling verticale). Scorre lentamente.")]
		[SerializeField]
		private RawImage scanlines;

		// Token: 0x04005CB9 RID: 23737
		[Tooltip("Velocita' di scorrimento verticale delle scanline (uv/sec).")]
		[SerializeField]
		private float scanlineScrollSpeed = 0.15f;

		// Token: 0x04005CBA RID: 23738
		private float _t;
	}
}
