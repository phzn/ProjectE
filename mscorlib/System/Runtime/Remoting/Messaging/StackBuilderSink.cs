using System;
using System.Reflection;
using System.Runtime.Remoting.Proxies;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000639 RID: 1593
	internal class StackBuilderSink : IMessageSink
	{
		// Token: 0x06003C1A RID: 15386 RVA: 0x000D0ED4 File Offset: 0x000CF0D4
		public StackBuilderSink(MarshalByRefObject obj, bool forceInternalExecute)
		{
			this._target = obj;
			if (!forceInternalExecute && RemotingServices.IsTransparentProxy(obj))
			{
				this._rp = RemotingServices.GetRealProxy(obj);
			}
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x000D0EFA File Offset: 0x000CF0FA
		public IMessage SyncProcessMessage(IMessage msg)
		{
			this.CheckParameters(msg);
			if (this._rp != null)
			{
				return this._rp.Invoke(msg);
			}
			return RemotingServices.InternalExecuteMessage(this._target, (IMethodCallMessage)msg);
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x000D0F2C File Offset: 0x000CF12C
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			object[] array = new object[] { msg, replySink };
			ThreadPool.QueueUserWorkItem(delegate(object data)
			{
				try
				{
					this.ExecuteAsyncMessage(data);
				}
				catch
				{
				}
			}, array);
			return null;
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x000D0F5C File Offset: 0x000CF15C
		private void ExecuteAsyncMessage(object ob)
		{
			object[] array = (object[])ob;
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)array[0];
			IMessageSink messageSink = (IMessageSink)array[1];
			this.CheckParameters(methodCallMessage);
			IMessage message;
			if (this._rp != null)
			{
				message = this._rp.Invoke(methodCallMessage);
			}
			else
			{
				message = RemotingServices.InternalExecuteMessage(this._target, methodCallMessage);
			}
			messageSink.SyncProcessMessage(message);
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003C1E RID: 15390 RVA: 0x0000AF5E File Offset: 0x0000915E
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x000D0FB4 File Offset: 0x000CF1B4
		private void CheckParameters(IMessage msg)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			ParameterInfo[] parameters = methodCallMessage.MethodBase.GetParameters();
			int num = 0;
			foreach (ParameterInfo parameterInfo in parameters)
			{
				object arg = methodCallMessage.GetArg(num++);
				Type type = parameterInfo.ParameterType;
				if (type.IsByRef)
				{
					type = type.GetElementType();
				}
				if (arg != null && !type.IsInstanceOfType(arg))
				{
					throw new RemotingException(string.Concat(new string[]
					{
						"Cannot cast argument ",
						parameterInfo.Position.ToString(),
						" of type '",
						arg.GetType().AssemblyQualifiedName,
						"' to type '",
						type.AssemblyQualifiedName,
						"'"
					}));
				}
			}
		}

		// Token: 0x040026EC RID: 9964
		private MarshalByRefObject _target;

		// Token: 0x040026ED RID: 9965
		private RealProxy _rp;
	}
}
