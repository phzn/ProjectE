using System;
using Unity.Collections;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000460 RID: 1120
	internal class ShaderInfoStorage<T> : BaseShaderInfoStorage where T : struct
	{
		// Token: 0x06002301 RID: 8961 RVA: 0x00087BC4 File Offset: 0x00085DC4
		public ShaderInfoStorage(TextureFormat format, Func<Color, T> convert, int initialSize = 64, int maxSize = 4096)
		{
			Debug.Assert(maxSize <= SystemInfo.maxTextureSize);
			Debug.Assert(initialSize <= maxSize);
			Debug.Assert(Mathf.IsPowerOfTwo(initialSize));
			Debug.Assert(Mathf.IsPowerOfTwo(maxSize));
			Debug.Assert(convert != null);
			this.m_InitialSize = initialSize;
			this.m_MaxSize = maxSize;
			this.m_Format = format;
			this.m_Convert = convert;
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x00087C3C File Offset: 0x00085E3C
		protected override void Dispose(bool disposing)
		{
			bool flag = !base.disposed && disposing;
			if (flag)
			{
				UIRUtility.Destroy(this.m_Texture);
				this.m_Texture = null;
				this.m_Texels = default(NativeArray<T>);
				UIRAtlasAllocator allocator = this.m_Allocator;
				if (allocator != null)
				{
					allocator.Dispose();
				}
				this.m_Allocator = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002303 RID: 8963 RVA: 0x00087C9B File Offset: 0x00085E9B
		public override Texture2D texture
		{
			get
			{
				return this.m_Texture;
			}
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x00087CA4 File Offset: 0x00085EA4
		public override bool AllocateRect(int width, int height, out RectInt uvs)
		{
			bool disposed = base.disposed;
			bool flag;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
				uvs = default(RectInt);
				flag = false;
			}
			else
			{
				bool flag2 = this.m_Allocator == null;
				if (flag2)
				{
					this.m_Allocator = new UIRAtlasAllocator(this.m_InitialSize, this.m_MaxSize, 0);
				}
				bool flag3 = !this.m_Allocator.TryAllocate(width, height, out uvs);
				if (flag3)
				{
					flag = false;
				}
				else
				{
					uvs = new RectInt(uvs.x, uvs.y, width, height);
					this.CreateOrExpandTexture();
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x00087D34 File Offset: 0x00085F34
		public override void SetTexel(int x, int y, Color color)
		{
			bool disposed = base.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				bool flag = !this.m_Texels.IsCreated;
				if (flag)
				{
					this.m_Texels = this.m_Texture.GetRawTextureData<T>();
				}
				this.m_Texels[x + y * this.m_Texture.width] = this.m_Convert(color);
			}
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x00087DA4 File Offset: 0x00085FA4
		public override void UpdateTexture()
		{
			bool disposed = base.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				bool flag = this.m_Texture == null || !this.m_Texels.IsCreated;
				if (!flag)
				{
					this.m_Texture.Apply(false, false);
					this.m_Texels = default(NativeArray<T>);
				}
			}
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x00087E08 File Offset: 0x00086008
		private void CreateOrExpandTexture()
		{
			int physicalWidth = this.m_Allocator.physicalWidth;
			int physicalHeight = this.m_Allocator.physicalHeight;
			bool flag = false;
			bool flag2 = this.m_Texture != null;
			if (flag2)
			{
				bool flag3 = this.m_Texture.width == physicalWidth && this.m_Texture.height == physicalHeight;
				if (flag3)
				{
					return;
				}
				flag = true;
			}
			Texture2D texture2D = new Texture2D(this.m_Allocator.physicalWidth, this.m_Allocator.physicalHeight, this.m_Format, false)
			{
				name = "UIR Shader Info " + BaseShaderInfoStorage.s_TextureCounter++.ToString(),
				hideFlags = HideFlags.HideAndDontSave,
				filterMode = FilterMode.Point
			};
			bool flag4 = flag;
			if (flag4)
			{
				NativeArray<T> nativeArray = (this.m_Texels.IsCreated ? this.m_Texels : this.m_Texture.GetRawTextureData<T>());
				NativeArray<T> rawTextureData = texture2D.GetRawTextureData<T>();
				ShaderInfoStorage<T>.CpuBlit(nativeArray, this.m_Texture.width, this.m_Texture.height, rawTextureData, texture2D.width, texture2D.height);
				this.m_Texels = rawTextureData;
			}
			else
			{
				this.m_Texels = default(NativeArray<T>);
			}
			UIRUtility.Destroy(this.m_Texture);
			this.m_Texture = texture2D;
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x00087F54 File Offset: 0x00086154
		private static void CpuBlit(NativeArray<T> src, int srcWidth, int srcHeight, NativeArray<T> dst, int dstWidth, int dstHeight)
		{
			Debug.Assert(dstWidth >= srcWidth && dstHeight >= srcHeight);
			int num = dstWidth - srcWidth;
			int num2 = dstHeight - srcHeight;
			int num3 = srcWidth * srcHeight;
			int i = 0;
			int num4 = 0;
			int num5 = srcWidth;
			while (i < num3)
			{
				while (i < num5)
				{
					dst[num4] = src[i];
					num4++;
					i++;
				}
				num5 += srcWidth;
				num4 += num;
			}
		}

		// Token: 0x04001021 RID: 4129
		private readonly int m_InitialSize;

		// Token: 0x04001022 RID: 4130
		private readonly int m_MaxSize;

		// Token: 0x04001023 RID: 4131
		private readonly TextureFormat m_Format;

		// Token: 0x04001024 RID: 4132
		private readonly Func<Color, T> m_Convert;

		// Token: 0x04001025 RID: 4133
		private UIRAtlasAllocator m_Allocator;

		// Token: 0x04001026 RID: 4134
		private Texture2D m_Texture;

		// Token: 0x04001027 RID: 4135
		private NativeArray<T> m_Texels;
	}
}
