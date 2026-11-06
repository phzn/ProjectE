using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000381 RID: 897
	[AddComponentMenu("")]
	public class FeelSpringsAdvancedFloatDemo : MonoBehaviour
	{
		// Token: 0x0600188F RID: 6287 RVA: 0x000683FB File Offset: 0x000665FB
		protected virtual void Awake()
		{
			this._newPosition = this.MovingObject.transform.localPosition;
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x00068414 File Offset: 0x00066614
		public virtual void RandomMove()
		{
			this._newPosition.x = Random.Range(-this._range, this._range);
			this.PositionSpring.MoveTo(this._newPosition);
			this.ScaleSpring.BumpRandom();
			this.RotationSpring.BumpRandom();
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x00068468 File Offset: 0x00066668
		public virtual void RandomBump()
		{
			this._newBump.x = 0f;
			this._newBump.y = 0f;
			this._newBump.z = 1000f * this.BumpAmountSlider.value;
			this.RotationSpring.Bump(this._newBump);
			this._newBump.x = 10f * this.BumpAmountSlider.value;
			this._newBump.y = 10f * this.BumpAmountSlider.value;
			this._newBump.z = 0f;
			this.ScaleSpring.Bump(this._newBump);
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x0006851C File Offset: 0x0006671C
		protected virtual void Update()
		{
			this.PositionSpring.SpringVector3.SetDamping(this.PositionDampingSlider.value * Vector3.one);
			this.PositionSpring.SpringVector3.SetFrequency(this.PositionFrequencySlider.value * Vector3.one);
			this.RotationSpring.SpringVector3.SetDamping(this.RotationDampingSlider.value * Vector3.one);
			this.RotationSpring.SpringVector3.SetFrequency(this.RotationFrequencySlider.value * Vector3.one);
			this.ScaleSpring.SpringVector3.SetDamping(this.ScaleDampingSlider.value * Vector3.one);
			this.ScaleSpring.SpringVector3.SetFrequency(this.ScaleFrequencySlider.value * Vector3.one);
		}

		// Token: 0x04001C9A RID: 7322
		[Header("Bindings")]
		public MMSpringPosition PositionSpring;

		// Token: 0x04001C9B RID: 7323
		public MMSpringRotation RotationSpring;

		// Token: 0x04001C9C RID: 7324
		public MMSpringScale ScaleSpring;

		// Token: 0x04001C9D RID: 7325
		public FeelSpringsDemoSlider PositionDampingSlider;

		// Token: 0x04001C9E RID: 7326
		public FeelSpringsDemoSlider PositionFrequencySlider;

		// Token: 0x04001C9F RID: 7327
		public FeelSpringsDemoSlider RotationDampingSlider;

		// Token: 0x04001CA0 RID: 7328
		public FeelSpringsDemoSlider RotationFrequencySlider;

		// Token: 0x04001CA1 RID: 7329
		public FeelSpringsDemoSlider ScaleDampingSlider;

		// Token: 0x04001CA2 RID: 7330
		public FeelSpringsDemoSlider ScaleFrequencySlider;

		// Token: 0x04001CA3 RID: 7331
		public FeelSpringsDemoSlider BumpAmountSlider;

		// Token: 0x04001CA4 RID: 7332
		public Transform MovingObject;

		// Token: 0x04001CA5 RID: 7333
		protected Vector3 _newPosition = Vector3.zero;

		// Token: 0x04001CA6 RID: 7334
		protected Vector3 _newBump = Vector3.zero;

		// Token: 0x04001CA7 RID: 7335
		protected float _range = 0.375f;
	}
}
