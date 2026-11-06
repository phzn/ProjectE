using System;
using System.Collections;
using System.Collections.Generic;
using Features.Flash;
using MagicaCloth2;
using ProjectExhibitionist.Core.Rendering;
using RayznGames.DressRigger;
using UnityEngine;
using UnityEngine.Events;

namespace Features.RainCoat
{
	// Token: 0x02000DBE RID: 3518
	public class RainCoatOpenSwap : MonoBehaviour
	{
		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x060067CC RID: 26572 RVA: 0x00298361 File Offset: 0x00296561
		public bool IsOpenCoatVisible
		{
			get
			{
				return this._openInstance != null && this._openInstance.activeInHierarchy;
			}
		}

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x060067CD RID: 26573 RVA: 0x0029837E File Offset: 0x0029657E
		private string WornObjectName
		{
			get
			{
				if (!(this.coatFlash != null) || this.coatFlash.Config == null)
				{
					return "RainCoat_A";
				}
				return this.coatFlash.Config.wornObjectName;
			}
		}

		// Token: 0x060067CE RID: 26574 RVA: 0x002983B4 File Offset: 0x002965B4
		private void Awake()
		{
			if (this.coatFlash == null)
			{
				this.coatFlash = this.ResolveCoatFlash();
			}
			if (this.animator == null && this.coatFlash != null)
			{
				this.animator = this.coatFlash.GetComponentInParent<Animator>();
			}
		}

		// Token: 0x060067CF RID: 26575 RVA: 0x00298408 File Offset: 0x00296608
		private void OnEnable()
		{
			if (this.coatFlash == null)
			{
				this.coatFlash = this.ResolveCoatFlash();
			}
			if (this.coatFlash != null)
			{
				this.coatFlash.OnOpened.AddListener(new UnityAction(this.OnFlashOpened));
				this.coatFlash.OnClosed.AddListener(new UnityAction(this.OnFlashClosed));
			}
		}

		// Token: 0x060067D0 RID: 26576 RVA: 0x00298478 File Offset: 0x00296678
		private void OnDisable()
		{
			if (this.coatFlash != null)
			{
				this.coatFlash.OnOpened.RemoveListener(new UnityAction(this.OnFlashOpened));
				this.coatFlash.OnClosed.RemoveListener(new UnityAction(this.OnFlashClosed));
			}
			if (this._swapRoutine != null)
			{
				base.StopCoroutine(this._swapRoutine);
				this._swapRoutine = null;
			}
			this.RestoreOriginalCoat();
		}

		// Token: 0x060067D1 RID: 26577 RVA: 0x002984EC File Offset: 0x002966EC
		private ArmsFlashController ResolveCoatFlash()
		{
			foreach (ArmsFlashController armsFlashController in base.GetComponents<ArmsFlashController>())
			{
				if (armsFlashController.Config != null && armsFlashController.Config.flashLayerName == this.coatFlashLayerName)
				{
					return armsFlashController;
				}
			}
			Debug.LogWarning(string.Concat(new string[] { "[RainCoatOpenSwap] Nessun ArmsFlashController con layer '", this.coatFlashLayerName, "' su '", base.name, "'. Open-swap disattivato." }), this);
			return null;
		}

		// Token: 0x060067D2 RID: 26578 RVA: 0x00298570 File Offset: 0x00296770
		public void OnFlashOpened()
		{
			if (this.coatFlash == null || !this.coatFlash.IsWorn)
			{
				return;
			}
			if (this._swapRoutine != null)
			{
				base.StopCoroutine(this._swapRoutine);
			}
			this._swapRoutine = base.StartCoroutine(this.ShowOpenAfterOpenClip());
		}

		// Token: 0x060067D3 RID: 26579 RVA: 0x002985BF File Offset: 0x002967BF
		public void OnFlashClosed()
		{
			if (this._swapRoutine != null)
			{
				base.StopCoroutine(this._swapRoutine);
				this._swapRoutine = null;
			}
			this.RestoreOriginalCoat();
		}

		// Token: 0x060067D4 RID: 26580 RVA: 0x002985E2 File Offset: 0x002967E2
		public void OnGarmentRestored()
		{
			this._wornCoatContainer = null;
			if (this._openInstance != null)
			{
				Object.Destroy(this._openInstance);
				this._openInstance = null;
			}
		}

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x060067D5 RID: 26581 RVA: 0x0029860B File Offset: 0x0029680B
		private bool IsOpen
		{
			get
			{
				return this.coatFlash != null && this.coatFlash.IsOpen;
			}
		}

