using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000386 RID: 902
	[AddComponentMenu("")]
	public class FeelSpringsFloatDemo : MonoBehaviour
	{
		// Token: 0x060018A8 RID: 6312 RVA: 0x00068CD0 File Offset: 0x00066ED0
		protected virtual void OnEnable()
		{
			this.FloatSpring.CurrentValue = 0f;
			this.FloatSpring.TargetValue = 0f;
			this.FloatSpring.Velocity = 0f;
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x00068D02 File Offset: 0x00066F02
		public virtual void RandomMove()
		{
			this.FloatSpring.MoveTo(Random.Range(-1f, 1f));
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x00068D20 File Offset: 0x00066F20
		public virtual void RandomBump()
		{
			float value = this.BumpAmountSlider.value;
			this.FloatSpring.BumpRandom(-value, value);
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x00068D48 File Offset: 0x00066F48
		protected virtual void Update()
		{
			this.FloatSpring.Damping = this.DampingSlider.value;
			this.FloatSpring.Frequency = this.FrequencySlider.value;
			this.FloatSpring.UpdateSpringValue(Time.deltaTime);
			this._newPosition = this.MovingObject.transform.localPosition;
			this._newPosition.x = MMMaths.Remap(this.FloatSpring.CurrentValue, -1f, 1f, -this._range, this._range);
			this.MovingObject.transform.localPosition = this._newPosition;
		}

		// Token: 0x04001CBB RID: 7355
		[Header("Spring")]
		public MMSpringFloat FloatSpring;

		// Token: 0x04001CBC RID: 7356
		[Header("Bindings")]
		public FeelSpringsDemoSlider DampingSlider;

		// Token: 0x04001CBD RID: 7357
		public FeelSpringsDemoSlider FrequencySlider;

		// Token: 0x04001CBE RID: 7358
		public FeelSpringsDemoSlider BumpAmountSlider;

		// Token: 0x04001CBF RID: 7359
		public Transform MovingObject;

		// Token: 0x04001CC0 RID: 7360
		protected Vector3 _newPosition;

		// Token: 0x04001CC1 RID: 7361
		protected float _range = 0.375f;
	}
}
