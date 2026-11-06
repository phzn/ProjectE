using System;
using System.ComponentModel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Processors;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Composites
{
	// Token: 0x0200014F RID: 335
	[DisplayStringFormat("{negative}/{positive}")]
	[DisplayName("Positive/Negative Binding")]
	public class AxisComposite : InputBindingComposite<float>
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600124A RID: 4682 RVA: 0x00056C9D File Offset: 0x00054E9D
		public float midPoint
		{
			get
			{
				return (this.maxValue + this.minValue) / 2f;
			}
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00056CB4 File Offset: 0x00054EB4
		public override float ReadValue(ref InputBindingCompositeContext context)
		{
			float num = Mathf.Abs(context.ReadValue<float>(this.negative));
			float num2 = Mathf.Abs(context.ReadValue<float>(this.positive));
			bool flag = num > Mathf.Epsilon;
			bool flag2 = num2 > Mathf.Epsilon;
			if (flag == flag2)
			{
				switch (this.whichSideWins)
				{
				case AxisComposite.WhichSideWins.Neither:
					return this.midPoint;
				case AxisComposite.WhichSideWins.Positive:
					flag = false;
					break;
				}
			}
			float midPoint = this.midPoint;
			if (flag)
			{
				return midPoint - (midPoint - this.minValue) * num;
			}
			return midPoint + (this.maxValue - midPoint) * num2;
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00056D50 File Offset: 0x00054F50
		public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
		{
			float num = this.ReadValue(ref context);
			if (num < this.midPoint)
			{
				num = Mathf.Abs(num - this.midPoint);
				return NormalizeProcessor.Normalize(num, 0f, Mathf.Abs(this.minValue), 0f);
			}
			num = Mathf.Abs(num - this.midPoint);
			return NormalizeProcessor.Normalize(num, 0f, Mathf.Abs(this.maxValue), 0f);
		}

		// Token: 0x04000740 RID: 1856
		[InputControl(layout = "Axis")]
		public int negative;

		// Token: 0x04000741 RID: 1857
		[InputControl(layout = "Axis")]
		public int positive;

		// Token: 0x04000742 RID: 1858
		[Tooltip("Value to return when the negative side is fully actuated.")]
		public float minValue = -1f;

		// Token: 0x04000743 RID: 1859
		[Tooltip("Value to return when the positive side is fully actuated.")]
		public float maxValue = 1f;

		// Token: 0x04000744 RID: 1860
		[Tooltip("If both the positive and negative side are actuated, decides what value to return. 'Neither' (default) means that the resulting value is the midpoint between min and max. 'Positive' means that max will be returned. 'Negative' means that min will be returned.")]
		public AxisComposite.WhichSideWins whichSideWins;

		// Token: 0x02000257 RID: 599
		public enum WhichSideWins
		{
			// Token: 0x04000CB5 RID: 3253
			Neither,
			// Token: 0x04000CB6 RID: 3254
			Positive,
			// Token: 0x04000CB7 RID: 3255
			Negative
		}
	}
}
