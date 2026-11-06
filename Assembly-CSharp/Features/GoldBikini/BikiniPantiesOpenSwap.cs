using System;
using System.Collections;
using System.Collections.Generic;
using Features.Flash;
using UnityEngine;
using UnityEngine.Events;

namespace Features.GoldBikini
{
	// Token: 0x020011BE RID: 4542
	public class BikiniPantiesOpenSwap : MonoBehaviour
	{
		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x06008B69 RID: 35689 RVA: 0x0038F833 File Offset: 0x0038DA33
		private string WornObjectName
		{
			get
			{
				if (!(this.pantiesFlash != null) || this.pantiesFlash.Config == null)
				{
					return "EPBikini_Panties";
				}
				return this.pantiesFlash.Config.wornObjectName;
			}
		}

		// Token: 0x06008B6A RID: 35690 RVA: 0x0038F868 File Offset: 0x0038DA68
		private void Awake()
		{
			if (this.pantiesFlash == null)
			{
				this.pantiesFlash = this.ResolvePantiesFlash();
			}
			if (this.animator == null && this.pantiesFlash != null)
			{
				this.animator = this.pantiesFlash.GetComponentInParent<Animator>();
			}
		}

		// Token: 0x06008B6B RID: 35691 RVA: 0x0038F8BC File Offset: 0x0038DABC
		private void OnEnable()
		{
			if (this.pantiesFlash == null)
			{
				this.pantiesFlash = this.ResolvePantiesFlash();
			}
			if (this.pantiesFlash != null)
			{
				this.pantiesFlash.OnOpened.AddListener(new UnityAction(this.OnFlashOpened));
				this.pantiesFlash.OnClosed.AddListener(new UnityAction(this.OnFlashClosed));
			}
		}

		// Token: 0x06008B6C RID: 35692 RVA: 0x0038F92C File Offset: 0x0038DB2C
		private void OnDisable()
		{
			if (this.pantiesFlash != null)
			{
				this.pantiesFlash.OnOpened.RemoveListener(new UnityAction(this.OnFlashOpened));
				this.pantiesFlash.OnClosed.RemoveListener(new UnityAction(this.OnFlashClosed));
			}
			if (this._swapRoutine != null)
			{
				base.StopCoroutine(this._swapRoutine);
				this._swapRoutine = null;
			}
			this.RestoreOriginalPanties();
		}

		// Token: 0x06008B6D RID: 35693 RVA: 0x0038F9A0 File Offset: 0x0038DBA0
		private ArmsFlashController ResolvePantiesFlash()
		{
			foreach (ArmsFlashController armsFlashController in base.GetComponents<ArmsFlashController>())
			{
				if (armsFlashController.Config != null && armsFlashController.Config.flashLayerName == this.pantiesLayerName)
				{
					return armsFlashController;
				}
			}
			Debug.LogWarning(string.Concat(new string[] { "[BikiniPantiesOpenSwap] Nessun ArmsFlashController con layer '", this.pantiesLayerName, "' su '", base.name, "'. Open-swap disattivato." }), this);
			return null;
		}

		// Token: 0x06008B6E RID: 35694 RVA: 0x0038FA24 File Offset: 0x0038DC24
		public void OnFlashOpened()
		{
			if (this.pantiesFlash == null || !this.pantiesFlash.IsWorn)
			{
				return;
			}
			if (this._swapRoutine != null)
			{
				base.StopCoroutine(this._swapRoutine);
			}
			this._swapRoutine = base.StartCoroutine(this.ShowOpenAfterLowerClip());
		}

		// Token: 0x06008B6F RID: 35695 RVA: 0x0038FA73 File Offset: 0x0038DC73
		public void OnFlashClosed()
		{
			if (this._swapRoutine != null)
			{
				base.StopCoroutine(this._swapRoutine);
				this._swapRoutine = null;
			}
			this.RestoreOriginalPanties();
		}

		// Token: 0x06008B70 RID: 35696 RVA: 0x0038FA96 File Offset: 0x0038DC96
		public void OnPantiesRestored()
		{
			this._wornPantiesContainer = null;
			this.DestroyOpenInstance();
		}

		// Token: 0x06008B71 RID: 35697 RVA: 0x0038FAA5 File Offset: 0x0038DCA5
		private void DestroyOpenInstance()
		{
			if (this._openInstance != null)
			{
				Object.Destroy(this._openInstance);
				this._openInstance = null;
			}
		}

		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x06008B72 RID: 35698 RVA: 0x0038FAC7 File Offset: 0x0038DCC7
		private bool IsOpen
		{
			get
			{
				return this.pantiesFlash != null && this.pantiesFlash.IsOpen;
			}
		}

