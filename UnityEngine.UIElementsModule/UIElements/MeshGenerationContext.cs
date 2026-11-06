using System;
using Unity.Profiling;
using UnityEngine.TextCore.Text;

namespace UnityEngine.UIElements
{
	// Token: 0x020002B0 RID: 688
	public class MeshGenerationContext
	{
		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x00046574 File Offset: 0x00044774
		public VisualElement visualElement
		{
			get
			{
				return this.painter.visualElement;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x00046594 File Offset: 0x00044794
		public Painter2D painter2D
		{
			get
			{
				bool flag = this.m_Painter2D == null;
				if (flag)
				{
					this.m_Painter2D = new Painter2D(this);
				}
				return this.m_Painter2D;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x000465C5 File Offset: 0x000447C5
		internal bool hasPainter2D
		{
			get
			{
				return this.m_Painter2D != null;
			}
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x000465D0 File Offset: 0x000447D0
		internal MeshGenerationContext(IStylePainter painter)
		{
			this.painter = painter;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x000465E4 File Offset: 0x000447E4
		public MeshWriteData Allocate(int vertexCount, int indexCount, Texture texture = null)
		{
			MeshWriteData meshWriteData;
			using (MeshGenerationContext.s_AllocateMarker.Auto())
			{
				meshWriteData = this.painter.DrawMesh(vertexCount, indexCount, texture, null, MeshGenerationContext.MeshFlags.None);
			}
			return meshWriteData;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00046634 File Offset: 0x00044834
		internal MeshWriteData Allocate(int vertexCount, int indexCount, Texture texture, Material material, MeshGenerationContext.MeshFlags flags)
		{
			MeshWriteData meshWriteData;
			using (MeshGenerationContext.s_AllocateMarker.Auto())
			{
				meshWriteData = this.painter.DrawMesh(vertexCount, indexCount, texture, material, flags);
			}
			return meshWriteData;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00046688 File Offset: 0x00044888
		public void DrawVectorImage(VectorImage vectorImage, Vector2 offset, Angle rotationAngle, Vector2 scale)
		{
			using (MeshGenerationContext.s_DrawVectorImageMarker.Auto())
			{
				this.painter.DrawVectorImage(vectorImage, offset, rotationAngle, scale);
			}
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x000466D8 File Offset: 0x000448D8
		public void DrawText(string text, Vector2 pos, float fontSize, Color color, FontAsset font = null)
		{
			bool flag = font == null;
			if (flag)
			{
				font = TextUtilities.GetFontAsset(this.visualElement);
			}
			this.painter.DrawText(text, pos, fontSize, color, font);
		}

		// Token: 0x0400093B RID: 2363
		private Painter2D m_Painter2D;

		// Token: 0x0400093C RID: 2364
		private static readonly ProfilerMarker s_AllocateMarker = new ProfilerMarker("UIR.MeshGenerationContext.Allocate");

		// Token: 0x0400093D RID: 2365
		private static readonly ProfilerMarker s_DrawVectorImageMarker = new ProfilerMarker("UIR.MeshGenerationContext.DrawVectorImage");

		// Token: 0x0400093E RID: 2366
		internal IStylePainter painter;

		// Token: 0x020002B1 RID: 689
		[Flags]
		internal enum MeshFlags
		{
			// Token: 0x04000940 RID: 2368
			None = 0,
			// Token: 0x04000941 RID: 2369
			UVisDisplacement = 1,
			// Token: 0x04000942 RID: 2370
			SkipDynamicAtlas = 2
		}
	}
}
