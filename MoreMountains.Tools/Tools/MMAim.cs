using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000062 RID: 98
	[Serializable]
	public class MMAim
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000A4FE File Offset: 0x000086FE
		// (set) Token: 0x06000222 RID: 546 RVA: 0x0000A506 File Offset: 0x00008706
		public virtual Vector3 CurrentPosition { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000A50F File Offset: 0x0000870F
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000A517 File Offset: 0x00008717
		public virtual Vector2 PrimaryMovement { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000A520 File Offset: 0x00008720
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000A528 File Offset: 0x00008728
		public virtual Vector2 SecondaryMovement { get; set; }

		// Token: 0x06000227 RID: 551 RVA: 0x0000A534 File Offset: 0x00008734
		public virtual void Initialization()
		{
			if (this.RotationMode == MMAim.RotationModes.Strict4Directions)
			{
				this._possibleAngleValues = new float[5];
				this._possibleAngleValues[0] = -180f;
				this._possibleAngleValues[1] = -90f;
				this._possibleAngleValues[2] = 0f;
				this._possibleAngleValues[3] = 90f;
				this._possibleAngleValues[4] = 180f;
			}
			if (this.RotationMode == MMAim.RotationModes.Strict8Directions)
			{
				this._possibleAngleValues = new float[9];
				this._possibleAngleValues[0] = -180f;
				this._possibleAngleValues[1] = -135f;
				this._possibleAngleValues[2] = -90f;
				this._possibleAngleValues[3] = -45f;
				this._possibleAngleValues[4] = 0f;
				this._possibleAngleValues[5] = 45f;
				this._possibleAngleValues[6] = 90f;
				this._possibleAngleValues[7] = 135f;
				this._possibleAngleValues[8] = 180f;
			}
			this._mainCamera = Camera.main;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000A630 File Offset: 0x00008830
		public virtual Vector2 GetCurrentAim()
		{
			switch (this.AimControl)
			{
			case MMAim.AimControls.Off:
				this._currentAim = Vector2.zero;
				break;
			case MMAim.AimControls.PrimaryMovement:
				this._currentAim = this.PrimaryMovement;
				break;
			case MMAim.AimControls.SecondaryMovement:
				this._currentAim = this.SecondaryMovement;
				break;
			case MMAim.AimControls.Mouse:
				this._mousePosition = Input.mousePosition;
				this._mousePosition.z = 10f;
				this._direction = this._mainCamera.ScreenToWorldPoint(this._mousePosition);
				this._direction.z = this.CurrentPosition.z;
				this._currentAim = this._direction - this.CurrentPosition;
				break;
			case MMAim.AimControls.Script:
				break;
			default:
				this._currentAim = Vector2.zero;
				break;
			}
			this.CurrentAngle = Mathf.Atan2(this._currentAim.y, this._currentAim.x) * 57.29578f;
			if (this.CurrentAngle < this.MinimumAngle || this.CurrentAngle > this.MaximumAngle)
			{
				float num = Mathf.DeltaAngle(this.CurrentAngle, this.MinimumAngle);
				float num2 = Mathf.DeltaAngle(this.CurrentAngle, this.MaximumAngle);
				this.CurrentAngle = ((Mathf.Abs(num) < Mathf.Abs(num2)) ? this.MinimumAngle : this.MaximumAngle);
			}
			if (this.RotationMode == MMAim.RotationModes.Strict4Directions || this.RotationMode == MMAim.RotationModes.Strict8Directions)
			{
				this.CurrentAngle = MMMaths.RoundToClosest(this.CurrentAngle, this._possibleAngleValues, false);
			}
			this.CurrentAngle = Mathf.Clamp(this.CurrentAngle, this.MinimumAngle, this.MaximumAngle);
			this._currentAim = ((this._currentAim.magnitude == 0f) ? Vector2.zero : MMMaths.RotateVector2(Vector2.right, this.CurrentAngle));
			return this._currentAim;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000A821 File Offset: 0x00008A21
		public virtual void SetAim(Vector2 newAim)
		{
			this._currentAim = newAim;
		}

		// Token: 0x040002F7 RID: 759
		[Header("Control Mode")]
		[MMInformation("Pick a control mode : mouse (aims towards the pointer), primary movement (you'll aim towards the current input direction), or secondary movement (aims towards a second input axis, think twin stick shooters), and set minimum and maximum angles.", MMInformationAttribute.InformationType.Info, false)]
		public MMAim.AimControls AimControl = MMAim.AimControls.SecondaryMovement;

		// Token: 0x040002F8 RID: 760
		public MMAim.RotationModes RotationMode;

		// Token: 0x040002F9 RID: 761
		[Header("Limits")]
		[Range(-180f, 180f)]
		public float MinimumAngle = -180f;

		// Token: 0x040002FA RID: 762
		[Range(-180f, 180f)]
		public float MaximumAngle = 180f;

		// Token: 0x040002FB RID: 763
		[MMReadOnly]
		public float CurrentAngle;

		// Token: 0x040002FF RID: 767
		protected float[] _possibleAngleValues;

		// Token: 0x04000300 RID: 768
		protected Vector3 _currentAim = Vector3.zero;

		// Token: 0x04000301 RID: 769
		protected Vector3 _direction;

		// Token: 0x04000302 RID: 770
		protected Vector3 _mousePosition;

		// Token: 0x04000303 RID: 771
		protected Vector2 _inputSystemMousePosition;

		// Token: 0x04000304 RID: 772
		protected Camera _mainCamera;

		// Token: 0x020003CF RID: 975
		public enum AimControls
		{
			// Token: 0x04001DFD RID: 7677
			Off,
			// Token: 0x04001DFE RID: 7678
			PrimaryMovement,
			// Token: 0x04001DFF RID: 7679
			SecondaryMovement,
			// Token: 0x04001E00 RID: 7680
			Mouse,
			// Token: 0x04001E01 RID: 7681
			Script
		}

		// Token: 0x020003D0 RID: 976
		public enum RotationModes
		{
			// Token: 0x04001E03 RID: 7683
			Free,
			// Token: 0x04001E04 RID: 7684
			Strict4Directions,
			// Token: 0x04001E05 RID: 7685
			Strict8Directions
		}
	}
}
