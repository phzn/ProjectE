using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000118 RID: 280
	public class MMEmmiterReceiver : MonoBehaviour
	{
		// Token: 0x06000636 RID: 1590 RVA: 0x0001FB10 File Offset: 0x0001DD10
		protected virtual void Awake()
		{
			this.EmitterProperty.Initialization(this.EmitterProperty.TargetComponent.gameObject);
			this.ReceiverProperty.Initialization(this.ReceiverProperty.TargetComponent.gameObject);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001FB48 File Offset: 0x0001DD48
		protected virtual void Update()
		{
			this.EmitValue();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001FB50 File Offset: 0x0001DD50
		protected virtual void EmitValue()
		{
			if (!this.Emitting)
			{
				return;
			}
			float level = this.EmitterProperty.GetLevel();
			if (level != this._levelLastFrame)
			{
				MMEmmiterReceiver.OnValueChangeDelegate onValueChange = this.OnValueChange;
				if (onValueChange != null)
				{
					onValueChange();
				}
				MMPropertyReceiver receiverProperty = this.ReceiverProperty;
				if (receiverProperty != null)
				{
					receiverProperty.SetLevel(level);
				}
			}
			this._levelLastFrame = level;
		}

		// Token: 0x040007EA RID: 2026
		[MMInformation("This component lets you very easily have one property drive the value of another property. To do so, drag the object with the property you want to 'read' from into the Emitter Property slot, then select the component the property is on, and finally the property itself.Then drag the object with the property you want to 'write' to into the ReceiverProperty slot, and pick the property you want to drive with the emitter's value.", MMInformationAttribute.InformationType.Info, false)]
		public bool Emitting = true;

		// Token: 0x040007EB RID: 2027
		[Header("Emitter")]
		[Tooltip("the property whose value you want to read and to have drive the ReceiverProperty's value")]
		public MMPropertyEmitter EmitterProperty;

		// Token: 0x040007EC RID: 2028
		[Header("Receiver")]
		[Tooltip("the property whose value you want to be driven by the EmitterProperty's value")]
		public MMPropertyReceiver ReceiverProperty;

		// Token: 0x040007ED RID: 2029
		public MMEmmiterReceiver.OnValueChangeDelegate OnValueChange;

		// Token: 0x040007EE RID: 2030
		protected float _levelLastFrame;

		// Token: 0x0200043B RID: 1083
		// (Invoke) Token: 0x06001A2A RID: 6698
		public delegate void OnValueChangeDelegate();
	}
}
