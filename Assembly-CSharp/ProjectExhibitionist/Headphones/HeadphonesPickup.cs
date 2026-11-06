using System;
using System.Collections;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectExhibitionist.Headphones
{
	// Token: 0x020009D8 RID: 2520
	[DisallowMultipleComponent]
	public sealed class HeadphonesPickup : MonoBehaviour, IInteractable, IInteractionPickupCandidate, IInteractionCandidate
	{
		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06004703 RID: 18179 RVA: 0x001ADA0C File Offset: 0x001ABC0C
		public InteractionConfigSO Config
		{
			get
			{
				return this.interactionConfig;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06004704 RID: 18180 RVA: 0x001ADA14 File Offset: 0x001ABC14
		public bool IsBeingUsed
		{
			get
			{
				return this._holding;
			}
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06004705 RID: 18181 RVA: 0x001ADA1C File Offset: 0x001ABC1C
		public bool IsEquipped
		{
			get
			{
				return this._equipped;
			}
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06004706 RID: 18182 RVA: 0x001ADA24 File Offset: 0x001ABC24
		public HeadphonesFitProfileSO FitProfile
		{
			get
			{
				return this.fitProfile;
			}
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06004707 RID: 18183 RVA: 0x001ADA2C File Offset: 0x001ABC2C
		public float CandidateSqrDistance
		{
			get
			{
				if (!(this._playerTransform != null))
				{
					return float.MaxValue;
				}
				return this.GetInteractionSqrDistance(this._playerTransform.position);
			}
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06004708 RID: 18184 RVA: 0x001ADA53 File Offset: 0x001ABC53
		public bool IsCandidateEligible
		{
			get
			{
				return this.CandidateSqrDistance <= this.interactionRadius * this.interactionRadius && this.CanInteract(this._playerRoot);
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06004709 RID: 18185 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x0600470A RID: 18186 RVA: 0x001ADA78 File Offset: 0x001ABC78
		public bool CanPrioritizePickup
		{
			get
			{
				return this.CanInteract(this._playerRoot);
			}
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x001ADA86 File Offset: 0x001ABC86
		private void Awake()
		{
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			this.CacheWorldGeometry();
			this.CacheOriginalWorldState();
			this.SetPromptVisible(false, false);
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x001ADAB7 File Offset: 0x001ABCB7
		private void Start()
		{
			this.ResolvePlayer();
			this.PlacePromptAboveVisual();
			this.SetPromptVisible(false, false);
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x001ADACE File Offset: 0x001ABCCE
		private void OnEnable()
		{
			this._nextResolveFrame = 0;
			if (!this._equipped)
			{
				InteractionCandidateRegistry.Register(this);
			}
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x001ADAE5 File Offset: 0x001ABCE5
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this._candidateSqrDistance = float.MaxValue;
			this._inOwnRange = false;
			this.ResetHold();
			this.SetPromptVisible(false, false);
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x001ADB10 File Offset: 0x001ABD10
		private void Update()
		{
			if (this._equipped)
			{
				return;
			}
			if (this._playerTransform == null || this._equipment == null)
			{
				if (Time.frameCount < this._nextResolveFrame)
				{
					return;
				}
				this._nextResolveFrame = Time.frameCount + 30 + (base.GetInstanceID() & 15);
				if (!this.ResolvePlayer())
				{
					return;
				}
			}
			if (this._equipment.HasHeadphones && this._equipment.EquippedRoot != base.transform)
			{
				this.ResetHold();
				this.SetPromptVisible(false, false);
				if (this.hideDuplicateWhenAlreadyWorn)
				{
					base.gameObject.SetActive(false);
				}
				return;
			}
			this._candidateSqrDistance = this.GetInteractionSqrDistance(this._playerTransform.position);
			this._inOwnRange = this._candidateSqrDistance <= this.interactionRadius * this.interactionRadius;
			bool flag = this.IsCandidateEligible && InteractionCandidateRegistry.IsWinner(this) && PlayerActivityGate.Allows(PlayerActivity.Interact);
			bool flag2 = flag && this.promptUI != null && this.interactionConfig != null && (InteractablePromptUI.Current != this.promptUI || !this.promptUI.IsVisible);
			if (flag && (!this._promptVisible || flag2))
			{
				this.SetPromptVisible(true, true);
			}
			else if (!flag)
			{
				this.ResetHold();
				this.SetPromptVisible(false, false);
			}
			if (flag)
			{
				this.UpdateHoldInput();
			}
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x001ADC84 File Offset: 0x001ABE84
		private void UpdateHoldInput()
		{
			if (this.interactInput == null)
			{
				return;
			}
			if (this.interactInput.GetButtonDown())
			{
				this._holding = true;
				this._holdTime = 0f;
			}
			if (!this._holding)
			{
				return;
			}
			if (this.interactInput.GetButtonUp() || !this.interactInput.GetButton())
			{
				this.ResetHold();
				return;
			}
			this._holdTime += Time.deltaTime;
			if (this._holdTime < this.holdDuration)
			{
				return;
			}
			this.ResetHold();
			this.TryEquip(this._playerRoot);
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x001ADD17 File Offset: 0x001ABF17
		public bool CanInteract(GameObject player)
		{
			return !this._equipped && player != null && this._equipment != null && !this._equipment.HasHeadphones;
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x001ADD48 File Offset: 0x001ABF48
		public void OnInteract(GameObject player)
		{
			this.TryEquip(player);
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x001ADD52 File Offset: 0x001ABF52
		public void OnActionExecuted(string actionName, GameObject player)
		{
			this.TryEquip(player);
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x001ADD5C File Offset: 0x001ABF5C
		public bool TryEquip(GameObject player)
		{
			if (this._equipment == null || this._playerRoot == null)
			{
				this.ResolvePlayer();
			}
			if (!this.CanInteract((player != null) ? player : this._playerRoot))
			{
				return false;
			}
			if (!InteractionCandidateRegistry.TryCommitWorldInteraction())
			{
				return false;
			}
			Vector3 visualBoundsCenter = this.GetVisualBoundsCenter();
			this.SetPromptVisible(false, false);
			this.ResetHold();
			this.EnsureOriginalWorldStateCached();
			this.StopDropSettlement();
			if (!this._equipment.TryEquip(base.transform, this.fitProfile))
			{
				return false;
			}
			this._equipped = true;
			this.DisableWorldPhysics();
			if (this.equipSound != null)
			{
				AudioSource.PlayClipAtPoint(this.equipSound, visualBoundsCenter, this.equipSoundVolume);
			}
			UnityEvent unityEvent = this.onEquipped;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			base.enabled = false;
			return true;
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x001ADE34 File Offset: 0x001AC034
		private bool ResolvePlayer()
		{
			if (this._playerRoot == null)
			{
				this._playerRoot = PlayerRegistry.PlayerObject;
			}
			if (this._playerRoot == null)
			{
				return false;
			}
			this._playerTransform = this._playerRoot.transform;
			if (this._equipment == null)
			{
				this._equipment = HeadphonesEquipment.GetOrAdd(this._playerRoot);
			}
			return this._playerTransform != null && this._equipment != null;
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x001ADEB6 File Offset: 0x001AC0B6
		private void PlacePromptAboveVisual()
		{
			if (this.promptUI == null)
			{
				return;
			}
			this.promptUI.MatchGymPlacement(base.transform, this.promptWorldScale, this.promptVerticalGap);
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x001ADEE8 File Offset: 0x001AC0E8
		private void SetPromptVisible(bool visible, bool force = false)
		{
			if (!force && this._promptVisible == visible)
			{
				return;
			}
			this._promptVisible = visible;
			if (this.promptUI == null)
			{
				return;
			}
			if (visible)
			{
				if (this.promptUI.gameObject != base.gameObject && !this.promptUI.gameObject.activeSelf)
				{
					this.promptUI.gameObject.SetActive(true);
				}
				this.promptUI.Show(this.interactionConfig);
				return;
			}
			this.promptUI.Hide();
			if (this.promptUI.gameObject != base.gameObject)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x001ADF9C File Offset: 0x001AC19C
		private void ResetHold()
		{
			this._holding = false;
			this._holdTime = 0f;
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x001ADFB0 File Offset: 0x001AC1B0
		private void CacheWorldGeometry()
		{
			this._worldColliders = Array.FindAll<Collider>(base.GetComponentsInChildren<Collider>(true), (Collider component) => this.promptUI == null || !component.transform.IsChildOf(this.promptUI.transform));
			this._worldRenderers = Array.FindAll<Renderer>(base.GetComponentsInChildren<Renderer>(true), (Renderer component) => this.promptUI == null || !component.transform.IsChildOf(this.promptUI.transform));
			this._worldBodies = base.GetComponentsInChildren<Rigidbody>(true);
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x001AE008 File Offset: 0x001AC208
		private void CacheOriginalWorldState()
		{
			if (this._originalWorldStateCached)
			{
				return;
			}
			this._originalName = base.gameObject.name;
			this._originalWorldRotation = base.transform.rotation;
			this._originalWorldScale = base.transform.lossyScale;
			this._layerTransforms = base.GetComponentsInChildren<Transform>(true);
			this._originalLayers = new int[this._layerTransforms.Length];
			for (int i = 0; i < this._layerTransforms.Length; i++)
			{
				this._originalLayers[i] = ((this._layerTransforms[i] != null) ? this._layerTransforms[i].gameObject.layer : 0);
			}
			this._originalColliderEnabled = new bool[this._worldColliders.Length];
			for (int j = 0; j < this._worldColliders.Length; j++)
			{
				this._originalColliderEnabled[j] = this._worldColliders[j] != null && this._worldColliders[j].enabled;
			}
			this._originalBodyDetectCollisions = new bool[this._worldBodies.Length];
			for (int k = 0; k < this._worldBodies.Length; k++)
			{
				this._originalBodyDetectCollisions[k] = this._worldBodies[k] != null && this._worldBodies[k].detectCollisions;
			}
			this._originalWorldStateCached = true;
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x001AE154 File Offset: 0x001AC354
		private void EnsureOriginalWorldStateCached()
		{
			if (this._originalWorldStateCached)
			{
				return;
			}
			this.CacheWorldGeometry();
			this.CacheOriginalWorldState();
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x001AE16C File Offset: 0x001AC36C
		private float GetInteractionSqrDistance(Vector3 playerPosition)
		{
			float num = (base.transform.position - playerPosition).sqrMagnitude;
			bool flag = false;
			for (int i = 0; i < this._worldColliders.Length; i++)
			{
				Collider collider = this._worldColliders[i];
				if (!(collider == null) && collider.enabled && collider.gameObject.activeInHierarchy)
				{
					float sqrMagnitude = (collider.bounds.ClosestPoint(playerPosition) - playerPosition).sqrMagnitude;
					if (!flag || sqrMagnitude < num)
					{
						num = sqrMagnitude;
					}
					flag = true;
				}
			}
			if (flag)
			{
				return num;
			}
			for (int j = 0; j < this._worldRenderers.Length; j++)
			{
				Renderer renderer = this._worldRenderers[j];
				if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy)
				{
					float sqrMagnitude2 = (renderer.bounds.ClosestPoint(playerPosition) - playerPosition).sqrMagnitude;
					if (!flag || sqrMagnitude2 < num)
					{
						num = sqrMagnitude2;
					}
					flag = true;
				}
			}
			return num;
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x001AE278 File Offset: 0x001AC478
		private Vector3 GetVisualBoundsCenter()
		{
			Bounds bounds = default(Bounds);
			bool flag = false;
			for (int i = 0; i < this._worldRenderers.Length; i++)
			{
				Renderer renderer = this._worldRenderers[i];
				if (!(renderer == null) && renderer.enabled)
				{
					if (!flag)
					{
						bounds = renderer.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
			}
			if (!flag)
			{
				return base.transform.position;
			}
			return bounds.center;
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x001AE2F0 File Offset: 0x001AC4F0
		private void DisableWorldPhysics()
		{
			this.EnsureOriginalWorldStateCached();
			for (int i = 0; i < this._worldColliders.Length; i++)
			{
				if (this._worldColliders[i] != null)
				{
					this._worldColliders[i].enabled = false;
				}
			}
			for (int j = 0; j < this._worldBodies.Length; j++)
			{
				Rigidbody rigidbody = this._worldBodies[j];
				if (!(rigidbody == null))
				{
					rigidbody.velocity = Vector3.zero;
					rigidbody.angularVelocity = Vector3.zero;
					rigidbody.isKinematic = true;
					rigidbody.useGravity = false;
					rigidbody.detectCollisions = false;
				}
			}
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x001AE384 File Offset: 0x001AC584
		internal void RestoreToWorldAndBeginDrop(Vector3 groundSpot, GameObject player)
		{
			this.EnsureOriginalWorldStateCached();
			this.StopDropSettlement();
			this._playerRoot = player;
			this._playerTransform = ((player != null) ? player.transform : null);
			this._equipment = ((player != null) ? HeadphonesEquipment.GetOrAdd(player) : this._equipment);
			base.gameObject.name = (string.IsNullOrEmpty(this._originalName) ? "Headphones" : this._originalName);
			base.transform.rotation = this._originalWorldRotation;
			base.transform.localScale = this._originalWorldScale;
			base.transform.position = groundSpot + Vector3.up * this.dropSpawnHeight;
			this.RestoreOriginalLayers();
			this.RestoreWorldCollidersForManualDrop();
			this._equipped = false;
			this.ResetHold();
			this.SetPromptVisible(false, false);
			this.PlacePromptAboveVisual();
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			base.enabled = true;
			this._dropRoutine = base.StartCoroutine(this.FallAndSettleToGround());
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x001AE49C File Offset: 0x001AC69C
		private void RestoreOriginalLayers()
		{
			int num = Mathf.Min(this._layerTransforms.Length, this._originalLayers.Length);
			for (int i = 0; i < num; i++)
			{
				if (this._layerTransforms[i] != null)
				{
					this._layerTransforms[i].gameObject.layer = this._originalLayers[i];
				}
			}
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x001AE4F8 File Offset: 0x001AC6F8
		private void RestoreWorldCollidersForManualDrop()
		{
			int num = Mathf.Min(this._worldColliders.Length, this._originalColliderEnabled.Length);
			for (int i = 0; i < num; i++)
			{
				if (this._worldColliders[i] != null)
				{
					this._worldColliders[i].enabled = this._originalColliderEnabled[i];
				}
			}
			int num2 = Mathf.Min(this._worldBodies.Length, this._originalBodyDetectCollisions.Length);
			for (int j = 0; j < num2; j++)
			{
				Rigidbody rigidbody = this._worldBodies[j];
				if (!(rigidbody == null))
				{
					rigidbody.velocity = Vector3.zero;
					rigidbody.angularVelocity = Vector3.zero;
					rigidbody.isKinematic = true;
					rigidbody.useGravity = false;
					rigidbody.detectCollisions = this._originalBodyDetectCollisions[j];
				}
			}
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x001AE5B9 File Offset: 0x001AC7B9
		private IEnumerator FallAndSettleToGround()
		{
			float downwardSpeed = Mathf.Max(0.1f, this.dropInitialSpeed);
			float minimumY = base.transform.position.y - Mathf.Max(5f, this.dropRayDistance);
			while (!this._equipped && base.transform != null)
			{
				Vector3 position = base.transform.position;
				float deltaTime = Time.deltaTime;
				downwardSpeed = Mathf.Min(Mathf.Max(0.5f, this.dropMaximumSpeed), downwardSpeed + Mathf.Max(0.1f, this.dropAcceleration) * deltaTime);
				RaycastHit raycastHit;
				if (this.TryResolveDropGround(position + Vector3.up * 0.5f, out raycastHit))
				{
					float pivotToVisualBottom = this.GetPivotToVisualBottom(position);
					float num = raycastHit.point.y + pivotToVisualBottom + this.dropRestHeight;
					if (position.y <= num + 0.0005f)
					{
						base.transform.position = new Vector3(position.x, num, position.z);
						break;
					}
					float num2 = Mathf.Max(num, position.y - downwardSpeed * deltaTime);
					base.transform.position = new Vector3(position.x, num2, position.z);
				}
				else
				{
					base.transform.position = position - Vector3.up * downwardSpeed * deltaTime;
					if (position.y <= minimumY)
					{
						break;
					}
				}
				yield return null;
			}
			this._dropRoutine = null;
			if (!this._equipped)
			{
				this.PlacePromptAboveVisual();
			}
			yield break;
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x001AE5C8 File Offset: 0x001AC7C8
		private bool TryResolveDropGround(Vector3 origin, out RaycastHit nearest)
		{
			nearest = default(RaycastHit);
			RaycastHit[] array = Physics.RaycastAll(origin, Vector3.down, this.dropRayDistance, this.dropGroundMask, QueryTriggerInteraction.Ignore);
			float num = float.PositiveInfinity;
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				Collider collider = array[i].collider;
				if (!(collider == null))
				{
					Transform transform = collider.transform;
					if (!HeadphonesPickup.IsSameOrChild(transform, base.transform) && !HeadphonesPickup.IsSameOrChild(transform, this._playerTransform) && array[i].distance < num)
					{
						nearest = array[i];
						num = array[i].distance;
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x001AE67C File Offset: 0x001AC87C
		private float GetPivotToVisualBottom(Vector3 pivotPosition)
		{
			Bounds bounds = default(Bounds);
			bool flag = false;
			for (int i = 0; i < this._worldRenderers.Length; i++)
			{
				Renderer renderer = this._worldRenderers[i];
				if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy)
				{
					if (!flag)
					{
						bounds = renderer.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
			}
			if (!flag)
			{
				return 0f;
			}
			return pivotPosition.y - bounds.min.y;
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x001AE704 File Offset: 0x001AC904
		private void StopDropSettlement()
		{
			if (this._dropRoutine == null)
			{
				return;
			}
			base.StopCoroutine(this._dropRoutine);
			this._dropRoutine = null;
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x001AE722 File Offset: 0x001AC922
		private static bool IsSameOrChild(Transform candidate, Transform root)
		{
			return candidate != null && root != null && (candidate == root || candidate.IsChildOf(root));
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x001AE74A File Offset: 0x001AC94A
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(1f, 0.3f, 0.65f, 0.55f);
			Gizmos.DrawWireSphere(base.transform.position, this.interactionRadius);
		}

		// Token: 0x04004695 RID: 18069
		[Header("Interaction")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04004696 RID: 18070
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04004697 RID: 18071
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x04004698 RID: 18072
		[SerializeField]
		[Min(0.25f)]
		private float interactionRadius = 1.2f;

		// Token: 0x04004699 RID: 18073
		[SerializeField]
		[Min(0.1f)]
		private float holdDuration = 0.55f;

		// Token: 0x0400469A RID: 18074
		[Header("World prompt placement")]
		[SerializeField]
		[Min(0.0001f)]
		private float promptWorldScale = 0.0055f;

		// Token: 0x0400469B RID: 18075
		[SerializeField]
		[Min(0f)]
		private float promptVerticalGap = 0.12f;

		// Token: 0x0400469C RID: 18076
		[Header("World drop")]
		[SerializeField]
		[Min(0f)]
		private float dropSpawnHeight = 1.2f;

		// Token: 0x0400469D RID: 18077
		[SerializeField]
		[Min(0f)]
		private float dropRestHeight = 0.03f;

		// Token: 0x0400469E RID: 18078
		[SerializeField]
		[Min(0.1f)]
		private float dropInitialSpeed = 0.8f;

		// Token: 0x0400469F RID: 18079
		[SerializeField]
		[Min(0.1f)]
		private float dropAcceleration = 14f;

		// Token: 0x040046A0 RID: 18080
		[SerializeField]
		[Min(0.5f)]
		private float dropMaximumSpeed = 7f;

		// Token: 0x040046A1 RID: 18081
		[SerializeField]
		[Min(1f)]
		private float dropRayDistance = 60f;

		// Token: 0x040046A2 RID: 18082
		[SerializeField]
		private LayerMask dropGroundMask = -1;

		// Token: 0x040046A3 RID: 18083
		[Header("Worn fit")]
		[SerializeField]
		private HeadphonesFitProfileSO fitProfile;

		// Token: 0x040046A4 RID: 18084
		[Tooltip("When Ciel already owns the unique worn pair, scene reload copies stay hidden.")]
		[SerializeField]
		private bool hideDuplicateWhenAlreadyWorn = true;

		// Token: 0x040046A5 RID: 18085
		[Header("Feedback")]
		[SerializeField]
		private AudioClip equipSound;

		// Token: 0x040046A6 RID: 18086
		[SerializeField]
		[Range(0f, 1f)]
		private float equipSoundVolume = 1f;

		// Token: 0x040046A7 RID: 18087
		[SerializeField]
		private UnityEvent onEquipped;

		// Token: 0x040046A8 RID: 18088
		private GameObject _playerRoot;

		// Token: 0x040046A9 RID: 18089
		private Transform _playerTransform;

		// Token: 0x040046AA RID: 18090
		private HeadphonesEquipment _equipment;

		// Token: 0x040046AB RID: 18091
		private Collider[] _worldColliders = Array.Empty<Collider>();

		// Token: 0x040046AC RID: 18092
		private Renderer[] _worldRenderers = Array.Empty<Renderer>();

		// Token: 0x040046AD RID: 18093
		private Rigidbody[] _worldBodies = Array.Empty<Rigidbody>();

		// Token: 0x040046AE RID: 18094
		private bool[] _originalColliderEnabled = Array.Empty<bool>();

		// Token: 0x040046AF RID: 18095
		private bool[] _originalBodyDetectCollisions = Array.Empty<bool>();

		// Token: 0x040046B0 RID: 18096
		private Transform[] _layerTransforms = Array.Empty<Transform>();

		// Token: 0x040046B1 RID: 18097
		private int[] _originalLayers = Array.Empty<int>();

		// Token: 0x040046B2 RID: 18098
		private string _originalName;

		// Token: 0x040046B3 RID: 18099
		private Quaternion _originalWorldRotation;

		// Token: 0x040046B4 RID: 18100
		private Vector3 _originalWorldScale;

		// Token: 0x040046B5 RID: 18101
		private Coroutine _dropRoutine;

		// Token: 0x040046B6 RID: 18102
		private float _candidateSqrDistance = float.MaxValue;

		// Token: 0x040046B7 RID: 18103
		private float _holdTime;

		// Token: 0x040046B8 RID: 18104
		private bool _inOwnRange;

		// Token: 0x040046B9 RID: 18105
		private bool _promptVisible;

		// Token: 0x040046BA RID: 18106
		private bool _holding;

		// Token: 0x040046BB RID: 18107
		private bool _equipped;

		// Token: 0x040046BC RID: 18108
		private bool _originalWorldStateCached;

		// Token: 0x040046BD RID: 18109
		private int _nextResolveFrame;

		// Token: 0x040046BE RID: 18110
		private const int ReferenceResolveRetryFrames = 30;
	}
}
