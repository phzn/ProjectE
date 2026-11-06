using System;
using System.Collections.Generic;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000029 RID: 41
	public struct InputBindingCompositeContext
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000D02C File Offset: 0x0000B22C
		public unsafe IEnumerable<InputBindingCompositeContext.PartBinding> controls
		{
			get
			{
				if (this.m_State == null)
				{
					yield break;
				}
				int totalBindingCount = this.m_State.totalBindingCount;
				int num;
				for (int bindingIndex = this.m_BindingIndex + 1; bindingIndex < totalBindingCount; bindingIndex = num)
				{
					InputActionState.BindingState bindingState = *this.m_State.GetBindingState(bindingIndex);
					if (!bindingState.isPartOfComposite)
					{
						break;
					}
					int controlStartIndex = bindingState.controlStartIndex;
					for (int i = 0; i < bindingState.controlCount; i = num)
					{
						InputControl inputControl = this.m_State.controls[controlStartIndex + i];
						yield return new InputBindingCompositeContext.PartBinding
						{
							part = bindingState.partIndex,
							control = inputControl
						};
						num = i + 1;
					}
					num = bindingIndex + 1;
				}
				yield break;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000D041 File Offset: 0x0000B241
		public float EvaluateMagnitude(int partNumber)
		{
			return this.m_State.EvaluateCompositePartMagnitude(this.m_BindingIndex, partNumber);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000D058 File Offset: 0x0000B258
		public TValue ReadValue<TValue>(int partNumber) where TValue : struct, IComparable<TValue>
		{
			if (this.m_State == null)
			{
				return default(TValue);
			}
			int num;
			return this.m_State.ReadCompositePartValue<TValue, InputBindingCompositeContext.DefaultComparer<TValue>>(this.m_BindingIndex, partNumber, null, out num, default(InputBindingCompositeContext.DefaultComparer<TValue>));
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000D098 File Offset: 0x0000B298
		public TValue ReadValue<TValue>(int partNumber, out InputControl sourceControl) where TValue : struct, IComparable<TValue>
		{
			if (this.m_State == null)
			{
				sourceControl = null;
				return default(TValue);
			}
			int num;
			TValue tvalue = this.m_State.ReadCompositePartValue<TValue, InputBindingCompositeContext.DefaultComparer<TValue>>(this.m_BindingIndex, partNumber, null, out num, default(InputBindingCompositeContext.DefaultComparer<TValue>));
			if (num != -1)
			{
				sourceControl = this.m_State.controls[num];
				return tvalue;
			}
			sourceControl = null;
			return tvalue;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000D0F0 File Offset: 0x0000B2F0
		public TValue ReadValue<TValue, TComparer>(int partNumber, TComparer comparer = default(TComparer)) where TValue : struct where TComparer : IComparer<TValue>
		{
			if (this.m_State == null)
			{
				return default(TValue);
			}
			int num;
			return this.m_State.ReadCompositePartValue<TValue, TComparer>(this.m_BindingIndex, partNumber, null, out num, comparer);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000D128 File Offset: 0x0000B328
		public TValue ReadValue<TValue, TComparer>(int partNumber, out InputControl sourceControl, TComparer comparer = default(TComparer)) where TValue : struct where TComparer : IComparer<TValue>
		{
			if (this.m_State == null)
			{
				sourceControl = null;
				return default(TValue);
			}
			int num;
			TValue tvalue = this.m_State.ReadCompositePartValue<TValue, TComparer>(this.m_BindingIndex, partNumber, null, out num, comparer);
			if (num != -1)
			{
				sourceControl = this.m_State.controls[num];
				return tvalue;
			}
			sourceControl = null;
			return tvalue;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000D178 File Offset: 0x0000B378
		public unsafe bool ReadValueAsButton(int partNumber)
		{
			if (this.m_State == null)
			{
				return false;
			}
			bool flag = false;
			int num;
			this.m_State.ReadCompositePartValue<float, InputBindingCompositeContext.DefaultComparer<float>>(this.m_BindingIndex, partNumber, &flag, out num, default(InputBindingCompositeContext.DefaultComparer<float>));
			return flag;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000D1B3 File Offset: 0x0000B3B3
		public unsafe void ReadValue(int partNumber, void* buffer, int bufferSize)
		{
			InputActionState state = this.m_State;
			if (state == null)
			{
				return;
			}
			state.ReadCompositePartValue(this.m_BindingIndex, partNumber, buffer, bufferSize);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000D1CF File Offset: 0x0000B3CF
		public object ReadValueAsObject(int partNumber)
		{
			return this.m_State.ReadCompositePartValueAsObject(this.m_BindingIndex, partNumber);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000D1E3 File Offset: 0x0000B3E3
		public double GetPressTime(int partNumber)
		{
			return this.m_State.GetCompositePartPressTime(this.m_BindingIndex, partNumber);
		}

		// Token: 0x040000F0 RID: 240
		internal InputActionState m_State;

		// Token: 0x040000F1 RID: 241
		internal int m_BindingIndex;

		// Token: 0x02000183 RID: 387
		public struct PartBinding
		{
			// Token: 0x17000553 RID: 1363
			// (get) Token: 0x060013A8 RID: 5032 RVA: 0x0005BB47 File Offset: 0x00059D47
			// (set) Token: 0x060013A9 RID: 5033 RVA: 0x0005BB4F File Offset: 0x00059D4F
			public int part { readonly get; set; }

			// Token: 0x17000554 RID: 1364
			// (get) Token: 0x060013AA RID: 5034 RVA: 0x0005BB58 File Offset: 0x00059D58
			// (set) Token: 0x060013AB RID: 5035 RVA: 0x0005BB60 File Offset: 0x00059D60
			public InputControl control { readonly get; set; }
		}

		// Token: 0x02000184 RID: 388
		private struct DefaultComparer<TValue> : IComparer<TValue> where TValue : IComparable<TValue>
		{
			// Token: 0x060013AC RID: 5036 RVA: 0x0005BB69 File Offset: 0x00059D69
			public int Compare(TValue x, TValue y)
			{
				return x.CompareTo(y);
			}
		}
	}
}
