using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000441 RID: 1089
	internal class DrawParams
	{
		// Token: 0x0600224C RID: 8780 RVA: 0x00083A90 File Offset: 0x00081C90
		public void Reset()
		{
			this.view.Clear();
			this.view.Push(Matrix4x4.identity);
			this.scissor.Clear();
			this.scissor.Push(DrawParams.k_UnlimitedRect);
			this.renderTexture.Clear();
			this.defaultMaterial.Clear();
		}

		// Token: 0x04000F1C RID: 3868
		internal static readonly Rect k_UnlimitedRect = new Rect(-100000f, -100000f, 200000f, 200000f);

		// Token: 0x04000F1D RID: 3869
		internal static readonly Rect k_FullNormalizedRect = new Rect(-1f, -1f, 2f, 2f);

		// Token: 0x04000F1E RID: 3870
		internal readonly Stack<Matrix4x4> view = new Stack<Matrix4x4>(8);

		// Token: 0x04000F1F RID: 3871
		internal readonly Stack<Rect> scissor = new Stack<Rect>(8);

		// Token: 0x04000F20 RID: 3872
		internal readonly List<RenderTexture> renderTexture = new List<RenderTexture>(8);

		// Token: 0x04000F21 RID: 3873
		internal readonly List<Material> defaultMaterial = new List<Material>(8);
	}
}
