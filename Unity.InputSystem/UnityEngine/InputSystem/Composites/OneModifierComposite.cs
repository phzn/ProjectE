using System;
using System.ComponentModel;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Composites
{
	// Token: 0x02000152 RID: 338
	[DisplayStringFormat("{modifier}+{binding}")]
	[DisplayName("Binding With One Modifier")]
	public class OneModifierComposite : InputBindingComposite
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001258 RID: 4696 RVA: 0x00056F50 File Offset: 0x00055150
		public override Type valueType
		{
			get
			{
				return this.m_ValueType;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001259 RID: 4697 RVA: 0x00056F58 File Offset: 0x00055158
		public override int valueSizeInBytes
		{
			get
			{
				return this.m_ValueSizeInBytes;
			}
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00056F60 File Offset: 0x00055160
		public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
		{
			if (this.ModifierIsPressed(ref context))
			{
				return context.EvaluateMagnitude(this.binding);
			}
			return 0f;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00056F7D File Offset: 0x0005517D
		public unsafe override void ReadValue(ref InputBindingCompositeContext context, void* buffer, int bufferSize)
		{
			if (this.ModifierIsPressed(ref context))
			{
				context.ReadValue(this.binding, buffer, bufferSize);
				return;
			}
			UnsafeUtility.MemClear(buffer, (long)this.m_ValueSizeInBytes);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00056FA4 File Offset: 0x000551A4
		private bool ModifierIsPressed(ref InputBindingCompositeContext context)
		{
			bool flag = context.ReadValueAsButton(this.modifier);
			if (flag && this.m_BindingIsButton && this.modifiersOrder == OneModifierComposite.ModifiersOrder.Ordered)
			{
				double pressTime = context.GetPressTime(this.binding);
				return context.GetPressTime(this.modifier) <= pressTime;
			}
			return flag;
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00056FF4 File Offset: 0x000551F4
		protected override void FinishSetup(ref InputBindingCompositeContext context)
		{
			OneModifierComposite.DetermineValueTypeAndSize(ref context, this.binding, out this.m_ValueType, out this.m_ValueSizeInBytes, out this.m_BindingIsButton);
			if (this.modifiersOrder == OneModifierComposite.ModifiersOrder.Default)
			{
				if (this.overrideModifiersNeedToBePressedFirst)
				{
					this.modifiersOrder = OneModifierComposite.ModifiersOrder.Unordered;
					return;
				}
				this.modifiersOrder = (InputSystem.settings.shortcutKeysConsumeInput ? OneModifierComposite.ModifiersOrder.Ordered : OneModifierComposite.ModifiersOrder.Unordered);
			}
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0005704D File Offset: 0x0005524D
		public override object ReadValueAsObject(ref InputBindingCompositeContext context)
		{
			if (context.ReadValueAsButton(this.modifier))
			{
				return context.ReadValueAsObject(this.binding);
			}
			return null;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0005706C File Offset: 0x0005526C
		internal static void DetermineValueTypeAndSize(ref InputBindingCompositeContext context, int part, out Type valueType, out int valueSizeInBytes, out bool isButton)
		{
			valueSizeInBytes = 0;
			isButton = true;
			Type type = null;
			foreach (InputBindingCompositeContext.PartBinding partBinding in context.controls)
			{
				if (partBinding.part == part)
				{
					Type valueType2 = partBinding.control.valueType;
					if (type == null || valueType2.IsAssignableFrom(type))
					{
						type = valueType2;
					}
					else if (!type.IsAssignableFrom(valueType2))
					{
						type = typeof(Object);
					}
					valueSizeInBytes = Math.Max(partBinding.control.valueSizeInBytes, valueSizeInBytes);
					isButton &= partBinding.control.isButton;
				}
			}
			valueType = type;
		}

		// Token: 0x0400074E RID: 1870
		[InputControl(layout = "Button")]
		public int modifier;

		// Token: 0x0400074F RID: 1871
		[InputControl]
		public int binding;

		// Token: 0x04000750 RID: 1872
		[Tooltip("Obsolete please use modifiers Order. If enabled, this will override the Input Consumption setting, allowing the modifier keys to be pressed after the button and the composite will still trigger.")]
		[Obsolete("Use ModifiersOrder.Unordered with 'modifiersOrder' instead")]
		public bool overrideModifiersNeedToBePressedFirst;

		// Token: 0x04000751 RID: 1873
		[Tooltip("By default it follows the Input Consumption setting to determine if the modifers keys need to be pressed first.")]
		public OneModifierComposite.ModifiersOrder modifiersOrder;

		// Token: 0x04000752 RID: 1874
		private int m_ValueSizeInBytes;

		// Token: 0x04000753 RID: 1875
		private Type m_ValueType;

		// Token: 0x04000754 RID: 1876
		private bool m_BindingIsButton;

		// Token: 0x0200025A RID: 602
		public enum ModifiersOrder
		{
			// Token: 0x04000CC1 RID: 3265
			Default,
			// Token: 0x04000CC2 RID: 3266
			Ordered,
			// Token: 0x04000CC3 RID: 3267
			Unordered
		}
	}
}
