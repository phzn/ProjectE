using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000066 RID: 102
	[AddComponentMenu("MagicaCloth2/MagicaCloth")]
	[HelpURL("https://magicasoft.jp/en/mc2_magicaclothcomponent/")]
	public class MagicaCloth : ClothBehaviour, IValid
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000F194 File Offset: 0x0000D394
		public ClothSerializeData SerializeData
		{
			get
			{
				return this.serializeData;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0000F19C File Offset: 0x0000D39C
		public ClothProcess Process
		{
			get
			{
				this.process.cloth = this;
				return this.process;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
		public Transform ClothTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600016E RID: 366 RVA: 0x0000F1B8 File Offset: 0x0000D3B8
		public MagicaCloth SyncPartnerCloth
		{
			get
			{
				MagicaCloth magicaCloth = (this.SerializeData.IsBoneSpring() ? null : this.SerializeData.selfCollisionConstraint.GetSyncPartner());
				if (!(magicaCloth == this))
				{
					return magicaCloth;
				}
				return null;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000F1F2 File Offset: 0x0000D3F2
		public bool IsValid()
		{
			return MagicaManager.IsPlaying() && this.Process.IsValid() && this.Process.TeamId > 0;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000706D File Offset: 0x0000526D
		protected void Reset()
		{
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000F218 File Offset: 0x0000D418
		protected void OnValidate()
		{
			this.Process.DataUpdate();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000F225 File Offset: 0x0000D425
		protected void Awake()
		{
			if (MagicaManager.initializationLocation == MagicaManager.InitializationLocation.Awake)
			{
				this.Process.Init();
				MagicaManager.Team.RemoveMonitoringProcess(this.Process);
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000F24A File Offset: 0x0000D44A
		protected void OnEnable()
		{
			this.Process.StartUse();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000F257 File Offset: 0x0000D457
		protected void OnDisable()
		{
			this.Process.EndUse();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000F264 File Offset: 0x0000D464
		protected void Start()
		{
			if (MagicaManager.initializationLocation == MagicaManager.InitializationLocation.Start)
			{
				this.Process.Init();
				MagicaManager.Team.RemoveMonitoringProcess(this.Process);
			}
			this.Process.AutoBuild();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000F294 File Offset: 0x0000D494
		protected void OnDestroy()
		{
			this.Process.Dispose();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000F2A1 File Offset: 0x0000D4A1
		public override int GetMagicaHashCode()
		{
			return this.SerializeData.GetHashCode() + this.serializeData2.GetHashCode() + (base.isActiveAndEnabled ? base.GetInstanceID() : 0);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000F2CC File Offset: 0x0000D4CC
		internal void InitAnimationProperty()
		{
			this.animationPoseRatioProperty = this.serializeData.animationPoseRatio;
			this._animationPoseRatioProperty = this.animationPoseRatioProperty;
			this.gravityProperty = this.serializeData.gravity;
			this._gravityProperty = this.gravityProperty;
			this.dampingProperty = this.serializeData.damping.value;
			this._dampingProperty = this.dampingProperty;
			this.worldInertiaProperty = this.serializeData.inertiaConstraint.worldInertia;
			this._worldInertiaProperty = this.worldInertiaProperty;
			this.localInertiaProperty = this.serializeData.inertiaConstraint.localInertia;
			this._localInertiaProperty = this.localInertiaProperty;
			this.windInfluenceProperty = this.serializeData.wind.influence;
			this._windInfluenceProperty = this.windInfluenceProperty;
			this.blendWeightProperty = this.serializeData.blendWeight;
			this._blendWeightProperty = this.blendWeightProperty;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000F3B8 File Offset: 0x0000D5B8
		private void OnDidApplyAnimationProperties()
		{
			if (Application.isPlaying)
			{
				if (this.animationPoseRatioProperty != this._animationPoseRatioProperty)
				{
					this._animationPoseRatioProperty = this.animationPoseRatioProperty;
					this.serializeData.animationPoseRatio = this.animationPoseRatioProperty;
					this.SetParameterChange();
				}
				if (this.gravityProperty != this._gravityProperty)
				{
					this._gravityProperty = this.gravityProperty;
					this.serializeData.gravity = this.gravityProperty;
					this.SetParameterChange();
				}
				if (this.dampingProperty != this._dampingProperty)
				{
					this._dampingProperty = this.dampingProperty;
					this.serializeData.damping.value = this.dampingProperty;
					this.SetParameterChange();
				}
				if (this.worldInertiaProperty != this._worldInertiaProperty)
				{
					this._worldInertiaProperty = this.worldInertiaProperty;
					this.serializeData.inertiaConstraint.worldInertia = this.worldInertiaProperty;
					this.SetParameterChange();
				}
				if (this.localInertiaProperty != this._localInertiaProperty)
				{
					this._localInertiaProperty = this.localInertiaProperty;
					this.serializeData.inertiaConstraint.localInertia = this.localInertiaProperty;
					this.SetParameterChange();
				}
				if (this.windInfluenceProperty != this._windInfluenceProperty)
				{
					this._windInfluenceProperty = this.windInfluenceProperty;
					this.serializeData.wind.influence = this.windInfluenceProperty;
					this.SetParameterChange();
				}
				if (this.blendWeightProperty != this._blendWeightProperty)
				{
					this._blendWeightProperty = this.blendWeightProperty;
					this.serializeData.blendWeight = this.blendWeightProperty;
					this.SetParameterChange();
				}
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000F53A File Offset: 0x0000D73A
		public ClothSerializeData2 GetSerializeData2()
		{
			return this.serializeData2;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000F542 File Offset: 0x0000D742
		public void Initialize()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.Process.Init();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000F557 File Offset: 0x0000D757
		public void DisableAutoBuild()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.Process.SetState(7, true);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000F570 File Offset: 0x0000D770
		public bool BuildAndRun()
		{
			bool flag = false;
			bool flag2 = true;
			try
			{
				if (!Application.isPlaying)
				{
					throw new MagicaClothProcessingException();
				}
				this.DisableAutoBuild();
				if (this.Process.IsState(5))
				{
					object obj = "Already built.:" + base.name;
					Develop.LogError(in obj);
					throw new MagicaClothProcessingException();
				}
				if (!this.Process.GenerateInitialization())
				{
					throw new MagicaClothProcessingException();
				}
				if (!this.serializeData2.preBuildData.UsePreBuild())
				{
					ClothProcess.ClothType clothType = this.serializeData.clothType;
					if (clothType == ClothProcess.ClothType.BoneCloth || clothType == ClothProcess.ClothType.BoneSpring)
					{
						SelectionData selectionData = this.serializeData2.selectionData;
						if ((selectionData == null || !selectionData.IsValid() || !selectionData.IsUserEdit()) && !this.Process.GenerateBoneClothSelection())
						{
							throw new MagicaClothProcessingException();
						}
					}
					flag = this.Process.StartRuntimeBuild();
					if (flag)
					{
						flag2 = false;
					}
				}
				else
				{
					flag = this.Process.PreBuildDataConstruction();
				}
			}
			catch (MagicaClothProcessingException)
			{
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			finally
			{
				if (flag2)
				{
					Action<MagicaCloth, bool> onBuildComplete = this.OnBuildComplete;
					if (onBuildComplete != null)
					{
						onBuildComplete(this, flag);
					}
				}
			}
			return flag;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000F69C File Offset: 0x0000D89C
		public void ReplaceTransform(Dictionary<string, Transform> targetTransformDict)
		{
			HashSet<Transform> hashSet = new HashSet<Transform>();
			this.Process.GetUsedTransform(hashSet);
			Dictionary<int, Transform> dictionary = new Dictionary<int, Transform>();
			foreach (Transform transform in hashSet)
			{
				if (transform && targetTransformDict.ContainsKey(transform.name))
				{
					dictionary.Add(transform.GetInstanceID(), targetTransformDict[transform.name]);
				}
			}
			this.Process.ReplaceTransform(dictionary);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000F738 File Offset: 0x0000D938
		public HashSet<Transform> GetUsedTransform()
		{
			HashSet<Transform> hashSet = new HashSet<Transform>();
			this.Process.GetUsedTransform(hashSet);
			return hashSet;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000F758 File Offset: 0x0000D958
		public void SetParameterChange()
		{
			if (this.IsValid())
			{
				this.Process.DataUpdate();
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000F76D File Offset: 0x0000D96D
		public void SetTimeScale(float timeScale)
		{
			if (this.IsValid())
			{
				MagicaManager.Team.GetTeamDataRef(this.Process.TeamId).timeScale = Mathf.Clamp01(timeScale);
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000F797 File Offset: 0x0000D997
		public float GetTimeScale()
		{
			if (this.IsValid())
			{
				return MagicaManager.Team.GetTeamDataRef(this.Process.TeamId).timeScale;
			}
			return 1f;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000F7C4 File Offset: 0x0000D9C4
		public void ResetCloth(bool keepPose = false)
		{
			if (this.IsValid())
			{
				ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(this.Process.TeamId);
				if (keepPose)
				{
					teamDataRef.flag.SetBits(9, true);
					return;
				}
				teamDataRef.flag.SetBits(2, true);
				teamDataRef.flag.SetBits(3, true);
				teamDataRef.flag.SetBits(12, false);
				this.Process.SetState(9, false);
				this.Process.UpdateRendererUse();
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000F844 File Offset: 0x0000DA44
		public Vector3 GetCenterPosition()
		{
			if (this.IsValid())
			{
				ref InertiaConstraint.CenterData centerDataRef = ref MagicaManager.Team.GetCenterDataRef(this.Process.TeamId);
				return this.ClothTransform.TransformPoint(centerDataRef.frameLocalPosition);
			}
			return Vector3.zero;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000F88C File Offset: 0x0000DA8C
		public void AddForce(Vector3 forceDirection, float forceVelocity, ClothForceMode fmode = ClothForceMode.VelocityAdd)
		{
			if (this.IsValid() && forceDirection.magnitude > 0f && forceVelocity > 0f && fmode != ClothForceMode.None)
			{
				ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(this.Process.TeamId);
				teamDataRef.forceMode = fmode;
				teamDataRef.impactForce = forceDirection.normalized * forceVelocity;
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000F8ED File Offset: 0x0000DAED
		public void SetSkipWriting(bool sw)
		{
			if (this.IsValid())
			{
				this.Process.SetSkipWriting(sw);
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000F904 File Offset: 0x0000DB04
		private RenderData GetRenderData(Renderer ren)
		{
			if (!this.IsValid() || ren == null)
			{
				return null;
			}
			int instanceID = ren.GetInstanceID();
			return MagicaManager.Render.GetRendererData(instanceID);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000F938 File Offset: 0x0000DB38
		public Mesh GetOriginalMesh(Renderer ren)
		{
			RenderData renderData = this.GetRenderData(ren);
			if (renderData == null)
			{
				return null;
			}
			return renderData.originalMesh;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000F958 File Offset: 0x0000DB58
		public Mesh GetCustomMesh(Renderer ren)
		{
			RenderData renderData = this.GetRenderData(ren);
			if (renderData == null)
			{
				return null;
			}
			return renderData.customMesh;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000F978 File Offset: 0x0000DB78
		public List<Transform> GetCustomBones(Renderer ren)
		{
			RenderData renderData = this.GetRenderData(ren);
			return ((renderData != null) ? renderData.transformList : null) ?? null;
		}

		// Token: 0x04000245 RID: 581
		[SerializeField]
		private ClothSerializeData serializeData = new ClothSerializeData();

		// Token: 0x04000246 RID: 582
		[SerializeField]
		internal ClothSerializeData2 serializeData2 = new ClothSerializeData2();

		// Token: 0x04000247 RID: 583
		private ClothProcess process = new ClothProcess();

		// Token: 0x04000248 RID: 584
		[HideInInspector]
		public float animationPoseRatioProperty;

		// Token: 0x04000249 RID: 585
		private float _animationPoseRatioProperty;

		// Token: 0x0400024A RID: 586
		[HideInInspector]
		public float gravityProperty;

		// Token: 0x0400024B RID: 587
		private float _gravityProperty;

		// Token: 0x0400024C RID: 588
		[HideInInspector]
		public float dampingProperty;

		// Token: 0x0400024D RID: 589
		private float _dampingProperty;

		// Token: 0x0400024E RID: 590
		[HideInInspector]
		public float worldInertiaProperty;

		// Token: 0x0400024F RID: 591
		private float _worldInertiaProperty;

		// Token: 0x04000250 RID: 592
		[HideInInspector]
		public float localInertiaProperty;

		// Token: 0x04000251 RID: 593
		private float _localInertiaProperty;

		// Token: 0x04000252 RID: 594
		[HideInInspector]
		public float windInfluenceProperty;

		// Token: 0x04000253 RID: 595
		private float _windInfluenceProperty;

		// Token: 0x04000254 RID: 596
		[HideInInspector]
		public float blendWeightProperty;

		// Token: 0x04000255 RID: 597
		private float _blendWeightProperty;

		// Token: 0x04000256 RID: 598
		public Action<MagicaCloth, bool> OnBuildComplete;

		// Token: 0x04000257 RID: 599
		public Action<MagicaCloth, Renderer, bool> OnRendererMeshChange;
	}
}
