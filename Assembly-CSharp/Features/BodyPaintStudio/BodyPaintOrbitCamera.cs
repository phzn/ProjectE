using System;
using Features.MainMenu;
using Features.NeckSigns;
using Invector.vCamera;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.BodyPaintStudio
{
	// Token: 0x02001511 RID: 5393
	public class BodyPaintOrbitCamera : MonoBehaviour
	{
		// Token: 0x170019B4 RID: 6580
		// (get) Token: 0x0600A6A9 RID: 42665 RVA: 0x0045AD4F File Offset: 0x00458F4F
		public bool IsActive
		{
			get
			{
				return this._active;
			}
		}

		// Token: 0x170019B5 RID: 6581
		// (get) Token: 0x0600A6AA RID: 42666 RVA: 0x0045AD57 File Offset: 0x00458F57
		// (set) Token: 0x0600A6AB RID: 42667 RVA: 0x0045AD5F File Offset: 0x00458F5F
		public Func<bool> InputBlocker { get; set; }

		// Token: 0x170019B6 RID: 6582
		// (get) Token: 0x0600A6AC RID: 42668 RVA: 0x0045AD68 File Offset: 0x00458F68
		public Transform AnimatedPivot
		{
			get
			{
				return this._animatedPivot;
			}
		}

		// Token: 0x170019B7 RID: 6583
		// (get) Token: 0x0600A6AD RID: 42669 RVA: 0x0045AD70 File Offset: 0x00458F70
		// (set) Token: 0x0600A6AE RID: 42670 RVA: 0x0045AD78 File Offset: 0x00458F78
		public Vector3 OrbitPivot { get; private set; }

		// Token: 0x170019B8 RID: 6584
		// (get) Token: 0x0600A6AF RID: 42671 RVA: 0x0045AD81 File Offset: 0x00458F81
		public float RequestedDistance
		{
			get
			{
				return this._currentDistance;
			}
		}

		// Token: 0x170019B9 RID: 6585
		// (get) Token: 0x0600A6B0 RID: 42672 RVA: 0x0045AD89 File Offset: 0x00458F89
		// (set) Token: 0x0600A6B1 RID: 42673 RVA: 0x0045AD91 File Offset: 0x00458F91
		public float ResolvedDistance { get; private set; }

		// Token: 0x170019BA RID: 6586
		// (get) Token: 0x0600A6B2 RID: 42674 RVA: 0x0045AD9A File Offset: 0x00458F9A
		// (set) Token: 0x0600A6B3 RID: 42675 RVA: 0x0045ADA2 File Offset: 0x00458FA2
		public bool ObstacleLimited { get; private set; }

		// Token: 0x0600A6B4 RID: 42676 RVA: 0x0045ADAB File Offset: 0x00458FAB
		public void LockZoom(float fixedDistance)
		{
			this._lockedDistance = Mathf.Clamp(fixedDistance, this.minDistance, this.maxDistance);
			this._currentDistance = this._lockedDistance;
		}

		// Token: 0x0600A6B5 RID: 42677 RVA: 0x0045ADD4 File Offset: 0x00458FD4
		public void EnableZoom(float initialDistance, float sessionMinDistance, float sessionMaxDistance)
		{
			float num = Mathf.Min(sessionMinDistance, sessionMaxDistance);
			float num2 = Mathf.Max(sessionMinDistance, sessionMaxDistance);
			this._activeMinDistance = Mathf.Clamp(num, this.minDistance, this.maxDistance);
			this._activeMaxDistance = Mathf.Clamp(num2, this._activeMinDistance, this.maxDistance);
			this._lockedDistance = -1f;
			this._currentDistance = Mathf.Clamp(initialDistance, this._activeMinDistance, this._activeMaxDistance);
		}

		// Token: 0x0600A6B6 RID: 42678 RVA: 0x0045AE44 File Offset: 0x00459044
		public void FollowAnimatedPivot(Transform animatedPivot)
		{
			this._animatedPivot = animatedPivot;
			this._followAnimatedPivotY = false;
			this._animatedPivotOffset = Vector3.zero;
		}

		// Token: 0x0600A6B7 RID: 42679 RVA: 0x0045AE5F File Offset: 0x0045905F
		public void FollowAnimatedPivotFull(Transform animatedPivot, Vector3 worldOffset)
		{
			this._animatedPivot = animatedPivot;
			this._followAnimatedPivotY = true;
			this._animatedPivotOffset = worldOffset;
		}

		// Token: 0x0600A6B8 RID: 42680 RVA: 0x0045AE78 File Offset: 0x00459078
		public void FramePaintSurface(NeckSignSurface surface)
		{
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			if (!this._active || surface == null || !surface.TryGetPaintFrame(out vector, out vector2, out vector3))
			{
				return;
			}
			this.FollowAnimatedPivotFull(surface.transform, vector - surface.transform.position);
			this._surfaceFrame = Quaternion.LookRotation(-vector2, vector3);
			this._yaw = 0f;
			this._pitch = 0f;
			this._pivotYOffset = 0f;
			Vector3 extents = surface.PaintBounds.extents;
			Vector3 vector4 = this._surfaceFrame * Vector3.right;
			Vector3 vector5 = this._surfaceFrame * Vector3.up;
			float num = Vector3.Dot(extents, new Vector3(Mathf.Abs(vector4.x), Mathf.Abs(vector4.y), Mathf.Abs(vector4.z)));
			float num2 = Vector3.Dot(extents, new Vector3(Mathf.Abs(vector5.x), Mathf.Abs(vector5.y), Mathf.Abs(vector5.z)));
			float num3 = Mathf.Tan(((this._mainCamera != null) ? this._mainCamera.fieldOfView : 60f) * 0.5f * 0.017453292f);
			float num4 = ((this._mainCamera != null) ? this._mainCamera.aspect : 1.7777778f);
			float num5 = Mathf.Max(num2 / num3, num / (num3 * num4 * 0.7f)) * 1.3f;
			this.EnableZoom(Mathf.Max(0.35f, num5), 0.12f, this.maxDistance);
			this.OrbitPivot = vector;
			this._tpCamera.transform.SetPositionAndRotation(vector + vector2 * this._currentDistance, this._surfaceFrame);
		}

		// Token: 0x0600A6B9 RID: 42681 RVA: 0x0045B042 File Offset: 0x00459242
		public void EnableObstacleAvoidance(bool enabled)
		{
			this._obstacleAvoidance = enabled;
			this.ObstacleLimited = false;
		}

		// Token: 0x0600A6BA RID: 42682 RVA: 0x0045B054 File Offset: 0x00459254
		public void Activate(vThirdPersonCamera tpCamera, Transform target)
		{
			if (tpCamera == null || target == null)
			{
				Debug.LogWarning("[BodyPaintOrbitCamera] Attivazione fallita: camera o target mancanti.");
				return;
			}
			this._tpCamera = tpCamera;
			this._target = target;
			this._animatedPivot = null;
			this._followAnimatedPivotY = false;
			this._animatedPivotOffset = Vector3.zero;
			this._obstacleAvoidance = false;
			this.ObstacleLimited = false;
			this.ResolvedDistance = 0f;
			this._pivotYOffset = 0f;
			this._surfaceFrame = Quaternion.identity;
			this._yaw = target.eulerAngles.y + 180f;
			this._pitch = 10f;
			this._lockedDistance = -1f;
			this._activeMinDistance = this.minDistance;
			this._activeMaxDistance = this.maxDistance;
			this._currentDistance = Mathf.Clamp(this.distance, this._activeMinDistance, this._activeMaxDistance);
			this._mainCamera = Camera.main;
			if (this._mainCamera != null)
			{
				this._savedNearClipPlane = this._mainCamera.nearClipPlane;
				this._mainCamera.nearClipPlane = this.paintNearClipPlane;
			}
			this._tpCamera.isFreezed = true;
			this._active = true;
		}

		// Token: 0x0600A6BB RID: 42683 RVA: 0x0045B184 File Offset: 0x00459384
		public void Deactivate()
		{
			this.InputBlocker = null;
			if (!this._active)
			{
				return;
			}
			this._active = false;
			this._lockedDistance = -1f;
			if (this._mainCamera != null)
			{
				this._mainCamera.nearClipPlane = this._savedNearClipPlane;
			}
			this._mainCamera = null;
			if (this._tpCamera != null)
			{
				this._tpCamera.isFreezed = false;
			}
			this._tpCamera = null;
			this._target = null;
			this._animatedPivot = null;
			this._followAnimatedPivotY = false;
			this._animatedPivotOffset = Vector3.zero;
			this._obstacleAvoidance = false;
			this.ObstacleLimited = false;
			this.ResolvedDistance = 0f;
		}

		// Token: 0x0600A6BC RID: 42684 RVA: 0x0045B234 File Offset: 0x00459434
		private Vector3 PivotPosition()
		{
			float num = Mathf.Clamp(this.pivotHeight + this._pivotYOffset, this.minPivotY, this.maxPivotY);
			Vector3 position = this._target.position;
			if (this._animatedPivot != null)
			{
				if (this._followAnimatedPivotY)
				{
					float num2 = Mathf.Clamp(this._pivotYOffset, -Mathf.Max(0.05f, this.animatedPivotPanLimit), Mathf.Max(0.05f, this.animatedPivotPanLimit));
					return this._animatedPivot.position + this._animatedPivotOffset + this._surfaceFrame * Vector3.up * num2;
				}
				position.x = this._animatedPivot.position.x;
				position.z = this._animatedPivot.position.z;
			}
			return position + Vector3.up * num;
		}

		// Token: 0x0600A6BD RID: 42685 RVA: 0x0045B324 File Offset: 0x00459524
		private Vector3 ResolveObstacleCollision(Vector3 pivot, Vector3 candidatePosition)
		{
			Vector3 vector = candidatePosition - pivot;
			float magnitude = vector.magnitude;
			this.ResolvedDistance = magnitude;
			this.ObstacleLimited = false;
			if (!this._obstacleAvoidance || magnitude <= 0.0001f)
			{
				return candidatePosition;
			}
			Vector3 vector2 = vector / magnitude;
			int num = Physics.SphereCastNonAlloc(pivot, Mathf.Max(0.01f, this.cameraCollisionRadius), vector2, this._cameraCollisionHits, magnitude, this.cameraCollisionMask, QueryTriggerInteraction.Ignore);
			float num2 = magnitude;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this._cameraCollisionHits[i];
				if (!(raycastHit.collider == null) && raycastHit.distance > 0.0001f)
				{
					Transform transform = raycastHit.collider.transform;
					if ((!(this._target != null) || (!(transform == this._target) && !transform.IsChildOf(this._target))) && raycastHit.distance < num2)
					{
						num2 = raycastHit.distance;
					}
				}
			}
			if (num2 >= magnitude)
			{
				return candidatePosition;
			}
			float num3 = Mathf.Max(0.05f, num2 - Mathf.Max(0f, this.cameraCollisionPadding));
			this.ResolvedDistance = num3;
			this.ObstacleLimited = true;
			return pivot + vector2 * num3;
		}

		// Token: 0x0600A6BE RID: 42686 RVA: 0x0045B46C File Offset: 0x0045966C
		private void ReadInput()
		{
			if (this.InputBlocker != null && this.InputBlocker())
			{
				return;
			}
			if (ControllerInput.IsActive)
			{
				this.ReadControllerInput(Time.unscaledDeltaTime);
				return;
			}
			if (GameplayInput.GetMouseButton(1))
			{
				Vector2 vector = GameSettingsService.ApplyMouseLookInversion(new Vector2(GameplayInput.GetAxis("Mouse X"), GameplayInput.GetAxis("Mouse Y")));
				this._yaw += vector.x * this.rotateSensitivity;
				this._pitch -= vector.y * this.rotateSensitivity;
				this._pitch = Mathf.Clamp(this._pitch, this.minPitch, this.maxPitch);
			}
			if (GameplayInput.GetMouseButton(2))
			{
				this._pivotYOffset -= GameplayInput.GetAxis("Mouse Y") * this.panSensitivity;
				this.ClampPivotOffset();
			}
			float y = GameplayInput.mouseScrollDelta.y;
			if (this._lockedDistance <= 0f && !Mathf.Approximately(y, 0f))
			{
				this._currentDistance -= y * this._currentDistance * this.zoomSensitivity;
				this._currentDistance = Mathf.Clamp(this._currentDistance, this._activeMinDistance, this._activeMaxDistance);
			}
		}

		// Token: 0x0600A6BF RID: 42687 RVA: 0x0045B5A4 File Offset: 0x004597A4
		private void ReadControllerInput(float deltaTime)
		{
			if (ControllerWorldPointer.WantsUIFocus || ControllerWorldPointer.BlocksContextInput)
			{
				return;
			}
			Vector2 vector = ControllerInput.Move * (30f * deltaTime * this.rotateSensitivity);
			this._yaw += vector.x;
			this._pitch = Mathf.Clamp(this._pitch - vector.y, this.minPitch, this.maxPitch);
			float num = (ControllerInput.GetButton(ControllerButton.RightShoulder) ? 1f : 0f) - (ControllerInput.GetButton(ControllerButton.LeftShoulder) ? 1f : 0f);
			this._pivotYOffset += num * this.panSensitivity * 6f * deltaTime;
			this.ClampPivotOffset();
			if (this._lockedDistance <= 0f && ControllerInput.Modifier)
			{
				this._currentDistance *= Mathf.Exp(-ControllerInput.Look.y * deltaTime * this.zoomSensitivity * 10f);
				this._currentDistance = Mathf.Clamp(this._currentDistance, this._activeMinDistance, this._activeMaxDistance);
			}
		}

		// Token: 0x0600A6C0 RID: 42688 RVA: 0x0045B6BC File Offset: 0x004598BC
		private void ClampPivotOffset()
		{
			if (this._animatedPivot != null && this._followAnimatedPivotY)
			{
				float num = Mathf.Max(0.05f, this.animatedPivotPanLimit);
				this._pivotYOffset = Mathf.Clamp(this._pivotYOffset, -num, num);
				return;
			}
			this._pivotYOffset = Mathf.Clamp(this._pivotYOffset, this.minPivotY - this.pivotHeight, this.maxPivotY - this.pivotHeight);
		}

		// Token: 0x0600A6C1 RID: 42689 RVA: 0x0045B730 File Offset: 0x00459930
		private void LateUpdate()
		{
			if (!this._active || this._tpCamera == null || this._target == null)
			{
				return;
			}
			this.ReadInput();
			Vector3 vector = this.PivotPosition();
			this.OrbitPivot = vector;
			Quaternion quaternion = this._surfaceFrame * Quaternion.Euler(this._pitch, this._yaw, 0f);
			Vector3 vector2 = vector + quaternion * (Vector3.back * this._currentDistance);
			float num = 1f - Mathf.Exp(-this.smoothing * Time.unscaledDeltaTime);
			Vector3 vector3 = Vector3.Lerp(this._tpCamera.transform.position, vector2, num);
			Vector3 vector4 = this.ResolveObstacleCollision(vector, vector3);
			Quaternion quaternion2 = Quaternion.LookRotation(vector - vector4, this._surfaceFrame * Vector3.up);
			this._tpCamera.transform.position = vector4;
			this._tpCamera.transform.rotation = Quaternion.Slerp(this._tpCamera.transform.rotation, quaternion2, num);
		}

		// Token: 0x0400AB79 RID: 43897
		[Header("Orbit")]
		[SerializeField]
		private float pivotHeight = 1f;

		// Token: 0x0400AB7A RID: 43898
		[SerializeField]
		private float distance = 2.2f;

		// Token: 0x0400AB7B RID: 43899
		[SerializeField]
		private float minDistance = 0.12f;

		// Token: 0x0400AB7C RID: 43900
		[SerializeField]
		private float maxDistance = 4.5f;

		// Token: 0x0400AB7D RID: 43901
		[SerializeField]
		private float rotateSensitivity = 4f;

		// Token: 0x0400AB7E RID: 43902
		[Tooltip("Zoom proporzionale: frazione della distanza corrente per scatto di rotella")]
		[SerializeField]
		private float zoomSensitivity = 0.18f;

		// Token: 0x0400AB7F RID: 43903
		[SerializeField]
		private float minPitch = -35f;

		// Token: 0x0400AB80 RID: 43904
		[SerializeField]
		private float maxPitch = 75f;

		// Token: 0x0400AB81 RID: 43905
		[SerializeField]
		private float smoothing = 12f;

		// Token: 0x0400AB82 RID: 43906
		[Header("Pan verticale (middle mouse)")]
		[SerializeField]
		private float panSensitivity = 0.12f;

		// Token: 0x0400AB83 RID: 43907
		[Tooltip("Range assoluto del punto orbitato rispetto ai piedi (modello 1.4x)")]
		[SerializeField]
		private float minPivotY = 0.1f;

		// Token: 0x0400AB84 RID: 43908
		[SerializeField]
		private float maxPivotY = 2.3f;

		// Token: 0x0400AB85 RID: 43909
		[Tooltip("Escursione verticale massima quando il pivot segue completamente un bone (close-up groping).")]
		[SerializeField]
		[Min(0.05f)]
		private float animatedPivotPanLimit = 0.36f;

		// Token: 0x0400AB86 RID: 43910
		[Header("Close-up")]
		[Tooltip("Near clip plane durante la modalita' paint (default ~0.3 taglierebbe la mesh)")]
		[SerializeField]
		private float paintNearClipPlane = 0.01f;

		// Token: 0x0400AB87 RID: 43911
		[Header("Collisione ambiente (sessione)")]
		[Tooltip("Raggio del volume che protegge la camera da muri, vasca e arredi.")]
		[SerializeField]
		[Min(0.01f)]
		private float cameraCollisionRadius = 0.08f;

		// Token: 0x0400AB88 RID: 43912
		[Tooltip("Margine aggiuntivo lasciato davanti alla superficie colpita.")]
		[SerializeField]
		[Min(0f)]
		private float cameraCollisionPadding = 0.03f;

		// Token: 0x0400AB89 RID: 43913
		[Tooltip("Layer solidi che possono limitare la camera. Esclude player e proxy soap.")]
		[SerializeField]
		private LayerMask cameraCollisionMask = 277352449;

		// Token: 0x0400AB8A RID: 43914
		private vThirdPersonCamera _tpCamera;

		// Token: 0x0400AB8B RID: 43915
		private Transform _target;

		// Token: 0x0400AB8C RID: 43916
		private Transform _animatedPivot;

		// Token: 0x0400AB8D RID: 43917
		private bool _followAnimatedPivotY;

		// Token: 0x0400AB8E RID: 43918
		private Vector3 _animatedPivotOffset;

		// Token: 0x0400AB8F RID: 43919
		private Camera _mainCamera;

		// Token: 0x0400AB90 RID: 43920
		private float _yaw;

		// Token: 0x0400AB91 RID: 43921
		private float _pitch;

		// Token: 0x0400AB92 RID: 43922
		private float _currentDistance;

		// Token: 0x0400AB93 RID: 43923
		private float _pivotYOffset;

		// Token: 0x0400AB94 RID: 43924
		private float _savedNearClipPlane;

		// Token: 0x0400AB95 RID: 43925
		private bool _active;

		// Token: 0x0400AB96 RID: 43926
		private bool _obstacleAvoidance;

		// Token: 0x0400AB97 RID: 43927
		private float _lockedDistance = -1f;

		// Token: 0x0400AB98 RID: 43928
		private float _activeMinDistance;

		// Token: 0x0400AB99 RID: 43929
		private float _activeMaxDistance;

		// Token: 0x0400AB9A RID: 43930
		private Quaternion _surfaceFrame = Quaternion.identity;

		// Token: 0x0400AB9B RID: 43931
		private readonly RaycastHit[] _cameraCollisionHits = new RaycastHit[16];
	}
}
