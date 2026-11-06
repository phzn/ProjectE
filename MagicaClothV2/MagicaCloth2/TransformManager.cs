using System;
using System.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace MagicaCloth2
{
	// Token: 0x020000C0 RID: 192
	public class TransformManager : IManager, IDisposable, IValid
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000360 RID: 864 RVA: 0x000236AF File Offset: 0x000218AF
		internal int Count
		{
			get
			{
				ExNativeArray<ExBitFlag8> exNativeArray = this.flagArray;
				if (exNativeArray == null)
				{
					return 0;
				}
				return exNativeArray.Count;
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000236C4 File Offset: 0x000218C4
		public void Dispose()
		{
			this.isValid = false;
			ExNativeArray<ExBitFlag8> exNativeArray = this.flagArray;
			if (exNativeArray != null)
			{
				exNativeArray.Dispose();
			}
			ExNativeArray<float3> exNativeArray2 = this.initLocalPositionArray;
			if (exNativeArray2 != null)
			{
				exNativeArray2.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray3 = this.initLocalRotationArray;
			if (exNativeArray3 != null)
			{
				exNativeArray3.Dispose();
			}
			ExNativeArray<float3> exNativeArray4 = this.baseLocalPositionArray;
			if (exNativeArray4 != null)
			{
				exNativeArray4.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray5 = this.baseLocalRotationArray;
			if (exNativeArray5 != null)
			{
				exNativeArray5.Dispose();
			}
			ExNativeArray<float3> exNativeArray6 = this.positionArray;
			if (exNativeArray6 != null)
			{
				exNativeArray6.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray7 = this.rotationArray;
			if (exNativeArray7 != null)
			{
				exNativeArray7.Dispose();
			}
			ExNativeArray<float3> exNativeArray8 = this.scaleArray;
			if (exNativeArray8 != null)
			{
				exNativeArray8.Dispose();
			}
			ExNativeArray<float3> exNativeArray9 = this.localPositionArray;
			if (exNativeArray9 != null)
			{
				exNativeArray9.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray10 = this.localRotationArray;
			if (exNativeArray10 != null)
			{
				exNativeArray10.Dispose();
			}
			ExNativeArray<float3> exNativeArray11 = this.localScaleArray;
			if (exNativeArray11 != null)
			{
				exNativeArray11.Dispose();
			}
			ExNativeArray<float4x4> exNativeArray12 = this.localToWorldMatrixArray;
			if (exNativeArray12 != null)
			{
				exNativeArray12.Dispose();
			}
			ExNativeArray<short> exNativeArray13 = this.teamIdArray;
			if (exNativeArray13 != null)
			{
				exNativeArray13.Dispose();
			}
			this.flagArray = null;
			this.initLocalPositionArray = null;
			this.initLocalRotationArray = null;
			this.baseLocalPositionArray = null;
			this.baseLocalRotationArray = null;
			this.positionArray = null;
			this.rotationArray = null;
			this.scaleArray = null;
			this.localPositionArray = null;
			this.localRotationArray = null;
			this.localScaleArray = null;
			this.localToWorldMatrixArray = null;
			this.teamIdArray = null;
			if (this.transformAccessArray.isCreated)
			{
				this.transformAccessArray.Dispose();
			}
			ExNativeArray<float3> exNativeArray14 = this.componentPositionArray;
			if (exNativeArray14 != null)
			{
				exNativeArray14.Dispose();
			}
			ExNativeArray<float> exNativeArray15 = this.componentMinScaleArray;
			if (exNativeArray15 != null)
			{
				exNativeArray15.Dispose();
			}
			if (this.componentTransformAccessArray.isCreated)
			{
				this.componentTransformAccessArray.Dispose();
			}
			if (this.existFixedTeam.IsCreated)
			{
				this.existFixedTeam.Dispose();
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0002387A File Offset: 0x00021A7A
		public void EnterdEditMode()
		{
			this.Dispose();
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00023884 File Offset: 0x00021A84
		public void Initialize()
		{
			this.Dispose();
			this.flagArray = new ExNativeArray<ExBitFlag8>(256, false);
			this.initLocalPositionArray = new ExNativeArray<float3>(256, false);
			this.initLocalRotationArray = new ExNativeArray<quaternion>(256, false);
			this.baseLocalPositionArray = new ExNativeArray<float3>(256, false);
			this.baseLocalRotationArray = new ExNativeArray<quaternion>(256, false);
			this.positionArray = new ExNativeArray<float3>(256, false);
			this.rotationArray = new ExNativeArray<quaternion>(256, false);
			this.scaleArray = new ExNativeArray<float3>(256, false);
			this.localPositionArray = new ExNativeArray<float3>(256, false);
			this.localRotationArray = new ExNativeArray<quaternion>(256, false);
			this.localScaleArray = new ExNativeArray<float3>(256, false);
			this.localToWorldMatrixArray = new ExNativeArray<float4x4>(256, false);
			this.teamIdArray = new ExNativeArray<short>(256, false);
			this.transformAccessArray = new TransformAccessArray(256, -1);
			this.componentPositionArray = new ExNativeArray<float3>(256, false);
			this.componentMinScaleArray = new ExNativeArray<float>(256, false);
			this.componentTransformAccessArray = new TransformAccessArray(256, -1);
			this.existFixedTeam = new NativeReference<bool>(Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.isValid = true;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000239D1 File Offset: 0x00021BD1
		public bool IsValid()
		{
			return this.isValid;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000239DC File Offset: 0x00021BDC
		internal DataChunk AddTransform(VirtualMeshContainer cmesh, int teamId)
		{
			if (!this.isValid)
			{
				return default(DataChunk);
			}
			int transformCount = cmesh.GetTransformCount();
			DataChunk dataChunk = this.flagArray.AddRange(cmesh.shareVirtualMesh.transformData.flagArray);
			this.initLocalPositionArray.AddRange(cmesh.shareVirtualMesh.transformData.initLocalPositionArray);
			this.initLocalRotationArray.AddRange(cmesh.shareVirtualMesh.transformData.initLocalRotationArray);
			this.baseLocalPositionArray.AddRange(cmesh.shareVirtualMesh.transformData.initLocalPositionArray);
			this.baseLocalRotationArray.AddRange(cmesh.shareVirtualMesh.transformData.initLocalRotationArray);
			this.positionArray.AddRange(transformCount);
			this.rotationArray.AddRange(transformCount);
			this.scaleArray.AddRange(transformCount);
			this.localPositionArray.AddRange(transformCount);
			this.localRotationArray.AddRange(transformCount);
			this.localScaleArray.AddRange(transformCount);
			this.localToWorldMatrixArray.AddRange(transformCount);
			this.teamIdArray.AddRange(transformCount, (short)teamId);
			int i = this.transformAccessArray.length;
			Transform centerTransform = cmesh.GetCenterTransform();
			int startIndex = dataChunk.startIndex;
			while (i < startIndex)
			{
				this.transformAccessArray.Add(centerTransform);
				i++;
			}
			for (int j = 0; j < transformCount; j++)
			{
				Transform transform = cmesh.GetTransformFromIndex(j);
				if (transform == null)
				{
					transform = centerTransform;
				}
				int num = dataChunk.startIndex + j;
				if (num < i)
				{
					this.transformAccessArray[num] = transform;
				}
				else
				{
					this.transformAccessArray.Add(transform);
				}
			}
			return dataChunk;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00023B84 File Offset: 0x00021D84
		internal DataChunk AddTransform(int count, int teamId, Transform t)
		{
			if (!this.isValid)
			{
				return default(DataChunk);
			}
			DataChunk dataChunk = this.flagArray.AddRange(count);
			this.initLocalPositionArray.AddRange(count);
			this.initLocalRotationArray.AddRange(count);
			this.baseLocalPositionArray.AddRange(count);
			this.baseLocalRotationArray.AddRange(count);
			this.positionArray.AddRange(count);
			this.rotationArray.AddRange(count);
			this.scaleArray.AddRange(count);
			this.localPositionArray.AddRange(count);
			this.localRotationArray.AddRange(count);
			this.localScaleArray.AddRange(count);
			this.localToWorldMatrixArray.AddRange(count);
			this.teamIdArray.AddRange(count, (short)teamId);
			int i = this.transformAccessArray.length;
			int startIndex = dataChunk.startIndex;
			while (i < startIndex)
			{
				this.transformAccessArray.Add(t);
				i++;
			}
			for (int j = 0; j < count; j++)
			{
				int num = dataChunk.startIndex + j;
				if (num < i)
				{
					this.transformAccessArray[num] = t;
				}
				else
				{
					this.transformAccessArray.Add(t);
				}
			}
			return dataChunk;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00023CB4 File Offset: 0x00021EB4
		internal DataChunk AddTransform(Transform t, ExBitFlag8 flag, int teamId)
		{
			if (!this.isValid)
			{
				return default(DataChunk);
			}
			DataChunk dataChunk = this.flagArray.Add(flag);
			this.initLocalPositionArray.Add(t.localPosition);
			this.initLocalRotationArray.Add(t.localRotation);
			this.baseLocalPositionArray.Add(t.localPosition);
			this.baseLocalRotationArray.Add(t.localRotation);
			this.positionArray.Add(t.position);
			this.rotationArray.Add(t.rotation);
			this.scaleArray.Add(t.lossyScale);
			this.localPositionArray.Add(t.localPosition);
			this.localRotationArray.Add(t.localRotation);
			this.localScaleArray.Add(t.localScale);
			this.localToWorldMatrixArray.Add(float4x4.identity);
			this.teamIdArray.Add((short)teamId);
			int length = this.transformAccessArray.length;
			int startIndex = dataChunk.startIndex;
			if (startIndex < length)
			{
				this.transformAccessArray[startIndex] = t;
				return dataChunk;
			}
			this.transformAccessArray.Add(t);
			return dataChunk;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00023E18 File Offset: 0x00022018
		internal void SetTransform(Transform t, ExBitFlag8 flag, int index, int teamId)
		{
			if (!this.isValid)
			{
				return;
			}
			if (t != null)
			{
				this.flagArray[index] = flag;
				this.initLocalPositionArray[index] = t.localPosition;
				this.initLocalRotationArray[index] = t.localRotation;
				this.baseLocalPositionArray[index] = t.localPosition;
				this.baseLocalRotationArray[index] = t.localRotation;
				this.positionArray[index] = t.position;
				this.rotationArray[index] = t.rotation;
				this.scaleArray[index] = t.lossyScale;
				this.localPositionArray[index] = t.localPosition;
				this.localRotationArray[index] = t.localRotation;
				this.localScaleArray[index] = t.localScale;
				this.teamIdArray[index] = (short)teamId;
				this.transformAccessArray[index] = t;
				return;
			}
			this.flagArray[index] = default(ExBitFlag8);
			this.transformAccessArray[index] = null;
			this.teamIdArray[index] = 0;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00023F7C File Offset: 0x0002217C
		internal void CopyTransform(int fromIndex, int toIndex)
		{
			if (!this.isValid)
			{
				return;
			}
			this.flagArray[toIndex] = this.flagArray[fromIndex];
			this.initLocalPositionArray[toIndex] = this.initLocalPositionArray[fromIndex];
			this.initLocalRotationArray[toIndex] = this.initLocalRotationArray[fromIndex];
			this.baseLocalPositionArray[toIndex] = this.baseLocalPositionArray[fromIndex];
			this.baseLocalRotationArray[toIndex] = this.baseLocalRotationArray[fromIndex];
			this.positionArray[toIndex] = this.positionArray[fromIndex];
			this.rotationArray[toIndex] = this.rotationArray[fromIndex];
			this.scaleArray[toIndex] = this.scaleArray[fromIndex];
			this.localPositionArray[toIndex] = this.localPositionArray[fromIndex];
			this.localRotationArray[toIndex] = this.localRotationArray[fromIndex];
			this.localScaleArray[toIndex] = this.localScaleArray[fromIndex];
			this.transformAccessArray[toIndex] = this.transformAccessArray[fromIndex];
			this.teamIdArray[toIndex] = this.teamIdArray[fromIndex];
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000240CC File Offset: 0x000222CC
		internal void RemoveTransform(DataChunk c)
		{
			if (!this.isValid)
			{
				return;
			}
			if (!c.IsValid)
			{
				return;
			}
			this.flagArray.RemoveAndFill(c, default(ExBitFlag8));
			this.initLocalPositionArray.Remove(c);
			this.initLocalRotationArray.Remove(c);
			this.baseLocalPositionArray.Remove(c);
			this.baseLocalRotationArray.Remove(c);
			this.positionArray.Remove(c);
			this.rotationArray.Remove(c);
			this.scaleArray.Remove(c);
			this.localPositionArray.Remove(c);
			this.localRotationArray.Remove(c);
			this.localScaleArray.Remove(c);
			this.localToWorldMatrixArray.Remove(c);
			this.teamIdArray.RemoveAndFill(c, 0);
			for (int i = 0; i < c.dataLength; i++)
			{
				int num = c.startIndex + i;
				this.transformAccessArray[num] = null;
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x000241BC File Offset: 0x000223BC
		internal void EnableTransform(DataChunk c, bool sw)
		{
			if (!this.isValid)
			{
				return;
			}
			if (!c.IsValid)
			{
				return;
			}
			new TransformManager.EnableTransformJob
			{
				chunk = c,
				sw = sw,
				flagList = this.flagArray.GetNativeArray()
			}.Run<TransformManager.EnableTransformJob>();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0002420C File Offset: 0x0002240C
		internal void EnableTransform(int index, bool sw)
		{
			if (!this.isValid)
			{
				return;
			}
			if (index < 0)
			{
				return;
			}
			ExBitFlag8 exBitFlag = this.flagArray[index];
			if (exBitFlag.Value == 0)
			{
				return;
			}
			exBitFlag.SetFlag(16, sw);
			this.flagArray[index] = exBitFlag;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00024254 File Offset: 0x00022454
		internal DataChunk Expand(DataChunk c, int newLength)
		{
			if (!this.isValid)
			{
				return default(DataChunk);
			}
			DataChunk dataChunk = this.flagArray.Expand(c, newLength);
			this.initLocalPositionArray.Expand(c, newLength);
			this.initLocalRotationArray.Expand(c, newLength);
			this.baseLocalPositionArray.Expand(c, newLength);
			this.baseLocalRotationArray.Expand(c, newLength);
			this.positionArray.Expand(c, newLength);
			this.rotationArray.Expand(c, newLength);
			this.scaleArray.Expand(c, newLength);
			this.localPositionArray.Expand(c, newLength);
			this.localRotationArray.Expand(c, newLength);
			this.localScaleArray.Expand(c, newLength);
			this.localToWorldMatrixArray.Expand(c, newLength);
			this.teamIdArray.Expand(c, newLength);
			if (c.startIndex != dataChunk.startIndex)
			{
				Transform transform = this.transformAccessArray[c.startIndex];
				while (this.transformAccessArray.length < dataChunk.startIndex + dataChunk.dataLength)
				{
					this.transformAccessArray.Add(transform);
				}
				for (int i = 0; i < c.dataLength; i++)
				{
					Transform transform2 = this.transformAccessArray[c.startIndex + i];
					this.transformAccessArray[dataChunk.startIndex + i] = transform2;
					this.transformAccessArray[c.startIndex + i] = null;
				}
			}
			return dataChunk;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000243C4 File Offset: 0x000225C4
		public JobHandle RestoreTransform(JobHandle jobHandle)
		{
			this.existFixedTeam.Value = false;
			if (this.Count > 0)
			{
				jobHandle = new TransformManager.RestoreTransformJob
				{
					existFixedTeam = this.existFixedTeam,
					flagList = this.flagArray.GetNativeArray(),
					localPositionArray = this.initLocalPositionArray.GetNativeArray(),
					localRotationArray = this.initLocalRotationArray.GetNativeArray(),
					teamIdArray = this.teamIdArray.GetNativeArray(),
					teamDataArray = MagicaManager.Team.teamDataArray.GetNativeArray()
				}.Schedule(this.transformAccessArray, jobHandle);
			}
			return jobHandle;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0002446C File Offset: 0x0002266C
		public JobHandle RestoreBaseTransform(JobHandle jobHandle)
		{
			if (this.Count > 0)
			{
				jobHandle = new TransformManager.RestoreBaseTransformJob
				{
					flagList = this.flagArray.GetNativeArray(),
					baseLocalPositionArray = this.baseLocalPositionArray.GetNativeArray(),
					baseLocalRotationArray = this.baseLocalRotationArray.GetNativeArray(),
					teamIdArray = this.teamIdArray.GetNativeArray(),
					teamDataArray = MagicaManager.Team.teamDataArray.GetNativeArray()
				}.Schedule(this.transformAccessArray, jobHandle);
			}
			return jobHandle;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000244F8 File Offset: 0x000226F8
		public JobHandle ReadTransformSchedule(JobHandle jobHandle)
		{
			if (this.Count > 0)
			{
				jobHandle = new TransformManager.ReadTransformJob
				{
					fixedUpdateCount = MagicaManager.Time.FixedUpdateCount,
					flagList = this.flagArray.GetNativeArray(),
					positionArray = this.positionArray.GetNativeArray(),
					rotationArray = this.rotationArray.GetNativeArray(),
					scaleList = this.scaleArray.GetNativeArray(),
					localPositionArray = this.localPositionArray.GetNativeArray(),
					localRotationArray = this.localRotationArray.GetNativeArray(),
					localScaleArray = this.localScaleArray.GetNativeArray(),
					localToWorldMatrixArray = this.localToWorldMatrixArray.GetNativeArray(),
					baseLocalPositionArray = this.baseLocalPositionArray.GetNativeArray(),
					baseLocalRotationArray = this.baseLocalRotationArray.GetNativeArray(),
					teamIdArray = this.teamIdArray.GetNativeArray(),
					teamDataArray = MagicaManager.Team.teamDataArray.GetNativeArray()
				}.ScheduleReadOnly(this.transformAccessArray, 8, jobHandle);
			}
			return jobHandle;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00024618 File Offset: 0x00022818
		public JobHandle WriteTransformSchedule(JobHandle jobHandle)
		{
			jobHandle = new TransformManager.WriteTransformJob
			{
				flagList = this.flagArray.GetNativeArray(),
				worldPositions = this.positionArray.GetNativeArray(),
				worldRotations = this.rotationArray.GetNativeArray(),
				localPositions = this.localPositionArray.GetNativeArray(),
				localRotations = this.localRotationArray.GetNativeArray(),
				teamIdArray = this.teamIdArray.GetNativeArray(),
				teamDataArray = MagicaManager.Team.teamDataArray.GetNativeArray()
			}.Schedule(this.transformAccessArray, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x000246C0 File Offset: 0x000228C0
		internal int AddComponentTransform(Transform t)
		{
			if (!this.isValid)
			{
				return -1;
			}
			int startIndex = this.componentPositionArray.Add(float3.zero).startIndex;
			this.componentMinScaleArray.Add(1f);
			int length = this.componentTransformAccessArray.length;
			if (startIndex < length)
			{
				this.componentTransformAccessArray[startIndex] = t;
			}
			else
			{
				this.componentTransformAccessArray.Add(t);
			}
			return startIndex;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0002472A File Offset: 0x0002292A
		internal void RemoveComponentTransform(int index)
		{
			if (!this.isValid)
			{
				return;
			}
			if (index < 0)
			{
				return;
			}
			this.componentPositionArray.Remove(index);
			this.componentMinScaleArray.Remove(index);
			this.componentTransformAccessArray[index] = null;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00024760 File Offset: 0x00022960
		internal JobHandle ReadComponentTransform(JobHandle jobHandle)
		{
			if (this.componentPositionArray.Count > 0)
			{
				jobHandle = new TransformManager.ReadComponentTransformJob
				{
					positionArray = this.componentPositionArray.GetNativeArray(),
					minScaleArray = this.componentMinScaleArray.GetNativeArray()
				}.ScheduleReadOnly(this.componentTransformAccessArray, 16, jobHandle);
			}
			return jobHandle;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000247BC File Offset: 0x000229BC
		public void InformationLog(StringBuilder allsb)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("========== Transform Manager ==========");
			if (!this.IsValid())
			{
				stringBuilder.AppendLine("Transform Manager. Invalid.");
			}
			else
			{
				int num = (this.transformAccessArray.isCreated ? this.transformAccessArray.length : 0);
				stringBuilder.AppendLine(string.Format("Transform Manager. Length:{0}", num));
				stringBuilder.AppendLine("  -flagArray:" + this.flagArray.ToSummary());
				stringBuilder.AppendLine("  -initLocalPositionArray:" + this.initLocalPositionArray.ToSummary());
				stringBuilder.AppendLine("  -initLocalRotationArray:" + this.initLocalRotationArray.ToSummary());
				stringBuilder.AppendLine("  -baseLocalPositionArray:" + this.baseLocalPositionArray.ToSummary());
				stringBuilder.AppendLine("  -baseLocalRotationArray:" + this.baseLocalRotationArray.ToSummary());
				stringBuilder.AppendLine("  -positionArray:" + this.positionArray.ToSummary());
				stringBuilder.AppendLine("  -rotationArray:" + this.rotationArray.ToSummary());
				stringBuilder.AppendLine("  -scaleArray:" + this.scaleArray.ToSummary());
				stringBuilder.AppendLine("  -localPositionArray:" + this.localPositionArray.ToSummary());
				stringBuilder.AppendLine("  -localRotationArray:" + this.localRotationArray.ToSummary());
				stringBuilder.AppendLine("  -localScaleArray:" + this.localScaleArray.ToSummary());
				stringBuilder.AppendLine("  -localToWorldMatirxArray:" + this.localToWorldMatrixArray.ToSummary());
				stringBuilder.AppendLine("  -teamIdArray:" + this.teamIdArray.ToSummary());
				if (this.transformAccessArray.isCreated)
				{
					for (int i = 0; i < num; i++)
					{
						Transform transform = this.transformAccessArray[i];
						ExBitFlag8 exBitFlag = this.flagArray[i];
						short num2 = this.teamIdArray[i];
						stringBuilder.Append(string.Format("  [{0}] team:{1} (", i, num2));
						stringBuilder.Append(exBitFlag.IsSet(16) ? "E" : "");
						stringBuilder.Append(exBitFlag.IsSet(8) ? "R" : "");
						stringBuilder.Append(exBitFlag.IsSet(1) ? "r" : "");
						stringBuilder.Append(exBitFlag.IsSet(2) ? "W" : "");
						stringBuilder.Append(exBitFlag.IsSet(4) ? "w" : "");
						if (transform)
						{
							stringBuilder.AppendLine(") " + transform.name);
						}
						else
						{
							stringBuilder.AppendLine(") (null)");
						}
					}
				}
				num = (this.componentTransformAccessArray.isCreated ? this.componentTransformAccessArray.length : 0);
				stringBuilder.AppendLine(string.Format("Component Transform Manager. Length:{0}", num));
				stringBuilder.AppendLine("  -componentPositionArray:" + this.componentPositionArray.ToSummary());
				stringBuilder.AppendLine("  -componentMinScaleArray:" + this.componentMinScaleArray.ToSummary());
				if (this.componentTransformAccessArray.isCreated)
				{
					for (int j = 0; j < num; j++)
					{
						Transform transform2 = this.componentTransformAccessArray[j];
						ExBitFlag8 exBitFlag2 = this.flagArray[j];
						short num3 = this.teamIdArray[j];
						stringBuilder.Append(string.Format("  [{0}] team:{1} (", j, num3));
						stringBuilder.Append(exBitFlag2.IsSet(16) ? "E" : "");
						stringBuilder.Append(exBitFlag2.IsSet(8) ? "R" : "");
						stringBuilder.Append(exBitFlag2.IsSet(1) ? "r" : "");
						stringBuilder.Append(exBitFlag2.IsSet(2) ? "W" : "");
						stringBuilder.Append(exBitFlag2.IsSet(4) ? "w" : "");
						if (transform2)
						{
							stringBuilder.AppendLine(") " + transform2.name);
						}
						else
						{
							stringBuilder.AppendLine(") (null)");
						}
					}
				}
			}
			stringBuilder.AppendLine();
			Debug.Log(stringBuilder.ToString());
			allsb.Append(stringBuilder);
		}

		// Token: 0x0400072E RID: 1838
		internal const byte Flag_Read = 1;

		// Token: 0x0400072F RID: 1839
		internal const byte Flag_WorldRotWrite = 2;

		// Token: 0x04000730 RID: 1840
		internal const byte Flag_LocalPosRotWrite = 4;

		// Token: 0x04000731 RID: 1841
		internal const byte Flag_Restore = 8;

		// Token: 0x04000732 RID: 1842
		internal const byte Flag_Enable = 16;

		// Token: 0x04000733 RID: 1843
		internal ExNativeArray<ExBitFlag8> flagArray;

		// Token: 0x04000734 RID: 1844
		internal ExNativeArray<float3> initLocalPositionArray;

		// Token: 0x04000735 RID: 1845
		internal ExNativeArray<quaternion> initLocalRotationArray;

		// Token: 0x04000736 RID: 1846
		internal ExNativeArray<float3> baseLocalPositionArray;

		// Token: 0x04000737 RID: 1847
		internal ExNativeArray<quaternion> baseLocalRotationArray;

		// Token: 0x04000738 RID: 1848
		internal ExNativeArray<float3> positionArray;

		// Token: 0x04000739 RID: 1849
		internal ExNativeArray<quaternion> rotationArray;

		// Token: 0x0400073A RID: 1850
		internal ExNativeArray<float3> scaleArray;

		// Token: 0x0400073B RID: 1851
		internal ExNativeArray<float3> localPositionArray;

		// Token: 0x0400073C RID: 1852
		internal ExNativeArray<quaternion> localRotationArray;

		// Token: 0x0400073D RID: 1853
		internal ExNativeArray<float3> localScaleArray;

		// Token: 0x0400073E RID: 1854
		internal ExNativeArray<float4x4> localToWorldMatrixArray;

		// Token: 0x0400073F RID: 1855
		internal ExNativeArray<short> teamIdArray;

		// Token: 0x04000740 RID: 1856
		internal TransformAccessArray transformAccessArray;

		// Token: 0x04000741 RID: 1857
		internal ExNativeArray<float3> componentPositionArray;

		// Token: 0x04000742 RID: 1858
		internal ExNativeArray<float> componentMinScaleArray;

		// Token: 0x04000743 RID: 1859
		internal TransformAccessArray componentTransformAccessArray;

		// Token: 0x04000744 RID: 1860
		internal NativeReference<bool> existFixedTeam;

		// Token: 0x04000745 RID: 1861
		private bool isValid;

		// Token: 0x020000C1 RID: 193
		[BurstCompile]
		private struct EnableTransformJob : IJob
		{
			// Token: 0x06000377 RID: 887 RVA: 0x00024C74 File Offset: 0x00022E74
			public void Execute()
			{
				for (int i = 0; i < this.chunk.dataLength; i++)
				{
					int num = this.chunk.startIndex + i;
					ExBitFlag8 exBitFlag = this.flagList[num];
					if (exBitFlag.Value != 0)
					{
						exBitFlag.SetFlag(16, this.sw);
						this.flagList[num] = exBitFlag;
					}
				}
			}

			// Token: 0x04000746 RID: 1862
			public DataChunk chunk;

			// Token: 0x04000747 RID: 1863
			public bool sw;

			// Token: 0x04000748 RID: 1864
			public NativeArray<ExBitFlag8> flagList;
		}

		// Token: 0x020000C2 RID: 194
		[BurstCompile]
		private struct RestoreTransformJob : IJobParallelForTransform
		{
			// Token: 0x06000378 RID: 888 RVA: 0x00024CD8 File Offset: 0x00022ED8
			public void Execute(int index, TransformAccess transform)
			{
				if (!transform.isValid)
				{
					return;
				}
				ExBitFlag8 exBitFlag = this.flagList[index];
				if (!exBitFlag.IsSet(8))
				{
					return;
				}
				int num = (int)this.teamIdArray[index];
				TeamManager.TeamData teamData = this.teamDataArray[num];
				if (!exBitFlag.IsSet(16) && !teamData.flag.IsSet(20))
				{
					return;
				}
				if ((teamData.IsCameraCullingInvisible && teamData.IsCameraCullingKeep) || teamData.IsDistanceCullingInvisible)
				{
					return;
				}
				transform.SetLocalPositionAndRotation(this.localPositionArray[index], this.localRotationArray[index]);
				if (teamData.IsFixedUpdate)
				{
					this.existFixedTeam.Value = true;
				}
			}

			// Token: 0x04000749 RID: 1865
			[NativeDisableParallelForRestriction]
			public NativeReference<bool> existFixedTeam;

			// Token: 0x0400074A RID: 1866
			[ReadOnly]
			public NativeArray<ExBitFlag8> flagList;

			// Token: 0x0400074B RID: 1867
			[ReadOnly]
			public NativeArray<float3> localPositionArray;

			// Token: 0x0400074C RID: 1868
			[ReadOnly]
			public NativeArray<quaternion> localRotationArray;

			// Token: 0x0400074D RID: 1869
			[ReadOnly]
			public NativeArray<short> teamIdArray;

			// Token: 0x0400074E RID: 1870
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;
		}

		// Token: 0x020000C3 RID: 195
		[BurstCompile]
		private struct RestoreBaseTransformJob : IJobParallelForTransform
		{
			// Token: 0x06000379 RID: 889 RVA: 0x00024D98 File Offset: 0x00022F98
			public void Execute(int index, TransformAccess transform)
			{
				if (!transform.isValid)
				{
					return;
				}
				ExBitFlag8 exBitFlag = this.flagList[index];
				if (!exBitFlag.IsSet(8))
				{
					return;
				}
				int num = (int)this.teamIdArray[index];
				TeamManager.TeamData teamData = this.teamDataArray[num];
				if (!teamData.IsFixedUpdate)
				{
					return;
				}
				if (!exBitFlag.IsSet(16) && !teamData.flag.IsSet(20))
				{
					return;
				}
				if ((teamData.IsCameraCullingInvisible && teamData.IsCameraCullingKeep) || teamData.IsDistanceCullingInvisible)
				{
					return;
				}
				transform.SetLocalPositionAndRotation(this.baseLocalPositionArray[index], this.baseLocalRotationArray[index]);
			}

			// Token: 0x0400074F RID: 1871
			[ReadOnly]
			public NativeArray<ExBitFlag8> flagList;

			// Token: 0x04000750 RID: 1872
			[ReadOnly]
			public NativeArray<float3> baseLocalPositionArray;

			// Token: 0x04000751 RID: 1873
			[ReadOnly]
			public NativeArray<quaternion> baseLocalRotationArray;

			// Token: 0x04000752 RID: 1874
			[ReadOnly]
			public NativeArray<short> teamIdArray;

			// Token: 0x04000753 RID: 1875
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;
		}

		// Token: 0x020000C4 RID: 196
		[BurstCompile]
		private struct ReadTransformJob : IJobParallelForTransform
		{
			// Token: 0x0600037A RID: 890 RVA: 0x00024E4C File Offset: 0x0002304C
			public void Execute(int index, TransformAccess transform)
			{
				if (!transform.isValid)
				{
					return;
				}
				ExBitFlag8 exBitFlag = this.flagList[index];
				if (!exBitFlag.IsSet(16))
				{
					return;
				}
				if (!exBitFlag.IsSet(1))
				{
					return;
				}
				int num = (int)this.teamIdArray[index];
				TeamManager.TeamData teamData = this.teamDataArray[num];
				if (teamData.IsCullingInvisible)
				{
					return;
				}
				Vector3 vector;
				Quaternion quaternion;
				transform.GetPositionAndRotation(out vector, out quaternion);
				float4x4 float4x = transform.localToWorldMatrix;
				this.positionArray[index] = vector;
				this.rotationArray[index] = quaternion;
				this.localPositionArray[index] = transform.localPosition;
				this.localRotationArray[index] = transform.localRotation;
				this.localScaleArray[index] = transform.localScale;
				float4x4 float4x2 = math.mul(new float4x4(math.inverse(quaternion), float3.zero), float4x);
				float3 @float = new float3(float4x2.c0.x, float4x2.c1.y, float4x2.c2.z);
				this.scaleList[index] = @float;
				this.localToWorldMatrixArray[index] = float4x;
				if ((teamData.IsFixedUpdate && this.fixedUpdateCount > 0) || !teamData.IsFixedUpdate)
				{
					this.baseLocalPositionArray[index] = transform.localPosition;
					this.baseLocalRotationArray[index] = transform.localRotation;
				}
			}

			// Token: 0x04000754 RID: 1876
			public int fixedUpdateCount;

			// Token: 0x04000755 RID: 1877
			[ReadOnly]
			public NativeArray<ExBitFlag8> flagList;

			// Token: 0x04000756 RID: 1878
			[WriteOnly]
			public NativeArray<float3> positionArray;

			// Token: 0x04000757 RID: 1879
			[WriteOnly]
			public NativeArray<quaternion> rotationArray;

			// Token: 0x04000758 RID: 1880
			[WriteOnly]
			public NativeArray<float3> scaleList;

			// Token: 0x04000759 RID: 1881
			[WriteOnly]
			public NativeArray<float3> localPositionArray;

			// Token: 0x0400075A RID: 1882
			[WriteOnly]
			public NativeArray<quaternion> localRotationArray;

			// Token: 0x0400075B RID: 1883
			[WriteOnly]
			public NativeArray<float3> localScaleArray;

			// Token: 0x0400075C RID: 1884
			[WriteOnly]
			public NativeArray<float4x4> localToWorldMatrixArray;

			// Token: 0x0400075D RID: 1885
			[WriteOnly]
			public NativeArray<float3> baseLocalPositionArray;

			// Token: 0x0400075E RID: 1886
			[WriteOnly]
			public NativeArray<quaternion> baseLocalRotationArray;

			// Token: 0x0400075F RID: 1887
			[ReadOnly]
			public NativeArray<short> teamIdArray;

			// Token: 0x04000760 RID: 1888
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;
		}

		// Token: 0x020000C5 RID: 197
		[BurstCompile]
		private struct WriteTransformJob : IJobParallelForTransform
		{
			// Token: 0x0600037B RID: 891 RVA: 0x00024FE4 File Offset: 0x000231E4
			public void Execute(int index, TransformAccess transform)
			{
				if (!transform.isValid)
				{
					return;
				}
				ExBitFlag8 exBitFlag = this.flagList[index];
				if (!exBitFlag.IsSet(16))
				{
					return;
				}
				int num = (int)this.teamIdArray[index];
				TeamManager.TeamData teamData = this.teamDataArray[num];
				if (teamData.IsCullingInvisible)
				{
					return;
				}
				if (teamData.IsScaleSuspend)
				{
					return;
				}
				if (teamData.flag.IsSet(14))
				{
					return;
				}
				if (exBitFlag.IsSet(2))
				{
					transform.rotation = this.worldRotations[index];
					if (teamData.IsSpring)
					{
						transform.position = this.worldPositions[index];
						return;
					}
				}
				else if (exBitFlag.IsSet(4))
				{
					transform.SetLocalPositionAndRotation(this.localPositions[index], this.localRotations[index]);
				}
			}

			// Token: 0x04000761 RID: 1889
			[ReadOnly]
			public NativeArray<ExBitFlag8> flagList;

			// Token: 0x04000762 RID: 1890
			[ReadOnly]
			public NativeArray<float3> worldPositions;

			// Token: 0x04000763 RID: 1891
			[ReadOnly]
			public NativeArray<quaternion> worldRotations;

			// Token: 0x04000764 RID: 1892
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000765 RID: 1893
			[ReadOnly]
			public NativeArray<quaternion> localRotations;

			// Token: 0x04000766 RID: 1894
			[ReadOnly]
			public NativeArray<short> teamIdArray;

			// Token: 0x04000767 RID: 1895
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;
		}

		// Token: 0x020000C6 RID: 198
		[BurstCompile]
		private struct ReadComponentTransformJob : IJobParallelForTransform
		{
			// Token: 0x0600037C RID: 892 RVA: 0x000250CC File Offset: 0x000232CC
			public void Execute(int index, TransformAccess transform)
			{
				if (!transform.isValid)
				{
					return;
				}
				this.positionArray[index] = transform.position;
				float num = math.cmin(math.abs(transform.localToWorldMatrix.lossyScale));
				this.minScaleArray[index] = num;
			}

			// Token: 0x04000768 RID: 1896
			[WriteOnly]
			public NativeArray<float3> positionArray;

			// Token: 0x04000769 RID: 1897
			[WriteOnly]
			public NativeArray<float> minScaleArray;
		}
	}
}
