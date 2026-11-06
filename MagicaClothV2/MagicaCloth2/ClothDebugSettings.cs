using System;

namespace MagicaCloth2
{
	// Token: 0x020000EA RID: 234
	[Serializable]
	public class ClothDebugSettings
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x0000706A File Offset: 0x0000526A
		public bool CheckParticleDrawing(int index)
		{
			return true;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000706A File Offset: 0x0000526A
		public bool CheckTriangleDrawing(int index)
		{
			return true;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000706A File Offset: 0x0000526A
		public bool CheckRadiusDrawing()
		{
			return true;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00029F37 File Offset: 0x00028137
		public float GetPointSize()
		{
			return 0.01f;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00029F3E File Offset: 0x0002813E
		public float GetLineSize()
		{
			return 0.05f;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00029F37 File Offset: 0x00028137
		public float GetInertiaCenterRadius()
		{
			return 0.01f;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00029F45 File Offset: 0x00028145
		public float GetCustomSkinningRadius()
		{
			return 0.02f;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00002137 File Offset: 0x00000337
		public bool IsReferOldPos()
		{
			return false;
		}

		// Token: 0x0400084F RID: 2127
		public bool enable;

		// Token: 0x04000850 RID: 2128
		public bool ztest;

		// Token: 0x04000851 RID: 2129
		public bool position = true;

		// Token: 0x04000852 RID: 2130
		public ClothDebugSettings.DebugAxis axis;

		// Token: 0x04000853 RID: 2131
		public bool shape;

		// Token: 0x04000854 RID: 2132
		public bool baseLine;

		// Token: 0x04000855 RID: 2133
		public bool depth;

		// Token: 0x04000856 RID: 2134
		public bool collider = true;

		// Token: 0x04000857 RID: 2135
		public bool animatedPosition;

		// Token: 0x04000858 RID: 2136
		public ClothDebugSettings.DebugAxis animatedAxis;

		// Token: 0x04000859 RID: 2137
		public bool animatedShape;

		// Token: 0x0400085A RID: 2138
		public bool inertiaCenter = true;

		// Token: 0x0400085B RID: 2139
		public bool customSkinningBone = true;

		// Token: 0x020000EB RID: 235
		public enum DebugAxis
		{
			// Token: 0x0400085D RID: 2141
			None,
			// Token: 0x0400085E RID: 2142
			Normal,
			// Token: 0x0400085F RID: 2143
			All
		}
	}
}
