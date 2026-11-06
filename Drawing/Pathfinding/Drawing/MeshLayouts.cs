using System;
using UnityEngine.Rendering;

namespace Pathfinding.Drawing
{
	// Token: 0x0200004F RID: 79
	internal static class MeshLayouts
	{
		// Token: 0x04000139 RID: 313
		internal static readonly VertexAttributeDescriptor[] MeshLayout = new VertexAttributeDescriptor[]
		{
			new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0),
			new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, 0),
			new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4, 0),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, 0)
		};

		// Token: 0x0400013A RID: 314
		internal static readonly VertexAttributeDescriptor[] MeshLayoutText = new VertexAttributeDescriptor[]
		{
			new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0),
			new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4, 0),
			new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, 0)
		};
	}
}
