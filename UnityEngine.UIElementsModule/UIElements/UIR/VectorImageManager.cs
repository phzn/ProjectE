using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000474 RID: 1140
	internal class VectorImageManager : IDisposable
	{
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002347 RID: 9031 RVA: 0x000890E8 File Offset: 0x000872E8
		public Texture2D atlas
		{
			get
			{
				GradientSettingsAtlas gradientSettingsAtlas = this.m_GradientSettingsAtlas;
				return (gradientSettingsAtlas != null) ? gradientSettingsAtlas.atlas : null;
			}
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x0008910C File Offset: 0x0008730C
		public VectorImageManager(AtlasBase atlas)
		{
			VectorImageManager.instances.Add(this);
			this.m_Atlas = atlas;
			this.m_Registered = new Dictionary<VectorImage, VectorImageRenderInfo>(32);
			this.m_RenderInfoPool = new VectorImageRenderInfoPool();
			this.m_GradientRemapPool = new GradientRemapPool();
			this.m_GradientSettingsAtlas = new GradientSettingsAtlas(4096);
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x00089167 File Offset: 0x00087367
		// (set) Token: 0x0600234A RID: 9034 RVA: 0x0008916F File Offset: 0x0008736F
		private protected bool disposed { protected get; private set; }

		// Token: 0x0600234B RID: 9035 RVA: 0x00089178 File Offset: 0x00087378
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x0008918C File Offset: 0x0008738C
		protected virtual void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.m_Registered.Clear();
					this.m_RenderInfoPool.Clear();
					this.m_GradientRemapPool.Clear();
					this.m_GradientSettingsAtlas.Dispose();
					VectorImageManager.instances.Remove(this);
				}
				this.disposed = true;
			}
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x000891F4 File Offset: 0x000873F4
		public void Reset()
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				this.m_Registered.Clear();
				this.m_RenderInfoPool.Clear();
				this.m_GradientRemapPool.Clear();
				this.m_GradientSettingsAtlas.Reset();
			}
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x00089248 File Offset: 0x00087448
		public void Commit()
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				this.m_GradientSettingsAtlas.Commit();
			}
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x00089278 File Offset: 0x00087478
		public GradientRemap AddUser(VectorImage vi, VisualElement context)
		{
			bool disposed = this.disposed;
			GradientRemap gradientRemap;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
				gradientRemap = null;
			}
			else
			{
				bool flag = vi == null;
				if (flag)
				{
					gradientRemap = null;
				}
				else
				{
					VectorImageRenderInfo vectorImageRenderInfo;
					bool flag2 = this.m_Registered.TryGetValue(vi, out vectorImageRenderInfo);
					if (flag2)
					{
						vectorImageRenderInfo.useCount++;
					}
					else
					{
						vectorImageRenderInfo = this.Register(vi, context);
					}
					gradientRemap = vectorImageRenderInfo.firstGradientRemap;
				}
			}
			return gradientRemap;
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x000892E4 File Offset: 0x000874E4
		public void RemoveUser(VectorImage vi)
		{
			bool disposed = this.disposed;
			if (disposed)
			{
				DisposeHelper.NotifyDisposedUsed(this);
			}
			else
			{
				bool flag = vi == null;
				if (!flag)
				{
					VectorImageRenderInfo vectorImageRenderInfo;
					bool flag2 = this.m_Registered.TryGetValue(vi, out vectorImageRenderInfo);
					if (flag2)
					{
						vectorImageRenderInfo.useCount--;
						bool flag3 = vectorImageRenderInfo.useCount == 0;
						if (flag3)
						{
							this.Unregister(vi, vectorImageRenderInfo);
						}
					}
				}
			}
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x00089350 File Offset: 0x00087550
		private VectorImageRenderInfo Register(VectorImage vi, VisualElement context)
		{
			VectorImageRenderInfo vectorImageRenderInfo = this.m_RenderInfoPool.Get();
			vectorImageRenderInfo.useCount = 1;
			this.m_Registered[vi] = vectorImageRenderInfo;
			GradientSettings[] settings = vi.settings;
			bool flag = settings != null && settings.Length != 0;
			if (flag)
			{
				int num = vi.settings.Length;
				Alloc alloc = this.m_GradientSettingsAtlas.Add(num);
				bool flag2 = alloc.size > 0U;
				if (flag2)
				{
					TextureId textureId;
					RectInt rectInt;
					bool flag3 = this.m_Atlas.TryGetAtlas(context, vi.atlas, out textureId, out rectInt);
					if (flag3)
					{
						GradientRemap gradientRemap = null;
						for (int i = 0; i < num; i++)
						{
							GradientRemap gradientRemap2 = this.m_GradientRemapPool.Get();
							bool flag4 = i > 0;
							if (flag4)
							{
								gradientRemap.next = gradientRemap2;
							}
							else
							{
								vectorImageRenderInfo.firstGradientRemap = gradientRemap2;
							}
							gradientRemap = gradientRemap2;
							gradientRemap2.origIndex = i;
							gradientRemap2.destIndex = (int)(alloc.start + (uint)i);
							GradientSettings gradientSettings = vi.settings[i];
							RectInt location = gradientSettings.location;
							location.x += rectInt.x;
							location.y += rectInt.y;
							gradientRemap2.location = location;
							gradientRemap2.atlas = textureId;
						}
						this.m_GradientSettingsAtlas.Write(alloc, vi.settings, vectorImageRenderInfo.firstGradientRemap);
					}
					else
					{
						GradientRemap gradientRemap3 = null;
						for (int j = 0; j < num; j++)
						{
							GradientRemap gradientRemap4 = this.m_GradientRemapPool.Get();
							bool flag5 = j > 0;
							if (flag5)
							{
								gradientRemap3.next = gradientRemap4;
							}
							else
							{
								vectorImageRenderInfo.firstGradientRemap = gradientRemap4;
							}
							gradientRemap3 = gradientRemap4;
							gradientRemap4.origIndex = j;
							gradientRemap4.destIndex = (int)(alloc.start + (uint)j);
							gradientRemap4.atlas = TextureId.invalid;
						}
						this.m_GradientSettingsAtlas.Write(alloc, vi.settings, null);
					}
				}
				else
				{
					bool flag6 = !this.m_LoggedExhaustedSettingsAtlas;
					if (flag6)
					{
						string text = "Exhausted max gradient settings (";
						string text2 = this.m_GradientSettingsAtlas.length.ToString();
						string text3 = ") for atlas: ";
						Texture2D atlas = this.m_GradientSettingsAtlas.atlas;
						Debug.LogError(text + text2 + text3 + ((atlas != null) ? atlas.name : null));
						this.m_LoggedExhaustedSettingsAtlas = true;
					}
				}
			}
			return vectorImageRenderInfo;
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000895AC File Offset: 0x000877AC
		private void Unregister(VectorImage vi, VectorImageRenderInfo renderInfo)
		{
			bool flag = renderInfo.gradientSettingsAlloc.size > 0U;
			if (flag)
			{
				this.m_GradientSettingsAtlas.Remove(renderInfo.gradientSettingsAlloc);
			}
			GradientRemap next;
			for (GradientRemap gradientRemap = renderInfo.firstGradientRemap; gradientRemap != null; gradientRemap = next)
			{
				next = gradientRemap.next;
				this.m_GradientRemapPool.Return(gradientRemap);
			}
			this.m_Registered.Remove(vi);
			this.m_RenderInfoPool.Return(renderInfo);
		}

		// Token: 0x0400106C RID: 4204
		public static List<VectorImageManager> instances = new List<VectorImageManager>(16);

		// Token: 0x0400106D RID: 4205
		private static ProfilerMarker s_MarkerRegister = new ProfilerMarker("UIR.VectorImageManager.Register");

		// Token: 0x0400106E RID: 4206
		private static ProfilerMarker s_MarkerUnregister = new ProfilerMarker("UIR.VectorImageManager.Unregister");

		// Token: 0x0400106F RID: 4207
		private readonly AtlasBase m_Atlas;

		// Token: 0x04001070 RID: 4208
		private Dictionary<VectorImage, VectorImageRenderInfo> m_Registered;

		// Token: 0x04001071 RID: 4209
		private VectorImageRenderInfoPool m_RenderInfoPool;

		// Token: 0x04001072 RID: 4210
		private GradientRemapPool m_GradientRemapPool;

		// Token: 0x04001073 RID: 4211
		private GradientSettingsAtlas m_GradientSettingsAtlas;

		// Token: 0x04001074 RID: 4212
		private bool m_LoggedExhaustedSettingsAtlas;
	}
}
