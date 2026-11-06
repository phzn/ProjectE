using System;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x0200003F RID: 63
	public class SelfCollisionConstraint : IDisposable
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000103 RID: 259 RVA: 0x0000AD7F File Offset: 0x00008F7F
		// (set) Token: 0x06000104 RID: 260 RVA: 0x0000AD87 File Offset: 0x00008F87
		public int PointPrimitiveCount { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000AD90 File Offset: 0x00008F90
		// (set) Token: 0x06000106 RID: 262 RVA: 0x0000AD98 File Offset: 0x00008F98
		public int EdgePrimitiveCount { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000107 RID: 263 RVA: 0x0000ADA1 File Offset: 0x00008FA1
		// (set) Token: 0x06000108 RID: 264 RVA: 0x0000ADA9 File Offset: 0x00008FA9
		public int TrianglePrimitiveCount { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000109 RID: 265 RVA: 0x0000ADB2 File Offset: 0x00008FB2
		// (set) Token: 0x0600010A RID: 266 RVA: 0x0000ADBA File Offset: 0x00008FBA
		internal int IntersectCount { get; private set; }

		// Token: 0x0600010B RID: 267 RVA: 0x0000ADC4 File Offset: 0x00008FC4
		public SelfCollisionConstraint()
		{
			this.intersectFlagArray = new NativeArray<byte>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.primitiveArrayB = new ExNativeArray<SelfCollisionConstraint.Primitive>(0, true);
			this.uniformGridStartCountBuffer = new ExNativeArray<SelfCollisionConstraint.GridInfo>(0, true);
			this.contactQueue = new NativeQueue<SelfCollisionConstraint.ContactInfo>(Allocator.Persistent);
			this.contactList = new NativeList<SelfCollisionConstraint.ContactInfo>(Allocator.Persistent);
			this.intersectQueue = new NativeQueue<SelfCollisionConstraint.IntersectInfo>(Allocator.Persistent);
			this.intersectList = new NativeList<SelfCollisionConstraint.IntersectInfo>(Allocator.Persistent);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000AE44 File Offset: 0x00009044
		public void Dispose()
		{
			this.PointPrimitiveCount = 0;
			this.EdgePrimitiveCount = 0;
			this.TrianglePrimitiveCount = 0;
			(ref this.intersectFlagArray).MC2DisposeSafe<byte>();
			ExNativeArray<SelfCollisionConstraint.Primitive> exNativeArray = this.primitiveArrayB;
			if (exNativeArray != null)
			{
				exNativeArray.Dispose();
			}
			this.primitiveArrayB = null;
			ExNativeArray<SelfCollisionConstraint.GridInfo> exNativeArray2 = this.uniformGridStartCountBuffer;
			if (exNativeArray2 != null)
			{
				exNativeArray2.Dispose();
			}
			this.uniformGridStartCountBuffer = null;
			if (this.contactQueue.IsCreated)
			{
				this.contactQueue.Dispose();
			}
			if (this.contactList.IsCreated)
			{
				this.contactList.Dispose();
			}
			if (this.intersectQueue.IsCreated)
			{
				this.intersectQueue.Dispose();
			}
			if (this.intersectList.IsCreated)
			{
				this.intersectList.Dispose();
			}
			this.IntersectCount = 0;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000AF08 File Offset: 0x00009108
		public bool HasPrimitive()
		{
			return this.PointPrimitiveCount + this.EdgePrimitiveCount + this.TrianglePrimitiveCount > 0;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000AF24 File Offset: 0x00009124
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[SelfCollisionConstraint]");
			stringBuilder.AppendLine(string.Format("  -intersectFlagArray:{0}", this.intersectFlagArray.IsCreated ? this.intersectFlagArray.Length : 0));
			return stringBuilder.ToString();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000AF78 File Offset: 0x00009178
		internal void Register(ClothProcess cprocess)
		{
			this.UpdateTeam(cprocess.TeamId);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000AF86 File Offset: 0x00009186
		internal void Exit(ClothProcess cprocess)
		{
			if (cprocess != null && cprocess.TeamId > 0)
			{
				this.UpdateTeam(cprocess.TeamId);
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000AFA0 File Offset: 0x000091A0
		internal void UpdateTeam(int teamId)
		{
			TeamManager team = MagicaManager.Team;
			if (!team.ContainsTeamData(teamId))
			{
				return;
			}
			ref TeamManager.TeamData teamDataRef = ref team.GetTeamDataRef(teamId);
			BitField64 flag = teamDataRef.flag;
			bool flag2 = teamDataRef.flag.IsSet(8);
			if (flag2 && teamDataRef.syncTeamId != 0 && team.ContainsTeamData(teamDataRef.syncTeamId))
			{
				ref TeamManager.TeamData teamDataRef2 = ref team.GetTeamDataRef(teamDataRef.syncTeamId);
				team.RemoveSyncParent(ref teamDataRef2, teamId);
			}
			ref ClothParameters parametersRef = ref team.GetParametersRef(teamId);
			int num = (int)(flag2 ? SelfCollisionConstraint.SelfCollisionMode.None : parametersRef.selfCollisionConstraint.selfMode);
			SelfCollisionConstraint.SelfCollisionMode selfCollisionMode = (flag2 ? SelfCollisionConstraint.SelfCollisionMode.None : parametersRef.selfCollisionConstraint.syncMode);
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			bool flag10 = false;
			bool flag11 = false;
			bool flag12 = false;
			bool flag13 = false;
			bool flag14 = false;
			bool flag15 = false;
			bool flag16 = false;
			bool flag17 = false;
			bool flag18 = false;
			bool flag19 = false;
			bool flag20 = false;
			if (num == 2)
			{
				if (teamDataRef.EdgeCount > 0)
				{
					flag4 = true;
					flag6 = true;
				}
				if (teamDataRef.TriangleCount > 0)
				{
					flag3 = true;
					flag5 = true;
					flag7 = true;
					flag8 = true;
				}
				if (teamDataRef.EdgeCount > 0 && teamDataRef.TriangleCount > 0)
				{
					flag9 = true;
					flag10 = true;
				}
			}
			if (selfCollisionMode != SelfCollisionConstraint.SelfCollisionMode.None && team.ContainsTeamData(teamDataRef.syncTeamId))
			{
				ref TeamManager.TeamData teamDataRef3 = ref team.GetTeamDataRef(teamDataRef.syncTeamId);
				if (selfCollisionMode == SelfCollisionConstraint.SelfCollisionMode.FullMesh)
				{
					if (teamDataRef.EdgeCount > 0 && teamDataRef3.EdgeCount > 0)
					{
						flag4 = true;
						flag11 = true;
					}
					if (teamDataRef.TriangleCount > 0)
					{
						flag5 = true;
						flag13 = true;
					}
					if (teamDataRef3.TriangleCount > 0)
					{
						flag3 = true;
						flag12 = true;
					}
					if (teamDataRef.EdgeCount > 0 && teamDataRef3.TriangleCount > 0)
					{
						flag14 = true;
					}
					if (teamDataRef.TriangleCount > 0 && teamDataRef3.EdgeCount > 0)
					{
						flag15 = true;
					}
				}
			}
			if (teamDataRef.syncParentTeamId.Length > 0 && !flag2)
			{
				for (int i = 0; i < teamDataRef.syncParentTeamId.Length; i++)
				{
					int num2 = teamDataRef.syncParentTeamId[i];
					ref TeamManager.TeamData teamDataRef4 = ref team.GetTeamDataRef(num2);
					if (teamDataRef4.IsValid && team.GetParametersRef(num2).selfCollisionConstraint.syncMode == SelfCollisionConstraint.SelfCollisionMode.FullMesh)
					{
						if (teamDataRef4.EdgeCount > 0 && teamDataRef.EdgeCount > 0)
						{
							flag4 = true;
							flag16 = true;
						}
						if (teamDataRef4.TriangleCount > 0)
						{
							flag3 = true;
							flag17 = true;
						}
						if (teamDataRef.TriangleCount > 0)
						{
							flag5 = true;
							flag18 = true;
						}
						if (teamDataRef.EdgeCount > 0 && teamDataRef4.TriangleCount > 0)
						{
							flag19 = true;
						}
						if (teamDataRef.TriangleCount > 0 && teamDataRef4.EdgeCount > 0)
						{
							flag20 = true;
						}
					}
				}
			}
			teamDataRef.flag.SetBits(32, flag3);
			teamDataRef.flag.SetBits(33, flag4);
			teamDataRef.flag.SetBits(34, flag5);
			teamDataRef.flag.SetBits(35, flag6);
			teamDataRef.flag.SetBits(38, flag7);
			teamDataRef.flag.SetBits(41, flag8);
			teamDataRef.flag.SetBits(44, flag9);
			teamDataRef.flag.SetBits(47, flag10);
			teamDataRef.flag.SetBits(36, flag11);
			teamDataRef.flag.SetBits(39, flag12);
			teamDataRef.flag.SetBits(42, flag13);
			teamDataRef.flag.SetBits(45, flag14);
			teamDataRef.flag.SetBits(48, flag15);
			teamDataRef.flag.SetBits(37, flag16);
			teamDataRef.flag.SetBits(40, flag17);
			teamDataRef.flag.SetBits(43, flag18);
			teamDataRef.flag.SetBits(46, flag19);
			teamDataRef.flag.SetBits(49, flag20);
			if (flag3 && !teamDataRef.selfPointChunk.IsValid)
			{
				int particleCount = teamDataRef.ParticleCount;
				teamDataRef.selfPointChunk = this.primitiveArrayB.AddRange(particleCount);
				this.uniformGridStartCountBuffer.AddRange(particleCount);
				int startIndex = teamDataRef.selfPointChunk.startIndex;
				this.InitPrimitive(teamId, teamDataRef, 0U, startIndex, particleCount);
				this.PointPrimitiveCount += particleCount;
			}
			else if (!flag3 && teamDataRef.selfPointChunk.IsValid)
			{
				this.primitiveArrayB.Remove(teamDataRef.selfPointChunk);
				this.uniformGridStartCountBuffer.Remove(teamDataRef.selfPointChunk);
				this.PointPrimitiveCount -= teamDataRef.selfPointChunk.dataLength;
				teamDataRef.selfPointChunk.Clear();
			}
			if (flag4 && !teamDataRef.selfEdgeChunk.IsValid)
			{
				int edgeCount = teamDataRef.EdgeCount;
				teamDataRef.selfEdgeChunk = this.primitiveArrayB.AddRange(edgeCount);
				this.uniformGridStartCountBuffer.AddRange(edgeCount);
				int startIndex2 = teamDataRef.selfEdgeChunk.startIndex;
				this.InitPrimitive(teamId, teamDataRef, 1U, startIndex2, edgeCount);
				this.EdgePrimitiveCount += edgeCount;
			}
			else if (!flag4 && teamDataRef.selfEdgeChunk.IsValid)
			{
				this.primitiveArrayB.Remove(teamDataRef.selfEdgeChunk);
				this.uniformGridStartCountBuffer.Remove(teamDataRef.selfEdgeChunk);
				this.EdgePrimitiveCount -= teamDataRef.selfEdgeChunk.dataLength;
				teamDataRef.selfEdgeChunk.Clear();
			}
			if (flag5 && !teamDataRef.selfTriangleChunk.IsValid)
			{
				int triangleCount = teamDataRef.TriangleCount;
				teamDataRef.selfTriangleChunk = this.primitiveArrayB.AddRange(triangleCount);
				this.uniformGridStartCountBuffer.AddRange(triangleCount);
				int startIndex3 = teamDataRef.selfTriangleChunk.startIndex;
				this.InitPrimitive(teamId, teamDataRef, 2U, startIndex3, triangleCount);
				this.TrianglePrimitiveCount += triangleCount;
			}
			else if (!flag5 && teamDataRef.selfTriangleChunk.IsValid)
			{
				this.primitiveArrayB.Remove(teamDataRef.selfTriangleChunk);
				this.uniformGridStartCountBuffer.Remove(teamDataRef.selfTriangleChunk);
				this.TrianglePrimitiveCount -= teamDataRef.selfTriangleChunk.dataLength;
				teamDataRef.selfTriangleChunk.Clear();
			}
			bool flag21 = teamDataRef.flag.TestAny(44, 6);
			bool flag22 = flag.TestAny(44, 6);
			if (flag21 && !flag22)
			{
				int num3 = this.IntersectCount;
				this.IntersectCount = num3 + 1;
			}
			else if (!flag21 && flag22)
			{
				int num3 = this.IntersectCount;
				this.IntersectCount = num3 - 1;
			}
			if (teamDataRef.syncTeamId != 0 && team.ContainsTeamData(teamDataRef.syncTeamId))
			{
				this.UpdateTeam(teamDataRef.syncTeamId);
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000B5D0 File Offset: 0x000097D0
		private void InitPrimitive(int teamId, TeamManager.TeamData tdata, uint kind, int startPrimitive, int length)
		{
			VirtualMeshManager vmesh = MagicaManager.VMesh;
			new SelfCollisionConstraint.InitPrimitiveJob
			{
				teamId = teamId,
				tdata = tdata,
				kind = kind,
				startPrimitive = startPrimitive,
				edges = vmesh.edges.GetNativeArray(),
				triangles = vmesh.triangles.GetNativeArray(),
				attributes = vmesh.attributes.GetNativeArray(),
				vertexDepths = vmesh.vertexDepths.GetNativeArray(),
				primitiveArrayB = this.primitiveArrayB.GetNativeArray()
			}.Run(length);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000B670 File Offset: 0x00009870
		internal void WorkBufferUpdate()
		{
			if (this.IntersectCount > 0)
			{
				int particleCount = MagicaManager.Simulation.ParticleCount;
				(ref this.intersectFlagArray).MC2Resize<byte>(particleCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000B6A0 File Offset: 0x000098A0
		private unsafe static void UpdatePrimitive(int k, int teamId, ref TeamManager.TeamData tdata, ref ClothParameters param, ref NativeArray<float3> nextPosArray, ref NativeArray<float3> oldPosArray, ref NativeArray<float> frictionArray, bool useIntersect, ref NativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB, ref NativeArray<byte> intersectFlagArray)
		{
			SelfCollisionConstraint.Primitive* unsafePtr = (SelfCollisionConstraint.Primitive*)primitiveArrayB.GetUnsafePtr<SelfCollisionConstraint.Primitive>();
			float num = 0f;
			DataChunk dataChunk = (((long)k == 0L) ? tdata.selfPointChunk : (((long)k == 1L) ? tdata.selfEdgeChunk : tdata.selfTriangleChunk));
			if (dataChunk.IsValid)
			{
				float3x3 float3x = 0;
				float3x3 float3x2 = 0;
				int num2 = dataChunk.startIndex;
				int i = 0;
				while (i < dataChunk.dataLength)
				{
					ref SelfCollisionConstraint.Primitive ptr = ref unsafePtr[num2];
					if (!ptr.IsIgnore())
					{
						int num3 = k + 1;
						uint num4 = 67108864U;
						uint num5 = 0U;
						for (int j = 0; j < num3; j++)
						{
							int num6 = ptr.particleIndices[j];
							*float3x[j] = nextPosArray[num6];
							*float3x2[j] = oldPosArray[num6];
							bool flag = (ptr.flag & num4) > 0U;
							ptr.invMass[j] = MathUtility.CalcSelfCollisionInverseMass(frictionArray[num6], flag, param.selfCollisionConstraint.clothMass);
							num4 <<= 1;
							if (useIntersect && intersectFlagArray[num6] != 0)
							{
								num5 |= 1U << j;
							}
						}
						float num7 = (in param.selfCollisionConstraint.surfaceThicknessCurveData).MC2EvaluateCurve(ptr.depth);
						num7 *= tdata.scaleRatio;
						ptr.thickness = num7;
						float3 @float = math.min(*float3x[0], *float3x2[0]);
						float3 float2 = math.max(*float3x[0], *float3x2[0]);
						AABB aabb = new AABB(in @float, in float2);
						for (int l = 1; l < num3; l++)
						{
							aabb.Encapsulate(float3x[l]);
							aabb.Encapsulate(float3x2[l]);
						}
						float maxSideLength = aabb.MaxSideLength;
						num = math.max(num, maxSideLength);
						aabb.Expand(num7);
						ptr.aabb = aabb;
						ptr.flag = (ptr.flag & 4294967288U) | num5;
					}
					i++;
					num2++;
				}
			}
			if ((long)k == 1L)
			{
				float num8 = num * 3f;
				tdata.selfGridSize = num8;
				tdata.selfMaxPrimitiveSize = num;
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000B900 File Offset: 0x00009B00
		private unsafe static void UpdateGrid(int k, int teamId, ref TeamManager.TeamData tdata, ref NativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB, ref NativeArray<SelfCollisionConstraint.GridInfo> uniformGridStartCountBuffer)
		{
			SelfCollisionConstraint.Primitive* unsafeReadOnlyPtr = (SelfCollisionConstraint.Primitive*)primitiveArrayB.GetUnsafeReadOnlyPtr<SelfCollisionConstraint.Primitive>();
			SelfCollisionConstraint.GridInfo* unsafePtr = (SelfCollisionConstraint.GridInfo*)uniformGridStartCountBuffer.GetUnsafePtr<SelfCollisionConstraint.GridInfo>();
			if (tdata.flag.IsSet(35) || tdata.flag.IsSet(38) || tdata.flag.IsSet(41) || tdata.flag.IsSet(37) || tdata.flag.IsSet(40) || tdata.flag.IsSet(43))
			{
				DataChunk dataChunk = (((long)k == 0L) ? tdata.selfPointChunk : (((long)k == 1L) ? tdata.selfEdgeChunk : tdata.selfTriangleChunk));
				if (dataChunk.IsValid)
				{
					int num = dataChunk.startIndex;
					int i = 0;
					while (i < dataChunk.dataLength)
					{
						ref SelfCollisionConstraint.Primitive ptr = ref unsafeReadOnlyPtr[num];
						if (ptr.IsIgnore())
						{
							ptr.grid = 1000000;
						}
						else
						{
							ptr.grid = SelfCollisionConstraint.GetGrid(ptr.aabb.Center, tdata.selfGridSize);
						}
						i++;
						num++;
					}
					NativeSortExtension.Sort<SelfCollisionConstraint.Primitive>(unsafeReadOnlyPtr + dataChunk.startIndex, dataChunk.dataLength);
					int3 @int = 0;
					int num2 = 0;
					int num3 = 0;
					int startIndex = dataChunk.startIndex;
					int num4 = startIndex;
					int num5 = 0;
					num = dataChunk.startIndex;
					int j = 0;
					while (j < dataChunk.dataLength)
					{
						ref SelfCollisionConstraint.Primitive ptr2 = ref unsafeReadOnlyPtr[num];
						if (j == 0)
						{
							@int = ptr2.grid;
							num2 = num;
							num3 = 0;
						}
						else if (!ptr2.grid.Equals(@int.xyz))
						{
							uniformGridStartCountBuffer[num4] = new SelfCollisionConstraint.GridInfo
							{
								hash = @int.GetHashCode(),
								start = num2,
								count = num3
							};
							num4++;
							num5++;
							@int = ptr2.grid;
							num2 = num;
							num3 = 0;
						}
						num3++;
						j++;
						num++;
					}
					if (num3 > 0)
					{
						uniformGridStartCountBuffer[num4] = new SelfCollisionConstraint.GridInfo
						{
							hash = @int.GetHashCode(),
							start = num2,
							count = num3
						};
						num4++;
						num5++;
					}
					switch (k)
					{
					case 0:
						tdata.selfPointGridCount = num5;
						break;
					case 1:
						tdata.selfEdgeGridCount = num5;
						break;
					case 2:
						tdata.selfTriangleGridCount = num5;
						break;
					}
					NativeSortExtension.Sort<SelfCollisionConstraint.GridInfo>(unsafePtr + startIndex, num5);
				}
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000BB93 File Offset: 0x00009D93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int3 GetGrid(float3 pos, float gridSize)
		{
			return new int3(math.floor(pos / gridSize));
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000BBA8 File Offset: 0x00009DA8
		private unsafe static void DetectionContacts(int workerCount, int workerIndex, int myTeamId, ref TeamManager.TeamData myTeam, uint myKind, int targetTeamId, ref TeamManager.TeamData targetTeam, uint targetKind, ref NativeArray<float3> nextPosArray, ref NativeArray<float3> oldPosArray, ref NativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB, ref NativeArray<SelfCollisionConstraint.GridInfo> uniformGridStartCountBuffer, ref NativeQueue<SelfCollisionConstraint.ContactInfo>.ParallelWriter contactQueue)
		{
			SelfCollisionConstraint.Primitive* unsafeReadOnlyPtr = (SelfCollisionConstraint.Primitive*)primitiveArrayB.GetUnsafeReadOnlyPtr<SelfCollisionConstraint.Primitive>();
			SelfCollisionConstraint.GridInfo* unsafeReadOnlyPtr2 = (SelfCollisionConstraint.GridInfo*)uniformGridStartCountBuffer.GetUnsafeReadOnlyPtr<SelfCollisionConstraint.GridInfo>();
			DataChunk dataChunk = ((myKind == 0U) ? myTeam.selfPointChunk : ((myKind == 1U) ? myTeam.selfEdgeChunk : myTeam.selfTriangleChunk));
			if (!dataChunk.IsValid)
			{
				return;
			}
			int2 @int = MathUtility.CalcSplitRange(dataChunk.dataLength, workerCount, workerIndex);
			dataChunk.startIndex += @int.x;
			dataChunk.dataLength = @int.y - @int.x;
			DataChunk dataChunk2 = ((targetKind == 0U) ? targetTeam.selfPointChunk : ((targetKind == 1U) ? targetTeam.selfEdgeChunk : targetTeam.selfTriangleChunk));
			if (!dataChunk2.IsValid)
			{
				return;
			}
			int startIndex = dataChunk2.startIndex;
			int num = 0;
			switch (targetKind)
			{
			case 0U:
				num = targetTeam.selfPointGridCount;
				break;
			case 1U:
				num = targetTeam.selfEdgeGridCount;
				break;
			case 2U:
				num = targetTeam.selfTriangleGridCount;
				break;
			}
			float selfMaxPrimitiveSize = targetTeam.selfMaxPrimitiveSize;
			float selfGridSize = targetTeam.selfGridSize;
			bool flag = myTeamId == targetTeamId && myKind == targetKind;
			bool flag2 = myTeamId == targetTeamId;
			bool flag3 = false;
			byte b = 0;
			if (myKind == 0U && targetKind == 2U)
			{
				b = 1;
			}
			else if (myKind == 2U && targetKind == 0U)
			{
				b = 1;
				flag3 = true;
			}
			SelfCollisionConstraint.GridInfo gridInfo = default(SelfCollisionConstraint.GridInfo);
			int num2 = dataChunk.startIndex;
			int i = 0;
			while (i < dataChunk.dataLength)
			{
				ref SelfCollisionConstraint.Primitive ptr = ref unsafeReadOnlyPtr[num2];
				if (!ptr.IsIgnore())
				{
					bool flag4 = (ptr.flag & 536870912U) > 0U;
					float3 @float = ptr.aabb.Min - selfMaxPrimitiveSize * 0.5f;
					float3 float2 = ptr.aabb.Max + selfMaxPrimitiveSize * 0.5f;
					int3 grid = SelfCollisionConstraint.GetGrid(@float, selfGridSize);
					int3 grid2 = SelfCollisionConstraint.GetGrid(float2, selfGridSize);
					int3 int2 = grid;
					bool flag5 = false;
					while (!flag5)
					{
						int hashCode = int2.GetHashCode();
						gridInfo.hash = hashCode;
						int num3 = NativeSortExtension.BinarySearch<SelfCollisionConstraint.GridInfo>(unsafeReadOnlyPtr2 + startIndex, num, gridInfo);
						if (num3 >= 0)
						{
							ref SelfCollisionConstraint.GridInfo ptr2 = ref (unsafeReadOnlyPtr2 + startIndex)[num3];
							int start = ptr2.start;
							int num4 = start + ptr2.count;
							if (!flag || num4 >= num2)
							{
								for (int j = (flag ? math.max(start, num2) : start); j < num4; j++)
								{
									if (!flag || num2 != j)
									{
										ref SelfCollisionConstraint.Primitive ptr3 = ref unsafeReadOnlyPtr[j];
										if (ptr.aabb.Overlaps(in ptr3.aabb) && !ptr3.IsIgnore() && (!flag4 || (ptr3.flag & 536870912U) == 0U) && (!flag2 || !ptr.AnyParticle(ref ptr3)))
										{
											SelfCollisionConstraint.ContactInfo contactInfo = new SelfCollisionConstraint.ContactInfo
											{
												primitiveIndex0 = ((!flag3) ? num2 : j),
												primitiveIndex1 = ((!flag3) ? j : num2),
												contactType = b,
												thickness = (half)(ptr.thickness + ptr3.thickness)
											};
											SelfCollisionConstraint.UpdateContactInfo(ref contactInfo, unsafeReadOnlyPtr, ref nextPosArray, ref oldPosArray, 2f, true);
											if (contactInfo.enable != 0)
											{
												contactQueue.Enqueue(contactInfo);
											}
										}
									}
								}
							}
						}
						int2.x++;
						if (int2.x > grid2.x)
						{
							int2.x = grid.x;
							int2.y++;
							if (int2.y > grid2.y)
							{
								int2.y = grid.y;
								int2.z++;
								if (int2.z > grid2.z)
								{
									flag5 = true;
								}
							}
						}
					}
				}
				i++;
				num2++;
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000BF84 File Offset: 0x0000A184
		private unsafe static void UpdateContactInfo(ref SelfCollisionConstraint.ContactInfo contact, SelfCollisionConstraint.Primitive* pt, ref NativeArray<float3> nextPosArray, ref NativeArray<float3> oldPosArray, float scrScale, bool first)
		{
			ref SelfCollisionConstraint.Primitive ptr = ref pt[contact.primitiveIndex0];
			ref SelfCollisionConstraint.Primitive ptr2 = ref pt[contact.primitiveIndex1];
			float num = contact.thickness;
			float num2 = num * scrScale;
			contact.enable = 0;
			if (contact.contactType != 0)
			{
				if (contact.contactType == 1)
				{
					float3 @float = nextPosArray[ptr.particleIndices.x];
					float3 float2 = oldPosArray[ptr.particleIndices.x];
					float3 float3 = nextPosArray[ptr2.particleIndices.x];
					float3 float4 = nextPosArray[ptr2.particleIndices.y];
					float3 float5 = nextPosArray[ptr2.particleIndices.z];
					float3 float6 = oldPosArray[ptr2.particleIndices.x];
					float3 float7 = oldPosArray[ptr2.particleIndices.y];
					float3 float8 = oldPosArray[ptr2.particleIndices.z];
					float3 float9 = @float - float2;
					float3 float10 = float3 - float6;
					float3 float11 = float4 - float7;
					float3 float12 = float5 - float8;
					float3 float14;
					float3 float13 = MathUtility.ClosestPtPointTriangle(in float2, in float6, in float7, in float8, out float14);
					float3 float15 = float10 * float14.x + float11 * float14.y + float12 * float14.z;
					float3 float16 = float13 - float2;
					float num3 = math.length(float16);
					if (num3 <= 1E-08f)
					{
						return;
					}
					float3 float17 = float16 / num3;
					float num4 = math.dot(float17, float9);
					float num5 = math.dot(float17, float15);
					if (num3 - num4 + num5 >= num + num2)
					{
						return;
					}
					float num6 = contact.s;
					if (first)
					{
						float3 float18 = MathUtility.TriangleNormal(in float6, in float7, in float8);
						float17 = math.normalize(float2 - float13);
						float num7 = math.dot(float18, float17);
						if (math.abs(num7) < Define.System.SelfCollisionPointTriangleAngleCos)
						{
							return;
						}
						num6 = math.sign(num7);
					}
					contact.s = (half)num6;
					contact.enable = 1;
				}
				return;
			}
			float3 float19 = nextPosArray[ptr.particleIndices.x];
			float3 float20 = nextPosArray[ptr.particleIndices.y];
			float3 float21 = nextPosArray[ptr2.particleIndices.x];
			float3 float22 = nextPosArray[ptr2.particleIndices.y];
			float3 float23 = oldPosArray[ptr.particleIndices.x];
			float3 float24 = oldPosArray[ptr.particleIndices.y];
			float3 float25 = oldPosArray[ptr2.particleIndices.x];
			float3 float26 = oldPosArray[ptr2.particleIndices.y];
			float num9;
			float num10;
			float3 float27;
			float3 float28;
			float num8 = math.sqrt(MathUtility.ClosestPtSegmentSegment(in float23, in float24, in float25, in float26, out num9, out num10, out float27, out float28));
			if (num8 < 1E-09f)
			{
				return;
			}
			float3 float29 = (float27 - float28) / num8;
			float3 float30 = float19 - float23;
			float3 float31 = float20 - float24;
			float3 float32 = float21 - float25;
			float3 float33 = float22 - float26;
			float3 float34 = math.lerp(float30, float31, num9);
			float3 float35 = math.lerp(float32, float33, num10);
			float num11 = math.dot(float29, float34);
			float num12 = math.dot(float29, float35);
			if (num8 + num11 - num12 > num + num2)
			{
				return;
			}
			contact.enable = 1;
			contact.s = (half)num9;
			contact.t = (half)num10;
			contact.n = (half3)float29;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000C2FC File Offset: 0x0000A4FC
		private unsafe static void DetectionIntersect(int workerCount, int workerIndex, int frameIndex, int myTeamId, ref TeamManager.TeamData myTeam, uint myKind, int targetTeamId, ref TeamManager.TeamData targetTeam, uint targetKind, ref NativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB, ref NativeArray<SelfCollisionConstraint.GridInfo> uniformGridStartCountBuffer, ref NativeQueue<SelfCollisionConstraint.IntersectInfo>.ParallelWriter intersectQueue)
		{
			SelfCollisionConstraint.Primitive* unsafeReadOnlyPtr = (SelfCollisionConstraint.Primitive*)primitiveArrayB.GetUnsafeReadOnlyPtr<SelfCollisionConstraint.Primitive>();
			SelfCollisionConstraint.GridInfo* unsafeReadOnlyPtr2 = (SelfCollisionConstraint.GridInfo*)uniformGridStartCountBuffer.GetUnsafeReadOnlyPtr<SelfCollisionConstraint.GridInfo>();
			DataChunk dataChunk = ((myKind == 0U) ? myTeam.selfPointChunk : ((myKind == 1U) ? myTeam.selfEdgeChunk : myTeam.selfTriangleChunk));
			if (!dataChunk.IsValid)
			{
				return;
			}
			DataChunk dataChunk2 = ((targetKind == 0U) ? targetTeam.selfPointChunk : ((targetKind == 1U) ? targetTeam.selfEdgeChunk : targetTeam.selfTriangleChunk));
			if (!dataChunk2.IsValid)
			{
				return;
			}
			int startIndex = dataChunk2.startIndex;
			int num = 0;
			switch (targetKind)
			{
			case 0U:
				num = targetTeam.selfPointGridCount;
				break;
			case 1U:
				num = targetTeam.selfEdgeGridCount;
				break;
			case 2U:
				num = targetTeam.selfTriangleGridCount;
				break;
			}
			float selfMaxPrimitiveSize = targetTeam.selfMaxPrimitiveSize;
			float selfGridSize = targetTeam.selfGridSize;
			if (selfMaxPrimitiveSize <= 1E-08f || selfGridSize <= 1E-08f)
			{
				return;
			}
			bool flag = myTeamId == targetTeamId;
			bool flag2 = myKind != 1U;
			DataChunk workerChunk = MathUtility.GetWorkerChunk(dataChunk.dataLength, workerCount, workerIndex);
			if (!workerChunk.IsValid)
			{
				return;
			}
			SelfCollisionConstraint.GridInfo gridInfo = default(SelfCollisionConstraint.GridInfo);
			int num2 = dataChunk.startIndex + workerChunk.startIndex;
			int i = 0;
			while (i < workerChunk.dataLength)
			{
				if (num2 % 2 == frameIndex)
				{
					ref SelfCollisionConstraint.Primitive ptr = ref unsafeReadOnlyPtr[num2];
					if (!ptr.IsIgnore())
					{
						bool flag3 = (ptr.flag & 536870912U) > 0U;
						float3 @float = ptr.aabb.Min - selfMaxPrimitiveSize * 0.5f;
						float3 float2 = ptr.aabb.Max + selfMaxPrimitiveSize * 0.5f;
						int3 grid = SelfCollisionConstraint.GetGrid(@float, selfGridSize);
						int3 grid2 = SelfCollisionConstraint.GetGrid(float2, selfGridSize);
						int3 @int = grid;
						bool flag4 = false;
						while (!flag4)
						{
							int hashCode = @int.GetHashCode();
							gridInfo.hash = hashCode;
							int num3 = NativeSortExtension.BinarySearch<SelfCollisionConstraint.GridInfo>(unsafeReadOnlyPtr2 + startIndex, num, gridInfo);
							if (num3 >= 0)
							{
								ref SelfCollisionConstraint.GridInfo ptr2 = ref (unsafeReadOnlyPtr2 + startIndex)[num3];
								int start = ptr2.start;
								int num4 = start + ptr2.count;
								for (int j = start; j < num4; j++)
								{
									ref SelfCollisionConstraint.Primitive ptr3 = ref unsafeReadOnlyPtr[j];
									if (ptr.aabb.Overlaps(in ptr3.aabb) && !ptr3.IsIgnore() && (!flag3 || (ptr3.flag & 536870912U) == 0U) && (!flag || !ptr.AnyParticle(ref ptr3)))
									{
										SelfCollisionConstraint.IntersectInfo intersectInfo = new SelfCollisionConstraint.IntersectInfo
										{
											edgeParticeIndices = ((!flag2) ? ptr.particleIndices.xy : ptr3.particleIndices.xy),
											triangleParticleIndices = ((!flag2) ? ptr3.particleIndices : ptr.particleIndices)
										};
										intersectQueue.Enqueue(intersectInfo);
									}
								}
							}
							@int.x++;
							if (@int.x > grid2.x)
							{
								@int.x = grid.x;
								@int.y++;
								if (@int.y > grid2.y)
								{
									@int.y = grid.y;
									@int.z++;
									if (@int.z > grid2.z)
									{
										flag4 = true;
									}
								}
							}
						}
					}
				}
				i++;
				num2++;
			}
		}

		// Token: 0x04000186 RID: 390
		public const uint KindPoint = 0U;

		// Token: 0x04000187 RID: 391
		public const uint KindEdge = 1U;

		// Token: 0x04000188 RID: 392
		public const uint KindTriangle = 2U;

		// Token: 0x04000189 RID: 393
		public const uint Flag_KindMask = 50331648U;

		// Token: 0x0400018A RID: 394
		public const uint Flag_Fix0 = 67108864U;

		// Token: 0x0400018B RID: 395
		public const uint Flag_Fix1 = 134217728U;

		// Token: 0x0400018C RID: 396
		public const uint Flag_Fix2 = 268435456U;

		// Token: 0x0400018D RID: 397
		public const uint Flag_AllFix = 536870912U;

		// Token: 0x0400018E RID: 398
		public const uint Flag_Ignore = 1073741824U;

		// Token: 0x0400018F RID: 399
		public const uint Flag_Enable = 2147483648U;

		// Token: 0x04000190 RID: 400
		public const uint Flag_Intersect0 = 1U;

		// Token: 0x04000191 RID: 401
		public const uint Flag_Intersect1 = 2U;

		// Token: 0x04000192 RID: 402
		public const uint Flag_Intersect2 = 4U;

		// Token: 0x04000193 RID: 403
		public const uint Flag_FixIntersect0 = 67108865U;

		// Token: 0x04000194 RID: 404
		public const uint Flag_FixIntersect1 = 134217730U;

		// Token: 0x04000195 RID: 405
		public const uint Flag_FixIntersect2 = 268435460U;

		// Token: 0x04000196 RID: 406
		internal ExNativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB;

		// Token: 0x04000197 RID: 407
		internal ExNativeArray<SelfCollisionConstraint.GridInfo> uniformGridStartCountBuffer;

		// Token: 0x04000198 RID: 408
		internal const byte ContactType_EdgeEdge = 0;

		// Token: 0x04000199 RID: 409
		internal const byte ContactType_PointTriangle = 1;

		// Token: 0x0400019A RID: 410
		internal const byte ContactType_TrianglePoint = 2;

		// Token: 0x0400019B RID: 411
		internal NativeQueue<SelfCollisionConstraint.ContactInfo> contactQueue;

		// Token: 0x0400019C RID: 412
		internal NativeList<SelfCollisionConstraint.ContactInfo> contactList;

		// Token: 0x0400019D RID: 413
		internal NativeQueue<SelfCollisionConstraint.IntersectInfo> intersectQueue;

		// Token: 0x0400019E RID: 414
		internal NativeList<SelfCollisionConstraint.IntersectInfo> intersectList;

		// Token: 0x040001A2 RID: 418
		internal NativeArray<byte> intersectFlagArray;

		// Token: 0x02000040 RID: 64
		public enum SelfCollisionMode
		{
			// Token: 0x040001A5 RID: 421
			None,
			// Token: 0x040001A6 RID: 422
			FullMesh = 2
		}

		// Token: 0x02000041 RID: 65
		[Serializable]
		public class SerializeData : IDataValidate
		{
			// Token: 0x0600011A RID: 282 RVA: 0x0000C667 File Offset: 0x0000A867
			public SerializeData()
			{
				this.selfMode = SelfCollisionConstraint.SelfCollisionMode.None;
				this.syncMode = SelfCollisionConstraint.SelfCollisionMode.None;
			}

			// Token: 0x0600011B RID: 283 RVA: 0x0000C698 File Offset: 0x0000A898
			public void DataValidate()
			{
				this.surfaceThickness.DataValidate(0.001f, 0.05f);
				this.clothMass = Mathf.Clamp01(this.clothMass);
			}

			// Token: 0x0600011C RID: 284 RVA: 0x0000C6C0 File Offset: 0x0000A8C0
			public SelfCollisionConstraint.SerializeData Clone()
			{
				return new SelfCollisionConstraint.SerializeData
				{
					selfMode = this.selfMode,
					surfaceThickness = this.surfaceThickness.Clone(),
					syncMode = this.syncMode,
					syncPartner = this.syncPartner,
					clothMass = this.clothMass
				};
			}

			// Token: 0x0600011D RID: 285 RVA: 0x0000C713 File Offset: 0x0000A913
			public MagicaCloth GetSyncPartner()
			{
				if (this.syncMode == SelfCollisionConstraint.SelfCollisionMode.None)
				{
					return null;
				}
				return this.syncPartner;
			}

			// Token: 0x040001A7 RID: 423
			public SelfCollisionConstraint.SelfCollisionMode selfMode;

			// Token: 0x040001A8 RID: 424
			public CurveSerializeData surfaceThickness = new CurveSerializeData(0.005f, 0.5f, 1f, false);

			// Token: 0x040001A9 RID: 425
			public SelfCollisionConstraint.SelfCollisionMode syncMode;

			// Token: 0x040001AA RID: 426
			public MagicaCloth syncPartner;

			// Token: 0x040001AB RID: 427
			[Range(0f, 1f)]
			public float clothMass;
		}

		// Token: 0x02000042 RID: 66
		public struct SelfCollisionConstraintParams
		{
			// Token: 0x0600011E RID: 286 RVA: 0x0000C728 File Offset: 0x0000A928
			public void Convert(SelfCollisionConstraint.SerializeData sdata, ClothProcess.ClothType clothType)
			{
				this.selfMode = ((clothType == ClothProcess.ClothType.BoneSpring) ? SelfCollisionConstraint.SelfCollisionMode.None : sdata.selfMode);
				this.surfaceThicknessCurveData = sdata.surfaceThickness.ConvertFloatArray();
				this.syncMode = ((clothType == ClothProcess.ClothType.BoneSpring) ? SelfCollisionConstraint.SelfCollisionMode.None : sdata.syncMode);
				this.clothMass = sdata.clothMass;
			}

			// Token: 0x040001AC RID: 428
			public SelfCollisionConstraint.SelfCollisionMode selfMode;

			// Token: 0x040001AD RID: 429
			public float4x4 surfaceThicknessCurveData;

			// Token: 0x040001AE RID: 430
			public SelfCollisionConstraint.SelfCollisionMode syncMode;

			// Token: 0x040001AF RID: 431
			public float clothMass;
		}

		// Token: 0x02000043 RID: 67
		internal struct Primitive : IComparable<SelfCollisionConstraint.Primitive>
		{
			// Token: 0x0600011F RID: 287 RVA: 0x0000C77A File Offset: 0x0000A97A
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool IsIgnore()
			{
				return (this.flag & 1073741824U) > 0U;
			}

			// Token: 0x06000120 RID: 288 RVA: 0x0000C78B File Offset: 0x0000A98B
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool IsAllFix()
			{
				return (this.flag & 536870912U) > 0U;
			}

			// Token: 0x06000121 RID: 289 RVA: 0x0000C79C File Offset: 0x0000A99C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool AnyParticle(ref SelfCollisionConstraint.Primitive pri)
			{
				uint num = ((this.flag & 50331648U) >> 24) + 1U;
				int num2 = 0;
				while ((long)num2 < (long)((ulong)num))
				{
					int num3 = this.particleIndices[num2];
					if (!math.all(pri.particleIndices - num3))
					{
						return true;
					}
					num2++;
				}
				return false;
			}

			// Token: 0x06000122 RID: 290 RVA: 0x0000C7EC File Offset: 0x0000A9EC
			public int CompareTo(SelfCollisionConstraint.Primitive other)
			{
				if (this.grid.x != other.grid.x)
				{
					return this.grid.x - other.grid.x;
				}
				if (this.grid.y != other.grid.y)
				{
					return this.grid.y - other.grid.y;
				}
				return this.grid.z - other.grid.z;
			}

			// Token: 0x040001B0 RID: 432
			public uint flag;

			// Token: 0x040001B1 RID: 433
			public int3 particleIndices;

			// Token: 0x040001B2 RID: 434
			public float3 invMass;

			// Token: 0x040001B3 RID: 435
			public AABB aabb;

			// Token: 0x040001B4 RID: 436
			public int3 grid;

			// Token: 0x040001B5 RID: 437
			public float depth;

			// Token: 0x040001B6 RID: 438
			public float thickness;
		}

		// Token: 0x02000044 RID: 68
		internal struct GridInfo : IComparable<SelfCollisionConstraint.GridInfo>
		{
			// Token: 0x06000123 RID: 291 RVA: 0x0000C870 File Offset: 0x0000AA70
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int CompareTo(SelfCollisionConstraint.GridInfo other)
			{
				if (this.hash < other.hash)
				{
					return -1;
				}
				if (this.hash > other.hash)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x040001B7 RID: 439
			public int hash;

			// Token: 0x040001B8 RID: 440
			public int start;

			// Token: 0x040001B9 RID: 441
			public int count;
		}

		// Token: 0x02000045 RID: 69
		internal struct ContactInfo
		{
			// Token: 0x040001BA RID: 442
			public int primitiveIndex0;

			// Token: 0x040001BB RID: 443
			public int primitiveIndex1;

			// Token: 0x040001BC RID: 444
			public byte contactType;

			// Token: 0x040001BD RID: 445
			public byte enable;

			// Token: 0x040001BE RID: 446
			public half thickness;

			// Token: 0x040001BF RID: 447
			public half s;

			// Token: 0x040001C0 RID: 448
			public half t;

			// Token: 0x040001C1 RID: 449
			public half3 n;
		}

		// Token: 0x02000046 RID: 70
		internal struct IntersectInfo
		{
			// Token: 0x040001C2 RID: 450
			public int2 edgeParticeIndices;

			// Token: 0x040001C3 RID: 451
			public int3 triangleParticleIndices;
		}

		// Token: 0x02000047 RID: 71
		[BurstCompile]
		private struct InitPrimitiveJob : IJobParallelFor
		{
			// Token: 0x06000124 RID: 292 RVA: 0x0000C894 File Offset: 0x0000AA94
			public void Execute(int index)
			{
				int num = this.startPrimitive + index;
				SelfCollisionConstraint.Primitive primitive = this.primitiveArrayB[num];
				int startIndex = this.tdata.particleChunk.startIndex;
				int3 @int = -1;
				if (this.kind == 0U)
				{
					@int[0] = startIndex + index;
				}
				else if (this.kind == 1U)
				{
					int startIndex2 = this.tdata.proxyEdgeChunk.startIndex;
					@int.xy = this.edges[startIndex2 + index] + startIndex;
				}
				else if (this.kind == 2U)
				{
					int startIndex3 = this.tdata.proxyTriangleChunk.startIndex;
					@int.xyz = this.triangles[startIndex3 + index] + startIndex;
				}
				uint num2 = 0U;
				uint num3 = 67108864U;
				bool flag = false;
				int num4 = 0;
				float num5 = 0f;
				int num6 = (int)(this.kind + 1U);
				for (int i = 0; i < num6; i++)
				{
					int num7 = @int[i];
					int num8 = this.tdata.proxyCommonChunk.startIndex + num7 - startIndex;
					VertexAttribute vertexAttribute = this.attributes[num8];
					if (vertexAttribute.IsMove())
					{
						num2 &= ~num3;
					}
					else
					{
						num2 |= num3;
						num4++;
					}
					num3 <<= 1;
					if (vertexAttribute.IsInvalid())
					{
						flag = true;
					}
					num5 += this.vertexDepths[num8];
				}
				if (num4 == num6)
				{
					num2 |= 536870912U;
				}
				else
				{
					num2 &= 3758096383U;
				}
				if (flag)
				{
					num2 |= 1073741824U;
				}
				num5 /= (float)num6;
				primitive.flag = (this.kind << 24) | num2;
				primitive.particleIndices = @int;
				primitive.depth = num5;
				primitive.grid = 1000000;
				this.primitiveArrayB[num] = primitive;
			}

			// Token: 0x040001C4 RID: 452
			public int teamId;

			// Token: 0x040001C5 RID: 453
			public TeamManager.TeamData tdata;

			// Token: 0x040001C6 RID: 454
			public uint kind;

			// Token: 0x040001C7 RID: 455
			public int startPrimitive;

			// Token: 0x040001C8 RID: 456
			[ReadOnly]
			public NativeArray<int2> edges;

			// Token: 0x040001C9 RID: 457
			[ReadOnly]
			public NativeArray<int3> triangles;

			// Token: 0x040001CA RID: 458
			[ReadOnly]
			public NativeArray<VertexAttribute> attributes;

			// Token: 0x040001CB RID: 459
			[ReadOnly]
			public NativeArray<float> vertexDepths;

			// Token: 0x040001CC RID: 460
			[NativeDisableParallelForRestriction]
			public NativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB;
		}

		// Token: 0x02000048 RID: 72
		[BurstCompile]
		internal struct SelfStep_UpdatePrimitiveJob : IJobParallelFor
		{
			// Token: 0x06000125 RID: 293 RVA: 0x0000CA74 File Offset: 0x0000AC74
			public unsafe void Execute(int index)
			{
				int num = index / this.workerCount;
				int num2 = index % this.workerCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				ClothParameters* unsafeReadOnlyPtr2 = (ClothParameters*)this.parameterArray.GetUnsafeReadOnlyPtr<ClothParameters>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				ref ClothParameters ptr2 = ref unsafeReadOnlyPtr2[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				SelfCollisionConstraint.UpdatePrimitive(num2, num3, ref ptr, ref ptr2, ref this.nextPosArray, ref this.oldPosArray, ref this.frictionArray, this.useIntersect, ref this.primitiveArrayB, ref this.intersectFlagArray);
			}

			// Token: 0x040001CD RID: 461
			public int workerCount;

			// Token: 0x040001CE RID: 462
			public int updateIndex;

			// Token: 0x040001CF RID: 463
			public float4 simulationPower;

			// Token: 0x040001D0 RID: 464
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040001D1 RID: 465
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040001D2 RID: 466
			[ReadOnly]
			public NativeArray<ClothParameters> parameterArray;

			// Token: 0x040001D3 RID: 467
			[ReadOnly]
			public NativeArray<float3> nextPosArray;

			// Token: 0x040001D4 RID: 468
			[ReadOnly]
			public NativeArray<float3> oldPosArray;

			// Token: 0x040001D5 RID: 469
			[ReadOnly]
			public NativeArray<float> frictionArray;

			// Token: 0x040001D6 RID: 470
			public bool useIntersect;

			// Token: 0x040001D7 RID: 471
			[NativeDisableParallelForRestriction]
			public NativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB;

			// Token: 0x040001D8 RID: 472
			[ReadOnly]
			public NativeArray<byte> intersectFlagArray;
		}

		// Token: 0x02000049 RID: 73
		[BurstCompile]
		internal struct SelfStep_UpdateGridJob : IJobParallelFor
		{
			// Token: 0x06000126 RID: 294 RVA: 0x0000CB2C File Offset: 0x0000AD2C
			public unsafe void Execute(int index)
			{
				int num = index / this.kindCount;
				int num2 = index % this.kindCount;
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				int num3 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num3];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				if (this.updateIndex == 0)
				{
					SelfCollisionConstraint.UpdateGrid(num2, num3, ref ptr, ref this.primitiveArrayB, ref this.uniformGridStartCountBuffer);
				}
			}

			// Token: 0x040001D9 RID: 473
			public int kindCount;

			// Token: 0x040001DA RID: 474
			public int updateIndex;

			// Token: 0x040001DB RID: 475
			public float4 simulationPower;

			// Token: 0x040001DC RID: 476
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040001DD RID: 477
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040001DE RID: 478
			[ReadOnly]
			public NativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB;

			// Token: 0x040001DF RID: 479
			[NativeDisableParallelForRestriction]
			[NativeDisableContainerSafetyRestriction]
			public NativeArray<SelfCollisionConstraint.GridInfo> uniformGridStartCountBuffer;
		}

		// Token: 0x0200004A RID: 74
		[BurstCompile]
		internal struct SelfStep_DetectionContactJob : IJobParallelFor
		{
			// Token: 0x06000127 RID: 295 RVA: 0x0000CBB4 File Offset: 0x0000ADB4
			public unsafe void Execute(int index)
			{
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				int num = index / (6 * this.workerCount);
				index %= 6 * this.workerCount;
				int num2 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num2];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				int num3 = index / 6;
				index %= 6;
				int num4 = index / 3;
				index %= 3;
				int num5 = index;
				uint num6 = 0U;
				uint num7 = 0U;
				switch (num5)
				{
				case 0:
					num6 = 1U;
					num7 = 1U;
					break;
				case 1:
					num6 = 0U;
					num7 = 2U;
					break;
				case 2:
					num6 = 2U;
					num7 = 0U;
					break;
				}
				if (num4 == 0)
				{
					if (num5 == 2)
					{
						return;
					}
					int num8 = 35 + num5 * 3;
					if (ptr.flag.IsSet(num8))
					{
						SelfCollisionConstraint.DetectionContacts(this.workerCount, num3, num2, ref ptr, num6, num2, ref ptr, num7, ref this.nextPosArray, ref this.oldPosArray, ref this.primitiveArrayB, ref this.uniformGridStartCountBuffer, ref this.contactQueue);
						return;
					}
				}
				else if (num4 == 1 && ptr.syncTeamId > 0)
				{
					ref TeamManager.TeamData ptr2 = ref unsafeReadOnlyPtr[ptr.syncTeamId];
					int num9 = 36 + num5 * 3;
					if (ptr.flag.IsSet(num9) && ptr2.IsProcess && ptr2.ParticleCount > 0)
					{
						SelfCollisionConstraint.DetectionContacts(this.workerCount, num3, num2, ref ptr, num6, ptr.syncTeamId, ref ptr2, num7, ref this.nextPosArray, ref this.oldPosArray, ref this.primitiveArrayB, ref this.uniformGridStartCountBuffer, ref this.contactQueue);
					}
				}
			}

			// Token: 0x040001E0 RID: 480
			public int updateIndex;

			// Token: 0x040001E1 RID: 481
			public int workerCount;

			// Token: 0x040001E2 RID: 482
			public int teamCount;

			// Token: 0x040001E3 RID: 483
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040001E4 RID: 484
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040001E5 RID: 485
			[ReadOnly]
			public NativeArray<float3> nextPosArray;

			// Token: 0x040001E6 RID: 486
			[ReadOnly]
			public NativeArray<float3> oldPosArray;

			// Token: 0x040001E7 RID: 487
			[ReadOnly]
			public NativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB;

			// Token: 0x040001E8 RID: 488
			[ReadOnly]
			public NativeArray<SelfCollisionConstraint.GridInfo> uniformGridStartCountBuffer;

			// Token: 0x040001E9 RID: 489
			[NativeDisableParallelForRestriction]
			public NativeQueue<SelfCollisionConstraint.ContactInfo>.ParallelWriter contactQueue;
		}

		// Token: 0x0200004B RID: 75
		[BurstCompile]
		internal struct SelfStep_ConvertContactListJob : IJob
		{
			// Token: 0x06000128 RID: 296 RVA: 0x0000CD43 File Offset: 0x0000AF43
			public void Execute()
			{
				this.contactList.Clear();
				if (this.contactQueue.Count > 0)
				{
					this.contactList.AddRange(this.contactQueue.ToArray(Allocator.Temp));
				}
			}

			// Token: 0x040001EA RID: 490
			[ReadOnly]
			public NativeQueue<SelfCollisionConstraint.ContactInfo> contactQueue;

			// Token: 0x040001EB RID: 491
			[NativeDisableParallelForRestriction]
			public NativeList<SelfCollisionConstraint.ContactInfo> contactList;
		}

		// Token: 0x0200004C RID: 76
		[BurstCompile]
		internal struct SelfStep_UpdateContactJob : IJobParallelForDefer
		{
			// Token: 0x06000129 RID: 297 RVA: 0x0000CD7C File Offset: 0x0000AF7C
			public unsafe void Execute(int index)
			{
				SelfCollisionConstraint.Primitive* unsafeReadOnlyPtr = (SelfCollisionConstraint.Primitive*)this.primitiveArrayB.GetUnsafeReadOnlyPtr<SelfCollisionConstraint.Primitive>();
				SelfCollisionConstraint.UpdateContactInfo(ref this.contactList.GetUnsafePtr<SelfCollisionConstraint.ContactInfo>()[index], unsafeReadOnlyPtr, ref this.nextPosArray, ref this.oldPosArray, 2f, this.first);
			}

			// Token: 0x040001EC RID: 492
			public bool first;

			// Token: 0x040001ED RID: 493
			[NativeDisableParallelForRestriction]
			public NativeList<SelfCollisionConstraint.ContactInfo> contactList;

			// Token: 0x040001EE RID: 494
			[ReadOnly]
			public NativeArray<float3> nextPosArray;

			// Token: 0x040001EF RID: 495
			[ReadOnly]
			public NativeArray<float3> oldPosArray;

			// Token: 0x040001F0 RID: 496
			[ReadOnly]
			public NativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB;
		}

		// Token: 0x0200004D RID: 77
		[BurstCompile]
		internal struct SelfStep_SolverContactJob : IJobParallelForDefer
		{
			// Token: 0x0600012A RID: 298 RVA: 0x0000CDC8 File Offset: 0x0000AFC8
			public unsafe void Execute(int index)
			{
				ref SelfCollisionConstraint.ContactInfo unsafeReadOnlyPtr = ref *this.contactList.GetUnsafeReadOnlyPtr<SelfCollisionConstraint.ContactInfo>();
				SelfCollisionConstraint.Primitive* unsafeReadOnlyPtr2 = (SelfCollisionConstraint.Primitive*)this.primitiveArrayB.GetUnsafeReadOnlyPtr<SelfCollisionConstraint.Primitive>();
				int* unsafePtr = (int*)this.tempCountBuffer.GetUnsafePtr<int>();
				int* unsafePtr2 = (int*)this.tempVectorBufferA.GetUnsafePtr<float3>();
				ref SelfCollisionConstraint.ContactInfo ptr = (ref unsafeReadOnlyPtr) + (IntPtr)index * (IntPtr)sizeof(SelfCollisionConstraint.ContactInfo);
				if (ptr.enable == 0)
				{
					return;
				}
				ref SelfCollisionConstraint.Primitive ptr2 = ref unsafeReadOnlyPtr2[ptr.primitiveIndex0];
				ref SelfCollisionConstraint.Primitive ptr3 = ref unsafeReadOnlyPtr2[ptr.primitiveIndex1];
				float num = ptr.thickness;
				if (ptr.contactType == 0)
				{
					float3 @float = this.nextPosArray[ptr2.particleIndices.x];
					float3 float2 = this.nextPosArray[ptr2.particleIndices.y];
					float3 float3 = this.nextPosArray[ptr3.particleIndices.x];
					float3 float4 = this.nextPosArray[ptr3.particleIndices.y];
					float num2 = ptr.s;
					float num3 = ptr.t;
					float3 float5 = ptr.n;
					float3 float6 = math.lerp(@float, float2, num2);
					float3 float7 = math.lerp(float3, float4, num3);
					float3 float8 = float6 - float7;
					float num4 = math.dot(float5, float8);
					if (num4 > num)
					{
						return;
					}
					float x = ptr2.invMass.x;
					float y = ptr2.invMass.y;
					float x2 = ptr3.invMass.x;
					float y2 = ptr3.invMass.y;
					float num5 = num - num4;
					float num6 = 1f - num2;
					float num7 = num2;
					float num8 = 1f - num3;
					float num9 = num3;
					float3 float9 = float5 * num6;
					float3 float10 = float5 * num7;
					float3 float11 = -float5 * num8;
					float3 float12 = -float5 * num9;
					float num10 = x * num6 * num6 + y * num7 * num7 + x2 * num8 * num8 + y2 * num9 * num9;
					if (num10 == 0f)
					{
						return;
					}
					num10 = num5 / num10;
					float3 float13 = num10 * x * float9;
					float3 float14 = num10 * y * float10;
					float3 float15 = num10 * x2 * float11;
					float3 float16 = num10 * y2 * float12;
					if ((ptr2.flag & 67108865U) == 0U)
					{
						InterlockUtility.AddFloat3(ptr2.particleIndices.x, float13, unsafePtr, unsafePtr2);
					}
					if ((ptr2.flag & 134217730U) == 0U)
					{
						InterlockUtility.AddFloat3(ptr2.particleIndices.y, float14, unsafePtr, unsafePtr2);
					}
					if ((ptr3.flag & 67108865U) == 0U)
					{
						InterlockUtility.AddFloat3(ptr3.particleIndices.x, float15, unsafePtr, unsafePtr2);
					}
					if ((ptr3.flag & 134217730U) == 0U)
					{
						InterlockUtility.AddFloat3(ptr3.particleIndices.y, float16, unsafePtr, unsafePtr2);
						return;
					}
				}
				else if (ptr.contactType == 1)
				{
					float3 float17 = this.nextPosArray[ptr3.particleIndices.x];
					float3 float18 = this.nextPosArray[ptr3.particleIndices.y];
					float3 float19 = this.nextPosArray[ptr3.particleIndices.z];
					float x3 = ptr3.invMass.x;
					float y3 = ptr3.invMass.y;
					float z = ptr3.invMass.z;
					float3 float20 = MathUtility.TriangleNormal(in float17, in float18, in float19);
					float3 float21 = this.nextPosArray[ptr2.particleIndices.x];
					float x4 = ptr2.invMass.x;
					float3 float22;
					MathUtility.ClosestPtPointTriangle(in float21, in float17, in float18, in float19, out float22);
					float num11 = ptr.s;
					float3 float23 = float20 * num11;
					float num12 = math.dot(float23, float21 - float17);
					if (num12 >= num)
					{
						return;
					}
					float num13 = num;
					float num14 = num12 - num13;
					float3 float24 = float23;
					float3 float25 = -float23 * float22[0];
					float3 float26 = -float23 * float22[1];
					float3 float27 = -float23 * float22[2];
					float num15 = x4 + x3 * float22.x * float22.x + y3 * float22.y * float22.y + z * float22.z * float22.z;
					if (num15 == 0f)
					{
						return;
					}
					num15 = num14 / num15;
					float3 float28 = -num15 * x4 * float24;
					float3 float29 = -num15 * x3 * float25;
					float3 float30 = -num15 * y3 * float26;
					float3 float31 = -num15 * z * float27;
					if ((ptr2.flag & 67108865U) == 0U)
					{
						InterlockUtility.AddFloat3(ptr2.particleIndices.x, float28, unsafePtr, unsafePtr2);
					}
					if ((ptr3.flag & 67108865U) == 0U)
					{
						InterlockUtility.AddFloat3(ptr3.particleIndices.x, float29, unsafePtr, unsafePtr2);
					}
					if ((ptr3.flag & 134217730U) == 0U)
					{
						InterlockUtility.AddFloat3(ptr3.particleIndices.y, float30, unsafePtr, unsafePtr2);
					}
					if ((ptr3.flag & 268435460U) == 0U)
					{
						InterlockUtility.AddFloat3(ptr3.particleIndices.z, float31, unsafePtr, unsafePtr2);
					}
				}
			}

			// Token: 0x040001F1 RID: 497
			[ReadOnly]
			public NativeArray<float3> nextPosArray;

			// Token: 0x040001F2 RID: 498
			[ReadOnly]
			public NativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB;

			// Token: 0x040001F3 RID: 499
			[ReadOnly]
			public NativeList<SelfCollisionConstraint.ContactInfo> contactList;

			// Token: 0x040001F4 RID: 500
			[NativeDisableParallelForRestriction]
			public NativeArray<float3> tempVectorBufferA;

			// Token: 0x040001F5 RID: 501
			[NativeDisableParallelForRestriction]
			public NativeArray<int> tempCountBuffer;
		}

		// Token: 0x0200004E RID: 78
		[BurstCompile]
		internal struct SelfStep_SumContactJob : IJobParallelFor
		{
			// Token: 0x0600012B RID: 299 RVA: 0x0000D304 File Offset: 0x0000B504
			public unsafe void Execute(int localIndex)
			{
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				int* unsafePtr = (int*)this.tempCountBuffer.GetUnsafePtr<int>();
				int* unsafePtr2 = (int*)this.tempVectorBufferA.GetUnsafePtr<float3>();
				float3* unsafePtr3 = (float3*)this.nextPosArray.GetUnsafePtr<float3>();
				int num = this.batchSelfTeamList[localIndex];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num];
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				if (this.updateIndex < ptr.updateCount)
				{
					int num2 = ptr.particleChunk.startIndex;
					int num3 = num2 * 3;
					int i = 0;
					while (i < ptr.particleChunk.dataLength)
					{
						int num4 = unsafePtr[num2];
						if (num4 > 0)
						{
							float3 @float = new float3((float)unsafePtr2[num3], (float)unsafePtr2[num3 + 1], (float)unsafePtr2[num3 + 2]);
							@float /= (float)num4;
							@float *= 1E-06f;
							unsafePtr3[num2] += @float;
						}
						i++;
						num2++;
						num3 += 3;
					}
				}
				int num5 = ptr.particleChunk.startIndex;
				int j = 0;
				while (j < ptr.particleChunk.dataLength)
				{
					this.tempCountBuffer[num5] = 0;
					this.tempVectorBufferA[num5] = 0;
					j++;
					num5++;
				}
			}

			// Token: 0x040001F6 RID: 502
			public int updateIndex;

			// Token: 0x040001F7 RID: 503
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x040001F8 RID: 504
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x040001F9 RID: 505
			[NativeDisableParallelForRestriction]
			public NativeArray<float3> nextPosArray;

			// Token: 0x040001FA RID: 506
			[NativeDisableParallelForRestriction]
			public NativeArray<float3> tempVectorBufferA;

			// Token: 0x040001FB RID: 507
			[NativeDisableParallelForRestriction]
			public NativeArray<int> tempCountBuffer;
		}

		// Token: 0x0200004F RID: 79
		[BurstCompile]
		internal struct SelfDetectionIntersectJob : IJobParallelFor
		{
			// Token: 0x0600012C RID: 300 RVA: 0x0000D480 File Offset: 0x0000B680
			public unsafe void Execute(int index)
			{
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				int num = index / this.workerCount;
				int num2 = this.batchSelfTeamList[num];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num2];
				if (this.updateIndex >= ptr.updateCount)
				{
					return;
				}
				if (ptr.updateCount == 0)
				{
					return;
				}
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				int num3 = index % this.workerCount;
				if (ptr.flag.IsSet(44))
				{
					SelfCollisionConstraint.DetectionIntersect(this.workerCount, num3, this.frameIndex, num2, ref ptr, 1U, num2, ref ptr, 2U, ref this.primitiveArrayB, ref this.uniformGridStartCountBuffer, ref this.intersectQueue);
				}
				if (ptr.syncTeamId > 0)
				{
					ref TeamManager.TeamData ptr2 = ref unsafeReadOnlyPtr[ptr.syncTeamId];
					if (ptr2.IsProcess && ptr2.ParticleCount > 0)
					{
						if (ptr.flag.IsSet(45))
						{
							SelfCollisionConstraint.DetectionIntersect(this.workerCount, num3, this.frameIndex, num2, ref ptr, 1U, ptr.syncTeamId, ref ptr2, 2U, ref this.primitiveArrayB, ref this.uniformGridStartCountBuffer, ref this.intersectQueue);
						}
						if (ptr.flag.IsSet(48))
						{
							SelfCollisionConstraint.DetectionIntersect(this.workerCount, num3, this.frameIndex, num2, ref ptr, 2U, ptr.syncTeamId, ref ptr2, 1U, ref this.primitiveArrayB, ref this.uniformGridStartCountBuffer, ref this.intersectQueue);
						}
					}
				}
			}

			// Token: 0x040001FC RID: 508
			public int updateIndex;

			// Token: 0x040001FD RID: 509
			public int workerCount;

			// Token: 0x040001FE RID: 510
			public int frameIndex;

			// Token: 0x040001FF RID: 511
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x04000200 RID: 512
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x04000201 RID: 513
			[ReadOnly]
			public NativeArray<SelfCollisionConstraint.Primitive> primitiveArrayB;

			// Token: 0x04000202 RID: 514
			[ReadOnly]
			public NativeArray<SelfCollisionConstraint.GridInfo> uniformGridStartCountBuffer;

			// Token: 0x04000203 RID: 515
			[NativeDisableParallelForRestriction]
			public NativeQueue<SelfCollisionConstraint.IntersectInfo>.ParallelWriter intersectQueue;
		}

		// Token: 0x02000050 RID: 80
		[BurstCompile]
		internal struct SelfConvertIntersectListJob : IJob
		{
			// Token: 0x0600012D RID: 301 RVA: 0x0000D5E0 File Offset: 0x0000B7E0
			public void Execute()
			{
				this.intersectList.Clear();
				if (this.intersectQueue.Count > 0)
				{
					this.intersectList.AddRange(this.intersectQueue.ToArray(Allocator.Temp));
				}
			}

			// Token: 0x04000204 RID: 516
			[ReadOnly]
			public NativeQueue<SelfCollisionConstraint.IntersectInfo> intersectQueue;

			// Token: 0x04000205 RID: 517
			[NativeDisableParallelForRestriction]
			public NativeList<SelfCollisionConstraint.IntersectInfo> intersectList;
		}

		// Token: 0x02000051 RID: 81
		[BurstCompile]
		internal struct SelfClearIntersectJob : IJobParallelFor
		{
			// Token: 0x0600012E RID: 302 RVA: 0x0000D618 File Offset: 0x0000B818
			public unsafe void Execute(int index)
			{
				TeamManager.TeamData* unsafeReadOnlyPtr = (TeamManager.TeamData*)this.teamDataArray.GetUnsafeReadOnlyPtr<TeamManager.TeamData>();
				int num = this.batchSelfTeamList[index];
				ref TeamManager.TeamData ptr = ref unsafeReadOnlyPtr[num];
				if (!ptr.IsProcess)
				{
					return;
				}
				if (ptr.ParticleCount == 0)
				{
					return;
				}
				int num2 = ptr.particleChunk.startIndex;
				int i = 0;
				while (i < ptr.particleChunk.dataLength)
				{
					this.intersectFlagArray[num2] = 0;
					i++;
					num2++;
				}
			}

			// Token: 0x04000206 RID: 518
			[ReadOnly]
			public NativeList<int> batchSelfTeamList;

			// Token: 0x04000207 RID: 519
			[ReadOnly]
			public NativeArray<TeamManager.TeamData> teamDataArray;

			// Token: 0x04000208 RID: 520
			[WriteOnly]
			[NativeDisableParallelForRestriction]
			public NativeArray<byte> intersectFlagArray;
		}

		// Token: 0x02000052 RID: 82
		[BurstCompile]
		internal struct SelfSolverIntersectJob : IJobParallelForDefer
		{
			// Token: 0x0600012F RID: 303 RVA: 0x0000D694 File Offset: 0x0000B894
			public unsafe void Execute(int index)
			{
				ref SelfCollisionConstraint.IntersectInfo ptr = ref this.intersectList.GetUnsafeReadOnlyPtr<SelfCollisionConstraint.IntersectInfo>()[index];
				float3 @float = this.nextPosArray[ptr.edgeParticeIndices.x];
				float3 float2 = this.nextPosArray[ptr.edgeParticeIndices.y];
				float3 float3 = this.nextPosArray[ptr.triangleParticleIndices.x];
				float3 float4 = this.nextPosArray[ptr.triangleParticleIndices.y];
				float3 float5 = this.nextPosArray[ptr.triangleParticleIndices.z];
				float3 float6 = @float - float2;
				float3 float7 = float5 - float3;
				float3 float8 = float4 - float3;
				float3 float9 = math.cross(float8, float7);
				float num = math.dot(float6, float9);
				if (math.abs(num) < 1E-08f)
				{
					return;
				}
				if (num < 0f)
				{
					@float = float2;
					float6 = -float6;
					num = -num;
				}
				float3 float10 = @float - float3;
				float num2 = math.dot(float10, float9);
				if (num2 < 0f)
				{
					return;
				}
				if (num2 > num)
				{
					return;
				}
				float3 float11 = math.cross(float6, float10);
				float num3 = math.dot(float7, float11);
				if (num3 < 0f || num3 > num)
				{
					return;
				}
				float num4 = -math.dot(float8, float11);
				if (num4 < 0f || num3 + num4 > num)
				{
					return;
				}
				this.intersectFlagArray[ptr.edgeParticeIndices.x] = 1;
				this.intersectFlagArray[ptr.edgeParticeIndices.y] = 1;
			}

			// Token: 0x04000209 RID: 521
			[ReadOnly]
			public NativeArray<float3> nextPosArray;

			// Token: 0x0400020A RID: 522
			[ReadOnly]
			public NativeList<SelfCollisionConstraint.IntersectInfo> intersectList;

			// Token: 0x0400020B RID: 523
			[WriteOnly]
			[NativeDisableParallelForRestriction]
			public NativeArray<byte> intersectFlagArray;
		}
	}
}
