using System;
using UnityEngine;

namespace ProjectExhibitionist.Headphones
{
	// Token: 0x020009D3 RID: 2515
	[CreateAssetMenu(fileName = "HeadphonesFitProfile", menuName = "Project Exhibitionist/Headphones/Fit Profile")]
	public sealed class HeadphonesFitProfileSO : ScriptableObject
	{
		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x060046C1 RID: 18113 RVA: 0x001ABB09 File Offset: 0x001A9D09
		public Quaternion LocalRotation
		{
			get
			{
				return Quaternion.Euler(this.localEulerAngles);
			}
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x001ABB18 File Offset: 0x001A9D18
		private void OnValidate()
		{
			this.localScale.x = HeadphonesFitProfileSO.KeepNonZero(this.localScale.x);
			this.localScale.y = HeadphonesFitProfileSO.KeepNonZero(this.localScale.y);
			this.localScale.z = HeadphonesFitProfileSO.KeepNonZero(this.localScale.z);
		}

		// Token: 0x060046C3 RID: 18115 RVA: 0x001ABB76 File Offset: 0x001A9D76
		private static float KeepNonZero(float value)
		{
			if (Mathf.Abs(value) >= 0.0001f)
			{
				return value;
			}
			if (value >= 0f)
			{
				return 0.0001f;
			}
			return -0.0001f;
		}

		// Token: 0x0400464C RID: 17996
		[Tooltip("Preferred rigid-accessory bone. Ciel's customization system also uses head.x.")]
		public string preferredBoneName = "head.x";

		// Token: 0x0400464D RID: 17997
		[Header("Local pose under the head bone")]
		public Vector3 localPosition = new Vector3(0f, -0.021f, -0.007f);

		// Token: 0x0400464E RID: 17998
		public Vector3 localEulerAngles = Vector3.zero;

		// Token: 0x0400464F RID: 17999
		public Vector3 localScale = Vector3.one;

		// Token: 0x04004650 RID: 18000
		[Header("Rendering")]
		[Tooltip("The worn visual moves off the world-prop layer and onto Ciel's clothing layer.")]
		public string wornLayerName = "PlayerClothing";
	}
}
