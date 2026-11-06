using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	/// <summary>Represents a Discretionary Access Control List (DACL).</summary>
	// Token: 0x02000522 RID: 1314
	public sealed class DiscretionaryAcl : CommonAcl
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> class with the specified values.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object is a container.</param>
		/// <param name="isDS">
		///   <see langword="true" /> if the new <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object is a directory object Access Control List (ACL).</param>
		/// <param name="capacity">The number of Access Control Entries (ACEs) this <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object can contain. This number is to be used only as a hint.</param>
		// Token: 0x0600340B RID: 13323 RVA: 0x000BE770 File Offset: 0x000BC970
		public DiscretionaryAcl(bool isContainer, bool isDS, int capacity)
			: base(isContainer, isDS, capacity)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> class with the specified values from the specified <see cref="T:System.Security.AccessControl.RawAcl" /> object.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object is a container.</param>
		/// <param name="isDS">
		///   <see langword="true" /> if the new <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object is a directory object Access Control List (ACL).</param>
		/// <param name="rawAcl">The underlying <see cref="T:System.Security.AccessControl.RawAcl" /> object for the new <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object. Specify <see langword="null" /> to create an empty ACL.</param>
		// Token: 0x0600340C RID: 13324 RVA: 0x000BE77B File Offset: 0x000BC97B
		public DiscretionaryAcl(bool isContainer, bool isDS, RawAcl rawAcl)
			: base(isContainer, isDS, rawAcl)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> class with the specified values.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object is a container.</param>
		/// <param name="isDS">
		///   <see langword="true" /> if the new <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object is a directory object Access Control List (ACL).</param>
		/// <param name="revision">The revision level of the new <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object.</param>
		/// <param name="capacity">The number of Access Control Entries (ACEs) this <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object can contain. This number is to be used only as a hint.</param>
		// Token: 0x0600340D RID: 13325 RVA: 0x000BE786 File Offset: 0x000BC986
		public DiscretionaryAcl(bool isContainer, bool isDS, byte revision, int capacity)
			: base(isContainer, isDS, revision, capacity)
		{
		}

		/// <summary>Adds an Access Control Entry (ACE) with the specified settings to the current <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to add.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to add an ACE.</param>
		/// <param name="accessMask">The access rule for the new ACE.</param>
		/// <param name="inheritanceFlags">Flags that specify the inheritance properties of the new ACE.</param>
		/// <param name="propagationFlags">Flags that specify the inheritance propagation properties for the new ACE.</param>
		// Token: 0x0600340E RID: 13326 RVA: 0x000BE793 File Offset: 0x000BC993
		public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			base.AddAce(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None);
		}

		/// <summary>Adds an Access Control Entry (ACE) with the specified settings to the current <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object. Use this method for directory object Access Control Lists (ACLs) when specifying the object type or the inherited object type for the new ACE.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to add.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to add an ACE.</param>
		/// <param name="accessMask">The access rule for the new ACE.</param>
		/// <param name="inheritanceFlags">Flags that specify the inheritance properties of the new ACE.</param>
		/// <param name="propagationFlags">Flags that specify the inheritance propagation properties for the new ACE.</param>
		/// <param name="objectFlags">Flags that specify if the <paramref name="objectType" /> and <paramref name="inheritedObjectType" /> parameters contain non-<see langword="null" /> values.</param>
		/// <param name="objectType">The identity of the class of objects to which the new ACE applies.</param>
		/// <param name="inheritedObjectType">The identity of the class of child objects which can inherit the new ACE.</param>
		// Token: 0x0600340F RID: 13327 RVA: 0x000BE7A8 File Offset: 0x000BC9A8
		public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			base.AddAce(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None, objectFlags, objectType, inheritedObjectType);
		}

		/// <summary>Adds an Access Control Entry (ACE) with the specified settings to the current <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to add.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to add an ACE.</param>
		/// <param name="rule">The <see cref="T:System.Security.AccessControl.ObjectAccessRule" /> for the new access.</param>
		// Token: 0x06003410 RID: 13328 RVA: 0x000BE7D0 File Offset: 0x000BC9D0
		public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
		{
			this.AddAccess(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		/// <summary>Removes the specified access control rule from the current <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to remove.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to remove an access control rule.</param>
		/// <param name="accessMask">The access mask for the rule to be removed.</param>
		/// <param name="inheritanceFlags">Flags that specify the inheritance properties of the rule to be removed.</param>
		/// <param name="propagationFlags">Flags that specify the inheritance propagation properties for the rule to be removed.</param>
		/// <returns>
		///   <see langword="true" /> if this method successfully removes the specified access; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003411 RID: 13329 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			throw new NotImplementedException();
		}

		/// <summary>Removes the specified access control rule from the current <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object. Use this method for directory object Access Control Lists (ACLs) when specifying the object type or the inherited object type.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to remove.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to remove an access control rule.</param>
		/// <param name="accessMask">The access mask for the access control rule to be removed.</param>
		/// <param name="inheritanceFlags">Flags that specify the inheritance properties of the access control rule to be removed.</param>
		/// <param name="propagationFlags">Flags that specify the inheritance propagation properties for the access control rule to be removed.</param>
		/// <param name="objectFlags">Flags that specify if the <paramref name="objectType" /> and <paramref name="inheritedObjectType" /> parameters contain non-<see langword="null" /> values.</param>
		/// <param name="objectType">The identity of the class of objects to which the removed access control rule applies.</param>
		/// <param name="inheritedObjectType">The identity of the class of child objects which can inherit the removed access control rule.</param>
		/// <returns>
		///   <see langword="true" /> if this method successfully removes the specified access; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003412 RID: 13330 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			throw new NotImplementedException();
		}

		/// <summary>Removes the specified access control rule from the current <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to remove.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to remove an access control rule.</param>
		/// <param name="rule">The <see cref="T:System.Security.AccessControl.ObjectAccessRule" /> for which to remove access.</param>
		/// <returns>Returns <see cref="T:System.Boolean" />.</returns>
		// Token: 0x06003413 RID: 13331 RVA: 0x000BE80C File Offset: 0x000BCA0C
		public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
		{
			return this.RemoveAccess(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		/// <summary>Removes the specified Access Control Entry (ACE) from the current <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to remove.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to remove an ACE.</param>
		/// <param name="accessMask">The access mask for the ACE to be removed.</param>
		/// <param name="inheritanceFlags">Flags that specify the inheritance properties of the ACE to be removed.</param>
		/// <param name="propagationFlags">Flags that specify the inheritance propagation properties for the ACE to be removed.</param>
		// Token: 0x06003414 RID: 13332 RVA: 0x000BE845 File Offset: 0x000BCA45
		public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			base.RemoveAceSpecific(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None);
		}

		/// <summary>Removes the specified Access Control Entry (ACE) from the current <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object. Use this method for directory object Access Control Lists (ACLs) when specifying the object type or the inherited object type for the ACE to be removed.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to remove.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to remove an ACE.</param>
		/// <param name="accessMask">The access mask for the ACE to be removed.</param>
		/// <param name="inheritanceFlags">Flags that specify the inheritance properties of the ACE to be removed.</param>
		/// <param name="propagationFlags">Flags that specify the inheritance propagation properties for the ACE to be removed.</param>
		/// <param name="objectFlags">Flags that specify if the <paramref name="objectType" /> and <paramref name="inheritedObjectType" /> parameters contain non-<see langword="null" /> values.</param>
		/// <param name="objectType">The identity of the class of objects to which the removed ACE applies.</param>
		/// <param name="inheritedObjectType">The identity of the class of child objects which can inherit the removed ACE.</param>
		// Token: 0x06003415 RID: 13333 RVA: 0x000BE85C File Offset: 0x000BCA5C
		public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			base.RemoveAceSpecific(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None, objectFlags, objectType, inheritedObjectType);
		}

		/// <summary>Removes the specified Access Control Entry (ACE) from the current <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to remove.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to remove an ACE.</param>
		/// <param name="rule">The <see cref="T:System.Security.AccessControl.ObjectAccessRule" /> for which to remove access.</param>
		// Token: 0x06003416 RID: 13334 RVA: 0x000BE884 File Offset: 0x000BCA84
		public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
		{
			this.RemoveAccessSpecific(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		/// <summary>Sets the specified access control for the specified <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to set.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to set an ACE.</param>
		/// <param name="accessMask">The access rule for the new ACE.</param>
		/// <param name="inheritanceFlags">Flags that specify the inheritance properties of the new ACE.</param>
		/// <param name="propagationFlags">Flags that specify the inheritance propagation properties for the new ACE.</param>
		// Token: 0x06003417 RID: 13335 RVA: 0x000BE8BD File Offset: 0x000BCABD
		public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			base.SetAce(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None);
		}

		/// <summary>Sets the specified access control for the specified <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to set.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to set an ACE.</param>
		/// <param name="accessMask">The access rule for the new ACE.</param>
		/// <param name="inheritanceFlags">Flags that specify the inheritance properties of the new ACE.</param>
		/// <param name="propagationFlags">Flags that specify the inheritance propagation properties for the new ACE.</param>
		/// <param name="objectFlags">Flags that specify if the <paramref name="objectType" /> and <paramref name="inheritedObjectType" /> parameters contain non-<see langword="null" /> values.</param>
		/// <param name="objectType">The identity of the class of objects to which the new ACE applies.</param>
		/// <param name="inheritedObjectType">The identity of the class of child objects which can inherit the new ACE.</param>
		// Token: 0x06003418 RID: 13336 RVA: 0x000BE8D4 File Offset: 0x000BCAD4
		public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			base.SetAce(DiscretionaryAcl.GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None, objectFlags, objectType, inheritedObjectType);
		}

		/// <summary>Sets the specified access control for the specified <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</summary>
		/// <param name="accessType">The type of access control (allow or deny) to set.</param>
		/// <param name="sid">The <see cref="T:System.Security.Principal.SecurityIdentifier" /> for which to set an ACE.</param>
		/// <param name="rule">The <see cref="T:System.Security.AccessControl.ObjectAccessRule" /> for which to set access.</param>
		// Token: 0x06003419 RID: 13337 RVA: 0x000BE8FC File Offset: 0x000BCAFC
		public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
		{
			this.SetAccess(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x000BE938 File Offset: 0x000BCB38
		internal override void ApplyCanonicalSortToExplicitAces()
		{
			int canonicalExplicitAceCount = base.GetCanonicalExplicitAceCount();
			int canonicalExplicitDenyAceCount = base.GetCanonicalExplicitDenyAceCount();
			base.ApplyCanonicalSortToExplicitAces(0, canonicalExplicitDenyAceCount);
			base.ApplyCanonicalSortToExplicitAces(canonicalExplicitDenyAceCount, canonicalExplicitAceCount - canonicalExplicitDenyAceCount);
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x000BE965 File Offset: 0x000BCB65
		internal override int GetAceInsertPosition(AceQualifier aceQualifier)
		{
			if (aceQualifier == AceQualifier.AccessAllowed)
			{
				return base.GetCanonicalExplicitDenyAceCount();
			}
			return 0;
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x000BE972 File Offset: 0x000BCB72
		private static AceQualifier GetAceQualifier(AccessControlType accessType)
		{
			if (accessType == AccessControlType.Allow)
			{
				return AceQualifier.AccessAllowed;
			}
			if (AccessControlType.Deny == accessType)
			{
				return AceQualifier.AccessDenied;
			}
			throw new ArgumentOutOfRangeException("accessType");
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x000BE98C File Offset: 0x000BCB8C
		internal override bool IsAceMeaningless(GenericAce ace)
		{
			if (base.IsAceMeaningless(ace))
			{
				return true;
			}
			if (ace.AuditFlags != AuditFlags.None)
			{
				return true;
			}
			QualifiedAce qualifiedAce = ace as QualifiedAce;
			return null != qualifiedAce && qualifiedAce.AceQualifier != AceQualifier.AccessAllowed && AceQualifier.AccessDenied != qualifiedAce.AceQualifier;
		}
	}
}
