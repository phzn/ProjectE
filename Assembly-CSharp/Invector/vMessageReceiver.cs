using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x020004A1 RID: 1185
	[vClassHeader("MESSAGE RECEIVER", "Use this component with the vMessageSender to call Events.")]
	public class vMessageReceiver : vMonoBehaviour
	{
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06001D04 RID: 7428 RVA: 0x000B3158 File Offset: 0x000B1358
		// (remove) Token: 0x06001D05 RID: 7429 RVA: 0x000B318C File Offset: 0x000B138C
		public static event vMessageReceiver.OnReceiveMessage onReceiveGlobalMessage;

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06001D06 RID: 7430 RVA: 0x000B31C0 File Offset: 0x000B13C0
		// (remove) Token: 0x06001D07 RID: 7431 RVA: 0x000B31F8 File Offset: 0x000B13F8
		public event vMessageReceiver.OnReceiveMessage onReceiveMessage;

		// Token: 0x06001D08 RID: 7432 RVA: 0x000B3230 File Offset: 0x000B1430
		private void Start()
		{
			for (int i = 0; i < this.messagesListeners.Count; i++)
			{
				vMessageReceiver.vMessageListener vMessageListener = this.messagesListeners[i];
				if (vMessageListener.receiveFromGlobal)
				{
					vMessageReceiver.onReceiveGlobalMessage -= vMessageListener.OnReceiveMessage;
					vMessageReceiver.onReceiveGlobalMessage += vMessageListener.OnReceiveMessage;
				}
				else
				{
					this.onReceiveMessage -= vMessageListener.OnReceiveMessage;
					this.onReceiveMessage += vMessageListener.OnReceiveMessage;
				}
			}
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000B32B0 File Offset: 0x000B14B0
		public void AddListener(string name, UnityAction<string> listener)
		{
			if (this.messagesListeners.Exists((vMessageReceiver.vMessageListener l) => l.Name.Equals(name)))
			{
				this.messagesListeners.Find((vMessageReceiver.vMessageListener l) => l.Name.Equals(name)).onReceiveMessage.AddListener(listener);
				return;
			}
			this.messagesListeners.Add(new vMessageReceiver.vMessageListener(name, listener));
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x000B3320 File Offset: 0x000B1520
		public void RemoveListener(string name, UnityAction<string> listener)
		{
			if (this.messagesListeners.Exists((vMessageReceiver.vMessageListener l) => l.Name.Equals(name)))
			{
				this.messagesListeners.Find((vMessageReceiver.vMessageListener l) => l.Name.Equals(name)).onReceiveMessage.RemoveListener(listener);
			}
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x000B3375 File Offset: 0x000B1575
		public void Send(string name)
		{
			if (!base.enabled)
			{
				return;
			}
			vMessageReceiver.OnReceiveMessage onReceiveMessage = this.onReceiveMessage;
			if (onReceiveMessage == null)
			{
				return;
			}
			onReceiveMessage(name, string.Empty);
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000B3396 File Offset: 0x000B1596
		public void Send(string name, string message)
		{
			if (!base.enabled)
			{
				return;
			}
			vMessageReceiver.OnReceiveMessage onReceiveMessage = this.onReceiveMessage;
			if (onReceiveMessage == null)
			{
				return;
			}
			onReceiveMessage(name, message);
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000B33B3 File Offset: 0x000B15B3
		public static void SendGlobal(string name, string message = null)
		{
			vMessageReceiver.OnReceiveMessage onReceiveMessage = vMessageReceiver.onReceiveGlobalMessage;
			if (onReceiveMessage == null)
			{
				return;
			}
			onReceiveMessage(name, message);
		}

		// Token: 0x04001E26 RID: 7718
		public List<vMessageReceiver.vMessageListener> messagesListeners;

		// Token: 0x020004A2 RID: 1186
		// (Invoke) Token: 0x06001D10 RID: 7440
		[Serializable]
		public delegate void OnReceiveMessage(string name, string message = null);

		// Token: 0x020004A3 RID: 1187
		[Serializable]
		public class OnReceiveMessageEvent : UnityEvent<string>
		{
		}

		// Token: 0x020004A4 RID: 1188
		[Serializable]
		public class vMessageListener
		{
			// Token: 0x06001D14 RID: 7444 RVA: 0x000B33CE File Offset: 0x000B15CE
			public void OnReceiveMessage(string name, string message = null)
			{
				if (this.Name.Equals(name))
				{
					this.onReceiveMessage.Invoke(string.IsNullOrEmpty(message) ? string.Empty : message);
				}
			}

			// Token: 0x06001D15 RID: 7445 RVA: 0x000B33F9 File Offset: 0x000B15F9
			public vMessageListener(string name)
			{
				this.Name = name;
			}

			// Token: 0x06001D16 RID: 7446 RVA: 0x000B3408 File Offset: 0x000B1608
			public vMessageListener(string name, UnityAction<string> listener)
			{
				this.Name = name;
				this.onReceiveMessage.AddListener(listener);
			}

			// Token: 0x04001E28 RID: 7720
			public string Name;

			// Token: 0x04001E29 RID: 7721
			public bool receiveFromGlobal;

			// Token: 0x04001E2A RID: 7722
			public vMessageReceiver.OnReceiveMessageEvent onReceiveMessage;
		}
	}
}
