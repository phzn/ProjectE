using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.Panel
{
	// Token: 0x020013DA RID: 5082
	public class CielOSPanel : MonoBehaviour
	{
		// Token: 0x1400011A RID: 282
		// (add) Token: 0x06009CEF RID: 40175 RVA: 0x00406758 File Offset: 0x00404958
		// (remove) Token: 0x06009CF0 RID: 40176 RVA: 0x00406790 File Offset: 0x00404990
		public event Action PowerPressed;

		// Token: 0x06009CF1 RID: 40177 RVA: 0x004067C8 File Offset: 0x004049C8
		private void Awake()
		{
			if (this.appMenuButton != null)
			{
				this.appMenuButton.onClick.AddListener(new UnityAction(this.ToggleAppMenu));
			}
			if (this.powerButton != null)
			{
				this.powerButton.onClick.AddListener(delegate
				{
					Action powerPressed = this.PowerPressed;
					if (powerPressed == null)
					{
						return;
					}
					powerPressed();
				});
			}
			CielOSSfx.Attach(this.appMenuButton);
			CielOSSfx.Attach(this.powerButton);
		}

		// Token: 0x06009CF2 RID: 40178 RVA: 0x00406840 File Offset: 0x00404A40
		public void BindRuntimeControls(TMP_Text clock, Button power, Button appMenuTrigger)
		{
			if (clock != null)
			{
				this.clockText = clock;
			}
			if (power != null)
			{
				this.powerButton = power;
				this.powerButton.onClick.RemoveListener(new UnityAction(this.FirePower));
				this.powerButton.onClick.AddListener(new UnityAction(this.FirePower));
			}
			if (appMenuTrigger != null)
			{
				this.appMenuButton = appMenuTrigger;
				this.appMenuButton.onClick.RemoveListener(new UnityAction(this.ToggleAppMenu));
				this.appMenuButton.onClick.AddListener(new UnityAction(this.ToggleAppMenu));
			}
			this.UpdateClock();
		}

		// Token: 0x06009CF3 RID: 40179 RVA: 0x004068F3 File Offset: 0x00404AF3
		public void FirePower()
		{
			Action powerPressed = this.PowerPressed;
			if (powerPressed == null)
			{
				return;
			}
			powerPressed();
		}

		// Token: 0x06009CF4 RID: 40180 RVA: 0x00406905 File Offset: 0x00404B05
		private void OnEnable()
		{
			this.UpdateClock();
		}

		// Token: 0x06009CF5 RID: 40181 RVA: 0x00406905 File Offset: 0x00404B05
		private void Update()
		{
			this.UpdateClock();
		}

		// Token: 0x06009CF6 RID: 40182 RVA: 0x0040690D File Offset: 0x00404B0D
		private void ToggleAppMenu()
		{
			if (this.appMenu != null)
			{
				this.appMenu.Toggle();
			}
		}

		// Token: 0x06009CF7 RID: 40183 RVA: 0x00406928 File Offset: 0x00404B28
		private void UpdateClock()
		{
			if (this.clockText == null)
			{
				return;
			}
			this.clockText.text = DateTime.Now.ToString(this.clockFormat);
		}

		// Token: 0x0400A178 RID: 41336
		[Header("App Menu")]
		[SerializeField]
		private Button appMenuButton;

		// Token: 0x0400A179 RID: 41337
		[SerializeField]
		private CielOSAppMenu appMenu;

		// Token: 0x0400A17A RID: 41338
		[Header("Clock")]
		[SerializeField]
		private TMP_Text clockText;

		// Token: 0x0400A17B RID: 41339
		[Tooltip("Clock format. Default HH:mm.")]
		[SerializeField]
		private string clockFormat = "HH:mm";

		// Token: 0x0400A17C RID: 41340
		[Header("Power")]
		[SerializeField]
		private Button powerButton;
	}
}
