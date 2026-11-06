using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Invector.vCamera;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x020006BD RID: 1725
	[vClassHeader("Lock-On", true, "icon_v2", false, "")]
	public abstract class vLockOnBehaviour : vMonoBehaviour
	{
		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002A90 RID: 10896 RVA: 0x000EE64C File Offset: 0x000EC84C
		// (set) Token: 0x06002A91 RID: 10897 RVA: 0x000EE654 File Offset: 0x000EC854
		protected virtual Transform target
		{
			get
			{
				return this._target;
			}
			set
			{
				this._target = value;
			}
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000EE65D File Offset: 0x000EC85D
		protected virtual void Start()
		{
			if (this.rangeToExitLockOn < this.rangeToEnterLockOn)
			{
				this.rangeToExitLockOn = this.rangeToEnterLockOn;
			}
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000EE679 File Offset: 0x000EC879
		public virtual void ChangeTarget(int value)
		{
			base.StartCoroutine(this.ChangeTargetRoutine(value));
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06002A94 RID: 10900 RVA: 0x000EE689 File Offset: 0x000EC889
		public virtual Transform currentTarget
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000EE694 File Offset: 0x000EC894
		public virtual bool isCharacterAlive()
		{
			if (this.currentTarget == null)
			{
				return false;
			}
			vIHealthController component = this.currentTarget.GetComponent<vIHealthController>();
			return component != null && component.currentHealth > 0f;
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000EE6D4 File Offset: 0x000EC8D4
		public virtual bool isCharacterAlive(Transform other)
		{
			vIHealthController component = other.GetComponent<vIHealthController>();
			return component != null && component.currentHealth > 0f;
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000EE6FD File Offset: 0x000EC8FD
		public virtual void ResetLockOn()
		{
			this.target = null;
			this._inLockOn = false;
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002A98 RID: 10904 RVA: 0x000EE70D File Offset: 0x000EC90D
		public virtual List<Transform> allTargets
		{
			get
			{
				if (this.visibles != null && this.visibles.Count > 0)
				{
					return this.visibles;
				}
				return null;
			}
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000EE730 File Offset: 0x000EC930
		protected virtual void UpdateLockOn(bool value)
		{
			if (value && value != this._inLockOn)
			{
				this._inLockOn = value;
				this.visibles = this.GetPossibleTargets();
				this.index = 0;
				if (this.visibles != null && this.visibles.Count > 0)
				{
					this.target = this.visibles[this.index];
					return;
				}
			}
			else if (!value && value != this._inLockOn)
			{
				this._inLockOn = value;
				this.index = 0;
				this.target = null;
				if (this.visibles != null)
				{
					this.visibles.Clear();
				}
			}
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000EE7C5 File Offset: 0x000EC9C5
		protected virtual IEnumerator ChangeTargetRoutine(int value)
		{
			if (!this.changingTarget)
			{
				this.changingTarget = true;
				this.visibles = this.GetPossibleTargets();
				if (this._inLockOn && this.visibles != null && this.visibles.Count > 1)
				{
					if (this.index + value > this.visibles.Count - 1)
					{
						this.index = 0;
					}
					else if (this.index + value < 0)
					{
						this.index = this.visibles.Count - 1;
					}
					else
					{
						this.index += value;
					}
					this.target = this.visibles[this.index];
					this.SetTarget();
				}
				yield return new WaitForSeconds(this.timeToChangeTarget);
				this.changingTarget = false;
			}
			yield break;
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x00004F31 File Offset: 0x00003131
		protected virtual void SetTarget()
		{
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000EE7DC File Offset: 0x000EC9DC
		protected void OnGUI()
		{
			if (this.showDebug)
			{
				float num = (float)Screen.width - (float)Screen.width * this.screenMarginX;
				float num2 = (float)Screen.height - (float)Screen.height * this.screenMarginY;
				float num3 = (float)Screen.width * 0.5f - num * 0.5f;
				float num4 = (float)Screen.height * 0.5f - num2 * 0.5f;
				this.rect = new Rect(num3, num4, num, num2);
				GUI.Box(this.rect, "");
			}
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000EE864 File Offset: 0x000ECA64
		protected void Init()
		{
			if (Camera.main == null)
			{
				base.enabled = false;
			}
			this.tpCamera = Camera.main.transform.root.GetComponent<vThirdPersonCamera>();
			float num = (float)Screen.width - (float)Screen.width * this.screenMarginX;
			float num2 = (float)Screen.height - (float)Screen.height * this.screenMarginY;
			float num3 = (float)Screen.width * 0.5f - num * 0.5f;
			float num4 = (float)Screen.height * 0.5f - num2 * 0.5f;
			this.rect = new Rect(num3, num4, num, num2);
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x000EE904 File Offset: 0x000ECB04
		protected void OnDrawGizmos()
		{
			if (this.showDebug && this.watcher)
			{
				Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
				Gizmos.DrawSphere(this.watcher.position, this.rangeToEnterLockOn + 0.1f);
				if (this.visibles != null && this.visibles.Count > 0 && this.target != null)
				{
					this.visibles.ForEach(delegate(Transform _transform)
					{
						Gizmos.color = (_transform.Equals(this.currentTarget) ? Color.red : Color.yellow);
						Gizmos.DrawSphere(_transform.GetComponent<Collider>().bounds.center, 0.5f);
					});
				}
			}
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x000EE9A0 File Offset: 0x000ECBA0
		protected virtual List<Transform> GetPossibleTargets()
		{
			if (this.tpCamera != null && this.tpCamera.mainTarget != null)
			{
				this.watcher = this.tpCamera.mainTarget;
			}
			else
			{
				this.watcher = base.transform;
			}
			List<Transform> list = new List<Transform>();
			foreach (RaycastHit raycastHit in Physics.SphereCastAll(this.watcher.position, this.rangeToEnterLockOn, this.watcher.forward, 0.01f))
			{
				if (this.tagsToFind.Contains(raycastHit.transform.tag) && this.isCharacterAlive(raycastHit.transform.GetComponent<Transform>()))
				{
					Vector3[] array2 = this.BoundPoints(raycastHit.collider);
					int j = 0;
					while (j < array2.Length)
					{
						Vector3 vector = array2[j];
						RaycastHit raycastHit2;
						if (Physics.Linecast(base.transform.position, vector, out raycastHit2, this.layerOfObstacles))
						{
							if (raycastHit2.transform == raycastHit.transform)
							{
								list.Add(raycastHit.transform);
								if (this.showDebug)
								{
									Debug.DrawLine(base.transform.position, vector, Color.green, 2f);
									break;
								}
								break;
							}
							else
							{
								if (this.showDebug)
								{
									Debug.DrawLine(base.transform.position, vector, Color.red, 2f);
								}
								j++;
							}
						}
						else
						{
							list.Add(raycastHit.transform);
							if (this.showDebug)
							{
								Debug.DrawLine(base.transform.position, vector, Color.green, 2f);
								break;
							}
							break;
						}
					}
				}
			}
			this.SortTargets(ref list);
			return list;
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x000EEB6C File Offset: 0x000ECD6C
		protected void SortTargets(ref List<Transform> list)
		{
			if (this.tpCamera == null || this.tpCamera.targetCamera)
			{
				this.Init();
			}
			List<Transform> list2 = new List<Transform>();
			List<Transform> list3 = new List<Transform>();
			List<Transform> list4 = new List<Transform>();
			Plane[] array = GeometryUtility.CalculateFrustumPlanes(this.tpCamera.targetCamera);
			for (int i = 0; i < list.Count; i++)
			{
				Transform transform = list[i];
				Vector2 vector = this.tpCamera.targetCamera.WorldToScreenPoint(transform.transform.position);
				if (GeometryUtility.TestPlanesAABB(array, transform.GetComponent<Collider>().bounds))
				{
					if (this.rect.Contains(vector))
					{
						list2.Add(transform);
					}
					else if (!this.withinCameraView || (this.withinCameraView && !this.onlyInsideRect))
					{
						list3.Add(transform);
					}
				}
				else if (!this.withinCameraView)
				{
					list4.Add(transform);
				}
			}
			list2.Sort(delegate(Transform t1, Transform t2)
			{
				Vector2 vector2 = this.tpCamera.targetCamera.WorldToScreenPoint(t1.transform.position);
				Vector2 vector3 = this.tpCamera.targetCamera.WorldToScreenPoint(t2.transform.position);
				return Vector2.Distance(vector2, this.rect.center).CompareTo(Vector2.Distance(vector3, this.rect.center));
			});
			list3.Sort(delegate(Transform t1, Transform t2)
			{
				Vector2 vector4 = this.tpCamera.targetCamera.WorldToScreenPoint(t1.transform.position);
				Vector2 vector5 = this.tpCamera.targetCamera.WorldToScreenPoint(t2.transform.position);
				return Vector2.Distance(vector4, this.rect.center).CompareTo(Vector2.Distance(vector5, this.rect.center));
			});
			list4.Sort((Transform t1, Transform t2) => Vector3.Distance(t1.transform.position, base.transform.position).CompareTo(Vector3.Distance(t2.transform.position, base.transform.position)));
			list = list2.Union<Transform>(list3).Union<Transform>(list4).ToList<Transform>();
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x000EECB8 File Offset: 0x000ECEB8
		protected Vector3[] BoundPoints(Collider collider)
		{
			Vector3 min = collider.bounds.min;
			Vector3 max = collider.bounds.max;
			Vector3 vector = new Vector3(min.x, min.y, max.z);
			Vector3 vector2 = new Vector3(min.x, max.y, min.z);
			Vector3 vector3 = new Vector3(max.x, min.y, min.z);
			Vector3 vector4 = new Vector3(min.x, max.y, max.z);
			Vector3 vector5 = new Vector3(max.x, min.y, max.z);
			Vector3 vector6 = new Vector3(max.x, max.y, min.z);
			Color white = Color.white;
			if (this.showDebug)
			{
				Debug.DrawLine(vector4, max, white, 1f);
				Debug.DrawLine(max, vector6, white, 1f);
				Debug.DrawLine(vector6, vector2, white, 1f);
				Debug.DrawLine(vector2, vector4, white, 1f);
				Debug.DrawLine(vector, vector5, white, 1f);
				Debug.DrawLine(vector5, vector3, white, 1f);
				Debug.DrawLine(vector3, min, white, 1f);
				Debug.DrawLine(min, vector, white, 1f);
				Debug.DrawLine(vector4, vector, white, 1f);
				Debug.DrawLine(max, vector5, white, 1f);
				Debug.DrawLine(vector6, vector3, white, 1f);
				Debug.DrawLine(vector2, min, white, 1f);
			}
			return new Vector3[] { min, max, vector, vector2, vector3, vector4, vector5, vector6 };
		}

		// Token: 0x04002A7B RID: 10875
		protected Transform watcher;

		// Token: 0x04002A7C RID: 10876
		[Tooltip("Tags of objects that can be found")]
		public string[] tagsToFind = new string[] { "Enemy" };

		// Token: 0x04002A7D RID: 10877
		[Tooltip("Check this option to only Lock into target within the camera field of view")]
		public bool withinCameraView = true;

		// Token: 0x04002A7E RID: 10878
		[vHideInInspector("withinCameraView", false)]
		[Tooltip("Get only target inside screen margins")]
		public bool onlyInsideRect = true;

		// Token: 0x04002A7F RID: 10879
		[Tooltip("Layer of Obstacle to prevent the find for targets")]
		public LayerMask layerOfObstacles = 1;

		// Token: 0x04002A80 RID: 10880
		[Range(0f, 1f)]
		[Tooltip("Use this to set a margin of Aim point ")]
		public float screenMarginX = 0.8f;

		// Token: 0x04002A81 RID: 10881
		[Range(0f, 1f)]
		[Tooltip("Use this to set a margin of Aim point ")]
		public float screenMarginY = 0.1f;

		// Token: 0x04002A82 RID: 10882
		[Tooltip("Range of the search for targets")]
		public float rangeToEnterLockOn = 10f;

		// Token: 0x04002A83 RID: 10883
		[Tooltip("Range to auto exit lock")]
		public float rangeToExitLockOn = 15f;

		// Token: 0x04002A84 RID: 10884
		[Tooltip("Show the Gizmos and helpers")]
		public bool showDebug;

		// Token: 0x04002A85 RID: 10885
		public float timeToChangeTarget = 0.25f;

		// Token: 0x04002A86 RID: 10886
		protected int index;

		// Token: 0x04002A87 RID: 10887
		protected List<Transform> visibles;

		// Token: 0x04002A88 RID: 10888
		protected Transform _target;

		// Token: 0x04002A89 RID: 10889
		protected vThirdPersonCamera tpCamera;

		// Token: 0x04002A8A RID: 10890
		protected Rect rect;

		// Token: 0x04002A8B RID: 10891
		protected bool _inLockOn;

		// Token: 0x04002A8C RID: 10892
		protected bool changingTarget;
	}
}
