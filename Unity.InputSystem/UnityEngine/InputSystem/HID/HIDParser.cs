using System;
using System.Collections.Generic;

namespace UnityEngine.InputSystem.HID
{
	// Token: 0x0200009B RID: 155
	internal static class HIDParser
	{
		// Token: 0x06000C26 RID: 3110 RVA: 0x00041114 File Offset: 0x0003F314
		public unsafe static bool ParseReportDescriptor(byte[] buffer, ref HID.HIDDeviceDescriptor deviceDescriptor)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			byte* ptr;
			if (buffer == null || buffer.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &buffer[0];
			}
			return HIDParser.ParseReportDescriptor(ptr, buffer.Length, ref deviceDescriptor);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00041154 File Offset: 0x0003F354
		public unsafe static bool ParseReportDescriptor(byte* bufferPtr, int bufferLength, ref HID.HIDDeviceDescriptor deviceDescriptor)
		{
			HIDParser.HIDItemStateLocal hiditemStateLocal = default(HIDParser.HIDItemStateLocal);
			HIDParser.HIDItemStateGlobal hiditemStateGlobal = default(HIDParser.HIDItemStateGlobal);
			List<HIDParser.HIDReportData> list = new List<HIDParser.HIDReportData>();
			List<HID.HIDElementDescriptor> list2 = new List<HID.HIDElementDescriptor>();
			List<HID.HIDCollectionDescriptor> list3 = new List<HID.HIDCollectionDescriptor>();
			int num = -1;
			byte* ptr = bufferPtr + bufferLength;
			byte* ptr2 = bufferPtr;
			while (ptr2 < ptr)
			{
				byte b = *ptr2;
				if (b == 254)
				{
					throw new NotImplementedException("long item support");
				}
				byte b2 = b & 3;
				byte b3 = b & 252;
				ptr2++;
				if (b3 <= 84)
				{
					if (b3 <= 24)
					{
						if (b3 <= 8)
						{
							if (b3 != 4)
							{
								if (b3 == 8)
								{
									hiditemStateLocal.SetUsage(HIDParser.ReadData((int)b2, ptr2, ptr));
								}
							}
							else
							{
								hiditemStateGlobal.usagePage = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
							}
						}
						else if (b3 != 20)
						{
							if (b3 == 24)
							{
								hiditemStateLocal.usageMinimum = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
							}
						}
						else
						{
							hiditemStateGlobal.logicalMinimum = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
						}
					}
					else if (b3 <= 40)
					{
						if (b3 != 36)
						{
							if (b3 == 40)
							{
								hiditemStateLocal.usageMaximum = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
							}
						}
						else
						{
							hiditemStateGlobal.logicalMaximum = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
						}
					}
					else if (b3 != 52)
					{
						if (b3 != 68)
						{
							if (b3 == 84)
							{
								hiditemStateGlobal.unitExponent = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
							}
						}
						else
						{
							hiditemStateGlobal.physicalMaximum = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
						}
					}
					else
					{
						hiditemStateGlobal.physicalMinimum = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
					}
				}
				else
				{
					if (b3 <= 132)
					{
						if (b3 <= 116)
						{
							if (b3 == 100)
							{
								hiditemStateGlobal.unit = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
								goto IL_0521;
							}
							if (b3 != 116)
							{
								goto IL_0521;
							}
							hiditemStateGlobal.reportSize = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
							goto IL_0521;
						}
						else if (b3 != 128)
						{
							if (b3 != 132)
							{
								goto IL_0521;
							}
							hiditemStateGlobal.reportId = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
							goto IL_0521;
						}
					}
					else if (b3 <= 148)
					{
						if (b3 != 144)
						{
							if (b3 != 148)
							{
								goto IL_0521;
							}
							hiditemStateGlobal.reportCount = new int?(HIDParser.ReadData((int)b2, ptr2, ptr));
							goto IL_0521;
						}
					}
					else
					{
						if (b3 == 160)
						{
							int num2 = num;
							num = list3.Count;
							list3.Add(new HID.HIDCollectionDescriptor
							{
								type = (HID.HIDCollectionType)HIDParser.ReadData((int)b2, ptr2, ptr),
								parent = num2,
								usagePage = hiditemStateGlobal.GetUsagePage(0, ref hiditemStateLocal),
								usage = hiditemStateLocal.GetUsage(0),
								firstChild = list2.Count
							});
							HIDParser.HIDItemStateLocal.Reset(ref hiditemStateLocal);
							goto IL_0521;
						}
						if (b3 != 176)
						{
							if (b3 != 192)
							{
								goto IL_0521;
							}
							if (num == -1)
							{
								return false;
							}
							HID.HIDCollectionDescriptor hidcollectionDescriptor = list3[num];
							hidcollectionDescriptor.childCount = list2.Count - hidcollectionDescriptor.firstChild;
							list3[num] = hidcollectionDescriptor;
							num = hidcollectionDescriptor.parent;
							HIDParser.HIDItemStateLocal.Reset(ref hiditemStateLocal);
							goto IL_0521;
						}
					}
					HID.HIDReportType hidreportType = ((b3 == 128) ? HID.HIDReportType.Input : ((b3 == 144) ? HID.HIDReportType.Output : HID.HIDReportType.Feature));
					int num3 = HIDParser.HIDReportData.FindOrAddReport(hiditemStateGlobal.reportId, hidreportType, list);
					HIDParser.HIDReportData hidreportData = list[num3];
					if (hidreportData.currentBitOffset == 0 && hiditemStateGlobal.reportId != null)
					{
						hidreportData.currentBitOffset = 8;
					}
					int valueOrDefault = hiditemStateGlobal.reportCount.GetValueOrDefault(1);
					int num4 = HIDParser.ReadData((int)b2, ptr2, ptr);
					for (int i = 0; i < valueOrDefault; i++)
					{
						HID.HIDElementDescriptor hidelementDescriptor = new HID.HIDElementDescriptor
						{
							usage = (hiditemStateLocal.GetUsage(i) & 65535),
							usagePage = hiditemStateGlobal.GetUsagePage(i, ref hiditemStateLocal),
							reportType = hidreportType,
							reportSizeInBits = hiditemStateGlobal.reportSize.GetValueOrDefault(8),
							reportOffsetInBits = hidreportData.currentBitOffset,
							reportId = hiditemStateGlobal.reportId.GetValueOrDefault(1),
							flags = (HID.HIDElementFlags)num4,
							logicalMin = hiditemStateGlobal.logicalMinimum.GetValueOrDefault(0),
							logicalMax = hiditemStateGlobal.logicalMaximum.GetValueOrDefault(0),
							physicalMin = hiditemStateGlobal.GetPhysicalMin(),
							physicalMax = hiditemStateGlobal.GetPhysicalMax(),
							unitExponent = hiditemStateGlobal.unitExponent.GetValueOrDefault(0),
							unit = hiditemStateGlobal.unit.GetValueOrDefault(0)
						};
						hidreportData.currentBitOffset += hidelementDescriptor.reportSizeInBits;
						list2.Add(hidelementDescriptor);
					}
					list[num3] = hidreportData;
					HIDParser.HIDItemStateLocal.Reset(ref hiditemStateLocal);
				}
				IL_0521:
				if (b2 == 3)
				{
					ptr2 += 4;
				}
				else
				{
					ptr2 += b2;
				}
			}
			deviceDescriptor.elements = list2.ToArray();
			deviceDescriptor.collections = list3.ToArray();
			foreach (HID.HIDCollectionDescriptor hidcollectionDescriptor2 in list3)
			{
				if (hidcollectionDescriptor2.parent == -1 && hidcollectionDescriptor2.type == HID.HIDCollectionType.Application)
				{
					deviceDescriptor.usage = hidcollectionDescriptor2.usage;
					deviceDescriptor.usagePage = hidcollectionDescriptor2.usagePage;
					break;
				}
			}
			return true;
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00041728 File Offset: 0x0003F928
		private unsafe static int ReadData(int itemSize, byte* currentPtr, byte* endPtr)
		{
			if (itemSize == 0)
			{
				return 0;
			}
			if (itemSize == 1)
			{
				if (currentPtr >= endPtr)
				{
					return 0;
				}
				return (int)(*currentPtr);
			}
			else if (itemSize == 2)
			{
				if (currentPtr + 2 >= endPtr)
				{
					return 0;
				}
				byte b = *currentPtr;
				return ((int)currentPtr[1] << 8) | (int)b;
			}
			else
			{
				if (itemSize != 3)
				{
					return 0;
				}
				if (currentPtr + 4 >= endPtr)
				{
					return 0;
				}
				byte b2 = *currentPtr;
				byte b3 = currentPtr[1];
				byte b4 = currentPtr[2];
				return ((int)currentPtr[3] << 24) | ((int)b4 << 24) | ((int)b3 << 8) | (int)b2;
			}
		}

		// Token: 0x020001F3 RID: 499
		private struct HIDReportData
		{
			// Token: 0x060014DB RID: 5339 RVA: 0x00061748 File Offset: 0x0005F948
			public static int FindOrAddReport(int? reportId, HID.HIDReportType reportType, List<HIDParser.HIDReportData> reports)
			{
				int num = 1;
				if (reportId != null)
				{
					num = reportId.Value;
				}
				for (int i = 0; i < reports.Count; i++)
				{
					if (reports[i].reportId == num && reports[i].reportType == reportType)
					{
						return i;
					}
				}
				reports.Add(new HIDParser.HIDReportData
				{
					reportId = num,
					reportType = reportType
				});
				return reports.Count - 1;
			}

			// Token: 0x04000AFE RID: 2814
			public int reportId;

			// Token: 0x04000AFF RID: 2815
			public HID.HIDReportType reportType;

			// Token: 0x04000B00 RID: 2816
			public int currentBitOffset;
		}

		// Token: 0x020001F4 RID: 500
		private enum HIDItemTypeAndTag
		{
			// Token: 0x04000B02 RID: 2818
			Input = 128,
			// Token: 0x04000B03 RID: 2819
			Output = 144,
			// Token: 0x04000B04 RID: 2820
			Feature = 176,
			// Token: 0x04000B05 RID: 2821
			Collection = 160,
			// Token: 0x04000B06 RID: 2822
			EndCollection = 192,
			// Token: 0x04000B07 RID: 2823
			UsagePage = 4,
			// Token: 0x04000B08 RID: 2824
			LogicalMinimum = 20,
			// Token: 0x04000B09 RID: 2825
			LogicalMaximum = 36,
			// Token: 0x04000B0A RID: 2826
			PhysicalMinimum = 52,
			// Token: 0x04000B0B RID: 2827
			PhysicalMaximum = 68,
			// Token: 0x04000B0C RID: 2828
			UnitExponent = 84,
			// Token: 0x04000B0D RID: 2829
			Unit = 100,
			// Token: 0x04000B0E RID: 2830
			ReportSize = 116,
			// Token: 0x04000B0F RID: 2831
			ReportID = 132,
			// Token: 0x04000B10 RID: 2832
			ReportCount = 148,
			// Token: 0x04000B11 RID: 2833
			Push = 164,
			// Token: 0x04000B12 RID: 2834
			Pop = 180,
			// Token: 0x04000B13 RID: 2835
			Usage = 8,
			// Token: 0x04000B14 RID: 2836
			UsageMinimum = 24,
			// Token: 0x04000B15 RID: 2837
			UsageMaximum = 40,
			// Token: 0x04000B16 RID: 2838
			DesignatorIndex = 56,
			// Token: 0x04000B17 RID: 2839
			DesignatorMinimum = 72,
			// Token: 0x04000B18 RID: 2840
			DesignatorMaximum = 88,
			// Token: 0x04000B19 RID: 2841
			StringIndex = 120,
			// Token: 0x04000B1A RID: 2842
			StringMinimum = 136,
			// Token: 0x04000B1B RID: 2843
			StringMaximum = 152,
			// Token: 0x04000B1C RID: 2844
			Delimiter = 168
		}

		// Token: 0x020001F5 RID: 501
		private struct HIDItemStateLocal
		{
			// Token: 0x060014DC RID: 5340 RVA: 0x000617C0 File Offset: 0x0005F9C0
			public static void Reset(ref HIDParser.HIDItemStateLocal state)
			{
				List<int> list = state.usageList;
				state = default(HIDParser.HIDItemStateLocal);
				if (list != null)
				{
					list.Clear();
					state.usageList = list;
				}
			}

			// Token: 0x060014DD RID: 5341 RVA: 0x000617EC File Offset: 0x0005F9EC
			public void SetUsage(int value)
			{
				if (this.usage != null)
				{
					if (this.usageList == null)
					{
						this.usageList = new List<int>();
					}
					this.usageList.Add(this.usage.Value);
				}
				this.usage = new int?(value);
			}

			// Token: 0x060014DE RID: 5342 RVA: 0x0006183C File Offset: 0x0005FA3C
			public int GetUsage(int index)
			{
				if (this.usageMinimum != null && this.usageMaximum != null)
				{
					int value = this.usageMinimum.Value;
					int value2 = this.usageMaximum.Value;
					int num = value2 - value;
					if (num < 0)
					{
						return 0;
					}
					if (index >= num)
					{
						return value2;
					}
					return value + index;
				}
				else if (this.usageList != null && this.usageList.Count > 0)
				{
					int count = this.usageList.Count;
					if (index >= count)
					{
						return this.usage.Value;
					}
					return this.usageList[index];
				}
				else
				{
					if (this.usage != null)
					{
						return this.usage.Value;
					}
					return 0;
				}
			}

			// Token: 0x04000B1D RID: 2845
			public int? usage;

			// Token: 0x04000B1E RID: 2846
			public int? usageMinimum;

			// Token: 0x04000B1F RID: 2847
			public int? usageMaximum;

			// Token: 0x04000B20 RID: 2848
			public int? designatorIndex;

			// Token: 0x04000B21 RID: 2849
			public int? designatorMinimum;

			// Token: 0x04000B22 RID: 2850
			public int? designatorMaximum;

			// Token: 0x04000B23 RID: 2851
			public int? stringIndex;

			// Token: 0x04000B24 RID: 2852
			public int? stringMinimum;

			// Token: 0x04000B25 RID: 2853
			public int? stringMaximum;

			// Token: 0x04000B26 RID: 2854
			public List<int> usageList;
		}

		// Token: 0x020001F6 RID: 502
		private struct HIDItemStateGlobal
		{
			// Token: 0x060014DF RID: 5343 RVA: 0x000618E8 File Offset: 0x0005FAE8
			public HID.UsagePage GetUsagePage(int index, ref HIDParser.HIDItemStateLocal localItemState)
			{
				if (this.usagePage == null)
				{
					return (HID.UsagePage)(localItemState.GetUsage(index) >> 16);
				}
				return (HID.UsagePage)this.usagePage.Value;
			}

			// Token: 0x060014E0 RID: 5344 RVA: 0x00061910 File Offset: 0x0005FB10
			public int GetPhysicalMin()
			{
				if (this.physicalMinimum == null || this.physicalMaximum == null || (this.physicalMinimum.Value == 0 && this.physicalMaximum.Value == 0))
				{
					return this.logicalMinimum.GetValueOrDefault(0);
				}
				return this.physicalMinimum.Value;
			}

			// Token: 0x060014E1 RID: 5345 RVA: 0x0006196C File Offset: 0x0005FB6C
			public int GetPhysicalMax()
			{
				if (this.physicalMinimum == null || this.physicalMaximum == null || (this.physicalMinimum.Value == 0 && this.physicalMaximum.Value == 0))
				{
					return this.logicalMaximum.GetValueOrDefault(0);
				}
				return this.physicalMaximum.Value;
			}

			// Token: 0x04000B27 RID: 2855
			public int? usagePage;

			// Token: 0x04000B28 RID: 2856
			public int? logicalMinimum;

			// Token: 0x04000B29 RID: 2857
			public int? logicalMaximum;

			// Token: 0x04000B2A RID: 2858
			public int? physicalMinimum;

			// Token: 0x04000B2B RID: 2859
			public int? physicalMaximum;

			// Token: 0x04000B2C RID: 2860
			public int? unitExponent;

			// Token: 0x04000B2D RID: 2861
			public int? unit;

			// Token: 0x04000B2E RID: 2862
			public int? reportSize;

			// Token: 0x04000B2F RID: 2863
			public int? reportCount;

			// Token: 0x04000B30 RID: 2864
			public int? reportId;
		}
	}
}
