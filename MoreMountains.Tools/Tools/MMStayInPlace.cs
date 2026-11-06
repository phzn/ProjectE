using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200006D RID: 109
	[AddComponentMenu("More Mountains/Tools/Movement/MMStayInPlace")]
	public class MMStayInPlace : MonoBehaviour
	{
		// Token: 0x0600028D RID: 653 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000C7E0 File Offset: 0x0000A9E0
		protected virtual void Initialization()
		{
			this._initialPosition = ((this.Space == MMStayInPlace.Spaces.World) ? base.transform.position : base.transform.localPosition);
			this._initialRotation = ((this.Space == MMStayInPlace.Spaces.World) ? base.transform.rotation : base.transform.localRotation);
			this._initialScale = ((this.Space == MMStayInPlace.Spaces.World) ? base.transform.position : base.transform.localScale);
			if (this.OverridePosition)
			{
				this._initialPosition = this.OverridePositionValue;
			}
			if (this.OverrideRotation)
			{
				this._initialRotation = Quaternion.Euler(this.OverrideRotationValue);
			}
			if (this.OverrideScale)
			{
				this._initialScale = this.OverrideScaleValue;
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000C8A0 File Offset: 0x0000AAA0
		protected virtual void Update()
		{
			if (this.UpdateMode == MMStayInPlace.UpdateModes.Update)
			{
				this.StayInPlace();
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
		protected virtual void FixedUpdate()
		{
			if (this.UpdateMode == MMStayInPlace.UpdateModes.FixedUpdate)
			{
				this.StayInPlace();
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000C8C1 File Offset: 0x0000AAC1
		protected virtual void LateUpdate()
		{
			if (this.UpdateMode == MMStayInPlace.UpdateModes.LateUpdate)
			{
				this.StayInPlace();
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000C8D4 File Offset: 0x0000AAD4
		protected virtual void StayInPlace()
		{
			if (this.Space == MMStayInPlace.Spaces.World)
			{
				if (this.FixedPosition)
				{
					base.transform.position = this._initialPosition;
				}
				if (this.FixedRotation)
				{
					base.transform.rotation = this._initialRotation;
				}
			}
			else
			{
				if (this.FixedPosition)
				{
					base.transform.localPosition = this._initialPosition;
				}
				if (this.FixedRotation)
				{
					base.transform.localRotation = this._initialRotation;
				}
			}
			if (this.FixedScale)
			{
				base.transform.localScale = this._initialScale;
			}
		}

		// Token: 0x040003B4 RID: 948
		[Header("Modes")]
		public MMStayInPlace.UpdateModes UpdateMode = MMStayInPlace.UpdateModes.LateUpdate;

		// Token: 0x040003B5 RID: 949
		public MMStayInPlace.Spaces Space;

		// Token: 0x040003B6 RID: 950
		[Header("Attributes")]
		public bool FixedPosition = true;

		// Token: 0x040003B7 RID: 951
		public bool FixedRotation = true;

		// Token: 0x040003B8 RID: 952
		public bool FixedScale = true;

		// Token: 0x040003B9 RID: 953
		[Header("Overrides")]
		public bool OverridePosition;

		// Token: 0x040003BA RID: 954
		[MMCondition("OverridePosition", true)]
		public Vector3 OverridePositionValue;

		// Token: 0x040003BB RID: 955
		public bool OverrideRotation;

		// Token: 0x040003BC RID: 956
		[MMCondition("OverrideRotation", true)]
		public Vector3 OverrideRotationValue;

		// Token: 0x040003BD RID: 957
		public bool OverrideScale;

		// Token: 0x040003BE RID: 958
		[MMCondition("OverrideScale", true)]
		public Vector3 OverrideScaleValue;

		// Token: 0x040003BF RID: 959
		protected Vector3 _initialPosition;

		// Token: 0x040003C0 RID: 960
		protected Quaternion _initialRotation;

		// Token: 0x040003C1 RID: 961
		protected Vector3 _initialScale;

		// Token: 0x020003E4 RID: 996
		public enum Spaces
		{
			// Token: 0x04001E51 RID: 7761
			World,
			// Token: 0x04001E52 RID: 7762
			Local
		}

		// Token: 0x020003E5 RID: 997
		public enum UpdateModes
		{
			// Token: 0x04001E54 RID: 7764
			Update,
			// Token: 0x04001E55 RID: 7765
			FixedUpdate,
			// Token: 0x04001E56 RID: 7766
			LateUpdate
		}
	}
}
