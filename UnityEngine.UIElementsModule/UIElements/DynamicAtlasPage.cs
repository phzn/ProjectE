using System;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x020002A8 RID: 680
	internal class DynamicAtlasPage : IDisposable
	{
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x00044543 File Offset: 0x00042743
		// (set) Token: 0x0600138C RID: 5004 RVA: 0x0004454B File Offset: 0x0004274B
		public TextureId textureId { get; private set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x00044554 File Offset: 0x00042754
		// (set) Token: 0x0600138E RID: 5006 RVA: 0x0004455C File Offset: 0x0004275C
		public RenderTexture atlas { get; private set; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x00044565 File Offset: 0x00042765
		public RenderTextureFormat format { get; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x0004456D File Offset: 0x0004276D
		public FilterMode filterMode { get; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x00044575 File Offset: 0x00042775
		public Vector2Int minSize { get; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x0004457D File Offset: 0x0004277D
		public Vector2Int maxSize { get; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x00044585 File Offset: 0x00042785
		public Vector2Int currentSize
		{
			get
			{
				return this.m_CurrentSize;
			}
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00044590 File Offset: 0x00042790
		public DynamicAtlasPage(RenderTextureFormat format, FilterMode filterMode, Vector2Int minSize, Vector2Int maxSize)
		{
			this.textureId = TextureRegistry.instance.AllocAndAcquireDynamic();
			this.format = format;
			this.filterMode = filterMode;
			this.minSize = minSize;
			this.maxSize = maxSize;
			this.m_Allocator = new Allocator2D(minSize, maxSize, this.m_2Padding);
			this.m_Blitter = new TextureBlitter(64);
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x00044602 File Offset: 0x00042802
		// (set) Token: 0x06001396 RID: 5014 RVA: 0x0004460A File Offset: 0x0004280A
		private protected bool disposed { protected get; private set; }

		// Token: 0x06001397 RID: 5015 RVA: 0x00044613 File Offset: 0x00042813
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00044628 File Offset: 0x00042828
		protected virtual void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					bool flag = this.atlas != null;
					if (flag)
					{
						UIRUtility.Destroy(this.atlas);
						this.atlas = null;
					}
					bool flag2 = this.m_Allocator != null;
					if (flag2)
					{
						this.m_Allocator = null;
					}
					bool flag3 = this.m_Blitter != null;
					if (flag3)
					{
						this.m_Blitter.Dispose();
						this.m_Blitter = null;
					}
					bool flag4 = this.textureId != TextureId.invalid;
					if (flag4)
					{
						TextureRegistry.instance.Release(this.textureId);
						this.textureId = TextureId.invalid;
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x000446F0 File Offset: 0x000428F0
		public bool TryAdd(Texture2D image, out Allocator2D.Alloc2D alloc, out RectInt rect)
		{
			bool disposed = this.disposed;
			bool flag;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
				alloc = default(Allocator2D.Alloc2D);
				rect = default(RectInt);
				flag = false;
			}
			else
			{
				bool flag2 = !this.m_Allocator.TryAllocate(image.width + this.m_2Padding, image.height + this.m_2Padding, out alloc);
				if (flag2)
				{
					rect = default(RectInt);
					flag = false;
				}
				else
				{
					this.m_CurrentSize.x = Mathf.Max(this.m_CurrentSize.x, UIRUtility.GetNextPow2(alloc.rect.xMax));
					this.m_CurrentSize.y = Mathf.Max(this.m_CurrentSize.y, UIRUtility.GetNextPow2(alloc.rect.yMax));
					rect = new RectInt(alloc.rect.xMin + this.m_1Padding, alloc.rect.yMin + this.m_1Padding, image.width, image.height);
					this.Update(image, rect);
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00044808 File Offset: 0x00042A08
		public void Update(Texture2D image, RectInt rect)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				Debug.Assert(image != null && rect.width > 0 && rect.height > 0);
				this.m_Blitter.QueueBlit(image, new RectInt(0, 0, image.width, image.height), new Vector2Int(rect.x, rect.y), true, Color.white);
			}
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0004488C File Offset: 0x00042A8C
		public void Remove(Allocator2D.Alloc2D alloc)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				Debug.Assert(alloc.rect.width > 0 && alloc.rect.height > 0);
				this.m_Allocator.Free(alloc);
			}
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x000448E4 File Offset: 0x00042AE4
		public void Commit()
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				this.UpdateAtlasTexture();
				this.m_Blitter.Commit(this.atlas);
			}
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00044920 File Offset: 0x00042B20
		private void UpdateAtlasTexture()
		{
			bool flag = this.atlas == null;
			if (flag)
			{
				this.atlas = this.CreateAtlasTexture();
			}
			else
			{
				bool flag2 = this.atlas.width != this.m_CurrentSize.x || this.atlas.height != this.m_CurrentSize.y;
				if (flag2)
				{
					RenderTexture renderTexture = this.CreateAtlasTexture();
					bool flag3 = renderTexture == null;
					if (flag3)
					{
						Debug.LogErrorFormat("Failed to allocate a render texture for the dynamic atlas. Current Size = {0}x{1}. Requested Size = {2}x{3}.", new object[]
						{
							this.atlas.width,
							this.atlas.height,
							this.m_CurrentSize.x,
							this.m_CurrentSize.y
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

		// Token: 0x0600139E RID: 5022 RVA: 0x00044A54 File Offset: 0x00042C54
		private RenderTexture CreateAtlasTexture()
		{
			bool flag = this.m_CurrentSize.x == 0 || this.m_CurrentSize.y == 0;
			RenderTexture renderTexture;
			if (flag)
			{
				renderTexture = null;
			}
			else
			{
				renderTexture = new RenderTexture(this.m_CurrentSize.x, this.m_CurrentSize.y, 0, this.format)
				{
					hideFlags = HideFlags.HideAndDontSave,
					name = "UIR Dynamic Atlas Page " + DynamicAtlasPage.s_TextureCounter++.ToString(),
					filterMode = this.filterMode
				};
			}
			return renderTexture;
		}

		// Token: 0x040008ED RID: 2285
		private readonly int m_1Padding = 1;

		// Token: 0x040008EE RID: 2286
		private readonly int m_2Padding = 2;

		// Token: 0x040008EF RID: 2287
		private Allocator2D m_Allocator;

		// Token: 0x040008F0 RID: 2288
		private TextureBlitter m_Blitter;

		// Token: 0x040008F1 RID: 2289
		private Vector2Int m_CurrentSize;

		// Token: 0x040008F2 RID: 2290
		private static int s_TextureCounter;
	}
}
