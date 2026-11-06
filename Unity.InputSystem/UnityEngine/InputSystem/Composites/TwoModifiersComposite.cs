using System;
using System.ComponentModel;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Composites
{
	// Token: 0x02000153 RID: 339
	[DisplayStringFormat("{modifier1}+{modifier2}+{binding}")]
	[DisplayName("Binding With Two Modifiers")]
	public class TwoModifiersComposite : InputBindingComposite
	{
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x00057130 File Offset: 0x00055330
		public override Type valueType
		{
			get
			{
				return this.m_ValueType;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001262 RID: 4706 RVA: 0x00057138 File Offset: 0x00055338
		public override int valueSizeInBytes
		{
			get
			{
				return this.m_ValueSizeInBytes;
			}
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00057140 File Offset: 0x00055340
		public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
		{
			if (this.ModifiersArePressed(ref context))
			{
				return context.EvaluateMagnitude(this.binding);
			}
			return 0f;
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0005715D File Offset: 0x0005535D
		public unsafe override void ReadValue(ref InputBindingCompositeContext context, void* buffer, int bufferSize)
		{
			if (this.ModifiersArePressed(ref context))
			{
				context.ReadValue(this.binding, buffer, bufferSize);
				return;
			}
			UnsafeUtility.MemClear(buffer, (long)this.m_ValueSizeInBytes);
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00057184 File Offset: 0x00055384
		private bool ModifiersArePressed(ref InputBindingCompositeContext context)
		{
			bool flag = context.ReadValueAsButton(this.modifier1) && context.ReadValueAsButton(this.modifier2);
			if (flag && this.m_BindingIsButton && this.modifiersOrder == TwoModifiersComposite.ModifiersOrder.Ordered)
			{
				double pressTime = context.GetPressTime(this.binding);
				double pressTime2 = context.GetPressTime(this.modifier1);
				double pressTime3 = context.GetPressTime(this.modifier2);
				return pressTime2 <= pressTime && pressTime3 <= pressTime;
			}
			return flag;
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x000571F8 File Offset: 0x000553F8
		protected override void FinishSetup(ref InputBindingCompositeContext context)
		{
			OneModifierComposite.DetermineValueTypeAndSize(ref context, this.binding, out this.m_ValueType, out this.m_ValueSizeInBytes, out this.m_BindingIsButton);
			if (this.modifiersOrder == TwoModifiersComposite.ModifiersOrder.Default)
			{
				if (this.overrideModifiersNeedToBePressedFirst)
				{
					this.modifiersOrder = TwoModifiersComposite.ModifiersOrder.Unordered;
					return;
				}
				this.modifiersOrder = (InputSystem.settings.shortcutKeysConsumeInput ? TwoModifiersComposite.ModifiersOrder.Ordered : TwoModifiersComposite.ModifiersOrder.Unordered);
			}
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00057251 File Offset: 0x00055451
		public override object ReadValueAsObject(ref InputBindingCompositeContext context)
		{
			if (context.ReadValueAsButton(this.modifier1) && context.ReadValueAsButton(this.modifier2))
			{
				return context.ReadValueAsObject(this.binding);
			}
			return null;
		}

		// Token: 0x04000755 RID: 1877
		[InputControl(layout = "Button")]
		public int modifier1;

		// Token: 0x04000756 RID: 1878
		[InputControl(layout = "Button")]
		public int modifier2;

		// Token: 0x04000757 RID: 1879
		[InputControl]
		public int binding;

		// Token: 0x04000758 RID: 1880
		[Tooltip("Obsolete please use modifiers Order. If enabled, this will override the Input Consumption setting, allowing the modifier keys to be pressed after the button and the composite will still trigger.")]
		[Obsolete("Use ModifiersOrder.Unordered with 'modifiersOrder' instead")]
		public bool overrideModifiersNeedToBePressedFirst;

		// Token: 0x04000759 RID: 1881
		[Tooltip("By default it follows the Input Consumption setting to determine if the modifers keys need to be pressed first.")]
		public TwoModifiersComposite.ModifiersOrder modifiersOrder;

		// Token: 0x0400075A RID: 1882
		private int m_ValueSizeInBytes;

		// Token: 0x0400075B RID: 1883
		private Type m_ValueType;

		// Token: 0x0400075C RID: 1884
		private bool m_BindingIsButton;

		// Token: 0x0200025B RID: 603
		public enum ModifiersOrder
		{
			// Token: 0x04000CC5 RID: 3269
			Default,
			// Token: 0x04000CC6 RID: 3270
			Ordered,
			// Token: 0x04000CC7 RID: 3271
			Unordered
		}
	}
}
