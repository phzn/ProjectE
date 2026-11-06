using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	/// <summary>Provides the ability to control access to directory objects without direct manipulation of Access Control Lists (ACLs).</summary>
	// Token: 0x02000520 RID: 1312
	public abstract class DirectoryObjectSecurity : ObjectSecurity
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> class.</summary>
		// Token: 0x060033F3 RID: 13299 RVA: 0x000BE14D File Offset: 0x000BC34D
		protected DirectoryObjectSecurity()
			: base(true, true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> class with the specified security descriptor.</summary>
		/// <param name="securityDescriptor">The security descriptor to be associated with the new <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object.</param>
		// Token: 0x060033F4 RID: 13300 RVA: 0x000BD868 File Offset: 0x000BBA68
		protected DirectoryObjectSecurity(CommonSecurityDescriptor securityDescriptor)
			: base(securityDescriptor)
		{
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x0004D855 File Offset: 0x0004BA55
		private Exception GetNotImplementedException()
		{
			return new NotImplementedException();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.AccessRule" /> class with the specified values.</summary>
		/// <param name="identityReference">The identity to which the access rule applies.  It must be an object that can be cast as a <see cref="T:System.Security.Principal.SecurityIdentifier" />.</param>
		/// <param name="accessMask">The access mask of this rule. The access mask is a 32-bit collection of anonymous bits, the meaning of which is defined by the individual integrators.</param>
		/// <param name="isInherited">true if this rule is inherited from a parent container.</param>
		/// <param name="inheritanceFlags">Specifies the inheritance properties of the access rule.</param>
		/// <param name="propagationFlags">Specifies whether inherited access rules are automatically propagated. The propagation flags are ignored if <paramref name="inheritanceFlags" /> is set to <see cref="F:System.Security.AccessControl.InheritanceFlags.None" />.</param>
		/// <param name="type">Specifies the valid access control type.</param>
		/// <param name="objectType">The identity of the class of objects to which the new access rule applies.</param>
		/// <param name="inheritedObjectType">The identity of the class of child objects which can inherit the new access rule.</param>
		/// <returns>The <see cref="T:System.Security.AccessControl.AccessRule" /> object that this method creates.</returns>
		// Token: 0x060033F6 RID: 13302 RVA: 0x000BE157 File Offset: 0x000BC357
		public virtual AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type, Guid objectType, Guid inheritedObjectType)
		{
			throw this.GetNotImplementedException();
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x000BE160 File Offset: 0x000BC360
		internal override AccessRule InternalAccessRuleFactory(QualifiedAce ace, Type targetType, AccessControlType type)
		{
			ObjectAce objectAce = ace as ObjectAce;
			if (null == objectAce || objectAce.ObjectAceFlags == ObjectAceFlags.None)
			{
				return base.InternalAccessRuleFactory(ace, targetType, type);
			}
			return this.AccessRuleFactory(ace.SecurityIdentifier.Translate(targetType), ace.AccessMask, ace.IsInherited, ace.InheritanceFlags, ace.PropagationFlags, type, objectAce.ObjectAceType, objectAce.InheritedObjectAceType);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.AuditRule" /> class with the specified values.</summary>
		/// <param name="identityReference">The identity to which the audit rule applies.  It must be an object that can be cast as a <see cref="T:System.Security.Principal.SecurityIdentifier" />.</param>
		/// <param name="accessMask">The access mask of this rule. The access mask is a 32-bit collection of anonymous bits, the meaning of which is defined by the individual integrators.</param>
		/// <param name="isInherited">
		///   <see langword="true" /> if this rule is inherited from a parent container.</param>
		/// <param name="inheritanceFlags">Specifies the inheritance properties of the audit rule.</param>
		/// <param name="propagationFlags">Specifies whether inherited audit rules are automatically propagated. The propagation flags are ignored if <paramref name="inheritanceFlags" /> is set to <see cref="F:System.Security.AccessControl.InheritanceFlags.None" />.</param>
		/// <param name="flags">Specifies the conditions for which the rule is audited.</param>
		/// <param name="objectType">The identity of the class of objects to which the new audit rule applies.</param>
		/// <param name="inheritedObjectType">The identity of the class of child objects which can inherit the new audit rule.</param>
		/// <returns>The <see cref="T:System.Security.AccessControl.AuditRule" /> object that this method creates.</returns>
		// Token: 0x060033F8 RID: 13304 RVA: 0x000BE157 File Offset: 0x000BC357
		public virtual AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags, Guid objectType, Guid inheritedObjectType)
		{
			throw this.GetNotImplementedException();
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x000BE1C8 File Offset: 0x000BC3C8
		internal override AuditRule InternalAuditRuleFactory(QualifiedAce ace, Type targetType)
		{
			ObjectAce objectAce = ace as ObjectAce;
			if (null == objectAce || objectAce.ObjectAceFlags == ObjectAceFlags.None)
			{
				return base.InternalAuditRuleFactory(ace, targetType);
			}
			return this.AuditRuleFactory(ace.SecurityIdentifier.Translate(targetType), ace.AccessMask, ace.IsInherited, ace.InheritanceFlags, ace.PropagationFlags, ace.AuditFlags, objectAce.ObjectAceType, objectAce.InheritedObjectAceType);
		}

		/// <summary>Gets a collection of the access rules associated with the specified security identifier.</summary>
		/// <param name="includeExplicit">
		///   <see langword="true" /> to include access rules explicitly set for the object.</param>
		/// <param name="includeInherited">
		///   <see langword="true" /> to include inherited access rules.</param>
		/// <param name="targetType">The security identifier for which to retrieve access rules. This must be an object that can be cast as a <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</param>
		/// <returns>The collection of access rules associated with the specified <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</returns>
		// Token: 0x060033FA RID: 13306 RVA: 0x000BD871 File Offset: 0x000BBA71
		public AuthorizationRuleCollection GetAccessRules(bool includeExplicit, bool includeInherited, Type targetType)
		{
			return base.InternalGetAccessRules(includeExplicit, includeInherited, targetType);
		}

		/// <summary>Gets a collection of the audit rules associated with the specified security identifier.</summary>
		/// <param name="includeExplicit">
		///   <see langword="true" /> to include audit rules explicitly set for the object.</param>
		/// <param name="includeInherited">
		///   <see langword="true" /> to include inherited audit rules.</param>
		/// <param name="targetType">The security identifier for which to retrieve audit rules. This must be an object that can be cast as a <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</param>
		/// <returns>The collection of audit rules associated with the specified <see cref="T:System.Security.Principal.SecurityIdentifier" /> object.</returns>
		// Token: 0x060033FB RID: 13307 RVA: 0x000BD87C File Offset: 0x000BBA7C
		public AuthorizationRuleCollection GetAuditRules(bool includeExplicit, bool includeInherited, Type targetType)
		{
			return base.InternalGetAuditRules(includeExplicit, includeInherited, targetType);
		}

		/// <summary>Adds the specified access rule to the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object.</summary>
		/// <param name="rule">The access rule to add.</param>
		// Token: 0x060033FC RID: 13308 RVA: 0x000BE234 File Offset: 0x000BC434
		protected void AddAccessRule(ObjectAccessRule rule)
		{
			bool flag;
			this.ModifyAccess(AccessControlModification.Add, rule, out flag);
		}

		/// <summary>Removes access rules that contain the same security identifier and access mask as the specified access rule from the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object.</summary>
		/// <param name="rule">The access rule to remove.</param>
		/// <returns>
		///   <see langword="true" /> if the access rule was successfully removed; otherwise, <see langword="false" />.</returns>
		// Token: 0x060033FD RID: 13309 RVA: 0x000BE24C File Offset: 0x000BC44C
		protected bool RemoveAccessRule(ObjectAccessRule rule)
		{
			bool flag;
			return this.ModifyAccess(AccessControlModification.Remove, rule, out flag);
		}

		/// <summary>Removes all access rules that have the same security identifier as the specified access rule from the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object.</summary>
		/// <param name="rule">The access rule to remove.</param>
		// Token: 0x060033FE RID: 13310 RVA: 0x000BE264 File Offset: 0x000BC464
		protected void RemoveAccessRuleAll(ObjectAccessRule rule)
		{
			bool flag;
			this.ModifyAccess(AccessControlModification.RemoveAll, rule, out flag);
		}

		/// <summary>Removes all access rules that exactly match the specified access rule from the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object.</summary>
		/// <param name="rule">The access rule to remove.</param>
		// Token: 0x060033FF RID: 13311 RVA: 0x000BE27C File Offset: 0x000BC47C
		protected void RemoveAccessRuleSpecific(ObjectAccessRule rule)
		{
			bool flag;
			this.ModifyAccess(AccessControlModification.RemoveSpecific, rule, out flag);
		}

		/// <summary>Removes all access rules in the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object and then adds the specified access rule.</summary>
		/// <param name="rule">The access rule to reset.</param>
		// Token: 0x06003400 RID: 13312 RVA: 0x000BE294 File Offset: 0x000BC494
		protected void ResetAccessRule(ObjectAccessRule rule)
		{
			bool flag;
			this.ModifyAccess(AccessControlModification.Reset, rule, out flag);
		}

		/// <summary>Removes all access rules that contain the same security identifier and qualifier as the specified access rule in the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object and then adds the specified access rule.</summary>
		/// <param name="rule">The access rule to set.</param>
		// Token: 0x06003401 RID: 13313 RVA: 0x000BE2AC File Offset: 0x000BC4AC
		protected void SetAccessRule(ObjectAccessRule rule)
		{
			bool flag;
			this.ModifyAccess(AccessControlModification.Set, rule, out flag);
		}

		/// <summary>Applies the specified modification to the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object.</summary>
		/// <param name="modification">The modification to apply to the DACL.</param>
		/// <param name="rule">The access rule to modify.</param>
		/// <param name="modified">
		///   <see langword="true" /> if the DACL is successfully modified; otherwise, <see langword="false" />.</param>
		/// <returns>
		///   <see langword="true" /> if the DACL is successfully modified; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003402 RID: 13314 RVA: 0x000BE2C4 File Offset: 0x000BC4C4
		protected override bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			ObjectAccessRule objectAccessRule = rule as ObjectAccessRule;
			if (objectAccessRule == null)
			{
				throw new ArgumentException("rule");
			}
			modified = true;
			base.WriteLock();
			try
			{
				switch (modification)
				{
				case AccessControlModification.Add:
					break;
				case AccessControlModification.Set:
					this.descriptor.DiscretionaryAcl.SetAccess(objectAccessRule.AccessControlType, ObjectSecurity.SidFromIR(objectAccessRule.IdentityReference), objectAccessRule.AccessMask, objectAccessRule.InheritanceFlags, objectAccessRule.PropagationFlags, objectAccessRule.ObjectFlags, objectAccessRule.ObjectType, objectAccessRule.InheritedObjectType);
					goto IL_019D;
				case AccessControlModification.Reset:
					this.PurgeAccessRules(objectAccessRule.IdentityReference);
					break;
				case AccessControlModification.Remove:
					modified = this.descriptor.DiscretionaryAcl.RemoveAccess(objectAccessRule.AccessControlType, ObjectSecurity.SidFromIR(objectAccessRule.IdentityReference), rule.AccessMask, objectAccessRule.InheritanceFlags, objectAccessRule.PropagationFlags, objectAccessRule.ObjectFlags, objectAccessRule.ObjectType, objectAccessRule.InheritedObjectType);
					goto IL_019D;
				case AccessControlModification.RemoveAll:
					this.PurgeAccessRules(objectAccessRule.IdentityReference);
					goto IL_019D;
				case AccessControlModification.RemoveSpecific:
					this.descriptor.DiscretionaryAcl.RemoveAccessSpecific(objectAccessRule.AccessControlType, ObjectSecurity.SidFromIR(objectAccessRule.IdentityReference), objectAccessRule.AccessMask, objectAccessRule.InheritanceFlags, objectAccessRule.PropagationFlags, objectAccessRule.ObjectFlags, objectAccessRule.ObjectType, objectAccessRule.InheritedObjectType);
					goto IL_019D;
				default:
					throw new ArgumentOutOfRangeException("modification");
				}
				this.descriptor.DiscretionaryAcl.AddAccess(objectAccessRule.AccessControlType, ObjectSecurity.SidFromIR(objectAccessRule.IdentityReference), objectAccessRule.AccessMask, objectAccessRule.InheritanceFlags, objectAccessRule.PropagationFlags, objectAccessRule.ObjectFlags, objectAccessRule.ObjectType, objectAccessRule.InheritedObjectType);
				IL_019D:
				if (modified)
				{
					base.AccessRulesModified = true;
				}
			}
			finally
			{
				base.WriteUnlock();
			}
			return modified;
		}

		/// <summary>Adds the specified audit rule to the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object.</summary>
		/// <param name="rule">The audit rule to add.</param>
		// Token: 0x06003403 RID: 13315 RVA: 0x000BE4A0 File Offset: 0x000BC6A0
		protected void AddAuditRule(ObjectAuditRule rule)
		{
			bool flag;
			this.ModifyAudit(AccessControlModification.Add, rule, out flag);
		}

		/// <summary>Removes audit rules that contain the same security identifier and access mask as the specified audit rule from the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.CommonObjectSecurity" /> object.</summary>
		/// <param name="rule">The audit rule to remove.</param>
		/// <returns>
		///   <see langword="true" /> if the audit rule was successfully removed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003404 RID: 13316 RVA: 0x000BE4B8 File Offset: 0x000BC6B8
		protected bool RemoveAuditRule(ObjectAuditRule rule)
		{
			bool flag;
			return this.ModifyAudit(AccessControlModification.Remove, rule, out flag);
		}

		/// <summary>Removes all audit rules that have the same security identifier as the specified audit rule from the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object.</summary>
		/// <param name="rule">The audit rule to remove.</param>
		// Token: 0x06003405 RID: 13317 RVA: 0x000BE4D0 File Offset: 0x000BC6D0
		protected void RemoveAuditRuleAll(ObjectAuditRule rule)
		{
			bool flag;
			this.ModifyAudit(AccessControlModification.RemoveAll, rule, out flag);
		}

		/// <summary>Removes all audit rules that exactly match the specified audit rule from the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object.</summary>
		/// <param name="rule">The audit rule to remove.</param>
		// Token: 0x06003406 RID: 13318 RVA: 0x000BE4E8 File Offset: 0x000BC6E8
		protected void RemoveAuditRuleSpecific(ObjectAuditRule rule)
		{
			bool flag;
			this.ModifyAudit(AccessControlModification.RemoveSpecific, rule, out flag);
		}

		/// <summary>Removes all audit rules that contain the same security identifier and qualifier as the specified audit rule in the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object and then adds the specified audit rule.</summary>
		/// <param name="rule">The audit rule to set.</param>
		// Token: 0x06003407 RID: 13319 RVA: 0x000BE500 File Offset: 0x000BC700
		protected void SetAuditRule(ObjectAuditRule rule)
		{
			bool flag;
			this.ModifyAudit(AccessControlModification.Set, rule, out flag);
		}

		/// <summary>Applies the specified modification to the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.DirectoryObjectSecurity" /> object.</summary>
		/// <param name="modification">The modification to apply to the SACL.</param>
		/// <param name="rule">The audit rule to modify.</param>
		/// <param name="modified">
		///   <see langword="true" /> if the SACL is successfully modified; otherwise, <see langword="false" />.</param>
		/// <returns>
		///   <see langword="true" /> if the SACL is successfully modified; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003408 RID: 13320 RVA: 0x000BE518 File Offset: 0x000BC718
		protected override bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			ObjectAuditRule objectAuditRule = rule as ObjectAuditRule;
			if (objectAuditRule == null)
			{
				throw new ArgumentException("rule");
			}
			modified = true;
			base.WriteLock();
			try
			{
				switch (modification)
				{
				case AccessControlModification.Add:
					if (this.descriptor.SystemAcl == null)
					{
						this.descriptor.SystemAcl = new SystemAcl(base.IsContainer, base.IsDS, 1);
					}
					this.descriptor.SystemAcl.AddAudit(objectAuditRule.AuditFlags, ObjectSecurity.SidFromIR(objectAuditRule.IdentityReference), objectAuditRule.AccessMask, objectAuditRule.InheritanceFlags, objectAuditRule.PropagationFlags, objectAuditRule.ObjectFlags, objectAuditRule.ObjectType, objectAuditRule.InheritedObjectType);
					break;
				case AccessControlModification.Set:
					if (this.descriptor.SystemAcl == null)
					{
						this.descriptor.SystemAcl = new SystemAcl(base.IsContainer, base.IsDS, 1);
					}
					this.descriptor.SystemAcl.SetAudit(objectAuditRule.AuditFlags, ObjectSecurity.SidFromIR(objectAuditRule.IdentityReference), objectAuditRule.AccessMask, objectAuditRule.InheritanceFlags, objectAuditRule.PropagationFlags, objectAuditRule.ObjectFlags, objectAuditRule.ObjectType, objectAuditRule.InheritedObjectType);
					break;
				case AccessControlModification.Reset:
					break;
				case AccessControlModification.Remove:
					if (this.descriptor.SystemAcl == null)
					{
						modified = false;
					}
					else
					{
						modified = this.descriptor.SystemAcl.RemoveAudit(objectAuditRule.AuditFlags, ObjectSecurity.SidFromIR(objectAuditRule.IdentityReference), objectAuditRule.AccessMask, objectAuditRule.InheritanceFlags, objectAuditRule.PropagationFlags, objectAuditRule.ObjectFlags, objectAuditRule.ObjectType, objectAuditRule.InheritedObjectType);
					}
					break;
				case AccessControlModification.RemoveAll:
					this.PurgeAuditRules(objectAuditRule.IdentityReference);
					break;
				case AccessControlModification.RemoveSpecific:
					if (this.descriptor.SystemAcl != null)
					{
						this.descriptor.SystemAcl.RemoveAuditSpecific(objectAuditRule.AuditFlags, ObjectSecurity.SidFromIR(objectAuditRule.IdentityReference), objectAuditRule.AccessMask, objectAuditRule.InheritanceFlags, objectAuditRule.PropagationFlags, objectAuditRule.ObjectFlags, objectAuditRule.ObjectType, objectAuditRule.InheritedObjectType);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException("modification");
				}
				if (modified)
				{
					base.AuditRulesModified = true;
				}
			}
			finally
			{
				base.WriteUnlock();
			}
			return modified;
		}
	}
}
