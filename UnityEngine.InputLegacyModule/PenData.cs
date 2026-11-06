using System;

namespace UnityEngine
{
	// Token: 0x02000008 RID: 8
	public struct PenData
	{
		// Token: 0x04000027 RID: 39
		public Vector2 position;

		// Token: 0x04000028 RID: 40
		public Vector2 tilt;

		// Token: 0x04000029 RID: 41
		public PenStatus penStatus;

		// Token: 0x0400002A RID: 42
		public float twist;

		// Token: 0x0400002B RID: 43
		public float pressure;

		// Token: 0x0400002C RID: 44
		public PenEventType contactType;

		// Token: 0x0400002D RID: 45
		public Vector2 deltaPos;
	}
}
