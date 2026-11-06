using System;

namespace MoreMountains.Tools
{
	// Token: 0x02000124 RID: 292
	[Serializable]
	public class MMPropertyEmitter : MMPropertyPicker
	{
		// Token: 0x06000695 RID: 1685 RVA: 0x000213A6 File Offset: 0x0001F5A6
		public virtual float GetLevel()
		{
			return this._propertySetter.GetLevel(this, this._targetMMProperty);
		}

		// Token: 0x0400081F RID: 2079
		public bool ClampMin = true;

		// Token: 0x04000820 RID: 2080
		public bool ClampMax = true;

		// Token: 0x04000821 RID: 2081
		public MMPropertyEmitter.Vector2Options Vector2Option;

		// Token: 0x04000822 RID: 2082
		public MMPropertyEmitter.Vector3Options Vector3Option;

		// Token: 0x04000823 RID: 2083
		public MMPropertyEmitter.Vector4Options Vector4Option;

		// Token: 0x04000824 RID: 2084
		public float BoolRemapFalse;

		// Token: 0x04000825 RID: 2085
		public float BoolRemapTrue = 1f;

		// Token: 0x04000826 RID: 2086
		public int IntRemapMinToZero;

		// Token: 0x04000827 RID: 2087
		public int IntRemapMaxToOne = 1;

		// Token: 0x04000828 RID: 2088
		public float FloatRemapMinToZero;

		// Token: 0x04000829 RID: 2089
		public float FloatRemapMaxToOne = 1f;

		// Token: 0x0400082A RID: 2090
		public float QuaternionRemapMinToZero;

		// Token: 0x0400082B RID: 2091
		public float QuaternionRemapMaxToOne = 360f;

		// Token: 0x0400082C RID: 2092
		public float Level;

		// Token: 0x0200043D RID: 1085
		public enum Vector2Options
		{
			// Token: 0x04001FC3 RID: 8131
			X,
			// Token: 0x04001FC4 RID: 8132
			Y
		}

		// Token: 0x0200043E RID: 1086
		public enum Vector3Options
		{
			// Token: 0x04001FC6 RID: 8134
			X,
			// Token: 0x04001FC7 RID: 8135
			Y,
			// Token: 0x04001FC8 RID: 8136
			Z
		}

		// Token: 0x0200043F RID: 1087
		public enum Vector4Options
		{
			// Token: 0x04001FCA RID: 8138
			X,
			// Token: 0x04001FCB RID: 8139
			Y,
			// Token: 0x04001FCC RID: 8140
			Z,
			// Token: 0x04001FCD RID: 8141
			W
		}
	}
}
