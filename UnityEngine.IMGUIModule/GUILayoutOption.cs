using System;

namespace UnityEngine
{
	// Token: 0x02000020 RID: 32
	public sealed class GUILayoutOption
	{
		// Token: 0x06000255 RID: 597 RVA: 0x00009923 File Offset: 0x00007B23
		internal GUILayoutOption(GUILayoutOption.Type type, object value)
		{
			this.type = type;
			this.value = value;
		}

		// Token: 0x04000083 RID: 131
		internal GUILayoutOption.Type type;

		// Token: 0x04000084 RID: 132
		internal object value;

		// Token: 0x02000021 RID: 33
		internal enum Type
		{
			// Token: 0x04000086 RID: 134
			fixedWidth,
			// Token: 0x04000087 RID: 135
			fixedHeight,
			// Token: 0x04000088 RID: 136
			minWidth,
			// Token: 0x04000089 RID: 137
			maxWidth,
			// Token: 0x0400008A RID: 138
			minHeight,
			// Token: 0x0400008B RID: 139
			maxHeight,
			// Token: 0x0400008C RID: 140
			stretchWidth,
			// Token: 0x0400008D RID: 141
			stretchHeight,
			// Token: 0x0400008E RID: 142
			alignStart,
			// Token: 0x0400008F RID: 143
			alignMiddle,
			// Token: 0x04000090 RID: 144
			alignEnd,
			// Token: 0x04000091 RID: 145
			alignJustify,
			// Token: 0x04000092 RID: 146
			equalSize,
			// Token: 0x04000093 RID: 147
			spacing
		}
	}
}
