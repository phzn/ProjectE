using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000126 RID: 294
	[Serializable]
	public class MMPropertyReceiver : MMPropertyPicker
	{
		// Token: 0x0600069C RID: 1692 RVA: 0x000216F1 File Offset: 0x0001F8F1
		public virtual void SetLevel(float newLevel)
		{
			if (!this.PropertyFound)
			{
				return;
			}
			if (!this.ShouldModifyValue)
			{
				return;
			}
			this._propertySetter.SetLevel(this, this._targetMMProperty, newLevel);
		}

		// Token: 0x04000835 RID: 2101
		public bool ShouldModifyValue = true;

		// Token: 0x04000836 RID: 2102
		public bool RelativeValue = true;

		// Token: 0x04000837 RID: 2103
		public bool ModifyX = true;

		// Token: 0x04000838 RID: 2104
		public bool ModifyY = true;

		// Token: 0x04000839 RID: 2105
		public bool ModifyZ = true;

		// Token: 0x0400083A RID: 2106
		public bool ModifyW = true;

		// Token: 0x0400083B RID: 2107
		public float Threshold = 0.5f;

		// Token: 0x0400083C RID: 2108
		public bool BoolRemapZero;

		// Token: 0x0400083D RID: 2109
		public bool BoolRemapOne = true;

		// Token: 0x0400083E RID: 2110
		public string StringRemapZero = "Zero";

		// Token: 0x0400083F RID: 2111
		public string StringRemapOne = "One";

		// Token: 0x04000840 RID: 2112
		public int IntRemapZero;

		// Token: 0x04000841 RID: 2113
		public int IntRemapOne = 1;

		// Token: 0x04000842 RID: 2114
		public float FloatRemapZero;

		// Token: 0x04000843 RID: 2115
		public float FloatRemapOne = 1f;

		// Token: 0x04000844 RID: 2116
		public Vector2 Vector2RemapZero = Vector2.zero;

		// Token: 0x04000845 RID: 2117
		public Vector2 Vector2RemapOne = Vector2.one;

		// Token: 0x04000846 RID: 2118
		public Vector3 Vector3RemapZero = Vector3.zero;

		// Token: 0x04000847 RID: 2119
		public Vector3 Vector3RemapOne = Vector3.one;

		// Token: 0x04000848 RID: 2120
		public Vector4 Vector4RemapZero = Vector4.zero;

		// Token: 0x04000849 RID: 2121
		public Vector4 Vector4RemapOne = Vector4.one;

		// Token: 0x0400084A RID: 2122
		public Vector3 QuaternionRemapZero = Vector3.zero;

		// Token: 0x0400084B RID: 2123
		public Vector3 QuaternionRemapOne = new Vector3(180f, 180f, 180f);

		// Token: 0x0400084C RID: 2124
		[ColorUsage(true, true)]
		public Color ColorRemapZero = Color.white;

		// Token: 0x0400084D RID: 2125
		[ColorUsage(true, true)]
		public Color ColorRemapOne = Color.black;

		// Token: 0x0400084E RID: 2126
		public float Level;
	}
}
