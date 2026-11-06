using System;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000036 RID: 54
	public abstract class InputProcessor<TValue> : InputProcessor where TValue : struct
	{
		// Token: 0x0600046A RID: 1130
		public abstract TValue Process(TValue value, InputControl control);

		// Token: 0x0600046B RID: 1131 RVA: 0x000137C0 File Offset: 0x000119C0
		public override object ProcessAsObject(object value, InputControl control)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is TValue))
			{
				throw new ArgumentException(string.Format("Expecting value of type '{0}' but got value '{1}' of type '{2}'", typeof(TValue).Name, value, value.GetType().Name), "value");
			}
			TValue tvalue = (TValue)((object)value);
			return this.Process(tvalue, control);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00013828 File Offset: 0x00011A28
		public unsafe override void Process(void* buffer, int bufferSize, InputControl control)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = UnsafeUtility.SizeOf<TValue>();
			if (bufferSize < num)
			{
				throw new ArgumentException(string.Format("Expected buffer of at least {0} bytes but got buffer with just {1} bytes", num, bufferSize), "bufferSize");
			}
			TValue tvalue = default(TValue);
			void* ptr = UnsafeUtility.AddressOf<TValue>(ref tvalue);
			UnsafeUtility.MemCpy(ptr, buffer, (long)num);
			tvalue = this.Process(tvalue, control);
			ptr = UnsafeUtility.AddressOf<TValue>(ref tvalue);
			UnsafeUtility.MemCpy(buffer, ptr, (long)num);
		}
	}
}
