using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000030 RID: 48
	public abstract class InputControl<TValue> : InputControl where TValue : struct
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x00010AA0 File Offset: 0x0000ECA0
		public override Type valueType
		{
			get
			{
				return typeof(TValue);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x00010AAC File Offset: 0x0000ECAC
		public override int valueSizeInBytes
		{
			get
			{
				return UnsafeUtility.SizeOf<TValue>();
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00010AB4 File Offset: 0x0000ECB4
		public unsafe readonly ref TValue value
		{
			get
			{
				if (!InputSystem.s_Manager.readValueCachingFeatureEnabled || this.m_CachedValueIsStale || this.evaluateProcessorsEveryRead)
				{
					this.m_CachedValue = this.ProcessValue(*this.unprocessedValue);
					this.m_CachedValueIsStale = false;
				}
				return ref this.m_CachedValue;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x00010B04 File Offset: 0x0000ED04
		internal readonly ref TValue unprocessedValue
		{
			get
			{
				if (base.currentStatePtr == null)
				{
					return ref this.m_UnprocessedCachedValue;
				}
				if (!InputSystem.s_Manager.readValueCachingFeatureEnabled || this.m_UnprocessedCachedValueIsStale)
				{
					this.m_UnprocessedCachedValue = this.ReadUnprocessedValueFromState(base.currentStatePtr);
					this.m_UnprocessedCachedValueIsStale = false;
				}
				return ref this.m_UnprocessedCachedValue;
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00010B55 File Offset: 0x0000ED55
		public unsafe TValue ReadValue()
		{
			return *this.value;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00010B62 File Offset: 0x0000ED62
		public TValue ReadValueFromPreviousFrame()
		{
			return this.ReadValueFromState(base.previousFrameStatePtr);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00010B70 File Offset: 0x0000ED70
		public TValue ReadDefaultValue()
		{
			return this.ReadValueFromState(base.defaultStatePtr);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00010B7E File Offset: 0x0000ED7E
		public unsafe TValue ReadValueFromState(void* statePtr)
		{
			if (statePtr == null)
			{
				throw new ArgumentNullException("statePtr");
			}
			return this.ProcessValue(this.ReadUnprocessedValueFromState(statePtr));
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00010B9D File Offset: 0x0000ED9D
		public unsafe TValue ReadValueFromStateWithCaching(void* statePtr)
		{
			if (statePtr != base.currentStatePtr)
			{
				return this.ReadValueFromState(statePtr);
			}
			return *this.value;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00010BBB File Offset: 0x0000EDBB
		public unsafe TValue ReadUnprocessedValueFromStateWithCaching(void* statePtr)
		{
			if (statePtr != base.currentStatePtr)
			{
				return this.ReadUnprocessedValueFromState(statePtr);
			}
			return *this.unprocessedValue;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00010BD9 File Offset: 0x0000EDD9
		public unsafe TValue ReadUnprocessedValue()
		{
			return *this.unprocessedValue;
		}

		// Token: 0x060003EC RID: 1004
		public unsafe abstract TValue ReadUnprocessedValueFromState(void* statePtr);

		// Token: 0x060003ED RID: 1005 RVA: 0x00010BE6 File Offset: 0x0000EDE6
		public unsafe override object ReadValueFromStateAsObject(void* statePtr)
		{
			return this.ReadValueFromState(statePtr);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00010BF4 File Offset: 0x0000EDF4
		public unsafe override void ReadValueFromStateIntoBuffer(void* statePtr, void* bufferPtr, int bufferSize)
		{
			if (statePtr == null)
			{
				throw new ArgumentNullException("statePtr");
			}
			if (bufferPtr == null)
			{
				throw new ArgumentNullException("bufferPtr");
			}
			int num = UnsafeUtility.SizeOf<TValue>();
			if (bufferSize < num)
			{
				throw new ArgumentException(string.Format("bufferSize={0} < sizeof(TValue)={1}", bufferSize, num), "bufferSize");
			}
			TValue tvalue = this.ReadValueFromState(statePtr);
			void* ptr = UnsafeUtility.AddressOf<TValue>(ref tvalue);
			UnsafeUtility.MemCpy(bufferPtr, ptr, (long)num);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00010C68 File Offset: 0x0000EE68
		public unsafe override void WriteValueFromBufferIntoState(void* bufferPtr, int bufferSize, void* statePtr)
		{
			if (bufferPtr == null)
			{
				throw new ArgumentNullException("bufferPtr");
			}
			if (statePtr == null)
			{
				throw new ArgumentNullException("statePtr");
			}
			int num = UnsafeUtility.SizeOf<TValue>();
			if (bufferSize < num)
			{
				throw new ArgumentException(string.Format("bufferSize={0} < sizeof(TValue)={1}", bufferSize, num), "bufferSize");
			}
			TValue tvalue = default(TValue);
			UnsafeUtility.MemCpy(UnsafeUtility.AddressOf<TValue>(ref tvalue), bufferPtr, (long)num);
			this.WriteValueIntoState(tvalue, statePtr);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00010CE0 File Offset: 0x0000EEE0
		public unsafe override void WriteValueFromObjectIntoState(object value, void* statePtr)
		{
			if (statePtr == null)
			{
				throw new ArgumentNullException("statePtr");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is TValue))
			{
				value = Convert.ChangeType(value, typeof(TValue));
			}
			TValue tvalue = (TValue)((object)value);
			this.WriteValueIntoState(tvalue, statePtr);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00010D34 File Offset: 0x0000EF34
		public unsafe virtual void WriteValueIntoState(TValue value, void* statePtr)
		{
			throw new NotSupportedException(string.Format("Control '{0}' does not support writing", this));
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00010D48 File Offset: 0x0000EF48
		public unsafe override object ReadValueFromBufferAsObject(void* buffer, int bufferSize)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = UnsafeUtility.SizeOf<TValue>();
			if (bufferSize < num)
			{
				throw new ArgumentException(string.Format("Expecting buffer of at least {0} bytes for value of type {1} but got buffer of only {2} bytes instead", num, typeof(TValue).Name, bufferSize), "bufferSize");
			}
			TValue tvalue = default(TValue);
			UnsafeUtility.MemCpy(UnsafeUtility.AddressOf<TValue>(ref tvalue), buffer, (long)num);
			return tvalue;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00010DBC File Offset: 0x0000EFBC
		private unsafe static bool CompareValue(ref TValue firstValue, ref TValue secondValue)
		{
			void* ptr = UnsafeUtility.AddressOf<TValue>(ref firstValue);
			void* ptr2 = UnsafeUtility.AddressOf<TValue>(ref secondValue);
			return UnsafeUtility.MemCmp(ptr, ptr2, (long)UnsafeUtility.SizeOf<TValue>()) != 0;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00010DE8 File Offset: 0x0000EFE8
		public unsafe override bool CompareValue(void* firstStatePtr, void* secondStatePtr)
		{
			TValue tvalue = this.ReadValueFromState(firstStatePtr);
			TValue tvalue2 = this.ReadValueFromState(secondStatePtr);
			return InputControl<TValue>.CompareValue(ref tvalue, ref tvalue2);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00010E0E File Offset: 0x0000F00E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TValue ProcessValue(TValue value)
		{
			this.ProcessValue(ref value);
			return value;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00010E1C File Offset: 0x0000F01C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProcessValue(ref TValue value)
		{
			if (this.m_ProcessorStack.length <= 0)
			{
				return;
			}
			value = this.m_ProcessorStack.firstValue.Process(value, this);
			if (this.m_ProcessorStack.additionalValues == null)
			{
				return;
			}
			for (int i = 0; i < this.m_ProcessorStack.length - 1; i++)
			{
				value = this.m_ProcessorStack.additionalValues[i].Process(value, this);
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00010E9C File Offset: 0x0000F09C
		internal TProcessor TryGetProcessor<TProcessor>() where TProcessor : InputProcessor<TValue>
		{
			if (this.m_ProcessorStack.length > 0)
			{
				TProcessor tprocessor = this.m_ProcessorStack.firstValue as TProcessor;
				if (tprocessor != null)
				{
					return tprocessor;
				}
				if (this.m_ProcessorStack.additionalValues != null)
				{
					for (int i = 0; i < this.m_ProcessorStack.length - 1; i++)
					{
						TProcessor tprocessor2 = this.m_ProcessorStack.additionalValues[i] as TProcessor;
						if (tprocessor2 != null)
						{
							return tprocessor2;
						}
					}
				}
			}
			return default(TProcessor);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00010F28 File Offset: 0x0000F128
		internal override void AddProcessor(object processor)
		{
			InputProcessor<TValue> inputProcessor = processor as InputProcessor<TValue>;
			if (inputProcessor == null)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Cannot add processor of type '",
					processor.GetType().Name,
					"' to control of type '",
					base.GetType().Name,
					"'"
				}), "processor");
			}
			this.m_ProcessorStack.Append(inputProcessor);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00010F98 File Offset: 0x0000F198
		protected override void FinishSetup()
		{
			using (IEnumerator<InputProcessor<TValue>> enumerator = this.m_ProcessorStack.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.cachingPolicy == InputProcessor.CachingPolicy.EvaluateOnEveryRead)
					{
						this.evaluateProcessorsEveryRead = true;
					}
				}
			}
			base.FinishSetup();
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00010FF4 File Offset: 0x0000F1F4
		internal InputProcessor<TValue>[] processors
		{
			get
			{
				return this.m_ProcessorStack.ToArray();
			}
		}

		// Token: 0x0400013D RID: 317
		internal InlinedArray<InputProcessor<TValue>> m_ProcessorStack;

		// Token: 0x0400013E RID: 318
		private TValue m_CachedValue;

		// Token: 0x0400013F RID: 319
		private TValue m_UnprocessedCachedValue;

		// Token: 0x04000140 RID: 320
		internal bool evaluateProcessorsEveryRead;
	}
}
