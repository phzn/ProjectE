using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004CB RID: 1227
	[vClassHeader("Effect Sender", "Use this component with the vEffectReceiver added on your Player Root to trigger Effects", openClose = false)]
	public class vEffectSender : vMonoBehaviour
	{
		// Token: 0x06001D9D RID: 7581 RVA: 0x000B5B52 File Offset: 0x000B3D52
		public virtual void SetOverrideEffectSender(Transform t)
		{
			this.overrideEffectSender = t;
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x000B5B5B File Offset: 0x000B3D5B
		public void Send(GameObject target)
		{
			this.Send(target.transform);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x000B5B6C File Offset: 0x000B3D6C
		public void Send(Transform target)
		{
			for (int i = 0; i < this.effects.Length; i++)
			{
				vEffectSender.vEffectStruct vEffectStruct = this.effects[i];
				vEffectStruct.Sender = (this.overrideEffectSender ? this.overrideEffectSender : base.transform);
				vEffectStruct.EffectPosition = base.transform.position;
				vEffectReceiver vEffectReceiver = null;
				switch (vEffectStruct.getReceiverMethod)
				{
				case vEffectSender.vEffectStruct.GetReceiverMethod.InTarget:
					if (target.TryGetComponent<vEffectReceiver>(out vEffectReceiver))
					{
						vEffectReceiver.OnReceiveEffect<vEffectSender.vEffectStruct>(vEffectStruct);
					}
					break;
				case vEffectSender.vEffectStruct.GetReceiverMethod.InParent:
					vEffectReceiver = target.GetComponentInParent<vEffectReceiver>();
					if (vEffectReceiver)
					{
						vEffectReceiver.OnReceiveEffect<vEffectSender.vEffectStruct>(vEffectStruct);
					}
					break;
				case vEffectSender.vEffectStruct.GetReceiverMethod.InChildren:
					vEffectReceiver = target.GetComponentInChildren<vEffectReceiver>();
					if (vEffectReceiver)
					{
						vEffectReceiver.OnReceiveEffect<vEffectSender.vEffectStruct>(vEffectStruct);
					}
					break;
				}
			}
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000B5C2B File Offset: 0x000B3E2B
		public void Send(Collider target)
		{
			this.Send(target.transform);
		}

		// Token: 0x04001EC2 RID: 7874
		[vHelpBox("Make sure you check where the vEffectReceiver is on your player, root object, parented or inside as children. By default is located inside the Invector Components > vEffects", vHelpBoxAttribute.MessageType.None)]
		public vEffectSender.vEffectStruct[] effects;

		// Token: 0x04001EC3 RID: 7875
		public Transform overrideEffectSender;

		// Token: 0x020004CC RID: 1228
		[Serializable]
		public class vEffectStruct : vIEffect
		{
			// Token: 0x17000558 RID: 1368
			// (get) Token: 0x06001DA2 RID: 7586 RVA: 0x000B5C39 File Offset: 0x000B3E39
			public string EffectName
			{
				get
				{
					return this.effectName;
				}
			}

			// Token: 0x17000559 RID: 1369
			// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x000B5C41 File Offset: 0x000B3E41
			public float EffectDuration
			{
				get
				{
					return this.effectDuration;
				}
			}

			// Token: 0x1700055A RID: 1370
			// (get) Token: 0x06001DA4 RID: 7588 RVA: 0x000B5C49 File Offset: 0x000B3E49
			// (set) Token: 0x06001DA5 RID: 7589 RVA: 0x000B5C51 File Offset: 0x000B3E51
			public Vector3 EffectPosition { get; set; }

			// Token: 0x1700055B RID: 1371
			// (get) Token: 0x06001DA6 RID: 7590 RVA: 0x000B5C5A File Offset: 0x000B3E5A
			// (set) Token: 0x06001DA7 RID: 7591 RVA: 0x000B5C62 File Offset: 0x000B3E62
			public Transform Sender { get; set; }

			// Token: 0x04001EC4 RID: 7876
			public vEffectSender.vEffectStruct.GetReceiverMethod getReceiverMethod = vEffectSender.vEffectStruct.GetReceiverMethod.InChildren;

			// Token: 0x04001EC5 RID: 7877
			[SerializeField]
			protected string effectName;

			// Token: 0x04001EC6 RID: 7878
			[SerializeField]
			protected float effectDuration;

			// Token: 0x020004CD RID: 1229
			public enum GetReceiverMethod
			{
				// Token: 0x04001ECA RID: 7882
				InTarget,
				// Token: 0x04001ECB RID: 7883
				InParent,
				// Token: 0x04001ECC RID: 7884
				InChildren
			}
		}
	}
}
