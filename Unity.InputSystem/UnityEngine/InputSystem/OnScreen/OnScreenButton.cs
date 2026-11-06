using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.OnScreen
{
	// Token: 0x02000096 RID: 150
	[AddComponentMenu("Input/On-Screen Button")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/OnScreen.html#on-screen-buttons")]
	public class OnScreenButton : OnScreenControl, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x06000BEE RID: 3054 RVA: 0x0003FB97 File Offset: 0x0003DD97
		public void OnPointerUp(PointerEventData eventData)
		{
			base.SendValueToControl<float>(0f);
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0003FBA4 File Offset: 0x0003DDA4
		public void OnPointerDown(PointerEventData eventData)
		{
			base.SendValueToControl<float>(1f);
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0003FBB1 File Offset: 0x0003DDB1
		// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x0003FBB9 File Offset: 0x0003DDB9
		protected override string controlPathInternal
		{
			get
			{
				return this.m_ControlPath;
			}
			set
			{
				this.m_ControlPath = value;
			}
		}

		// Token: 0x04000460 RID: 1120
		[InputControl(layout = "Button")]
		[SerializeField]
		private string m_ControlPath;
	}
}
