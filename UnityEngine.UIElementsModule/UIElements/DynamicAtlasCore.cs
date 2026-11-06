using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x020002A7 RID: 679
	internal class DynamicAtlasCore : IDisposable
	{
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001378 RID: 4984 RVA: 0x00043E6C File Offset: 0x0004206C
		public int maxImageSize { get; }

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x00043E74 File Offset: 0x00042074
		public RenderTextureFormat format { get; }

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600137A RID: 4986 RVA: 0x00043E7C File Offset: 0x0004207C
		// (set) Token: 0x0600137B RID: 4987 RVA: 0x00043E84 File Offset: 0x00042084
		public RenderTexture atlas { get; private set; }

		// Token: 0x0600137C RID: 4988 RVA: 0x00043E90 File Offset: 0x00042090
		public DynamicAtlasCore(RenderTextureFormat format = RenderTextureFormat.ARGB32, FilterMode filterMode = FilterMode.Bilinear, int maxImageSize = 64, int initialSize = 64, int maxAtlasSize = 4096)
		{
			Debug.Assert(filterMode == FilterMode.Bilinear || filterMode == FilterMode.Point);
			Debug.Assert(maxAtlasSize <= SystemInfo.maxRenderTextureSize);
			Debug.Assert(initialSize <= maxAtlasSize);
			Debug.Assert(Mathf.IsPowerOfTwo(maxImageSize));
			Debug.Assert(Mathf.IsPowerOfTwo(initialSize));
			Debug.Assert(Mathf.IsPowerOfTwo(maxAtlasSize));
			this.m_MaxAtlasSize = maxAtlasSize;
			this.format = format;
			this.maxImageSize = maxImageSize;
			this.m_FilterMode = filterMode;
			this.m_UVs = new Dictionary<Texture2D, RectInt>(64);
			this.m_Blitter = new TextureBlitter(64);
			this.m_InitialSize = initialSize;
			this.m_2SidePadding = ((filterMode == FilterMode.Point) ? 0 : 2);
			this.m_1SidePadding = ((filterMode == FilterMode.Point) ? 0 : 1);
			this.m_Allocator = new UIRAtlasAllocator(this.m_InitialSize, this.m_MaxAtlasSize, this.m_1SidePadding);
			this.m_ColorSpace = QualitySettings.activeColorSpace;
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600137D RID: 4989 RVA: 0x00043F7E File Offset: 0x0004217E
		// (set) Token: 0x0600137E RID: 4990 RVA: 0x00043F86 File Offset: 0x00042186
		private protected bool disposed { protected get; private set; }

		// Token: 0x0600137F RID: 4991 RVA: 0x00043F8F File Offset: 0x0004218F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00043FA4 File Offset: 0x000421A4
		protected virtual void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					UIRUtility.Destroy(this.atlas);
					this.atlas = null;
					bool flag = this.m_Allocator != null;
					if (flag)
					{
						this.m_Allocator.Dispose();
						this.m_Allocator = null;
					}
					bool flag2 = this.m_Blitter != null;
					if (flag2)
					{
						this.m_Blitter.Dispose();
						this.m_Blitter = null;
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00044027 File Offset: 0x00042227
		private static void LogDisposeError()
		{
			Debug.LogError("An attempt to use a disposed atlas manager has been detected.");
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00044038 File Offset: 0x00042238
		public bool IsReleased()
		{
			return this.atlas != null && !this.atlas.IsCreated();
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0004406C File Offset: 0x0004226C
		public bool TryGetRect(Texture2D image, out RectInt uvs, Func<Texture2D, bool> filter = null)
		{
			uvs = default(RectInt);
			bool disposed = this.disposed;
			bool flag;
			if (disposed)
			{
				DynamicAtlasCore.LogDisposeError();
				flag = false;
			}
			else
			{
				bool flag2 = image == null;
				if (flag2)
				{
					flag = false;
				}
				else
				{
					bool flag3 = this.m_UVs.TryGetValue(image, out uvs);
					if (flag3)
					{
						flag = true;
					}
					else
					{
						bool flag4 = filter != null && !filter(image);
						if (flag4)
						{
							flag = false;
						}
						else
						{
							bool flag5 = !this.AllocateRect(image.width, image.height, out uvs);
							if (flag5)
							{
								flag = false;
							}
							else
							{
								this.m_UVs[image] = uvs;
								this.m_Blitter.QueueBlit(image, new RectInt(0, 0, image.width, image.height), new Vector2Int(uvs.x, uvs.y), true, Color.white);
								flag = true;
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0004414C File Offset: 0x0004234C
		public void UpdateTexture(Texture2D image)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DynamicAtlasCore.LogDisposeError();
			}
			else
			{
				RectInt rectInt;
				bool flag = !this.m_UVs.TryGetValue(image, out rectInt);
				if (!flag)
				{
					this.m_Blitter.QueueBlit(image, new RectInt(0, 0, image.width, image.height), new Vector2Int(rectInt.x, rectInt.y), true, Color.white);
				}
			}
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x000441C0 File Offset: 0x000423C0
		public bool AllocateRect(int width, int height, out RectInt uvs)
		{
			bool flag = !this.m_Allocator.TryAllocate(width + this.m_2SidePadding, height + this.m_2SidePadding, out uvs);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				uvs = new RectInt(uvs.x + this.m_1SidePadding, uvs.y + this.m_1SidePadding, width, height);
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00044221 File Offset: 0x00042421
		public void EnqueueBlit(Texture image, RectInt srcRect, int x, int y, bool addBorder, Color tint)
		{
			this.m_Blitter.QueueBlit(image, srcRect, new Vector2Int(x, y), addBorder, tint);
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00044240 File Offset: 0x00042440
		public void Commit()
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DynamicAtlasCore.LogDisposeError();
			}
			else
			{
				this.UpdateAtlasTexture();
				bool forceReblitAll = this.m_ForceReblitAll;
				if (forceReblitAll)
				{
					this.m_ForceReblitAll = false;
					this.m_Blitter.Reset();
					foreach (KeyValuePair<Texture2D, RectInt> keyValuePair in this.m_UVs)
					{
						this.m_Blitter.QueueBlit(keyValuePair.Key, new RectInt(0, 0, keyValuePair.Key.width, keyValuePair.Key.height), new Vector2Int(keyValuePair.Value.x, keyValuePair.Value.y), true, Color.white);
					}
				}
				this.m_Blitter.Commit(this.atlas);
			}
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00044340 File Offset: 0x00042540
		private void UpdateAtlasTexture()
		{
			bool flag = this.atlas == null;
			if (flag)
			{
				bool flag2 = this.m_UVs.Count > this.m_Blitter.queueLength;
				if (flag2)
				{
					this.m_ForceReblitAll = true;
				}
				this.atlas = this.CreateAtlasTexture();
			}
			else
			{
				bool flag3 = this.atlas.width != this.m_Allocator.physicalWidth || this.atlas.height != this.m_Allocator.physicalHeight;
				if (flag3)
				{
					RenderTexture renderTexture = this.CreateAtlasTexture();
					bool flag4 = renderTexture == null;
					if (flag4)
					{
						Debug.LogErrorFormat("Failed to allocate a render texture for the dynamic atlas. Current Size = {0}x{1}. Requested Size = {2}x{3}.", new object[]
						{
							this.atlas.width,
							this.atlas.height,
							this.m_Allocator.physicalWidth,
							this.m_Allocator.physicalHeight
						});
					}
					else
					{
						this.m_Blitter.BlitOneNow(renderTexture, this.atlas, new RectInt(0, 0, this.atlas.width, this.atlas.height), new Vector2Int(0, 0), false, Color.white);
					}
					UIRUtility.Destroy(this.atlas);
					this.atlas = renderTexture;
				}
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0004449C File Offset: 0x0004269C
		private RenderTexture CreateAtlasTexture()
		{
			bool flag = this.m_Allocator.physicalWidth == 0 || this.m_Allocator.physicalHeight == 0;
			RenderTexture renderTexture;
			if (flag)
			{
				renderTexture = null;
			}
			else
			{
				renderTexture = new RenderTexture(this.m_Allocator.physicalWidth, this.m_Allocator.physicalHeight, 0, this.format)
				{
					hideFlags = HideFlags.HideAndDontSave,
					name = "UIR Dynamic Atlas " + DynamicAtlasCore.s_TextureCounter++.ToString(),
					filterMode = this.m_FilterMode
				};
			}
			return renderTexture;
		}

		// Token: 0x040008D7 RID: 2263
		private int m_InitialSize;

		// Token: 0x040008D8 RID: 2264
		private UIRAtlasAllocator m_Allocator;

		// Token: 0x040008D9 RID: 2265
		private Dictionary<Texture2D, RectInt> m_UVs;

		// Token: 0x040008DA RID: 2266
		private bool m_ForceReblitAll;

		// Token: 0x040008DB RID: 2267
		private FilterMode m_FilterMode;

		// Token: 0x040008DC RID: 2268
		private ColorSpace m_ColorSpace;

		// Token: 0x040008DD RID: 2269
		private TextureBlitter m_Blitter;

		// Token: 0x040008DE RID: 2270
		private int m_2SidePadding;

		// Token: 0x040008DF RID: 2271
		private int m_1SidePadding;

		// Token: 0x040008E0 RID: 2272
		private int m_MaxAtlasSize;

		// Token: 0x040008E1 RID: 2273
		private static ProfilerMarker s_MarkerReset = new ProfilerMarker("UIR.AtlasManager.Reset");

		// Token: 0x040008E4 RID: 2276
		private static int s_TextureCounter;
	}
}
