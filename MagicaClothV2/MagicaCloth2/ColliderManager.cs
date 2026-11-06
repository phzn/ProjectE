using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000095 RID: 149
	public class ColliderManager : IManager, IDisposable, IValid
	{
		// Token: 0x0600028A RID: 650 RVA: 0x00014E04 File Offset: 0x00013004
		public void Dispose()
		{
			this.isValid = false;
			ExNativeArray<short> exNativeArray = this.teamIdArray;
			if (exNativeArray != null)
			{
				exNativeArray.Dispose();
			}
			ExNativeArray<ExBitFlag16> exNativeArray2 = this.flagArray;
			if (exNativeArray2 != null)
			{
				exNativeArray2.Dispose();
			}
			ExNativeArray<float3> exNativeArray3 = this.centerArray;
			if (exNativeArray3 != null)
			{
				exNativeArray3.Dispose();
			}
			ExNativeArray<float3> exNativeArray4 = this.sizeArray;
			if (exNativeArray4 != null)
			{
				exNativeArray4.Dispose();
			}
			ExNativeArray<float3> exNativeArray5 = this.framePositions;
			if (exNativeArray5 != null)
			{
				exNativeArray5.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray6 = this.frameRotations;
			if (exNativeArray6 != null)
			{
				exNativeArray6.Dispose();
			}
			ExNativeArray<float3> exNativeArray7 = this.frameScales;
			if (exNativeArray7 != null)
			{
				exNativeArray7.Dispose();
			}
			ExNativeArray<float3> exNativeArray8 = this.nowPositions;
			if (exNativeArray8 != null)
			{
				exNativeArray8.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray9 = this.nowRotations;
			if (exNativeArray9 != null)
			{
				exNativeArray9.Dispose();
			}
			ExNativeArray<float3> exNativeArray10 = this.oldFramePositions;
			if (exNativeArray10 != null)
			{
				exNativeArray10.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray11 = this.oldFrameRotations;
			if (exNativeArray11 != null)
			{
				exNativeArray11.Dispose();
			}
			ExNativeArray<float3> exNativeArray12 = this.oldPositions;
			if (exNativeArray12 != null)
			{
				exNativeArray12.Dispose();
			}
			ExNativeArray<quaternion> exNativeArray13 = this.oldRotations;
			if (exNativeArray13 != null)
			{
				exNativeArray13.Dispose();
			}
			ExNativeArray<ColliderManager.WorkData> exNativeArray14 = this.workDataArray;
			if (exNativeArray14 != null)
			{
				exNativeArray14.Dispose();
			}
			ExNativeArray<int> exNativeArray15 = this.mainColliderIndices;
			if (exNativeArray15 != null)
			{
				exNativeArray15.Dispose();
			}
			this.teamIdArray = null;
			this.flagArray = null;
			this.sizeArray = null;
			this.framePositions = null;
			this.frameRotations = null;
			this.frameScales = null;
			this.nowPositions = null;
			this.nowRotations = null;
			this.oldFramePositions = null;
			this.oldFrameRotations = null;
			this.oldPositions = null;
			this.oldRotations = null;
			this.workDataArray = null;
			this.mainColliderIndices = null;
			this.colliderSet.Clear();
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00014F84 File Offset: 0x00013184
		public void EnterdEditMode()
		{
			this.Dispose();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00014F8C File Offset: 0x0001318C
		public void Initialize()
		{
			this.Dispose();
			this.teamIdArray = new ExNativeArray<short>(256, false);
			this.flagArray = new ExNativeArray<ExBitFlag16>(256, false);
			this.centerArray = new ExNativeArray<float3>(256, false);
			this.sizeArray = new ExNativeArray<float3>(256, false);
			this.framePositions = new ExNativeArray<float3>(256, false);
			this.frameRotations = new ExNativeArray<quaternion>(256, false);
			this.frameScales = new ExNativeArray<float3>(256, false);
			this.nowPositions = new ExNativeArray<float3>(256, false);
			this.nowRotations = new ExNativeArray<quaternion>(256, false);
			this.oldFramePositions = new ExNativeArray<float3>(256, false);
			this.oldFrameRotations = new ExNativeArray<quaternion>(256, false);
			this.oldPositions = new ExNativeArray<float3>(256, false);
			this.oldRotations = new ExNativeArray<quaternion>(256, false);
			this.workDataArray = new ExNativeArray<ColliderManager.WorkData>(256, false);
			this.mainColliderIndices = new ExNativeArray<int>(256, false);
			this.isValid = true;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x000150A5 File Offset: 0x000132A5
		public bool IsValid()
		{
			return this.isValid;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000150B0 File Offset: 0x000132B0
		public void Register(ClothProcess cprocess)
		{
			if (!this.isValid)
			{
				return;
			}
			if (cprocess.cloth.SerializeData.colliderCollisionConstraint.ColliderLength > 0)
			{
				List<ColliderComponent> colliderList = cprocess.cloth.SerializeData.colliderCollisionConstraint.colliderList;
				int activeCount = 0;
				colliderList.ForEach(delegate(ColliderComponent x)
				{
					if (x)
					{
						activeCount += ((x.symmetryMode != ColliderSymmetryMode.None) ? 2 : 1);
					}
				});
				int teamId = cprocess.TeamId;
				ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(teamId);
				teamDataRef.colliderChunk = this.teamIdArray.AddRange(activeCount, (short)teamId);
				this.flagArray.AddRange(activeCount, default(ExBitFlag16));
				this.centerArray.AddRange(activeCount);
				this.sizeArray.AddRange(activeCount);
				this.framePositions.AddRange(activeCount);
				this.frameRotations.AddRange(activeCount);
				this.frameScales.AddRange(activeCount);
				this.nowPositions.AddRange(activeCount);
				this.nowRotations.AddRange(activeCount);
				this.oldFramePositions.AddRange(activeCount);
				this.oldFrameRotations.AddRange(activeCount);
				this.oldPositions.AddRange(activeCount);
				this.oldRotations.AddRange(activeCount);
				this.workDataArray.AddRange(activeCount);
				this.mainColliderIndices.AddRange(activeCount);
				Transform clothTransform = cprocess.cloth.ClothTransform;
				teamDataRef.colliderTransformChunk = MagicaManager.Bone.AddTransform(activeCount, teamId, clothTransform);
				teamDataRef.colliderCount = 0;
				for (int i = 0; i < colliderList.Count; i++)
				{
					ColliderComponent colliderComponent = colliderList[i];
					if (colliderComponent && !cprocess.colliderDict.ContainsKey(colliderComponent))
					{
						this.AddCollider(cprocess, colliderComponent);
					}
				}
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000152B4 File Offset: 0x000134B4
		public void Exit(ClothProcess cprocess)
		{
			if (!this.isValid)
			{
				return;
			}
			int teamId = cprocess.TeamId;
			foreach (ColliderComponent colliderComponent in cprocess.colliderDict.Keys)
			{
				if (colliderComponent && colliderComponent.Exit(teamId))
				{
					this.colliderSet.Remove(colliderComponent);
				}
			}
			cprocess.colliderDict.Clear();
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(teamId);
			DataChunk colliderChunk = teamDataRef.colliderChunk;
			this.teamIdArray.RemoveAndFill(colliderChunk, 0);
			this.flagArray.RemoveAndFill(colliderChunk, default(ExBitFlag16));
			this.centerArray.Remove(colliderChunk);
			this.sizeArray.Remove(colliderChunk);
			this.framePositions.Remove(colliderChunk);
			this.frameRotations.Remove(colliderChunk);
			this.frameScales.Remove(colliderChunk);
			this.nowPositions.Remove(colliderChunk);
			this.nowRotations.Remove(colliderChunk);
			this.oldFramePositions.Remove(colliderChunk);
			this.oldFrameRotations.Remove(colliderChunk);
			this.oldPositions.Remove(colliderChunk);
			this.oldRotations.Remove(colliderChunk);
			this.workDataArray.Remove(colliderChunk);
			this.mainColliderIndices.Remove(colliderChunk);
			teamDataRef.colliderChunk.Clear();
			teamDataRef.colliderCount = 0;
			MagicaManager.Bone.RemoveTransform(teamDataRef.colliderTransformChunk);
			teamDataRef.colliderTransformChunk.Clear();
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00015444 File Offset: 0x00013644
		internal void UpdateColliders(ClothProcess cprocess)
		{
			if (!this.isValid)
			{
				return;
			}
			List<ColliderComponent> colliderList = cprocess.cloth.SerializeData.colliderCollisionConstraint.colliderList;
			foreach (ColliderComponent colliderComponent in cprocess.colliderDict.Keys.ToList<ColliderComponent>())
			{
				if (colliderComponent && !colliderList.Contains(colliderComponent))
				{
					this.RemoveCollider(colliderComponent, cprocess.TeamId);
				}
			}
			foreach (ColliderComponent colliderComponent2 in colliderList)
			{
				if (colliderComponent2 && !cprocess.colliderDict.ContainsKey(colliderComponent2))
				{
					this.AddCollider(cprocess, colliderComponent2);
				}
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0001552C File Offset: 0x0001372C
		private void AddCollider(ClothProcess cprocess, ColliderComponent col)
		{
			if (!this.isValid)
			{
				return;
			}
			if (col == null)
			{
				return;
			}
			if (cprocess.colliderDict.ContainsKey(col))
			{
				return;
			}
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(cprocess.TeamId);
			this.AddColliderInternal(ref teamDataRef, cprocess, col, false);
			this.colliderSet.Add(col);
			col.SetActiveSymmetryMode(true);
			this.AddColliderInternal(ref teamDataRef, cprocess, col, true);
			col.Register(cprocess.TeamId);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000155A0 File Offset: 0x000137A0
		private void AddColliderInternal(ref TeamManager.TeamData tdata, ClothProcess cprocess, ColliderComponent col, bool isSymmetry)
		{
			if (isSymmetry)
			{
				ColliderSymmetryMode? colliderSymmetryMode = col.ActiveSymmetryMode;
				ColliderSymmetryMode colliderSymmetryMode2 = ColliderSymmetryMode.None;
				if ((colliderSymmetryMode.GetValueOrDefault() == colliderSymmetryMode2) & (colliderSymmetryMode != null))
				{
					return;
				}
				if (col.ActiveSymmetryTarget == null)
				{
					return;
				}
			}
			int teamId = cprocess.TeamId;
			if (!tdata.colliderChunk.IsValid)
			{
				int num = 8;
				tdata.colliderChunk = this.teamIdArray.AddRange(num, (short)cprocess.TeamId);
				this.flagArray.AddRange(num, default(ExBitFlag16));
				this.centerArray.AddRange(num);
				this.sizeArray.AddRange(num);
				this.framePositions.AddRange(num);
				this.frameRotations.AddRange(num);
				this.frameScales.AddRange(num);
				this.nowPositions.AddRange(num);
				this.nowRotations.AddRange(num);
				this.oldFramePositions.AddRange(num);
				this.oldFrameRotations.AddRange(num);
				this.oldPositions.AddRange(num);
				this.oldRotations.AddRange(num);
				this.workDataArray.AddRange(num);
				this.mainColliderIndices.AddRange(num);
				Transform clothTransform = cprocess.cloth.ClothTransform;
				tdata.colliderTransformChunk = MagicaManager.Bone.AddTransform(num, cprocess.TeamId, clothTransform);
				tdata.colliderCount = 0;
			}
			else if (tdata.UseColliderCount == tdata.colliderChunk.dataLength)
			{
				int num2 = tdata.colliderChunk.dataLength + 8;
				DataChunk colliderChunk = tdata.colliderChunk;
				tdata.colliderChunk = this.teamIdArray.Expand(colliderChunk, num2);
				this.flagArray.ExpandAndFill(colliderChunk, num2, default(ExBitFlag16), default(ExBitFlag16));
				this.centerArray.Expand(colliderChunk, num2);
				this.sizeArray.Expand(colliderChunk, num2);
				this.framePositions.Expand(colliderChunk, num2);
				this.frameRotations.Expand(colliderChunk, num2);
				this.frameScales.Expand(colliderChunk, num2);
				this.nowPositions.Expand(colliderChunk, num2);
				this.nowRotations.Expand(colliderChunk, num2);
				this.oldFramePositions.Expand(colliderChunk, num2);
				this.oldFrameRotations.Expand(colliderChunk, num2);
				this.oldPositions.Expand(colliderChunk, num2);
				this.oldRotations.Expand(colliderChunk, num2);
				this.workDataArray.Expand(colliderChunk, num2);
				this.mainColliderIndices.Expand(colliderChunk, num2);
				DataChunk colliderTransformChunk = tdata.colliderTransformChunk;
				tdata.colliderTransformChunk = MagicaManager.Bone.Expand(colliderTransformChunk, num2);
			}
			int useColliderCount = tdata.UseColliderCount;
			int num3 = tdata.colliderChunk.startIndex + useColliderCount;
			int num4 = tdata.colliderTransformChunk.startIndex + useColliderCount;
			ExBitFlag16 exBitFlag = DataUtility.SetColliderType(default(ExBitFlag16), col.GetColliderType());
			exBitFlag.SetFlag(256, true);
			exBitFlag.SetFlag(512, col.isActiveAndEnabled);
			exBitFlag.SetFlag(1024, true);
			exBitFlag.SetFlag(2048, col.IsReverseDirection());
			Transform transform = col.transform;
			float3 zero = float3.zero;
			quaternion identity = quaternion.identity;
			float3 @float = 1;
			float3 float2 = col.center;
			Transform transform2 = transform;
			if (isSymmetry)
			{
				ColliderSymmetryMode? colliderSymmetryMode = col.ActiveSymmetryMode;
				if (colliderSymmetryMode != null)
				{
					ColliderSymmetryMode colliderSymmetryMode2 = colliderSymmetryMode.GetValueOrDefault();
					ColliderManager.SymmetryType symmetryType;
					switch (colliderSymmetryMode2)
					{
					case ColliderSymmetryMode.X_Symmetry:
						symmetryType = ColliderManager.SymmetryType.X_Symmetry;
						break;
					case ColliderSymmetryMode.Y_Symmetry:
						symmetryType = ColliderManager.SymmetryType.Y_Symmetry;
						break;
					case ColliderSymmetryMode.Z_Symmetry:
						symmetryType = ColliderManager.SymmetryType.Z_Symmetry;
						break;
					default:
						if (colliderSymmetryMode2 != ColliderSymmetryMode.XYZ_Symmetry)
						{
							goto IL_03B4;
						}
						symmetryType = ColliderManager.SymmetryType.XYZ_Symmetry;
						break;
					}
					exBitFlag = DataUtility.SetSymmetryType(exBitFlag, symmetryType);
					float num5 = 1f;
					if (col is MagicaCapsuleCollider)
					{
						MagicaCapsuleCollider magicaCapsuleCollider = col as MagicaCapsuleCollider;
						colliderSymmetryMode = col.ActiveSymmetryMode;
						colliderSymmetryMode2 = ColliderSymmetryMode.X_Symmetry;
						if (((colliderSymmetryMode.GetValueOrDefault() == colliderSymmetryMode2) & (colliderSymmetryMode != null)) && magicaCapsuleCollider.direction == MagicaCapsuleCollider.Direction.X)
						{
							num5 = -1f;
						}
						else
						{
							colliderSymmetryMode = col.ActiveSymmetryMode;
							colliderSymmetryMode2 = ColliderSymmetryMode.Y_Symmetry;
							if (((colliderSymmetryMode.GetValueOrDefault() == colliderSymmetryMode2) & (colliderSymmetryMode != null)) && magicaCapsuleCollider.direction == MagicaCapsuleCollider.Direction.Y)
							{
								num5 = -1f;
							}
							else
							{
								colliderSymmetryMode = col.ActiveSymmetryMode;
								colliderSymmetryMode2 = ColliderSymmetryMode.Z_Symmetry;
								if (((colliderSymmetryMode.GetValueOrDefault() == colliderSymmetryMode2) & (colliderSymmetryMode != null)) && magicaCapsuleCollider.direction == MagicaCapsuleCollider.Direction.Z)
								{
									num5 = -1f;
								}
								else
								{
									colliderSymmetryMode = col.ActiveSymmetryMode;
									colliderSymmetryMode2 = ColliderSymmetryMode.XYZ_Symmetry;
									if ((colliderSymmetryMode.GetValueOrDefault() == colliderSymmetryMode2) & (colliderSymmetryMode != null))
									{
										num5 = -1f;
									}
								}
							}
						}
					}
					else if (col is MagicaPlaneCollider)
					{
						colliderSymmetryMode = col.ActiveSymmetryMode;
						if (colliderSymmetryMode != null)
						{
							colliderSymmetryMode2 = colliderSymmetryMode.GetValueOrDefault();
							if (colliderSymmetryMode2 == ColliderSymmetryMode.Y_Symmetry || colliderSymmetryMode2 == ColliderSymmetryMode.XYZ_Symmetry)
							{
								num5 = -1f;
							}
						}
					}
					exBitFlag.SetFlag(4096, true);
					exBitFlag.SetFlag(8192, num5 < 0f);
					transform2 = col.ActiveSymmetryTarget;
					goto IL_0518;
				}
				IL_03B4:
				object obj = "Unknown active symmetry mode.";
				Develop.LogError(in obj);
				return;
			}
			IL_0518:
			this.teamIdArray[num3] = (short)teamId;
			this.flagArray[num3] = exBitFlag;
			this.centerArray[num3] = float2;
			this.sizeArray[num3] = math.max(col.GetSize(), 0.0001f);
			this.framePositions[num3] = zero;
			this.frameRotations[num3] = identity;
			this.frameScales[num3] = @float;
			this.nowPositions[num3] = zero;
			this.nowRotations[num3] = identity;
			this.oldFramePositions[num3] = zero;
			this.oldFrameRotations[num3] = identity;
			this.oldPositions[num3] = zero;
			this.oldRotations[num3] = identity;
			this.mainColliderIndices[num3] = 0;
			if (cprocess.colliderDict.ContainsKey(col))
			{
				int2 @int = cprocess.colliderDict[col];
				if (isSymmetry)
				{
					@int.y = useColliderCount;
					this.mainColliderIndices[num3] = @int.x;
				}
				else
				{
					@int.x = useColliderCount;
				}
				cprocess.colliderDict[col] = @int;
			}
			else
			{
				cprocess.colliderDict.Add(col, new int2(useColliderCount, 0));
			}
			bool flag = cprocess.IsEnable && exBitFlag.IsSet(512);
			ExBitFlag8 exBitFlag2 = new ExBitFlag8(1);
			exBitFlag2.SetFlag(16, flag);
			MagicaManager.Bone.SetTransform(transform2, exBitFlag2, num4, teamId);
			tdata.colliderCount++;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00015C4F File Offset: 0x00013E4F
		internal void RemoveCollider(ColliderComponent col, int teamId)
		{
			if (!this.isValid)
			{
				return;
			}
			if (col == null || teamId == 0)
			{
				return;
			}
			this.RemoveColliderInternal(col, teamId, true);
			this.RemoveColliderInternal(col, teamId, false);
			if (col.Exit(teamId))
			{
				this.colliderSet.Remove(col);
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00015C90 File Offset: 0x00013E90
		private void RemoveColliderInternal(ColliderComponent col, int teamId, bool isSymmetry)
		{
			if (!this.isValid)
			{
				return;
			}
			if (col == null || teamId == 0)
			{
				return;
			}
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(teamId);
			int colliderCount = teamDataRef.colliderCount;
			if (colliderCount == 0)
			{
				return;
			}
			ClothProcess clothProcess = MagicaManager.Team.GetClothProcess(teamId);
			if (clothProcess == null)
			{
				return;
			}
			if (!clothProcess.colliderDict.ContainsKey(col))
			{
				return;
			}
			int2 @int = clothProcess.colliderDict[col];
			int num = @int.x;
			if (isSymmetry)
			{
				if (@int.y == 0)
				{
					return;
				}
				num = @int.y;
			}
			int num2 = teamDataRef.colliderChunk.startIndex + num;
			int num3 = teamDataRef.colliderTransformChunk.startIndex + num;
			int num4 = colliderCount - 1;
			int num5 = teamDataRef.colliderChunk.startIndex + num4;
			int num6 = teamDataRef.colliderTransformChunk.startIndex + num4;
			if (num2 < num5)
			{
				this.flagArray[num2] = this.flagArray[num5];
				this.teamIdArray[num2] = this.teamIdArray[num5];
				this.centerArray[num2] = this.centerArray[num5];
				this.sizeArray[num2] = this.sizeArray[num5];
				this.framePositions[num2] = this.framePositions[num5];
				this.frameRotations[num2] = this.frameRotations[num5];
				this.frameScales[num2] = this.frameScales[num5];
				this.nowPositions[num2] = this.nowPositions[num5];
				this.nowRotations[num2] = this.nowRotations[num5];
				this.oldFramePositions[num2] = this.oldFramePositions[num5];
				this.oldFrameRotations[num2] = this.oldFrameRotations[num5];
				this.oldPositions[num2] = this.oldPositions[num5];
				this.oldRotations[num2] = this.oldRotations[num5];
				this.mainColliderIndices[num2] = this.mainColliderIndices[num5];
				this.flagArray[num5] = default(ExBitFlag16);
				this.teamIdArray[num5] = 0;
				MagicaManager.Bone.CopyTransform(num6, num3);
				MagicaManager.Bone.SetTransform(null, default(ExBitFlag8), num6, 0);
				using (List<ColliderComponent>.Enumerator enumerator = clothProcess.colliderDict.Keys.ToList<ColliderComponent>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ColliderComponent colliderComponent = enumerator.Current;
						int2 int2 = clothProcess.colliderDict[colliderComponent];
						if (int2.x == num4)
						{
							int2.x = num;
							clothProcess.colliderDict[colliderComponent] = int2;
							break;
						}
						if (int2.y == num4)
						{
							int2.y = num;
							clothProcess.colliderDict[colliderComponent] = int2;
							break;
						}
					}
					goto IL_034B;
				}
			}
			this.flagArray[num2] = default(ExBitFlag16);
			this.teamIdArray[num2] = 0;
			MagicaManager.Bone.SetTransform(null, default(ExBitFlag8), num3, 0);
			IL_034B:
			if (isSymmetry)
			{
				@int.y = 0;
			}
			else
			{
				@int.x = 0;
			}
			clothProcess.colliderDict[col] = @int;
			if (@int.x == 0 && @int.y == 0 && !isSymmetry)
			{
				clothProcess.colliderDict.Remove(col);
			}
			teamDataRef.colliderCount--;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00016048 File Offset: 0x00014248
		internal void EnableCollider(ColliderComponent col, int teamId, bool sw)
		{
			if (!this.IsValid())
			{
				return;
			}
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(teamId);
			if (!teamDataRef.IsValid)
			{
				return;
			}
			ClothProcess clothProcess = MagicaManager.Team.GetClothProcess(teamId);
			if (!clothProcess.colliderDict.ContainsKey(col))
			{
				return;
			}
			int2 @int = clothProcess.colliderDict[col];
			for (int i = 0; i < 2; i++)
			{
				int num = @int[i];
				if (i != 1 || num != 0)
				{
					int num2 = teamDataRef.colliderChunk.startIndex + num;
					ExBitFlag16 exBitFlag = this.flagArray[num2];
					exBitFlag.SetFlag(512, sw);
					exBitFlag.SetFlag(1024, true);
					this.flagArray[num2] = exBitFlag;
					int num3 = teamDataRef.colliderTransformChunk.startIndex + num;
					bool flag = clothProcess.IsEnable && exBitFlag.IsSet(512);
					MagicaManager.Bone.EnableTransform(num3, flag);
				}
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00016140 File Offset: 0x00014340
		internal void EnableTeamCollider(int teamId)
		{
			if (!this.IsValid())
			{
				return;
			}
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(teamId);
			if (!teamDataRef.IsValid)
			{
				return;
			}
			if (teamDataRef.UseColliderCount == 0)
			{
				return;
			}
			bool isEnable = teamDataRef.IsEnable;
			DataChunk colliderTransformChunk = teamDataRef.colliderTransformChunk;
			for (int i = 0; i < colliderTransformChunk.dataLength; i++)
			{
				int num = teamDataRef.colliderChunk.startIndex + i;
				int num2 = colliderTransformChunk.startIndex + i;
				ExBitFlag16 exBitFlag = this.flagArray[num];
				exBitFlag.SetFlag(1024, true);
				this.flagArray[num] = exBitFlag;
				bool flag = isEnable && exBitFlag.IsSet(512);
				MagicaManager.Bone.EnableTransform(num2, flag);
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000161FC File Offset: 0x000143FC
		internal void UpdateParameters(ColliderComponent col, int teamId, bool changeSymmetry)
		{
			if (!this.IsValid())
			{
				return;
			}
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(teamId);
			if (!teamDataRef.IsValid)
			{
				return;
			}
			ClothProcess clothProcess = MagicaManager.Team.GetClothProcess(teamId);
			if (!clothProcess.colliderDict.ContainsKey(col))
			{
				return;
			}
			int2 @int = clothProcess.colliderDict[col];
			int num = @int.x;
			int num2 = teamDataRef.colliderChunk.startIndex + num;
			ExBitFlag16 exBitFlag = this.flagArray[num2];
			exBitFlag = DataUtility.SetColliderType(exBitFlag, col.GetColliderType());
			exBitFlag.SetFlag(2048, col.IsReverseDirection());
			this.flagArray[num2] = exBitFlag;
			this.centerArray[num2] = col.center;
			this.sizeArray[num2] = math.max(col.GetSize(), 0.0001f);
			ColliderSymmetryMode? activeSymmetryMode = col.ActiveSymmetryMode;
			ColliderSymmetryMode colliderSymmetryMode = ColliderSymmetryMode.None;
			if (!((activeSymmetryMode.GetValueOrDefault() == colliderSymmetryMode) & (activeSymmetryMode != null)) && col.ActiveSymmetryTarget != null)
			{
				if (changeSymmetry)
				{
					this.RemoveColliderInternal(col, teamId, true);
					this.AddColliderInternal(ref teamDataRef, clothProcess, col, true);
					return;
				}
				if (@int.y > 0)
				{
					num = @int.y;
					num2 = teamDataRef.colliderChunk.startIndex + num;
					exBitFlag = this.flagArray[num2];
					exBitFlag = DataUtility.SetColliderType(exBitFlag, col.GetColliderType());
					exBitFlag.SetFlag(2048, col.IsReverseDirection());
					this.flagArray[num2] = exBitFlag;
					this.centerArray[num2] = col.center;
					this.sizeArray[num2] = math.max(col.GetSize(), 0.0001f);
					return;
				}
			}
			else
			{
				this.RemoveColliderInternal(col, teamId, true);
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x000163D8 File Offset: 0x000145D8
		internal static void SimulationPreUpdate(DataChunk chunk, ref TeamManager.TeamData tdata, ref InertiaConstraint.CenterData cdata, ref NativeArray<ExBitFlag16> flagArray, ref NativeArray<float3> centerArray, ref NativeArray<float3> framePositions, ref NativeArray<quaternion> frameRotations, ref NativeArray<float3> frameScales, ref NativeArray<float3> oldFramePositions, ref NativeArray<quaternion> oldFrameRotations, ref NativeArray<float3> nowPositions, ref NativeArray<quaternion> nowRotations, ref NativeArray<float3> oldPositions, ref NativeArray<quaternion> oldRotations, ref NativeArray<int> mainColliderIndices, ref NativeArray<float3> transformPositionArray, ref NativeArray<quaternion> transformRotationArray, ref NativeArray<float3> transformScaleArray, ref NativeArray<float3> transformLocalPositionArray, ref NativeArray<quaternion> transformLocalRotationArray, ref NativeArray<float3> transformLocalScaleArray)
		{
			int num = tdata.colliderChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				ExBitFlag16 exBitFlag = flagArray[num];
				if (exBitFlag.IsSet(256) && exBitFlag.IsSet(512))
				{
					float3 @float = centerArray[num];
					int num2 = num - tdata.colliderChunk.startIndex;
					int num3 = tdata.colliderTransformChunk.startIndex + num2;
					bool flag = false;
					float3 float2 = transformScaleArray[num3];
					for (int j = 0; j < 3; j++)
					{
						float num4 = float2[j];
						if (num4 < 1E-06f && num4 > -1E-06f)
						{
							float2[j] = 1E-06f;
							flag = true;
						}
					}
					exBitFlag.SetFlag(16384, flag);
					float3 float9;
					quaternion quaternion3;
					float3 float10;
					if (exBitFlag.IsSet(4096))
					{
						int num5 = mainColliderIndices[num];
						int num6 = tdata.colliderTransformChunk.startIndex + num5;
						float3 float3 = transformLocalPositionArray[num6];
						quaternion quaternion = transformLocalRotationArray[num6];
						float3 float4 = MathUtility.ToEuler(in quaternion);
						float3 float5 = transformLocalScaleArray[num6];
						switch (DataUtility.GetSymmetryType(in exBitFlag))
						{
						case ColliderManager.SymmetryType.X_Symmetry:
							float3.x = -float3.x;
							@float.x = -@float.x;
							float4.y = -float4.y;
							float4.z = -float4.z;
							break;
						case ColliderManager.SymmetryType.Y_Symmetry:
							float3.y = -float3.y;
							@float.y = -@float.y;
							float4.x = -float4.x;
							float4.z = -float4.z;
							break;
						case ColliderManager.SymmetryType.Z_Symmetry:
							float3.z = -float3.z;
							@float.z = -@float.z;
							float4.x = -float4.x;
							float4.y = -float4.y;
							break;
						case ColliderManager.SymmetryType.XYZ_Symmetry:
							float3 = -float3;
							@float = -@float;
							break;
						}
						float3 float6 = transformPositionArray[num3];
						quaternion quaternion2 = transformRotationArray[num3];
						float3 float7 = math.sign(float2);
						float3 float8 = 1;
						if (float2.x < 0f || float2.y < 0f || float2.z < 0f)
						{
							float8 = float7 * -1f;
						}
						float9 = MathUtility.TransformPoint(in float3, in float6, in quaternion2, in float2);
						quaternion3 = math.mul(quaternion2, quaternion.Euler(math.radians(float4 * float8), math.RotationOrder.ZXY));
						float10 = float2 * float5;
						float9 += math.mul(quaternion3, @float * float7) * float10 * float7;
					}
					else
					{
						float9 = transformPositionArray[num3];
						quaternion3 = transformRotationArray[num3];
						float10 = float2;
						float3 float11 = math.sign(float10);
						float9 += math.mul(quaternion3, @float * float11) * float10 * float11;
					}
					framePositions[num] = float9;
					frameRotations[num] = quaternion3;
					frameScales[num] = float10;
					if (tdata.IsReset || exBitFlag.IsSet(1024) || flag)
					{
						oldFramePositions[num] = float9;
						oldFrameRotations[num] = quaternion3;
						nowPositions[num] = float9;
						nowRotations[num] = quaternion3;
						oldPositions[num] = float9;
						oldRotations[num] = quaternion3;
						exBitFlag.SetFlag(1024, false);
					}
					else if (tdata.IsInertiaShift || tdata.IsNegativeScaleTeleport)
					{
						float3 float12 = oldFramePositions[num];
						quaternion quaternion4 = oldFrameRotations[num];
						float3 float13 = nowPositions[num];
						quaternion quaternion5 = nowRotations[num];
						float3 float14 = oldPositions[num];
						quaternion quaternion6 = oldRotations[num];
						if (tdata.IsNegativeScaleTeleport)
						{
							float4x4 negativeScaleMatrix = cdata.negativeScaleMatrix;
							float12 = MathUtility.TransformPoint(in float12, in negativeScaleMatrix);
							quaternion4 = MathUtility.TransformRotation(in quaternion4, in negativeScaleMatrix, in tdata.negativeScaleChange);
							float13 = MathUtility.TransformPoint(in float13, in negativeScaleMatrix);
							quaternion5 = MathUtility.TransformRotation(in quaternion5, in negativeScaleMatrix, in tdata.negativeScaleChange);
							float14 = MathUtility.TransformPoint(in float14, in negativeScaleMatrix);
							quaternion6 = MathUtility.TransformRotation(in quaternion6, in negativeScaleMatrix, in tdata.negativeScaleChange);
						}
						if (tdata.IsInertiaShift)
						{
							float3 oldComponentWorldPosition = cdata.oldComponentWorldPosition;
							float12 = MathUtility.ShiftPosition(in float12, in oldComponentWorldPosition, in cdata.frameComponentShiftVector, in cdata.frameComponentShiftRotation);
							quaternion4 = math.mul(cdata.frameComponentShiftRotation, quaternion4);
							float13 = MathUtility.ShiftPosition(in float13, in oldComponentWorldPosition, in cdata.frameComponentShiftVector, in cdata.frameComponentShiftRotation);
							quaternion5 = math.mul(cdata.frameComponentShiftRotation, quaternion5);
							float14 = MathUtility.ShiftPosition(in float14, in oldComponentWorldPosition, in cdata.frameComponentShiftVector, in cdata.frameComponentShiftRotation);
							quaternion6 = math.mul(cdata.frameComponentShiftRotation, quaternion6);
						}
						oldFramePositions[num] = float12;
						oldFrameRotations[num] = quaternion4;
						nowPositions[num] = float13;
						nowRotations[num] = quaternion5;
						oldPositions[num] = float14;
						oldRotations[num] = quaternion6;
					}
					flagArray[num] = exBitFlag;
				}
				i++;
				num++;
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00016910 File Offset: 0x00014B10
		internal static void SimulationStartStep(ref TeamManager.TeamData tdata, ref InertiaConstraint.CenterData cdata, ref NativeArray<ExBitFlag16> flagArray, ref NativeArray<float3> sizeArray, ref NativeArray<float3> framePositions, ref NativeArray<quaternion> frameRotations, ref NativeArray<float3> frameScales, ref NativeArray<float3> oldFramePositions, ref NativeArray<quaternion> oldFrameRotations, ref NativeArray<float3> nowPositions, ref NativeArray<quaternion> nowRotations, ref NativeArray<float3> oldPositions, ref NativeArray<quaternion> oldRotations, ref NativeArray<ColliderManager.WorkData> workDataArray)
		{
			int num = tdata.colliderChunk.startIndex;
			int i = 0;
			while (i < tdata.colliderChunk.dataLength)
			{
				ExBitFlag16 exBitFlag = flagArray[num];
				if (exBitFlag.IsSet(256) && exBitFlag.IsSet(512) && !exBitFlag.IsSet(16384))
				{
					float3 @float = math.lerp(oldFramePositions[num], framePositions[num], tdata.frameInterpolation);
					quaternion quaternion = math.slerp(oldFrameRotations[num], frameRotations[num], tdata.frameInterpolation);
					quaternion = math.normalize(quaternion);
					nowPositions[num] = @float;
					nowRotations[num] = quaternion;
					float3 float2 = oldPositions[num];
					quaternion quaternion2 = oldRotations[num];
					float2 = math.lerp(float2, @float, cdata.stepMoveInertiaRatio);
					quaternion2 = math.slerp(quaternion2, quaternion, cdata.stepRotationInertiaRatio);
					oldPositions[num] = float2;
					oldRotations[num] = math.normalize(quaternion2);
					ColliderManager.ColliderType colliderType = DataUtility.GetColliderType(in exBitFlag);
					ColliderManager.WorkData workData = default(ColliderManager.WorkData);
					float3 float3 = sizeArray[num];
					float3 float4 = frameScales[num];
					workData.inverseOldRot = math.inverse(quaternion2);
					workData.rot = quaternion;
					if (colliderType == ColliderManager.ColliderType.Sphere)
					{
						float num2 = float3.x * math.abs(float4.x);
						workData.radius = num2;
						float3 float5 = math.min(float2, @float);
						float3 float6 = math.max(float2, @float);
						AABB aabb = new AABB(in float5, in float6);
						aabb.Expand(num2);
						workData.aabb = aabb;
						workData.oldPos.c0 = float2;
						workData.nextPos.c0 = @float;
					}
					else if (colliderType >= ColliderManager.ColliderType.CapsuleX_Center && colliderType <= ColliderManager.ColliderType.CapsuleZ_Start)
					{
						object obj = colliderType >= ColliderManager.ColliderType.CapsuleX_Center && colliderType <= ColliderManager.ColliderType.CapsuleZ_Center;
						float3 float7 = ((colliderType == ColliderManager.ColliderType.CapsuleX_Center || colliderType == ColliderManager.ColliderType.CapsuleX_Start) ? math.right() : ((colliderType == ColliderManager.ColliderType.CapsuleY_Center || colliderType == ColliderManager.ColliderType.CapsuleY_Start) ? math.up() : math.forward()));
						float num3 = math.dot(float4, float7);
						float7 *= math.sign(num3);
						float num4 = math.abs(num3);
						if (exBitFlag.IsSet(2048))
						{
							float7 = -float7;
						}
						if (exBitFlag.IsSet(4096) && exBitFlag.IsSet(8192))
						{
							float7 = -float7;
						}
						float3 *= num4;
						float x = float3.x;
						float y = float3.y;
						float z = float3.z;
						object obj2 = obj;
						float num5 = ((obj2 != null) ? (z * 0.5f) : 0f);
						float num6 = ((obj2 != null) ? (z * 0.5f) : (z - x));
						num5 = math.max(num5 - x, 0f);
						num6 = math.max(num6 - y, 0f);
						float3 float8 = float2 + math.mul(quaternion2, float7 * num5);
						float3 float9 = float2 - math.mul(quaternion2, float7 * num6);
						float3 float10 = @float + math.mul(quaternion, float7 * num5);
						float3 float11 = @float - math.mul(quaternion, float7 * num6);
						float3 float5 = math.min(float8, float10) - x;
						float3 float6 = math.max(float8, float10) + x;
						AABB aabb2 = new AABB(in float5, in float6);
						float5 = math.min(float9, float11) - y;
						float6 = math.max(float9, float11) + y;
						AABB aabb3 = new AABB(in float5, in float6);
						aabb2.Encapsulate(in aabb3);
						workData.aabb = aabb2;
						workData.radius = new float2(x, y);
						workData.oldPos = new float3x2(float8, float9);
						workData.nextPos = new float3x2(float10, float11);
					}
					else if (colliderType == ColliderManager.ColliderType.Plane)
					{
						float3 float12 = math.up();
						float12 *= math.sign(float4.y);
						if (exBitFlag.IsSet(4096) && exBitFlag.IsSet(8192))
						{
							float12 = -float12;
						}
						float3 float13 = math.mul(quaternion, float12);
						workData.oldPos.c0 = float13;
						workData.nextPos.c0 = @float;
					}
					workDataArray[num] = workData;
				}
				i++;
				num++;
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00016D70 File Offset: 0x00014F70
		internal static void SimulationEndStep(DataChunk chunk, ref TeamManager.TeamData tdata, ref NativeArray<float3> nowPositions, ref NativeArray<quaternion> nowRotations, ref NativeArray<float3> oldPositions, ref NativeArray<quaternion> oldRotations)
		{
			int num = tdata.colliderChunk.startIndex + chunk.startIndex;
			int i = 0;
			while (i < chunk.dataLength)
			{
				oldPositions[num] = nowPositions[num];
				oldRotations[num] = nowRotations[num];
				i++;
				num++;
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00016DC4 File Offset: 0x00014FC4
		internal static void SimulationPostUpdate(ref TeamManager.TeamData tdata, ref NativeArray<float3> framePositions, ref NativeArray<quaternion> frameRotations, ref NativeArray<float3> oldFramePositions, ref NativeArray<quaternion> oldFrameRotations)
		{
			if (tdata.colliderCount == 0)
			{
				return;
			}
			if (!tdata.IsRunning)
			{
				return;
			}
			int num = tdata.colliderChunk.startIndex;
			int i = 0;
			while (i < tdata.colliderChunk.dataLength)
			{
				oldFramePositions[num] = framePositions[num];
				oldFrameRotations[num] = frameRotations[num];
				i++;
				num++;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00016E28 File Offset: 0x00015028
		public void InformationLog(StringBuilder allsb)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("========== Collider Manager ==========");
			if (!this.IsValid())
			{
				stringBuilder.AppendLine("Collider Manager. Invalid.");
			}
			else
			{
				ExNativeArray<short> exNativeArray = this.teamIdArray;
				int num = ((exNativeArray != null) ? exNativeArray.Count : 0);
				stringBuilder.AppendLine(string.Format("Use:{0}", num));
				stringBuilder.AppendLine("  -flagArray:" + this.flagArray.ToSummary());
				stringBuilder.AppendLine("  -centerArray:" + this.centerArray.ToSummary());
				stringBuilder.AppendLine("  -sizeArray:" + this.sizeArray.ToSummary());
				stringBuilder.AppendLine("  -framePositions:" + this.framePositions.ToSummary());
				stringBuilder.AppendLine("  -frameRotations:" + this.frameRotations.ToSummary());
				stringBuilder.AppendLine("  -frameScales:" + this.frameScales.ToSummary());
				stringBuilder.AppendLine("  -oldFramePositions:" + this.oldFramePositions.ToSummary());
				stringBuilder.AppendLine("  -oldFrameRotations:" + this.oldFrameRotations.ToSummary());
				stringBuilder.AppendLine("  -nowPositions:" + this.nowPositions.ToSummary());
				stringBuilder.AppendLine("  -nowRotations:" + this.nowRotations.ToSummary());
				stringBuilder.AppendLine("  -oldPositions:" + this.oldPositions.ToSummary());
				stringBuilder.AppendLine("  -oldRotations:" + this.oldRotations.ToSummary());
				stringBuilder.AppendLine("  -mainColliderIndices:" + this.mainColliderIndices.ToSummary());
				stringBuilder.AppendLine("[Colliders]");
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					ExBitFlag16 exBitFlag = this.flagArray[i];
					if (exBitFlag.IsSet(256))
					{
						num2++;
						ColliderManager.ColliderType colliderType = DataUtility.GetColliderType(in exBitFlag);
						string text = (exBitFlag.IsSet(4096) ? "Symmetry" : "");
						stringBuilder.AppendLine(string.Format("  [{0}] tid:{1}, flag:0x{2:X}, type:{3}, size:{4}, cen:{5}, {6}", new object[]
						{
							i,
							this.teamIdArray[i],
							exBitFlag.Value,
							colliderType,
							this.sizeArray[i],
							this.centerArray[i],
							text
						}));
					}
				}
				stringBuilder.AppendLine(string.Format("  ActiveCount:{0}", num2));
				stringBuilder.AppendLine(string.Format("[Collider Components:{0}]", this.colliderSet.Count));
				foreach (ColliderComponent colliderComponent in this.colliderSet)
				{
					if (colliderComponent)
					{
						stringBuilder.Append(string.Format("({0})  {1}", colliderComponent.UseTeamCount, colliderComponent.name));
						ColliderSymmetryMode? activeSymmetryMode = colliderComponent.ActiveSymmetryMode;
						ColliderSymmetryMode colliderSymmetryMode = ColliderSymmetryMode.None;
						if (!((activeSymmetryMode.GetValueOrDefault() == colliderSymmetryMode) & (activeSymmetryMode != null)))
						{
							stringBuilder.Append(string.Format("  Symmetry:{0}", colliderComponent.ActiveSymmetryMode));
						}
						stringBuilder.AppendLine();
					}
					else
					{
						stringBuilder.AppendLine("  (null!)");
					}
				}
			}
			stringBuilder.AppendLine();
			Debug.Log(stringBuilder.ToString());
			allsb.Append(stringBuilder);
		}

		// Token: 0x0400041B RID: 1051
		public ExNativeArray<short> teamIdArray;

		// Token: 0x0400041C RID: 1052
		public const ushort Flag_Valid = 256;

		// Token: 0x0400041D RID: 1053
		public const ushort Flag_Enable = 512;

		// Token: 0x0400041E RID: 1054
		public const ushort Flag_Reset = 1024;

		// Token: 0x0400041F RID: 1055
		public const ushort Flag_Reverse = 2048;

		// Token: 0x04000420 RID: 1056
		public const ushort Flag_Symmetry = 4096;

		// Token: 0x04000421 RID: 1057
		public const ushort Flag_SymmetryReverse = 8192;

		// Token: 0x04000422 RID: 1058
		public const ushort Flag_ScaleSuspend = 16384;

		// Token: 0x04000423 RID: 1059
		public ExNativeArray<ExBitFlag16> flagArray;

		// Token: 0x04000424 RID: 1060
		public ExNativeArray<float3> centerArray;

		// Token: 0x04000425 RID: 1061
		public ExNativeArray<float3> sizeArray;

		// Token: 0x04000426 RID: 1062
		public ExNativeArray<float3> framePositions;

		// Token: 0x04000427 RID: 1063
		public ExNativeArray<quaternion> frameRotations;

		// Token: 0x04000428 RID: 1064
		public ExNativeArray<float3> frameScales;

		// Token: 0x04000429 RID: 1065
		public ExNativeArray<float3> oldFramePositions;

		// Token: 0x0400042A RID: 1066
		public ExNativeArray<quaternion> oldFrameRotations;

		// Token: 0x0400042B RID: 1067
		public ExNativeArray<float3> nowPositions;

		// Token: 0x0400042C RID: 1068
		public ExNativeArray<quaternion> nowRotations;

		// Token: 0x0400042D RID: 1069
		public ExNativeArray<float3> oldPositions;

		// Token: 0x0400042E RID: 1070
		public ExNativeArray<quaternion> oldRotations;

		// Token: 0x0400042F RID: 1071
		public ExNativeArray<int> mainColliderIndices;

		// Token: 0x04000430 RID: 1072
		private HashSet<ColliderComponent> colliderSet = new HashSet<ColliderComponent>();

		// Token: 0x04000431 RID: 1073
		private bool isValid;

		// Token: 0x04000432 RID: 1074
		internal ExNativeArray<ColliderManager.WorkData> workDataArray;

		// Token: 0x02000096 RID: 150
		public enum ColliderType : byte
		{
			// Token: 0x04000434 RID: 1076
			None,
			// Token: 0x04000435 RID: 1077
			Sphere,
			// Token: 0x04000436 RID: 1078
			CapsuleX_Center,
			// Token: 0x04000437 RID: 1079
			CapsuleY_Center,
			// Token: 0x04000438 RID: 1080
			CapsuleZ_Center,
			// Token: 0x04000439 RID: 1081
			CapsuleX_Start,
			// Token: 0x0400043A RID: 1082
			CapsuleY_Start,
			// Token: 0x0400043B RID: 1083
			CapsuleZ_Start,
			// Token: 0x0400043C RID: 1084
			Plane,
			// Token: 0x0400043D RID: 1085
			Box
		}

		// Token: 0x02000097 RID: 151
		public enum SymmetryType : byte
		{
			// Token: 0x0400043F RID: 1087
			None,
			// Token: 0x04000440 RID: 1088
			X_Symmetry,
			// Token: 0x04000441 RID: 1089
			Y_Symmetry,
			// Token: 0x04000442 RID: 1090
			Z_Symmetry,
			// Token: 0x04000443 RID: 1091
			XYZ_Symmetry
		}

		// Token: 0x02000098 RID: 152
		internal struct WorkData
		{
			// Token: 0x04000444 RID: 1092
			public AABB aabb;

			// Token: 0x04000445 RID: 1093
			public float2 radius;

			// Token: 0x04000446 RID: 1094
			public float3x2 oldPos;

			// Token: 0x04000447 RID: 1095
			public float3x2 nextPos;

			// Token: 0x04000448 RID: 1096
			public quaternion inverseOldRot;

			// Token: 0x04000449 RID: 1097
			public quaternion rot;
		}
	}
}
