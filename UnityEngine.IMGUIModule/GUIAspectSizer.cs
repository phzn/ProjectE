using System;

namespace UnityEngine
{
	// Token: 0x02000033 RID: 51
	internal sealed class GUIAspectSizer : GUILayoutEntry
	{
		// Token: 0x060003F9 RID: 1017 RVA: 0x0000D410 File Offset: 0x0000B610
		public GUIAspectSizer(float aspect, GUILayoutOption[] options)
			: base(0f, 0f, 0f, 0f, GUIStyle.none)
		{
			this.aspect = aspect;
			this.ApplyOptions(options);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000D444 File Offset: 0x0000B644
		public override void CalcHeight()
		{
			this.minHeight = (this.maxHeight = this.rect.width / this.aspect);
		}

		// Token: 0x040000FD RID: 253
		private float aspect;
	}
}
