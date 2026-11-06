using System;
using Unity.Profiling;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000443 RID: 1091
	internal class GradientSettingsAtlas : IDisposable
	{
		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x000842E4 File Offset: 0x000824E4
		internal int length
		{
			get
			{
				return this.m_Length;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x000842FC File Offset: 0x000824FC
		// (set) Token: 0x06002259 RID: 8793 RVA: 0x00084304 File Offset: 0x00082504
		private protected bool disposed { protected get; private set; }

		// Token: 0x0600225A RID: 8794 RVA: 0x0008430D File Offset: 0x0008250D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x00084320 File Offset: 0x00082520
		protected virtual void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					UIRUtility.Destroy(this.m_Atlas);
				}
				this.disposed = true;
			}
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x00084357 File Offset: 0x00082557
		public GradientSettingsAtlas(int length = 4096)
		{
			this.m_Length = length;
			this.m_ElemWidth = 3;
			this.Reset();
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00084378 File Offset: 0x00082578
		public void Reset()
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				this.m_Allocator = new BestFitAllocator((uint)this.m_Length);
				UIRUtility.Destroy(this.m_Atlas);
				this.m_RawAtlas = default(GradientSettingsAtlas.RawTexture);
				this.MustCommit = false;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x0600225E RID: 8798 RVA: 0x000843CC File Offset: 0x000825CC
		public Texture2D atlas
		{
			get
			{
				return this.m_Atlas;
			}
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x000843E4 File Offset: 0x000825E4
		public Alloc Add(int count)
		{
			Debug.Assert(count > 0);
			bool disposed = this.disposed;
			Alloc alloc;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
				alloc = default(Alloc);
			}
			else
			{
				Alloc alloc2 = this.m_Allocator.Allocate((uint)count);
				alloc = alloc2;
			}
			return alloc;
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x0008442C File Offset: 0x0008262C
		public void Remove(Alloc alloc)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				this.m_Allocator.Free(alloc);
			}
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x0008445C File Offset: 0x0008265C
		public void Write(Alloc alloc, GradientSettings[] settings, GradientRemap remap)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				bool flag = this.m_RawAtlas.rgba == null;
				if (flag)
				{
					this.m_RawAtlas = new GradientSettingsAtlas.RawTexture
					{
						rgba = new Color32[this.m_ElemWidth * this.m_Length],
						width = this.m_ElemWidth,
						height = this.m_Length
					};
					int num = this.m_ElemWidth * this.m_Length;
					for (int i = 0; i < num; i++)
					{
						this.m_RawAtlas.rgba[i] = Color.black;
					}
				}
				int num2 = (int)alloc.start;
				int j = 0;
				int num3 = settings.Length;
				while (j < num3)
				{
					int num4 = 0;
					GradientSettings gradientSettings = settings[j];
					Debug.Assert(remap == null || num2 == remap.destIndex);
					bool flag2 = gradientSettings.gradientType == GradientType.Radial;
					if (flag2)
					{
						Vector2 vector = gradientSettings.radialFocus;
						vector += Vector2.one;
						vector /= 2f;
						vector.y = 1f - vector.y;
						this.m_RawAtlas.WriteRawFloat4Packed(0.003921569f, (float)gradientSettings.addressMode / 255f, vector.x, vector.y, num4++, num2);
					}
					else
					{
						bool flag3 = gradientSettings.gradientType == GradientType.Linear;
						if (flag3)
						{
							this.m_RawAtlas.WriteRawFloat4Packed(0f, (float)gradientSettings.addressMode / 255f, 0f, 0f, num4++, num2);
						}
					}
					Vector2Int vector2Int = new Vector2Int(gradientSettings.location.x, gradientSettings.location.y);
					Vector2 vector2 = new Vector2((float)(gradientSettings.location.width - 1), (float)(gradientSettings.location.height - 1));
					bool flag4 = remap != null;
					if (flag4)
					{
						vector2Int = new Vector2Int(remap.location.x, remap.location.y);
						vector2 = new Vector2((float)(remap.location.width - 1), (float)(remap.location.height - 1));
					}
					this.m_RawAtlas.WriteRawInt2Packed(vector2Int.x, vector2Int.y, num4++, num2);
					this.m_RawAtlas.WriteRawInt2Packed((int)vector2.x, (int)vector2.y, num4++, num2);
					remap = ((remap != null) ? remap.next : null);
					num2++;
					j++;
				}
				this.MustCommit = true;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06002262 RID: 8802 RVA: 0x0008471D File Offset: 0x0008291D
		// (set) Token: 0x06002263 RID: 8803 RVA: 0x00084725 File Offset: 0x00082925
		public bool MustCommit { get; private set; }

		// Token: 0x06002264 RID: 8804 RVA: 0x00084730 File Offset: 0x00082930
		public void Commit()
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				bool flag = !this.MustCommit;
				if (!flag)
				{
					this.PrepareAtlas();
					this.m_Atlas.SetPixels32(this.m_RawAtlas.rgba);
					this.m_Atlas.Apply();
					this.MustCommit = false;
				}
			}
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x00084794 File Offset: 0x00082994
		private void PrepareAtlas()
		{
			bool flag = this.m_Atlas != null;
			if (!flag)
			{
				this.m_Atlas = new Texture2D(this.m_ElemWidth, this.m_Length, TextureFormat.ARGB32, 0, true)
				{
					hideFlags = HideFlags.HideAndDontSave,
					name = "GradientSettings " + GradientSettingsAtlas.s_TextureCounter++.ToString(),
					filterMode = FilterMode.Point
				};
			}
		}

		// Token: 0x04000F2E RID: 3886
		private static ProfilerMarker s_MarkerWrite = new ProfilerMarker("UIR.GradientSettingsAtlas.Write");

		// Token: 0x04000F2F RID: 3887
		private static ProfilerMarker s_MarkerCommit = new ProfilerMarker("UIR.GradientSettingsAtlas.Commit");

		// Token: 0x04000F30 RID: 3888
		private readonly int m_Length;

		// Token: 0x04000F31 RID: 3889
		private readonly int m_ElemWidth;

		// Token: 0x04000F32 RID: 3890
		private BestFitAllocator m_Allocator;

		// Token: 0x04000F33 RID: 3891
		private Texture2D m_Atlas;

		// Token: 0x04000F34 RID: 3892
		private GradientSettingsAtlas.RawTexture m_RawAtlas;

		// Token: 0x04000F35 RID: 3893
		private static int s_TextureCounter;

		// Token: 0x02000444 RID: 1092
		private struct RawTexture
		{
			// Token: 0x06002267 RID: 8807 RVA: 0x00084828 File Offset: 0x00082A28
			public void WriteRawInt2Packed(int v0, int v1, int destX, int destY)
			{
				byte b = (byte)(v0 / 255);
				byte b2 = (byte)(v0 - (int)(b * byte.MaxValue));
				byte b3 = (byte)(v1 / 255);
				byte b4 = (byte)(v1 - (int)(b3 * byte.MaxValue));
				int num = destY * this.width + destX;
				this.rgba[num] = new Color32(b, b2, b3, b4);
			}

			// Token: 0x06002268 RID: 8808 RVA: 0x00084884 File Offset: 0x00082A84
			public void WriteRawFloat4Packed(float f0, float f1, float f2, float f3, int destX, int destY)
			{
				byte b = (byte)(f0 * 255f + 0.5f);
				byte b2 = (byte)(f1 * 255f + 0.5f);
				byte b3 = (byte)(f2 * 255f + 0.5f);
				byte b4 = (byte)(f3 * 255f + 0.5f);
				int num = destY * this.width + destX;
				this.rgba[num] = new Color32(b, b2, b3, b4);
			}

			// Token: 0x04000F38 RID: 3896
			public Color32[] rgba;

			// Token: 0x04000F39 RID: 3897
			public int width;

			// Token: 0x04000F3A RID: 3898
			public int height;
		}
	}
}
