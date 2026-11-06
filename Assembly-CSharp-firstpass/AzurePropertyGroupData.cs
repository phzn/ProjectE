using System;
using System.Collections.Generic;

namespace UnityEngine.AzureSky
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public sealed class AzurePropertyGroupData
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000387C File Offset: 0x00001A7C
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00003884 File Offset: 0x00001A84
		public List<AzurePropertyData> propertyDataList
		{
			get
			{
				return this.m_propertyDataList;
			}
			set
			{
				this.m_propertyDataList = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000388D File Offset: 0x00001A8D
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00003895 File Offset: 0x00001A95
		public AnimationCurve rampCurve
		{
			get
			{
				return this.m_rampCurve;
			}
			set
			{
				this.m_rampCurve = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000389E File Offset: 0x00001A9E
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000038A6 File Offset: 0x00001AA6
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

		// Token: 0x04000089 RID: 137
		[SerializeField]
		private List<AzurePropertyData> m_propertyDataList = new List<AzurePropertyData>();

		// Token: 0x0400008A RID: 138
		[SerializeField]
		private AnimationCurve m_rampCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400008B RID: 139
		[SerializeField]
		private string m_name;
	}
}
