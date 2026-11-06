using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NeckSigns
{
	// Token: 0x02000F7B RID: 3963
	[DisallowMultipleComponent]
	public sealed class NeckSignSurface : MonoBehaviour
	{
		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x06007748 RID: 30536 RVA: 0x00308C08 File Offset: 0x00306E08
		public string PartId
		{
			get
			{
				return this.partId;
			}
		}

		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x06007749 RID: 30537 RVA: 0x00308C10 File Offset: 0x00306E10
		public Renderer TargetRenderer
		{
			get
			{
				return this.targetRenderer;
			}
		}

		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x0600774A RID: 30538 RVA: 0x00308C18 File Offset: 0x00306E18
		public Texture2D Texture
		{
			get
			{
				this.EnsureTexture();
				return this._texture;
			}
		}

		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x0600774B RID: 30539 RVA: 0x00308C26 File Offset: 0x00306E26
		public bool CanUndo
		{
			get
			{
				return this._undoPixels != null;
			}
		}

		// Token: 0x1700138E RID: 5006
		// (get) Token: 0x0600774C RID: 30540 RVA: 0x00308C31 File Offset: 0x00306E31
		public bool HasPaint
		{
			get
			{
				return this._hasPaint;
			}
		}

		// Token: 0x1700138F RID: 5007
		// (get) Token: 0x0600774D RID: 30541 RVA: 0x00308C39 File Offset: 0x00306E39
		public Color CanvasBackground
		{
			get
			{
				return this.canvasBackground;
			}
		}

		// Token: 0x17001390 RID: 5008
		// (get) Token: 0x0600774E RID: 30542 RVA: 0x00308C41 File Offset: 0x00306E41
		public Bounds PaintBounds
		{
			get
			{
				if (!(this.targetRenderer != null))
				{
					return new Bounds(base.transform.position, Vector3.one * 0.1f);
				}
				return this.targetRenderer.bounds;
			}
		}

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x0600774F RID: 30543 RVA: 0x00308C7C File Offset: 0x00306E7C
		// (remove) Token: 0x06007750 RID: 30544 RVA: 0x00308CB4 File Offset: 0x00306EB4
		public event Action DesignChanged;

		// Token: 0x06007751 RID: 30545 RVA: 0x00308CEC File Offset: 0x00306EEC
		public void Configure(string id, Renderer renderer, int index, float surfaceAspectRatio = 2f, Color? background = null)
		{
			Color color = this.canvasBackground;
			this.partId = id;
			this.targetRenderer = renderer;
			this.materialIndex = Mathf.Max(0, index);
			this.aspectRatio = Mathf.Clamp(surfaceAspectRatio, 1f, 4f);
			this.canvasBackground = background ?? Color.white;
			this.canvasBackground.a = 1f;
			if (color != this.canvasBackground)
			{
				NeckSignSurface.Dispose(this._texture);
				this._texture = null;
			}
			this.ReleasePaintSurface();
			this.EnsureTexture();
			this.ApplyTexture();
		}

		// Token: 0x06007752 RID: 30546 RVA: 0x00308D92 File Offset: 0x00306F92
		private void OnEnable()
		{
			this.EnsureTexture();
			this.ApplyTexture();
		}

		// Token: 0x06007753 RID: 30547 RVA: 0x00308DA0 File Offset: 0x00306FA0
		private void OnDisable()
		{
			this.ReleasePaintSurface();
		}

		// Token: 0x06007754 RID: 30548 RVA: 0x00308DA8 File Offset: 0x00306FA8
		private void EnsureTexture()
		{
			int num = Mathf.Clamp(Mathf.RoundToInt(512f / this.aspectRatio), 128, 512);
			if (this._texture != null && this._texture.height == num)
			{
				return;
			}
			NeckSignSurface.Dispose(this._texture);
			this._texture = new Texture2D(512, num, TextureFormat.RGBA32, false)
			{
				name = "Neck sign drawing (instance)",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear,
				hideFlags = HideFlags.DontSave
			};
			this._pixels = new Color32[512 * num];
			this._undoPixels = null;
			this._strokeBefore = null;
			this._strokeChanged = false;
			this.FillBackground(this._pixels);
			this._hasPaint = false;
			this._encodedPng = string.Empty;
			this.Upload();
		}

		// Token: 0x06007755 RID: 30549 RVA: 0x00308E80 File Offset: 0x00307080
		public void ApplyTexture()
		{
			if (this.targetRenderer == null || this._texture == null || this.materialIndex >= this.targetRenderer.sharedMaterials.Length)
			{
				return;
			}
			if (this._properties == null)
			{
				this._properties = new MaterialPropertyBlock();
			}
			this.targetRenderer.GetPropertyBlock(this._properties, this.materialIndex);
			this._properties.SetTexture("_MainTex", this._texture);
			this._properties.SetTexture("_BaseMap", this._texture);
			this.targetRenderer.SetPropertyBlock(this._properties, this.materialIndex);
		}

		// Token: 0x06007756 RID: 30550 RVA: 0x00308F2C File Offset: 0x0030712C
		public NeckSignDesignData CaptureDesign()
		{
			this.EnsureTexture();
			this.FlushPaint();
			if (this._encodedPng == null)
			{
				this._encodedPng = (this._hasPaint ? Convert.ToBase64String(this._texture.EncodeToPNG()) : string.Empty);
			}
			return new NeckSignDesignData
			{
				partId = this.partId,
				pngBase64 = this._encodedPng
			};
		}

		// Token: 0x06007757 RID: 30551 RVA: 0x00308F90 File Offset: 0x00307190
		public void ApplyDesign(NeckSignDesignData design)
		{
			this.EnsureTexture();
			this.FillBackground(this._pixels);
			this._hasPaint = false;
			this._encodedPng = string.Empty;
			this._undoPixels = null;
			this._strokeBefore = null;
			this._strokeChanged = false;
			if (design != null && (string.IsNullOrEmpty(design.partId) || string.Equals(this.partId, design.partId, StringComparison.OrdinalIgnoreCase)) && !string.IsNullOrEmpty(design.pngBase64) && design.pngBase64.Length < 4194304)
			{
				Texture2D texture2D = null;
				try
				{
					byte[] array = Convert.FromBase64String(design.pngBase64);
					if (NeckSignSurface.IsExpectedPng(array, this._texture.width, this._texture.height))
					{
						texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
						if (texture2D.LoadImage(array))
						{
							this._pixels = texture2D.GetPixels32();
							this._hasPaint = this.HasPaintPixel(this._pixels);
							this._encodedPng = (this._hasPaint ? design.pngBase64 : string.Empty);
						}
					}
				}
				catch (FormatException)
				{
				}
				finally
				{
					NeckSignSurface.Dispose(texture2D);
				}
			}
			this.Upload();
		}

		// Token: 0x06007758 RID: 30552 RVA: 0x003090CC File Offset: 0x003072CC
		private static bool IsExpectedPng(byte[] bytes, int width, int height)
		{
			if (bytes == null || bytes.Length < 24 || bytes[0] != 137 || bytes[1] != 80 || bytes[2] != 78 || bytes[3] != 71)
			{
				return false;
			}
			int num = ((int)bytes[16] << 24) | ((int)bytes[17] << 16) | ((int)bytes[18] << 8) | (int)bytes[19];
			int num2 = ((int)bytes[20] << 24) | ((int)bytes[21] << 16) | ((int)bytes[22] << 8) | (int)bytes[23];
			return num == width && num2 == height;
		}

		// Token: 0x06007759 RID: 30553 RVA: 0x00309144 File Offset: 0x00307344
		public void PreparePaintSurface()
		{
			this._vertices.Clear();
			this._uvs.Clear();
			this._triangles.Clear();
			this._hasHit = false;
			Mesh mesh = null;
			SkinnedMeshRenderer skinnedMeshRenderer = this.targetRenderer as SkinnedMeshRenderer;
			if (skinnedMeshRenderer != null)
			{
				if (this._bakedMesh == null)
				{
					this._bakedMesh = new Mesh
					{
						name = "Neck sign raycast mesh",
						hideFlags = HideFlags.DontSave
					};
				}
				skinnedMeshRenderer.BakeMesh(this._bakedMesh);
				mesh = this._bakedMesh;
			}
			else if (this.targetRenderer != null)
			{
				MeshFilter component = this.targetRenderer.GetComponent<MeshFilter>();
				if (component != null)
				{
					mesh = component.sharedMesh;
				}
			}
			if (mesh != null && this.materialIndex < mesh.subMeshCount)
			{
				mesh.GetVertices(this._vertices);
				mesh.GetUVs(0, this._uvs);
				mesh.GetTriangles(this._triangles, this.materialIndex);
			}
			this._prepared = this._vertices.Count > 0 && this._uvs.Count == this._vertices.Count;
		}

		// Token: 0x0600775A RID: 30554 RVA: 0x00309266 File Offset: 0x00307466
		public void ReleasePaintSurface()
		{
			this._prepared = false;
			this._hasHit = false;
		}

		// Token: 0x0600775B RID: 30555 RVA: 0x00309276 File Offset: 0x00307476
		public void BuildPaintCollider()
		{
			this.PreparePaintSurface();
		}

		// Token: 0x0600775C RID: 30556 RVA: 0x00308DA0 File Offset: 0x00306FA0
		public void DestroyPaintCollider()
		{
			this.ReleasePaintSurface();
		}

		// Token: 0x0600775D RID: 30557 RVA: 0x00309280 File Offset: 0x00307480
		public bool TryGetPaintFrame(out Vector3 center, out Vector3 normal, out Vector3 up)
		{
			center = this.PaintBounds.center;
			normal = base.transform.forward;
			up = base.transform.up;
			if (!this._prepared)
			{
				this.PreparePaintSurface();
			}
			if (!this._prepared || this.targetRenderer == null)
			{
				return false;
			}
			Transform transform = this.targetRenderer.transform;
			int num = 0;
			while (num + 2 < this._triangles.Count)
			{
				int num2 = this._triangles[num];
				int num3 = this._triangles[num + 1];
				int num4 = this._triangles[num + 2];
				Vector3 vector = transform.TransformVector(this._vertices[num3] - this._vertices[num2]);
				Vector3 vector2 = transform.TransformVector(this._vertices[num4] - this._vertices[num2]);
				Vector2 vector3 = this._uvs[num3] - this._uvs[num2];
				Vector2 vector4 = this._uvs[num4] - this._uvs[num2];
				float num5 = vector3.x * vector4.y - vector3.y * vector4.x;
				if (Mathf.Abs(num5) >= 1E-07f)
				{
					normal = Vector3.Cross(vector, vector2).normalized;
					up = ((vector2 * vector3.x - vector * vector4.x) / num5).normalized;
					return normal.sqrMagnitude > 0.9f && up.sqrMagnitude > 0.9f;
				}
				num += 3;
			}
			return false;
		}

		// Token: 0x0600775E RID: 30558 RVA: 0x00309468 File Offset: 0x00307668
		public bool RaycastPaintSurface(Ray ray, out Vector3 worldPoint)
		{
			Vector2 vector;
			return this.RaycastPaintSurface(ray, out vector, out worldPoint);
		}

		// Token: 0x0600775F RID: 30559 RVA: 0x00309480 File Offset: 0x00307680
		public bool RaycastPaintSurface(Ray worldRay, out Vector2 uv, out Vector3 worldPoint)
		{
			uv = default(Vector2);
			worldPoint = default(Vector3);
			this._hasHit = false;
			if (this.targetRenderer == null || !this.targetRenderer.enabled || !this.targetRenderer.gameObject.activeInHierarchy)
			{
				return false;
			}
			if (!this._prepared)
			{
				this.PreparePaintSurface();
			}
			if (!this._prepared)
			{
				return false;
			}
			Transform transform = this.targetRenderer.transform;
			Vector3 vector = transform.InverseTransformPoint(worldRay.origin);
			Vector3 vector2 = transform.InverseTransformVector(worldRay.direction);
			float num = float.PositiveInfinity;
			int num2 = 0;
			while (num2 + 2 < this._triangles.Count)
			{
				int num3 = this._triangles[num2];
				int num4 = this._triangles[num2 + 1];
				int num5 = this._triangles[num2 + 2];
				Vector3 vector3 = this._vertices[num3];
				Vector3 vector4 = this._vertices[num4] - vector3;
				Vector3 vector5 = this._vertices[num5] - vector3;
				Vector3 vector6 = Vector3.Cross(vector2, vector5);
				float num6 = Vector3.Dot(vector4, vector6);
				if (num6 > 1E-08f)
				{
					float num7 = 1f / num6;
					Vector3 vector7 = vector - vector3;
					float num8 = Vector3.Dot(vector7, vector6) * num7;
					if (num8 >= 0f && num8 <= 1f)
					{
						Vector3 vector8 = Vector3.Cross(vector7, vector4);
						float num9 = Vector3.Dot(vector2, vector8) * num7;
						if (num9 >= 0f && num8 + num9 <= 1f)
						{
							float num10 = Vector3.Dot(vector5, vector8) * num7;
							if (num10 > 0f && num10 < num)
							{
								num = num10;
								uv = this._uvs[num3] + num8 * (this._uvs[num4] - this._uvs[num3]) + num9 * (this._uvs[num5] - this._uvs[num3]);
								worldPoint = worldRay.GetPoint(num10);
								Vector2 vector9 = this._uvs[num4] - this._uvs[num3];
								Vector2 vector10 = this._uvs[num5] - this._uvs[num3];
								float num11 = vector9.x * vector10.y - vector9.y * vector10.x;
								this._worldU = Vector3.zero;
								this._worldV = Vector3.zero;
								if (Mathf.Abs(num11) > 1E-07f)
								{
									this._worldU = transform.TransformVector((vector4 * vector10.y - vector5 * vector9.y) / num11);
									this._worldV = transform.TransformVector((vector5 * vector9.x - vector4 * vector10.x) / num11);
								}
								this._hasHit = true;
							}
						}
					}
				}
				num2 += 3;
			}
			if (this._hasHit)
			{
				this._lastUv = uv;
				this._lastWorldPoint = worldPoint;
			}
			return this._hasHit;
		}

		// Token: 0x06007760 RID: 30560 RVA: 0x003097EC File Offset: 0x003079EC
		public void StampBrushWorld(Vector3 center, Vector3 direction, Color color, float radiusMeters, float hardness, float opacity, bool erase)
		{
			if (!this._hasHit || (center - this._lastWorldPoint).sqrMagnitude > 0.001f)
			{
				return;
			}
			this.EnsureTexture();
			float num = Vector3.Dot(this._worldU, this._worldU);
			float num2 = Vector3.Dot(this._worldV, this._worldV);
			float num3 = Vector3.Dot(this._worldU, this._worldV);
			float num4 = num * num2 - num3 * num3;
			if (num4 <= 1E-12f || radiusMeters <= 0f)
			{
				return;
			}
			float num5 = radiusMeters * Mathf.Sqrt(num2 / num4);
			float num6 = radiusMeters * Mathf.Sqrt(num / num4);
			this.BeginMutation();
			int num7 = Mathf.Max(0, Mathf.FloorToInt((this._lastUv.x - num5) * (float)this._texture.width));
			int num8 = Mathf.Min(this._texture.width - 1, Mathf.CeilToInt((this._lastUv.x + num5) * (float)this._texture.width));
			int num9 = Mathf.Max(0, Mathf.FloorToInt((this._lastUv.y - num6) * (float)this._texture.height));
			int num10 = Mathf.Min(this._texture.height - 1, Mathf.CeilToInt((this._lastUv.y + num6) * (float)this._texture.height));
			for (int i = num9; i <= num10; i++)
			{
				for (int j = num7; j <= num8; j++)
				{
					float num11 = ((float)j + 0.5f) / (float)this._texture.width - this._lastUv.x;
					float num12 = ((float)i + 0.5f) / (float)this._texture.height - this._lastUv.y;
					float num13 = Mathf.Sqrt(Mathf.Max(0f, num11 * num11 * num + 2f * num11 * num12 * num3 + num12 * num12 * num2)) / radiusMeters;
					this.BlendPixel(j, i, num13, color, hardness, opacity, erase);
				}
			}
			this._uploadDirty = true;
		}

		// Token: 0x06007761 RID: 30561 RVA: 0x00309A0C File Offset: 0x00307C0C
		public void DrawStroke(Vector2 fromUv, Vector2 toUv, Color color, float radiusPixels, bool erase)
		{
			this.EnsureTexture();
			this.BeginMutation();
			Vector2 vector = new Vector2(fromUv.x * (float)this._texture.width, fromUv.y * (float)this._texture.height);
			Vector2 vector2 = new Vector2(toUv.x * (float)this._texture.width, toUv.y * (float)this._texture.height);
			float num = Mathf.Clamp(radiusPixels, 0.5f, 128f);
			int num2 = Mathf.Max(1, Mathf.CeilToInt(Vector2.Distance(vector, vector2) / Mathf.Max(1f, num * 0.4f)));
			for (int i = 0; i <= num2; i++)
			{
				Vector2 vector3 = Vector2.Lerp(vector, vector2, (float)i / (float)num2);
				int num3 = Mathf.Max(0, Mathf.FloorToInt(vector3.x - num));
				int num4 = Mathf.Min(this._texture.width - 1, Mathf.CeilToInt(vector3.x + num));
				int num5 = Mathf.Max(0, Mathf.FloorToInt(vector3.y - num));
				int num6 = Mathf.Min(this._texture.height - 1, Mathf.CeilToInt(vector3.y + num));
				for (int j = num5; j <= num6; j++)
				{
					for (int k = num3; k <= num4; k++)
					{
						this.BlendPixel(k, j, Vector2.Distance(new Vector2((float)k + 0.5f, (float)j + 0.5f), vector3) / num, color, 0.85f, color.a, erase);
					}
				}
			}
			this.Upload();
		}

		// Token: 0x06007762 RID: 30562 RVA: 0x00309BAC File Offset: 0x00307DAC
		private void BlendPixel(int x, int y, float distance, Color color, float hardness, float opacity, bool erase)
		{
			if (distance >= 1f || opacity <= 0f)
			{
				return;
			}
			float num = 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(Mathf.Clamp(hardness, 0f, 0.999f), 1f, distance));
			int num2 = y * this._texture.width + x;
			Color color2 = Color.Lerp(this._pixels[num2], erase ? this.canvasBackground : color, Mathf.Clamp01(opacity) * num);
			color2.a = 1f;
			this._pixels[num2] = color2;
			this._strokeChanged = true;
			this._hasPaint = true;
			this._encodedPng = null;
		}

		// Token: 0x06007763 RID: 30563 RVA: 0x00309C6E File Offset: 0x00307E6E
		private void BeginMutation()
		{
			if (this._strokeBefore == null)
			{
				this._strokeBefore = (Color32[])this._pixels.Clone();
			}
		}

		// Token: 0x06007764 RID: 30564 RVA: 0x00309C8E File Offset: 0x00307E8E
		public void CommitStroke()
		{
			this.CommitDesign();
		}

		// Token: 0x06007765 RID: 30565 RVA: 0x00309C98 File Offset: 0x00307E98
		public void CommitDesign()
		{
			this.FlushPaint();
			if (!this._strokeChanged)
			{
				this._strokeBefore = null;
				return;
			}
			this._undoPixels = this._strokeBefore;
			this._strokeBefore = null;
			this._strokeChanged = false;
			this._hasPaint = this.HasPaintPixel(this._pixels);
			this._encodedPng = null;
			Action designChanged = this.DesignChanged;
			if (designChanged == null)
			{
				return;
			}
			designChanged();
		}

		// Token: 0x06007766 RID: 30566 RVA: 0x00309CFE File Offset: 0x00307EFE
		public void ClearCanvas()
		{
			this.ClearPaint();
		}

		// Token: 0x06007767 RID: 30567 RVA: 0x00309D08 File Offset: 0x00307F08
		public void ClearPaint()
		{
			this.EnsureTexture();
			this.CommitDesign();
			this.BeginMutation();
			this.FillBackground(this._pixels);
			this._strokeChanged = true;
			this._hasPaint = false;
			this._encodedPng = string.Empty;
			this.Upload();
			this.CommitDesign();
		}

		// Token: 0x06007768 RID: 30568 RVA: 0x00309D58 File Offset: 0x00307F58
		public void Undo()
		{
			this.CommitDesign();
			if (this._undoPixels == null)
			{
				return;
			}
			Color32[] pixels = this._pixels;
			this._pixels = this._undoPixels;
			this._undoPixels = pixels;
			this._hasPaint = this.HasPaintPixel(this._pixels);
			this._encodedPng = null;
			this.Upload();
			Action designChanged = this.DesignChanged;
			if (designChanged == null)
			{
				return;
			}
			designChanged();
		}

		// Token: 0x06007769 RID: 30569 RVA: 0x00309DBD File Offset: 0x00307FBD
		public void FlushPaint()
		{
			if (this._uploadDirty)
			{
				this.Upload();
			}
		}

		// Token: 0x0600776A RID: 30570 RVA: 0x00309DCD File Offset: 0x00307FCD
		private void Upload()
		{
			this._texture.SetPixels32(this._pixels);
			this._texture.Apply(false, false);
			this._uploadDirty = false;
			this.ApplyTexture();
		}

		// Token: 0x0600776B RID: 30571 RVA: 0x00309DFC File Offset: 0x00307FFC
		private void FillBackground(Color32[] pixels)
		{
			Color32 color = this.canvasBackground;
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = color;
			}
		}

		// Token: 0x0600776C RID: 30572 RVA: 0x00309E2C File Offset: 0x0030802C
		private bool HasPaintPixel(Color32[] pixels)
		{
			Color32 color = this.canvasBackground;
			foreach (Color32 color2 in pixels)
			{
				if (color2.r != color.r || color2.g != color.g || color2.b != color.b)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600776D RID: 30573 RVA: 0x00309E8A File Offset: 0x0030808A
		private void OnDestroy()
		{
			NeckSignSurface.Dispose(this._texture);
			NeckSignSurface.Dispose(this._bakedMesh);
		}

		// Token: 0x0600776E RID: 30574 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void Dispose(Object item)
		{
			if (item == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(item);
				return;
			}
			Object.DestroyImmediate(item);
		}

		// Token: 0x04007A9B RID: 31387
		public const int CanvasWidth = 512;

		// Token: 0x04007A9C RID: 31388
		[SerializeField]
		private string partId;

		// Token: 0x04007A9D RID: 31389
		[SerializeField]
		private Renderer targetRenderer;

		// Token: 0x04007A9E RID: 31390
		[SerializeField]
		private int materialIndex;

		// Token: 0x04007A9F RID: 31391
		[SerializeField]
		[Range(1f, 4f)]
		private float aspectRatio = 2f;

		// Token: 0x04007AA0 RID: 31392
		[SerializeField]
		private Color canvasBackground = Color.white;

		// Token: 0x04007AA1 RID: 31393
		private Texture2D _texture;

		// Token: 0x04007AA2 RID: 31394
		private Color32[] _pixels;

		// Token: 0x04007AA3 RID: 31395
		private Color32[] _undoPixels;

		// Token: 0x04007AA4 RID: 31396
		private Color32[] _strokeBefore;

		// Token: 0x04007AA5 RID: 31397
		private MaterialPropertyBlock _properties;

		// Token: 0x04007AA6 RID: 31398
		private Mesh _bakedMesh;

		// Token: 0x04007AA7 RID: 31399
		private readonly List<Vector3> _vertices = new List<Vector3>();

		// Token: 0x04007AA8 RID: 31400
		private readonly List<Vector2> _uvs = new List<Vector2>();

		// Token: 0x04007AA9 RID: 31401
		private readonly List<int> _triangles = new List<int>();

		// Token: 0x04007AAA RID: 31402
		private bool _prepared;

		// Token: 0x04007AAB RID: 31403
		private bool _strokeChanged;

		// Token: 0x04007AAC RID: 31404
		private bool _hasPaint;

		// Token: 0x04007AAD RID: 31405
		private bool _uploadDirty;

		// Token: 0x04007AAE RID: 31406
		private string _encodedPng;

		// Token: 0x04007AAF RID: 31407
		private Vector2 _lastUv;

		// Token: 0x04007AB0 RID: 31408
		private Vector3 _lastWorldPoint;

		// Token: 0x04007AB1 RID: 31409
		private Vector3 _worldU;

		// Token: 0x04007AB2 RID: 31410
		private Vector3 _worldV;

		// Token: 0x04007AB3 RID: 31411
		private bool _hasHit;
	}
}
