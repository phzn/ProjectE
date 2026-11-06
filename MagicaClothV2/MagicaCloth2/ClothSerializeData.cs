using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x0200001C RID: 28
	[Serializable]
	public class ClothSerializeData : IDataValidate, IValid, ITransform
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00006196 File Offset: 0x00004396
		public Define.Result VerificationResult
		{
			get
			{
				return this.verificationResult.Result;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00006300 File Offset: 0x00004500
		public bool IsValid()
		{
			this.verificationResult.SetResult(Define.Result.Empty);
			ClothProcess.ClothType clothType = this.clothType;
			if (clothType != ClothProcess.ClothType.MeshCloth)
			{
				if (clothType != ClothProcess.ClothType.BoneCloth && clothType != ClothProcess.ClothType.BoneSpring)
				{
					return false;
				}
				if (this.rootBones == null || this.rootBones.Count == 0)
				{
					return false;
				}
				if (this.rootBones.Count<Transform>((Transform x) => x != null) == 0)
				{
					return false;
				}
				if (this.rootBones.Distinct<Transform>().Count<Transform>() != this.rootBones.Count)
				{
					this.verificationResult.SetError(Define.Result.SerializeData_DuplicateRootBone);
					return false;
				}
			}
			else
			{
				if (this.sourceRenderers == null || this.sourceRenderers.Count == 0)
				{
					return false;
				}
				if (this.sourceRenderers.Count<Renderer>((Renderer x) => x != null) == 0)
				{
					return false;
				}
				if (this.sourceRenderers.Count > 31)
				{
					this.verificationResult.SetError(Define.Result.SerializeData_Over31Renderers);
					return false;
				}
				if (this.sourceRenderers.Distinct<Renderer>().Count<Renderer>() != this.sourceRenderers.Count)
				{
					this.verificationResult.SetError(Define.Result.SerializeData_DuplicateRenderer);
					return false;
				}
			}
			this.verificationResult.SetSuccess();
			return true;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000644C File Offset: 0x0000464C
		public void DataValidate()
		{
			this.rotationalInterpolation = Mathf.Clamp01(this.rotationalInterpolation);
			this.rootRotation = Mathf.Clamp01(this.rootRotation);
			this.animationPoseRatio = Mathf.Clamp01(this.animationPoseRatio);
			this.reductionSetting.DataValidate();
			this.customSkinningSetting.DataValidate();
			this.normalAlignmentSetting.DataValidate();
			this.cullingSettings.DataValidate();
			this.gravity = Mathf.Clamp(this.gravity, 0f, 20f);
			if (math.length(this.gravityDirection) > 1E-08f)
			{
				this.gravityDirection = math.normalize(this.gravityDirection);
			}
			else
			{
				this.gravityDirection = 0;
			}
			this.gravityFalloff = Mathf.Clamp01(this.gravityFalloff);
			this.stablizationTimeAfterReset = Mathf.Clamp01(this.stablizationTimeAfterReset);
			this.blendWeight = Mathf.Clamp01(this.blendWeight);
			this.damping.DataValidate(0f, 1f);
			this.radius.DataValidate(0.001f, 1f);
			this.inertiaConstraint.DataValidate();
			this.tetherConstraint.DataValidate();
			this.distanceConstraint.DataValidate();
			this.triangleBendingConstraint.DataValidate();
			this.angleRestorationConstraint.DataValidate();
			this.angleLimitConstraint.DataValidate();
			this.motionConstraint.DataValidate();
			this.colliderCollisionConstraint.DataValidate();
			this.selfCollisionConstraint.DataValidate();
			this.wind.DataValidate();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000065D0 File Offset: 0x000047D0
		public override int GetHashCode()
		{
			int num = 0;
			num = (int)(num + this.clothType);
			foreach (Renderer renderer in this.sourceRenderers)
			{
				num += ((renderer != null) ? renderer.GetInstanceID() : (-3910836));
			}
			foreach (Transform transform in this.rootBones)
			{
				Stack<Transform> stack = new Stack<Transform>(30);
				stack.Push(transform);
				while (stack.Count > 0)
				{
					Transform transform2 = stack.Pop();
					if (transform2 == null)
					{
						num += -3910836;
					}
					else
					{
						num += transform2.GetInstanceID();
						num += transform2.localPosition.GetHashCode();
						num += transform2.localRotation.GetHashCode();
						int childCount = transform2.childCount;
						for (int i = 0; i < childCount; i++)
						{
							stack.Push(transform2.GetChild(i));
						}
					}
				}
			}
			num = (int)(num + this.connectionMode * (RenderSetupData.BoneConnectionMode)10);
			num += this.reductionSetting.GetHashCode();
			num += this.customSkinningSetting.GetHashCode();
			num += this.normalAlignmentSetting.GetHashCode();
			num += this.cullingSettings.GetHashCode();
			num = (int)(num + this.paintMode);
			foreach (Texture2D texture2D in this.paintMaps)
			{
				if (texture2D)
				{
					num += texture2D.GetInstanceID();
					num += (texture2D.isReadable ? 1 : 0);
				}
			}
			num += this.paintMapUvChannel * 123;
			num += this.colliderCollisionConstraint.GetHashCode();
			return num;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000067F4 File Offset: 0x000049F4
		public ClothParameters GetClothParameters()
		{
			ClothParameters clothParameters = default(ClothParameters);
			clothParameters.gravity = ((this.clothType == ClothProcess.ClothType.BoneSpring) ? 0f : this.gravity);
			clothParameters.worldGravityDirection = this.gravityDirection;
			clothParameters.gravityFalloff = this.gravityFalloff;
			clothParameters.stablizationTimeAfterReset = this.stablizationTimeAfterReset;
			clothParameters.blendWeight = this.blendWeight;
			clothParameters.dampingCurveData = this.damping.ConvertFloatArray() * 0.2f;
			clothParameters.radiusCurveData = this.radius.ConvertFloatArray();
			clothParameters.normalAxis = this.normalAxis;
			clothParameters.rotationalInterpolation = this.rotationalInterpolation;
			clothParameters.rootRotation = this.rootRotation;
			clothParameters.culling.Convert(this.cullingSettings);
			clothParameters.inertiaConstraint.Convert(this.inertiaConstraint);
			clothParameters.tetherConstraint.Convert(this.tetherConstraint, this.clothType);
			clothParameters.distanceConstraint.Convert(this.distanceConstraint, this.clothType);
			clothParameters.triangleBendingConstraint.Convert(this.triangleBendingConstraint);
			clothParameters.angleConstraint.Convert(this.angleRestorationConstraint, this.angleLimitConstraint);
			clothParameters.motionConstraint.Convert(this.motionConstraint, this.clothType);
			clothParameters.colliderCollisionConstraint.Convert(this.colliderCollisionConstraint, this.clothType);
			clothParameters.selfCollisionConstraint.Convert(this.selfCollisionConstraint, this.clothType);
			clothParameters.wind.Convert(this.wind, this.clothType);
			clothParameters.springConstraint.Convert(this.springConstraint, this.clothType);
			return clothParameters;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000069A7 File Offset: 0x00004BA7
		public string ExportJson()
		{
			return JsonUtility.ToJson(this);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000069B0 File Offset: 0x00004BB0
		public bool ImportJson(string json)
		{
			bool flag;
			try
			{
				ClothSerializeData.TempBuffer tempBuffer = new ClothSerializeData.TempBuffer(this);
				JsonUtility.FromJsonOverwrite(json, this);
				tempBuffer.Pop(this);
				this.DataValidate();
				flag = true;
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000069F4 File Offset: 0x00004BF4
		public void Import(ClothSerializeData sdata, bool deepCopy = false)
		{
			ClothSerializeData.TempBuffer tempBuffer = (deepCopy ? null : new ClothSerializeData.TempBuffer(this));
			if (deepCopy)
			{
				this.clothType = sdata.clothType;
				this.sourceRenderers = new List<Renderer>(sdata.sourceRenderers);
				this.paintMode = sdata.paintMode;
				this.paintMaps = new List<Texture2D>(sdata.paintMaps);
				this.paintMapUvChannel = sdata.paintMapUvChannel;
				this.rootBones = new List<Transform>(sdata.rootBones);
				this.connectionMode = sdata.connectionMode;
				this.rotationalInterpolation = sdata.rotationalInterpolation;
				this.rootRotation = sdata.rootRotation;
				this.updateMode = sdata.updateMode;
				this.animationPoseRatio = sdata.animationPoseRatio;
				this.reductionSetting = sdata.reductionSetting.Clone();
				this.customSkinningSetting = sdata.customSkinningSetting.Clone();
				this.normalAlignmentSetting = sdata.normalAlignmentSetting.Clone();
				this.normalAxis = sdata.normalAxis;
				this.stablizationTimeAfterReset = sdata.stablizationTimeAfterReset;
				this.blendWeight = sdata.blendWeight;
				this.cullingSettings = sdata.cullingSettings.Clone();
			}
			this.gravity = sdata.gravity;
			this.gravityDirection = sdata.gravityDirection;
			this.gravityFalloff = sdata.gravityFalloff;
			this.damping = sdata.damping.Clone();
			this.radius = sdata.radius.Clone();
			this.inertiaConstraint = sdata.inertiaConstraint.Clone();
			this.tetherConstraint = sdata.tetherConstraint.Clone();
			this.distanceConstraint = sdata.distanceConstraint.Clone();
			this.triangleBendingConstraint = sdata.triangleBendingConstraint.Clone();
			this.angleRestorationConstraint = sdata.angleRestorationConstraint.Clone();
			this.angleLimitConstraint = sdata.angleLimitConstraint.Clone();
			this.motionConstraint = sdata.motionConstraint.Clone();
			this.colliderCollisionConstraint = sdata.colliderCollisionConstraint.Clone();
			this.selfCollisionConstraint = sdata.selfCollisionConstraint.Clone();
			this.wind = sdata.wind.Clone();
			if (!deepCopy)
			{
				tempBuffer.Pop(this);
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00006C09 File Offset: 0x00004E09
		public void Import(MagicaCloth src, bool deepCopy = false)
		{
			this.Import(src.SerializeData, deepCopy);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006C18 File Offset: 0x00004E18
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			foreach (Transform transform in this.rootBones)
			{
				if (transform)
				{
					transformSet.Add(transform);
				}
			}
			this.customSkinningSetting.GetUsedTransform(transformSet);
			this.normalAlignmentSetting.GetUsedTransform(transformSet);
			this.colliderCollisionConstraint.GetUsedTransform(transformSet);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00006C98 File Offset: 0x00004E98
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			for (int i = 0; i < this.rootBones.Count; i++)
			{
				Transform transform = this.rootBones[i];
				if (transform && replaceDict.ContainsKey(transform.GetInstanceID()))
				{
					this.rootBones[i] = replaceDict[transform.GetInstanceID()];
				}
			}
			this.customSkinningSetting.ReplaceTransform(replaceDict);
			this.normalAlignmentSetting.ReplaceTransform(replaceDict);
			this.colliderCollisionConstraint.ReplaceTransform(replaceDict);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00006D1A File Offset: 0x00004F1A
		public bool IsBoneSpring()
		{
			return this.clothType == ClothProcess.ClothType.BoneSpring;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00006D28 File Offset: 0x00004F28
		public int GetUvChannel()
		{
			ClothSerializeData.PaintMode paintMode = this.paintMode;
			if (paintMode - ClothSerializeData.PaintMode.Texture_Fixed_Move <= 1)
			{
				return this.paintMapUvChannel;
			}
			return 0;
		}

		// Token: 0x040000A1 RID: 161
		public ClothProcess.ClothType clothType;

		// Token: 0x040000A2 RID: 162
		public List<Renderer> sourceRenderers = new List<Renderer>();

		// Token: 0x040000A3 RID: 163
		public ClothMeshWriteMode meshWriteMode;

		// Token: 0x040000A4 RID: 164
		public ClothSerializeData.PaintMode paintMode;

		// Token: 0x040000A5 RID: 165
		public List<Texture2D> paintMaps = new List<Texture2D>();

		// Token: 0x040000A6 RID: 166
		[Range(0f, 7f)]
		public int paintMapUvChannel;

		// Token: 0x040000A7 RID: 167
		public List<Transform> rootBones = new List<Transform>();

		// Token: 0x040000A8 RID: 168
		public RenderSetupData.BoneConnectionMode connectionMode;

		// Token: 0x040000A9 RID: 169
		[Range(0f, 1f)]
		public float rotationalInterpolation = 0.5f;

		// Token: 0x040000AA RID: 170
		[Range(0f, 1f)]
		public float rootRotation = 0.5f;

		// Token: 0x040000AB RID: 171
		public ClothUpdateMode updateMode = ClothUpdateMode.AnimatorLinkage;

		// Token: 0x040000AC RID: 172
		[Range(0f, 1f)]
		public float animationPoseRatio;

		// Token: 0x040000AD RID: 173
		public ReductionSettings reductionSetting = new ReductionSettings();

		// Token: 0x040000AE RID: 174
		public CustomSkinningSettings customSkinningSetting = new CustomSkinningSettings();

		// Token: 0x040000AF RID: 175
		public NormalAlignmentSettings normalAlignmentSetting = new NormalAlignmentSettings();

		// Token: 0x040000B0 RID: 176
		public CullingSettings cullingSettings = new CullingSettings();

		// Token: 0x040000B1 RID: 177
		public ClothNormalAxis normalAxis = ClothNormalAxis.Up;

		// Token: 0x040000B2 RID: 178
		[Range(0f, 10f)]
		public float gravity = 5f;

		// Token: 0x040000B3 RID: 179
		public float3 gravityDirection = new float3(0f, -1f, 0f);

		// Token: 0x040000B4 RID: 180
		[Range(0f, 1f)]
		public float gravityFalloff;

		// Token: 0x040000B5 RID: 181
		[Range(0f, 1f)]
		public float stablizationTimeAfterReset = 0.1f;

		// Token: 0x040000B6 RID: 182
		[Range(0f, 1f)]
		public float blendWeight = 1f;

		// Token: 0x040000B7 RID: 183
		public CurveSerializeData damping = new CurveSerializeData(0.05f);

		// Token: 0x040000B8 RID: 184
		public CurveSerializeData radius = new CurveSerializeData(0.02f);

		// Token: 0x040000B9 RID: 185
		public InertiaConstraint.SerializeData inertiaConstraint = new InertiaConstraint.SerializeData();

		// Token: 0x040000BA RID: 186
		public TetherConstraint.SerializeData tetherConstraint = new TetherConstraint.SerializeData();

		// Token: 0x040000BB RID: 187
		public DistanceConstraint.SerializeData distanceConstraint = new DistanceConstraint.SerializeData();

		// Token: 0x040000BC RID: 188
		public TriangleBendingConstraint.SerializeData triangleBendingConstraint = new TriangleBendingConstraint.SerializeData();

		// Token: 0x040000BD RID: 189
		public AngleConstraint.RestorationSerializeData angleRestorationConstraint = new AngleConstraint.RestorationSerializeData();

		// Token: 0x040000BE RID: 190
		public AngleConstraint.LimitSerializeData angleLimitConstraint = new AngleConstraint.LimitSerializeData();

		// Token: 0x040000BF RID: 191
		public MotionConstraint.SerializeData motionConstraint = new MotionConstraint.SerializeData();

		// Token: 0x040000C0 RID: 192
		public ColliderCollisionConstraint.SerializeData colliderCollisionConstraint = new ColliderCollisionConstraint.SerializeData();

		// Token: 0x040000C1 RID: 193
		public SelfCollisionConstraint.SerializeData selfCollisionConstraint = new SelfCollisionConstraint.SerializeData();

		// Token: 0x040000C2 RID: 194
		public WindSettings wind = new WindSettings();

		// Token: 0x040000C3 RID: 195
		public SpringConstraint.SerializeData springConstraint = new SpringConstraint.SerializeData();

		// Token: 0x040000C4 RID: 196
		private ResultCode verificationResult;

		// Token: 0x0200001D RID: 29
		public enum PaintMode
		{
			// Token: 0x040000C6 RID: 198
			Manual,
			// Token: 0x040000C7 RID: 199
			[InspectorName("Texture Fixed(RD) Move(GR) Ignore(BK)")]
			Texture_Fixed_Move,
			// Token: 0x040000C8 RID: 200
			[InspectorName("Texture Fixed(RD) Move(GR) Limit(BL) Ignore(BK)")]
			Texture_Fixed_Move_Limit
		}

		// Token: 0x0200001E RID: 30
		private class TempBuffer
		{
			// Token: 0x06000086 RID: 134 RVA: 0x00006D4A File Offset: 0x00004F4A
			internal TempBuffer(ClothSerializeData sdata)
			{
				this.Push(sdata);
			}

			// Token: 0x06000087 RID: 135 RVA: 0x00006D5C File Offset: 0x00004F5C
			internal void Push(ClothSerializeData sdata)
			{
				this.clothType = sdata.clothType;
				this.sourceRenderers = new List<Renderer>(sdata.sourceRenderers);
				this.meshWriteMode = sdata.meshWriteMode;
				this.paintMode = sdata.paintMode;
				this.paintMaps = new List<Texture2D>(sdata.paintMaps);
				this.paintMapUvChannel = sdata.paintMapUvChannel;
				this.rootBones = new List<Transform>(sdata.rootBones);
				this.connectionMode = sdata.connectionMode;
				this.rotationalInterpolation = sdata.rotationalInterpolation;
				this.rootRotation = sdata.rootRotation;
				this.updateMode = sdata.updateMode;
				this.animationPoseRatio = sdata.animationPoseRatio;
				this.reductionSetting = sdata.reductionSetting.Clone();
				this.customSkinningSetting = sdata.customSkinningSetting.Clone();
				this.normalAlignmentSetting = sdata.normalAlignmentSetting.Clone();
				this.normalAxis = sdata.normalAxis;
				this.colliderList = new List<ColliderComponent>(sdata.colliderCollisionConstraint.colliderList);
				this.collisionBones = new List<Transform>(sdata.colliderCollisionConstraint.collisionBones);
				this.synchronization = sdata.selfCollisionConstraint.syncPartner;
				this.stablizationTimeAfterReset = sdata.stablizationTimeAfterReset;
				this.blendWeight = sdata.blendWeight;
				this.cullingSetting = sdata.cullingSettings.Clone();
				this.anchor = sdata.inertiaConstraint.anchor;
				this.anchorInertia = sdata.inertiaConstraint.anchorInertia;
			}

			// Token: 0x06000088 RID: 136 RVA: 0x00006ED0 File Offset: 0x000050D0
			internal void Pop(ClothSerializeData sdata)
			{
				sdata.clothType = this.clothType;
				sdata.sourceRenderers = this.sourceRenderers;
				sdata.meshWriteMode = this.meshWriteMode;
				sdata.paintMode = this.paintMode;
				sdata.paintMaps = this.paintMaps;
				sdata.paintMapUvChannel = this.paintMapUvChannel;
				sdata.rootBones = this.rootBones;
				sdata.connectionMode = this.connectionMode;
				sdata.rotationalInterpolation = this.rotationalInterpolation;
				sdata.rootRotation = this.rootRotation;
				sdata.updateMode = this.updateMode;
				sdata.animationPoseRatio = this.animationPoseRatio;
				sdata.reductionSetting = this.reductionSetting;
				sdata.customSkinningSetting = this.customSkinningSetting;
				sdata.normalAlignmentSetting = this.normalAlignmentSetting;
				sdata.normalAxis = this.normalAxis;
				sdata.colliderCollisionConstraint.colliderList = this.colliderList;
				sdata.colliderCollisionConstraint.collisionBones = this.collisionBones;
				sdata.selfCollisionConstraint.syncPartner = this.synchronization;
				sdata.stablizationTimeAfterReset = this.stablizationTimeAfterReset;
				sdata.blendWeight = this.blendWeight;
				sdata.cullingSettings = this.cullingSetting;
				sdata.inertiaConstraint.anchor = this.anchor;
				sdata.inertiaConstraint.anchorInertia = this.anchorInertia;
			}

			// Token: 0x040000C9 RID: 201
			private ClothProcess.ClothType clothType;

			// Token: 0x040000CA RID: 202
			private List<Renderer> sourceRenderers;

			// Token: 0x040000CB RID: 203
			private ClothMeshWriteMode meshWriteMode;

			// Token: 0x040000CC RID: 204
			private ClothSerializeData.PaintMode paintMode;

			// Token: 0x040000CD RID: 205
			private List<Texture2D> paintMaps;

			// Token: 0x040000CE RID: 206
			private int paintMapUvChannel;

			// Token: 0x040000CF RID: 207
			private List<Transform> rootBones;

			// Token: 0x040000D0 RID: 208
			private RenderSetupData.BoneConnectionMode connectionMode;

			// Token: 0x040000D1 RID: 209
			private float rotationalInterpolation;

			// Token: 0x040000D2 RID: 210
			private float rootRotation;

			// Token: 0x040000D3 RID: 211
			private ClothUpdateMode updateMode;

			// Token: 0x040000D4 RID: 212
			private float animationPoseRatio;

			// Token: 0x040000D5 RID: 213
			private ReductionSettings reductionSetting;

			// Token: 0x040000D6 RID: 214
			private CustomSkinningSettings customSkinningSetting;

			// Token: 0x040000D7 RID: 215
			private NormalAlignmentSettings normalAlignmentSetting;

			// Token: 0x040000D8 RID: 216
			private ClothNormalAxis normalAxis;

			// Token: 0x040000D9 RID: 217
			private List<ColliderComponent> colliderList;

			// Token: 0x040000DA RID: 218
			private List<Transform> collisionBones;

			// Token: 0x040000DB RID: 219
			private MagicaCloth synchronization;

			// Token: 0x040000DC RID: 220
			private float stablizationTimeAfterReset;

			// Token: 0x040000DD RID: 221
			private float blendWeight;

			// Token: 0x040000DE RID: 222
			private CullingSettings cullingSetting;

			// Token: 0x040000DF RID: 223
			private Transform anchor;

			// Token: 0x040000E0 RID: 224
			private float anchorInertia;
		}
	}
}
