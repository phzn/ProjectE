using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MagicaCloth2;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RayznGames.DressRigger
{
	// Token: 0x02000783 RID: 1923
	[AddComponentMenu("Dress Rigger/Player Dresser")]
	public class PlayerDresser : MonoBehaviour
	{
		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x0600327B RID: 12923 RVA: 0x0011DC29 File Offset: 0x0011BE29
		public SkinnedMeshRenderer CharacterBodyRenderer
		{
			get
			{
				this.Initialize();
				return this.characterBody;
			}
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x0011DC38 File Offset: 0x0011BE38
		private Transform GetOrCreateReplacedBonesTrash()
		{
			if (this.replacedBonesTrash == null)
			{
				GameObject gameObject = new GameObject("__PlayerDresser_ReplacedBones_Trash_" + base.gameObject.name);
				gameObject.SetActive(false);
				this.replacedBonesTrash = gameObject.transform;
			}
			return this.replacedBonesTrash;
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x0011DC87 File Offset: 0x0011BE87
		private void OnDestroy()
		{
			this.attachedClothing.Clear();
			if (this.replacedBonesTrash != null)
			{
				Object.Destroy(this.replacedBonesTrash.gameObject);
				this.replacedBonesTrash = null;
			}
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x0011DCB9 File Offset: 0x0011BEB9
		private void Awake()
		{
			this.Initialize();
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x0011DCC4 File Offset: 0x0011BEC4
		public bool Initialize()
		{
			if (this.isInitialized)
			{
				return true;
			}
			if (this.characterBody == null && this.autoFindBody)
			{
				this.characterBody = this.FindCharacterBody();
			}
			if (this.characterBody == null)
			{
				Debug.LogError("[PlayerDresser] No character body found on " + base.gameObject.name + ". Please assign it manually.");
				return false;
			}
			this.BuildBoneMap();
			this.isInitialized = true;
			if (this.debugMode)
			{
				Debug.Log(string.Format("[PlayerDresser] Initialized with {0} bones from {1}", this.boneMap.Count, this.characterBody.name));
			}
			return true;
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x0011DD6C File Offset: 0x0011BF6C
		private Transform ResolveClothingParent()
		{
			if (this.clothingParent != null)
			{
				return this.clothingParent;
			}
			if (!(this.characterBody != null))
			{
				return base.transform;
			}
			if (!(this.characterBody.rootBone != null))
			{
				return this.characterBody.transform;
			}
			if (!(this.characterBody.rootBone.parent != null))
			{
				return this.characterBody.transform;
			}
			return this.characterBody.rootBone.parent;
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x0011DDF6 File Offset: 0x0011BFF6
		private Transform ResolveAttachmentPoseRoot()
		{
			if (this.attachmentPoseRoot != null)
			{
				return this.attachmentPoseRoot;
			}
			return this.ResolveClothingParent();
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x0011DE14 File Offset: 0x0011C014
		private List<SkinnedMeshRenderer> HideCharacterRenderersForAttach()
		{
			List<SkinnedMeshRenderer> list = new List<SkinnedMeshRenderer>();
			Transform transform = this.ResolveAttachmentPoseRoot();
			Transform transform2 = ((transform != null) ? transform : ((this.characterBody != null) ? this.characterBody.transform : base.transform));
			if (transform2 == null)
			{
				return list;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in transform2.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (skinnedMeshRenderer != null && skinnedMeshRenderer.enabled)
				{
					skinnedMeshRenderer.enabled = false;
					list.Add(skinnedMeshRenderer);
				}
			}
			return list;
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x0011DEAC File Offset: 0x0011C0AC
		private static void RestoreHiddenRenderers(List<SkinnedMeshRenderer> hiddenRenderers)
		{
			if (hiddenRenderers == null)
			{
				return;
			}
			for (int i = 0; i < hiddenRenderers.Count; i++)
			{
				if (hiddenRenderers[i] != null)
				{
					hiddenRenderers[i].enabled = true;
				}
			}
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x0011DEEC File Offset: 0x0011C0EC
		private Animator ResolveCharacterAnimator()
		{
			if (this.characterBody == null)
			{
				return base.GetComponentInParent<Animator>();
			}
			Animator componentInParent = this.characterBody.GetComponentInParent<Animator>();
			if (!(componentInParent != null))
			{
				return base.GetComponentInParent<Animator>();
			}
			return componentInParent;
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x0011DF2C File Offset: 0x0011C12C
		private SkinnedMeshRenderer FindCharacterBody()
		{
			SkinnedMeshRenderer[] componentsInChildren = base.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			SkinnedMeshRenderer skinnedMeshRenderer = (from r in componentsInChildren
				where r != null && r.bones != null && r.bones.Length > 50
				where string.Equals(r.name, "Body_sotai_NSFW", StringComparison.OrdinalIgnoreCase) || string.Equals(r.name, "BMAC_Body", StringComparison.OrdinalIgnoreCase)
				orderby r.bones.Length descending
				select r).FirstOrDefault<SkinnedMeshRenderer>();
			if (skinnedMeshRenderer != null)
			{
				return skinnedMeshRenderer;
			}
			SkinnedMeshRenderer skinnedMeshRenderer2 = (from r in componentsInChildren
				where r != null && r.bones != null && r.bones.Length > 50
				where r.name.IndexOf("body", StringComparison.OrdinalIgnoreCase) >= 0 || (r.sharedMesh != null && r.sharedMesh.name.IndexOf("body", StringComparison.OrdinalIgnoreCase) >= 0)
				orderby r.bones.Length descending
				select r).FirstOrDefault<SkinnedMeshRenderer>();
			if (skinnedMeshRenderer2 != null)
			{
				return skinnedMeshRenderer2;
			}
			SkinnedMeshRenderer skinnedMeshRenderer3 = null;
			int num = 0;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer4 in componentsInChildren)
			{
				if (skinnedMeshRenderer4.bones != null && skinnedMeshRenderer4.bones.Length > num)
				{
					num = skinnedMeshRenderer4.bones.Length;
					skinnedMeshRenderer3 = skinnedMeshRenderer4;
				}
			}
			return skinnedMeshRenderer3;
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x0011E084 File Offset: 0x0011C284
		private void BuildBoneMap()
		{
			this.boneMap.Clear();
			this.normalizedBoneMap.Clear();
			if (this.characterBody == null || this.characterBody.bones == null)
			{
				return;
			}
			foreach (Transform transform in this.characterBody.bones)
			{
				this.RegisterBone(transform);
			}
			if (this.characterBody.rootBone != null)
			{
				this.RegisterBone(this.characterBody.rootBone);
				Transform[] componentsInChildren = this.characterBody.rootBone.GetComponentsInChildren<Transform>(true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					this.RegisterBone(componentsInChildren[j]);
				}
			}
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x0011E13C File Offset: 0x0011C33C
		private void RegisterBone(Transform bone)
		{
			if (bone == null)
			{
				return;
			}
			if (!this.boneMap.ContainsKey(bone.name))
			{
				this.boneMap.Add(bone.name, bone);
			}
			string text = PlayerDresser.NormalizeBoneName(bone.name);
			if (!string.IsNullOrEmpty(text) && !this.normalizedBoneMap.ContainsKey(text))
			{
				this.normalizedBoneMap.Add(text, bone);
			}
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x0011E1A8 File Offset: 0x0011C3A8
		private static string NormalizeBoneName(string boneName)
		{
			if (string.IsNullOrWhiteSpace(boneName))
			{
				return string.Empty;
			}
			string text = boneName.Trim().ToLowerInvariant();
			int num = text.LastIndexOf(':');
			if (num >= 0 && num < text.Length - 1)
			{
				text = text.Substring(num + 1);
			}
			int num2 = text.LastIndexOf('/');
			if (num2 >= 0 && num2 < text.Length - 1)
			{
				text = text.Substring(num2 + 1);
			}
			return text;
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x0011E214 File Offset: 0x0011C414
		public bool AttachClothing(GameObject clothingObject)
		{
			if (!this.Initialize())
			{
				Debug.LogError("[PlayerDresser] Failed to initialize");
				return false;
			}
			if (clothingObject == null)
			{
				Debug.LogError("[PlayerDresser] Clothing object is null");
				return false;
			}
			Animator animator = null;
			bool flag = false;
			bool flag2 = false;
			Transform transform = null;
			Quaternion quaternion = Quaternion.identity;
			bool flag3 = false;
			List<SkinnedMeshRenderer> list = null;
			RigBuilder rigBuilder = null;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6;
			try
			{
				if (this.hideCharacterRenderersDuringAttach)
				{
					list = this.HideCharacterRenderersForAttach();
				}
				transform = this.ResolveAttachmentPoseRoot();
				if (this.normalizeAttachmentRootRotationDuringAttach && transform != null)
				{
					quaternion = transform.localRotation;
					transform.localRotation = Quaternion.identity;
					flag3 = true;
				}
				animator = this.ResolveCharacterAnimator();
				if (animator != null)
				{
					rigBuilder = animator.GetComponent<RigBuilder>();
					if (rigBuilder != null && rigBuilder.enabled)
					{
						flag4 = true;
						flag5 = rigBuilder.graph.IsValid();
						rigBuilder.enabled = false;
						if (rigBuilder.graph.IsValid())
						{
							rigBuilder.Clear();
						}
					}
				}
				if ((this.temporarilyDisableAnimatorDuringAttach || this.forceBindPoseDuringAttach) && animator != null)
				{
					flag2 = animator.enabled;
					flag = true;
					if (this.forceBindPoseDuringAttach && animator.gameObject.activeInHierarchy)
					{
						if (animator.avatar != null && animator.avatar.isValid)
						{
							if (!animator.enabled)
							{
								animator.enabled = true;
							}
							animator.Rebind();
							animator.Update(0f);
						}
						else if (this.debugMode)
						{
							Debug.LogWarning("[PlayerDresser] Skipping Rebind — Animator on '" + animator.gameObject.name + "' has no valid Avatar.");
						}
					}
					if (this.temporarilyDisableAnimatorDuringAttach && animator.enabled)
					{
						animator.enabled = false;
					}
				}
				SkinnedMeshRenderer[] componentsInChildren = clothingObject.GetComponentsInChildren<SkinnedMeshRenderer>();
				if (componentsInChildren.Length == 0)
				{
					Debug.LogWarning("[PlayerDresser] No SkinnedMeshRenderer found in " + clothingObject.name);
					flag6 = false;
				}
				else
				{
					List<MagicaCloth> list2 = this.PrepareMagicaClothsForAttach(clothingObject);
					string text = clothingObject.name + "_" + Guid.NewGuid().ToString().Substring(0, 8);
					PlayerDresser.ClothingData clothingData = new PlayerDresser.ClothingData
					{
						clothingRoot = clothingObject,
						renderers = componentsInChildren,
						originalParent = clothingObject.transform.parent,
						clothingId = text
					};
					Transform transform2 = this.ResolveClothingParent();
					clothingObject.transform.SetParent(transform2, false);
					clothingObject.transform.localPosition = Vector3.zero;
					clothingObject.transform.localRotation = Quaternion.identity;
					clothingObject.transform.localScale = Vector3.one;
					if (componentsInChildren.Length != 0 && componentsInChildren[0].bones != null)
					{
						clothingData.originalBones = componentsInChildren[0].bones.ToArray<Transform>();
						clothingData.originalRootBone = componentsInChildren[0].rootBone;
					}
					HashSet<Transform> hashSet = new HashSet<Transform>();
					HashSet<Transform> hashSet2 = new HashSet<Transform>();
					foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
					{
						this.RebindRendererBones(skinnedMeshRenderer, hashSet, hashSet2);
					}
					if (this.reparentPhysicsBones && hashSet2.Count > 0)
					{
						this.ReparentPhysicsBones(hashSet2);
						clothingData.reparentedPhysicsBones = hashSet2.ToList<Transform>();
					}
					if (this.destroyOriginalBones)
					{
						this.DestroyReplacedBones(hashSet, hashSet2);
					}
					this.DestroyLeftoverDonorSkeleton(clothingObject, hashSet2);
					this.FinalizeMagicaClothsAfterAttach(list2);
					this.attachedClothing.Add(clothingData);
					if (this.debugMode)
					{
						Debug.Log(string.Format("[PlayerDresser] Attached {0} (ID: {1}) with {2} renderer(s), {3} physics bones", new object[]
						{
							clothingObject.name,
							text,
							componentsInChildren.Length,
							clothingData.reparentedPhysicsBones.Count
						}));
					}
					flag6 = true;
				}
			}
			finally
			{
				try
				{
					if (flag3 && transform != null)
					{
						transform.localRotation = quaternion;
					}
					if (flag && animator != null)
					{
						animator.enabled = flag2;
					}
				}
				finally
				{
					try
					{
						if (flag4 && rigBuilder != null)
						{
							rigBuilder.enabled = true;
							if (!Application.isPlaying && flag5 && rigBuilder.isActiveAndEnabled)
							{
								rigBuilder.Build();
							}
						}
					}
					finally
					{
						PlayerDresser.RestoreHiddenRenderers(list);
					}
				}
			}
			return flag6;
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x0011E668 File Offset: 0x0011C868
		public bool AttachClothingRuntime(GameObject clothingObject)
		{
			if (!this.Initialize())
			{
				Debug.LogError("[PlayerDresser] Failed to initialize");
				return false;
			}
			if (clothingObject == null)
			{
				Debug.LogError("[PlayerDresser] Clothing object is null");
				return false;
			}
			SkinnedMeshRenderer[] componentsInChildren = clothingObject.GetComponentsInChildren<SkinnedMeshRenderer>();
			if (componentsInChildren.Length == 0)
			{
				Debug.LogWarning("[PlayerDresser] No SkinnedMeshRenderer found in " + clothingObject.name);
				return false;
			}
			List<MagicaCloth> list = this.PrepareMagicaClothsForAttach(clothingObject);
			string text = clothingObject.name + "_" + Guid.NewGuid().ToString().Substring(0, 8);
			PlayerDresser.ClothingData clothingData = new PlayerDresser.ClothingData
			{
				clothingRoot = clothingObject,
				renderers = componentsInChildren,
				originalParent = clothingObject.transform.parent,
				clothingId = text,
				isRuntimeAttached = true
			};
			Transform transform = this.ResolveClothingParent();
			clothingObject.transform.SetParent(transform, false);
			clothingObject.transform.localPosition = Vector3.zero;
			clothingObject.transform.localRotation = Quaternion.identity;
			clothingObject.transform.localScale = Vector3.one;
			if (componentsInChildren[0].bones != null)
			{
				clothingData.originalBones = componentsInChildren[0].bones.ToArray<Transform>();
				clothingData.originalRootBone = componentsInChildren[0].rootBone;
			}
			HashSet<Transform> hashSet = new HashSet<Transform>();
			HashSet<Transform> hashSet2 = new HashSet<Transform>();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
			{
				this.RebindRendererBones(skinnedMeshRenderer, hashSet, hashSet2);
			}
			if (this.reparentPhysicsBones && hashSet2.Count > 0)
			{
				foreach (Transform transform2 in hashSet2)
				{
					if (!(transform2 == null))
					{
						clothingData.physicsBoneOriginalStates.Add(new PlayerDresser.PhysicsBoneOriginalState
						{
							bone = transform2,
							originalParent = transform2.parent,
							originalLocalPosition = transform2.localPosition,
							originalLocalRotation = transform2.localRotation,
							originalLocalScale = transform2.localScale
						});
					}
				}
				this.ReparentPhysicsBones(hashSet2);
				clothingData.reparentedPhysicsBones = hashSet2.ToList<Transform>();
			}
			List<Transform> list2 = new List<Transform>();
			foreach (Transform transform3 in hashSet)
			{
				if (!(transform3 == null) && !hashSet2.Contains(transform3) && !this.IsChildOfPhysicsBone(transform3, hashSet2) && !this.IsPartOfCharacter(transform3))
				{
					list2.Add(transform3);
				}
			}
			HashSet<Transform> hashSet3 = new HashSet<Transform>(list2);
			Transform transform4 = null;
			foreach (Transform transform5 in list2)
			{
				if (!(transform5 == null) && (!(transform5.parent != null) || !hashSet3.Contains(transform5.parent)))
				{
					if (transform4 == null)
					{
						transform4 = this.GetOrCreateReplacedBonesTrash();
					}
					clothingData.replacedBoneStates.Add(new PlayerDresser.ReplacedBoneState
					{
						bone = transform5,
						originalParent = transform5.parent,
						originalLocalPosition = transform5.localPosition,
						originalLocalRotation = transform5.localRotation,
						originalLocalScale = transform5.localScale
					});
					transform5.SetParent(transform4, false);
				}
			}
			foreach (Transform transform6 in this.CollectLeftoverDonorSkeletonRoots(clothingObject, hashSet2))
			{
				Transform orCreateReplacedBonesTrash = this.GetOrCreateReplacedBonesTrash();
				clothingData.replacedBoneStates.Add(new PlayerDresser.ReplacedBoneState
				{
					bone = transform6,
					originalParent = transform6.parent,
					originalLocalPosition = transform6.localPosition,
					originalLocalRotation = transform6.localRotation,
					originalLocalScale = transform6.localScale
				});
				transform6.SetParent(orCreateReplacedBonesTrash, false);
			}
			this.FinalizeMagicaClothsAfterAttach(list);
			this.attachedClothing.Add(clothingData);
			if (this.debugMode)
			{
				Debug.Log(string.Format("[PlayerDresser] Runtime-attached {0} (ID: {1}): {2} renderer(s), {3} physics bones, {4} replaced-bone subtrees moved to trash", new object[]
				{
					clothingObject.name,
					text,
					componentsInChildren.Length,
					clothingData.reparentedPhysicsBones.Count,
					clothingData.replacedBoneStates.Count
				}));
			}
			return true;
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x0011EB10 File Offset: 0x0011CD10
		public bool DetachClothingRuntime(string clothingId, bool destroyClothingObject = false)
		{
			PlayerDresser.ClothingData clothingData = this.attachedClothing.Find((PlayerDresser.ClothingData c) => c.clothingId == clothingId);
			return clothingData != null && this.DetachClothingRuntimeInternal(clothingData, destroyClothingObject);
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x0011EB50 File Offset: 0x0011CD50
		public bool DetachClothingRuntime(GameObject clothingObject, bool destroyClothingObject = false)
		{
			PlayerDresser.ClothingData clothingData = this.attachedClothing.Find((PlayerDresser.ClothingData c) => c.clothingRoot == clothingObject);
			return clothingData != null && this.DetachClothingRuntimeInternal(clothingData, destroyClothingObject);
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x0011EB90 File Offset: 0x0011CD90
		private bool DetachClothingRuntimeInternal(PlayerDresser.ClothingData data, bool destroyClothingObject)
		{
			if (!data.isRuntimeAttached)
			{
				this.DetachClothingInternal(data);
				return true;
			}
			for (int i = 0; i < data.physicsBoneOriginalStates.Count; i++)
			{
				PlayerDresser.PhysicsBoneOriginalState physicsBoneOriginalState = data.physicsBoneOriginalStates[i];
				if (!(physicsBoneOriginalState.bone == null))
				{
					if (physicsBoneOriginalState.originalParent != null)
					{
						physicsBoneOriginalState.bone.SetParent(physicsBoneOriginalState.originalParent, false);
					}
					physicsBoneOriginalState.bone.localPosition = physicsBoneOriginalState.originalLocalPosition;
					physicsBoneOriginalState.bone.localRotation = physicsBoneOriginalState.originalLocalRotation;
					physicsBoneOriginalState.bone.localScale = physicsBoneOriginalState.originalLocalScale;
				}
			}
			for (int j = 0; j < data.replacedBoneStates.Count; j++)
			{
				PlayerDresser.ReplacedBoneState replacedBoneState = data.replacedBoneStates[j];
				if (!(replacedBoneState.bone == null))
				{
					if (replacedBoneState.originalParent != null)
					{
						replacedBoneState.bone.SetParent(replacedBoneState.originalParent, false);
					}
					replacedBoneState.bone.localPosition = replacedBoneState.originalLocalPosition;
					replacedBoneState.bone.localRotation = replacedBoneState.originalLocalRotation;
					replacedBoneState.bone.localScale = replacedBoneState.originalLocalScale;
				}
			}
			if (data.renderers != null && data.renderers.Length != 0 && data.originalBones != null)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = data.renderers[0];
				if (skinnedMeshRenderer != null)
				{
					skinnedMeshRenderer.bones = data.originalBones;
					if (data.originalRootBone != null)
					{
						skinnedMeshRenderer.rootBone = data.originalRootBone;
					}
				}
			}
			if (data.clothingRoot != null)
			{
				if (data.originalParent != null)
				{
					data.clothingRoot.transform.SetParent(data.originalParent, true);
				}
				else
				{
					data.clothingRoot.transform.SetParent(null, true);
				}
				if (destroyClothingObject)
				{
					if (Application.isPlaying)
					{
						Object.Destroy(data.clothingRoot);
					}
					else
					{
						Object.DestroyImmediate(data.clothingRoot);
					}
				}
			}
			this.attachedClothing.Remove(data);
			if (this.debugMode)
			{
				Debug.Log(string.Format("[PlayerDresser] Runtime-detached clothing (ID: {0}), destroyed={1}", data.clothingId, destroyClothingObject));
			}
			return true;
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x0011EDA8 File Offset: 0x0011CFA8
		public List<PlayerDresser.AttachedClothingInfo> GetAttachedClothingInfo()
		{
			List<PlayerDresser.AttachedClothingInfo> list = new List<PlayerDresser.AttachedClothingInfo>(this.attachedClothing.Count);
			for (int i = 0; i < this.attachedClothing.Count; i++)
			{
				PlayerDresser.ClothingData clothingData = this.attachedClothing[i];
				list.Add(new PlayerDresser.AttachedClothingInfo
				{
					clothingId = clothingData.clothingId,
					clothingRoot = clothingData.clothingRoot,
					isRuntimeAttached = clothingData.isRuntimeAttached,
					rendererCount = ((clothingData.renderers != null) ? clothingData.renderers.Length : 0),
					reparentedPhysicsBoneCount = ((clothingData.reparentedPhysicsBones != null) ? clothingData.reparentedPhysicsBones.Count : 0)
				});
			}
			return list;
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x0011EE5C File Offset: 0x0011D05C
		private void RebindRendererBones(SkinnedMeshRenderer renderer, HashSet<Transform> bonesToDestroy, HashSet<Transform> physicsBonesToReparent)
		{
			if (renderer == null || renderer.bones == null)
			{
				return;
			}
			Transform[] bones = renderer.bones;
			Transform[] array = new Transform[bones.Length];
			int num = 0;
			int num2 = 0;
			HashSet<Transform> hashSet = new HashSet<Transform>(bones.Where<Transform>((Transform b) => b != null));
			if (bones.Length == 0 && this.debugMode)
			{
				Debug.LogWarning("[PlayerDresser] Renderer '" + renderer.name + "' has 0 bones in SMR.bones. Check FBX import/rig setup for this clothing prefab.");
			}
			for (int i = 0; i < bones.Length; i++)
			{
				Transform transform = bones[i];
				Transform transform2;
				if (transform == null)
				{
					array[i] = null;
				}
				else if (this.TryGetCharacterBone(transform.name, out transform2))
				{
					array[i] = transform2;
					num++;
					if (transform != transform2)
					{
						bonesToDestroy.Add(transform);
					}
				}
				else
				{
					array[i] = transform;
					if (this.IsPhysicsBone(transform.name))
					{
						num2++;
						if (!this.IsPhysicsBoneChild(transform, hashSet))
						{
							physicsBonesToReparent.Add(transform);
						}
						if (this.debugMode)
						{
							Debug.Log("[PlayerDresser] Physics bone preserved: '" + transform.name + "' in " + renderer.name);
						}
					}
					else if (this.debugMode && !transform.name.EndsWith("_end"))
					{
						Debug.LogWarning("[PlayerDresser] Unmatched bone (not physics): '" + transform.name + "' in " + renderer.name);
					}
				}
			}
			renderer.bones = array;
			Transform transform3;
			if (renderer.rootBone != null && this.boneMap.TryGetValue(renderer.rootBone.name, out transform3))
			{
				renderer.rootBone = transform3;
			}
			else if (this.characterBody.rootBone != null)
			{
				renderer.rootBone = this.characterBody.rootBone;
			}
			renderer.localBounds = this.characterBody.localBounds;
			renderer.updateWhenOffscreen = true;
			if (num == 0 && bones.Length != 0 && this.debugMode)
			{
				int num3 = Mathf.Min(5, bones.Length);
				string[] array2 = new string[num3];
				for (int j = 0; j < num3; j++)
				{
					array2[j] = ((bones[j] != null) ? bones[j].name : "<null>");
				}
				Debug.LogWarning("[PlayerDresser] No character bone matches found for renderer '" + renderer.name + "'. Example clothing bones: " + string.Join(", ", array2));
			}
			if (this.debugMode)
			{
				Debug.Log(string.Format("[PlayerDresser] Rebound {0}: {1} matched, {2} physics bones preserved", renderer.name, num, num2));
			}
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x0011F0F8 File Offset: 0x0011D2F8
		private bool TryGetCharacterBone(string clothingBoneName, out Transform characterBone)
		{
			characterBone = null;
			if (string.IsNullOrWhiteSpace(clothingBoneName))
			{
				return false;
			}
			if (this.boneMap.TryGetValue(clothingBoneName, out characterBone))
			{
				return true;
			}
			string text = PlayerDresser.NormalizeBoneName(clothingBoneName);
			return !string.IsNullOrEmpty(text) && this.normalizedBoneMap.TryGetValue(text, out characterBone);
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x0011F144 File Offset: 0x0011D344
		private bool IsPhysicsBone(string boneName)
		{
			if (string.IsNullOrEmpty(boneName))
			{
				return false;
			}
			string text = boneName.ToLowerInvariant();
			foreach (string text2 in this.physicsBonePatterns)
			{
				if (text.Contains(text2.ToLowerInvariant()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x0011F18C File Offset: 0x0011D38C
		private bool IsPhysicsBoneChild(Transform bone, HashSet<Transform> allBones)
		{
			Transform transform = bone.parent;
			while (transform != null)
			{
				if (!allBones.Contains(transform))
				{
					transform = transform.parent;
				}
				else
				{
					if (this.IsPhysicsBone(transform.name))
					{
						return true;
					}
					transform = transform.parent;
				}
			}
			return false;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x0011F1D8 File Offset: 0x0011D3D8
		private void ReparentPhysicsBones(HashSet<Transform> physicsBones)
		{
			foreach (Transform transform in physicsBones)
			{
				if (!(transform == null))
				{
					Transform parent = transform.parent;
					Transform transform2 = this.FindBestParentBone(transform);
					if (transform2 != null)
					{
						Transform transform3;
						bool flag = parent != null && this.TryGetCharacterBone(parent.name, out transform3) && transform3 == transform2;
						if (flag)
						{
							Vector3 localPosition = transform.localPosition;
							Quaternion localRotation = transform.localRotation;
							Vector3 localScale = transform.localScale;
							transform.SetParent(transform2, false);
							transform.localPosition = localPosition;
							transform.localRotation = localRotation;
							transform.localScale = localScale;
						}
						else
						{
							Vector3 position = transform.position;
							Quaternion rotation = transform.rotation;
							transform.SetParent(transform2);
							transform.position = position;
							transform.rotation = rotation;
						}
						if (transform.parent != transform2)
						{
							Debug.LogError("[PlayerDresser] Reparent di '" + transform.name + "' FALLITO (istanza di prefab collegata?). Il capo va attaccato da un'istanza spacchettata o da un clone (Object.Instantiate). Usa i bottoni del PlayerDresser (gestiscono l'unpack) e non trascinare il prefab in scena a mano.");
						}
						else if (this.debugMode)
						{
							Debug.Log(string.Format("[PlayerDresser] Reparented physics bone '{0}' under '{1}' (localTRS={2})", transform.name, transform2.name, flag));
						}
					}
				}
			}
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x0011F338 File Offset: 0x0011D538
		private Transform FindBestParentBone(Transform physicsBone)
		{
			if (physicsBone.parent != null)
			{
				string name = physicsBone.parent.name;
				Transform transform;
				if (this.boneMap.TryGetValue(name, out transform))
				{
					return transform;
				}
				foreach (KeyValuePair<string, Transform> keyValuePair in this.boneMap)
				{
					if (keyValuePair.Key.Contains(name) || name.Contains(keyValuePair.Key))
					{
						return keyValuePair.Value;
					}
				}
			}
			Transform transform2;
			if (this.boneMap.TryGetValue(this.defaultPhysicsBoneParent, out transform2))
			{
				return transform2;
			}
			foreach (string text in new string[] { "Hips", "hips", "pelvis", "Pelvis", "root.x", "Root" })
			{
				Transform transform3;
				if (this.boneMap.TryGetValue(text, out transform3))
				{
					return transform3;
				}
			}
			SkinnedMeshRenderer skinnedMeshRenderer = this.characterBody;
			if (skinnedMeshRenderer == null)
			{
				return null;
			}
			return skinnedMeshRenderer.rootBone;
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x0011F470 File Offset: 0x0011D670
		private void DestroyReplacedBones(HashSet<Transform> bonesToDestroy, HashSet<Transform> physicsBones)
		{
			List<Transform> list = new List<Transform>();
			foreach (Transform transform in bonesToDestroy)
			{
				if (!(transform == null) && !physicsBones.Contains(transform) && !this.IsChildOfPhysicsBone(transform, physicsBones) && !this.IsPartOfCharacter(transform))
				{
					list.Add(transform);
				}
			}
			list.Sort((Transform a, Transform b) => this.GetDepth(b).CompareTo(this.GetDepth(a)));
			foreach (Transform transform2 in list)
			{
				if (transform2 != null)
				{
					if (this.debugMode)
					{
						Debug.Log("[PlayerDresser] Destroying replaced bone: " + transform2.name);
					}
					if (Application.isPlaying)
					{
						transform2.SetParent(this.GetOrCreateReplacedBonesTrash(), false);
						Object.Destroy(transform2.gameObject);
					}
					else
					{
						Object.DestroyImmediate(transform2.gameObject);
					}
				}
			}
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x0011F58C File Offset: 0x0011D78C
		private void ExtractRenderersFromDonorSkeleton(GameObject clothingObject)
		{
			if (clothingObject == null)
			{
				return;
			}
			foreach (Renderer renderer in clothingObject.GetComponentsInChildren<Renderer>(true))
			{
				if (!(renderer == null) && !(renderer.transform == clothingObject.transform))
				{
					bool flag = false;
					Transform transform = renderer.transform.parent;
					while (transform != null && transform != clothingObject.transform)
					{
						if (!this.IsPartOfCharacter(transform) && this.NameCollidesWithCharacterBone(transform.name))
						{
							flag = true;
							break;
						}
						transform = transform.parent;
					}
					if (flag)
					{
						renderer.transform.SetParent(clothingObject.transform, true);
						if (this.debugMode)
						{
							Debug.Log("[PlayerDresser] Renderer '" + renderer.name + "' estratto dallo scheletro donatore di " + clothingObject.name);
						}
					}
				}
			}
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x0011F674 File Offset: 0x0011D874
		private List<Transform> CollectLeftoverDonorSkeletonRoots(GameObject clothingObject, HashSet<Transform> preservedPhysicsBones)
		{
			List<Transform> list = new List<Transform>();
			if (clothingObject == null)
			{
				return list;
			}
			this.ExtractRenderersFromDonorSkeleton(clothingObject);
			List<Transform> list2 = new List<Transform>();
			foreach (Transform transform in clothingObject.GetComponentsInChildren<Transform>(true))
			{
				if (!(transform == null) && !(transform == clothingObject.transform) && !this.IsPartOfCharacter(transform) && this.NameCollidesWithCharacterBone(transform.name) && !preservedPhysicsBones.Contains(transform) && !this.IsChildOfPhysicsBone(transform, preservedPhysicsBones) && transform.GetComponentsInChildren<Renderer>(true).Length == 0)
				{
					list2.Add(transform);
				}
			}
			HashSet<Transform> hashSet = new HashSet<Transform>(list2);
			foreach (Transform transform2 in list2)
			{
				bool flag = false;
				Transform transform3 = transform2.parent;
				while (transform3 != null && transform3 != clothingObject.transform)
				{
					if (hashSet.Contains(transform3))
					{
						flag = true;
						break;
					}
					transform3 = transform3.parent;
				}
				if (!flag)
				{
					list.Add(transform2);
				}
			}
			return list;
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x0011F7A8 File Offset: 0x0011D9A8
		private void DestroyLeftoverDonorSkeleton(GameObject clothingObject, HashSet<Transform> preservedPhysicsBones)
		{
			foreach (Transform transform in this.CollectLeftoverDonorSkeletonRoots(clothingObject, preservedPhysicsBones))
			{
				if (this.debugMode)
				{
					Debug.Log("[PlayerDresser] Rimosso scheletro donatore residuo: '" + transform.name + "' in " + clothingObject.name);
				}
				if (Application.isPlaying)
				{
					transform.SetParent(this.GetOrCreateReplacedBonesTrash(), false);
					Object.Destroy(transform.gameObject);
				}
				else
				{
					Object.DestroyImmediate(transform.gameObject);
				}
			}
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x0011F84C File Offset: 0x0011DA4C
		private bool NameCollidesWithCharacterBone(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			if (this.boneMap.ContainsKey(name))
			{
				return true;
			}
			string text = PlayerDresser.NormalizeBoneName(name);
			return !string.IsNullOrEmpty(text) && this.normalizedBoneMap.ContainsKey(text);
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x0011F890 File Offset: 0x0011DA90
		private bool IsChildOfPhysicsBone(Transform t, HashSet<Transform> physicsBones)
		{
			Transform transform = t.parent;
			while (transform != null)
			{
				if (physicsBones.Contains(transform))
				{
					return true;
				}
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x0011F8C4 File Offset: 0x0011DAC4
		private int GetDepth(Transform t)
		{
			int num = 0;
			while (t.parent != null)
			{
				num++;
				t = t.parent;
			}
			return num;
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x0011F8F0 File Offset: 0x0011DAF0
		private bool IsPartOfCharacter(Transform t)
		{
			return this.boneMap.ContainsValue(t);
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x0011F900 File Offset: 0x0011DB00
		public void DetachClothing(GameObject clothingObject)
		{
			PlayerDresser.ClothingData clothingData = this.attachedClothing.Find((PlayerDresser.ClothingData c) => c.clothingRoot == clothingObject);
			if (clothingData != null)
			{
				this.DetachClothingInternal(clothingData);
			}
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x0011F93C File Offset: 0x0011DB3C
		public void DetachClothingById(string clothingId)
		{
			PlayerDresser.ClothingData clothingData = this.attachedClothing.Find((PlayerDresser.ClothingData c) => c.clothingId == clothingId);
			if (clothingData != null)
			{
				this.DetachClothingInternal(clothingData);
			}
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x0011F978 File Offset: 0x0011DB78
		private void DetachClothingInternal(PlayerDresser.ClothingData data)
		{
			foreach (Transform transform in data.reparentedPhysicsBones)
			{
				if (transform != null)
				{
					if (this.debugMode)
					{
						Debug.Log("[PlayerDresser] Destroying physics bone: " + transform.name);
					}
					if (Application.isPlaying)
					{
						transform.SetParent(this.GetOrCreateReplacedBonesTrash(), false);
						Object.Destroy(transform.gameObject);
					}
					else
					{
						Object.DestroyImmediate(transform.gameObject);
					}
				}
			}
			if (data.clothingRoot != null)
			{
				if (this.debugMode)
				{
					Debug.Log("[PlayerDresser] Detaching clothing: " + data.clothingRoot.name);
				}
				if (Application.isPlaying)
				{
					data.clothingRoot.transform.SetParent(this.GetOrCreateReplacedBonesTrash(), false);
					Object.Destroy(data.clothingRoot);
				}
				else
				{
					Object.DestroyImmediate(data.clothingRoot);
				}
			}
			this.attachedClothing.Remove(data);
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x0011FA8C File Offset: 0x0011DC8C
		public bool IsClothingAttached(string clothingId)
		{
			return this.attachedClothing.Exists((PlayerDresser.ClothingData c) => c.clothingId == clothingId);
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x0011FABD File Offset: 0x0011DCBD
		public List<string> GetAttachedClothingIds()
		{
			return this.attachedClothing.Select<PlayerDresser.ClothingData, string>((PlayerDresser.ClothingData c) => c.clothingId).ToList<string>();
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x0011FAF0 File Offset: 0x0011DCF0
		public void DetachAllClothing()
		{
			foreach (PlayerDresser.ClothingData clothingData in this.attachedClothing.ToList<PlayerDresser.ClothingData>())
			{
				this.DetachClothingInternal(clothingData);
			}
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x0011FB48 File Offset: 0x0011DD48
		public SkinnedMeshRenderer GetCharacterBody()
		{
			return this.characterBody;
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x0011FB50 File Offset: 0x0011DD50
		public Dictionary<string, Transform> GetBoneMap()
		{
			if (!this.isInitialized)
			{
				this.Initialize();
			}
			return new Dictionary<string, Transform>(this.boneMap);
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x0011FB6C File Offset: 0x0011DD6C
		public void SetCharacterBody(SkinnedMeshRenderer body)
		{
			this.characterBody = body;
			this.isInitialized = false;
			this.Initialize();
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x0011FB83 File Offset: 0x0011DD83
		public void SetClothingParent(Transform parent)
		{
			this.clothingParent = parent;
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x0011FB8C File Offset: 0x0011DD8C
		public void RefreshBoneMap()
		{
			this.BuildBoneMap();
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x0011FB94 File Offset: 0x0011DD94
		private List<MagicaCloth> PrepareMagicaClothsForAttach(GameObject clothingObject)
		{
			List<MagicaCloth> list = new List<MagicaCloth>();
			if (!Application.isPlaying)
			{
				return list;
			}
			foreach (MagicaCloth magicaCloth in clothingObject.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (!(magicaCloth == null))
				{
					MagicaCloth magicaCloth2 = magicaCloth;
					if (magicaCloth2.Process != null && magicaCloth2.Process.IsState(5))
					{
						magicaCloth2 = this.RecreateMagicaCloth(magicaCloth2);
						if (magicaCloth2 == null)
						{
							goto IL_0090;
						}
					}
					magicaCloth2.Initialize();
					magicaCloth2.DisableAutoBuild();
					list.Add(magicaCloth2);
					if (this.debugMode)
					{
						Debug.Log("[PlayerDresser] MagicaCloth '" + magicaCloth2.name + "': auto-build sospesa in attesa del rebind.");
					}
				}
				IL_0090:;
			}
			return list;
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x0011FC3C File Offset: 0x0011DE3C
		private void FinalizeMagicaClothsAfterAttach(List<MagicaCloth> cloths)
		{
			if (cloths == null || cloths.Count == 0 || !Application.isPlaying)
			{
				return;
			}
			foreach (MagicaCloth magicaCloth in cloths)
			{
				if (!(magicaCloth == null))
				{
					Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
					foreach (Transform transform in magicaCloth.GetUsedTransform())
					{
						Transform transform2;
						if (!(transform == null) && !dictionary.ContainsKey(transform.name) && this.TryGetCharacterBone(transform.name, out transform2) && transform != transform2)
						{
							dictionary.Add(transform.name, transform2);
						}
					}
					if (dictionary.Count > 0)
					{
						magicaCloth.ReplaceTransform(dictionary);
					}
					this.ApplyColliderProfile(magicaCloth);
					magicaCloth.enabled = true;
					if (magicaCloth.BuildAndRun())
					{
						ClothQualityService.Register(magicaCloth);
						if (this.debugMode)
						{
							Debug.Log(string.Format("[PlayerDresser] MagicaCloth '{0}': {1} transform rimappati, build avviata.", magicaCloth.name, dictionary.Count));
						}
					}
					else
					{
						Debug.LogError("[PlayerDresser] MagicaCloth '" + magicaCloth.name + "': BuildAndRun fallita dopo l'attach. Controlla source renderer / paint map / root bones.");
					}
				}
			}
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x0011FDB8 File Offset: 0x0011DFB8
		private void ApplyColliderProfile(MagicaCloth cloth)
		{
			if (cloth == null || cloth.SerializeData == null || cloth.SerializeData.colliderCollisionConstraint == null)
			{
				return;
			}
			ClothColliderProfile componentInParent = cloth.GetComponentInParent<ClothColliderProfile>(true);
			ClothColliderProfile.SelectionMode selectionMode = ((componentInParent != null) ? componentInParent.Mode : ClothColliderProfile.SelectionMode.AllPlayerColliders);
			List<ColliderComponent> list = cloth.SerializeData.colliderCollisionConstraint.colliderList;
			if (list == null)
			{
				list = new List<ColliderComponent>();
				cloth.SerializeData.colliderCollisionConstraint.colliderList = list;
			}
			list.RemoveAll((ColliderComponent c) => c == null);
			if (selectionMode == ClothColliderProfile.SelectionMode.None)
			{
				return;
			}
			ColliderComponent[] array = this.CollectCharacterMagicaColliders();
			int num = 0;
			int num2 = 0;
			if (selectionMode == ClothColliderProfile.SelectionMode.NamedOnly)
			{
				using (IEnumerator<string> enumerator = componentInParent.ColliderNames.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						if (!string.IsNullOrEmpty(text))
						{
							ColliderComponent colliderComponent = null;
							for (int i = 0; i < array.Length; i++)
							{
								if (array[i] != null && array[i].name == text)
								{
									colliderComponent = array[i];
									break;
								}
							}
							if (colliderComponent == null)
							{
								num2++;
								Debug.LogWarning(string.Concat(new string[] { "[PlayerDresser] ClothColliderProfile di '", cloth.name, "': collider '", text, "' non trovato sul personaggio." }));
							}
							else if (!list.Contains(colliderComponent))
							{
								list.Add(colliderComponent);
								num++;
							}
						}
					}
					goto IL_01BA;
				}
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != null && !list.Contains(array[j]))
				{
					list.Add(array[j]);
					num++;
				}
			}
			IL_01BA:
			if (this.debugMode)
			{
				Debug.Log(string.Format("[PlayerDresser] Collider profile su '{0}': mode={1}, aggiunti {2}, mancanti {3}, totale lista {4}", new object[] { cloth.name, selectionMode, num, num2, list.Count }));
			}
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x0011FFE0 File Offset: 0x0011E1E0
		private ColliderComponent[] CollectCharacterMagicaColliders()
		{
			Animator animator = this.ResolveCharacterAnimator();
			ColliderComponent[] componentsInChildren = ((animator != null) ? animator.transform : base.transform.root).GetComponentsInChildren<ColliderComponent>(true);
			List<ColliderComponent> list = new List<ColliderComponent>(componentsInChildren.Length);
			foreach (ColliderComponent colliderComponent in componentsInChildren)
			{
				if (!(colliderComponent == null))
				{
					bool flag = false;
					for (int j = 0; j < this.attachedClothing.Count; j++)
					{
						GameObject clothingRoot = this.attachedClothing[j].clothingRoot;
						if (clothingRoot != null && colliderComponent.transform.IsChildOf(clothingRoot.transform))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						list.Add(colliderComponent);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x001200A4 File Offset: 0x0011E2A4
		private MagicaCloth RecreateMagicaCloth(MagicaCloth oldCloth)
		{
			MagicaCloth magicaCloth = oldCloth.gameObject.AddComponent<MagicaCloth>();
			magicaCloth.enabled = false;
			magicaCloth.SerializeData.Import(oldCloth.SerializeData, true);
			FieldInfo field = typeof(MagicaCloth).GetField("serializeData2", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field != null)
			{
				field.SetValue(magicaCloth, field.GetValue(oldCloth));
			}
			if (this.debugMode)
			{
				Debug.Log("[PlayerDresser] MagicaCloth '" + oldCloth.name + "' era già buildato: componente ricreato per il re-build post-attach.");
			}
			Object.Destroy(oldCloth);
			return magicaCloth;
		}

		// Token: 0x04003333 RID: 13107
		[Header("Character Reference")]
		[Tooltip("The main body SkinnedMeshRenderer of the character (used to get bone references)")]
		[SerializeField]
		private SkinnedMeshRenderer characterBody;

		// Token: 0x04003334 RID: 13108
		[Header("Settings")]
		[Tooltip("Automatically find character body if not assigned")]
		[SerializeField]
		private bool autoFindBody = true;

		// Token: 0x04003335 RID: 13109
		[Tooltip("Destroy original clothing bones after rebinding (recommended for clean hierarchy)")]
		[SerializeField]
		private bool destroyOriginalBones = true;

		// Token: 0x04003336 RID: 13110
		[Tooltip("Reparent physics bones (skirt, hair, etc.) under the appropriate character bone")]
		[SerializeField]
		private bool reparentPhysicsBones = true;

		// Token: 0x04003337 RID: 13111
		[Tooltip("Default parent bone name for physics bones that can't find their parent")]
		[SerializeField]
		private string defaultPhysicsBoneParent = "Hips";

		// Token: 0x04003338 RID: 13112
		[Tooltip("Patterns to identify physics bones that should be preserved (case-insensitive)")]
		[SerializeField]
		private string[] physicsBonePatterns = new string[] { "skirt", "cloth", "ribbon", "cape", "tail", "chain", "hair", "headband" };

		// Token: 0x04003339 RID: 13113
		[Tooltip("Show debug logs")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x0400333A RID: 13114
		[Header("Attachment")]
		[Tooltip("Optional parent for spawned clothing roots. If null, PlayerDresser transform is used.")]
		[SerializeField]
		private Transform clothingParent;

		// Token: 0x0400333B RID: 13115
		[Tooltip("Temporarily disable the character Animator while attaching clothing to avoid pose-dependent offsets.")]
		[SerializeField]
		private bool temporarilyDisableAnimatorDuringAttach = true;

		// Token: 0x0400333C RID: 13116
		[Tooltip("Force Animator Rebind+Update(0) before attach to normalize skeleton pose (bind/T-pose-like) deterministically.")]
		[SerializeField]
		private bool forceBindPoseDuringAttach = true;

		// Token: 0x0400333D RID: 13117
		[Tooltip("Temporarily disable visible character skinned mesh renderers while preparing and attaching clothing.")]
		[SerializeField]
		private bool hideCharacterRenderersDuringAttach = true;

		// Token: 0x0400333E RID: 13118
		[Tooltip("Optional model root used for temporary pose normalization during attach. If null, clothingParent/model root fallback is used.")]
		[SerializeField]
		private Transform attachmentPoseRoot;

		// Token: 0x0400333F RID: 13119
		[Tooltip("Temporarily sets attachment pose root local rotation to identity during attach, then restores it.")]
		[SerializeField]
		private bool normalizeAttachmentRootRotationDuringAttach = true;

		// Token: 0x04003340 RID: 13120
		private Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();

		// Token: 0x04003341 RID: 13121
		private Dictionary<string, Transform> normalizedBoneMap = new Dictionary<string, Transform>();

		// Token: 0x04003342 RID: 13122
		private List<PlayerDresser.ClothingData> attachedClothing = new List<PlayerDresser.ClothingData>();

		// Token: 0x04003343 RID: 13123
		private bool isInitialized;

		// Token: 0x04003344 RID: 13124
		private Transform replacedBonesTrash;

		// Token: 0x02000784 RID: 1924
		[Serializable]
		private class ClothingData
		{
			// Token: 0x04003345 RID: 13125
			public GameObject clothingRoot;

			// Token: 0x04003346 RID: 13126
			public SkinnedMeshRenderer[] renderers;

			// Token: 0x04003347 RID: 13127
			public Transform[] originalBones;

			// Token: 0x04003348 RID: 13128
			public Transform originalRootBone;

			// Token: 0x04003349 RID: 13129
			public Transform originalParent;

			// Token: 0x0400334A RID: 13130
			public List<Transform> reparentedPhysicsBones = new List<Transform>();

			// Token: 0x0400334B RID: 13131
			public string clothingId;

			// Token: 0x0400334C RID: 13132
			public bool isRuntimeAttached;

			// Token: 0x0400334D RID: 13133
			public List<PlayerDresser.ReplacedBoneState> replacedBoneStates = new List<PlayerDresser.ReplacedBoneState>();

			// Token: 0x0400334E RID: 13134
			public List<PlayerDresser.PhysicsBoneOriginalState> physicsBoneOriginalStates = new List<PlayerDresser.PhysicsBoneOriginalState>();
		}

		// Token: 0x02000785 RID: 1925
		private struct PhysicsBoneOriginalState
		{
			// Token: 0x0400334F RID: 13135
			public Transform bone;

			// Token: 0x04003350 RID: 13136
			public Transform originalParent;

			// Token: 0x04003351 RID: 13137
			public Vector3 originalLocalPosition;

			// Token: 0x04003352 RID: 13138
			public Quaternion originalLocalRotation;

			// Token: 0x04003353 RID: 13139
			public Vector3 originalLocalScale;
		}

		// Token: 0x02000786 RID: 1926
		private struct ReplacedBoneState
		{
			// Token: 0x04003354 RID: 13140
			public Transform bone;

			// Token: 0x04003355 RID: 13141
			public Transform originalParent;

			// Token: 0x04003356 RID: 13142
			public Vector3 originalLocalPosition;

			// Token: 0x04003357 RID: 13143
			public Quaternion originalLocalRotation;

			// Token: 0x04003358 RID: 13144
			public Vector3 originalLocalScale;
		}

		// Token: 0x02000787 RID: 1927
		public struct AttachedClothingInfo
		{
			// Token: 0x04003359 RID: 13145
			public string clothingId;

			// Token: 0x0400335A RID: 13146
			public GameObject clothingRoot;

			// Token: 0x0400335B RID: 13147
			public bool isRuntimeAttached;

			// Token: 0x0400335C RID: 13148
			public int rendererCount;

			// Token: 0x0400335D RID: 13149
			public int reparentedPhysicsBoneCount;
		}
	}
}
