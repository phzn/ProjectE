using System;
using System.Collections.Generic;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000019 RID: 25
	public class RollerSkatesAnchorProvider : MonoBehaviour, IVehicleAnchorProvider
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00006F09 File Offset: 0x00005109
		public Transform LeftSkateAttach
		{
			get
			{
				return this.leftSkateAttach;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00006F11 File Offset: 0x00005111
		public Transform RightSkateAttach
		{
			get
			{
				return this.rightSkateAttach;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00006F19 File Offset: 0x00005119
		public Transform PelvisReference
		{
			get
			{
				return this.pelvisReference;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00006F21 File Offset: 0x00005121
		public Transform SpineReference
		{
			get
			{
				return this.spineReference;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006F29 File Offset: 0x00005129
		private void Awake()
		{
			this.speedProvider = base.GetComponent<IVehicleSpeedProvider>();
			this.ValidateSetup();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00006F40 File Offset: 0x00005140
		private void ValidateSetup()
		{
			if (this.leftSkateAttach == null)
			{
				Debug.LogWarning("[RollerSkatesAnchorProvider] Left skate attach point not assigned!");
			}
			if (this.rightSkateAttach == null)
			{
				Debug.LogWarning("[RollerSkatesAnchorProvider] Right skate attach point not assigned!");
			}
			if (this.pelvisReference == null)
			{
				Debug.LogWarning("[RollerSkatesAnchorProvider] Pelvis reference not assigned!");
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006F98 File Offset: 0x00005198
		public Transform GetAnchor(VehicleAnchorType type)
		{
			Transform transform;
			switch (type)
			{
			case VehicleAnchorType.Seat:
				transform = this.pelvisReference;
				break;
			case VehicleAnchorType.LeftHandGrip:
				transform = this.leftHandReference;
				break;
			case VehicleAnchorType.RightHandGrip:
				transform = this.rightHandReference;
				break;
			case VehicleAnchorType.LeftFootRest:
				transform = this.leftSkateAttach;
				break;
			case VehicleAnchorType.RightFootRest:
				transform = this.rightSkateAttach;
				break;
			case VehicleAnchorType.SpineAimReference:
				transform = this.spineReference;
				break;
			case VehicleAnchorType.PelvisReference:
				transform = this.pelvisReference;
				break;
			default:
				transform = null;
				break;
			}
			return transform;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000700B File Offset: 0x0000520B
		public bool HasAnchor(VehicleAnchorType type)
		{
			return this.GetAnchor(type) != null;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x0000701A File Offset: 0x0000521A
		public float CurrentSpeed
		{
			get
			{
				if (this.speedProvider == null)
				{
					return this.manualSpeed;
				}
				return Mathf.Abs(this.speedProvider.GetCurrentSpeed());
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000703B File Offset: 0x0000523B
		public void SetSpeed(float speed)
		{
			this.manualSpeed = speed;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00007044 File Offset: 0x00005244
		public Transform VehicleTransform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x0000704C File Offset: 0x0000524C
		public bool UsesInvertedHierarchy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00007050 File Offset: 0x00005250
		public void AttachSkateVisuals(Transform leftFootBone, Transform rightFootBone)
		{
			if (this.leftSkatePrefab != null && leftFootBone != null)
			{
				this.leftSkateInstance = Object.Instantiate<GameObject>(this.leftSkatePrefab, leftFootBone);
				this.leftSkateInstance.transform.localPosition = Vector3.zero;
				this.leftSkateInstance.transform.localRotation = Quaternion.identity;
				this.leftWheelAxles = this.FindWheelAxles(this.leftSkateInstance);
			}
			if (this.rightSkatePrefab != null && rightFootBone != null)
			{
				this.rightSkateInstance = Object.Instantiate<GameObject>(this.rightSkatePrefab, rightFootBone);
				this.rightSkateInstance.transform.localPosition = Vector3.zero;
				this.rightSkateInstance.transform.localRotation = Quaternion.identity;
				this.rightWheelAxles = this.FindWheelAxles(this.rightSkateInstance);
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00007128 File Offset: 0x00005328
		public void DetachSkateVisuals()
		{
			if (this.leftSkateInstance != null)
			{
				Object.Destroy(this.leftSkateInstance);
				this.leftSkateInstance = null;
			}
			if (this.rightSkateInstance != null)
			{
				Object.Destroy(this.rightSkateInstance);
				this.rightSkateInstance = null;
			}
			this.leftWheelAxles = null;
			this.rightWheelAxles = null;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00007184 File Offset: 0x00005384
		public void UpdateWheelRotation(float speed)
		{
			if (speed < 0.1f)
			{
				return;
			}
			float num = 0.02f;
			float num2 = speed / num * 57.29578f * Time.deltaTime;
			this.RotateWheels(this.leftWheelAxles, num2);
			this.RotateWheels(this.rightWheelAxles, num2);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000071CC File Offset: 0x000053CC
		private void RotateWheels(Transform[] axles, float rotationAmount)
		{
			if (axles == null)
			{
				return;
			}
			foreach (Transform transform in axles)
			{
				if (transform != null)
				{
					transform.Rotate(Vector3.right, rotationAmount, Space.Self);
				}
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00007208 File Offset: 0x00005408
		private Transform[] FindWheelAxles(GameObject skateInstance)
		{
			List<Transform> list = new List<Transform>();
			foreach (Transform transform in skateInstance.GetComponentsInChildren<Transform>())
			{
				if ((transform.name.Contains("Axle") || transform.name.Contains("axle") || transform.name.Contains("Wheel") || transform.name.Contains("wheel")) && (transform.childCount > 0 || transform.name.Contains("Axle")))
				{
					list.Add(transform);
				}
			}
			if (list.Count <= 0)
			{
				return null;
			}
			return list.ToArray();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000072AF File Offset: 0x000054AF
		public bool AreSkatesAttached()
		{
			return this.leftSkateInstance != null || this.rightSkateInstance != null;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000072D0 File Offset: 0x000054D0
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			if (this.leftSkateAttach != null)
			{
				Gizmos.DrawWireSphere(this.leftSkateAttach.position, 0.05f);
				Gizmos.DrawLine(this.leftSkateAttach.position, this.leftSkateAttach.position + this.leftSkateAttach.forward * 0.15f);
			}
			Gizmos.color = Color.blue;
			if (this.rightSkateAttach != null)
			{
				Gizmos.DrawWireSphere(this.rightSkateAttach.position, 0.05f);
				Gizmos.DrawLine(this.rightSkateAttach.position, this.rightSkateAttach.position + this.rightSkateAttach.forward * 0.15f);
			}
			Gizmos.color = Color.magenta;
			if (this.pelvisReference != null)
			{
				Gizmos.DrawWireSphere(this.pelvisReference.position, 0.1f);
			}
			Gizmos.color = Color.yellow;
			if (this.spineReference != null)
			{
				Gizmos.DrawWireSphere(this.spineReference.position, 0.08f);
				Gizmos.DrawRay(this.spineReference.position, this.spineReference.forward * 0.2f);
			}
			Gizmos.color = Color.green;
			if (this.leftHandReference != null)
			{
				Gizmos.DrawWireSphere(this.leftHandReference.position, 0.04f);
			}
			if (this.rightHandReference != null)
			{
				Gizmos.DrawWireSphere(this.rightHandReference.position, 0.04f);
			}
		}

		// Token: 0x040000E6 RID: 230
		[Header("Skate Attachment Points")]
		[Tooltip("Transform where the left skate attaches (should be at foot bone level)")]
		[SerializeField]
		private Transform leftSkateAttach;

		// Token: 0x040000E7 RID: 231
		[Tooltip("Transform where the right skate attaches (should be at foot bone level)")]
		[SerializeField]
		private Transform rightSkateAttach;

		// Token: 0x040000E8 RID: 232
		[Header("Body Reference Points")]
		[Tooltip("Reference for pelvis positioning (standing skating pose)")]
		[SerializeField]
		private Transform pelvisReference;

		// Token: 0x040000E9 RID: 233
		[Tooltip("Reference for spine/upper body lean during turns")]
		[SerializeField]
		private Transform spineReference;

		// Token: 0x040000EA RID: 234
		[Header("Hand Positions (Optional)")]
		[Tooltip("Left hand position for arm swing animation")]
		[SerializeField]
		private Transform leftHandReference;

		// Token: 0x040000EB RID: 235
		[Tooltip("Right hand position for arm swing animation")]
		[SerializeField]
		private Transform rightHandReference;

		// Token: 0x040000EC RID: 236
		[Header("Visual Skate Prefabs")]
		[Tooltip("Prefab for left skate visual")]
		[SerializeField]
		private GameObject leftSkatePrefab;

		// Token: 0x040000ED RID: 237
		[Tooltip("Prefab for right skate visual")]
		[SerializeField]
		private GameObject rightSkatePrefab;

		// Token: 0x040000EE RID: 238
		private GameObject leftSkateInstance;

		// Token: 0x040000EF RID: 239
		private GameObject rightSkateInstance;

		// Token: 0x040000F0 RID: 240
		private IVehicleSpeedProvider speedProvider;

		// Token: 0x040000F1 RID: 241
		private float manualSpeed;

		// Token: 0x040000F2 RID: 242
		private Transform[] leftWheelAxles;

		// Token: 0x040000F3 RID: 243
		private Transform[] rightWheelAxles;
	}
}
