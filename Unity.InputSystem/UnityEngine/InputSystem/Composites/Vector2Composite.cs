using System;
using System.ComponentModel;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Composites
{
	// Token: 0x02000154 RID: 340
	[DisplayStringFormat("{up}/{left}/{down}/{right}")]
	[DisplayName("Up/Down/Left/Right Composite")]
	public class Vector2Composite : InputBindingComposite<Vector2>
	{
		// Token: 0x06001269 RID: 4713 RVA: 0x00057288 File Offset: 0x00055488
		public override Vector2 ReadValue(ref InputBindingCompositeContext context)
		{
			Vector2Composite.Mode mode = this.mode;
			if (mode == Vector2Composite.Mode.Analog)
			{
				float num = context.ReadValue<float>(this.up);
				float num2 = context.ReadValue<float>(this.down);
				float num3 = context.ReadValue<float>(this.left);
				float num4 = context.ReadValue<float>(this.right);
				return DpadControl.MakeDpadVector(num, num2, num3, num4);
			}
			bool flag = context.ReadValueAsButton(this.up);
			bool flag2 = context.ReadValueAsButton(this.down);
			bool flag3 = context.ReadValueAsButton(this.left);
			bool flag4 = context.ReadValueAsButton(this.right);
			if (!this.normalize)
			{
				mode = Vector2Composite.Mode.Digital;
			}
			return DpadControl.MakeDpadVector(flag, flag2, flag3, flag4, mode == Vector2Composite.Mode.DigitalNormalized);
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0005732C File Offset: 0x0005552C
		public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
		{
			return this.ReadValue(ref context).magnitude;
		}

		// Token: 0x0400075D RID: 1885
		[InputControl(layout = "Axis")]
		public int up;

		// Token: 0x0400075E RID: 1886
		[InputControl(layout = "Axis")]
		public int down;

		// Token: 0x0400075F RID: 1887
		[InputControl(layout = "Axis")]
		public int left;

		// Token: 0x04000760 RID: 1888
		[InputControl(layout = "Axis")]
		public int right;

		// Token: 0x04000761 RID: 1889
		[Obsolete("Use Mode.DigitalNormalized with 'mode' instead")]
		public bool normalize = true;

		// Token: 0x04000762 RID: 1890
		public Vector2Composite.Mode mode;

		// Token: 0x0200025C RID: 604
		public enum Mode
		{
			// Token: 0x04000CC9 RID: 3273
			Analog = 2,
			// Token: 0x04000CCA RID: 3274
			DigitalNormalized = 0,
			// Token: 0x04000CCB RID: 3275
			Digital
		}
	}
}
