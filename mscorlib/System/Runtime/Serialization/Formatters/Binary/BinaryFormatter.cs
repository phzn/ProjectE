using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary
{
	/// <summary>Serializes and deserializes an object, or an entire graph of connected objects, in binary format.</summary>
	// Token: 0x020006A9 RID: 1705
	[ComVisible(true)]
	public sealed class BinaryFormatter : IRemotingFormatter, IFormatter
	{
		/// <summary>Gets or sets the format in which type descriptions are laid out in the serialized stream.</summary>
		/// <returns>The style of type layouts to use.</returns>
		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06003E9B RID: 16027 RVA: 0x000D884B File Offset: 0x000D6A4B
		// (set) Token: 0x06003E9C RID: 16028 RVA: 0x000D8853 File Offset: 0x000D6A53
		public FormatterTypeStyle TypeFormat
		{
			get
			{
				return this.m_typeFormat;
			}
			set
			{
				this.m_typeFormat = value;
			}
		}

		/// <summary>Gets or sets the behavior of the deserializer with regards to finding and loading assemblies.</summary>
		/// <returns>One of the <see cref="T:System.Runtime.Serialization.Formatters.FormatterAssemblyStyle" /> values that specifies the deserializer behavior.</returns>
		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06003E9D RID: 16029 RVA: 0x000D885C File Offset: 0x000D6A5C
		// (set) Token: 0x06003E9E RID: 16030 RVA: 0x000D8864 File Offset: 0x000D6A64
		public FormatterAssemblyStyle AssemblyFormat
		{
			get
			{
				return this.m_assemblyFormat;
			}
			set
			{
				this.m_assemblyFormat = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Runtime.Serialization.Formatters.TypeFilterLevel" /> of automatic deserialization the <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter" /> performs.</summary>
		/// <returns>The <see cref="T:System.Runtime.Serialization.Formatters.TypeFilterLevel" /> that represents the current automatic deserialization level.</returns>
		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06003E9F RID: 16031 RVA: 0x000D886D File Offset: 0x000D6A6D
		// (set) Token: 0x06003EA0 RID: 16032 RVA: 0x000D8875 File Offset: 0x000D6A75
		public TypeFilterLevel FilterLevel
		{
			get
			{
				return this.m_securityLevel;
			}
			set
			{
				this.m_securityLevel = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> that controls type substitution during serialization and deserialization.</summary>
		/// <returns>The surrogate selector to use with this formatter.</returns>
		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06003EA1 RID: 16033 RVA: 0x000D887E File Offset: 0x000D6A7E
		// (set) Token: 0x06003EA2 RID: 16034 RVA: 0x000D8886 File Offset: 0x000D6A86
		public ISurrogateSelector SurrogateSelector
		{
			get
			{
				return this.m_surrogates;
			}
			set
			{
				this.m_surrogates = value;
			}
		}

		/// <summary>Gets or sets an object of type <see cref="T:System.Runtime.Serialization.SerializationBinder" /> that controls the binding of a serialized object to a type.</summary>
		/// <returns>The serialization binder to use with this formatter.</returns>
		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06003EA3 RID: 16035 RVA: 0x000D888F File Offset: 0x000D6A8F
		// (set) Token: 0x06003EA4 RID: 16036 RVA: 0x000D8897 File Offset: 0x000D6A97
		public SerializationBinder Binder
		{
			get
			{
				return this.m_binder;
			}
			set
			{
				this.m_binder = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Runtime.Serialization.StreamingContext" /> for this formatter.</summary>
		/// <returns>The streaming context to use with this formatter.</returns>
		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06003EA5 RID: 16037 RVA: 0x000D88A0 File Offset: 0x000D6AA0
		// (set) Token: 0x06003EA6 RID: 16038 RVA: 0x000D88A8 File Offset: 0x000D6AA8
		public StreamingContext Context
		{
			get
			{
				return this.m_context;
			}
			set
			{
				this.m_context = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter" /> class with default values.</summary>
		// Token: 0x06003EA7 RID: 16039 RVA: 0x000D88B1 File Offset: 0x000D6AB1
		public BinaryFormatter()
		{
			this.m_surrogates = null;
			this.m_context = new StreamingContext(StreamingContextStates.All);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter" /> class with a given surrogate selector and streaming context.</summary>
		/// <param name="selector">The <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> to use. Can be <see langword="null" />.</param>
		/// <param name="context">The source and destination for the serialized data.</param>
		// Token: 0x06003EA8 RID: 16040 RVA: 0x000D88DE File Offset: 0x000D6ADE
		public BinaryFormatter(ISurrogateSelector selector, StreamingContext context)
		{
			this.m_surrogates = selector;
			this.m_context = context;
		}

		/// <summary>Deserializes the specified stream into an object graph.</summary>
		/// <param name="serializationStream">The stream from which to deserialize the object graph.</param>
		/// <returns>The top (root) of the object graph.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream" /> supports seeking, but its length is 0.  
		///  -or-  
		///  The target type is a <see cref="T:System.Decimal" />, but the value is out of range of the <see cref="T:System.Decimal" /> type.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06003EA9 RID: 16041 RVA: 0x000D8902 File Offset: 0x000D6B02
		public object Deserialize(Stream serializationStream)
		{
			return this.Deserialize(serializationStream, null);
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x000D890C File Offset: 0x000D6B0C
		[SecurityCritical]
		internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck)
		{
			return this.Deserialize(serializationStream, handler, fCheck, null);
		}

		/// <summary>Deserializes the specified stream into an object graph. The provided <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> handles any headers in that stream.</summary>
		/// <param name="serializationStream">The stream from which to deserialize the object graph.</param>
		/// <param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> that handles any headers in the <paramref name="serializationStream" />. Can be <see langword="null" />.</param>
		/// <returns>The deserialized object or the top object (root) of the object graph.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream" /> supports seeking, but its length is 0.  
		///  -or-  
		///  The target type is a <see cref="T:System.Decimal" />, but the value is out of range of the <see cref="T:System.Decimal" /> type.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06003EAB RID: 16043 RVA: 0x000D8918 File Offset: 0x000D6B18
		[SecuritySafeCritical]
		public object Deserialize(Stream serializationStream, HeaderHandler handler)
		{
			return this.Deserialize(serializationStream, handler, true);
		}

		/// <summary>Deserializes a response to a remote method call from the provided <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="serializationStream">The stream from which to deserialize the object graph.</param>
		/// <param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> that handles any headers in the <paramref name="serializationStream" />. Can be <see langword="null" />.</param>
		/// <param name="methodCallMessage">The <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage" /> that contains details about where the call came from.</param>
		/// <returns>The deserialized response to the remote method call.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream" /> supports seeking, but its length is 0.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06003EAC RID: 16044 RVA: 0x000D8923 File Offset: 0x000D6B23
		[SecuritySafeCritical]
		public object DeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
		{
			return this.Deserialize(serializationStream, handler, true, methodCallMessage);
		}

		/// <summary>Deserializes the specified stream into an object graph. The provided <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> handles any headers in that stream.</summary>
		/// <param name="serializationStream">The stream from which to deserialize the object graph.</param>
		/// <param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> that handles any headers in the <paramref name="serializationStream" />. Can be <see langword="null" />.</param>
		/// <returns>The deserialized object or the top object (root) of the object graph.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream" /> supports seeking, but its length is 0.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06003EAD RID: 16045 RVA: 0x000D892F File Offset: 0x000D6B2F
		[SecurityCritical]
		[ComVisible(false)]
		public object UnsafeDeserialize(Stream serializationStream, HeaderHandler handler)
		{
			return this.Deserialize(serializationStream, handler, false);
		}

		/// <summary>Deserializes a response to a remote method call from the provided <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="serializationStream">The stream from which to deserialize the object graph.</param>
		/// <param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> that handles any headers in the <paramref name="serializationStream" />. Can be <see langword="null" />.</param>
		/// <param name="methodCallMessage">The <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage" /> that contains details about where the call came from.</param>
		/// <returns>The deserialized response to the remote method call.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream" /> supports seeking, but its length is 0.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06003EAE RID: 16046 RVA: 0x000D893A File Offset: 0x000D6B3A
		[SecurityCritical]
		[ComVisible(false)]
		public object UnsafeDeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
		{
			return this.Deserialize(serializationStream, handler, false, methodCallMessage);
		}

		// Token: 0x06003EAF RID: 16047 RVA: 0x000D8946 File Offset: 0x000D6B46
		[SecurityCritical]
		internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck, IMethodCallMessage methodCallMessage)
		{
			return this.Deserialize(serializationStream, handler, fCheck, false, methodCallMessage);
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x000D8954 File Offset: 0x000D6B54
		[SecurityCritical]
		internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck, bool isCrossAppDomain, IMethodCallMessage methodCallMessage)
		{
			if (serializationStream == null)
			{
				throw new ArgumentNullException("serializationStream", Environment.GetResourceString("Parameter '{0}' cannot be null.", new object[] { serializationStream }));
			}
			if (serializationStream.CanSeek && serializationStream.Length == 0L)
			{
				throw new SerializationException(Environment.GetResourceString("Attempting to deserialize an empty stream."));
			}
			InternalFE internalFE = new InternalFE();
			internalFE.FEtypeFormat = this.m_typeFormat;
			internalFE.FEserializerTypeEnum = InternalSerializerTypeE.Binary;
			internalFE.FEassemblyFormat = this.m_assemblyFormat;
			internalFE.FEsecurityLevel = this.m_securityLevel;
			ObjectReader objectReader = new ObjectReader(serializationStream, this.m_surrogates, this.m_context, internalFE, this.m_binder);
			objectReader.crossAppDomainArray = this.m_crossAppDomainArray;
			return objectReader.Deserialize(handler, new __BinaryParser(serializationStream, objectReader), fCheck, isCrossAppDomain, methodCallMessage);
		}

		/// <summary>Serializes the object, or graph of objects with the specified top (root), to the given stream.</summary>
		/// <param name="serializationStream">The stream to which the graph is to be serialized.</param>
		/// <param name="graph">The object at the root of the graph to serialize.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="graph" /> is null.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An error has occurred during serialization, such as if an object in the <paramref name="graph" /> parameter is not marked as serializable.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06003EB1 RID: 16049 RVA: 0x000D8A0D File Offset: 0x000D6C0D
		public void Serialize(Stream serializationStream, object graph)
		{
			this.Serialize(serializationStream, graph, null);
		}

		/// <summary>Serializes the object, or graph of objects with the specified top (root), to the given stream attaching the provided headers.</summary>
		/// <param name="serializationStream">The stream to which the object is to be serialized.</param>
		/// <param name="graph">The object at the root of the graph to serialize.</param>
		/// <param name="headers">Remoting headers to include in the serialization. Can be <see langword="null" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An error has occurred during serialization, such as if an object in the <paramref name="graph" /> parameter is not marked as serializable.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06003EB2 RID: 16050 RVA: 0x000D8A18 File Offset: 0x000D6C18
		[SecuritySafeCritical]
		public void Serialize(Stream serializationStream, object graph, Header[] headers)
		{
			this.Serialize(serializationStream, graph, headers, true);
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x000D8A24 File Offset: 0x000D6C24
		[SecurityCritical]
		internal void Serialize(Stream serializationStream, object graph, Header[] headers, bool fCheck)
		{
			if (serializationStream == null)
			{
				throw new ArgumentNullException("serializationStream", Environment.GetResourceString("Parameter '{0}' cannot be null.", new object[] { serializationStream }));
			}
			InternalFE internalFE = new InternalFE();
			internalFE.FEtypeFormat = this.m_typeFormat;
			internalFE.FEserializerTypeEnum = InternalSerializerTypeE.Binary;
			internalFE.FEassemblyFormat = this.m_assemblyFormat;
			ObjectWriter objectWriter = new ObjectWriter(this.m_surrogates, this.m_context, internalFE, this.m_binder);
			__BinaryWriter _BinaryWriter = new __BinaryWriter(serializationStream, objectWriter, this.m_typeFormat);
			objectWriter.Serialize(graph, headers, _BinaryWriter, fCheck);
			this.m_crossAppDomainArray = objectWriter.crossAppDomainArray;
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x000D8AB8 File Offset: 0x000D6CB8
		internal static TypeInformation GetTypeInformation(Type type)
		{
			Dictionary<Type, TypeInformation> dictionary = BinaryFormatter.typeNameCache;
			TypeInformation typeInformation2;
			lock (dictionary)
			{
				TypeInformation typeInformation = null;
				if (!BinaryFormatter.typeNameCache.TryGetValue(type, out typeInformation))
				{
					bool flag2;
					string clrAssemblyName = FormatterServices.GetClrAssemblyName(type, out flag2);
					typeInformation = new TypeInformation(FormatterServices.GetClrTypeFullName(type), clrAssemblyName, flag2);
					BinaryFormatter.typeNameCache.Add(type, typeInformation);
				}
				typeInformation2 = typeInformation;
			}
			return typeInformation2;
		}

		// Token: 0x040028CE RID: 10446
		internal ISurrogateSelector m_surrogates;

		// Token: 0x040028CF RID: 10447
		internal StreamingContext m_context;

		// Token: 0x040028D0 RID: 10448
		internal SerializationBinder m_binder;

		// Token: 0x040028D1 RID: 10449
		internal FormatterTypeStyle m_typeFormat = FormatterTypeStyle.TypesAlways;

		// Token: 0x040028D2 RID: 10450
		internal FormatterAssemblyStyle m_assemblyFormat;

		// Token: 0x040028D3 RID: 10451
		internal TypeFilterLevel m_securityLevel = TypeFilterLevel.Full;

		// Token: 0x040028D4 RID: 10452
		internal object[] m_crossAppDomainArray;

		// Token: 0x040028D5 RID: 10453
		private static Dictionary<Type, TypeInformation> typeNameCache = new Dictionary<Type, TypeInformation>();
	}
}
