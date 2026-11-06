using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Features.Detection;
using Features.Flash;
using Features.Inventory;
using Features.Inventory.Wearable;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.WardrobeFeedback
{
	// Token: 0x02000B56 RID: 2902
	[DefaultExecutionOrder(31000)]
	public class WearAdmireAnimation : MonoBehaviour
	{
		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x0600524C RID: 21068 RVA: 0x001FE180 File Offset: 0x001FC380
		public static bool IsPresentationSuppressed
		{
			get
			{
				return WearAdmireAnimation.PresentationSuppressionOwners.Count > 0 || WardrobeGameplayContext.IsPresentationOnly((WearAdmireAnimation._instance != null) ? WearAdmireAnimation._instance._wear : null);
			}
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x001FE1B0 File Offset: 0x001FC3B0
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (WearAdmireAnimation._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("WearAdmireAnimation");
			Object.DontDestroyOnLoad(gameObject);
			WearAdmireAnimation._instance = gameObject.AddComponent<WearAdmireAnimation>();
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x001FE1DA File Offset: 0x001FC3DA
		private void Awake()
		{
			this._muteUntil = Time.unscaledTime + this.bootMute;
			SceneManager.sceneLoaded += this.HandleSceneLoaded;
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x001FE200 File Offset: 0x001FC400
		private void OnDestroy()
		{
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			this.CancelAndParkAdmire();
			if (this._wear != null)
			{
				this._wear.OnItemWorn -= this.HandleItemWorn;
			}
			if (WearAdmireAnimation._instance == this)
			{
				WearAdmireAnimation._instance = null;
			}
		}

		// Token: 0x06005250 RID: 21072 RVA: 0x001FE25C File Offset: 0x001FC45C
		private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this.CancelAndParkAdmire();
			this._muteUntil = Time.unscaledTime + this.bootMute;
		}

		// Token: 0x06005251 RID: 21073 RVA: 0x001FE276 File Offset: 0x001FC476
		private void OnEnable()
		{
			PlayerActivityGate.ResourcesChanged += this.HandleResourcesChanged;
			ClothActionGate.Started += this.HandleClothActionStarted;
		}

		// Token: 0x06005252 RID: 21074 RVA: 0x001FE29A File Offset: 0x001FC49A
		private void OnDisable()
		{
			PlayerActivityGate.ResourcesChanged -= this.HandleResourcesChanged;
			ClothActionGate.Started -= this.HandleClothActionStarted;
			this.CancelAndParkAdmire();
		}

		// Token: 0x06005253 RID: 21075 RVA: 0x001FE2C4 File Offset: 0x001FC4C4
		private void HandleResourcesChanged(PlayerContextResource resources)
		{
			if (this._playRoutine != null && this.HasConflictingActivity)
			{
				this.CancelAndParkAdmire();
			}
		}

		// Token: 0x06005254 RID: 21076 RVA: 0x001FE2DC File Offset: 0x001FC4DC
		private void HandleClothActionStarted(object owner)
		{
			if (this._playRoutine != null)
			{
				this.CancelAndParkAdmire();
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06005255 RID: 21077 RVA: 0x001FE2EC File Offset: 0x001FC4EC
		private bool HasConflictingActivity
		{
			get
			{
				return ClothActionGate.IsBusy || PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.Rider | PlayerContextResource.HeldItem) || (this._cover != null && this._cover.IsCovering) || (this._character != null && (this._character.customAction || this._character.ragdolled || !this._character.isGrounded || this._character.isCrouching || this._character.input.sqrMagnitude > 0.01f));
			}
		}

		// Token: 0x06005256 RID: 21078 RVA: 0x001FE38C File Offset: 0x001FC58C
		private void Update()
		{
			if (this._playRoutine != null && this.HasConflictingActivity)
			{
				this.CancelAndParkAdmire();
			}
			if (WardrobeGameplayContext.IsPresentationOnly(this._wear))
			{
				if (this._playRoutine != null)
				{
					this.CancelAndParkAdmire();
				}
				return;
			}
			if ((this._wear == null || this._animator == null || this._layerIndex < 0) && Time.unscaledTime >= this._nextResolveTime)
			{
				this._nextResolveTime = Time.unscaledTime + 1f;
				this.Resolve();
			}
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x001FE414 File Offset: 0x001FC614
		private void Resolve()
		{
			WearableUseController wearableUseController = Object.FindObjectOfType<WearableUseController>();
			if (wearableUseController == null)
			{
				return;
			}
			if (this._wear != null)
			{
				this._wear.OnItemWorn -= this.HandleItemWorn;
			}
			this._wear = wearableUseController;
			this._wear.OnItemWorn += this.HandleItemWorn;
			this._animator = this._wear.GetComponentInParent<Animator>();
			if (this._animator == null)
			{
				this._animator = this._wear.transform.root.GetComponentInChildren<Animator>();
			}
			this._layerIndex = ((this._animator != null) ? this._animator.GetLayerIndex("WearAdmire") : (-1));
			this._character = ((this._animator != null) ? this._animator.GetComponent<vThirdPersonController>() : null);
			this._cover = ((this._animator != null) ? this._animator.GetComponent<CoverPoseController>() : null);
			if (this._animator != null && this._animator.isHuman)
			{
				this._leftHand = this._animator.GetBoneTransform(HumanBodyBones.LeftHand);
				this._rightHand = this._animator.GetBoneTransform(HumanBodyBones.RightHand);
				this._head = this._animator.GetBoneTransform(HumanBodyBones.Head);
			}
			this.ResolveEyeSystem();
			this._muteUntil = Mathf.Max(this._muteUntil, Time.unscaledTime + 0.5f);
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x001FE590 File Offset: 0x001FC790
		private void ResolveEyeSystem()
		{
			this._npcEyeLook = null;
			this._eyeL = (this._eyeR = null);
			if (this._animator == null)
			{
				return;
			}
			PlayerEyeLookAt componentInChildren = this._animator.transform.root.GetComponentInChildren<PlayerEyeLookAt>(true);
			if (componentInChildren != null)
			{
				this._npcEyeLook = componentInChildren;
				Type type = componentInChildren.GetType();
				FieldInfo field = type.GetField("leftEye", BindingFlags.Instance | BindingFlags.NonPublic);
				this._eyeL = ((field != null) ? field.GetValue(componentInChildren) : null) as Transform;
				FieldInfo field2 = type.GetField("rightEye", BindingFlags.Instance | BindingFlags.NonPublic);
				this._eyeR = ((field2 != null) ? field2.GetValue(componentInChildren) : null) as Transform;
				FieldInfo field3 = type.GetField("invertHorizontal", BindingFlags.Instance | BindingFlags.NonPublic);
				this._invertH = (((field3 != null) ? field3.GetValue(componentInChildren) : null) as bool?).GetValueOrDefault();
				FieldInfo field4 = type.GetField("invertVertical", BindingFlags.Instance | BindingFlags.NonPublic);
				this._invertV = (((field4 != null) ? field4.GetValue(componentInChildren) : null) as bool?).GetValueOrDefault();
			}
			if ((this._eyeL == null || this._eyeR == null) && this._animator.isHuman)
			{
				if (this._eyeL == null)
				{
					this._eyeL = this._animator.GetBoneTransform(HumanBodyBones.LeftEye);
				}
				if (this._eyeR == null)
				{
					this._eyeR = this._animator.GetBoneTransform(HumanBodyBones.RightEye);
				}
			}
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x001FE710 File Offset: 0x001FC910
		public static bool PlayCosmeticAdmire()
		{
			if (WearAdmireAnimation._instance == null || WearAdmireAnimation.IsPresentationSuppressed || WearAdmireAnimation._instance.HasConflictingActivity)
			{
				return false;
			}
			if (WearAdmireAnimation._instance._animator == null || WearAdmireAnimation._instance._layerIndex < 0)
			{
				WearAdmireAnimation._instance.Resolve();
			}
			if (WearAdmireAnimation._instance._animator == null || WearAdmireAnimation._instance._layerIndex < 0 || WearAdmireAnimation._instance._playRoutine != null)
			{
				return false;
			}
			WearAdmireAnimation._instance._lastPlayTime = Time.unscaledTime;
			WearAdmireAnimation._instance._playRoutine = WearAdmireAnimation._instance.StartCoroutine(WearAdmireAnimation._instance.PlayAdmire());
			return true;
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x001FE7C4 File Offset: 0x001FC9C4
		private void HandleItemWorn(WearableItemSO item)
		{
			if (item == null || this._animator == null || this._layerIndex < 0)
			{
				return;
			}
			if (WearAdmireAnimation.IsPresentationSuppressed || this.HasConflictingActivity)
			{
				return;
			}
			float unscaledTime = Time.unscaledTime;
			if (unscaledTime < this._muteUntil)
			{
				return;
			}
			if (unscaledTime - this._lastPlayTime < this.cooldown)
			{
				return;
			}
			if (!this.IsImportant(item))
			{
				return;
			}
			if (this._playRoutine != null)
			{
				return;
			}
			this._lastPlayTime = unscaledTime;
			this._playRoutine = base.StartCoroutine(this.PlayAdmire());
		}

		// Token: 0x0600525B RID: 21083 RVA: 0x001FE850 File Offset: 0x001FCA50
		public static void SetPresentationSuppressed(Object owner, bool suppressed)
		{
			if (owner == null)
			{
				return;
			}
			int instanceID = owner.GetInstanceID();
			if ((suppressed ? WearAdmireAnimation.PresentationSuppressionOwners.Add(instanceID) : WearAdmireAnimation.PresentationSuppressionOwners.Remove(instanceID)) && suppressed && WearAdmireAnimation._instance != null)
			{
				WearAdmireAnimation._instance.CancelAndParkAdmire();
			}
		}

		// Token: 0x0600525C RID: 21084 RVA: 0x001FE8A4 File Offset: 0x001FCAA4
		private void CancelAndParkAdmire()
		{
			if (this._playRoutine != null)
			{
				base.StopCoroutine(this._playRoutine);
				this._playRoutine = null;
			}
			if (this._animator != null && this._layerIndex >= 0)
			{
				this.SetWeight(0f);
				this._animator.Play("Empty", this._layerIndex, 0f);
			}
			else
			{
				this._admireWeight = 0f;
			}
			this.EndEyeOverride();
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x001FE91C File Offset: 0x001FCB1C
		private bool IsImportant(WearableItemSO item)
		{
			for (int i = 0; i < this.importantClothes.Count; i++)
			{
				string text = this.importantClothes[i];
				if (!string.IsNullOrEmpty(text))
				{
					if (string.Equals(text, item.itemName, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
					if (string.Equals(text, item.name, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600525E RID: 21086 RVA: 0x001FE977 File Offset: 0x001FCB77
		private IEnumerator PlayAdmire()
		{
			this.BeginEyeOverride();
			this._animator.Play("LookAtHands", this._layerIndex, 0f);
			yield return null;
			float clipLength = 2.5f;
			AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(this._layerIndex);
			if (currentAnimatorStateInfo.IsName("LookAtHands") && currentAnimatorStateInfo.length > 0.1f)
			{
				clipLength = currentAnimatorStateInfo.length;
			}
			for (float t = 0f; t < this.fadeIn; t += Time.deltaTime)
			{
				if (this._animator == null)
				{
					this.AbortAdmire();
					yield break;
				}
				this.SetWeight(this.maxWeight * (t / this.fadeIn));
				yield return null;
			}
			this.SetWeight(this.maxWeight);
			float num = Mathf.Max(0f, Mathf.Min(clipLength, this.maxDuration) - this.fadeIn - this.fadeOut);
			yield return new WaitForSeconds(num);
			for (float t = 0f; t < this.fadeOut; t += Time.deltaTime)
			{
				if (this._animator == null)
				{
					this.AbortAdmire();
					yield break;
				}
				this.SetWeight(this.maxWeight * (1f - t / this.fadeOut));
				yield return null;
			}
			this.SetWeight(0f);
			this._animator.Play("Empty", this._layerIndex, 0f);
			this.EndEyeOverride();
			this._playRoutine = null;
			yield break;
		}

		// Token: 0x0600525F RID: 21087 RVA: 0x001FE986 File Offset: 0x001FCB86
		private void AbortAdmire()
		{
			this._admireWeight = 0f;
			this.EndEyeOverride();
			this._playRoutine = null;
		}

		// Token: 0x06005260 RID: 21088 RVA: 0x001FE9A0 File Offset: 0x001FCBA0
		private void SetWeight(float w)
		{
			this._admireWeight = w;
			this._animator.SetLayerWeight(this._layerIndex, w);
		}

		// Token: 0x06005261 RID: 21089 RVA: 0x001FE9BC File Offset: 0x001FCBBC
		private void BeginEyeOverride()
		{
			if (!this.eyeLook || this._eyeOverrideActive)
			{
				return;
			}
			if (this._eyeL == null && this._eyeR == null)
			{
				return;
			}
			if (this._npcEyeLook != null)
			{
				this._npcEyeLookWasEnabled = this._npcEyeLook.enabled;
				this._npcEyeLook.enabled = false;
				Type type = this._npcEyeLook.GetType();
				FieldInfo field = type.GetField("_leftEyeRestPosition", BindingFlags.Instance | BindingFlags.NonPublic);
				Vector3? vector = ((field != null) ? field.GetValue(this._npcEyeLook) : null) as Vector3?;
				FieldInfo field2 = type.GetField("_rightEyeRestPosition", BindingFlags.Instance | BindingFlags.NonPublic);
				Vector3? vector2 = ((field2 != null) ? field2.GetValue(this._npcEyeLook) : null) as Vector3?;
				FieldInfo field3 = type.GetField("_hasRestPose", BindingFlags.Instance | BindingFlags.NonPublic);
				bool valueOrDefault = (((field3 != null) ? field3.GetValue(this._npcEyeLook) : null) as bool?).GetValueOrDefault();
				this._eyeLRest = ((valueOrDefault && vector != null) ? vector.Value : ((this._eyeL != null) ? this._eyeL.localPosition : Vector3.zero));
				this._eyeRRest = ((valueOrDefault && vector2 != null) ? vector2.Value : ((this._eyeR != null) ? this._eyeR.localPosition : Vector3.zero));
			}
			else
			{
				this._eyeLRest = ((this._eyeL != null) ? this._eyeL.localPosition : Vector3.zero);
				this._eyeRRest = ((this._eyeR != null) ? this._eyeR.localPosition : Vector3.zero);
			}
			this._eyeOffsetCurrent = Vector3.zero;
			this._eyeOverrideActive = true;
		}

		// Token: 0x06005262 RID: 21090 RVA: 0x001FEB8C File Offset: 0x001FCD8C
		private void EndEyeOverride()
		{
			if (!this._eyeOverrideActive)
			{
				return;
			}
			this._eyeOverrideActive = false;
			if (this._eyeL != null)
			{
				this._eyeL.localPosition = this._eyeLRest;
			}
			if (this._eyeR != null)
			{
				this._eyeR.localPosition = this._eyeRRest;
			}
			if (this._npcEyeLook != null)
			{
				this._npcEyeLook.enabled = this._npcEyeLookWasEnabled;
			}
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x001FEC08 File Offset: 0x001FCE08
		private void LateUpdate()
		{
			if (!this._eyeOverrideActive)
			{
				return;
			}
			if (this._eyeL == null && this._eyeR == null)
			{
				return;
			}
			if (this._leftHand == null || this._rightHand == null || this._head == null)
			{
				return;
			}
			Vector3 vector = (this._leftHand.position + this._rightHand.position) * 0.5f;
			Vector3 vector2 = ((this._eyeL != null && this._eyeR != null) ? ((this._eyeL.position + this._eyeR.position) * 0.5f) : ((this._eyeL != null) ? this._eyeL.position : this._eyeR.position));
			Vector3 vector3 = this._head.InverseTransformDirection((vector - vector2).normalized);
			float num = Mathf.Clamp(Mathf.Atan2(vector3.x, vector3.z) / 1.5707964f, -1f, 1f);
			float num2 = Mathf.Clamp(vector3.y, -1f, 1f);
			if (this._invertH)
			{
				num = -num;
			}
			if (this._invertV)
			{
				num2 = -num2;
			}
			Vector3 vector4 = (this._head.right * (num * this.eyeMaxHorizontal) + this._head.up * (num2 * this.eyeMaxVertical)) * this._admireWeight;
			float num3 = 1f - Mathf.Exp(-this.eyeSmoothing * Time.deltaTime);
			this._eyeOffsetCurrent = Vector3.Lerp(this._eyeOffsetCurrent, vector4, num3);
			if (this._eyeL != null && this._eyeL.parent != null)
			{
				this._eyeL.localPosition = this._eyeLRest + this._eyeL.parent.InverseTransformVector(this._eyeOffsetCurrent);
			}
			if (this._eyeR != null && this._eyeR.parent != null)
			{
				this._eyeR.localPosition = this._eyeRRest + this._eyeR.parent.InverseTransformVector(this._eyeOffsetCurrent);
			}
		}

		// Token: 0x040052D9 RID: 21209
		private static WearAdmireAnimation _instance;

		// Token: 0x040052DA RID: 21210
		private static readonly HashSet<int> PresentationSuppressionOwners = new HashSet<int>();

		// Token: 0x040052DB RID: 21211
		[Header("Trigger")]
		[Tooltip("Capi che fanno partire l'ammirazione (match su itemName O nome asset, case-insensitive).")]
		public List<string> importantClothes = new List<string> { "Top", "Top 2", "Bottom", "Swimsuit", "One-Piece Swimsuit", "Cheerleader Suit", "Cheerleader Suite", "Raincoat", "Towel", "Rope" };

		// Token: 0x040052DC RID: 21212
		[Tooltip("Cooldown minimo tra due ammirazioni (secondi).")]
		public float cooldown = 10f;

		// Token: 0x040052DD RID: 21213
		[Tooltip("Secondi di mute dopo boot/cambio scena (il sync iniziale del wardrobe non triggera).")]
		public float bootMute = 3f;

		// Token: 0x040052DE RID: 21214
		[Header("Blending")]
		[Tooltip("Ramp-in del peso layer.")]
		public float fadeIn = 0.3f;

		// Token: 0x040052DF RID: 21215
		[Tooltip("Ramp-out del peso layer.")]
		public float fadeOut = 0.5f;

		// Token: 0x040052E0 RID: 21216
		[Tooltip("Peso massimo del layer durante l'ammirazione.")]
		[Range(0f, 1f)]
		public float maxWeight = 1f;

		// Token: 0x040052E1 RID: 21217
		[Tooltip("Durata massima dell'ammirazione (la clip KA dura 9.3s: la tagliamo qui col ramp-out).")]
		public float maxDuration = 4.5f;

		// Token: 0x040052E2 RID: 21218
		[Header("Pupille (guardano le mani durante l'ammirazione)")]
		[Tooltip("Se attivo, le pupille vengono spostate verso le mani (stessa tecnica displacement di PlayerEyeLookAt, che nel frattempo viene sospeso).")]
		public bool eyeLook = true;

		// Token: 0x040052E3 RID: 21219
		[Tooltip("Spostamento orizzontale massimo delle pupille (metri, lungo il right della testa).")]
		public float eyeMaxHorizontal = 0.003f;

		// Token: 0x040052E4 RID: 21220
		[Tooltip("Spostamento verticale massimo delle pupille (metri, lungo l'up della testa). Guardare le mani e' uno sguardo molto basso: serve piu' corsa del glance NPC.")]
		public float eyeMaxVertical = 0.008f;

		// Token: 0x040052E5 RID: 21221
		[Tooltip("Smoothing del movimento pupille (piu' alto = piu' reattivo).")]
		public float eyeSmoothing = 12f;

		// Token: 0x040052E6 RID: 21222
		private const string LayerName = "WearAdmire";

		// Token: 0x040052E7 RID: 21223
		private const string StateName = "LookAtHands";

		// Token: 0x040052E8 RID: 21224
		private const string EmptyState = "Empty";

		// Token: 0x040052E9 RID: 21225
		private WearableUseController _wear;

		// Token: 0x040052EA RID: 21226
		private Animator _animator;

		// Token: 0x040052EB RID: 21227
		private vThirdPersonController _character;

		// Token: 0x040052EC RID: 21228
		private CoverPoseController _cover;

		// Token: 0x040052ED RID: 21229
		private int _layerIndex = -1;

		// Token: 0x040052EE RID: 21230
		private Transform _leftHand;

		// Token: 0x040052EF RID: 21231
		private Transform _rightHand;

		// Token: 0x040052F0 RID: 21232
		private Transform _head;

		// Token: 0x040052F1 RID: 21233
		private Behaviour _npcEyeLook;

		// Token: 0x040052F2 RID: 21234
		private bool _npcEyeLookWasEnabled;

		// Token: 0x040052F3 RID: 21235
		private Transform _eyeL;

		// Token: 0x040052F4 RID: 21236
		private Transform _eyeR;

		// Token: 0x040052F5 RID: 21237
		private Vector3 _eyeLRest;

		// Token: 0x040052F6 RID: 21238
		private Vector3 _eyeRRest;

		// Token: 0x040052F7 RID: 21239
		private bool _eyeOverrideActive;

		// Token: 0x040052F8 RID: 21240
		private bool _invertH;

		// Token: 0x040052F9 RID: 21241
		private bool _invertV;

		// Token: 0x040052FA RID: 21242
		private Vector3 _eyeOffsetCurrent;

		// Token: 0x040052FB RID: 21243
		private float _admireWeight;

		// Token: 0x040052FC RID: 21244
		private float _muteUntil;

		// Token: 0x040052FD RID: 21245
		private float _lastPlayTime = -999f;

		// Token: 0x040052FE RID: 21246
		private float _nextResolveTime;

		// Token: 0x040052FF RID: 21247
		private Coroutine _playRoutine;
	}
}
