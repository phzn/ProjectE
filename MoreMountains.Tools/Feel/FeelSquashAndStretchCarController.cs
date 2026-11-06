using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000389 RID: 905
	[AddComponentMenu("")]
	public class FeelSquashAndStretchCarController : MonoBehaviour
	{
		// Token: 0x060018B7 RID: 6327 RVA: 0x000692E4 File Offset: 0x000674E4
		protected virtual void Start()
		{
			this._bounds = this.BoundaryCollider.bounds;
			MMFeedbacks teleportFeedbacks = this.TeleportFeedbacks;
			if (teleportFeedbacks != null)
			{
				teleportFeedbacks.Initialization(false);
			}
			this._trailTime = this.Trails[0].time;
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x00069320 File Offset: 0x00067520
		protected virtual void HandleInput()
		{
			this._input = FeelDemosInputHelper.GetDirectionAxis(ref this._input);
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x00069333 File Offset: 0x00067533
		protected virtual void Update()
		{
			this.HandleInput();
			this.MoveCar();
			this.HandleBounds();
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x00069348 File Offset: 0x00067548
		protected virtual void MoveCar()
		{
			base.transform.Rotate(this._rotationAxis, this._input.x * Time.deltaTime * this.RotationSpeed);
			base.transform.Translate(base.transform.forward * this._input.y * this.Speed * Time.deltaTime, Space.World);
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x000693BC File Offset: 0x000675BC
		protected virtual void HandleBounds()
		{
			this._newPosition = (this._thisPosition = base.transform.position);
			if (this._thisPosition.x < this._bounds.min.x)
			{
				this._newPosition.x = this._bounds.max.x;
			}
			else if (this._thisPosition.x > this._bounds.max.x)
			{
				this._newPosition.x = this._bounds.min.x;
			}
			if (this._thisPosition.z < this._bounds.min.z)
			{
				this._newPosition.z = this._bounds.max.z;
			}
			else if (this._thisPosition.z > this._bounds.max.z)
			{
				this._newPosition.z = this._bounds.min.z;
			}
			if (this._newPosition != this._thisPosition)
			{
				base.StartCoroutine(this.TeleportSequence());
			}
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x000694E7 File Offset: 0x000676E7
		protected virtual IEnumerator TeleportSequence()
		{
			MMFeedbacks teleportFeedbacks = this.TeleportFeedbacks;
			if (teleportFeedbacks != null)
			{
				teleportFeedbacks.PlayFeedbacks();
			}
			this.SetTrails(false);
			yield return MMCoroutine.WaitForFrames(1);
			base.transform.position = this._newPosition;
			MMFeedbacks teleportFeedbacks2 = this.TeleportFeedbacks;
			if (teleportFeedbacks2 != null)
			{
				teleportFeedbacks2.PlayFeedbacks();
			}
			this.SetTrails(true);
			yield break;
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x000694F8 File Offset: 0x000676F8
		protected virtual void SetTrails(bool status)
		{
			foreach (TrailRenderer trailRenderer in this.Trails)
			{
				trailRenderer.Clear();
			}
		}

		// Token: 0x04001CD9 RID: 7385
		[Header("Car Settings")]
		public float Speed = 2f;

		// Token: 0x04001CDA RID: 7386
		public float RotationSpeed = 2f;

		// Token: 0x04001CDB RID: 7387
		[Header("Bindings")]
		public Collider BoundaryCollider;

		// Token: 0x04001CDC RID: 7388
		public List<TrailRenderer> Trails;

		// Token: 0x04001CDD RID: 7389
		public MMFeedbacks TeleportFeedbacks;

		// Token: 0x04001CDE RID: 7390
		protected Vector2 _input;

		// Token: 0x04001CDF RID: 7391
		protected Vector3 _rotationAxis = Vector3.up;

		// Token: 0x04001CE0 RID: 7392
		protected const string _horizontalAxis = "Horizontal";

		// Token: 0x04001CE1 RID: 7393
		protected const string _verticalAxis = "Vertical";

		// Token: 0x04001CE2 RID: 7394
		protected Bounds _bounds;

		// Token: 0x04001CE3 RID: 7395
		protected Vector3 _thisPosition;

		// Token: 0x04001CE4 RID: 7396
		protected Vector3 _newPosition;

		// Token: 0x04001CE5 RID: 7397
		protected float _trailTime;
	}
}
