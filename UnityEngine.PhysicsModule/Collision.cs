using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace UnityEngine
{
	// Token: 0x02000010 RID: 16
	public class Collision
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000024FC File Offset: 0x000006FC
		public Vector3 impulse
		{
			get
			{
				return this.m_Pair.ImpulseSum;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002509 File Offset: 0x00000709
		public Vector3 relativeVelocity
		{
			get
			{
				return this.m_Flipped ? this.m_Header.m_RelativeVelocity : (-this.m_Header.m_RelativeVelocity);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002530 File Offset: 0x00000730
		public Rigidbody rigidbody
		{
			get
			{
				return this.body as Rigidbody;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000044 RID: 68 RVA: 0x0000253D File Offset: 0x0000073D
		public ArticulationBody articulationBody
		{
			get
			{
				return this.body as ArticulationBody;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000045 RID: 69 RVA: 0x0000254A File Offset: 0x0000074A
		public Component body
		{
			get
			{
				return this.m_Flipped ? this.m_Header.Body : this.m_Header.OtherBody;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000046 RID: 70 RVA: 0x0000256C File Offset: 0x0000076C
		public Collider collider
		{
			get
			{
				return this.m_Flipped ? this.m_Pair.Collider : this.m_Pair.OtherCollider;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002590 File Offset: 0x00000790
		public Transform transform
		{
			get
			{
				return (this.rigidbody != null) ? this.rigidbody.transform : this.collider.transform;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000025C8 File Offset: 0x000007C8
		public GameObject gameObject
		{
			get
			{
				return (this.body != null) ? this.body.gameObject : this.collider.gameObject;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002600 File Offset: 0x00000800
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002618 File Offset: 0x00000818
		internal bool Flipped
		{
			get
			{
				return this.m_Flipped;
			}
			set
			{
				this.m_Flipped = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002624 File Offset: 0x00000824
		public int contactCount
		{
			get
			{
				return (int)this.m_Pair.m_NbPoints;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002644 File Offset: 0x00000844
		public ContactPoint[] contacts
		{
			get
			{
				bool flag = this.m_LegacyContacts == null;
				if (flag)
				{
					this.m_LegacyContacts = new ContactPoint[this.m_Pair.m_NbPoints];
					this.m_Pair.ExtractContactsArray(this.m_LegacyContacts, this.m_Flipped);
				}
				return this.m_LegacyContacts;
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002699 File Offset: 0x00000899
		public Collision()
		{
			this.m_Header = default(ContactPairHeader);
			this.m_Pair = default(ContactPair);
			this.m_Flipped = false;
			this.m_LegacyContacts = null;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000026D0 File Offset: 0x000008D0
		internal Collision(in ContactPairHeader header, in ContactPair pair, bool flipped)
		{
			this.m_LegacyContacts = new ContactPoint[pair.m_NbPoints];
			pair.ExtractContactsArray(this.m_LegacyContacts, flipped);
			this.m_Header = header;
			this.m_Pair = pair;
			this.m_Flipped = flipped;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000272A File Offset: 0x0000092A
		internal void Reuse(in ContactPairHeader header, in ContactPair pair)
		{
			this.m_Header = header;
			this.m_Pair = pair;
			this.m_LegacyContacts = null;
			this.m_Flipped = false;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002754 File Offset: 0x00000954
		public unsafe ContactPoint GetContact(int index)
		{
			bool flag = index < 0 || index >= this.contactCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot get contact at index {0}. There are {1} contact(s).", index, this.contactCount));
			}
			bool flag2 = this.m_LegacyContacts != null;
			ContactPoint contactPoint;
			if (flag2)
			{
				contactPoint = this.m_LegacyContacts[index];
			}
			else
			{
				float num = (this.m_Flipped ? (-1f) : 1f);
				ContactPairPoint* contactPoint_Internal = this.m_Pair.GetContactPoint_Internal(index);
				contactPoint = new ContactPoint(contactPoint_Internal->m_Position, contactPoint_Internal->m_Normal * num, contactPoint_Internal->m_Impulse, contactPoint_Internal->m_Separation, this.m_Flipped ? this.m_Pair.OtherColliderInstanceID : this.m_Pair.ColliderInstanceID, this.m_Flipped ? this.m_Pair.ColliderInstanceID : this.m_Pair.OtherColliderInstanceID);
			}
			return contactPoint;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002848 File Offset: 0x00000A48
		public int GetContacts(ContactPoint[] contacts)
		{
			bool flag = contacts == null;
			if (flag)
			{
				throw new NullReferenceException("Cannot get contacts as the provided array is NULL.");
			}
			bool flag2 = this.m_LegacyContacts != null;
			int num2;
			if (flag2)
			{
				int num = Mathf.Min(this.m_LegacyContacts.Length, contacts.Length);
				Array.Copy(this.m_LegacyContacts, contacts, num);
				num2 = num;
			}
			else
			{
				num2 = this.m_Pair.ExtractContactsArray(contacts, this.m_Flipped);
			}
			return num2;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000028B0 File Offset: 0x00000AB0
		public int GetContacts(List<ContactPoint> contacts)
		{
			bool flag = contacts == null;
			if (flag)
			{
				throw new NullReferenceException("Cannot get contacts as the provided list is NULL.");
			}
			contacts.Clear();
			bool flag2 = this.m_LegacyContacts != null;
			int num;
			if (flag2)
			{
				contacts.AddRange(this.m_LegacyContacts);
				num = this.m_LegacyContacts.Length;
			}
			else
			{
				int nbPoints = (int)this.m_Pair.m_NbPoints;
				bool flag3 = nbPoints == 0;
				if (flag3)
				{
					num = 0;
				}
				else
				{
					bool flag4 = contacts.Capacity < nbPoints;
					if (flag4)
					{
						contacts.Capacity = nbPoints;
					}
					num = this.m_Pair.ExtractContacts(contacts, this.m_Flipped);
				}
			}
			return num;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002948 File Offset: 0x00000B48
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Do not use Collision.GetEnumerator(), enumerate using non-allocating array returned by Collision.GetContacts() or enumerate using Collision.GetContact(index) instead.", false)]
		public virtual IEnumerator GetEnumerator()
		{
			return this.contacts.GetEnumerator();
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002968 File Offset: 0x00000B68
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use Collision.relativeVelocity instead. (UnityUpgradable) -> relativeVelocity", false)]
		public Vector3 impactForceSum
		{
			get
			{
				return Vector3.zero;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002980 File Offset: 0x00000B80
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Will always return zero.", true)]
		public Vector3 frictionForceSum
		{
			get
			{
				return Vector3.zero;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002998 File Offset: 0x00000B98
		[Obsolete("Please use Collision.rigidbody, Collision.transform or Collision.collider instead", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Component other
		{
			get
			{
				return (this.body != null) ? this.body : this.collider;
			}
		}

		// Token: 0x04000047 RID: 71
		private ContactPairHeader m_Header;

		// Token: 0x04000048 RID: 72
		private ContactPair m_Pair;

		// Token: 0x04000049 RID: 73
		private bool m_Flipped;

		// Token: 0x0400004A RID: 74
		private ContactPoint[] m_LegacyContacts = null;
	}
}
