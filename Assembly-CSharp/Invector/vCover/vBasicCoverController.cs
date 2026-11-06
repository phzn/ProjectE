using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Features.Flash;
using Features.Inventory;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCover
{
	// Token: 0x0200067A RID: 1658
	[vClassHeader("Basic Cover Controller", true, "icon_v2", false, "")]
	public class vBasicCoverController : vMonoBehaviour
	{
		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002657 RID: 9815 RVA: 0x000D4E02 File Offset: 0x000D3002
		protected bool UsesFirstPersonCoverView
		{
			get
			{
				if (this.firstPersonController == null)
				{
					this.firstPersonController = base.GetComponent<FirstPersonController>();
				}
				return this.firstPersonController != null && this.firstPersonController.IsFirstPerson;
			}
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000D4E3C File Offset: 0x000D303C
		public bool CanReleasePeekCameraForFirstPerson(Camera output)
		{
			if (!this.inCover || !this.hasPeekCameraLease || this.brainWasEnabledBeforePeek || this.cinemachineBrain == null || output == null || this.cinemachineBrain.OutputCamera != output)
			{
				return false;
			}
			if (!this.IsOwnedPeekCameraOrEmpty(this.cinemachineBrain.ActiveVirtualCamera))
			{
				return false;
			}
			CinemachineBlend activeBlend = this.cinemachineBrain.ActiveBlend;
			return activeBlend == null || (this.IsOwnedPeekCameraOrEmpty(activeBlend.CamA) && this.IsOwnedPeekCameraOrEmpty(activeBlend.CamB));
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x000D4ECF File Offset: 0x000D30CF
		public bool TryReleasePeekCameraForFirstPerson(Camera output)
		{
			if (!this.CanReleasePeekCameraForFirstPerson(output))
			{
				return false;
			}
			bool flag = this.hasSavedCameraState;
			this.DeactivatePeekCameraImmediate();
			if (flag)
			{
				output.fieldOfView = this.savedCameraFieldOfView;
				output.nearClipPlane = this.savedCameraNearClipPlane;
			}
			return true;
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x000D4F03 File Offset: 0x000D3103
		private bool IsOwnedPeekCameraOrEmpty(ICinemachineCamera camera)
		{
			return camera == null || camera == this.peekVirtualCamera || camera == this.anchorCamera;
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x0600265B RID: 9819 RVA: 0x000D4F1C File Offset: 0x000D311C
		public float rotationSpeed
		{
			get
			{
				return Mathf.Lerp(this.rotationSpeedIdle, this.rotationSpeedMoving, Mathf.Clamp(this.tpInput.cc.input.magnitude, 0f, 1f));
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x0600265C RID: 9820 RVA: 0x000D4F61 File Offset: 0x000D3161
		// (set) Token: 0x0600265D RID: 9821 RVA: 0x000D4F69 File Offset: 0x000D3169
		public bool CanPeekNow { get; protected set; }

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x0600265E RID: 9822 RVA: 0x000D4F72 File Offset: 0x000D3172
		protected float PositionOffsetZ
		{
			get
			{
				return (this.inCover ? this.tpInput.cc._capsuleCollider.radius : this.capsuleRadiusInCover) + this.positionOffsetZ;
			}
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x000D4FA0 File Offset: 0x000D31A0
		protected virtual void OnEnable()
		{
			if (this.coverControllerInitialized)
			{
				this.SubscribeCoverInputCallbacks();
			}
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x000D4FB0 File Offset: 0x000D31B0
		protected virtual void OnDisable()
		{
			this.UnsubscribeCoverInputCallbacks();
			this.ReleaseCoverActivity();
			if (!this.coverControllerInitialized)
			{
				return;
			}
			if (this.inCover || this.goingToCoverPoint)
			{
				this.ExitCover(true);
				return;
			}
			this.CanPeekNow = false;
			this.peekInputRequiresRelease = false;
			this.isPeeking = false;
			if (this.tpInput != null && this.tpInput.cc != null && this.tpInput.cc.animator != null)
			{
				this.tpInput.cc.animator.SetBool(this.peekingParameter, false);
			}
			this.DeactivatePeekCameraImmediate();
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x000D505A File Offset: 0x000D325A
		protected IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			this.tpInput = base.GetComponent<vThirdPersonInput>();
			if (this.tpInput == null)
			{
				Debug.LogError("[vBasicCoverController] vThirdPersonInput not found! This controller requires vThirdPersonInput.");
				base.enabled = false;
				yield break;
			}
			this.originalWalkByDefault = this.tpInput.cc.freeSpeed.walkByDefault;
			this.genericAction = base.GetComponent<vGenericAction>();
			this.helper = new GameObject("CoverHelper").transform;
			this.helper.gameObject.hideFlags = HideFlags.HideInHierarchy;
			this.lastMoveToDirectionInFree = this.tpInput.cc.moveToDirectionInFree;
			this.onEnterCover.AddListener(delegate
			{
				this.tpInput.cc.colliderRadius = this.capsuleRadiusInCover;
			});
			this.onExitCover.AddListener(delegate
			{
				this.tpInput.cc.colliderRadius = this.tpInput.cc.colliderRadiusDefault;
			});
			this.additionalLayerIndices.Clear();
			foreach (string text in this.additionalAnimatorLayers)
			{
				if (!string.IsNullOrEmpty(text))
				{
					int layerIndex = this.tpInput.cc.animator.GetLayerIndex(text);
					if (layerIndex >= 0)
					{
						this.additionalLayerIndices.Add(layerIndex);
						if (this.debug)
						{
							Debug.Log(string.Format("[vBasicCoverController] Additional layer '{0}' index: {1}", text, layerIndex));
						}
					}
					else if (this.debug)
					{
						Debug.LogWarning("[vBasicCoverController] Layer '" + text + "' not found in animator");
					}
				}
			}
			if (this.debug)
			{
				Debug.Log("[vBasicCoverController] Initialized with vThirdPersonInput");
			}
			this.InitializePeekCamera();
			this.coverControllerInitialized = true;
			this.SubscribeCoverInputCallbacks();
			yield break;
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x000D506C File Offset: 0x000D326C
		protected virtual void SubscribeCoverInputCallbacks()
		{
			if (this.coverInputCallbacksSubscribed || this.tpInput == null || !base.isActiveAndEnabled)
			{
				return;
			}
			this.tpInput.onFixedUpdate += this.UpdateCover;
			this.tpInput.onUpdate += this.UpdateInput;
			this.coverInputCallbacksSubscribed = true;
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x000D50D0 File Offset: 0x000D32D0
		protected virtual void UnsubscribeCoverInputCallbacks()
		{
			if (!this.coverInputCallbacksSubscribed || this.tpInput == null)
			{
				return;
			}
			this.tpInput.onFixedUpdate -= this.UpdateCover;
			this.tpInput.onUpdate -= this.UpdateInput;
			this.coverInputCallbacksSubscribed = false;
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x000D512C File Offset: 0x000D332C
		protected virtual void InitializePeekCamera()
		{
			if (this.peekVirtualCamera != null)
			{
				this.originalPeekCameraPriority = this.peekVirtualCamera.Priority;
				this.originalPeekFollow = this.peekVirtualCamera.Follow;
				this.originalPeekLookAt = this.peekVirtualCamera.LookAt;
				this.peekCameraTransposer = this.peekVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
				this.peekCameraComposer = this.peekVirtualCamera.GetCinemachineComponent<CinemachineComposer>();
				if (this.peekCameraTransposer != null)
				{
					this.originalPeekCameraFollowOffset = this.peekCameraTransposer.m_FollowOffset;
				}
				else
				{
					Debug.LogWarning("[vBasicCoverController] Peek camera does not have a CinemachineTransposer. Camera offset will not work.");
				}
				if (this.peekCameraComposer != null)
				{
					this.originalPeekCameraAimOffset = this.peekCameraComposer.m_TrackedObjectOffset;
					if (this.originalPeekCameraAimOffset.sqrMagnitude > 100f)
					{
						this.originalPeekCameraAimOffset = Vector3.zero;
						this.peekCameraComposer.m_TrackedObjectOffset = Vector3.zero;
					}
				}
				else
				{
					Debug.LogWarning("[vBasicCoverController] Peek camera does not have a CinemachineComposer. Aim offset will not work.");
				}
				this.ConfigurePeekCameraCollision();
				this.CreatePeekCameraTargets();
				this.peekVirtualCamera.Priority = this.originalPeekCameraPriority;
				this.peekVirtualCamera.gameObject.SetActive(false);
				if (this.debug)
				{
					Debug.Log(string.Format("[vBasicCoverController] Peek camera initialized and disabled. Priority: {0}", this.originalPeekCameraPriority));
				}
			}
			if (this.tpInput != null && this.tpInput.cameraMain != null)
			{
				this.cinemachineBrain = this.tpInput.cameraMain.GetComponent<CinemachineBrain>();
				if (this.cinemachineBrain == null)
				{
					Debug.LogWarning("[vBasicCoverController] CinemachineBrain not found on main camera. Peek camera switching may not work correctly.");
				}
			}
			this.CreateAnchorCamera();
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x000D52C8 File Offset: 0x000D34C8
		protected virtual void ConfigurePeekCameraCollision()
		{
			if (this.peekVirtualCamera == null)
			{
				return;
			}
			this.peekCameraCollider = this.peekVirtualCamera.GetComponent<CinemachineCollider>();
			if (this.peekCameraCollider == null)
			{
				return;
			}
			this.peekCameraCollider.m_AvoidObstacles = false;
			this.peekCameraCollider.m_CollideAgainst = 0;
			this.peekCameraCollider.enabled = false;
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x000D5330 File Offset: 0x000D3530
		protected virtual void CreateAnchorCamera()
		{
			if (this.anchorCamera != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("PeekAnchorCamera");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			this.anchorCamera = gameObject.AddComponent<CinemachineVirtualCamera>();
			this.anchorCamera.Priority = 5;
			this.anchorCamera.m_Lens.FieldOfView = ((this.tpInput.cameraMain != null) ? this.tpInput.cameraMain.fieldOfView : 60f);
			gameObject.SetActive(false);
			if (this.debug)
			{
				Debug.Log("[vBasicCoverController] Anchor camera created.");
			}
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x000D53CC File Offset: 0x000D35CC
		protected virtual void CreatePeekCameraTargets()
		{
			if (this.peekFollowTarget == null)
			{
				this.peekFollowTarget = new GameObject("PeekFollowTarget")
				{
					hideFlags = HideFlags.HideAndDontSave
				}.transform;
			}
			if (this.peekLookTarget == null)
			{
				this.peekLookTarget = new GameObject("PeekLookTarget")
				{
					hideFlags = HideFlags.HideAndDontSave
				}.transform;
			}
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x000D5434 File Offset: 0x000D3634
		protected virtual void LateUpdate()
		{
			if (this.inCover && PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return;
			}
			if (this.UsesFirstPersonCoverView)
			{
				if (this.hasPeekCameraLease || this.peekCameraTransitionRoutine != null)
				{
					this.DeactivatePeekCameraImmediate();
				}
			}
			else if (this.isPeeking && !this.hasPeekCameraLease && this.peekCameraTransitionRoutine == null && this.TryAcquirePeekActivityLease())
			{
				this.ActivatePeekCamera(this.currentPeekSide);
			}
			if (this.isPeeking)
			{
				this.UpdatePeekCameraTargets(this.currentPeekSide);
			}
			if (this.hasPeekCameraLease && !this.brainWasEnabledBeforePeek)
			{
				this.UpdateAnchorFromInvectorRig();
			}
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x000D54CC File Offset: 0x000D36CC
		public bool TryGetFirstPersonPeekGeometry(out Vector3 corner, out Vector3 outward, out Vector3 along)
		{
			corner = (outward = (along = Vector3.zero));
			if (!this.inCover || !this.isPeeking || this.currentCoverPoint == null)
			{
				return false;
			}
			outward = Vector3.ProjectOnPlane(this.currentCoverPoint.transform.forward, Vector3.up);
			along = Vector3.ProjectOnPlane(this.currentCoverPoint.transform.right, Vector3.up);
			if (outward.sqrMagnitude < 0.0001f || along.sqrMagnitude < 0.0001f)
			{
				return false;
			}
			outward.Normalize();
			along = ((this.currentPeekSide == vBasicCoverController.Side.left) ? along : (-along)).normalized;
			CoverWallSetup componentInParent = this.currentCoverPoint.GetComponentInParent<CoverWallSetup>();
			CoverWallSetup.WallBounds wallBounds;
			if (componentInParent != null && componentInParent.TryGetVisualBounds(out wallBounds))
			{
				float num = ((Vector3.Dot(along, componentInParent.transform.right) >= 0f) ? wallBounds.max.x : wallBounds.min.x);
				corner = componentInParent.transform.TransformPoint(new Vector3(num, wallBounds.min.y, wallBounds.max.z));
				return true;
			}
			Transform transform = ((this.currentPeekSide == vBasicCoverController.Side.left) ? this.currentCoverPoint.peekLeftRangeEnd : this.currentCoverPoint.peekRightRangeEnd);
			corner = ((transform != null) ? transform.position : this.currentCoverPoint.transform.position);
			if (transform == null && this.currentCoverPoint.boxCollider != null)
			{
				BoxCollider boxCollider = this.currentCoverPoint.boxCollider;
				Vector3 center = boxCollider.center;
				center.x += ((this.currentPeekSide == vBasicCoverController.Side.left) ? 1f : (-1f)) * boxCollider.size.x * 0.5f;
				corner = boxCollider.transform.TransformPoint(center);
			}
			return true;
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000D56EC File Offset: 0x000D38EC
		protected virtual void UpdatePeekCameraTargets(vBasicCoverController.Side peekSide)
		{
			if (this.currentCoverPoint == null)
			{
				return;
			}
			this.CreatePeekCameraTargets();
			Vector3 forward = this.currentCoverPoint.transform.forward;
			forward.y = 0f;
			if (forward.sqrMagnitude < 0.0001f)
			{
				return;
			}
			forward.Normalize();
			Vector3 vector = this.currentCoverPoint.transform.right;
			vector.y = 0f;
			vector = ((peekSide == vBasicCoverController.Side.left) ? vector : (-vector)).normalized;
			Transform transform = ((peekSide == vBasicCoverController.Side.left) ? this.currentCoverPoint.peekLeftRangeEnd : this.currentCoverPoint.peekRightRangeEnd);
			Vector3 vector2 = ((transform != null) ? transform.position : this.currentCoverPoint.transform.position);
			if (transform == null && this.currentCoverPoint.boxCollider != null)
			{
				vector2 += vector * this.currentCoverPoint.boxCollider.size.x * 0.5f;
			}
			this.peekFollowTarget.SetPositionAndRotation(base.transform.position, Quaternion.LookRotation(vector, Vector3.up));
			this.peekLookTarget.SetPositionAndRotation(new Vector3(vector2.x, base.transform.position.y + Mathf.Max(0f, this.peekLookHeight), vector2.z) + vector * Mathf.Max(0f, this.peekLookPastCorner) - forward * Mathf.Max(1f, this.peekLookBehindDistance), Quaternion.LookRotation(-forward, Vector3.up));
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x000D58A0 File Offset: 0x000D3AA0
		protected virtual void UpdateAnchorFromInvectorRig()
		{
			if (!this.hasSavedCameraState || this.anchorCamera == null || this.tpInput == null || this.tpInput.cameraMain == null)
			{
				return;
			}
			Transform parent = this.tpInput.cameraMain.transform.parent;
			if (parent != null)
			{
				this.anchorCamera.transform.SetPositionAndRotation(parent.TransformPoint(this.savedCameraLocalPosition), parent.rotation * this.savedCameraLocalRotation);
				return;
			}
			this.anchorCamera.transform.SetPositionAndRotation(this.savedCameraLocalPosition, this.savedCameraLocalRotation);
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x000D5950 File Offset: 0x000D3B50
		protected virtual void OnDestroy()
		{
			this.DeactivatePeekCameraImmediate();
			this.UnsubscribeCoverInputCallbacks();
			if (this.anchorCamera != null)
			{
				Object.Destroy(this.anchorCamera.gameObject);
			}
			if (this.peekFollowTarget != null)
			{
				Object.Destroy(this.peekFollowTarget.gameObject);
			}
			if (this.peekLookTarget != null)
			{
				Object.Destroy(this.peekLookTarget.gameObject);
			}
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000D59C4 File Offset: 0x000D3BC4
		protected virtual void UpdateCover()
		{
			if (this.inCover && !this.IsEntryTargetUsable(this.currentCoverPoint))
			{
				this.ExitCover(true);
				return;
			}
			this.EnforceCoverRigidbodyInterpolation();
			if (this.inCover && PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				if (this.tpInput != null && this.tpInput.cc != null)
				{
					this.tpInput.cc.input = Vector3.zero;
					this.tpInput.cc.inputSmooth = Vector3.zero;
					this.tpInput.cc.moveDirection = Vector3.zero;
				}
				this.timeToExitByDirection = 0f;
				return;
			}
			if (this.helper == null)
			{
				this.helper = new GameObject("CoverHelper").transform;
				this.helper.gameObject.hideFlags = HideFlags.HideInHierarchy;
			}
			this.DetectCoverPointsWithOverlap();
			this.HandlePossibleCoverPoint();
			this.HandleCoverIndicator();
			this.HandleCoverMovement();
			this.HandleCornerPeek();
			this.CheckAutoExitCover();
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x000D5ACC File Offset: 0x000D3CCC
		protected virtual void CheckAutoExitCover()
		{
			if (!this.inCover || !this.currentCoverPoint)
			{
				return;
			}
			if (this.DistanceToPoseLine(this.currentCoverPoint) <= this.maxEnterCoverDistance)
			{
				return;
			}
			vCoverPoint vCoverPoint = this.FindNearestConnectedCoverPoint();
			if (vCoverPoint != null && this.DistanceToPoseLine(vCoverPoint) <= this.maxEnterCoverDistance)
			{
				if (this.debug)
				{
					Debug.Log("[vBasicCoverController] Switching cover point from " + this.currentCoverPoint.name + " to " + vCoverPoint.name);
				}
				this.currentCoverPoint = vCoverPoint;
				this.currentCoverPoint.EnterCover(base.gameObject);
				return;
			}
			if (this.debug)
			{
				Debug.Log("[vBasicCoverController] Auto-exiting cover - too far from all cover points");
			}
			this.ExitCover(false);
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x000D5B84 File Offset: 0x000D3D84
		protected virtual vCoverPoint FindNearestConnectedCoverPoint()
		{
			if (!this.currentCoverPoint)
			{
				return null;
			}
			vCoverPoint vCoverPoint = null;
			float num = float.MaxValue;
			if (this.currentCoverPoint.left != null)
			{
				float num2 = this.DistanceToPoseLine(this.currentCoverPoint.left);
				if (num2 < num)
				{
					num = num2;
					vCoverPoint = this.currentCoverPoint.left;
				}
			}
			if (this.currentCoverPoint.right != null)
			{
				float num3 = this.DistanceToPoseLine(this.currentCoverPoint.right);
				if (num3 < num)
				{
					vCoverPoint = this.currentCoverPoint.right;
				}
			}
			return vCoverPoint;
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000D5C18 File Offset: 0x000D3E18
		protected virtual void SynchronizeCurrentCoverPoint()
		{
			if (this.currentCoverPoint == null)
			{
				return;
			}
			vCoverPoint vCoverPoint = this.FindPeekRangeOwnerAtPosition(this.currentCoverPoint);
			if (vCoverPoint != null)
			{
				if (vCoverPoint != this.currentCoverPoint)
				{
					this.currentCoverPoint = vCoverPoint;
					this.currentCoverPoint.EnterCover(base.gameObject);
				}
				return;
			}
			for (int i = 0; i < 8; i++)
			{
				vCoverPoint vCoverPoint2 = this.currentCoverPoint;
				float num = this.DistanceToPoseLine(vCoverPoint2);
				vCoverPoint left = this.currentCoverPoint.left;
				if (left != null && left.isActiveAndEnabled)
				{
					float num2 = this.DistanceToPoseLine(left);
					if (num2 + 0.0001f < num)
					{
						vCoverPoint2 = left;
						num = num2;
					}
				}
				vCoverPoint right = this.currentCoverPoint.right;
				if (right != null && right.isActiveAndEnabled && this.DistanceToPoseLine(right) + 0.0001f < num)
				{
					vCoverPoint2 = right;
				}
				if (vCoverPoint2 == this.currentCoverPoint)
				{
					break;
				}
				this.currentCoverPoint = vCoverPoint2;
				this.currentCoverPoint.EnterCover(base.gameObject);
			}
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x000D5D28 File Offset: 0x000D3F28
		protected virtual vCoverPoint FindPeekRangeOwnerAtPosition(vCoverPoint point)
		{
			if (point == null)
			{
				return null;
			}
			if (this.IsInsideConfiguredPeekRange(point))
			{
				return point;
			}
			vCoverPoint left = point.left;
			vCoverPoint right = point.right;
			bool flag = this.IsInsideConfiguredPeekRange(left);
			bool flag2 = this.IsInsideConfiguredPeekRange(right);
			if (!flag)
			{
				if (!flag2)
				{
					return null;
				}
				return right;
			}
			else
			{
				if (!flag2 || left == right)
				{
					return left;
				}
				float num = this.DistanceToPeekRangeEndSqr(left);
				float num2 = this.DistanceToPeekRangeEndSqr(right);
				if (Mathf.Abs(num - num2) > 0.0001f)
				{
					if (num >= num2)
					{
						return right;
					}
					return left;
				}
				else
				{
					if (left.transform.GetSiblingIndex() > right.transform.GetSiblingIndex())
					{
						return right;
					}
					return left;
				}
			}
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x000D5DC4 File Offset: 0x000D3FC4
		protected virtual bool IsInsideConfiguredPeekRange(vCoverPoint point)
		{
			return !(point == null) && point.isActiveAndEnabled && ((point.HasLeftPeekRange && point.IsInLeftPeekRange(base.transform.position)) || (point.HasRightPeekRange && point.IsInRightPeekRange(base.transform.position)));
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x000D5E1C File Offset: 0x000D401C
		protected virtual float DistanceToPeekRangeEndSqr(vCoverPoint point)
		{
			float num = float.PositiveInfinity;
			if (point.HasLeftPeekRange && point.IsInLeftPeekRange(base.transform.position))
			{
				num = (base.transform.position - point.peekLeftRangeEnd.position).sqrMagnitude;
			}
			if (point.HasRightPeekRange && point.IsInRightPeekRange(base.transform.position))
			{
				num = Mathf.Min(num, (base.transform.position - point.peekRightRangeEnd.position).sqrMagnitude);
			}
			return num;
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000D5EB4 File Offset: 0x000D40B4
		protected virtual void DetectCoverPointsWithOverlap()
		{
			this.coverPoints.RemoveAll((vCoverPoint cp) => !this.IsEntryTargetUsable(cp) || this.DistanceToPoseLine(cp) > this.maxEnterCoverDistance * 1.5f);
			foreach (Collider collider in Physics.OverlapSphere(base.transform.position + Vector3.up, this.maxEnterCoverDistance, -1, QueryTriggerInteraction.Collide))
			{
				vCoverPoint component = collider.GetComponent<vCoverPoint>();
				if (component != null)
				{
					bool flag = ((1 << collider.gameObject.layer) & this.coverLayer) != 0;
					bool flag2 = this.coverTag.Contains(collider.gameObject.tag);
					if (this.debug)
					{
						Debug.Log(string.Concat(new string[]
						{
							"[vBasicCoverController] Found vCoverPoint: ",
							component.name,
							", ",
							string.Format("Layer={0}({1}), layerMatch={2}, ", LayerMask.LayerToName(collider.gameObject.layer), collider.gameObject.layer, flag),
							string.Format("Tag={0}, tagMatch={1}", collider.gameObject.tag, flag2)
						}));
					}
					if (flag && flag2 && !this.coverPoints.Contains(component))
					{
						this.coverPoints.Add(component);
					}
				}
			}
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x000D6005 File Offset: 0x000D4205
		protected virtual void UpdateInput()
		{
			this.HandleEnterExitCover();
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002676 RID: 9846 RVA: 0x000D600D File Offset: 0x000D420D
		protected virtual bool isRolling
		{
			get
			{
				return this.tpInput.cc.isRolling;
			}
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x000D6020 File Offset: 0x000D4220
		protected virtual void HandleEnterExitCover()
		{
			if (!PlayerActivityGate.Allows(PlayerActivity.Cover) || (!this.inCover && !this.goingToCoverPoint && !this.CanPresentCover))
			{
				return;
			}
			bool flag = false;
			float num = 0f;
			this.enterExitInput.GetButtonTimer(ref num, ref flag, 0.2f);
			bool buttonDown = this.enterExitInput.GetButtonDown();
			bool flag2 = !this.tpInput.cc.customAction && !this.isRolling && !this.tpInput.cc.isJumping && !this.tpInput.cc.ragdolled && !this.tpInput.cc.isDead;
			bool clothingOwnsCoverExitInput = this.ClothingOwnsCoverExitInput;
			if (this.debug && buttonDown)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[vBasicCoverController] Button pressed! ",
					string.Format("possibleCoverPoint={0}, ", this.possibleCoverPoint != null),
					string.Format("inCover={0}, ", this.inCover),
					string.Format("wasInCover={0}, ", this.wasInCover),
					string.Format("goingToCoverPoint={0}, ", this.goingToCoverPoint),
					string.Format("canInteract={0}", flag2)
				}));
			}
			if (buttonDown && !this.inCover && !this.goingToCoverPoint && this.possibleCoverPoint && flag2 && this.IsOnPlayerSideOfCover(this.possibleCoverPoint) && this.DistanceToPoseLine(this.possibleCoverPoint) <= this.enterCoverDistance)
			{
				if (this.debug)
				{
					Debug.Log("[vBasicCoverController] ENTERING COVER!");
				}
				InteractionCandidateRegistry.MarkContextInputConsumed(this.enterExitInput);
				this.timeToExit = Time.time + this.exitCooldown;
				this.pendingCoverPoint = this.possibleCoverPoint;
				base.StartCoroutine(this.EnterCoverPointRoutine(this.pendingCoverPoint));
				this.possibleCoverPoint = null;
				return;
			}
			bool flag3 = this.timeToExit < Time.time;
			bool flag4;
			if (this.exitOnButtonPress)
			{
				flag4 = this.inCover && this.wasInCover && buttonDown && flag3 && !clothingOwnsCoverExitInput;
			}
			else
			{
				flag4 = this.inCover && this.wasInCover && flag && num > 0f && flag3 && !clothingOwnsCoverExitInput;
			}
			bool flag5 = this.inCover && this.wasInCover && !flag2;
			bool flag6 = this.tpInput.cc.IsAnimatorTag("ExitCover");
			bool flag7 = flag4 || flag5 || flag6;
			if (this.debug && flag7)
			{
				Debug.Log(string.Format("[vBasicCoverController] EXIT Cover - ByInput:{0} ByAction:{1} Force:{2}", flag4, flag5, flag6));
			}
			if (flag7 && this.inCover)
			{
				if (flag4)
				{
					InteractionCandidateRegistry.MarkContextInputConsumed(this.enterExitInput);
				}
				this.ExitCover(flag6);
			}
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000D6308 File Offset: 0x000D4508
		protected virtual void HandlePossibleCoverPoint()
		{
			if (this.goingToCoverPoint)
			{
				return;
			}
			vCoverPoint nearCoverPoint = this.GetNearCoverPoint();
			if (nearCoverPoint)
			{
				this.coverIndicatorPosition = nearCoverPoint.posePosition;
			}
			if (nearCoverPoint != this.possibleCoverPoint)
			{
				if (this.debug && nearCoverPoint)
				{
					Debug.Log("Found possible cover point", nearCoverPoint);
				}
				this.possibleCoverPoint = nearCoverPoint;
			}
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x000D636C File Offset: 0x000D456C
		protected virtual void HandleCoverIndicator()
		{
			if (this.coverUI)
			{
				bool flag = this.possibleCoverPoint != null && this.CanPresentCover;
				if (flag)
				{
					Vector3 vector = this.possibleCoverPoint.transform.InverseTransformPoint(this.coverIndicatorPosition);
					vector.z = 0f;
					vector.y = this.coverIndicaterHeight;
					Vector3 vector2 = this.possibleCoverPoint.transform.TransformPoint(vector);
					float num = Vector3.Distance(vector2, this.tpInput.cameraMain.transform.position);
					this.coverUI.transform.position = vector2;
					this.coverUI.transform.forward = this.possibleCoverPoint.transform.forward;
					this.coverUI.transform.localScale = Vector3.one * Mathf.Max(1f, num / this.coverIndicatorDistance);
				}
				if (this.coverUI.activeSelf != flag)
				{
					this.coverUI.SetActive(flag);
				}
			}
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000D647C File Offset: 0x000D467C
		protected virtual IEnumerator EnterCoverPointRoutine(vCoverPoint target)
		{
			this.pendingCoverPoint = target;
			if (!this.IsEntryTargetUsable(target) || !this.TryAcquireCoverActivity())
			{
				this.pendingCoverPoint = null;
				yield break;
			}
			this.wasLockCameraInputBeforeCover = this.tpInput.lockCameraInput;
			this.onStartGoToCoverPoint.Invoke();
			if (!this.IsEntryTargetUsable(target) || !base.isActiveAndEnabled || this.HasClothingFocus || this.coverActivityLease == null || !this.coverActivityLease.IsActive)
			{
				this.ReleaseCoverActivity();
				this.pendingCoverPoint = null;
				yield break;
			}
			this.inCover = false;
			Rigidbody body = this.tpInput.cc._rigidbody;
			this.AcquireCoverRigidbodyInterpolation(body);
			this.goingToCoverPoint = true;
			this.CheckCoverHeight(target);
			this.enterCoverFromLeftSide = this.EnterCoverFromLeftSide(target);
			Vector3 vector = (this.enterCoverFromLeftSide ? target.transform.right : (-target.transform.right));
			this.side = (this.enterCoverFromLeftSide ? vBasicCoverController.Side.left : vBasicCoverController.Side.right);
			this.wasCrouchingBeforeCover = this.tpInput.cc.isCrouching;
			this.wasUsingRootMotionBeforeCover = this.tpInput.cc.useRootMotion;
			this.wasIgnoringAnimatorMovementBeforeCover = this.tpInput.cc.ignoreAnimatorMovement;
			float t = 0f;
			Vector3 p = base.transform.position;
			Vector3 p2 = this.GetCoverEntryTargetPosition(target, p);
			Quaternion r = base.transform.rotation;
			Quaternion r2 = Quaternion.LookRotation(vector);
			this.tpInput.cc.lockRotation = true;
			this.tpInput.cc.lockMovement = true;
			this.tpInput.cc.inputMagnitude = 0f;
			this.tpInput.cc.input = Vector3.zero;
			this.tpInput.cc.inputSmooth = Vector3.zero;
			this.tpInput.cc.moveDirection = Vector3.zero;
			this.tpInput.cc.targetVelocity = Vector3.zero;
			this.tpInput.cc.useRootMotion = false;
			this.tpInput.cc.ignoreAnimatorMovement = true;
			if (body != null)
			{
				body.velocity = Vector3.zero;
				body.interpolation = RigidbodyInterpolation.None;
			}
			this.tpInput.cc.ResetInputAnimatorParameters();
			this.tpInput.cc.moveToDirectionInFree = true;
			target.EnterCover(base.gameObject);
			if (!this.CanContinueCoverEntry(target))
			{
				yield break;
			}
			yield return new WaitForFixedUpdate();
			while (t < 1f)
			{
				if (!this.CanContinueCoverEntry(target))
				{
					yield break;
				}
				t = Mathf.Clamp01(t + Time.fixedDeltaTime * Mathf.Max(0.01f, this.enterCoverAlignSpeed));
				float num = ((body != null) ? body.position.y : base.transform.position.y);
				p.y = num;
				p2.y = num;
				Vector3 vector2 = Vector3.Lerp(p, p2, t);
				Quaternion quaternion = Quaternion.Slerp(r, r2, t);
				if (body != null && !body.isKinematic)
				{
					body.MovePosition(vector2);
					body.MoveRotation(quaternion);
					body.velocity = Vector3.zero;
				}
				else
				{
					this.tpInput.cc.transform.SetPositionAndRotation(vector2, quaternion);
				}
				this.tpInput.cc.targetVelocity = Vector3.zero;
				if (t < 1f)
				{
					yield return new WaitForFixedUpdate();
				}
			}
			if (!this.CanContinueCoverEntry(target))
			{
				yield break;
			}
			this.tpInput.cc.lockMovement = false;
			this.currentCoverPoint = target;
			this.pendingCoverPoint = null;
			this.goingToCoverPoint = false;
			this.inCover = true;
			this.tpInput.cc.useRootMotion = false;
			this.tpInput.cc.targetVelocity = Vector3.zero;
			this.EnterCoverLocomotionState();
			this.onFinishGoToCoverPoint.Invoke();
			this.onEnterCover.Invoke();
			yield return new WaitForEndOfFrame();
			this.wasInCover = true;
			yield break;
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000D6494 File Offset: 0x000D4694
		protected virtual Vector3 GetCoverEntryTargetPosition(vCoverPoint target, Vector3 worldPosition)
		{
			if (target == null)
			{
				return worldPosition;
			}
			Vector3 vector = target.transform.InverseTransformPoint(worldPosition);
			float num;
			float num2;
			if (this.TryGetWallCoverSlideLimits(target, target.transform, out num, out num2))
			{
				vector.x = Mathf.Clamp(vector.x, num, num2);
			}
			vector.z = 0f;
			Vector3 vector2 = target.transform.TransformPoint(vector);
			Vector3 forward = target.transform.forward;
			forward.y = 0f;
			if (forward.sqrMagnitude > 0.0001f)
			{
				vector2 += forward.normalized * this.PositionOffsetZ;
			}
			return vector2;
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x000D653A File Offset: 0x000D473A
		protected virtual bool IsEntryTargetUsable(vCoverPoint target)
		{
			return target != null && target.isActiveAndEnabled && target.gameObject.activeInHierarchy;
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x000D655A File Offset: 0x000D475A
		protected virtual bool CanContinueCoverEntry(vCoverPoint target)
		{
			if (this.IsEntryTargetUsable(target) && base.isActiveAndEnabled && !this.HasClothingFocus && this.coverActivityLease != null && this.coverActivityLease.IsActive)
			{
				return true;
			}
			this.ExitCover(true);
			return false;
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x0600267E RID: 9854 RVA: 0x000D6594 File Offset: 0x000D4794
		public bool CanStartCover
		{
			get
			{
				return !this.inCover && !this.goingToCoverPoint && this.CanStartCoverActivity();
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x0600267F RID: 9855 RVA: 0x000D65AE File Offset: 0x000D47AE
		public bool CanPresentCover
		{
			get
			{
				return this.CanStartCover;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002680 RID: 9856 RVA: 0x000D65B6 File Offset: 0x000D47B6
		private bool HasClothingFocus
		{
			get
			{
				return PlayerInventoryManager.Instance != null && PlayerInventoryManager.Instance.ActiveClothIndex >= 0;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002681 RID: 9857 RVA: 0x000D65D7 File Offset: 0x000D47D7
		public bool ClothingOwnsCoverExitInput
		{
			get
			{
				return this.inCover && this.HasClothingFocus;
			}
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000D65E9 File Offset: 0x000D47E9
		protected virtual bool CanStartCoverActivity()
		{
			return !this.HasClothingFocus && this.CanApproachCover();
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000D65FB File Offset: 0x000D47FB
		private bool CanApproachCover()
		{
			return !ClothActionGate.IsBusy && PlayerActivityGate.CanStartWorldInteraction && PlayerActivityGate.Allows(PlayerActivity.Cover) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation);
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002684 RID: 9860 RVA: 0x000D661F File Offset: 0x000D481F
		public bool CanOpenModalMap
		{
			get
			{
				return this.inCover && !this.goingToCoverPoint && this.coverActivityLease != null && this.coverActivityLease.IsActive && PlayerActivityGate.CanAcquireAlongside(PlayerActivity.AllPlayerActions, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.Rider | PlayerContextResource.HeldItem | PlayerContextResource.WorldSimulation | PlayerContextResource.WaterMovement, this, "WallCover.InCover");
			}
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000D665D File Offset: 0x000D485D
		public bool CanOpenModalInventory(PlayerContextResource resources)
		{
			return this.inCover && !this.goingToCoverPoint && this.coverActivityLease != null && this.coverActivityLease.IsActive && PlayerActivityGate.CanAcquireAlongside(PlayerActivity.Inventory, resources, this, "WallCover.InCover");
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x000D6698 File Offset: 0x000D4898
		public void PrepareForModalUI()
		{
			this.CanPeekNow = false;
			this.isPeeking = false;
			this.peekInputRequiresRelease = true;
			if (this.tpInput != null && this.tpInput.cc != null && this.tpInput.cc.animator != null)
			{
				this.tpInput.cc.animator.SetBool(this.peekingParameter, false);
			}
			this.DeactivatePeekCameraImmediate();
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x000D6718 File Offset: 0x000D4918
		protected virtual bool TryAcquireCoverActivity()
		{
			return !this.HasClothingFocus && ((this.coverActivityLease != null && this.coverActivityLease.IsActive) || (this.CanStartCoverActivity() && PlayerActivityGate.TryAcquire("WallCover.Active", this, PlayerActivity.Cover | PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation, PlayerActivity.Emote | PlayerActivity.Flash | PlayerActivity.Interact | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.Crouch, out this.coverActivityLease, false, null)));
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x000D676C File Offset: 0x000D496C
		protected virtual void ReleaseCoverActivity()
		{
			PlayerActivityLease playerActivityLease = this.coverActivityLease;
			if (playerActivityLease != null)
			{
				playerActivityLease.Dispose();
			}
			this.coverActivityLease = null;
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x000D6788 File Offset: 0x000D4988
		protected virtual void AcquireCoverRigidbodyInterpolation(Rigidbody body)
		{
			if (this.hasCoverInterpolationLease && this.coverInterpolationBody == body)
			{
				if (body != null)
				{
					body.interpolation = RigidbodyInterpolation.None;
				}
				return;
			}
			this.ReleaseCoverRigidbodyInterpolation();
			if (body == null)
			{
				return;
			}
			this.coverInterpolationBody = body;
			this.interpolationBeforeCover = body.interpolation;
			this.hasCoverInterpolationLease = true;
			body.interpolation = RigidbodyInterpolation.None;
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x000D67F0 File Offset: 0x000D49F0
		protected virtual void EnforceCoverRigidbodyInterpolation()
		{
			if (!this.inCover && !this.goingToCoverPoint)
			{
				return;
			}
			Rigidbody rigidbody = ((this.tpInput != null && this.tpInput.cc != null) ? this.tpInput.cc._rigidbody : null);
			this.EnforceCoverRigidbodyInterpolation(rigidbody);
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x000D684A File Offset: 0x000D4A4A
		protected virtual void EnforceCoverRigidbodyInterpolation(Rigidbody body)
		{
			if (!this.hasCoverInterpolationLease || this.coverInterpolationBody != body)
			{
				this.AcquireCoverRigidbodyInterpolation(body);
				return;
			}
			if (body != null && body.interpolation != RigidbodyInterpolation.None)
			{
				body.interpolation = RigidbodyInterpolation.None;
			}
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x000D6884 File Offset: 0x000D4A84
		protected virtual void ReleaseCoverRigidbodyInterpolation()
		{
			if (!this.hasCoverInterpolationLease)
			{
				return;
			}
			Rigidbody rigidbody = this.coverInterpolationBody;
			RigidbodyInterpolation rigidbodyInterpolation = this.interpolationBeforeCover;
			this.hasCoverInterpolationLease = false;
			this.coverInterpolationBody = null;
			this.interpolationBeforeCover = RigidbodyInterpolation.None;
			if (rigidbody != null)
			{
				rigidbody.interpolation = rigidbodyInterpolation;
			}
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000D68D0 File Offset: 0x000D4AD0
		protected virtual void EnterCoverLocomotionState()
		{
			if (this.debug)
			{
				Debug.Log("Enter Cover Locomotion State");
			}
			this.tpInput.cc.freeSpeed.walkByDefault = this.originalWalkByDefault;
			this.tpInput.lockCameraInput = false;
			this.tpInput.cc.lockRotation = true;
			this.tpInput.cc.isStrafing = false;
			this.tpInput.cc.lockInStrafe = false;
			this.tpInput.cc.isSprinting = false;
			this.tpInput.lockMoveInput = false;
			this.tpInput.lockUpdateMoveDirection = true;
			this.PlayCoverLocomotionState();
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000D6978 File Offset: 0x000D4B78
		protected virtual void PlayCoverLocomotionState()
		{
			this.ControlCoverHelperTransform();
			bool flag = this.CheckCoverHeight(this.currentCoverPoint);
			float num = (this.enterCoverFromLeftSide ? (-1f) : 1f);
			if (!string.IsNullOrEmpty(this.coverSideParameter))
			{
				this.tpInput.cc.animator.SetFloat(this.coverSideParameter, num);
			}
			string text = (flag ? this.coverCrouched : this.coverStanding);
			this.CrossFadeOnAllLayers(text, this.enterCoverAnimCrossfade);
			this.tpInput.cc.isCrouching = flag;
			if (this.debug)
			{
				Debug.Log(string.Format("[vBasicCoverController] PlayCoverLocomotionState: state={0}, side={1}, isWallLow={2}", text, num, flag));
			}
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000D6A2C File Offset: 0x000D4C2C
		protected virtual void CrossFadeOnAllLayers(string stateName, float crossfadeDuration)
		{
			this.tpInput.cc.animator.CrossFadeInFixedTime(stateName, crossfadeDuration, 0);
			int num = Animator.StringToHash(stateName);
			foreach (int num2 in this.additionalLayerIndices)
			{
				if (this.tpInput.cc.animator.GetLayerWeight(num2) > 0f)
				{
					if (this.tpInput.cc.animator.HasState(num2, num))
					{
						this.tpInput.cc.animator.CrossFadeInFixedTime(stateName, crossfadeDuration, num2);
						if (this.debug)
						{
							Debug.Log(string.Format("[vBasicCoverController] CrossFade on layer {0}: {1}", num2, stateName));
						}
					}
					else if (this.debug)
					{
						Debug.Log(string.Format("[vBasicCoverController] State '{0}' not found on layer {1}, skipping", stateName, num2));
					}
				}
			}
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x000D6B2C File Offset: 0x000D4D2C
		public bool IsCoverAnimationState(AnimatorStateInfo state)
		{
			return state.IsName(this.coverStanding) || state.IsName(this.coverCrouched) || state.IsName(this.goToCoverPoint) || state.IsName("CoverPeakStanding") || state.IsName("CoverPeakCrouched") || state.IsName(this.cornerPeekLeftStanding) || state.IsName(this.cornerPeekRightStanding) || state.IsName(this.cornerPeekLeftCrouched) || state.IsName(this.cornerPeekRightCrouched);
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000D6BC0 File Offset: 0x000D4DC0
		private void ExitCoverAnimationStates(string exitState)
		{
			Animator animator = this.tpInput.cc.animator;
			int num = Animator.StringToHash(exitState);
			int num2 = Animator.StringToHash(this.defaultFreeStanding);
			int num3 = (animator.HasState(0, num) ? num : num2);
			if (this.useCrossFadeOnExit && animator.HasState(0, num3))
			{
				animator.CrossFadeInFixedTime(num3, this.exitCoverAnimCrossfade, 0);
			}
			foreach (int num4 in this.additionalLayerIndices)
			{
				int num5 = (animator.HasState(num4, num) ? num : num2);
				if (animator.HasState(num4, num5))
				{
					if (animator.GetLayerWeight(num4) > 0f)
					{
						if (this.useCrossFadeOnExit)
						{
							animator.CrossFadeInFixedTime(num5, this.exitCoverAnimCrossfade, num4);
						}
					}
					else
					{
						AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(num4);
						bool flag = this.IsCoverAnimationState(currentAnimatorStateInfo);
						if (animator.IsInTransition(num4))
						{
							AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(num4);
							flag = this.IsCoverAnimationState(nextAnimatorStateInfo) || (flag && (nextAnimatorStateInfo.IsName(this.defaultFreeStanding) || nextAnimatorStateInfo.IsName(this.defaultFreeCrouched)));
						}
						if (flag)
						{
							animator.Play(num5, num4, 0f);
						}
					}
				}
			}
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x000D6D24 File Offset: 0x000D4F24
		public bool InterruptForEncounter()
		{
			if (!this.inCover && !this.goingToCoverPoint)
			{
				return false;
			}
			this.ExitCover(true);
			return true;
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x000D6D40 File Offset: 0x000D4F40
		protected virtual void ExitCover(bool forceExit = false)
		{
			if (this.debug)
			{
				Debug.Log("Exit Cover");
			}
			this.EnforceCoverRigidbodyInterpolation();
			if (this.inCover || this.goingToCoverPoint)
			{
				this.onExitCover.Invoke();
			}
			base.StopAllCoroutines();
			this.ResetPeekState(true);
			this.tpInput.tpCamera.switchRight = 1f;
			this.tpInput.ResetCameraState();
			this.tpInput.lockUpdateMoveDirection = false;
			this.tpInput.lockCameraInput = this.wasLockCameraInputBeforeCover;
			this.tpInput.lockMoveInput = false;
			this.tpInput.cc.ignoreAnimatorMovement = this.wasIgnoringAnimatorMovementBeforeCover;
			this.tpInput.cc.lockRotation = false;
			this.tpInput.cc.lockMovement = false;
			this.tpInput.cc.isSprinting = false;
			this.tpInput.cc.lockSetMoveSpeed = false;
			this.tpInput.cc.ResetCapsule();
			this.tpInput.cc.lockInStrafe = false;
			this.tpInput.cc.moveToDirectionInFree = this.lastMoveToDirectionInFree;
			this.tpInput.cc.useRootMotion = this.wasUsingRootMotionBeforeCover;
			this.tpInput.cc.targetVelocity = Vector3.zero;
			if (this.alwaysStandUpOnExit)
			{
				this.tpInput.cc.isCrouching = false;
			}
			this.currentCoverPoint = null;
			this.possibleCoverPoint = null;
			this.pendingCoverPoint = null;
			this.goingToCoverPoint = false;
			this.isRightCorner = false;
			this.isLeftCorner = false;
			this.canMove = false;
			if (!this.alwaysStandUpOnExit && this.wasCrouchingBeforeCover)
			{
				this.tpInput.cc.isCrouching = true;
			}
			string text = (this.tpInput.cc.isCrouching ? this.defaultFreeCrouched : this.defaultFreeStanding);
			this.ExitCoverAnimationStates(text);
			this.wasInCover = false;
			this.inCover = false;
			this.ReleaseCoverRigidbodyInterpolation();
			this.wasCrouchingBeforeCover = false;
			this.tpInput.cc.animator.speed = 1f;
			this.ReleaseCoverActivity();
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x000D6F64 File Offset: 0x000D5164
		protected virtual void HandleCoverMovement()
		{
			if (!this.inCover)
			{
				return;
			}
			if (!this.currentCoverPoint)
			{
				return;
			}
			this.SynchronizeCurrentCoverPoint();
			this.ControlCoverHelperTransform();
			this.ControlCrouchStateInCover();
			this.ControlInputDirectionInCover();
			this.ControlSideInCover();
			this.ControlRotationInCover();
			this.ControlMovementInCover();
			this.ControlCameraState();
			this.ControlExitCoverByAngle();
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x000D6FC0 File Offset: 0x000D51C0
		protected virtual void ControlCrouchStateInCover()
		{
			bool flag = this.CheckCoverHeight(this.currentCoverPoint);
			if (!flag && this.tpInput.cc.isCrouching && !this.tpInput.cc.CanExitCrouch())
			{
				flag = true;
			}
			this.tpInput.cc.isCrouching = flag;
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000D7014 File Offset: 0x000D5214
		protected virtual void ControlExitCoverByAngle()
		{
			if (!this.autoExitCover || this.tpInput.cc.input.magnitude < 0.1f)
			{
				this.timeToExitByDirection = 0f;
				return;
			}
			if (Vector3.Angle(this.inputDirection, -this.helper.transform.forward) > this.exitInputDirectionAngle)
			{
				this.timeToExitByDirection += Time.fixedDeltaTime;
				if (this.timeToExitByDirection > this.exitDirectionTimer)
				{
					this.ExitCover(false);
					return;
				}
			}
			else
			{
				this.timeToExitByDirection = 0f;
			}
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x000D70B0 File Offset: 0x000D52B0
		protected virtual void ControlCoverHelperTransform()
		{
			if (!this.currentCoverPoint)
			{
				return;
			}
			Vector3 forward = this.currentCoverPoint.transform.forward;
			Vector3 vector = this.currentCoverPoint.posePosition;
			forward.y = 0f;
			forward.Normalize();
			vector -= forward * this.currentCoverPoint.posePositionZ;
			if (forward.magnitude > 0f)
			{
				this.helper.transform.position = vector;
				this.helper.forward = forward;
				this.coverNormal = forward;
			}
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x000D7148 File Offset: 0x000D5348
		protected virtual void ControlInputDirectionInCover()
		{
			this.cameraRightDir = this.tpInput.cameraMain.transform.right;
			this.cameraRightDir.y = 0f;
			this.cameraRightDir.Normalize();
			this.cameraForwardDir = Quaternion.AngleAxis(-90f, Vector3.up) * this.cameraRightDir;
			this.inputDirection = this.tpInput.cc.inputSmooth.x * this.cameraRightDir + this.tpInput.cc.inputSmooth.z * this.cameraForwardDir;
			this.coverMoveDirection = this.helper.InverseTransformDirection(this.inputDirection);
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x000D7210 File Offset: 0x000D5410
		protected virtual void ControlSideInCover()
		{
			Vector3 vector = this.coverMoveDirection;
			if (Mathf.Abs(vector.y) > 0f)
			{
				vector.y = 0f;
			}
			float num = (((double)this.inputDirection.magnitude > 0.1 && (double)this.coverNormal.magnitude > 0.1) ? Mathf.Abs(this.inputDirection.AngleFormOtherDirection(this.coverNormal).y) : 0f);
			if (vector.magnitude > 0.1f && num.IsInSideRange(20f, 160f))
			{
				if ((double)vector.x > 0.1)
				{
					this.side = vBasicCoverController.Side.left;
				}
				else if ((double)vector.x < -0.1)
				{
					this.side = vBasicCoverController.Side.right;
				}
			}
			this.enterCoverFromLeftSide = this.side == vBasicCoverController.Side.left;
			float num2 = ((this.side == vBasicCoverController.Side.left) ? (-1f) : 1f);
			if (!string.IsNullOrEmpty(this.coverSideParameter))
			{
				this.tpInput.cc.animator.SetFloat(this.coverSideParameter, num2, 0.2f, Time.fixedDeltaTime);
			}
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x000D7340 File Offset: 0x000D5540
		protected virtual void ControlRotationInCover()
		{
			if (!this.inCover)
			{
				return;
			}
			this.tpInput.cc.lockRotation = true;
			Vector3 vector = Quaternion.AngleAxis((float)((vBasicCoverController.Side)(-90) * this.side), Vector3.up) * this.coverNormal.normalized;
			Vector3 vector2 = Quaternion.AngleAxis((float)((vBasicCoverController.Side)(-70) * this.side), Vector3.up) * this.coverNormal.normalized;
			float num = ((Vector3.Angle(base.transform.forward, vector2) > 25f) ? Quaternion.LookRotation(vector2).eulerAngles.y : Quaternion.LookRotation(vector).eulerAngles.y);
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			eulerAngles.y = Mathf.LerpAngle(eulerAngles.y, num, this.rotationSpeed * Time.fixedDeltaTime);
			Quaternion quaternion = Quaternion.Euler(eulerAngles);
			Rigidbody rigidbody = this.tpInput.cc._rigidbody;
			if (rigidbody != null && !rigidbody.isKinematic)
			{
				rigidbody.MoveRotation(quaternion);
				return;
			}
			base.transform.rotation = quaternion;
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x000D746C File Offset: 0x000D566C
		protected virtual void ControlMovementInCover()
		{
			Vector3 vector = this.coverMoveDirection;
			if (Mathf.Abs(vector.y) >= 0f)
			{
				vector.y = 0f;
			}
			if (Mathf.Abs(vector.x) <= 0.1f)
			{
				vector.x = 0f;
			}
			this.tpInput.cc.ignoreAnimatorMovement = true;
			this.tpInput.cc.useRootMotion = false;
			Rigidbody rigidbody = this.tpInput.cc._rigidbody;
			Vector3 vector2 = ((rigidbody != null) ? rigidbody.position : base.transform.position);
			this.localCharPosition = this.helper.InverseTransformPoint(vector2);
			vector.z = 0f;
			this.canMove = vector.magnitude >= 0.1f && ((vector.x > 0f && this.side == vBasicCoverController.Side.left) || (vector.x < 0f && this.side == vBasicCoverController.Side.right));
			this.tpInput.cc.moveDirection = this.helper.TransformDirection(vector) * (this.canMove ? 1f : 0f);
			if (!this.canMove || vector.magnitude == 0f)
			{
				this.tpInput.cc.inputMagnitude = 0f;
			}
			float num = 0f;
			if (this.canMove)
			{
				float num2 = (this.tpInput.cc.isCrouching ? this.crouchedMoveSpeed : this.standingMoveSpeed);
				CoverWallSetup componentInParent = this.currentCoverPoint.GetComponentInParent<CoverWallSetup>();
				if (componentInParent != null && componentInParent.maxSlideSpeed > 0f)
				{
					num2 = componentInParent.maxSlideSpeed;
				}
				num = vector.x * num2;
			}
			float num3 = Mathf.Max(0.0001f, Time.fixedDeltaTime);
			float num4;
			float num5;
			if (this.TryGetWallCoverSlideLimits(out num4, out num5))
			{
				num = (Mathf.Clamp(this.localCharPosition.x + num * num3, num4, num5) - this.localCharPosition.x) / num3;
				if (this.localCharPosition.x < num4 || this.localCharPosition.x > num5)
				{
					float num6 = Mathf.Max(0.1f, this.coverEndpointRecoverySpeed);
					num = Mathf.Clamp(num, -num6, num6);
				}
			}
			if (Mathf.Abs(num) < 0.001f)
			{
				num = 0f;
				if (this.canMove)
				{
					this.tpInput.cc.inputMagnitude = 0f;
				}
			}
			float num7 = this.PositionOffsetZ - this.localCharPosition.z;
			float num8 = Mathf.Clamp(num7 / num3, -Mathf.Max(0f, this.coverLineCorrectionSpeed), Mathf.Max(0f, this.coverLineCorrectionSpeed));
			if (Mathf.Abs(num7) < 0.001f)
			{
				num8 = 0f;
			}
			this.tpInput.cc.targetVelocity = this.helper.right * num + this.helper.forward * num8;
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000D7776 File Offset: 0x000D5976
		protected virtual bool TryGetWallCoverSlideLimits(out float minX, out float maxX)
		{
			return this.TryGetWallCoverSlideLimits(this.currentCoverPoint, this.helper, out minX, out maxX);
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000D778C File Offset: 0x000D598C
		protected virtual bool TryGetWallCoverSlideLimits(vCoverPoint referencePoint, Transform referenceFrame, out float minX, out float maxX)
		{
			minX = float.PositiveInfinity;
			maxX = float.NegativeInfinity;
			CoverWallSetup coverWallSetup = ((referencePoint != null) ? referencePoint.GetComponentInParent<CoverWallSetup>() : null);
			Transform transform = ((coverWallSetup != null && coverWallSetup.coverLine != null) ? coverWallSetup.coverLine.CPTransform : null);
			if (transform == null || referenceFrame == null)
			{
				return false;
			}
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if (child.gameObject.activeInHierarchy)
				{
					vCoverPoint component = child.GetComponent<vCoverPoint>();
					if (!(component == null) && !(component.boxCollider == null) && component.boxCollider.enabled)
					{
						BoxCollider boxCollider = component.boxCollider;
						Vector3 vector = child.TransformPoint(new Vector3(boxCollider.center.x - boxCollider.size.x * 0.5f, 0f, 0f));
						Vector3 vector2 = child.TransformPoint(new Vector3(boxCollider.center.x + boxCollider.size.x * 0.5f, 0f, 0f));
						float x = referenceFrame.InverseTransformPoint(vector).x;
						float x2 = referenceFrame.InverseTransformPoint(vector2).x;
						minX = Mathf.Min(new float[] { minX, x, x2 });
						maxX = Mathf.Max(new float[] { maxX, x, x2 });
					}
				}
			}
			if (float.IsInfinity(minX) || float.IsInfinity(maxX))
			{
				return false;
			}
			float magnitude = referenceFrame.TransformVector(Vector3.right).magnitude;
			float num = Mathf.Max(0f, this.coverEdgeInset) / Mathf.Max(0.0001f, magnitude);
			if (maxX - minX <= num * 2f)
			{
				float num2 = (minX + maxX) * 0.5f;
				minX = (maxX = num2);
			}
			else
			{
				minX += num;
				maxX -= num;
			}
			return true;
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000D79B4 File Offset: 0x000D5BB4
		protected virtual void ControlCameraState()
		{
			string text = (this.tpInput.cc.isCrouching ? this.crouchCameraState : this.standCameraState);
			if (this.tpInput.customCameraState != text)
			{
				this.tpInput.ChangeCameraState(text, true);
			}
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000D7A04 File Offset: 0x000D5C04
		protected virtual void HandleCornerPeek()
		{
			if (!this.inCover || this.goingToCoverPoint || !this.currentCoverPoint)
			{
				this.CanPeekNow = false;
				this.peekInputRequiresRelease = this.peekInput.GetButton();
				this.ResetPeekState(false);
				return;
			}
			bool button = this.peekInput.GetButton();
			if (this.tpInput.cc.isCrouching)
			{
				this.CanPeekNow = false;
				if (button)
				{
					this.peekInputRequiresRelease = true;
				}
				this.ResetPeekState(true);
				return;
			}
			if (this.peekInputRequiresRelease)
			{
				if (button)
				{
					this.CanPeekNow = false;
					this.ResetPeekState(false);
					return;
				}
				this.peekInputRequiresRelease = false;
			}
			this.CalculateCameraAngle();
			this.UpdateCornerRangeDetection();
			bool flag = this.isInLeftCornerRange && this.side == vBasicCoverController.Side.left;
			bool flag2 = this.isInRightCornerRange && this.side == vBasicCoverController.Side.right;
			bool flag3 = (flag || flag2) && this.IsPeekCameraResourceAvailable();
			this.CanPeekNow = flag3;
			if (this.debug)
			{
				Debug.Log(string.Format("[Peek] inCover={0}, corner={1}, side={2}, ", this.inCover, this.currentCoverPoint.corner, this.side) + string.Format("isLeftCorner={0}, isRightCorner={1}, ", this.IsLeftCoverCorner(this.currentCoverPoint), this.IsRightCoverCorner(this.currentCoverPoint)) + string.Format("inLeftRange={0}, inRightRange={1}, ", this.isInLeftCornerRange, this.isInRightCornerRange) + string.Format("canPeek={0}, peekInput={1}", flag3, this.peekInput.GetButton()));
			}
			if (this.peekToggleMode)
			{
				if (flag3 && this.peekInput.GetButtonDown() && !this.isPeeking)
				{
					this.StartPeek(flag ? vBasicCoverController.Side.left : vBasicCoverController.Side.right);
				}
				else if (this.isPeeking && this.peekInput.GetButtonDown())
				{
					this.EndPeek();
				}
			}
			else if (flag3 && this.peekInput.GetButton())
			{
				this.currentTimeToPeek += Time.fixedDeltaTime;
				if (this.currentTimeToPeek >= this.timeToEnterPeek && !this.isPeeking)
				{
					this.StartPeek(flag ? vBasicCoverController.Side.left : vBasicCoverController.Side.right);
				}
			}
			else if (this.isPeeking && !this.peekInput.GetButton())
			{
				this.EndPeek();
			}
			else if (!flag3 && !this.isPeeking)
			{
				this.currentTimeToPeek = 0f;
			}
			if (this.isPeeking && !flag3)
			{
				this.EndPeek();
			}
			float num = (this.isPeeking ? 1f : 0f);
			this.currentPeekWeight = Mathf.Lerp(this.currentPeekWeight, num, this.peekWeightSmooth * Time.fixedDeltaTime);
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x000D7CBC File Offset: 0x000D5EBC
		protected virtual void CalculateCameraAngle()
		{
			if (this.tpInput.cameraMain == null || !this.currentCoverPoint)
			{
				return;
			}
			Vector3 forward = this.tpInput.cameraMain.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			Vector3 vector = -this.currentCoverPoint.transform.forward;
			vector.y = 0f;
			vector.Normalize();
			this.cameraAngle = Vector3.SignedAngle(vector, forward, Vector3.up);
			this.inCornerAngle = (this.side == vBasicCoverController.Side.left && this.cameraAngle < this.angleToEnterCorner) || (this.side == vBasicCoverController.Side.right && this.cameraAngle > -this.angleToEnterCorner);
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x000D7D8C File Offset: 0x000D5F8C
		protected virtual void UpdateCornerRangeDetection()
		{
			if (!this.currentCoverPoint)
			{
				return;
			}
			if (this.currentCoverPoint.HasLeftPeekRange)
			{
				this.isInLeftCornerRange = this.currentCoverPoint.IsInLeftPeekRange(base.transform.position);
			}
			else if (this.IsLeftCoverCorner(this.currentCoverPoint))
			{
				Vector3 vector = this.currentCoverPoint.transform.InverseTransformPoint(base.transform.position);
				float colliderRadiusDefault = this.tpInput.cc.colliderRadiusDefault;
				float num = this.currentCoverPoint.boxCollider.size.x * 0.5f;
				this.isInLeftCornerRange = vector.x >= num - colliderRadiusDefault - 0.3f;
			}
			else
			{
				this.isInLeftCornerRange = false;
			}
			if (this.currentCoverPoint.HasRightPeekRange)
			{
				this.isInRightCornerRange = this.currentCoverPoint.IsInRightPeekRange(base.transform.position);
			}
			else if (this.IsRightCoverCorner(this.currentCoverPoint))
			{
				Vector3 vector2 = this.currentCoverPoint.transform.InverseTransformPoint(base.transform.position);
				float colliderRadiusDefault2 = this.tpInput.cc.colliderRadiusDefault;
				float num2 = this.currentCoverPoint.boxCollider.size.x * 0.5f;
				this.isInRightCornerRange = vector2.x <= -num2 + colliderRadiusDefault2 + 0.3f;
			}
			else
			{
				this.isInRightCornerRange = false;
			}
			if (this.debug && (this.isInLeftCornerRange || this.isInRightCornerRange))
			{
				Vector3 vector3 = this.currentCoverPoint.transform.InverseTransformPoint(base.transform.position);
				Debug.Log(string.Format("[Peek] LocalPos.x={0:F2}, inLeftRange={1}, inRightRange={2}, ", vector3.x, this.isInLeftCornerRange, this.isInRightCornerRange) + string.Format("hasLeftPeekRange={0}, hasRightPeekRange={1}", this.currentCoverPoint.HasLeftPeekRange, this.currentCoverPoint.HasRightPeekRange));
			}
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x000D7F90 File Offset: 0x000D6190
		protected virtual bool IsLookingTowardsCorner()
		{
			if (!this.currentCoverPoint || this.tpInput.cc.input.magnitude < 0.1f)
			{
				return this.inCornerAngle;
			}
			float x = this.coverMoveDirection.x;
			return (Vector3.Angle(-this.currentCoverPoint.transform.right * (float)this.side, base.transform.forward) < this.minAngleToStartMoveToCorner && ((this.isInLeftCornerRange && x < -0.2f) || (this.isInRightCornerRange && x > 0.2f)) && this.cameraAngle * (float)this.side < this.angleToEnterCorner) || this.inCornerAngle;
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x000D8058 File Offset: 0x000D6258
		protected virtual void StartPeek(vBasicCoverController.Side peekSide)
		{
			if (this.isPeeking || !this.CanPeekNow || this.tpInput == null || this.tpInput.cc == null || this.tpInput.cc.isCrouching || this.peekInputRequiresRelease || (!this.UsesFirstPersonCoverView && this.peekVirtualCamera == null) || this.tpInput.cameraMain == null || !this.TryAcquirePeekActivityLease())
			{
				return;
			}
			this.isPeeking = true;
			this.currentTimeToPeek = 0f;
			this.currentPeekSide = peekSide;
			this.tpInput.cc.animator.SetBool(this.peekingParameter, true);
			this.tpInput.cc.animator.SetFloat(this.peekSideParameter, (float)peekSide);
			this.ActivatePeekCamera(peekSide);
			if (this.debug)
			{
				Debug.Log(string.Format("[vBasicCoverController] StartPeek: side={0}, PeekSide={1}", peekSide, (int)peekSide));
			}
			this.onStartPeek.Invoke();
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x000D816C File Offset: 0x000D636C
		protected virtual bool IsPeekCameraResourceAvailable()
		{
			if (this.UsesFirstPersonCoverView)
			{
				return PlayerActivityGate.Allows(PlayerActivity.Cover);
			}
			return (this.peekActivityLease != null && this.peekActivityLease.IsActive) || (PlayerActivityGate.Allows(PlayerActivity.Cover) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.Camera) && this.peekVirtualCamera != null && this.tpInput != null && this.tpInput.cameraMain != null);
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x000D81E0 File Offset: 0x000D63E0
		protected virtual bool TryAcquirePeekActivityLease()
		{
			if (this.UsesFirstPersonCoverView)
			{
				return PlayerActivityGate.Allows(PlayerActivity.Cover);
			}
			if (this.peekActivityLease != null && this.peekActivityLease.IsActive)
			{
				return true;
			}
			PlayerActivityLease playerActivityLease = this.peekActivityLease;
			if (playerActivityLease != null)
			{
				playerActivityLease.Dispose();
			}
			this.peekActivityLease = null;
			return PlayerActivityGate.TryAcquire("Wall cover peek camera", this, PlayerActivity.Cover, PlayerContextResource.Camera, PlayerActivity.None, out this.peekActivityLease, false, null);
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x000D8244 File Offset: 0x000D6444
		protected virtual void EndPeek()
		{
			if (!this.isPeeking)
			{
				return;
			}
			this.isPeeking = false;
			this.currentTimeToPeek = 0f;
			this.tpInput.cc.animator.SetBool(this.peekingParameter, false);
			this.DeactivatePeekCamera();
			this.PlayCoverLocomotionState();
			if (this.debug)
			{
				Debug.Log("[vBasicCoverController] EndPeek");
			}
			this.onEndPeek.Invoke();
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000D82B4 File Offset: 0x000D64B4
		protected virtual void ResetPeekState(bool immediate = false)
		{
			this.CanPeekNow = false;
			if (this.isPeeking)
			{
				this.isPeeking = false;
				this.tpInput.cc.animator.SetBool(this.peekingParameter, false);
				if (immediate)
				{
					this.DeactivatePeekCameraImmediate();
				}
				else
				{
					this.DeactivatePeekCamera();
				}
				this.onEndPeek.Invoke();
			}
			else if (immediate)
			{
				this.DeactivatePeekCameraImmediate();
			}
			this.currentTimeToPeek = 0f;
			this.currentPeekWeight = 0f;
			this.isInLeftCornerRange = false;
			this.isInRightCornerRange = false;
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x000D833E File Offset: 0x000D653E
		protected virtual void DeactivatePeekCameraImmediate()
		{
			this.CancelPeekCameraTransition();
			this.DisablePeekVirtualCamera();
			this.ReleasePeekCameraLease(false);
			if (this.debug)
			{
				Debug.Log("[vBasicCoverController] Peek camera immediate teardown complete.");
			}
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x000D8365 File Offset: 0x000D6565
		protected virtual void ActivatePeekCamera(vBasicCoverController.Side peekSide)
		{
			if (this.UsesFirstPersonCoverView)
			{
				return;
			}
			if (this.peekVirtualCamera == null)
			{
				return;
			}
			this.ConfigurePeekCameraCollision();
			this.CancelPeekCameraTransition();
			this.peekCameraTransitionRoutine = base.StartCoroutine(this.ActivatePeekCameraRoutine(peekSide));
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x000D839E File Offset: 0x000D659E
		protected virtual IEnumerator ActivatePeekCameraRoutine(vBasicCoverController.Side peekSide)
		{
			if (this.UsesFirstPersonCoverView)
			{
				this.peekCameraTransitionRoutine = null;
				yield break;
			}
			int transitionVersion = this.peekCameraTransitionVersion;
			vThirdPersonInput vThirdPersonInput = this.tpInput;
			Camera camera = ((vThirdPersonInput != null) ? vThirdPersonInput.cameraMain : null);
			if (camera == null)
			{
				this.peekCameraTransitionRoutine = null;
				this.ReleasePeekCameraLease(false);
				yield break;
			}
			this.AcquirePeekCameraLease(camera);
			this.UpdatePeekCameraTargets(peekSide);
			this.peekVirtualCamera.Follow = this.peekFollowTarget;
			this.peekVirtualCamera.LookAt = this.peekLookTarget;
			if (this.peekCameraTransposer != null)
			{
				this.peekCameraTransposer.m_FollowOffset = ((peekSide == vBasicCoverController.Side.left) ? this.peekLeftCameraOffset : this.peekRightCameraOffset);
			}
			if (this.peekCameraComposer != null)
			{
				this.peekCameraComposer.m_TrackedObjectOffset = Vector3.zero;
			}
			this.ConfigurePeekCameraPriorities();
			if (this.cinemachineBrain != null)
			{
				this.appliedPeekBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, Mathf.Max(0f, this.peekCameraTransitionTime));
				this.cinemachineBrain.m_DefaultBlend = this.appliedPeekBlend;
			}
			if (!this.brainWasEnabledBeforePeek && this.cinemachineBrain != null)
			{
				this.CreateAnchorCamera();
				this.anchorCamera.transform.position = camera.transform.position;
				this.anchorCamera.transform.rotation = camera.transform.rotation;
				this.anchorCamera.m_Lens.FieldOfView = camera.fieldOfView;
				this.anchorCamera.Priority = this.activePeekCameraPriority - 1;
				this.anchorCamera.gameObject.SetActive(true);
				this.cinemachineBrain.enabled = true;
				yield return null;
				if (transitionVersion != this.peekCameraTransitionVersion || !this.isPeeking || this.tpInput == null || this.tpInput.cc == null || this.tpInput.cc.isCrouching || this.UsesFirstPersonCoverView)
				{
					if (transitionVersion == this.peekCameraTransitionVersion)
					{
						this.DisablePeekVirtualCamera();
						this.ReleasePeekCameraLease(false);
						this.peekCameraTransitionRoutine = null;
					}
					yield break;
				}
			}
			this.peekVirtualCamera.Priority = this.activePeekCameraPriority;
			if (!this.peekVirtualCamera.gameObject.activeSelf)
			{
				this.peekVirtualCamera.gameObject.SetActive(true);
			}
			this.peekCameraWasPromoted = true;
			this.peekCameraTransitionRoutine = null;
			if (this.debug)
			{
				Debug.Log(string.Format("[vBasicCoverController] Peek camera activated. Side={0}, Priority={1}", peekSide, this.peekVirtualCamera.Priority));
			}
			yield break;
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x000D83B4 File Offset: 0x000D65B4
		protected virtual void DeactivatePeekCamera()
		{
			this.CancelPeekCameraTransition();
			if (this.peekVirtualCamera == null || !base.isActiveAndEnabled)
			{
				this.DisablePeekVirtualCamera();
				this.ReleasePeekCameraLease(false);
				return;
			}
			this.peekCameraTransitionRoutine = base.StartCoroutine(this.DeactivatePeekCameraRoutine());
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x000D83F2 File Offset: 0x000D65F2
		protected virtual IEnumerator DeactivatePeekCameraRoutine()
		{
			int transitionVersion = this.peekCameraTransitionVersion;
			if (this.cinemachineBrain != null && this.brainWasEnabledBeforePeek && this.hasPeekCameraLease && vBasicCoverController.BlendEquals(this.cinemachineBrain.m_DefaultBlend, this.appliedPeekBlend))
			{
				this.cinemachineBrain.m_DefaultBlend = this.savedBrainDefaultBlend;
			}
			this.DisablePeekVirtualCamera();
			if (this.hasPeekCameraLease && this.peekCameraWasPromoted && !this.brainWasEnabledBeforePeek && this.cinemachineBrain != null)
			{
				if (this.debug)
				{
					Debug.Log("[vBasicCoverController] Peek camera deactivated, blending to anchor...");
				}
				yield return new WaitForSecondsRealtime(Mathf.Max(0f, this.peekCameraTransitionTime));
				if (transitionVersion != this.peekCameraTransitionVersion)
				{
					yield break;
				}
			}
			else if (this.hasPeekCameraLease && this.peekCameraWasPromoted && this.brainWasEnabledBeforePeek && this.cinemachineBrain != null)
			{
				yield return null;
				float timeout = Time.realtimeSinceStartup + Mathf.Max(1f, this.peekCameraTransitionTime * 3f + 0.25f);
				while (this.cinemachineBrain.IsBlending && Time.realtimeSinceStartup < timeout)
				{
					if (transitionVersion != this.peekCameraTransitionVersion)
					{
						yield break;
					}
					yield return null;
				}
			}
			this.ReleasePeekCameraLease(true);
			this.peekCameraTransitionRoutine = null;
			if (this.debug)
			{
				Debug.Log("[vBasicCoverController] Peek camera deactivation complete.");
			}
			yield break;
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x000D8404 File Offset: 0x000D6604
		protected virtual void AcquirePeekCameraLease(Camera mainCam)
		{
			if (this.hasPeekCameraLease)
			{
				return;
			}
			this.hasPeekCameraLease = true;
			this.peekCameraWasPromoted = false;
			this.previousCameraState = ((this.tpInput != null) ? this.tpInput.customCameraState : string.Empty);
			this.brainWasEnabledBeforePeek = this.cinemachineBrain != null && this.cinemachineBrain.enabled;
			if (this.cinemachineBrain != null)
			{
				this.savedBrainDefaultBlend = this.cinemachineBrain.m_DefaultBlend;
			}
			if (this.tpInput != null)
			{
				this.savedLockCameraInput = this.tpInput.lockCameraInput;
				this.tpInput.lockCameraInput = true;
			}
			if (!this.brainWasEnabledBeforePeek && mainCam != null)
			{
				this.savedCameraLocalPosition = mainCam.transform.localPosition;
				this.savedCameraLocalRotation = mainCam.transform.localRotation;
				this.savedCameraFieldOfView = mainCam.fieldOfView;
				this.savedCameraNearClipPlane = mainCam.nearClipPlane;
				this.hasSavedCameraState = true;
			}
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x000D850C File Offset: 0x000D670C
		protected virtual void ReleasePeekCameraLease(bool restoreVirtualCameraConfiguration)
		{
			bool flag = this.hasPeekCameraLease;
			if (flag && restoreVirtualCameraConfiguration)
			{
				this.RestorePeekVirtualCameraConfiguration();
			}
			if (this.anchorCamera != null)
			{
				this.anchorCamera.Priority = 5;
				this.anchorCamera.gameObject.SetActive(false);
			}
			if (flag && this.cinemachineBrain != null)
			{
				if (vBasicCoverController.BlendEquals(this.cinemachineBrain.m_DefaultBlend, this.appliedPeekBlend))
				{
					this.cinemachineBrain.m_DefaultBlend = this.savedBrainDefaultBlend;
				}
				if (!this.brainWasEnabledBeforePeek)
				{
					this.cinemachineBrain.enabled = false;
				}
			}
			vThirdPersonInput vThirdPersonInput = this.tpInput;
			Camera camera = ((vThirdPersonInput != null) ? vThirdPersonInput.cameraMain : null);
			if (flag && this.hasSavedCameraState && camera != null)
			{
				camera.transform.localPosition = this.savedCameraLocalPosition;
				camera.transform.localRotation = this.savedCameraLocalRotation;
			}
			if (flag)
			{
				this.hasSavedCameraState = false;
				if (this.tpInput != null)
				{
					this.tpInput.lockCameraInput = this.savedLockCameraInput;
				}
			}
			this.hasPeekCameraLease = false;
			this.peekCameraWasPromoted = false;
			PlayerActivityLease playerActivityLease = this.peekActivityLease;
			if (playerActivityLease != null)
			{
				playerActivityLease.Dispose();
			}
			this.peekActivityLease = null;
			if (flag && this.tpInput != null && this.inCover)
			{
				this.ControlCameraState();
			}
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x000D8659 File Offset: 0x000D6859
		protected static bool BlendEquals(CinemachineBlendDefinition a, CinemachineBlendDefinition b)
		{
			return a.m_Style == b.m_Style && Mathf.Approximately(a.m_Time, b.m_Time);
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x000D867C File Offset: 0x000D687C
		protected virtual void RestorePeekVirtualCameraConfiguration()
		{
			if (this.peekVirtualCamera == null)
			{
				return;
			}
			this.peekVirtualCamera.Priority = this.originalPeekCameraPriority;
			this.peekVirtualCamera.Follow = this.originalPeekFollow;
			this.peekVirtualCamera.LookAt = this.originalPeekLookAt;
			if (this.peekCameraTransposer != null)
			{
				this.peekCameraTransposer.m_FollowOffset = this.originalPeekCameraFollowOffset;
			}
			if (this.peekCameraComposer != null)
			{
				this.peekCameraComposer.m_TrackedObjectOffset = this.originalPeekCameraAimOffset;
			}
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x000D870C File Offset: 0x000D690C
		protected virtual void DisablePeekVirtualCamera()
		{
			if (this.peekVirtualCamera == null)
			{
				return;
			}
			this.peekVirtualCamera.Priority = this.originalPeekCameraPriority;
			if (this.peekVirtualCamera.gameObject.activeSelf)
			{
				this.peekVirtualCamera.gameObject.SetActive(false);
			}
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x000D875C File Offset: 0x000D695C
		protected virtual void ConfigurePeekCameraPriorities()
		{
			int num = this.originalPeekCameraPriority;
			foreach (CinemachineVirtualCameraBase cinemachineVirtualCameraBase in Object.FindObjectsOfType<CinemachineVirtualCameraBase>(true))
			{
				if (!(cinemachineVirtualCameraBase == null) && !(cinemachineVirtualCameraBase == this.peekVirtualCamera) && !(cinemachineVirtualCameraBase == this.anchorCamera) && cinemachineVirtualCameraBase.isActiveAndEnabled)
				{
					num = Mathf.Max(num, cinemachineVirtualCameraBase.Priority);
				}
			}
			long num2 = (long)num + (long)Mathf.Max(2, this.peekCameraPriorityBoost);
			this.activePeekCameraPriority = (int)Math.Min(2147483647L, num2);
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x000D87ED File Offset: 0x000D69ED
		protected virtual void CancelPeekCameraTransition()
		{
			this.peekCameraTransitionVersion++;
			if (this.peekCameraTransitionRoutine == null)
			{
				return;
			}
			base.StopCoroutine(this.peekCameraTransitionRoutine);
			this.peekCameraTransitionRoutine = null;
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000D881C File Offset: 0x000D6A1C
		public virtual float DistanceToPoseLine(vCoverPoint coverPoint)
		{
			if (coverPoint == null)
			{
				return float.MaxValue;
			}
			Transform transform = coverPoint.transform;
			Vector3 vector = transform.InverseTransformPoint(base.transform.position);
			Vector3 vector2 = transform.InverseTransformPoint(coverPoint.posePosition);
			float num = (coverPoint.boxCollider ? (coverPoint.boxCollider.size.x * 0.5f) : 0f);
			float num2 = vector2.x + (coverPoint.boxCollider ? coverPoint.boxCollider.center.x : 0f);
			vector2.x = Mathf.Clamp(vector.x, num2 - num, num2 + num);
			return Vector3.Distance(base.transform.position, transform.TransformPoint(vector2));
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x000D88E8 File Offset: 0x000D6AE8
		protected virtual vCoverPoint GetNearCoverPoint()
		{
			float num = (this.currentCoverPoint ? this.DistanceToPoseLine(this.currentCoverPoint) : this.maxEnterCoverDistance);
			vCoverPoint vCoverPoint = null;
			this.coverPoints = this.coverPoints.FindAll((vCoverPoint c) => this.IsEntryTargetUsable(c) && this.IsOnPlayerSideOfCover(c) && this.DistanceToPoseLine(c) < this.maxEnterCoverDistance);
			for (int i = 0; i < this.coverPoints.Count; i++)
			{
				float num2 = this.DistanceToPoseLine(this.coverPoints[i]);
				if (num2 < num - 0.0001f || (Mathf.Abs(num2 - num) <= 0.0001f && vCoverPoint != null && this.coverPoints[i].GetInstanceID() < vCoverPoint.GetInstanceID()))
				{
					vCoverPoint = this.coverPoints[i];
					num = num2;
				}
			}
			return vCoverPoint;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000D89AC File Offset: 0x000D6BAC
		protected virtual bool IsOnPlayerSideOfCover(vCoverPoint coverPoint)
		{
			if (coverPoint == null)
			{
				return false;
			}
			if (coverPoint.GetComponentInParent<CoverWallSetup>() == null)
			{
				return true;
			}
			Vector3 forward = coverPoint.transform.forward;
			forward.y = 0f;
			if (forward.sqrMagnitude < 0.0001f)
			{
				return true;
			}
			forward.Normalize();
			Vector3 vector = base.transform.position - coverPoint.transform.position;
			vector.y = 0f;
			return Vector3.Dot(vector, forward) >= -0.05f;
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000D8A3C File Offset: 0x000D6C3C
		protected virtual bool EnterCoverFromLeftSide(vCoverPoint coverPoint)
		{
			if (coverPoint == null)
			{
				return false;
			}
			this.isLeftCorner = this.IsLeftCoverCorner(coverPoint);
			this.isRightCorner = this.IsRightCoverCorner(coverPoint);
			if (this.isLeftCorner && this.isRightCorner)
			{
				Transform peekLeftRangeEnd = coverPoint.peekLeftRangeEnd;
				Transform peekRightRangeEnd = coverPoint.peekRightRangeEnd;
				if (peekLeftRangeEnd != null && peekRightRangeEnd != null)
				{
					return Vector3.SqrMagnitude(base.transform.position - peekLeftRangeEnd.position) <= Vector3.SqrMagnitude(base.transform.position - peekRightRangeEnd.position);
				}
				ref Vector3 ptr = coverPoint.transform.InverseTransformPoint(base.transform.position);
				float num = ((coverPoint.boxCollider != null) ? coverPoint.boxCollider.center.x : 0f);
				return ptr.x >= num;
			}
			else
			{
				if (this.isRightCorner)
				{
					return false;
				}
				if (this.isLeftCorner)
				{
					return true;
				}
				Vector3 vector = coverPoint.transform.position - this.tpInput.cameraMain.transform.position;
				vector.y = 0f;
				return (Quaternion.LookRotation(vector.normalized).eulerAngles - Quaternion.LookRotation(-coverPoint.transform.forward).eulerAngles).NormalizeAngle().y < -15f;
			}
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000D8BB8 File Offset: 0x000D6DB8
		protected virtual bool CheckCoverHeight(vCoverPoint coverPoint)
		{
			if (coverPoint == null)
			{
				return false;
			}
			ICoverStanceOverride componentInParent = coverPoint.GetComponentInParent<ICoverStanceOverride>();
			bool flag;
			if (componentInParent != null && componentInParent.TryGetForcedCrouch(out flag))
			{
				return flag;
			}
			Vector3 vector = (this.inCover ? this.helper.position : coverPoint.posePosition);
			Vector3 forward = coverPoint.transform.forward;
			Vector3 vector2 = vector + Vector3.up * this.crouchHeight + forward * 0.5f;
			float num = this.tpInput.cc.colliderRadiusDefault * 0.5f;
			int num2 = 0;
			bool flag2 = Physics.SphereCast(new Ray(vector2, -forward.normalized), this.crouchRayRadius, this.crouchRayDistance, this.obstaclesLayer, QueryTriggerInteraction.Ignore);
			if (!flag2)
			{
				num2++;
			}
			Vector3 vector3 = vector2 + coverPoint.transform.right * num;
			bool flag3 = Physics.SphereCast(new Ray(vector3, -forward.normalized), this.crouchRayRadius, this.crouchRayDistance, this.obstaclesLayer, QueryTriggerInteraction.Ignore);
			if (!flag3)
			{
				num2++;
			}
			Vector3 vector4 = vector2 - coverPoint.transform.right * num;
			bool flag4 = Physics.SphereCast(new Ray(vector4, -forward.normalized), this.crouchRayRadius, this.crouchRayDistance, this.obstaclesLayer, QueryTriggerInteraction.Ignore);
			if (!flag4)
			{
				num2++;
			}
			bool flag5 = num2 > 1;
			if (this.debug)
			{
				Debug.DrawRay(vector2, -forward.normalized * this.crouchRayDistance, flag2 ? Color.green : Color.red, 2f);
				Debug.DrawRay(vector3, -forward.normalized * this.crouchRayDistance, flag3 ? Color.green : Color.red, 2f);
				Debug.DrawRay(vector4, -forward.normalized * this.crouchRayDistance, flag4 ? Color.green : Color.red, 2f);
			}
			return flag5;
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x000D8DDD File Offset: 0x000D6FDD
		protected virtual bool IsLeftCoverCorner(vCoverPoint coverPoint)
		{
			return (coverPoint.corner & vCoverPoint.Corner.Left) > (vCoverPoint.Corner)0;
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000D8DEA File Offset: 0x000D6FEA
		protected virtual bool IsRightCoverCorner(vCoverPoint coverPoint)
		{
			return (coverPoint.corner & vCoverPoint.Corner.Right) > (vCoverPoint.Corner)0;
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000D8DF7 File Offset: 0x000D6FF7
		protected virtual bool IsCoverCorner(vCoverPoint coverPoint)
		{
			return this.IsRightCoverCorner(coverPoint) || this.IsLeftCoverCorner(coverPoint);
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000D8E0C File Offset: 0x000D700C
		protected virtual void OnTriggerStay(Collider other)
		{
			if (!this.coverTag.Contains(other.gameObject.tag))
			{
				return;
			}
			vCoverPoint component = other.gameObject.GetComponent<vCoverPoint>();
			if (component && !this.coverPoints.Contains(component))
			{
				this.coverPoints.Add(component);
			}
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x000D8E60 File Offset: 0x000D7060
		protected virtual void OnTriggerExit(Collider other)
		{
			if (!this.coverTag.Contains(other.gameObject.tag))
			{
				return;
			}
			vCoverPoint component = other.gameObject.GetComponent<vCoverPoint>();
			if (component && this.coverPoints.Contains(component))
			{
				this.coverPoints.Remove(component);
			}
		}

		// Token: 0x04002638 RID: 9784
		private FirstPersonController firstPersonController;

		// Token: 0x04002639 RID: 9785
		[vEditorToolbar("Cover Settings", false, "", false, false)]
		[vSeparator("Enter/Exit Cover Input", "")]
		public GenericInput enterExitInput = new GenericInput("Space", "A", "A");

		// Token: 0x0400263A RID: 9786
		[Tooltip("If true, exit cover on button press. If false, requires holding the button.")]
		public bool exitOnButtonPress = true;

		// Token: 0x0400263B RID: 9787
		[Tooltip("Time to wait before allowing exit after entering cover")]
		public float exitCooldown = 0.5f;

		// Token: 0x0400263C RID: 9788
		[Tooltip("Auto Exit Cover based on Input Direction")]
		public bool autoExitCover = true;

		// Token: 0x0400263D RID: 9789
		[vHideInInspector("autoExitCover", false)]
		[Tooltip("Angle of Input direction to exit cover")]
		public float exitInputDirectionAngle = 135f;

		// Token: 0x0400263E RID: 9790
		[vHideInInspector("autoExitCover", false)]
		[Tooltip("Timer to exit when Exit direction angle is reached")]
		public float exitDirectionTimer = 0.25f;

		// Token: 0x0400263F RID: 9791
		[vSeparator("Get Cover Settings", "")]
		[Range(1f, 25f)]
		public float maxEnterCoverDistance = 4f;

		// Token: 0x04002640 RID: 9792
		[Range(0.1f, 5f)]
		public float enterCoverDistance = 0.75f;

		// Token: 0x04002641 RID: 9793
		public float capsuleRadiusInCover = 0.2f;

		// Token: 0x04002642 RID: 9794
		[Range(15f, 90f)]
		public float minAngleOfValidCover = 80f;

		// Token: 0x04002643 RID: 9795
		public float positionOffsetZ;

		// Token: 0x04002644 RID: 9796
		[vSeparator("Crouch Settings", "")]
		public bool alwaysStandUpOnExit = true;

		// Token: 0x04002645 RID: 9797
		public float crouchRayRadius = 0.05f;

		// Token: 0x04002646 RID: 9798
		public float crouchRayDistance = 0.5f;

		// Token: 0x04002647 RID: 9799
		public float crouchHeight = 1.5f;

		// Token: 0x04002648 RID: 9800
		public float timeToStandUp = 0.5f;

		// Token: 0x04002649 RID: 9801
		[vEditorToolbar("Movement Speed", false, "", false, false)]
		public float crouchedMoveSpeed = 2f;

		// Token: 0x0400264A RID: 9802
		public float standingMoveSpeed = 2f;

		// Token: 0x0400264B RID: 9803
		[Tooltip("Maximum speed used to pull Ciel back to the cover plane after an external physics displacement.")]
		public float coverLineCorrectionSpeed = 6f;

		// Token: 0x0400264C RID: 9804
		[Tooltip("Maximum speed used to pull Ciel back inside a WallCover endpoint after an external physics displacement.")]
		[Min(0.1f)]
		public float coverEndpointRecoverySpeed = 6f;

		// Token: 0x0400264D RID: 9805
		[Tooltip("How far the capsule centre stays inside each physical end of a WallCover.")]
		public float coverEdgeInset = 0.2f;

		// Token: 0x0400264E RID: 9806
		public float goToCoverPointRunningMoveSpeed = 2f;

		// Token: 0x0400264F RID: 9807
		public float enterCoverAlignSpeed = 2f;

		// Token: 0x04002650 RID: 9808
		public float rotationSpeedMoving = 5f;

		// Token: 0x04002651 RID: 9809
		public float rotationSpeedIdle = 10f;

		// Token: 0x04002652 RID: 9810
		[vEditorToolbar("Layers and Tags", false, "", false, false)]
		public LayerMask obstaclesLayer;

		// Token: 0x04002653 RID: 9811
		public vTagMask checkHeightIgnoreTags = new vTagMask(new string[] { "Glass" });

		// Token: 0x04002654 RID: 9812
		public List<string> checkHeightIgnoreNames = new List<string>();

		// Token: 0x04002655 RID: 9813
		public LayerMask coverLayer;

		// Token: 0x04002656 RID: 9814
		public vTagMask coverTag;

		// Token: 0x04002657 RID: 9815
		[vEditorToolbar("Cover UI", false, "", false, false)]
		public GameObject coverUI;

		// Token: 0x04002658 RID: 9816
		public float coverIndicatorDistance = 5f;

		// Token: 0x04002659 RID: 9817
		public float coverIndicaterHeight = 1f;

		// Token: 0x0400265A RID: 9818
		[vSeparator("Corner Peek Settings", "")]
		[Tooltip("Input to trigger corner peek when at a corner")]
		public GenericInput peekInput = new GenericInput("F", "LB", "LB");

		// Token: 0x0400265B RID: 9819
		[Tooltip("Time to hold input before peek activates")]
		public float timeToEnterPeek = 0.1f;

		// Token: 0x0400265C RID: 9820
		[Tooltip("Smooth speed for peek weight transition")]
		public float peekWeightSmooth = 10f;

		// Token: 0x0400265D RID: 9821
		[Tooltip("Angle threshold to detect if looking towards corner")]
		public float angleToEnterCorner = -10f;

		// Token: 0x0400265E RID: 9822
		[Tooltip("Minimum angle to start moving towards corner")]
		public float minAngleToStartMoveToCorner = 10f;

		// Token: 0x0400265F RID: 9823
		[vEditorToolbar("Animation States", false, "", false, false)]
		[Tooltip("If true, use CrossFade to exit state. If false, let Animator handle exit via transitions.")]
		public bool useCrossFadeOnExit;

		// Token: 0x04002660 RID: 9824
		[Tooltip("Full path to free standing state (e.g. 'Free Locomotion.Female Movement.Idle'). Only used if useCrossFadeOnExit is true.")]
		public string defaultFreeStanding = "Free Locomotion.Female Movement.Idle";

		// Token: 0x04002661 RID: 9825
		[Tooltip("Full path to free crouched state. Only used if useCrossFadeOnExit is true.")]
		public string defaultFreeCrouched = "Free Locomotion.Female Movement.Free Crouch";

		// Token: 0x04002662 RID: 9826
		public string goToCoverPoint = "Go To Cover Point";

		// Token: 0x04002663 RID: 9827
		[Tooltip("State name for standing cover (Blend Tree that uses CoverSide parameter)")]
		public string coverStanding = "CoverStanding";

		// Token: 0x04002664 RID: 9828
		[Tooltip("State name for crouched cover (Blend Tree that uses CoverSide parameter)")]
		public string coverCrouched = "CoverCrouched";

		// Token: 0x04002665 RID: 9829
		[vSeparator("Corner Peek Animation States", "")]
		[Tooltip("State name for corner peek left standing")]
		public string cornerPeekLeftStanding = "CornerPeekLeftStanding";

		// Token: 0x04002666 RID: 9830
		[Tooltip("State name for corner peek right standing")]
		public string cornerPeekRightStanding = "CornerPeekRightStanding";

		// Token: 0x04002667 RID: 9831
		[Tooltip("State name for corner peek left crouched")]
		public string cornerPeekLeftCrouched = "CornerPeekLeftCrouched";

		// Token: 0x04002668 RID: 9832
		[Tooltip("State name for corner peek right crouched")]
		public string cornerPeekRightCrouched = "CornerPeekRightCrouched";

		// Token: 0x04002669 RID: 9833
		[Range(0.1f, 0.5f)]
		public float enterCoverAnimCrossfade = 0.25f;

		// Token: 0x0400266A RID: 9834
		[Range(0.1f, 0.5f)]
		public float exitCoverAnimCrossfade = 0.25f;

		// Token: 0x0400266B RID: 9835
		[Range(0.1f, 0.5f)]
		public float goToCoverAnimCrossfade = 0.25f;

		// Token: 0x0400266C RID: 9836
		[vSeparator("Animator Layer Settings", "")]
		[Tooltip("Names of additional animator layers to sync (e.g. 'Naked', 'Confident', 'Embarassed')")]
		public List<string> additionalAnimatorLayers = new List<string> { "Naked", "Confident", "Embarassed" };

		// Token: 0x0400266D RID: 9837
		[Tooltip("Animator parameter name for cover side (0=left, 1=right)")]
		public string coverSideParameter = "CoverSide";

		// Token: 0x0400266E RID: 9838
		[vEditorToolbar("Camera Settings", false, "", false, false)]
		public string standCameraState = "CoverStand";

		// Token: 0x0400266F RID: 9839
		public string crouchCameraState = "CoverCrouch";

		// Token: 0x04002670 RID: 9840
		[vSeparator("Cinemachine Peek Camera", "")]
		[Tooltip("Cinemachine Virtual Camera to use during peek. Will be activated when peeking.")]
		public CinemachineVirtualCamera peekVirtualCamera;

		// Token: 0x04002671 RID: 9841
		[Tooltip("Camera offset when peeking left (applied to CinemachineTransposer)")]
		public Vector3 peekLeftCameraOffset = new Vector3(-1.5f, 0.5f, -2f);

		// Token: 0x04002672 RID: 9842
		[Tooltip("Camera offset when peeking right (applied to CinemachineTransposer)")]
		public Vector3 peekRightCameraOffset = new Vector3(1.5f, 0.5f, -2f);

		// Token: 0x04002673 RID: 9843
		[HideInInspector]
		[Tooltip("Legacy serialized value. Peek aim now uses a stable cover-space target.")]
		public Vector3 peekLeftAimOffset = Vector3.zero;

		// Token: 0x04002674 RID: 9844
		[HideInInspector]
		[Tooltip("Legacy serialized value. Peek aim now uses a stable cover-space target.")]
		public Vector3 peekRightAimOffset = Vector3.zero;

		// Token: 0x04002675 RID: 9845
		[Tooltip("How far behind the wall the peek camera looks. Uses a stable cover-space target instead of the character yaw.")]
		public float peekLookBehindDistance = 10f;

		// Token: 0x04002676 RID: 9846
		[Tooltip("Height of the stable peek look target above the character root.")]
		public float peekLookHeight = 1.25f;

		// Token: 0x04002677 RID: 9847
		[Tooltip("How far beyond the current corner the stable peek look target sits.")]
		public float peekLookPastCorner = 1f;

		// Token: 0x04002678 RID: 9848
		[Tooltip("Priority boost for peek camera when active")]
		public int peekCameraPriorityBoost = 10;

		// Token: 0x04002679 RID: 9849
		[Tooltip("Smooth time for camera offset transition")]
		public float peekCameraTransitionTime = 0.2f;

		// Token: 0x0400267A RID: 9850
		[vSeparator("Peek Input Mode", "")]
		[Tooltip("If true, peek works as toggle (press to start, press again to stop). If false, peek requires holding the button.")]
		public bool peekToggleMode;

		// Token: 0x0400267B RID: 9851
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onEnterCover;

		// Token: 0x0400267C RID: 9852
		public UnityEvent onExitCover;

		// Token: 0x0400267D RID: 9853
		public UnityEvent onStartGoToCoverPoint;

		// Token: 0x0400267E RID: 9854
		public UnityEvent onFinishGoToCoverPoint;

		// Token: 0x0400267F RID: 9855
		public UnityEvent onStartPeek;

		// Token: 0x04002680 RID: 9856
		public UnityEvent onEndPeek;

		// Token: 0x04002681 RID: 9857
		[vEditorToolbar("Debug", false, "", false, false)]
		public bool debug;

		// Token: 0x04002682 RID: 9858
		[vReadOnly(true)]
		[SerializeField]
		protected string previousCameraState;

		// Token: 0x04002683 RID: 9859
		[vReadOnly(true)]
		public bool inCover;

		// Token: 0x04002684 RID: 9860
		[vReadOnly(true)]
		public bool wasInCover;

		// Token: 0x04002685 RID: 9861
		[vReadOnly(true)]
		public bool goingToCoverPoint;

		// Token: 0x04002686 RID: 9862
		[vReadOnly(true)]
		public bool isPeeking;

		// Token: 0x04002687 RID: 9863
		[vReadOnly(true)]
		public vCoverPoint currentCoverPoint;

		// Token: 0x04002688 RID: 9864
		[vReadOnly(true)]
		public vCoverPoint possibleCoverPoint;

		// Token: 0x04002689 RID: 9865
		[vReadOnly(true)]
		public vCoverPoint pendingCoverPoint;

		// Token: 0x0400268A RID: 9866
		[vReadOnly(true)]
		[SerializeField]
		protected bool isInLeftCornerRange;

		// Token: 0x0400268B RID: 9867
		[vReadOnly(true)]
		[SerializeField]
		protected bool isInRightCornerRange;

		// Token: 0x0400268C RID: 9868
		[vReadOnly(true)]
		[SerializeField]
		protected float currentPeekWeight;

		// Token: 0x0400268E RID: 9870
		protected vThirdPersonInput tpInput;

		// Token: 0x0400268F RID: 9871
		protected vGenericAction genericAction;

		// Token: 0x04002690 RID: 9872
		protected Transform helper;

		// Token: 0x04002691 RID: 9873
		protected List<vCoverPoint> coverPoints = new List<vCoverPoint>();

		// Token: 0x04002692 RID: 9874
		protected Vector3 coverIndicatorPosition;

		// Token: 0x04002693 RID: 9875
		protected Vector3 coverNormal;

		// Token: 0x04002694 RID: 9876
		protected Vector3 inputDirection;

		// Token: 0x04002695 RID: 9877
		protected Vector3 cameraForwardDir;

		// Token: 0x04002696 RID: 9878
		protected Vector3 cameraRightDir;

		// Token: 0x04002697 RID: 9879
		protected Vector3 coverMoveDirection;

		// Token: 0x04002698 RID: 9880
		protected Vector3 localCharPosition;

		// Token: 0x04002699 RID: 9881
		protected float timeToExit;

		// Token: 0x0400269A RID: 9882
		protected float currentTimeToGetUp;

		// Token: 0x0400269B RID: 9883
		protected float timeToExitByDirection;

		// Token: 0x0400269C RID: 9884
		protected bool enterCoverFromLeftSide;

		// Token: 0x0400269D RID: 9885
		protected bool originalWalkByDefault;

		// Token: 0x0400269E RID: 9886
		protected bool lastMoveToDirectionInFree;

		// Token: 0x0400269F RID: 9887
		protected bool isLeftCorner;

		// Token: 0x040026A0 RID: 9888
		protected bool isRightCorner;

		// Token: 0x040026A1 RID: 9889
		protected bool canMove;

		// Token: 0x040026A2 RID: 9890
		protected bool wasCrouchingBeforeCover;

		// Token: 0x040026A3 RID: 9891
		protected bool wasUsingRootMotionBeforeCover;

		// Token: 0x040026A4 RID: 9892
		protected bool wasIgnoringAnimatorMovementBeforeCover;

		// Token: 0x040026A5 RID: 9893
		protected bool wasLockCameraInputBeforeCover;

		// Token: 0x040026A6 RID: 9894
		protected RigidbodyInterpolation interpolationBeforeCover;

		// Token: 0x040026A7 RID: 9895
		protected Rigidbody coverInterpolationBody;

		// Token: 0x040026A8 RID: 9896
		protected bool hasCoverInterpolationLease;

		// Token: 0x040026A9 RID: 9897
		protected bool coverInputCallbacksSubscribed;

		// Token: 0x040026AA RID: 9898
		protected bool coverControllerInitialized;

		// Token: 0x040026AB RID: 9899
		protected List<int> additionalLayerIndices = new List<int>();

		// Token: 0x040026AC RID: 9900
		protected vBasicCoverController.Side side = vBasicCoverController.Side.right;

		// Token: 0x040026AD RID: 9901
		protected float currentTimeToPeek;

		// Token: 0x040026AE RID: 9902
		protected float cameraAngle;

		// Token: 0x040026AF RID: 9903
		protected bool inCornerAngle;

		// Token: 0x040026B0 RID: 9904
		protected string peekTriggerParameter = "PeekTrigger";

		// Token: 0x040026B1 RID: 9905
		protected string peekingParameter = "IsPeeking";

		// Token: 0x040026B2 RID: 9906
		protected string peekSideParameter = "PeekSide";

		// Token: 0x040026B3 RID: 9907
		protected vBasicCoverController.Side currentPeekSide = vBasicCoverController.Side.left;

		// Token: 0x040026B4 RID: 9908
		protected int originalPeekCameraPriority;

		// Token: 0x040026B5 RID: 9909
		protected CinemachineTransposer peekCameraTransposer;

		// Token: 0x040026B6 RID: 9910
		protected CinemachineComposer peekCameraComposer;

		// Token: 0x040026B7 RID: 9911
		protected CinemachineCollider peekCameraCollider;

		// Token: 0x040026B8 RID: 9912
		protected Vector3 originalPeekCameraAimOffset;

		// Token: 0x040026B9 RID: 9913
		protected Vector3 originalPeekCameraFollowOffset;

		// Token: 0x040026BA RID: 9914
		protected CinemachineBrain cinemachineBrain;

		// Token: 0x040026BB RID: 9915
		protected Transform originalPeekFollow;

		// Token: 0x040026BC RID: 9916
		protected Transform originalPeekLookAt;

		// Token: 0x040026BD RID: 9917
		protected Transform peekFollowTarget;

		// Token: 0x040026BE RID: 9918
		protected Transform peekLookTarget;

		// Token: 0x040026BF RID: 9919
		protected Coroutine peekCameraTransitionRoutine;

		// Token: 0x040026C0 RID: 9920
		protected int peekCameraTransitionVersion;

		// Token: 0x040026C1 RID: 9921
		protected bool peekInputRequiresRelease;

		// Token: 0x040026C2 RID: 9922
		protected bool hasPeekCameraLease;

		// Token: 0x040026C3 RID: 9923
		protected bool brainWasEnabledBeforePeek;

		// Token: 0x040026C4 RID: 9924
		protected CinemachineBlendDefinition savedBrainDefaultBlend;

		// Token: 0x040026C5 RID: 9925
		protected int activePeekCameraPriority;

		// Token: 0x040026C6 RID: 9926
		protected PlayerActivityLease peekActivityLease;

		// Token: 0x040026C7 RID: 9927
		protected PlayerActivityLease coverActivityLease;

		// Token: 0x040026C8 RID: 9928
		protected const PlayerContextResource CoverResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation;

		// Token: 0x040026C9 RID: 9929
		protected bool savedLockCameraInput;

		// Token: 0x040026CA RID: 9930
		protected CinemachineBlendDefinition appliedPeekBlend;

		// Token: 0x040026CB RID: 9931
		protected bool peekCameraWasPromoted;

		// Token: 0x040026CC RID: 9932
		protected Vector3 savedCameraLocalPosition;

		// Token: 0x040026CD RID: 9933
		protected Quaternion savedCameraLocalRotation;

		// Token: 0x040026CE RID: 9934
		private float savedCameraFieldOfView;

		// Token: 0x040026CF RID: 9935
		private float savedCameraNearClipPlane;

		// Token: 0x040026D0 RID: 9936
		protected bool hasSavedCameraState;

		// Token: 0x040026D1 RID: 9937
		protected CinemachineVirtualCamera anchorCamera;

		// Token: 0x0200067B RID: 1659
		protected enum Side
		{
			// Token: 0x040026D3 RID: 9939
			left = -1,
			// Token: 0x040026D4 RID: 9940
			right = 1
		}
	}
}
