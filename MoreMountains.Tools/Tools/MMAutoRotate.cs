using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000063 RID: 99
	[AddComponentMenu("More Mountains/Tools/Movement/MMAutoRotate")]
	public class MMAutoRotate : MonoBehaviour
	{
		// Token: 0x0600022B RID: 555 RVA: 0x0000A85F File Offset: 0x00008A5F
		protected virtual void Start()
		{
			this._rotationPlane = default(Plane);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000A86D File Offset: 0x00008A6D
		protected virtual void Update()
		{
			if (this.UpdateMode == MMAutoRotate.UpdateModes.Update)
			{
				this.Rotate();
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000A87D File Offset: 0x00008A7D
		protected virtual void FixedUpdate()
		{
			if (this.UpdateMode == MMAutoRotate.UpdateModes.FixedUpdate)
			{
				this.Rotate();
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000A88E File Offset: 0x00008A8E
		protected virtual void LateUpdate()
		{
			if (this.UpdateMode == MMAutoRotate.UpdateModes.LateUpdate)
			{
				this.Rotate();
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000A89F File Offset: 0x00008A9F
		public virtual void Rotate(bool status)
		{
			this.Rotating = status;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000A8A8 File Offset: 0x00008AA8
		public virtual void Orbit(bool status)
		{
			this.Orbiting = status;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000A8B4 File Offset: 0x00008AB4
		protected virtual void Rotate()
		{
			if (this.Rotating)
			{
				base.transform.Rotate(this.RotationSpeed * Time.deltaTime, this.RotationSpace);
			}
			if (this.Orbiting)
			{
				this._orbitCenter = this.OrbitCenterTransform.transform.position + this.OrbitCenterOffset;
				if (this.AdditiveOrbitRotation)
				{
					this._worldRotationAxis = this.OrbitCenterTransform.TransformDirection(this.OrbitRotationAxis);
				}
				else
				{
					this._worldRotationAxis = this.OrbitRotationAxis;
				}
				this._rotationPlane.SetNormalAndPosition(this._worldRotationAxis.normalized, this._orbitCenter);
				this._snappedPosition = this._rotationPlane.ClosestPointOnPlane(base.transform.position);
				this._radius = this.OrbitRadius * Vector3.Normalize(this._snappedPosition - this._orbitCenter);
				this._newRotation = Quaternion.AngleAxis(this.OrbitRotationSpeed * Time.deltaTime, this._worldRotationAxis);
				this._desiredOrbitPosition = this._orbitCenter + this._newRotation * this._radius;
				base.transform.position = Vector3.Lerp(base.transform.position, this._desiredOrbitPosition, this.OrbitCorrectionSpeed * Time.deltaTime);
				this._previousPosition = this._desiredOrbitPosition;
			}
		}

		// Token: 0x04000305 RID: 773
		[Header("Rotation")]
		public bool Rotating = true;

		// Token: 0x04000306 RID: 774
		[MMCondition("Rotating", true)]
		public Space RotationSpace = Space.Self;

		// Token: 0x04000307 RID: 775
		public MMAutoRotate.UpdateModes UpdateMode;

		// Token: 0x04000308 RID: 776
		[MMCondition("Rotating", true)]
		public Vector3 RotationSpeed = new Vector3(100f, 0f, 0f);

		// Token: 0x04000309 RID: 777
		[Header("Orbit")]
		public bool Orbiting;

		// Token: 0x0400030A RID: 778
		[MMCondition("Orbiting", true)]
		public bool AdditiveOrbitRotation;

		// Token: 0x0400030B RID: 779
		[MMCondition("Orbiting", true)]
		public Transform OrbitCenterTransform;

		// Token: 0x0400030C RID: 780
		[MMCondition("Orbiting", true)]
		public Vector3 OrbitCenterOffset = Vector3.zero;

		// Token: 0x0400030D RID: 781
		[MMCondition("Orbiting", true)]
		public Vector3 OrbitRotationAxis = new Vector3(0f, 1f, 0f);

		// Token: 0x0400030E RID: 782
		[MMCondition("Orbiting", true)]
		public float OrbitRotationSpeed = 10f;

		// Token: 0x0400030F RID: 783
		[MMCondition("Orbiting", true)]
		public float OrbitRadius = 3f;

		// Token: 0x04000310 RID: 784
		[MMCondition("Orbiting", true)]
		public float OrbitCorrectionSpeed = 10f;

		// Token: 0x04000311 RID: 785
		[Header("Settings")]
		public bool DrawGizmos = true;

		// Token: 0x04000312 RID: 786
		[MMCondition("DrawGizmos", true)]
		public Color OrbitPlaneColor = new Color(54f, 169f, 225f, 0.02f);

		// Token: 0x04000313 RID: 787
		[MMCondition("DrawGizmos", true)]
		public Color OrbitLineColor = new Color(225f, 225f, 225f, 0.1f);

		// Token: 0x04000314 RID: 788
		[HideInInspector]
		public Vector3 _orbitCenter;

		// Token: 0x04000315 RID: 789
		[HideInInspector]
		public Vector3 _worldRotationAxis;

		// Token: 0x04000316 RID: 790
		[HideInInspector]
		public Plane _rotationPlane;

		// Token: 0x04000317 RID: 791
		[HideInInspector]
		public Vector3 _snappedPosition;

		// Token: 0x04000318 RID: 792
		[HideInInspector]
		public Vector3 _radius;

		// Token: 0x04000319 RID: 793
		protected Quaternion _newRotation;

		// Token: 0x0400031A RID: 794
		protected Vector3 _desiredOrbitPosition;

		// Token: 0x0400031B RID: 795
		private Vector3 _previousPosition;

		// Token: 0x020003D1 RID: 977
		public enum UpdateModes
		{
			// Token: 0x04001E07 RID: 7687
			Update,
			// Token: 0x04001E08 RID: 7688
			LateUpdate,
			// Token: 0x04001E09 RID: 7689
			FixedUpdate
		}
	}
}
