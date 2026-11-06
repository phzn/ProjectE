using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001A1 RID: 417
	[ExecuteAlways]
	[AddComponentMenu("")]
	public class DemoButton : MonoBehaviour
	{
		// Token: 0x060009D6 RID: 2518 RVA: 0x0002DE80 File Offset: 0x0002C080
		protected virtual void OnEnable()
		{
			this.HandleWebGL();
			this.TargetButton.onClick.AddListener(new UnityAction(this.OnClickEvent));
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0002DEA4 File Offset: 0x0002C0A4
		protected void OnDisable()
		{
			this.TargetButton.onClick.RemoveListener(new UnityAction(this.OnClickEvent));
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0002DEC2 File Offset: 0x0002C0C2
		public void OnClickEvent()
		{
			MMF_Player targetMMF_Player = this.TargetMMF_Player;
			if (targetMMF_Player == null)
			{
				return;
			}
			targetMMF_Player.PlayFeedbacks();
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0002DED4 File Offset: 0x0002C0D4
		protected virtual void HandleWebGL()
		{
			if (this.WebGL != null)
			{
				this.WebGL.gameObject.SetActive(false);
				this.TargetButton.interactable = true;
				this.ButtonText.color = Color.white;
			}
		}

		// Token: 0x04000C16 RID: 3094
		[Header("Behaviour")]
		public bool NotSupportedInWebGL;

		// Token: 0x04000C17 RID: 3095
		[Header("Bindings")]
		public Button TargetButton;

		// Token: 0x04000C18 RID: 3096
		public Text ButtonText;

		// Token: 0x04000C19 RID: 3097
		public Text WebGL;

		// Token: 0x04000C1A RID: 3098
		public MMF_Player TargetMMF_Player;

		// Token: 0x04000C1B RID: 3099
		protected Color _disabledColor = new Color(255f, 255f, 255f, 0.5f);
	}
}
