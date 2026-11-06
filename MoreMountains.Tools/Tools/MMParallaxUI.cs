using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000050 RID: 80
	public class MMParallaxUI : MonoBehaviour
	{
		// Token: 0x060001C3 RID: 451 RVA: 0x00008BB6 File Offset: 0x00006DB6
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008BC0 File Offset: 0x00006DC0
		public virtual void Initialization()
		{
			foreach (MMParallaxUI.ParallaxLayer parallaxLayer in this.ParallaxLayers)
			{
				parallaxLayer.StartPosition = parallaxLayer.Rect.position;
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00008C20 File Offset: 0x00006E20
		protected virtual void Update()
		{
			this.MoveLayers();
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008C28 File Offset: 0x00006E28
		protected virtual void MoveLayers()
		{
			MMParallaxUI.Modes mode = this.Mode;
			if (mode != MMParallaxUI.Modes.Mouse)
			{
				if (mode == MMParallaxUI.Modes.Gyroscope)
				{
					this._referencePosition = MMGyroscope.CalibratedInputAcceleration;
				}
			}
			else
			{
				this._mousePosition = Input.mousePosition;
				this._referencePosition = Camera.main.ScreenToViewportPoint(this._mousePosition);
			}
			foreach (MMParallaxUI.ParallaxLayer parallaxLayer in this.ParallaxLayers)
			{
				if (parallaxLayer.Active)
				{
					this._newPosition.x = Mathf.Lerp(parallaxLayer.Rect.position.x, parallaxLayer.StartPosition.x + this._referencePosition.x * parallaxLayer.Amplitude * this.AmplitudeMultiplier, parallaxLayer.Speed * this.SpeedMultiplier * Time.deltaTime);
					this._newPosition.y = Mathf.Lerp(parallaxLayer.Rect.position.y, parallaxLayer.StartPosition.y + this._referencePosition.y * parallaxLayer.Amplitude * this.AmplitudeMultiplier, parallaxLayer.Speed * this.SpeedMultiplier * Time.deltaTime);
					this._newPosition.z = 0f;
					parallaxLayer.Rect.position = this._newPosition;
				}
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00008DA8 File Offset: 0x00006FA8
		public virtual void SetReferencePosition(Vector3 newReferencePosition)
		{
			this._referencePosition = newReferencePosition;
		}

		// Token: 0x04000264 RID: 612
		public MMParallaxUI.Modes Mode;

		// Token: 0x04000265 RID: 613
		public float AmplitudeMultiplier = 1f;

		// Token: 0x04000266 RID: 614
		public float SpeedMultiplier = 1f;

		// Token: 0x04000267 RID: 615
		public List<MMParallaxUI.ParallaxLayer> ParallaxLayers;

		// Token: 0x04000268 RID: 616
		protected Vector2 _referencePosition;

		// Token: 0x04000269 RID: 617
		protected Vector3 _newPosition;

		// Token: 0x0400026A RID: 618
		protected Vector2 _mousePosition;

		// Token: 0x020003C5 RID: 965
		[Serializable]
		public class ParallaxLayer
		{
			// Token: 0x04001DD9 RID: 7641
			public RectTransform Rect;

			// Token: 0x04001DDA RID: 7642
			public float Speed = 2f;

			// Token: 0x04001DDB RID: 7643
			public float Amplitude = 50f;

			// Token: 0x04001DDC RID: 7644
			[HideInInspector]
			public Vector2 StartPosition;

			// Token: 0x04001DDD RID: 7645
			public bool Active = true;
		}

		// Token: 0x020003C6 RID: 966
		public enum Modes
		{
			// Token: 0x04001DDF RID: 7647
			Mouse,
			// Token: 0x04001DE0 RID: 7648
			Gyroscope,
			// Token: 0x04001DE1 RID: 7649
			Script
		}
	}
}
