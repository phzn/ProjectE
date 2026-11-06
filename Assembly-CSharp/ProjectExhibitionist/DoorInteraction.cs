using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace ProjectExhibitionist
{
	// Token: 0x020008D7 RID: 2263
	public class DoorInteraction : MonoBehaviour, IInteractionInputCandidate, IInteractionCandidate
	{
		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06003D09 RID: 15625 RVA: 0x0016AF91 File Offset: 0x00169191
		public float CandidateSqrDistance
		{
			get
			{
				return this._candidateSqrDistance;
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06003D0A RID: 15626 RVA: 0x0016AF99 File Offset: 0x00169199
		public bool IsCandidateEligible
		{
			get
			{
				return this._playerInRange != null && !this._simulationPaused;
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06003D0B RID: 15627 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06003D0C RID: 15628 RVA: 0x0016AFB4 File Offset: 0x001691B4
		public string CandidateInputBinding
		{
			get
			{
				return InteractionCandidateRegistry.ActiveBinding(this.interactInput);
			}
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06003D0D RID: 15629 RVA: 0x0016AFC1 File Offset: 0x001691C1
		// (set) Token: 0x06003D0E RID: 15630 RVA: 0x0016AFC9 File Offset: 0x001691C9
		public bool SimulationPaused
		{
			get
			{
				return this._simulationPaused;
			}
			set
			{
				this._simulationPaused = value;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06003D0F RID: 15631 RVA: 0x0016AFD2 File Offset: 0x001691D2
		private DoorInteraction.DoorMechanism ResolvedMechanism
		{
			get
			{
				if (this.mechanism != DoorInteraction.DoorMechanism.Auto)
				{
					return this.mechanism;
				}
				if (!(this.interactiveObject != null) && !(this.doorAnimation != null))
				{
					return DoorInteraction.DoorMechanism.Rotate;
				}
				return DoorInteraction.DoorMechanism.Animation;
			}
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x0016B004 File Offset: 0x00169204
		private void Awake()
		{
			this.ResolveSiblingRefs();
			if (this.interactiveObject == null)
			{
				this.interactiveObject = base.GetComponentInChildren<InteractiveObject>(true);
			}
			if (this.doorAnimation == null)
			{
				this.doorAnimation = base.GetComponentInChildren<Animation>(true);
			}
			if (this.rotatePivot == null)
			{
				this.rotatePivot = base.transform;
			}
			this.ResolveZoneCollider();
			this.InitRotateRotations();
			if (this.audioSource == null)
			{
				this.audioSource = base.GetComponent<AudioSource>();
			}
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x0016B090 File Offset: 0x00169290
		private void ResolveSiblingRefs()
		{
			Transform parent = base.transform.parent;
			string text = (string.IsNullOrEmpty(this.siblingKey) ? base.name : this.siblingKey);
			if (this.promptUI == null)
			{
				Transform transform = DoorInteraction.FindDeep(base.transform, "DoorInteractUI_" + text) ?? ((parent != null) ? parent.Find("DoorInteractUI_" + text) : null);
				if (transform != null)
				{
					this.promptUI = transform.GetComponent<InteractablePromptUI>();
				}
				if (this.promptUI == null)
				{
					this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
				}
			}
			if (this.zoneCollider == null)
			{
				Transform transform2 = DoorInteraction.FindDeep(base.transform, "DoorTriggerZone_" + text) ?? ((parent != null) ? parent.Find("DoorTriggerZone_" + text) : null);
				if (transform2 != null)
				{
					this.zoneCollider = transform2.GetComponent<Collider>();
				}
			}
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x0016B198 File Offset: 0x00169398
		private static Transform FindDeep(Transform root, string childName)
		{
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				if (child.name == childName)
				{
					return child;
				}
				Transform transform = DoorInteraction.FindDeep(child, childName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x0016B1E4 File Offset: 0x001693E4
		private void ResolveZoneCollider()
		{
			if (this.zoneCollider != null)
			{
				this._zoneCollider = this.zoneCollider;
				return;
			}
			Transform transform = base.transform.Find("DoorTriggerZone");
			if (transform != null)
			{
				this._zoneCollider = transform.GetComponent<Collider>();
			}
			if (this._zoneCollider == null)
			{
				foreach (Collider collider in base.GetComponentsInChildren<Collider>(true))
				{
					if (collider.isTrigger)
					{
						this._zoneCollider = collider;
						return;
					}
				}
			}
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x0016B26A File Offset: 0x0016946A
		private void Start()
		{
			if (this.promptUI != null)
			{
				this.promptUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void OnEnable()
		{
			InteractionCandidateRegistry.Register(this);
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x0016B28B File Offset: 0x0016948B
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this._candidateSqrDistance = float.MaxValue;
			this.HidePrompt();
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x0016B2A4 File Offset: 0x001694A4
		private void InitRotateRotations()
		{
			if (this._rotateInitialised || this.rotatePivot == null)
			{
				return;
			}
			this._closedRotation = this.rotatePivot.localRotation;
			this._openRotation = this._closedRotation * Quaternion.Euler(0f, this.openAngle, 0f);
			if (this.secondaryRotatePivot != null && this.secondaryRotatePivot != this.rotatePivot)
			{
				this._secondaryClosedRotation = this.secondaryRotatePivot.localRotation;
				this._secondaryOpenRotation = this._secondaryClosedRotation * Quaternion.Euler(0f, this.secondaryOpenAngle, 0f);
			}
			this._rotateInitialised = true;
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x0016B360 File Offset: 0x00169560
		private void Update()
		{
			if (this._simulationPaused)
			{
				return;
			}
			this.UpdatePlayerInRange();
			this._candidateSqrDistance = ((this._playerInRange != null) ? (this.GetDoorCenter() - this._playerInRange.transform.position).sqrMagnitude : float.MaxValue);
			this.RefreshPrompt();
			if (this._playerInRange == null || !InteractionCandidateRegistry.IsWinner(this))
			{
				return;
			}
			if (this.interactInput == null || !this.interactInput.GetButtonDown() || !InteractionCandidateRegistry.TryConsumeContextInput(this.interactInput) || !InteractionCandidateRegistry.TryCommitWorldInteraction())
			{
				return;
			}
			this.Toggle();
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x0016B408 File Offset: 0x00169608
		private void UpdatePlayerInRange()
		{
			Transform transform = this.ResolvePlayer();
			bool flag = false;
			if (transform != null && this._zoneCollider != null)
			{
				flag = this._zoneCollider.bounds.Contains(transform.position);
			}
			if (flag)
			{
				if (this._playerInRange == null)
				{
					this._playerInRange = transform.gameObject;
					this.RefreshPrompt();
					return;
				}
			}
			else if (this._playerInRange != null)
			{
				this._playerInRange = null;
				this.HidePrompt();
			}
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x0016B490 File Offset: 0x00169690
		private Transform ResolvePlayer()
		{
			Transform player = PlayerRegistry.Player;
			if (player != null)
			{
				return player;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag(this.playerTag);
			if (!(gameObject != null))
			{
				return null;
			}
			return gameObject.transform;
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x0016B4CB File Offset: 0x001696CB
		private void Toggle()
		{
			this.LastPlayerInteractTime = Time.time;
			if (this._isOpen)
			{
				this.CloseDoor();
				return;
			}
			this.OpenDoor();
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06003D1C RID: 15644 RVA: 0x0016B4ED File Offset: 0x001696ED
		// (set) Token: 0x06003D1D RID: 15645 RVA: 0x0016B4F5 File Offset: 0x001696F5
		public float LastPlayerInteractTime { get; private set; } = -999f;

		// Token: 0x06003D1E RID: 15646 RVA: 0x0016B4FE File Offset: 0x001696FE
		internal void ShiftSimulationTime(float seconds)
		{
			this.LastPlayerInteractTime += Mathf.Max(0f, seconds);
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x0016B518 File Offset: 0x00169718
		public void OpenDoor()
		{
			if (this._isOpen)
			{
				return;
			}
			this._isOpen = true;
			DoorInteraction.DoorMechanism resolvedMechanism = this.ResolvedMechanism;
			if (resolvedMechanism != DoorInteraction.DoorMechanism.Animation)
			{
				if (resolvedMechanism == DoorInteraction.DoorMechanism.Rotate)
				{
					this.StartRotate(this._openRotation, this._secondaryOpenRotation);
					this.PlaySound(this.openSound);
				}
			}
			else
			{
				this.DriveAnimation(true);
			}
			UnityEvent onDoorOpened = this.OnDoorOpened;
			if (onDoorOpened != null)
			{
				onDoorOpened.Invoke();
			}
			this.RefreshPrompt();
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x0016B584 File Offset: 0x00169784
		public void CloseDoor()
		{
			if (!this._isOpen)
			{
				return;
			}
			this._isOpen = false;
			DoorInteraction.DoorMechanism resolvedMechanism = this.ResolvedMechanism;
			if (resolvedMechanism != DoorInteraction.DoorMechanism.Animation)
			{
				if (resolvedMechanism == DoorInteraction.DoorMechanism.Rotate)
				{
					this.StartRotate(this._closedRotation, this._secondaryClosedRotation);
					this.PlaySound(this.closeSound);
				}
			}
			else
			{
				this.DriveAnimation(false);
			}
			UnityEvent onDoorClosed = this.OnDoorClosed;
			if (onDoorClosed != null)
			{
				onDoorClosed.Invoke();
			}
			this.RefreshPrompt();
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x0016B5F0 File Offset: 0x001697F0
		private void DriveAnimation(bool open)
		{
			if (this.interactiveObject != null)
			{
				this.interactiveObject.TrigegrInteraction();
				return;
			}
			if (this.doorAnimation != null)
			{
				string text = (open ? this.openClipName : this.closeClipName);
				if (!string.IsNullOrEmpty(text) && this.doorAnimation.GetClip(text) != null)
				{
					this.doorAnimation.Play(text);
				}
			}
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x0016B660 File Offset: 0x00169860
		private void StartRotate(Quaternion target, Quaternion secondaryTarget)
		{
			this.InitRotateRotations();
			if (this._rotateRoutine != null)
			{
				base.StopCoroutine(this._rotateRoutine);
			}
			this._rotateRoutine = base.StartCoroutine(this.RotateTo(target, secondaryTarget));
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x0016B690 File Offset: 0x00169890
		private IEnumerator RotateTo(Quaternion target, Quaternion secondaryTarget)
		{
			while ((this.rotatePivot != null && Quaternion.Angle(this.rotatePivot.localRotation, target) > 0.05f) || (this.secondaryRotatePivot != null && this.secondaryRotatePivot != this.rotatePivot && Quaternion.Angle(this.secondaryRotatePivot.localRotation, secondaryTarget) > 0.05f))
			{
				while (this._simulationPaused)
				{
					yield return null;
				}
				float num = this.rotateSpeed * Time.deltaTime;
				if (this.rotatePivot != null)
				{
					this.rotatePivot.localRotation = Quaternion.RotateTowards(this.rotatePivot.localRotation, target, num);
				}
				if (this.secondaryRotatePivot != null && this.secondaryRotatePivot != this.rotatePivot)
				{
					this.secondaryRotatePivot.localRotation = Quaternion.RotateTowards(this.secondaryRotatePivot.localRotation, secondaryTarget, num);
				}
				yield return null;
			}
			if (this.rotatePivot != null)
			{
				this.rotatePivot.localRotation = target;
			}
			if (this.secondaryRotatePivot != null && this.secondaryRotatePivot != this.rotatePivot)
			{
				this.secondaryRotatePivot.localRotation = secondaryTarget;
			}
			this._rotateRoutine = null;
			yield break;
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x0016B6B0 File Offset: 0x001698B0
		private void PlaySound(AudioClip clip)
		{
			if (clip == null)
			{
				return;
			}
			if (this.audioSource != null)
			{
				this.audioSource.PlayOneShot(clip);
				return;
			}
			AudioSource.PlayClipAtPoint(clip, ((this.rotatePivot != null) ? this.rotatePivot : base.transform).position);
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x0016B70C File Offset: 0x0016990C
		private void RefreshPrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			if (this._playerInRange == null || !InteractionCandidateRegistry.IsPromptWinner(this))
			{
				this.HidePrompt();
				return;
			}
			InteractionConfigSO interactionConfigSO = (this._isOpen ? ((this.configOpen != null) ? this.configOpen : this.configClosed) : this.configClosed);
			if (!this._promptShown || this._promptShownIsOpen != this._isOpen)
			{
				if (interactionConfigSO != null)
				{
					this.promptUI.Show(interactionConfigSO);
				}
				else
				{
					this.promptUI.gameObject.SetActive(true);
				}
				this._promptShown = true;
				this._promptShownIsOpen = this._isOpen;
			}
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x0016B7C3 File Offset: 0x001699C3
		private void HidePrompt()
		{
			if (this.promptUI != null && this._promptShown)
			{
				this.promptUI.Hide();
			}
			this._promptShown = false;
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06003D27 RID: 15655 RVA: 0x0016B7ED File Offset: 0x001699ED
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06003D28 RID: 15656 RVA: 0x0016B7F5 File Offset: 0x001699F5
		public InteractablePromptUI PromptUI
		{
			get
			{
				return this.promptUI;
			}
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x0016B7FD File Offset: 0x001699FD
		public bool TryGetZoneBounds(out Bounds bounds)
		{
			if (this._zoneCollider != null)
			{
				bounds = this._zoneCollider.bounds;
				return true;
			}
			bounds = default(Bounds);
			return false;
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x0016B828 File Offset: 0x00169A28
		public void ForceOpen()
		{
			if (!this._isOpen)
			{
				this.OpenDoor();
			}
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x0016B838 File Offset: 0x00169A38
		public void ForceClose()
		{
			if (this._isOpen)
			{
				this.CloseDoor();
			}
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x0016B848 File Offset: 0x00169A48
		public void RecenterPrompt()
		{
			if (this.promptUI == null)
			{
				return;
			}
			Bounds bounds;
			if (this.TryGetDoorBounds(out bounds))
			{
				this.promptUI.transform.position = bounds.center;
			}
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x0016B888 File Offset: 0x00169A88
		public Vector3 GetDoorCenter()
		{
			Bounds bounds;
			if (!this.TryGetDoorBounds(out bounds))
			{
				return base.transform.position;
			}
			return bounds.center;
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x0016B8B4 File Offset: 0x00169AB4
		public Vector3 GetFrontNormal()
		{
			Renderer renderer = null;
			float num = -1f;
			List<Renderer> list;
			Vector3 vector5;
			using (CollectionPool<List<Renderer>, Renderer>.Get(out list))
			{
				base.GetComponentsInChildren<Renderer>(true, list);
				foreach (Renderer renderer2 in list)
				{
					if ((!(this.promptUI != null) || !renderer2.transform.IsChildOf(this.promptUI.transform)) && (renderer2 is MeshRenderer || renderer2 is SkinnedMeshRenderer))
					{
						MeshFilter component = renderer2.GetComponent<MeshFilter>();
						Vector3 vector = ((component != null && component.sharedMesh != null) ? component.sharedMesh.bounds.size : Vector3.one);
						float num2 = vector.x * vector.y * vector.z;
						if (num2 > num)
						{
							num = num2;
							renderer = renderer2;
						}
					}
				}
				if (renderer != null)
				{
					MeshFilter component2 = renderer.GetComponent<MeshFilter>();
					Vector3 vector2 = ((component2 != null && component2.sharedMesh != null) ? component2.sharedMesh.bounds.size : Vector3.one);
					Vector3 vector3 = ((vector2.x <= vector2.y && vector2.x <= vector2.z) ? Vector3.right : ((vector2.z <= vector2.x && vector2.z <= vector2.y) ? Vector3.forward : Vector3.up));
					Vector3 vector4 = renderer.transform.TransformDirection(vector3);
					vector4.y = 0f;
					if (vector4.sqrMagnitude > 0.0001f)
					{
						return vector4.normalized;
					}
				}
				Vector3 forward = base.transform.forward;
				forward.y = 0f;
				vector5 = ((forward.sqrMagnitude > 0.0001f) ? forward.normalized : Vector3.forward);
			}
			return vector5;
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x0016BAFC File Offset: 0x00169CFC
		private bool TryGetDoorBounds(out Bounds bounds)
		{
			bounds = default(Bounds);
			List<Renderer> list;
			bool flag2;
			using (CollectionPool<List<Renderer>, Renderer>.Get(out list))
			{
				base.GetComponentsInChildren<Renderer>(true, list);
				bool flag = false;
				foreach (Renderer renderer in list)
				{
					if (!(this.promptUI != null) || !renderer.transform.IsChildOf(this.promptUI.transform))
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
				flag2 = flag;
			}
			return flag2;
		}

		// Token: 0x04003CEB RID: 15595
		[Header("Trigger")]
		[Tooltip("Tag the player GameObject must have to be detected (fallback when no PlayerRegistry).")]
		[SerializeField]
		private string playerTag = "Player";

		// Token: 0x04003CEC RID: 15596
		[Tooltip("Generic input (keyboard / joystick / mobile) the player presses to open/close the door. Same input type used across the game's interactions.")]
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "X", "E");

		// Token: 0x04003CED RID: 15597
		[Header("Prompt UI (shared with the Showers)")]
		[Tooltip("Shared interaction prompt. If null, found in children/this object. Shown when the player is in range.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04003CEE RID: 15598
		[Tooltip("Config shown by the prompt while the door is CLOSED (e.g. action = 'Apri').")]
		[SerializeField]
		private InteractionConfigSO configClosed;

		// Token: 0x04003CEF RID: 15599
		[Tooltip("Config shown by the prompt while the door is OPEN (e.g. action = 'Chiudi'). If null, configClosed is reused.")]
		[SerializeField]
		private InteractionConfigSO configOpen;

		// Token: 0x04003CF0 RID: 15600
		[Header("Detection")]
		[Tooltip("Trigger collider whose world bounds define the interaction area. If null, the sibling 'DoorTriggerZone_<siblingKey>' is used. The door is detected by bounds-containment polled each frame, so this stays STILL while the door moves (it is NOT a child of the moving part).")]
		[SerializeField]
		private Collider zoneCollider;

		// Token: 0x04003CF1 RID: 15601
		[Tooltip("Unique key the setup tool stamps to name this door's sibling zone/prompt ('DoorTriggerZone_<key>' / 'DoorInteractUI_<key>'), so they can be self-wired by name even when Unity drops the serialized scene references. Set by the setup tool.")]
		[SerializeField]
		private string siblingKey = "";

		// Token: 0x04003CF2 RID: 15602
		[Header("Open Mechanism")]
		[Tooltip("Auto picks Animation when an InteractiveObject/Animation is present, else Rotate.")]
		[SerializeField]
		private DoorInteraction.DoorMechanism mechanism;

		// Token: 0x04003CF3 RID: 15603
		[Header("Animation mechanism")]
		[Tooltip("The third-party InteractiveObject that toggles the door's Open/Close clips + sound. If null, looked up on this object and its children.")]
		[SerializeField]
		private InteractiveObject interactiveObject;

		// Token: 0x04003CF4 RID: 15604
		[Tooltip("Fallback Animation component (with 'Open'/'Close' clips) if there is no InteractiveObject. If null, looked up on this object and its children.")]
		[SerializeField]
		private Animation doorAnimation;

		// Token: 0x04003CF5 RID: 15605
		[SerializeField]
		private string openClipName = "Open";

		// Token: 0x04003CF6 RID: 15606
		[SerializeField]
		private string closeClipName = "Close";

		// Token: 0x04003CF7 RID: 15607
		[Header("Rotate mechanism")]
		[Tooltip("Transform that rotates when opening/closing. If null, this transform is used. For SM_door_basic_01 the mesh itself is rotated.")]
		[SerializeField]
		private Transform rotatePivot;

		// Token: 0x04003CF8 RID: 15608
		[Tooltip("Degrees the door swings open (around the pivot's local Y).")]
		[SerializeField]
		private float openAngle = 90f;

		// Token: 0x04003CF9 RID: 15609
		[Tooltip("Optional second leaf operated by the same interaction. Use a different pivot from rotatePivot; leave empty for a single door.")]
		[SerializeField]
		private Transform secondaryRotatePivot;

		// Token: 0x04003CFA RID: 15610
		[Tooltip("Degrees the second leaf swings around its own local Y. Use the opposite sign for a mirrored pair of doors.")]
		[SerializeField]
		private float secondaryOpenAngle = -90f;

		// Token: 0x04003CFB RID: 15611
		[Tooltip("How fast the door swings, in degrees per second.")]
		[SerializeField]
		private float rotateSpeed = 180f;

		// Token: 0x04003CFC RID: 15612
		[Tooltip("Optional sounds for the Rotate mechanism (the Animation path uses InteractiveObject's own sounds).")]
		[SerializeField]
		private AudioClip openSound;

		// Token: 0x04003CFD RID: 15613
		[SerializeField]
		private AudioClip closeSound;

		// Token: 0x04003CFE RID: 15614
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04003CFF RID: 15615
		[Header("Events")]
		public UnityEvent OnDoorOpened;

		// Token: 0x04003D00 RID: 15616
		public UnityEvent OnDoorClosed;

		// Token: 0x04003D01 RID: 15617
		private GameObject _playerInRange;

		// Token: 0x04003D02 RID: 15618
		private bool _isOpen;

		// Token: 0x04003D03 RID: 15619
		private bool _promptShown;

		// Token: 0x04003D04 RID: 15620
		private bool _promptShownIsOpen;

		// Token: 0x04003D05 RID: 15621
		private Collider _zoneCollider;

		// Token: 0x04003D06 RID: 15622
		private Quaternion _closedRotation;

		// Token: 0x04003D07 RID: 15623
		private Quaternion _openRotation;

		// Token: 0x04003D08 RID: 15624
		private Quaternion _secondaryClosedRotation;

		// Token: 0x04003D09 RID: 15625
		private Quaternion _secondaryOpenRotation;

		// Token: 0x04003D0A RID: 15626
		private Coroutine _rotateRoutine;

		// Token: 0x04003D0B RID: 15627
		private bool _rotateInitialised;

		// Token: 0x04003D0C RID: 15628
		private bool _simulationPaused;

		// Token: 0x04003D0D RID: 15629
		private float _candidateSqrDistance = float.MaxValue;

		// Token: 0x020008D8 RID: 2264
		public enum DoorMechanism
		{
			// Token: 0x04003D10 RID: 15632
			Auto,
			// Token: 0x04003D11 RID: 15633
			Animation,
			// Token: 0x04003D12 RID: 15634
			Rotate
		}
	}
}
