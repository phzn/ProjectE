using System;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000021 RID: 33
	[Serializable]
	public struct InputActionProperty : IEquatable<InputActionProperty>, IEquatable<InputAction>, IEquatable<InputActionReference>
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001FD RID: 509 RVA: 0x000074DF File Offset: 0x000056DF
		public InputAction action
		{
			get
			{
				if (!this.m_UseReference)
				{
					return this.m_Action;
				}
				if (!(this.m_Reference != null))
				{
					return null;
				}
				return this.m_Reference.action;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000750B File Offset: 0x0000570B
		public InputActionReference reference
		{
			get
			{
				if (!this.m_UseReference)
				{
					return null;
				}
				return this.m_Reference;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000751D File Offset: 0x0000571D
		internal InputAction serializedAction
		{
			get
			{
				return this.m_Action;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00007525 File Offset: 0x00005725
		internal InputActionReference serializedReference
		{
			get
			{
				return this.m_Reference;
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000752D File Offset: 0x0000572D
		public InputActionProperty(InputAction action)
		{
			this.m_UseReference = false;
			this.m_Action = action;
			this.m_Reference = null;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00007544 File Offset: 0x00005744
		public InputActionProperty(InputActionReference reference)
		{
			this.m_UseReference = true;
			this.m_Action = null;
			this.m_Reference = reference;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000755B File Offset: 0x0000575B
		public bool Equals(InputActionProperty other)
		{
			return this.m_Reference == other.m_Reference && this.m_UseReference == other.m_UseReference && this.m_Action == other.m_Action;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000758E File Offset: 0x0000578E
		public bool Equals(InputAction other)
		{
			return this.action == other;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00007599 File Offset: 0x00005799
		public bool Equals(InputActionReference other)
		{
			return this.m_Reference == other;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000075A7 File Offset: 0x000057A7
		public override bool Equals(object obj)
		{
			if (this.m_UseReference)
			{
				return this.Equals(obj as InputActionReference);
			}
			return this.Equals(obj as InputAction);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000075CA File Offset: 0x000057CA
		public override int GetHashCode()
		{
			if (this.m_UseReference)
			{
				if (!(this.m_Reference != null))
				{
					return 0;
				}
				return this.m_Reference.GetHashCode();
			}
			else
			{
				if (this.m_Action == null)
				{
					return 0;
				}
				return this.m_Action.GetHashCode();
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00007605 File Offset: 0x00005805
		public static bool operator ==(InputActionProperty left, InputActionProperty right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000760F File Offset: 0x0000580F
		public static bool operator !=(InputActionProperty left, InputActionProperty right)
		{
			return !left.Equals(right);
		}

		// Token: 0x040000C4 RID: 196
		[SerializeField]
		private bool m_UseReference;

		// Token: 0x040000C5 RID: 197
		[SerializeField]
		private InputAction m_Action;

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		private InputActionReference m_Reference;
	}
}
