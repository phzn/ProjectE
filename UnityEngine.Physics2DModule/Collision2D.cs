using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000019 RID: 25
	[RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class Collision2D
	{
		// Token: 0x06000228 RID: 552 RVA: 0x000068B0 File Offset: 0x00004AB0
		private ContactPoint2D[] GetContacts_Internal()
		{
			return (this.m_LegacyContacts == null) ? this.m_ReusedContacts : this.m_LegacyContacts;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000068D8 File Offset: 0x00004AD8
		public Collider2D collider
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_Collider) as Collider2D;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600022A RID: 554 RVA: 0x000068FC File Offset: 0x00004AFC
		public Collider2D otherCollider
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_OtherCollider) as Collider2D;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00006920 File Offset: 0x00004B20
		public Rigidbody2D rigidbody
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_Rigidbody) as Rigidbody2D;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00006944 File Offset: 0x00004B44
		public Rigidbody2D otherRigidbody
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_OtherRigidbody) as Rigidbody2D;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600022D RID: 557 RVA: 0x00006968 File Offset: 0x00004B68
		public Transform transform
		{
			get
			{
				return (this.rigidbody != null) ? this.rigidbody.transform : this.collider.transform;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600022E RID: 558 RVA: 0x000069A0 File Offset: 0x00004BA0
		public GameObject gameObject
		{
			get
			{
				return (this.rigidbody != null) ? this.rigidbody.gameObject : this.collider.gameObject;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600022F RID: 559 RVA: 0x000069D8 File Offset: 0x00004BD8
		public Vector2 relativeVelocity
		{
			get
			{
				return this.m_RelativeVelocity;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000230 RID: 560 RVA: 0x000069F0 File Offset: 0x00004BF0
		public bool enabled
		{
			get
			{
				return this.m_Enabled == 1;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000231 RID: 561 RVA: 0x00006A0C File Offset: 0x00004C0C
		public ContactPoint2D[] contacts
		{
			get
			{
				bool flag = this.m_LegacyContacts == null;
				if (flag)
				{
					this.m_LegacyContacts = new ContactPoint2D[this.m_ContactCount];
					Array.Copy(this.m_ReusedContacts, this.m_LegacyContacts, this.m_ContactCount);
				}
				return this.m_LegacyContacts;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00006A5C File Offset: 0x00004C5C
		public int contactCount
		{
			get
			{
				return this.m_ContactCount;
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00006A74 File Offset: 0x00004C74
		public ContactPoint2D GetContact(int index)
		{
			bool flag = index < 0 || index >= this.m_ContactCount;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot get contact at index {0}. There are {1} contact(s).", index, this.m_ContactCount));
			}
			return this.GetContacts_Internal()[index];
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00006ACC File Offset: 0x00004CCC
		public int GetContacts(ContactPoint2D[] contacts)
		{
			bool flag = contacts == null;
			if (flag)
			{
				throw new NullReferenceException("Cannot get contacts as the provided array is NULL.");
			}
			int num = Mathf.Min(this.m_ContactCount, contacts.Length);
			Array.Copy(this.GetContacts_Internal(), contacts, num);
			return num;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00006B10 File Offset: 0x00004D10
		public int GetContacts(List<ContactPoint2D> contacts)
		{
			bool flag = contacts == null;
			if (flag)
			{
				throw new NullReferenceException("Cannot get contacts as the provided list is NULL.");
			}
			contacts.Clear();
			ContactPoint2D[] contacts_Internal = this.GetContacts_Internal();
			for (int i = 0; i < this.m_ContactCount; i++)
			{
				contacts.Add(contacts_Internal[i]);
			}
			return this.m_ContactCount;
		}

		// Token: 0x0400006B RID: 107
		internal int m_Collider;

		// Token: 0x0400006C RID: 108
		internal int m_OtherCollider;

		// Token: 0x0400006D RID: 109
		internal int m_Rigidbody;

		// Token: 0x0400006E RID: 110
		internal int m_OtherRigidbody;

		// Token: 0x0400006F RID: 111
		internal Vector2 m_RelativeVelocity;

		// Token: 0x04000070 RID: 112
		internal int m_Enabled;

		// Token: 0x04000071 RID: 113
		internal int m_ContactCount;

		// Token: 0x04000072 RID: 114
		internal ContactPoint2D[] m_ReusedContacts;

		// Token: 0x04000073 RID: 115
		internal ContactPoint2D[] m_LegacyContacts;
	}
}
