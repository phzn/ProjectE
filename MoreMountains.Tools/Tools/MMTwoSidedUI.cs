using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x02000058 RID: 88
	[ExecuteAlways]
	public class MMTwoSidedUI : MonoBehaviour
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x000094B0 File Offset: 0x000076B0
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000094B8 File Offset: 0x000076B8
		protected virtual void Initialization()
		{
			this._rectTransform = base.gameObject.GetComponent<RectTransform>();
			this._initialized = true;
			float scaleValue = this.GetScaleValue();
			this.BackVisible = scaleValue < this.ScaleThreshold;
			this.Front.SetActive(!this.BackVisible);
			this.Back.SetActive(this.BackVisible);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009518 File Offset: 0x00007718
		protected virtual void Update()
		{
			float scaleValue = this.GetScaleValue();
			if (scaleValue < this.ScaleThreshold != this.BackVisible)
			{
				this.Front.SetActive(this.BackVisible);
				this.Back.SetActive(!this.BackVisible);
				UnityEvent onFlip = this.OnFlip;
				if (onFlip != null)
				{
					onFlip.Invoke();
				}
			}
			this.BackVisible = scaleValue < this.ScaleThreshold;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00009584 File Offset: 0x00007784
		protected virtual void IfEditor()
		{
			if (!this._initialized)
			{
				this.Initialization();
			}
			if (this.DebugMode)
			{
				switch (this.FlipAxis)
				{
				case MMTwoSidedUI.Axis.x:
					this._rectTransform.localScale = new Vector3(this.ScaleValue, this._rectTransform.localScale.y, this._rectTransform.localScale.z);
					return;
				case MMTwoSidedUI.Axis.y:
					this._rectTransform.localScale = new Vector3(this._rectTransform.localScale.x, this.ScaleValue, this._rectTransform.localScale.z);
					return;
				case MMTwoSidedUI.Axis.z:
					this._rectTransform.localScale = new Vector3(this._rectTransform.localScale.x, this._rectTransform.localScale.y, this.ScaleValue);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00009668 File Offset: 0x00007868
		protected virtual float GetScaleValue()
		{
			switch (this.FlipAxis)
			{
			case MMTwoSidedUI.Axis.x:
				return this._rectTransform.localScale.x;
			case MMTwoSidedUI.Axis.y:
				return this._rectTransform.localScale.y;
			case MMTwoSidedUI.Axis.z:
				return this._rectTransform.localScale.z;
			default:
				return 0f;
			}
		}

		// Token: 0x0400028A RID: 650
		[Header("Bindings")]
		public GameObject Front;

		// Token: 0x0400028B RID: 651
		public GameObject Back;

		// Token: 0x0400028C RID: 652
		[Header("Axis")]
		public MMTwoSidedUI.Axis FlipAxis;

		// Token: 0x0400028D RID: 653
		public float ScaleThreshold;

		// Token: 0x0400028E RID: 654
		[Header("Events")]
		public UnityEvent OnFlip;

		// Token: 0x0400028F RID: 655
		[Header("Debug")]
		public bool DebugMode;

		// Token: 0x04000290 RID: 656
		[Range(-1f, 1f)]
		public float ScaleValue;

		// Token: 0x04000291 RID: 657
		[MMReadOnly]
		public bool BackVisible;

		// Token: 0x04000292 RID: 658
		protected RectTransform _rectTransform;

		// Token: 0x04000293 RID: 659
		protected bool _initialized;

		// Token: 0x020003C8 RID: 968
		public enum Axis
		{
			// Token: 0x04001DE6 RID: 7654
			x,
			// Token: 0x04001DE7 RID: 7655
			y,
			// Token: 0x04001DE8 RID: 7656
			z
		}
	}
}
