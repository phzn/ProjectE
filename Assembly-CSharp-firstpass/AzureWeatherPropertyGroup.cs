using System;
using System.Collections.Generic;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000025 RID: 37
	[Serializable]
	public sealed class AzureWeatherPropertyGroup
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00007B09 File Offset: 0x00005D09
		// (set) Token: 0x06000202 RID: 514 RVA: 0x00007B11 File Offset: 0x00005D11
		public string name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00007B1A File Offset: 0x00005D1A
		// (set) Token: 0x06000204 RID: 516 RVA: 0x00007B22 File Offset: 0x00005D22
		public List<AzureWeatherProperty> weatherPropertyList
		{
			get
			{
				return this.m_weatherPropertyList;
			}
			set
			{
				this.m_weatherPropertyList = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00007B2B File Offset: 0x00005D2B
		// (set) Token: 0x06000206 RID: 518 RVA: 0x00007B33 File Offset: 0x00005D33
		public bool isEnabled
		{
			get
			{
				return this.m_isEnabled;
			}
			set
			{
				this.m_isEnabled = value;
			}
		}

		// Token: 0x0400017C RID: 380
		[SerializeField]
		private string m_name = "GroupName";

		// Token: 0x0400017D RID: 381
		[SerializeField]
		private List<AzureWeatherProperty> m_weatherPropertyList = new List<AzureWeatherProperty>();

		// Token: 0x0400017E RID: 382
		[SerializeField]
		private bool m_isEnabled = true;
	}
}
