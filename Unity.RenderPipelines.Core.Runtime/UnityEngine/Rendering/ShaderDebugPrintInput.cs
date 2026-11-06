using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200007C RID: 124
	public struct ShaderDebugPrintInput
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00010F69 File Offset: 0x0000F169
		// (set) Token: 0x060003DB RID: 987 RVA: 0x00010F71 File Offset: 0x0000F171
		public Vector2 pos { readonly get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003DC RID: 988 RVA: 0x00010F7A File Offset: 0x0000F17A
		// (set) Token: 0x060003DD RID: 989 RVA: 0x00010F82 File Offset: 0x0000F182
		public bool leftDown { readonly get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003DE RID: 990 RVA: 0x00010F8B File Offset: 0x0000F18B
		// (set) Token: 0x060003DF RID: 991 RVA: 0x00010F93 File Offset: 0x0000F193
		public bool rightDown { readonly get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x00010F9C File Offset: 0x0000F19C
		// (set) Token: 0x060003E1 RID: 993 RVA: 0x00010FA4 File Offset: 0x0000F1A4
		public bool middleDown { readonly get; set; }

		// Token: 0x060003E2 RID: 994 RVA: 0x00010FB0 File Offset: 0x0000F1B0
		public string String()
		{
			return string.Format("Mouse: {0}x{1}  Btns: Left:{2} Right:{3} Middle:{4} ", new object[]
			{
				this.pos.x,
				this.pos.y,
				this.leftDown,
				this.rightDown,
				this.middleDown
			});
		}
	}
}