		// Token: 0x060067D6 RID: 26582 RVA: 0x00298628 File Offset: 0x00296828
		private IEnumerator ShowOpenAfterOpenClip()
		{
			Animator detail = ((this.coatFlash != null) ? this.coatFlash.EnsureDetailAnimator() : null);
			int detailLayer = ((this.coatFlash != null && this.coatFlash.Config != null) ? this.coatFlash.Config.detailLayerIndex : 0);
			float clipLen = this.ResolveOpenClipLength(detail);
			float threshold = Mathf.Clamp01(this.swapAtNormalizedTime);
			float t = 0f;
			while (this.IsOpen)
			{
				float num;
				if (this.TryGetOpenNormalizedTime(detail, detailLayer, out num))
				{
					if (num >= threshold)
					{
						break;
					}
				}
				else if (t >= clipLen * threshold)
				{
					break;
				}
				t += Time.deltaTime;
				yield return null;
			}
			if (!this.IsOpen)
			{
				yield break;
			}
			this.ShowOpenCoat();
			this._swapRoutine = null;
			yield break;
		}

		// Token: 0x060067D7 RID: 26583 RVA: 0x00298638 File Offset: 0x00296838
		private bool TryGetOpenNormalizedTime(Animator detail, int layer, out float normalized)
		{
			normalized = 0f;
			if (detail == null || !detail.isActiveAndEnabled || detail.runtimeAnimatorController == null)
			{
				return false;
			}
			layer = Mathf.Clamp(layer, 0, detail.layerCount - 1);
			AnimatorStateInfo currentAnimatorStateInfo = detail.GetCurrentAnimatorStateInfo(layer);
			if (currentAnimatorStateInfo.IsName(this.openStateName))
			{
				normalized = currentAnimatorStateInfo.normalizedTime;
				return true;
			}
			return false;
		}

		// Token: 0x060067D8 RID: 26584 RVA: 0x002986A4 File Offset: 0x002968A4
		private float ResolveOpenClipLength(Animator detail)
		{
			if (detail == null || detail.runtimeAnimatorController == null)
			{
				return 1.5f;
			}
			AnimationClip[] animationClips = detail.runtimeAnimatorController.animationClips;
			int i = 0;
			while (i < animationClips.Length)
			{
				AnimationClip animationClip = animationClips[i];
				if (animationClip != null && animationClip.name.StartsWith("RC_FlashShape_Open"))
				{
					if (animationClip.length <= 0.01f)
					{
						return 1.5f;
					}
					return animationClip.length;
				}
				else
				{
					i++;
				}
			}
			return 1.5f;
		}

		// Token: 0x060067D9 RID: 26585 RVA: 0x00298728 File Offset: 0x00296928
		private void ShowOpenCoat()
		{
			if (this.openCoatPrefab == null)
			{
				Debug.LogWarning("[RainCoatOpenSwap] openCoatPrefab non assegnato: impossibile lo swap al mesh open.", this);
				return;
			}
			Transform transform = this.ResolveWornContainer();
			if (transform == null)
			{
				return;
			}
			this._wornCoatContainer = transform;
			if (this._openInstance == null)
			{
				Transform transform2 = ((transform.parent != null) ? transform.parent : ((this.animator != null) ? this.animator.transform : transform));
				this._openInstance = Object.Instantiate<GameObject>(this.openCoatPrefab, transform2);
				this._openInstance.name = "RainCoat_B_Open_Flash";
				this._openInstance.transform.localPosition = Vector3.zero;
				this._openInstance.transform.localRotation = Quaternion.identity;
				this._openInstance.transform.localScale = Vector3.one;
				MagicaCloth[] componentsInChildren = this._openInstance.GetComponentsInChildren<MagicaCloth>(true);
				foreach (MagicaCloth magicaCloth in componentsInChildren)
				{
					magicaCloth.Initialize();
					magicaCloth.DisableAutoBuild();
				}
				Animator component = this._openInstance.GetComponent<Animator>();
				if (component != null)
				{
					component.enabled = false;
				}
				this.RebindToBodyBones(this._openInstance, transform);
				this.FinalizeMagicaCloths(componentsInChildren);
				DonorSkeleton.Detach(this._openInstance, this._liveBonesByName, this);
			}
			this.InheritWornAppearance(transform, this._openInstance);
			this._openInstance.SetActive(true);
			transform.gameObject.SetActive(false);
		}

