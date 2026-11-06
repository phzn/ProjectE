using System;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012D4 RID: 4820
	public class PlayerEyeLookAt : MonoBehaviour
	{
		// Token: 0x06009563 RID: 38243 RVA: 0x003D51CD File Offset: 0x003D33CD
		private void Awake()
		{
			this._playerTransform = base.transform;
		}

		// Token: 0x06009564 RID: 38244 RVA: 0x003D51DB File Offset: 0x003D33DB
		private void Start()
		{
			this.CaptureRestPose();
			if (this.leftEye == null || this.rightEye == null)
			{
				Debug.LogWarning("[PlayerEyeLookAt] Eye bones not assigned! Please assign leftEye and rightEye transforms.");
				base.enabled = false;
			}
		}

		// Token: 0x06009565 RID: 38245 RVA: 0x003D5210 File Offset: 0x003D3410
		private void LateUpdate()
		{
			if (!this._hasRestPose)
			{
				return;
			}
			if (Time.time >= this._nextScanTime)
			{
				this.ScanForNearestNPC();
				this._nextScanTime = Time.time + this.scanInterval;
			}
			if (this.enableSaccades && Time.time >= this._nextSaccadeTime)
			{
				this.UpdateSaccade();
				this._nextSaccadeTime = Time.time + this.saccadeInterval + Random.Range(-0.05f, 0.05f);
			}
			Vector3 vector = this.CalculateEyeOffset();
			if (this.enableSaccades)
			{
				vector += this._saccadeOffset * 0.001f;
			}
			float num = 1f - Mathf.Exp(-this.smoothing * Time.deltaTime);
			this._currentLeftOffset = Vector3.Lerp(this._currentLeftOffset, vector, num);
			this._currentRightOffset = Vector3.Lerp(this._currentRightOffset, vector, num);
			if (this._currentTarget == null)
			{
				if (this._currentLeftOffset.sqrMagnitude < 1E-07f)
				{
					this._currentLeftOffset = Vector3.zero;
				}
				if (this._currentRightOffset.sqrMagnitude < 1E-07f)
				{
					this._currentRightOffset = Vector3.zero;
				}
			}
			this.ApplyEyePositions();
		}

		// Token: 0x06009566 RID: 38246 RVA: 0x003D533C File Offset: 0x003D353C
		private void ScanForNearestNPC()
		{
			this._currentTarget = null;
			this._currentTargetHead = null;
			if (this._cachedNPCs == null || Time.time - this._npcCacheTime > 1f)
			{
				this._cachedNPCs = GameObject.FindGameObjectsWithTag(this.npcTag);
				this._npcCacheTime = Time.time;
				if (this.verboseScanLog)
				{
					Debug.Log(string.Format("[PlayerEyeLookAt] NPC cache refreshed: {0} NPCs with tag '{1}'", this._cachedNPCs.Length, this.npcTag));
				}
			}
			if (this._cachedNPCs == null || this._cachedNPCs.Length == 0)
			{
				if (this.verboseScanLog)
				{
					Debug.Log("[PlayerEyeLookAt] No NPCs found with tag '" + this.npcTag + "'");
				}
				return;
			}
			float num = float.MaxValue;
			Transform transform = null;
			Transform transform2 = null;
			Vector3 forward = this._playerTransform.forward;
			Vector3 position = this._playerTransform.position;
			float num2 = this.detectionFOV * 0.5f;
			for (int i = 0; i < this._cachedNPCs.Length; i++)
			{
				GameObject gameObject = this._cachedNPCs[i];
				if (!(gameObject == null) && gameObject.activeInHierarchy)
				{
					Transform transform3 = gameObject.transform;
					Vector3 vector = transform3.position - position;
					float magnitude = vector.magnitude;
					if (magnitude > this.detectionRange)
					{
						if (this.verboseScanLog)
						{
							Debug.Log(string.Format("[PlayerEyeLookAt] {0} out of range: {1:F1}m > {2}m", gameObject.name, magnitude, this.detectionRange));
						}
					}
					else
					{
						float num3 = Vector3.Angle(forward, vector);
						if (num3 > num2)
						{
							if (this.verboseScanLog)
							{
								Debug.Log(string.Format("[PlayerEyeLookAt] {0} out of FOV: {1:F1}° > {2}°", gameObject.name, num3, num2));
							}
						}
						else if (magnitude < num)
						{
							num = magnitude;
							transform = transform3;
							transform2 = this.FindHeadBone(transform3);
						}
					}
				}
			}
			if (transform != null)
			{
				this._currentTarget = transform;
				this._currentTargetHead = transform2;
				if (this.debugLog)
				{
					Debug.Log(string.Format("[PlayerEyeLookAt] Target: {0} at {1:F1}m", transform.name, num) + ((transform2 != null) ? (" (head: " + transform2.name + ")") : ""));
					return;
				}
			}
			else if (this.verboseScanLog)
			{
				Debug.Log(string.Format("[PlayerEyeLookAt] No valid NPC target found (checked {0} NPCs)", this._cachedNPCs.Length));
			}
		}

		// Token: 0x06009567 RID: 38247 RVA: 0x003D5598 File Offset: 0x003D3798
		private Transform FindHeadBone(Transform npcRoot)
		{
			Animator componentInChildren = npcRoot.GetComponentInChildren<Animator>();
			if (componentInChildren != null && componentInChildren.isHuman)
			{
				Transform boneTransform = componentInChildren.GetBoneTransform(HumanBodyBones.Head);
				if (boneTransform != null)
				{
					return boneTransform;
				}
			}
			return this.FindChildByName(npcRoot, "Head");
		}

		// Token: 0x06009568 RID: 38248 RVA: 0x003D55E0 File Offset: 0x003D37E0
		private Transform FindChildByName(Transform parent, string name)
		{
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				if (transform.name.Contains(name))
				{
					return transform;
				}
				Transform transform2 = this.FindChildByName(transform, name);
				if (transform2 != null)
				{
					return transform2;
				}
			}
			return null;
		}

		// Token: 0x06009569 RID: 38249 RVA: 0x003D565C File Offset: 0x003D385C
		private void CaptureRestPose()
		{
			if (this.leftEye != null)
			{
				this._leftEyeRestPosition = this.leftEye.localPosition;
			}
			if (this.rightEye != null)
			{
				this._rightEyeRestPosition = this.rightEye.localPosition;
			}
			this._hasRestPose = this.leftEye != null && this.rightEye != null;
			if (this.debugLog && this._hasRestPose)
			{
				Debug.Log(string.Format("[PlayerEyeLookAt] Rest pose captured - Left: {0}, Right: {1}", this._leftEyeRestPosition, this._rightEyeRestPosition));
			}
		}

		// Token: 0x0600956A RID: 38250 RVA: 0x003D5700 File Offset: 0x003D3900
		private Vector3 CalculateEyeOffset()
		{
			if (this._currentTarget == null)
			{
				return Vector3.zero;
			}
			Vector3 vector = ((this._currentTargetHead != null) ? this._currentTargetHead.position : (this._currentTarget.position + Vector3.up * 1.6f));
			Vector3 vector2 = (this.leftEye.position + this.rightEye.position) * 0.5f;
			Vector3 vector3 = vector - vector2;
			Vector3 vector4 = this._playerTransform.InverseTransformDirection(vector3.normalized);
			float num = Mathf.Atan2(vector4.x, vector4.z) / 1.5707964f;
			float num2 = vector4.y;
			num = Mathf.Clamp(num, -1f, 1f);
			num2 = Mathf.Clamp(num2, -1f, 1f);
			if (this.invertHorizontal)
			{
				num = -num;
			}
			if (this.invertVertical)
			{
				num2 = -num2;
			}
			float num3 = num * this.maxHorizontalDisplacement;
			float num4 = num2 * this.maxVerticalDisplacement;
			return new Vector3(num3, num4, 0f);
		}

		// Token: 0x0600956B RID: 38251 RVA: 0x003D5818 File Offset: 0x003D3A18
		private void UpdateSaccade()
		{
			this._saccadeOffset = new Vector3(Random.Range(-this.saccadeIntensity, this.saccadeIntensity), Random.Range(-this.saccadeIntensity * 0.5f, this.saccadeIntensity * 0.5f), 0f);
		}

		// Token: 0x0600956C RID: 38252 RVA: 0x003D5868 File Offset: 0x003D3A68
		private void ApplyEyePositions()
		{
			if (this.leftEye != null)
			{
				this.leftEye.localPosition = this._leftEyeRestPosition + this._currentLeftOffset;
			}
			if (this.rightEye != null)
			{
				this.rightEye.localPosition = this._rightEyeRestPosition + this._currentRightOffset;
			}
		}

		// Token: 0x0600956D RID: 38253 RVA: 0x003D58C9 File Offset: 0x003D3AC9
		public void ForceLookAt(Transform target)
		{
			this._currentTarget = target;
			this._currentTargetHead = this.FindHeadBone(target);
		}

		// Token: 0x0600956E RID: 38254 RVA: 0x003D58DF File Offset: 0x003D3ADF
		public void ClearForcedTarget()
		{
			this._currentTarget = null;
			this._currentTargetHead = null;
		}

		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x0600956F RID: 38255 RVA: 0x003D58EF File Offset: 0x003D3AEF
		public Transform CurrentTarget
		{
			get
			{
				return this._currentTarget;
			}
		}

		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x06009570 RID: 38256 RVA: 0x003D58F7 File Offset: 0x003D3AF7
		public bool IsLookingAtNPC
		{
			get
			{
				return this._currentTarget != null;
			}
		}

		// Token: 0x06009571 RID: 38257 RVA: 0x003D5908 File Offset: 0x003D3B08
		private void OnDrawGizmosSelected()
		{
			if (!this.drawGizmos)
			{
				return;
			}
			Vector3 position = base.transform.position;
			Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
			Gizmos.DrawWireSphere(position, this.detectionRange);
			Gizmos.color = Color.yellow;
			Vector3 forward = base.transform.forward;
			float num = this.detectionFOV * 0.5f;
			Vector3 vector = Quaternion.Euler(0f, -num, 0f) * forward;
			Vector3 vector2 = Quaternion.Euler(0f, num, 0f) * forward;
			Gizmos.DrawLine(position, position + vector * this.detectionRange);
			Gizmos.DrawLine(position, position + vector2 * this.detectionRange);
			if (this._currentTarget != null)
			{
				Gizmos.color = Color.red;
				Vector3 vector3 = ((this._currentTargetHead != null) ? this._currentTargetHead.position : this._currentTarget.position);
				Gizmos.DrawWireSphere(vector3, 0.2f);
				if (this.leftEye != null && this.rightEye != null)
				{
					Gizmos.color = Color.cyan;
					Gizmos.DrawLine(this.leftEye.position, vector3);
					Gizmos.DrawLine(this.rightEye.position, vector3);
				}
			}
		}

		// Token: 0x04009A78 RID: 39544
		[Header("Eye Bone References")]
		[Tooltip("Left eye bone transform")]
		[SerializeField]
		private Transform leftEye;

		// Token: 0x04009A79 RID: 39545
		[Tooltip("Right eye bone transform")]
		[SerializeField]
		private Transform rightEye;

		// Token: 0x04009A7A RID: 39546
		[Header("NPC Detection")]
		[Tooltip("Tag used to identify NPCs")]
		[SerializeField]
		private string npcTag = "NPC";

		// Token: 0x04009A7B RID: 39547
		[Tooltip("Maximum distance to detect NPCs")]
		[SerializeField]
		private float detectionRange = 10f;

		// Token: 0x04009A7C RID: 39548
		[Tooltip("Field of view angle for NPC detection (degrees)")]
		[Range(30f, 180f)]
		[SerializeField]
		private float detectionFOV = 120f;

		// Token: 0x04009A7D RID: 39549
		[Tooltip("How often to scan for nearest NPC (seconds)")]
		[SerializeField]
		private float scanInterval = 0.2f;

		// Token: 0x04009A7E RID: 39550
		[Header("Eye Movement Settings")]
		[Tooltip("Maximum horizontal eye displacement (local X axis)")]
		[SerializeField]
		private float maxHorizontalDisplacement = 0.003f;

		// Token: 0x04009A7F RID: 39551
		[Tooltip("Maximum vertical eye displacement (local Y axis)")]
		[SerializeField]
		private float maxVerticalDisplacement = 0.002f;

		// Token: 0x04009A80 RID: 39552
		[Tooltip("Invert horizontal movement direction")]
		[SerializeField]
		private bool invertHorizontal;

		// Token: 0x04009A81 RID: 39553
		[Tooltip("Invert vertical movement direction")]
		[SerializeField]
		private bool invertVertical;

		// Token: 0x04009A82 RID: 39554
		[Header("Saccadic Movement")]
		[Tooltip("Enable micro-movements for natural eye behavior")]
		[SerializeField]
		private bool enableSaccades = true;

		// Token: 0x04009A83 RID: 39555
		[Tooltip("Intensity of saccadic micro-movements")]
		[Range(0f, 2f)]
		[SerializeField]
		private float saccadeIntensity = 0.5f;

		// Token: 0x04009A84 RID: 39556
		[Tooltip("How often saccades occur (seconds)")]
		[SerializeField]
		private float saccadeInterval = 0.15f;

		// Token: 0x04009A85 RID: 39557
		[Header("Smoothing")]
		[Tooltip("Smoothing factor for eye movement (higher = smoother)")]
		[Range(5f, 25f)]
		[SerializeField]
		private float smoothing = 12f;

		// Token: 0x04009A86 RID: 39558
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04009A87 RID: 39559
		[SerializeField]
		private bool drawGizmos;

		// Token: 0x04009A88 RID: 39560
		[Tooltip("Log scan results every interval (verbose)")]
		[SerializeField]
		private bool verboseScanLog;

		// Token: 0x04009A89 RID: 39561
		private Transform _currentTarget;

		// Token: 0x04009A8A RID: 39562
		private Transform _currentTargetHead;

		// Token: 0x04009A8B RID: 39563
		private Vector3 _targetLookOffset;

		// Token: 0x04009A8C RID: 39564
		private Vector3 _currentLeftOffset;

		// Token: 0x04009A8D RID: 39565
		private Vector3 _currentRightOffset;

		// Token: 0x04009A8E RID: 39566
		private Vector3 _leftEyeRestPosition;

		// Token: 0x04009A8F RID: 39567
		private Vector3 _rightEyeRestPosition;

		// Token: 0x04009A90 RID: 39568
		private bool _hasRestPose;

		// Token: 0x04009A91 RID: 39569
		private Vector3 _saccadeOffset;

		// Token: 0x04009A92 RID: 39570
		private float _nextSaccadeTime;

		// Token: 0x04009A93 RID: 39571
		private float _nextScanTime;

		// Token: 0x04009A94 RID: 39572
		private GameObject[] _cachedNPCs;

		// Token: 0x04009A95 RID: 39573
		private float _npcCacheTime;

		// Token: 0x04009A96 RID: 39574
		private Transform _playerTransform;
	}
}
