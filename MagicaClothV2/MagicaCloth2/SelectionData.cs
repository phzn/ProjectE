using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x02000069 RID: 105
	[Serializable]
	public class SelectionData : IValid
	{
		// Token: 0x06000193 RID: 403 RVA: 0x000020C3 File Offset: 0x000002C3
		public SelectionData()
		{
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000FAA0 File Offset: 0x0000DCA0
		public SelectionData(int cnt)
		{
			this.positions = new float3[cnt];
			this.attributes = new VertexAttribute[cnt];
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000FAC0 File Offset: 0x0000DCC0
		public SelectionData(VirtualMesh vmesh, float4x4 transformMatrix)
		{
			if (vmesh != null && vmesh.VertexCount > 0)
			{
				using (NativeArray<float3> nativeArray = new NativeArray<float3>(vmesh.localPositions.GetNativeArray(), Allocator.TempJob))
				{
					new SelectionData.TransformPositionJob
					{
						transformMatrix = transformMatrix,
						localPositions = nativeArray
					}.Run(vmesh.VertexCount);
					this.positions = nativeArray.ToArray();
					this.attributes = vmesh.attributes.ToArray();
					this.maxConnectionDistance = vmesh.maxVertexDistance.Value;
				}
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000FB68 File Offset: 0x0000DD68
		public int Count
		{
			get
			{
				float3[] array = this.positions;
				if (array == null)
				{
					return 0;
				}
				return array.Length;
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000FB78 File Offset: 0x0000DD78
		public bool IsValid()
		{
			return this.positions != null && this.positions.Length != 0 && this.attributes != null && this.attributes.Length != 0 && this.positions.Length == this.attributes.Length;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000FBB5 File Offset: 0x0000DDB5
		public bool IsUserEdit()
		{
			return this.userEdit;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
		public SelectionData Clone()
		{
			SelectionData selectionData = new SelectionData();
			float3[] array = this.positions;
			selectionData.positions = ((array != null) ? array.Clone() : null) as float3[];
			VertexAttribute[] array2 = this.attributes;
			selectionData.attributes = ((array2 != null) ? array2.Clone() : null) as VertexAttribute[];
			selectionData.maxConnectionDistance = this.maxConnectionDistance;
			selectionData.userEdit = this.userEdit;
			return selectionData;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000FC24 File Offset: 0x0000DE24
		public bool Compare(SelectionData sdata)
		{
			float3[] array = this.positions;
			int? num = ((array != null) ? new int?(array.Length) : null);
			float3[] array2 = sdata.positions;
			int? num2 = ((array2 != null) ? new int?(array2.Length) : null);
			if (!((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null))))
			{
				return false;
			}
			VertexAttribute[] array3 = this.attributes;
			num2 = ((array3 != null) ? new int?(array3.Length) : null);
			VertexAttribute[] array4 = sdata.attributes;
			num = ((array4 != null) ? new int?(array4.Length) : null);
			if (!((num2.GetValueOrDefault() == num.GetValueOrDefault()) & (num2 != null == (num != null))))
			{
				return false;
			}
			if (this.userEdit != sdata.userEdit)
			{
				return false;
			}
			int num3 = this.attributes.Length;
			for (int i = 0; i < num3; i++)
			{
				if (this.attributes[i] != sdata.attributes[i])
				{
					return false;
				}
				if (!this.positions[i].Equals(sdata.positions[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000FD60 File Offset: 0x0000DF60
		public void AddRange(float3[] addPositions, VertexAttribute[] addAttributes = null)
		{
			if (this.Count == 0)
			{
				this.positions = addPositions;
				this.attributes = ((addAttributes != null) ? addAttributes : new VertexAttribute[addPositions.Length]);
				return;
			}
			int count = this.Count;
			int num = addPositions.Length;
			float3[] array = new float3[count + num];
			VertexAttribute[] array2 = new VertexAttribute[count + num];
			Array.Copy(this.positions, 0, array, 0, count);
			Array.Copy(addPositions, 0, array, count, num);
			Array.Copy(this.attributes, 0, array2, 0, count);
			if (addAttributes != null)
			{
				Array.Copy(addAttributes, 0, array2, count, num);
			}
			this.positions = array;
			this.attributes = array2;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000FDF1 File Offset: 0x0000DFF1
		public void Fill(VertexAttribute attr)
		{
			Array.Fill<VertexAttribute>(this.attributes, attr);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000FDFF File Offset: 0x0000DFFF
		public NativeArray<float3> GetPositionNativeArray()
		{
			return new NativeArray<float3>(this.positions, Allocator.Persistent);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000FE10 File Offset: 0x0000E010
		public NativeArray<float3> GetPositionNativeArray(float4x4 transformMatrix)
		{
			NativeArray<float3> positionNativeArray = this.GetPositionNativeArray();
			new SelectionData.TransformPositionJob
			{
				transformMatrix = transformMatrix,
				localPositions = positionNativeArray
			}.Run(this.Count);
			return positionNativeArray;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000FE49 File Offset: 0x0000E049
		public NativeArray<VertexAttribute> GetAttributeNativeArray()
		{
			return new NativeArray<VertexAttribute>(this.attributes, Allocator.Persistent);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000FE58 File Offset: 0x0000E058
		public static GridMap<int> CreateGridMapRun(float gridSize, in NativeArray<float3> positions, in NativeArray<VertexAttribute> attributes, bool move = true, bool fix = true, bool ignore = true, bool invalid = true)
		{
			NativeArray<float3> nativeArray = positions;
			GridMap<int> gridMap = new GridMap<int>(nativeArray.Length);
			new SelectionData.CreateGridMapJob
			{
				move = move,
				fix = fix,
				ignore = ignore,
				invalid = invalid,
				gridMap = gridMap.GetMultiHashMap(),
				gridSize = gridSize,
				positions = positions,
				attribute = attributes
			}.Run<SelectionData.CreateGridMapJob>();
			return gridMap;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000FEDC File Offset: 0x0000E0DC
		public void Merge(SelectionData from)
		{
			if (from.Count == 0)
			{
				return;
			}
			int num = this.Count + from.Count;
			List<float3> list = new List<float3>(num);
			List<VertexAttribute> list2 = new List<VertexAttribute>(num);
			if (this.positions != null)
			{
				list.AddRange(this.positions);
			}
			if (this.attributes != null)
			{
				list2.AddRange(this.attributes);
			}
			list.AddRange(from.positions);
			list2.AddRange(from.attributes);
			this.positions = list.ToArray();
			this.attributes = list2.ToArray();
			this.maxConnectionDistance = math.max(this.maxConnectionDistance, from.maxConnectionDistance);
			this.userEdit = this.userEdit || from.userEdit;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000FF94 File Offset: 0x0000E194
		public void ConvertFrom(SelectionData from)
		{
			if (from.Count == 0)
			{
				return;
			}
			if (this.Count == 0)
			{
				return;
			}
			using (NativeArray<float3> positionNativeArray = this.GetPositionNativeArray())
			{
				using (NativeArray<VertexAttribute> attributeNativeArray = this.GetAttributeNativeArray())
				{
					NativeArray<float3> positionNativeArray2 = from.GetPositionNativeArray();
					try
					{
						NativeArray<VertexAttribute> attributeNativeArray2 = from.GetAttributeNativeArray();
						try
						{
							using (NativeReference<AABB> nativeReference = new NativeReference<AABB>(Allocator.TempJob, NativeArrayOptions.ClearMemory))
							{
								JobUtility.CalcAABBRun(positionNativeArray, this.Count, nativeReference);
								float num = nativeReference.Value.MaxSideLength * 0.2f;
								num = math.max(num, 1E-05f);
								float num2 = num * 0.5f;
								using (GridMap<int> gridMap = SelectionData.CreateGridMapRun(num2, in positionNativeArray2, in attributeNativeArray2, true, true, true, true))
								{
									new SelectionData.ConvertSelectionJob
									{
										gridSize = num2,
										radius = num,
										toPositions = positionNativeArray,
										toAttributes = attributeNativeArray,
										gridMap = gridMap.GetMultiHashMap(),
										fromPositions = positionNativeArray2,
										fromAttributes = attributeNativeArray2
									}.Run(this.Count);
									this.positions = positionNativeArray.ToArray();
									this.attributes = attributeNativeArray.ToArray();
								}
							}
						}
						finally
						{
							((IDisposable)attributeNativeArray2).Dispose();
						}
					}
					finally
					{
						((IDisposable)positionNativeArray2).Dispose();
					}
				}
			}
		}

		// Token: 0x0400025E RID: 606
		public float3[] positions;

		// Token: 0x0400025F RID: 607
		public VertexAttribute[] attributes;

		// Token: 0x04000260 RID: 608
		public float maxConnectionDistance;

		// Token: 0x04000261 RID: 609
		public bool userEdit;

		// Token: 0x0200006A RID: 106
		[BurstCompile]
		private struct TransformPositionJob : IJobParallelFor
		{
			// Token: 0x060001A3 RID: 419 RVA: 0x00010184 File Offset: 0x0000E384
			public void Execute(int index)
			{
				float3 @float = this.localPositions[index];
				@float = math.transform(this.transformMatrix, @float);
				this.localPositions[index] = @float;
			}

			// Token: 0x04000262 RID: 610
			public float4x4 transformMatrix;

			// Token: 0x04000263 RID: 611
			public NativeArray<float3> localPositions;
		}

		// Token: 0x0200006B RID: 107
		[BurstCompile]
		private struct CreateGridMapJob : IJob
		{
			// Token: 0x060001A4 RID: 420 RVA: 0x000101B8 File Offset: 0x0000E3B8
			public void Execute()
			{
				int length = this.positions.Length;
				for (int i = 0; i < length; i++)
				{
					VertexAttribute vertexAttribute = this.attribute[i];
					if ((this.move || !vertexAttribute.IsMove()) && (this.fix || !vertexAttribute.IsFixed()) && (this.invalid || !vertexAttribute.IsInvalid()))
					{
						GridMap<int>.AddGrid(this.positions[i], i, this.gridMap, this.gridSize);
					}
				}
			}

			// Token: 0x04000264 RID: 612
			public bool move;

			// Token: 0x04000265 RID: 613
			public bool fix;

			// Token: 0x04000266 RID: 614
			public bool ignore;

			// Token: 0x04000267 RID: 615
			public bool invalid;

			// Token: 0x04000268 RID: 616
			public NativeParallelMultiHashMap<int3, int> gridMap;

			// Token: 0x04000269 RID: 617
			public float gridSize;

			// Token: 0x0400026A RID: 618
			[ReadOnly]
			public NativeArray<float3> positions;

			// Token: 0x0400026B RID: 619
			[ReadOnly]
			public NativeArray<VertexAttribute> attribute;
		}

		// Token: 0x0200006C RID: 108
		[BurstCompile]
		private struct ConvertSelectionJob : IJobParallelFor
		{
			// Token: 0x060001A5 RID: 421 RVA: 0x0001023C File Offset: 0x0000E43C
			public void Execute(int vindex)
			{
				float3 @float = this.toPositions[vindex];
				VertexAttribute vertexAttribute = VertexAttribute.Invalid;
				float num = float.MaxValue;
				foreach (int3 @int in GridMap<int>.GetArea(@float, this.radius, this.gridMap, this.gridSize))
				{
					if (this.gridMap.ContainsKey(@int))
					{
						foreach (int num2 in this.gridMap.GetValuesForKey(@int))
						{
							float3 float2 = this.fromPositions[num2];
							float num3 = math.distance(@float, float2);
							if (num3 <= this.radius && num3 <= num)
							{
								num = num3;
								vertexAttribute = this.fromAttributes[num2];
							}
						}
					}
				}
				this.toAttributes[vindex] = vertexAttribute;
			}

			// Token: 0x0400026C RID: 620
			public float gridSize;

			// Token: 0x0400026D RID: 621
			public float radius;

			// Token: 0x0400026E RID: 622
			[ReadOnly]
			public NativeArray<float3> toPositions;

			// Token: 0x0400026F RID: 623
			[WriteOnly]
			public NativeArray<VertexAttribute> toAttributes;

			// Token: 0x04000270 RID: 624
			[ReadOnly]
			public NativeParallelMultiHashMap<int3, int> gridMap;

			// Token: 0x04000271 RID: 625
			[ReadOnly]
			public NativeArray<float3> fromPositions;

			// Token: 0x04000272 RID: 626
			[ReadOnly]
			public NativeArray<VertexAttribute> fromAttributes;
		}
	}
}
