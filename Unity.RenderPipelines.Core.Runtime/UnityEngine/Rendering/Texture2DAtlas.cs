using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
	// Token: 0x020000C0 RID: 192
	public class Texture2DAtlas
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001DFC3 File Offset: 0x0001C1C3
		public static int maxMipLevelPadding
		{
			get
			{
				return Texture2DAtlas.s_MaxMipLevelPadding;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001DFCA File Offset: 0x0001C1CA
		public RTHandle AtlasTexture
		{
			get
			{
				return this.m_AtlasTexture;
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001DFD4 File Offset: 0x0001C1D4
		public Texture2DAtlas(int width, int height, GraphicsFormat format, FilterMode filterMode = FilterMode.Point, bool powerOfTwoPadding = false, string name = "", bool useMipMap = true)
		{
			this.m_Width = width;
			this.m_Height = height;
			this.m_Format = format;
			this.m_UseMipMaps = useMipMap;
			this.m_AtlasTexture = RTHandles.Alloc(this.m_Width, this.m_Height, 1, DepthBits.None, this.m_Format, filterMode, TextureWrapMode.Clamp, TextureDimension.Tex2D, false, useMipMap, false, false, 1, 0f, MSAASamples.None, false, false, RenderTextureMemoryless.None, VRTextureUsage.None, name);
			this.m_IsAtlasTextureOwner = true;
			int num = (useMipMap ? this.GetTextureMipmapCount(this.m_Width, this.m_Height) : 1);
			for (int i = 0; i < num; i++)
			{
				Graphics.SetRenderTarget(this.m_AtlasTexture, i);
				GL.Clear(false, true, Color.clear);
			}
			this.m_AtlasAllocator = new AtlasAllocator(width, height, powerOfTwoPadding);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001E0B5 File Offset: 0x0001C2B5
		public void Release()
		{
			this.ResetAllocator();
			if (this.m_IsAtlasTextureOwner)
			{
				RTHandles.Release(this.m_AtlasTexture);
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001E0D0 File Offset: 0x0001C2D0
		public void ResetAllocator()
		{
			this.m_AtlasAllocator.Reset();
			this.m_AllocationCache.Clear();
			this.m_IsGPUTextureUpToDate.Clear();
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001E0F4 File Offset: 0x0001C2F4
		public void ClearTarget(CommandBuffer cmd)
		{
			int num = (this.m_UseMipMaps ? this.GetTextureMipmapCount(this.m_Width, this.m_Height) : 1);
			for (int i = 0; i < num; i++)
			{
				cmd.SetRenderTarget(this.m_AtlasTexture, i);
				Blitter.BlitQuad(cmd, Texture2D.blackTexture, Texture2DAtlas.fullScaleOffset, Texture2DAtlas.fullScaleOffset, i, true);
			}
			this.m_IsGPUTextureUpToDate.Clear();
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001E15F File Offset: 0x0001C35F
		private protected int GetTextureMipmapCount(int width, int height)
		{
			if (!this.m_UseMipMaps)
			{
				return 1;
			}
			return Mathf.FloorToInt(Mathf.Log((float)Mathf.Max(width, height), 2f)) + 1;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001E184 File Offset: 0x0001C384
		private protected bool Is2D(Texture texture)
		{
			RenderTexture renderTexture = texture as RenderTexture;
			return texture is Texture2D || (renderTexture != null && renderTexture.dimension == TextureDimension.Tex2D);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001E1B0 File Offset: 0x0001C3B0
		private protected bool IsSingleChannelBlit(Texture source, Texture destination)
		{
			uint componentCount = GraphicsFormatUtility.GetComponentCount(source.graphicsFormat);
			uint componentCount2 = GraphicsFormatUtility.GetComponentCount(destination.graphicsFormat);
			if (componentCount == 1U || componentCount2 == 1U)
			{
				if (componentCount != componentCount2)
				{
					return true;
				}
				int num = (1 << (int)(GraphicsFormatUtility.GetSwizzleA(source.graphicsFormat) & (FormatSwizzle)7) << 24) | (1 << (int)(GraphicsFormatUtility.GetSwizzleB(source.graphicsFormat) & (FormatSwizzle)7) << 16) | (1 << (int)(GraphicsFormatUtility.GetSwizzleG(source.graphicsFormat) & (FormatSwizzle)7) << 8) | (1 << (int)(GraphicsFormatUtility.GetSwizzleR(source.graphicsFormat) & (FormatSwizzle)7));
				int num2 = (1 << (int)(GraphicsFormatUtility.GetSwizzleA(destination.graphicsFormat) & (FormatSwizzle)7) << 24) | (1 << (int)(GraphicsFormatUtility.GetSwizzleB(destination.graphicsFormat) & (FormatSwizzle)7) << 16) | (1 << (int)(GraphicsFormatUtility.GetSwizzleG(destination.graphicsFormat) & (FormatSwizzle)7) << 8) | (1 << (int)(GraphicsFormatUtility.GetSwizzleR(destination.graphicsFormat) & (FormatSwizzle)7));
				if (num != num2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001E294 File Offset: 0x0001C494
		private void Blit2DTexture(CommandBuffer cmd, Vector4 scaleOffset, Texture texture, Vector4 sourceScaleOffset, bool blitMips, Texture2DAtlas.BlitType blitType)
		{
			int num = this.GetTextureMipmapCount(texture.width, texture.height);
			if (!blitMips)
			{
				num = 1;
			}
			for (int i = 0; i < num; i++)
			{
				cmd.SetRenderTarget(this.m_AtlasTexture, i);
				switch (blitType)
				{
				case Texture2DAtlas.BlitType.Default:
					Blitter.BlitQuad(cmd, texture, sourceScaleOffset, scaleOffset, i, true);
					break;
				case Texture2DAtlas.BlitType.CubeTo2DOctahedral:
					Blitter.BlitCubeToOctahedral2DQuad(cmd, texture, scaleOffset, i);
					break;
				case Texture2DAtlas.BlitType.SingleChannel:
					Blitter.BlitQuadSingleChannel(cmd, texture, sourceScaleOffset, scaleOffset, i);
					break;
				case Texture2DAtlas.BlitType.CubeTo2DOctahedralSingleChannel:
					Blitter.BlitCubeToOctahedral2DQuadSingleChannel(cmd, texture, scaleOffset, i);
					break;
				}
			}
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001E320 File Offset: 0x0001C520
		private protected void MarkGPUTextureValid(int instanceId, bool mipAreValid = false)
		{
			this.m_IsGPUTextureUpToDate[instanceId] = (mipAreValid ? 2 : 1);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001E335 File Offset: 0x0001C535
		private protected void MarkGPUTextureInvalid(int instanceId)
		{
			this.m_IsGPUTextureUpToDate[instanceId] = 0;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001E344 File Offset: 0x0001C544
		public virtual void BlitTexture(CommandBuffer cmd, Vector4 scaleOffset, Texture texture, Vector4 sourceScaleOffset, bool blitMips = true, int overrideInstanceID = -1)
		{
			if (this.Is2D(texture))
			{
				Texture2DAtlas.BlitType blitType = Texture2DAtlas.BlitType.Default;
				if (this.IsSingleChannelBlit(texture, this.m_AtlasTexture.m_RT))
				{
					blitType = Texture2DAtlas.BlitType.SingleChannel;
				}
				this.Blit2DTexture(cmd, scaleOffset, texture, sourceScaleOffset, blitMips, blitType);
				int num = ((overrideInstanceID != -1) ? overrideInstanceID : this.GetTextureID(texture));
				this.MarkGPUTextureValid(num, blitMips);
				this.m_TextureHashes[num] = CoreUtils.GetTextureHash(texture);
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001E3AC File Offset: 0x0001C5AC
		public virtual void BlitOctahedralTexture(CommandBuffer cmd, Vector4 scaleOffset, Texture texture, Vector4 sourceScaleOffset, bool blitMips = true, int overrideInstanceID = -1)
		{
			this.BlitTexture(cmd, scaleOffset, texture, sourceScaleOffset, blitMips, overrideInstanceID);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001E3C0 File Offset: 0x0001C5C0
		public virtual void BlitCubeTexture2D(CommandBuffer cmd, Vector4 scaleOffset, Texture texture, bool blitMips = true, int overrideInstanceID = -1)
		{
			if (texture.dimension == TextureDimension.Cube)
			{
				Texture2DAtlas.BlitType blitType = Texture2DAtlas.BlitType.CubeTo2DOctahedral;
				if (this.IsSingleChannelBlit(texture, this.m_AtlasTexture.m_RT))
				{
					blitType = Texture2DAtlas.BlitType.CubeTo2DOctahedralSingleChannel;
				}
				this.Blit2DTexture(cmd, scaleOffset, texture, new Vector4(1f, 1f, 0f, 0f), blitMips, blitType);
				int num = ((overrideInstanceID != -1) ? overrideInstanceID : this.GetTextureID(texture));
				this.MarkGPUTextureValid(num, blitMips);
				this.m_TextureHashes[num] = CoreUtils.GetTextureHash(texture);
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001E440 File Offset: 0x0001C640
		public virtual bool AllocateTexture(CommandBuffer cmd, ref Vector4 scaleOffset, Texture texture, int width, int height, int overrideInstanceID = -1)
		{
			int num = ((overrideInstanceID != -1) ? overrideInstanceID : this.GetTextureID(texture));
			bool flag = this.AllocateTextureWithoutBlit(num, width, height, ref scaleOffset);
			if (flag)
			{
				if (this.Is2D(texture))
				{
					this.BlitTexture(cmd, scaleOffset, texture, Texture2DAtlas.fullScaleOffset, true, -1);
				}
				else
				{
					this.BlitCubeTexture2D(cmd, scaleOffset, texture, true, -1);
				}
				this.MarkGPUTextureValid(num, true);
				this.m_TextureHashes[num] = CoreUtils.GetTextureHash(texture);
			}
			return flag;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001E4B7 File Offset: 0x0001C6B7
		public bool AllocateTextureWithoutBlit(Texture texture, int width, int height, ref Vector4 scaleOffset)
		{
			return this.AllocateTextureWithoutBlit(texture.GetInstanceID(), width, height, ref scaleOffset);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001E4CC File Offset: 0x0001C6CC
		public virtual bool AllocateTextureWithoutBlit(int instanceId, int width, int height, ref Vector4 scaleOffset)
		{
			scaleOffset = Vector4.zero;
			if (this.m_AtlasAllocator.Allocate(ref scaleOffset, width, height))
			{
				scaleOffset.Scale(new Vector4(1f / (float)this.m_Width, 1f / (float)this.m_Height, 1f / (float)this.m_Width, 1f / (float)this.m_Height));
				this.m_AllocationCache[instanceId] = new ValueTuple<Vector4, Vector2Int>(scaleOffset, new Vector2Int(width, height));
				this.MarkGPUTextureInvalid(instanceId);
				this.m_TextureHashes[instanceId] = -1;
				return true;
			}
			return false;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001E56C File Offset: 0x0001C76C
		private protected int GetTextureHash(Texture textureA, Texture textureB)
		{
			return CoreUtils.GetTextureHash(textureA) + 23 * CoreUtils.GetTextureHash(textureB);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001E57E File Offset: 0x0001C77E
		public int GetTextureID(Texture texture)
		{
			return texture.GetInstanceID();
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001E586 File Offset: 0x0001C786
		public int GetTextureID(Texture textureA, Texture textureB)
		{
			return this.GetTextureID(textureA) + 23 * this.GetTextureID(textureB);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001E59A File Offset: 0x0001C79A
		public bool IsCached(out Vector4 scaleOffset, Texture textureA, Texture textureB)
		{
			return this.IsCached(out scaleOffset, this.GetTextureID(textureA, textureB));
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001E5AB File Offset: 0x0001C7AB
		public bool IsCached(out Vector4 scaleOffset, Texture texture)
		{
			return this.IsCached(out scaleOffset, this.GetTextureID(texture));
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001E5BC File Offset: 0x0001C7BC
		public bool IsCached(out Vector4 scaleOffset, int id)
		{
			ValueTuple<Vector4, Vector2Int> valueTuple;
			bool flag = this.m_AllocationCache.TryGetValue(id, out valueTuple);
			scaleOffset = valueTuple.Item1;
			return flag;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001E5E4 File Offset: 0x0001C7E4
		internal Vector2Int GetCachedTextureSize(int id)
		{
			ValueTuple<Vector4, Vector2Int> valueTuple;
			this.m_AllocationCache.TryGetValue(id, out valueTuple);
			return valueTuple.Item2;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001E608 File Offset: 0x0001C808
		public virtual bool NeedsUpdate(Texture texture, bool needMips = false)
		{
			RenderTexture renderTexture = texture as RenderTexture;
			int textureID = this.GetTextureID(texture);
			int textureHash = CoreUtils.GetTextureHash(texture);
			if (renderTexture != null)
			{
				int num;
				if (this.m_IsGPUTextureUpToDate.TryGetValue(textureID, out num))
				{
					if ((ulong)renderTexture.updateCount != (ulong)((long)num))
					{
						this.m_IsGPUTextureUpToDate[textureID] = (int)renderTexture.updateCount;
						return true;
					}
				}
				else
				{
					this.m_IsGPUTextureUpToDate[textureID] = (int)renderTexture.updateCount;
				}
			}
			else
			{
				int num2;
				if (this.m_TextureHashes.TryGetValue(textureID, out num2) && num2 != textureHash)
				{
					this.m_TextureHashes[textureID] = textureHash;
					return true;
				}
				int num3;
				if (this.m_IsGPUTextureUpToDate.TryGetValue(textureID, out num3))
				{
					return num3 == 0 || (needMips && num3 == 1);
				}
			}
			return false;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001E6C0 File Offset: 0x0001C8C0
		public virtual bool NeedsUpdate(Texture textureA, Texture textureB, bool needMips = false)
		{
			RenderTexture renderTexture = textureA as RenderTexture;
			RenderTexture renderTexture2 = textureB as RenderTexture;
			int textureID = this.GetTextureID(textureA, textureB);
			int textureHash = this.GetTextureHash(textureA, textureB);
			if (renderTexture != null || renderTexture2 != null)
			{
				int num;
				if (this.m_IsGPUTextureUpToDate.TryGetValue(textureID, out num))
				{
					if (renderTexture != null && renderTexture2 != null && (ulong)Math.Min(renderTexture.updateCount, renderTexture2.updateCount) != (ulong)((long)num))
					{
						this.m_IsGPUTextureUpToDate[textureID] = (int)Math.Min(renderTexture.updateCount, renderTexture2.updateCount);
						return true;
					}
					if (renderTexture != null && (ulong)renderTexture.updateCount != (ulong)((long)num))
					{
						this.m_IsGPUTextureUpToDate[textureID] = (int)renderTexture.updateCount;
						return true;
					}
					if (renderTexture2 != null && (ulong)renderTexture2.updateCount != (ulong)((long)num))
					{
						this.m_IsGPUTextureUpToDate[textureID] = (int)renderTexture2.updateCount;
						return true;
					}
				}
				else
				{
					this.m_IsGPUTextureUpToDate[textureID] = textureHash;
				}
			}
			else
			{
				int num2;
				if (this.m_TextureHashes.TryGetValue(textureID, out num2) && num2 != textureHash)
				{
					this.m_TextureHashes[textureID] = textureID;
					return true;
				}
				int num3;
				if (this.m_IsGPUTextureUpToDate.TryGetValue(textureID, out num3))
				{
					return num3 == 0 || (needMips && num3 == 1);
				}
			}
			return false;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001E804 File Offset: 0x0001CA04
		public virtual bool AddTexture(CommandBuffer cmd, ref Vector4 scaleOffset, Texture texture)
		{
			return this.IsCached(out scaleOffset, texture) || this.AllocateTexture(cmd, ref scaleOffset, texture, texture.width, texture.height, -1);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001E828 File Offset: 0x0001CA28
		public virtual bool UpdateTexture(CommandBuffer cmd, Texture oldTexture, Texture newTexture, ref Vector4 scaleOffset, Vector4 sourceScaleOffset, bool updateIfNeeded = true, bool blitMips = true)
		{
			if (this.IsCached(out scaleOffset, oldTexture))
			{
				if (updateIfNeeded && this.NeedsUpdate(newTexture, false))
				{
					if (this.Is2D(newTexture))
					{
						this.BlitTexture(cmd, scaleOffset, newTexture, sourceScaleOffset, blitMips, -1);
					}
					else
					{
						this.BlitCubeTexture2D(cmd, scaleOffset, newTexture, blitMips, -1);
					}
					this.MarkGPUTextureValid(this.GetTextureID(newTexture), blitMips);
				}
				return true;
			}
			return this.AllocateTexture(cmd, ref scaleOffset, newTexture, newTexture.width, newTexture.height, -1);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001E8A7 File Offset: 0x0001CAA7
		public virtual bool UpdateTexture(CommandBuffer cmd, Texture texture, ref Vector4 scaleOffset, bool updateIfNeeded = true, bool blitMips = true)
		{
			return this.UpdateTexture(cmd, texture, texture, ref scaleOffset, Texture2DAtlas.fullScaleOffset, updateIfNeeded, blitMips);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001E8BC File Offset: 0x0001CABC
		internal bool EnsureTextureSlot(out bool isUploadNeeded, ref Vector4 scaleBias, int key, int width, int height)
		{
			isUploadNeeded = false;
			ValueTuple<Vector4, Vector2Int> valueTuple;
			if (this.m_AllocationCache.TryGetValue(key, out valueTuple))
			{
				scaleBias = valueTuple.Item1;
				return true;
			}
			if (!this.m_AtlasAllocator.Allocate(ref scaleBias, width, height))
			{
				return false;
			}
			isUploadNeeded = true;
			scaleBias.Scale(new Vector4(1f / (float)this.m_Width, 1f / (float)this.m_Height, 1f / (float)this.m_Width, 1f / (float)this.m_Height));
			this.m_AllocationCache.Add(key, new ValueTuple<Vector4, Vector2Int>(scaleBias, new Vector2Int(width, height)));
			return true;
		}

		// Token: 0x04000426 RID: 1062
		private protected const int kGPUTexInvalid = 0;

		// Token: 0x04000427 RID: 1063
		private protected const int kGPUTexValidMip0 = 1;

		// Token: 0x04000428 RID: 1064
		private protected const int kGPUTexValidMipAll = 2;

		// Token: 0x04000429 RID: 1065
		private protected RTHandle m_AtlasTexture;

		// Token: 0x0400042A RID: 1066
		private protected int m_Width;

		// Token: 0x0400042B RID: 1067
		private protected int m_Height;

		// Token: 0x0400042C RID: 1068
		private protected GraphicsFormat m_Format;

		// Token: 0x0400042D RID: 1069
		private protected bool m_UseMipMaps;

		// Token: 0x0400042E RID: 1070
		private bool m_IsAtlasTextureOwner;

		// Token: 0x0400042F RID: 1071
		private AtlasAllocator m_AtlasAllocator;

		// Token: 0x04000430 RID: 1072
		[TupleElementNames(new string[] { "scaleOffset", "size" })]
		private Dictionary<int, ValueTuple<Vector4, Vector2Int>> m_AllocationCache = new Dictionary<int, ValueTuple<Vector4, Vector2Int>>();

		// Token: 0x04000431 RID: 1073
		private Dictionary<int, int> m_IsGPUTextureUpToDate = new Dictionary<int, int>();

		// Token: 0x04000432 RID: 1074
		private Dictionary<int, int> m_TextureHashes = new Dictionary<int, int>();

		// Token: 0x04000433 RID: 1075
		private static readonly Vector4 fullScaleOffset = new Vector4(1f, 1f, 0f, 0f);

		// Token: 0x04000434 RID: 1076
		private static readonly int s_MaxMipLevelPadding = 10;

		// Token: 0x020001C4 RID: 452
		private enum BlitType
		{
			// Token: 0x04000750 RID: 1872
			Default,
			// Token: 0x04000751 RID: 1873
			CubeTo2DOctahedral,
			// Token: 0x04000752 RID: 1874
			SingleChannel,
			// Token: 0x04000753 RID: 1875
			CubeTo2DOctahedralSingleChannel
		}
	}
}
