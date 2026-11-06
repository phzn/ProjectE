using System;

namespace UnityEngine.Timeline
{
	// Token: 0x0200000C RID: 12
	internal static class MatchTargetFieldConstants
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000028DE File Offset: 0x00000ADE
		public static bool HasAny(this MatchTargetFields me, MatchTargetFields fields)
		{
			return (me & fields) != MatchTargetFieldConstants.None;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000028ED File Offset: 0x00000AED
		public static MatchTargetFields Toggle(this MatchTargetFields me, MatchTargetFields flag)
		{
			return me ^ flag;
		}

		// Token: 0x0400002B RID: 43
		public static MatchTargetFields All = MatchTargetFields.PositionX | MatchTargetFields.PositionY | MatchTargetFields.PositionZ | MatchTargetFields.RotationX | MatchTargetFields.RotationY | MatchTargetFields.RotationZ;

		// Token: 0x0400002C RID: 44
		public static MatchTargetFields None = (MatchTargetFields)0;

		// Token: 0x0400002D RID: 45
		public static MatchTargetFields Position = MatchTargetFields.PositionX | MatchTargetFields.PositionY | MatchTargetFields.PositionZ;

		// Token: 0x0400002E RID: 46
		public static MatchTargetFields Rotation = MatchTargetFields.RotationX | MatchTargetFields.RotationY | MatchTargetFields.RotationZ;
	}
}
