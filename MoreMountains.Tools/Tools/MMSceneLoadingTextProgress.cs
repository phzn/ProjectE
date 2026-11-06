using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000149 RID: 329
	public class MMSceneLoadingTextProgress : MonoBehaviour
	{
		// Token: 0x06000742 RID: 1858 RVA: 0x000232EB File Offset: 0x000214EB
		protected virtual void Awake()
		{
			this._text = base.gameObject.GetComponent<Text>();
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00023300 File Offset: 0x00021500
		public virtual void SetProgress(float newValue)
		{
			float num = MMMaths.RoundToDecimal(MMMaths.Remap(newValue, 0f, 1f, this.RemapMin, this.RemapMax), this.NumberOfDecimals);
			this._text.text = num.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x040008CA RID: 2250
		[Tooltip("the value to which the progress' zero value should be remapped to")]
		public float RemapMin;

		// Token: 0x040008CB RID: 2251
		[Tooltip("the value to which the progress' one value should be remapped to")]
		public float RemapMax = 100f;

		// Token: 0x040008CC RID: 2252
		[Tooltip("the amount of decimals to display")]
		public int NumberOfDecimals;

		// Token: 0x040008CD RID: 2253
		protected Text _text;
	}
}
