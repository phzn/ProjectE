using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000117 RID: 279
	[AddComponentMenu("More Mountains/Tools/Property Controllers/TransformController")]
	public class TransformController : MonoBehaviour
	{
		// Token: 0x06000634 RID: 1588 RVA: 0x0001F830 File Offset: 0x0001DA30
		protected virtual void Update()
		{
			this._position = base.transform.position;
			this._localPosition = base.transform.localPosition;
			this._rotation = base.transform.eulerAngles;
			this._localRotation = base.transform.localEulerAngles;
			this._scale = base.transform.localScale;
			if (this.ControlPositionX)
			{
				this._position.x = this.PositionX;
				base.transform.position = this._position;
			}
			if (this.ControlPositionY)
			{
				this._position.y = this.PositionY;
				base.transform.position = this._position;
			}
			if (this.ControlPositionZ)
			{
				this._position.z = this.PositionZ;
				base.transform.position = this._position;
			}
			if (this.ControlLocalPositionX)
			{
				this._localPosition.x = this.LocalPositionX;
				base.transform.localPosition = this._localPosition;
			}
			if (this.ControlLocalPositionY)
			{
				this._localPosition.y = this.LocalPositionY;
				base.transform.localPosition = this._localPosition;
			}
			if (this.ControlLocalPositionZ)
			{
				this._localPosition.z = this.LocalPositionZ;
				base.transform.localPosition = this._localPosition;
			}
			if (this.ControlRotationX)
			{
				this._rotation.x = this.RotationX;
				base.transform.eulerAngles = this._rotation;
			}
			if (this.ControlRotationY)
			{
				this._rotation.y = this.RotationY;
				base.transform.eulerAngles = this._rotation;
			}
			if (this.ControlRotationZ)
			{
				this._rotation.z = this.RotationZ;
				base.transform.eulerAngles = this._rotation;
			}
			if (this.ControlLocalRotationX)
			{
				this._localRotation.x = this.LocalRotationX;
				base.transform.localEulerAngles = this._localRotation;
			}
			if (this.ControlLocalRotationY)
			{
				this._localRotation.y = this.LocalRotationY;
				base.transform.localEulerAngles = this._localRotation;
			}
			if (this.ControlLocalRotationZ)
			{
				this._localRotation.z = this.LocalRotationZ;
				base.transform.localEulerAngles = this._localRotation;
			}
			if (this.ControlScaleX)
			{
				this._scale.x = this.ScaleX;
				base.transform.localScale = this._scale;
			}
			if (this.ControlScaleY)
			{
				this._scale.y = this.ScaleY;
				base.transform.localScale = this._scale;
			}
			if (this.ControlScaleZ)
			{
				this._scale.z = this.ScaleZ;
				base.transform.localScale = this._scale;
			}
		}

		// Token: 0x040007C7 RID: 1991
		[Header("Position")]
		public bool ControlPositionX;

		// Token: 0x040007C8 RID: 1992
		[MMCondition("ControlPositionX", true)]
		public float PositionX;

		// Token: 0x040007C9 RID: 1993
		public bool ControlPositionY;

		// Token: 0x040007CA RID: 1994
		[MMCondition("ControlPositionY", true)]
		public float PositionY;

		// Token: 0x040007CB RID: 1995
		public bool ControlPositionZ;

		// Token: 0x040007CC RID: 1996
		[MMCondition("ControlPositionZ", true)]
		public float PositionZ;

		// Token: 0x040007CD RID: 1997
		[Header("Local Position")]
		public bool ControlLocalPositionX;

		// Token: 0x040007CE RID: 1998
		[MMCondition("ControlLocalPositionX", true)]
		public float LocalPositionX;

		// Token: 0x040007CF RID: 1999
		public bool ControlLocalPositionY;

		// Token: 0x040007D0 RID: 2000
		[MMCondition("ControlLocalPositionY", true)]
		public float LocalPositionY;

		// Token: 0x040007D1 RID: 2001
		public bool ControlLocalPositionZ;

		// Token: 0x040007D2 RID: 2002
		[MMCondition("ControlLocalPositionZ", true)]
		public float LocalPositionZ;

		// Token: 0x040007D3 RID: 2003
		[Header("Rotation")]
		public bool ControlRotationX;

		// Token: 0x040007D4 RID: 2004
		[MMCondition("ControlRotationX", true)]
		public float RotationX;

		// Token: 0x040007D5 RID: 2005
		public bool ControlRotationY;

		// Token: 0x040007D6 RID: 2006
		[MMCondition("ControlRotationY", true)]
		public float RotationY;

		// Token: 0x040007D7 RID: 2007
		public bool ControlRotationZ;

		// Token: 0x040007D8 RID: 2008
		[MMCondition("ControlRotationZ", true)]
		public float RotationZ;

		// Token: 0x040007D9 RID: 2009
		[Header("Local Rotation")]
		public bool ControlLocalRotationX;

		// Token: 0x040007DA RID: 2010
		[MMCondition("ControlLocalRotationX", true)]
		public float LocalRotationX;

		// Token: 0x040007DB RID: 2011
		public bool ControlLocalRotationY;

		// Token: 0x040007DC RID: 2012
		[MMCondition("ControlLocalRotationY", true)]
		public float LocalRotationY;

		// Token: 0x040007DD RID: 2013
		public bool ControlLocalRotationZ;

		// Token: 0x040007DE RID: 2014
		[MMCondition("ControlLocalRotationZ", true)]
		public float LocalRotationZ;

		// Token: 0x040007DF RID: 2015
		[Header("Scale")]
		public bool ControlScaleX;

		// Token: 0x040007E0 RID: 2016
		[MMCondition("ControlScaleX", true)]
		public float ScaleX;

		// Token: 0x040007E1 RID: 2017
		public bool ControlScaleY;

		// Token: 0x040007E2 RID: 2018
		[MMCondition("ControlScaleY", true)]
		public float ScaleY;

		// Token: 0x040007E3 RID: 2019
		public bool ControlScaleZ;

		// Token: 0x040007E4 RID: 2020
		[MMCondition("ControlScaleZ", true)]
		public float ScaleZ;

		// Token: 0x040007E5 RID: 2021
		protected Vector3 _position;

		// Token: 0x040007E6 RID: 2022
		protected Vector3 _localPosition;

		// Token: 0x040007E7 RID: 2023
		protected Vector3 _rotation;

		// Token: 0x040007E8 RID: 2024
		protected Vector3 _localRotation;

		// Token: 0x040007E9 RID: 2025
		protected Vector3 _scale;
	}
}