		// Token: 0x06008B73 RID: 35699 RVA: 0x0038FAE4 File Offset: 0x0038DCE4
		private IEnumerator ShowOpenAfterLowerClip()
		{
			Animator detail = ((this.pantiesFlash != null) ? this.pantiesFlash.EnsureDetailAnimator() : null);
			float clipLen = this.ResolveStartClipLength(detail);
			float threshold = Mathf.Clamp01(this.swapAtNormalizedTime);
			float t = 0f;
			while (this.IsOpen)
			{
				float num;
				if (this.TryGetLowerNormalizedTime(detail, out num))
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
			this.ShowOpenPanties();
			this._swapRoutine = null;
			yield break;
		}

		// Token: 0x06008B74 RID: 35700 RVA: 0x0038FAF4 File Offset: 0x0038DCF4
		private bool TryGetLowerNormalizedTime(Animator detail, out float normalized)
		{
			normalized = 0f;
			if (detail == null || !detail.isActiveAndEnabled || detail.runtimeAnimatorController == null)
			{
				return false;
			}
			AnimatorStateInfo currentAnimatorStateInfo = detail.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.IsName(this.lowerClipName) || currentAnimatorStateInfo.IsName(this.lowerClipName + "Shape"))
			{
				normalized = currentAnimatorStateInfo.normalizedTime;
				return true;
			}
			return false;
		}

		// Token: 0x06008B75 RID: 35701 RVA: 0x0038FB68 File Offset: 0x0038DD68
		private float ResolveStartClipLength(Animator detail)
		{
			if (detail == null || detail.runtimeAnimatorController == null)
			{
				return 0.7f;
			}
			AnimationClip[] animationClips = detail.runtimeAnimatorController.animationClips;
			int i = 0;
			while (i < animationClips.Length)
			{
				AnimationClip animationClip = animationClips[i];
				if (animationClip != null && animationClip.name.StartsWith(this.lowerClipName))
				{
					if (animationClip.length <= 0.01f)
					{
						return 0.7f;
					}
					return animationClip.length;
				}
				else
				{
					i++;
				}
			}
			return 0.7f;
		}

		// Token: 0x06008B76 RID: 35702 RVA: 0x0038FBEC File Offset: 0x0038DDEC
		private void ShowOpenPanties()
		{
			if (this.openPantiesPrefab == null)
			{
				Debug.LogWarning("[BikiniPantiesOpenSwap] openPantiesPrefab is unassigned; cannot swap to the open mesh.", this);
				return;
			}
			Transform transform = this.ResolveWornContainer();
			if (transform == null)
			{
				return;
			}
			this._wornPantiesContainer = transform;
			if (this._openInstance == null)
			{
				Transform transform2 = ((transform.parent != null) ? transform.parent : ((this.animator != null) ? this.animator.transform : transform));
				this._openInstance = Object.Instantiate<GameObject>(this.openPantiesPrefab, transform2);
				this._openInstance.name = this.openPantiesObjectName + "_Flash";
				this._openInstance.transform.localPosition = Vector3.zero;
				this._openInstance.transform.localRotation = Quaternion.identity;
				this._openInstance.transform.localScale = Vector3.one;
				Animator component = this._openInstance.GetComponent<Animator>();
				if (component != null)
				{
					component.enabled = false;
				}
				this.RebindToBodyBones(this._openInstance, transform);
			}
			this._openInstance.SetActive(true);
			transform.gameObject.SetActive(false);
		}

		// Token: 0x06008B77 RID: 35703 RVA: 0x0038FD1C File Offset: 0x0038DF1C
		private void RestoreOriginalPanties()
		{
			if (this._openInstance != null)
			{
				this._openInstance.SetActive(false);
			}
			if (this._wornPantiesContainer != null)
			{
				this._wornPantiesContainer.gameObject.SetActive(true);
			}
		}

		// Token: 0x06008B78 RID: 35704 RVA: 0x0038FD57 File Offset: 0x0038DF57
		private Transform ResolveWornContainer()
		{
			if (this._wornPantiesContainer != null && this._wornPantiesContainer.GetComponentInChildren<SkinnedMeshRenderer>(true) != null)
			{
				return this._wornPantiesContainer;
			}
			return this.FindWornPantiesContainer();
		}

		// Token: 0x06008B79 RID: 35705 RVA: 0x0038FD88 File Offset: 0x0038DF88
		private Transform FindWornPantiesContainer()
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

