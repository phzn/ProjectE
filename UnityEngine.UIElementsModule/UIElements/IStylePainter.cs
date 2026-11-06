using System;
using UnityEngine.TextCore.Text;

namespace UnityEngine.UIElements
{
	// Token: 0x02000326 RID: 806
	internal interface IStylePainter
	{
		// Token: 0x06001B43 RID: 6979
		MeshWriteData DrawMesh(int vertexCount, int indexCount, Texture texture, Material material, MeshGenerationContext.MeshFlags flags);

		// Token: 0x06001B44 RID: 6980
		void DrawText(TextElement te);

		// Token: 0x06001B45 RID: 6981
		void DrawText(string text, Vector2 pos, float fontSize, Color color, FontAsset font);

		// Token: 0x06001B46 RID: 6982
		void DrawRectangle(MeshGenerationContextUtils.RectangleParams rectParams);

		// Token: 0x06001B47 RID: 6983
		void DrawBorder(MeshGenerationContextUtils.BorderParams borderParams);

		// Token: 0x06001B48 RID: 6984
		void DrawImmediate(Action callback, bool cullingEnabled);

		// Token: 0x06001B49 RID: 6985
		void DrawVectorImage(VectorImage vectorImage, Vector2 pos, Angle rotationAngle, Vector2 scale);

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06001B4A RID: 6986
		VisualElement visualElement { get; }
	}
}
