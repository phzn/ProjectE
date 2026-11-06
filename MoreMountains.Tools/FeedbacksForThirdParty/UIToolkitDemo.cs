using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002DF RID: 735
	public class UIToolkitDemo : MonoBehaviour
	{
		// Token: 0x06001494 RID: 5268 RVA: 0x000582AC File Offset: 0x000564AC
		private void OnEnable()
		{
			VisualElement rootVisualElement = base.GetComponent<UIDocument>().rootVisualElement;
			rootVisualElement.Q<VisualElement>("DemoFace", null).style.backgroundImage = this.FaceTexture;
			using (List<UIToolkitDemoAction>.Enumerator enumerator = this.Actions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIToolkitDemoAction action = enumerator.Current;
					this._button = rootVisualElement.Q<Button>(action.ButtonName, null);
					this._button.text = this._button.text.ToUpper();
					this._button.RegisterCallback<ClickEvent>(delegate(ClickEvent ev)
					{
						this.PlayFeedback(action.TargetPlayer);
					}, TrickleDown.NoTrickleDown);
				}
			}
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x00058384 File Offset: 0x00056584
		private void PlayFeedback(MMF_Player player)
		{
			player.PlayFeedbacks();
		}

		// Token: 0x04001788 RID: 6024
		public Texture2D FaceTexture;

		// Token: 0x04001789 RID: 6025
		public List<UIToolkitDemoAction> Actions;

		// Token: 0x0400178A RID: 6026
		private Button _button;
	}
}
