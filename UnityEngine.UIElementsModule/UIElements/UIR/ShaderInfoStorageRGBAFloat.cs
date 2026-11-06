using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000463 RID: 1123
	internal class ShaderInfoStorageRGBAFloat : ShaderInfoStorage<Color>
	{
		// Token: 0x0600230E RID: 8974 RVA: 0x00088010 File Offset: 0x00086210
		public ShaderInfoStorageRGBAFloat(int initialSize = 64, int maxSize = 4096)
			: base(TextureFormat.RGBAFloat, ShaderInfoStorageRGBAFloat.s_Convert, initialSize, maxSize)
		{
		}

		// Token: 0x0400102A RID: 4138
		private static readonly Func<Color, Color> s_Convert = (Color c) => c;
	}
}
