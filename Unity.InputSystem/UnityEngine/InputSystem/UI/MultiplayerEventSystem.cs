using System;
using UnityEngine.EventSystems;

namespace UnityEngine.InputSystem.UI
{
	// Token: 0x0200008D RID: 141
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/UISupport.html#multiplayer-uis")]
	public class MultiplayerEventSystem : EventSystem
	{
		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0003E044 File Offset: 0x0003C244
		// (set) Token: 0x06000B86 RID: 2950 RVA: 0x0003E04C File Offset: 0x0003C24C
		public GameObject playerRoot
		{
			get
			{
				return this.m_PlayerRoot;
			}
			set
			{
				this.m_PlayerRoot = value;
				this.InitializePlayerRoot();
			}
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0003E05B File Offset: 0x0003C25B
		protected override void OnEnable()
		{
			base.OnEnable();
			this.InitializePlayerRoot();
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0003E069 File Offset: 0x0003C269
		protected override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0003E074 File Offset: 0x0003C274
		private void InitializePlayerRoot()
		{
			if (this.m_PlayerRoot == null)
			{
				return;
			}
			InputSystemUIInputModule component = base.GetComponent<InputSystemUIInputModule>();
			if (component != null)
			{
				component.localMultiPlayerRoot = this.m_PlayerRoot;
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0003E0AC File Offset: 0x0003C2AC
		protected override void Update()
		{
			EventSystem current = EventSystem.current;
			EventSystem.current = this;
			try
			{
				base.Update();
			}
			finally
			{
				EventSystem.current = current;
			}
		}

		// Token: 0x0400041F RID: 1055
		[Tooltip("If set, only process mouse and navigation events for any game objects which are children of this game object.")]
		[SerializeField]
		private GameObject m_PlayerRoot;
	}
}
