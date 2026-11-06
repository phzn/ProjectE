using System;
using System.Collections;
using System.Collections.Generic;
using Features.DebugTools;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Features.PoolDive
{
	// Token: 0x02000DE3 RID: 3555
	public class SwimsuitSlipController : MonoBehaviour
	{
		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x06006956 RID: 26966 RVA: 0x002A2909 File Offset: 0x002A0B09
		public bool HasSlipped
		{
			get
			{
				return this._slipped;
			}
		}

		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x06006957 RID: 26967 RVA: 0x002A2911 File Offset: 0x002A0B11
		public bool IsSlipping
		{
			get
			{
				return this._slipping;
			}
		}

		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x06006958 RID: 26968 RVA: 0x002A2919 File Offset: 0x002A0B19
		public GameObject EnvironmentInstance
		{
			get
			{
				return this._environmentInstance;
			}
		}

		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x06006959 RID: 26969 RVA: 0x002A2921 File Offset: 0x002A0B21
		public bool WasStolen
		{
			get
			{
				return this._stolen;
			}
		}

		// Token: 0x0600695A RID: 26970 RVA: 0x002A2929 File Offset: 0x002A0B29
		public void SuspendFloatBob()
		{
			this._floatEnvironment = false;
		}

		// Token: 0x0600695B RID: 26971 RVA: 0x002A2934 File Offset: 0x002A0B34
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInParent<Animator>();
			}
			if (this.flashController == null)
			{
				this.flashController = base.GetComponentInParent<SwimsuitFlashController>();
			}
			if (this.flashController == null)
			{
				this.flashController = base.GetComponentInChildren<SwimsuitFlashController>();
			}
			if (this.audioSource == null)
			{
				this.audioSource = base.gameObject.AddComponent<AudioSource>();
				this.audioSource.playOnAwake = false;
				this.audioSource.spatialBlend = 0f;
			}
			if (this.slipSound != null && this.slipSound.loadState == AudioDataLoadState.Unloaded)
			{
				this.slipSound.LoadAudioData();
			}
		}

		// Token: 0x0600695C RID: 26972 RVA: 0x002A29F4 File Offset: 0x002A0BF4
		private void Update()
		{
			if (this.allowLandSlipKey && !FlyCamera.IsActive && this.slipInput != null && this.slipInput.GetButtonDown())
			{
				this.TrySlipLand();
			}
			if (this._floatEnvironment && this._environmentInstance != null)
			{
				Vector3 position = this._environmentInstance.transform.position;
				position.y = this.waterlineY + Mathf.Sin(Time.time * this.floatBobSpeed) * this.floatBobAmplitude;
				this._environmentInstance.transform.position = position;
			}
		}

		// Token: 0x0600695D RID: 26973 RVA: 0x002A2A8A File Offset: 0x002A0C8A
		public bool TrySlipLand()
		{
			return this.TrySlip(this.landSlipPrefab, this.landEnvironmentPrefab, this.landSlipClipName, false);
		}

		// Token: 0x0600695E RID: 26974 RVA: 0x002A2AA5 File Offset: 0x002A0CA5
		public bool SlipInWater()
		{
			return this.TrySlip(this.waterSlipPrefab, this.waterEnvironmentPrefab, this.waterSlipClipName, true);
		}

		// Token: 0x0600695F RID: 26975 RVA: 0x002A2AC0 File Offset: 0x002A0CC0
		private bool TrySlip(GameObject slipPrefab, GameObject envPrefab, string clipName, bool floatEnv)
		{
			if (this._slipping)
			{
				return false;
			}
			if (this.flashController != null && (this.flashController.IsBusy || this.flashController.IsOpen))
			{
				return false;
			}
			this.ResolveWornSwimsuit();
			if (this.wornSwimsuit == null || !this.wornSwimsuit.activeInHierarchy)
			{
				return false;
			}
			this._slipped = false;
			this._stolen = false;
			if (this.flashController != null)
			{
				this.flashController.SetSwimsuitRestored();
			}
			if (slipPrefab == null)
			{
				Debug.LogWarning("[SwimsuitSlipController] Slip prefab not assigned for this context.", this);
				return false;
			}
			base.StartCoroutine(this.SlipRoutine(slipPrefab, envPrefab, clipName, floatEnv));
			return true;
		}

		// Token: 0x06006960 RID: 26976 RVA: 0x002A2B74 File Offset: 0x002A0D74
		private IEnumerator SlipRoutine(GameObject slipPrefab, GameObject envPrefab, string clipName, bool floatEnv)
		{
			this._slipping = true;
			if (this.slipAnimationDelay > 0f)
			{
				yield return new WaitForSeconds(this.slipAnimationDelay);
			}
			Transform transform = this.ResolveAnchor();
			Vector3 spawnPos = transform.position;
			Quaternion spawnRot = transform.rotation;
			Vector3 spawnScale = transform.lossyScale;
			if (this.wornSwimsuit != null)
			{
				this.wornSwimsuit.SetActive(false);
			}
			GameObject slipInstance = Object.Instantiate<GameObject>(slipPrefab);
			slipInstance.name = slipPrefab.name;
			if (this.followBodyDuringSlip)
			{
				slipInstance.transform.SetParent(transform, false);
				slipInstance.transform.localPosition = Vector3.zero;
				slipInstance.transform.localRotation = Quaternion.identity;
				slipInstance.transform.localScale = Vector3.one;
			}
			else
			{
				slipInstance.transform.SetPositionAndRotation(spawnPos, spawnRot);
				slipInstance.transform.localScale = spawnScale;
			}
			if (this.rebindToBodyBones)
			{
				this.RebindSlipToBodyBones(slipInstance);
			}
			if (this.slipSound != null && this.audioSource != null)
			{
				this.audioSource.PlayOneShot(this.slipSound, this.slipSoundVolume);
			}
			yield return new WaitForSeconds(this.ResolveSlipDuration(slipInstance, clipName));
			if (envPrefab != null)
			{
				Vector3 vector = ((this.followBodyDuringSlip && slipInstance != null) ? slipInstance.transform.position : spawnPos);
				Quaternion quaternion = ((this.followBodyDuringSlip && slipInstance != null) ? slipInstance.transform.rotation : spawnRot);
				this._environmentInstance = Object.Instantiate<GameObject>(envPrefab);
				this._environmentInstance.name = envPrefab.name;
				if (floatEnv)
				{
					Vector3 vector2 = vector;
					vector2.y = this.waterlineY;
					this._environmentInstance.transform.SetPositionAndRotation(vector2, quaternion);
				}
				else
				{
					this._environmentInstance.transform.SetPositionAndRotation(vector, quaternion);
				}
				this._environmentInstance.transform.localScale = spawnScale;
			}
			Object.Destroy(slipInstance);
			this._slipped = true;
			this._slipping = false;
			this._floatEnvironment = floatEnv;
			if (this.flashController != null)
			{
				this.flashController.SetSwimsuitRemoved();
			}
			UnityEvent onSwimsuitSlipped = this.OnSwimsuitSlipped;
			if (onSwimsuitSlipped != null)
			{
				onSwimsuitSlipped.Invoke();
			}
			yield break;
		}

		// Token: 0x06006961 RID: 26977 RVA: 0x002A2BA0 File Offset: 0x002A0DA0
		public bool NotifySwimsuitStolen()
		{
			if (!this._slipped || this._environmentInstance == null)
			{
				return false;
			}
			Object.Destroy(this._environmentInstance);
			this._environmentInstance = null;
			this._floatEnvironment = false;
			this._stolen = true;
			if (this.flashController != null)
			{
				this.flashController.SetSwimsuitRemoved();
			}
			UnityEvent onSwimsuitStolen = this.OnSwimsuitStolen;
			if (onSwimsuitStolen != null)
			{
				onSwimsuitStolen.Invoke();
			}
			return true;
		}

		// Token: 0x06006962 RID: 26978 RVA: 0x002A2C10 File Offset: 0x002A0E10
		public bool RestoreWornSwimsuit()
		{
			if (!this._slipped)
			{
				return false;
			}
			if (this._environmentInstance != null)
			{
				Object.Destroy(this._environmentInstance);
				this._environmentInstance = null;
			}
			this._floatEnvironment = false;
			if (this.wornSwimsuit != null)
			{
				this.wornSwimsuit.SetActive(true);
			}
			this._slipped = false;
			if (this.flashController != null)
			{
				this.flashController.SetSwimsuitRestored();
			}
			UnityEvent onSwimsuitRestored = this.OnSwimsuitRestored;
			if (onSwimsuitRestored != null)
			{
				onSwimsuitRestored.Invoke();
			}
			return true;
		}

		// Token: 0x06006963 RID: 26979 RVA: 0x002A2C9C File Offset: 0x002A0E9C
		private void RebindSlipToBodyBones(GameObject slipInstance)
		{
			if (slipInstance == null || this.animator == null)
			{
				return;
			}
			Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
			SkinnedMeshRenderer skinnedMeshRenderer = ((this.wornSwimsuit != null) ? this.wornSwimsuit.GetComponentInChildren<SkinnedMeshRenderer>(true) : null);
			if (skinnedMeshRenderer != null && skinnedMeshRenderer.bones != null)
			{
				foreach (Transform transform in skinnedMeshRenderer.bones)
				{
					if (transform != null && !dictionary.ContainsKey(transform.name))
					{
						dictionary[transform.name] = transform;
					}
				}
				if (skinnedMeshRenderer.rootBone != null && !dictionary.ContainsKey(skinnedMeshRenderer.rootBone.name))
				{
					dictionary[skinnedMeshRenderer.rootBone.name] = skinnedMeshRenderer.rootBone;
				}
			}
			if (dictionary.Count == 0)
			{
				Debug.LogWarning("[SwimsuitSlipController] Re-bind skipped: could not read the worn swimsuit's live bones (no worn SMR). The slip will use its baked pose.", this);
				return;
			}
			int num = 0;
			int num2 = 0;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in slipInstance.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				Transform[] bones2 = skinnedMeshRenderer2.bones;
				if (bones2 != null && bones2.Length != 0)
				{
					Transform[] array = new Transform[bones2.Length];
					int num3 = 0;
					for (int j = 0; j < bones2.Length; j++)
					{
						Transform transform2;
						if (bones2[j] != null && dictionary.TryGetValue(bones2[j].name, out transform2))
						{
							array[j] = transform2;
							num3++;
						}
						else
						{
							array[j] = bones2[j];
						}
					}
					skinnedMeshRenderer2.bones = array;
					Transform transform3;
					if (skinnedMeshRenderer2.rootBone != null && dictionary.TryGetValue(skinnedMeshRenderer2.rootBone.name, out transform3))
					{
						skinnedMeshRenderer2.rootBone = transform3;
					}
					skinnedMeshRenderer2.updateWhenOffscreen = true;
					num += num3;
					num2 += bones2.Length;
				}
			}
			Debug.Log(string.Format("[SwimsuitSlipController] Re-bound slip to Ciel's live bones: {0}/{1} ", num, num2) + "bones matched on '" + slipInstance.name + "'.", this);
		}

		// Token: 0x06006964 RID: 26980 RVA: 0x002A2EB0 File Offset: 0x002A10B0
		private Transform ResolveAnchor()
		{
			if (this.spawnAnchor != null)
			{
				return this.spawnAnchor;
			}
			if (this.animator != null && !string.IsNullOrEmpty(this.modelRootNameContains))
			{
				foreach (Transform transform in this.animator.GetComponentsInChildren<Transform>(true))
				{
					if (transform.name.Contains(this.modelRootNameContains))
					{
						return transform;
					}
				}
			}
			if (!(this.animator != null))
			{
				return base.transform;
			}
			return this.animator.transform;
		}

		// Token: 0x06006965 RID: 26981 RVA: 0x002A2F44 File Offset: 0x002A1144
		private float ResolveSlipDuration(GameObject slipInstance, string clipName)
		{
			Animator componentInChildren = slipInstance.GetComponentInChildren<Animator>();
			if (componentInChildren != null && componentInChildren.runtimeAnimatorController != null)
			{
				foreach (AnimationClip animationClip in componentInChildren.runtimeAnimatorController.animationClips)
				{
					if (animationClip != null && animationClip.name == clipName)
					{
						return animationClip.length;
					}
				}
				AnimationClip[] animationClips2 = componentInChildren.runtimeAnimatorController.animationClips;
				if (animationClips2 != null && animationClips2.Length != 0 && animationClips2[0] != null)
				{
					return animationClips2[0].length;
				}
			}
			return this.fallbackSlipDuration;
		}

		// Token: 0x06006966 RID: 26982 RVA: 0x002A2FDC File Offset: 0x002A11DC
		private void ResolveWornSwimsuit()
		{
			if (this.wornSwimsuit != null && this.wornSwimsuit.activeInHierarchy)
			{
				return;
			}
			if (this.animator == null)
			{
				return;
			}
			this.wornSwimsuit = SwimsuitSlipController.FindDeepActive(this.animator.transform, this.wornSwimsuitName) ?? SwimsuitSlipController.FindDeep(this.animator.transform, this.wornSwimsuitName);
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x002A304C File Offset: 0x002A124C
		private static GameObject FindDeep(Transform parent, string name)
		{
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				if (transform.name == name || transform.name.StartsWith(name))
				{
					return transform.gameObject;
				}
				GameObject gameObject = SwimsuitSlipController.FindDeep(transform, name);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
			return null;
		}

		// Token: 0x06006968 RID: 26984 RVA: 0x002A30DC File Offset: 0x002A12DC
		private static GameObject FindDeepActive(Transform parent, string name)
		{
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				if ((transform.name == name || transform.name.StartsWith(name)) && transform.gameObject.activeInHierarchy)
				{
					return transform.gameObject;
				}
				GameObject gameObject = SwimsuitSlipController.FindDeepActive(transform, name);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
			return null;
		}

		// Token: 0x04006C0A RID: 27658
		[Header("Refs")]
		[SerializeField]
		private Animator animator;

		// Token: 0x04006C0B RID: 27659
		[SerializeField]
		private SwimsuitFlashController flashController;

		// Token: 0x04006C0C RID: 27660
		[Tooltip("Worn swimsuit SMR object under the player. Auto-found by name under the Animator if not assigned.")]
		[SerializeField]
		private GameObject wornSwimsuit;

		// Token: 0x04006C0D RID: 27661
		[SerializeField]
		private string wornSwimsuitName = "School_Swimsuit";

		// Token: 0x04006C0E RID: 27662
		[Tooltip("Node the slip/environment prefabs align to. If null, the model FBX root is found automatically (the slip FBX shares that origin + scale), like the towel slip.")]
		[SerializeField]
		private Transform spawnAnchor;

		// Token: 0x04006C0F RID: 27663
		[Tooltip("Name fragment of the Ciel model FBX root the slip aligns to (its world TRS, incl. scale, matches the slip FBX). The node has a date in its name and is swapped periodically, so we match by fragment.")]
		[SerializeField]
		private string modelRootNameContains = "_ass";

		// Token: 0x04006C10 RID: 27664
		[Header("Land slip (key N)")]
		[Tooltip("Slip prefab: rigged swimsuit with its own Animator playing the SlipLand clip on enable.")]
		[SerializeField]
		private GameObject landSlipPrefab;

		// Token: 0x04006C11 RID: 27665
		[Tooltip("Static swimsuit mesh left on the ground once the land slip finishes.")]
		[SerializeField]
		private GameObject landEnvironmentPrefab;

		// Token: 0x04006C12 RID: 27666
		[SerializeField]
		private string landSlipClipName = "School_SwimsuitShape_SlipLand";

		// Token: 0x04006C13 RID: 27667
		[Header("Water slip (from dive)")]
		[Tooltip("Slip prefab playing the SlipWater clip on enable.")]
		[SerializeField]
		private GameObject waterSlipPrefab;

		// Token: 0x04006C14 RID: 27668
		[Tooltip("Static swimsuit mesh left floating at the waterline once the water slip finishes.")]
		[SerializeField]
		private GameObject waterEnvironmentPrefab;

		// Token: 0x04006C15 RID: 27669
		[SerializeField]
		private string waterSlipClipName = "School_SwimsuitShape_SlipWater";

		// Token: 0x04006C16 RID: 27670
		[Tooltip("World Y of the pool waterline; the surfaced swimsuit floats here. POI-Pool waterline = -5.66.")]
		[SerializeField]
		private float waterlineY = -5.66f;

		// Token: 0x04006C17 RID: 27671
		[Tooltip("How far the floating swimsuit bobs up/down.")]
		[SerializeField]
		private float floatBobAmplitude = 0.06f;

		// Token: 0x04006C18 RID: 27672
		[SerializeField]
		private float floatBobSpeed = 1.2f;

		// Token: 0x04006C19 RID: 27673
		[Header("Common")]
		[SerializeField]
		private float fallbackSlipDuration = 1.7f;

		// Token: 0x04006C1A RID: 27674
		[Tooltip("Delay (seconds) before the slip animation actually starts (prefab spawn + worn-mesh hide + sound). Lets you sync the slip to the right beat of the action. 0 = instant.")]
		[SerializeField]
		private float slipAnimationDelay;

		// Token: 0x04006C1B RID: 27675
		[Tooltip("Parent the spawned slip prefab to the model root so it FOLLOWS the body while the slip clip plays. Required for the water slip during a dive: the body moves/rotates fast (root motion), and a world-static slip prefab would visibly detach and look frozen (the 'T-pose costume' bug). When following, the slip stays glued to Ciel.")]
		[SerializeField]
		private bool followBodyDuringSlip = true;

		// Token: 0x04006C1C RID: 27676
		[Tooltip("Re-bind the slip mesh to Ciel's LIVE bones (same skeleton names) so it DEFORMS with her dive pose. The slip clip only animates blendShapes — it does NOT pose the skeleton — so without this the slip mesh stays in its baked standing pose while Ciel dives (the suit follows her position but not her dive rotation/pose). With it on, the mesh skins onto Ciel's actual bones and the blendShape clip still drives the slip-off.")]
		[SerializeField]
		private bool rebindToBodyBones = true;

		// Token: 0x04006C1D RID: 27677
		[Header("Audio")]
		[SerializeField]
		private AudioClip slipSound;

		// Token: 0x04006C1E RID: 27678
		[SerializeField]
		[Range(0f, 1f)]
		private float slipSoundVolume = 1f;

		// Token: 0x04006C1F RID: 27679
		[Tooltip("Persistent AudioSource used to play the slip sound with no startup delay. Auto-created on this GameObject in Awake if not assigned (avoids PlayClipAtPoint's per-call object spawn, which delayed the sound).")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04006C20 RID: 27680
		[Header("Input")]
		[Tooltip("Land slip key (matches the towel slip key N).")]
		[SerializeField]
		private GenericInput slipInput = new GenericInput("N", "X", "N");

		// Token: 0x04006C21 RID: 27681
		[Tooltip("Allow the N key to drop the suit on land. The water slip is triggered by the dive, not by this key.")]
		[SerializeField]
		private bool allowLandSlipKey = true;

		// Token: 0x04006C22 RID: 27682
		[Header("Events")]
		public UnityEvent OnSwimsuitSlipped;

		// Token: 0x04006C23 RID: 27683
		public UnityEvent OnSwimsuitRestored;

		// Token: 0x04006C24 RID: 27684
		public UnityEvent OnSwimsuitStolen;

		// Token: 0x04006C25 RID: 27685
		private bool _slipped;

		// Token: 0x04006C26 RID: 27686
		private bool _slipping;

		// Token: 0x04006C27 RID: 27687
		private bool _stolen;

		// Token: 0x04006C28 RID: 27688
		private GameObject _environmentInstance;

		// Token: 0x04006C29 RID: 27689
		private bool _floatEnvironment;
	}
}
