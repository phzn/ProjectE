using System;
using TMPro;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002C7 RID: 711
	public class MMFloatingTextMeshPro : MMFloatingText
	{
		// Token: 0x06001427 RID: 5159 RVA: 0x00056F02 File Offset: 0x00055102
		protected override void Initialization()
		{
			base.Initialization();
			this._initialTextColor = this.TargetTextMeshPro.color;
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00056F1B File Offset: 0x0005511B
		public override void SetText(string newValue)
		{
			this.TargetTextMeshPro.text = newValue;
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00056F29 File Offset: 0x00055129
		public override void SetColor(Color newColor)
		{
			this.TargetTextMeshPro.color = newColor;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00056F37 File Offset: 0x00055137
		public override void SetOpacity(float newOpacity)
		{
			this._newColor = this.TargetTextMeshPro.color;
			this._newColor.a = newOpacity;
			this.TargetTextMeshPro.color = this._newColor;
		}

		// Token: 0x04001733 RID: 5939
		[Header("TextMeshPro")]
		public TextMeshPro TargetTextMeshPro;
	}
}
