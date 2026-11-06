using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vCharacterController.AI
{
	// Token: 0x020006E5 RID: 1765
	public class vSimpleMeleeAI_SphereSensor : MonoBehaviour
	{
		// Token: 0x06002CF6 RID: 11510 RVA: 0x000F82DE File Offset: 0x000F64DE
		protected virtual void Start()
		{
			this.targetsInArea = new List<Transform>();
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x000F82EB File Offset: 0x000F64EB
		public virtual void AddTarget(Transform _transform)
		{
			if (!this.targetsInArea.Contains(_transform))
			{
				this.targetsInArea.Add(_transform);
			}
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x000F8308 File Offset: 0x000F6508
		public virtual void SetColliderRadius(float radius)
		{
			SphereCollider component = base.GetComponent<SphereCollider>();
			if (component)
			{
				component.radius = radius;
			}
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x000F832B File Offset: 0x000F652B
		public virtual Transform GetTargetTransform()
		{
			if (this.targetsInArea.Count > 0)
			{
				this.SortTargets();
				if (this.targetsInArea.Count > 0)
				{
					return this.targetsInArea[0].transform;
				}
			}
			return null;
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x000F8364 File Offset: 0x000F6564
		public virtual vIHealthController GetTargetvCharacter()
		{
			if (this.targetsInArea.Count > 0)
			{
				this.SortCharacters();
				if (this.targetsInArea.Count > 0)
				{
					vIHealthController component = this.targetsInArea[0].GetComponent<vIHealthController>();
					if (component != null && component.currentHealth > 0f)
					{
						return component;
					}
				}
			}
			return null;
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000F83B8 File Offset: 0x000F65B8
		protected virtual void SortCharacters()
		{
			for (int i = this.targetsInArea.Count - 1; i >= 0; i--)
			{
				Transform transform = this.targetsInArea[i];
				float num = Vector3.Distance(base.transform.position, this.targetsInArea[i].transform.position);
				if (transform == null || num > this.lastDetectionDistance || transform.GetComponent<vIHealthController>() == null)
				{
					this.targetsInArea.RemoveAt(i);
				}
			}
			if (this.getFromDistance)
			{
				this.targetsInArea.Sort((Transform c1, Transform c2) => Vector3.Distance(base.transform.position, (c1 != null) ? c1.transform.position : (Vector3.one * float.PositiveInfinity)).CompareTo(Vector3.Distance(base.transform.position, (c2 != null) ? c2.transform.position : (Vector3.one * float.PositiveInfinity))));
			}
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x000F8458 File Offset: 0x000F6658
		protected virtual void SortTargets()
		{
			for (int i = this.targetsInArea.Count - 1; i >= 0; i--)
			{
				Object @object = this.targetsInArea[i];
				float num = Vector3.Distance(base.transform.position, this.targetsInArea[i].transform.position);
				if (@object == null || num > this.lastDetectionDistance)
				{
					this.targetsInArea.RemoveAt(i);
				}
			}
			if (this.getFromDistance)
			{
				this.targetsInArea.Sort((Transform c1, Transform c2) => Vector3.Distance(base.transform.position, (c1 != null) ? c1.transform.position : (Vector3.one * float.PositiveInfinity)).CompareTo(Vector3.Distance(base.transform.position, (c2 != null) ? c2.transform.position : (Vector3.one * float.PositiveInfinity))));
			}
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x000F84EC File Offset: 0x000F66EC
		public virtual void CheckTargetsAround(float FOV, float minDistance, float maxDistance, vTagMask detectTags, LayerMask detectMask, bool getTargetFromDistance = false)
		{
			this.getFromDistance = getTargetFromDistance;
			this.lastDetectionDistance = maxDistance;
			Collider[] array = Physics.OverlapSphere(base.transform.position, maxDistance, detectMask);
			array = Array.FindAll<Collider>(array, (Collider t) => this.root && this.root != t.transform && detectTags != null && detectTags.Count > 0 && detectTags.Contains(t.gameObject.tag) && this.InFovAngle(t.transform, minDistance, FOV));
			this.targetsInArea = Array.ConvertAll<Collider, Transform>(array, (Collider c) => c.transform).vToList<Transform>();
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x000F8588 File Offset: 0x000F6788
		protected virtual bool InFovAngle(Transform target, float minDistance, float FOV)
		{
			if (Vector3.Distance(base.transform.position, target.position) < minDistance)
			{
				return true;
			}
			Quaternion quaternion = Quaternion.LookRotation(target.position - base.transform.position, Vector3.up);
			Vector3 eulerAngles = base.transform.eulerAngles;
			Vector3 vector = quaternion.eulerAngles - eulerAngles;
			float y = vector.NormalizeAngle().y;
			float x = vector.NormalizeAngle().x;
			return y <= FOV * 0.5f && y >= -(FOV * 0.5f) && x <= FOV * 0.5f && x >= -(FOV * 0.5f);
		}

		// Token: 0x04002BE2 RID: 11234
		public Transform root;

		// Token: 0x04002BE3 RID: 11235
		public List<Transform> targetsInArea;

		// Token: 0x04002BE4 RID: 11236
		protected bool getFromDistance;

		// Token: 0x04002BE5 RID: 11237
		protected float lastDetectionDistance;
	}
}
