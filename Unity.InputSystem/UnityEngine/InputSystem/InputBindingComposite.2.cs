using System;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000028 RID: 40
	public abstract class InputBindingComposite<TValue> : InputBindingComposite where TValue : struct
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000CF79 File Offset: 0x0000B179
		public override Type valueType
		{
			get
			{
				return typeof(TValue);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000CF85 File Offset: 0x0000B185
		public override int valueSizeInBytes
		{
			get
			{
				return UnsafeUtility.SizeOf<TValue>();
			}
		}

		// Token: 0x060002E7 RID: 743
		public abstract TValue ReadValue(ref InputBindingCompositeContext context);

		// Token: 0x060002E8 RID: 744 RVA: 0x0000CF8C File Offset: 0x0000B18C
		public unsafe override void ReadValue(ref InputBindingCompositeContext context, void* buffer, int bufferSize)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = UnsafeUtility.SizeOf<TValue>();
			if (bufferSize < num)
			{
				throw new ArgumentException(string.Format("Expected buffer of at least {0} bytes but got buffer of only {1} bytes instead", UnsafeUtility.SizeOf<TValue>(), bufferSize), "bufferSize");
			}
			TValue tvalue = this.ReadValue(ref context);
			void* ptr = UnsafeUtility.AddressOf<TValue>(ref tvalue);
			UnsafeUtility.MemCpy(buffer, ptr, (long)num);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000CFF4 File Offset: 0x0000B1F4
		public unsafe override object ReadValueAsObject(ref InputBindingCompositeContext context)
		{
			TValue tvalue = default(TValue);
			void* ptr = UnsafeUtility.AddressOf<TValue>(ref tvalue);
			this.ReadValue(ref context, ptr, UnsafeUtility.SizeOf<TValue>());
			return tvalue;
		}
	}
}
