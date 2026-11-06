using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Mono;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x0200057F RID: 1407
	[StructLayout(LayoutKind.Sequential)]
	internal class TransparentProxy
	{
		// Token: 0x0600371B RID: 14107 RVA: 0x000C6D90 File Offset: 0x000C4F90
		internal RuntimeType GetProxyType()
		{
			return (RuntimeType)Type.GetTypeFromHandle(this._class.ProxyClass.GetTypeHandle());
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x0600371C RID: 14108 RVA: 0x000C6DBA File Offset: 0x000C4FBA
		private bool IsContextBoundObject
		{
			get
			{
				return this.GetProxyType().IsContextful;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x0600371D RID: 14109 RVA: 0x000C6DC7 File Offset: 0x000C4FC7
		private Context TargetContext
		{
			get
			{
				return this._rp._targetContext;
			}
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000C6DD4 File Offset: 0x000C4FD4
		private bool InCurrentContext()
		{
			return this.IsContextBoundObject && this.TargetContext == Thread.CurrentContext;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000C6DF0 File Offset: 0x000C4FF0
		internal object LoadRemoteFieldNew(IntPtr classPtr, IntPtr fieldPtr)
		{
			RuntimeClassHandle runtimeClassHandle = new RuntimeClassHandle(classPtr);
			RuntimeFieldHandle runtimeFieldHandle = new RuntimeFieldHandle(fieldPtr);
			RuntimeTypeHandle typeHandle = runtimeClassHandle.GetTypeHandle();
			FieldInfo fieldFromHandle = FieldInfo.GetFieldFromHandle(runtimeFieldHandle);
			if (this.InCurrentContext())
			{
				object server = this._rp._server;
				return fieldFromHandle.GetValue(server);
			}
			string fullName = Type.GetTypeFromHandle(typeHandle).FullName;
			string name = fieldFromHandle.Name;
			object[] array = new object[] { fullName, name };
			object[] array2 = new object[1];
			MethodInfo method = typeof(object).GetMethod("FieldGetter", BindingFlags.Instance | BindingFlags.NonPublic);
			if (method == null)
			{
				throw new MissingMethodException("System.Object", "FieldGetter");
			}
			MonoMethodMessage monoMethodMessage = new MonoMethodMessage(method, array, array2);
			Exception ex;
			object[] array3;
			RealProxy.PrivateInvoke(this._rp, monoMethodMessage, out ex, out array3);
			if (ex != null)
			{
				throw ex;
			}
			return array3[0];
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x000C6EBC File Offset: 0x000C50BC
		internal void StoreRemoteField(IntPtr classPtr, IntPtr fieldPtr, object arg)
		{
			RuntimeClassHandle runtimeClassHandle = new RuntimeClassHandle(classPtr);
			RuntimeFieldHandle runtimeFieldHandle = new RuntimeFieldHandle(fieldPtr);
			RuntimeTypeHandle typeHandle = runtimeClassHandle.GetTypeHandle();
			FieldInfo fieldFromHandle = FieldInfo.GetFieldFromHandle(runtimeFieldHandle);
			if (this.InCurrentContext())
			{
				object server = this._rp._server;
				fieldFromHandle.SetValue(server, arg);
				return;
			}
			string fullName = Type.GetTypeFromHandle(typeHandle).FullName;
			string name = fieldFromHandle.Name;
			object[] array = new object[] { fullName, name, arg };
			MethodInfo method = typeof(object).GetMethod("FieldSetter", BindingFlags.Instance | BindingFlags.NonPublic);
			if (method == null)
			{
				throw new MissingMethodException("System.Object", "FieldSetter");
			}
			MonoMethodMessage monoMethodMessage = new MonoMethodMessage(method, array, null);
			Exception ex;
			object[] array2;
			RealProxy.PrivateInvoke(this._rp, monoMethodMessage, out ex, out array2);
			if (ex != null)
			{
				throw ex;
			}
		}

		// Token: 0x04002577 RID: 9591
		public RealProxy _rp;

		// Token: 0x04002578 RID: 9592
		private RuntimeRemoteClassHandle _class;

		// Token: 0x04002579 RID: 9593
		private bool _custom_type_info;
	}
}
