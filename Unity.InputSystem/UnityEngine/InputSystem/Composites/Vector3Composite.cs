using System;
using System.ComponentModel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Composites
{
	// Token: 0x02000155 RID: 341
	[DisplayStringFormat("{up}+{down}/{left}+{right}/{forward}+{backward}")]
	[DisplayName("Up/Down/Left/Right/Forward/Backward Composite")]
	public class Vector3Composite : InputBindingComposite<Vector3>
	{
		// Token: 0x0600126C RID: 4716 RVA: 0x00057358 File Offset: 0x00055558
		public override Vector3 ReadValue(ref InputBindingCompositeContext context)
		{
			if (this.mode == Vector3Composite.Mode.Analog)
			{
				float num = context.ReadValue<float>(this.up);
				float num2 = context.ReadValue<float>(this.down);
				float num3 = context.ReadValue<float>(this.left);
				float num4 = context.ReadValue<float>(this.right);
				float num5 = context.ReadValue<float>(this.forward);
				float num6 = context.ReadValue<float>(this.backward);
				return new Vector3(num4 - num3, num - num2, num5 - num6);
			}
			float num7 = (context.ReadValueAsButton(this.up) ? 1f : 0f);
			float num8 = (context.ReadValueAsButton(this.down) ? (-1f) : 0f);
			float num9 = (context.ReadValueAsButton(this.left) ? (-1f) : 0f);
			float num10 = (context.ReadValueAsButton(this.right) ? 1f : 0f);
			float num11 = (context.ReadValueAsButton(this.forward) ? 1f : 0f);
			float num12 = (context.ReadValueAsButton(this.backward) ? (-1f) : 0f);
			Vector3 normalized = new Vector3(num9 + num10, num7 + num8, num11 + num12);
			if (this.mode == Vector3Composite.Mode.DigitalNormalized)
			{
				normalized = normalized.normalized;
			}
			return normalized;
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0005749C File Offset: 0x0005569C
		public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
		{
			return this.ReadValue(ref context).magnitude;
		}

		// Token: 0x04000763 RID: 1891
		[InputControl(layout = "Axis")]
		public int up;

		// Token: 0x04000764 RID: 1892
		[InputControl(layout = "Axis")]
		public int down;

		// Token: 0x04000765 RID: 1893
		[InputControl(layout = "Axis")]
		public int left;

		// Token: 0x04000766 RID: 1894
		[InputControl(layout = "Axis")]
		public int right;

		// Token: 0x04000767 RID: 1895
		[InputControl(layout = "Axis")]
		public int forward;

		// Token: 0x04000768 RID: 1896
		[InputControl(layout = "Axis")]
		public int backward;

		// Token: 0x04000769 RID: 1897
		public Vector3Composite.Mode mode;

		// Token: 0x0200025D RID: 605
		public enum Mode
		{
			// Token: 0x04000CCD RID: 3277
			Analog,
			// Token: 0x04000CCE RID: 3278
			DigitalNormalized,
			// Token: 0x04000CCF RID: 3279
			Digital
		}
	}
}
