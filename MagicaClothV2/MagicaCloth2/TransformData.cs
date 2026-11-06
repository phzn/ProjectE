using System;
using System.Collections.Generic;
using System.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace MagicaCloth2
{
	// Token: 0x020000BB RID: 187
	public class TransformData : IDisposable
	{
		// Token: 0x0600033A RID: 826 RVA: 0x000020C3 File Offset: 0x000002C3
		public TransformData()
		{
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00021F0B File Offset: 0x0002010B
		public TransformData(int capacity)
		{
			this.Init(capacity);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00021F1C File Offset: 0x0002011C
		public void Init(int capacity)
		{
			this.transformList = new List<Transform>(capacity);
			this.idArray = new ExSimpleNativeArray<int>(capacity, true);
			this.parentIdArray = new ExSimpleNativeArray<int>(capacity, true);
			this.flagArray = new ExSimpleNativeArray<ExBitFlag8>(capacity, true);
			this.initLocalPositionArray = new ExSimpleNativeArray<float3>(capacity, true);
			this.initLocalRotationArray = new ExSimpleNativeArray<quaternion>(capacity, true);
			this.positionArray = new ExSimpleNativeArray<float3>(capacity, true);
			this.rotationArray = new ExSimpleNativeArray<quaternion>(capacity, true);
			this.scaleArray = new ExSimpleNativeArray<float3>(capacity, true);
			this.localPositionArray = new ExSimpleNativeArray<float3>(capacity, true);
			this.localRotationArray = new ExSimpleNativeArray<quaternion>(capacity, true);
			this.inverseRotationArray = new ExSimpleNativeArray<quaternion>(capacity, true);
			this.emptyStack = new Queue<int>(capacity);
			this.isDirty = true;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00021FD8 File Offset: 0x000201D8
		public void Dispose()
		{
			List<Transform> list = this.transformList;
			if (list != null)
			{
				list.Clear();
			}
			ExSimpleNativeArray<int> exSimpleNativeArray = this.idArray;
			if (exSimpleNativeArray != null)
			{
				exSimpleNativeArray.Dispose();
			}
			ExSimpleNativeArray<int> exSimpleNativeArray2 = this.parentIdArray;
			if (exSimpleNativeArray2 != null)
			{
				exSimpleNativeArray2.Dispose();
			}
			ExSimpleNativeArray<ExBitFlag8> exSimpleNativeArray3 = this.flagArray;
			if (exSimpleNativeArray3 != null)
			{
				exSimpleNativeArray3.Dispose();
			}
			ExSimpleNativeArray<float3> exSimpleNativeArray4 = this.initLocalPositionArray;
			if (exSimpleNativeArray4 != null)
			{
				exSimpleNativeArray4.Dispose();
			}
			ExSimpleNativeArray<quaternion> exSimpleNativeArray5 = this.initLocalRotationArray;
			if (exSimpleNativeArray5 != null)
			{
				exSimpleNativeArray5.Dispose();
			}
			ExSimpleNativeArray<float3> exSimpleNativeArray6 = this.positionArray;
			if (exSimpleNativeArray6 != null)
			{
				exSimpleNativeArray6.Dispose();
			}
			ExSimpleNativeArray<quaternion> exSimpleNativeArray7 = this.rotationArray;
			if (exSimpleNativeArray7 != null)
			{
				exSimpleNativeArray7.Dispose();
			}
			ExSimpleNativeArray<float3> exSimpleNativeArray8 = this.scaleArray;
			if (exSimpleNativeArray8 != null)
			{
				exSimpleNativeArray8.Dispose();
			}
			ExSimpleNativeArray<float3> exSimpleNativeArray9 = this.localPositionArray;
			if (exSimpleNativeArray9 != null)
			{
				exSimpleNativeArray9.Dispose();
			}
			ExSimpleNativeArray<quaternion> exSimpleNativeArray10 = this.localRotationArray;
			if (exSimpleNativeArray10 != null)
			{
				exSimpleNativeArray10.Dispose();
			}
			ExSimpleNativeArray<quaternion> exSimpleNativeArray11 = this.inverseRotationArray;
			if (exSimpleNativeArray11 != null)
			{
				exSimpleNativeArray11.Dispose();
			}
			Queue<int> queue = this.emptyStack;
			if (queue != null)
			{
				queue.Clear();
			}
			if (this.transformAccessArray.isCreated)
			{
				this.transformAccessArray.Dispose();
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600033E RID: 830 RVA: 0x000220DA File Offset: 0x000202DA
		public int Count
		{
			get
			{
				return this.transformList.Count;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600033F RID: 831 RVA: 0x000220E7 File Offset: 0x000202E7
		public int RootCount
		{
			get
			{
				List<int> list = this.rootIdList;
				if (list == null)
				{
					return 0;
				}
				return list.Count;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000340 RID: 832 RVA: 0x000220FA File Offset: 0x000202FA
		public bool IsDirty
		{
			get
			{
				return this.isDirty;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00022102 File Offset: 0x00020302
		public bool IsEmpty
		{
			get
			{
				return this.transformList == null;
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00022110 File Offset: 0x00020310
		public int AddTransform(Transform t, int tid = 0, int pid = 0, byte flag = 1, bool checkDuplicate = true)
		{
			int num;
			if (checkDuplicate)
			{
				num = this.ReferenceIndexOf<Transform>(this.transformList, t);
				if (num >= 0)
				{
					return num;
				}
			}
			if (this.emptyStack.Count > 0)
			{
				num = this.emptyStack.Dequeue();
				this.transformList[num] = t;
				if (tid == 0)
				{
					this.idArray[num] = t.GetInstanceID();
					this.parentIdArray[num] = (t.parent ? t.parent.GetInstanceID() : 0);
					this.initLocalPositionArray[num] = t.localPosition;
					this.initLocalRotationArray[num] = t.localRotation;
					this.positionArray[num] = t.position;
					this.rotationArray[num] = t.rotation;
					this.scaleArray[num] = t.lossyScale;
					this.localPositionArray[num] = t.localPosition;
					this.localRotationArray[num] = t.localRotation;
					this.inverseRotationArray[num] = Quaternion.Inverse(t.rotation);
					this.flagArray[num] = new ExBitFlag8(flag);
				}
				else
				{
					this.idArray[num] = tid;
					this.parentIdArray[num] = pid;
					this.initLocalPositionArray[num] = 0;
					this.initLocalRotationArray[num] = quaternion.identity;
					this.positionArray[num] = 0;
					this.rotationArray[num] = quaternion.identity;
					this.scaleArray[num] = 1;
					this.localPositionArray[num] = 0;
					this.localRotationArray[num] = quaternion.identity;
					this.inverseRotationArray[num] = quaternion.identity;
					this.flagArray[num] = new ExBitFlag8(flag);
				}
			}
			else
			{
				num = this.Count;
				this.transformList.Add(t);
				if (tid == 0)
				{
					this.idArray.Add(t.GetInstanceID());
					this.parentIdArray.Add(t.parent ? t.parent.GetInstanceID() : 0);
					this.initLocalPositionArray.Add(t.localPosition);
					this.initLocalRotationArray.Add(t.localRotation);
					this.positionArray.Add(t.position);
					this.rotationArray.Add(t.rotation);
					this.scaleArray.Add(t.lossyScale);
					this.localPositionArray.Add(t.localPosition);
					this.localRotationArray.Add(t.localRotation);
					this.inverseRotationArray.Add(Quaternion.Inverse(t.rotation));
					this.flagArray.Add(new ExBitFlag8(flag));
				}
				else
				{
					this.idArray.Add(tid);
					this.parentIdArray.Add(pid);
					this.initLocalPositionArray.Add(0);
					this.initLocalRotationArray.Add(quaternion.identity);
					this.positionArray.Add(0);
					this.rotationArray.Add(quaternion.identity);
					this.scaleArray.Add(1);
					this.localPositionArray.Add(0);
					this.localRotationArray.Add(quaternion.identity);
					this.inverseRotationArray.Add(quaternion.identity);
					this.flagArray.Add(new ExBitFlag8(flag));
				}
			}
			this.isDirty = true;
			return num;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00022504 File Offset: 0x00020704
		public int AddTransform(TransformRecord record, int pid = 0, byte flag = 1, bool checkDuplicate = true)
		{
			int num;
			if (checkDuplicate)
			{
				num = this.ReferenceIndexOf<Transform>(this.transformList, record.transform);
				if (num >= 0)
				{
					return num;
				}
			}
			if (this.emptyStack.Count > 0)
			{
				num = this.emptyStack.Dequeue();
				this.transformList[num] = record.transform;
				this.idArray[num] = record.id;
				this.parentIdArray[num] = pid;
				this.initLocalPositionArray[num] = record.localPosition;
				this.initLocalRotationArray[num] = record.localRotation;
				this.positionArray[num] = record.position;
				this.rotationArray[num] = record.rotation;
				this.scaleArray[num] = record.scale;
				this.localPositionArray[num] = record.localPosition;
				this.localRotationArray[num] = record.localRotation;
				this.inverseRotationArray[num] = Quaternion.Inverse(record.rotation);
				this.flagArray[num] = new ExBitFlag8(flag);
			}
			else
			{
				num = this.Count;
				this.transformList.Add(record.transform);
				this.idArray.Add(record.id);
				this.parentIdArray.Add(pid);
				this.initLocalPositionArray.Add(record.localPosition);
				this.initLocalRotationArray.Add(record.localRotation);
				this.positionArray.Add(record.position);
				this.rotationArray.Add(record.rotation);
				this.scaleArray.Add(record.scale);
				this.localPositionArray.Add(record.localPosition);
				this.localRotationArray.Add(record.localRotation);
				this.inverseRotationArray.Add(Quaternion.Inverse(record.rotation));
				this.flagArray.Add(new ExBitFlag8(flag));
			}
			this.isDirty = true;
			return num;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00022754 File Offset: 0x00020954
		public int AddTransform(TransformData srcData, int srcIndex, bool checkDuplicate = true)
		{
			Transform transform = srcData.transformList[srcIndex];
			int num;
			if (checkDuplicate)
			{
				num = this.ReferenceIndexOf<Transform>(this.transformList, transform);
				if (num >= 0)
				{
					return num;
				}
			}
			int num2 = srcData.idArray[srcIndex];
			int num3 = srcData.parentIdArray[srcIndex];
			float3 @float = srcData.initLocalPositionArray[srcIndex];
			quaternion quaternion = srcData.initLocalRotationArray[srcIndex];
			float3 float2 = srcData.positionArray[srcIndex];
			quaternion quaternion2 = srcData.rotationArray[srcIndex];
			float3 float3 = srcData.scaleArray[srcIndex];
			float3 float4 = srcData.localPositionArray[srcIndex];
			quaternion quaternion3 = srcData.localRotationArray[srcIndex];
			quaternion quaternion4 = srcData.inverseRotationArray[srcIndex];
			ExBitFlag8 exBitFlag = srcData.flagArray[srcIndex];
			if (this.emptyStack.Count > 0)
			{
				num = this.emptyStack.Dequeue();
				this.transformList[num] = transform;
				this.idArray[num] = num2;
				this.parentIdArray[num] = num3;
				this.initLocalPositionArray[num] = @float;
				this.initLocalRotationArray[num] = quaternion;
				this.positionArray[num] = float2;
				this.rotationArray[num] = quaternion2;
				this.scaleArray[num] = float3;
				this.localPositionArray[num] = float4;
				this.localRotationArray[num] = quaternion3;
				this.inverseRotationArray[num] = quaternion4;
				this.flagArray[num] = exBitFlag;
			}
			else
			{
				num = this.Count;
				this.transformList.Add(transform);
				this.idArray.Add(num2);
				this.parentIdArray.Add(num3);
				this.initLocalPositionArray.Add(@float);
				this.initLocalRotationArray.Add(quaternion);
				this.positionArray.Add(float2);
				this.rotationArray.Add(quaternion2);
				this.scaleArray.Add(float3);
				this.localPositionArray.Add(float4);
				this.localRotationArray.Add(quaternion3);
				this.inverseRotationArray.Add(quaternion4);
				this.flagArray.Add(exBitFlag);
			}
			this.isDirty = true;
			return num;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0002298C File Offset: 0x00020B8C
		public int[] AddTransformRange(List<Transform> tlist, List<int> idList, List<int> pidList, int copyCount = 0)
		{
			int num = ((copyCount > 0) ? copyCount : tlist.Count);
			int count = this.Count;
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				this.transformList.Add(tlist[i]);
				this.idArray.Add(idList[i]);
				this.parentIdArray.Add(pidList[i]);
				array[i] = count + i;
			}
			this.flagArray.AddRange(num, new ExBitFlag8(1));
			this.initLocalPositionArray.AddRange(num);
			this.initLocalRotationArray.AddRange(num);
			this.positionArray.AddRange(num);
			this.rotationArray.AddRange(num);
			this.scaleArray.AddRange(num);
			this.localPositionArray.AddRange(num);
			this.localRotationArray.AddRange(num);
			this.inverseRotationArray.AddRange(num);
			this.isDirty = true;
			return array;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00022A79 File Offset: 0x00020C79
		public int[] AddTransformRange(TransformData stdata, int copyCount = 0)
		{
			return this.AddTransformRange(stdata.transformList, new List<int>(stdata.idArray.ToArray()), new List<int>(stdata.parentIdArray.ToArray()), copyCount);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00022AA8 File Offset: 0x00020CA8
		public int[] AddTransformRange(List<Transform> tlist, List<int> idList, List<int> pidList, List<int> rootIds, NativeArray<float3> localPositions, NativeArray<quaternion> localRotations, NativeArray<float3> positions, NativeArray<quaternion> rotations, NativeArray<float3> scales, NativeArray<quaternion> inverseRotations)
		{
			int count = tlist.Count;
			int count2 = this.Count;
			int[] array = new int[count];
			this.transformList.AddRange(tlist);
			for (int i = 0; i < count; i++)
			{
				this.idArray.Add(idList[i]);
				this.parentIdArray.Add(pidList[i]);
				array[i] = count2 + i;
			}
			if (rootIds != null && rootIds.Count > 0)
			{
				if (this.rootIdList == null)
				{
					this.rootIdList = new List<int>(rootIds);
				}
				else
				{
					this.rootIdList.AddRange(rootIds);
				}
			}
			this.flagArray.AddRange(count, new ExBitFlag8(1));
			this.initLocalPositionArray.AddRange(localPositions);
			this.initLocalRotationArray.AddRange(localRotations);
			this.positionArray.AddRange(positions);
			this.rotationArray.AddRange(rotations);
			this.scaleArray.AddRange(scales);
			this.localPositionArray.AddRange(localPositions);
			this.localRotationArray.AddRange(localRotations);
			this.inverseRotationArray.AddRange(inverseRotations);
			this.isDirty = true;
			return array;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00022BC0 File Offset: 0x00020DC0
		public void RemoveTransformIndex(int index)
		{
			this.transformList[index] = null;
			this.flagArray[index] = default(ExBitFlag8);
			this.emptyStack.Enqueue(index);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00022BFC File Offset: 0x00020DFC
		public int ReplaceTransform(int index, Transform t, int tid = 0, int pid = 0, byte flag = 1)
		{
			this.transformList[index] = t;
			this.flagArray[index] = new ExBitFlag8(flag);
			if (tid == 0)
			{
				this.idArray[index] = t.GetInstanceID();
				this.parentIdArray[index] = (t.parent ? t.parent.GetInstanceID() : 0);
				this.initLocalPositionArray[index] = t.localPosition;
				this.initLocalRotationArray[index] = t.localRotation;
				this.positionArray[index] = t.position;
				this.rotationArray[index] = t.rotation;
				this.scaleArray[index] = t.lossyScale;
				this.localPositionArray[index] = t.localPosition;
				this.localRotationArray[index] = t.localRotation;
			}
			else
			{
				this.idArray[index] = tid;
				this.parentIdArray[index] = pid;
				this.initLocalPositionArray[index] = 0;
				this.initLocalRotationArray[index] = quaternion.identity;
				this.positionArray[index] = 0;
				this.rotationArray[index] = quaternion.identity;
				this.scaleArray[index] = 1;
				this.localPositionArray[index] = 0;
				this.localRotationArray[index] = quaternion.identity;
			}
			this.isDirty = true;
			return index;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00022DAC File Offset: 0x00020FAC
		private int ReferenceIndexOf<T>(List<T> list, T item) where T : class
		{
			if (list == null)
			{
				return -1;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == item)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00022DE6 File Offset: 0x00020FE6
		public void UpdateWorkData()
		{
			if (this.isDirty)
			{
				if (this.transformAccessArray.isCreated)
				{
					this.transformAccessArray.Dispose();
				}
				this.transformAccessArray = new TransformAccessArray(this.transformList.ToArray(), -1);
				this.isDirty = false;
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00022E28 File Offset: 0x00021028
		public JobHandle RestoreTransform(int count, JobHandle jobHandle = default(JobHandle))
		{
			this.UpdateWorkData();
			jobHandle = new TransformData.RestoreTransformJob
			{
				count = count,
				flagList = this.flagArray.GetNativeArray(),
				localPositionArray = this.initLocalPositionArray.GetNativeArray(),
				localRotationArray = this.initLocalRotationArray.GetNativeArray()
			}.Schedule(this.transformAccessArray, jobHandle);
			return jobHandle;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00022E94 File Offset: 0x00021094
		public JobHandle ReadTransform(JobHandle jobHandle = default(JobHandle))
		{
			this.UpdateWorkData();
			jobHandle = new TransformData.ReadTransformJob
			{
				flagList = this.flagArray.GetNativeArray(),
				positionArray = this.positionArray.GetNativeArray(),
				rotationArray = this.rotationArray.GetNativeArray(),
				scaleList = this.scaleArray.GetNativeArray(),
				localPositionArray = this.localPositionArray.GetNativeArray(),
				localRotationArray = this.localRotationArray.GetNativeArray(),
				inverseRotationArray = this.inverseRotationArray.GetNativeArray()
			}.ScheduleReadOnly(this.transformAccessArray, 16, jobHandle);
			return jobHandle;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00022F40 File Offset: 0x00021140
		public void ReadTransformRun()
		{
			this.UpdateWorkData();
			new TransformData.ReadTransformJob
			{
				flagList = this.flagArray.GetNativeArray(),
				positionArray = this.positionArray.GetNativeArray(),
				rotationArray = this.rotationArray.GetNativeArray(),
				scaleList = this.scaleArray.GetNativeArray(),
				localPositionArray = this.localPositionArray.GetNativeArray(),
				localRotationArray = this.localRotationArray.GetNativeArray(),
				inverseRotationArray = this.inverseRotationArray.GetNativeArray()
			}.RunReadOnly(this.transformAccessArray);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00022FE8 File Offset: 0x000211E8
		public void OrganizeReductionTransform(VirtualMesh vmesh, ReductionWorkData workData)
		{
			List<int> list = new List<int>(workData.newSkinBoneCount.Value + 2);
			foreach (KeyValue<int, int> keyValue in workData.useSkinBoneMap)
			{
				list.Add(vmesh.skinBoneTransformIndices[keyValue.Key]);
			}
			int count = list.Count;
			list.Add(vmesh.skinRootIndex);
			int count2 = list.Count;
			list.Add(vmesh.centerTransformIndex);
			int count3 = list.Count;
			List<Transform> list2 = new List<Transform>(count3);
			ExSimpleNativeArray<int> exSimpleNativeArray = new ExSimpleNativeArray<int>(count3, false);
			ExSimpleNativeArray<int> exSimpleNativeArray2 = new ExSimpleNativeArray<int>(count3, false);
			ExSimpleNativeArray<ExBitFlag8> exSimpleNativeArray3 = new ExSimpleNativeArray<ExBitFlag8>(count3, false);
			ExSimpleNativeArray<float3> exSimpleNativeArray4 = new ExSimpleNativeArray<float3>(count3, false);
			ExSimpleNativeArray<quaternion> exSimpleNativeArray5 = new ExSimpleNativeArray<quaternion>(count3, false);
			ExSimpleNativeArray<float3> exSimpleNativeArray6 = new ExSimpleNativeArray<float3>(count3, false);
			ExSimpleNativeArray<quaternion> exSimpleNativeArray7 = new ExSimpleNativeArray<quaternion>(count3, false);
			ExSimpleNativeArray<float3> exSimpleNativeArray8 = new ExSimpleNativeArray<float3>(count3, false);
			for (int i = 0; i < count3; i++)
			{
				int num = list[i];
				list2.Add(this.transformList[num]);
				exSimpleNativeArray[i] = this.idArray[num];
				exSimpleNativeArray2[i] = this.parentIdArray[num];
				exSimpleNativeArray3[i] = this.flagArray[num];
				exSimpleNativeArray4[i] = this.initLocalPositionArray[num];
				exSimpleNativeArray5[i] = this.initLocalRotationArray[num];
				exSimpleNativeArray6[i] = this.positionArray[num];
				exSimpleNativeArray7[i] = this.rotationArray[num];
				exSimpleNativeArray8[i] = this.scaleArray[num];
			}
			this.transformList.Clear();
			this.idArray.Dispose();
			this.parentIdArray.Dispose();
			this.flagArray.Dispose();
			this.initLocalPositionArray.Dispose();
			this.initLocalRotationArray.Dispose();
			this.positionArray.Dispose();
			this.rotationArray.Dispose();
			this.scaleArray.Dispose();
			this.transformList = list2;
			this.idArray = exSimpleNativeArray;
			this.parentIdArray = exSimpleNativeArray2;
			this.flagArray = exSimpleNativeArray3;
			this.initLocalPositionArray = exSimpleNativeArray4;
			this.initLocalRotationArray = exSimpleNativeArray5;
			this.positionArray = exSimpleNativeArray6;
			this.rotationArray = exSimpleNativeArray7;
			this.scaleArray = exSimpleNativeArray8;
			this.emptyStack.Clear();
			vmesh.centerTransformIndex = count2;
			vmesh.skinRootIndex = count;
			this.isDirty = true;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00023290 File Offset: 0x00021490
		public Transform GetTransformFromIndex(int index)
		{
			return this.transformList[index];
		}

		// Token: 0x06000351 RID: 849 RVA: 0x000232A0 File Offset: 0x000214A0
		public int GetTransformIndexFormId(int id)
		{
			NativeArray<int> nativeArray = this.idArray.GetNativeArray();
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				if (nativeArray[i] == id)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x000232DA File Offset: 0x000214DA
		public int GetTransformIdFromIndex(int index)
		{
			return this.idArray[index];
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000232E8 File Offset: 0x000214E8
		public int GetParentIdFromIndex(int index)
		{
			return this.parentIdArray[index];
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000232F8 File Offset: 0x000214F8
		public float4x4 GetLocalToWorldMatrix(int index)
		{
			float3 @float = this.positionArray[index];
			quaternion quaternion = this.rotationArray[index];
			float3 float2 = this.scaleArray[index];
			return Matrix4x4.TRS(@float, quaternion, float2);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00023346 File Offset: 0x00021546
		public float4x4 GetWorldToLocalMatrix(int index)
		{
			return math.inverse(this.GetLocalToWorldMatrix(index));
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00023354 File Offset: 0x00021554
		public override string ToString()
		{
			List<Transform> list = this.transformList;
			int num = ((list != null) ? list.Count : 0);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("==== TransformData ====");
			stringBuilder.AppendLine(string.Format("isDirty:{0}", this.isDirty));
			stringBuilder.AppendLine(string.Format("transformList:{0}", num));
			stringBuilder.AppendLine(string.Format("flagArray:{0}", this.flagArray.Length));
			return stringBuilder.ToString();
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000233E0 File Offset: 0x000215E0
		public TransformData.ShareSerializationData ShareSerialize()
		{
			TransformData.ShareSerializationData shareSerializationData = new TransformData.ShareSerializationData();
			try
			{
				shareSerializationData.flagArray = this.flagArray.Serialize();
				shareSerializationData.initLocalPositionArray = this.initLocalPositionArray.Serialize();
				shareSerializationData.initLocalRotationArray = this.initLocalRotationArray.Serialize();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			return shareSerializationData;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00023440 File Offset: 0x00021640
		public static TransformData ShareDeserialize(TransformData.ShareSerializationData sdata)
		{
			if (sdata == null)
			{
				return null;
			}
			TransformData transformData = new TransformData();
			try
			{
				transformData.flagArray = new ExSimpleNativeArray<ExBitFlag8>(sdata.flagArray);
				transformData.initLocalPositionArray = new ExSimpleNativeArray<float3>(sdata.initLocalPositionArray);
				transformData.initLocalRotationArray = new ExSimpleNativeArray<quaternion>(sdata.initLocalRotationArray);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			return transformData;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000234A8 File Offset: 0x000216A8
		public TransformData.UniqueSerializationData UniqueSerialize()
		{
			TransformData.UniqueSerializationData uniqueSerializationData = new TransformData.UniqueSerializationData();
			try
			{
				uniqueSerializationData.transformArray = this.transformList.ToArray();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			return uniqueSerializationData;
		}

		// Token: 0x0400070F RID: 1807
		internal List<Transform> transformList;

		// Token: 0x04000710 RID: 1808
		internal ExSimpleNativeArray<ExBitFlag8> flagArray;

		// Token: 0x04000711 RID: 1809
		internal ExSimpleNativeArray<float3> initLocalPositionArray;

		// Token: 0x04000712 RID: 1810
		internal ExSimpleNativeArray<quaternion> initLocalRotationArray;

		// Token: 0x04000713 RID: 1811
		internal ExSimpleNativeArray<float3> positionArray;

		// Token: 0x04000714 RID: 1812
		internal ExSimpleNativeArray<quaternion> rotationArray;

		// Token: 0x04000715 RID: 1813
		internal ExSimpleNativeArray<quaternion> inverseRotationArray;

		// Token: 0x04000716 RID: 1814
		internal ExSimpleNativeArray<float3> scaleArray;

		// Token: 0x04000717 RID: 1815
		internal ExSimpleNativeArray<float3> localPositionArray;

		// Token: 0x04000718 RID: 1816
		internal ExSimpleNativeArray<quaternion> localRotationArray;

		// Token: 0x04000719 RID: 1817
		internal ExSimpleNativeArray<int> idArray;

		// Token: 0x0400071A RID: 1818
		internal ExSimpleNativeArray<int> parentIdArray;

		// Token: 0x0400071B RID: 1819
		internal List<int> rootIdList;

		// Token: 0x0400071C RID: 1820
		private bool isDirty;

		// Token: 0x0400071D RID: 1821
		internal TransformAccessArray transformAccessArray;

		// Token: 0x0400071E RID: 1822
		private Queue<int> emptyStack;

		// Token: 0x020000BC RID: 188
		[BurstCompile]
		private struct RestoreTransformJob : IJobParallelForTransform
		{
			// Token: 0x0600035A RID: 858 RVA: 0x000234E8 File Offset: 0x000216E8
			public void Execute(int index, TransformAccess transform)
			{
				if (index >= this.count)
				{
					return;
				}
				if (!transform.isValid)
				{
					return;
				}
				transform.SetLocalPositionAndRotation(this.localPositionArray[index], this.localRotationArray[index]);
			}

			// Token: 0x0400071F RID: 1823
			public int count;

			// Token: 0x04000720 RID: 1824
			[ReadOnly]
			public NativeArray<ExBitFlag8> flagList;

			// Token: 0x04000721 RID: 1825
			[ReadOnly]
			public NativeArray<float3> localPositionArray;

			// Token: 0x04000722 RID: 1826
			[ReadOnly]
			public NativeArray<quaternion> localRotationArray;
		}

		// Token: 0x020000BD RID: 189
		[BurstCompile]
		private struct ReadTransformJob : IJobParallelForTransform
		{
			// Token: 0x0600035B RID: 859 RVA: 0x00023528 File Offset: 0x00021728
			public void Execute(int index, TransformAccess transform)
			{
				if (!transform.isValid)
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
				float4x4 float4x2 = math.mul(new float4x4(math.inverse(quaternion), float3.zero), float4x);
				float3 @float = new float3(float4x2.c0.x, float4x2.c1.y, float4x2.c2.z);
				this.scaleList[index] = @float;
				this.inverseRotationArray[index] = math.inverse(quaternion);
			}

			// Token: 0x04000723 RID: 1827
			[ReadOnly]
			public NativeArray<ExBitFlag8> flagList;

			// Token: 0x04000724 RID: 1828
			[WriteOnly]
			public NativeArray<float3> positionArray;

			// Token: 0x04000725 RID: 1829
			[WriteOnly]
			public NativeArray<quaternion> rotationArray;

			// Token: 0x04000726 RID: 1830
			[WriteOnly]
			public NativeArray<float3> scaleList;

			// Token: 0x04000727 RID: 1831
			[WriteOnly]
			public NativeArray<float3> localPositionArray;

			// Token: 0x04000728 RID: 1832
			[WriteOnly]
			public NativeArray<quaternion> localRotationArray;

			// Token: 0x04000729 RID: 1833
			[WriteOnly]
			public NativeArray<quaternion> inverseRotationArray;
		}

		// Token: 0x020000BE RID: 190
		[Serializable]
		public class ShareSerializationData
		{
			// Token: 0x0400072A RID: 1834
			public ExSimpleNativeArray<ExBitFlag8>.SerializationData flagArray;

			// Token: 0x0400072B RID: 1835
			public ExSimpleNativeArray<float3>.SerializationData initLocalPositionArray;

			// Token: 0x0400072C RID: 1836
			public ExSimpleNativeArray<quaternion>.SerializationData initLocalRotationArray;
		}

		// Token: 0x020000BF RID: 191
		[Serializable]
		public class UniqueSerializationData : ITransform
		{
			// Token: 0x0600035D RID: 861 RVA: 0x00023614 File Offset: 0x00021814
			public void GetUsedTransform(HashSet<Transform> transformSet)
			{
				if (this.transformArray != null)
				{
					foreach (Transform transform in this.transformArray)
					{
						if (transform)
						{
							transformSet.Add(transform);
						}
					}
				}
			}

			// Token: 0x0600035E RID: 862 RVA: 0x00023654 File Offset: 0x00021854
			public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
			{
				if (this.transformArray != null)
				{
					for (int i = 0; i < this.transformArray.Length; i++)
					{
						Transform transform = this.transformArray[i];
						if (transform)
						{
							int instanceID = transform.GetInstanceID();
							if (instanceID != 0 && replaceDict.ContainsKey(instanceID))
							{
								this.transformArray[i] = replaceDict[instanceID];
							}
						}
					}
				}
			}

			// Token: 0x0400072D RID: 1837
			public Transform[] transformArray;
		}
	}
}
