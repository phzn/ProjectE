using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000388 RID: 904
	[AddComponentMenu("")]
	public class FeelSpringsVector3Demo : MonoBehaviour
	{
		// Token: 0x060018B2 RID: 6322 RVA: 0x00069008 File Offset: 0x00067208
		protected virtual void OnEnable()
		{
			this.SpringX.CurrentValue = 0f;
			this.SpringX.TargetValue = 0f;
			this.SpringX.Velocity = 0f;
			this.SpringY.CurrentValue = 0f;
			this.SpringY.TargetValue = 0f;
			this.SpringY.Velocity = 0f;
			this.SpringZ.CurrentValue = 0f;
			this.SpringZ.TargetValue = 0f;
			this.SpringZ.Velocity = 0f;
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x000690A8 File Offset: 0x000672A8
		public virtual void RandomMove()
		{
			this.SpringX.MoveTo(Random.Range(-1f, 1f));
			this.SpringY.MoveTo(Random.Range(-1f, 1f));
			this.SpringZ.MoveTo(Random.Range(-1f, 1f));
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x00069104 File Offset: 0x00067304
		public virtual void RandomBump()
		{
			float value = this.BumpAmountSlider.value;
			this.SpringX.BumpRandom(-value, value);
			this.SpringY.BumpRandom(-value, value);
			this.SpringZ.BumpRandom(-value, value);
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x00069148 File Offset: 0x00067348
		protected virtual void Update()
		{
			this.SpringX.Damping = this.DampingXSlider.value;
			this.SpringY.Damping = this.DampingYSlider.value;
			this.SpringX.Frequency = this.FrequencyXSlider.value;
			this.SpringY.Frequency = this.FrequencyYSlider.value;
			this.SpringZ.Damping = this.DampingZSlider.value;
			this.SpringZ.Frequency = this.FrequencyZSlider.value;
			this.SpringX.UpdateSpringValue(Time.deltaTime);
			this.SpringY.UpdateSpringValue(Time.deltaTime);
			this.SpringZ.UpdateSpringValue(Time.deltaTime);
			this._newPosition = this.MovingObject.transform.localPosition;
			this._newPosition.x = MMMaths.Remap(this.SpringX.CurrentValue, -1f, 1f, -this._range, this._range);
			this._newPosition.y = MMMaths.Remap(this.SpringY.CurrentValue, -1f, 1f, -this._range, this._range) + 1f;
			this._newPosition.z = MMMaths.Remap(this.SpringZ.CurrentValue, -1f, 1f, -this._range, this._range);
			this.MovingObject.transform.localPosition = this._newPosition;
		}

		// Token: 0x04001CCC RID: 7372
		[Header("Spring")]
		public MMSpringFloat SpringX;

		// Token: 0x04001CCD RID: 7373
		public MMSpringFloat SpringY;

		// Token: 0x04001CCE RID: 7374
		public MMSpringFloat SpringZ;

		// Token: 0x04001CCF RID: 7375
		[Header("Bindings")]
		public FeelSpringsDemoSlider DampingXSlider;

		// Token: 0x04001CD0 RID: 7376
		public FeelSpringsDemoSlider FrequencyXSlider;

		// Token: 0x04001CD1 RID: 7377
		public FeelSpringsDemoSlider DampingYSlider;

		// Token: 0x04001CD2 RID: 7378
		public FeelSpringsDemoSlider FrequencyYSlider;

		// Token: 0x04001CD3 RID: 7379
		public FeelSpringsDemoSlider DampingZSlider;

		// Token: 0x04001CD4 RID: 7380
		public FeelSpringsDemoSlider FrequencyZSlider;

		// Token: 0x04001CD5 RID: 7381
		public FeelSpringsDemoSlider BumpAmountSlider;

		// Token: 0x04001CD6 RID: 7382
		public Transform MovingObject;

		// Token: 0x04001CD7 RID: 7383
		protected Vector3 _newPosition;

		// Token: 0x04001CD8 RID: 7384
		protected float _range = 0.375f;
	}
}
