using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.TerrainTools
{
	// Token: 0x02000021 RID: 33
	[MovedFrom("UnityEngine.Experimental.TerrainAPI")]
	public struct BrushTransform
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00005056 File Offset: 0x00003256
		public readonly Vector2 brushOrigin { get; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000505E File Offset: 0x0000325E
		public readonly Vector2 brushU { get; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00005066 File Offset: 0x00003266
		public readonly Vector2 brushV { get; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000506E File Offset: 0x0000326E
		public readonly Vector2 targetOrigin { get; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00005076 File Offset: 0x00003276
		public readonly Vector2 targetX { get; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000507E File Offset: 0x0000327E
		public readonly Vector2 targetY { get; }

		// Token: 0x060001C5 RID: 453 RVA: 0x00005088 File Offset: 0x00003288
		public BrushTransform(Vector2 brushOrigin, Vector2 brushU, Vector2 brushV)
		{
			float num = brushU.x * brushV.y - brushU.y * brushV.x;
			float num2 = (Mathf.Approximately(num, 0f) ? 1f : (1f / num));
			Vector2 vector = new Vector2(brushV.y, -brushU.y) * num2;
			Vector2 vector2 = new Vector2(-brushV.x, brushU.x) * num2;
			Vector2 vector3 = -brushOrigin.x * vector - brushOrigin.y * vector2;
			this.brushOrigin = brushOrigin;
			this.brushU = brushU;
			this.brushV = brushV;
			this.targetOrigin = vector3;
			this.targetX = vector;
			this.targetY = vector2;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000514C File Offset: 0x0000334C
		public Rect GetBrushXYBounds()
		{
			Vector2 vector = this.brushOrigin + this.brushU;
			Vector2 vector2 = this.brushOrigin + this.brushV;
			Vector2 vector3 = this.brushOrigin + this.brushU + this.brushV;
			float num = Mathf.Min(Mathf.Min(this.brushOrigin.x, vector.x), Mathf.Min(vector2.x, vector3.x));
			float num2 = Mathf.Max(Mathf.Max(this.brushOrigin.x, vector.x), Mathf.Max(vector2.x, vector3.x));
			float num3 = Mathf.Min(Mathf.Min(this.brushOrigin.y, vector.y), Mathf.Min(vector2.y, vector3.y));
			float num4 = Mathf.Max(Mathf.Max(this.brushOrigin.y, vector.y), Mathf.Max(vector2.y, vector3.y));
			return Rect.MinMaxRect(num, num3, num2, num4);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00005264 File Offset: 0x00003464
		public static BrushTransform FromRect(Rect brushRect)
		{
			Vector2 min = brushRect.min;
			Vector2 vector = new Vector2(brushRect.width, 0f);
			Vector2 vector2 = new Vector2(0f, brushRect.height);
			return new BrushTransform(min, vector, vector2);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000052AC File Offset: 0x000034AC
		public Vector2 ToBrushUV(Vector2 targetXY)
		{
			return targetXY.x * this.targetX + targetXY.y * this.targetY + this.targetOrigin;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000052F0 File Offset: 0x000034F0
		public Vector2 FromBrushUV(Vector2 brushUV)
		{
			return brushUV.x * this.brushU + brushUV.y * this.brushV + this.brushOrigin;
		}
	}
}
