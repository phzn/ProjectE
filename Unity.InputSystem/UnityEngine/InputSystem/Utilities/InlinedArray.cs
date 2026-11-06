using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000134 RID: 308
	internal struct InlinedArray<TValue> : IEnumerable<TValue>, IEnumerable
	{
		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x0600111B RID: 4379 RVA: 0x00051C8D File Offset: 0x0004FE8D
		public int Capacity
		{
			get
			{
				TValue[] array = this.additionalValues;
				if (array == null)
				{
					return 1;
				}
				return array.Length + 1;
			}
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00051C9F File Offset: 0x0004FE9F
		public InlinedArray(TValue value)
		{
			this.length = 1;
			this.firstValue = value;
			this.additionalValues = null;
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00051CB6 File Offset: 0x0004FEB6
		public InlinedArray(TValue firstValue, params TValue[] additionalValues)
		{
			this.length = 1 + additionalValues.Length;
			this.firstValue = firstValue;
			this.additionalValues = additionalValues;
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00051CD4 File Offset: 0x0004FED4
		public InlinedArray(IEnumerable<TValue> values)
		{
			this = default(InlinedArray<TValue>);
			this.length = values.Count<TValue>();
			if (this.length > 1)
			{
				this.additionalValues = new TValue[this.length - 1];
			}
			else
			{
				this.additionalValues = null;
			}
			int num = 0;
			foreach (TValue tvalue in values)
			{
				if (num == 0)
				{
					this.firstValue = tvalue;
				}
				else
				{
					this.additionalValues[num - 1] = tvalue;
				}
				num++;
			}
		}

		// Token: 0x170004D6 RID: 1238
		public TValue this[int index]
		{
			get
			{
				if (index < 0 || index >= this.length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index == 0)
				{
					return this.firstValue;
				}
				return this.additionalValues[index - 1];
			}
			set
			{
				if (index < 0 || index >= this.length)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index == 0)
				{
					this.firstValue = value;
					return;
				}
				this.additionalValues[index - 1] = value;
			}
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00051DD6 File Offset: 0x0004FFD6
		public void Clear()
		{
			this.length = 0;
			this.firstValue = default(TValue);
			this.additionalValues = null;
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x00051DF4 File Offset: 0x0004FFF4
		public void ClearWithCapacity()
		{
			this.firstValue = default(TValue);
			for (int i = 0; i < this.length - 1; i++)
			{
				this.additionalValues[i] = default(TValue);
			}
			this.length = 0;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00051E3C File Offset: 0x0005003C
		public InlinedArray<TValue> Clone()
		{
			return new InlinedArray<TValue>
			{
				length = this.length,
				firstValue = this.firstValue,
				additionalValues = ((this.additionalValues != null) ? ArrayHelpers.Copy<TValue>(this.additionalValues) : null)
			};
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00051E8C File Offset: 0x0005008C
		public void SetLength(int size)
		{
			if (size < this.length)
			{
				for (int i = size; i < this.length; i++)
				{
					this[i] = default(TValue);
				}
			}
			this.length = size;
			if (size > 1 && (this.additionalValues == null || this.additionalValues.Length < size - 1))
			{
				Array.Resize<TValue>(ref this.additionalValues, size - 1);
			}
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x00051EF1 File Offset: 0x000500F1
		public TValue[] ToArray()
		{
			return ArrayHelpers.Join<TValue>(this.firstValue, this.additionalValues);
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00051F04 File Offset: 0x00050104
		public TOther[] ToArray<TOther>(Func<TValue, TOther> mapFunction)
		{
			if (this.length == 0)
			{
				return null;
			}
			TOther[] array = new TOther[this.length];
			for (int i = 0; i < this.length; i++)
			{
				array[i] = mapFunction(this[i]);
			}
			return array;
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00051F50 File Offset: 0x00050150
		public int IndexOf(TValue value)
		{
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			if (this.length > 0)
			{
				if (@default.Equals(this.firstValue, value))
				{
					return 0;
				}
				if (this.additionalValues != null)
				{
					for (int i = 0; i < this.length - 1; i++)
					{
						if (@default.Equals(this.additionalValues[i], value))
						{
							return i + 1;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00051FB4 File Offset: 0x000501B4
		public int Append(TValue value)
		{
			if (this.length == 0)
			{
				this.firstValue = value;
			}
			else if (this.additionalValues == null)
			{
				this.additionalValues = new TValue[1];
				this.additionalValues[0] = value;
			}
			else
			{
				Array.Resize<TValue>(ref this.additionalValues, this.length);
				this.additionalValues[this.length - 1] = value;
			}
			int num = this.length;
			this.length++;
			return num;
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00052030 File Offset: 0x00050230
		public int AppendWithCapacity(TValue value, int capacityIncrement = 10)
		{
			if (this.length == 0)
			{
				this.firstValue = value;
			}
			else
			{
				int num = this.length - 1;
				ArrayHelpers.AppendWithCapacity<TValue>(ref this.additionalValues, ref num, value, capacityIncrement);
			}
			int num2 = this.length;
			this.length++;
			return num2;
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0005207C File Offset: 0x0005027C
		public void AssignWithCapacity(InlinedArray<TValue> values)
		{
			if (this.Capacity < values.length && values.length > 1)
			{
				this.additionalValues = new TValue[values.length - 1];
			}
			this.length = values.length;
			if (this.length > 0)
			{
				this.firstValue = values.firstValue;
			}
			if (this.length > 1)
			{
				Array.Copy(values.additionalValues, this.additionalValues, this.length - 1);
			}
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x000520F8 File Offset: 0x000502F8
		public void Append(IEnumerable<TValue> values)
		{
			foreach (TValue tvalue in values)
			{
				this.Append(tvalue);
			}
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00052144 File Offset: 0x00050344
		public void Remove(TValue value)
		{
			if (this.length < 1)
			{
				return;
			}
			if (EqualityComparer<TValue>.Default.Equals(this.firstValue, value))
			{
				this.RemoveAt(0);
				return;
			}
			if (this.additionalValues != null)
			{
				for (int i = 0; i < this.length - 1; i++)
				{
					if (EqualityComparer<TValue>.Default.Equals(this.additionalValues[i], value))
					{
						this.RemoveAt(i + 1);
						return;
					}
				}
			}
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x000521B4 File Offset: 0x000503B4
		public void RemoveAtWithCapacity(int index)
		{
			if (index < 0 || index >= this.length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (index == 0)
			{
				if (this.length == 1)
				{
					this.firstValue = default(TValue);
				}
				else if (this.length == 2)
				{
					this.firstValue = this.additionalValues[0];
					this.additionalValues[0] = default(TValue);
				}
				else
				{
					this.firstValue = this.additionalValues[0];
					int num = this.length - 1;
					this.additionalValues.EraseAtWithCapacity<TValue>(ref num, 0);
				}
			}
			else
			{
				int num2 = this.length - 1;
				this.additionalValues.EraseAtWithCapacity<TValue>(ref num2, index - 1);
			}
			this.length--;
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00052278 File Offset: 0x00050478
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (index == 0)
			{
				if (this.additionalValues != null)
				{
					this.firstValue = this.additionalValues[0];
					if (this.additionalValues.Length == 1)
					{
						this.additionalValues = null;
					}
					else
					{
						Array.Copy(this.additionalValues, 1, this.additionalValues, 0, this.additionalValues.Length - 1);
						Array.Resize<TValue>(ref this.additionalValues, this.additionalValues.Length - 1);
					}
				}
				else
				{
					this.firstValue = default(TValue);
				}
			}
			else
			{
				int num = this.length - 1;
				if (num == 1)
				{
					this.additionalValues = null;
				}
				else if (index == this.length - 1)
				{
					Array.Resize<TValue>(ref this.additionalValues, num - 1);
				}
				else
				{
					TValue[] array = new TValue[num - 1];
					if (index >= 2)
					{
						Array.Copy(this.additionalValues, 0, array, 0, index - 1);
					}
					Array.Copy(this.additionalValues, index + 1 - 1, array, index - 1, this.length - index - 1);
					this.additionalValues = array;
				}
			}
			this.length--;
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00052398 File Offset: 0x00050598
		public void RemoveAtByMovingTailWithCapacity(int index)
		{
			if (index < 0 || index >= this.length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int num = this.length - 1;
			if (index == 0)
			{
				if (this.length > 1)
				{
					this.firstValue = this.additionalValues[num - 1];
					this.additionalValues[num - 1] = default(TValue);
				}
				else
				{
					this.firstValue = default(TValue);
				}
			}
			else
			{
				ArrayHelpers.EraseAtByMovingTail<TValue>(this.additionalValues, ref num, index - 1);
			}
			this.length--;
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x0005242C File Offset: 0x0005062C
		public bool RemoveByMovingTailWithCapacity(TValue value)
		{
			int num = this.IndexOf(value);
			if (num == -1)
			{
				return false;
			}
			this.RemoveAtByMovingTailWithCapacity(num);
			return true;
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00052450 File Offset: 0x00050650
		public bool Contains(TValue value, IEqualityComparer<TValue> comparer)
		{
			for (int i = 0; i < this.length; i++)
			{
				if (comparer.Equals(this[i], value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00052484 File Offset: 0x00050684
		public void Merge(InlinedArray<TValue> other)
		{
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			for (int i = 0; i < other.length; i++)
			{
				TValue tvalue = other[i];
				if (!this.Contains(tvalue, @default))
				{
					this.Append(tvalue);
				}
			}
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x000524C4 File Offset: 0x000506C4
		public IEnumerator<TValue> GetEnumerator()
		{
			return new InlinedArray<TValue>.Enumerator
			{
				array = this,
				index = -1
			};
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000524F4 File Offset: 0x000506F4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400070B RID: 1803
		public int length;

		// Token: 0x0400070C RID: 1804
		public TValue firstValue;

		// Token: 0x0400070D RID: 1805
		public TValue[] additionalValues;

		// Token: 0x02000245 RID: 581
		private struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
		{
			// Token: 0x0600161B RID: 5659 RVA: 0x00065C26 File Offset: 0x00063E26
			public bool MoveNext()
			{
				if (this.index >= this.array.length)
				{
					return false;
				}
				this.index++;
				return this.index < this.array.length;
			}

			// Token: 0x0600161C RID: 5660 RVA: 0x00065C5E File Offset: 0x00063E5E
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x170005FD RID: 1533
			// (get) Token: 0x0600161D RID: 5661 RVA: 0x00065C67 File Offset: 0x00063E67
			public TValue Current
			{
				get
				{
					return this.array[this.index];
				}
			}

			// Token: 0x170005FE RID: 1534
			// (get) Token: 0x0600161E RID: 5662 RVA: 0x00065C7A File Offset: 0x00063E7A
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0600161F RID: 5663 RVA: 0x00065C87 File Offset: 0x00063E87
			public void Dispose()
			{
			}

			// Token: 0x04000C6E RID: 3182
			public InlinedArray<TValue> array;

			// Token: 0x04000C6F RID: 3183
			public int index;
		}
	}
}
