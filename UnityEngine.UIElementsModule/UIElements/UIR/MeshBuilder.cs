using System;
using Unity.Collections;
using Unity.Profiling;
using UnityEngine.TextCore.Text;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200044E RID: 1102
	internal static class MeshBuilder
	{
		// Token: 0x06002287 RID: 8839 RVA: 0x00084EB4 File Offset: 0x000830B4
		private static Vertex ConvertTextVertexToUIRVertex(MeshInfo info, int index, Vector2 offset, VertexFlags flags = VertexFlags.IsText, bool isDynamicColor = false)
		{
			float num = 0f;
			bool flag = info.uvs2[index].y < 0f;
			if (flag)
			{
				num = 1f;
			}
			return new Vertex
			{
				position = new Vector3(info.vertices[index].x + offset.x, info.vertices[index].y + offset.y, 0f),
				uv = new Vector2(info.uvs0[index].x, info.uvs0[index].y),
				tint = info.colors32[index],
				flags = new Color32((byte)flags, (byte)(num * 255f), 0, isDynamicColor ? 1 : 0)
			};
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x00084F98 File Offset: 0x00083198
		private static Vertex ConvertTextVertexToUIRVertex(TextVertex textVertex, Vector2 offset)
		{
			return new Vertex
			{
				position = new Vector3(textVertex.position.x + offset.x, textVertex.position.y + offset.y, 0f),
				uv = textVertex.uv0,
				tint = textVertex.color,
				flags = new Color32(1, 0, 0, 0)
			};
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x00085014 File Offset: 0x00083214
		private static int LimitTextVertices(int vertexCount, bool logTruncation = true)
		{
			bool flag = vertexCount <= MeshBuilder.s_MaxTextMeshVertices;
			int num;
			if (flag)
			{
				num = vertexCount;
			}
			else
			{
				if (logTruncation)
				{
					Debug.LogWarning(string.Format("Generated text will be truncated because it exceeds {0} vertices.", MeshBuilder.s_MaxTextMeshVertices));
				}
				num = MeshBuilder.s_MaxTextMeshVertices;
			}
			return num;
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x00085060 File Offset: 0x00083260
		internal static void MakeText(MeshInfo meshInfo, Vector2 offset, MeshBuilder.AllocMeshData meshAlloc, VertexFlags flags = VertexFlags.IsText, bool isDynamicColor = false)
		{
			int num = MeshBuilder.LimitTextVertices(meshInfo.vertexCount, true);
			int num2 = num / 4;
			MeshWriteData meshWriteData = meshAlloc.Allocate((uint)(num2 * 4), (uint)(num2 * 6));
			int i = 0;
			int num3 = 0;
			while (i < num2)
			{
				meshWriteData.SetNextVertex(MeshBuilder.ConvertTextVertexToUIRVertex(meshInfo, num3, offset, flags, isDynamicColor));
				meshWriteData.SetNextVertex(MeshBuilder.ConvertTextVertexToUIRVertex(meshInfo, num3 + 1, offset, flags, isDynamicColor));
				meshWriteData.SetNextVertex(MeshBuilder.ConvertTextVertexToUIRVertex(meshInfo, num3 + 2, offset, flags, isDynamicColor));
				meshWriteData.SetNextVertex(MeshBuilder.ConvertTextVertexToUIRVertex(meshInfo, num3 + 3, offset, flags, isDynamicColor));
				meshWriteData.SetNextIndex((ushort)num3);
				meshWriteData.SetNextIndex((ushort)(num3 + 1));
				meshWriteData.SetNextIndex((ushort)(num3 + 2));
				meshWriteData.SetNextIndex((ushort)(num3 + 2));
				meshWriteData.SetNextIndex((ushort)(num3 + 3));
				meshWriteData.SetNextIndex((ushort)num3);
				i++;
				num3 += 4;
			}
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x00085148 File Offset: 0x00083348
		internal static void MakeText(NativeArray<TextVertex> uiVertices, Vector2 offset, MeshBuilder.AllocMeshData meshAlloc)
		{
			int num = MeshBuilder.LimitTextVertices(uiVertices.Length, true);
			int num2 = num / 4;
			MeshWriteData meshWriteData = meshAlloc.Allocate((uint)(num2 * 4), (uint)(num2 * 6));
			int i = 0;
			int num3 = 0;
			while (i < num2)
			{
				meshWriteData.SetNextVertex(MeshBuilder.ConvertTextVertexToUIRVertex(uiVertices[num3], offset));
				meshWriteData.SetNextVertex(MeshBuilder.ConvertTextVertexToUIRVertex(uiVertices[num3 + 1], offset));
				meshWriteData.SetNextVertex(MeshBuilder.ConvertTextVertexToUIRVertex(uiVertices[num3 + 2], offset));
				meshWriteData.SetNextVertex(MeshBuilder.ConvertTextVertexToUIRVertex(uiVertices[num3 + 3], offset));
				meshWriteData.SetNextIndex((ushort)num3);
				meshWriteData.SetNextIndex((ushort)(num3 + 1));
				meshWriteData.SetNextIndex((ushort)(num3 + 2));
				meshWriteData.SetNextIndex((ushort)(num3 + 2));
				meshWriteData.SetNextIndex((ushort)(num3 + 3));
				meshWriteData.SetNextIndex((ushort)num3);
				i++;
				num3 += 4;
			}
		}

		// Token: 0x04000F6C RID: 3948
		private static ProfilerMarker s_VectorGraphics9Slice = new ProfilerMarker("UIR.MakeVector9Slice");

		// Token: 0x04000F6D RID: 3949
		private static ProfilerMarker s_VectorGraphicsSplitTriangle = new ProfilerMarker("UIR.SplitTriangle");

		// Token: 0x04000F6E RID: 3950
		private static ProfilerMarker s_VectorGraphicsScaleTriangle = new ProfilerMarker("UIR.ScaleTriangle");

		// Token: 0x04000F6F RID: 3951
		private static ProfilerMarker s_VectorGraphicsStretch = new ProfilerMarker("UIR.MakeVectorStretch");

		// Token: 0x04000F70 RID: 3952
		internal static readonly int s_MaxTextMeshVertices = 49152;

		// Token: 0x0200044F RID: 1103
		internal struct AllocMeshData
		{
			// Token: 0x0600228D RID: 8845 RVA: 0x00085290 File Offset: 0x00083490
			internal MeshWriteData Allocate(uint vertexCount, uint indexCount)
			{
				return this.alloc(vertexCount, indexCount, ref this);
			}

			// Token: 0x04000F71 RID: 3953
			internal MeshBuilder.AllocMeshData.Allocator alloc;

			// Token: 0x04000F72 RID: 3954
			internal Texture texture;

			// Token: 0x04000F73 RID: 3955
			internal TextureId svgTexture;

			// Token: 0x04000F74 RID: 3956
			internal Material material;

			// Token: 0x04000F75 RID: 3957
			internal MeshGenerationContext.MeshFlags flags;

			// Token: 0x04000F76 RID: 3958
			internal BMPAlloc colorAlloc;

			// Token: 0x02000450 RID: 1104
			// (Invoke) Token: 0x0600228F RID: 8847
			internal delegate MeshWriteData Allocator(uint vertexCount, uint indexCount, ref MeshBuilder.AllocMeshData allocatorData);
		}
	}
}
