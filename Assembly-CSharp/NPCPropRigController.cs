using System;
using System.Collections;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

// Token: 0x020000DD RID: 221
[DisallowMultipleComponent]
public class NPCPropRigController : MonoBehaviour
{
	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x060008D2 RID: 2258 RVA: 0x00040B8D File Offset: 0x0003ED8D
	public Transform PrimaryTarget
	{
		get
		{
			return this._primaryTarget;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00040B95 File Offset: 0x0003ED95
	public Transform PrimaryHint
	{
		get
		{
			return this._primaryHint;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00040B9D File Offset: 0x0003ED9D
	public Transform SecondaryTarget
	{
		get
		{
			return this._secondaryTarget;
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x060008D5 RID: 2261 RVA: 0x00040BA5 File Offset: 0x0003EDA5
	public Transform SecondaryHint
	{
		get
		{
			return this._secondaryHint;
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00040BAD File Offset: 0x0003EDAD
	public TwoBoneIKConstraint PrimaryHandIK
	{
		get
		{
			return this.primaryHandIK;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x060008D7 RID: 2263 RVA: 0x00040BB5 File Offset: 0x0003EDB5
	public TwoBoneIKConstraint SecondaryHandIK
	{
		get
		{
			return this.secondaryHandIK;
		}
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x00040BBD File Offset: 0x0003EDBD
	private void Awake()
	{
		if (this.rigBuilder == null)
		{
			this.rigBuilder = base.GetComponent<RigBuilder>();
		}
		this.Initialize();
		this.CacheConstraintBoneNames();
		this.SubscribeToOutfitSystem();
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x00040BEC File Offset: 0x0003EDEC
	private void OnDestroy()
	{
		this.UnsubscribeFromOutfitSystem();
		if (this._propRigDetached && this._propRigTransform != null && this._propRigTransform.parent == null)
		{
			Object.Destroy(this._propRigTransform.gameObject);
		}
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00040C38 File Offset: 0x0003EE38
	public void Initialize()
	{
		if (this._initialized)
		{
			return;
		}
		this.CacheTargets();
		this.ZeroAll();
		this._initialized = true;
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x00040C56 File Offset: 0x0003EE56
	public void ForceFullRebuild()
	{
		if (this.rigBuilder == null)
		{
			return;
		}
		this.rigBuilder.enabled = false;
		this.rigBuilder.enabled = true;
		this.CacheTargets();
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x00040C85 File Offset: 0x0003EE85
	public void ActivatePrimary(float weight = 1f)
	{
		if (!this._initialized)
		{
			this.Initialize();
		}
		if (this.primaryHandIK != null)
		{
			this.primaryHandIK.weight = weight;
		}
		this.RefreshRigWeight();
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x00040CB5 File Offset: 0x0003EEB5
	public void ActivateSecondary(float weight = 1f)
	{
		if (!this._initialized)
		{
			this.Initialize();
		}
		if (this.secondaryHandIK != null)
		{
			this.secondaryHandIK.weight = weight;
		}
		this.RefreshRigWeight();
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00040CE5 File Offset: 0x0003EEE5
	public void DeactivateAll()
	{
		this.ZeroAll();
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x00040CED File Offset: 0x0003EEED
	public void SetPrimaryTarget(Vector3 worldPos, Quaternion worldRot)
	{
		if (this._primaryTarget == null)
		{
			return;
		}
		this._primaryTarget.position = worldPos;
		this._primaryTarget.rotation = worldRot;
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x00040D16 File Offset: 0x0003EF16
	public void SetPrimaryHint(Vector3 worldPos)
	{
		if (this._primaryHint != null)
		{
			this._primaryHint.position = worldPos;
		}
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x00040D32 File Offset: 0x0003EF32
	public void SetSecondaryTarget(Vector3 worldPos, Quaternion worldRot)
	{
		if (this._secondaryTarget == null)
		{
			return;
		}
		this._secondaryTarget.position = worldPos;
		this._secondaryTarget.rotation = worldRot;
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x00040D5B File Offset: 0x0003EF5B
	public void SetSecondaryHint(Vector3 worldPos)
	{
		if (this._secondaryHint != null)
		{
			this._secondaryHint.position = worldPos;
		}
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x00040D78 File Offset: 0x0003EF78
	private void CacheTargets()
	{
		TwoBoneIKConstraint twoBoneIKConstraint = this.primaryHandIK;
		this._primaryTarget = ((twoBoneIKConstraint != null) ? twoBoneIKConstraint.data.target : null);
		TwoBoneIKConstraint twoBoneIKConstraint2 = this.primaryHandIK;
		this._primaryHint = ((twoBoneIKConstraint2 != null) ? twoBoneIKConstraint2.data.hint : null);
		TwoBoneIKConstraint twoBoneIKConstraint3 = this.secondaryHandIK;
		this._secondaryTarget = ((twoBoneIKConstraint3 != null) ? twoBoneIKConstraint3.data.target : null);
		TwoBoneIKConstraint twoBoneIKConstraint4 = this.secondaryHandIK;
		this._secondaryHint = ((twoBoneIKConstraint4 != null) ? twoBoneIKConstraint4.data.hint : null);
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00040DFC File Offset: 0x0003EFFC
	private void RefreshRigWeight()
	{
		if (this.propRig == null)
		{
			return;
		}
		bool flag = (this.primaryHandIK != null && this.primaryHandIK.weight > 0f) || (this.secondaryHandIK != null && this.secondaryHandIK.weight > 0f);
		this.propRig.weight = (flag ? 1f : 0f);
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x00040E7C File Offset: 0x0003F07C
	private void ZeroAll()
	{
		if (this.propRig != null)
		{
			this.propRig.weight = 0f;
		}
		if (this.primaryHandIK != null)
		{
			this.primaryHandIK.weight = 0f;
		}
		if (this.secondaryHandIK != null)
		{
			this.secondaryHandIK.weight = 0f;
		}
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x00040EE3 File Offset: 0x0003F0E3
	private void CacheConstraintBoneNames()
	{
		this._cachedConstraintBones.Clear();
		this.AddCachedConstraint(this.primaryHandIK);
		this.AddCachedConstraint(this.secondaryHandIK);
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x00040F08 File Offset: 0x0003F108
	private unsafe void AddCachedConstraint(TwoBoneIKConstraint c)
	{
		if (c == null)
		{
			return;
		}
		TwoBoneIKConstraintData twoBoneIKConstraintData = *c.data;
		this._cachedConstraintBones.Add(new NPCPropRigController.CachedConstraintBones
		{
			constraint = c,
			rootName = ((twoBoneIKConstraintData.root != null) ? twoBoneIKConstraintData.root.name : null),
			midName = ((twoBoneIKConstraintData.mid != null) ? twoBoneIKConstraintData.mid.name : null),
			tipName = ((twoBoneIKConstraintData.tip != null) ? twoBoneIKConstraintData.tip.name : null)
		});
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00040FB8 File Offset: 0x0003F1B8
	private void SubscribeToOutfitSystem()
	{
		if (this._hasSubscribedToRigChanged)
		{
			return;
		}
		this._outfitSystem = base.GetComponentInChildren<OutfitSystem>(true);
		if (this._outfitSystem == null)
		{
			return;
		}
		OutfitSystem outfitSystem = this._outfitSystem;
		outfitSystem.OnBeforeRigChanged = (UnityAction)Delegate.Combine(outfitSystem.OnBeforeRigChanged, new UnityAction(this.HandleBeforeRigChanged));
		OutfitSystem outfitSystem2 = this._outfitSystem;
		outfitSystem2.OnRigChanged = (UnityAction<SkinnedMeshRenderer>)Delegate.Combine(outfitSystem2.OnRigChanged, new UnityAction<SkinnedMeshRenderer>(this.HandleRigChanged));
		this._hasSubscribedToRigChanged = true;
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00041040 File Offset: 0x0003F240
	private void UnsubscribeFromOutfitSystem()
	{
		if (!this._hasSubscribedToRigChanged || this._outfitSystem == null)
		{
			return;
		}
		OutfitSystem outfitSystem = this._outfitSystem;
		outfitSystem.OnBeforeRigChanged = (UnityAction)Delegate.Remove(outfitSystem.OnBeforeRigChanged, new UnityAction(this.HandleBeforeRigChanged));
		OutfitSystem outfitSystem2 = this._outfitSystem;
		outfitSystem2.OnRigChanged = (UnityAction<SkinnedMeshRenderer>)Delegate.Remove(outfitSystem2.OnRigChanged, new UnityAction<SkinnedMeshRenderer>(this.HandleRigChanged));
		this._hasSubscribedToRigChanged = false;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x000410BC File Offset: 0x0003F2BC
	private void HandleBeforeRigChanged()
	{
		if (this.propRig == null)
		{
			return;
		}
		Transform transform = this.propRig.transform;
		Transform parent = transform.parent;
		if (parent == null)
		{
			return;
		}
		this._propRigTransform = transform;
		this._propRigParentBoneName = parent.name;
		this._propRigLocalPosition = transform.localPosition;
		this._propRigLocalRotation = transform.localRotation;
		this._propRigLocalScale = transform.localScale;
		transform.SetParent(null, false);
		this._propRigDetached = true;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0004113C File Offset: 0x0003F33C
	private void ReattachPropRig(Dictionary<string, Transform> boneMap, SkinnedMeshRenderer newBody)
	{
		if (!this._propRigDetached || this._propRigTransform == null)
		{
			return;
		}
		Transform transform;
		Transform transform2;
		if (!string.IsNullOrEmpty(this._propRigParentBoneName) && boneMap.TryGetValue(this._propRigParentBoneName, out transform))
		{
			transform2 = transform;
		}
		else
		{
			transform2 = ((newBody.rootBone != null) ? newBody.rootBone : base.transform);
			Debug.LogWarning(string.Concat(new string[] { "[NPCPropRig] Parent bone '", this._propRigParentBoneName, "' non trovato post-merge, fallback su '", transform2.name, "' su '", base.name, "'" }));
		}
		this._propRigTransform.SetParent(transform2, false);
		this._propRigTransform.localPosition = this._propRigLocalPosition;
		this._propRigTransform.localRotation = this._propRigLocalRotation;
		this._propRigTransform.localScale = this._propRigLocalScale;
		this._propRigDetached = false;
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x00041234 File Offset: 0x0003F434
	private unsafe void HandleRigChanged(SkinnedMeshRenderer newBody)
	{
		if (newBody == null || newBody.bones == null)
		{
			return;
		}
		Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>(newBody.bones.Length);
		foreach (Transform transform in newBody.bones)
		{
			if (transform != null && !dictionary.ContainsKey(transform.name))
			{
				dictionary[transform.name] = transform;
			}
		}
		if (newBody.rootBone != null && !dictionary.ContainsKey(newBody.rootBone.name))
		{
			dictionary[newBody.rootBone.name] = newBody.rootBone;
		}
		this.ReattachPropRig(dictionary, newBody);
		int num = 0;
		foreach (NPCPropRigController.CachedConstraintBones cachedConstraintBones in this._cachedConstraintBones)
		{
			if (!(cachedConstraintBones.constraint == null))
			{
				TwoBoneIKConstraintData twoBoneIKConstraintData = *cachedConstraintBones.constraint.data;
				Transform transform2;
				if (cachedConstraintBones.rootName != null && dictionary.TryGetValue(cachedConstraintBones.rootName, out transform2))
				{
					twoBoneIKConstraintData.root = transform2;
				}
				Transform transform3;
				if (cachedConstraintBones.midName != null && dictionary.TryGetValue(cachedConstraintBones.midName, out transform3))
				{
					twoBoneIKConstraintData.mid = transform3;
				}
				Transform transform4;
				if (cachedConstraintBones.tipName != null && dictionary.TryGetValue(cachedConstraintBones.tipName, out transform4))
				{
					twoBoneIKConstraintData.tip = transform4;
				}
				*cachedConstraintBones.constraint.data = twoBoneIKConstraintData;
				num++;
			}
		}
		this.CacheTargets();
		if (base.isActiveAndEnabled)
		{
			base.StartCoroutine(this.RebuildNextFrame());
		}
		if (num > 0)
		{
			Debug.Log(string.Format("[NPCPropRig] Rebound {0} TwoBoneIK constraint(s) after BMAC merge on '{1}'", num, base.name));
		}
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0004140C File Offset: 0x0003F60C
	private IEnumerator RebuildNextFrame()
	{
		yield return null;
		this.ForceFullRebuild();
		yield break;
	}

	// Token: 0x040007ED RID: 2029
	[Header("Prop Rig")]
	[SerializeField]
	private RigBuilder rigBuilder;

	// Token: 0x040007EE RID: 2030
	[SerializeField]
	private Rig propRig;

	// Token: 0x040007EF RID: 2031
	[Header("IK Constraints — assegna dall'Inspector")]
	[SerializeField]
	private TwoBoneIKConstraint primaryHandIK;

	// Token: 0x040007F0 RID: 2032
	[SerializeField]
	private TwoBoneIKConstraint secondaryHandIK;

	// Token: 0x040007F1 RID: 2033
	private Transform _primaryTarget;

	// Token: 0x040007F2 RID: 2034
	private Transform _primaryHint;

	// Token: 0x040007F3 RID: 2035
	private Transform _secondaryTarget;

	// Token: 0x040007F4 RID: 2036
	private Transform _secondaryHint;

	// Token: 0x040007F5 RID: 2037
	private bool _initialized;

	// Token: 0x040007F6 RID: 2038
	private readonly List<NPCPropRigController.CachedConstraintBones> _cachedConstraintBones = new List<NPCPropRigController.CachedConstraintBones>();

	// Token: 0x040007F7 RID: 2039
	private OutfitSystem _outfitSystem;

	// Token: 0x040007F8 RID: 2040
	private bool _hasSubscribedToRigChanged;

	// Token: 0x040007F9 RID: 2041
	private Transform _propRigTransform;

	// Token: 0x040007FA RID: 2042
	private string _propRigParentBoneName;

	// Token: 0x040007FB RID: 2043
	private Vector3 _propRigLocalPosition;

	// Token: 0x040007FC RID: 2044
	private Quaternion _propRigLocalRotation;

	// Token: 0x040007FD RID: 2045
	private Vector3 _propRigLocalScale;

	// Token: 0x040007FE RID: 2046
	private bool _propRigDetached;

	// Token: 0x020000DE RID: 222
	private struct CachedConstraintBones
	{
		// Token: 0x040007FF RID: 2047
		public TwoBoneIKConstraint constraint;

		// Token: 0x04000800 RID: 2048
		public string rootName;

		// Token: 0x04000801 RID: 2049
		public string midName;

		// Token: 0x04000802 RID: 2050
		public string tipName;
	}
}
