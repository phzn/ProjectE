using System;
using System.Collections.Generic;
using MagicaCloth2;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization
{
	// Token: 0x02000ACF RID: 2767
	[DisallowMultipleComponent]
	public sealed class CielHairGarmentPhysicsFit : MonoBehaviour
	{
		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06004E76 RID: 20086 RVA: 0x001E71ED File Offset: 0x001E53ED
		// (set) Token: 0x06004E77 RID: 20087 RVA: 0x001E71F5 File Offset: 0x001E53F5
		public float CurrentCover { get; private set; }

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06004E78 RID: 20088 RVA: 0x001E71FE File Offset: 0x001E53FE
		// (set) Token: 0x06004E79 RID: 20089 RVA: 0x001E7206 File Offset: 0x001E5406
		public float CurrentCrownCover { get; private set; }

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06004E7A RID: 20090 RVA: 0x001E720F File Offset: 0x001E540F
		public int CrownBoneCount
		{
			get
			{
				if (this.crownBones == null)
				{
					return 0;
				}
				return this.crownBones.Length;
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06004E7B RID: 20091 RVA: 0x001E7223 File Offset: 0x001E5423
		public float CurrentPhysicsWeight
		{
			get
			{
				if (!(this._cloth != null))
				{
					return 0f;
				}
				return this._cloth.SerializeData.blendWeight;
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06004E7C RID: 20092 RVA: 0x001E7249 File Offset: 0x001E5449
		public int BoundRendererCount
		{
			get
			{
				if (this._bindings == null)
				{
					return 0;
				}
				return this._bindings.Length;
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06004E7D RID: 20093 RVA: 0x001E725D File Offset: 0x001E545D
		// (set) Token: 0x06004E7E RID: 20094 RVA: 0x001E7265 File Offset: 0x001E5465
		public int ReleaseResetRequests { get; private set; }

		// Token: 0x06004E7F RID: 20095 RVA: 0x001E7270 File Offset: 0x001E5470
		public void Configure(GameObject root)
		{
			if (root == null || !base.transform.IsChildOf(root.transform))
			{
				throw new ArgumentException("The hairstyle root must contain this HairPhysics component.", "root");
			}
			this.Release();
			this.hairstyleRoot = root.transform;
			this.hairRenderers = root.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			this.CacheRenderers();
			if (!Application.isPlaying)
			{
				this.AuthorCrownMask();
			}
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x001E72DC File Offset: 0x001E54DC
		private void OnEnable()
		{
			this.CacheRenderers();
			if (Application.isPlaying)
			{
				MagicaManager.OnPreSimulation = (Action)Delegate.Combine(MagicaManager.OnPreSimulation, new Action(this.PrepareSimulation));
				MagicaManager.OnPostSimulation = (Action)Delegate.Combine(MagicaManager.OnPostSimulation, new Action(this.FinishSimulation));
			}
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x001E7338 File Offset: 0x001E5538
		private void OnDisable()
		{
			MagicaManager.OnPreSimulation = (Action)Delegate.Remove(MagicaManager.OnPreSimulation, new Action(this.PrepareSimulation));
			MagicaManager.OnPostSimulation = (Action)Delegate.Remove(MagicaManager.OnPostSimulation, new Action(this.FinishSimulation));
			this.Release();
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x001E738B File Offset: 0x001E558B
		private void OnDestroy()
		{
			this.OnDisable();
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x001E7393 File Offset: 0x001E5593
		private void PrepareSimulation()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this._lastSimulationFrame = Time.frameCount;
			this.RefreshFit(Time.deltaTime);
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x001E73B4 File Offset: 0x001E55B4
		private void LateUpdate()
		{
			if (Application.isPlaying && this._lastSimulationFrame != Time.frameCount && (this._cloth == null || !this._cloth.IsValid() || !this._cloth.isActiveAndEnabled))
			{
				this.RefreshFit(Time.deltaTime);
			}
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x001E7408 File Offset: 0x001E5608
		private void CacheRenderers()
		{
			if (this._bindings != null)
			{
				return;
			}
			if (this.hairRenderers == null)
			{
				this.hairRenderers = Array.Empty<SkinnedMeshRenderer>();
			}
			this._bindings = new CielHairGarmentPhysicsFit.RendererBinding[this.hairRenderers.Length];
			for (int i = 0; i < this.hairRenderers.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = this.hairRenderers[i];
				if (skinnedMeshRenderer != null && (this.hairstyleRoot == null || !skinnedMeshRenderer.transform.IsChildOf(this.hairstyleRoot)))
				{
					skinnedMeshRenderer = null;
				}
				this._bindings[i] = new CielHairGarmentPhysicsFit.RendererBinding
				{
					renderer = skinnedMeshRenderer
				};
				this._bindings[i].RefreshIndices();
			}
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x001E74B0 File Offset: 0x001E56B0
		private void AuthorCrownMask()
		{
			this.crownBones = Array.Empty<CielHairGarmentPhysicsFit.CrownBone>();
			MagicaCloth component = base.GetComponent<MagicaCloth>();
			if (component == null)
			{
				return;
			}
			Transform transform = null;
			Transform[] componentsInChildren = this.hairstyleRoot.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].name == "head.x")
				{
					transform = componentsInChildren[i];
					break;
				}
			}
			if (transform == null)
			{
				return;
			}
			float y = this.hairstyleRoot.InverseTransformPoint(transform.position).y;
			List<CielHairGarmentPhysicsFit.CrownBone> list = new List<CielHairGarmentPhysicsFit.CrownBone>();
			HashSet<Transform> hashSet = new HashSet<Transform>();
			foreach (Transform transform2 in component.SerializeData.rootBones)
			{
				if (!(transform2 == null) && transform2.IsChildOf(this.hairstyleRoot))
				{
					foreach (Transform transform3 in transform2.GetComponentsInChildren<Transform>(true))
					{
						if (!(transform3 == transform2) && hashSet.Add(transform3))
						{
							float num = this.hairstyleRoot.InverseTransformPoint(transform3.position).y - y;
							float num2 = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(-0.02f, 0.08f, num));
							if (num2 > 1E-05f)
							{
								list.Add(new CielHairGarmentPhysicsFit.CrownBone
								{
									bone = transform3,
									restLocalPosition = transform3.localPosition,
									restLocalRotation = transform3.localRotation,
									weight = num2
								});
							}
						}
					}
				}
			}
			this.crownBones = list.ToArray();
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x001E7694 File Offset: 0x001E5894
		public void RefreshFit(float deltaTime)
		{
			if (!base.isActiveAndEnabled || this.hairstyleRoot == null)
			{
				return;
			}
			this.CacheRenderers();
			MagicaCloth component = base.GetComponent<MagicaCloth>();
			if (component == null)
			{
				return;
			}
			if (component != this._cloth)
			{
				float blendWeight = component.SerializeData.blendWeight;
				bool flag = this._ownsBlend && Mathf.Approximately(blendWeight, this._lastApplied);
				this._cloth = component;
				if (!flag)
				{
					this._baseline = blendWeight;
				}
				this._lastApplied = blendWeight;
				this._ownsBlend = true;
			}
			else if (this._ownsBlend && !Mathf.Approximately(this._cloth.SerializeData.blendWeight, this._lastApplied))
			{
				this._baseline = this._cloth.SerializeData.blendWeight;
			}
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < this._bindings.Length; i++)
			{
				num = Mathf.Max(num, this._bindings[i].ReadCover());
				num2 = Mathf.Max(num2, this._bindings[i].ReadCrownCover());
			}
			if (!this._initialized)
			{
				this.CurrentCover = num;
				this.CurrentCrownCover = num2;
				this._initialized = true;
			}
			else
			{
				if (this._wasFullyCovered && num < 0.999f)
				{
					this._cloth.ResetCloth(false);
					int releaseResetRequests = this.ReleaseResetRequests;
					this.ReleaseResetRequests = releaseResetRequests + 1;
					this._wasFullyCovered = false;
				}
				float num3 = ((num > this.CurrentCover) ? this.coverFadeSeconds : this.releaseFadeSeconds);
				this.CurrentCover = Mathf.MoveTowards(this.CurrentCover, num, Mathf.Max(0f, deltaTime) / Mathf.Max(0.001f, num3));
				float num4 = ((num2 > this.CurrentCrownCover) ? this.coverFadeSeconds : this.releaseFadeSeconds);
				this.CurrentCrownCover = Mathf.MoveTowards(this.CurrentCrownCover, num2, Mathf.Max(0f, deltaTime) / Mathf.Max(0.001f, num4));
			}
			if (this.CurrentCover >= 0.999f && num >= 0.999f)
			{
				this._wasFullyCovered = true;
			}
			this.ApplyWeight(this._baseline * (1f - this.CurrentCover));
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x001E78BC File Offset: 0x001E5ABC
		public void ApplyCrownFit()
		{
			if (!base.isActiveAndEnabled || this.CurrentCrownCover <= 0f || this.crownBones == null)
			{
				return;
			}
			for (int i = 0; i < this.crownBones.Length; i++)
			{
				CielHairGarmentPhysicsFit.CrownBone crownBone = this.crownBones[i];
				if (!(crownBone.bone == null))
				{
					float num = crownBone.weight * this.CurrentCrownCover;
					crownBone.bone.localPosition = Vector3.Lerp(crownBone.bone.localPosition, crownBone.restLocalPosition, num);
					crownBone.bone.localRotation = Quaternion.Slerp(crownBone.bone.localRotation, crownBone.restLocalRotation, num);
				}
			}
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x001E7968 File Offset: 0x001E5B68
		private void FinishSimulation()
		{
			if (this._cloth == null || !this._cloth.isActiveAndEnabled || !this._cloth.IsValid() || this._cloth.Process.TeamId <= 0)
			{
				return;
			}
			ref TeamManager.TeamData teamDataRef = ref MagicaManager.Team.GetTeamDataRef(this._cloth.Process.TeamId);
			if (!teamDataRef.IsEnable || teamDataRef.IsCullingInvisible || teamDataRef.IsScaleSuspend || teamDataRef.flag.IsSet(14))
			{
				return;
			}
			this.ApplyCrownFit();
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x001E79FA File Offset: 0x001E5BFA
		private void ApplyWeight(float value)
		{
			this._lastApplied = value;
			if (Mathf.Approximately(this._cloth.SerializeData.blendWeight, value))
			{
				return;
			}
			this._cloth.SerializeData.blendWeight = value;
			this._cloth.SetParameterChange();
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x001E7A38 File Offset: 0x001E5C38
		public void Release()
		{
			if (this._ownsBlend && this._cloth != null && Mathf.Approximately(this._cloth.SerializeData.blendWeight, this._lastApplied))
			{
				if (this.CurrentCover > 0f || this.CurrentCrownCover > 0f)
				{
					this._cloth.ResetCloth(false);
				}
				this.ApplyWeight(this._baseline);
			}
			this._cloth = null;
			this._bindings = null;
			this._ownsBlend = (this._initialized = (this._wasFullyCovered = false));
			this.CurrentCover = 0f;
			this.CurrentCrownCover = 0f;
			this._lastSimulationFrame = -1;
		}

		// Token: 0x04004FD2 RID: 20434
		[SerializeField]
		private Transform hairstyleRoot;

		// Token: 0x04004FD3 RID: 20435
		[SerializeField]
		private SkinnedMeshRenderer[] hairRenderers = Array.Empty<SkinnedMeshRenderer>();

		// Token: 0x04004FD4 RID: 20436
		[SerializeField]
		[Min(0.001f)]
		private float coverFadeSeconds = 0.12f;

		// Token: 0x04004FD5 RID: 20437
		[SerializeField]
		[Min(0.001f)]
		private float releaseFadeSeconds = 0.25f;

		// Token: 0x04004FD6 RID: 20438
		[SerializeField]
		private CielHairGarmentPhysicsFit.CrownBone[] crownBones = Array.Empty<CielHairGarmentPhysicsFit.CrownBone>();

		// Token: 0x04004FD7 RID: 20439
		private CielHairGarmentPhysicsFit.RendererBinding[] _bindings;

		// Token: 0x04004FD8 RID: 20440
		private MagicaCloth _cloth;

		// Token: 0x04004FD9 RID: 20441
		private float _baseline;

		// Token: 0x04004FDA RID: 20442
		private float _lastApplied;

		// Token: 0x04004FDB RID: 20443
		private bool _ownsBlend;

		// Token: 0x04004FDC RID: 20444
		private bool _initialized;

		// Token: 0x04004FDD RID: 20445
		private bool _wasFullyCovered;

		// Token: 0x04004FDE RID: 20446
		private int _lastSimulationFrame = -1;

		// Token: 0x02000AD0 RID: 2768
		[Serializable]
		private struct CrownBone
		{
			// Token: 0x04004FE2 RID: 20450
			public Transform bone;

			// Token: 0x04004FE3 RID: 20451
			public Vector3 restLocalPosition;

			// Token: 0x04004FE4 RID: 20452
			public Quaternion restLocalRotation;

			// Token: 0x04004FE5 RID: 20453
			[Range(0f, 1f)]
			public float weight;
		}

		// Token: 0x02000AD1 RID: 2769
		private sealed class RendererBinding
		{
			// Token: 0x06004E8D RID: 20109 RVA: 0x001E7B2C File Offset: 0x001E5D2C
			public void RefreshIndices()
			{
				this.mesh = ((this.renderer != null) ? this.renderer.sharedMesh : null);
				this.ghost = ((this.mesh != null) ? this.mesh.GetBlendShapeIndex("Ghost_HideHair") : (-1));
				this.rain = ((this.mesh != null) ? this.mesh.GetBlendShapeIndex("RainCoat_HideHair") : (-1));
				this.bag = ((this.mesh != null) ? this.mesh.GetBlendShapeIndex("BAG_ON") : (-1));
				this.pantiesHead = ((this.mesh != null) ? this.mesh.GetBlendShapeIndex("PantiesHead_HairFit") : (-1));
				this.pantiesFace = ((this.mesh != null) ? this.mesh.GetBlendShapeIndex("PantiesHeadFace_HairFit") : (-1));
			}

			// Token: 0x06004E8E RID: 20110 RVA: 0x001E7C20 File Offset: 0x001E5E20
			public float ReadCover()
			{
				if (this.renderer == null)
				{
					return 0f;
				}
				if (this.renderer.sharedMesh != this.mesh)
				{
					this.RefreshIndices();
				}
				if (this.mesh == null)
				{
					return 0f;
				}
				float num = ((this.ghost >= 0) ? this.renderer.GetBlendShapeWeight(this.ghost) : 0f);
				if (this.rain >= 0)
				{
					num = Mathf.Max(num, this.renderer.GetBlendShapeWeight(this.rain));
				}
				if (this.bag >= 0)
				{
					num = Mathf.Max(num, this.renderer.GetBlendShapeWeight(this.bag));
				}
				return Mathf.Clamp01(num * 0.01f);
			}

			// Token: 0x06004E8F RID: 20111 RVA: 0x001E7CE4 File Offset: 0x001E5EE4
			public float ReadCrownCover()
			{
				if (this.renderer == null)
				{
					return 0f;
				}
				if (this.renderer.sharedMesh != this.mesh)
				{
					this.RefreshIndices();
				}
				if (this.mesh == null)
				{
					return 0f;
				}
				float num = ((this.pantiesHead >= 0) ? this.renderer.GetBlendShapeWeight(this.pantiesHead) : 0f);
				if (this.pantiesFace >= 0)
				{
					num = Mathf.Max(num, this.renderer.GetBlendShapeWeight(this.pantiesFace));
				}
				return Mathf.Clamp01(num * 0.01f);
			}

			// Token: 0x04004FE6 RID: 20454
			public SkinnedMeshRenderer renderer;

			// Token: 0x04004FE7 RID: 20455
			public Mesh mesh;

			// Token: 0x04004FE8 RID: 20456
			public int ghost = -1;

			// Token: 0x04004FE9 RID: 20457
			public int rain = -1;

			// Token: 0x04004FEA RID: 20458
			public int bag = -1;

			// Token: 0x04004FEB RID: 20459
			public int pantiesHead = -1;

			// Token: 0x04004FEC RID: 20460
			public int pantiesFace = -1;
		}
	}
}