		// Token: 0x060067DA RID: 26586 RVA: 0x002988A8 File Offset: 0x00296AA8
		private void FinalizeMagicaCloths(MagicaCloth[] cloths)
		{
			if (cloths == null || cloths.Length == 0)
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
						Debug.Log(string.Format("[RainCoatOpenSwap] MagicaCloth '{0}': build avviata ({1} bones rimappati).", magicaCloth.name, dictionary.Count), this);
					}
					else
					{
						Debug.LogError("[RainCoatOpenSwap] MagicaCloth '" + magicaCloth.name + "': BuildAndRun fallita.", this);
					}
				}
			}
		}

		// Token: 0x060067DB RID: 26587 RVA: 0x002989E8 File Offset: 0x00296BE8
		private void ResolveClothColliders(MagicaCloth cloth)
		{
			if (cloth == null || cloth.SerializeData == null || cloth.SerializeData.colliderCollisionConstraint == null)
			{
				return;
			}
			List<ColliderComponent> list = cloth.SerializeData.colliderCollisionConstraint.colliderList;
			if (list == null)
			{
				list = new List<ColliderComponent>();
				cloth.SerializeData.colliderCollisionConstraint.colliderList = list;
			}
			list.RemoveAll((ColliderComponent c) => c == null);
			ClothColliderProfile componentInParent = cloth.GetComponentInParent<ClothColliderProfile>(true);
			if (componentInParent == null || this.animator == null)
			{
				return;
			}
			ColliderComponent[] componentsInChildren = this.animator.GetComponentsInChildren<ColliderComponent>(true);
			ClothColliderProfile.SelectionMode mode = componentInParent.Mode;
			if (mode != ClothColliderProfile.SelectionMode.NamedOnly)
			{
				if (mode == ClothColliderProfile.SelectionMode.None)
				{
					return;
				}
			}
			else
			{
				using (IEnumerator<string> enumerator = componentInParent.ColliderNames.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						if (!string.IsNullOrEmpty(text))
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
					}
					return;
				}
			}
			foreach (ColliderComponent colliderComponent2 in componentsInChildren)
			{
				if (colliderComponent2 != null && !list.Contains(colliderComponent2))
				{
					list.Add(colliderComponent2);
				}
			}
		}

		// Token: 0x060067DC RID: 26588 RVA: 0x00298B74 File Offset: 0x00296D74
		private void RestoreOriginalCoat()
		{
			if (this._openInstance != null)
			{
				this._openInstance.SetActive(false);
			}
			if (this._wornCoatContainer != null)
			{
				this._wornCoatContainer.gameObject.SetActive(true);
			}
		}

		// Token: 0x060067DD RID: 26589 RVA: 0x00298BB0 File Offset: 0x00296DB0
		private void InheritWornAppearance(Transform worn, GameObject openInstance)
		{
			if (!this.inheritWornAppearance || worn == null || openInstance == null)
			{
				return;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = worn.GetComponent<SkinnedMeshRenderer>() ?? worn.GetComponentInChildren<SkinnedMeshRenderer>(true);
			if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null)
			{
				return;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in openInstance.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer2.sharedMesh == null))
				{
					if (skinnedMeshRenderer2.sharedMaterials.Length == skinnedMeshRenderer.sharedMaterials.Length)
					{
						skinnedMeshRenderer2.sharedMaterials = skinnedMeshRenderer.sharedMaterials;
					}
					skinnedMeshRenderer2.shadowCastingMode = skinnedMeshRenderer.shadowCastingMode;
					for (int j = 0; j < skinnedMeshRenderer2.sharedMesh.blendShapeCount; j++)
					{
						int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(skinnedMeshRenderer2.sharedMesh.GetBlendShapeName(j));
						if (blendShapeIndex >= 0)
						{
							skinnedMeshRenderer2.SetBlendShapeWeight(j, skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex));
						}
					}
					if (this.forceHoodDownWhileOpen)
					{
						int blendShapeIndex2 = skinnedMeshRenderer2.sharedMesh.GetBlendShapeIndex("RainCoat_Hood_Down");
						if (blendShapeIndex2 >= 0)
						{
							skinnedMeshRenderer2.SetBlendShapeWeight(blendShapeIndex2, 100f);
						}
						int blendShapeIndex3 = skinnedMeshRenderer2.sharedMesh.GetBlendShapeIndex("RainCoat_Hood_DownStretch");
						if (blendShapeIndex3 >= 0)
						{
							skinnedMeshRenderer2.SetBlendShapeWeight(blendShapeIndex3, 0f);
						}
					}
				}
			}
		}

		// Token: 0x060067DE RID: 26590 RVA: 0x00298CF6 File Offset: 0x00296EF6
		private Transform ResolveWornContainer()
		{
			if (this._wornCoatContainer != null && this._wornCoatContainer.GetComponentInChildren<SkinnedMeshRenderer>(true) != null)
			{
				return this._wornCoatContainer;
			}
			return this.FindWornCoatContainer();
		}

		// Token: 0x060067DF RID: 26591 RVA: 0x00298D28 File Offset: 0x00296F28
		private Transform FindWornCoatContainer()
		{
			if (this.animator == null)
			{
				return null;
			}
			foreach (Transform transform in this.animator.GetComponentsInChildren<Transform>(true))
			{
				if (this.NameMatches(transform.name) && transform.gameObject.activeInHierarchy && transform.GetComponent<SkinnedMeshRenderer>() != null)
				{
					return transform;
				}
			}
			foreach (Transform transform2 in this.animator.GetComponentsInChildren<Transform>(true))
			{
				if (this.NameMatches(transform2.name) && transform2.gameObject.activeInHierarchy && transform2.GetComponentInChildren<SkinnedMeshRenderer>(true) != null)
				{
					return transform2;
				}
			}
			foreach (Transform transform3 in this.animator.GetComponentsInChildren<Transform>(true))
			{
				if (this.NameMatches(transform3.name) && transform3.GetComponentInChildren<SkinnedMeshRenderer>(true) != null)
				{
					return transform3;
				}
			}
			return null;
		}

		// Token: 0x060067E0 RID: 26592 RVA: 0x00298E20 File Offset: 0x00297020
		private bool NameMatches(string n)
		{
			if (string.IsNullOrEmpty(n))
			{
				return false;
			}
			string wornObjectName = this.WornObjectName;
			return n == wornObjectName || n.StartsWith(wornObjectName);
		}

		// Token: 0x060067E1 RID: 26593 RVA: 0x00298E50 File Offset: 0x00297050
		private void RebindToBodyBones(GameObject openInstance, Transform worn)
		{
			if (openInstance == null || worn == null)
			{
				return;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = worn.GetComponent<SkinnedMeshRenderer>() ?? worn.GetComponentInChildren<SkinnedMeshRenderer>(true);
			if (skinnedMeshRenderer == null || skinnedMeshRenderer.rootBone == null)
			{
				Debug.LogWarning("[RainCoatOpenSwap] Re-bind saltato: il worn coat non ha SMR/rootBone bindati.", this);
				return;
			}
			Transform transform = skinnedMeshRenderer.rootBone;
			while (transform.parent != null && RainCoatOpenSwap.IsBoneName(transform.parent.name))
			{
				transform = transform.parent;
			}
			Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
			foreach (Transform transform2 in transform.GetComponentsInChildren<Transform>(true))
			{
				if (!dictionary.ContainsKey(transform2.name))
				{
					dictionary[transform2.name] = transform2;
				}
			}
			foreach (Transform transform3 in skinnedMeshRenderer.bones)
			{
				if (transform3 != null && !dictionary.ContainsKey(transform3.name))
				{
					dictionary[transform3.name] = transform3;
				}
			}
			this._liveBonesByName = dictionary;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in openInstance.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				Transform[] bones = skinnedMeshRenderer2.bones;
				if (bones != null && bones.Length != 0)
				{
					Transform[] array2 = new Transform[bones.Length];
					int num = 0;
					int num2 = 0;
					for (int j = 0; j < bones.Length; j++)
					{
						Transform transform4;
						if (bones[j] != null && dictionary.TryGetValue(bones[j].name, out transform4))
						{
							array2[j] = transform4;
							num++;
						}
						else
						{
							array2[j] = bones[j];
							if (bones[j] != null)
							{
								num2++;
								Debug.LogWarning("[RainCoatOpenSwap] Bone open '" + bones[j].name + "' senza match live sullo scheletro di Ciel.", this);
							}
						}
					}
					skinnedMeshRenderer2.bones = array2;
					Transform transform5;
					if (skinnedMeshRenderer2.rootBone != null && dictionary.TryGetValue(skinnedMeshRenderer2.rootBone.name, out transform5))
					{
						skinnedMeshRenderer2.rootBone = transform5;
					}
					skinnedMeshRenderer2.updateWhenOffscreen = true;
					Debug.Log(string.Format("[RainCoatOpenSwap] Open re-bind: {0}/{1} bones matchate (root '{2}', {3} live){4}.", new object[]
					{
						num,
						bones.Length,
						transform.name,
						dictionary.Count,
						(num2 > 0) ? string.Format(", {0} MISSING", num2) : ""
					}), this);
				}
			}
		}

		// Token: 0x060067E2 RID: 26594 RVA: 0x002990DC File Offset: 0x002972DC
		private static bool IsBoneName(string n)
		{
			return !string.IsNullOrEmpty(n) && !n.StartsWith("RainCoat") && !n.StartsWith("Ciel") && !n.Contains("_ass") && (n.StartsWith("c_") || n.EndsWith(".x") || n.EndsWith(".l") || n.EndsWith(".r") || n.StartsWith("root") || n.Contains("spine") || n.Contains("Spine") || n.Contains("arm") || n.Contains("Arm") || n.Contains("hand") || n.Contains("Hand") || n.Contains("leg") || n.Contains("Leg") || n.Contains("thigh") || n.Contains("Thigh"));
		}

		// Token: 0x04006A4D RID: 27213
		[Tooltip("Il flash controller del raincoat. Auto-trovato su questo GameObject per layer name.")]
		[SerializeField]
		private ArmsFlashController coatFlash;

		// Token: 0x04006A4E RID: 27214
		[Tooltip("Layer name che identifica QUALE ArmsFlashController su questo GO è il flash del coat (sul GO convive anche il controller del cappuccio).")]
		[SerializeField]
		private string coatFlashLayerName = "RainCoatFlash";

		// Token: 0x04006A4F RID: 27215
		[Tooltip("Player Animator (il sottoalbero dove vive il coat indossato). Auto-trovato dal flash.")]
		[SerializeField]
		private Animator animator;

		// Token: 0x04006A50 RID: 27216
		[Header("Open Swap")]
		[Tooltip("FBX/prefab del coat aperto (RainCoat_B_Open).")]
		[SerializeField]
		private GameObject openCoatPrefab;

		// Token: 0x04006A51 RID: 27217
		[Tooltip("Frazione della clip di APERTURA del detail dopo la quale il worn viene scambiato col mesh open. Poco sotto 1 così il cutover avviene mentre la posa aperta si sta assestando.")]
		[Range(0.5f, 1f)]
		[SerializeField]
		private float swapAtNormalizedTime = 0.9f;

		// Token: 0x04006A52 RID: 27218
		[Tooltip("Nome dello stato di APERTURA sul layer flash del detail animator (per leggere il progresso reale della clip).")]
		[SerializeField]
		private string openStateName = "Flash_Open";

		// Token: 0x04006A53 RID: 27219
		[Tooltip("Se true (default), allo swap il mesh open eredita dal worn i MATERIALI (il worn usa la variante PVC trasparente, l'FBX open esce con i materiali di import) e TUTTI i blendshape con lo stesso nome (fitting body: Breast_Size/Hips/Waist..., cappuccio: Hood_Down + Hood_DownStretch).")]
		[SerializeField]
		private bool inheritWornAppearance = true;

		// Token: 0x04006A54 RID: 27220
		[Tooltip("Mentre il coat è tenuto aperto, il cappuccio sul mesh open è FORZATO abbassato (Hood_Down=100): lo sculpt di default di RainCoat_B_Open ha il cappuccio staccato dalla testa in una posa 'a metà' — tirando aperto il coat il cappuccio scivola giù, mai pose intermedie. Alla chiusura il worn torna con il suo stato reale.")]
		[SerializeField]
		private bool forceHoodDownWhileOpen = true;

		// Token: 0x04006A55 RID: 27221
		private Transform _wornCoatContainer;

		// Token: 0x04006A56 RID: 27222
		private GameObject _openInstance;

		// Token: 0x04006A57 RID: 27223
		private Coroutine _swapRoutine;

		// Token: 0x04006A58 RID: 27224
		private Dictionary<string, Transform> _liveBonesByName;
	}
}
