using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000264 RID: 612
	internal class DefaultGroupManager : IGroupManager
	{
		// Token: 0x06001165 RID: 4453 RVA: 0x0003F0C0 File Offset: 0x0003D2C0
		public void Init(IGroupBox groupBox)
		{
			this.m_GroupBox = groupBox;
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x0003F0CC File Offset: 0x0003D2CC
		public IGroupBoxOption GetSelectedOption()
		{
			return this.m_SelectedOption;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x0003F0E4 File Offset: 0x0003D2E4
		public void OnOptionSelectionChanged(IGroupBoxOption selectedOption)
		{
			bool flag = this.m_SelectedOption == selectedOption;
			if (!flag)
			{
				this.m_SelectedOption = selectedOption;
				foreach (IGroupBoxOption groupBoxOption in this.m_GroupOptions)
				{
					groupBoxOption.SetSelected(groupBoxOption == this.m_SelectedOption);
				}
			}
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0003F15C File Offset: 0x0003D35C
		public void RegisterOption(IGroupBoxOption option)
		{
			bool flag = !this.m_GroupOptions.Contains(option);
			if (flag)
			{
				this.m_GroupOptions.Add(option);
				this.m_GroupBox.OnOptionAdded(option);
			}
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0003F199 File Offset: 0x0003D399
		public void UnregisterOption(IGroupBoxOption option)
		{
			this.m_GroupOptions.Remove(option);
			this.m_GroupBox.OnOptionRemoved(option);
		}

		// Token: 0x040007A7 RID: 1959
		private List<IGroupBoxOption> m_GroupOptions = new List<IGroupBoxOption>();

		// Token: 0x040007A8 RID: 1960
		private IGroupBoxOption m_SelectedOption;

		// Token: 0x040007A9 RID: 1961
		private IGroupBox m_GroupBox;
	}
}