		// Token: 0x06008B7A RID: 35706 RVA: 0x0038FE80 File Offset: 0x0038E080
		private bool NameMatches(string n)
		{
			if (string.IsNullOrEmpty(n))
			{
				return false;
			}
			string wornObjectName = this.WornObjectName;
			return n == wornObjectName || n.StartsWith(wornObjectName);
		}

		// Token: 0x06008B7B RID: 35707 RVA: 0x0038FEB0 File Offset: 0x0038E0B0
		private void RebindToBodyBones(GameObject openInstance, Transform worn)
		{
			if (openInstance == null || worn == null)
			{
				return;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = worn.GetComponent<SkinnedMeshRenderer>() ?? worn.GetComponentInChildren<SkinnedMeshRenderer>(true);
			if (skinnedMeshRenderer == null || skinnedMeshRenderer.rootBone == null)
			{
				Debug.LogWarning("[BikiniPantiesOpenSwap] Re-bind skipped: worn panties have no bound SMR/rootBone.", this);
				return;
			}
			Transform transform = skinnedMeshRenderer.rootBone;
			while (transform.parent != null && BikiniPantiesOpenSwap.IsBoneName(transform.parent.name))
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
								Debug.LogWarning("[BikiniPantiesOpenSwap] Open bone '" + bones[j].name + "' has no live match on Ciel's skeleton.", this);
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
					Debug.Log(string.Format("[BikiniPantiesOpenSwap] Open re-bind: {0}/{1} bones matched (skeleton root '{2}', {3} live bones indexed){4}.", new object[]
					{
						num,
						bones.Length,
						transform.name,
						dictionary.Count,
						(num2 > 0) ? string.Format(", {0} MISSING", num2) : ""
					}), this);
				}
			}
			DonorSkeleton.Detach(openInstance, dictionary, this);
		}

		// Token: 0x06008B7C RID: 35708 RVA: 0x0039013C File Offset: 0x0038E33C
		private static bool IsBoneName(string n)
		{
			return !string.IsNullOrEmpty(n) && !n.StartsWith("EPBikini") && !n.StartsWith("Ciel") && !n.Contains("_ass") && (n.StartsWith("c_") || n.EndsWith(".x") || n.EndsWith(".l") || n.EndsWith(".r") || n.StartsWith("root") || n.Contains("spine") || n.Contains("Thigh") || n.Contains("thigh") || n.Contains("Butt") || n.Contains("leg") || n.Contains("Leg"));
		}

		// Token: 0x04008E4A RID: 36426
		[Tooltip("The panties flash controller this swap follows. Auto-found on this GameObject. Used to read the worn-object name, the detail animator (for accurate swap timing) and IsOpen.")]
		[SerializeField]
		private ArmsFlashController pantiesFlash;

		// Token: 0x04008E4B RID: 36427
		[Tooltip("Player Animator (subtree the worn panties live under). Auto-found from pantiesFlash.")]
		[SerializeField]
		private Animator animator;

		// Token: 0x04008E4C RID: 36428
		[Header("Open Swap")]
		[Tooltip("Improved 'lowered' panties FBX (EPBikini_Panties_Open).")]
		[SerializeField]
		private GameObject openPantiesPrefab;

		// Token: 0x04008E4D RID: 36429
		[Tooltip("Mesh root name of the open panties (the SMR object inside the open FBX).")]
		[SerializeField]
		private string openPantiesObjectName = "EPBikini_Panties_Open";

		// Token: 0x04008E4E RID: 36430
		[Tooltip("Fraction of the LOWER clip after which the worn panties are swapped for the open mesh. Slightly below 1 so the cutover happens while the lowering is still settling.")]
		[Range(0.5f, 1f)]
		[SerializeField]
		private float swapAtNormalizedTime = 0.85f;

		// Token: 0x04008E4F RID: 36431
		[Tooltip("Name of the panties-only LOWER state/clip on the detail animator (used to read its real progress so the swap lands mid-motion).")]
		[SerializeField]
		private string lowerClipName = "EPPanties_Lower";

		// Token: 0x04008E50 RID: 36432
		private Transform _wornPantiesContainer;

		// Token: 0x04008E51 RID: 36433
		private GameObject _openInstance;

		// Token: 0x04008E52 RID: 36434
		private Coroutine _swapRoutine;

		// Token: 0x04008E53 RID: 36435
		[Tooltip("Layer name identifying WHICH ArmsFlashController on this GameObject is the panties (so the swap self-wires to the right one when several flash controllers share the GameObject).")]
		[SerializeField]
		private string pantiesLayerName = "BikiniPantiesFlash";
	}
}
