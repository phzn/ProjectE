using System;
using System.Collections.Generic;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200001C RID: 28
	public class SimpleRollerSkatesController : MonoBehaviour
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00007C55 File Offset: 0x00005E55
		public float CurrentSpeed
		{
			get
			{
				return this.currentSpeed;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00007C5D File Offset: 0x00005E5D
		public bool IsActive
		{
			get
			{
				return this.isActive;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00007C65 File Offset: 0x00005E65
		public bool IsGrounded
		{
			get
			{
				return this.isGrounded;
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00007C6D File Offset: 0x00005E6D
		private void Update()
		{
			if (!this.isActive || this.riderTransform == null)
			{
				return;
			}
			this.CheckGround();
			this.HandleMovement();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00007C94 File Offset: 0x00005E94
		private void CheckGround()
		{
			this.isGrounded = Physics.Raycast(this.riderTransform.position + Vector3.up * 0.1f, Vector3.down, this.groundCheckDistance + 0.1f, this.groundLayer);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00007CE8 File Offset: 0x00005EE8
		private void HandleMovement()
		{
			if (!this.isGrounded)
			{
				return;
			}
			if (this.brakeInput)
			{
				this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, 0f, this.brakePower * Time.deltaTime);
			}
			else if (this.verticalInput > 0.1f)
			{
				this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, this.maxSpeed * this.verticalInput, this.acceleration * Time.deltaTime);
			}
			else if (this.verticalInput < -0.1f)
			{
				this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, this.maxSpeed * 0.3f * this.verticalInput, this.acceleration * Time.deltaTime);
			}
			else
			{
				this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, 0f, this.deceleration * Time.deltaTime);
			}
			if (Mathf.Abs(this.horizontalInput) > 0.05f)
			{
				float num = Mathf.Abs(this.currentSpeed);
				float num2;
				if (num < this.movementThreshold)
				{
					num2 = this.horizontalInput * this.stationaryTurnSpeed * Time.deltaTime;
				}
				else
				{
					float num3 = Mathf.Lerp(1f, 1f - this.highSpeedTurnReduction, num / this.maxSpeed);
					num2 = this.horizontalInput * this.movingTurnSpeed * num3 * Time.deltaTime;
				}
				this.riderTransform.Rotate(0f, num2, 0f);
			}
			Vector3 vector = this.riderTransform.forward * this.currentSpeed;
			if (this.riderCharacterController != null)
			{
				vector.y = -9.81f;
				this.riderCharacterController.Move(vector * Time.deltaTime);
				return;
			}
			if (this.riderRigidbody != null)
			{
				this.riderRigidbody.velocity = new Vector3(vector.x, this.riderRigidbody.velocity.y, vector.z);
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00007ED4 File Offset: 0x000060D4
		public void Activate(Transform rider, Animator riderAnimator)
		{
			if (rider == null)
			{
				Debug.LogError("[SimpleRollerSkatesController] Cannot activate - rider is null");
				return;
			}
			this.riderTransform = rider;
			this.riderCharacterController = rider.GetComponent<CharacterController>();
			this.riderRigidbody = rider.GetComponent<Rigidbody>();
			if (riderAnimator != null)
			{
				this.leftFootBone = riderAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
				this.rightFootBone = riderAnimator.GetBoneTransform(HumanBodyBones.RightFoot);
			}
			this.AttachSkateVisuals();
			this.HideObjects();
			this.ApplyBlendShapeOverrides();
			this.currentSpeed = 0f;
			this.isActive = true;
			Debug.Log("[SimpleRollerSkatesController] Activated on " + rider.name);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00007F70 File Offset: 0x00006170
		public void Deactivate()
		{
			this.DetachSkateVisuals();
			this.RestoreObjects();
			this.RestoreBlendShapes();
			this.riderTransform = null;
			this.riderCharacterController = null;
			this.riderRigidbody = null;
			this.leftFootBone = null;
			this.rightFootBone = null;
			this.currentSpeed = 0f;
			this.isActive = false;
			Debug.Log("[SimpleRollerSkatesController] Deactivated");
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00007FCE File Offset: 0x000061CE
		public void SetInput(float vertical, float horizontal, bool brake)
		{
			this.verticalInput = vertical;
			this.horizontalInput = horizontal;
			this.brakeInput = brake;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00007FE8 File Offset: 0x000061E8
		private void AttachSkateVisuals()
		{
			if (this.leftSkatePrefab != null && this.leftFootBone != null)
			{
				this.leftSkateInstance = Object.Instantiate<GameObject>(this.leftSkatePrefab, this.leftFootBone);
				this.leftSkateInstance.transform.localPosition = this.skateOffset;
				this.leftSkateInstance.transform.localRotation = Quaternion.Euler(this.skateRotationOffset);
				Debug.Log("[SimpleRollerSkatesController] Left skate attached");
			}
			if (this.rightSkatePrefab != null && this.rightFootBone != null)
			{
				this.rightSkateInstance = Object.Instantiate<GameObject>(this.rightSkatePrefab, this.rightFootBone);
				this.rightSkateInstance.transform.localPosition = this.skateOffset;
				this.rightSkateInstance.transform.localRotation = Quaternion.Euler(this.skateRotationOffset);
				Debug.Log("[SimpleRollerSkatesController] Right skate attached");
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000080D4 File Offset: 0x000062D4
		private void DetachSkateVisuals()
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
			Debug.Log("[SimpleRollerSkatesController] Skate visuals detached");
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000812C File Offset: 0x0000632C
		private void HideObjects()
		{
			this.originalActiveStates.Clear();
			foreach (GameObject gameObject in this.objectsToHide)
			{
				if (gameObject != null)
				{
					this.originalActiveStates[gameObject] = gameObject.activeSelf;
					gameObject.SetActive(false);
				}
			}
			if (this.objectsToHide.Count > 0)
			{
				Debug.Log(string.Format("[SimpleRollerSkatesController] Hidden {0} objects", this.objectsToHide.Count));
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000081D4 File Offset: 0x000063D4
		private void RestoreObjects()
		{
			foreach (KeyValuePair<GameObject, bool> keyValuePair in this.originalActiveStates)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.SetActive(keyValuePair.Value);
				}
			}
			this.originalActiveStates.Clear();
			Debug.Log("[SimpleRollerSkatesController] Restored hidden objects");
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00008258 File Offset: 0x00006458
		private void ApplyBlendShapeOverrides()
		{
			if (this.targetMeshRenderer == null || this.blendShapeOverrides.Count == 0)
			{
				return;
			}
			this.originalBlendShapeValues.Clear();
			Mesh sharedMesh = this.targetMeshRenderer.sharedMesh;
			foreach (BlendShapeOverride blendShapeOverride in this.blendShapeOverrides)
			{
				if (!string.IsNullOrEmpty(blendShapeOverride.blendShapeName))
				{
					int blendShapeIndex = sharedMesh.GetBlendShapeIndex(blendShapeOverride.blendShapeName);
					if (blendShapeIndex >= 0)
					{
						this.originalBlendShapeValues[blendShapeIndex] = this.targetMeshRenderer.GetBlendShapeWeight(blendShapeIndex);
						this.targetMeshRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeOverride.targetValue);
					}
					else
					{
						Debug.LogWarning("[SimpleRollerSkatesController] BlendShape '" + blendShapeOverride.blendShapeName + "' not found");
					}
				}
			}
			Debug.Log(string.Format("[SimpleRollerSkatesController] Applied {0} blend shape overrides", this.originalBlendShapeValues.Count));
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000835C File Offset: 0x0000655C
		private void RestoreBlendShapes()
		{
			if (this.targetMeshRenderer == null)
			{
				return;
			}
			foreach (KeyValuePair<int, float> keyValuePair in this.originalBlendShapeValues)
			{
				this.targetMeshRenderer.SetBlendShapeWeight(keyValuePair.Key, keyValuePair.Value);
			}
			this.originalBlendShapeValues.Clear();
			Debug.Log("[SimpleRollerSkatesController] Restored blend shapes");
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000083E8 File Offset: 0x000065E8
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = (this.isGrounded ? Color.green : Color.red);
			Vector3 vector = base.transform.position + Vector3.up * 0.1f;
			Gizmos.DrawLine(vector, vector + Vector3.down * (this.groundCheckDistance + 0.1f));
		}

		// Token: 0x04000116 RID: 278
		[Header("Movement Settings")]
		[SerializeField]
		private float maxSpeed = 12f;

		// Token: 0x04000117 RID: 279
		[SerializeField]
		private float acceleration = 8f;

		// Token: 0x04000118 RID: 280
		[SerializeField]
		private float deceleration = 5f;

		// Token: 0x04000119 RID: 281
		[SerializeField]
		private float brakePower = 15f;

		// Token: 0x0400011A RID: 282
		[Header("Rotation Settings")]
		[Tooltip("Velocità di rotazione quando fermo (gradi/secondo)")]
		[SerializeField]
		private float stationaryTurnSpeed = 280f;

		// Token: 0x0400011B RID: 283
		[Tooltip("Velocità di rotazione in movimento (gradi/secondo)")]
		[SerializeField]
		private float movingTurnSpeed = 150f;

		// Token: 0x0400011C RID: 284
		[Tooltip("Velocità minima per considerare il personaggio in movimento")]
		[SerializeField]
		private float movementThreshold = 0.5f;

		// Token: 0x0400011D RID: 285
		[Tooltip("Quanto ridurre la rotazione alla velocità massima (0 = nessuna riduzione, 1 = rotazione zero)")]
		[Range(0f, 0.8f)]
		[SerializeField]
		private float highSpeedTurnReduction = 0.4f;

		// Token: 0x0400011E RID: 286
		[Header("Skate Visuals")]
		[SerializeField]
		private GameObject leftSkatePrefab;

		// Token: 0x0400011F RID: 287
		[SerializeField]
		private GameObject rightSkatePrefab;

		// Token: 0x04000120 RID: 288
		[SerializeField]
		private Vector3 skateOffset = new Vector3(0f, -0.05f, 0f);

		// Token: 0x04000121 RID: 289
		[SerializeField]
		private Vector3 skateRotationOffset = Vector3.zero;

		// Token: 0x04000122 RID: 290
		[Header("Ground Check")]
		[SerializeField]
		private float groundCheckDistance = 0.3f;

		// Token: 0x04000123 RID: 291
		[SerializeField]
		private LayerMask groundLayer = -1;

		// Token: 0x04000124 RID: 292
		[Header("Visual Toggles")]
		[Tooltip("GameObject da disattivare quando si indossano i pattini (es. scarpe)")]
		[SerializeField]
		private List<GameObject> objectsToHide = new List<GameObject>();

		// Token: 0x04000125 RID: 293
		[Header("Blend Shapes")]
		[Tooltip("SkinnedMeshRenderer che contiene i blend shapes")]
		[SerializeField]
		private SkinnedMeshRenderer targetMeshRenderer;

		// Token: 0x04000126 RID: 294
		[Tooltip("Blend shapes da modificare quando si indossano i pattini")]
		[SerializeField]
		private List<BlendShapeOverride> blendShapeOverrides = new List<BlendShapeOverride>();

		// Token: 0x04000127 RID: 295
		private float currentSpeed;

		// Token: 0x04000128 RID: 296
		private bool isActive;

		// Token: 0x04000129 RID: 297
		private bool isGrounded;

		// Token: 0x0400012A RID: 298
		private Transform riderTransform;

		// Token: 0x0400012B RID: 299
		private CharacterController riderCharacterController;

		// Token: 0x0400012C RID: 300
		private Rigidbody riderRigidbody;

		// Token: 0x0400012D RID: 301
		private GameObject leftSkateInstance;

		// Token: 0x0400012E RID: 302
		private GameObject rightSkateInstance;

		// Token: 0x0400012F RID: 303
		private Transform leftFootBone;

		// Token: 0x04000130 RID: 304
		private Transform rightFootBone;

		// Token: 0x04000131 RID: 305
		private Dictionary<GameObject, bool> originalActiveStates = new Dictionary<GameObject, bool>();

		// Token: 0x04000132 RID: 306
		private Dictionary<int, float> originalBlendShapeValues = new Dictionary<int, float>();

		// Token: 0x04000133 RID: 307
		private float verticalInput;

		// Token: 0x04000134 RID: 308
		private float horizontalInput;

		// Token: 0x04000135 RID: 309
		private bool brakeInput;
	}
}
