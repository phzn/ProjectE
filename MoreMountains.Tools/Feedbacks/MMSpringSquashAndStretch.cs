using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000287 RID: 647
	[AddComponentMenu("More Mountains/Springs/MMSpringSquashAndStretch")]
	public class MMSpringSquashAndStretch : MMSpringFloatComponent<Transform>
	{
		// Token: 0x0600125B RID: 4699 RVA: 0x0004F88C File Offset: 0x0004DA8C
		protected override void Initialization()
		{
			base.Initialization();
			this.FloatSpring.ClampSettings.ClampMin = true;
			this.FloatSpring.ClampSettings.ClampMinValue = 0f;
			this.FloatSpring.ClampSettings.ClampMinBounce = true;
			this._initialScale = this.Target.localScale;
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0004F8E8 File Offset: 0x0004DAE8
		protected override void ApplyValue(float newValue)
		{
			float num = 1f / Mathf.Sqrt(newValue);
			switch (this.Axis)
			{
			case MMSpringSquashAndStretch.PossibleAxis.XtoYZ:
				this._newScale.x = newValue;
				this._newScale.y = num;
				this._newScale.z = num;
				break;
			case MMSpringSquashAndStretch.PossibleAxis.XtoY:
				this._newScale.x = newValue;
				this._newScale.y = num;
				this._newScale.z = this._initialScale.z;
				break;
			case MMSpringSquashAndStretch.PossibleAxis.XtoZ:
				this._newScale.x = newValue;
				this._newScale.y = this._initialScale.y;
				this._newScale.z = num;
				break;
			case MMSpringSquashAndStretch.PossibleAxis.YtoXZ:
				this._newScale.x = num;
				this._newScale.y = newValue;
				this._newScale.z = num;
				break;
			case MMSpringSquashAndStretch.PossibleAxis.YtoX:
				this._newScale.x = num;
				this._newScale.y = newValue;
				this._newScale.z = this._initialScale.z;
				break;
			case MMSpringSquashAndStretch.PossibleAxis.YtoZ:
				this._newScale.x = newValue;
				this._newScale.y = this._initialScale.y;
				this._newScale.z = num;
				break;
			case MMSpringSquashAndStretch.PossibleAxis.ZtoXZ:
				this._newScale.x = num;
				this._newScale.y = num;
				this._newScale.z = newValue;
				break;
			case MMSpringSquashAndStretch.PossibleAxis.ZtoX:
				this._newScale.x = num;
				this._newScale.y = this._initialScale.y;
				this._newScale.z = newValue;
				break;
			case MMSpringSquashAndStretch.PossibleAxis.ZtoY:
				this._newScale.x = this._initialScale.x;
				this._newScale.y = num;
				this._newScale.z = newValue;
				break;
			}
			this.Target.localScale = this._newScale;
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x0004FAEB File Offset: 0x0004DCEB
		protected override void GrabCurrentValue()
		{
			this.FloatSpring.CurrentValue = this.Target.localScale.x;
		}

		// Token: 0x040014D9 RID: 5337
		[MMInspectorGroup("Target", true, 17, false)]
		public MMSpringSquashAndStretch.PossibleAxis Axis;

		// Token: 0x040014DA RID: 5338
		protected Vector3 _newScale;

		// Token: 0x040014DB RID: 5339
		protected Vector3 _initialScale;

		// Token: 0x0200052A RID: 1322
		public enum PossibleAxis
		{
			// Token: 0x040023B2 RID: 9138
			XtoYZ,
			// Token: 0x040023B3 RID: 9139
			XtoY,
			// Token: 0x040023B4 RID: 9140
			XtoZ,
			// Token: 0x040023B5 RID: 9141
			YtoXZ,
			// Token: 0x040023B6 RID: 9142
			YtoX,
			// Token: 0x040023B7 RID: 9143
			YtoZ,
			// Token: 0x040023B8 RID: 9144
			ZtoXZ,
			// Token: 0x040023B9 RID: 9145
			ZtoX,
			// Token: 0x040023BA RID: 9146
			ZtoY
		}
	}
}
