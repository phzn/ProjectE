using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System.Runtime.Remoting.Messaging
{
	/// <summary>Selects the remoting surrogate that can be used to serialize an object that derives from a <see cref="T:System.MarshalByRefObject" />.</summary>
	// Token: 0x02000634 RID: 1588
	[ComVisible(true)]
	public class RemotingSurrogateSelector : ISurrogateSelector
	{
		/// <summary>Gets or sets the <see cref="T:System.Runtime.Remoting.Messaging.MessageSurrogateFilter" /> delegate for the current instance of the <see cref="T:System.Runtime.Remoting.Messaging.RemotingSurrogateSelector" />.</summary>
		/// <returns>The <see cref="T:System.Runtime.Remoting.Messaging.MessageSurrogateFilter" /> delegate for the current instance of the <see cref="T:System.Runtime.Remoting.Messaging.RemotingSurrogateSelector" />.</returns>
		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003BEA RID: 15338 RVA: 0x000D09D5 File Offset: 0x000CEBD5
		// (set) Token: 0x06003BEB RID: 15339 RVA: 0x000D09DD File Offset: 0x000CEBDD
		public MessageSurrogateFilter Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				this._filter = value;
			}
		}

		/// <summary>Adds the specified <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> to the surrogate selector chain.</summary>
		/// <param name="selector">The next <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> to examine.</param>
		// Token: 0x06003BEC RID: 15340 RVA: 0x000D09E6 File Offset: 0x000CEBE6
		[SecurityCritical]
		public virtual void ChainSelector(ISurrogateSelector selector)
		{
			if (this._next != null)
			{
				selector.ChainSelector(this._next);
			}
			this._next = selector;
		}

		/// <summary>Returns the next <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> in the chain of surrogate selectors.</summary>
		/// <returns>The next <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> in the chain of surrogate selectors.</returns>
		// Token: 0x06003BED RID: 15341 RVA: 0x000D0A03 File Offset: 0x000CEC03
		[SecurityCritical]
		public virtual ISurrogateSelector GetNextSelector()
		{
			return this._next;
		}

		/// <summary>Returns the object at the root of the object graph.</summary>
		/// <returns>The object at the root of the object graph.</returns>
		// Token: 0x06003BEE RID: 15342 RVA: 0x000D0A0B File Offset: 0x000CEC0B
		public object GetRootObject()
		{
			return this._rootObj;
		}

		/// <summary>Returns the appropriate surrogate for the given type in the given context.</summary>
		/// <param name="type">The <see cref="T:System.Type" /> for which the surrogate is requested.</param>
		/// <param name="context">The source or destination of serialization.</param>
		/// <param name="ssout">When this method returns, contains an <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> that is appropriate for the specified object type. This parameter is passed uninitialized.</param>
		/// <returns>The appropriate surrogate for the given type in the given context.</returns>
		// Token: 0x06003BEF RID: 15343 RVA: 0x000D0A14 File Offset: 0x000CEC14
		[SecurityCritical]
		public virtual ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector ssout)
		{
			if (type.IsMarshalByRef)
			{
				ssout = this;
				return RemotingSurrogateSelector._objRemotingSurrogate;
			}
			if (RemotingSurrogateSelector.s_cachedTypeObjRef.IsAssignableFrom(type))
			{
				ssout = this;
				return RemotingSurrogateSelector._objRefSurrogate;
			}
			if (this._next != null)
			{
				return this._next.GetSurrogate(type, context, out ssout);
			}
			ssout = null;
			return null;
		}

		/// <summary>Sets the object at the root of the object graph.</summary>
		/// <param name="obj">The object at the root of the object graph.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="obj" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003BF0 RID: 15344 RVA: 0x000D0A63 File Offset: 0x000CEC63
		public void SetRootObject(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException();
			}
			this._rootObj = obj;
		}

		/// <summary>Sets up the current surrogate selector to use the SOAP format.</summary>
		// Token: 0x06003BF1 RID: 15345 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public virtual void UseSoapFormat()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040026D6 RID: 9942
		private static Type s_cachedTypeObjRef = typeof(ObjRef);

		// Token: 0x040026D7 RID: 9943
		private static ObjRefSurrogate _objRefSurrogate = new ObjRefSurrogate();

		// Token: 0x040026D8 RID: 9944
		private static RemotingSurrogate _objRemotingSurrogate = new RemotingSurrogate();

		// Token: 0x040026D9 RID: 9945
		private object _rootObj;

		// Token: 0x040026DA RID: 9946
		private MessageSurrogateFilter _filter;

		// Token: 0x040026DB RID: 9947
		private ISurrogateSelector _next;
	}
}
