using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Runtime.Remoting.Messaging
{
	/// <summary>Implements the <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage" /> interface to create a request message that acts as a method call on a remote object.</summary>
	// Token: 0x02000627 RID: 1575
	[ComVisible(true)]
	public class MethodCallMessageWrapper : InternalMessageWrapper, IMethodCallMessage, IMethodMessage, IMessage
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Messaging.MethodCallMessageWrapper" /> class by wrapping an <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage" /> interface.</summary>
		/// <param name="msg">A message that acts as an outgoing method call on a remote object.</param>
		// Token: 0x06003B44 RID: 15172 RVA: 0x000CEE35 File Offset: 0x000CD035
		public MethodCallMessageWrapper(IMethodCallMessage msg)
			: base(msg)
		{
			this._args = ((IMethodCallMessage)this.WrappedMessage).Args;
			this._inArgInfo = new ArgInfo(msg.MethodBase, ArgInfoType.In);
		}

		/// <summary>Gets the number of arguments passed to the method.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that represents the number of arguments passed to a method.</returns>
		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06003B45 RID: 15173 RVA: 0x000CEE66 File Offset: 0x000CD066
		public virtual int ArgCount
		{
			[SecurityCritical]
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).ArgCount;
			}
		}

		/// <summary>Gets an array of arguments passed to the method.</summary>
		/// <returns>An array of type <see cref="T:System.Object" /> that represents the arguments passed to a method.</returns>
		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06003B46 RID: 15174 RVA: 0x000CEE78 File Offset: 0x000CD078
		// (set) Token: 0x06003B47 RID: 15175 RVA: 0x000CEE80 File Offset: 0x000CD080
		public virtual object[] Args
		{
			[SecurityCritical]
			get
			{
				return this._args;
			}
			set
			{
				this._args = value;
			}
		}

		/// <summary>Gets a value indicating whether the method can accept a variable number of arguments.</summary>
		/// <returns>
		///   <see langword="true" /> if the method can accept a variable number of arguments; otherwise, <see langword="false" />.</returns>
		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06003B48 RID: 15176 RVA: 0x000CEE89 File Offset: 0x000CD089
		public virtual bool HasVarArgs
		{
			[SecurityCritical]
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).HasVarArgs;
			}
		}

		/// <summary>Gets the number of arguments in the method call that are not marked as <see langword="out" /> parameters.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that represents the number of arguments in the method call that are not marked as <see langword="out" /> parameters.</returns>
		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06003B49 RID: 15177 RVA: 0x000CEE9B File Offset: 0x000CD09B
		public virtual int InArgCount
		{
			[SecurityCritical]
			get
			{
				return this._inArgInfo.GetInOutArgCount();
			}
		}

		/// <summary>Gets an array of arguments in the method call that are not marked as <see langword="out" /> parameters.</summary>
		/// <returns>An array of type <see cref="T:System.Object" /> that represents arguments in the method call that are not marked as <see langword="out" /> parameters.</returns>
		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06003B4A RID: 15178 RVA: 0x000CEEA8 File Offset: 0x000CD0A8
		public virtual object[] InArgs
		{
			[SecurityCritical]
			get
			{
				return this._inArgInfo.GetInOutArgs(this._args);
			}
		}

		/// <summary>Gets the <see cref="T:System.Runtime.Remoting.Messaging.LogicalCallContext" /> for the current method call.</summary>
		/// <returns>The <see cref="T:System.Runtime.Remoting.Messaging.LogicalCallContext" /> for the current method call.</returns>
		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06003B4B RID: 15179 RVA: 0x000CEEBB File Offset: 0x000CD0BB
		public virtual LogicalCallContext LogicalCallContext
		{
			[SecurityCritical]
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).LogicalCallContext;
			}
		}

		/// <summary>Gets the <see cref="T:System.Reflection.MethodBase" /> of the called method.</summary>
		/// <returns>The <see cref="T:System.Reflection.MethodBase" /> of the called method.</returns>
		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06003B4C RID: 15180 RVA: 0x000CEECD File Offset: 0x000CD0CD
		public virtual MethodBase MethodBase
		{
			[SecurityCritical]
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).MethodBase;
			}
		}

		/// <summary>Gets the name of the invoked method.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the name of the invoked method.</returns>
		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06003B4D RID: 15181 RVA: 0x000CEEDF File Offset: 0x000CD0DF
		public virtual string MethodName
		{
			[SecurityCritical]
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).MethodName;
			}
		}

		/// <summary>Gets an object that contains the method signature.</summary>
		/// <returns>A <see cref="T:System.Object" /> that contains the method signature.</returns>
		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06003B4E RID: 15182 RVA: 0x000CEEF1 File Offset: 0x000CD0F1
		public virtual object MethodSignature
		{
			[SecurityCritical]
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).MethodSignature;
			}
		}

		/// <summary>An <see cref="T:System.Collections.IDictionary" /> that represents a collection of the remoting message's properties.</summary>
		/// <returns>An <see cref="T:System.Collections.IDictionary" /> interface that represents a collection of the remoting message's properties.</returns>
		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06003B4F RID: 15183 RVA: 0x000CEF03 File Offset: 0x000CD103
		public virtual IDictionary Properties
		{
			[SecurityCritical]
			get
			{
				if (this._properties == null)
				{
					this._properties = new MethodCallMessageWrapper.DictionaryWrapper(this, this.WrappedMessage.Properties);
				}
				return this._properties;
			}
		}

		/// <summary>Gets the full type name of the remote object on which the method call is being made.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the full type name of the remote object on which the method call is being made.</returns>
		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06003B50 RID: 15184 RVA: 0x000CEF2A File Offset: 0x000CD12A
		public virtual string TypeName
		{
			[SecurityCritical]
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).TypeName;
			}
		}

		/// <summary>Gets the Uniform Resource Identifier (URI) of the remote object on which the method call is being made.</summary>
		/// <returns>The URI of a remote object.</returns>
		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06003B51 RID: 15185 RVA: 0x000CEF3C File Offset: 0x000CD13C
		// (set) Token: 0x06003B52 RID: 15186 RVA: 0x000CEF50 File Offset: 0x000CD150
		public virtual string Uri
		{
			[SecurityCritical]
			get
			{
				return ((IMethodCallMessage)this.WrappedMessage).Uri;
			}
			set
			{
				IInternalMessage internalMessage = this.WrappedMessage as IInternalMessage;
				if (internalMessage != null)
				{
					internalMessage.Uri = value;
					return;
				}
				this.Properties["__Uri"] = value;
			}
		}

		/// <summary>Gets a method argument, as an object, at a specified index.</summary>
		/// <param name="argNum">The index of the requested argument.</param>
		/// <returns>The method argument as an object.</returns>
		// Token: 0x06003B53 RID: 15187 RVA: 0x000CEF85 File Offset: 0x000CD185
		[SecurityCritical]
		public virtual object GetArg(int argNum)
		{
			return this._args[argNum];
		}

		/// <summary>Gets the name of a method argument at a specified index.</summary>
		/// <param name="index">The index of the requested argument.</param>
		/// <returns>The name of the method argument.</returns>
		// Token: 0x06003B54 RID: 15188 RVA: 0x000CEF8F File Offset: 0x000CD18F
		[SecurityCritical]
		public virtual string GetArgName(int index)
		{
			return ((IMethodCallMessage)this.WrappedMessage).GetArgName(index);
		}

		/// <summary>Gets a method argument at a specified index that is not marked as an <see langword="out" /> parameter.</summary>
		/// <param name="argNum">The index of the requested argument.</param>
		/// <returns>The method argument that is not marked as an <see langword="out" /> parameter.</returns>
		// Token: 0x06003B55 RID: 15189 RVA: 0x000CEFA2 File Offset: 0x000CD1A2
		[SecurityCritical]
		public virtual object GetInArg(int argNum)
		{
			return this._args[this._inArgInfo.GetInOutArgIndex(argNum)];
		}

		/// <summary>Gets the name of a method argument at a specified index that is not marked as an out parameter.</summary>
		/// <param name="index">The index of the requested argument.</param>
		/// <returns>The name of the method argument that is not marked as an out parameter.</returns>
		// Token: 0x06003B56 RID: 15190 RVA: 0x000CEFB7 File Offset: 0x000CD1B7
		[SecurityCritical]
		public virtual string GetInArgName(int index)
		{
			return this._inArgInfo.GetInOutArgName(index);
		}

		// Token: 0x040026A0 RID: 9888
		private object[] _args;

		// Token: 0x040026A1 RID: 9889
		private ArgInfo _inArgInfo;

		// Token: 0x040026A2 RID: 9890
		private MethodCallMessageWrapper.DictionaryWrapper _properties;

		// Token: 0x02000628 RID: 1576
		private class DictionaryWrapper : MCMDictionary
		{
			// Token: 0x06003B57 RID: 15191 RVA: 0x000CEFC5 File Offset: 0x000CD1C5
			public DictionaryWrapper(IMethodMessage message, IDictionary wrappedDictionary)
				: base(message)
			{
				this._wrappedDictionary = wrappedDictionary;
				base.MethodKeys = MethodCallMessageWrapper.DictionaryWrapper._keys;
			}

			// Token: 0x06003B58 RID: 15192 RVA: 0x000CEFE0 File Offset: 0x000CD1E0
			protected override IDictionary AllocInternalProperties()
			{
				return this._wrappedDictionary;
			}

			// Token: 0x06003B59 RID: 15193 RVA: 0x000CEFE8 File Offset: 0x000CD1E8
			protected override void SetMethodProperty(string key, object value)
			{
				if (key == "__Args")
				{
					((MethodCallMessageWrapper)this._message)._args = (object[])value;
					return;
				}
				base.SetMethodProperty(key, value);
			}

			// Token: 0x06003B5A RID: 15194 RVA: 0x000CF016 File Offset: 0x000CD216
			protected override object GetMethodProperty(string key)
			{
				if (key == "__Args")
				{
					return ((MethodCallMessageWrapper)this._message)._args;
				}
				return base.GetMethodProperty(key);
			}

			// Token: 0x040026A3 RID: 9891
			private IDictionary _wrappedDictionary;

			// Token: 0x040026A4 RID: 9892
			private static string[] _keys = new string[] { "__Args" };
		}
	}
}
