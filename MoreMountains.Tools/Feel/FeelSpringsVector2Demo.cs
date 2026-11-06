using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000387 RID: 903
	[AddComponentMenu("")]
	public class FeelSpringsVector2Demo : MonoBehaviour
	{
		// Token: 0x060018AD RID: 6317 RVA: 0x00068E04 File Offset: 0x00067004
		protected virtual void OnEnable()
		{
			this.SpringX.CurrentValue = 0f;
			this.SpringX.TargetValue = 0f;
			this.SpringX.Velocity = 0f;
			this.SpringY.CurrentValue = 0f;
			this.SpringY.TargetValue = 0f;
			this.SpringY.Velocity = 0f;
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x00068E71 File Offset: 0x00067071
		public virtual void RandomMove()
		{
			this.SpringX.MoveTo(Random.Range(-1f, 1f));
			this.SpringY.MoveTo(Random.Range(-1f, 1f));
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x00068EA8 File Offset: 0x000670A8
		public virtual void RandomBump()
		{
			float value = this.BumpAmountSlider.value;
			this.SpringX.BumpRandom(-value, value);
			this.SpringY.BumpRandom(-value, value);
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x00068EE0 File Offset: 0x000670E0
		protected virtual void Update()
		{
			this.SpringX.Damping = this.DampingXSlider.value;
			this.SpringY.Damping = this.DampingYSlider.value;
			this.SpringX.Frequency = this.FrequencyXSlider.value;
			this.SpringY.Frequency = this.FrequencyYSlider.value;
			this.SpringX.UpdateSpringValue(Time.deltaTime);
			this.SpringY.UpdateSpringValue(Time.deltaTime);
			this._newPosition = this.MovingObject.transform.localPosition;
			this._newPosition.x = MMMaths.Remap(this.SpringX.CurrentValue, -1f, 1f, -this._range, this._range);
			this._newPosition.y = MMMaths.Remap(this.SpringY.CurrentValue, -1f, 1f, -this._range, this._range);
			this.MovingObject.transform.localPosition = this._newPosition;
		}

		// Token: 0x04001CC2 RID: 7362
		[Header("Spring")]
		public MMSpringFloat SpringX;

		// Token: 0x04001CC3 RID: 7363
		public MMSpringFloat SpringY;

		// Token: 0x04001CC4 RID: 7364
		[Header("Bindings")]
		public FeelSpringsDemoSlider DampingXSlider;

		// Token: 0x04001CC5 RID: 7365
		public FeelSpringsDemoSlider FrequencyXSlider;

		// Token: 0x04001CC6 RID: 7366
		public FeelSpringsDemoSlider DampingYSlider;

		// Token: 0x04001CC7 RID: 7367
		public FeelSpringsDemoSlider FrequencyYSlider;

		// Token: 0x04001CC8 RID: 7368
		public FeelSpringsDemoSlider BumpAmountSlider;

		// Token: 0x04001CC9 RID: 7369
		public Transform MovingObject;

		// Token: 0x04001CCA RID: 7370
		protected Vector3 _newPosition;

		// Token: 0x04001CCB RID: 7371
		protected float _range = 0.375f;
	}
}
