using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200007A RID: 122
	[AddComponentMenu("More Mountains/Tools/Rigidbody Interface/MMRigidbodyInterface")]
	public class MMRigidbodyInterface : MonoBehaviour
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000DE13 File Offset: 0x0000C013
		// (set) Token: 0x060002BF RID: 703 RVA: 0x0000DE53 File Offset: 0x0000C053
		public Vector3 position
		{
			get
			{
				if (this._rigidbody2D != null)
				{
					return this._rigidbody2D.position;
				}
				if (this._rigidbody != null)
				{
					return this._rigidbody.position;
				}
				return Vector3.zero;
			}
			set
			{
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000DE55 File Offset: 0x0000C055
		public Rigidbody2D InternalRigidBody2D
		{
			get
			{
				return this._rigidbody2D;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000DE5D File Offset: 0x0000C05D
		public Rigidbody InternalRigidBody
		{
			get
			{
				return this._rigidbody;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000DE68 File Offset: 0x0000C068
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x0000DECC File Offset: 0x0000C0CC
		public Vector3 Velocity
		{
			get
			{
				if (this._mode == "2D")
				{
					return this._rigidbody2D.velocity;
				}
				if (this._mode == "3D")
				{
					return this._rigidbody.velocity;
				}
				return new Vector3(0f, 0f, 0f);
			}
			set
			{
				if (this._mode == "2D")
				{
					this._rigidbody2D.velocity = value;
				}
				if (this._mode == "3D")
				{
					this._rigidbody.velocity = value;
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000DF1C File Offset: 0x0000C11C
		public Bounds ColliderBounds
		{
			get
			{
				if (this._rigidbody2D != null)
				{
					return this._collider2D.bounds;
				}
				if (this._rigidbody != null)
				{
					return this._collider.bounds;
				}
				return default(Bounds);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000DF66 File Offset: 0x0000C166
		public bool isKinematic
		{
			get
			{
				if (this._mode == "2D")
				{
					return this._rigidbody2D.isKinematic;
				}
				return this._mode == "3D" && this._rigidbody.isKinematic;
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
		protected virtual void Awake()
		{
			this._rigidbody2D = base.GetComponent<Rigidbody2D>();
			this._rigidbody = base.GetComponent<Rigidbody>();
			if (this._rigidbody2D != null)
			{
				this._mode = "2D";
				this._collider2D = base.GetComponent<Collider2D>();
			}
			if (this._rigidbody != null)
			{
				this._mode = "3D";
				this._collider = base.GetComponent<Collider>();
			}
			if (this._rigidbody == null && this._rigidbody2D == null)
			{
				string text = "A RigidBodyInterface has been added to ";
				GameObject gameObject = base.gameObject;
				Debug.LogWarning(text + ((gameObject != null) ? gameObject.ToString() : null) + " but there's no Rigidbody or Rigidbody2D on it.", base.gameObject);
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000E060 File Offset: 0x0000C260
		public virtual void AddForce(Vector3 force)
		{
			if (this._mode == "2D")
			{
				this._rigidbody2D.AddForce(force, ForceMode2D.Impulse);
			}
			if (this._mode == "3D")
			{
				this._rigidbody.AddForce(force);
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000E0B0 File Offset: 0x0000C2B0
		public virtual void AddRelativeForce(Vector3 force)
		{
			if (this._mode == "2D")
			{
				this._rigidbody2D.AddRelativeForce(force, ForceMode2D.Impulse);
			}
			if (this._mode == "3D")
			{
				this._rigidbody.AddRelativeForce(force);
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000E100 File Offset: 0x0000C300
		public virtual void MovePosition(Vector3 newPosition)
		{
			if (this._mode == "2D")
			{
				this._rigidbody2D.MovePosition(newPosition);
			}
			if (this._mode == "3D")
			{
				this._rigidbody.MovePosition(newPosition);
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000E150 File Offset: 0x0000C350
		public virtual void ResetAngularVelocity()
		{
			if (this._mode == "2D")
			{
				this._rigidbody2D.angularVelocity = 0f;
			}
			if (this._mode == "3D")
			{
				this._rigidbody.angularVelocity = Vector3.zero;
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000E1A4 File Offset: 0x0000C3A4
		public virtual void ResetRotation()
		{
			if (this._mode == "2D")
			{
				this._rigidbody2D.rotation = 0f;
			}
			if (this._mode == "3D")
			{
				this._rigidbody.rotation = Quaternion.identity;
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000E1F5 File Offset: 0x0000C3F5
		public virtual void IsKinematic(bool status)
		{
			if (this._mode == "2D")
			{
				this._rigidbody2D.isKinematic = status;
			}
			if (this._mode == "3D")
			{
				this._rigidbody.isKinematic = status;
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000E233 File Offset: 0x0000C433
		public virtual void EnableBoxCollider(bool status)
		{
			if (this._mode == "2D")
			{
				base.GetComponent<Collider2D>().enabled = status;
			}
			if (this._mode == "3D")
			{
				base.GetComponent<Collider>().enabled = status;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000E271 File Offset: 0x0000C471
		public bool Is3D
		{
			get
			{
				return this._mode == "3D";
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0000E288 File Offset: 0x0000C488
		public bool Is2D
		{
			get
			{
				return this._mode == "2D";
			}
		}

		// Token: 0x040003F2 RID: 1010
		protected string _mode;

		// Token: 0x040003F3 RID: 1011
		protected Rigidbody2D _rigidbody2D;

		// Token: 0x040003F4 RID: 1012
		protected Rigidbody _rigidbody;

		// Token: 0x040003F5 RID: 1013
		protected Collider2D _collider2D;

		// Token: 0x040003F6 RID: 1014
		protected Collider _collider;

		// Token: 0x040003F7 RID: 1015
		protected Bounds _colliderBounds;
	}
}
