using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x0200006D RID: 109
	[AddComponentMenu("MagicaCloth2/MagicaWindZone")]
	[HelpURL("https://magicasoft.jp/en/mc2_windzone_component/")]
	public class MagicaWindZone : ClothBehaviour
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0001035C File Offset: 0x0000E55C
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00010364 File Offset: 0x0000E564
		public int WindId { get; private set; } = -1;

		// Token: 0x060001A8 RID: 424 RVA: 0x0001036D File Offset: 0x0000E56D
		protected void Awake()
		{
			this.WindId = MagicaManager.Wind.AddWind(this);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00010380 File Offset: 0x0000E580
		protected void OnEnable()
		{
			MagicaManager.Wind.SetEnable(this.WindId, true);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00010393 File Offset: 0x0000E593
		protected void OnDisable()
		{
			WindManager wind = MagicaManager.Wind;
			if (wind == null)
			{
				return;
			}
			wind.SetEnable(this.WindId, false);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000103AB File Offset: 0x0000E5AB
		protected void OnDestroy()
		{
			WindManager wind = MagicaManager.Wind;
			if (wind != null)
			{
				wind.RemoveWind(this.WindId);
			}
			this.WindId = -1;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000103CC File Offset: 0x0000E5CC
		public bool IsDirection()
		{
			MagicaWindZone.Mode mode = this.mode;
			return mode <= MagicaWindZone.Mode.BoxDirection;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000103E7 File Offset: 0x0000E5E7
		public bool IsRadial()
		{
			return this.mode == MagicaWindZone.Mode.SphereRadial;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000103F6 File Offset: 0x0000E5F6
		public bool IsAddition()
		{
			return this.isAddition;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00010400 File Offset: 0x0000E600
		public Vector3 GetWindDirection(bool localSpace = false)
		{
			Vector3 vector = Quaternion.Euler(this.directionAngleX, this.directionAngleY, 0f) * Vector3.forward;
			if (!localSpace)
			{
				return base.transform.TransformDirection(vector);
			}
			return vector;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00010440 File Offset: 0x0000E640
		public void SetWindDirection(Vector3 dir, bool localSpace = false)
		{
			Vector3 vector = (localSpace ? dir : base.transform.InverseTransformDirection(dir));
			Vector3 eulerAngles = Quaternion.FromToRotation(Vector3.forward, vector).eulerAngles;
			this.directionAngleX = ((eulerAngles.x > 180f) ? (eulerAngles.x - 360f) : eulerAngles.x);
			this.directionAngleY = ((eulerAngles.y > 180f) ? (eulerAngles.y - 360f) : eulerAngles.y);
		}

		// Token: 0x04000273 RID: 627
		public MagicaWindZone.Mode mode;

		// Token: 0x04000274 RID: 628
		public Vector3 size = new Vector3(10f, 10f, 10f);

		// Token: 0x04000275 RID: 629
		public float radius = 10f;

		// Token: 0x04000276 RID: 630
		[Range(0f, 30f)]
		public float main = 5f;

		// Token: 0x04000277 RID: 631
		[Range(0f, 1f)]
		public float turbulence = 1f;

		// Token: 0x04000278 RID: 632
		[Range(-180f, 180f)]
		public float directionAngleX;

		// Token: 0x04000279 RID: 633
		[Range(-180f, 180f)]
		public float directionAngleY;

		// Token: 0x0400027A RID: 634
		public AnimationCurve attenuation = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

		// Token: 0x0400027B RID: 635
		public bool isAddition;

		// Token: 0x0200006E RID: 110
		public enum Mode
		{
			// Token: 0x0400027E RID: 638
			GlobalDirection,
			// Token: 0x0400027F RID: 639
			SphereDirection,
			// Token: 0x04000280 RID: 640
			BoxDirection,
			// Token: 0x04000281 RID: 641
			SphereRadial = 10
		}
	}
}
