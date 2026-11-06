using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace MagicaCloth2
{
	// Token: 0x02000123 RID: 291
	public class VirtualMesh : IDisposable, IValid
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x00030268 File Offset: 0x0002E468
		public void ImportFrom(RenderSetupData rsetup, int uvChannel)
		{
			try
			{
				if (rsetup == null)
				{
					this.result.SetError(Define.Result.VirtualMesh_InvalidSetup);
					throw new MagicaClothProcessingException();
				}
				if (rsetup.IsFaild())
				{
					this.result.SetError(Define.Result.VirtualMesh_InvalidSetup);
					throw new MagicaClothProcessingException();
				}
				if (this.transformData == null)
				{
					this.transformData = new TransformData(rsetup.TransformCount);
				}
				int[] array = this.transformData.AddTransformRange(rsetup.transformList, rsetup.transformIdList, rsetup.transformParentIdList, rsetup.rootTransformIdList, rsetup.transformLocalPositions, rsetup.transformLocalRotations, rsetup.transformPositions, rsetup.transformRotations, rsetup.transformScales, rsetup.transformInverseRotations);
				this.centerTransformIndex = array[rsetup.renderTransformIndex];
				this.initLocalToWorld = rsetup.initRenderLocalToWorld;
				this.initWorldToLocal = rsetup.initRenderWorldtoLocal;
				this.initRotation = rsetup.initRenderRotation;
				this.initInverseRotation = math.inverse(this.initRotation);
				this.initScale = rsetup.initRenderScale;
				if (rsetup.setupType == RenderSetupData.SetupType.MeshCloth)
				{
					this.meshType = VirtualMesh.MeshType.NormalMesh;
					this.isBoneCloth = false;
					this.ImportMeshType(rsetup, array, uvChannel);
					if (rsetup.hasBoneWeight)
					{
						this.ImportMeshSkinning();
					}
				}
				else
				{
					if (rsetup.setupType != RenderSetupData.SetupType.BoneCloth && rsetup.setupType != RenderSetupData.SetupType.BoneSpring)
					{
						this.result.SetError(Define.Result.RenderSetup_InvalidType);
						throw new IndexOutOfRangeException();
					}
					this.meshType = VirtualMesh.MeshType.NormalBoneMesh;
					this.isBoneCloth = true;
					this.ImportBoneType(rsetup, array);
				}
				this.boundingBox = new NativeReference<AABB>(Allocator.Persistent, NativeArrayOptions.ClearMemory);
				JobUtility.CalcAABBRun(this.localPositions.GetNativeArray(), this.VertexCount, this.boundingBox);
				RenderSetupData.SetupType setupType = rsetup.setupType;
				if (setupType - RenderSetupData.SetupType.BoneCloth <= 1 && this.TriangleCount > 0)
				{
					JobUtility.CalcUVWithSphereMappingRun(this.localPositions.GetNativeArray(), this.VertexCount, this.boundingBox, this.uv.GetNativeArray());
				}
				this.CalcAverageAndMaxVertexDistanceRun();
			}
			catch (Exception)
			{
				if (this.result.IsNone())
				{
					this.result.SetError(Define.Result.VirtualMesh_ImportError);
				}
				throw;
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0003047C File Offset: 0x0002E67C
		private void ImportMeshType(RenderSetupData rsetup, int[] transformIndices, int uvChannel)
		{
			this.skinRootIndex = transformIndices[rsetup.skinRootBoneIndex];
			this.skinBoneTransformIndices.AddRange(transformIndices, rsetup.skinBoneCount);
			this.skinBoneBindPoses.AddRange<Matrix4x4>(rsetup.bindPoseList.ToArray());
			Mesh.MeshData meshData = rsetup.meshDataArray[0];
			int vertexCount = meshData.vertexCount;
			this.localPositions.AddRange(vertexCount);
			this.localNormals.AddRange(vertexCount);
			this.localTangents.AddRange(vertexCount);
			this.uv.AddRange(vertexCount);
			this.boneWeights.AddRange(vertexCount);
			meshData.GetVertices(this.localPositions.GetNativeArray<Vector3>());
			meshData.GetNormals(this.localNormals.GetNativeArray<Vector3>());
			if (meshData.HasVertexAttribute(VertexAttribute.Tangent))
			{
				using (NativeArray<Vector4> nativeArray = new NativeArray<Vector4>(vertexCount, Allocator.TempJob, NativeArrayOptions.ClearMemory))
				{
					meshData.GetTangents(nativeArray);
					this.localTangents.CopyFromWithTypeChangeStride<Vector4>(nativeArray);
					goto IL_0117;
				}
			}
			new VirtualMesh.Import_GenerateTangentJob
			{
				localNormals = this.localNormals.GetNativeArray(),
				localTangents = this.localTangents.GetNativeArray()
			}.Run(vertexCount);
			IL_0117:
			if (meshData.HasVertexAttribute(VertexAttribute.TexCoord0))
			{
				uvChannel = Mathf.Clamp(uvChannel, 0, 7);
				VertexAttribute vertexAttribute = VertexAttribute.TexCoord0 + uvChannel;
				if (!meshData.HasVertexAttribute(vertexAttribute))
				{
					object obj = string.Format("[{0}] UV{1} not found! => Use UV0.", this.name, uvChannel);
					Develop.LogWarning(in obj);
					uvChannel = 0;
				}
				meshData.GetUVs(uvChannel, this.uv.GetNativeArray<Vector2>());
			}
			else
			{
				object obj = "[" + this.name + "] UV0 not found!";
				Develop.LogWarning(in obj);
			}
			this.attributes.AddRange(vertexCount);
			this.referenceIndices.AddRange(vertexCount);
			using (NativeArray<int> nativeArray2 = new NativeArray<int>(vertexCount, Allocator.TempJob, NativeArrayOptions.ClearMemory))
			{
				JobUtility.SerialNumberRun(this.referenceIndices.GetNativeArray(), vertexCount);
				if (rsetup.hasBoneWeight)
				{
					new VirtualMesh.Import_BoneWeightJob1
					{
						vcnt = vertexCount,
						bonesPerVertexArray = rsetup.bonesPerVertexArray,
						startBoneWeightIndices = nativeArray2
					}.Run<VirtualMesh.Import_BoneWeightJob1>();
					new VirtualMesh.Import_BoneWeightJob2
					{
						startBoneWeightIndices = nativeArray2,
						boneWeightArray = rsetup.boneWeightArray,
						bonesPerVertexArray = rsetup.bonesPerVertexArray,
						boneWeights = this.boneWeights.GetNativeArray()
					}.Run(vertexCount);
				}
				else
				{
					JobUtility.FillRun(this.boneWeights.GetNativeArray(), vertexCount, new VirtualMeshBoneWeight(0, new float4(1f, 0f, 0f, 0f)));
				}
				for (int i = 0; i < meshData.subMeshCount; i++)
				{
					using (NativeArray<int> nativeArray3 = new NativeArray<int>(meshData.GetSubMesh(i).indexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory))
					{
						meshData.GetIndices(nativeArray3, i, true);
						this.triangles.AddRangeTypeChange<int>(nativeArray3);
					}
				}
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000307B4 File Offset: 0x0002E9B4
		private void ImportMeshSkinning()
		{
			new VirtualMesh.Import_CalcSkinningJob
			{
				localPositions = this.localPositions.GetNativeArray(),
				localNormals = this.localNormals.GetNativeArray(),
				localTangents = this.localTangents.GetNativeArray(),
				boneWeights = this.boneWeights.GetNativeArray(),
				skinBoneTransformIndices = this.skinBoneTransformIndices.GetNativeArray(),
				bindPoses = this.skinBoneBindPoses.GetNativeArray(),
				transformPositionArray = this.transformData.positionArray.GetNativeArray(),
				transformRotationArray = this.transformData.rotationArray.GetNativeArray(),
				transformScaleArray = this.transformData.scaleArray.GetNativeArray(),
				toM = this.initWorldToLocal
			}.Run(this.VertexCount);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00030894 File Offset: 0x0002EA94
		private void ImportBoneType(RenderSetupData rsetup, int[] transformIndices)
		{
			int num = rsetup.TransformCount - 1;
			this.localPositions.AddRange(num);
			this.localNormals.AddRange(num);
			this.localTangents.AddRange(num);
			this.uv.AddRange(num);
			this.boneWeights.AddRange(num);
			this.attributes.AddRange(num);
			this.referenceIndices.AddRange(num);
			this.skinBoneTransformIndices.AddRange(transformIndices, rsetup.skinBoneCount);
			this.skinBoneBindPoses.AddRange(num);
			float4x4 initRenderWorldtoLocal = rsetup.initRenderWorldtoLocal;
			float4x4 initRenderLocalToWorld = rsetup.initRenderLocalToWorld;
			new VirtualMesh.Import_BoneVertexJob
			{
				WtoL = initRenderWorldtoLocal,
				LtoW = initRenderLocalToWorld,
				transformPositions = rsetup.transformPositions,
				transformRotations = rsetup.transformRotations,
				transformScales = rsetup.transformScales,
				localPositions = this.localPositions.GetNativeArray(),
				localNormals = this.localNormals.GetNativeArray(),
				localTangents = this.localTangents.GetNativeArray(),
				boneWeights = this.boneWeights.GetNativeArray(),
				skinBoneBindPoses = this.skinBoneBindPoses.GetNativeArray()
			}.Run(num);
			JobUtility.SerialNumberRun(this.referenceIndices.GetNativeArray(), num);
			if (rsetup.boneConnectionMode == RenderSetupData.BoneConnectionMode.Line)
			{
				List<int2> list = new List<int2>(num);
				for (int i = 0; i < num; i++)
				{
					int parentTransformIndex = rsetup.GetParentTransformIndex(i, true);
					if (parentTransformIndex >= 0)
					{
						int2 @int = DataUtility.PackInt2(parentTransformIndex, i);
						list.Add(@int);
					}
				}
				if (rsetup.setupType == RenderSetupData.SetupType.BoneSpring)
				{
					this.attributes.Fill(0, num, VertexAttribute.DisableCollision);
					if (rsetup.collisionBoneIndexList != null)
					{
						foreach (int num2 in rsetup.collisionBoneIndexList)
						{
							if (num2 >= 0)
							{
								this.attributes[num2] = VertexAttribute.Invalid;
							}
						}
					}
				}
				if (list.Count > 0)
				{
					this.lines = new ExSimpleNativeArray<int2>(list.ToArray());
					return;
				}
			}
			else
			{
				Dictionary<int, int> dictionary = new Dictionary<int, int>(num);
				for (int j = 0; j < num; j++)
				{
					if (!dictionary.ContainsKey(rsetup.transformIdList[j]))
					{
						dictionary.Add(rsetup.transformIdList[j], j);
					}
				}
				bool flag = rsetup.boneConnectionMode == RenderSetupData.BoneConnectionMode.SequentialLoopMesh;
				bool flag2 = rsetup.boneConnectionMode == RenderSetupData.BoneConnectionMode.SequentialLoopMesh || rsetup.boneConnectionMode == RenderSetupData.BoneConnectionMode.SequentialNonLoopMesh;
				List<int> list2 = new List<int>(rsetup.rootTransformIdList);
				int count = list2.Count;
				int num3 = count - 1;
				if (rsetup.boneConnectionMode == RenderSetupData.BoneConnectionMode.AutomaticMesh)
				{
					List<int> list3 = new List<int>(list2);
					list2.Clear();
					list2.Add(list3[0]);
					float num4 = 0f;
					while (list3.Count > 0)
					{
						int num5 = list2[list2.Count - 1];
						list3.Remove(num5);
						int num6 = dictionary[num5];
						float3 @float = this.localPositions[num6];
						float num7 = float.MaxValue;
						int num8 = 0;
						for (int k = 0; k < list3.Count; k++)
						{
							int num9 = list3[k];
							int num10 = dictionary[num9];
							float3 float2 = this.localPositions[num10];
							float num11 = math.distance(@float, float2);
							if (num11 < num7)
							{
								num7 = num11;
								num8 = num9;
							}
						}
						if (num8 != 0)
						{
							if (num4 == 0f || num7 < num4 * 1.5f)
							{
								list2.Add(num8);
								num4 = ((num4 == 0f) ? num7 : ((num4 + num7) * 0.5f));
							}
							else
							{
								list2.Reverse();
								num4 = 0f;
							}
						}
					}
					if (list2.Count >= 3)
					{
						int num12 = list2[0];
						int num13 = list2[list2.Count - 1];
						int num14 = dictionary[num12];
						int num15 = dictionary[num13];
						float3 float3 = this.localPositions[num14];
						float3 float4 = this.localPositions[num15];
						if (math.distance(float3, float4) < num4 * 1.5f)
						{
							flag = true;
						}
					}
				}
				FixedList128Bytes<int>[] array = new FixedList128Bytes<int>[num];
				int[] array2 = new int[num];
				int[] array3 = new int[num];
				List<FixedList512Bytes<int>> list4 = new List<FixedList512Bytes<int>>();
				HashSet<uint> hashSet = new HashSet<uint>();
				Stack<int> stack = new Stack<int>(num);
				Stack<int> stack2 = new Stack<int>(num);
				for (int l = 0; l < count; l++)
				{
					stack.Clear();
					stack.Push(list2[l]);
					stack2.Clear();
					stack2.Push(0);
					while (stack.Count > 0)
					{
						int num16 = stack.Pop();
						int num17 = stack2.Pop();
						int num18 = dictionary[num16];
						float3 float5 = this.localPositions[num18];
						if (list4.Count <= num17)
						{
							list4.Add(default(FixedList512Bytes<int>));
						}
						FixedList512Bytes<int> fixedList512Bytes = list4[num17];
						fixedList512Bytes.Add(in num18);
						list4[num17] = fixedList512Bytes;
						FixedList128Bytes<int> fixedList128Bytes = default(FixedList128Bytes<int>);
						int num19 = rsetup.transformParentIdList[num18];
						if (dictionary.ContainsKey(num19))
						{
							int num20 = dictionary[num19];
							fixedList128Bytes.Add(in num20);
							uint num21 = DataUtility.Pack32Sort(num18, num20);
							hashSet.Add(num21);
						}
						FixedList512Bytes<int> fixedList512Bytes2 = rsetup.transformChildIdList[num18];
						if (fixedList512Bytes2.Length > 0)
						{
							for (int m = 0; m < fixedList512Bytes2.Length; m++)
							{
								int num22 = fixedList512Bytes2[m];
								stack.Push(num22);
								stack2.Push(num17 + 1);
								int num23 = dictionary[num22];
								fixedList128Bytes.Add(in num23);
								uint num24 = DataUtility.Pack32Sort(num18, num23);
								hashSet.Add(num24);
							}
						}
						array[num18] = fixedList128Bytes;
						array2[num18] = num17;
						array3[num18] = l;
					}
				}
				uint num25 = DataUtility.Pack32Sort(0, num3);
				for (int n = 0; n < num; n++)
				{
					int num26 = array2[n];
					FixedList512Bytes<int> fixedList512Bytes3 = list4[num26];
					float3 float6 = this.localPositions[n];
					FixedList128Bytes<int> fixedList128Bytes2 = array[n];
					int num27 = array3[n];
					float num28 = float.MaxValue;
					int num29 = -1;
					foreach (int num30 in fixedList512Bytes3)
					{
						if (num30 != n)
						{
							int num31 = array3[num30];
							bool flag3 = num25 == DataUtility.Pack32Sort(num27, num31) && num25 > 0U;
							if ((flag || !flag3) && (!flag2 || (flag && flag3) || math.abs(num27 - num31) <= 1))
							{
								float3 float7 = this.localPositions[num30];
								float num32 = math.distance(float6, float7);
								if (num32 < num28)
								{
									num28 = num32;
									num29 = num30;
								}
							}
						}
					}
					if (num29 >= 0)
					{
						fixedList128Bytes2.Add(in num29);
						num28 = (flag2 ? float.MaxValue : (num28 * 1.5f));
						foreach (int num33 in fixedList512Bytes3)
						{
							if (num33 != n && num33 != num29)
							{
								int num34 = array3[num33];
								bool flag4 = num25 == DataUtility.Pack32Sort(num27, num34) && num25 > 0U;
								if ((flag || !flag4) && (!flag2 || (flag && flag4) || math.abs(num27 - num34) <= 1))
								{
									float3 float8 = this.localPositions[num33];
									if (math.distance(float6, float8) <= num28)
									{
										fixedList128Bytes2.Add(in num33);
									}
								}
							}
						}
					}
					array[n] = fixedList128Bytes2;
				}
				HashSet<int2> hashSet2 = new HashSet<int2>();
				HashSet<int2> hashSet3 = new HashSet<int2>();
				HashSet<int3> hashSet4 = new HashSet<int3>();
				for (int num35 = 0; num35 < num; num35++)
				{
					FixedList128Bytes<int> fixedList128Bytes3 = array[num35];
					if (fixedList128Bytes3.Length == 0)
					{
						Debug.LogError(string.Format("Connection 0! [{0}]", num35));
					}
					else if (fixedList128Bytes3.Length == 1)
					{
						hashSet2.Add(DataUtility.PackInt2(num35, fixedList128Bytes3[0]));
					}
					else
					{
						for (int num36 = 0; num36 < fixedList128Bytes3.Length; num36++)
						{
							int num37 = fixedList128Bytes3[num36];
							hashSet2.Add(DataUtility.PackInt2(num35, num37));
						}
						int num38 = array3[num35];
						float3 float9 = this.localPositions[num35];
						for (int num39 = 0; num39 < fixedList128Bytes3.Length - 1; num39++)
						{
							int num40 = fixedList128Bytes3[num39];
							float3 float10 = this.localPositions[num40] - float9;
							for (int num41 = num39 + 1; num41 < fixedList128Bytes3.Length; num41++)
							{
								int num42 = fixedList128Bytes3[num41];
								float3 float11 = this.localPositions[num42] - float9;
								if (math.lengthsq(float10) >= 1E-06f && math.lengthsq(float11) >= 1E-06f && math.degrees(MathUtility.Angle(in float10, in float11)) < 120f)
								{
									int num43 = array3[num40];
									int num44 = array3[num42];
									if ((num43 == num38 || num44 == num38 || num43 == num44) && false + hashSet.Contains(DataUtility.Pack32Sort(num35, num40)) + hashSet.Contains(DataUtility.Pack32Sort(num35, num42)) + hashSet.Contains(DataUtility.Pack32Sort(num40, num42)))
									{
										int3 int2 = DataUtility.PackInt3(num35, num40, num42);
										if (!hashSet4.Contains(int2))
										{
											hashSet4.Add(int2);
											hashSet3.Add(DataUtility.PackInt2(num35, num40));
											hashSet3.Add(DataUtility.PackInt2(num35, num42));
										}
									}
								}
							}
						}
					}
				}
				if (hashSet4.Count > 0)
				{
					this.triangles = new ExSimpleNativeArray<int3>(hashSet4.Count, false);
					int num45 = 0;
					foreach (int3 int3 in hashSet4)
					{
						this.triangles[num45] = int3;
						num45++;
					}
				}
				foreach (int2 int4 in hashSet3)
				{
					hashSet2.Remove(int4);
				}
				if (hashSet2.Count > 0)
				{
					this.lines = new ExSimpleNativeArray<int2>(hashSet2.Count, false);
					int num46 = 0;
					foreach (int2 int5 in hashSet2)
					{
						this.lines[num46] = int5;
						num46++;
					}
				}
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000313C8 File Offset: 0x0002F5C8
		public void ImportFrom(RenderData renderData, int uvChannel)
		{
			try
			{
				if (renderData == null)
				{
					this.result.SetError(Define.Result.VirtualMesh_InvalidRenderData);
					throw new MagicaClothProcessingException();
				}
				this.ImportFrom(renderData.setupData, uvChannel);
			}
			catch (MagicaClothProcessingException)
			{
				if (!this.result.IsError())
				{
					this.result.SetError(Define.Result.VirtualMesh_ImportError);
				}
				throw;
			}
			catch (Exception)
			{
				this.result.SetError(Define.Result.VirtualMesh_ImportError);
				throw;
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0003144C File Offset: 0x0002F64C
		public void SelectionMesh(SelectionData selectionData, float4x4 selectionLocalToWorldMatrix, float mergin)
		{
			try
			{
				if (selectionData == null || !selectionData.IsValid())
				{
					this.result.SetError(Define.Result.VirtualMesh_InvalidSelection);
					throw new MagicaClothProcessingException();
				}
				int count = selectionData.Count;
				NativeArray<float3> positionNativeArray = selectionData.GetPositionNativeArray();
				try
				{
					NativeArray<VertexAttribute> attributeNativeArray = selectionData.GetAttributeNativeArray();
					try
					{
						if (!MathUtility.CompareMatrix(in selectionLocalToWorldMatrix, in this.initLocalToWorld))
						{
							float4x4 float4x = MathUtility.Transform(in selectionLocalToWorldMatrix, in this.initWorldToLocal);
							JobUtility.TransformPositionRun(positionNativeArray, count, in float4x);
						}
						float num = mergin * 1f;
						using (GridMap<int> gridMap = SelectionData.CreateGridMapRun(num, in positionNativeArray, in attributeNativeArray, true, true, true, false))
						{
							using (NativeList<int3> nativeList = new NativeList<int3>(this.TriangleCount, Allocator.Persistent))
							{
								using (NativeArray<int> nativeArray = new NativeArray<int>(this.VertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory))
								{
									using (NativeReference<int> nativeReference = new NativeReference<int>(Allocator.Persistent, NativeArrayOptions.ClearMemory))
									{
										new VirtualMesh.Select_GridJob
										{
											gridSize = num,
											gridMap = gridMap.GetMultiHashMap(),
											selectionCount = count,
											selectionPositions = positionNativeArray,
											selectionAttributes = attributeNativeArray,
											vertexCount = this.VertexCount,
											triangleCount = this.TriangleCount,
											searchRadius = mergin,
											meshPositions = this.localPositions.GetNativeArray(),
											meshTriangles = this.triangles.GetNativeArray(),
											newTriangles = nativeList,
											newVertexRemapIndices = nativeArray,
											newVertexCount = nativeReference
										}.Run<VirtualMesh.Select_GridJob>();
										if (nativeReference.Value < this.VertexCount)
										{
											int value = nativeReference.Value;
											ExSimpleNativeArray<int> exSimpleNativeArray = new ExSimpleNativeArray<int>(value, false);
											ExSimpleNativeArray<VertexAttribute> exSimpleNativeArray2 = new ExSimpleNativeArray<VertexAttribute>(value, false);
											ExSimpleNativeArray<float3> exSimpleNativeArray3 = new ExSimpleNativeArray<float3>(value, false);
											ExSimpleNativeArray<float3> exSimpleNativeArray4 = new ExSimpleNativeArray<float3>(value, false);
											ExSimpleNativeArray<float3> exSimpleNativeArray5 = new ExSimpleNativeArray<float3>(value, false);
											ExSimpleNativeArray<float2> exSimpleNativeArray6 = new ExSimpleNativeArray<float2>(value, false);
											ExSimpleNativeArray<VirtualMeshBoneWeight> exSimpleNativeArray7 = new ExSimpleNativeArray<VirtualMeshBoneWeight>(value, false);
											new VirtualMesh.Select_PackVertexJob
											{
												vertexCount = this.VertexCount,
												newVertexRemapIndices = nativeArray,
												attributes = this.attributes.GetNativeArray(),
												localPositions = this.localPositions.GetNativeArray(),
												localNormals = this.localNormals.GetNativeArray(),
												localTangents = this.localTangents.GetNativeArray(),
												uv = this.uv.GetNativeArray(),
												boneWeights = this.boneWeights.GetNativeArray(),
												newReferenceIndices = exSimpleNativeArray.GetNativeArray(),
												newAttributes = exSimpleNativeArray2.GetNativeArray(),
												newLocalPositions = exSimpleNativeArray3.GetNativeArray(),
												newLocalNormals = exSimpleNativeArray4.GetNativeArray(),
												newLocalTangents = exSimpleNativeArray5.GetNativeArray(),
												newUv = exSimpleNativeArray6.GetNativeArray(),
												newBoneWeights = exSimpleNativeArray7.GetNativeArray()
											}.Run<VirtualMesh.Select_PackVertexJob>();
											this.referenceIndices.Dispose();
											this.attributes.Dispose();
											this.localPositions.Dispose();
											this.localNormals.Dispose();
											this.localTangents.Dispose();
											this.uv.Dispose();
											this.boneWeights.Dispose();
											this.referenceIndices = exSimpleNativeArray;
											this.attributes = exSimpleNativeArray2;
											this.localPositions = exSimpleNativeArray3;
											this.localNormals = exSimpleNativeArray4;
											this.localTangents = exSimpleNativeArray5;
											this.uv = exSimpleNativeArray6;
											this.boneWeights = exSimpleNativeArray7;
											ExSimpleNativeArray<int3> exSimpleNativeArray8 = new ExSimpleNativeArray<int3>(nativeList);
											this.triangles.Dispose();
											this.triangles = exSimpleNativeArray8;
										}
									}
								}
							}
						}
					}
					finally
					{
						((IDisposable)attributeNativeArray).Dispose();
					}
				}
				finally
				{
					((IDisposable)positionNativeArray).Dispose();
				}
			}
			catch (MagicaClothProcessingException)
			{
				if (!this.result.IsError())
				{
					this.result.SetError(Define.Result.VirtualMesh_SelectionUnknownError);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				this.result.SetError(Define.Result.VirtualMesh_SelectionException);
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000318E4 File Offset: 0x0002FAE4
		public float CalcSelectionMergin(ReductionSettings settings)
		{
			float value = this.averageVertexDistance.Value;
			float num = 0f;
			if (settings != null && settings.IsEnabled)
			{
				num = this.boundingBox.Value.MaxSideLength * settings.GetMaxConnectionDistance();
			}
			return math.max(value, num) * 1.5f;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00031934 File Offset: 0x0002FB34
		public void AddMesh(VirtualMesh cmesh)
		{
			try
			{
				if (this.IsError)
				{
					throw new InvalidOperationException();
				}
				if (cmesh == null || !cmesh.IsSuccess || cmesh.IsError)
				{
					throw new InvalidOperationException();
				}
				int skinBoneCount = this.SkinBoneCount;
				int vertexCount = this.VertexCount;
				int triangleCount = this.TriangleCount;
				int lineCount = this.LineCount;
				int count = this.transformData.Count;
				float4x4 float4x = cmesh.CenterTransformTo(this);
				int skinBoneCount2 = cmesh.SkinBoneCount;
				this.skinBoneTransformIndices.AddRange(skinBoneCount2);
				for (int i = 0; i < skinBoneCount2; i++)
				{
					int num = cmesh.skinBoneTransformIndices[i];
					int num2 = this.transformData.AddTransform(cmesh.transformData, num, true);
					this.skinBoneTransformIndices[skinBoneCount + i] = num2;
				}
				int vertexCount2 = cmesh.VertexCount;
				cmesh.mergeChunk = new DataChunk(this.VertexCount, vertexCount2);
				this.attributes.AddRange(vertexCount2);
				this.localPositions.AddRange(vertexCount2);
				this.localNormals.AddRange(vertexCount2);
				this.localTangents.AddRange(vertexCount2);
				this.uv.AddRange(vertexCount2);
				this.boneWeights.AddRange(vertexCount2);
				new VirtualMesh.Add_CopyVerticesJob
				{
					vertexOffset = vertexCount,
					skinBoneOffset = skinBoneCount,
					toM = float4x,
					srcAttributes = cmesh.attributes.GetNativeArray(),
					srclocalPositions = cmesh.localPositions.GetNativeArray(),
					srclocalNormals = cmesh.localNormals.GetNativeArray(),
					srclocalTangents = cmesh.localTangents.GetNativeArray(),
					srcUV = cmesh.uv.GetNativeArray(),
					srcBoneWeights = cmesh.boneWeights.GetNativeArray(),
					dstAttributes = this.attributes.GetNativeArray(),
					dstlocalPositions = this.localPositions.GetNativeArray(),
					dstlocalNormals = this.localNormals.GetNativeArray(),
					dstlocalTangents = this.localTangents.GetNativeArray(),
					dstUV = this.uv.GetNativeArray(),
					dstBoneWeights = this.boneWeights.GetNativeArray(),
					dstSkinBoneIndices = this.skinBoneTransformIndices.GetNativeArray()
				}.Run(cmesh.VertexCount);
				this.skinBoneBindPoses.AddRange(skinBoneCount2);
				new VirtualMesh.Add_CalcBindPoseJob
				{
					skinBoneOffset = skinBoneCount,
					srcSkinBoneTransformIndices = cmesh.skinBoneTransformIndices.GetNativeArray(),
					srcTransformPositionArray = cmesh.transformData.positionArray.GetNativeArray(),
					srcTransformRotationArray = cmesh.transformData.rotationArray.GetNativeArray(),
					srcTransformScaleArray = cmesh.transformData.scaleArray.GetNativeArray(),
					dstCenterLocalToWorldMatrix = this.initLocalToWorld,
					dstSkinBoneBindPoses = this.skinBoneBindPoses.GetNativeArray()
				}.Run(skinBoneCount2);
				if (cmesh.TriangleCount > 0)
				{
					this.triangles.AddRange(cmesh.TriangleCount);
					new JobUtility.AddInt3DataCopyJob
					{
						dstOffset = triangleCount,
						addData = vertexCount,
						srcData = cmesh.triangles.GetNativeArray(),
						dstData = this.triangles.GetNativeArray()
					}.Run(cmesh.TriangleCount);
				}
				if (cmesh.LineCount > 0)
				{
					this.lines.AddRange(cmesh.LineCount);
					new JobUtility.AddInt2DataCopyJob
					{
						dstOffset = lineCount,
						addData = vertexCount,
						srcData = cmesh.lines.GetNativeArray(),
						dstData = this.lines.GetNativeArray()
					}.Run(cmesh.LineCount);
				}
				float3 @float = cmesh.boundingBox.Value.Min;
				float3 float2 = cmesh.boundingBox.Value.Max;
				@float = math.transform(float4x, @float);
				float2 = math.transform(float4x, float2);
				AABB aabb = new AABB(in @float, in float2);
				if (this.boundingBox.IsCreated)
				{
					AABB value = this.boundingBox.Value;
					value.Encapsulate(in aabb);
					this.boundingBox.Value = value;
				}
				else
				{
					this.boundingBox = new NativeReference<AABB>(aabb, Allocator.Persistent);
				}
				float num3 = math.length(cmesh.initScale) / math.length(this.initScale);
				this.averageVertexDistance.Value = math.max(this.averageVertexDistance.Value, cmesh.averageVertexDistance.Value * num3);
				this.maxVertexDistance.Value = math.max(this.maxVertexDistance.Value, cmesh.maxVertexDistance.Value * num3);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				this.result.SetError(Define.Result.Error);
				throw;
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00031E0C File Offset: 0x0003000C
		public void SetTransform(Transform center, Transform skinRoot = null, int centerId = 0, int skinRootId = 0)
		{
			this.SetCenterTransform(center, centerId);
			if (skinRoot != null)
			{
				this.SetSkinRoot(skinRoot, skinRootId);
			}
			else
			{
				this.SetSkinRoot(center, centerId);
			}
			this.initLocalToWorld = center.localToWorldMatrix;
			this.initWorldToLocal = math.inverse(this.initLocalToWorld);
			this.initRotation = center.rotation;
			this.initInverseRotation = math.inverse(this.initRotation);
			this.initScale = center.lossyScale;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00031E94 File Offset: 0x00030094
		public void SetTransform(TransformRecord centerRecord, TransformRecord skinRootRecord = null)
		{
			this.centerTransformIndex = this.transformData.AddTransform(centerRecord, 0, 1, true);
			if (skinRootRecord != null)
			{
				this.skinRootIndex = this.transformData.AddTransform(skinRootRecord, 0, 1, true);
			}
			else
			{
				this.skinRootIndex = this.centerTransformIndex;
			}
			this.initLocalToWorld = centerRecord.localToWorldMatrix;
			this.initWorldToLocal = centerRecord.worldToLocalMatrix;
			this.initRotation = centerRecord.rotation;
			this.initInverseRotation = math.inverse(this.initRotation);
			this.initScale = centerRecord.scale;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00031F31 File Offset: 0x00030131
		public void SetCenterTransform(Transform t, int tid = 0)
		{
			if (t)
			{
				if (this.centerTransformIndex >= 0)
				{
					this.transformData.ReplaceTransform(this.centerTransformIndex, t, tid, 0, 1);
					return;
				}
				this.centerTransformIndex = this.transformData.AddTransform(t, tid, 0, 1, true);
			}
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00031F71 File Offset: 0x00030171
		public void SetSkinRoot(Transform t, int tid = 0)
		{
			if (t)
			{
				if (this.skinRootIndex >= 0)
				{
					this.transformData.ReplaceTransform(this.skinRootIndex, t, tid, 0, 1);
					return;
				}
				this.skinRootIndex = this.transformData.AddTransform(t, tid, 0, 1, true);
			}
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00031FB1 File Offset: 0x000301B1
		public Transform GetCenterTransform()
		{
			return this.transformData.GetTransformFromIndex(this.centerTransformIndex);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00031FC4 File Offset: 0x000301C4
		public void SetCustomSkinningBones(TransformRecord clothTransformRecord, List<TransformRecord> bones)
		{
			if (bones == null || bones.Count == 0)
			{
				return;
			}
			this.customSkinningBoneIndices = new int[bones.Count];
			for (int i = 0; i < bones.Count; i++)
			{
				TransformRecord transformRecord = bones[i];
				int num = -1;
				if (transformRecord.IsValid())
				{
					transformRecord.localPosition = clothTransformRecord.worldToLocalMatrix.MultiplyPoint(transformRecord.position);
					num = this.skinBoneTransformIndices.Count;
					int num2 = this.transformData.AddTransform(transformRecord, 0, 1, false);
					this.skinBoneTransformIndices.Add(num2);
					float4x4 float4x = math.mul(transformRecord.worldToLocalMatrix, this.initLocalToWorld);
					this.skinBoneBindPoses.Add(float4x);
				}
				this.customSkinningBoneIndices[i] = num;
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00032084 File Offset: 0x00030284
		public bool CompareSpace(VirtualMesh target)
		{
			return MathUtility.CompareMatrix(in this.initLocalToWorld, in target.initLocalToWorld);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00032097 File Offset: 0x00030297
		public float4x4 CenterTransformTo(VirtualMesh to)
		{
			if (!this.CompareSpace(to))
			{
				return MathUtility.Transform(in this.initLocalToWorld, in to.initWorldToLocal);
			}
			return float4x4.identity;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x000320BC File Offset: 0x000302BC
		public void Mapping(VirtualMesh proxyMesh)
		{
			try
			{
				if (this.IsError)
				{
					throw new MagicaClothProcessingException();
				}
				if (proxyMesh == null || !proxyMesh.IsSuccess)
				{
					this.result.SetError(Define.Result.MappingMesh_ProxyError);
					throw new MagicaClothProcessingException();
				}
				this.result.SetProcess();
				float4x4 float4x = this.CenterTransformTo(proxyMesh);
				using (NativeArray<VirtualMesh.MappingWorkData> nativeArray = new NativeArray<VirtualMesh.MappingWorkData>(this.VertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory))
				{
					if (this.mergeChunk.IsValid)
					{
						new VirtualMesh.Mapping_DirectConnectionVertexDataJob
						{
							toP = float4x,
							vcnt = this.VertexCount,
							mergeChunk = this.mergeChunk,
							localPositions = this.localPositions.GetNativeArray(),
							attributes = this.attributes.GetNativeArray(),
							joinIndices = proxyMesh.joinIndices,
							proxyAttributes = proxyMesh.attributes.GetNativeArray(),
							proxyLocalPositions = proxyMesh.localPositions.GetNativeArray(),
							mappingWorkData = nativeArray
						}.Run<VirtualMesh.Mapping_DirectConnectionVertexDataJob>();
						float num = proxyMesh.averageVertexDistance.Value * 1.5f;
						using (NativeParallelHashSet<ushort> nativeParallelHashSet = new NativeParallelHashSet<ushort>(1024, Allocator.Persistent))
						{
							new VirtualMesh.Mapping_CalcDirectWeightJob
							{
								vcnt = nativeArray.Length,
								weightLength = num,
								mappingWorkData = nativeArray,
								attributes = this.attributes.GetNativeArray(),
								boneWeights = this.boneWeights.GetNativeArray(),
								proxyLocalPositions = proxyMesh.localPositions.GetNativeArray(),
								proxyVertexToVertexIndexArray = proxyMesh.vertexToVertexIndexArray,
								proxyVertexToVertexDataArray = proxyMesh.vertexToVertexDataArray,
								useSet = nativeParallelHashSet
							}.Run<VirtualMesh.Mapping_CalcDirectWeightJob>();
							goto IL_0375;
						}
					}
					float num2 = math.max(MathUtility.TransformLength(this.averageVertexDistance.Value, in float4x), 1E-05f);
					float num3 = num2 * 2.5f;
					float num4 = num2 * 1.5f;
					using (GridMap<int> gridMap = proxyMesh.CreateVertexIndexGridMapRun(num4))
					{
						new VirtualMesh.Mapping_CalcConnectionVertexDataJob
						{
							gridSize = num4,
							searchRadius = num3,
							toP = float4x,
							vcnt = this.VertexCount,
							localPositions = this.localPositions.GetNativeArray(),
							boneWeights = this.boneWeights.GetNativeArray(),
							transformIds = this.transformData.idArray.GetNativeArray(),
							attributes = this.attributes.GetNativeArray(),
							gridMap = gridMap.GetMultiHashMap(),
							proxyAttributes = proxyMesh.attributes.GetNativeArray(),
							proxyLocalPositions = proxyMesh.localPositions.GetNativeArray(),
							proxyBoneWeights = proxyMesh.boneWeights.GetNativeArray(),
							proxyTransformIds = proxyMesh.transformData.idArray.GetNativeArray(),
							mappingWorkData = nativeArray
						}.Run<VirtualMesh.Mapping_CalcConnectionVertexDataJob>();
						if (nativeArray.Length > 0)
						{
							new VirtualMesh.Mapping_CalcWeightJob
							{
								mappingWorkData = nativeArray,
								attributes = this.attributes.GetNativeArray(),
								boneWeights = this.boneWeights.GetNativeArray(),
								proxyAttributes = proxyMesh.attributes.GetNativeArray(),
								proxyLocalPositions = proxyMesh.localPositions.GetNativeArray(),
								proxyLocalNormals = proxyMesh.localNormals.GetNativeArray(),
								proxyVertexToVertexIndexArray = proxyMesh.vertexToVertexIndexArray,
								proxyVertexToVertexDataArray = proxyMesh.vertexToVertexDataArray
							}.Run(nativeArray.Length);
						}
					}
					IL_0375:
					this.mappingProxyMesh = proxyMesh;
					this.toProxyMatrix = float4x;
					this.toProxyRotation = math.mul(proxyMesh.initInverseRotation, this.initRotation);
					this.meshType = VirtualMesh.MeshType.Mapping;
					this.result.SetSuccess();
				}
			}
			catch (MagicaClothProcessingException)
			{
				if (!this.result.IsError())
				{
					this.result.SetError(Define.Result.MappingMesh_UnknownError);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				this.result.SetError(Define.Result.MappingMesh_Exception);
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0003253C File Offset: 0x0003073C
		private static float4 CalcVertexWeights(float4 distances)
		{
			distances = math.max(distances, 0);
			distances = math.pow(distances, 4f);
			float num = distances[0];
			for (int i = 0; i < 4; i++)
			{
				distances[i] = ((distances[i] > 0f) ? (num / distances[i]) : 0f);
			}
			float num2 = math.csum(distances);
			if (num2 <= 0f)
			{
				return new float4(1f, 0f, 0f, 0f);
			}
			distances /= num2;
			for (int j = 3; j >= 1; j--)
			{
				if (distances[j] < 0.01f)
				{
					distances[j] = 0f;
				}
			}
			distances /= math.csum(distances);
			return distances;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00032610 File Offset: 0x00030810
		public void Optimization()
		{
			try
			{
				this.RemoveDuplicateTriangles();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				this.result.SetError(Define.Result.Optimize_Exception);
			}
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0003264C File Offset: 0x0003084C
		private void RemoveDuplicateTriangles()
		{
			if (this.TriangleCount < 2)
			{
				return;
			}
			using (NativeParallelHashMap<int2, FixedList128Bytes<int>> nativeParallelHashMap = new NativeParallelHashMap<int2, FixedList128Bytes<int>>(this.TriangleCount * 2, Allocator.Persistent))
			{
				using (NativeList<int3> nativeList = new NativeList<int3>(this.TriangleCount, Allocator.Persistent))
				{
					using (NativeParallelHashSet<int4> nativeParallelHashSet = new NativeParallelHashSet<int4>(this.TriangleCount / 4, Allocator.Persistent))
					{
						using (NativeParallelHashSet<int3> nativeParallelHashSet2 = new NativeParallelHashSet<int3>(this.TriangleCount / 4, Allocator.Persistent))
						{
							new VirtualMesh.Optimize_EdgeToTrianlgeJob
							{
								tcnt = this.TriangleCount,
								triangles = this.triangles.GetNativeArray(),
								localPositions = this.localPositions.GetNativeArray(),
								edgeToTriangleList = nativeParallelHashMap,
								newTriangles = nativeList,
								useQuadSet = nativeParallelHashSet,
								removeTriangleSet = nativeParallelHashSet2
							}.Run<VirtualMesh.Optimize_EdgeToTrianlgeJob>();
							ExSimpleNativeArray<int3> exSimpleNativeArray = this.triangles;
							if (exSimpleNativeArray != null)
							{
								exSimpleNativeArray.Dispose();
							}
							this.triangles = new ExSimpleNativeArray<int3>();
							if (nativeList.Length > 0)
							{
								this.triangles.AddRange(nativeList);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x000327B4 File Offset: 0x000309B4
		private bool CheckTwoTriangleOpen(in int3 tri1, in int3 tri2, in int2 edge, in float3 tri1n)
		{
			int num = DataUtility.RemainingData(in tri2, in edge);
			float3 @float = math.normalize(this.localPositions[num] - this.localPositions[edge.x]);
			return math.dot(tri1n, @float) <= 0f;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00032808 File Offset: 0x00030A08
		private float CalcTwoTriangleAngle(in int3 tri1, in int3 tri2, in int2 edge)
		{
			int num = DataUtility.RemainingData(in tri1, in edge);
			int num2 = DataUtility.RemainingData(in tri2, in edge);
			float3 @float = this.localPositions[edge.y] - this.localPositions[edge.x];
			float3 float2 = this.localPositions[num] - this.localPositions[edge.x];
			float3 float3 = this.localPositions[num2] - this.localPositions[edge.x];
			float3 float4 = math.cross(@float, float2);
			float3 float5 = math.cross(float3, @float);
			return math.degrees(MathUtility.Angle(in float4, in float5));
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x000328B4 File Offset: 0x00030AB4
		public void ConvertProxyMesh(ClothSerializeData sdata, TransformRecord clothTransformRecord, List<TransformRecord> customSkinningBoneRecords, TransformRecord normalAdjustmentTransformRecord)
		{
			try
			{
				if (this.IsError)
				{
					throw new MagicaClothProcessingException();
				}
				if (sdata.customSkinningSetting.enable && !sdata.IsBoneSpring())
				{
					this.SetCustomSkinningBones(clothTransformRecord, customSkinningBoneRecords);
				}
				this.vertexToTriangles = new NativeArray<FixedList32Bytes<uint>>(this.VertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.vertexBindPosePositions = new NativeArray<float3>(this.VertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.vertexBindPoseRotations = new NativeArray<quaternion>(this.VertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.vertexToTransformRotations = new NativeArray<quaternion>(this.VertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				JobUtility.FillRun(this.vertexToTransformRotations, this.VertexCount, quaternion.identity);
				using (MultiDataBuilder<ushort> multiDataBuilder = new MultiDataBuilder<ushort>(this.VertexCount, this.VertexCount * 4))
				{
					using (NativeParallelHashSet<int2> nativeParallelHashSet = new NativeParallelHashSet<int2>(this.VertexCount * 2, Allocator.Persistent))
					{
						if (this.TriangleCount > 0)
						{
							new VirtualMesh.Proxy_CalcVertexToVertexFromTriangleJob
							{
								triangleCount = this.TriangleCount,
								triangles = this.triangles.GetNativeArray(),
								vertexToVertexMap = multiDataBuilder.Map,
								edgeSet = nativeParallelHashSet
							}.Run<VirtualMesh.Proxy_CalcVertexToVertexFromTriangleJob>();
						}
						if (this.LineCount > 0)
						{
							new VirtualMesh.Proxy_CalcVertexToVertexFromLineJob
							{
								lineCount = this.LineCount,
								lines = this.lines.GetNativeArray(),
								vertexToVertexMap = multiDataBuilder.Map,
								edgeSet = nativeParallelHashSet
							}.Run<VirtualMesh.Proxy_CalcVertexToVertexFromLineJob>();
						}
						this.edges = nativeParallelHashSet.ToNativeArray(Allocator.Persistent);
						multiDataBuilder.ToNativeArray(out this.vertexToVertexIndexArray, out this.vertexToVertexDataArray);
						if (this.TriangleCount > 0)
						{
							this.edgeToTriangles = new NativeParallelMultiHashMap<int2, ushort>(this.TriangleCount * 2, Allocator.Persistent);
							new VirtualMesh.Proxy_CalcEdgeToTriangleJob
							{
								tcnt = this.TriangleCount,
								triangles = this.triangles.GetNativeArray(),
								edgeToTriangles = this.edgeToTriangles
							}.Run<VirtualMesh.Proxy_CalcEdgeToTriangleJob>();
							using (NativeArray<float3> nativeArray = new NativeArray<float3>(this.TriangleCount, Allocator.Persistent, NativeArrayOptions.ClearMemory))
							{
								new VirtualMesh.Proxy_CalcTriangleNormalJob
								{
									triangles = this.triangles.GetNativeArray(),
									localPositins = this.localPositions.GetNativeArray(),
									triangleNormals = nativeArray
								}.Run(this.TriangleCount);
								this.OptimizeTriangleDirection(nativeArray, 80f);
								using (NativeArray<float3> nativeArray2 = new NativeArray<float3>(this.TriangleCount, Allocator.Persistent, NativeArrayOptions.ClearMemory))
								{
									new VirtualMesh.Proxy_CalcTriangleTangentJob
									{
										triangles = this.triangles.GetNativeArray(),
										localPositins = this.localPositions.GetNativeArray(),
										uv = this.uv.GetNativeArray(),
										triangleTangents = nativeArray2
									}.Run(this.TriangleCount);
									new VirtualMesh.Proxy_CreateVertexToTrianglesJob
									{
										triangles = this.triangles.GetNativeArray(),
										vertexToTriangles = this.vertexToTriangles
									}.Run<VirtualMesh.Proxy_CreateVertexToTrianglesJob>();
									new VirtualMesh.Proxy_OrganizeVertexToTrianglsJob
									{
										vertexToTriangles = this.vertexToTriangles,
										triangleNormals = nativeArray,
										triangleTangents = nativeArray2,
										attributes = this.attributes.GetNativeArray()
									}.Run(this.VertexCount);
									new VirtualMesh.Proxy_CalcVertexNormalTangentFromTriangleJob
									{
										triangleNormals = nativeArray,
										triangleTangents = nativeArray2,
										vertexToTriangles = this.vertexToTriangles,
										localNormals = this.localNormals.GetNativeArray(),
										localTangents = this.localTangents.GetNativeArray()
									}.Run(this.VertexCount);
								}
								goto IL_038F;
							}
						}
						this.edgeToTriangles = new NativeParallelMultiHashMap<int2, ushort>(1, Allocator.Persistent);
						IL_038F:
						this.ProxyCreateFixedListAndAABB();
						if (this.isBoneCloth)
						{
							this.CreateTransformBaseLine();
						}
						else
						{
							this.CreateMeshBaseLine();
						}
						this.ProxyNormalAdjustment(sdata, normalAdjustmentTransformRecord);
						if (this.isBoneCloth)
						{
							new VirtualMesh.Proxy_CalcVertexToTransformJob
							{
								invRot = this.initInverseRotation,
								localNormals = this.localNormals.GetNativeArray(),
								localTangents = this.localTangents.GetNativeArray(),
								vertexToTransformRotations = this.vertexToTransformRotations,
								transformRotations = this.transformData.rotationArray.GetNativeArray()
							}.Run(this.VertexCount);
						}
						new VirtualMesh.Proxy_CalcVertexBindPoseJob2
						{
							localPositions = this.localPositions.GetNativeArray(),
							localNormals = this.localNormals.GetNativeArray(),
							localTangents = this.localTangents.GetNativeArray(),
							vertexBindPosePositions = this.vertexBindPosePositions,
							vertexBindPoseRotations = this.vertexBindPoseRotations
						}.Run(this.VertexCount);
						this.edgeFlags = new NativeArray<ExBitFlag8>(this.EdgeCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
						if (this.EdgeCount > 0)
						{
							new VirtualMesh.Proxy_CreateEdgeFlagJob
							{
								edges = this.edges,
								edgeToTriangles = this.edgeToTriangles,
								edgeFlags = this.edgeFlags
							}.Run(this.EdgeCount);
						}
						this.CreateBaseLinePose();
						this.CreateVertexRootAndDepth();
						if (sdata.customSkinningSetting.enable && !sdata.IsBoneSpring())
						{
							this.CreateCustomSkinning(sdata.customSkinningSetting, customSkinningBoneRecords);
						}
						if (this.isBoneCloth)
						{
							this.meshType = VirtualMesh.MeshType.ProxyBoneMesh;
						}
						else
						{
							this.meshType = VirtualMesh.MeshType.ProxyMesh;
						}
					}
				}
			}
			catch (MagicaClothProcessingException)
			{
				if (!this.result.IsError())
				{
					this.result.SetError(Define.Result.ProxyMesh_UnknownError);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				this.result.SetError(Define.Result.ProxyMesh_Exception);
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00032ED4 File Offset: 0x000310D4
		private void ProxyNormalAdjustment(ClothSerializeData sdata, TransformRecord normalAdjustmentTransformRecord)
		{
			int vertexCount = this.VertexCount;
			if (vertexCount == 0)
			{
				return;
			}
			this.normalAdjustmentRotations = new NativeArray<quaternion>(vertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			JobUtility.FillRun(this.normalAdjustmentRotations, vertexCount, quaternion.identity);
			NormalAlignmentSettings.AlignmentMode alignmentMode = sdata.normalAlignmentSetting.alignmentMode;
			if (alignmentMode == NormalAlignmentSettings.AlignmentMode.None)
			{
				return;
			}
			if (alignmentMode == NormalAlignmentSettings.AlignmentMode.BoundingBoxCenter || alignmentMode == NormalAlignmentSettings.AlignmentMode.Transform)
			{
				float3 @float;
				if (alignmentMode == NormalAlignmentSettings.AlignmentMode.BoundingBoxCenter)
				{
					@float = this.boundingBox.Value.Center;
				}
				else
				{
					@float = math.transform(this.initWorldToLocal, normalAdjustmentTransformRecord.position);
				}
				new VirtualMesh.ProxyNormalRadiationAdjustmentJob
				{
					center = @float,
					localPositions = this.localPositions.GetNativeArray(),
					localNormals = this.localNormals.GetNativeArray(),
					localTangents = this.localTangents.GetNativeArray(),
					normalAdjustmentRotations = this.normalAdjustmentRotations
				}.Run(vertexCount);
			}
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00032FB4 File Offset: 0x000311B4
		private void ProxyCreateFixedListAndAABB()
		{
			this.localCenterPosition = new NativeReference<float3>(0, Allocator.Persistent);
			using (NativeList<ushort> nativeList = new NativeList<ushort>(this.VertexCount / 20 + 1, Allocator.TempJob))
			{
				new VirtualMesh.ProxyCreateFixedListAndAABBJob
				{
					vcnt = this.VertexCount,
					attributes = this.attributes.GetNativeArray(),
					localPositions = this.localPositions.GetNativeArray(),
					vertexToVertexIndexArray = this.vertexToVertexIndexArray,
					vertexToVertexDataArray = this.vertexToVertexDataArray,
					outAABB = this.boundingBox,
					fixedList = nativeList,
					localCenterPosition = this.localCenterPosition
				}.Run<VirtualMesh.ProxyCreateFixedListAndAABBJob>();
				if (nativeList.Length > 0)
				{
					this.centerFixedList = nativeList.AsArray().ToArray();
				}
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000330AC File Offset: 0x000312AC
		private unsafe void OptimizeTriangleDirection(NativeArray<float3> triangleNormals, float sameSurfaceAngle)
		{
			if (this.TriangleCount == 0)
			{
				return;
			}
			int i = 0;
			HashSet<int> hashSet = new HashSet<int>();
			Queue<int> queue = new Queue<int>(this.TriangleCount / 2);
			List<int> list = new List<int>(this.TriangleCount);
			while (i < this.TriangleCount)
			{
				if (hashSet.Contains(i))
				{
					i++;
				}
				else
				{
					hashSet.Add(i);
					queue.Clear();
					queue.Enqueue(i);
					list.Clear();
					int num = 0;
					int num2 = 0;
					while (queue.Count > 0)
					{
						int num3 = queue.Dequeue();
						float3 @float = triangleNormals[num3];
						int3 @int = this.triangles[num3];
						list.Add(num3);
						int2 xy = @int.xy;
						int2 int2 = DataUtility.PackInt2(in xy);
						int2 yz = @int.yz;
						int2 int3 = DataUtility.PackInt2(in yz);
						int2 zx = @int.zx;
						int2x3 int2x = new int2x3(int2, int3, DataUtility.PackInt2(in zx));
						for (int j = 0; j < 3; j++)
						{
							int2 int4 = *int2x[j];
							if (this.edgeToTriangles.ContainsKey(int4))
							{
								using (NativeParallelMultiHashMap<int2, ushort>.Enumerator enumerator = this.edgeToTriangles.GetValuesForKey(int4).GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										int num4 = (int)enumerator.Current;
										if (!hashSet.Contains(num4))
										{
											int3 int5 = this.triangles[num4];
											float3 float2 = triangleNormals[num4];
											if (this.CalcTwoTriangleAngle(in @int, in int5, in int4) <= sameSurfaceAngle)
											{
												if (math.dot(@float, float2) < 0f)
												{
													int5 = MathUtility.FlipTriangle(in int5);
													this.triangles[num4] = int5;
													triangleNormals[num4] = -float2;
												}
												if (this.CheckTwoTriangleOpen(in @int, in int5, in int4, in @float))
												{
													num++;
												}
												else
												{
													num2++;
												}
												hashSet.Add(num4);
												queue.Enqueue(num4);
											}
										}
									}
								}
							}
						}
					}
					if (num2 > num)
					{
						foreach (int num5 in list)
						{
							ExSimpleNativeArray<int3> exSimpleNativeArray = this.triangles;
							int num6 = num5;
							int3 int6 = this.triangles[num5];
							exSimpleNativeArray[num6] = MathUtility.FlipTriangle(in int6);
							triangleNormals[num5] = -triangleNormals[num5];
						}
					}
				}
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0003333C File Offset: 0x0003153C
		private void CreateCustomSkinning(CustomSkinningSettings setting, List<TransformRecord> bones)
		{
			if (this.CustomSkinningBoneCount == 0)
			{
				return;
			}
			using (NativeList<VirtualMesh.SkinningBoneInfo> nativeList = new NativeList<VirtualMesh.SkinningBoneInfo>(this.CustomSkinningBoneCount * 2, Allocator.Persistent))
			{
				for (int i = 0; i < this.CustomSkinningBoneCount; i++)
				{
					int num = this.customSkinningBoneIndices[i];
					if (num != -1)
					{
						VirtualMesh.SkinningBoneInfo skinningBoneInfo = default(VirtualMesh.SkinningBoneInfo);
						skinningBoneInfo.childTransformIndex = num;
						skinningBoneInfo.childPos = bones[i].localPosition;
						nativeList.Add(in skinningBoneInfo);
					}
				}
				if (nativeList.Length != 0)
				{
					new VirtualMesh.Proxy_CalcCustomSkinningWeightsJobV2
					{
						isBoneCloth = this.isBoneCloth,
						angularAttenuation = 1f,
						distanceReduction = 0.6f,
						distancePow = 2f,
						attributes = this.attributes.GetNativeArray(),
						localPositions = this.localPositions.GetNativeArray(),
						boneInfoList = nativeList,
						boneWeights = this.boneWeights.GetNativeArray()
					}.Run(this.VertexCount);
				}
			}
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00033468 File Offset: 0x00031668
		public void ApplySelectionAttribute(SelectionData selectionData)
		{
			try
			{
				NativeArray<float3> positionNativeArray = selectionData.GetPositionNativeArray();
				try
				{
					NativeArray<VertexAttribute> attributeNativeArray = selectionData.GetAttributeNativeArray();
					try
					{
						float num = math.max(this.averageVertexDistance.Value, selectionData.maxConnectionDistance);
						num = math.max(num, 1E-05f);
						float num2 = num * 1.5f;
						using (GridMap<int> gridMap = SelectionData.CreateGridMapRun(num2, in positionNativeArray, in attributeNativeArray, true, true, true, true))
						{
							new VirtualMesh.Proxy_ApplySelectionJob
							{
								gridSize = num2,
								radius = num,
								localPositions = this.localPositions.GetNativeArray(),
								attributes = this.attributes.GetNativeArray(),
								gridMap = gridMap.GetMultiHashMap(),
								selectionPositions = positionNativeArray,
								selectionAttributes = attributeNativeArray
							}.Run(this.VertexCount);
							if (this.isBoneCloth)
							{
								new VirtualMesh.Proxy_BoneClothApplayTransformFlagJob
								{
									attributes = this.attributes.GetNativeArray(),
									transformFlags = this.transformData.flagArray.GetNativeArray()
								}.Run(this.VertexCount);
							}
						}
					}
					finally
					{
						((IDisposable)attributeNativeArray).Dispose();
					}
				}
				finally
				{
					((IDisposable)positionNativeArray).Dispose();
				}
			}
			catch (Exception)
			{
				this.result.SetError(Define.Result.ProxyMesh_ApplySelectionError);
			}
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00033614 File Offset: 0x00031814
		private void CreateMeshBaseLine()
		{
			int vertexCount = this.VertexCount;
			this.vertexParentIndices = new NativeArray<int>(vertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			using (MultiDataBuilder<ushort> multiDataBuilder = new MultiDataBuilder<ushort>(vertexCount, vertexCount))
			{
				JobUtility.FillRun(this.vertexParentIndices, vertexCount, -1);
				using (NativeList<int> nativeList = new NativeList<int>(vertexCount, Allocator.Persistent))
				{
					new VirtualMesh.BaseLine_Mesh_CareteFixedListJob
					{
						vcnt = vertexCount,
						attribues = this.attributes.GetNativeArray(),
						fixedList = nativeList
					}.Run<VirtualMesh.BaseLine_Mesh_CareteFixedListJob>();
					if (nativeList.Length == 0)
					{
						this.vertexChildIndexArray = new NativeArray<uint>(vertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
						this.vertexChildDataArray = new NativeArray<ushort>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
					}
					else
					{
						using (NativeList<VirtualMesh.BaseLineWork> nativeList2 = new NativeList<VirtualMesh.BaseLineWork>(vertexCount, Allocator.Persistent))
						{
							using (NativeArray<byte> nativeArray = new NativeArray<byte>(vertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory))
							{
								using (NativeParallelHashMap<int, VirtualMesh.BaseLineWork> nativeParallelHashMap = new NativeParallelHashMap<int, VirtualMesh.BaseLineWork>(vertexCount, Allocator.Persistent))
								{
									new VirtualMesh.BaseLine_Mesh_CreateParentJob2
									{
										vcnt = vertexCount,
										avgDist = this.averageVertexDistance.Value,
										attribues = this.attributes.GetNativeArray(),
										localPositions = this.localPositions.GetNativeArray(),
										vertexToVertexIndexArray = this.vertexToVertexIndexArray,
										vertexToVertexDataArray = this.vertexToVertexDataArray,
										vertexParentIndices = this.vertexParentIndices,
										vertexChildMap = multiDataBuilder.Map,
										fixedList = nativeList,
										nextList = nativeList2,
										markBuff = nativeArray,
										vertexMap = nativeParallelHashMap
									}.Run<VirtualMesh.BaseLine_Mesh_CreateParentJob2>();
									Stack<int> stack = new Stack<int>(vertexCount);
									List<ExBitFlag8> list = new List<ExBitFlag8>(nativeList.Length);
									List<ushort> list2 = new List<ushort>(nativeList.Length);
									List<ushort> list3 = new List<ushort>(nativeList.Length);
									List<ushort> list4 = new List<ushort>(vertexCount);
									for (int i = 0; i < nativeList.Length; i++)
									{
										int num = nativeList[i];
										if (multiDataBuilder.GetDataCount(num) != 0)
										{
											stack.Clear();
											stack.Push(num);
											ushort num2 = (ushort)list4.Count;
											ushort num3 = 0;
											ExBitFlag8 exBitFlag = default(ExBitFlag8);
											while (stack.Count > 0)
											{
												int num4 = stack.Pop();
												list4.Add((ushort)num4);
												num3 += 1;
												if (!this.attributes[num4].IsSet(128))
												{
													exBitFlag.SetFlag(1, true);
												}
												if (multiDataBuilder.Map.ContainsKey(num4))
												{
													foreach (ushort num5 in multiDataBuilder.Map.GetValuesForKey(num4))
													{
														stack.Push((int)num5);
													}
												}
											}
											list.Add(exBitFlag);
											list2.Add(num2);
											list3.Add(num3);
										}
									}
									this.baseLineFlags = new NativeArray<ExBitFlag8>(list.ToArray(), Allocator.Persistent);
									this.baseLineStartDataIndices = new NativeArray<ushort>(list2.ToArray(), Allocator.Persistent);
									this.baseLineDataCounts = new NativeArray<ushort>(list3.ToArray(), Allocator.Persistent);
									this.baseLineData = new NativeArray<ushort>(list4.ToArray(), Allocator.Persistent);
									ValueTuple<ushort[], uint[]> valueTuple = multiDataBuilder.ToArray();
									ushort[] item = valueTuple.Item1;
									uint[] item2 = valueTuple.Item2;
									this.vertexChildIndexArray = new NativeArray<uint>(item2, Allocator.Persistent);
									this.vertexChildDataArray = new NativeArray<ushort>(item, Allocator.Persistent);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00033A24 File Offset: 0x00031C24
		private void CreateTransformBaseLine()
		{
			int vertexCount = this.VertexCount;
			this.vertexParentIndices = new NativeArray<int>(vertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			using (MultiDataBuilder<ushort> multiDataBuilder = new MultiDataBuilder<ushort>(vertexCount, vertexCount * 2))
			{
				Dictionary<int, int> dictionary = new Dictionary<int, int>(vertexCount);
				NativeArray<int> nativeArray = this.transformData.idArray.GetNativeArray();
				NativeArray<int> nativeArray2 = this.transformData.parentIdArray.GetNativeArray();
				for (int i = 0; i < vertexCount; i++)
				{
					dictionary.Add(nativeArray[i], i);
				}
				for (int j = 0; j < vertexCount; j++)
				{
					int num = nativeArray2[j];
					if (dictionary.ContainsKey(num))
					{
						this.vertexParentIndices[j] = dictionary[num];
					}
					else
					{
						this.vertexParentIndices[j] = -1;
					}
				}
				new VirtualMesh.BaseLine_Bone_CreateBoneChildInfoJob
				{
					vcnt = vertexCount,
					parentIndices = this.vertexParentIndices,
					childMap = multiDataBuilder.Map
				}.Run<VirtualMesh.BaseLine_Bone_CreateBoneChildInfoJob>();
				int rootCount = this.transformData.RootCount;
				Stack<int> stack = new Stack<int>(vertexCount);
				Stack<int> stack2 = new Stack<int>(vertexCount);
				List<ExBitFlag8> list = new List<ExBitFlag8>(rootCount);
				List<ushort> list2 = new List<ushort>(rootCount);
				List<ushort> list3 = new List<ushort>(rootCount);
				List<ushort> list4 = new List<ushort>(vertexCount);
				foreach (int num2 in this.transformData.rootIdList)
				{
					stack.Clear();
					int num3 = dictionary[num2];
					stack.Push(num3);
					while (stack.Count > 0)
					{
						int num4 = stack.Pop();
						if (this.attributes[num4].IsDontMove())
						{
							bool flag = false;
							foreach (ushort num5 in multiDataBuilder.Map.GetValuesForKey(num4))
							{
								if (this.attributes[(int)num5].IsMove())
								{
									flag = true;
								}
							}
							if (!flag)
							{
								using (NativeParallelMultiHashMap<int, ushort>.Enumerator enumerator2 = multiDataBuilder.Map.GetValuesForKey(num4).GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										ushort num6 = enumerator2.Current;
										if (this.attributes[(int)num6].IsDontMove())
										{
											stack.Push((int)num6);
										}
									}
									continue;
								}
							}
							stack2.Clear();
							stack2.Push(num4);
							ushort num7 = (ushort)list4.Count;
							ushort num8 = 0;
							ExBitFlag8 exBitFlag = default(ExBitFlag8);
							while (stack2.Count > 0)
							{
								int num9 = stack2.Pop();
								list4.Add((ushort)num9);
								num8 += 1;
								if (!this.attributes[num9].IsSet(128))
								{
									exBitFlag.SetFlag(1, true);
								}
								if (multiDataBuilder.Map.ContainsKey(num9))
								{
									foreach (ushort num10 in multiDataBuilder.Map.GetValuesForKey(num9))
									{
										if (!this.attributes[(int)num10].IsDontMove())
										{
											stack2.Push((int)num10);
										}
									}
								}
							}
							list.Add(exBitFlag);
							list2.Add(num7);
							list3.Add(num8);
						}
					}
				}
				this.baseLineFlags = new NativeArray<ExBitFlag8>(list.ToArray(), Allocator.Persistent);
				this.baseLineStartDataIndices = new NativeArray<ushort>(list2.ToArray(), Allocator.Persistent);
				this.baseLineDataCounts = new NativeArray<ushort>(list3.ToArray(), Allocator.Persistent);
				this.baseLineData = new NativeArray<ushort>(list4.ToArray(), Allocator.Persistent);
				multiDataBuilder.ToNativeArray(out this.vertexChildIndexArray, out this.vertexChildDataArray);
			}
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00033E80 File Offset: 0x00032080
		private void CreateBaseLinePose()
		{
			int length = this.baseLineData.Length;
			this.vertexLocalPositions = new NativeArray<float3>(this.VertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.vertexLocalRotations = new NativeArray<quaternion>(this.VertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			new VirtualMesh.BaseLine_CalcLocalPositionRotationJob
			{
				parentIndices = this.vertexParentIndices,
				localPositions = this.localPositions.GetNativeArray(),
				localNormals = this.localNormals.GetNativeArray(),
				localTangents = this.localTangents.GetNativeArray(),
				baseLineIndices = this.baseLineData,
				vertexLocalPositions = this.vertexLocalPositions,
				vertexLocalRotations = this.vertexLocalRotations
			}.Run(length);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00033F38 File Offset: 0x00032138
		private void CreateVertexRootAndDepth()
		{
			int vertexCount = this.VertexCount;
			this.vertexDepths = new NativeArray<float>(vertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.vertexRootIndices = new NativeArray<int>(vertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			using (NativeArray<float> nativeArray = new NativeArray<float>(vertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory))
			{
				new VirtualMesh.BaseLine_CalcMaxBaseLineLengthJob
				{
					vcnt = vertexCount,
					attribues = this.attributes.GetNativeArray(),
					localPositions = this.localPositions.GetNativeArray(),
					vertexParentIndices = this.vertexParentIndices,
					vertexDepths = this.vertexDepths,
					vertexRootIndices = this.vertexRootIndices,
					rootLengthArray = nativeArray
				}.Run<VirtualMesh.BaseLine_CalcMaxBaseLineLengthJob>();
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00033FFC File Offset: 0x000321FC
		public void Reduction(ReductionSettings settings, CancellationToken ct)
		{
			try
			{
				using (ReductionWorkData reductionWorkData = new ReductionWorkData(this))
				{
					this.InitReductionWorkData(reductionWorkData);
					if (this.result.IsError())
					{
						throw new MagicaClothProcessingException();
					}
					float maxSideLength = this.boundingBox.Value.MaxSideLength;
					if (maxSideLength < 1E-08f)
					{
						this.result.SetError(Define.Result.Reduction_MaxSideLengthZero);
						throw new MagicaClothProcessingException();
					}
					float num = maxSideLength * math.saturate(0.001f);
					float num2 = maxSideLength * math.saturate(settings.simpleDistance);
					float num3 = maxSideLength * math.saturate(settings.shapeDistance);
					ct.ThrowIfCancellationRequested();
					using (SameDistanceReduction sameDistanceReduction = new SameDistanceReduction(this.name, this, reductionWorkData, num))
					{
						sameDistanceReduction.Reduction();
						if (sameDistanceReduction.Result.IsError())
						{
							this.result = sameDistanceReduction.Result;
							throw new MagicaClothProcessingException();
						}
					}
					ct.ThrowIfCancellationRequested();
					if (num2 > num)
					{
						float num4 = math.min(num * 2f, num2);
						using (SimpleDistanceReduction simpleDistanceReduction = new SimpleDistanceReduction(this.name, this, reductionWorkData, num4, num2, 100, true, 1f))
						{
							simpleDistanceReduction.Reduction();
							if (simpleDistanceReduction.Result.IsError())
							{
								this.result = simpleDistanceReduction.Result;
								throw new MagicaClothProcessingException();
							}
						}
					}
					ct.ThrowIfCancellationRequested();
					if (num3 > 0f && num3 > num2)
					{
						float num5 = math.min(math.max(num * 2f, num2), num3);
						using (ShapeDistanceReduction shapeDistanceReduction = new ShapeDistanceReduction(this.name, this, reductionWorkData, num5, num3, 100, true, 1f))
						{
							shapeDistanceReduction.Reduction();
							if (shapeDistanceReduction.Result.IsError())
							{
								this.result = shapeDistanceReduction.Result;
								throw new MagicaClothProcessingException();
							}
						}
					}
					ct.ThrowIfCancellationRequested();
					this.Organization(settings, reductionWorkData);
					if (this.result.IsError())
					{
						throw new MagicaClothProcessingException();
					}
					ct.ThrowIfCancellationRequested();
					this.OrganizeStoreVirtualMesh(reductionWorkData);
					if (this.result.IsError())
					{
						throw new MagicaClothProcessingException();
					}
					this.CalcAverageAndMaxVertexDistanceRun();
					ct.ThrowIfCancellationRequested();
				}
			}
			catch (MagicaClothProcessingException)
			{
				if (!this.result.IsError())
				{
					this.result.SetError(Define.Result.Reduction_UnknownError);
				}
			}
			catch (OperationCanceledException)
			{
				this.result.SetCancel();
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				this.result.SetError(Define.Result.Reduction_Exception);
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00034314 File Offset: 0x00032514
		private void InitReductionWorkData(ReductionWorkData workData)
		{
			try
			{
				int vertexCount = this.VertexCount;
				int triangleCount = this.TriangleCount;
				workData.vertexJoinIndices = new NativeArray<int>(vertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				JobUtility.FillRun(workData.vertexJoinIndices, vertexCount, -1);
				workData.vertexToVertexMap = new NativeParallelMultiHashMap<ushort, ushort>(vertexCount, Allocator.Persistent);
				new VirtualMesh.Reduction_InitVertexToVertexJob2
				{
					triangleCount = triangleCount,
					triangles = this.triangles.GetNativeArray(),
					vertexToVertexMap = workData.vertexToVertexMap
				}.Run<VirtualMesh.Reduction_InitVertexToVertexJob2>();
			}
			catch (Exception)
			{
				this.result.SetError(Define.Result.Reduction_InitError);
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x000343B8 File Offset: 0x000325B8
		private void Organization(ReductionSettings setting, ReductionWorkData workData)
		{
			try
			{
				this.OrganizationInit(setting, workData);
				this.OrganizationCreateRemapData(workData);
				this.OrganizationCreateBasicData(workData);
				this.OrganizationCreateLineTriangle(workData);
			}
			catch (Exception)
			{
				this.result.SetError(Define.Result.Reduction_OrganizationError);
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00034408 File Offset: 0x00032608
		private void OrganizationInit(ReductionSettings setting, ReductionWorkData workData)
		{
			workData.oldVertexCount = this.VertexCount;
			workData.newVertexCount = workData.oldVertexCount - workData.removeVertexCount;
			int newVertexCount = workData.newVertexCount;
			workData.vertexRemapIndices = new NativeArray<int>(workData.oldVertexCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			workData.useSkinBoneMap = new NativeParallelHashMap<int, int>(this.SkinBoneCount, Allocator.Persistent);
			workData.newSkinBoneCount = new NativeReference<int>(Allocator.Persistent, NativeArrayOptions.ClearMemory);
			workData.newSkinBoneTransformIndices = new NativeList<int>(this.SkinBoneCount, Allocator.Persistent);
			workData.newSkinBoneBindPoseList = new NativeList<float4x4>(this.SkinBoneCount, Allocator.Persistent);
			workData.newAttributes = new ExSimpleNativeArray<VertexAttribute>(newVertexCount, false);
			workData.newLocalPositions = new ExSimpleNativeArray<float3>(newVertexCount, false);
			workData.newLocalNormals = new ExSimpleNativeArray<float3>(newVertexCount, false);
			workData.newLocalTangents = new ExSimpleNativeArray<float3>(newVertexCount, false);
			workData.newUv = new ExSimpleNativeArray<float2>(newVertexCount, false);
			workData.newBoneWeights = new ExSimpleNativeArray<VirtualMeshBoneWeight>(newVertexCount, false);
			workData.newVertexToVertexMap = new NativeParallelMultiHashMap<ushort, ushort>(newVertexCount, Allocator.Persistent);
			workData.edgeSet = new NativeParallelHashSet<int2>(newVertexCount * 2, Allocator.Persistent);
			workData.triangleSet = new NativeParallelHashSet<int3>(newVertexCount, Allocator.Persistent);
			workData.newLineList = new NativeList<int2>(newVertexCount, Allocator.Persistent);
			workData.newTriangleList = new NativeList<int3>(newVertexCount, Allocator.Persistent);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00034550 File Offset: 0x00032750
		private void OrganizationCreateRemapData(ReductionWorkData workData)
		{
			new VirtualMesh.Organize_RemapVertexJob
			{
				oldVertexCount = workData.oldVertexCount,
				joinIndices = workData.vertexJoinIndices,
				vertexRemapIndices = workData.vertexRemapIndices
			}.Run<VirtualMesh.Organize_RemapVertexJob>();
			using (NativeList<int> nativeList = new NativeList<int>(Allocator.Persistent))
			{
				new VirtualMesh.Organize_CollectUseSkinBoneJob
				{
					oldVertexCount = workData.oldVertexCount,
					joinIndices = workData.vertexJoinIndices,
					oldBoneWeights = this.boneWeights.GetNativeArray(),
					oldBindPoses = this.skinBoneBindPoses.GetNativeArray(),
					useSkinBoneMap = workData.useSkinBoneMap,
					newSkinBoneTransformIndices = workData.newSkinBoneTransformIndices,
					newSkinBoneBindPoses = workData.newSkinBoneBindPoseList,
					newSkinBoneCount = workData.newSkinBoneCount,
					useSkinBoneMapKeyList = nativeList
				}.Run<VirtualMesh.Organize_CollectUseSkinBoneJob>();
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00034648 File Offset: 0x00032848
		private void OrganizationCreateBasicData(ReductionWorkData workData)
		{
			int newVertexCount = workData.newVertexCount;
			int oldVertexCount = workData.oldVertexCount;
			new VirtualMesh.Organize_CopyVertexJob
			{
				joinIndices = workData.vertexJoinIndices,
				vertexRemapIndices = workData.vertexRemapIndices,
				oldAttributes = this.attributes.GetNativeArray(),
				oldLocalPositions = this.localPositions.GetNativeArray(),
				oldLocalNormals = this.localNormals.GetNativeArray(),
				oldLocalTangents = this.localTangents.GetNativeArray(),
				newAttributes = workData.newAttributes.GetNativeArray(),
				newLocalPositions = workData.newLocalPositions.GetNativeArray(),
				newLocalNormals = workData.newLocalNormals.GetNativeArray(),
				newLocalTangents = workData.newLocalTangents.GetNativeArray()
			}.Run(oldVertexCount);
			JobUtility.CalcUVWithSphereMappingRun(workData.newLocalPositions.GetNativeArray(), newVertexCount, workData.vmesh.boundingBox, workData.newUv.GetNativeArray());
			new VirtualMesh.Organize_RemapBoneWeightJob
			{
				joinIndices = workData.vertexJoinIndices,
				vertexRemapIndices = workData.vertexRemapIndices,
				useSkinBoneMap = workData.useSkinBoneMap,
				oldSkinBoneIndices = this.skinBoneTransformIndices.GetNativeArray(),
				oldBoneWeights = this.boneWeights.GetNativeArray(),
				newBoneWeights = workData.newBoneWeights.GetNativeArray()
			}.Run(oldVertexCount);
			new VirtualMesh.Organize_RemapLinkPointArrayJob
			{
				joinIndices = workData.vertexJoinIndices,
				vertexRemapIndices = workData.vertexRemapIndices,
				oldVertexToVertexMap = workData.vertexToVertexMap,
				newVertexToVertexMap = workData.newVertexToVertexMap
			}.Run(this.VertexCount);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000347F8 File Offset: 0x000329F8
		private void OrganizationCreateLineTriangle(ReductionWorkData workData)
		{
			new VirtualMesh.Organize_CreateLineTriangleJob
			{
				newVertexCount = workData.newVertexCount,
				newVertexToVertexMap = workData.newVertexToVertexMap,
				edgeSet = workData.edgeSet
			}.Run<VirtualMesh.Organize_CreateLineTriangleJob>();
			new VirtualMesh.Organize_CreateLineTriangleJob2
			{
				newVertexToVertexMap = workData.newVertexToVertexMap,
				newLineList = workData.newLineList,
				edgeSet = workData.edgeSet,
				triangleSet = workData.triangleSet
			}.Run<VirtualMesh.Organize_CreateLineTriangleJob2>();
			new VirtualMesh.Organize_CreateNewTriangleJob3
			{
				newTriangleList = workData.newTriangleList,
				triangleSet = workData.triangleSet
			}.Run<VirtualMesh.Organize_CreateNewTriangleJob3>();
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000348A4 File Offset: 0x00032AA4
		private void OrganizeStoreVirtualMesh(ReductionWorkData workData)
		{
			try
			{
				int newVertexCount = workData.newVertexCount;
				this.referenceIndices.Dispose();
				this.referenceIndices = new ExSimpleNativeArray<int>(newVertexCount, false);
				JobUtility.SerialNumberRun(this.referenceIndices.GetNativeArray(), newVertexCount);
				this.attributes.Dispose();
				this.attributes = workData.newAttributes;
				workData.newAttributes = null;
				this.localPositions.Dispose();
				this.localPositions = workData.newLocalPositions;
				workData.newLocalPositions = null;
				this.localNormals.Dispose();
				this.localNormals = workData.newLocalNormals;
				workData.newLocalNormals = null;
				this.localTangents.Dispose();
				this.localTangents = workData.newLocalTangents;
				workData.newLocalTangents = null;
				this.uv.Dispose();
				this.uv = workData.newUv;
				workData.newUv = null;
				this.boneWeights.Dispose();
				this.boneWeights = workData.newBoneWeights;
				workData.newBoneWeights = null;
				this.lines.Dispose();
				this.lines = new ExSimpleNativeArray<int2>(workData.newLineList);
				this.triangles.Dispose();
				this.triangles = new ExSimpleNativeArray<int3>(workData.newTriangleList);
				this.transformData.OrganizeReductionTransform(this, workData);
				this.skinBoneTransformIndices.Dispose();
				this.skinBoneTransformIndices = new ExSimpleNativeArray<int>(workData.newSkinBoneTransformIndices);
				this.skinBoneBindPoses.Dispose();
				this.skinBoneBindPoses = new ExSimpleNativeArray<float4x4>(workData.newSkinBoneBindPoseList);
				this.joinIndices = new NativeArray<int>(workData.vertexRemapIndices, Allocator.Persistent);
			}
			catch (Exception)
			{
				this.result.SetError(Define.Result.Reduction_StoreVirtualMeshError);
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00034A58 File Offset: 0x00032C58
		public VirtualMesh.ShareSerializationData ShareSerialize()
		{
			VirtualMesh.ShareSerializationData shareSerializationData = new VirtualMesh.ShareSerializationData();
			try
			{
				shareSerializationData.name = this.name;
				shareSerializationData.meshType = this.meshType;
				shareSerializationData.isBoneCloth = this.isBoneCloth;
				shareSerializationData.referenceIndices = this.referenceIndices.Serialize();
				shareSerializationData.attributes = this.attributes.Serialize();
				shareSerializationData.localPositions = this.localPositions.Serialize();
				shareSerializationData.localNormals = this.localNormals.Serialize();
				shareSerializationData.localTangents = this.localTangents.Serialize();
				shareSerializationData.uv = this.uv.Serialize();
				shareSerializationData.boneWeights = this.boneWeights.Serialize();
				shareSerializationData.triangles = this.triangles.Serialize();
				shareSerializationData.lines = this.lines.Serialize();
				shareSerializationData.centerTransformIndex = this.centerTransformIndex;
				shareSerializationData.initLocalToWorld = this.initLocalToWorld;
				shareSerializationData.initWorldToLocal = this.initWorldToLocal;
				shareSerializationData.initRotation = this.initRotation;
				shareSerializationData.initInverseRotation = this.initInverseRotation;
				shareSerializationData.initScale = this.initScale;
				shareSerializationData.skinRootIndex = this.skinRootIndex;
				shareSerializationData.skinBoneTransformIndices = this.skinBoneTransformIndices.Serialize();
				shareSerializationData.skinBoneBindPoses = this.skinBoneBindPoses.Serialize();
				VirtualMesh.ShareSerializationData shareSerializationData2 = shareSerializationData;
				TransformData transformData = this.transformData;
				shareSerializationData2.transformData = ((transformData != null) ? transformData.ShareSerialize() : null);
				if (this.boundingBox.IsCreated)
				{
					shareSerializationData.boundingBox = this.boundingBox.Value;
				}
				if (this.averageVertexDistance.IsCreated)
				{
					shareSerializationData.averageVertexDistance = this.averageVertexDistance.Value;
				}
				if (this.maxVertexDistance.IsCreated)
				{
					shareSerializationData.maxVertexDistance = this.maxVertexDistance.Value;
				}
				shareSerializationData.vertexToTriangles = (ref this.vertexToTriangles).MC2ToRawBytes<FixedList32Bytes<uint>>();
				shareSerializationData.vertexToVertexIndexArray = (ref this.vertexToVertexIndexArray).MC2ToRawBytes<uint>();
				shareSerializationData.vertexToVertexDataArray = (ref this.vertexToVertexDataArray).MC2ToRawBytes<ushort>();
				shareSerializationData.edges = (ref this.edges).MC2ToRawBytes<int2>();
				shareSerializationData.edgeFlags = (ref this.edgeFlags).MC2ToRawBytes<ExBitFlag8>();
				VirtualMesh.ShareSerializationData shareSerializationData3 = shareSerializationData;
				VirtualMesh.ShareSerializationData shareSerializationData4 = shareSerializationData;
				ValueTuple<int2[], ushort[]> valueTuple = (ref this.edgeToTriangles).MC2Serialize<int2, ushort>();
				shareSerializationData3.edgeToTrianglesKeys = valueTuple.Item1;
				shareSerializationData4.edgeToTrianglesValues = valueTuple.Item2;
				shareSerializationData.vertexBindPosePositions = (ref this.vertexBindPosePositions).MC2ToRawBytes<float3>();
				shareSerializationData.vertexBindPoseRotations = (ref this.vertexBindPoseRotations).MC2ToRawBytes<quaternion>();
				shareSerializationData.vertexToTransformRotations = (ref this.vertexToTransformRotations).MC2ToRawBytes<quaternion>();
				shareSerializationData.vertexDepths = (ref this.vertexDepths).MC2ToRawBytes<float>();
				shareSerializationData.vertexRootIndices = (ref this.vertexRootIndices).MC2ToRawBytes<int>();
				shareSerializationData.vertexParentIndices = (ref this.vertexParentIndices).MC2ToRawBytes<int>();
				shareSerializationData.vertexChildIndexArray = (ref this.vertexChildIndexArray).MC2ToRawBytes<uint>();
				shareSerializationData.vertexChildDataArray = (ref this.vertexChildDataArray).MC2ToRawBytes<ushort>();
				shareSerializationData.vertexLocalPositions = (ref this.vertexLocalPositions).MC2ToRawBytes<float3>();
				shareSerializationData.vertexLocalRotations = (ref this.vertexLocalRotations).MC2ToRawBytes<quaternion>();
				shareSerializationData.normalAdjustmentRotations = (ref this.normalAdjustmentRotations).MC2ToRawBytes<quaternion>();
				shareSerializationData.baseLineFlags = (ref this.baseLineFlags).MC2ToRawBytes<ExBitFlag8>();
				shareSerializationData.baseLineStartDataIndices = (ref this.baseLineStartDataIndices).MC2ToRawBytes<ushort>();
				shareSerializationData.baseLineDataCounts = (ref this.baseLineDataCounts).MC2ToRawBytes<ushort>();
				shareSerializationData.baseLineData = (ref this.baseLineData).MC2ToRawBytes<ushort>();
				DataUtility.ArrayCopy<int>(this.customSkinningBoneIndices, ref shareSerializationData.customSkinningBoneIndices);
				DataUtility.ArrayCopy<ushort>(this.centerFixedList, ref shareSerializationData.centerFixedList);
				if (this.localCenterPosition.IsCreated)
				{
					shareSerializationData.localCenterPosition = this.localCenterPosition.Value;
				}
				shareSerializationData.centerWorldPosition = this.centerWorldPosition;
				shareSerializationData.centerWorldRotation = this.centerWorldRotation;
				shareSerializationData.centerWorldScale = this.centerWorldScale;
				shareSerializationData.toProxyMatrix = this.toProxyMatrix;
				shareSerializationData.toProxyRotation = this.toProxyRotation;
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			return shareSerializationData;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00034E2C File Offset: 0x0003302C
		public static VirtualMesh ShareDeserialize(VirtualMesh.ShareSerializationData sdata)
		{
			VirtualMesh virtualMesh = new VirtualMesh();
			virtualMesh.isManaged = true;
			try
			{
				virtualMesh.name = sdata.name;
				virtualMesh.meshType = sdata.meshType;
				virtualMesh.isBoneCloth = sdata.isBoneCloth;
				virtualMesh.referenceIndices.Deserialize(sdata.referenceIndices);
				virtualMesh.attributes.Deserialize(sdata.attributes);
				virtualMesh.localPositions.Deserialize(sdata.localPositions);
				virtualMesh.localNormals.Deserialize(sdata.localNormals);
				virtualMesh.localTangents.Deserialize(sdata.localTangents);
				virtualMesh.uv.Deserialize(sdata.uv);
				virtualMesh.boneWeights.Deserialize(sdata.boneWeights);
				virtualMesh.triangles.Deserialize(sdata.triangles);
				virtualMesh.lines.Deserialize(sdata.lines);
				virtualMesh.centerTransformIndex = sdata.centerTransformIndex;
				virtualMesh.initLocalToWorld = sdata.initLocalToWorld;
				virtualMesh.initWorldToLocal = sdata.initWorldToLocal;
				virtualMesh.initRotation = sdata.initRotation;
				virtualMesh.initInverseRotation = sdata.initInverseRotation;
				virtualMesh.initScale = sdata.initScale;
				virtualMesh.skinRootIndex = sdata.skinRootIndex;
				virtualMesh.skinBoneTransformIndices.Deserialize(sdata.skinBoneTransformIndices);
				virtualMesh.skinBoneBindPoses.Deserialize(sdata.skinBoneBindPoses);
				virtualMesh.transformData = TransformData.ShareDeserialize(sdata.transformData);
				virtualMesh.boundingBox = new NativeReference<AABB>(sdata.boundingBox, Allocator.Persistent);
				virtualMesh.averageVertexDistance = new NativeReference<float>(sdata.averageVertexDistance, Allocator.Persistent);
				virtualMesh.maxVertexDistance = new NativeReference<float>(sdata.maxVertexDistance, Allocator.Persistent);
				virtualMesh.vertexToTriangles = NativeArrayExtensions.MC2FromRawBytes<FixedList32Bytes<uint>>(sdata.vertexToTriangles, Allocator.Persistent);
				virtualMesh.vertexToVertexIndexArray = NativeArrayExtensions.MC2FromRawBytes<uint>(sdata.vertexToVertexIndexArray, Allocator.Persistent);
				virtualMesh.vertexToVertexDataArray = NativeArrayExtensions.MC2FromRawBytes<ushort>(sdata.vertexToVertexDataArray, Allocator.Persistent);
				virtualMesh.edges = NativeArrayExtensions.MC2FromRawBytes<int2>(sdata.edges, Allocator.Persistent);
				virtualMesh.edgeFlags = NativeArrayExtensions.MC2FromRawBytes<ExBitFlag8>(sdata.edgeFlags, Allocator.Persistent);
				virtualMesh.edgeToTriangles = NativeMultiHashMapExtensions.MC2Deserialize(sdata.edgeToTrianglesKeys, sdata.edgeToTrianglesValues);
				virtualMesh.vertexBindPosePositions = NativeArrayExtensions.MC2FromRawBytes<float3>(sdata.vertexBindPosePositions, Allocator.Persistent);
				virtualMesh.vertexBindPoseRotations = NativeArrayExtensions.MC2FromRawBytes<quaternion>(sdata.vertexBindPoseRotations, Allocator.Persistent);
				virtualMesh.vertexToTransformRotations = NativeArrayExtensions.MC2FromRawBytes<quaternion>(sdata.vertexToTransformRotations, Allocator.Persistent);
				virtualMesh.vertexDepths = NativeArrayExtensions.MC2FromRawBytes<float>(sdata.vertexDepths, Allocator.Persistent);
				virtualMesh.vertexRootIndices = NativeArrayExtensions.MC2FromRawBytes<int>(sdata.vertexRootIndices, Allocator.Persistent);
				virtualMesh.vertexParentIndices = NativeArrayExtensions.MC2FromRawBytes<int>(sdata.vertexParentIndices, Allocator.Persistent);
				virtualMesh.vertexChildIndexArray = NativeArrayExtensions.MC2FromRawBytes<uint>(sdata.vertexChildIndexArray, Allocator.Persistent);
				virtualMesh.vertexChildDataArray = NativeArrayExtensions.MC2FromRawBytes<ushort>(sdata.vertexChildDataArray, Allocator.Persistent);
				virtualMesh.vertexLocalPositions = NativeArrayExtensions.MC2FromRawBytes<float3>(sdata.vertexLocalPositions, Allocator.Persistent);
				virtualMesh.vertexLocalRotations = NativeArrayExtensions.MC2FromRawBytes<quaternion>(sdata.vertexLocalRotations, Allocator.Persistent);
				virtualMesh.normalAdjustmentRotations = NativeArrayExtensions.MC2FromRawBytes<quaternion>(sdata.normalAdjustmentRotations, Allocator.Persistent);
				virtualMesh.baseLineFlags = NativeArrayExtensions.MC2FromRawBytes<ExBitFlag8>(sdata.baseLineFlags, Allocator.Persistent);
				virtualMesh.baseLineStartDataIndices = NativeArrayExtensions.MC2FromRawBytes<ushort>(sdata.baseLineStartDataIndices, Allocator.Persistent);
				virtualMesh.baseLineDataCounts = NativeArrayExtensions.MC2FromRawBytes<ushort>(sdata.baseLineDataCounts, Allocator.Persistent);
				virtualMesh.baseLineData = NativeArrayExtensions.MC2FromRawBytes<ushort>(sdata.baseLineData, Allocator.Persistent);
				DataUtility.ArrayCopy<int>(sdata.customSkinningBoneIndices, ref virtualMesh.customSkinningBoneIndices);
				DataUtility.ArrayCopy<ushort>(sdata.centerFixedList, ref virtualMesh.centerFixedList);
				virtualMesh.localCenterPosition = new NativeReference<float3>(sdata.localCenterPosition, Allocator.Persistent);
				virtualMesh.centerWorldPosition = sdata.centerWorldPosition;
				virtualMesh.centerWorldRotation = sdata.centerWorldRotation;
				virtualMesh.centerWorldScale = sdata.centerWorldScale;
				virtualMesh.toProxyMatrix = sdata.toProxyMatrix;
				virtualMesh.toProxyRotation = sdata.toProxyRotation;
				virtualMesh.result.SetSuccess();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				virtualMesh.result.SetError(Define.Result.PreBuildData_VirtualMeshDeserializationException);
			}
			return virtualMesh;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00035210 File Offset: 0x00033410
		public VirtualMesh.UniqueSerializationData UniqueSerialize()
		{
			VirtualMesh.UniqueSerializationData uniqueSerializationData = new VirtualMesh.UniqueSerializationData();
			try
			{
				VirtualMesh.UniqueSerializationData uniqueSerializationData2 = uniqueSerializationData;
				TransformData transformData = this.transformData;
				uniqueSerializationData2.transformData = ((transformData != null) ? transformData.UniqueSerialize() : null);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			return uniqueSerializationData;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00035258 File Offset: 0x00033458
		internal void CalcAverageAndMaxVertexDistanceRun()
		{
			try
			{
				if (!this.averageVertexDistance.IsCreated)
				{
					this.averageVertexDistance = new NativeReference<float>(Allocator.Persistent, NativeArrayOptions.ClearMemory);
				}
				if (!this.maxVertexDistance.IsCreated)
				{
					this.maxVertexDistance = new NativeReference<float>(Allocator.Persistent, NativeArrayOptions.ClearMemory);
				}
				this.averageVertexDistance.Value = 0f;
				this.maxVertexDistance.Value = 0f;
				using (NativeReference<int> nativeReference = new NativeReference<int>(Allocator.TempJob, NativeArrayOptions.ClearMemory))
				{
					if (this.TriangleCount > 0)
					{
						new VirtualMesh.Work_AverageTriangleDistanceJob
						{
							vcnt = this.VertexCount,
							tcnt = this.TriangleCount,
							localPositions = this.localPositions.GetNativeArray(),
							triangles = this.triangles.GetNativeArray(),
							averageVertexDistance = this.averageVertexDistance,
							averageCount = nativeReference,
							maxVertexDistance = this.maxVertexDistance
						}.Run<VirtualMesh.Work_AverageTriangleDistanceJob>();
					}
					if (this.LineCount > 0)
					{
						new VirtualMesh.Work_AverageLineDistanceJob
						{
							vcnt = this.VertexCount,
							lcnt = this.LineCount,
							localPositions = this.localPositions.GetNativeArray(),
							lines = this.lines.GetNativeArray(),
							averageVertexDistance = this.averageVertexDistance,
							averageCount = nativeReference,
							maxVertexDistance = this.maxVertexDistance
						}.Run<VirtualMesh.Work_AverageLineDistanceJob>();
					}
					int value = nativeReference.Value;
					if (value > 0)
					{
						float value2 = this.averageVertexDistance.Value;
						this.averageVertexDistance.Value = math.sqrt(value2 / (float)value);
						this.maxVertexDistance.Value = math.sqrt(this.maxVertexDistance.Value);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				this.result.SetError(Define.Result.Reduction_CalcAverageException);
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00035468 File Offset: 0x00033668
		internal GridMap<int> CreateVertexIndexGridMapRun(float gridSize)
		{
			int vertexCount = this.VertexCount;
			GridMap<int> gridMap = new GridMap<int>(vertexCount);
			if (vertexCount > 0)
			{
				new VirtualMesh.Work_AddVertexIndexGirdMapJob
				{
					gridSize = gridSize,
					vcnt = vertexCount,
					positins = this.localPositions.GetNativeArray(),
					gridMap = gridMap.GetMultiHashMap()
				}.Run<VirtualMesh.Work_AddVertexIndexGirdMapJob>();
			}
			return gridMap;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000354C8 File Offset: 0x000336C8
		public VirtualMeshRaycastHit IntersectRayMesh(float3 rayPos, float3 rayDir, bool doubleSide, float pointRadius)
		{
			Transform centerTransform = this.GetCenterTransform();
			float3 @float = centerTransform.InverseTransformPoint(rayPos);
			float3 float2 = centerTransform.InverseTransformDirection(rayDir);
			float3 float3 = rayPos + rayDir * 1000f;
			float3 float4 = centerTransform.InverseTransformPoint(float3);
			float num = pointRadius / centerTransform.lossyScale.x;
			int num2 = 100;
			VirtualMeshRaycastHit virtualMeshRaycastHit;
			using (NativeList<VirtualMeshRaycastHit> nativeList = new NativeList<VirtualMeshRaycastHit>(num2, Allocator.TempJob))
			{
				JobHandle jobHandle = default(JobHandle);
				jobHandle = new VirtualMesh.Work_IntersectTriangleJob
				{
					localRayPos = @float,
					localRayDir = float2,
					localRayEndPos = float4,
					doubleSide = doubleSide,
					localPositions = this.localPositions.GetNativeArray(),
					triangles = this.triangles.GetNativeArray(),
					hitList = nativeList.AsParallelWriter()
				}.Schedule(this.TriangleCount, 16, jobHandle);
				jobHandle = new VirtualMesh.Work_IntersectEdgeJob
				{
					localRayPos = @float,
					localRayDir = float2,
					localRayEndPos = float4,
					rayDir = float2,
					localEdgeRadius = num,
					localPositions = this.localPositions.GetNativeArray(),
					edges = this.edges,
					edgeToTriangles = this.edgeToTriangles,
					hitList = nativeList.AsParallelWriter()
				}.Schedule(this.EdgeCount, 16, jobHandle);
				new VirtualMesh.Work_IntersetcSortJob
				{
					hitList = nativeList
				}.Schedule(jobHandle).Complete();
				VirtualMeshRaycastHit virtualMeshRaycastHit2;
				if (nativeList.Length <= 0)
				{
					virtualMeshRaycastHit = default(VirtualMeshRaycastHit);
					virtualMeshRaycastHit2 = virtualMeshRaycastHit;
				}
				else
				{
					virtualMeshRaycastHit2 = nativeList[0];
				}
				virtualMeshRaycastHit = virtualMeshRaycastHit2;
			}
			return virtualMeshRaycastHit;
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x000356B0 File Offset: 0x000338B0
		public float InitCalcScale
		{
			get
			{
				return this.initScale.x;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x000356BD File Offset: 0x000338BD
		public bool IsSuccess
		{
			get
			{
				return this.result.IsSuccess();
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x000356CA File Offset: 0x000338CA
		public bool IsError
		{
			get
			{
				return this.result.IsError();
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x000356D7 File Offset: 0x000338D7
		public bool IsProcess
		{
			get
			{
				return this.result.IsProcess();
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x000356E4 File Offset: 0x000338E4
		public int VertexCount
		{
			get
			{
				return this.localPositions.Count;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x000356F1 File Offset: 0x000338F1
		public int TriangleCount
		{
			get
			{
				return this.triangles.Count;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x000356FE File Offset: 0x000338FE
		public int LineCount
		{
			get
			{
				return this.lines.Count;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x0003570B File Offset: 0x0003390B
		public int SkinBoneCount
		{
			get
			{
				return this.skinBoneTransformIndices.Count;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00035718 File Offset: 0x00033918
		public int TransformCount
		{
			get
			{
				TransformData transformData = this.transformData;
				if (transformData == null)
				{
					return 0;
				}
				return transformData.Count;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0003572B File Offset: 0x0003392B
		public bool IsProxy
		{
			get
			{
				return this.meshType == VirtualMesh.MeshType.ProxyMesh || this.meshType == VirtualMesh.MeshType.ProxyBoneMesh;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00035741 File Offset: 0x00033941
		public bool IsMapping
		{
			get
			{
				return this.meshType == VirtualMesh.MeshType.Mapping;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x0003574C File Offset: 0x0003394C
		public int BaseLineCount
		{
			get
			{
				if (!this.baseLineStartDataIndices.IsCreated)
				{
					return 0;
				}
				return this.baseLineStartDataIndices.Length;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00035768 File Offset: 0x00033968
		public int EdgeCount
		{
			get
			{
				if (!this.edges.IsCreated)
				{
					return 0;
				}
				return this.edges.Length;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00035784 File Offset: 0x00033984
		public int CustomSkinningBoneCount
		{
			get
			{
				int[] array = this.customSkinningBoneIndices;
				if (array == null)
				{
					return 0;
				}
				return array.Length;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x00035794 File Offset: 0x00033994
		public int CenterFixedPointCount
		{
			get
			{
				ushort[] array = this.centerFixedList;
				if (array == null)
				{
					return 0;
				}
				return array.Length;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x000357A4 File Offset: 0x000339A4
		public int NormalAdjustmentRotationCount
		{
			get
			{
				if (!this.normalAdjustmentRotations.IsCreated)
				{
					return 0;
				}
				return this.normalAdjustmentRotations.Length;
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000357C0 File Offset: 0x000339C0
		public VirtualMesh()
		{
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00035868 File Offset: 0x00033A68
		public VirtualMesh(bool initialize)
		{
			if (initialize)
			{
				this.transformData = new TransformData(100);
				this.averageVertexDistance = new NativeReference<float>(0f, Allocator.Persistent);
				this.maxVertexDistance = new NativeReference<float>(0f, Allocator.Persistent);
				this.result.SetProcess();
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00035954 File Offset: 0x00033B54
		public VirtualMesh(string name)
			: this(true)
		{
			this.name = name;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00035964 File Offset: 0x00033B64
		public void Dispose()
		{
			if (this.isManaged)
			{
				return;
			}
			this.result.Clear();
			this.referenceIndices.Dispose();
			this.attributes.Dispose();
			this.localPositions.Dispose();
			this.localNormals.Dispose();
			this.localTangents.Dispose();
			this.uv.Dispose();
			this.boneWeights.Dispose();
			this.triangles.Dispose();
			this.lines.Dispose();
			this.skinBoneTransformIndices.Dispose();
			this.skinBoneBindPoses.Dispose();
			if (this.joinIndices.IsCreated)
			{
				this.joinIndices.Dispose();
			}
			if (this.vertexToTriangles.IsCreated)
			{
				this.vertexToTriangles.Dispose();
			}
			if (this.vertexToVertexIndexArray.IsCreated)
			{
				this.vertexToVertexIndexArray.Dispose();
			}
			if (this.vertexToVertexDataArray.IsCreated)
			{
				this.vertexToVertexDataArray.Dispose();
			}
			if (this.edges.IsCreated)
			{
				this.edges.Dispose();
			}
			if (this.edgeFlags.IsCreated)
			{
				this.edgeFlags.Dispose();
			}
			if (this.edgeToTriangles.IsCreated)
			{
				this.edgeToTriangles.Dispose();
			}
			if (this.vertexBindPosePositions.IsCreated)
			{
				this.vertexBindPosePositions.Dispose();
			}
			if (this.vertexBindPoseRotations.IsCreated)
			{
				this.vertexBindPoseRotations.Dispose();
			}
			if (this.vertexToTransformRotations.IsCreated)
			{
				this.vertexToTransformRotations.Dispose();
			}
			if (this.vertexRootIndices.IsCreated)
			{
				this.vertexRootIndices.Dispose();
			}
			if (this.vertexParentIndices.IsCreated)
			{
				this.vertexParentIndices.Dispose();
			}
			if (this.vertexChildIndexArray.IsCreated)
			{
				this.vertexChildIndexArray.Dispose();
			}
			if (this.vertexChildDataArray.IsCreated)
			{
				this.vertexChildDataArray.Dispose();
			}
			if (this.baseLineFlags.IsCreated)
			{
				this.baseLineFlags.Dispose();
			}
			if (this.baseLineStartDataIndices.IsCreated)
			{
				this.baseLineStartDataIndices.Dispose();
			}
			if (this.baseLineDataCounts.IsCreated)
			{
				this.baseLineDataCounts.Dispose();
			}
			if (this.baseLineData.IsCreated)
			{
				this.baseLineData.Dispose();
			}
			if (this.localCenterPosition.IsCreated)
			{
				this.localCenterPosition.Dispose();
			}
			if (this.vertexLocalPositions.IsCreated)
			{
				this.vertexLocalPositions.Dispose();
			}
			if (this.vertexLocalRotations.IsCreated)
			{
				this.vertexLocalRotations.Dispose();
			}
			if (this.normalAdjustmentRotations.IsCreated)
			{
				this.normalAdjustmentRotations.Dispose();
			}
			if (this.vertexDepths.IsCreated)
			{
				this.vertexDepths.Dispose();
			}
			if (this.boundingBox.IsCreated)
			{
				this.boundingBox.Dispose();
			}
			if (this.averageVertexDistance.IsCreated)
			{
				this.averageVertexDistance.Dispose();
			}
			if (this.maxVertexDistance.IsCreated)
			{
				this.maxVertexDistance.Dispose();
			}
			TransformData transformData = this.transformData;
			if (transformData == null)
			{
				return;
			}
			transformData.Dispose();
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00035C7E File Offset: 0x00033E7E
		public void SetName(string newName)
		{
			this.name = newName;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00035C87 File Offset: 0x00033E87
		public bool IsValid()
		{
			return this.transformData != null && (this.centerTransformIndex < 0 || this.transformData.IsEmpty || !(this.transformData.GetTransformFromIndex(this.centerTransformIndex) == null));
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00035CC8 File Offset: 0x00033EC8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("===== " + this.name + " =====");
			stringBuilder.AppendLine("Result:" + this.result.GetResultString());
			stringBuilder.AppendLine(string.Format("Type:{0}", this.meshType));
			stringBuilder.AppendLine(string.Format("Vertex:{0}", this.VertexCount));
			stringBuilder.AppendLine(string.Format("Line:{0}", this.LineCount));
			stringBuilder.AppendLine(string.Format("Triangle:{0}", this.TriangleCount));
			stringBuilder.AppendLine(string.Format("Edge:{0}", this.EdgeCount));
			stringBuilder.AppendLine(string.Format("SkinBone:{0}", this.SkinBoneCount));
			stringBuilder.AppendLine(string.Format("Transform:{0}", this.TransformCount));
			if (this.averageVertexDistance.IsCreated)
			{
				stringBuilder.AppendLine(string.Format("avgDist:{0}", this.averageVertexDistance.Value));
			}
			if (this.maxVertexDistance.IsCreated)
			{
				stringBuilder.AppendLine(string.Format("maxDist:{0}", this.maxVertexDistance.Value));
			}
			if (this.boundingBox.IsCreated)
			{
				stringBuilder.AppendLine(string.Format("AABB:{0}", this.boundingBox.Value));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("<<< Proxy >>>");
			stringBuilder.AppendLine(string.Format("BaseLine:{0}", this.BaseLineCount));
			stringBuilder.AppendLine(string.Format("EdgeCount:{0}", this.EdgeCount));
			int num = (this.edgeToTriangles.IsCreated ? this.edgeToTriangles.Count() : 0);
			stringBuilder.AppendLine(string.Format("edgeToTriangles:{0}", num));
			stringBuilder.AppendLine(string.Format("CustomSkinningBoneCount:{0}", this.CustomSkinningBoneCount));
			stringBuilder.AppendLine(string.Format("CenterFixedPointCount:{0}", this.CenterFixedPointCount));
			stringBuilder.AppendLine(string.Format("NormalAdjustmentRotationCount:{0}", this.NormalAdjustmentRotationCount));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("<<< Mapping >>>");
			stringBuilder.AppendLine(string.Format("centerWorldPosition:{0}", this.centerWorldPosition));
			stringBuilder.AppendLine();
			StringBuilder stringBuilder2 = stringBuilder;
			TransformData transformData = this.transformData;
			stringBuilder2.AppendLine(((transformData != null) ? transformData.ToString() : null) ?? "(none)");
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		// Token: 0x040008C1 RID: 2241
		public string name = string.Empty;

		// Token: 0x040008C2 RID: 2242
		public ResultCode result;

		// Token: 0x040008C3 RID: 2243
		public bool isManaged;

		// Token: 0x040008C4 RID: 2244
		public VirtualMesh.MeshType meshType;

		// Token: 0x040008C5 RID: 2245
		public bool isBoneCloth;

		// Token: 0x040008C6 RID: 2246
		public ExSimpleNativeArray<int> referenceIndices = new ExSimpleNativeArray<int>();

		// Token: 0x040008C7 RID: 2247
		public ExSimpleNativeArray<VertexAttribute> attributes = new ExSimpleNativeArray<VertexAttribute>();

		// Token: 0x040008C8 RID: 2248
		public ExSimpleNativeArray<float3> localPositions = new ExSimpleNativeArray<float3>();

		// Token: 0x040008C9 RID: 2249
		public ExSimpleNativeArray<float3> localNormals = new ExSimpleNativeArray<float3>();

		// Token: 0x040008CA RID: 2250
		public ExSimpleNativeArray<float3> localTangents = new ExSimpleNativeArray<float3>();

		// Token: 0x040008CB RID: 2251
		public ExSimpleNativeArray<float2> uv = new ExSimpleNativeArray<float2>();

		// Token: 0x040008CC RID: 2252
		public ExSimpleNativeArray<VirtualMeshBoneWeight> boneWeights = new ExSimpleNativeArray<VirtualMeshBoneWeight>();

		// Token: 0x040008CD RID: 2253
		public ExSimpleNativeArray<int3> triangles = new ExSimpleNativeArray<int3>();

		// Token: 0x040008CE RID: 2254
		public ExSimpleNativeArray<int2> lines = new ExSimpleNativeArray<int2>();

		// Token: 0x040008CF RID: 2255
		public int centerTransformIndex = -1;

		// Token: 0x040008D0 RID: 2256
		public float4x4 initLocalToWorld;

		// Token: 0x040008D1 RID: 2257
		public float4x4 initWorldToLocal;

		// Token: 0x040008D2 RID: 2258
		public quaternion initRotation;

		// Token: 0x040008D3 RID: 2259
		public quaternion initInverseRotation;

		// Token: 0x040008D4 RID: 2260
		public float3 initScale;

		// Token: 0x040008D5 RID: 2261
		public int skinRootIndex = -1;

		// Token: 0x040008D6 RID: 2262
		public ExSimpleNativeArray<int> skinBoneTransformIndices = new ExSimpleNativeArray<int>();

		// Token: 0x040008D7 RID: 2263
		public ExSimpleNativeArray<float4x4> skinBoneBindPoses = new ExSimpleNativeArray<float4x4>();

		// Token: 0x040008D8 RID: 2264
		public TransformData transformData;

		// Token: 0x040008D9 RID: 2265
		public NativeReference<AABB> boundingBox;

		// Token: 0x040008DA RID: 2266
		public NativeReference<float> averageVertexDistance;

		// Token: 0x040008DB RID: 2267
		public NativeReference<float> maxVertexDistance;

		// Token: 0x040008DC RID: 2268
		public DataChunk mergeChunk;

		// Token: 0x040008DD RID: 2269
		public NativeArray<int> joinIndices;

		// Token: 0x040008DE RID: 2270
		public NativeArray<FixedList32Bytes<uint>> vertexToTriangles;

		// Token: 0x040008DF RID: 2271
		public NativeArray<uint> vertexToVertexIndexArray;

		// Token: 0x040008E0 RID: 2272
		public NativeArray<ushort> vertexToVertexDataArray;

		// Token: 0x040008E1 RID: 2273
		public NativeArray<int2> edges;

		// Token: 0x040008E2 RID: 2274
		public const byte EdgeFlag_Cut = 1;

		// Token: 0x040008E3 RID: 2275
		public NativeArray<ExBitFlag8> edgeFlags;

		// Token: 0x040008E4 RID: 2276
		public NativeParallelMultiHashMap<int2, ushort> edgeToTriangles;

		// Token: 0x040008E5 RID: 2277
		public NativeArray<float3> vertexBindPosePositions;

		// Token: 0x040008E6 RID: 2278
		public NativeArray<quaternion> vertexBindPoseRotations;

		// Token: 0x040008E7 RID: 2279
		public NativeArray<quaternion> vertexToTransformRotations;

		// Token: 0x040008E8 RID: 2280
		public NativeArray<float> vertexDepths;

		// Token: 0x040008E9 RID: 2281
		public NativeArray<int> vertexRootIndices;

		// Token: 0x040008EA RID: 2282
		public NativeArray<int> vertexParentIndices;

		// Token: 0x040008EB RID: 2283
		public NativeArray<uint> vertexChildIndexArray;

		// Token: 0x040008EC RID: 2284
		public NativeArray<ushort> vertexChildDataArray;

		// Token: 0x040008ED RID: 2285
		public NativeArray<float3> vertexLocalPositions;

		// Token: 0x040008EE RID: 2286
		public NativeArray<quaternion> vertexLocalRotations;

		// Token: 0x040008EF RID: 2287
		public NativeArray<quaternion> normalAdjustmentRotations;

		// Token: 0x040008F0 RID: 2288
		public const byte BaseLineFlag_IncludeLine = 1;

		// Token: 0x040008F1 RID: 2289
		public NativeArray<ExBitFlag8> baseLineFlags;

		// Token: 0x040008F2 RID: 2290
		public NativeArray<ushort> baseLineStartDataIndices;

		// Token: 0x040008F3 RID: 2291
		public NativeArray<ushort> baseLineDataCounts;

		// Token: 0x040008F4 RID: 2292
		public NativeArray<ushort> baseLineData;

		// Token: 0x040008F5 RID: 2293
		public int[] customSkinningBoneIndices;

		// Token: 0x040008F6 RID: 2294
		public ushort[] centerFixedList;

		// Token: 0x040008F7 RID: 2295
		public NativeReference<float3> localCenterPosition;

		// Token: 0x040008F8 RID: 2296
		public VirtualMesh mappingProxyMesh;

		// Token: 0x040008F9 RID: 2297
		public float3 centerWorldPosition;

		// Token: 0x040008FA RID: 2298
		public quaternion centerWorldRotation;

		// Token: 0x040008FB RID: 2299
		public float3 centerWorldScale;

		// Token: 0x040008FC RID: 2300
		public float4x4 toProxyMatrix;

		// Token: 0x040008FD RID: 2301
		public quaternion toProxyRotation;

		// Token: 0x040008FE RID: 2302
		public int mappingId;

		// Token: 0x02000124 RID: 292
		[BurstCompile]
		private struct Import_GenerateTangentJob : IJobParallelFor
		{
			// Token: 0x0600063F RID: 1599 RVA: 0x00035F98 File Offset: 0x00034198
			public void Execute(int vindex)
			{
				float3 @float = this.localNormals[vindex];
				float3 float2 = math.up();
				if ((double)math.dot(@float, float2) < 0.9)
				{
					float2 = math.normalize(math.cross(@float, float2));
				}
				else
				{
					float2 = math.normalize(math.cross(@float, math.right()));
				}
				this.localTangents[vindex] = float2;
			}

			// Token: 0x040008FF RID: 2303
			[ReadOnly]
			public NativeArray<float3> localNormals;

			// Token: 0x04000900 RID: 2304
			[WriteOnly]
			public NativeArray<float3> localTangents;
		}

		// Token: 0x02000125 RID: 293
		[BurstCompile]
		private struct Import_CalcSkinningJob : IJobParallelFor
		{
			// Token: 0x06000640 RID: 1600 RVA: 0x00035FF8 File Offset: 0x000341F8
			public void Execute(int vindex)
			{
				VirtualMeshBoneWeight virtualMeshBoneWeight = this.boneWeights[vindex];
				int count = virtualMeshBoneWeight.Count;
				float3 @float = 0;
				float3 float2 = 0;
				float3 float3 = 0;
				for (int i = 0; i < count; i++)
				{
					float num = virtualMeshBoneWeight.weights[i];
					int num2 = virtualMeshBoneWeight.boneIndices[i];
					float4x4 float4x = this.bindPoses[num2];
					float4 float4 = new float4(this.localPositions[vindex], 1f);
					float4 float5 = new float4(this.localNormals[vindex], 0f);
					float4 float6 = new float4(this.localTangents[vindex], 0f);
					float3 xyz = math.mul(float4x, float4).xyz;
					float3 xyz2 = math.mul(float4x, float5).xyz;
					float3 xyz3 = math.mul(float4x, float6).xyz;
					int num3 = this.skinBoneTransformIndices[num2];
					float3 float7 = this.transformPositionArray[num3];
					quaternion quaternion = this.transformRotationArray[num3];
					float3 float8 = this.transformScaleArray[num3];
					MathUtility.TransformPositionNormalTangent(in float7, in quaternion, in float8, ref xyz, ref xyz2, ref xyz3);
					@float += xyz * num;
					float2 += xyz2 * num;
					float3 += xyz3 * num;
				}
				this.localPositions[vindex] = MathUtility.TransformPoint(in @float, in this.toM);
				this.localNormals[vindex] = MathUtility.TransformNormal(in float2, in this.toM, math.up());
				this.localTangents[vindex] = MathUtility.TransformNormal(in float3, in this.toM, math.right());
			}

			// Token: 0x04000901 RID: 2305
			public NativeArray<float3> localPositions;

			// Token: 0x04000902 RID: 2306
			public NativeArray<float3> localNormals;

			// Token: 0x04000903 RID: 2307
			public NativeArray<float3> localTangents;

			// Token: 0x04000904 RID: 2308
			[ReadOnly]
			public NativeArray<VirtualMeshBoneWeight> boneWeights;

			// Token: 0x04000905 RID: 2309
			[ReadOnly]
			public NativeArray<int> skinBoneTransformIndices;

			// Token: 0x04000906 RID: 2310
			[ReadOnly]
			public NativeArray<float4x4> bindPoses;

			// Token: 0x04000907 RID: 2311
			[ReadOnly]
			public NativeArray<float3> transformPositionArray;

			// Token: 0x04000908 RID: 2312
			[ReadOnly]
			public NativeArray<quaternion> transformRotationArray;

			// Token: 0x04000909 RID: 2313
			[ReadOnly]
			public NativeArray<float3> transformScaleArray;

			// Token: 0x0400090A RID: 2314
			public float4x4 toM;
		}

		// Token: 0x02000126 RID: 294
		[BurstCompile]
		private struct Import_BoneWeightJob1 : IJob
		{
			// Token: 0x06000641 RID: 1601 RVA: 0x000361C8 File Offset: 0x000343C8
			public void Execute()
			{
				int num = 0;
				for (int i = 0; i < this.vcnt; i++)
				{
					this.startBoneWeightIndices[i] = num;
					num += (int)this.bonesPerVertexArray[i];
				}
			}

			// Token: 0x0400090B RID: 2315
			public int vcnt;

			// Token: 0x0400090C RID: 2316
			[ReadOnly]
			public NativeArray<byte> bonesPerVertexArray;

			// Token: 0x0400090D RID: 2317
			[WriteOnly]
			public NativeArray<int> startBoneWeightIndices;
		}

		// Token: 0x02000127 RID: 295
		[BurstCompile]
		private struct Import_BoneWeightJob2 : IJobParallelFor
		{
			// Token: 0x06000642 RID: 1602 RVA: 0x00036204 File Offset: 0x00034404
			public void Execute(int vindex)
			{
				int num = this.startBoneWeightIndices[vindex];
				int num2 = (int)this.bonesPerVertexArray[vindex];
				VirtualMeshBoneWeight virtualMeshBoneWeight = default(VirtualMeshBoneWeight);
				int num3 = 0;
				int num4 = 0;
				while (num4 < num2 && num4 < 4)
				{
					BoneWeight1 boneWeight = this.boneWeightArray[num + num4];
					if (boneWeight.weight > 0f)
					{
						virtualMeshBoneWeight.weights[num3] = boneWeight.weight;
						virtualMeshBoneWeight.boneIndices[num3] = boneWeight.boneIndex;
						num3++;
					}
					num4++;
				}
				if (num2 > 4)
				{
					virtualMeshBoneWeight.AdjustWeight();
				}
				this.boneWeights[vindex] = virtualMeshBoneWeight;
			}

			// Token: 0x0400090E RID: 2318
			[ReadOnly]
			public NativeArray<int> startBoneWeightIndices;

			// Token: 0x0400090F RID: 2319
			[ReadOnly]
			public NativeArray<BoneWeight1> boneWeightArray;

			// Token: 0x04000910 RID: 2320
			[ReadOnly]
			public NativeArray<byte> bonesPerVertexArray;

			// Token: 0x04000911 RID: 2321
			[WriteOnly]
			public NativeArray<VirtualMeshBoneWeight> boneWeights;
		}

		// Token: 0x02000128 RID: 296
		[BurstCompile]
		private struct Import_BoneVertexJob : IJobParallelFor
		{
			// Token: 0x06000643 RID: 1603 RVA: 0x000362B0 File Offset: 0x000344B0
			public void Execute(int vindex)
			{
				float3 @float = this.transformPositions[vindex];
				quaternion quaternion = this.transformRotations[vindex];
				float3 float2 = this.transformScales[vindex];
				float3 float3 = MathUtility.InverseTransformPoint(in @float, in this.WtoL);
				float3 float4 = math.mul(quaternion, math.up());
				float3 float5 = math.mul(quaternion, math.forward());
				float4 = MathUtility.InverseTransformDirection(in float4, in this.WtoL);
				float5 = MathUtility.InverseTransformDirection(in float5, in this.WtoL);
				this.localPositions[vindex] = float3;
				this.localNormals[vindex] = float4;
				this.localTangents[vindex] = float5;
				VirtualMeshBoneWeight virtualMeshBoneWeight = new VirtualMeshBoneWeight(new int4(vindex, 0, 0, 0), new float4(1f, 0f, 0f, 0f));
				this.boneWeights[vindex] = virtualMeshBoneWeight;
				float4x4 float4x = math.mul(math.inverse(float4x4.TRS(@float, quaternion, float2)), this.LtoW);
				this.skinBoneBindPoses[vindex] = float4x;
			}

			// Token: 0x04000912 RID: 2322
			public float4x4 WtoL;

			// Token: 0x04000913 RID: 2323
			public float4x4 LtoW;

			// Token: 0x04000914 RID: 2324
			[ReadOnly]
			public NativeArray<float3> transformPositions;

			// Token: 0x04000915 RID: 2325
			[ReadOnly]
			public NativeArray<quaternion> transformRotations;

			// Token: 0x04000916 RID: 2326
			[ReadOnly]
			public NativeArray<float3> transformScales;

			// Token: 0x04000917 RID: 2327
			[WriteOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000918 RID: 2328
			[WriteOnly]
			public NativeArray<float3> localNormals;

			// Token: 0x04000919 RID: 2329
			[WriteOnly]
			public NativeArray<float3> localTangents;

			// Token: 0x0400091A RID: 2330
			[WriteOnly]
			public NativeArray<VirtualMeshBoneWeight> boneWeights;

			// Token: 0x0400091B RID: 2331
			[WriteOnly]
			public NativeArray<float4x4> skinBoneBindPoses;
		}

		// Token: 0x02000129 RID: 297
		[BurstCompile]
		private struct Select_PackVertexJob : IJob
		{
			// Token: 0x06000644 RID: 1604 RVA: 0x000363B4 File Offset: 0x000345B4
			public void Execute()
			{
				for (int i = 0; i < this.vertexCount; i++)
				{
					int num = this.newVertexRemapIndices[i];
					if (num >= 0)
					{
						this.newReferenceIndices[num] = i;
						this.newAttributes[num] = this.attributes[i];
						this.newLocalPositions[num] = this.localPositions[i];
						this.newLocalNormals[num] = this.localNormals[i];
						this.newLocalTangents[num] = this.localTangents[i];
						this.newUv[num] = this.uv[i];
						this.newBoneWeights[num] = this.boneWeights[i];
					}
				}
			}

			// Token: 0x0400091C RID: 2332
			public int vertexCount;

			// Token: 0x0400091D RID: 2333
			[ReadOnly]
			public NativeArray<int> newVertexRemapIndices;

			// Token: 0x0400091E RID: 2334
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x0400091F RID: 2335
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000920 RID: 2336
			[ReadOnly]
			public NativeArray<float3> localNormals;

			// Token: 0x04000921 RID: 2337
			[ReadOnly]
			public NativeArray<float3> localTangents;

			// Token: 0x04000922 RID: 2338
			[ReadOnly]
			public NativeArray<float2> uv;

			// Token: 0x04000923 RID: 2339
			[ReadOnly]
			public NativeArray<VirtualMeshBoneWeight> boneWeights;

			// Token: 0x04000924 RID: 2340
			[WriteOnly]
			public NativeArray<int> newReferenceIndices;

			// Token: 0x04000925 RID: 2341
			[WriteOnly]
			public NativeArray<VertexAttribute> newAttributes;

			// Token: 0x04000926 RID: 2342
			[WriteOnly]
			public NativeArray<float3> newLocalPositions;

			// Token: 0x04000927 RID: 2343
			[WriteOnly]
			public NativeArray<float3> newLocalNormals;

			// Token: 0x04000928 RID: 2344
			[WriteOnly]
			public NativeArray<float3> newLocalTangents;

			// Token: 0x04000929 RID: 2345
			[WriteOnly]
			public NativeArray<float2> newUv;

			// Token: 0x0400092A RID: 2346
			[WriteOnly]
			public NativeArray<VirtualMeshBoneWeight> newBoneWeights;
		}

		// Token: 0x0200012A RID: 298
		[BurstCompile]
		private struct Select_GridJob : IJob
		{
			// Token: 0x06000645 RID: 1605 RVA: 0x0003648C File Offset: 0x0003468C
			public void Execute()
			{
				for (int i = 0; i < this.vertexCount; i++)
				{
					float3 @float = this.meshPositions[i];
					int num = -1;
					foreach (int3 @int in GridMap<int>.GetArea(@float, this.searchRadius, this.gridMap, this.gridSize))
					{
						if (this.gridMap.ContainsKey(@int))
						{
							foreach (int num2 in this.gridMap.GetValuesForKey(@int))
							{
								float3 float2 = this.selectionPositions[num2];
								if (math.distance(@float, float2) <= this.searchRadius)
								{
									num = 1;
									break;
								}
							}
							if (num >= 0)
							{
								break;
							}
						}
					}
					this.newVertexRemapIndices[i] = num;
				}
				for (int j = 0; j < this.triangleCount; j++)
				{
					int3 int2 = this.meshTriangles[j];
					if (this.newVertexRemapIndices[int2.x] >= 0 || this.newVertexRemapIndices[int2.y] >= 0 || this.newVertexRemapIndices[int2.z] >= 0)
					{
						this.newTriangles.Add(in int2);
					}
				}
				int length = this.newTriangles.Length;
				for (int k = 0; k < length; k++)
				{
					int3 int3 = this.newTriangles[k];
					this.newVertexRemapIndices[int3.x] = 1;
					this.newVertexRemapIndices[int3.y] = 1;
					this.newVertexRemapIndices[int3.z] = 1;
				}
				int num3 = 0;
				for (int l = 0; l < this.vertexCount; l++)
				{
					if (this.newVertexRemapIndices[l] >= 0)
					{
						this.newVertexRemapIndices[l] = num3;
						num3++;
					}
				}
				this.newVertexCount.Value = num3;
				int length2 = this.newTriangles.Length;
				for (int m = 0; m < length2; m++)
				{
					int3 int4 = this.newTriangles[m];
					int4.x = this.newVertexRemapIndices[int4.x];
					int4.y = this.newVertexRemapIndices[int4.y];
					int4.z = this.newVertexRemapIndices[int4.z];
					this.newTriangles[m] = int4;
				}
			}

			// Token: 0x0400092B RID: 2347
			public float gridSize;

			// Token: 0x0400092C RID: 2348
			[ReadOnly]
			public NativeParallelMultiHashMap<int3, int> gridMap;

			// Token: 0x0400092D RID: 2349
			public int selectionCount;

			// Token: 0x0400092E RID: 2350
			[ReadOnly]
			public NativeArray<float3> selectionPositions;

			// Token: 0x0400092F RID: 2351
			[ReadOnly]
			public NativeArray<VertexAttribute> selectionAttributes;

			// Token: 0x04000930 RID: 2352
			public int vertexCount;

			// Token: 0x04000931 RID: 2353
			public int triangleCount;

			// Token: 0x04000932 RID: 2354
			public float searchRadius;

			// Token: 0x04000933 RID: 2355
			[ReadOnly]
			public NativeArray<float3> meshPositions;

			// Token: 0x04000934 RID: 2356
			[ReadOnly]
			public NativeArray<int3> meshTriangles;

			// Token: 0x04000935 RID: 2357
			public NativeList<int3> newTriangles;

			// Token: 0x04000936 RID: 2358
			public NativeArray<int> newVertexRemapIndices;

			// Token: 0x04000937 RID: 2359
			[WriteOnly]
			public NativeReference<int> newVertexCount;
		}

		// Token: 0x0200012B RID: 299
		[BurstCompile]
		private struct Add_CalcBindPoseJob : IJobParallelFor
		{
			// Token: 0x06000646 RID: 1606 RVA: 0x00036750 File Offset: 0x00034950
			public void Execute(int boneIndex)
			{
				int num = this.srcSkinBoneTransformIndices[boneIndex];
				float3 @float = this.srcTransformPositionArray[num];
				quaternion quaternion = this.srcTransformRotationArray[num];
				float3 float2 = this.srcTransformScaleArray[num];
				float4x4 float4x = math.mul(math.inverse(float4x4.TRS(@float, quaternion, float2)), this.dstCenterLocalToWorldMatrix);
				this.dstSkinBoneBindPoses[this.skinBoneOffset + boneIndex] = float4x;
			}

			// Token: 0x04000938 RID: 2360
			public int skinBoneOffset;

			// Token: 0x04000939 RID: 2361
			[ReadOnly]
			public NativeArray<int> srcSkinBoneTransformIndices;

			// Token: 0x0400093A RID: 2362
			[ReadOnly]
			public NativeArray<float3> srcTransformPositionArray;

			// Token: 0x0400093B RID: 2363
			[ReadOnly]
			public NativeArray<quaternion> srcTransformRotationArray;

			// Token: 0x0400093C RID: 2364
			[ReadOnly]
			public NativeArray<float3> srcTransformScaleArray;

			// Token: 0x0400093D RID: 2365
			public float4x4 dstCenterLocalToWorldMatrix;

			// Token: 0x0400093E RID: 2366
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float4x4> dstSkinBoneBindPoses;
		}

		// Token: 0x0200012C RID: 300
		[BurstCompile]
		private struct Add_CopyVerticesJob : IJobParallelFor
		{
			// Token: 0x06000647 RID: 1607 RVA: 0x000367BC File Offset: 0x000349BC
			public void Execute(int vindex)
			{
				int num = this.vertexOffset + vindex;
				float3 @float = this.srclocalPositions[vindex];
				float3 float2 = this.srclocalNormals[vindex];
				float3 float3 = this.srclocalTangents[vindex];
				@float = MathUtility.TransformPoint(in @float, in this.toM);
				float2 = MathUtility.TransformDirection(in float2, in this.toM);
				float3 = MathUtility.TransformDirection(in float3, in this.toM);
				this.dstlocalPositions[num] = @float;
				this.dstlocalNormals[num] = float2;
				this.dstlocalTangents[num] = float3;
				this.dstUV[num] = this.srcUV[vindex];
				if (vindex < this.srcBoneWeights.Length)
				{
					VirtualMeshBoneWeight virtualMeshBoneWeight = this.srcBoneWeights[vindex];
					virtualMeshBoneWeight.boneIndices += this.skinBoneOffset;
					for (int i = 0; i < 4; i++)
					{
						if (virtualMeshBoneWeight.weights[i] >= 1E-06f)
						{
							int num2 = virtualMeshBoneWeight.boneIndices[i];
							int num3 = this.dstSkinBoneIndices[num2];
							for (int j = 0; j < num2; j++)
							{
								if (this.dstSkinBoneIndices[j] == num3)
								{
									virtualMeshBoneWeight.boneIndices[i] = j;
									break;
								}
							}
						}
					}
					this.dstBoneWeights[num] = virtualMeshBoneWeight;
				}
				this.dstAttributes[num] = this.srcAttributes[vindex];
			}

			// Token: 0x0400093F RID: 2367
			public int vertexOffset;

			// Token: 0x04000940 RID: 2368
			public int skinBoneOffset;

			// Token: 0x04000941 RID: 2369
			public float4x4 toM;

			// Token: 0x04000942 RID: 2370
			[ReadOnly]
			public NativeArray<VertexAttribute> srcAttributes;

			// Token: 0x04000943 RID: 2371
			[ReadOnly]
			public NativeArray<float3> srclocalPositions;

			// Token: 0x04000944 RID: 2372
			[ReadOnly]
			public NativeArray<float3> srclocalNormals;

			// Token: 0x04000945 RID: 2373
			[ReadOnly]
			public NativeArray<float3> srclocalTangents;

			// Token: 0x04000946 RID: 2374
			[ReadOnly]
			public NativeArray<float2> srcUV;

			// Token: 0x04000947 RID: 2375
			[ReadOnly]
			public NativeArray<VirtualMeshBoneWeight> srcBoneWeights;

			// Token: 0x04000948 RID: 2376
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<VertexAttribute> dstAttributes;

			// Token: 0x04000949 RID: 2377
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> dstlocalPositions;

			// Token: 0x0400094A RID: 2378
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> dstlocalNormals;

			// Token: 0x0400094B RID: 2379
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> dstlocalTangents;

			// Token: 0x0400094C RID: 2380
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float2> dstUV;

			// Token: 0x0400094D RID: 2381
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<VirtualMeshBoneWeight> dstBoneWeights;

			// Token: 0x0400094E RID: 2382
			[ReadOnly]
			public NativeArray<int> dstSkinBoneIndices;
		}

		// Token: 0x0200012D RID: 301
		private struct MappingWorkData
		{
			// Token: 0x0400094F RID: 2383
			public float3 position;

			// Token: 0x04000950 RID: 2384
			public int vertexIndex;

			// Token: 0x04000951 RID: 2385
			public int proxyVertexIndex;

			// Token: 0x04000952 RID: 2386
			public float proxyVertexDistance;
		}

		// Token: 0x0200012E RID: 302
		[BurstCompile]
		private struct Mapping_DirectConnectionVertexDataJob : IJob
		{
			// Token: 0x06000648 RID: 1608 RVA: 0x0003693C File Offset: 0x00034B3C
			public void Execute()
			{
				for (int i = 0; i < this.vcnt; i++)
				{
					int num = this.joinIndices[this.mergeChunk.startIndex + i];
					VertexAttribute vertexAttribute = this.proxyAttributes[num];
					if (vertexAttribute.IsInvalid())
					{
						this.attributes[i] = VertexAttribute.Invalid;
					}
					else
					{
						float3 @float = this.localPositions[i];
						float3 float2 = MathUtility.TransformPoint(in @float, in this.toP);
						float3 float3 = this.proxyLocalPositions[num];
						VirtualMesh.MappingWorkData mappingWorkData = new VirtualMesh.MappingWorkData
						{
							position = float2,
							vertexIndex = i,
							proxyVertexIndex = num,
							proxyVertexDistance = math.distance(float2, float3)
						};
						this.mappingWorkData[i] = mappingWorkData;
						this.attributes[i] = vertexAttribute;
					}
				}
			}

			// Token: 0x04000953 RID: 2387
			public float4x4 toP;

			// Token: 0x04000954 RID: 2388
			public int vcnt;

			// Token: 0x04000955 RID: 2389
			public DataChunk mergeChunk;

			// Token: 0x04000956 RID: 2390
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000957 RID: 2391
			[WriteOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x04000958 RID: 2392
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x04000959 RID: 2393
			[ReadOnly]
			public NativeArray<VertexAttribute> proxyAttributes;

			// Token: 0x0400095A RID: 2394
			[ReadOnly]
			public NativeArray<float3> proxyLocalPositions;

			// Token: 0x0400095B RID: 2395
			[WriteOnly]
			public NativeArray<VirtualMesh.MappingWorkData> mappingWorkData;
		}

		// Token: 0x0200012F RID: 303
		[BurstCompile]
		private struct Mapping_CalcDirectWeightJob : IJob
		{
			// Token: 0x06000649 RID: 1609 RVA: 0x00036A1C File Offset: 0x00034C1C
			public void Execute()
			{
				FixedList4096Bytes<ushort> fixedList4096Bytes = default(FixedList4096Bytes<ushort>);
				for (int i = 0; i < this.vcnt; i++)
				{
					if (!this.attributes[i].IsInvalid())
					{
						VirtualMesh.MappingWorkData mappingWorkData = this.mappingWorkData[i];
						ushort num = (ushort)mappingWorkData.proxyVertexIndex;
						this.useSet.Clear();
						fixedList4096Bytes.Clear();
						ExCostSortedList4 exCostSortedList = new ExCostSortedList4(-1f);
						(ref fixedList4096Bytes).MC2Push(num);
						while (!fixedList4096Bytes.IsEmpty)
						{
							num = (ref fixedList4096Bytes).MC2Pop<ushort>();
							if (!this.useSet.Contains(num))
							{
								this.useSet.Add(num);
								float num2 = math.distance(mappingWorkData.position, this.proxyLocalPositions[(int)num]);
								if (num2 <= this.weightLength)
								{
									float num3 = Mathf.Clamp01(1f - num2 / this.weightLength);
									num3 = Mathf.Pow(num3, 3f);
									exCostSortedList.Add(1f - num3, (int)num);
									int num4;
									int num5;
									DataUtility.Unpack12_20(this.proxyVertexToVertexIndexArray[(int)num], out num4, out num5);
									int num6 = 0;
									while (num6 < num4 && !(ref fixedList4096Bytes).MC2IsCapacity<ushort>())
									{
										ushort num7 = this.proxyVertexToVertexDataArray[num5 + num6];
										if (!this.useSet.Contains(num7))
										{
											num2 = math.distance(mappingWorkData.position, this.proxyLocalPositions[(int)num7]);
											if (num2 <= this.weightLength)
											{
												(ref fixedList4096Bytes).MC2Push(num7);
											}
										}
										num6++;
									}
								}
							}
						}
						if (exCostSortedList.Count == 0)
						{
							exCostSortedList.Add(1f, (int)num);
						}
						else
						{
							int count = exCostSortedList.Count;
							for (int j = 0; j < 4; j++)
							{
								if (j < count)
								{
									exCostSortedList.costs[j] = 1f - exCostSortedList.costs[j];
								}
								else
								{
									exCostSortedList.costs[j] = 0f;
									exCostSortedList.data[j] = 0;
								}
							}
							float num8 = math.csum(exCostSortedList.costs);
							if (num8 == 0f)
							{
								float num9 = 1f / (float)count;
								for (int k = 0; k < count; k++)
								{
									exCostSortedList.costs[k] = num9;
								}
							}
							else
							{
								exCostSortedList.costs = math.saturate(exCostSortedList.costs / num8);
							}
						}
						this.boneWeights[i] = new VirtualMeshBoneWeight(exCostSortedList.data, exCostSortedList.costs);
					}
				}
			}

			// Token: 0x0400095C RID: 2396
			public int vcnt;

			// Token: 0x0400095D RID: 2397
			public float weightLength;

			// Token: 0x0400095E RID: 2398
			[ReadOnly]
			public NativeArray<VirtualMesh.MappingWorkData> mappingWorkData;

			// Token: 0x0400095F RID: 2399
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x04000960 RID: 2400
			[WriteOnly]
			public NativeArray<VirtualMeshBoneWeight> boneWeights;

			// Token: 0x04000961 RID: 2401
			[ReadOnly]
			public NativeArray<float3> proxyLocalPositions;

			// Token: 0x04000962 RID: 2402
			[ReadOnly]
			public NativeArray<uint> proxyVertexToVertexIndexArray;

			// Token: 0x04000963 RID: 2403
			[ReadOnly]
			public NativeArray<ushort> proxyVertexToVertexDataArray;

			// Token: 0x04000964 RID: 2404
			public NativeParallelHashSet<ushort> useSet;
		}

		// Token: 0x02000130 RID: 304
		[BurstCompile]
		private struct Mapping_CalcConnectionVertexDataJob : IJob
		{
			// Token: 0x0600064A RID: 1610 RVA: 0x00036CAC File Offset: 0x00034EAC
			public void Execute()
			{
				for (int i = 0; i < this.vcnt; i++)
				{
					float3 @float = this.localPositions[i];
					float3 float2 = MathUtility.TransformPoint(in @float, in this.toP);
					int num = this.transformIds[this.boneWeights[i].boneIndices[0]];
					ExCostSortedList1 exCostSortedList = new ExCostSortedList1(-1f);
					ExCostSortedList1 exCostSortedList2 = new ExCostSortedList1(-1f);
					foreach (int3 @int in GridMap<int>.GetArea(float2, this.searchRadius, this.gridMap, this.gridSize))
					{
						if (this.gridMap.ContainsKey(@int))
						{
							foreach (int num2 in this.gridMap.GetValuesForKey(@int))
							{
								float3 float3 = this.proxyLocalPositions[num2];
								float num3 = math.distance(float2, float3);
								if (num3 <= this.searchRadius)
								{
									VirtualMeshBoneWeight virtualMeshBoneWeight = this.proxyBoneWeights[num2];
									bool flag = false;
									int num4 = 0;
									while (num4 < virtualMeshBoneWeight.Count && !flag)
									{
										if (this.proxyTransformIds[virtualMeshBoneWeight.boneIndices[num4]] == num)
										{
											flag = true;
										}
										num4++;
									}
									if (flag)
									{
										exCostSortedList2.Add(num3, num2);
									}
									exCostSortedList.Add(num3, num2);
								}
							}
						}
					}
					ExCostSortedList1 exCostSortedList3 = exCostSortedList;
					if (exCostSortedList2.IsValid && exCostSortedList2.Cost < exCostSortedList.Cost * 3f)
					{
						exCostSortedList3 = exCostSortedList2;
					}
					if (!exCostSortedList3.IsValid)
					{
						this.attributes[i] = VertexAttribute.Invalid;
					}
					else
					{
						VertexAttribute vertexAttribute = this.proxyAttributes[exCostSortedList3.Data];
						if (vertexAttribute.IsInvalid())
						{
							this.attributes[i] = VertexAttribute.Invalid;
						}
						else
						{
							VirtualMesh.MappingWorkData mappingWorkData = new VirtualMesh.MappingWorkData
							{
								position = float2,
								vertexIndex = i,
								proxyVertexIndex = exCostSortedList3.Data,
								proxyVertexDistance = exCostSortedList3.Cost
							};
							this.mappingWorkData[i] = mappingWorkData;
							this.attributes[i] = vertexAttribute;
						}
					}
				}
			}

			// Token: 0x04000965 RID: 2405
			public float gridSize;

			// Token: 0x04000966 RID: 2406
			public float searchRadius;

			// Token: 0x04000967 RID: 2407
			public float4x4 toP;

			// Token: 0x04000968 RID: 2408
			public int vcnt;

			// Token: 0x04000969 RID: 2409
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x0400096A RID: 2410
			[ReadOnly]
			public NativeArray<VirtualMeshBoneWeight> boneWeights;

			// Token: 0x0400096B RID: 2411
			[ReadOnly]
			public NativeArray<int> transformIds;

			// Token: 0x0400096C RID: 2412
			[WriteOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x0400096D RID: 2413
			[ReadOnly]
			public NativeParallelMultiHashMap<int3, int> gridMap;

			// Token: 0x0400096E RID: 2414
			[ReadOnly]
			public NativeArray<VertexAttribute> proxyAttributes;

			// Token: 0x0400096F RID: 2415
			[ReadOnly]
			public NativeArray<float3> proxyLocalPositions;

			// Token: 0x04000970 RID: 2416
			[ReadOnly]
			public NativeArray<VirtualMeshBoneWeight> proxyBoneWeights;

			// Token: 0x04000971 RID: 2417
			[ReadOnly]
			public NativeArray<int> proxyTransformIds;

			// Token: 0x04000972 RID: 2418
			[WriteOnly]
			public NativeArray<VirtualMesh.MappingWorkData> mappingWorkData;
		}

		// Token: 0x02000131 RID: 305
		[BurstCompile]
		private struct Mapping_CalcWeightJob : IJobParallelFor
		{
			// Token: 0x0600064B RID: 1611 RVA: 0x00036F3C File Offset: 0x0003513C
			public void Execute(int vindex)
			{
				if (this.attributes[vindex].IsInvalid())
				{
					return;
				}
				VirtualMesh.MappingWorkData mappingWorkData = this.mappingWorkData[vindex];
				int num = mappingWorkData.proxyVertexIndex;
				float3 @float = mappingWorkData.position;
				float3 float2 = this.proxyLocalPositions[num];
				float3 float3 = this.proxyLocalNormals[num];
				float3 float4 = @float - float2;
				@float -= math.project(float4, float3);
				float num2 = math.distance(@float, float2);
				float num3 = num2 * 4f;
				ExCostSortedList4 exCostSortedList = new ExCostSortedList4(-1f);
				exCostSortedList.Add(num2, num);
				int num4;
				int num5;
				DataUtility.Unpack12_20(this.proxyVertexToVertexIndexArray[num], out num4, out num5);
				for (int i = 0; i < num4; i++)
				{
					int num6 = (int)this.proxyVertexToVertexDataArray[num5 + i];
					if (!exCostSortedList.Contains(num6))
					{
						float3 float5 = this.proxyLocalPositions[num6];
						float num7 = math.distance(@float, float5);
						if (num7 <= num3)
						{
							exCostSortedList.Add(num7, num6);
						}
						int num8;
						int num9;
						DataUtility.Unpack12_20(this.proxyVertexToVertexIndexArray[num6], out num8, out num9);
						for (int j = 0; j < num8; j++)
						{
							int num10 = (int)this.proxyVertexToVertexDataArray[num9 + j];
							if (num10 != num && num10 != num6 && !exCostSortedList.Contains(num10))
							{
								float5 = this.proxyLocalPositions[num10];
								num7 = math.distance(@float, float5);
								if (num7 <= num3)
								{
									exCostSortedList.Add(num7, num10);
								}
							}
						}
					}
				}
				float4 float6 = VirtualMesh.CalcVertexWeights(exCostSortedList.costs);
				VirtualMeshBoneWeight virtualMeshBoneWeight = new VirtualMeshBoneWeight(exCostSortedList.data, float6);
				this.boneWeights[vindex] = virtualMeshBoneWeight;
				float num11 = 0f;
				float num12 = 0f;
				int count = virtualMeshBoneWeight.Count;
				for (int k = 0; k < count; k++)
				{
					num = virtualMeshBoneWeight.boneIndices[k];
					VertexAttribute vertexAttribute = this.proxyAttributes[num];
					if (vertexAttribute.IsMove())
					{
						num12 += virtualMeshBoneWeight.weights[k];
					}
					else if (vertexAttribute.IsFixed())
					{
						num11 += virtualMeshBoneWeight.weights[k];
					}
				}
				this.attributes[vindex] = ((num12 > num11) ? VertexAttribute.Move : VertexAttribute.Fixed);
			}

			// Token: 0x04000973 RID: 2419
			[ReadOnly]
			public NativeArray<VirtualMesh.MappingWorkData> mappingWorkData;

			// Token: 0x04000974 RID: 2420
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x04000975 RID: 2421
			[WriteOnly]
			public NativeArray<VirtualMeshBoneWeight> boneWeights;

			// Token: 0x04000976 RID: 2422
			[ReadOnly]
			public NativeArray<VertexAttribute> proxyAttributes;

			// Token: 0x04000977 RID: 2423
			[ReadOnly]
			public NativeArray<float3> proxyLocalPositions;

			// Token: 0x04000978 RID: 2424
			[ReadOnly]
			public NativeArray<float3> proxyLocalNormals;

			// Token: 0x04000979 RID: 2425
			[ReadOnly]
			public NativeArray<uint> proxyVertexToVertexIndexArray;

			// Token: 0x0400097A RID: 2426
			[ReadOnly]
			public NativeArray<ushort> proxyVertexToVertexDataArray;
		}

		// Token: 0x02000132 RID: 306
		[BurstCompile]
		private struct Optimize_EdgeToTrianlgeJob : IJob
		{
			// Token: 0x0600064C RID: 1612 RVA: 0x00037190 File Offset: 0x00035390
			public unsafe void Execute()
			{
				for (int i = 0; i < this.tcnt; i++)
				{
					int3 @int = this.triangles[i];
					int2x3 int2x = new int2x3(@int.xy, @int.yz, @int.zx);
					for (int j = 0; j < 3; j++)
					{
						int2 int2 = DataUtility.PackInt2(int2x[j]);
						if (this.edgeToTriangleList.ContainsKey(int2))
						{
							FixedList128Bytes<int> fixedList128Bytes = this.edgeToTriangleList[int2];
							(ref fixedList128Bytes).MC2Set(i);
							this.edgeToTriangleList[int2] = fixedList128Bytes;
						}
						else
						{
							FixedList128Bytes<int> fixedList128Bytes2 = default(FixedList128Bytes<int>);
							(ref fixedList128Bytes2).MC2Set(i);
							this.edgeToTriangleList.Add(int2, fixedList128Bytes2);
						}
					}
				}
				foreach (KeyValue<int2, FixedList128Bytes<int>> keyValue in this.edgeToTriangleList)
				{
					int2 key = keyValue.Key;
					float3 @float = this.localPositions[key.x];
					float3 float2 = this.localPositions[key.y];
					FixedList128Bytes<int> fixedList128Bytes3 = *keyValue.Value;
					int length = fixedList128Bytes3.Length;
					for (int k = 0; k < length - 1; k++)
					{
						int3 int3 = this.triangles[fixedList128Bytes3[k]];
						int num = DataUtility.RemainingData(in int3, in key);
						float3 float3 = this.localPositions[num];
						for (int l = k + 1; l < length; l++)
						{
							int3 int4 = this.triangles[fixedList128Bytes3[l]];
							int num2 = DataUtility.RemainingData(in int4, in key);
							float3 float4 = this.localPositions[num2];
							if (math.abs(math.degrees(MathUtility.TriangleAngle(in @float, in float2, in float3, in float4))) <= 20f)
							{
								float num3;
								float num4;
								float3 float5;
								float3 float6;
								MathUtility.ClosestPtSegmentSegment(in @float, in float2, in float3, in float4, out num3, out num4, out float5, out float6);
								if (num3 != 0f && num3 != 1f && num4 != 0f && num4 != 1f)
								{
									int4 int5 = DataUtility.PackInt4(key.x, key.y, num, num2);
									if (this.useQuadSet.Contains(int5))
									{
										this.removeTriangleSet.Add(int3);
										this.removeTriangleSet.Add(int4);
									}
									else
									{
										this.useQuadSet.Add(int5);
									}
								}
							}
						}
					}
				}
				for (int m = 0; m < this.tcnt; m++)
				{
					int3 int6 = this.triangles[m];
					if (!this.removeTriangleSet.Contains(int6))
					{
						this.newTriangles.AddNoResize(int6);
					}
				}
			}

			// Token: 0x0400097B RID: 2427
			public int tcnt;

			// Token: 0x0400097C RID: 2428
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x0400097D RID: 2429
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x0400097E RID: 2430
			public NativeParallelHashMap<int2, FixedList128Bytes<int>> edgeToTriangleList;

			// Token: 0x0400097F RID: 2431
			[WriteOnly]
			public NativeList<int3> newTriangles;

			// Token: 0x04000980 RID: 2432
			public NativeParallelHashSet<int4> useQuadSet;

			// Token: 0x04000981 RID: 2433
			public NativeParallelHashSet<int3> removeTriangleSet;
		}

		// Token: 0x02000133 RID: 307
		[BurstCompile]
		private struct ProxyNormalRadiationAdjustmentJob : IJobParallelFor
		{
			// Token: 0x0600064D RID: 1613 RVA: 0x0003746C File Offset: 0x0003566C
			public void Execute(int vindex)
			{
				float3 @float = this.localPositions[vindex] - this.center;
				if (math.length(@float) < 1E-08f)
				{
					return;
				}
				float3 float2 = math.normalize(@float);
				float3 float3 = this.localNormals[vindex];
				float3 float4 = this.localTangents[vindex];
				quaternion quaternion = MathUtility.ToRotation(in float3, in float4);
				if (math.dot(float2, float4) < 0.99f)
				{
					float4 = math.normalize(math.cross(math.normalize(math.cross(float2, float4)), float2));
				}
				else
				{
					float4 = math.normalize(math.cross(math.normalize(math.cross(float3, float4)), float2));
				}
				this.localNormals[vindex] = float2;
				this.localTangents[vindex] = float4;
				quaternion quaternion2 = MathUtility.ToRotation(in float2, in float4);
				this.normalAdjustmentRotations[vindex] = math.mul(math.inverse(quaternion), quaternion2);
			}

			// Token: 0x04000982 RID: 2434
			public float3 center;

			// Token: 0x04000983 RID: 2435
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000984 RID: 2436
			public NativeArray<float3> localNormals;

			// Token: 0x04000985 RID: 2437
			public NativeArray<float3> localTangents;

			// Token: 0x04000986 RID: 2438
			[WriteOnly]
			public NativeArray<quaternion> normalAdjustmentRotations;
		}

		// Token: 0x02000134 RID: 308
		[BurstCompile]
		private struct ProxyCreateFixedListAndAABBJob : IJob
		{
			// Token: 0x0600064E RID: 1614 RVA: 0x0003754C File Offset: 0x0003574C
			public void Execute()
			{
				this.fixedList.Clear();
				float3 @float = 0;
				int num = 0;
				int num2 = 0;
				float3 float2 = float.MaxValue;
				float3 float3 = float.MinValue;
				int i = 0;
				while (i < this.vcnt)
				{
					float3 float4 = this.localPositions[i];
					if (this.attributes[i].IsMove())
					{
						goto IL_00D7;
					}
					int num3;
					int num4;
					DataUtility.Unpack12_20(this.vertexToVertexIndexArray[i], out num3, out num4);
					int j;
					for (j = 0; j < num3; j++)
					{
						int num5 = (int)this.vertexToVertexDataArray[num4 + j];
						if (this.attributes[num5].IsMove())
						{
							break;
						}
					}
					if (j != num3 || num3 <= 0)
					{
						ushort num6 = (ushort)i;
						this.fixedList.Add(in num6);
						@float += float4;
						num++;
						goto IL_00D7;
					}
					IL_00EF:
					i++;
					continue;
					IL_00D7:
					float2 = math.min(float2, float4);
					float3 = math.max(float3, float4);
					num2++;
					goto IL_00EF;
				}
				this.outAABB.Value = ((num2 > 0) ? new AABB(in float2, in float3) : default(AABB));
				if (num > 0)
				{
					@float /= (float)num;
				}
				this.localCenterPosition.Value = @float;
			}

			// Token: 0x04000987 RID: 2439
			public int vcnt;

			// Token: 0x04000988 RID: 2440
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x04000989 RID: 2441
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x0400098A RID: 2442
			[ReadOnly]
			public NativeArray<uint> vertexToVertexIndexArray;

			// Token: 0x0400098B RID: 2443
			[ReadOnly]
			public NativeArray<ushort> vertexToVertexDataArray;

			// Token: 0x0400098C RID: 2444
			[WriteOnly]
			public NativeReference<AABB> outAABB;

			// Token: 0x0400098D RID: 2445
			[WriteOnly]
			public NativeList<ushort> fixedList;

			// Token: 0x0400098E RID: 2446
			[WriteOnly]
			public NativeReference<float3> localCenterPosition;
		}

		// Token: 0x02000135 RID: 309
		[BurstCompile]
		private struct Proxy_CalcTriangleNormalJob : IJobParallelFor
		{
			// Token: 0x0600064F RID: 1615 RVA: 0x00037698 File Offset: 0x00035898
			public void Execute(int tindex)
			{
				int3 @int = this.triangles[tindex];
				float3 @float = this.localPositins[@int.x];
				float3 float2 = this.localPositins[@int.y];
				float3 float3 = this.localPositins[@int.z];
				float3 float4 = MathUtility.TriangleNormal(in @float, in float2, in float3);
				this.triangleNormals[tindex] = float4;
			}

			// Token: 0x0400098F RID: 2447
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x04000990 RID: 2448
			[ReadOnly]
			public NativeArray<float3> localPositins;

			// Token: 0x04000991 RID: 2449
			[WriteOnly]
			public NativeArray<float3> triangleNormals;
		}

		// Token: 0x02000136 RID: 310
		[BurstCompile]
		private struct Proxy_CalcTriangleTangentJob : IJobParallelFor
		{
			// Token: 0x06000650 RID: 1616 RVA: 0x00037704 File Offset: 0x00035904
			public void Execute(int tindex)
			{
				int3 @int = this.triangles[tindex];
				float3 @float = this.localPositins[@int.x];
				float3 float2 = this.localPositins[@int.y];
				float3 float3 = this.localPositins[@int.z];
				float2 float4 = this.uv[@int.x];
				float2 float5 = this.uv[@int.y];
				float2 float6 = this.uv[@int.z];
				float3 float7 = MathUtility.TriangleTangent(in @float, in float2, in float3, in float4, in float5, in float6);
				this.triangleTangents[tindex] = float7;
			}

			// Token: 0x04000992 RID: 2450
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x04000993 RID: 2451
			[ReadOnly]
			public NativeArray<float3> localPositins;

			// Token: 0x04000994 RID: 2452
			[ReadOnly]
			public NativeArray<float2> uv;

			// Token: 0x04000995 RID: 2453
			[WriteOnly]
			public NativeArray<float3> triangleTangents;
		}

		// Token: 0x02000137 RID: 311
		[BurstCompile]
		private struct Proxy_CreateVertexToTrianglesJob : IJob
		{
			// Token: 0x06000651 RID: 1617 RVA: 0x000377B0 File Offset: 0x000359B0
			public unsafe void Execute()
			{
				FixedList32Bytes<uint>* unsafePtr = (FixedList32Bytes<uint>*)this.vertexToTriangles.GetUnsafePtr<FixedList32Bytes<uint>>();
				int length = this.triangles.Length;
				uint num = 0U;
				while ((ulong)num < (ulong)((long)length))
				{
					int3 @int = this.triangles[(int)num];
					FixedList32Bytes<uint>* ptr = unsafePtr + (IntPtr)@int.x * (IntPtr)sizeof(FixedList32Bytes<uint>) / (IntPtr)sizeof(FixedList32Bytes<uint>);
					FixedList32Bytes<uint>* ptr2 = unsafePtr + (IntPtr)@int.y * (IntPtr)sizeof(FixedList32Bytes<uint>) / (IntPtr)sizeof(FixedList32Bytes<uint>);
					FixedList32Bytes<uint>* ptr3 = unsafePtr + (IntPtr)@int.z * (IntPtr)sizeof(FixedList32Bytes<uint>) / (IntPtr)sizeof(FixedList32Bytes<uint>);
					if (ptr->Length < 7)
					{
						(ref *ptr).MC2Set(num);
					}
					if (ptr2->Length < 7)
					{
						(ref *ptr2).MC2Set(num);
					}
					if (ptr3->Length < 7)
					{
						(ref *ptr3).MC2Set(num);
					}
					num += 1U;
				}
			}

			// Token: 0x04000996 RID: 2454
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x04000997 RID: 2455
			public NativeArray<FixedList32Bytes<uint>> vertexToTriangles;
		}

		// Token: 0x02000138 RID: 312
		[BurstCompile]
		private struct Proxy_OrganizeVertexToTrianglsJob : IJobParallelFor
		{
			// Token: 0x06000652 RID: 1618 RVA: 0x00037860 File Offset: 0x00035A60
			public void Execute(int vindex)
			{
				FixedList32Bytes<uint> fixedList32Bytes = this.vertexToTriangles[vindex];
				int length = fixedList32Bytes.Length;
				if (length == 0)
				{
					return;
				}
				VertexAttribute vertexAttribute = this.attributes[vindex];
				vertexAttribute.SetFlag(128, true);
				this.attributes[vindex] = vertexAttribute;
				float3 @float = 0;
				float3 float2 = 0;
				for (int i = 0; i < length; i++)
				{
					int num = (int)fixedList32Bytes[i];
					@float += this.triangleNormals[num];
					float2 += this.triangleTangents[num];
				}
				if (math.length(@float) < 0.5f)
				{
					float num2 = -1f;
					@float = 0;
					for (int j = 0; j < length; j++)
					{
						int num3 = (int)fixedList32Bytes[j];
						float3 float3 = 0;
						float3 float4 = this.triangleNormals[num3];
						for (int k = 0; k < length; k++)
						{
							int num4 = (int)fixedList32Bytes[k];
							if (num4 != num3)
							{
								float3 float5 = this.triangleNormals[num4];
								if (math.dot(float4, float5) >= 0f)
								{
									float3 += float5;
								}
								else
								{
									float3 += -float5;
								}
							}
						}
						float num5 = math.lengthsq(float3);
						if (num5 > num2)
						{
							num2 = num5;
							@float = float4;
						}
					}
				}
				else
				{
					@float = math.normalize(@float);
				}
				if (math.length(float2) < 0.5f)
				{
					float num6 = -1f;
					float2 = 0;
					for (int l = 0; l < length; l++)
					{
						int num7 = (int)fixedList32Bytes[l];
						float3 float6 = 0;
						float3 float7 = this.triangleTangents[num7];
						for (int m = 0; m < length; m++)
						{
							int num8 = (int)fixedList32Bytes[m];
							if (num8 != num7)
							{
								float3 float8 = this.triangleTangents[num8];
								if (math.dot(float7, float8) >= 0f)
								{
									float6 += float8;
								}
								else
								{
									float6 += -float8;
								}
							}
						}
						float num9 = math.lengthsq(float6);
						if (num9 > num6)
						{
							num6 = num9;
							float2 = float7;
						}
					}
				}
				else
				{
					float2 = math.normalize(float2);
				}
				for (int n = 0; n < length; n++)
				{
					int num10 = (int)fixedList32Bytes[n];
					float3 float9 = this.triangleNormals[num10];
					float3 float10 = this.triangleTangents[num10];
					int num11 = 0;
					if (math.dot(@float, float9) < 0f)
					{
						num11 |= 1;
					}
					if (math.dot(float2, float10) < 0f)
					{
						num11 |= 2;
					}
					fixedList32Bytes[n] = DataUtility.Pack12_20(num11, num10);
				}
				this.vertexToTriangles[vindex] = fixedList32Bytes;
			}

			// Token: 0x04000998 RID: 2456
			public NativeArray<FixedList32Bytes<uint>> vertexToTriangles;

			// Token: 0x04000999 RID: 2457
			[ReadOnly]
			public NativeArray<float3> triangleNormals;

			// Token: 0x0400099A RID: 2458
			[ReadOnly]
			public NativeArray<float3> triangleTangents;

			// Token: 0x0400099B RID: 2459
			public NativeArray<VertexAttribute> attributes;
		}

		// Token: 0x02000139 RID: 313
		[BurstCompile]
		private struct Proxy_CalcVertexNormalTangentFromTriangleJob : IJobParallelFor
		{
			// Token: 0x06000653 RID: 1619 RVA: 0x00037B30 File Offset: 0x00035D30
			public void Execute(int vindex)
			{
				FixedList32Bytes<uint> fixedList32Bytes = this.vertexToTriangles[vindex];
				int length = fixedList32Bytes.Length;
				if (length > 0)
				{
					float3 @float = 0;
					float3 float2 = 0;
					for (int i = 0; i < length; i++)
					{
						uint num = fixedList32Bytes[i];
						int num2 = DataUtility.Unpack12_20Hi(num);
						int num3 = DataUtility.Unpack12_20Low(num);
						@float += this.triangleNormals[num3] * (float)(((num2 & 1) == 0) ? 1 : (-1));
						float2 += this.triangleTangents[num3] * (float)(((num2 & 2) == 0) ? 1 : (-1));
					}
					@float = math.normalize(@float);
					float3 float3 = math.normalize(math.cross(@float, float2));
					this.localNormals[vindex] = @float;
					this.localTangents[vindex] = float3;
				}
			}

			// Token: 0x0400099C RID: 2460
			[ReadOnly]
			public NativeArray<float3> triangleNormals;

			// Token: 0x0400099D RID: 2461
			[ReadOnly]
			public NativeArray<float3> triangleTangents;

			// Token: 0x0400099E RID: 2462
			[ReadOnly]
			public NativeArray<FixedList32Bytes<uint>> vertexToTriangles;

			// Token: 0x0400099F RID: 2463
			public NativeArray<float3> localNormals;

			// Token: 0x040009A0 RID: 2464
			public NativeArray<float3> localTangents;
		}

		// Token: 0x0200013A RID: 314
		[BurstCompile]
		private struct Proxy_CalcVertexToTransformJob : IJobParallelFor
		{
			// Token: 0x06000654 RID: 1620 RVA: 0x00037C08 File Offset: 0x00035E08
			public void Execute(int vindex)
			{
				quaternion quaternion = math.mul(this.invRot, this.transformRotations[vindex]);
				float3 @float = this.localNormals[vindex];
				float3 float2 = this.localTangents[vindex];
				quaternion quaternion2 = math.mul(math.inverse(MathUtility.ToRotation(in @float, in float2)), quaternion);
				this.vertexToTransformRotations[vindex] = quaternion2;
			}

			// Token: 0x040009A1 RID: 2465
			public quaternion invRot;

			// Token: 0x040009A2 RID: 2466
			[ReadOnly]
			public NativeArray<float3> localNormals;

			// Token: 0x040009A3 RID: 2467
			[ReadOnly]
			public NativeArray<float3> localTangents;

			// Token: 0x040009A4 RID: 2468
			[WriteOnly]
			public NativeArray<quaternion> vertexToTransformRotations;

			// Token: 0x040009A5 RID: 2469
			[ReadOnly]
			public NativeArray<quaternion> transformRotations;
		}

		// Token: 0x0200013B RID: 315
		[BurstCompile]
		private struct Proxy_CalcEdgeToTriangleJob : IJob
		{
			// Token: 0x06000655 RID: 1621 RVA: 0x00037C6C File Offset: 0x00035E6C
			public unsafe void Execute()
			{
				for (int i = 0; i < this.tcnt; i++)
				{
					int3 @int = this.triangles[i];
					int2 xy = @int.xy;
					int2 int2 = DataUtility.PackInt2(in xy);
					int2 yz = @int.yz;
					int2 int3 = DataUtility.PackInt2(in yz);
					int2 zx = @int.zx;
					int2x3 int2x = new int2x3(int2, int3, DataUtility.PackInt2(in zx));
					for (int j = 0; j < 3; j++)
					{
						int2 int4 = *int2x[j];
						(ref this.edgeToTriangles).MC2UniqueAdd(int4, (ushort)i);
					}
				}
			}

			// Token: 0x040009A6 RID: 2470
			public int tcnt;

			// Token: 0x040009A7 RID: 2471
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x040009A8 RID: 2472
			public NativeParallelMultiHashMap<int2, ushort> edgeToTriangles;
		}

		// Token: 0x0200013C RID: 316
		[BurstCompile]
		private struct Proxy_CalcVertexBindPoseJob2 : IJobParallelFor
		{
			// Token: 0x06000656 RID: 1622 RVA: 0x00037CFC File Offset: 0x00035EFC
			public void Execute(int vindex)
			{
				float3 @float = this.localPositions[vindex];
				float3 float2 = this.localNormals[vindex];
				float3 float3 = this.localTangents[vindex];
				quaternion quaternion = MathUtility.ToRotation(in float2, in float3);
				this.vertexBindPosePositions[vindex] = -@float;
				this.vertexBindPoseRotations[vindex] = math.inverse(quaternion);
			}

			// Token: 0x040009A9 RID: 2473
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x040009AA RID: 2474
			[ReadOnly]
			public NativeArray<float3> localNormals;

			// Token: 0x040009AB RID: 2475
			[ReadOnly]
			public NativeArray<float3> localTangents;

			// Token: 0x040009AC RID: 2476
			[WriteOnly]
			public NativeArray<float3> vertexBindPosePositions;

			// Token: 0x040009AD RID: 2477
			[WriteOnly]
			public NativeArray<quaternion> vertexBindPoseRotations;
		}

		// Token: 0x0200013D RID: 317
		[BurstCompile]
		private struct Proxy_CalcVertexToVertexFromTriangleJob : IJob
		{
			// Token: 0x06000657 RID: 1623 RVA: 0x00037D60 File Offset: 0x00035F60
			public void Execute()
			{
				for (int i = 0; i < this.triangleCount; i++)
				{
					int3 @int = this.triangles[i];
					ushort num = (ushort)@int.x;
					ushort num2 = (ushort)@int.y;
					ushort num3 = (ushort)@int.z;
					(ref this.vertexToVertexMap).MC2UniqueAdd(@int.x, num2);
					(ref this.vertexToVertexMap).MC2UniqueAdd(@int.x, num3);
					(ref this.vertexToVertexMap).MC2UniqueAdd(@int.y, num);
					(ref this.vertexToVertexMap).MC2UniqueAdd(@int.y, num3);
					(ref this.vertexToVertexMap).MC2UniqueAdd(@int.z, num);
					(ref this.vertexToVertexMap).MC2UniqueAdd(@int.z, num2);
					int2 int2 = @int.xy;
					this.edgeSet.Add(DataUtility.PackInt2(in int2));
					int2 = @int.yz;
					this.edgeSet.Add(DataUtility.PackInt2(in int2));
					int2 = @int.zx;
					this.edgeSet.Add(DataUtility.PackInt2(in int2));
				}
			}

			// Token: 0x040009AE RID: 2478
			public int triangleCount;

			// Token: 0x040009AF RID: 2479
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x040009B0 RID: 2480
			public NativeParallelMultiHashMap<int, ushort> vertexToVertexMap;

			// Token: 0x040009B1 RID: 2481
			public NativeParallelHashSet<int2> edgeSet;
		}

		// Token: 0x0200013E RID: 318
		[BurstCompile]
		private struct Proxy_CalcVertexToVertexFromLineJob : IJob
		{
			// Token: 0x06000658 RID: 1624 RVA: 0x00037E6C File Offset: 0x0003606C
			public void Execute()
			{
				for (int i = 0; i < this.lineCount; i++)
				{
					int2 @int = this.lines[i];
					(ref this.vertexToVertexMap).MC2UniqueAdd(@int.x, (ushort)@int.y);
					(ref this.vertexToVertexMap).MC2UniqueAdd(@int.y, (ushort)@int.x);
					this.edgeSet.Add(DataUtility.PackInt2(in @int));
				}
			}

			// Token: 0x040009B2 RID: 2482
			public int lineCount;

			// Token: 0x040009B3 RID: 2483
			[ReadOnly]
			public NativeArray<int2> lines;

			// Token: 0x040009B4 RID: 2484
			public NativeParallelMultiHashMap<int, ushort> vertexToVertexMap;

			// Token: 0x040009B5 RID: 2485
			public NativeParallelHashSet<int2> edgeSet;
		}

		// Token: 0x0200013F RID: 319
		[BurstCompile]
		private struct Proxy_CreateEdgeFlagJob : IJobParallelFor
		{
			// Token: 0x06000659 RID: 1625 RVA: 0x00037EDC File Offset: 0x000360DC
			public void Execute(int eindex)
			{
				ExBitFlag8 exBitFlag = default(ExBitFlag8);
				int2 @int = this.edges[eindex];
				if (this.edgeToTriangles.ContainsKey(@int) && this.edgeToTriangles.CountValuesForKey(@int) <= 1)
				{
					exBitFlag.SetFlag(1, true);
				}
				this.edgeFlags[eindex] = exBitFlag;
			}

			// Token: 0x040009B6 RID: 2486
			[ReadOnly]
			public NativeArray<int2> edges;

			// Token: 0x040009B7 RID: 2487
			[ReadOnly]
			public NativeParallelMultiHashMap<int2, ushort> edgeToTriangles;

			// Token: 0x040009B8 RID: 2488
			[WriteOnly]
			public NativeArray<ExBitFlag8> edgeFlags;
		}

		// Token: 0x02000140 RID: 320
		private struct SkinningBoneInfo
		{
			// Token: 0x040009B9 RID: 2489
			public int parentTransformIndex;

			// Token: 0x040009BA RID: 2490
			public float3 parentPos;

			// Token: 0x040009BB RID: 2491
			public int childTransformIndex;

			// Token: 0x040009BC RID: 2492
			public float3 childPos;
		}

		// Token: 0x02000141 RID: 321
		[BurstCompile]
		private struct Proxy_CalcCustomSkinningWeightsJobV2 : IJobParallelFor
		{
			// Token: 0x0600065A RID: 1626 RVA: 0x00037F34 File Offset: 0x00036134
			public void Execute(int vindex)
			{
				if (this.attributes[vindex].IsDontMove())
				{
					return;
				}
				float3 @float = this.localPositions[vindex];
				ExCostSortedList4 exCostSortedList = new ExCostSortedList4(-1f);
				int length = this.boneInfoList.Length;
				for (int i = 0; i < length; i++)
				{
					VirtualMesh.SkinningBoneInfo skinningBoneInfo = this.boneInfoList[i];
					float3 childPos = skinningBoneInfo.childPos;
					int childTransformIndex = skinningBoneInfo.childTransformIndex;
					float num = math.length(@float - childPos);
					int num2 = exCostSortedList.indexOf(childTransformIndex);
					if (num2 >= 0)
					{
						if (num < exCostSortedList.costs[num2])
						{
							exCostSortedList.RemoveItem(childTransformIndex);
							exCostSortedList.Add(num, childTransformIndex);
						}
					}
					else
					{
						exCostSortedList.Add(num, childTransformIndex);
					}
				}
				int num3 = exCostSortedList.Count;
				float num4 = exCostSortedList.MinCost * this.distanceReduction;
				for (int j = 0; j < num3; j++)
				{
					exCostSortedList.costs[j] = exCostSortedList.costs[j] - num4;
				}
				for (int k = 0; k < num3; k++)
				{
					exCostSortedList.costs[k] = math.pow(exCostSortedList.costs[k], this.distancePow);
				}
				if (exCostSortedList.MinCost < 1E-08f)
				{
					exCostSortedList.costs = new float4(1f, 0f, 0f, 0f);
					exCostSortedList.data = new int4(exCostSortedList.data[0], 0, 0, 0);
				}
				else
				{
					num3 = exCostSortedList.Count;
					float num5 = 0f;
					for (int l = 0; l < num3; l++)
					{
						num5 += 1f / exCostSortedList.costs[l];
					}
					for (int m = 0; m < num3; m++)
					{
						exCostSortedList.costs[m] = 1f / exCostSortedList.costs[m] / num5;
					}
					num5 = 0f;
					for (int n = 0; n < 4; n++)
					{
						if (exCostSortedList.costs[n] < 0.001f || n >= num3)
						{
							exCostSortedList.costs[n] = 0f;
							exCostSortedList.data[n] = 0;
						}
						else
						{
							num5 += exCostSortedList.costs[n];
						}
					}
					exCostSortedList.costs /= num5;
				}
				VirtualMeshBoneWeight virtualMeshBoneWeight = new VirtualMeshBoneWeight(exCostSortedList.data, exCostSortedList.costs);
				this.boneWeights[vindex] = virtualMeshBoneWeight;
			}

			// Token: 0x040009BD RID: 2493
			public bool isBoneCloth;

			// Token: 0x040009BE RID: 2494
			public float angularAttenuation;

			// Token: 0x040009BF RID: 2495
			public float distanceReduction;

			// Token: 0x040009C0 RID: 2496
			public float distancePow;

			// Token: 0x040009C1 RID: 2497
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x040009C2 RID: 2498
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x040009C3 RID: 2499
			[ReadOnly]
			public NativeList<VirtualMesh.SkinningBoneInfo> boneInfoList;

			// Token: 0x040009C4 RID: 2500
			[WriteOnly]
			public NativeArray<VirtualMeshBoneWeight> boneWeights;
		}

		// Token: 0x02000142 RID: 322
		[BurstCompile]
		private struct Proxy_ApplySelectionJob : IJobParallelFor
		{
			// Token: 0x0600065B RID: 1627 RVA: 0x000381DC File Offset: 0x000363DC
			public void Execute(int vindex)
			{
				float3 @float = this.localPositions[vindex];
				VertexAttribute vertexAttribute = this.attributes[vindex];
				float num = float.MaxValue;
				VertexAttribute vertexAttribute2 = VertexAttribute.Invalid;
				foreach (int3 @int in GridMap<int>.GetArea(@float, this.radius, this.gridMap, this.gridSize))
				{
					if (this.gridMap.ContainsKey(@int))
					{
						foreach (int num2 in this.gridMap.GetValuesForKey(@int))
						{
							float3 float2 = this.selectionPositions[num2];
							float num3 = math.distance(@float, float2);
							if (num3 <= this.radius && num3 <= num)
							{
								num = num3;
								vertexAttribute2 = this.selectionAttributes[num2];
							}
						}
					}
				}
				vertexAttribute.SetFlag(vertexAttribute2, true);
				this.attributes[vindex] = vertexAttribute;
			}

			// Token: 0x040009C5 RID: 2501
			public float gridSize;

			// Token: 0x040009C6 RID: 2502
			public float radius;

			// Token: 0x040009C7 RID: 2503
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x040009C8 RID: 2504
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x040009C9 RID: 2505
			[ReadOnly]
			public NativeParallelMultiHashMap<int3, int> gridMap;

			// Token: 0x040009CA RID: 2506
			[ReadOnly]
			public NativeArray<float3> selectionPositions;

			// Token: 0x040009CB RID: 2507
			[ReadOnly]
			public NativeArray<VertexAttribute> selectionAttributes;
		}

		// Token: 0x02000143 RID: 323
		[BurstCompile]
		private struct Proxy_BoneClothApplayTransformFlagJob : IJobParallelFor
		{
			// Token: 0x0600065C RID: 1628 RVA: 0x00038314 File Offset: 0x00036514
			public void Execute(int vindex)
			{
				VertexAttribute vertexAttribute = this.attributes[vindex];
				ExBitFlag8 exBitFlag = this.transformFlags[vindex];
				if (vertexAttribute.IsMove())
				{
					exBitFlag.SetFlag(4, true);
				}
				else if (vertexAttribute.IsFixed())
				{
					exBitFlag.SetFlag(2, true);
				}
				if (!vertexAttribute.IsInvalid())
				{
					exBitFlag.SetFlag(8, true);
				}
				this.transformFlags[vindex] = exBitFlag;
			}

			// Token: 0x040009CC RID: 2508
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x040009CD RID: 2509
			public NativeArray<ExBitFlag8> transformFlags;
		}

		// Token: 0x02000144 RID: 324
		private struct BaseLineWork : IComparable<VirtualMesh.BaseLineWork>
		{
			// Token: 0x0600065D RID: 1629 RVA: 0x00038380 File Offset: 0x00036580
			public int CompareTo(VirtualMesh.BaseLineWork other)
			{
				return (int)math.sign(this.dist - other.dist);
			}

			// Token: 0x040009CE RID: 2510
			public int vindex;

			// Token: 0x040009CF RID: 2511
			public float dist;
		}

		// Token: 0x02000145 RID: 325
		[BurstCompile]
		private struct BaseLine_Mesh_CreateParentJob2 : IJob
		{
			// Token: 0x0600065E RID: 1630 RVA: 0x00038398 File Offset: 0x00036598
			public void Execute()
			{
				foreach (int num in this.fixedList)
				{
					VirtualMesh.BaseLineWork baseLineWork = default(VirtualMesh.BaseLineWork);
					baseLineWork.vindex = num;
					baseLineWork.dist = 0f;
					this.nextList.Add(in baseLineWork);
				}
				int num2 = 0;
				while (this.nextList.Length > 0)
				{
					foreach (VirtualMesh.BaseLineWork baseLineWork2 in this.nextList)
					{
						int vindex = baseLineWork2.vindex;
						if (!this.attribues[vindex].IsDontMove())
						{
							float3 @float = this.localPositions[vindex];
							ExCostSortedList1 exCostSortedList = new ExCostSortedList1(-1f, -1);
							int num3;
							int num4;
							DataUtility.Unpack12_20(this.vertexToVertexIndexArray[vindex], out num3, out num4);
							for (int i = 0; i < num3; i++)
							{
								int num5 = (int)this.vertexToVertexDataArray[num4 + i];
								if (this.markBuff[num5] != 0)
								{
									float3 float2 = this.localPositions[num5];
									if (this.attribues[num5].IsDontMove())
									{
										float num6 = math.distance(@float, float2);
										exCostSortedList.Add(num6, num5);
									}
									else
									{
										int num7 = this.vertexParentIndices[num5];
										float3 float3 = float2 - @float;
										float3 float4 = this.localPositions[num7] - float2;
										float num8 = MathUtility.Angle(in float3, in float4);
										exCostSortedList.Add(num8, num5);
									}
								}
							}
							if (exCostSortedList.IsValid)
							{
								int data = exCostSortedList.data;
								this.vertexParentIndices[vindex] = data;
								this.markBuff[vindex] = 1;
							}
						}
					}
					foreach (VirtualMesh.BaseLineWork baseLineWork3 in this.nextList)
					{
						int vindex2 = baseLineWork3.vindex;
						this.markBuff[vindex2] = 2;
						int num9 = this.vertexParentIndices[vindex2];
						if (num9 >= 0)
						{
							(ref this.vertexChildMap).MC2UniqueAdd(num9, (ushort)vindex2);
						}
					}
					this.vertexMap.Clear();
					int num10 = 0;
					foreach (VirtualMesh.BaseLineWork baseLineWork4 in this.nextList)
					{
						int vindex3 = baseLineWork4.vindex;
						int num11;
						int num12;
						DataUtility.Unpack12_20(this.vertexToVertexIndexArray[vindex3], out num11, out num12);
						if (num11 != 0)
						{
							float3 float5 = this.localPositions[vindex3];
							for (int j = 0; j < num11; j++)
							{
								int num13 = (int)this.vertexToVertexDataArray[num12 + j];
								if (!this.attribues[num13].IsInvalid() && this.markBuff[num13] == 0)
								{
									float num14 = math.distance(float5, this.localPositions[num13]);
									if (this.vertexMap.ContainsKey(num13))
									{
										VirtualMesh.BaseLineWork baseLineWork5 = this.vertexMap[num13];
										if (num14 < baseLineWork5.dist)
										{
											baseLineWork5.dist = num14;
											this.vertexMap[num13] = baseLineWork5;
										}
									}
									else
									{
										int num15 = num13;
										VirtualMesh.BaseLineWork baseLineWork = new VirtualMesh.BaseLineWork
										{
											vindex = num13,
											dist = num14
										};
										this.vertexMap.Add(num15, baseLineWork);
										num10++;
									}
								}
							}
						}
					}
					this.nextList.Clear();
					if (num10 > 0)
					{
						foreach (KeyValue<int, VirtualMesh.BaseLineWork> keyValue in this.vertexMap)
						{
							this.nextList.Add(keyValue.Value);
						}
						this.nextList.Sort<VirtualMesh.BaseLineWork>();
					}
					num2++;
				}
			}

			// Token: 0x040009D0 RID: 2512
			public int vcnt;

			// Token: 0x040009D1 RID: 2513
			public float avgDist;

			// Token: 0x040009D2 RID: 2514
			[ReadOnly]
			public NativeArray<VertexAttribute> attribues;

			// Token: 0x040009D3 RID: 2515
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x040009D4 RID: 2516
			[ReadOnly]
			public NativeArray<uint> vertexToVertexIndexArray;

			// Token: 0x040009D5 RID: 2517
			[ReadOnly]
			public NativeArray<ushort> vertexToVertexDataArray;

			// Token: 0x040009D6 RID: 2518
			public NativeArray<int> vertexParentIndices;

			// Token: 0x040009D7 RID: 2519
			public NativeParallelMultiHashMap<int, ushort> vertexChildMap;

			// Token: 0x040009D8 RID: 2520
			[ReadOnly]
			public NativeList<int> fixedList;

			// Token: 0x040009D9 RID: 2521
			public NativeList<VirtualMesh.BaseLineWork> nextList;

			// Token: 0x040009DA RID: 2522
			public NativeArray<byte> markBuff;

			// Token: 0x040009DB RID: 2523
			public NativeParallelHashMap<int, VirtualMesh.BaseLineWork> vertexMap;
		}

		// Token: 0x02000146 RID: 326
		[BurstCompile]
		private struct BaseLine_Mesh_CareteFixedListJob : IJob
		{
			// Token: 0x0600065F RID: 1631 RVA: 0x00038828 File Offset: 0x00036A28
			public void Execute()
			{
				for (int i = 0; i < this.vcnt; i++)
				{
					if (this.attribues[i].IsFixed())
					{
						this.fixedList.Add(in i);
					}
				}
			}

			// Token: 0x040009DC RID: 2524
			public int vcnt;

			// Token: 0x040009DD RID: 2525
			[ReadOnly]
			public NativeArray<VertexAttribute> attribues;

			// Token: 0x040009DE RID: 2526
			public NativeList<int> fixedList;
		}

		// Token: 0x02000147 RID: 327
		[BurstCompile]
		private struct BaseLine_Bone_CreateBoneChildInfoJob : IJob
		{
			// Token: 0x06000660 RID: 1632 RVA: 0x0003886C File Offset: 0x00036A6C
			public void Execute()
			{
				for (int i = 0; i < this.vcnt; i++)
				{
					int num = this.parentIndices[i];
					if (num >= 0)
					{
						this.childMap.Add(num, (ushort)i);
					}
				}
			}

			// Token: 0x040009DF RID: 2527
			public int vcnt;

			// Token: 0x040009E0 RID: 2528
			[ReadOnly]
			public NativeArray<int> parentIndices;

			// Token: 0x040009E1 RID: 2529
			public NativeParallelMultiHashMap<int, ushort> childMap;
		}

		// Token: 0x02000148 RID: 328
		[BurstCompile]
		private struct BaseLine_CalcLocalPositionRotationJob : IJobParallelFor
		{
			// Token: 0x06000661 RID: 1633 RVA: 0x000388AC File Offset: 0x00036AAC
			public void Execute(int index)
			{
				int num = (int)this.baseLineIndices[index];
				int num2 = this.parentIndices[num];
				if (num2 >= 0)
				{
					float3 @float = this.localPositions[num2];
					float3 float2 = this.localNormals[num2];
					float3 float3 = this.localTangents[num2];
					quaternion quaternion = math.inverse(MathUtility.ToRotation(in float2, in float3));
					float3 float4 = this.localPositions[num];
					float3 float5 = this.localNormals[num];
					float3 float6 = this.localTangents[num];
					quaternion quaternion2 = MathUtility.ToRotation(in float5, in float6);
					float3 float7 = math.mul(quaternion, float4 - @float);
					quaternion quaternion3 = math.mul(quaternion, quaternion2);
					this.vertexLocalPositions[num] = float7;
					this.vertexLocalRotations[num] = quaternion3;
					return;
				}
				this.vertexLocalPositions[num] = 0;
				this.vertexLocalRotations[num] = quaternion.identity;
			}

			// Token: 0x040009E2 RID: 2530
			[ReadOnly]
			public NativeArray<int> parentIndices;

			// Token: 0x040009E3 RID: 2531
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x040009E4 RID: 2532
			[ReadOnly]
			public NativeArray<float3> localNormals;

			// Token: 0x040009E5 RID: 2533
			[ReadOnly]
			public NativeArray<float3> localTangents;

			// Token: 0x040009E6 RID: 2534
			[ReadOnly]
			public NativeArray<ushort> baseLineIndices;

			// Token: 0x040009E7 RID: 2535
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> vertexLocalPositions;

			// Token: 0x040009E8 RID: 2536
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<quaternion> vertexLocalRotations;
		}

		// Token: 0x02000149 RID: 329
		[BurstCompile]
		private struct BaseLine_CalcMaxBaseLineLengthJob : IJob
		{
			// Token: 0x06000662 RID: 1634 RVA: 0x000389A0 File Offset: 0x00036BA0
			public void Execute()
			{
				float num = 0f;
				for (int i = 0; i < this.vcnt; i++)
				{
					int num2 = -1;
					float num3 = 0f;
					if (this.attribues[i].IsMove())
					{
						int num4 = i;
						for (int j = this.vertexParentIndices[num4]; j >= 0; j = this.vertexParentIndices[num4])
						{
							float3 @float = this.localPositions[num4];
							float3 float2 = this.localPositions[j];
							float num5 = math.distance(@float, float2);
							num3 += num5;
							num2 = j;
							if (!this.attribues[j].IsMove())
							{
								break;
							}
							num4 = j;
						}
					}
					this.vertexRootIndices[i] = num2;
					this.rootLengthArray[i] = num3;
					num = math.max(num, num3);
				}
				if (num > 1E-08f)
				{
					for (int k = 0; k < this.vcnt; k++)
					{
						float num6 = math.saturate(this.rootLengthArray[k] / num);
						this.vertexDepths[k] = num6;
					}
				}
			}

			// Token: 0x040009E9 RID: 2537
			public int vcnt;

			// Token: 0x040009EA RID: 2538
			[ReadOnly]
			public NativeArray<VertexAttribute> attribues;

			// Token: 0x040009EB RID: 2539
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x040009EC RID: 2540
			[ReadOnly]
			public NativeArray<int> vertexParentIndices;

			// Token: 0x040009ED RID: 2541
			[WriteOnly]
			public NativeArray<float> vertexDepths;

			// Token: 0x040009EE RID: 2542
			[WriteOnly]
			public NativeArray<int> vertexRootIndices;

			// Token: 0x040009EF RID: 2543
			public NativeArray<float> rootLengthArray;
		}

		// Token: 0x0200014A RID: 330
		[BurstCompile]
		private struct Reduction_InitVertexToVertexJob2 : IJob
		{
			// Token: 0x06000663 RID: 1635 RVA: 0x00038AC0 File Offset: 0x00036CC0
			public void Execute()
			{
				for (int i = 0; i < this.triangleCount; i++)
				{
					int3 @int = this.triangles[i];
					ushort num = (ushort)@int.x;
					ushort num2 = (ushort)@int.y;
					ushort num3 = (ushort)@int.z;
					this.vertexToVertexMap.Add(num, num2);
					this.vertexToVertexMap.Add(num, num3);
					this.vertexToVertexMap.Add(num2, num);
					this.vertexToVertexMap.Add(num2, num3);
					this.vertexToVertexMap.Add(num3, num);
					this.vertexToVertexMap.Add(num3, num2);
				}
			}

			// Token: 0x040009F0 RID: 2544
			public int triangleCount;

			// Token: 0x040009F1 RID: 2545
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x040009F2 RID: 2546
			public NativeParallelMultiHashMap<ushort, ushort> vertexToVertexMap;
		}

		// Token: 0x0200014B RID: 331
		[BurstCompile]
		private struct Organize_RemapVertexJob : IJob
		{
			// Token: 0x06000664 RID: 1636 RVA: 0x00038B50 File Offset: 0x00036D50
			public void Execute()
			{
				int num = 0;
				for (int i = 0; i < this.oldVertexCount; i++)
				{
					if (this.joinIndices[i] < 0)
					{
						this.vertexRemapIndices[i] = num;
						num++;
					}
				}
				for (int j = 0; j < this.oldVertexCount; j++)
				{
					int num2 = this.joinIndices[j];
					if (num2 >= 0)
					{
						this.vertexRemapIndices[j] = this.vertexRemapIndices[num2];
					}
				}
			}

			// Token: 0x040009F3 RID: 2547
			public int oldVertexCount;

			// Token: 0x040009F4 RID: 2548
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x040009F5 RID: 2549
			public NativeArray<int> vertexRemapIndices;
		}

		// Token: 0x0200014C RID: 332
		[BurstCompile]
		private struct Organize_CollectUseSkinBoneJob : IJob
		{
			// Token: 0x06000665 RID: 1637 RVA: 0x00038BCC File Offset: 0x00036DCC
			public void Execute()
			{
				for (int i = 0; i < this.oldVertexCount; i++)
				{
					if (this.joinIndices[i] < 0)
					{
						VirtualMeshBoneWeight virtualMeshBoneWeight = this.oldBoneWeights[i];
						for (int j = 0; j < 4; j++)
						{
							if (virtualMeshBoneWeight.weights[j] > 0f)
							{
								int num = virtualMeshBoneWeight.boneIndices[j];
								this.useSkinBoneMap.TryAdd(virtualMeshBoneWeight.boneIndices[j], 0);
							}
						}
					}
				}
				this.useSkinBoneMapKeyList.Clear();
				foreach (KeyValue<int, int> keyValue in this.useSkinBoneMap)
				{
					int key = keyValue.Key;
					this.useSkinBoneMapKeyList.Add(in key);
				}
				for (int k = 0; k < this.useSkinBoneMapKeyList.Length; k++)
				{
					int num2 = this.useSkinBoneMapKeyList[k];
					this.useSkinBoneMap[num2] = k;
					this.newSkinBoneTransformIndices.Add(in k);
					float4x4 float4x = this.oldBindPoses[num2];
					this.newSkinBoneBindPoses.Add(in float4x);
				}
				this.newSkinBoneCount.Value = this.useSkinBoneMapKeyList.Length;
			}

			// Token: 0x040009F6 RID: 2550
			public int oldVertexCount;

			// Token: 0x040009F7 RID: 2551
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x040009F8 RID: 2552
			[ReadOnly]
			public NativeArray<VirtualMeshBoneWeight> oldBoneWeights;

			// Token: 0x040009F9 RID: 2553
			[ReadOnly]
			public NativeArray<float4x4> oldBindPoses;

			// Token: 0x040009FA RID: 2554
			public NativeParallelHashMap<int, int> useSkinBoneMap;

			// Token: 0x040009FB RID: 2555
			public NativeList<int> newSkinBoneTransformIndices;

			// Token: 0x040009FC RID: 2556
			public NativeList<float4x4> newSkinBoneBindPoses;

			// Token: 0x040009FD RID: 2557
			public NativeReference<int> newSkinBoneCount;

			// Token: 0x040009FE RID: 2558
			public NativeList<int> useSkinBoneMapKeyList;
		}

		// Token: 0x0200014D RID: 333
		[BurstCompile]
		private struct Organize_CopyVertexJob : IJobParallelFor
		{
			// Token: 0x06000666 RID: 1638 RVA: 0x00038D28 File Offset: 0x00036F28
			public void Execute(int index)
			{
				if (this.joinIndices[index] < 0)
				{
					int num = this.vertexRemapIndices[index];
					this.newAttributes[num] = this.oldAttributes[index];
					this.newLocalPositions[num] = this.oldLocalPositions[index];
					this.newLocalNormals[num] = this.oldLocalNormals[index];
					this.newLocalTangents[num] = this.oldLocalTangents[index];
				}
			}

			// Token: 0x040009FF RID: 2559
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x04000A00 RID: 2560
			[ReadOnly]
			public NativeArray<int> vertexRemapIndices;

			// Token: 0x04000A01 RID: 2561
			[ReadOnly]
			public NativeArray<VertexAttribute> oldAttributes;

			// Token: 0x04000A02 RID: 2562
			[ReadOnly]
			public NativeArray<float3> oldLocalPositions;

			// Token: 0x04000A03 RID: 2563
			[ReadOnly]
			public NativeArray<float3> oldLocalNormals;

			// Token: 0x04000A04 RID: 2564
			[ReadOnly]
			public NativeArray<float3> oldLocalTangents;

			// Token: 0x04000A05 RID: 2565
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<VertexAttribute> newAttributes;

			// Token: 0x04000A06 RID: 2566
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> newLocalPositions;

			// Token: 0x04000A07 RID: 2567
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> newLocalNormals;

			// Token: 0x04000A08 RID: 2568
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<float3> newLocalTangents;
		}

		// Token: 0x0200014E RID: 334
		[BurstCompile]
		private struct Organize_RemapBoneWeightJob : IJobParallelFor
		{
			// Token: 0x06000667 RID: 1639 RVA: 0x00038DB4 File Offset: 0x00036FB4
			public void Execute(int vindex)
			{
				if (this.joinIndices[vindex] < 0)
				{
					int num = this.vertexRemapIndices[vindex];
					VirtualMeshBoneWeight virtualMeshBoneWeight = this.oldBoneWeights[vindex];
					for (int i = 0; i < 4; i++)
					{
						if (virtualMeshBoneWeight.weights[i] > 0f)
						{
							int num2 = virtualMeshBoneWeight.boneIndices[i];
							virtualMeshBoneWeight.boneIndices[i] = this.useSkinBoneMap[num2];
						}
						else
						{
							virtualMeshBoneWeight.boneIndices[i] = 0;
						}
					}
					this.newBoneWeights[num] = virtualMeshBoneWeight;
				}
			}

			// Token: 0x04000A09 RID: 2569
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x04000A0A RID: 2570
			[ReadOnly]
			public NativeArray<int> vertexRemapIndices;

			// Token: 0x04000A0B RID: 2571
			[ReadOnly]
			public NativeParallelHashMap<int, int> useSkinBoneMap;

			// Token: 0x04000A0C RID: 2572
			[ReadOnly]
			public NativeArray<int> oldSkinBoneIndices;

			// Token: 0x04000A0D RID: 2573
			[ReadOnly]
			public NativeArray<VirtualMeshBoneWeight> oldBoneWeights;

			// Token: 0x04000A0E RID: 2574
			[NativeDisableParallelForRestriction]
			[WriteOnly]
			public NativeArray<VirtualMeshBoneWeight> newBoneWeights;
		}

		// Token: 0x0200014F RID: 335
		[BurstCompile]
		private struct Organize_RemapLinkPointArrayJob : IJobParallelFor
		{
			// Token: 0x06000668 RID: 1640 RVA: 0x00038E50 File Offset: 0x00037050
			public void Execute(int vindex)
			{
				if (this.joinIndices[vindex] >= 0)
				{
					return;
				}
				int num = this.vertexRemapIndices[vindex];
				foreach (ushort num2 in this.oldVertexToVertexMap.GetValuesForKey((ushort)vindex))
				{
					int num3 = this.vertexRemapIndices[(int)num2];
					(ref this.newVertexToVertexMap).MC2UniqueAdd((ushort)num, (ushort)num3);
				}
			}

			// Token: 0x04000A0F RID: 2575
			[ReadOnly]
			public NativeArray<int> joinIndices;

			// Token: 0x04000A10 RID: 2576
			[ReadOnly]
			public NativeArray<int> vertexRemapIndices;

			// Token: 0x04000A11 RID: 2577
			[ReadOnly]
			public NativeParallelMultiHashMap<ushort, ushort> oldVertexToVertexMap;

			// Token: 0x04000A12 RID: 2578
			[NativeDisableParallelForRestriction]
			public NativeParallelMultiHashMap<ushort, ushort> newVertexToVertexMap;
		}

		// Token: 0x02000150 RID: 336
		[BurstCompile]
		private struct Organize_CreateLineTriangleJob : IJob
		{
			// Token: 0x06000669 RID: 1641 RVA: 0x00038EE4 File Offset: 0x000370E4
			public void Execute()
			{
				for (int i = 0; i < this.newVertexCount; i++)
				{
					foreach (ushort num in this.newVertexToVertexMap.GetValuesForKey((ushort)i))
					{
						int2 @int = DataUtility.PackInt2(i, (int)num);
						this.edgeSet.Add(@int);
					}
				}
			}

			// Token: 0x04000A13 RID: 2579
			public int newVertexCount;

			// Token: 0x04000A14 RID: 2580
			[ReadOnly]
			public NativeParallelMultiHashMap<ushort, ushort> newVertexToVertexMap;

			// Token: 0x04000A15 RID: 2581
			[WriteOnly]
			public NativeParallelHashSet<int2> edgeSet;
		}

		// Token: 0x02000151 RID: 337
		[BurstCompile]
		private struct Organize_CreateLineTriangleJob2 : IJob
		{
			// Token: 0x0600066A RID: 1642 RVA: 0x00038F64 File Offset: 0x00037164
			public void Execute()
			{
				foreach (int2 @int in this.edgeSet)
				{
					int num = 0;
					foreach (ushort num2 in this.newVertexToVertexMap.GetValuesForKey((ushort)@int.x))
					{
						if ((int)num2 != @int.x && (int)num2 != @int.y && (ref this.newVertexToVertexMap).MC2Contains((ushort)@int.y, num2))
						{
							int3 int2 = DataUtility.PackInt3(@int.x, @int.y, (int)num2);
							this.triangleSet.Add(int2);
							num++;
						}
					}
					if (num == 0)
					{
						this.newLineList.Add(in @int);
					}
				}
			}

			// Token: 0x04000A16 RID: 2582
			[ReadOnly]
			public NativeParallelMultiHashMap<ushort, ushort> newVertexToVertexMap;

			// Token: 0x04000A17 RID: 2583
			[WriteOnly]
			public NativeList<int2> newLineList;

			// Token: 0x04000A18 RID: 2584
			[ReadOnly]
			public NativeParallelHashSet<int2> edgeSet;

			// Token: 0x04000A19 RID: 2585
			[WriteOnly]
			public NativeParallelHashSet<int3> triangleSet;
		}

		// Token: 0x02000152 RID: 338
		[BurstCompile]
		private struct Organize_CreateNewTriangleJob3 : IJob
		{
			// Token: 0x0600066B RID: 1643 RVA: 0x00039068 File Offset: 0x00037268
			public void Execute()
			{
				foreach (int3 @int in this.triangleSet)
				{
					this.newTriangleList.Add(in @int);
				}
			}

			// Token: 0x04000A1A RID: 2586
			[WriteOnly]
			public NativeList<int3> newTriangleList;

			// Token: 0x04000A1B RID: 2587
			[ReadOnly]
			public NativeParallelHashSet<int3> triangleSet;
		}

		// Token: 0x02000153 RID: 339
		[Serializable]
		public class ShareSerializationData
		{
			// Token: 0x0600066C RID: 1644 RVA: 0x000390C4 File Offset: 0x000372C4
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				stringBuilder.AppendLine("===== VirtualMesh.SerializeData =====");
				stringBuilder.AppendLine("name:" + this.name);
				stringBuilder.AppendLine(string.Format("meshType:{0}", this.meshType));
				stringBuilder.AppendLine(string.Format("isBoneCloth:{0}", this.isBoneCloth));
				stringBuilder.AppendLine(string.Format("VertexCount:{0}", this.attributes.count));
				stringBuilder.AppendLine(string.Format("TriangleCount:{0}", this.triangles.count));
				stringBuilder.AppendLine(string.Format("LineCount:{0}", this.lines.count));
				return stringBuilder.ToString();
			}

			// Token: 0x04000A1C RID: 2588
			public string name;

			// Token: 0x04000A1D RID: 2589
			public VirtualMesh.MeshType meshType;

			// Token: 0x04000A1E RID: 2590
			public bool isBoneCloth;

			// Token: 0x04000A1F RID: 2591
			public ExSimpleNativeArray<int>.SerializationData referenceIndices;

			// Token: 0x04000A20 RID: 2592
			public ExSimpleNativeArray<VertexAttribute>.SerializationData attributes;

			// Token: 0x04000A21 RID: 2593
			public ExSimpleNativeArray<float3>.SerializationData localPositions;

			// Token: 0x04000A22 RID: 2594
			public ExSimpleNativeArray<float3>.SerializationData localNormals;

			// Token: 0x04000A23 RID: 2595
			public ExSimpleNativeArray<float3>.SerializationData localTangents;

			// Token: 0x04000A24 RID: 2596
			public ExSimpleNativeArray<float2>.SerializationData uv;

			// Token: 0x04000A25 RID: 2597
			public ExSimpleNativeArray<VirtualMeshBoneWeight>.SerializationData boneWeights;

			// Token: 0x04000A26 RID: 2598
			public ExSimpleNativeArray<int3>.SerializationData triangles;

			// Token: 0x04000A27 RID: 2599
			public ExSimpleNativeArray<int2>.SerializationData lines;

			// Token: 0x04000A28 RID: 2600
			public int centerTransformIndex;

			// Token: 0x04000A29 RID: 2601
			public float4x4 initLocalToWorld;

			// Token: 0x04000A2A RID: 2602
			public float4x4 initWorldToLocal;

			// Token: 0x04000A2B RID: 2603
			public quaternion initRotation;

			// Token: 0x04000A2C RID: 2604
			public quaternion initInverseRotation;

			// Token: 0x04000A2D RID: 2605
			public float3 initScale;

			// Token: 0x04000A2E RID: 2606
			public int skinRootIndex;

			// Token: 0x04000A2F RID: 2607
			public ExSimpleNativeArray<int>.SerializationData skinBoneTransformIndices;

			// Token: 0x04000A30 RID: 2608
			public ExSimpleNativeArray<float4x4>.SerializationData skinBoneBindPoses;

			// Token: 0x04000A31 RID: 2609
			public TransformData.ShareSerializationData transformData;

			// Token: 0x04000A32 RID: 2610
			public AABB boundingBox;

			// Token: 0x04000A33 RID: 2611
			public float averageVertexDistance;

			// Token: 0x04000A34 RID: 2612
			public float maxVertexDistance;

			// Token: 0x04000A35 RID: 2613
			public byte[] vertexToTriangles;

			// Token: 0x04000A36 RID: 2614
			public byte[] vertexToVertexIndexArray;

			// Token: 0x04000A37 RID: 2615
			public byte[] vertexToVertexDataArray;

			// Token: 0x04000A38 RID: 2616
			public byte[] edges;

			// Token: 0x04000A39 RID: 2617
			public byte[] edgeFlags;

			// Token: 0x04000A3A RID: 2618
			public int2[] edgeToTrianglesKeys;

			// Token: 0x04000A3B RID: 2619
			public ushort[] edgeToTrianglesValues;

			// Token: 0x04000A3C RID: 2620
			public byte[] vertexBindPosePositions;

			// Token: 0x04000A3D RID: 2621
			public byte[] vertexBindPoseRotations;

			// Token: 0x04000A3E RID: 2622
			public byte[] vertexToTransformRotations;

			// Token: 0x04000A3F RID: 2623
			public byte[] vertexDepths;

			// Token: 0x04000A40 RID: 2624
			public byte[] vertexRootIndices;

			// Token: 0x04000A41 RID: 2625
			public byte[] vertexParentIndices;

			// Token: 0x04000A42 RID: 2626
			public byte[] vertexChildIndexArray;

			// Token: 0x04000A43 RID: 2627
			public byte[] vertexChildDataArray;

			// Token: 0x04000A44 RID: 2628
			public byte[] vertexLocalPositions;

			// Token: 0x04000A45 RID: 2629
			public byte[] vertexLocalRotations;

			// Token: 0x04000A46 RID: 2630
			public byte[] normalAdjustmentRotations;

			// Token: 0x04000A47 RID: 2631
			public byte[] baseLineFlags;

			// Token: 0x04000A48 RID: 2632
			public byte[] baseLineStartDataIndices;

			// Token: 0x04000A49 RID: 2633
			public byte[] baseLineDataCounts;

			// Token: 0x04000A4A RID: 2634
			public byte[] baseLineData;

			// Token: 0x04000A4B RID: 2635
			public int[] customSkinningBoneIndices;

			// Token: 0x04000A4C RID: 2636
			public ushort[] centerFixedList;

			// Token: 0x04000A4D RID: 2637
			public float3 localCenterPosition;

			// Token: 0x04000A4E RID: 2638
			public float3 centerWorldPosition;

			// Token: 0x04000A4F RID: 2639
			public quaternion centerWorldRotation;

			// Token: 0x04000A50 RID: 2640
			public float3 centerWorldScale;

			// Token: 0x04000A51 RID: 2641
			public float4x4 toProxyMatrix;

			// Token: 0x04000A52 RID: 2642
			public quaternion toProxyRotation;
		}

		// Token: 0x02000154 RID: 340
		[Serializable]
		public class UniqueSerializationData : ITransform
		{
			// Token: 0x0600066E RID: 1646 RVA: 0x0003919E File Offset: 0x0003739E
			public void GetUsedTransform(HashSet<Transform> transformSet)
			{
				TransformData.UniqueSerializationData uniqueSerializationData = this.transformData;
				if (uniqueSerializationData == null)
				{
					return;
				}
				uniqueSerializationData.GetUsedTransform(transformSet);
			}

			// Token: 0x0600066F RID: 1647 RVA: 0x000391B1 File Offset: 0x000373B1
			public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
			{
				TransformData.UniqueSerializationData uniqueSerializationData = this.transformData;
				if (uniqueSerializationData == null)
				{
					return;
				}
				uniqueSerializationData.ReplaceTransform(replaceDict);
			}

			// Token: 0x04000A53 RID: 2643
			public TransformData.UniqueSerializationData transformData;
		}

		// Token: 0x02000155 RID: 341
		[BurstCompile]
		private struct Work_AverageTriangleDistanceJob : IJob
		{
			// Token: 0x06000671 RID: 1649 RVA: 0x000391C4 File Offset: 0x000373C4
			public void Execute()
			{
				int num = math.max(this.tcnt / 100, 1);
				float num2 = 0f;
				float num3 = 0f;
				int num4 = 0;
				for (int i = 0; i < this.tcnt; i += num)
				{
					int3 @int = this.triangles[i];
					float3 @float = this.localPositions[@int.x];
					float3 float2 = this.localPositions[@int.y];
					float3 float3 = this.localPositions[@int.z];
					float num5 = math.distancesq(@float, float2);
					float num6 = math.distancesq(float2, float3);
					float num7 = math.distancesq(float3, @float);
					num2 += num5;
					num2 += num6;
					num2 += num7;
					num4 += 3;
					num3 = math.max(num3, num5);
					num3 = math.max(num3, num6);
					num3 = math.max(num3, num7);
				}
				this.averageVertexDistance.Value = this.averageVertexDistance.Value + num2;
				this.averageCount.Value = this.averageCount.Value + num4;
				this.maxVertexDistance.Value = math.max(this.maxVertexDistance.Value, num3);
			}

			// Token: 0x04000A54 RID: 2644
			public int vcnt;

			// Token: 0x04000A55 RID: 2645
			public int tcnt;

			// Token: 0x04000A56 RID: 2646
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000A57 RID: 2647
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x04000A58 RID: 2648
			public NativeReference<float> averageVertexDistance;

			// Token: 0x04000A59 RID: 2649
			public NativeReference<int> averageCount;

			// Token: 0x04000A5A RID: 2650
			public NativeReference<float> maxVertexDistance;
		}

		// Token: 0x02000156 RID: 342
		[BurstCompile]
		private struct Work_AverageLineDistanceJob : IJob
		{
			// Token: 0x06000672 RID: 1650 RVA: 0x000392F0 File Offset: 0x000374F0
			public void Execute()
			{
				int num = math.max(this.lcnt / 100, 1);
				float num2 = 0f;
				int num3 = 0;
				float num4 = 0f;
				for (int i = 0; i < this.lcnt; i += num)
				{
					int2 @int = this.lines[i];
					float3 @float = this.localPositions[@int.x];
					float3 float2 = this.localPositions[@int.y];
					float num5 = math.distancesq(@float, float2);
					num2 += num5;
					num3++;
					num4 = math.max(num4, num5);
				}
				this.averageVertexDistance.Value = this.averageVertexDistance.Value + num2;
				this.averageCount.Value = this.averageCount.Value + num3;
				this.maxVertexDistance.Value = math.max(this.maxVertexDistance.Value, num4);
			}

			// Token: 0x04000A5B RID: 2651
			public int vcnt;

			// Token: 0x04000A5C RID: 2652
			public int lcnt;

			// Token: 0x04000A5D RID: 2653
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000A5E RID: 2654
			[ReadOnly]
			public NativeArray<int2> lines;

			// Token: 0x04000A5F RID: 2655
			public NativeReference<float> averageVertexDistance;

			// Token: 0x04000A60 RID: 2656
			public NativeReference<int> averageCount;

			// Token: 0x04000A61 RID: 2657
			public NativeReference<float> maxVertexDistance;
		}

		// Token: 0x02000157 RID: 343
		[BurstCompile]
		private struct Work_AddVertexIndexGirdMapJob : IJob
		{
			// Token: 0x06000673 RID: 1651 RVA: 0x000393CC File Offset: 0x000375CC
			public void Execute()
			{
				for (int i = 0; i < this.vcnt; i++)
				{
					GridMap<int>.AddGrid(this.positins[i], i, this.gridMap, this.gridSize);
				}
			}

			// Token: 0x04000A62 RID: 2658
			public float gridSize;

			// Token: 0x04000A63 RID: 2659
			public int vcnt;

			// Token: 0x04000A64 RID: 2660
			[ReadOnly]
			public NativeArray<float3> positins;

			// Token: 0x04000A65 RID: 2661
			[WriteOnly]
			public NativeParallelMultiHashMap<int3, int> gridMap;
		}

		// Token: 0x02000158 RID: 344
		[BurstCompile]
		private struct Work_IntersectTriangleJob : IJobParallelFor
		{
			// Token: 0x06000674 RID: 1652 RVA: 0x0003940C File Offset: 0x0003760C
			public void Execute(int tindex)
			{
				int3 @int = this.triangles[tindex];
				float3 @float = this.localPositions[@int.x];
				float3 float2 = this.localPositions[@int.y];
				float3 float3 = this.localPositions[@int.z];
				float3 float4;
				float num;
				MathUtility.GetTriangleSphere(@float, float2, float3, out float4, out num);
				float num2 = 0f;
				float3 float5 = 0;
				if (!MathUtility.IntersectRaySphere(in this.localRayPos, in this.localRayDir, in float4, in num, ref num2, ref float5))
				{
					return;
				}
				float num3;
				float num4;
				float num5;
				if (!MathUtility.IntersectSegmentTriangle(in this.localRayPos, in this.localRayEndPos, @float, float2, float3, this.doubleSide, out num3, out num4, out num5, out num2))
				{
					return;
				}
				float3 float6 = math.lerp(this.localRayPos, this.localRayEndPos, num2);
				float3 float7 = MathUtility.TriangleNormal(in @float, in float2, in float3);
				this.hitList.AddNoResize(new VirtualMeshRaycastHit
				{
					type = VirtualMeshPrimitive.Triangle,
					index = tindex,
					position = float6,
					distance = num2,
					normal = float7
				});
			}

			// Token: 0x04000A66 RID: 2662
			public float3 localRayPos;

			// Token: 0x04000A67 RID: 2663
			public float3 localRayDir;

			// Token: 0x04000A68 RID: 2664
			public float3 localRayEndPos;

			// Token: 0x04000A69 RID: 2665
			public bool doubleSide;

			// Token: 0x04000A6A RID: 2666
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000A6B RID: 2667
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x04000A6C RID: 2668
			[WriteOnly]
			public NativeList<VirtualMeshRaycastHit>.ParallelWriter hitList;
		}

		// Token: 0x02000159 RID: 345
		[BurstCompile]
		private struct Work_IntersectEdgeJob : IJobParallelFor
		{
			// Token: 0x06000675 RID: 1653 RVA: 0x0003951C File Offset: 0x0003771C
			public void Execute(int eindex)
			{
				int2 @int = this.edges[eindex];
				if (this.edgeToTriangles.ContainsKey(@int))
				{
					return;
				}
				float3 @float = this.localPositions[@int.x];
				float3 float2 = this.localPositions[@int.y];
				float num;
				float num2;
				float3 float3;
				float3 float4;
				if (math.sqrt(MathUtility.ClosestPtSegmentSegment(in @float, in float2, in this.localRayPos, in this.localRayEndPos, out num, out num2, out float3, out float4)) > this.localEdgeRadius)
				{
					return;
				}
				float3 float5 = float4;
				this.hitList.AddNoResize(new VirtualMeshRaycastHit
				{
					type = VirtualMeshPrimitive.Edge,
					index = eindex,
					position = float5,
					distance = num2,
					normal = -this.rayDir
				});
			}

			// Token: 0x04000A6D RID: 2669
			public float3 localRayPos;

			// Token: 0x04000A6E RID: 2670
			public float3 localRayDir;

			// Token: 0x04000A6F RID: 2671
			public float3 localRayEndPos;

			// Token: 0x04000A70 RID: 2672
			public float3 rayDir;

			// Token: 0x04000A71 RID: 2673
			public float localEdgeRadius;

			// Token: 0x04000A72 RID: 2674
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000A73 RID: 2675
			[ReadOnly]
			public NativeArray<int2> edges;

			// Token: 0x04000A74 RID: 2676
			[ReadOnly]
			public NativeParallelMultiHashMap<int2, ushort> edgeToTriangles;

			// Token: 0x04000A75 RID: 2677
			[WriteOnly]
			public NativeList<VirtualMeshRaycastHit>.ParallelWriter hitList;
		}

		// Token: 0x0200015A RID: 346
		[BurstCompile]
		private struct Work_IntersectPointJob : IJobParallelFor
		{
			// Token: 0x06000676 RID: 1654 RVA: 0x000395E4 File Offset: 0x000377E4
			public void Execute(int vindex)
			{
				if (this.vertexToTriangles[vindex].Length > 0)
				{
					return;
				}
				float3 @float = this.localPositions[vindex];
				float num = 0f;
				float3 float2 = 0;
				if (!MathUtility.IntersectRaySphere(in this.localRayPos, in this.localRayDir, in @float, in this.localPointRadius, ref num, ref float2))
				{
					return;
				}
				this.hitList.AddNoResize(new VirtualMeshRaycastHit
				{
					type = VirtualMeshPrimitive.Point,
					index = vindex,
					position = @float,
					distance = num,
					normal = -this.rayDir
				});
			}

			// Token: 0x04000A76 RID: 2678
			public float3 localRayPos;

			// Token: 0x04000A77 RID: 2679
			public float3 localRayDir;

			// Token: 0x04000A78 RID: 2680
			public float3 rayDir;

			// Token: 0x04000A79 RID: 2681
			public float localPointRadius;

			// Token: 0x04000A7A RID: 2682
			[ReadOnly]
			public NativeArray<float3> localPositions;

			// Token: 0x04000A7B RID: 2683
			[ReadOnly]
			public NativeArray<FixedList32Bytes<int>> vertexToTriangles;

			// Token: 0x04000A7C RID: 2684
			[WriteOnly]
			public NativeList<VirtualMeshRaycastHit>.ParallelWriter hitList;
		}

		// Token: 0x0200015B RID: 347
		[BurstCompile]
		private struct Work_IntersetcSortJob : IJob
		{
			// Token: 0x06000677 RID: 1655 RVA: 0x0003968A File Offset: 0x0003788A
			public void Execute()
			{
				if (this.hitList.Length > 1)
				{
					this.hitList.Sort<VirtualMeshRaycastHit>();
				}
			}

			// Token: 0x04000A7D RID: 2685
			public NativeList<VirtualMeshRaycastHit> hitList;
		}

		// Token: 0x0200015C RID: 348
		public enum MeshType
		{
			// Token: 0x04000A7F RID: 2687
			NormalMesh,
			// Token: 0x04000A80 RID: 2688
			NormalBoneMesh,
			// Token: 0x04000A81 RID: 2689
			ProxyMesh,
			// Token: 0x04000A82 RID: 2690
			ProxyBoneMesh,
			// Token: 0x04000A83 RID: 2691
			Mapping
		}
	}
}
