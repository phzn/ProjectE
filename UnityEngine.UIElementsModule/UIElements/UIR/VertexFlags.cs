using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200043E RID: 1086
	internal enum VertexFlags
	{
		// Token: 0x04000F04 RID: 3844
		IsSolid,
		// Token: 0x04000F05 RID: 3845
		IsText,
		// Token: 0x04000F06 RID: 3846
		IsTextured,
		// Token: 0x04000F07 RID: 3847
		IsDynamic,
		// Token: 0x04000F08 RID: 3848
		IsSvgGradients,
		// Token: 0x04000F09 RID: 3849
		[Obsolete("Enum member VertexFlags.LastType has been deprecated. Use VertexFlags.IsGraphViewEdge instead.")]
		LastType = 10,
		// Token: 0x04000F0A RID: 3850
		IsGraphViewEdge = 10
	}
}
