using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000C9 RID: 201
	public interface INotifyValueChanged<T>
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060006C4 RID: 1732
		// (set) Token: 0x060006C5 RID: 1733
		T value { get; set; }

		// Token: 0x060006C6 RID: 1734
		void SetValueWithoutNotify(T newValue);
	}
}
