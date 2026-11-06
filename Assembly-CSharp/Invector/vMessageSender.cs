using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004A7 RID: 1191
	[vClassHeader("Message Sender", "Requires a vMessageReceiver to receive messages.", openClose = false)]
	public class vMessageSender : vMonoBehaviour
	{
		// Token: 0x06001D1D RID: 7453 RVA: 0x000B344C File Offset: 0x000B164C
		public virtual void SendToDefaultReceiver(int messageIndex)
		{
			vMessageSender.vMessage vMessage = ((this.messages.Count > 0 && messageIndex < this.messages.Count) ? this.messages[messageIndex] : null);
			if (vMessage != null)
			{
				for (int i = 0; i < vMessage.defaultReceivers.Count; i++)
				{
					if (vMessage.defaultReceivers[i])
					{
						vMessage.defaultReceivers[i].Send(vMessage.name, vMessage.message);
					}
				}
			}
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000B34D0 File Offset: 0x000B16D0
		public virtual void SendToDefaultReceiver(string messageName)
		{
			List<vMessageSender.vMessage> list = this.messages.FindAll((vMessageSender.vMessage m) => m.name.Equals(messageName));
			if (list != null && list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					for (int j = 0; j < list[i].defaultReceivers.Count; j++)
					{
						if (list[i].defaultReceivers[j])
						{
							list[i].defaultReceivers[j].Send(list[i].name, list[i].message);
						}
					}
				}
			}
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x000B3584 File Offset: 0x000B1784
		public virtual void SendToParentReceiver(int messageIndex)
		{
			vMessageReceiver componentInParent = base.GetComponentInParent<vMessageReceiver>();
			if (!componentInParent)
			{
				return;
			}
			vMessageSender.vMessage vMessage = ((this.messages.Count > 0 && messageIndex < this.messages.Count) ? this.messages[messageIndex] : null);
			if (vMessage != null)
			{
				componentInParent.Send(vMessage.name, vMessage.message);
			}
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x000B35E4 File Offset: 0x000B17E4
		public virtual void SendToParentReceiver(string messageName)
		{
			vMessageReceiver componentInParent = base.GetComponentInParent<vMessageReceiver>();
			if (!componentInParent)
			{
				return;
			}
			List<vMessageSender.vMessage> list = this.messages.FindAll((vMessageSender.vMessage m) => m.name.Equals(messageName));
			if (list != null && list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					componentInParent.Send(list[i].name, list[i].message);
				}
			}
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x000B3664 File Offset: 0x000B1864
		public virtual void Send(GameObject target, int messageIndex)
		{
			if (target)
			{
				return;
			}
			vMessageReceiver component = target.GetComponent<vMessageReceiver>();
			if (!component)
			{
				return;
			}
			vMessageSender.vMessage vMessage = ((this.messages.Count > 0 && messageIndex < this.messages.Count) ? this.messages[messageIndex] : null);
			if (vMessage != null)
			{
				component.Send(vMessage.name, vMessage.message);
			}
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000B36CB File Offset: 0x000B18CB
		public virtual void Send(Collider target, int messageIndex)
		{
			if (target)
			{
				return;
			}
			this.Send(target.gameObject, messageIndex);
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x000B36CB File Offset: 0x000B18CB
		public virtual void Send(Transform target, int messageIndex)
		{
			if (target)
			{
				return;
			}
			this.Send(target.gameObject, messageIndex);
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x000B36E4 File Offset: 0x000B18E4
		public virtual void Send(GameObject target, string messageName)
		{
			if (target)
			{
				return;
			}
			vMessageReceiver component = target.GetComponent<vMessageReceiver>();
			if (!component)
			{
				return;
			}
			List<vMessageSender.vMessage> list = this.messages.FindAll((vMessageSender.vMessage m) => m.name.Equals(messageName));
			if (list != null && list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					component.Send(list[i].name, list[i].message);
				}
			}
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x000B376A File Offset: 0x000B196A
		public virtual void Send(Collider target, string messageName)
		{
			if (!target)
			{
				return;
			}
			this.Send(target.gameObject, messageName);
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x000B376A File Offset: 0x000B196A
		public virtual void Send(Transform target, string messageName)
		{
			if (!target)
			{
				return;
			}
			this.Send(target.gameObject, messageName);
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x000B3784 File Offset: 0x000B1984
		public virtual void SendAllToDefaultReceiver()
		{
			if (this.messages != null && this.messages.Count > 0)
			{
				for (int i = 0; i < this.messages.Count; i++)
				{
					for (int j = 0; j < this.messages[i].defaultReceivers.Count; j++)
					{
						if (this.messages[i].defaultReceivers[j])
						{
							this.messages[i].defaultReceivers[j].Send(this.messages[i].name, this.messages[i].message);
						}
					}
				}
			}
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x000B3848 File Offset: 0x000B1A48
		public virtual void SendAll(GameObject target)
		{
			if (!target)
			{
				return;
			}
			vMessageReceiver component = target.GetComponent<vMessageReceiver>();
			if (!component)
			{
				return;
			}
			for (int i = 0; i < this.messages.Count; i++)
			{
				component.Send(this.messages[i].name, this.messages[i].message);
			}
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x000B38AC File Offset: 0x000B1AAC
		public virtual void OnTrigger(Collider target)
		{
			if (!target)
			{
				return;
			}
			vMessageReceiver component = target.gameObject.GetComponent<vMessageReceiver>();
			if (!component)
			{
				return;
			}
			for (int i = 0; i < this.messages.Count; i++)
			{
				if (this.messages[i].sendByTrigger)
				{
					component.Send(this.messages[i].name, this.messages[i].message);
				}
			}
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x000B3928 File Offset: 0x000B1B28
		public virtual void SendGlobal(string messageName)
		{
			List<vMessageSender.vGlobalMessage> list = this.globalMessages.FindAll((vMessageSender.vGlobalMessage m) => m.name.Equals(messageName));
			for (int i = 0; i < list.Count; i++)
			{
				vMessageReceiver.SendGlobal(list[i].name, list[i].message);
			}
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x000B3988 File Offset: 0x000B1B88
		public virtual void SendGlobal(int messageIndex)
		{
			vMessageSender.vGlobalMessage vGlobalMessage = ((this.globalMessages.Count > 0 && messageIndex < this.globalMessages.Count) ? this.globalMessages[messageIndex] : null);
			if (vGlobalMessage != null)
			{
				vMessageReceiver.SendGlobal(vGlobalMessage.name, vGlobalMessage.message);
			}
		}

		// Token: 0x04001E2D RID: 7725
		public List<vMessageSender.vMessage> messages;

		// Token: 0x04001E2E RID: 7726
		public List<vMessageSender.vGlobalMessage> globalMessages;

		// Token: 0x020004A8 RID: 1192
		[Serializable]
		public class vMessage
		{
			// Token: 0x04001E2F RID: 7727
			public string name;

			// Token: 0x04001E30 RID: 7728
			public string message;

			// Token: 0x04001E31 RID: 7729
			[vHelpBox("- sendByTrigger (You can use vSimpleTrigger to verify the Player and Send messages using Events by calling the 'OnTrigger' method", vHelpBoxAttribute.MessageType.None)]
			public bool sendByTrigger;

			// Token: 0x04001E32 RID: 7730
			public List<vMessageReceiver> defaultReceivers;
		}

		// Token: 0x020004A9 RID: 1193
		[Serializable]
		public class vGlobalMessage
		{
			// Token: 0x04001E33 RID: 7731
			public string name;

			// Token: 0x04001E34 RID: 7732
			public string message;
		}
	}
}
