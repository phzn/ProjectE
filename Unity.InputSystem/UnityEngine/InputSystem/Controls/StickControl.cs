using System;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x0200011E RID: 286
	public class StickControl : Vector2Control
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x0004F1DF File Offset: 0x0004D3DF
		// (set) Token: 0x0600104E RID: 4174 RVA: 0x0004F1E7 File Offset: 0x0004D3E7
		[InputControl(useStateFrom = "y", processors = "axisDeadzone", parameters = "clamp=2,clampMin=0,clampMax=1", synthetic = true, displayName = "Up")]
		[InputControl(name = "x", minValue = -1f, maxValue = 1f, layout = "Axis", processors = "axisDeadzone")]
		[InputControl(name = "y", minValue = -1f, maxValue = 1f, layout = "Axis", processors = "axisDeadzone")]
		public ButtonControl up { get; set; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x0600104F RID: 4175 RVA: 0x0004F1F0 File Offset: 0x0004D3F0
		// (set) Token: 0x06001050 RID: 4176 RVA: 0x0004F1F8 File Offset: 0x0004D3F8
		[InputControl(useStateFrom = "y", processors = "axisDeadzone", parameters = "clamp=2,clampMin=-1,clampMax=0,invert", synthetic = true, displayName = "Down")]
		public ButtonControl down { get; set; }

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x0004F201 File Offset: 0x0004D401
		// (set) Token: 0x06001052 RID: 4178 RVA: 0x0004F209 File Offset: 0x0004D409
		[InputControl(useStateFrom = "x", processors = "axisDeadzone", parameters = "clamp=2,clampMin=-1,clampMax=0,invert", synthetic = true, displayName = "Left")]
		public ButtonControl left { get; set; }

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x0004F212 File Offset: 0x0004D412
		// (set) Token: 0x06001054 RID: 4180 RVA: 0x0004F21A File Offset: 0x0004D41A
		[InputControl(useStateFrom = "x", processors = "axisDeadzone", parameters = "clamp=2,clampMin=0,clampMax=1", synthetic = true, displayName = "Right")]
		public ButtonControl right { get; set; }

		// Token: 0x06001055 RID: 4181 RVA: 0x0004F224 File Offset: 0x0004D424
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.up = base.GetChildControl<ButtonControl>("up");
			this.down = base.GetChildControl<ButtonControl>("down");
			this.left = base.GetChildControl<ButtonControl>("left");
			this.right = base.GetChildControl<ButtonControl>("right");
		}
	}
}
