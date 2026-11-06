using System;
using Unity.Profiling;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200045F RID: 1119
	internal abstract class BaseShaderInfoStorage : IDisposable
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x060022F7 RID: 8951
		public abstract Texture2D texture { get; }

		// Token: 0x060022F8 RID: 8952
		public abstract bool AllocateRect(int width, int height, out RectInt uvs);

		// Token: 0x060022F9 RID: 8953
		public abstract void SetTexel(int x, int y, Color color);

		// Token: 0x060022FA RID: 8954
		public abstract void UpdateTexture();

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x00087B45 File Offset: 0x00085D45
		// (set) Token: 0x060022FC RID: 8956 RVA: 0x00087B4D File Offset: 0x00085D4D
		private protected bool disposed { protected get; private set; }

		// Token: 0x060022FD RID: 8957 RVA: 0x00087B56 File Offset: 0x00085D56
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x00087B68 File Offset: 0x00085D68
		protected virtual void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				bool flag = !disposing;
				if (flag)
				{
				}
				this.disposed = true;
			}
		}

		// Token: 0x0400101C RID: 4124
		protected static int s_TextureCounter;

		// Token: 0x0400101D RID: 4125
		internal static ProfilerMarker s_MarkerCopyTexture = new ProfilerMarker("UIR.ShaderInfoStorage.CopyTexture");

		// Token: 0x0400101E RID: 4126
		internal static ProfilerMarker s_MarkerGetTextureData = new ProfilerMarker("UIR.ShaderInfoStorage.GetTextureData");

		// Token: 0x0400101F RID: 4127
		internal static ProfilerMarker s_MarkerUpdateTexture = new ProfilerMarker("UIR.ShaderInfoStorage.UpdateTexture");
	}
}
