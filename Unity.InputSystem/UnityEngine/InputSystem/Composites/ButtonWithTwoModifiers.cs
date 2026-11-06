using System;
using System.ComponentModel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Composites
{
	// Token: 0x02000151 RID: 337
	[DesignTimeVisible(false)]
	[DisplayStringFormat("{modifier1}+{modifier2}+{button}")]
	public class ButtonWithTwoModifiers : InputBindingComposite<float>
	{
		// Token: 0x06001253 RID: 4691 RVA: 0x00056E84 File Offset: 0x00055084
		public override float ReadValue(ref InputBindingCompositeContext context)
		{
			if (this.ModifiersArePressed(ref context))
			{
				return context.ReadValue<float>(this.button);
			}
			return 0f;
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00056EA4 File Offset: 0x000550A4
		private bool ModifiersArePressed(ref InputBindingCompositeContext context)
		{
			bool flag = context.ReadValueAsButton(this.modifier1) && context.ReadValueAsButton(this.modifier2);
			if (flag && this.modifiersOrder == ButtonWithTwoModifiers.ModifiersOrder.Ordered)
			{
				double pressTime = context.GetPressTime(this.button);
				double pressTime2 = context.GetPressTime(this.modifier1);
				double pressTime3 = context.GetPressTime(this.modifier2);
				return pressTime2 <= pressTime && pressTime3 <= pressTime;
			}
			return flag;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00056F0F File Offset: 0x0005510F
		public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
		{
			return this.ReadValue(ref context);
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00056F18 File Offset: 0x00055118
		protected override void FinishSetup(ref InputBindingCompositeContext context)
		{
			if (this.modifiersOrder == ButtonWithTwoModifiers.ModifiersOrder.Default)
			{
				if (this.overrideModifiersNeedToBePressedFirst)
				{
					this.modifiersOrder = ButtonWithTwoModifiers.ModifiersOrder.Unordered;
					return;
				}
				this.modifiersOrder = (InputSystem.settings.shortcutKeysConsumeInput ? ButtonWithTwoModifiers.ModifiersOrder.Ordered : ButtonWithTwoModifiers.ModifiersOrder.Unordered);
			}
		}

		// Token: 0x04000749 RID: 1865
		[InputControl(layout = "Button")]
		public int modifier1;

		// Token: 0x0400074A RID: 1866
		[InputControl(layout = "Button")]
		public int modifier2;

		// Token: 0x0400074B RID: 1867
		[InputControl(layout = "Button")]
		public int button;

		// Token: 0x0400074C RID: 1868
		[Tooltip("Obsolete please use modifiers Order. If enabled, this will override the Input Consumption setting, allowing the modifier keys to be pressed after the button and the composite will still trigger.")]
		[Obsolete("Use ModifiersOrder.Unordered with 'modifiersOrder' instead")]
		public bool overrideModifiersNeedToBePressedFirst;

		// Token: 0x0400074D RID: 1869
		[Tooltip("By default it follows the Input Consumption setting to determine if the modifers keys need to be pressed first.")]
		public ButtonWithTwoModifiers.ModifiersOrder modifiersOrder;

		// Token: 0x02000259 RID: 601
		public enum ModifiersOrder
		{
			// Token: 0x04000CBD RID: 3261
			Default,
			// Token: 0x04000CBE RID: 3262
			Ordered,
			// Token: 0x04000CBF RID: 3263
			Unordered
		}
	}
}
