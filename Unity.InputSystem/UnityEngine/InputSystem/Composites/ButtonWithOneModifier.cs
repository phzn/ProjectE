using System;
using System.ComponentModel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Composites
{
	// Token: 0x02000150 RID: 336
	[DesignTimeVisible(false)]
	[DisplayStringFormat("{modifier}+{button}")]
	public class ButtonWithOneModifier : InputBindingComposite<float>
	{
		// Token: 0x0600124E RID: 4686 RVA: 0x00056DDF File Offset: 0x00054FDF
		public override float ReadValue(ref InputBindingCompositeContext context)
		{
			if (this.ModifierIsPressed(ref context))
			{
				return context.ReadValue<float>(this.button);
			}
			return 0f;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00056DFC File Offset: 0x00054FFC
		private bool ModifierIsPressed(ref InputBindingCompositeContext context)
		{
			bool flag = context.ReadValueAsButton(this.modifier);
			if (flag && this.modifiersOrder == ButtonWithOneModifier.ModifiersOrder.Ordered)
			{
				double pressTime = context.GetPressTime(this.button);
				return context.GetPressTime(this.modifier) <= pressTime;
			}
			return flag;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00056E43 File Offset: 0x00055043
		public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
		{
			return this.ReadValue(ref context);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00056E4C File Offset: 0x0005504C
		protected override void FinishSetup(ref InputBindingCompositeContext context)
		{
			if (this.modifiersOrder == ButtonWithOneModifier.ModifiersOrder.Default)
			{
				if (this.overrideModifiersNeedToBePressedFirst)
				{
					this.modifiersOrder = ButtonWithOneModifier.ModifiersOrder.Unordered;
					return;
				}
				this.modifiersOrder = (InputSystem.settings.shortcutKeysConsumeInput ? ButtonWithOneModifier.ModifiersOrder.Ordered : ButtonWithOneModifier.ModifiersOrder.Unordered);
			}
		}

		// Token: 0x04000745 RID: 1861
		[InputControl(layout = "Button")]
		public int modifier;

		// Token: 0x04000746 RID: 1862
		[InputControl(layout = "Button")]
		public int button;

		// Token: 0x04000747 RID: 1863
		[Tooltip("Obsolete please use modifiers Order. If enabled, this will override the Input Consumption setting, allowing the modifier keys to be pressed after the button and the composite will still trigger.")]
		[Obsolete("Use ModifiersOrder.Unordered with 'modifiersOrder' instead")]
		public bool overrideModifiersNeedToBePressedFirst;

		// Token: 0x04000748 RID: 1864
		[Tooltip("By default it follows the Input Consumption setting to determine if the modifers keys need to be pressed first.")]
		public ButtonWithOneModifier.ModifiersOrder modifiersOrder;

		// Token: 0x02000258 RID: 600
		public enum ModifiersOrder
		{
			// Token: 0x04000CB9 RID: 3257
			Default,
			// Token: 0x04000CBA RID: 3258
			Ordered,
			// Token: 0x04000CBB RID: 3259
			Unordered
		}
	}
}
