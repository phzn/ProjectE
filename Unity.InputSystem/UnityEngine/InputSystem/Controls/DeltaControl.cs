using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.Scripting;

namespace UnityEngine.InputSystem.Controls
{
	// Token: 0x02000117 RID: 279
	[Preserve]
	public class DeltaControl : Vector2Control
	{
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001018 RID: 4120 RVA: 0x0004E852 File Offset: 0x0004CA52
		// (set) Token: 0x06001019 RID: 4121 RVA: 0x0004E85A File Offset: 0x0004CA5A
		[InputControl(useStateFrom = "y", parameters = "clamp=1,clampMin=0,clampMax=3.402823E+38", synthetic = true, displayName = "Up")]
		[Preserve]
		public AxisControl up { get; set; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600101A RID: 4122 RVA: 0x0004E863 File Offset: 0x0004CA63
		// (set) Token: 0x0600101B RID: 4123 RVA: 0x0004E86B File Offset: 0x0004CA6B
		[InputControl(useStateFrom = "y", parameters = "clamp=1,clampMin=-3.402823E+38,clampMax=0,invert", synthetic = true, displayName = "Down")]
		[Preserve]
		public AxisControl down { get; set; }

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x0600101C RID: 4124 RVA: 0x0004E874 File Offset: 0x0004CA74
		// (set) Token: 0x0600101D RID: 4125 RVA: 0x0004E87C File Offset: 0x0004CA7C
		[InputControl(useStateFrom = "x", parameters = "clamp=1,clampMin=-3.402823E+38,clampMax=0,invert", synthetic = true, displayName = "Left")]
		[Preserve]
		public AxisControl left { get; set; }

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x0004E885 File Offset: 0x0004CA85
		// (set) Token: 0x0600101F RID: 4127 RVA: 0x0004E88D File Offset: 0x0004CA8D
		[InputControl(useStateFrom = "x", parameters = "clamp=1,clampMin=0,clampMax=3.402823E+38", synthetic = true, displayName = "Right")]
		[Preserve]
		public AxisControl right { get; set; }

		// Token: 0x06001020 RID: 4128 RVA: 0x0004E898 File Offset: 0x0004CA98
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.up = base.GetChildControl<AxisControl>("up");
			this.down = base.GetChildControl<AxisControl>("down");
			this.left = base.GetChildControl<AxisControl>("left");
			this.right = base.GetChildControl<AxisControl>("right");
		}
	}
}
