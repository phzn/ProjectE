using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000263 RID: 611
	internal interface IGroupManager
	{
		// Token: 0x06001160 RID: 4448
		void Init(IGroupBox groupBox);

		// Token: 0x06001161 RID: 4449
		IGroupBoxOption GetSelectedOption();

		// Token: 0x06001162 RID: 4450
		void OnOptionSelectionChanged(IGroupBoxOption selectedOption);

		// Token: 0x06001163 RID: 4451
		void RegisterOption(IGroupBoxOption option);

		// Token: 0x06001164 RID: 4452
		void UnregisterOption(IGroupBoxOption option);
	}
}
