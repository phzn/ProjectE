using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feel
{
	// Token: 0x02000383 RID: 899
	[AddComponentMenu("")]
	public class FeelSpringsComparisonDemo : MonoBehaviour
	{
		// Token: 0x0600189C RID: 6300 RVA: 0x00068A18 File Offset: 0x00066C18
		protected virtual void OnEnable()
		{
			foreach (MMSpringFloat mmspringFloat in this.Springs)
			{
				mmspringFloat.CurrentValue = 0f;
				mmspringFloat.TargetValue = 0f;
				mmspringFloat.Velocity = 0f;
			}
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x00068A84 File Offset: 0x00066C84
		public virtual void RandomBump()
		{
			float value = this.BumpAmountSlider.value;
			foreach (MMSpringFloat mmspringFloat in this.Springs)
			{
				mmspringFloat.Bump(value);
			}
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x00068AE4 File Offset: 0x00066CE4
		protected virtual void Update()
		{
			for (int i = 0; i < this.Springs.Count; i++)
			{
				this.Springs[i].UpdateSpringValue(Time.deltaTime);
				this._newPosition = this.MovingObjects[i].transform.localPosition;
				this._newPosition.x = MMMaths.Remap(this.Springs[i].CurrentValue, -1f, 1f, -this._range, this._range);
				this.MovingObjects[i].transform.localPosition = this._newPosition;
			}
		}

		// Token: 0x04001CB2 RID: 7346
		[Header("Spring")]
		public List<MMSpringFloat> Springs;

		// Token: 0x04001CB3 RID: 7347
		public List<Transform> MovingObjects;

		// Token: 0x04001CB4 RID: 7348
		public FeelSpringsDemoSlider BumpAmountSlider;

		// Token: 0x04001CB5 RID: 7349
		protected Vector3 _newPosition;

		// Token: 0x04001CB6 RID: 7350
		protected float _range = 0.375f;
	}
}
