using System;
using Invector.vCharacterController;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000489 RID: 1161
	public class vPlatform : vMonoBehaviour
	{
		// Token: 0x06001C7E RID: 7294 RVA: 0x000B0B0C File Offset: 0x000AED0C
		private void OnDrawGizmos()
		{
			if (this.points == null || this.points.Length == 0 || this.startIndex >= this.points.Length)
			{
				return;
			}
			Transform transform = this.points[0].transform;
			Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
			if (!Application.isPlaying)
			{
				base.transform.position = this.points[this.startIndex].transform.position;
				base.transform.eulerAngles = this.points[this.startIndex].transform.eulerAngles;
			}
			foreach (vPlatform.vPlatformPoint vPlatformPoint in this.points)
			{
				if (vPlatformPoint.transform != null && vPlatformPoint.transform != transform)
				{
					Gizmos.DrawLine(transform.position, vPlatformPoint.transform.position);
					transform = vPlatformPoint.transform;
				}
			}
			foreach (vPlatform.vPlatformPoint vPlatformPoint2 in this.points)
			{
				if (vPlatformPoint2.transform)
				{
					Gizmos.matrix = Matrix4x4.TRS(vPlatformPoint2.transform.position, vPlatformPoint2.transform.rotation, base.transform.lossyScale);
					Gizmos.DrawCube(Vector3.zero, Vector3.one);
				}
			}
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000B0C68 File Offset: 0x000AEE68
		public void SetPause(bool value)
		{
			this.pause = value;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x000B0C74 File Offset: 0x000AEE74
		private void Start()
		{
			if (this.points.Length == 0 || this.startIndex >= this.points.Length)
			{
				return;
			}
			if (this.points.Length < 2)
			{
				return;
			}
			base.transform.position = this.points[this.startIndex].transform.position;
			base.transform.eulerAngles = this.points[this.startIndex].transform.eulerAngles;
			this.oldEuler = base.transform.eulerAngles;
			int num = this.startIndex;
			if (this.startIndex + 1 < this.points.Length)
			{
				num++;
			}
			else if (this.startIndex - 1 > 0)
			{
				num--;
				this.invert = true;
			}
			this.dist = Vector3.Distance(base.transform.position, this.points[num].transform.position);
			this.targetTransform = this.points[num].transform;
			this.currentTime = (this.points[this.startIndex].useDefaultStayTime ? this.defaultStayTime : this.points[this.index].stayTime);
			this.currentSpeed = (this.points[this.startIndex].useDefaultSpeed ? this.defaultSpeed : this.points[this.index].speedToNextPoint);
			this.index = num;
			this.canMove = true;
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x000B0DE4 File Offset: 0x000AEFE4
		private void FixedUpdate()
		{
			if (this.points.Length == 0 && !this.canMove)
			{
				return;
			}
			if (this.pause)
			{
				return;
			}
			this.currentDist = Vector3.Distance(base.transform.position, this.targetTransform.position);
			if (this.currentTime <= 0f)
			{
				float num = Mathf.Clamp((100f - 100f * this.currentDist / this.dist) * 0.01f, 0f, 1f);
				base.transform.position = Vector3.MoveTowards(base.transform.position, this.targetTransform.position, this.currentSpeed * Time.deltaTime);
				if (!float.IsNaN(num) && !float.IsInfinity(num) && this.oldEuler != this.oldEuler + (this.targetTransform.eulerAngles - this.oldEuler))
				{
					base.transform.eulerAngles = Vector3.Lerp(this.oldEuler, this.targetTransform.eulerAngles, num);
				}
			}
			else
			{
				this.currentTime -= Time.fixedDeltaTime;
			}
			if (this.currentDist < 0.02f)
			{
				this.currentSpeed = (this.points[this.index].useDefaultSpeed ? this.defaultSpeed : this.points[this.index].speedToNextPoint);
				this.currentTime = (this.points[this.index].useDefaultStayTime ? this.defaultStayTime : this.points[this.index].stayTime);
				if (!this.invert)
				{
					if (this.index + 1 < this.points.Length)
					{
						this.index++;
					}
					else
					{
						this.invert = true;
					}
				}
				else if (this.index - 1 >= 0)
				{
					this.index--;
				}
				else
				{
					this.invert = false;
				}
				this.dist = Vector3.Distance(this.targetTransform.position, this.points[this.index].transform.position);
				this.targetTransform = this.points[this.index].transform;
				this.oldEuler = base.transform.eulerAngles;
			}
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x000B1038 File Offset: 0x000AF238
		private void OnTriggerStay(Collider other)
		{
			if (other.transform.parent != base.transform && other.transform.CompareTag("Player") && other.GetComponent<vCharacter>() != null)
			{
				other.transform.parent = base.transform;
			}
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x000B1090 File Offset: 0x000AF290
		private void OnTriggerExit(Collider other)
		{
			if (other.transform.parent == base.transform && other.transform.CompareTag("Player"))
			{
				other.transform.parent = null;
				other.transform.eulerAngles = new Vector3(0f, other.transform.eulerAngles.y, 0f);
			}
		}

		// Token: 0x04001DB5 RID: 7605
		public vPlatform.vPlatformPoint[] points;

		// Token: 0x04001DB6 RID: 7606
		[Tooltip("Movement speed between points")]
		public float defaultSpeed = 1f;

		// Token: 0x04001DB7 RID: 7607
		[Tooltip("Time to stay in current point")]
		public float defaultStayTime = 2f;

		// Token: 0x04001DB8 RID: 7608
		[Tooltip("Index to Starting point")]
		public int startIndex;

		// Token: 0x04001DB9 RID: 7609
		public bool pause;

		// Token: 0x04001DBA RID: 7610
		[HideInInspector]
		public bool canMove;

		// Token: 0x04001DBB RID: 7611
		private Vector3 oldEuler;

		// Token: 0x04001DBC RID: 7612
		private int index;

		// Token: 0x04001DBD RID: 7613
		private bool invert;

		// Token: 0x04001DBE RID: 7614
		private float currentTime;

		// Token: 0x04001DBF RID: 7615
		private float currentSpeed;

		// Token: 0x04001DC0 RID: 7616
		private float dist;

		// Token: 0x04001DC1 RID: 7617
		private float currentDist;

		// Token: 0x04001DC2 RID: 7618
		private Transform targetTransform;

		// Token: 0x0200048A RID: 1162
		[Serializable]
		public class vPlatformPoint
		{
			// Token: 0x04001DC3 RID: 7619
			public Transform transform;

			// Token: 0x04001DC4 RID: 7620
			public bool useDefaultStayTime = true;

			// Token: 0x04001DC5 RID: 7621
			[vHideInInspector("useDefaultstayTime", true)]
			public float stayTime;

			// Token: 0x04001DC6 RID: 7622
			public bool useDefaultSpeed = true;

			// Token: 0x04001DC7 RID: 7623
			[vHideInInspector("useDefaultSpeed", true)]
			public float speedToNextPoint = 1f;
		}
	}
}
