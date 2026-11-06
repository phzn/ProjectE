using System;
using UnityEngine;

// Token: 0x020000DC RID: 220
[DisallowMultipleComponent]
public class NPCPropHolder : MonoBehaviour
{
	// Token: 0x170001EA RID: 490
	// (get) Token: 0x060008BA RID: 2234 RVA: 0x000402FA File Offset: 0x0003E4FA
	public bool HasProp
	{
		get
		{
			return this._currentProp != null;
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x060008BB RID: 2235 RVA: 0x00040308 File Offset: 0x0003E508
	public NPCPropDefinitionSO CurrentDef
	{
		get
		{
			return this._currentDef;
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x060008BC RID: 2236 RVA: 0x00040310 File Offset: 0x0003E510
	public bool IsRigReady
	{
		get
		{
			return this._rigInitialized;
		}
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x060008BD RID: 2237 RVA: 0x00040318 File Offset: 0x0003E518
	public GameObject CurrentPropObject
	{
		get
		{
			return this._currentProp;
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x060008BE RID: 2238 RVA: 0x00040320 File Offset: 0x0003E520
	public NPCPropRigController RigController
	{
		get
		{
			return this._rigController;
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x060008BF RID: 2239 RVA: 0x00040328 File Offset: 0x0003E528
	public Transform PrimaryHandAnchor
	{
		get
		{
			return this._primaryHandAnchor;
		}
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00040330 File Offset: 0x0003E530
	public Transform PrimaryElbowAnchor
	{
		get
		{
			return this._primaryElbowAnchor;
		}
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00040338 File Offset: 0x0003E538
	public Transform SecondaryHandAnchor
	{
		get
		{
			return this._secondaryHandAnchor;
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00040340 File Offset: 0x0003E540
	public Transform SecondaryElbowAnchor
	{
		get
		{
			return this._secondaryElbowAnchor;
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00040348 File Offset: 0x0003E548
	// (set) Token: 0x060008C4 RID: 2244 RVA: 0x00040350 File Offset: 0x0003E550
	public bool FreezeTargetTracking { get; set; }

	// Token: 0x060008C5 RID: 2245 RVA: 0x0004035C File Offset: 0x0003E55C
	public void InitializeRig()
	{
		if (this._rigInitialized)
		{
			return;
		}
		this._animator = base.GetComponentInChildren<Animator>();
		this._npcController = base.GetComponent<AStarNPCController>();
		if (this._animator == null)
		{
			this.Log("Animator non trovato");
			return;
		}
		if (!string.IsNullOrEmpty(this.armsLayerName))
		{
			this._armsLayerIndex = this._animator.GetLayerIndex(this.armsLayerName);
			if (this._armsLayerIndex < 0)
			{
				this.Log("Layer '" + this.armsLayerName + "' non trovato — weight management disabilitato");
			}
		}
		if (!this._animator.isHuman)
		{
			this.Log("Animator non-humanoid — IK non disponibile");
			this._rigInitialized = true;
			return;
		}
		this._rigController = this._animator.GetComponent<NPCPropRigController>();
		if (this._rigController == null)
		{
			this.Log("NPCPropRigController non trovato sul GO dell'Animator — IK disabilitato. Aggiungi il componente al prefab.");
		}
		NPCPropRigController rigController = this._rigController;
		if (rigController != null)
		{
			rigController.Initialize();
		}
		this._rigInitialized = true;
		this.Log((this._rigController != null) ? "Rig controller trovato e inizializzato" : "Modalità senza IK");
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x00040470 File Offset: 0x0003E670
	public void ReInitializeAfterMerge()
	{
		this._rigInitialized = false;
		this._armsLayerIndex = -1;
		this._ikActive = false;
		if (this._currentProp != null)
		{
			Object.Destroy(this._currentProp);
			this._currentProp = null;
		}
		NPCPropDefinitionSO currentDef = this._currentDef;
		this._currentDef = null;
		this.InitializeRig();
		NPCPropRigController rigController = this._rigController;
		if (rigController != null)
		{
			rigController.ForceFullRebuild();
		}
		if (currentDef != null && this._rigInitialized)
		{
			this.Equip(currentDef);
		}
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x000404F0 File Offset: 0x0003E6F0
	public void Equip(NPCPropDefinitionSO def)
	{
		if (def == null)
		{
			return;
		}
		if (def.prefab == null)
		{
			Debug.LogWarning(string.Concat(new string[] { "[NPCPropHolder] ", base.name, ": prefab null su '", def.propId, "'" }));
			return;
		}
		this.Unequip();
		if (!this._rigInitialized)
		{
			this.InitializeRig();
		}
		if (!this._rigInitialized)
		{
			this.Log("Rig non pronto");
			return;
		}
		Transform boneTransform = this._animator.GetBoneTransform(def.attachBone);
		if (boneTransform == null)
		{
			Debug.LogWarning(string.Format("[NPCPropHolder] {0}: bone '{1}' non trovato", base.name, def.attachBone));
			return;
		}
		this._currentProp = Object.Instantiate<GameObject>(def.prefab, boneTransform);
		this._currentProp.name = "Prop_" + def.propId;
		this._currentProp.transform.localPosition = def.localPosition;
		this._currentProp.transform.localEulerAngles = def.localEulerAngles;
		this._currentProp.transform.localScale = def.localScale;
		this._currentDef = def;
		this.ResolveAnchors();
		bool flag = def.usePrimaryHandIK && !NPCPropHolder.IsBoneInPrimaryIKChain(def.attachBone);
		if (def.usePrimaryHandIK && !flag)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"[NPCPropHolder] ",
				base.name,
				": Primary IK disabilitata su '",
				def.propId,
				"'. ",
				string.Format("Il prop è parentato a '{0}' che è nella catena IK primaria → feedback loop. ", def.attachBone),
				"Soluzioni: (1) disabilita Primary IK, (2) usa attachBone diverso (es. Chest), (3) usa solo Secondary IK per grip della mano libera."
			}));
		}
		bool flag2 = false;
		if (this._rigController != null)
		{
			this._rigController.ForceFullRebuild();
			if (flag)
			{
				this._rigController.ActivatePrimary(1f);
				flag2 = true;
			}
			if (def.useSecondaryHandIK)
			{
				this._rigController.ActivateSecondary(1f);
				flag2 = true;
			}
		}
		this._ikActive = flag2;
		if (!this.FreezeTargetTracking)
		{
			this.UpdateIKTargets();
		}
		if (def.upperBodyActionIndex > 0)
		{
			if (this._npcController != null)
			{
				this._npcController.SetAction((NPCInterestPoint.ActionType)def.upperBodyActionIndex);
			}
			this.SetArmsLayerWeight(1f);
		}
		this.Log(string.Format("Equipped: {0} | primaryIK={1} | secondaryIK={2}", def.propId, def.usePrimaryHandIK, def.useSecondaryHandIK));
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x00040770 File Offset: 0x0003E970
	public void Unequip()
	{
		this._ikActive = false;
		NPCPropRigController rigController = this._rigController;
		if (rigController != null)
		{
			rigController.DeactivateAll();
		}
		if (this._currentProp != null)
		{
			Object.Destroy(this._currentProp);
			this._currentProp = null;
		}
		this._primaryHandAnchor = (this._primaryElbowAnchor = (this._secondaryHandAnchor = (this._secondaryElbowAnchor = null)));
		if (this._currentDef != null && this._currentDef.upperBodyActionIndex > 0)
		{
			if (this._npcController != null)
			{
				this._npcController.SetAction(NPCInterestPoint.ActionType.None);
			}
			this.SetArmsLayerWeight(0f);
		}
		this._currentDef = null;
		this.Log("Unequipped");
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0004082A File Offset: 0x0003EA2A
	private void LateUpdate()
	{
		if (!this._ikActive || this.FreezeTargetTracking || this._currentProp == null || this._currentDef == null)
		{
			return;
		}
		this.UpdateIKTargets();
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00040860 File Offset: 0x0003EA60
	private void UpdateIKTargets()
	{
		if (this._rigController == null || this._currentProp == null || this._currentDef == null)
		{
			return;
		}
		Transform transform = this._currentProp.transform;
		if (this._currentDef.usePrimaryHandIK)
		{
			Transform transform2 = ((this._primaryHandAnchor != null) ? this._primaryHandAnchor : transform);
			Transform transform3 = ((this._primaryElbowAnchor != null) ? this._primaryElbowAnchor : transform);
			this._rigController.SetPrimaryTarget(transform2.TransformPoint(this._currentDef.primaryHandTargetOffset), transform2.rotation * Quaternion.Euler(this._currentDef.primaryHandTargetRotation));
			this._rigController.SetPrimaryHint(transform3.TransformPoint(this._currentDef.primaryElbowHintOffset));
		}
		if (this._currentDef.useSecondaryHandIK)
		{
			Transform transform4 = ((this._secondaryHandAnchor != null) ? this._secondaryHandAnchor : transform);
			Transform transform5 = ((this._secondaryElbowAnchor != null) ? this._secondaryElbowAnchor : transform);
			this._rigController.SetSecondaryTarget(transform4.TransformPoint(this._currentDef.secondaryHandTargetOffset), transform4.rotation * Quaternion.Euler(this._currentDef.secondaryHandTargetRotation));
			this._rigController.SetSecondaryHint(transform5.TransformPoint(this._currentDef.secondaryElbowHintOffset));
		}
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x000409CC File Offset: 0x0003EBCC
	private void ResolveAnchors()
	{
		NPCPropDefinitionSO currentDef = this._currentDef;
		this._primaryHandAnchor = this.FindAnchor((currentDef != null) ? currentDef.primaryHandAnchorName : null);
		NPCPropDefinitionSO currentDef2 = this._currentDef;
		this._primaryElbowAnchor = this.FindAnchor((currentDef2 != null) ? currentDef2.primaryElbowHintAnchorName : null);
		NPCPropDefinitionSO currentDef3 = this._currentDef;
		this._secondaryHandAnchor = this.FindAnchor((currentDef3 != null) ? currentDef3.secondaryHandAnchorName : null);
		NPCPropDefinitionSO currentDef4 = this._currentDef;
		this._secondaryElbowAnchor = this.FindAnchor((currentDef4 != null) ? currentDef4.secondaryElbowHintAnchorName : null);
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00040A54 File Offset: 0x0003EC54
	private Transform FindAnchor(string nameOrPath)
	{
		if (string.IsNullOrEmpty(nameOrPath) || this._currentProp == null)
		{
			return null;
		}
		Transform transform = this._currentProp.transform.Find(nameOrPath);
		if (transform != null)
		{
			return transform;
		}
		foreach (Transform transform2 in this._currentProp.GetComponentsInChildren<Transform>(true))
		{
			if (transform2.name == nameOrPath)
			{
				return transform2;
			}
		}
		Debug.LogWarning(string.Concat(new string[]
		{
			"[NPCPropHolder] ",
			base.name,
			": anchor '",
			nameOrPath,
			"' non trovato sul prop '",
			this._currentDef.propId,
			"'"
		}));
		return null;
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00040B11 File Offset: 0x0003ED11
	private void SetArmsLayerWeight(float weight)
	{
		if (this._animator != null && this._armsLayerIndex >= 0)
		{
			this._animator.SetLayerWeight(this._armsLayerIndex, weight);
		}
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x00040B3C File Offset: 0x0003ED3C
	private static bool IsBoneInPrimaryIKChain(HumanBodyBones bone)
	{
		return bone == HumanBodyBones.RightUpperArm || bone == HumanBodyBones.RightLowerArm || bone == HumanBodyBones.RightHand;
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x00040B4F File Offset: 0x0003ED4F
	private void OnDestroy()
	{
		if (this._currentProp != null)
		{
			Object.Destroy(this._currentProp);
		}
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x00040B6A File Offset: 0x0003ED6A
	private void Log(string msg)
	{
		bool flag = this.debugLog;
	}

	// Token: 0x040007DE RID: 2014
	[Header("Debug")]
	[SerializeField]
	private bool debugLog;

	// Token: 0x040007DF RID: 2015
	[Header("Arms Layer")]
	[Tooltip("Nome del layer animator per il prop holding (weight 0→1 su Equip/Unequip).")]
	[SerializeField]
	private string armsLayerName = "Arms";

	// Token: 0x040007E0 RID: 2016
	private Animator _animator;

	// Token: 0x040007E1 RID: 2017
	private AStarNPCController _npcController;

	// Token: 0x040007E2 RID: 2018
	private NPCPropRigController _rigController;

	// Token: 0x040007E3 RID: 2019
	private int _armsLayerIndex = -1;

	// Token: 0x040007E4 RID: 2020
	private GameObject _currentProp;

	// Token: 0x040007E5 RID: 2021
	private NPCPropDefinitionSO _currentDef;

	// Token: 0x040007E6 RID: 2022
	private Transform _primaryHandAnchor;

	// Token: 0x040007E7 RID: 2023
	private Transform _primaryElbowAnchor;

	// Token: 0x040007E8 RID: 2024
	private Transform _secondaryHandAnchor;

	// Token: 0x040007E9 RID: 2025
	private Transform _secondaryElbowAnchor;

	// Token: 0x040007EA RID: 2026
	private bool _rigInitialized;

	// Token: 0x040007EB RID: 2027
	private bool _ikActive;
}
