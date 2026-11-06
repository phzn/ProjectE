using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
	// Token: 0x020000C2 RID: 194
	internal class Texture2DAtlasDynamic
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x0001ED16 File Offset: 0x0001CF16
		public RTHandle AtlasTexture
		{
			get
			{
				return this.m_AtlasTexture;
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001ED20 File Offset: 0x0001CF20
		public Texture2DAtlasDynamic(int width, int height, int capacity, GraphicsFormat format)
		{
			this.m_Width = width;
			this.m_Height = height;
			this.m_Format = format;
			this.m_AtlasTexture = RTHandles.Alloc(this.m_Width, this.m_Height, 1, DepthBits.None, this.m_Format, FilterMode.Point, TextureWrapMode.Clamp, TextureDimension.Tex2D, false, true, false, false, 1, 0f, MSAASamples.None, false, false, RenderTextureMemoryless.None, VRTextureUsage.None, "");
			this.isAtlasTextureOwner = true;
			this.m_AtlasAllocator = new AtlasAllocatorDynamic(width, height, capacity);
			this.m_AllocationCache = new Dictionary<int, Vector4>(capacity);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001EDA0 File Offset: 0x0001CFA0
		public Texture2DAtlasDynamic(int width, int height, int capacity, RTHandle atlasTexture)
		{
			this.m_Width = width;
			this.m_Height = height;
			this.m_Format = atlasTexture.rt.graphicsFormat;
			this.m_AtlasTexture = atlasTexture;
			this.isAtlasTextureOwner = false;
			this.m_AtlasAllocator = new AtlasAllocatorDynamic(width, height, capacity);
			this.m_AllocationCache = new Dictionary<int, Vector4>(capacity);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001EDFC File Offset: 0x0001CFFC
		public void Release()
		{
			this.ResetAllocator();
			if (this.isAtlasTextureOwner)
			{
				RTHandles.Release(this.m_AtlasTexture);
			}
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001EE17 File Offset: 0x0001D017
		public void ResetAllocator()
		{
			this.m_AtlasAllocator.Release();
			this.m_AllocationCache.Clear();
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001EE30 File Offset: 0x0001D030
		public bool AddTexture(CommandBuffer cmd, out Vector4 scaleOffset, Texture texture)
		{
			int instanceID = texture.GetInstanceID();
			if (this.m_AllocationCache.TryGetValue(instanceID, out scaleOffset))
			{
				return true;
			}
			int width = texture.width;
			int height = texture.height;
			if (this.m_AtlasAllocator.Allocate(out scaleOffset, instanceID, width, height))
			{
				scaleOffset.Scale(new Vector4(1f / (float)this.m_Width, 1f / (float)this.m_Height, 1f / (float)this.m_Width, 1f / (float)this.m_Height));
				for (int i = 0; i < (texture as Texture2D).mipmapCount; i++)
				{
					cmd.SetRenderTarget(this.m_AtlasTexture, i);
					Blitter.BlitQuad(cmd, texture, new Vector4(1f, 1f, 0f, 0f), scaleOffset, i, false);
				}
				this.m_AllocationCache.Add(instanceID, scaleOffset);
				return true;
			}
			return false;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001EF1E File Offset: 0x0001D11E
		public bool IsCached(out Vector4 scaleOffset, int key)
		{
			return this.m_AllocationCache.TryGetValue(key, out scaleOffset);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001EF30 File Offset: 0x0001D130
		public bool EnsureTextureSlot(out bool isUploadNeeded, out Vector4 scaleOffset, int key, int width, int height)
		{
			isUploadNeeded = false;
			if (this.m_AllocationCache.TryGetValue(key, out scaleOffset))
			{
				return true;
			}
			if (!this.m_AtlasAllocator.Allocate(out scaleOffset, key, width, height))
			{
				return false;
			}
			isUploadNeeded = true;
			scaleOffset.Scale(new Vector4(1f / (float)this.m_Width, 1f / (float)this.m_Height, 1f / (float)this.m_Width, 1f / (float)this.m_Height));
			this.m_AllocationCache.Add(key, scaleOffset);
			return true;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001EFBB File Offset: 0x0001D1BB
		public void ReleaseTextureSlot(int key)
		{
			this.m_AtlasAllocator.Release(key);
			this.m_AllocationCache.Remove(key);
		}

		// Token: 0x0400043A RID: 1082
		private RTHandle m_AtlasTexture;

		// Token: 0x0400043B RID: 1083
		private bool isAtlasTextureOwner;

		// Token: 0x0400043C RID: 1084
		private int m_Width;

		// Token: 0x0400043D RID: 1085
		private int m_Height;

		// Token: 0x0400043E RID: 1086
		private GraphicsFormat m_Format;

		// Token: 0x0400043F RID: 1087
		private AtlasAllocatorDynamic m_AtlasAllocator;

		// Token: 0x04000440 RID: 1088
		private Dictionary<int, Vector4> m_AllocationCache;
	}
}
