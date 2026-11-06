using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000B1 RID: 177
	public class WindManager : IManager, IDisposable, IValid
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0001D954 File Offset: 0x0001BB54
		public int WindCount
		{
			get
			{
				ExNativeArray<WindManager.WindData> exNativeArray = this.windDataArray;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0001D967 File Offset: 0x0001BB67
		public void Dispose()
		{
			this.isValid = false;
			ExNativeArray<WindManager.WindData> exNativeArray = this.windDataArray;
			if (exNativeArray == null)
			{
				return;
			}
			exNativeArray.Dispose();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0001D980 File Offset: 0x0001BB80
		public void EnterdEditMode()
		{
			this.Dispose();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0001D988 File Offset: 0x0001BB88
		public void Initialize()
		{
			this.Dispose();
			this.windDataArray = new ExNativeArray<WindManager.WindData>(64, false);
			this.isValid = true;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0001D9A5 File Offset: 0x0001BBA5
		public bool IsValid()
		{
			return this.isValid;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001D9B0 File Offset: 0x0001BBB0
		public int AddWind(MagicaWindZone windZone)
		{
			if (!this.isValid || windZone == null)
			{
				return -1;
			}
			WindManager.WindData windData = default(WindManager.WindData);
			windData.flag.SetBits(0, true);
			DataChunk dataChunk = this.windDataArray.Add(windData);
			this.windZoneDict.Add(dataChunk.startIndex, windZone);
			return dataChunk.startIndex;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0001DA0C File Offset: 0x0001BC0C
		public void RemoveWind(int windId)
		{
			if (!this.isValid || windId < 0)
			{
				return;
			}
			DataChunk dataChunk = new DataChunk(windId);
			this.windDataArray.RemoveAndFill(dataChunk, default(WindManager.WindData));
			this.windZoneDict.Remove(dataChunk.startIndex);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0001DA58 File Offset: 0x0001BC58
		public void SetEnable(int windId, bool sw)
		{
			if (!this.isValid || windId < 0)
			{
				return;
			}
			WindManager.WindData windData = this.windDataArray[windId];
			windData.flag.SetBits(1, sw);
			this.windDataArray[windId] = windData;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0001DA9C File Offset: 0x0001BC9C
		internal void AlwaysWindUpdate()
		{
			foreach (KeyValuePair<int, MagicaWindZone> keyValuePair in this.windZoneDict)
			{
				int key = keyValuePair.Key;
				MagicaWindZone value = keyValuePair.Value;
				if (key >= 0 && !(value == null))
				{
					Transform transform = value.transform;
					ref WindManager.WindData @ref = ref this.windDataArray.GetRef(key);
					@ref.mode = value.mode;
					MagicaWindZone.Mode mode = value.mode;
					if (mode == MagicaWindZone.Mode.SphereDirection)
					{
						goto IL_008D;
					}
					if (mode != MagicaWindZone.Mode.BoxDirection)
					{
						if (mode == MagicaWindZone.Mode.SphereRadial)
						{
							goto IL_008D;
						}
					}
					else
					{
						@ref.size = value.size;
					}
					IL_009F:
					@ref.main = value.main;
					@ref.turbulence = value.turbulence;
					@ref.flag.SetBits(2, value.IsAddition());
					@ref.worldPositin = transform.position;
					@ref.worldRotation = transform.rotation;
					@ref.worldScale = transform.lossyScale;
					@ref.worldToLocalMatrix = transform.worldToLocalMatrix;
					float num = 0f;
					mode = @ref.mode;
					switch (mode)
					{
					case MagicaWindZone.Mode.GlobalDirection:
						num = float.MaxValue;
						break;
					case MagicaWindZone.Mode.SphereDirection:
						goto IL_017C;
					case MagicaWindZone.Mode.BoxDirection:
					{
						float3 @float = @ref.size * @ref.worldScale;
						num = @float.x * @float.y * @float.z;
						break;
					}
					default:
						if (mode == MagicaWindZone.Mode.SphereRadial)
						{
							goto IL_017C;
						}
						break;
					}
					IL_01AD:
					@ref.zoneVolume = num;
					if (value.IsDirection())
					{
						@ref.worldWindDirection = value.GetWindDirection(false);
						continue;
					}
					@ref.attenuation = DataUtility.ConvertAnimationCurve(value.attenuation);
					continue;
					IL_017C:
					float num2 = @ref.size.x * @ref.worldScale.x;
					num = 1.3333334f * num2 * num2 * num2 * 3.1415927f;
					goto IL_01AD;
					IL_008D:
					@ref.size = value.radius;
					goto IL_009F;
				}
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0001DCC8 File Offset: 0x0001BEC8
		public void InformationLog(StringBuilder allsb)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("========== Wind Manager ==========");
			if (!this.IsValid())
			{
				stringBuilder.AppendLine("Wind Manager. Invalid.");
			}
			else
			{
				stringBuilder.AppendLine(string.Format("Wind Manager. Count:{0}", this.WindCount));
				int windCount = this.WindCount;
				for (int i = 0; i < windCount; i++)
				{
					WindManager.WindData windData = this.windDataArray[i];
					if (windData.flag.IsSet(0))
					{
						stringBuilder.AppendLine(string.Format("  [{0}] flag:0x{1:X}, mode:{2}", i, windData.flag.Value, windData.mode));
					}
				}
			}
			stringBuilder.AppendLine();
			Debug.Log(stringBuilder.ToString());
			allsb.Append(stringBuilder);
		}

		// Token: 0x0400063A RID: 1594
		public const int Flag_Valid = 0;

		// Token: 0x0400063B RID: 1595
		public const int Flag_Enable = 1;

		// Token: 0x0400063C RID: 1596
		public const int Flag_Addition = 2;

		// Token: 0x0400063D RID: 1597
		public ExNativeArray<WindManager.WindData> windDataArray;

		// Token: 0x0400063E RID: 1598
		private bool isValid;

		// Token: 0x0400063F RID: 1599
		private Dictionary<int, MagicaWindZone> windZoneDict = new Dictionary<int, MagicaWindZone>();

		// Token: 0x020000B2 RID: 178
		public struct WindData
		{
			// Token: 0x060002E8 RID: 744 RVA: 0x0001DDA7 File Offset: 0x0001BFA7
			public bool IsValid()
			{
				return this.flag.IsSet(0);
			}

			// Token: 0x060002E9 RID: 745 RVA: 0x0001DDB5 File Offset: 0x0001BFB5
			public bool IsEnable()
			{
				return this.flag.IsSet(1);
			}

			// Token: 0x060002EA RID: 746 RVA: 0x0001DDC3 File Offset: 0x0001BFC3
			public bool IsAddition()
			{
				return this.flag.IsSet(2);
			}

			// Token: 0x04000640 RID: 1600
			public BitField32 flag;

			// Token: 0x04000641 RID: 1601
			public MagicaWindZone.Mode mode;

			// Token: 0x04000642 RID: 1602
			public float3 size;

			// Token: 0x04000643 RID: 1603
			public float main;

			// Token: 0x04000644 RID: 1604
			public float turbulence;

			// Token: 0x04000645 RID: 1605
			public float zoneVolume;

			// Token: 0x04000646 RID: 1606
			public float3 worldWindDirection;

			// Token: 0x04000647 RID: 1607
			public float3 worldPositin;

			// Token: 0x04000648 RID: 1608
			public quaternion worldRotation;

			// Token: 0x04000649 RID: 1609
			public float3 worldScale;

			// Token: 0x0400064A RID: 1610
			public float4x4 worldToLocalMatrix;

			// Token: 0x0400064B RID: 1611
			public float4x4 attenuation;
		}
	}
}
