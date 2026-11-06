using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001B5 RID: 437
	public class PegasusFollow : MonoBehaviour
	{
		// Token: 0x06000CDF RID: 3295 RVA: 0x0005811C File Offset: 0x0005631C
		private void Start()
		{
			if (this.m_target == null)
			{
				return;
			}
			this.m_updateStopDistance = false;
			this.m_targetPosition = this.m_target.position;
			this.m_distanceToTarget = (this.m_targetPosition - base.transform.position).magnitude;
			this.m_currentStopDistance = Random.Range(this.m_stopDistanceMin, this.m_stopDistanceMax);
			this.m_currentCollisionCount = 0;
			this.m_currentCollisionHitArray = new RaycastHit[20];
			if (this.m_avoidTreeCollisions)
			{
				this.m_terrainTreeManager = new TreeManager();
				this.m_terrainTreeManager.LoadTreesFromTerrain();
			}
			foreach (Collider collider in base.GetComponentsInChildren<Collider>())
			{
				this.m_myColliders.Add(collider.GetInstanceID(), collider);
			}
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x000581E8 File Offset: 0x000563E8
		private void Update()
		{
			if (this.m_target == null)
			{
				return;
			}
			Vector3 vector = this.GetTargetPositionWithNoise(this.m_target.position);
			vector = this.GetTargetPositionWithCollisions(vector);
			if (this.m_target.position == this.m_targetPosition && this.m_distanceToTarget <= this.m_currentStopDistance)
			{
				return;
			}
			this.m_targetPosition = vector;
			Vector3 vector2 = this.m_targetPosition - base.transform.position;
			this.m_distanceToTarget = vector2.magnitude;
			if (this.m_distanceToTarget > this.m_currentStopDistance)
			{
				this.m_updateStopDistance = true;
				Vector3 vector3 = new Vector3(vector2.x, 0f, vector2.z);
				if (this.m_deviationRate > 0f && this.m_maxDeviationY > 0f)
				{
					vector3 = new Vector3(vector2.x, vector2.y, vector2.z);
				}
				if (vector3 == Vector3.zero)
				{
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.identity, this.m_turnChange * Time.deltaTime);
				}
				else
				{
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(vector3), this.m_turnChange * Time.deltaTime);
				}
				float num = Time.deltaTime * this.m_walkSpeed;
				if (this.m_distanceToTarget > this.m_runIfFurtherThan)
				{
					num = Time.deltaTime * this.m_runSpeed;
				}
				this.m_currentMovementDistance = Mathf.Lerp(this.m_currentMovementDistance, num, this.m_movementChange * Time.deltaTime);
				this.m_currentVelocity = base.transform.forward * this.m_currentMovementDistance;
				base.transform.position += this.m_currentVelocity;
				this.m_currentVelocity *= 1f / Time.deltaTime;
				return;
			}
			this.m_currentMovementDistance = 0f;
			this.m_currentVelocity = Vector3.zero;
			if (this.m_updateStopDistance)
			{
				this.m_updateStopDistance = false;
				this.m_currentStopDistance = Random.Range(this.m_stopDistanceMin, this.m_stopDistanceMax);
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00058414 File Offset: 0x00056614
		private void LateUpdate()
		{
			if (this.m_target == null)
			{
				return;
			}
			if (this.m_groundToTerrain || this.m_rotateToTerrain)
			{
				Vector3 position = base.transform.position;
				Terrain terrain = this.GetTerrain(position);
				if (terrain == null)
				{
					return;
				}
				position.y = terrain.SampleHeight(position);
				if (this.m_groundToTerrain)
				{
					base.transform.position = position;
				}
				if (this.m_rotateToTerrain)
				{
					Vector3 vector = terrain.transform.InverseTransformPoint(position);
					Vector3 vector2 = new Vector3(Mathf.InverseLerp(0f, terrain.terrainData.size.x, vector.x), Mathf.InverseLerp(0f, terrain.terrainData.size.y, vector.y), Mathf.InverseLerp(0f, terrain.terrainData.size.z, vector.z));
					Vector3 interpolatedNormal = terrain.terrainData.GetInterpolatedNormal(vector2.x, vector2.z);
					base.transform.rotation = Quaternion.FromToRotation(base.transform.up, interpolatedNormal) * base.transform.rotation;
				}
			}
			if (this.m_avoidObjectCollisions)
			{
				this.m_currentCollisionCount = Physics.SphereCastNonAlloc(base.transform.position, this.m_collisionRange, base.transform.forward, this.m_currentCollisionHitArray, 0f, this.m_objectCollisionLayer, QueryTriggerInteraction.UseGlobal);
			}
			if (this.m_avoidTreeCollisions)
			{
				if (this.m_terrainTreeManager == null)
				{
					this.m_terrainTreeManager = new TreeManager();
					this.m_terrainTreeManager.LoadTreesFromTerrain();
				}
				this.m_currentTreeCollisionCount = this.m_terrainTreeManager.GetTrees(base.transform.position, this.m_collisionRange + this.m_collisionRange / 2f, ref this.m_currentTreeCollisionHitArray);
			}
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x000585EC File Offset: 0x000567EC
		private Vector3 GetTargetPositionWithNoise(Vector3 targetPosition)
		{
			if (this.m_deviationRate > 0f)
			{
				float num = (-0.5f + Mathf.PerlinNoise(targetPosition.x * this.m_deviationRate, targetPosition.z * this.m_deviationRate)) * 2f;
				Vector3 vector = new Vector3(num * this.m_maxDeviationX, num * this.m_maxDeviationY, num * this.m_maxDeviationZ);
				targetPosition += vector;
			}
			return targetPosition;
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0005865C File Offset: 0x0005685C
		private Vector3 GetTargetPositionWithCollisions(Vector3 targetPosition)
		{
			if (this.m_currentCollisionCount == 0 && this.m_currentTreeCollisionCount == 0)
			{
				return targetPosition;
			}
			float num = this.m_collisionRange * this.m_collisionRange;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = base.transform.position;
			vector2 += this.m_currentVelocity * 0.5f;
			for (int i = 0; i < this.m_currentCollisionCount; i++)
			{
				if (!this.m_myColliders.ContainsKey(this.m_currentCollisionHitArray[i].collider.GetInstanceID()))
				{
					Vector3 vector3 = this.m_currentCollisionHitArray[i].transform.position - vector2;
					RaycastHit raycastHit;
					if (Physics.Raycast(vector2, vector3, out raycastHit, this.m_collisionRange, this.m_objectCollisionLayer))
					{
						Vector3 vector4 = vector2 - raycastHit.point;
						float num2 = vector4.sqrMagnitude;
						float num3 = Mathf.Abs(1f - num2 / num);
						if (num3 > 0f)
						{
							float num4 = num3 * (num / num2);
							vector4 *= num4;
							vector += vector4;
						}
					}
				}
			}
			for (int j = 0; j < this.m_currentTreeCollisionCount; j++)
			{
				Vector3 vector4 = vector2 - this.m_currentTreeCollisionHitArray[j].position;
				float num2 = vector4.sqrMagnitude - 0.0625f;
				float num3 = Mathf.Abs(1f - num2 / num);
				if (num3 > 0f)
				{
					float num4 = num3 * (num / num2);
					vector4 *= num4;
					vector += vector4;
				}
			}
			return targetPosition + vector;
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00058810 File Offset: 0x00056A10
		private Terrain GetTerrain(Vector3 locationWU)
		{
			Vector3 vector = default(Vector3);
			Vector3 vector2 = default(Vector3);
			Terrain terrain = Terrain.activeTerrain;
			if (terrain != null)
			{
				vector = terrain.GetPosition();
				vector2 = vector + terrain.terrainData.size;
				if (locationWU.x >= vector.x && locationWU.x <= vector2.x && locationWU.z >= vector.z && locationWU.z <= vector2.z)
				{
					return terrain;
				}
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				terrain = Terrain.activeTerrains[i];
				vector = terrain.GetPosition();
				vector2 = vector + terrain.terrainData.size;
				if (locationWU.x >= vector.x && locationWU.x <= vector2.x && locationWU.z >= vector.z && locationWU.z <= vector2.z)
				{
					return terrain;
				}
			}
			return null;
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x000588FD File Offset: 0x00056AFD
		private void OnDrawGizmos()
		{
			this.DrawGizmos(false);
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00058906 File Offset: 0x00056B06
		private void OnDrawGizmosSelected()
		{
			this.DrawGizmos(true);
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00058910 File Offset: 0x00056B10
		private void DrawGizmos(bool isSelected)
		{
			if (!isSelected && !this.m_alwaysShowGizmos)
			{
				return;
			}
			Color color = Gizmos.color;
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(base.transform.position, this.m_targetPosition);
			if (this.m_avoidObjectCollisions || this.m_avoidTreeCollisions)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireSphere(base.transform.position, this.m_collisionRange);
				Gizmos.color = Color.red;
				for (int i = 0; i < this.m_currentCollisionCount; i++)
				{
					Gizmos.DrawLine(base.transform.position, this.m_currentCollisionHitArray[i].transform.position);
				}
				for (int j = 0; j < this.m_currentTreeCollisionCount; j++)
				{
					Gizmos.DrawLine(base.transform.position, this.m_currentTreeCollisionHitArray[j].position);
				}
			}
			Gizmos.color = color;
		}

		// Token: 0x04000D2C RID: 3372
		public Transform m_target;

		// Token: 0x04000D2D RID: 3373
		[Header("Local Overrides")]
		[Tooltip("Always show gizmo's, even when not selected")]
		public bool m_alwaysShowGizmos;

		// Token: 0x04000D2E RID: 3374
		[Tooltip("Ground character to terrain")]
		public bool m_groundToTerrain = true;

		// Token: 0x04000D2F RID: 3375
		[Tooltip("Rotate character to terrain. Best for quadrapeds.")]
		public bool m_rotateToTerrain;

		// Token: 0x04000D30 RID: 3376
		[Tooltip("Avoid collisions with terrain trees.")]
		public bool m_avoidTreeCollisions;

		// Token: 0x04000D31 RID: 3377
		[Tooltip("Avoid collisions with objects on the object collision layer.")]
		public bool m_avoidObjectCollisions;

		// Token: 0x04000D32 RID: 3378
		[Tooltip("Distance to check for collisions. Bigger values will slow your system down.")]
		[Range(1f, 10f)]
		public float m_collisionRange = 2f;

		// Token: 0x04000D33 RID: 3379
		[Tooltip("Layers to check for collisions on. Ensure that your terrain is NOT included in these layers.")]
		public LayerMask m_objectCollisionLayer;

		// Token: 0x04000D34 RID: 3380
		private int m_currentCollisionCount;

		// Token: 0x04000D35 RID: 3381
		private RaycastHit[] m_currentCollisionHitArray;

		// Token: 0x04000D36 RID: 3382
		private int m_currentTreeCollisionCount;

		// Token: 0x04000D37 RID: 3383
		private List<TreeManager.TreeStruct> m_currentTreeCollisionHitArray = new List<TreeManager.TreeStruct>();

		// Token: 0x04000D38 RID: 3384
		private TreeManager m_terrainTreeManager;

		// Token: 0x04000D39 RID: 3385
		private Dictionary<int, Collider> m_myColliders = new Dictionary<int, Collider>();

		// Token: 0x04000D3A RID: 3386
		[Header("Character Speeds")]
		[Tooltip("Walk speed")]
		public float m_walkSpeed = 2f;

		// Token: 0x04000D3B RID: 3387
		[Tooltip("Run speed")]
		public float m_runSpeed = 7f;

		// Token: 0x04000D3C RID: 3388
		[Header("Distance Thresholds")]
		[Tooltip("Minimum distance from target that character can stop.")]
		public float m_stopDistanceMin = 0.05f;

		// Token: 0x04000D3D RID: 3389
		[Tooltip("Maximum distance from target that character can stop.")]
		public float m_stopDistanceMax = 0.05f;

		// Token: 0x04000D3E RID: 3390
		private float m_currentStopDistance = 0.05f;

		// Token: 0x04000D3F RID: 3391
		private bool m_updateStopDistance;

		// Token: 0x04000D40 RID: 3392
		[Tooltip("Character will run if further away from target than this distance, otherwise will walk or is stopped.")]
		public float m_runIfFurtherThan = 15f;

		// Token: 0x04000D41 RID: 3393
		[Header("Change Rates")]
		[Range(0.001f, 3f)]
		[Tooltip("Turn rate. Lower values produce smoother turns, larger values produce more accurate turns.")]
		public float m_turnChange = 1.5f;

		// Token: 0x04000D42 RID: 3394
		[Range(0.001f, 3f)]
		[Tooltip("Movement rate. Lower values produce smoother movement, larger values produce more accurate movement.")]
		public float m_movementChange = 1.5f;

		// Token: 0x04000D43 RID: 3395
		[Header("Path Randomisation")]
		[Range(0f, 2f)]
		[Tooltip("Deviation rate. Smaller values produce slower more subtle deviations, larger values produce more obvious and rapid deviations.")]
		public float m_deviationRate;

		// Token: 0x04000D44 RID: 3396
		[Tooltip("Deviation range in X plane")]
		public float m_maxDeviationX;

		// Token: 0x04000D45 RID: 3397
		[Tooltip("Deviation range in Y plane")]
		public float m_maxDeviationY;

		// Token: 0x04000D46 RID: 3398
		[Tooltip("Deviation range in Z plane")]
		public float m_maxDeviationZ;

		// Token: 0x04000D47 RID: 3399
		private Vector3 m_targetPosition = Vector3.zero;

		// Token: 0x04000D48 RID: 3400
		private float m_distanceToTarget;

		// Token: 0x04000D49 RID: 3401
		private float m_currentMovementDistance;

		// Token: 0x04000D4A RID: 3402
		private Vector3 m_currentVelocity = Vector3.zero;
	}
}
