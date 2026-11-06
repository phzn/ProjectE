using System;
using System.Security.Principal;
using Unity;

namespace System.Security.AccessControl
{
	/// <summary>Represents an access control list (ACL) and is the base class for the <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> and <see cref="T:System.Security.AccessControl.SystemAcl" /> classes.</summary>
	// Token: 0x0200050F RID: 1295
	public abstract class CommonAcl : GenericAcl
	{
		// Token: 0x06003365 RID: 13157 RVA: 0x000BCCE0 File Offset: 0x000BAEE0
		internal CommonAcl(bool isContainer, bool isDS, RawAcl rawAcl)
		{
			if (rawAcl == null)
			{
				rawAcl = new RawAcl(isDS ? GenericAcl.AclRevisionDS : GenericAcl.AclRevision, 10);
			}
			else
			{
				byte[] array = new byte[rawAcl.BinaryLength];
				rawAcl.GetBinaryForm(array, 0);
				rawAcl = new RawAcl(array, 0);
			}
			this.Init(isContainer, isDS, rawAcl);
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x000BCD36 File Offset: 0x000BAF36
		internal CommonAcl(bool isContainer, bool isDS, byte revision, int capacity)
		{
			this.Init(isContainer, isDS, new RawAcl(revision, capacity));
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x000BCD4E File Offset: 0x000BAF4E
		internal CommonAcl(bool isContainer, bool isDS, int capacity)
			: this(isContainer, isDS, isDS ? GenericAcl.AclRevisionDS : GenericAcl.AclRevision, capacity)
		{
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x000BCD68 File Offset: 0x000BAF68
		private void Init(bool isContainer, bool isDS, RawAcl rawAcl)
		{
			this.is_container = isContainer;
			this.is_ds = isDS;
			this.raw_acl = rawAcl;
			this.CanonicalizeAndClearAefa();
		}

		/// <summary>Gets the length, in bytes, of the binary representation of the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object. This length should be used before marshaling the access control list (ACL) into a binary array by using the <see cref="M:System.Security.AccessControl.CommonAcl.GetBinaryForm(System.Byte[],System.Int32)" /> method.</summary>
		/// <returns>The length, in bytes, of the binary representation of the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object.</returns>
		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06003369 RID: 13161 RVA: 0x000BCD85 File Offset: 0x000BAF85
		public sealed override int BinaryLength
		{
			get
			{
				return this.raw_acl.BinaryLength;
			}
		}

		/// <summary>Gets the number of access control entries (ACEs) in the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object.</summary>
		/// <returns>The number of ACEs in the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object.</returns>
		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x0600336A RID: 13162 RVA: 0x000BCD92 File Offset: 0x000BAF92
		public sealed override int Count
		{
			get
			{
				return this.raw_acl.Count;
			}
		}

		/// <summary>Gets a Boolean value that specifies whether the access control entries (ACEs) in the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object are in canonical order.</summary>
		/// <returns>
		///   <see langword="true" /> if the ACEs in the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object are in canonical order; otherwise, <see langword="false" />.</returns>
		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x0600336B RID: 13163 RVA: 0x000BCD9F File Offset: 0x000BAF9F
		public bool IsCanonical
		{
			get
			{
				return this.is_canonical;
			}
		}

		/// <summary>Sets whether the <see cref="T:System.Security.AccessControl.CommonAcl" /> object is a container.</summary>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object is a container.</returns>
		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x0600336C RID: 13164 RVA: 0x000BCDA7 File Offset: 0x000BAFA7
		public bool IsContainer
		{
			get
			{
				return this.is_container;
			}
		}

		/// <summary>Sets whether the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object is a directory object access control list (ACL).</summary>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Security.AccessControl.CommonAcl" /> object is a directory object ACL.</returns>
		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x0600336D RID: 13165 RVA: 0x000BCDAF File Offset: 0x000BAFAF
		public bool IsDS
		{
			get
			{
				return this.is_ds;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x0600336E RID: 13166 RVA: 0x000BCDB7 File Offset: 0x000BAFB7
		// (set) Token: 0x0600336F RID: 13167 RVA: 0x000BCDBF File Offset: 0x000BAFBF
		internal bool IsAefa
		{
			get
			{
				return this.is_aefa;
			}
			set
			{
				this.is_aefa = value;
			}
		}

		/// <summary>Gets the revision level of the <see cref="T:System.Security.AccessControl.CommonAcl" />.</summary>
		/// <returns>A byte value that specifies the revision level of the <see cref="T:System.Security.AccessControl.CommonAcl" />.</returns>
		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06003370 RID: 13168 RVA: 0x000BCDC8 File Offset: 0x000BAFC8
		public sealed override byte Revision
		{
			get
			{
				return this.raw_acl.Revision;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Security.AccessControl.CommonAce" /> at the specified index.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Security.AccessControl.CommonAce" /> to get or set.</param>
		/// <returns>The <see cref="T:System.Security.AccessControl.CommonAce" /> at the specified index.</returns>
		// Token: 0x17000701 RID: 1793
		public sealed override GenericAce this[int index]
		{
			get
			{
				return CommonAcl.CopyAce(this.raw_acl[index]);
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Marshals the contents of the <see cref="T:System.Security.AccessControl.CommonAcl" /> object into the specified byte array beginning at the specified offset.</summary>
		/// <param name="binaryForm">The byte array into which the contents of the <see cref="T:System.Security.AccessControl.CommonAcl" /> is marshaled.</param>
		/// <param name="offset">The offset at which to start marshaling.</param>
		// Token: 0x06003373 RID: 13171 RVA: 0x000BCDE8 File Offset: 0x000BAFE8
		public sealed override void GetBinaryForm(byte[] binaryForm, int offset)
		{
			this.raw_acl.GetBinaryForm(binaryForm, offset);
		}

		/// <summary>Removes all access control entries (ACEs) contained by this <see cref="T:System.Security.AccessControl.CommonAcl" /> object that are associated with the specified <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</summary>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> object to check for.</param>
		// Token: 0x06003374 RID: 13172 RVA: 0x000BCDF8 File Offset: 0x000BAFF8
		public void Purge(SecurityIdentifier sid)
		{
			this.RequireCanonicity();
			this.RemoveAces<KnownAce>((KnownAce ace) => ace.SecurityIdentifier == sid);
		}

		/// <summary>Removes all inherited access control entries (ACEs) from this <see cref="T:System.Security.AccessControl.CommonAcl" /> object.</summary>
		// Token: 0x06003375 RID: 13173 RVA: 0x000BCE2A File Offset: 0x000BB02A
		public void RemoveInheritedAces()
		{
			this.RequireCanonicity();
			this.RemoveAces<GenericAce>((GenericAce ace) => ace.IsInherited);
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x000BCE57 File Offset: 0x000BB057
		internal void RequireCanonicity()
		{
			if (!this.IsCanonical)
			{
				throw new InvalidOperationException("ACL is not canonical.");
			}
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x000BCE6C File Offset: 0x000BB06C
		internal void CanonicalizeAndClearAefa()
		{
			this.RemoveAces<GenericAce>(new CommonAcl.RemoveAcesCallback<GenericAce>(this.IsAceMeaningless));
			this.is_canonical = this.TestCanonicity();
			if (this.IsCanonical)
			{
				this.ApplyCanonicalSortToExplicitAces();
				this.MergeExplicitAces();
			}
			this.IsAefa = false;
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x000BCEA8 File Offset: 0x000BB0A8
		internal virtual bool IsAceMeaningless(GenericAce ace)
		{
			AceFlags aceFlags = ace.AceFlags;
			KnownAce knownAce = ace as KnownAce;
			if (knownAce != null)
			{
				if (knownAce.AccessMask == 0)
				{
					return true;
				}
				if ((aceFlags & AceFlags.InheritOnly) != AceFlags.None)
				{
					if (knownAce is ObjectAce)
					{
						return true;
					}
					if (!this.IsContainer)
					{
						return true;
					}
					if ((aceFlags & (AceFlags.ObjectInherit | AceFlags.ContainerInherit)) == AceFlags.None)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x000BCEF8 File Offset: 0x000BB0F8
		private bool TestCanonicity()
		{
			AceEnumerator aceEnumerator = base.GetEnumerator();
			while (aceEnumerator.MoveNext())
			{
				if (!(aceEnumerator.Current is QualifiedAce))
				{
					return false;
				}
			}
			bool flag = false;
			aceEnumerator = base.GetEnumerator();
			while (aceEnumerator.MoveNext())
			{
				if (((QualifiedAce)aceEnumerator.Current).IsInherited)
				{
					flag = true;
				}
				else if (flag)
				{
					return false;
				}
			}
			bool flag2 = false;
			foreach (GenericAce genericAce in this)
			{
				QualifiedAce qualifiedAce = (QualifiedAce)genericAce;
				if (qualifiedAce.IsInherited)
				{
					break;
				}
				if (qualifiedAce.AceQualifier == AceQualifier.AccessAllowed)
				{
					flag2 = true;
				}
				else if (AceQualifier.AccessDenied == qualifiedAce.AceQualifier && flag2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x000BCF98 File Offset: 0x000BB198
		internal int GetCanonicalExplicitDenyAceCount()
		{
			int num = 0;
			while (num < this.Count && !this.raw_acl[num].IsInherited)
			{
				QualifiedAce qualifiedAce = this.raw_acl[num] as QualifiedAce;
				if (qualifiedAce == null || qualifiedAce.AceQualifier != AceQualifier.AccessDenied)
				{
					break;
				}
				num++;
			}
			return num;
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x000BCFF0 File Offset: 0x000BB1F0
		internal int GetCanonicalExplicitAceCount()
		{
			int num = 0;
			while (num < this.Count && !this.raw_acl[num].IsInherited)
			{
				num++;
			}
			return num;
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x000BD024 File Offset: 0x000BB224
		private void MergeExplicitAces()
		{
			int num = this.GetCanonicalExplicitAceCount();
			int i = 0;
			while (i < num - 1)
			{
				GenericAce genericAce = this.MergeExplicitAcePair(this.raw_acl[i], this.raw_acl[i + 1]);
				if (null != genericAce)
				{
					this.raw_acl[i] = genericAce;
					this.raw_acl.RemoveAce(i + 1);
					num--;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x000BD094 File Offset: 0x000BB294
		private GenericAce MergeExplicitAcePair(GenericAce ace1, GenericAce ace2)
		{
			QualifiedAce qualifiedAce = ace1 as QualifiedAce;
			QualifiedAce qualifiedAce2 = ace2 as QualifiedAce;
			if (!(null != qualifiedAce) || !(null != qualifiedAce2))
			{
				return null;
			}
			if (qualifiedAce.AceQualifier != qualifiedAce2.AceQualifier)
			{
				return null;
			}
			if (!(qualifiedAce.SecurityIdentifier == qualifiedAce2.SecurityIdentifier))
			{
				return null;
			}
			AceFlags aceFlags = qualifiedAce.AceFlags;
			AceFlags aceFlags2 = qualifiedAce2.AceFlags;
			int accessMask = qualifiedAce.AccessMask;
			int accessMask2 = qualifiedAce2.AccessMask;
			if (!this.IsContainer)
			{
				aceFlags &= ~(AceFlags.ObjectInherit | AceFlags.ContainerInherit | AceFlags.NoPropagateInherit | AceFlags.InheritOnly);
				aceFlags2 &= ~(AceFlags.ObjectInherit | AceFlags.ContainerInherit | AceFlags.NoPropagateInherit | AceFlags.InheritOnly);
			}
			AceFlags aceFlags3;
			int num;
			if (aceFlags != aceFlags2)
			{
				if (accessMask != accessMask2)
				{
					return null;
				}
				if ((aceFlags & ~(AceFlags.ObjectInherit | AceFlags.ContainerInherit)) == (aceFlags2 & ~(AceFlags.ObjectInherit | AceFlags.ContainerInherit)))
				{
					aceFlags3 = aceFlags | aceFlags2;
					num = accessMask;
				}
				else
				{
					if ((aceFlags & ~(AceFlags.SuccessfulAccess | AceFlags.FailedAccess)) != (aceFlags2 & ~(AceFlags.SuccessfulAccess | AceFlags.FailedAccess)))
					{
						return null;
					}
					aceFlags3 = aceFlags | aceFlags2;
					num = accessMask;
				}
			}
			else
			{
				aceFlags3 = aceFlags;
				num = accessMask | accessMask2;
			}
			CommonAce commonAce = ace1 as CommonAce;
			CommonAce commonAce2 = ace2 as CommonAce;
			if (null != commonAce && null != commonAce2)
			{
				return new CommonAce(aceFlags3, commonAce.AceQualifier, num, commonAce.SecurityIdentifier, commonAce.IsCallback, commonAce.GetOpaque());
			}
			ObjectAce objectAce = ace1 as ObjectAce;
			ObjectAce objectAce2 = ace2 as ObjectAce;
			if (null != objectAce && null != objectAce2)
			{
				Guid guid;
				Guid guid2;
				CommonAcl.GetObjectAceTypeGuids(objectAce, out guid, out guid2);
				Guid guid3;
				Guid guid4;
				CommonAcl.GetObjectAceTypeGuids(objectAce2, out guid3, out guid4);
				if (guid == guid3 && guid2 == guid4)
				{
					return new ObjectAce(aceFlags3, objectAce.AceQualifier, num, objectAce.SecurityIdentifier, objectAce.ObjectAceFlags, objectAce.ObjectAceType, objectAce.InheritedObjectAceType, objectAce.IsCallback, objectAce.GetOpaque());
				}
			}
			return null;
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x000BD23C File Offset: 0x000BB43C
		private static void GetObjectAceTypeGuids(ObjectAce ace, out Guid type, out Guid inheritedType)
		{
			type = Guid.Empty;
			inheritedType = Guid.Empty;
			if ((ace.ObjectAceFlags & ObjectAceFlags.ObjectAceTypePresent) != ObjectAceFlags.None)
			{
				type = ace.ObjectAceType;
			}
			if ((ace.ObjectAceFlags & ObjectAceFlags.InheritedObjectAceTypePresent) != ObjectAceFlags.None)
			{
				inheritedType = ace.InheritedObjectAceType;
			}
		}

		// Token: 0x0600337F RID: 13183
		internal abstract void ApplyCanonicalSortToExplicitAces();

		// Token: 0x06003380 RID: 13184 RVA: 0x000BD28C File Offset: 0x000BB48C
		internal void ApplyCanonicalSortToExplicitAces(int start, int count)
		{
			for (int i = start + 1; i < start + count; i++)
			{
				KnownAce knownAce = (KnownAce)this.raw_acl[i];
				SecurityIdentifier securityIdentifier = knownAce.SecurityIdentifier;
				int num = i;
				while (num > start && ((KnownAce)this.raw_acl[num - 1]).SecurityIdentifier.CompareTo(securityIdentifier) > 0)
				{
					this.raw_acl[num] = this.raw_acl[num - 1];
					num--;
				}
				this.raw_acl[num] = knownAce;
			}
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x000BD316 File Offset: 0x000BB516
		internal override string GetSddlForm(ControlFlags sdFlags, bool isDacl)
		{
			return this.raw_acl.GetSddlForm(sdFlags, isDacl);
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x000BD328 File Offset: 0x000BB528
		internal void RemoveAces<T>(CommonAcl.RemoveAcesCallback<T> callback) where T : GenericAce
		{
			int i = 0;
			while (i < this.raw_acl.Count)
			{
				if (this.raw_acl[i] is T && callback((T)((object)this.raw_acl[i])))
				{
					this.raw_acl.RemoveAce(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x000BD388 File Offset: 0x000BB588
		internal void AddAce(AceQualifier aceQualifier, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags)
		{
			QualifiedAce qualifiedAce = this.AddAceGetQualifiedAce(aceQualifier, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags);
			this.AddAce(qualifiedAce);
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x000BD3AC File Offset: 0x000BB5AC
		internal void AddAce(AceQualifier aceQualifier, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			QualifiedAce qualifiedAce = this.AddAceGetQualifiedAce(aceQualifier, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags, objectFlags, objectType, inheritedObjectType);
			this.AddAce(qualifiedAce);
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x000BD3D8 File Offset: 0x000BB5D8
		private QualifiedAce AddAceGetQualifiedAce(AceQualifier aceQualifier, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			if (!this.IsDS)
			{
				throw new InvalidOperationException("For this overload, IsDS must be true.");
			}
			if (objectFlags == ObjectAceFlags.None)
			{
				return this.AddAceGetQualifiedAce(aceQualifier, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags);
			}
			return new ObjectAce(this.GetAceFlags(inheritanceFlags, propagationFlags, auditFlags), aceQualifier, accessMask, sid, objectFlags, objectType, inheritedObjectType, false, null);
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x000BD428 File Offset: 0x000BB628
		private QualifiedAce AddAceGetQualifiedAce(AceQualifier aceQualifier, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags)
		{
			return new CommonAce(this.GetAceFlags(inheritanceFlags, propagationFlags, auditFlags), aceQualifier, accessMask, sid, false, null);
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x000BD440 File Offset: 0x000BB640
		private void AddAce(QualifiedAce newAce)
		{
			this.RequireCanonicity();
			int aceInsertPosition = this.GetAceInsertPosition(newAce.AceQualifier);
			this.raw_acl.InsertAce(aceInsertPosition, CommonAcl.CopyAce(newAce));
			this.CanonicalizeAndClearAefa();
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x000BD478 File Offset: 0x000BB678
		private static GenericAce CopyAce(GenericAce ace)
		{
			byte[] array = new byte[ace.BinaryLength];
			ace.GetBinaryForm(array, 0);
			return GenericAce.CreateFromBinaryForm(array, 0);
		}

		// Token: 0x06003389 RID: 13193
		internal abstract int GetAceInsertPosition(AceQualifier aceQualifier);

		// Token: 0x0600338A RID: 13194 RVA: 0x000BD4A0 File Offset: 0x000BB6A0
		private AceFlags GetAceFlags(InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags)
		{
			if (inheritanceFlags != InheritanceFlags.None && !this.IsContainer)
			{
				throw new ArgumentException("Flags only work with containers.", "inheritanceFlags");
			}
			if (inheritanceFlags == InheritanceFlags.None && propagationFlags != PropagationFlags.None)
			{
				throw new ArgumentException("Propagation flags need inheritance flags.", "propagationFlags");
			}
			AceFlags aceFlags = AceFlags.None;
			if ((InheritanceFlags.ContainerInherit & inheritanceFlags) != InheritanceFlags.None)
			{
				aceFlags |= AceFlags.ContainerInherit;
			}
			if ((InheritanceFlags.ObjectInherit & inheritanceFlags) != InheritanceFlags.None)
			{
				aceFlags |= AceFlags.ObjectInherit;
			}
			if ((PropagationFlags.InheritOnly & propagationFlags) != PropagationFlags.None)
			{
				aceFlags |= AceFlags.InheritOnly;
			}
			if ((PropagationFlags.NoPropagateInherit & propagationFlags) != PropagationFlags.None)
			{
				aceFlags |= AceFlags.NoPropagateInherit;
			}
			if ((AuditFlags.Success & auditFlags) != AuditFlags.None)
			{
				aceFlags |= AceFlags.SuccessfulAccess;
			}
			if ((AuditFlags.Failure & auditFlags) != AuditFlags.None)
			{
				aceFlags |= AceFlags.FailedAccess;
			}
			return aceFlags;
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x000BD51C File Offset: 0x000BB71C
		internal void RemoveAceSpecific(AceQualifier aceQualifier, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags)
		{
			this.RequireCanonicity();
			this.RemoveAces<CommonAce>((CommonAce ace) => ace.AccessMask == accessMask && ace.AceQualifier == aceQualifier && !(ace.SecurityIdentifier != sid) && ace.InheritanceFlags == inheritanceFlags && (inheritanceFlags == InheritanceFlags.None || ace.PropagationFlags == propagationFlags) && ace.AuditFlags == auditFlags);
			this.CanonicalizeAndClearAefa();
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x000BD57C File Offset: 0x000BB77C
		internal void RemoveAceSpecific(AceQualifier aceQualifier, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			if (!this.IsDS)
			{
				throw new InvalidOperationException("For this overload, IsDS must be true.");
			}
			if (objectFlags == ObjectAceFlags.None)
			{
				this.RemoveAceSpecific(aceQualifier, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags);
				return;
			}
			this.RequireCanonicity();
			this.RemoveAces<ObjectAce>((ObjectAce ace) => ace.AccessMask == accessMask && ace.AceQualifier == aceQualifier && !(ace.SecurityIdentifier != sid) && ace.InheritanceFlags == inheritanceFlags && (inheritanceFlags == InheritanceFlags.None || ace.PropagationFlags == propagationFlags) && ace.AuditFlags == auditFlags && ace.ObjectAceFlags == objectFlags && ((objectFlags & ObjectAceFlags.ObjectAceTypePresent) == ObjectAceFlags.None || !(ace.ObjectAceType != objectType)) && ((objectFlags & ObjectAceFlags.InheritedObjectAceTypePresent) == ObjectAceFlags.None || !(ace.InheritedObjectAceType != objectType)));
			this.CanonicalizeAndClearAefa();
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x000BD630 File Offset: 0x000BB830
		internal void SetAce(AceQualifier aceQualifier, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags)
		{
			QualifiedAce qualifiedAce = this.AddAceGetQualifiedAce(aceQualifier, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags);
			this.SetAce(qualifiedAce);
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x000BD654 File Offset: 0x000BB854
		internal void SetAce(AceQualifier aceQualifier, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			QualifiedAce qualifiedAce = this.AddAceGetQualifiedAce(aceQualifier, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags, objectFlags, objectType, inheritedObjectType);
			this.SetAce(qualifiedAce);
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x000BD680 File Offset: 0x000BB880
		private void SetAce(QualifiedAce newAce)
		{
			this.RequireCanonicity();
			this.RemoveAces<QualifiedAce>((QualifiedAce oldAce) => oldAce.AceQualifier == newAce.AceQualifier && oldAce.SecurityIdentifier == newAce.SecurityIdentifier);
			this.CanonicalizeAndClearAefa();
			this.AddAce(newAce);
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x000173AD File Offset: 0x000155AD
		internal CommonAcl()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0400243E RID: 9278
		private const int default_capacity = 10;

		// Token: 0x0400243F RID: 9279
		private bool is_aefa;

		// Token: 0x04002440 RID: 9280
		private bool is_canonical;

		// Token: 0x04002441 RID: 9281
		private bool is_container;

		// Token: 0x04002442 RID: 9282
		private bool is_ds;

		// Token: 0x04002443 RID: 9283
		internal RawAcl raw_acl;

		// Token: 0x02000510 RID: 1296
		// (Invoke) Token: 0x06003392 RID: 13202
		internal delegate bool RemoveAcesCallback<T>(T ace);
	}
}
