using System;
using System.Collections.Generic;

namespace MoreMountains.Tools
{
	// Token: 0x02000112 RID: 274
	public struct MMObservable<T>
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x0001D6E9 File Offset: 0x0001B8E9
		// (set) Token: 0x060005FE RID: 1534 RVA: 0x0001D6F4 File Offset: 0x0001B8F4
		public T Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (!EqualityComparer<T>.Default.Equals(value, this._value))
				{
					T value2 = this._value;
					this._value = value;
					Action onValueChanged = this.OnValueChanged;
					if (onValueChanged != null)
					{
						onValueChanged();
					}
					Action<T> onValueChangedTo = this.OnValueChangedTo;
					if (onValueChangedTo != null)
					{
						onValueChangedTo(this._value);
					}
					Action<T, T> onValueChangedFromTo = this.OnValueChangedFromTo;
					if (onValueChangedFromTo == null)
					{
						return;
					}
					onValueChangedFromTo(value2, this._value);
				}
			}
		}

		// Token: 0x04000722 RID: 1826
		public Action OnValueChanged;

		// Token: 0x04000723 RID: 1827
		public Action<T> OnValueChangedTo;

		// Token: 0x04000724 RID: 1828
		public Action<T, T> OnValueChangedFromTo;

		// Token: 0x04000725 RID: 1829
		private T _value;
	}
}
