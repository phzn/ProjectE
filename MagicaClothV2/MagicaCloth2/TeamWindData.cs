using System;
using Unity.Collections;

namespace MagicaCloth2
{
	// Token: 0x020000BA RID: 186
	public struct TeamWindData
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00021E18 File Offset: 0x00020018
		public int ZoneCount
		{
			get
			{
				return this.windZoneList.Length;
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00021E28 File Offset: 0x00020028
		public int IndexOf(int windId)
		{
			int length = this.windZoneList.Length;
			for (int i = 0; i < length; i++)
			{
				if (this.windZoneList[i].windId == windId)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00021E64 File Offset: 0x00020064
		public void ClearZoneList()
		{
			this.windZoneList.Clear();
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00021E74 File Offset: 0x00020074
		public void AddOrReplaceWindZone(TeamWindInfo windInfo, in TeamWindData oldWindData)
		{
			if (!windInfo.IsValid())
			{
				return;
			}
			TeamWindData teamWindData = oldWindData;
			int num = teamWindData.IndexOf(windInfo.windId);
			if (num >= 0)
			{
				TeamWindInfo teamWindInfo = oldWindData.windZoneList[num];
				windInfo.time = teamWindInfo.time;
			}
			this.windZoneList.AddNoResize(in windInfo);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00021ECC File Offset: 0x000200CC
		public void RemoveWindZone(int windId)
		{
			int num = this.IndexOf(windId);
			if (num >= 0)
			{
				this.windZoneList.RemoveAtSwapBack(num);
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00021EF1 File Offset: 0x000200F1
		public void CopyFrom(in TeamWindData wdata)
		{
			this.windZoneList = wdata.windZoneList;
			this.movingWind = wdata.movingWind;
		}

		// Token: 0x0400070D RID: 1805
		public FixedList128Bytes<TeamWindInfo> windZoneList;

		// Token: 0x0400070E RID: 1806
		public TeamWindInfo movingWind;
	}
}
