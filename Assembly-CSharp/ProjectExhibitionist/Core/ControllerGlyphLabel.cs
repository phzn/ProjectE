using System;
using TMPro;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A36 RID: 2614
	public sealed class ControllerGlyphLabel : MonoBehaviour
	{
		// Token: 0x06004A3F RID: 19007 RVA: 0x001CC9F2 File Offset: 0x001CABF2
		public static void Bind(TMP_Text text, string canonical)
		{
			if (text == null)
			{
				return;
			}
			ControllerGlyphLabel controllerGlyphLabel = text.GetComponent<ControllerGlyphLabel>() ?? text.gameObject.AddComponent<ControllerGlyphLabel>();
			controllerGlyphLabel._text = text;
			controllerGlyphLabel._source = canonical;
			controllerGlyphLabel._version = -1;
			controllerGlyphLabel.LateUpdate();
		}

		// Token: 0x06004A40 RID: 19008 RVA: 0x001CCA2D File Offset: 0x001CAC2D
		private void LateUpdate()
		{
			if (this._text == null || this._version == ControllerGlyphs.Version)
			{
				return;
			}
			this._version = ControllerGlyphs.Version;
			ControllerGlyphs.Apply(this._text, this._source);
		}

		// Token: 0x04004C7E RID: 19582
		private TMP_Text _text;

		// Token: 0x04004C7F RID: 19583
		private string _source;

		// Token: 0x04004C80 RID: 19584
		private int _version = -1;
	}
}
