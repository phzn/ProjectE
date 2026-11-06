using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bozo.AnimeCharacters;
using ProjectExhibitionist;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.BodyPaintStudio
{
	// Token: 0x02001507 RID: 5383
	public class BodyPaintCanvasService : MonoBehaviour
	{
		// Token: 0x170019A2 RID: 6562
		// (get) Token: 0x0600A637 RID: 42551 RVA: 0x004574E8 File Offset: 0x004556E8
		// (set) Token: 0x0600A638 RID: 42552 RVA: 0x004574F0 File Offset: 0x004556F0
		public bool IsApplyingPaint { get; private set; }

		// Token: 0x170019A3 RID: 6563
		// (get) Token: 0x0600A639 RID: 42553 RVA: 0x004574F9 File Offset: 0x004556F9
		public float RainFadeAmount
		{
			get
			{
				return this._rainFadeAmount;
			}
		}

		// Token: 0x170019A4 RID: 6564
		// (get) Token: 0x0600A63A RID: 42554 RVA: 0x00457501 File Offset: 0x00455701
		public float PaintOpacity
		{
			get
			{
				if (!this.IsApplyingPaint)
				{
					return (1f - this._rainFadeAmount) * (1f - this._lifetimeFadeAmount);
				}
				return 1f;
			}
		}

		// Token: 0x0600A63B RID: 42555 RVA: 0x0045752A File Offset: 0x0045572A
		public void BeginPaintApplication()
		{
			this.IsApplyingPaint = true;
			this.ApplyPaintOpacity();
		}

		// Token: 0x0600A63C RID: 42556 RVA: 0x00457539 File Offset: 0x00455739
		public void CompletePaintApplication()
		{
			if (!this.IsApplyingPaint)
			{
				return;
			}
			this.IsApplyingPaint = false;
			this._rainFadeAmount = 0f;
			this._lifetimeFadeAmount = 0f;
			this.ApplyPaintOpacity();
		}

		// Token: 0x170019A5 RID: 6565
		// (get) Token: 0x0600A63D RID: 42557 RVA: 0x00457567 File Offset: 0x00455767
		// (set) Token: 0x0600A63E RID: 42558 RVA: 0x0045756F File Offset: 0x0045576F
		public float PaintedCoverage { get; private set; }

		// Token: 0x170019A6 RID: 6566
		// (get) Token: 0x0600A63F RID: 42559 RVA: 0x00457578 File Offset: 0x00455778
		public float DetectionCoverage
		{
			get
			{
				return 9f * this.PaintedCoverage * this.PaintOpacity;
			}
		}

		// Token: 0x170019A7 RID: 6567
		// (get) Token: 0x0600A640 RID: 42560 RVA: 0x0045758D File Offset: 0x0045578D
		public Vector3 ColliderOffset
		{
			get
			{
				return this.colliderOffset;
			}
		}

		// Token: 0x170019A8 RID: 6568
		// (get) Token: 0x0600A641 RID: 42561 RVA: 0x00457595 File Offset: 0x00455795
		// (set) Token: 0x0600A642 RID: 42562 RVA: 0x0045759D File Offset: 0x0045579D
		public bool HasAnyPaint { get; private set; }

		// Token: 0x1400011F RID: 287
		// (add) Token: 0x0600A643 RID: 42563 RVA: 0x004575A8 File Offset: 0x004557A8
		// (remove) Token: 0x0600A644 RID: 42564 RVA: 0x004575E0 File Offset: 0x004557E0
		public event Action<bool> OnPaintStateChanged;

		// Token: 0x0600A645 RID: 42565 RVA: 0x00457615 File Offset: 0x00455815
		public SkinnedMeshRenderer GetSourceRenderer(BodyPaintCanvasService.PaintChannel channel)
		{
			return this.ResolveRenderer(channel);
		}

		// Token: 0x0600A646 RID: 42566 RVA: 0x0045761E File Offset: 0x0045581E
		private void Awake()
		{
			if (this.skinDecalController == null)
			{
				this.skinDecalController = base.GetComponentInParent<SkinDecalController>();
			}
		}

		// Token: 0x0600A647 RID: 42567 RVA: 0x0045763C File Offset: 0x0045583C
		private SkinnedMeshRenderer ResolveRenderer(BodyPaintCanvasService.PaintChannel channel)
		{
			if (channel == BodyPaintCanvasService.PaintChannel.Body)
			{
				if (this.bodyRendererOverride != null)
				{
					return this.bodyRendererOverride;
				}
				if (!(this.skinDecalController != null))
				{
					return null;
				}
				return this.skinDecalController.bodyRenderer;
			}
			else
			{
				if (this.headRendererOverride != null)
				{
					return this.headRendererOverride;
				}
				if (!(this.skinDecalController != null))
				{
					return null;
				}
				return this.skinDecalController.headRenderer;
			}
		}

		// Token: 0x0600A648 RID: 42568 RVA: 0x004576B0 File Offset: 0x004558B0
		public void EnsureOverlays()
		{
			if (this._overlaysBuilt)
			{
				return;
			}
			if (this._overlayContainer == null)
			{
				this._overlayContainer = new GameObject("BodyPaintOverlays").transform;
				this._overlayContainer.SetParent(base.transform, false);
				this._overlayContainer.localPosition = Vector3.zero;
				this._overlayContainer.localRotation = Quaternion.identity;
				this._overlayContainer.localScale = Vector3.one;
			}
			this.TryBuildChannel(BodyPaintCanvasService.PaintChannel.Body, this.bodyTextureSize);
			this.TryBuildChannel(BodyPaintCanvasService.PaintChannel.Head, this.headTextureSize);
			this._overlaysBuilt = this._channels.Count > 0;
			if (!this._overlaysBuilt)
			{
				Debug.LogWarning("[BodyPaintCanvasService] Nessun renderer disponibile: overlay non creati.");
			}
		}

		// Token: 0x0600A649 RID: 42569 RVA: 0x0045776C File Offset: 0x0045596C
		private void TryBuildChannel(BodyPaintCanvasService.PaintChannel id, int textureSize)
		{
			BodyPaintCanvasService.<>c__DisplayClass56_0 CS$<>8__locals1;
			CS$<>8__locals1.textureSize = textureSize;
			SkinnedMeshRenderer skinnedMeshRenderer = this.ResolveRenderer(id);
			if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null)
			{
				return;
			}
			RenderTexture renderTexture = BodyPaintCanvasService.<TryBuildChannel>g__MakeRT|56_0(string.Format("BodyPaintRT_{0}", id), RenderTextureFormat.ARGB32, ref CS$<>8__locals1);
			RenderTexture renderTexture2 = BodyPaintCanvasService.<TryBuildChannel>g__MakeRT|56_0(string.Format("BodyPaintDisplayRT_{0}", id), RenderTextureFormat.ARGB32, ref CS$<>8__locals1);
			RenderTexture renderTexture3 = BodyPaintCanvasService.<TryBuildChannel>g__MakeRT|56_0(string.Format("BodyPaintMaskRT_{0}", id), RenderTextureFormat.R8, ref CS$<>8__locals1);
			Shader shader = Shader.Find("ProjectExhibitionist/BodyPaintOverlay");
			if (shader == null)
			{
				Debug.LogError("[BodyPaintCanvasService] Shader 'ProjectExhibitionist/BodyPaintOverlay' non trovato!");
				renderTexture.Release();
				Object.Destroy(renderTexture);
				renderTexture2.Release();
				Object.Destroy(renderTexture2);
				renderTexture3.Release();
				Object.Destroy(renderTexture3);
				return;
			}
			Material material = new Material(shader)
			{
				name = string.Format("BodyPaintOverlayMat_{0}", id),
				renderQueue = ((id == BodyPaintCanvasService.PaintChannel.Head) ? 2008 : this.overlayRenderQueue)
			};
			material.SetTexture("_MainTex", renderTexture2);
			material.SetFloat("_Opacity", this.PaintOpacity);
			Mesh mesh = ((id == BodyPaintCanvasService.PaintChannel.Body) ? ((this.bodyPaintMesh != null) ? this.bodyPaintMesh : skinnedMeshRenderer.sharedMesh) : ((this.headPaintMesh != null) ? this.headPaintMesh : skinnedMeshRenderer.sharedMesh));
			bool[] array = new bool[mesh.subMeshCount];
			bool flag = false;
			Material[] sharedMaterials = skinnedMeshRenderer.sharedMaterials;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = true;
				if (i < sharedMaterials.Length && !(sharedMaterials[i] == null))
				{
					if (this.shellInvisibleMaterial != null && sharedMaterials[i] == this.shellInvisibleMaterial)
					{
						array[i] = false;
						flag = true;
					}
					else
					{
						foreach (string text in this.shellMaterialKeywords)
						{
							if (!string.IsNullOrEmpty(text) && sharedMaterials[i].name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
							{
								array[i] = false;
								flag = true;
								break;
							}
						}
					}
				}
			}
			string[] array3 = new string[6];
			array3[0] = string.Format("[BodyPaintCanvasService] Canale {0}: submesh dipingibili = ", id);
			array3[1] = "[";
			array3[2] = string.Join(",", Array.ConvertAll<bool, string>(array, delegate(bool p)
			{
				if (!p)
				{
					return "no";
				}
				return "SI";
			}));
			array3[3] = "] (materiali: ";
			array3[4] = string.Join("; ", Array.ConvertAll<Material, string>(sharedMaterials, delegate(Material m)
			{
				if (!(m == null))
				{
					return m.name;
				}
				return "null";
			}));
			array3[5] = ")";
			Debug.Log(string.Concat(array3));
			Material stampMaterial = this.StampMaterial;
			if (stampMaterial != null)
			{
				if (this._stampCommandBuffer == null)
				{
					this._stampCommandBuffer = new CommandBuffer
					{
						name = "BodyPaintStamp"
					};
				}
				this._stampCommandBuffer.Clear();
				this._stampCommandBuffer.SetRenderTarget(renderTexture3);
				for (int k = 0; k < mesh.subMeshCount; k++)
				{
					if (array[k])
					{
						this._stampCommandBuffer.DrawMesh(mesh, Matrix4x4.identity, stampMaterial, k, 2);
					}
				}
				Graphics.ExecuteCommandBuffer(this._stampCommandBuffer);
			}
			GameObject gameObject = new GameObject(string.Format("BodyPaintOverlay_{0}", id));
			gameObject.transform.SetParent(this._overlayContainer, false);
			gameObject.layer = skinnedMeshRenderer.gameObject.layer;
			SkinnedMeshRenderer skinnedMeshRenderer2 = gameObject.AddComponent<SkinnedMeshRenderer>();
			skinnedMeshRenderer2.sharedMesh = mesh;
			skinnedMeshRenderer2.bones = skinnedMeshRenderer.bones;
			skinnedMeshRenderer2.rootBone = skinnedMeshRenderer.rootBone;
			skinnedMeshRenderer2.localBounds = skinnedMeshRenderer.localBounds;
			Material[] array4 = new Material[mesh.subMeshCount];
			for (int l = 0; l < array4.Length; l++)
			{
				array4[l] = ((array[l] || this.shellInvisibleMaterial == null) ? material : this.shellInvisibleMaterial);
			}
			skinnedMeshRenderer2.sharedMaterials = array4;
			gameObject.AddComponent<BlendShapeSynchronizer>().SetSourceRenderer(skinnedMeshRenderer);
			this._channels.Add(new BodyPaintCanvasService.Channel
			{
				id = id,
				sourceRenderer = skinnedMeshRenderer,
				overlayRenderer = skinnedMeshRenderer2,
				paintRT = renderTexture,
				displayRT = renderTexture2,
				maskRT = renderTexture3,
				overlayGO = gameObject,
				overlayMaterial = material,
				paintableSubmesh = array,
				hasShellSubmesh = flag
			});
		}

		// Token: 0x0600A64A RID: 42570 RVA: 0x00457BE8 File Offset: 0x00455DE8
		private static void ClearRT(RenderTexture rt)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = rt;
			GL.Clear(true, true, Color.clear);
			RenderTexture.active = active;
		}

		// Token: 0x0600A64B RID: 42571 RVA: 0x00457C08 File Offset: 0x00455E08
		public void ClearAllPaint()
		{
			foreach (BodyPaintCanvasService.Channel channel in this._channels)
			{
				if (channel.paintRT != null)
				{
					BodyPaintCanvasService.ClearRT(channel.paintRT);
				}
				if (channel.displayRT != null)
				{
					BodyPaintCanvasService.ClearRT(channel.displayRT);
				}
				channel.displayDirty = false;
				channel.coverageDirty = false;
				channel.paintedFraction = 0f;
			}
			this.PaintedCoverage = 0f;
			this.SetHasAnyPaint(false);
			this.SetLifetimeFade(0f);
			this.SetRainFade(0f);
		}

		// Token: 0x0600A64C RID: 42572 RVA: 0x00457CC8 File Offset: 0x00455EC8
		public void SetRainFade(float amount)
		{
			this._rainFadeAmount = Mathf.Clamp01(amount);
			this.ApplyPaintOpacity();
		}

		// Token: 0x0600A64D RID: 42573 RVA: 0x00457CDC File Offset: 0x00455EDC
		public void SetLifetimeFade(float amount)
		{
			this._lifetimeFadeAmount = Mathf.Clamp01(amount);
			this.ApplyPaintOpacity();
		}

		// Token: 0x0600A64E RID: 42574 RVA: 0x00457CF0 File Offset: 0x00455EF0
		private void ApplyPaintOpacity()
		{
			foreach (BodyPaintCanvasService.Channel channel in this._channels)
			{
				Material overlayMaterial = channel.overlayMaterial;
				if (!(overlayMaterial == null) && overlayMaterial.HasProperty("_Opacity"))
				{
					overlayMaterial.SetFloat("_Opacity", this.PaintOpacity);
				}
			}
		}

		// Token: 0x0600A64F RID: 42575 RVA: 0x00457D68 File Offset: 0x00455F68
		private void SetHasAnyPaint(bool value)
		{
			if (this.HasAnyPaint == value)
			{
				return;
			}
			this.HasAnyPaint = value;
			Action<bool> onPaintStateChanged = this.OnPaintStateChanged;
			if (onPaintStateChanged == null)
			{
				return;
			}
			onPaintStateChanged(value);
		}

		// Token: 0x0600A650 RID: 42576 RVA: 0x00457D8C File Offset: 0x00455F8C
		public void RefreshCoverage()
		{
			float num = 0f;
			foreach (BodyPaintCanvasService.Channel channel in this._channels)
			{
				if (channel.coverageDirty && channel.paintRT != null && channel.maskRT != null)
				{
					channel.paintedFraction = BodyPaintCanvasService.MeasureCoverage(channel.paintRT, channel.maskRT);
					channel.coverageDirty = false;
				}
				float num2 = ((channel.id == BodyPaintCanvasService.PaintChannel.Body) ? 0.9f : 0.1f);
				num += channel.paintedFraction * num2;
			}
			this.PaintedCoverage = Mathf.Clamp01(num);
			this.SetHasAnyPaint(this.PaintedCoverage > 0f);
		}

		// Token: 0x0600A651 RID: 42577 RVA: 0x00457E60 File Offset: 0x00456060
		public static float MeasureCoverage(RenderTexture paint, RenderTexture skinMask)
		{
			if (paint == null || skinMask == null)
			{
				return 0f;
			}
			RenderTexture active = RenderTexture.active;
			RenderTexture temporary = RenderTexture.GetTemporary(128, 128, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			Texture2D texture2D = new Texture2D(128, 128, TextureFormat.RGBA32, false, true);
			float num3;
			try
			{
				Graphics.Blit(paint, temporary);
				RenderTexture.active = temporary;
				texture2D.ReadPixels(new Rect(0f, 0f, 128f, 128f), 0, 0, false);
				Color32[] pixels = texture2D.GetPixels32();
				Graphics.Blit(skinMask, temporary);
				RenderTexture.active = temporary;
				texture2D.ReadPixels(new Rect(0f, 0f, 128f, 128f), 0, 0, false);
				Color32[] pixels2 = texture2D.GetPixels32();
				double num = 0.0;
				double num2 = 0.0;
				for (int i = 0; i < pixels2.Length; i++)
				{
					num2 += (double)pixels2[i].r;
					num += (double)Math.Min(pixels[i].a, pixels2[i].r);
				}
				num3 = ((num2 > 0.0) ? Mathf.Clamp01((float)(num / num2)) : 0f);
			}
			finally
			{
				RenderTexture.active = active;
				RenderTexture.ReleaseTemporary(temporary);
				if (Application.isPlaying)
				{
					Object.Destroy(texture2D);
				}
				else
				{
					Object.DestroyImmediate(texture2D);
				}
			}
			return num3;
		}

		// Token: 0x0600A652 RID: 42578 RVA: 0x00457FD8 File Offset: 0x004561D8
		public void BuildPaintColliders(BodyPaintCanvasService.PaintChannel? only = null)
		{
			foreach (BodyPaintCanvasService.Channel channel in this._channels)
			{
				if (!(channel.sourceRenderer == null) && (only == null || channel.id == only.Value))
				{
					if (channel.bakedMesh == null)
					{
						channel.bakedMesh = new Mesh
						{
							name = string.Format("BodyPaintBaked_{0}", channel.id)
						};
					}
					SkinnedMeshRenderer skinnedMeshRenderer = ((channel.overlayRenderer != null) ? channel.overlayRenderer : channel.sourceRenderer);
					skinnedMeshRenderer.BakeMesh(channel.bakedMesh, true);
					if (channel.colliderGO == null)
					{
						channel.colliderGO = new GameObject(string.Format("BodyPaintCollider_{0}", channel.id));
						channel.colliderGO.layer = 2;
						channel.collider = channel.colliderGO.AddComponent<MeshCollider>();
						channel.collider.cookingOptions = MeshColliderCookingOptions.UseFastMidphase;
						channel.colliderGO.AddComponent<Rigidbody>().isKinematic = true;
						channel.collider.contactOffset = 0.003f;
					}
					Transform transform = skinnedMeshRenderer.transform;
					channel.colliderGO.transform.SetPositionAndRotation(transform.position + this.colliderOffset, transform.rotation);
					channel.colliderGO.transform.localScale = transform.lossyScale;
					Mesh mesh = channel.bakedMesh;
					if (channel.hasShellSubmesh)
					{
						bool flag = channel.colliderMesh == null || channel.colliderMesh.vertexCount != channel.bakedMesh.vertexCount;
						if (channel.colliderMesh == null)
						{
							channel.colliderMesh = new Mesh
							{
								name = string.Format("BodyPaintBakedSkin_{0}", channel.id)
							};
						}
						if (flag)
						{
							channel.colliderMesh.Clear();
							channel.colliderMesh.indexFormat = channel.bakedMesh.indexFormat;
							channel.colliderMesh.vertices = channel.bakedMesh.vertices;
							channel.colliderMesh.uv = channel.bakedMesh.uv;
							List<int> list = new List<int>();
							for (int i = 0; i < channel.bakedMesh.subMeshCount; i++)
							{
								if (channel.paintableSubmesh == null || i >= channel.paintableSubmesh.Length || channel.paintableSubmesh[i])
								{
									list.AddRange(channel.bakedMesh.GetTriangles(i));
								}
							}
							channel.colliderMesh.SetTriangles(list, 0);
						}
						else
						{
							channel.bakedMesh.GetVertices(this._vertexScratch);
							channel.colliderMesh.SetVertices(this._vertexScratch);
							channel.colliderMesh.RecalculateBounds();
						}
						mesh = channel.colliderMesh;
					}
					channel.collider.sharedMesh = null;
					channel.collider.sharedMesh = mesh;
					channel.collider.contactOffset = 0.003f;
					channel.colliderGO.SetActive(true);
				}
			}
		}

		// Token: 0x0600A653 RID: 42579 RVA: 0x0045830C File Offset: 0x0045650C
		public void DestroyPaintColliders()
		{
			foreach (BodyPaintCanvasService.Channel channel in this._channels)
			{
				if (channel.colliderGO != null)
				{
					Object.Destroy(channel.colliderGO);
					channel.colliderGO = null;
					channel.collider = null;
				}
			}
		}

		// Token: 0x0600A654 RID: 42580 RVA: 0x00458380 File Offset: 0x00456580
		public bool RaycastPaintSurface(Ray worldRay, out BodyPaintCanvasService.PaintChannel channel, out Vector2 uv, out Vector3 worldPoint)
		{
			Vector3 vector;
			return this.RaycastPaintSurface(worldRay, out channel, out uv, out worldPoint, out vector);
		}

		// Token: 0x0600A655 RID: 42581 RVA: 0x0045839C File Offset: 0x0045659C
		public bool RaycastPaintSurface(Ray worldRay, out BodyPaintCanvasService.PaintChannel channel, out Vector2 uv, out Vector3 worldPoint, out Vector3 worldNormal)
		{
			channel = BodyPaintCanvasService.PaintChannel.Body;
			uv = Vector2.zero;
			worldPoint = Vector3.zero;
			worldNormal = Vector3.up;
			Ray ray = new Ray(worldRay.origin + this.colliderOffset, worldRay.direction);
			float num = float.MaxValue;
			bool flag = false;
			foreach (BodyPaintCanvasService.Channel channel2 in this._channels)
			{
				RaycastHit raycastHit;
				if (!(channel2.collider == null) && channel2.colliderGO.activeInHierarchy && channel2.collider.Raycast(ray, out raycastHit, 100f) && raycastHit.distance < num)
				{
					num = raycastHit.distance;
					channel = channel2.id;
					uv = raycastHit.textureCoord;
					worldPoint = raycastHit.point - this.colliderOffset;
					worldNormal = raycastHit.normal;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x170019A9 RID: 6569
		// (get) Token: 0x0600A656 RID: 42582 RVA: 0x004584C4 File Offset: 0x004566C4
		private Material StampMaterial
		{
			get
			{
				if (this._stampMaterial == null)
				{
					Shader shader = Shader.Find("Hidden/ProjectExhibitionist/BodyPaintSphereStamp");
					if (shader == null)
					{
						Debug.LogError("[BodyPaintCanvasService] Shader BodyPaintSphereStamp non trovato!");
						return null;
					}
					this._stampMaterial = new Material(shader);
				}
				return this._stampMaterial;
			}
		}

		// Token: 0x0600A657 RID: 42583 RVA: 0x00458514 File Offset: 0x00456714
		public void StampBrushWorld(Vector3 worldCenter, Vector3 brushDirection, Color color, float radiusMeters, float hardness, float opacity, bool erase)
		{
			Material stampMaterial = this.StampMaterial;
			if (stampMaterial == null)
			{
				return;
			}
			stampMaterial.SetColor("_BrushColor", color);
			stampMaterial.SetVector("_BrushCenter", worldCenter);
			stampMaterial.SetVector("_BrushDir", brushDirection.normalized);
			stampMaterial.SetFloat("_Radius", Mathf.Max(0.001f, radiusMeters));
			stampMaterial.SetFloat("_Hardness", Mathf.Clamp(hardness, 0f, 0.98f));
			stampMaterial.SetFloat("_Opacity", Mathf.Clamp01(opacity));
			if (this._stampCommandBuffer == null)
			{
				this._stampCommandBuffer = new CommandBuffer
				{
					name = "BodyPaintStamp"
				};
			}
			int num = (erase ? 1 : 0);
			bool flag = false;
			foreach (BodyPaintCanvasService.Channel channel in this._channels)
			{
				if (!(channel.paintRT == null) && !(channel.bakedMesh == null) && !(channel.sourceRenderer == null))
				{
					if (channel.collider != null)
					{
						Bounds bounds = channel.collider.bounds;
						bounds.center -= this.colliderOffset;
						bounds.Expand(0.05f);
						if (bounds.SqrDistance(worldCenter) > radiusMeters * radiusMeters)
						{
							continue;
						}
					}
					Transform transform = ((channel.overlayRenderer != null) ? channel.overlayRenderer.transform : channel.sourceRenderer.transform);
					Matrix4x4 matrix4x = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
					this._stampCommandBuffer.Clear();
					this._stampCommandBuffer.SetRenderTarget(channel.paintRT);
					for (int i = 0; i < channel.bakedMesh.subMeshCount; i++)
					{
						if (channel.paintableSubmesh == null || i >= channel.paintableSubmesh.Length || channel.paintableSubmesh[i])
						{
							this._stampCommandBuffer.DrawMesh(channel.bakedMesh, matrix4x, stampMaterial, i, num);
						}
					}
					Graphics.ExecuteCommandBuffer(this._stampCommandBuffer);
					channel.displayDirty = true;
					channel.coverageDirty = true;
					flag = true;
				}
			}
			if (flag && !erase && opacity > 0f)
			{
				this.SetHasAnyPaint(true);
			}
		}

		// Token: 0x170019AA RID: 6570
		// (get) Token: 0x0600A658 RID: 42584 RVA: 0x004587A4 File Offset: 0x004569A4
		private Material DilateMaterial
		{
			get
			{
				if (this._dilateMaterial == null)
				{
					Shader shader = Shader.Find("Hidden/ProjectExhibitionist/BodyPaintDilate");
					if (shader == null)
					{
						Debug.LogError("[BodyPaintCanvasService] Shader BodyPaintDilate non trovato!");
						return null;
					}
					this._dilateMaterial = new Material(shader);
				}
				return this._dilateMaterial;
			}
		}

		// Token: 0x0600A659 RID: 42585 RVA: 0x004587F4 File Offset: 0x004569F4
		public void FlushDilation()
		{
			foreach (BodyPaintCanvasService.Channel channel in this._channels)
			{
				if (channel.displayDirty && !(channel.paintRT == null) && !(channel.displayRT == null))
				{
					Material dilateMaterial = this.DilateMaterial;
					if (dilateMaterial == null)
					{
						break;
					}
					dilateMaterial.SetTexture("_MaskTex", channel.maskRT);
					Graphics.Blit(channel.paintRT, channel.displayRT, dilateMaterial);
					channel.displayDirty = false;
				}
			}
		}

		// Token: 0x0600A65A RID: 42586 RVA: 0x004588A0 File Offset: 0x00456AA0
		public Vector2Int GetCanvasSize(BodyPaintCanvasService.PaintChannel channel)
		{
			this.EnsureOverlays();
			BodyPaintCanvasService.Channel channel2 = this._channels.Find((BodyPaintCanvasService.Channel c) => c.id == channel && c.paintRT != null);
			if (channel2 == null)
			{
				throw new InvalidOperationException("The body paint canvas is not ready.");
			}
			return new Vector2Int(channel2.paintRT.width, channel2.paintRT.height);
		}

		// Token: 0x170019AB RID: 6571
		// (get) Token: 0x0600A65B RID: 42587 RVA: 0x00458904 File Offset: 0x00456B04
		private Material PresetTransferMaterial
		{
			get
			{
				if (this._presetTransferMaterial == null)
				{
					Shader shader = Resources.Load<Shader>("BodyPaintPresetTransfer");
					if (shader == null || !shader.isSupported)
					{
						throw new InvalidOperationException("Body paint preset conversion is unavailable.");
					}
					this._presetTransferMaterial = new Material(shader)
					{
						hideFlags = HideFlags.HideAndDontSave
					};
				}
				return this._presetTransferMaterial;
			}
		}

		// Token: 0x0600A65C RID: 42588 RVA: 0x00458960 File Offset: 0x00456B60
		public Texture2D CapturePaintTexture(BodyPaintCanvasService.PaintChannel channel)
		{
			this.EnsureOverlays();
			BodyPaintCanvasService.Channel channel2 = this._channels.Find((BodyPaintCanvasService.Channel c) => c.id == channel && c.paintRT != null);
			if (channel2 == null)
			{
				throw new InvalidOperationException("The body paint canvas is not ready.");
			}
			Material presetTransferMaterial = this.PresetTransferMaterial;
			RenderTexture active = RenderTexture.active;
			bool sRGBWrite = GL.sRGBWrite;
			RenderTexture temporary = RenderTexture.GetTemporary(channel2.paintRT.width, channel2.paintRT.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			Texture2D texture2D = null;
			Texture2D texture2D2;
			try
			{
				GL.sRGBWrite = temporary.sRGB;
				Graphics.Blit(channel2.paintRT, temporary, presetTransferMaterial, 0);
				RenderTexture.active = temporary;
				texture2D = new Texture2D(temporary.width, temporary.height, TextureFormat.RGBA32, false, false)
				{
					name = "BodyPaintPreset_" + channel.ToString(),
					wrapMode = TextureWrapMode.Clamp,
					filterMode = FilterMode.Bilinear
				};
				texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0, false);
				texture2D.Apply(false, false);
				texture2D2 = texture2D;
			}
			catch
			{
				if (texture2D != null)
				{
					if (Application.isPlaying)
					{
						Object.Destroy(texture2D);
					}
					else
					{
						Object.DestroyImmediate(texture2D);
					}
				}
				throw;
			}
			finally
			{
				RenderTexture.active = active;
				GL.sRGBWrite = sRGBWrite;
				RenderTexture.ReleaseTemporary(temporary);
			}
			return texture2D2;
		}

		// Token: 0x0600A65D RID: 42589 RVA: 0x00458AD8 File Offset: 0x00456CD8
		public void RestorePaintTextures(Texture2D body, Texture2D head)
		{
			if (body == null || head == null)
			{
				throw new ArgumentException("A body paint preset needs both body and head images.");
			}
			this.EnsureOverlays();
			BodyPaintCanvasService.Channel channel = this._channels.Find((BodyPaintCanvasService.Channel c) => c.id == BodyPaintCanvasService.PaintChannel.Body && c.paintRT != null);
			BodyPaintCanvasService.Channel channel2 = this._channels.Find((BodyPaintCanvasService.Channel c) => c.id == BodyPaintCanvasService.PaintChannel.Head && c.paintRT != null);
			if (channel == null || channel2 == null)
			{
				throw new InvalidOperationException("The body paint canvas is not ready.");
			}
			if (body.width != channel.paintRT.width || body.height != channel.paintRT.height || head.width != channel2.paintRT.width || head.height != channel2.paintRT.height)
			{
				throw new ArgumentException("This preset uses a different body paint canvas size.");
			}
			Material presetTransferMaterial = this.PresetTransferMaterial;
			RenderTexture active = RenderTexture.active;
			bool sRGBWrite = GL.sRGBWrite;
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			try
			{
				renderTexture = RenderTexture.GetTemporary(channel.paintRT.descriptor);
				renderTexture2 = RenderTexture.GetTemporary(channel2.paintRT.descriptor);
				GL.sRGBWrite = renderTexture.sRGB;
				Graphics.Blit(body, renderTexture, presetTransferMaterial, 1);
				GL.sRGBWrite = renderTexture2.sRGB;
				Graphics.Blit(head, renderTexture2, presetTransferMaterial, 1);
				GL.sRGBWrite = channel.paintRT.sRGB;
				Graphics.Blit(renderTexture, channel.paintRT);
				GL.sRGBWrite = channel2.paintRT.sRGB;
				Graphics.Blit(renderTexture2, channel2.paintRT);
				channel.displayDirty = (channel2.displayDirty = true);
				channel.coverageDirty = (channel2.coverageDirty = true);
				this.SetRainFade(0f);
				this.SetLifetimeFade(0f);
				this.FlushDilation();
				this.RefreshCoverage();
			}
			finally
			{
				RenderTexture.active = active;
				GL.sRGBWrite = sRGBWrite;
				if (renderTexture != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
				if (renderTexture2 != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture2);
				}
			}
		}

		// Token: 0x0600A65E RID: 42590 RVA: 0x00458CF4 File Offset: 0x00456EF4
		public void DebugVisualizePaintMeshes(Vector3 sideOffset)
		{
			foreach (BodyPaintCanvasService.Channel channel in this._channels)
			{
				if (!(channel.bakedMesh == null))
				{
					GameObject gameObject = new GameObject(string.Format("DEBUG_PaintMesh_{0}", channel.id));
					gameObject.AddComponent<MeshFilter>().sharedMesh = channel.bakedMesh;
					Renderer renderer = gameObject.AddComponent<MeshRenderer>();
					Material material = new Material(Shader.Find("Sprites/Default"))
					{
						color = ((channel.id == BodyPaintCanvasService.PaintChannel.Head) ? Color.magenta : Color.cyan)
					};
					renderer.material = material;
					Transform transform = ((channel.overlayRenderer != null) ? channel.overlayRenderer.transform : channel.sourceRenderer.transform);
					gameObject.transform.SetPositionAndRotation(transform.position + sideOffset, transform.rotation);
					gameObject.transform.localScale = transform.lossyScale;
				}
			}
		}

		// Token: 0x0600A65F RID: 42591 RVA: 0x00458E10 File Offset: 0x00457010
		private void OnDestroy()
		{
			this.DestroyPaintColliders();
			foreach (BodyPaintCanvasService.Channel channel in this._channels)
			{
				if (channel.overlayMaterial != null)
				{
					Object.Destroy(channel.overlayMaterial);
				}
				if (channel.overlayGO != null)
				{
					Object.Destroy(channel.overlayGO);
				}
				if (channel.paintRT != null)
				{
					channel.paintRT.Release();
					Object.Destroy(channel.paintRT);
				}
				if (channel.displayRT != null)
				{
					channel.displayRT.Release();
					Object.Destroy(channel.displayRT);
				}
				if (channel.maskRT != null)
				{
					channel.maskRT.Release();
					Object.Destroy(channel.maskRT);
				}
				if (channel.bakedMesh != null)
				{
					Object.Destroy(channel.bakedMesh);
				}
				if (channel.colliderMesh != null)
				{
					Object.Destroy(channel.colliderMesh);
				}
			}
			this._channels.Clear();
			if (this._stampMaterial != null)
			{
				Object.Destroy(this._stampMaterial);
			}
			if (this._dilateMaterial != null)
			{
				Object.Destroy(this._dilateMaterial);
			}
			if (this._presetTransferMaterial != null)
			{
				Object.Destroy(this._presetTransferMaterial);
			}
			if (this._stampCommandBuffer != null)
			{
				this._stampCommandBuffer.Release();
				this._stampCommandBuffer = null;
			}
		}

		// Token: 0x0600A661 RID: 42593 RVA: 0x00459028 File Offset: 0x00457228
		[CompilerGenerated]
		internal static RenderTexture <TryBuildChannel>g__MakeRT|56_0(string rtName, RenderTextureFormat format, ref BodyPaintCanvasService.<>c__DisplayClass56_0 A_2)
		{
			RenderTexture renderTexture = new RenderTexture(A_2.textureSize, A_2.textureSize, 0, format);
			renderTexture.name = rtName;
			renderTexture.useMipMap = false;
			renderTexture.filterMode = FilterMode.Bilinear;
			renderTexture.wrapMode = TextureWrapMode.Clamp;
			renderTexture.Create();
			BodyPaintCanvasService.ClearRT(renderTexture);
			return renderTexture;
		}

		// Token: 0x0400AB04 RID: 43780
		private const string OverlayShaderName = "ProjectExhibitionist/BodyPaintOverlay";

		// Token: 0x0400AB05 RID: 43781
		public const float MaximumDetectionCoverage = 9f;

		// Token: 0x0400AB06 RID: 43782
		private const int CoverageSampleSize = 128;

		// Token: 0x0400AB07 RID: 43783
		[Header("References")]
		[Tooltip("Se nullo viene cercato sullo stesso GameObject: fornisce bodyRenderer/headRenderer")]
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x0400AB08 RID: 43784
		[SerializeField]
		private SkinnedMeshRenderer bodyRendererOverride;

		// Token: 0x0400AB09 RID: 43785
		[SerializeField]
		private SkinnedMeshRenderer headRendererOverride;

		// Token: 0x0400AB0A RID: 43786
		[Header("Canvas")]
		[SerializeField]
		private int bodyTextureSize = 2048;

		// Token: 0x0400AB0B RID: 43787
		[SerializeField]
		private int headTextureSize = 1024;

		// Token: 0x0400AB0C RID: 43788
		[Header("Paint Meshes (UV de-specchiate)")]
		[Tooltip("Copia della mesh body con le isole UV sovrapposte separate (capezzoli L/R). Generata da Tools > Body Paint > Generate Paint Meshes. Se nulla usa la sharedMesh (specchiata).")]
		[SerializeField]
		private Mesh bodyPaintMesh;

		// Token: 0x0400AB0D RID: 43789
		[SerializeField]
		private Mesh headPaintMesh;

		// Token: 0x0400AB0E RID: 43790
		[Header("Rendering")]
		[Tooltip("Render queue dell'overlay Body: sopra i decal di SkinDecalController (2001+). L'overlay Head usa HeadOverlayRenderQueue.")]
		[SerializeField]
		private int overlayRenderQueue = 2300;

		// Token: 0x0400AB0F RID: 43791
		private const int HeadOverlayRenderQueue = 2008;

		// Token: 0x0400AB10 RID: 43792
		[Header("Submesh shell (NON dipingibili)")]
		[Tooltip("Le submesh il cui materiale SORGENTE contiene una di queste keyword sono GUSCI che fluttuano sopra la pelle (faccia: [1] blush, [2] occhi). Vanno escluse da overlay/stamp/collider: altrimenti il pennello colpisce il guscio invisibile e il tratto resta appeso a mezz'aria sopra la faccia invece che sulla pelle.")]
		[SerializeField]
		private string[] shellMaterialKeywords = new string[] { "blush", "eye" };

		// Token: 0x0400AB11 RID: 43793
		[Tooltip("Materiale che non renderizza nulla (BodyPaint/Invisible): assegnato alle submesh shell dell'overlay. Assegnato da Tools > Body Paint > Setup Player.")]
		[SerializeField]
		private Material shellInvisibleMaterial;

		// Token: 0x0400AB12 RID: 43794
		[Header("Paint Colliders")]
		[Tooltip("Offset dei collider di painting per non interferire con la fisica di gioco")]
		[SerializeField]
		private Vector3 colliderOffset = new Vector3(0f, 5000f, 0f);

		// Token: 0x0400AB13 RID: 43795
		private readonly List<BodyPaintCanvasService.Channel> _channels = new List<BodyPaintCanvasService.Channel>();

		// Token: 0x0400AB14 RID: 43796
		private readonly List<Vector3> _vertexScratch = new List<Vector3>();

		// Token: 0x0400AB15 RID: 43797
		private Material _stampMaterial;

		// Token: 0x0400AB16 RID: 43798
		private Material _dilateMaterial;

		// Token: 0x0400AB17 RID: 43799
		private Material _presetTransferMaterial;

		// Token: 0x0400AB18 RID: 43800
		private CommandBuffer _stampCommandBuffer;

		// Token: 0x0400AB19 RID: 43801
		private Transform _overlayContainer;

		// Token: 0x0400AB1A RID: 43802
		private bool _overlaysBuilt;

		// Token: 0x0400AB1B RID: 43803
		private float _rainFadeAmount;

		// Token: 0x0400AB1C RID: 43804
		private float _lifetimeFadeAmount;

		// Token: 0x02001508 RID: 5384
		public enum PaintChannel
		{
			// Token: 0x0400AB22 RID: 43810
			Body,
			// Token: 0x0400AB23 RID: 43811
			Head
		}

		// Token: 0x02001509 RID: 5385
		private class Channel
		{
			// Token: 0x0400AB24 RID: 43812
			public BodyPaintCanvasService.PaintChannel id;

			// Token: 0x0400AB25 RID: 43813
			public SkinnedMeshRenderer sourceRenderer;

			// Token: 0x0400AB26 RID: 43814
			public SkinnedMeshRenderer overlayRenderer;

			// Token: 0x0400AB27 RID: 43815
			public RenderTexture paintRT;

			// Token: 0x0400AB28 RID: 43816
			public RenderTexture displayRT;

			// Token: 0x0400AB29 RID: 43817
			public RenderTexture maskRT;

			// Token: 0x0400AB2A RID: 43818
			public bool displayDirty;

			// Token: 0x0400AB2B RID: 43819
			public bool coverageDirty;

			// Token: 0x0400AB2C RID: 43820
			public float paintedFraction;

			// Token: 0x0400AB2D RID: 43821
			public GameObject overlayGO;

			// Token: 0x0400AB2E RID: 43822
			public Material overlayMaterial;

			// Token: 0x0400AB2F RID: 43823
			public GameObject colliderGO;

			// Token: 0x0400AB30 RID: 43824
			public MeshCollider collider;

			// Token: 0x0400AB31 RID: 43825
			public Mesh bakedMesh;

			// Token: 0x0400AB32 RID: 43826
			public bool[] paintableSubmesh;

			// Token: 0x0400AB33 RID: 43827
			public bool hasShellSubmesh;

			// Token: 0x0400AB34 RID: 43828
			public Mesh colliderMesh;
		}
	}
}
