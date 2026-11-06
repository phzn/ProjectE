using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000461 RID: 1121
	internal class ShaderInfoStorageRGBA32 : ShaderInfoStorage<Color32>
	{
		// Token: 0x06002309 RID: 8969 RVA: 0x00087FD3 File Offset: 0x000861D3
		public ShaderInfoStorageRGBA32(int initialSize = 64, int maxSize = 4096)
			: base(TextureFormat.RGBA32, ShaderInfoStorageRGBA32.s_Convert, initialSize, maxSize)
		{
		}

		// Token: 0x04001028 RID: 4136
		private static readonly Func<Color, Color32> s_Convert = (Color c) => c;
	}
}
