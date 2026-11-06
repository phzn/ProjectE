using System;
using Features.CensorBars;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Loading
{
	// Token: 0x020010DF RID: 4319
	[DisallowMultipleComponent]
	public class LoadingScreenRandomizer : MonoBehaviour
	{
		// Token: 0x060082FD RID: 33533 RVA: 0x00357624 File Offset: 0x00355824
		private void OnEnable()
		{
			this.EnsureTipBinding();
			if (this.backgroundImage != null && this.backgroundImage.GetComponent<StreamerArtworkImage>() == null)
			{
				Sprite sprite = this.backgroundImage.sprite;
				StreamerArtworkImage.Bind(this.backgroundImage, sprite);
				LoadingScreenArtworkCensor.Bind(this.backgroundImage, sprite, (this.tipLabel != null) ? this.tipLabel.font : null);
			}
			if (this.randomizeOnEnable)
			{
				this.Randomize();
			}
		}

		// Token: 0x060082FE RID: 33534 RVA: 0x003576A6 File Offset: 0x003558A6
		public void Randomize()
		{
			this.ApplyRandomBackground();
			this.ApplyRandomTip();
		}

		// Token: 0x060082FF RID: 33535 RVA: 0x003576B4 File Offset: 0x003558B4
		private void ApplyRandomBackground()
		{
			if (this.backgroundImage == null || this.backgroundSprites == null || this.backgroundSprites.Length == 0)
			{
				return;
			}
			int num = LoadingScreenRandomizer.PickIndex(this.backgroundSprites.Length, this._lastBackgroundIndex);
			this._lastBackgroundIndex = num;
			StreamerArtworkImage.Bind(this.backgroundImage, this.backgroundSprites[num]);
			LoadingScreenArtworkCensor.Bind(this.backgroundImage, this.backgroundSprites[num], (this.tipLabel != null) ? this.tipLabel.font : null);
		}

		// Token: 0x06008300 RID: 33536 RVA: 0x00357740 File Offset: 0x00355940
		private void ApplyRandomTip()
		{
			if (this.tipLabel == null || this.tips == null || this.tips.Length == 0)
			{
				return;
			}
			int num = LoadingScreenRandomizer.PickIndex(this.tips.Length, this._lastTipIndex);
			this._lastTipIndex = num;
			this.EnsureTipBinding();
			this._tipBinding.Refresh();
		}

		// Token: 0x06008301 RID: 33537 RVA: 0x0035779C File Offset: 0x0035599C
		private void EnsureTipBinding()
		{
			if (this.tipLabel == null || this._tipBinding != null)
			{
				return;
			}
			this._initialTip = this.tipLabel.text;
			this._tipBinding = LocalizedGameText.BindComputed(this.tipLabel, delegate
			{
				if (this._lastTipIndex < 0 || this.tips == null || this._lastTipIndex >= this.tips.Length)
				{
					return GameLocalization.GetSource("gameplay_prompts", this._initialTip);
				}
				return GameLocalization.GetSource("gameplay_prompts", this.tipPrefix) + GameLocalization.GetSource("gameplay_prompts", this.tips[this._lastTipIndex]);
			});
			this._tipBinding.FitTranslationToRectangle(false);
		}

		// Token: 0x06008302 RID: 33538 RVA: 0x00357800 File Offset: 0x00355A00
		private static int PickIndex(int count, int last)
		{
			if (count <= 1)
			{
				return 0;
			}
			int num = Random.Range(0, count);
			if (num == last)
			{
				num = (num + 1) % count;
			}
			return num;
		}

		// Token: 0x04008621 RID: 34337
		[Header("Background random")]
		[Tooltip("Image che mostra lo sfondo a tutto schermo. Lo Sprite viene scelto random in OnEnable.")]
		[SerializeField]
		private Image backgroundImage;

		// Token: 0x04008622 RID: 34338
		[Tooltip("Pool di sfondi (gli screenshot del gioco). Importati come Sprite. Uno viene scelto a caso ad ogni attivazione del pannello.")]
		[SerializeField]
		private Sprite[] backgroundSprites;

		// Token: 0x04008623 RID: 34339
		[Header("Tip random")]
		[Tooltip("Label TMP in basso che mostra un tip casuale.")]
		[SerializeField]
		private TMP_Text tipLabel;

		// Token: 0x04008624 RID: 34340
		[Tooltip("Prefisso opzionale prima del tip (es. 'TIP: '). Lascia vuoto per nessun prefisso.")]
		[SerializeField]
		private string tipPrefix = "TIP: ";

		// Token: 0x04008625 RID: 34341
		[Tooltip("Pool di tip. Uno viene scelto a caso ad ogni attivazione del pannello.")]
		[TextArea(2, 4)]
		[SerializeField]
		private string[] tips;

		// Token: 0x04008626 RID: 34342
		[Header("Comportamento")]
		[Tooltip("Se OFF, NON ri-randomizza automaticamente in OnEnable. La scelta di sfondo/tip è guidata dal LoadingScreenController, che chiama Randomize() UNA volta all'inizio di ogni caricamento. Tienilo OFF per evitare che la picture/tip cambi a metà transizione (es. la nuova istanza del pannello che riparte al cambio scena).")]
		[SerializeField]
		private bool randomizeOnEnable;

		// Token: 0x04008627 RID: 34343
		private int _lastBackgroundIndex = -1;

		// Token: 0x04008628 RID: 34344
		private int _lastTipIndex = -1;

		// Token: 0x04008629 RID: 34345
		private string _initialTip;

		// Token: 0x0400862A RID: 34346
		private LocalizedGameText _tipBinding;
	}
}
