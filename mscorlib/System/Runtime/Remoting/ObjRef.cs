using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization;
using System.Security;

namespace System.Runtime.Remoting
{
	/// <summary>Stores all relevant information required to generate a proxy in order to communicate with a remote object.</summary>
	// Token: 0x02000563 RID: 1379
	[ComVisible(true)]
	[Serializable]
	public class ObjRef : IObjectReference, ISerializable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class with default values.</summary>
		// Token: 0x06003605 RID: 13829 RVA: 0x000C2665 File Offset: 0x000C0865
		public ObjRef()
		{
			this.UpdateChannelInfo();
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x000C2673 File Offset: 0x000C0873
		internal ObjRef(string uri, IChannelInfo cinfo)
		{
			this.uri = uri;
			this.channel_info = cinfo;
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x000C268C File Offset: 0x000C088C
		internal ObjRef DeserializeInTheCurrentDomain(int domainId, byte[] tInfo)
		{
			string text = string.Copy(this.uri);
			ChannelInfo channelInfo = new ChannelInfo(new CrossAppDomainData(domainId));
			ObjRef objRef = new ObjRef(text, channelInfo);
			IRemotingTypeInfo remotingTypeInfo = (IRemotingTypeInfo)CADSerializer.DeserializeObjectSafe(tInfo);
			objRef.typeInfo = remotingTypeInfo;
			return objRef;
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x000C26C9 File Offset: 0x000C08C9
		internal byte[] SerializeType()
		{
			if (this.typeInfo == null)
			{
				throw new Exception("Attempt to serialize a null TypeInfo.");
			}
			return CADSerializer.SerializeObject(this.typeInfo).GetBuffer();
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x000C26F0 File Offset: 0x000C08F0
		internal ObjRef(ObjRef o, bool unmarshalAsProxy)
		{
			this.channel_info = o.channel_info;
			this.uri = o.uri;
			this.typeInfo = o.typeInfo;
			this.envoyInfo = o.envoyInfo;
			this.flags = o.flags;
			if (unmarshalAsProxy)
			{
				this.flags |= ObjRef.MarshalledObjectRef;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class to reference a specified <see cref="T:System.MarshalByRefObject" /> of a specified <see cref="T:System.Type" />.</summary>
		/// <param name="o">The object that the new <see cref="T:System.Runtime.Remoting.ObjRef" /> instance will reference.</param>
		/// <param name="requestedType">The <see cref="T:System.Type" /> of the object that the new <see cref="T:System.Runtime.Remoting.ObjRef" /> instance will reference.</param>
		// Token: 0x0600360A RID: 13834 RVA: 0x000C2754 File Offset: 0x000C0954
		public ObjRef(MarshalByRefObject o, Type requestedType)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			if (requestedType == null)
			{
				throw new ArgumentNullException("requestedType");
			}
			this.uri = RemotingServices.GetObjectUri(o);
			this.typeInfo = new TypeInfo(requestedType);
			if (!requestedType.IsInstanceOfType(o))
			{
				throw new RemotingException("The server object type cannot be cast to the requested type " + requestedType.FullName);
			}
			this.UpdateChannelInfo();
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x000C27C6 File Offset: 0x000C09C6
		internal ObjRef(Type type, string url, object remoteChannelData)
		{
			this.uri = url;
			this.typeInfo = new TypeInfo(type);
			if (remoteChannelData != null)
			{
				this.channel_info = new ChannelInfo(remoteChannelData);
			}
			this.flags |= ObjRef.WellKnowObjectRef;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.ObjRef" /> class from serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination of the exception.</param>
		// Token: 0x0600360C RID: 13836 RVA: 0x000C2804 File Offset: 0x000C0A04
		protected ObjRef(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			bool flag = true;
			while (enumerator.MoveNext())
			{
				string name = enumerator.Name;
				if (!(name == "uri"))
				{
					if (!(name == "typeInfo"))
					{
						if (!(name == "channelInfo"))
						{
							if (!(name == "envoyInfo"))
							{
								if (!(name == "fIsMarshalled"))
								{
									if (!(name == "objrefFlags"))
									{
										throw new NotSupportedException();
									}
									this.flags = Convert.ToInt32(enumerator.Value);
								}
								else
								{
									object value = enumerator.Value;
									int num;
									if (value is string)
									{
										num = ((IConvertible)value).ToInt32(null);
									}
									else
									{
										num = (int)value;
									}
									if (num == 0)
									{
										flag = false;
									}
								}
							}
							else
							{
								this.envoyInfo = (IEnvoyInfo)enumerator.Value;
							}
						}
						else
						{
							this.channel_info = (IChannelInfo)enumerator.Value;
						}
					}
					else
					{
						this.typeInfo = (IRemotingTypeInfo)enumerator.Value;
					}
				}
				else
				{
					this.uri = (string)enumerator.Value;
				}
			}
			if (flag)
			{
				this.flags |= ObjRef.MarshalledObjectRef;
			}
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal bool IsPossibleToCAD()
		{
			return false;
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x0600360E RID: 13838 RVA: 0x000C2937 File Offset: 0x000C0B37
		internal bool IsReferenceToWellKnow
		{
			get
			{
				return (this.flags & ObjRef.WellKnowObjectRef) > 0;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Runtime.Remoting.IChannelInfo" /> for the <see cref="T:System.Runtime.Remoting.ObjRef" />.</summary>
		/// <returns>The <see cref="T:System.Runtime.Remoting.IChannelInfo" /> interface for the <see cref="T:System.Runtime.Remoting.ObjRef" />.</returns>
		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x0600360F RID: 13839 RVA: 0x000C2948 File Offset: 0x000C0B48
		// (set) Token: 0x06003610 RID: 13840 RVA: 0x000C2950 File Offset: 0x000C0B50
		public virtual IChannelInfo ChannelInfo
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.channel_info;
			}
			set
			{
				this.channel_info = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Runtime.Remoting.IEnvoyInfo" /> for the <see cref="T:System.Runtime.Remoting.ObjRef" />.</summary>
		/// <returns>The <see cref="T:System.Runtime.Remoting.IEnvoyInfo" /> interface for the <see cref="T:System.Runtime.Remoting.ObjRef" />.</returns>
		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06003611 RID: 13841 RVA: 0x000C2959 File Offset: 0x000C0B59
		// (set) Token: 0x06003612 RID: 13842 RVA: 0x000C2961 File Offset: 0x000C0B61
		public virtual IEnvoyInfo EnvoyInfo
		{
			get
			{
				return this.envoyInfo;
			}
			set
			{
				this.envoyInfo = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Runtime.Remoting.IRemotingTypeInfo" /> for the object that the <see cref="T:System.Runtime.Remoting.ObjRef" /> describes.</summary>
		/// <returns>The <see cref="T:System.Runtime.Remoting.IRemotingTypeInfo" /> for the object that the <see cref="T:System.Runtime.Remoting.ObjRef" /> describes.</returns>
		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06003613 RID: 13843 RVA: 0x000C296A File Offset: 0x000C0B6A
		// (set) Token: 0x06003614 RID: 13844 RVA: 0x000C2972 File Offset: 0x000C0B72
		public virtual IRemotingTypeInfo TypeInfo
		{
			get
			{
				return this.typeInfo;
			}
			set
			{
				this.typeInfo = value;
			}
		}

		/// <summary>Gets or sets the URI of the specific object instance.</summary>
		/// <returns>The URI of the specific object instance.</returns>
		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06003615 RID: 13845 RVA: 0x000C297B File Offset: 0x000C0B7B
		// (set) Token: 0x06003616 RID: 13846 RVA: 0x000C2983 File Offset: 0x000C0B83
		public virtual string URI
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = value;
			}
		}

		/// <summary>Populates a specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the current <see cref="T:System.Runtime.Remoting.ObjRef" /> instance.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The contextual information about the source or destination of the serialization.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have serialization formatter permission.</exception>
		// Token: 0x06003617 RID: 13847 RVA: 0x000C298C File Offset: 0x000C0B8C
		[SecurityCritical]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.SetType(base.GetType());
			info.AddValue("uri", this.uri);
			info.AddValue("typeInfo", this.typeInfo, typeof(IRemotingTypeInfo));
			info.AddValue("envoyInfo", this.envoyInfo, typeof(IEnvoyInfo));
			info.AddValue("channelInfo", this.channel_info, typeof(IChannelInfo));
			info.AddValue("objrefFlags", this.flags);
		}

		/// <summary>Returns a reference to the remote object that the <see cref="T:System.Runtime.Remoting.ObjRef" /> describes.</summary>
		/// <param name="context">The context where the current object resides.</param>
		/// <returns>A reference to the remote object that the <see cref="T:System.Runtime.Remoting.ObjRef" /> describes.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have serialization formatter permission.</exception>
		// Token: 0x06003618 RID: 13848 RVA: 0x000C2A18 File Offset: 0x000C0C18
		[SecurityCritical]
		public virtual object GetRealObject(StreamingContext context)
		{
			if ((this.flags & ObjRef.MarshalledObjectRef) > 0)
			{
				return RemotingServices.Unmarshal(this);
			}
			return this;
		}

		/// <summary>Returns a Boolean value that indicates whether the current <see cref="T:System.Runtime.Remoting.ObjRef" /> instance references an object located in the current <see cref="T:System.AppDomain" />.</summary>
		/// <returns>A Boolean value that indicates whether the current <see cref="T:System.Runtime.Remoting.ObjRef" /> instance references an object located in the current <see cref="T:System.AppDomain" />.</returns>
		// Token: 0x06003619 RID: 13849 RVA: 0x000C2A34 File Offset: 0x000C0C34
		public bool IsFromThisAppDomain()
		{
			Identity identityForUri = RemotingServices.GetIdentityForUri(this.uri);
			return identityForUri != null && identityForUri.IsFromThisAppDomain;
		}

		/// <summary>Returns a Boolean value that indicates whether the current <see cref="T:System.Runtime.Remoting.ObjRef" /> instance references an object located in the current process.</summary>
		/// <returns>A Boolean value that indicates whether the current <see cref="T:System.Runtime.Remoting.ObjRef" /> instance references an object located in the current process.</returns>
		// Token: 0x0600361A RID: 13850 RVA: 0x000C2A58 File Offset: 0x000C0C58
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool IsFromThisProcess()
		{
			foreach (object obj in this.channel_info.ChannelData)
			{
				if (obj is CrossAppDomainData)
				{
					return ((CrossAppDomainData)obj).ProcessID == RemotingConfiguration.ProcessId;
				}
			}
			return true;
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x000C2AA2 File Offset: 0x000C0CA2
		internal void UpdateChannelInfo()
		{
			this.channel_info = new ChannelInfo();
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x0600361C RID: 13852 RVA: 0x000C2AAF File Offset: 0x000C0CAF
		internal Type ServerType
		{
			get
			{
				if (this._serverType == null)
				{
					this._serverType = Type.GetType(this.typeInfo.TypeName);
				}
				return this._serverType;
			}
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x00004BF9 File Offset: 0x00002DF9
		internal void SetDomainID(int id)
		{
		}

		// Token: 0x04002524 RID: 9508
		private IChannelInfo channel_info;

		// Token: 0x04002525 RID: 9509
		private string uri;

		// Token: 0x04002526 RID: 9510
		private IRemotingTypeInfo typeInfo;

		// Token: 0x04002527 RID: 9511
		private IEnvoyInfo envoyInfo;

		// Token: 0x04002528 RID: 9512
		private int flags;

		// Token: 0x04002529 RID: 9513
		private Type _serverType;

		// Token: 0x0400252A RID: 9514
		private static int MarshalledObjectRef = 1;

		// Token: 0x0400252B RID: 9515
		private static int WellKnowObjectRef = 2;
	}
}
