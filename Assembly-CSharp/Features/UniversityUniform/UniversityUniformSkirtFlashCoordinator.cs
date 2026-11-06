using System;
using System.Collections;
using System.Collections.Generic;
using Features.Flash;
using MagicaCloth2;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Futanari;
using RayznGames.DressRigger;
using UnityEngine;
using UnityEngine.Events;

namespace Features.UniversityUniform
{
	// Token: 0x02000BBE RID: 3006
	public sealed class UniversityUniformSkirtFlashCoordinator : MonoBehaviour
	{
		// Token: 0x06005631 RID: 22065 RVA: 0x00217CBC File Offset: 0x00215EBC
		private void Awake()
		{
			if (this.flashController == null)
			{
				this.flashController = base.GetComponent<ArmsFlashController>();
			}
			if (this.playerAnimator == null && this.flashController != null)
			{
				this.playerAnimator = this.flashController.GetComponentInParent<Animator>();
			}
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x00217D10 File Offset: 0x00215F10
		private void OnEnable()
		{
			if (this.flashController == null)
			{
				this.flashController = base.GetComponent<ArmsFlashController>();
			}
			if (this.flashController != null)
			{
				this.flashController.OnOpened.AddListener(new UnityAction(this.HandleOpened));
				this.flashController.OnClosed.AddListener(new UnityAction(this.HandleClosed));
			}
			this.OnGarmentRestored();
		}

		// Token: 0x06005633 RID: 22067 RVA: 0x00217D84 File Offset: 0x00215F84
		private void OnDisable()
		{
			if (this.flashController != null)
			{
				this.flashController.OnOpened.RemoveListener(new UnityAction(this.HandleOpened));
				this.flashController.OnClosed.RemoveListener(new UnityAction(this.HandleClosed));
			}
			this.StopPendingRoutines();
			this.RestoreImmediate();
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x00217DE4 File Offset: 0x00215FE4
		private void HandleOpened()
		{
			if (this.flashController == null || !this.flashController.IsWorn)
			{
				return;
			}
			this.StopPendingRoutines();
			this.ResolveWornRenderer();
			this.CaptureAndSuspendWornCloth();
			this._swapRoutine = base.StartCoroutine(this.SwapAfterLift());
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x00217E34 File Offset: 0x00216034
		private void HandleClosed()
		{
			if (this._swapRoutine != null)
			{
				base.StopCoroutine(this._swapRoutine);
				this._swapRoutine = null;
			}
			if (this._restoreRoutine != null)
			{
				base.StopCoroutine(this._restoreRoutine);
				this._restoreRoutine = null;
			}
			this.HideLifted();
			if (this._wornRenderer != null)
			{
				this._wornRenderer.gameObject.SetActive(true);
			}
			this._restoreRoutine = base.StartCoroutine(this.RestoreClothAfterLower());
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x00217EB0 File Offset: 0x002160B0
		public void OnGarmentRestored()
		{
			this.StopPendingRoutines();
			this.RestoreImmediate();
			this._wornRenderer = null;
			this._wornClothStates.Clear();
			this._liveBonesByName = null;
			if (this._liftedInstance != null)
			{
				Object.Destroy(this._liftedInstance);
				this._liftedInstance = null;
			}
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x00217F04 File Offset: 0x00216104
		public void SetGarmentRoot(GameObject root)
		{
			Transform transform = ((root != null) ? root.transform : null);
			if (this._garmentRoot == transform)
			{
				return;
			}
			this.OnGarmentRestored();
			this._garmentRoot = transform;
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x00217F40 File Offset: 0x00216140
		private IEnumerator SwapAfterLift()
		{
			Animator detail = ((this.flashController != null) ? this.flashController.EnsureDetailAnimator() : null);
			int layer = ((this.flashController != null && this.flashController.Config != null) ? this.flashController.Config.detailLayerIndex : 0);
			float fallbackLength = UniversityUniformSkirtFlashCoordinator.ResolveClipLength(detail, "SU_Skirt_01LiftShape", 1.5f);
			float swapTime = (this.NeedsGestureCollision() ? Mathf.Min(this.swapAtNormalizedTime, 0.7f) : this.swapAtNormalizedTime);
			float elapsed = 0f;
			float num;
			while (this.flashController != null && this.flashController.IsOpen && (!UniversityUniformSkirtFlashCoordinator.TryGetNormalizedTime(detail, layer, this.liftDetailStateName, out num) || num < swapTime) && elapsed < fallbackLength * swapTime)
			{
				elapsed += Time.unscaledDeltaTime;
				yield return null;
			}
			if (this.flashController == null || !this.flashController.IsOpen)
			{
				this._swapRoutine = null;
				yield break;
			}
			this.ShowLifted();
			this._swapRoutine = null;
			yield break;
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x00217F4F File Offset: 0x0021614F
		private IEnumerator RestoreClothAfterLower()
		{
			Animator detail = ((this.flashController != null) ? this.flashController.EnsureDetailAnimator() : null);
			int layer = ((this.flashController != null && this.flashController.Config != null) ? this.flashController.Config.detailLayerIndex : 0);
			float fallbackLength = UniversityUniformSkirtFlashCoordinator.ResolveClipLength(detail, "SU_Skirt_03LowerShape", 1.5f);
			float elapsed = 0f;
			float num;
			while (this.flashController != null && !this.flashController.IsOpen && (!UniversityUniformSkirtFlashCoordinator.TryGetNormalizedTime(detail, layer, this.lowerDetailStateName, out num) || num < this.restoreClothAtNormalizedTime) && elapsed < fallbackLength * this.restoreClothAtNormalizedTime)
			{
				elapsed += Time.unscaledDeltaTime;
				yield return null;
			}
			if (this.flashController == null || !this.flashController.IsOpen)
			{
				this.RestoreWornClothStates();
			}
			this._restoreRoutine = null;
			yield break;
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x00217F60 File Offset: 0x00216160
		private void CaptureAndSuspendWornCloth()
		{
			if (this._wornRenderer == null)
			{
				return;
			}
			this._wornClothStates.Clear();
			bool flag = this.NeedsGestureCollision();
			foreach (MagicaCloth magicaCloth in this._wornRenderer.GetComponents<MagicaCloth>())
			{
				if (!(magicaCloth == null))
				{
					this._wornClothStates[magicaCloth] = magicaCloth.enabled;
					if (!flag)
					{
						magicaCloth.enabled = false;
					}
				}
			}
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x00217FD4 File Offset: 0x002161D4
		private bool NeedsGestureCollision()
		{
			Animator animator = ((this.playerAnimator != null) ? this.playerAnimator : base.GetComponentInParent<Animator>());
			FutanariModeController futanariModeController = ((animator != null) ? animator.GetComponentInChildren<FutanariModeController>(true) : null);
			return futanariModeController != null && futanariModeController.IsEffectivelyVisible;
		}

		// Token: 0x0600563C RID: 22076 RVA: 0x00218024 File Offset: 0x00216224
		private void RestoreWornClothStates()
		{
			foreach (KeyValuePair<MagicaCloth, bool> keyValuePair in this._wornClothStates)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.enabled = keyValuePair.Value;
				}
			}
			this._wornClothStates.Clear();
		}

		// Token: 0x0600563D RID: 22077 RVA: 0x002180A0 File Offset: 0x002162A0
		private void ShowLifted()
		{
			this.ResolveWornRenderer();
			if (this._wornRenderer == null)
			{
				return;
			}
			if (this.liftedSkirtPrefab == null)
			{
				Debug.LogWarning("[UniversityUniformFlash] Lifted skirt prefab is not assigned.", this);
				return;
			}
			if (this._liftedInstance == null)
			{
				this.CreateLiftedInstance();
			}
			if (this._liftedInstance == null)
			{
				return;
			}
			this.InheritWornAppearance();
			this._liftedInstance.SetActive(true);
			foreach (MagicaCloth magicaCloth in this._liftedInstance.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth != null)
				{
					magicaCloth.enabled = true;
				}
			}
			this._wornRenderer.gameObject.SetActive(false);
		}

		// Token: 0x0600563E RID: 22078 RVA: 0x00218154 File Offset: 0x00216354
		private void CreateLiftedInstance()
		{
			Transform transform = ((this._wornRenderer != null && this._wornRenderer.transform.parent != null) ? this._wornRenderer.transform.parent : ((this.playerAnimator != null) ? this.playerAnimator.transform : base.transform));
			this._liftedInstance = Object.Instantiate<GameObject>(this.liftedSkirtPrefab, transform);
			this._liftedInstance.name = this.liftedInstanceName;
			this._liftedInstance.transform.localPosition = Vector3.zero;
			this._liftedInstance.transform.localRotation = Quaternion.identity;
			this._liftedInstance.transform.localScale = Vector3.one;
			MagicaCloth[] componentsInChildren = this._liftedInstance.GetComponentsInChildren<MagicaCloth>(true);
			foreach (MagicaCloth magicaCloth in componentsInChildren)
			{
				magicaCloth.Initialize();
				magicaCloth.DisableAutoBuild();
			}
			Animator[] componentsInChildren2 = this._liftedInstance.GetComponentsInChildren<Animator>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
			this.RebindToLiveBones();
			this.FinalizeLiftedCloth(componentsInChildren);
			DonorSkeleton.Detach(this._liftedInstance, this._liveBonesByName, this);
		}

		// Token: 0x0600563F RID: 22079 RVA: 0x00218290 File Offset: 0x00216490
		private void HideLifted()
		{
			if (this._liftedInstance == null)
			{
				return;
			}
			foreach (MagicaCloth magicaCloth in this._liftedInstance.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth != null)
				{
					magicaCloth.enabled = false;
				}
			}
			this._liftedInstance.SetActive(false);
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x002182E7 File Offset: 0x002164E7
		private void RestoreImmediate()
		{
			this.HideLifted();
			if (this._wornRenderer != null)
			{
				this._wornRenderer.gameObject.SetActive(true);
			}
			this.RestoreWornClothStates();
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x00218314 File Offset: 0x00216514
		public void InterruptForEncounter()
		{
			this.StopPendingRoutines();
			this.RestoreImmediate();
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x00218322 File Offset: 0x00216522
		private void StopPendingRoutines()
		{
			if (this._swapRoutine != null)
			{
				base.StopCoroutine(this._swapRoutine);
			}
			if (this._restoreRoutine != null)
			{
				base.StopCoroutine(this._restoreRoutine);
			}
			this._swapRoutine = null;
			this._restoreRoutine = null;
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x0021835C File Offset: 0x0021655C
		private void ResolveWornRenderer()
		{
			if (this._wornRenderer != null && (this._garmentRoot == null || this._wornRenderer.transform.IsChildOf(this._garmentRoot)) && this._wornRenderer.enabled && this._wornRenderer.gameObject.activeInHierarchy)
			{
				return;
			}
			this._wornRenderer = null;
			if (this._garmentRoot != null)
			{
				this._wornRenderer = this.FindRendererUnder(this._garmentRoot, true) ?? this.FindRendererUnder(this._garmentRoot, false);
				return;
			}
			if (this.playerAnimator == null && this.flashController != null)
			{
				this.playerAnimator = this.flashController.GetComponentInParent<Animator>();
			}
			if (this.playerAnimator == null)
			{
				return;
			}
			this._wornRenderer = this.FindRendererUnder(this.playerAnimator.transform, true);
			if (this._wornRenderer != null)
			{
				return;
			}
			this._wornRenderer = this.FindRendererUnder(this.playerAnimator.transform, false);
		}

		// Token: 0x06005644 RID: 22084 RVA: 0x00218474 File Offset: 0x00216674
		private SkinnedMeshRenderer FindRendererUnder(Transform root, bool activeOnly)
		{
			if (root == null)
			{
				return null;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if ((!(skinnedMeshRenderer.name != this.wornRendererName) || skinnedMeshRenderer.name.StartsWith(this.wornRendererName)) && (!activeOnly || (skinnedMeshRenderer.enabled && skinnedMeshRenderer.gameObject.activeInHierarchy)))
				{
					return skinnedMeshRenderer;
				}
			}
			return null;
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x002184E8 File Offset: 0x002166E8
		private void InheritWornAppearance()
		{
			if (this._wornRenderer == null || this._liftedInstance == null)
			{
				return;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._liftedInstance.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (skinnedMeshRenderer.sharedMaterials.Length == this._wornRenderer.sharedMaterials.Length)
				{
					skinnedMeshRenderer.sharedMaterials = this._wornRenderer.sharedMaterials;
				}
				skinnedMeshRenderer.shadowCastingMode = this._wornRenderer.shadowCastingMode;
			}
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x0021856C File Offset: 0x0021676C
		private void RebindToLiveBones()
		{
			if (this._wornRenderer == null || this._wornRenderer.rootBone == null || this._liftedInstance == null)
			{
				return;
			}
			Transform transform = this._wornRenderer.rootBone;
			while (transform.parent != null && UniversityUniformSkirtFlashCoordinator.IsBoneName(transform.parent.name))
			{
				transform = transform.parent;
			}
			Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
			foreach (Transform transform2 in transform.GetComponentsInChildren<Transform>(true))
			{
				if (!dictionary.ContainsKey(transform2.name))
				{
					dictionary.Add(transform2.name, transform2);
				}
			}
			foreach (Transform transform3 in this._wornRenderer.bones)
			{
				if (transform3 != null && !dictionary.ContainsKey(transform3.name))
				{
					dictionary.Add(transform3.name, transform3);
				}
			}
			this._liveBonesByName = dictionary;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._liftedInstance.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				Transform[] bones = skinnedMeshRenderer.bones;
				Transform[] array2 = new Transform[bones.Length];
				for (int j = 0; j < bones.Length; j++)
				{
					Transform transform4 = bones[j];
					Transform transform5;
					array2[j] = ((transform4 != null && dictionary.TryGetValue(transform4.name, out transform5)) ? transform5 : transform4);
				}
				skinnedMeshRenderer.bones = array2;
				Transform transform6;
				if (skinnedMeshRenderer.rootBone != null && dictionary.TryGetValue(skinnedMeshRenderer.rootBone.name, out transform6))
				{
					skinnedMeshRenderer.rootBone = transform6;
				}
				skinnedMeshRenderer.updateWhenOffscreen = true;
			}
		}

		// Token: 0x06005647 RID: 22087 RVA: 0x00218728 File Offset: 0x00216928
		private void FinalizeLiftedCloth(MagicaCloth[] cloths)
		{
			foreach (MagicaCloth magicaCloth in cloths)
			{
				if (!(magicaCloth == null))
				{
					Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
					foreach (Transform transform in magicaCloth.GetUsedTransform())
					{
						Transform transform2;
						if (!(transform == null) && !dictionary.ContainsKey(transform.name) && this._liveBonesByName != null && this._liveBonesByName.TryGetValue(transform.name, out transform2) && transform != transform2)
						{
							dictionary.Add(transform.name, transform2);
						}
					}
					if (dictionary.Count > 0)
					{
						magicaCloth.ReplaceTransform(dictionary);
					}
					this.ResolveClothColliders(magicaCloth);
					magicaCloth.enabled = true;
					if (magicaCloth.BuildAndRun())
					{
						ClothQualityService.Register(magicaCloth);
					}
					else
					{
						Debug.LogError("[UniversityUniformFlash] BuildAndRun failed for lifted cloth '" + magicaCloth.name + "'.", this);
					}
				}
			}
		}

		// Token: 0x06005648 RID: 22088 RVA: 0x0021883C File Offset: 0x00216A3C
		private void ResolveClothColliders(MagicaCloth cloth)
		{
			if (cloth == null || cloth.SerializeData == null || cloth.SerializeData.colliderCollisionConstraint == null || this.playerAnimator == null)
			{
				return;
			}
			List<ColliderComponent> list = cloth.SerializeData.colliderCollisionConstraint.colliderList;
			if (list == null)
			{
				list = new List<ColliderComponent>();
				cloth.SerializeData.colliderCollisionConstraint.colliderList = list;
			}
			list.RemoveAll((ColliderComponent collider) => collider == null);
			ClothColliderProfile componentInParent = cloth.GetComponentInParent<ClothColliderProfile>(true);
			if (componentInParent == null)
			{
				return;
			}
			ColliderComponent[] componentsInChildren = this.playerAnimator.GetComponentsInChildren<ColliderComponent>(true);
			if (componentInParent.Mode == ClothColliderProfile.SelectionMode.None)
			{
				return;
			}
			if (componentInParent.Mode == ClothColliderProfile.SelectionMode.NamedOnly)
			{
				foreach (string text in componentInParent.ColliderNames)
				{
					ColliderComponent[] array = componentsInChildren;
					int i = 0;
					while (i < array.Length)
					{
						ColliderComponent colliderComponent = array[i];
						if (colliderComponent != null && colliderComponent.name == text)
						{
							if (!list.Contains(colliderComponent))
							{
								list.Add(colliderComponent);
								break;
							}
							break;
						}
						else
						{
							i++;
						}
					}
				}
				return;
			}
			foreach (ColliderComponent colliderComponent2 in componentsInChildren)
			{
				if (colliderComponent2 != null && !list.Contains(colliderComponent2))
				{
					list.Add(colliderComponent2);
				}
			}
		}

		// Token: 0x06005649 RID: 22089 RVA: 0x002189B8 File Offset: 0x00216BB8
		private static bool TryGetNormalizedTime(Animator animator, int layer, string stateName, out float normalized)
		{
			normalized = 0f;
			if (animator == null || !animator.isActiveAndEnabled || animator.runtimeAnimatorController == null)
			{
				return false;
			}
			layer = Mathf.Clamp(layer, 0, animator.layerCount - 1);
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
			if (!currentAnimatorStateInfo.IsName(stateName))
			{
				return false;
			}
			normalized = currentAnimatorStateInfo.normalizedTime;
			return true;
		}

		// Token: 0x0600564A RID: 22090 RVA: 0x00218A1C File Offset: 0x00216C1C
		private static float ResolveClipLength(Animator animator, string clipName, float fallback)
		{
			if (animator == null || animator.runtimeAnimatorController == null)
			{
				return fallback;
			}
			foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
			{
				if (animationClip != null && animationClip.name == clipName && animationClip.length > 0.01f)
				{
					return animationClip.length;
				}
			}
			return fallback;
		}

		// Token: 0x0600564B RID: 22091 RVA: 0x00218A8C File Offset: 0x00216C8C
		private static bool IsBoneName(string name)
		{
			return !string.IsNullOrEmpty(name) && !name.StartsWith("UniversityUniform") && !name.StartsWith("SchoolUniform") && !name.StartsWith("Ciel") && (name.StartsWith("c_") || name.EndsWith(".x") || name.EndsWith(".l") || name.EndsWith(".r") || name.StartsWith("root") || name.Contains("spine") || name.Contains("Spine") || name.Contains("arm") || name.Contains("Arm") || name.Contains("hand") || name.Contains("Hand") || name.Contains("leg") || name.Contains("Leg") || name.Contains("thigh") || name.Contains("Thigh"));
		}

		// Token: 0x040056AE RID: 22190
		[SerializeField]
		private ArmsFlashController flashController;

		// Token: 0x040056AF RID: 22191
		[SerializeField]
		private Animator playerAnimator;

		// Token: 0x040056B0 RID: 22192
		[Header("Skirt swap")]
		[SerializeField]
		private GameObject liftedSkirtPrefab;

		// Token: 0x040056B1 RID: 22193
		[SerializeField]
		private string wornRendererName = "SchoolUniform_Skirt";

		// Token: 0x040056B2 RID: 22194
		[SerializeField]
		private string liftedInstanceName = "UniversityUniform_Skirt_Lifted";

		// Token: 0x040056B3 RID: 22195
		[SerializeField]
		private string liftDetailStateName = "UniversitySkirt_LiftShape";

		// Token: 0x040056B4 RID: 22196
		[SerializeField]
		private string lowerDetailStateName = "UniversitySkirt_LowerShape";

		// Token: 0x040056B5 RID: 22197
		[Range(0.5f, 1f)]
		[SerializeField]
		private float swapAtNormalizedTime = 0.92f;

		// Token: 0x040056B6 RID: 22198
		[Range(0.5f, 1f)]
		[SerializeField]
		private float restoreClothAtNormalizedTime = 0.98f;

		// Token: 0x040056B7 RID: 22199
		private SkinnedMeshRenderer _wornRenderer;

		// Token: 0x040056B8 RID: 22200
		private GameObject _liftedInstance;

		// Token: 0x040056B9 RID: 22201
		private Coroutine _swapRoutine;

		// Token: 0x040056BA RID: 22202
		private Coroutine _restoreRoutine;

		// Token: 0x040056BB RID: 22203
		private readonly Dictionary<MagicaCloth, bool> _wornClothStates = new Dictionary<MagicaCloth, bool>();

		// Token: 0x040056BC RID: 22204
		private Dictionary<string, Transform> _liveBonesByName;

		// Token: 0x040056BD RID: 22205
		private Transform _garmentRoot;
	}
}
