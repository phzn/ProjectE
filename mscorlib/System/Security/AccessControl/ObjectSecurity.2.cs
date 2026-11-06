using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	/// <summary>Provides the ability to control access to objects without direct manipulation of Access Control Lists (ACLs); also grants the ability to type-cast access rights.</summary>
	/// <typeparam name="T">The access rights for the object.</typeparam>
	// Token: 0x02000542 RID: 1346
	public abstract class ObjectSecurity<T> : NativeObjectSecurity where T : struct
	{
		/// <summary>Initializes a new instance of the ObjectSecurity`1 class.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new <see cref="T:System.Security.AccessControl.ObjectSecurity`1" /> object is a container object.</param>
		/// <param name="resourceType">The type of resource.</param>
		// Token: 0x06003546 RID: 13638 RVA: 0x000C0E49 File Offset: 0x000BF049
		protected ObjectSecurity(bool isContainer, ResourceType resourceType)
			: base(isContainer, resourceType)
		{
		}

		/// <summary>Initializes a new instance of the ObjectSecurity`1 class.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new <see cref="T:System.Security.AccessControl.ObjectSecurity`1" /> object is a container object.</param>
		/// <param name="resourceType">The type of resource.</param>
		/// <param name="safeHandle">A handle.</param>
		/// <param name="includeSections">The sections to include.</param>
		// Token: 0x06003547 RID: 13639 RVA: 0x000C0E53 File Offset: 0x000BF053
		protected ObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle safeHandle, AccessControlSections includeSections)
			: base(isContainer, resourceType, safeHandle, includeSections)
		{
		}

		/// <summary>Initializes a new instance of the ObjectSecurity`1 class.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new <see cref="T:System.Security.AccessControl.ObjectSecurity`1" /> object is a container object.</param>
		/// <param name="resourceType">The type of resource.</param>
		/// <param name="name">The name of the securable object with which the new <see cref="T:System.Security.AccessControl.ObjectSecurity`1" /> object is associated.</param>
		/// <param name="includeSections">The sections to include.</param>
		// Token: 0x06003548 RID: 13640 RVA: 0x000C0E60 File Offset: 0x000BF060
		protected ObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections)
			: base(isContainer, resourceType, name, includeSections)
		{
		}

		/// <summary>Initializes a new instance of the ObjectSecurity`1 class.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new <see cref="T:System.Security.AccessControl.ObjectSecurity`1" /> object is a container object.</param>
		/// <param name="resourceType">The type of resource.</param>
		/// <param name="safeHandle">A handle.</param>
		/// <param name="includeSections">The sections to include.</param>
		/// <param name="exceptionFromErrorCode">A delegate implemented by integrators that provides custom exceptions.</param>
		/// <param name="exceptionContext">An object that contains contextual information about the source or destination of the exception.</param>
		// Token: 0x06003549 RID: 13641 RVA: 0x000C0E6D File Offset: 0x000BF06D
		protected ObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle safeHandle, AccessControlSections includeSections, NativeObjectSecurity.ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext)
			: base(isContainer, resourceType, safeHandle, includeSections, exceptionFromErrorCode, exceptionContext)
		{
		}

		/// <summary>Initializes a new instance of the ObjectSecurity`1 class.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new <see cref="T:System.Security.AccessControl.ObjectSecurity`1" /> object is a container object.</param>
		/// <param name="resourceType">The type of resource.</param>
		/// <param name="name">The name of the securable object with which the new <see cref="T:System.Security.AccessControl.ObjectSecurity`1" /> object is associated.</param>
		/// <param name="includeSections">The sections to include.</param>
		/// <param name="exceptionFromErrorCode">A delegate implemented by integrators that provides custom exceptions.</param>
		/// <param name="exceptionContext">An object that contains contextual information about the source or destination of the exception.</param>
		// Token: 0x0600354A RID: 13642 RVA: 0x000C0E7E File Offset: 0x000BF07E
		protected ObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections, NativeObjectSecurity.ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext)
			: base(isContainer, resourceType, name, includeSections, exceptionFromErrorCode, exceptionContext)
		{
		}

		/// <summary>Gets the Type of the securable object associated with this ObjectSecurity`1 object.</summary>
		/// <returns>The type of the securable object associated with the current instance.</returns>
		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x0600354B RID: 13643 RVA: 0x000C0E8F File Offset: 0x000BF08F
		public override Type AccessRightType
		{
			get
			{
				return typeof(T);
			}
		}

		/// <summary>Gets the Type of the object associated with the access rules of this ObjectSecurity`1 object.</summary>
		/// <returns>The Type of the object associated with the access rules of the current instance.</returns>
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x0600354C RID: 13644 RVA: 0x000C0E9B File Offset: 0x000BF09B
		public override Type AccessRuleType
		{
			get
			{
				return typeof(AccessRule<T>);
			}
		}

		/// <summary>Gets the Type object associated with the audit rules of this ObjectSecurity`1 object.</summary>
		/// <returns>The Type object associated with the audit rules of the current instance.</returns>
		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x0600354D RID: 13645 RVA: 0x000C0EA7 File Offset: 0x000BF0A7
		public override Type AuditRuleType
		{
			get
			{
				return typeof(AuditRule<T>);
			}
		}

		/// <summary>Initializes a new instance of the ObjectAccessRule class that represents a new access control rule for the associated security object.</summary>
		/// <param name="identityReference">Represents a user account.</param>
		/// <param name="accessMask">The access type.</param>
		/// <param name="isInherited">
		///   <see langword="true" /> if the access rule is inherited; otherwise, <see langword="false" />.</param>
		/// <param name="inheritanceFlags">Specifies how to propagate access masks to child objects.</param>
		/// <param name="propagationFlags">Specifies how to propagate Access Control Entries (ACEs) to child objects.</param>
		/// <param name="type">Specifies whether access is allowed or denied.</param>
		/// <returns>Represents a new access control rule for the specified user, with the specified access rights, access control, and flags.</returns>
		// Token: 0x0600354E RID: 13646 RVA: 0x000C0EB3 File Offset: 0x000BF0B3
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new AccessRule<T>(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		/// <summary>Adds the specified access rule to the Discretionary Access Control List (DACL) associated with this ObjectSecurity`1 object.</summary>
		/// <param name="rule">The rule to add.</param>
		// Token: 0x0600354F RID: 13647 RVA: 0x000BC650 File Offset: 0x000BA850
		public virtual void AddAccessRule(AccessRule<T> rule)
		{
			base.AddAccessRule(rule);
		}

		/// <summary>Removes access rules that contain the same security identifier and access mask as the specified access rule from the Discretionary Access Control List (DACL) associated with this ObjectSecurity`1 object.</summary>
		/// <param name="rule">The rule to remove.</param>
		/// <returns>
		///   <see langword="true" /> if the access rule was successfully removed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003550 RID: 13648 RVA: 0x000BC66B File Offset: 0x000BA86B
		public virtual bool RemoveAccessRule(AccessRule<T> rule)
		{
			return base.RemoveAccessRule(rule);
		}

		/// <summary>Removes all access rules that have the same security identifier as the specified access rule from the Discretionary Access Control List (DACL) associated with this ObjectSecurity`1 object.</summary>
		/// <param name="rule">The access rule to remove.</param>
		// Token: 0x06003551 RID: 13649 RVA: 0x000BC674 File Offset: 0x000BA874
		public virtual void RemoveAccessRuleAll(AccessRule<T> rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		/// <summary>Removes all access rules that exactly match the specified access rule from the Discretionary Access Control List (DACL) associated with this ObjectSecurity`1 object</summary>
		/// <param name="rule">The access rule to remove.</param>
		// Token: 0x06003552 RID: 13650 RVA: 0x000BC67D File Offset: 0x000BA87D
		public virtual void RemoveAccessRuleSpecific(AccessRule<T> rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		/// <summary>Removes all access rules in the Discretionary Access Control List (DACL) associated with this ObjectSecurity`1 object and then adds the specified access rule.</summary>
		/// <param name="rule">The access rule to reset.</param>
		// Token: 0x06003553 RID: 13651 RVA: 0x000BC662 File Offset: 0x000BA862
		public virtual void ResetAccessRule(AccessRule<T> rule)
		{
			base.ResetAccessRule(rule);
		}

		/// <summary>Removes all access rules that contain the same security identifier and qualifier as the specified access rule in the Discretionary Access Control List (DACL) associated with this ObjectSecurity`1 object and then adds the specified access rule.</summary>
		/// <param name="rule">The access rule to set.</param>
		// Token: 0x06003554 RID: 13652 RVA: 0x000BC659 File Offset: 0x000BA859
		public virtual void SetAccessRule(AccessRule<T> rule)
		{
			base.SetAccessRule(rule);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.AuditRule" /> class representing the specified audit rule for the specified user.</summary>
		/// <param name="identityReference">Represents a user account.</param>
		/// <param name="accessMask">An integer that specifies an access type.</param>
		/// <param name="isInherited">
		///   <see langword="true" /> if the access rule is inherited; otherwise, <see langword="false" />.</param>
		/// <param name="inheritanceFlags">Specifies how to propagate access masks to child objects.</param>
		/// <param name="propagationFlags">Specifies how to propagate Access Control Entries (ACEs) to child objects.</param>
		/// <param name="flags">Describes the type of auditing to perform.</param>
		/// <returns>The specified audit rule for the specified user.</returns>
		// Token: 0x06003555 RID: 13653 RVA: 0x000C0EC3 File Offset: 0x000BF0C3
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new AuditRule<T>(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		/// <summary>Adds the specified audit rule to the System Access Control List (SACL) associated with this ObjectSecurity`1 object.</summary>
		/// <param name="rule">The audit rule to add.</param>
		// Token: 0x06003556 RID: 13654 RVA: 0x000BC686 File Offset: 0x000BA886
		public virtual void AddAuditRule(AuditRule<T> rule)
		{
			base.AddAuditRule(rule);
		}

		/// <summary>Removes audit rules that contain the same security identifier and access mask as the specified audit rule from the System Access Control List (SACL) associated with this ObjectSecurity`1 object.</summary>
		/// <param name="rule">The audit rule to remove</param>
		/// <returns>
		///   <see langword="true" /> if the object was removed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003557 RID: 13655 RVA: 0x000BC698 File Offset: 0x000BA898
		public virtual bool RemoveAuditRule(AuditRule<T> rule)
		{
			return base.RemoveAuditRule(rule);
		}

		/// <summary>Removes all audit rules that have the same security identifier as the specified audit rule from the System Access Control List (SACL) associated with this ObjectSecurity`1 object.</summary>
		/// <param name="rule">The audit rule to remove.</param>
		// Token: 0x06003558 RID: 13656 RVA: 0x000BC6A1 File Offset: 0x000BA8A1
		public virtual void RemoveAuditRuleAll(AuditRule<T> rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		/// <summary>Removes all audit rules that exactly match the specified audit rule from the System Access Control List (SACL) associated with this ObjectSecurity`1 object</summary>
		/// <param name="rule">The audit rule to remove.</param>
		// Token: 0x06003559 RID: 13657 RVA: 0x000BC6AA File Offset: 0x000BA8AA
		public virtual void RemoveAuditRuleSpecific(AuditRule<T> rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		/// <summary>Removes all audit rules that contain the same security identifier and qualifier as the specified audit rule in the System Access Control List (SACL) associated with this ObjectSecurity`1 object and then adds the specified audit rule.</summary>
		/// <param name="rule">The audit rule to set.</param>
		// Token: 0x0600355A RID: 13658 RVA: 0x000BC68F File Offset: 0x000BA88F
		public virtual void SetAuditRule(AuditRule<T> rule)
		{
			base.SetAuditRule(rule);
		}

		/// <summary>Saves the security descriptor associated with this ObjectSecurity`1 object to permanent storage, using the specified handle.</summary>
		/// <param name="handle">The handle of the securable object with which this ObjectSecurity`1 object is associated.</param>
		// Token: 0x0600355B RID: 13659 RVA: 0x000C0ED4 File Offset: 0x000BF0D4
		protected void Persist(SafeHandle handle)
		{
			base.WriteLock();
			try
			{
				this.Persist(handle, base.AccessControlSectionsModified);
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		/// <summary>Saves the security descriptor associated with this ObjectSecurity`1 object to permanent storage, using the specified name.</summary>
		/// <param name="name">The name of the securable object with which this ObjectSecurity`1 object is associated.</param>
		// Token: 0x0600355C RID: 13660 RVA: 0x000C0F10 File Offset: 0x000BF110
		protected void Persist(string name)
		{
			base.WriteLock();
			try
			{
				this.Persist(name, base.AccessControlSectionsModified);
			}
			finally
			{
				base.WriteUnlock();
			}
		}
	}
}
