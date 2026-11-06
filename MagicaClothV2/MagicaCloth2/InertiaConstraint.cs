using System;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace MagicaCloth2
{
	// Token: 0x02000036 RID: 54
	public class InertiaConstraint : IDisposable
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x0000A29F File Offset: 0x0000849F
		public InertiaConstraint()
		{
			this.fixedArray = new ExNativeArray<ushort>(0, true);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000A2B4 File Offset: 0x000084B4
		public void Dispose()
		{
			ExNativeArray<ushort> exNativeArray = this.fixedArray;
			if (exNativeArray != null)
			{
				exNativeArray.Dispose();
			}
			this.fixedArray = null;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000A2CE File Offset: 0x000084CE
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("[InertiaConstraint]");
			stringBuilder.AppendLine("  -fixedArray:" + this.fixedArray.ToSummary());
			return stringBuilder.ToString();
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000A304 File Offset: 0x00008504
		public static InertiaConstraint.ConstraintData CreateData(VirtualMesh proxyMesh, in ClothParameters parameters)
		{
			InertiaConstraint.ConstraintData constraintData = new InertiaConstraint.ConstraintData();
			try
			{
				InertiaConstraint.CenterData centerData = default(InertiaConstraint.CenterData);
				centerData.Initialize();
				centerData.centerTransformIndex = proxyMesh.centerTransformIndex;
				constraintData.centerData = centerData;
				float3 @float = 0;
				float3 float2 = 0;
				int centerFixedPointCount = proxyMesh.CenterFixedPointCount;
				if (centerFixedPointCount > 0)
				{
					for (int i = 0; i < centerFixedPointCount; i++)
					{
						int num = (int)proxyMesh.centerFixedList[i];
						float3 float3 = proxyMesh.localNormals[num];
						float3 float4 = proxyMesh.localTangents[num];
						quaternion quaternion = MathUtility.ToRotation(in float3, in float4);
						quaternion quaternion2 = proxyMesh.vertexBindPoseRotations[num];
						quaternion quaternion3 = math.mul(quaternion, quaternion2);
						@float += MathUtility.ToNormal(in quaternion3);
						float2 += MathUtility.ToTangent(in quaternion3);
					}
				}
				float3 float5 = new float3(0f, -1f, 0f);
				if (centerFixedPointCount > 0)
				{
					float3 float6 = math.normalize(@float);
					float3 float7 = math.normalize(float2);
					float5 = math.mul(math.inverse(MathUtility.ToRotation(in float6, in float7)), parameters.worldGravityDirection);
				}
				constraintData.initLocalGravityDirection = float5;
				constraintData.result.SetSuccess();
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				constraintData.result.SetError(Define.Result.Constraint_CreateInertiaException);
				throw;
			}
			return constraintData;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000A45C File Offset: 0x0000865C
		internal void Register(ClothProcess cprocess)
		{
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(cprocess.TeamId);
			ref InertiaConstraint.CenterData @ref = ref MagicaManager.Team.centerDataArray.GetRef(cprocess.TeamId);
			@ref.centerTransformIndex = teamDataRef.centerTransformIndex;
			@ref.initLocalGravityDirection = cprocess.inertiaConstraintData.initLocalGravityDirection;
			DataChunk dataChunk = default(DataChunk);
			if (cprocess.ProxyMeshContainer.shareVirtualMesh.CenterFixedPointCount > 0)
			{
				dataChunk = this.fixedArray.AddRange(cprocess.ProxyMeshContainer.shareVirtualMesh.centerFixedList);
			}
			teamDataRef.fixedDataChunk = dataChunk;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000A4EC File Offset: 0x000086EC
		internal void Exit(ClothProcess cprocess)
		{
			if (cprocess != null && cprocess.TeamId > 0)
			{
				ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(cprocess.TeamId);
				this.fixedArray.Remove(teamDataRef.fixedDataChunk);
				teamDataRef.fixedDataChunk.Clear();
			}
		}

		// Token: 0x0400012D RID: 301
		internal ExNativeArray<ushort> fixedArray;

		// Token: 0x02000037 RID: 55
		public enum TeleportMode
		{
			// Token: 0x0400012F RID: 303
			None,
			// Token: 0x04000130 RID: 304
			Reset,
			// Token: 0x04000131 RID: 305
			Keep
		}

		// Token: 0x02000038 RID: 56
		[Serializable]
		public class SerializeData : IDataValidate
		{
			// Token: 0x060000F6 RID: 246 RVA: 0x0000A534 File Offset: 0x00008734
			public SerializeData()
			{
				this.anchor = null;
				this.anchorInertia = 0f;
				this.worldInertia = 1f;
				this.movementInertiaSmoothing = 0.4f;
				this.movementSpeedLimit = new CheckSliderSerializeData(true, 5f);
				this.rotationSpeedLimit = new CheckSliderSerializeData(true, 720f);
				this.localInertia = 1f;
				this.localMovementSpeedLimit = new CheckSliderSerializeData(false, 5f);
				this.localRotationSpeedLimit = new CheckSliderSerializeData(false, 720f);
				this.depthInertia = 0f;
				this.centrifualAcceleration = 0f;
				this.particleSpeedLimit = new CheckSliderSerializeData(true, 4f);
				this.teleportMode = InertiaConstraint.TeleportMode.None;
				this.teleportDistance = 0.5f;
				this.teleportRotation = 90f;
			}

			// Token: 0x060000F7 RID: 247 RVA: 0x0000A604 File Offset: 0x00008804
			public InertiaConstraint.SerializeData Clone()
			{
				return new InertiaConstraint.SerializeData
				{
					anchor = this.anchor,
					anchorInertia = this.anchorInertia,
					worldInertia = this.worldInertia,
					movementInertiaSmoothing = this.movementInertiaSmoothing,
					movementSpeedLimit = this.movementSpeedLimit.Clone(),
					rotationSpeedLimit = this.rotationSpeedLimit.Clone(),
					localInertia = this.localInertia,
					localMovementSpeedLimit = this.localMovementSpeedLimit.Clone(),
					localRotationSpeedLimit = this.localRotationSpeedLimit.Clone(),
					depthInertia = this.depthInertia,
					centrifualAcceleration = this.centrifualAcceleration,
					particleSpeedLimit = this.particleSpeedLimit.Clone(),
					teleportMode = this.teleportMode,
					teleportDistance = this.teleportDistance,
					teleportRotation = this.teleportRotation
				};
			}

			// Token: 0x060000F8 RID: 248 RVA: 0x0000A6E4 File Offset: 0x000088E4
			public void DataValidate()
			{
				this.anchorInertia = Mathf.Clamp01(this.anchorInertia);
				this.worldInertia = Mathf.Clamp01(this.worldInertia);
				this.movementInertiaSmoothing = Mathf.Clamp01(this.movementInertiaSmoothing);
				this.movementSpeedLimit.DataValidate(0f, 10f);
				this.rotationSpeedLimit.DataValidate(0f, 1440f);
				this.localInertia = Mathf.Clamp01(this.localInertia);
				this.localMovementSpeedLimit.DataValidate(0f, 10f);
				this.localRotationSpeedLimit.DataValidate(0f, 1440f);
				this.centrifualAcceleration = Mathf.Clamp01(this.centrifualAcceleration);
				this.depthInertia = Mathf.Clamp01(this.depthInertia);
				this.particleSpeedLimit.DataValidate(0f, 10f);
				this.teleportDistance = Mathf.Max(this.teleportDistance, 0f);
				this.teleportRotation = Mathf.Max(this.teleportRotation, 0f);
			}

			// Token: 0x04000132 RID: 306
			public Transform anchor;

			// Token: 0x04000133 RID: 307
			[Range(0f, 1f)]
			public float anchorInertia;

			// Token: 0x04000134 RID: 308
			[FormerlySerializedAs("movementInertia")]
			[Range(0f, 1f)]
			public float worldInertia;

			// Token: 0x04000135 RID: 309
			[Range(0f, 1f)]
			public float movementInertiaSmoothing;

			// Token: 0x04000136 RID: 310
			public CheckSliderSerializeData movementSpeedLimit;

			// Token: 0x04000137 RID: 311
			public CheckSliderSerializeData rotationSpeedLimit;

			// Token: 0x04000138 RID: 312
			[Range(0f, 1f)]
			public float localInertia;

			// Token: 0x04000139 RID: 313
			public CheckSliderSerializeData localMovementSpeedLimit;

			// Token: 0x0400013A RID: 314
			public CheckSliderSerializeData localRotationSpeedLimit;

			// Token: 0x0400013B RID: 315
			[Range(0f, 1f)]
			public float depthInertia;

			// Token: 0x0400013C RID: 316
			[Range(0f, 1f)]
			public float centrifualAcceleration;

			// Token: 0x0400013D RID: 317
			public CheckSliderSerializeData particleSpeedLimit;

			// Token: 0x0400013E RID: 318
			public InertiaConstraint.TeleportMode teleportMode;

			// Token: 0x0400013F RID: 319
			public float teleportDistance;

			// Token: 0x04000140 RID: 320
			public float teleportRotation;
		}

		// Token: 0x02000039 RID: 57
		public struct InertiaConstraintParams
		{
			// Token: 0x060000F9 RID: 249 RVA: 0x0000A7EC File Offset: 0x000089EC
			public void Convert(InertiaConstraint.SerializeData sdata)
			{
				this.anchorInertia = sdata.anchorInertia;
				this.worldInertia = sdata.worldInertia;
				this.movementInertiaSmoothing = sdata.movementInertiaSmoothing;
				this.movementSpeedLimit = sdata.movementSpeedLimit.GetValue(-1f);
				this.rotationSpeedLimit = sdata.rotationSpeedLimit.GetValue(-1f);
				this.localInertia = sdata.localInertia;
				this.localMovementSpeedLimit = sdata.localMovementSpeedLimit.GetValue(-1f);
				this.localRotationSpeedLimit = sdata.localRotationSpeedLimit.GetValue(-1f);
				this.depthInertia = sdata.depthInertia;
				this.centrifualAcceleration = sdata.centrifualAcceleration;
				this.particleSpeedLimit = sdata.particleSpeedLimit.GetValue(-1f);
				this.teleportMode = sdata.teleportMode;
				this.teleportDistance = sdata.teleportDistance;
				this.teleportRotation = sdata.teleportRotation;
			}

			// Token: 0x04000141 RID: 321
			public float anchorInertia;

			// Token: 0x04000142 RID: 322
			public float worldInertia;

			// Token: 0x04000143 RID: 323
			public float movementInertiaSmoothing;

			// Token: 0x04000144 RID: 324
			public float movementSpeedLimit;

			// Token: 0x04000145 RID: 325
			public float rotationSpeedLimit;

			// Token: 0x04000146 RID: 326
			public float localInertia;

			// Token: 0x04000147 RID: 327
			public float localMovementSpeedLimit;

			// Token: 0x04000148 RID: 328
			public float localRotationSpeedLimit;

			// Token: 0x04000149 RID: 329
			public float depthInertia;

			// Token: 0x0400014A RID: 330
			public float centrifualAcceleration;

			// Token: 0x0400014B RID: 331
			public float particleSpeedLimit;

			// Token: 0x0400014C RID: 332
			public InertiaConstraint.TeleportMode teleportMode;

			// Token: 0x0400014D RID: 333
			public float teleportDistance;

			// Token: 0x0400014E RID: 334
			public float teleportRotation;
		}

		// Token: 0x0200003A RID: 58
		[Serializable]
		public struct CenterData
		{
			// Token: 0x060000FA RID: 250 RVA: 0x0000A8D4 File Offset: 0x00008AD4
			internal void Initialize()
			{
				this.anchorRotation = quaternion.identity;
				this.oldAnchorRotation = quaternion.identity;
				this.componentWorldRotation = quaternion.identity;
				this.componentWorldScale = 1;
				this.oldComponentWorldRotation = quaternion.identity;
				this.oldComponentWorldScale = 1;
				this.frameComponentShiftRotation = quaternion.identity;
				this.frameWorldRotation = quaternion.identity;
				this.oldFrameWorldRotation = quaternion.identity;
				this.nowWorldRotation = quaternion.identity;
				this.oldWorldRotation = quaternion.identity;
				this.stepRotation = quaternion.identity;
			}

			// Token: 0x0400014F RID: 335
			public float3 anchorPosition;

			// Token: 0x04000150 RID: 336
			public quaternion anchorRotation;

			// Token: 0x04000151 RID: 337
			public float3 oldAnchorPosition;

			// Token: 0x04000152 RID: 338
			public quaternion oldAnchorRotation;

			// Token: 0x04000153 RID: 339
			public float3 anchorComponentLocalPosition;

			// Token: 0x04000154 RID: 340
			public int centerTransformIndex;

			// Token: 0x04000155 RID: 341
			public float3 componentWorldPosition;

			// Token: 0x04000156 RID: 342
			public quaternion componentWorldRotation;

			// Token: 0x04000157 RID: 343
			public float3 componentWorldScale;

			// Token: 0x04000158 RID: 344
			public float3 oldComponentWorldPosition;

			// Token: 0x04000159 RID: 345
			public quaternion oldComponentWorldRotation;

			// Token: 0x0400015A RID: 346
			public float3 oldComponentWorldScale;

			// Token: 0x0400015B RID: 347
			public float3 frameComponentShiftVector;

			// Token: 0x0400015C RID: 348
			public quaternion frameComponentShiftRotation;

			// Token: 0x0400015D RID: 349
			public float frameMovingSpeed;

			// Token: 0x0400015E RID: 350
			public float3 frameMovingDirection;

			// Token: 0x0400015F RID: 351
			public float3 frameWorldPosition;

			// Token: 0x04000160 RID: 352
			public quaternion frameWorldRotation;

			// Token: 0x04000161 RID: 353
			public float3 frameWorldScale;

			// Token: 0x04000162 RID: 354
			public float3 frameLocalPosition;

			// Token: 0x04000163 RID: 355
			public float3 oldFrameWorldPosition;

			// Token: 0x04000164 RID: 356
			public quaternion oldFrameWorldRotation;

			// Token: 0x04000165 RID: 357
			public float3 oldFrameWorldScale;

			// Token: 0x04000166 RID: 358
			public float3 nowWorldPosition;

			// Token: 0x04000167 RID: 359
			public quaternion nowWorldRotation;

			// Token: 0x04000168 RID: 360
			public float3 oldWorldPosition;

			// Token: 0x04000169 RID: 361
			public quaternion oldWorldRotation;

			// Token: 0x0400016A RID: 362
			public float stepMoveInertiaRatio;

			// Token: 0x0400016B RID: 363
			public float stepRotationInertiaRatio;

			// Token: 0x0400016C RID: 364
			public float3 stepVector;

			// Token: 0x0400016D RID: 365
			public quaternion stepRotation;

			// Token: 0x0400016E RID: 366
			public float3 inertiaVector;

			// Token: 0x0400016F RID: 367
			public quaternion inertiaRotation;

			// Token: 0x04000170 RID: 368
			public float stepMovingSpeed;

			// Token: 0x04000171 RID: 369
			public float3 stepMovingDirection;

			// Token: 0x04000172 RID: 370
			public float angularVelocity;

			// Token: 0x04000173 RID: 371
			public float3 rotationAxis;

			// Token: 0x04000174 RID: 372
			public float3 initLocalGravityDirection;

			// Token: 0x04000175 RID: 373
			public float3 smoothingVelocity;

			// Token: 0x04000176 RID: 374
			public float4x4 negativeScaleMatrix;
		}

		// Token: 0x0200003B RID: 59
		[Serializable]
		public class ConstraintData
		{
			// Token: 0x04000177 RID: 375
			public ResultCode result;

			// Token: 0x04000178 RID: 376
			public InertiaConstraint.CenterData centerData;

			// Token: 0x04000179 RID: 377
			public float3 initLocalGravityDirection;
		}
	}
}
