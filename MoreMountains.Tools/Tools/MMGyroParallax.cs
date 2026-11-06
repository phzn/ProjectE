using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000025 RID: 37
	public class MMGyroParallax : MMGyroscope
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00004805 File Offset: 0x00002A05
		protected override void Start()
		{
			base.Start();
			this.Initialization();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004814 File Offset: 0x00002A14
		public virtual void Initialization()
		{
			foreach (MMGyroCam mmgyroCam in this.Cams)
			{
				mmgyroCam.InitialAngles = mmgyroCam.Cam.transform.localEulerAngles;
				mmgyroCam.InitialPosition = mmgyroCam.Cam.transform.position;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000488C File Offset: 0x00002A8C
		protected override void Update()
		{
			base.Update();
			this.MoveCameras();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000489C File Offset: 0x00002A9C
		protected virtual void MoveCameras()
		{
			foreach (MMGyroCam mmgyroCam in this.Cams)
			{
				float num = 0f;
				float num2 = 0f;
				Vector3 lerpedCalibratedGyroscopeGravity = MMGyroscope.LerpedCalibratedGyroscopeGravity;
				if (lerpedCalibratedGyroscopeGravity.x > 0f)
				{
					num = MMMaths.Remap(MMGyroscope.LerpedCalibratedGyroscopeGravity.x, 0.5f, 0f, mmgyroCam.MinRotation.x, 0f);
				}
				if (lerpedCalibratedGyroscopeGravity.x < 0f)
				{
					num = MMMaths.Remap(MMGyroscope.LerpedCalibratedGyroscopeGravity.x, 0f, -0.5f, 0f, mmgyroCam.MaxRotation.x);
				}
				if (lerpedCalibratedGyroscopeGravity.y > 0f)
				{
					num2 = MMMaths.Remap(MMGyroscope.LerpedCalibratedGyroscopeGravity.y, 0.5f, 0f, mmgyroCam.MinRotation.y, 0f);
				}
				if (lerpedCalibratedGyroscopeGravity.y < 0f)
				{
					num2 = MMMaths.Remap(MMGyroscope.LerpedCalibratedGyroscopeGravity.y, 0f, -0.5f, 0f, mmgyroCam.MaxRotation.y);
				}
				Transform transform = mmgyroCam.Cam.transform;
				if (mmgyroCam.AnimatedPosition != null)
				{
					this._newAngles = mmgyroCam.AnimatedPosition.localEulerAngles;
					this._newAngles.x = this._newAngles.x + num;
					this._newAngles.z = this._newAngles.z + num2;
					transform.position = mmgyroCam.AnimatedPosition.position;
					transform.localEulerAngles = mmgyroCam.AnimatedPosition.localEulerAngles;
				}
				else
				{
					this._newAngles = mmgyroCam.InitialAngles;
					this._newAngles.x = this._newAngles.x + num;
					this._newAngles.z = this._newAngles.z + num2;
					transform.position = mmgyroCam.InitialPosition;
					transform.localEulerAngles = mmgyroCam.InitialAngles;
				}
				Transform transform2 = mmgyroCam.RotationCenter.transform;
				transform.RotateAround(transform2.position, transform2.up, num);
				transform.RotateAround(transform2.position, transform2.right, num2);
				if (mmgyroCam.Cam.LookAt == null)
				{
					if (mmgyroCam.LookAt != null)
					{
						transform.LookAt(mmgyroCam.LookAt);
					}
					else
					{
						transform.LookAt(mmgyroCam.RotationCenter);
					}
				}
			}
		}

		// Token: 0x040000F1 RID: 241
		[Header("Cameras")]
		public List<MMGyroCam> Cams;

		// Token: 0x040000F2 RID: 242
		protected Vector3 _newAngles;
	}
}
