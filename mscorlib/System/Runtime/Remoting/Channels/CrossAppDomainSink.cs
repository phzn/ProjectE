using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005AE RID: 1454
	[MonoTODO("Handle domain unloading?")]
	internal class CrossAppDomainSink : IMessageSink
	{
		// Token: 0x0600385E RID: 14430 RVA: 0x000CA3C7 File Offset: 0x000C85C7
		internal CrossAppDomainSink(int domainID)
		{
			this._domainID = domainID;
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x000CA3D8 File Offset: 0x000C85D8
		internal static CrossAppDomainSink GetSink(int domainID)
		{
			object syncRoot = CrossAppDomainSink.s_sinks.SyncRoot;
			CrossAppDomainSink crossAppDomainSink;
			lock (syncRoot)
			{
				if (CrossAppDomainSink.s_sinks.ContainsKey(domainID))
				{
					crossAppDomainSink = (CrossAppDomainSink)CrossAppDomainSink.s_sinks[domainID];
				}
				else
				{
					CrossAppDomainSink crossAppDomainSink2 = new CrossAppDomainSink(domainID);
					CrossAppDomainSink.s_sinks[domainID] = crossAppDomainSink2;
					crossAppDomainSink = crossAppDomainSink2;
				}
			}
			return crossAppDomainSink;
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06003860 RID: 14432 RVA: 0x000CA45C File Offset: 0x000C865C
		internal int TargetDomainId
		{
			get
			{
				return this._domainID;
			}
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x000CA464 File Offset: 0x000C8664
		private static CrossAppDomainSink.ProcessMessageRes ProcessMessageInDomain(byte[] arrRequest, CADMethodCallMessage cadMsg)
		{
			CrossAppDomainSink.ProcessMessageRes processMessageRes = default(CrossAppDomainSink.ProcessMessageRes);
			try
			{
				AppDomain.CurrentDomain.ProcessMessageInDomain(arrRequest, cadMsg, out processMessageRes.arrResponse, out processMessageRes.cadMrm);
			}
			catch (Exception ex)
			{
				IMessage message = new MethodResponse(ex, new ErrorMessage());
				processMessageRes.arrResponse = CADSerializer.SerializeMessage(message).GetBuffer();
			}
			return processMessageRes;
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x000CA4C8 File Offset: 0x000C86C8
		public virtual IMessage SyncProcessMessage(IMessage msgRequest)
		{
			IMessage message = null;
			try
			{
				byte[] array = null;
				byte[] array2 = null;
				CADMethodReturnMessage cadmethodReturnMessage = null;
				CADMethodCallMessage cadmethodCallMessage = CADMethodCallMessage.Create(msgRequest);
				if (cadmethodCallMessage == null)
				{
					array2 = CADSerializer.SerializeMessage(msgRequest).GetBuffer();
				}
				Context currentContext = Thread.CurrentContext;
				try
				{
					CrossAppDomainSink.ProcessMessageRes processMessageRes = (CrossAppDomainSink.ProcessMessageRes)AppDomain.InvokeInDomainByID(this._domainID, CrossAppDomainSink.processMessageMethod, null, new object[] { array2, cadmethodCallMessage });
					array = processMessageRes.arrResponse;
					cadmethodReturnMessage = processMessageRes.cadMrm;
				}
				finally
				{
					AppDomain.InternalSetContext(currentContext);
				}
				if (array != null)
				{
					message = CADSerializer.DeserializeMessage(new MemoryStream(array), msgRequest as IMethodCallMessage);
				}
				else
				{
					message = new MethodResponse(msgRequest as IMethodCallMessage, cadmethodReturnMessage);
				}
			}
			catch (Exception ex)
			{
				try
				{
					message = new ReturnMessage(ex, msgRequest as IMethodCallMessage);
				}
				catch (Exception)
				{
				}
			}
			return message;
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x000CA5A0 File Offset: 0x000C87A0
		public virtual IMessageCtrl AsyncProcessMessage(IMessage reqMsg, IMessageSink replySink)
		{
			AsyncRequest asyncRequest = new AsyncRequest(reqMsg, replySink);
			ThreadPool.QueueUserWorkItem(delegate(object data)
			{
				try
				{
					this.SendAsyncMessage(data);
				}
				catch
				{
				}
			}, asyncRequest);
			return null;
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x000CA5CC File Offset: 0x000C87CC
		public void SendAsyncMessage(object data)
		{
			AsyncRequest asyncRequest = (AsyncRequest)data;
			IMessage message = this.SyncProcessMessage(asyncRequest.MsgRequest);
			asyncRequest.ReplySink.SyncProcessMessage(message);
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06003865 RID: 14437 RVA: 0x0000AF5E File Offset: 0x0000915E
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040025E2 RID: 9698
		private static Hashtable s_sinks = new Hashtable();

		// Token: 0x040025E3 RID: 9699
		private static MethodInfo processMessageMethod = typeof(CrossAppDomainSink).GetMethod("ProcessMessageInDomain", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x040025E4 RID: 9700
		private int _domainID;

		// Token: 0x020005AF RID: 1455
		private struct ProcessMessageRes
		{
			// Token: 0x040025E5 RID: 9701
			public byte[] arrResponse;

			// Token: 0x040025E6 RID: 9702
			public CADMethodReturnMessage cadMrm;
		}
	}
}
