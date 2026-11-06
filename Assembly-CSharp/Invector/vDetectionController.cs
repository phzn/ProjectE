using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x0200043F RID: 1087
	[vClassHeader("Detection", true, "icon_v2", false, "")]
	public class vDetectionController : vMonoBehaviour
	{
		// Token: 0x06001B54 RID: 6996 RVA: 0x000AC3D7 File Offset: 0x000AA5D7
		protected virtual void Start()
		{
			if (!this.eyesCenter)
			{
				this.eyesCenter = base.transform;
			}
			this.targetsInRange = new List<vDetectionTarget>();
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x000AC3FD File Offset: 0x000AA5FD
		protected virtual void LateUpdate()
		{
			if (this.autoUpdate)
			{
				this.UpdatedeDetection();
			}
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x000AC410 File Offset: 0x000AA610
		public virtual void UpdatedeDetection()
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.targetsInRange.Count > 0)
			{
				if (this.detectedTarget.target == null || this.targetsInRange.Exists((vDetectionTarget _target) => Vector3.Distance(this.detectedTarget.target.center.position, this.eyesCenter.position) > Vector3.Distance(_target.center.position, this.eyesCenter.position)))
				{
					if (this.currentTimeToFindTarget <= 0f)
					{
						vDetectionTarget vDetectionTarget = null;
						this.currentTimeToFindTarget = this.detectionFrequency;
						if (this.targetsInRange.Count > 0)
						{
							if (this.indexOfDetection >= this.targetsInRange.Count)
							{
								this.indexOfDetection = 0;
							}
							vDetectionTarget = this.targetsInRange.OrderBy<vDetectionTarget, float>((vDetectionTarget t) => Vector3.Distance(this.eyesCenter.position, t.transform.position)).ToList<vDetectionTarget>()[this.indexOfDetection];
						}
						if (!vDetectionTarget)
						{
							this.indexOfDetection++;
							return;
						}
						if (!(this.detectedTarget.target != vDetectionTarget))
						{
							this.indexOfDetection++;
							return;
						}
						this.detectedTarget.targetIsLosted = false;
						this.detectedTarget.target = vDetectionTarget;
						this.currentTimeToLostTarget = this.timeToLostTarget;
						this.HandleLineOfSight();
						if (!this.detectedTarget.targetIsInSight)
						{
							this.detectedTarget.Reset();
							this.indexOfDetection++;
							return;
						}
						this.OnFindTarget();
					}
					else if (this.currentTimeToFindTarget > 0f)
					{
						this.currentTimeToFindTarget -= Time.deltaTime;
					}
				}
				else
				{
					this.currentTimeToFindTarget = this.detectionFrequency;
				}
				if (this.detectedTarget.target)
				{
					this.HandleLostTarget();
					return;
				}
			}
			else if (this.detectedTarget.target)
			{
				this.HandleLostTargetTime();
			}
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x000AC5C4 File Offset: 0x000AA7C4
		protected virtual void HandleLostTarget()
		{
			this.HandleLineOfSight();
			if (!this.detectedTarget.targetIsInSight || this.detectedTarget.target.healthController == null || this.detectedTarget.target.healthController.isDead)
			{
				this.HandleLostTargetTime();
				return;
			}
			this.currentTimeToLostTarget = this.timeToLostTarget;
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x000AC620 File Offset: 0x000AA820
		protected virtual void HandleLostTargetTime()
		{
			if (this.detectedTarget.target.healthController == null || this.detectedTarget.target.healthController.isDead || this.currentTimeToLostTarget <= 0f)
			{
				this.onLostvTarget.Invoke(this.detectedTarget.target);
				this.onLostTransformTarget.Invoke(this.detectedTarget.detectedPoint);
				this.detectedTarget.Reset();
				return;
			}
			if (this.currentTimeToLostTarget > 0f)
			{
				this.currentTimeToLostTarget -= Time.deltaTime;
			}
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000AC6BA File Offset: 0x000AA8BA
		protected virtual void OnFindTarget()
		{
			this.currentTimeToTriggerFindTargetEvent = 0f;
			base.StopAllCoroutines();
			base.StartCoroutine(this.FindTargetEvent());
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x000AC6DA File Offset: 0x000AA8DA
		protected virtual IEnumerator FindTargetEvent()
		{
			this.currentTimeToTriggerFindTargetEvent = this.timeToTriggerFindTargetEvent;
			while (this.currentTimeToTriggerFindTargetEvent > 0f)
			{
				this.currentTimeToTriggerFindTargetEvent -= Time.deltaTime;
				yield return null;
			}
			if (this.detectedTarget != null && this.detectedTarget.target != null)
			{
				this.onFindTransformTarget.Invoke(this.detectedTarget.target.center.transform);
				this.onFindvTarget.Invoke(this.detectedTarget.target);
			}
			yield break;
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x000AC6EC File Offset: 0x000AA8EC
		protected virtual void HandleLineOfSight()
		{
			if (this.useDetectionPoints)
			{
				this.CheckTargetPointsInSight(this.detectedTarget);
				this.detectedTarget.targetIsInSight = this.detectedTarget.detectedPoint && this.detectedTarget.detectionValue >= this.requiredDetectionValue;
				return;
			}
			this.detectedTarget.targetIsInSight = this.IsInFOV(this.detectedTarget.target.center.transform) && !this.IsObstructed(this.detectedTarget.target.center.transform);
			this.detectedTarget.detectedPoint = this.detectedTarget.target.center;
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x000AC7A8 File Offset: 0x000AA9A8
		protected virtual bool IsInFOV(Transform target)
		{
			Vector3 position = target.position;
			this.targetDirection = position - this.eyesCenter.position;
			Vector3 vector = this.eyesCenter.InverseTransformDirection(this.targetDirection.normalized);
			vector.y = 0f;
			Vector3 vector2 = this.eyesCenter.InverseTransformDirection(this.targetDirection.normalized);
			vector2.x = 0f;
			float magnitude = this.targetDirection.magnitude;
			Vector3 vector3 = this.eyesCenter.TransformDirection(vector);
			Vector3 vector4 = this.eyesCenter.TransformDirection(vector2);
			Vector3 forward = this.eyesCenter.forward;
			float num = Vector3.Angle(forward, vector3.normalized);
			float num2 = Vector3.Angle(forward, vector4.normalized);
			bool flag = num <= this.maxAngleHorizontal && num2 <= this.maxAngleVertical;
			if (this.debugTargetInAngle)
			{
				Debug.Log(string.Concat(new string[]
				{
					target.gameObject.name,
					" angle V: ",
					(num2 <= this.maxAngleVertical).ToStringColor(),
					" angle H: ",
					(num <= this.maxAngleHorizontal).ToStringColor()
				}), target.gameObject);
			}
			if (this.debugRays)
			{
				Debug.DrawRay(this.eyesCenter.position, vector3.normalized * magnitude, (num <= this.maxAngleHorizontal) ? Color.green : Color.blue, 2f);
				Debug.DrawRay(this.eyesCenter.position, vector4.normalized * magnitude, (num2 <= this.maxAngleVertical) ? Color.yellow : Color.red, 2f);
			}
			return flag;
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x000AC968 File Offset: 0x000AAB68
		public void SetTarget(Transform target)
		{
			if (target == null)
			{
				return;
			}
			vDetectionTarget component = target.GetComponent<vDetectionTarget>();
			if (component && this.detectedTarget.target != component)
			{
				this.SetTarget(component);
			}
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x000AC9A8 File Offset: 0x000AABA8
		public void SetTarget(vDetectionTarget target)
		{
			if (target == null)
			{
				return;
			}
			if (target && this.detectedTarget.target != target)
			{
				if (this.detectedTarget.target && !this.detectedTarget.targetIsLosted)
				{
					this.onLostTransformTarget.Invoke(this.detectedTarget.target.transform);
					this.onLostvTarget.Invoke(this.detectedTarget.target);
				}
				this.detectedTarget.target = target;
				this.detectedTarget.targetIsInSight = true;
				this.detectedTarget.targetIsLosted = false;
				this.currentTimeToLostTarget = this.timeToLostTarget;
				this.onFindTransformTarget.Invoke(target.center.transform);
				this.onFindvTarget.Invoke(target);
			}
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x000ACA84 File Offset: 0x000AAC84
		protected virtual void OnTriggerEnter(Collider other)
		{
			if (base.enabled && this.targetTag.Contains(other.tag))
			{
				vDetectionTarget component = other.GetComponent<vDetectionTarget>();
				if (component && component.IsValid && !this.targetsInRange.Contains(component))
				{
					this.targetsInRange.Add(component);
				}
			}
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x000ACAE0 File Offset: 0x000AACE0
		protected virtual void OnTriggerExit(Collider other)
		{
			if (base.enabled && this.targetTag.Contains(other.tag) && this.targetsInRange.Exists((vDetectionTarget t) => t.gameObject.Equals(other.gameObject)))
			{
				this.targetsInRange.Remove(this.targetsInRange.Find((vDetectionTarget t) => t.gameObject.Equals(other.gameObject)));
			}
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x000ACB58 File Offset: 0x000AAD58
		protected virtual void CheckTargetPointsInSight(vDetectionController.DetectedTarget targetToCheck)
		{
			this.detectedTarget.detectionValue = 0f;
			float num = 0f;
			Transform transform = null;
			int num2 = 0;
			for (int i = 0; i < targetToCheck.target.DetectionPoints.Count; i++)
			{
				vDetectionTarget.DetectionPoint detectionPoint = targetToCheck.target.DetectionPoints[i];
				if (detectionPoint.center && this.IsInFOV(detectionPoint.center) && !this.IsObstructed(detectionPoint.center) && detectionPoint.targetPriority >= this.minPriority && detectionPoint.targetPriority <= this.maxPriority)
				{
					if (this.debugRays)
					{
						Debug.Log(detectionPoint.center.gameObject.name + " was detected ", detectionPoint.center);
					}
					num += detectionPoint.value;
					if (num2 < (int)detectionPoint.targetPriority)
					{
						transform = detectionPoint.center;
						targetToCheck.detectedPoint = transform;
						num2 = (int)detectionPoint.targetPriority;
					}
					if (num - targetToCheck.target.DetectionReductionValue >= this.requiredDetectionValue)
					{
						break;
					}
				}
			}
			this.detectedTarget.detectionValue = num - targetToCheck.target.DetectionReductionValue;
			this.detectedTarget.detectedPoint = transform;
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x000ACC9C File Offset: 0x000AAE9C
		protected virtual bool IsObstructed(Transform target)
		{
			if (!this.useObstacles)
			{
				return false;
			}
			Vector3 position = target.position;
			bool flag = Physics.Linecast(this.eyesCenter.position, position, out this.hit, this.obstacles);
			if (flag && this.debugTargetObstacle)
			{
				Debug.Log(target.gameObject.name + " is Obstructed by " + this.hit.collider.gameObject.name, this.hit.collider.gameObject);
			}
			return flag;
		}

		// Token: 0x04001C7A RID: 7290
		[vEditorToolbar("Detection", false, "", false, false)]
		public bool autoUpdate = true;

		// Token: 0x04001C7B RID: 7291
		public Collider sensorCollider;

		// Token: 0x04001C7C RID: 7292
		public Transform eyesCenter;

		// Token: 0x04001C7D RID: 7293
		public bool useDetectionPoints = true;

		// Token: 0x04001C7E RID: 7294
		public bool useObstacles;

		// Token: 0x04001C7F RID: 7295
		[vHideInInspector("useObstacles", false)]
		public LayerMask obstacles;

		// Token: 0x04001C80 RID: 7296
		public float requiredDetectionValue = 4f;

		// Token: 0x04001C81 RID: 7297
		public float timeToLostTarget;

		// Token: 0x04001C82 RID: 7298
		public float detectionFrequency = 1f;

		// Token: 0x04001C83 RID: 7299
		public float timeToTriggerFindTargetEvent;

		// Token: 0x04001C84 RID: 7300
		public vDetectionTarget.TargetPriority minPriority;

		// Token: 0x04001C85 RID: 7301
		public vDetectionTarget.TargetPriority maxPriority = vDetectionTarget.TargetPriority.Maximum;

		// Token: 0x04001C86 RID: 7302
		public vTagMask targetTag = new vTagMask(new string[] { "Enemy" });

		// Token: 0x04001C87 RID: 7303
		[Range(0f, 180f)]
		public float maxAngleVertical = 60f;

		// Token: 0x04001C88 RID: 7304
		[Range(0f, 180f)]
		public float maxAngleHorizontal = 60f;

		// Token: 0x04001C89 RID: 7305
		[vEditorToolbar("Events", false, "", false, false)]
		public vDetectionController.DetectedTransformEvent onFindTransformTarget;

		// Token: 0x04001C8A RID: 7306
		public vDetectionController.DetectedTransformEvent onLostTransformTarget;

		// Token: 0x04001C8B RID: 7307
		public vDetectionController.DetectedvTargetEvent onFindvTarget;

		// Token: 0x04001C8C RID: 7308
		public vDetectionController.DetectedvTargetEvent onLostvTarget;

		// Token: 0x04001C8D RID: 7309
		[vEditorToolbar("Debug", false, "", false, false)]
		public bool debugRays;

		// Token: 0x04001C8E RID: 7310
		public bool debugTargetInAngle;

		// Token: 0x04001C8F RID: 7311
		public bool debugTargetObstacle;

		// Token: 0x04001C90 RID: 7312
		public List<vDetectionTarget> targetsInRange;

		// Token: 0x04001C91 RID: 7313
		public vDetectionController.DetectedTarget detectedTarget = new vDetectionController.DetectedTarget();

		// Token: 0x04001C92 RID: 7314
		[vReadOnly(false)]
		[SerializeField]
		protected float currentTimeToFindTarget;

		// Token: 0x04001C93 RID: 7315
		[vReadOnly(false)]
		[SerializeField]
		protected float currentTimeToTriggerFindTargetEvent;

		// Token: 0x04001C94 RID: 7316
		[vReadOnly(false)]
		[SerializeField]
		protected float currentTimeToLostTarget;

		// Token: 0x04001C95 RID: 7317
		[vReadOnly(false)]
		[SerializeField]
		protected float targetAngleHorizontal;

		// Token: 0x04001C96 RID: 7318
		[vReadOnly(false)]
		[SerializeField]
		protected float targetAngleVertical;

		// Token: 0x04001C97 RID: 7319
		private RaycastHit hit;

		// Token: 0x04001C98 RID: 7320
		protected int indexOfDetection;

		// Token: 0x04001C99 RID: 7321
		protected Vector3 targetDirection;

		// Token: 0x02000440 RID: 1088
		[Serializable]
		public class DetectedvTargetEvent : UnityEvent<vDetectionTarget>
		{
		}

		// Token: 0x02000441 RID: 1089
		[Serializable]
		public class DetectedTransformEvent : UnityEvent<Transform>
		{
		}

		// Token: 0x02000442 RID: 1090
		[Serializable]
		public class DetectedTarget
		{
			// Token: 0x06001B68 RID: 7016 RVA: 0x000ACE1C File Offset: 0x000AB01C
			public void Reset()
			{
				this.target = null;
				this.detectedPoint = null;
				this.targetIsInSight = false;
				this.targetIsLosted = true;
			}

			// Token: 0x04001C9A RID: 7322
			[vReadOnly(false)]
			public vDetectionTarget target;

			// Token: 0x04001C9B RID: 7323
			[vReadOnly(false)]
			public Transform detectedPoint;

			// Token: 0x04001C9C RID: 7324
			[vReadOnly(false)]
			public bool targetIsInSight;

			// Token: 0x04001C9D RID: 7325
			[vReadOnly(false)]
			public float detectionValue;

			// Token: 0x04001C9E RID: 7326
			[vReadOnly(false)]
			public bool targetIsLosted;
		}
	}
}
