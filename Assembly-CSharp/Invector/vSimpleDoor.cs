using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Invector
{
	// Token: 0x0200048B RID: 1163
	[RequireComponent(typeof(BoxCollider))]
	[vClassHeader("Simple Door", true, "icon_v2", false, "", openClose = false)]
	public class vSimpleDoor : vMonoBehaviour
	{
		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001C86 RID: 7302 RVA: 0x000B113C File Offset: 0x000AF33C
		// (set) Token: 0x06001C87 RID: 7303 RVA: 0x000B1144 File Offset: 0x000AF344
		public virtual bool startOpened
		{
			get
			{
				return this._startOpened;
			}
			set
			{
				this._startOpened = value;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001C88 RID: 7304 RVA: 0x000B114D File Offset: 0x000AF34D
		// (set) Token: 0x06001C89 RID: 7305 RVA: 0x000B1155 File Offset: 0x000AF355
		public virtual bool autoOpen
		{
			get
			{
				return this._autoOpen;
			}
			set
			{
				this._autoOpen = value;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001C8A RID: 7306 RVA: 0x000B115E File Offset: 0x000AF35E
		// (set) Token: 0x06001C8B RID: 7307 RVA: 0x000B1166 File Offset: 0x000AF366
		public virtual bool autoClose
		{
			get
			{
				return this._autoClose;
			}
			set
			{
				this._autoClose = value;
			}
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000B116F File Offset: 0x000AF36F
		protected virtual void Start()
		{
			if (!this.pivot)
			{
				base.enabled = false;
			}
			if (this.startOpened)
			{
				this.state = vSimpleDoor.DoorState.Closed;
				this.Open();
				return;
			}
			this.onClose.Invoke();
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000B11A8 File Offset: 0x000AF3A8
		protected virtual void OnDrawGizmos()
		{
			if (this.pivot)
			{
				Gizmos.DrawSphere(base.transform.position, 0.1f);
				Gizmos.DrawLine(base.transform.position, this.pivot.position);
				Gizmos.DrawSphere(this.pivot.position, 0.1f);
			}
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000B1207 File Offset: 0x000AF407
		public virtual void SetAutoOpen(bool value)
		{
			this.autoOpen = value;
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x000B1210 File Offset: 0x000AF410
		public virtual void SetAutoClose(bool value)
		{
			this.autoClose = value;
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x000B1219 File Offset: 0x000AF419
		public virtual void Open(bool invert)
		{
			this._invertOpenSide = invert;
			this.Open();
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x000B1228 File Offset: 0x000AF428
		public virtual void Open()
		{
			if (this.state != vSimpleDoor.DoorState.Opening && this.state != vSimpleDoor.DoorState.Opening)
			{
				this.targetDoorAngle = (this.invertOpenSide ? (-this.angleOfOpen) : this.angleOfOpen);
				base.StartCoroutine(this.HandleDoor());
			}
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x000B1266 File Offset: 0x000AF466
		public virtual void Close()
		{
			if (this.state != vSimpleDoor.DoorState.Closing && this.state != vSimpleDoor.DoorState.Closed)
			{
				this.targetDoorAngle = 0f;
				base.StartCoroutine(this.HandleDoor());
			}
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x000B1291 File Offset: 0x000AF491
		public virtual void ToggleOpenClose()
		{
			if (this.state == vSimpleDoor.DoorState.Closed && this.state != vSimpleDoor.DoorState.Opening)
			{
				this.Open();
				return;
			}
			this.Close();
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x000B12B1 File Offset: 0x000AF4B1
		protected virtual IEnumerator HandleDoor()
		{
			bool open = Mathf.Abs(this.targetDoorAngle).Equals(this.angleOfOpen);
			this.state = (open ? vSimpleDoor.DoorState.Opening : vSimpleDoor.DoorState.Closing);
			vSimpleDoor.DoorState doorState = this.state;
			if (doorState != vSimpleDoor.DoorState.Closing)
			{
				if (doorState == vSimpleDoor.DoorState.Opening)
				{
					this.onStartOpen.Invoke();
					if (this.invertOpenSide)
					{
						this.onStartOpenLeft.Invoke();
					}
					else
					{
						this.onStartOpenRight.Invoke();
					}
				}
			}
			else
			{
				this.onStartClose.Invoke();
			}
			this.stopDoor = true;
			yield return new WaitForEndOfFrame();
			this.stopDoor = false;
			while (!this.stopDoor)
			{
				this.currentAngle.y = Mathf.MoveTowardsAngle(this.currentAngle.y, this.targetDoorAngle, open ? this.openSpeed : this.closeSpeed);
				if (Mathf.Abs(this.currentAngle.y - this.targetDoorAngle) < 0.01f)
				{
					this.currentAngle.y = this.targetDoorAngle;
					this.pivot.localEulerAngles = this.currentAngle;
					break;
				}
				this.pivot.localEulerAngles = this.currentAngle;
				yield return null;
			}
			if (!this.stopDoor)
			{
				this.state = (open ? vSimpleDoor.DoorState.Opened : vSimpleDoor.DoorState.Closed);
				if (open && this.autoClose && !this.colliderInTrigger)
				{
					this.CloseWithDelay();
				}
				doorState = this.state;
				if (doorState != vSimpleDoor.DoorState.Closed)
				{
					if (doorState == vSimpleDoor.DoorState.Opened)
					{
						this.onOpen.Invoke();
						if (this.invertOpenSide)
						{
							this.onOpenLeft.Invoke();
						}
						else
						{
							this.onOpenRight.Invoke();
						}
					}
				}
				else
				{
					this.onClose.Invoke();
				}
			}
			yield break;
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x000B12C0 File Offset: 0x000AF4C0
		protected virtual void OnTriggerStay(Collider other)
		{
			if (this.tagsToOpen.Contains(other.tag) && this.autoOpen && (this.state == vSimpleDoor.DoorState.Closing || this.state == vSimpleDoor.DoorState.Closed))
			{
				if (base.transform.InverseTransformPoint(other.transform.position).z > 0f)
				{
					this._invertOpenSide = false;
				}
				else
				{
					this._invertOpenSide = true;
				}
				this.angle = Mathf.Abs(Vector3.Angle(this._invertOpenSide ? base.transform.forward : (-base.transform.forward), other.transform.forward));
				if (this.angle < this.minAngleToOpen)
				{
					if (!this.colliderInTrigger)
					{
						this.colliderInTrigger = other;
					}
					this.Open();
				}
			}
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000B139C File Offset: 0x000AF59C
		protected virtual void OnTriggerExit(Collider other)
		{
			if (this.autoClose && this.tagsToOpen.Contains(other.tag) && ((this.colliderInTrigger != null && this.colliderInTrigger.gameObject.Equals(other.gameObject)) || this.colliderInTrigger == null))
			{
				this.colliderInTrigger = null;
				if (!this.closeOnlyWhenOpened || this.state == vSimpleDoor.DoorState.Opened)
				{
					this.CloseWithDelay();
				}
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x000B1416 File Offset: 0x000AF616
		protected virtual bool invertOpenSide
		{
			get
			{
				return this._invertOpenSide && this.openBothSide;
			}
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000B1428 File Offset: 0x000AF628
		protected virtual void CloseWithDelay()
		{
			base.CancelInvoke("Close");
			base.Invoke("Close", this.timeToClose);
		}

		// Token: 0x04001DC8 RID: 7624
		[vReadOnly(true)]
		public vSimpleDoor.DoorState state;

		// Token: 0x04001DC9 RID: 7625
		public Transform pivot;

		// Token: 0x04001DCA RID: 7626
		[SerializeField]
		protected bool _startOpened;

		// Token: 0x04001DCB RID: 7627
		[SerializeField]
		[FormerlySerializedAs("autoOpen")]
		protected bool _autoOpen = true;

		// Token: 0x04001DCC RID: 7628
		[SerializeField]
		[FormerlySerializedAs("autoClose")]
		protected bool _autoClose = true;

		// Token: 0x04001DCD RID: 7629
		[vHideInInspector("autoClose", false)]
		[Tooltip("Close the door only if door is completely opened\n**The TimeToClose will be used yet")]
		public bool closeOnlyWhenOpened;

		// Token: 0x04001DCE RID: 7630
		[Tooltip("Target angle of Opened door")]
		public float angleOfOpen = 90f;

		// Token: 0x04001DCF RID: 7631
		[vHideInInspector("autoOpen", false)]
		[Tooltip("Min angle between character forward and door that  can auto open")]
		public float minAngleToOpen = 45f;

		// Token: 0x04001DD0 RID: 7632
		[Tooltip("Door can open to left side and to right side, if false, door will open just in to right side")]
		public bool openBothSide = true;

		// Token: 0x04001DD1 RID: 7633
		public float closeSpeed = 2f;

		// Token: 0x04001DD2 RID: 7634
		public float openSpeed = 2f;

		// Token: 0x04001DD3 RID: 7635
		[vHideInInspector("autoClose", false)]
		[Tooltip("Time to auto close door after Opened")]
		public float timeToClose = 1f;

		// Token: 0x04001DD4 RID: 7636
		[Tooltip("Used when autoOpen or autoClose is checked")]
		public vTagMask tagsToOpen = new List<string> { "Player" };

		// Token: 0x04001DD5 RID: 7637
		private Vector3 currentAngle;

		// Token: 0x04001DD6 RID: 7638
		private float angle;

		// Token: 0x04001DD7 RID: 7639
		private bool _invertOpenSide;

		// Token: 0x04001DD8 RID: 7640
		private Collider colliderInTrigger;

		// Token: 0x04001DD9 RID: 7641
		public UnityEvent onStartOpen;

		// Token: 0x04001DDA RID: 7642
		public UnityEvent onStartOpenRight;

		// Token: 0x04001DDB RID: 7643
		public UnityEvent onStartOpenLeft;

		// Token: 0x04001DDC RID: 7644
		public UnityEvent onStartClose;

		// Token: 0x04001DDD RID: 7645
		public UnityEvent onOpen;

		// Token: 0x04001DDE RID: 7646
		public UnityEvent onOpenRight;

		// Token: 0x04001DDF RID: 7647
		public UnityEvent onOpenLeft;

		// Token: 0x04001DE0 RID: 7648
		public UnityEvent onClose;

		// Token: 0x04001DE1 RID: 7649
		private float targetDoorAngle;

		// Token: 0x04001DE2 RID: 7650
		private bool stopDoor;

		// Token: 0x0200048C RID: 1164
		public enum DoorState
		{
			// Token: 0x04001DE4 RID: 7652
			Closed,
			// Token: 0x04001DE5 RID: 7653
			Opened,
			// Token: 0x04001DE6 RID: 7654
			Closing,
			// Token: 0x04001DE7 RID: 7655
			Opening
		}
	}
}
