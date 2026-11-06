using System;
using Unity.Mathematics;

namespace Pathfinding.Drawing.Text
{
	// Token: 0x0200005B RID: 91
	internal struct SDFCharacter
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x0000F76F File Offset: 0x0000D96F
		public float2 uvTopLeft
		{
			get
			{
				return this.uvtopleft;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000F777 File Offset: 0x0000D977
		public float2 uvTopRight
		{
			get
			{
				return new float2(this.uvbottomright.x, this.uvtopleft.y);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000F794 File Offset: 0x0000D994
		public float2 uvBottomLeft
		{
			get
			{
				return new float2(this.uvtopleft.x, this.uvbottomright.y);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000F7B1 File Offset: 0x0000D9B1
		public float2 uvBottomRight
		{
			get
			{
				return this.uvbottomright;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000F7B9 File Offset: 0x0000D9B9
		public float2 vertexTopLeft
		{
			get
			{
				return this.vtopleft;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000F7C1 File Offset: 0x0000D9C1
		public float2 vertexTopRight
		{
			get
			{
				return new float2(this.vbottomright.x, this.vtopleft.y);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000F7DE File Offset: 0x0000D9DE
		public float2 vertexBottomLeft
		{
			get
			{
				return new float2(this.vtopleft.x, this.vbottomright.y);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000F7FB File Offset: 0x0000D9FB
		public float2 vertexBottomRight
		{
			get
			{
				return this.vbottomright;
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000F804 File Offset: 0x0000DA04
		public SDFCharacter(char codePoint, int x, int y, int width, int height, int originX, int originY, int advance, int textureWidth, int textureHeight, float defaultSize)
		{
			float2 @float = new float2((float)textureWidth, (float)textureHeight);
			this.codePoint = codePoint;
			float2 float2 = new float2((float)x, (float)y) / @float;
			float2 float3 = new float2((float)(x + width), (float)(y + height)) / @float;
			this.uvtopleft = new float2(float2.x, 1f - float2.y);
			this.uvbottomright = new float2(float3.x, 1f - float3.y);
			float2 float4 = new float2((float)(-(float)originX), (float)originY);
			this.vtopleft = (float4 + new float2(0f, 0f)) / defaultSize;
			this.vbottomright = (float4 + new float2((float)width, (float)(-(float)height))) / defaultSize;
			this.advance = (float)advance / defaultSize;
		}

		// Token: 0x04000178 RID: 376
		public char codePoint;

		// Token: 0x04000179 RID: 377
		private float2 uvtopleft;

		// Token: 0x0400017A RID: 378
		private float2 uvbottomright;

		// Token: 0x0400017B RID: 379
		private float2 vtopleft;

		// Token: 0x0400017C RID: 380
		private float2 vbottomright;

		// Token: 0x0400017D RID: 381
		public float advance;
	}
}
