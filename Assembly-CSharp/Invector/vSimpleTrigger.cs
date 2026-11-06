using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x0200048F RID: 1167
	[vClassHeader("SimpleTrigger", true, "icon_v2", false, "", openClose = false, useHelpBox = true, helpBoxText = "Tags and Layer To Detect : Use this to filter tags and layer that can interact with trigger, Select Nothing  to ignore filter")]
	public class vSimpleTrigger : vMonoBehaviour
	{
		// Token: 0x06001CA2 RID: 7330 RVA: 0x000B1713 File Offset: 0x000AF913
		public void ToggleGizmos()
		{
			vSimpleTrigger.drawGizmos = !vSimpleTrigger.drawGizmos;
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x000B1724 File Offset: 0x000AF924
		// (set) Token: 0x06001CA4 RID: 7332 RVA: 0x000B1788 File Offset: 0x000AF988
		public virtual Collider selfCollider
		{
			get
			{
				if (!this._selfCollider && base.transform.GetComponent<Collider>() == null)
				{
					this._selfCollider = base.gameObject.AddComponent<BoxCollider>();
				}
				else if (!this._selfCollider)
				{
					this._selfCollider = base.transform.GetComponent<Collider>();
				}
				return this._selfCollider;
			}
			protected set
			{
				this._selfCollider = value;
			}
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000B1794 File Offset: 0x000AF994
		protected virtual void OnDrawGizmos()
		{
			if (!vSimpleTrigger.drawGizmos)
			{
				return;
			}
			Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, base.transform.lossyScale);
			Vector3 vector = base.transform.InverseTransformPoint(this.selfCollider.bounds.center);
			Vector3 colliderSize = this.GetColliderSize();
			Color color = new Color(1f, 0f, 0f, 0.2f);
			Color color2 = new Color(0f, 1f, 0f, 0.2f);
			Gizmos.color = ((this.inCollision && Application.isPlaying) ? color : color2);
			if (this.selfCollider is BoxCollider)
			{
				Gizmos.DrawCube(vector, colliderSize);
				return;
			}
			if (this.selfCollider is SphereCollider)
			{
				Gizmos.DrawSphere(vector, (this.selfCollider as SphereCollider).radius);
				return;
			}
			if (this.selfCollider is CapsuleCollider)
			{
				Gizmos.DrawCube(vector, colliderSize * 2f);
				return;
			}
			if (this.selfCollider is MeshCollider)
			{
				Gizmos.DrawMesh((this.selfCollider as MeshCollider).sharedMesh, vector);
			}
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000B18C4 File Offset: 0x000AFAC4
		private Vector3 GetColliderSize()
		{
			if (this.selfCollider is BoxCollider)
			{
				return (this.selfCollider as BoxCollider).size;
			}
			if (this.selfCollider is SphereCollider)
			{
				return Vector3.one;
			}
			if (this.selfCollider is CapsuleCollider)
			{
				Vector3 zero = Vector3.zero;
				zero.x = (this.selfCollider as CapsuleCollider).radius;
				zero.z = (this.selfCollider as CapsuleCollider).radius;
				zero.y = (this.selfCollider as CapsuleCollider).height * 0.5f;
				return zero;
			}
			return Vector3.one;
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x000B1967 File Offset: 0x000AFB67
		protected virtual void Start()
		{
			this.inCollision = false;
			this.selfCollider.isTrigger = true;
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000B197C File Offset: 0x000AFB7C
		protected virtual void OnTriggerEnter(Collider other)
		{
			if (base.enabled && this.other == null && this.CanTrigger(other) && this.IsInTagMask(other.gameObject.tag) && this.IsInLayerMask(other.gameObject.layer))
			{
				this.inCollision = true;
				this.other = other;
				this.onTriggerEnter.Invoke(other);
				if (this.debugMode)
				{
					Debug.Log(other.gameObject.name + "TriggerEnter");
				}
				if (base.enabled && base.gameObject.activeInHierarchy)
				{
					base.StartCoroutine(this.TriggerStayRoutine());
				}
			}
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x000B1A35 File Offset: 0x000AFC35
		private bool CanTrigger(Collider other)
		{
			return !this.ignoreIsTriggerColliders || !other.isTrigger;
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x000B1A4C File Offset: 0x000AFC4C
		protected virtual void OnTriggerExit(Collider other)
		{
			if (base.enabled && this.other != null && this.CanTrigger(other) && this.other.gameObject == other.gameObject)
			{
				this.inCollision = false;
				this.onTriggerExit.Invoke(other);
				if (this.debugMode)
				{
					Debug.Log(other.gameObject.name + "TriggerExit");
				}
				this.other = null;
			}
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x000B1ACC File Offset: 0x000AFCCC
		protected virtual bool IsInTagMask(string tag)
		{
			return this.tagsToDetect.Count == 0 || this.tagsToDetect.Contains(tag);
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x000B1AE9 File Offset: 0x000AFCE9
		protected virtual bool IsInLayerMask(int layer)
		{
			return this.layersToDetect.value == 0 || (this.layersToDetect.value & (1 << layer)) > 0;
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x000B1B0F File Offset: 0x000AFD0F
		protected IEnumerator TriggerStayRoutine()
		{
			while (this.other != null)
			{
				if (this.other == null || !this.other.gameObject.activeInHierarchy)
				{
					this.OnTriggerExit(this.other);
					break;
				}
				this.onTriggerStay.Invoke(this.other);
				if (this.debugMode)
				{
					Debug.Log(this.other.gameObject.name + "TriggerStay");
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x04001DEE RID: 7662
		public static bool drawGizmos = true;

		// Token: 0x04001DEF RID: 7663
		[vButton("ToggleGizmos", "ToggleGizmos", typeof(vSimpleTrigger), false)]
		public bool useFilter = true;

		// Token: 0x04001DF0 RID: 7664
		public bool ignoreIsTriggerColliders;

		// Token: 0x04001DF1 RID: 7665
		public bool debugMode;

		// Token: 0x04001DF2 RID: 7666
		public vTagMask tagsToDetect = new List<string> { "Player" };

		// Token: 0x04001DF3 RID: 7667
		public LayerMask layersToDetect = 0;

		// Token: 0x04001DF4 RID: 7668
		public vSimpleTrigger.vTriggerEvent onTriggerEnter;

		// Token: 0x04001DF5 RID: 7669
		public vSimpleTrigger.vTriggerEvent onTriggerExit;

		// Token: 0x04001DF6 RID: 7670
		public vSimpleTrigger.vTriggerEvent onTriggerStay;

		// Token: 0x04001DF7 RID: 7671
		protected bool inCollision;

		// Token: 0x04001DF8 RID: 7672
		protected bool triggerStay;

		// Token: 0x04001DF9 RID: 7673
		protected Collider other;

		// Token: 0x04001DFA RID: 7674
		protected Collider _selfCollider;

		// Token: 0x02000490 RID: 1168
		[Serializable]
		public class vTriggerEvent : UnityEvent<Collider>
		{
		}
	}
}
