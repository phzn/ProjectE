using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000283 RID: 643
	[AddComponentMenu("More Mountains/Springs/MMSpringRotationAround")]
	public class MMSpringRotationAround : MMSpringFloatComponent<Transform>
	{
		// Token: 0x0600124D RID: 4685 RVA: 0x0004F65C File Offset: 0x0004D85C
		protected override void Initialization()
		{
			base.Initialization();
			this._currentAngle = 0f;
			this._initialPosition = this.Target.position;
			this._initialRotation = this.Target.rotation;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600124E RID: 4686 RVA: 0x0004F691 File Offset: 0x0004D891
		// (set) Token: 0x0600124F RID: 4687 RVA: 0x0004F69C File Offset: 0x0004D89C
		public override float TargetFloat
		{
			get
			{
				return this._currentAngle;
			}
			set
			{
				this._currentAngle = value;
				this.Target.position = this._initialPosition;
				this.Target.rotation = this._initialRotation;
				this.Target.position = MMMaths.RotatePointAroundPivot(this.Target.position, this.RotationCenter.position, this._currentAngle * this.RotationAxis);
				if (this.FaceRotationCenter)
				{
					this.Target.LookAt(this.RotationCenter);
				}
			}
		}

		// Token: 0x040014D3 RID: 5331
		public Transform RotationCenter;

		// Token: 0x040014D4 RID: 5332
		public Vector3 RotationAxis = Vector3.up;

		// Token: 0x040014D5 RID: 5333
		public bool FaceRotationCenter = true;

		// Token: 0x040014D6 RID: 5334
		protected float _currentAngle;

		// Token: 0x040014D7 RID: 5335
		protected Vector3 _initialPosition;

		// Token: 0x040014D8 RID: 5336
		protected Quaternion _initialRotation;
	}
}
