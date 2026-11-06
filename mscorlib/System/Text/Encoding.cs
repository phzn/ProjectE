using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace System.Text
{
	/// <summary>Represents a character encoding.</summary>
	// Token: 0x020003BC RID: 956
	[ComVisible(true)]
	[Serializable]
	public abstract class Encoding : ICloneable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Text.Encoding" /> class.</summary>
		// Token: 0x06002799 RID: 10137 RVA: 0x0009042E File Offset: 0x0008E62E
		protected Encoding()
			: this(0)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.Encoding" /> class that corresponds to the specified code page.</summary>
		/// <param name="codePage">The code page identifier of the preferred encoding.  
		///  -or-  
		///  0, to use the default encoding.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="codePage" /> is less than zero.</exception>
		// Token: 0x0600279A RID: 10138 RVA: 0x00090437 File Offset: 0x0008E637
		protected Encoding(int codePage)
		{
			this.m_isReadOnly = true;
			base..ctor();
			if (codePage < 0)
			{
				throw new ArgumentOutOfRangeException("codePage");
			}
			this.m_codePage = codePage;
			this.SetDefaultFallbacks();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.Encoding" /> class that corresponds to the specified code page with the specified encoder and decoder fallback strategies.</summary>
		/// <param name="codePage">The encoding code page identifier.</param>
		/// <param name="encoderFallback">An object that provides an error-handling procedure when a character cannot be encoded with the current encoding.</param>
		/// <param name="decoderFallback">An object that provides an error-handling procedure when a byte sequence cannot be decoded with the current encoding.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="codePage" /> is less than zero.</exception>
		// Token: 0x0600279B RID: 10139 RVA: 0x00090464 File Offset: 0x0008E664
		protected Encoding(int codePage, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
		{
			this.m_isReadOnly = true;
			base..ctor();
			if (codePage < 0)
			{
				throw new ArgumentOutOfRangeException("codePage");
			}
			this.m_codePage = codePage;
			this.encoderFallback = encoderFallback ?? new InternalEncoderBestFitFallback(this);
			this.decoderFallback = decoderFallback ?? new InternalDecoderBestFitFallback(this);
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000904B6 File Offset: 0x0008E6B6
		internal virtual void SetDefaultFallbacks()
		{
			this.encoderFallback = new InternalEncoderBestFitFallback(this);
			this.decoderFallback = new InternalDecoderBestFitFallback(this);
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000904D0 File Offset: 0x0008E6D0
		internal void OnDeserializing()
		{
			this.encoderFallback = null;
			this.decoderFallback = null;
			this.m_isReadOnly = true;
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000904E7 File Offset: 0x0008E6E7
		internal void OnDeserialized()
		{
			if (this.encoderFallback == null || this.decoderFallback == null)
			{
				this.m_deserializedFromEverett = true;
				this.SetDefaultFallbacks();
			}
			this.dataItem = null;
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x0009050D File Offset: 0x0008E70D
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.OnDeserializing();
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x00090515 File Offset: 0x0008E715
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this.OnDeserialized();
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x0009051D File Offset: 0x0008E71D
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.dataItem = null;
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x00090528 File Offset: 0x0008E728
		internal void DeserializeEncoding(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_codePage = (int)info.GetValue("m_codePage", typeof(int));
			this.dataItem = null;
			try
			{
				this.m_isReadOnly = (bool)info.GetValue("m_isReadOnly", typeof(bool));
				this.encoderFallback = (EncoderFallback)info.GetValue("encoderFallback", typeof(EncoderFallback));
				this.decoderFallback = (DecoderFallback)info.GetValue("decoderFallback", typeof(DecoderFallback));
			}
			catch (SerializationException)
			{
				this.m_deserializedFromEverett = true;
				this.m_isReadOnly = true;
				this.SetDefaultFallbacks();
			}
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x000905F4 File Offset: 0x0008E7F4
		internal void SerializeEncoding(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("m_isReadOnly", this.m_isReadOnly);
			info.AddValue("encoderFallback", this.EncoderFallback);
			info.AddValue("decoderFallback", this.DecoderFallback);
			info.AddValue("m_codePage", this.m_codePage);
			info.AddValue("dataItem", null);
			info.AddValue("Encoding+m_codePage", this.m_codePage);
			info.AddValue("Encoding+dataItem", null);
		}

		/// <summary>Converts an entire byte array from one encoding to another.</summary>
		/// <param name="srcEncoding">The encoding format of <paramref name="bytes" />.</param>
		/// <param name="dstEncoding">The target encoding format.</param>
		/// <param name="bytes">The bytes to convert.</param>
		/// <returns>An array of type <see cref="T:System.Byte" /> containing the results of converting <paramref name="bytes" /> from <paramref name="srcEncoding" /> to <paramref name="dstEncoding" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="srcEncoding" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="dstEncoding" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  srcEncoding. <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  dstEncoding. <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027A4 RID: 10148 RVA: 0x0009067C File Offset: 0x0008E87C
		public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			return Encoding.Convert(srcEncoding, dstEncoding, bytes, 0, bytes.Length);
		}

		/// <summary>Converts a range of bytes in a byte array from one encoding to another.</summary>
		/// <param name="srcEncoding">The encoding of the source array, <paramref name="bytes" />.</param>
		/// <param name="dstEncoding">The encoding of the output array.</param>
		/// <param name="bytes">The array of bytes to convert.</param>
		/// <param name="index">The index of the first element of <paramref name="bytes" /> to convert.</param>
		/// <param name="count">The number of bytes to convert.</param>
		/// <returns>An array of type <see cref="T:System.Byte" /> containing the result of converting a range of bytes in <paramref name="bytes" /> from <paramref name="srcEncoding" /> to <paramref name="dstEncoding" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="srcEncoding" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="dstEncoding" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> and <paramref name="count" /> do not specify a valid range in the byte array.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  srcEncoding. <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  dstEncoding. <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027A5 RID: 10149 RVA: 0x00090698 File Offset: 0x0008E898
		public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes, int index, int count)
		{
			if (srcEncoding == null || dstEncoding == null)
			{
				throw new ArgumentNullException((srcEncoding == null) ? "srcEncoding" : "dstEncoding", Environment.GetResourceString("Array cannot be null."));
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("Array cannot be null."));
			}
			return dstEncoding.GetBytes(srcEncoding.GetChars(bytes, index, count));
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060027A6 RID: 10150 RVA: 0x000906F4 File Offset: 0x0008E8F4
		private static object InternalSyncObject
		{
			get
			{
				if (Encoding.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange<object>(ref Encoding.s_InternalSyncObject, obj, null);
				}
				return Encoding.s_InternalSyncObject;
			}
		}

		/// <summary>Registers an encoding provider.</summary>
		/// <param name="provider">A subclass of <see cref="T:System.Text.EncodingProvider" /> that provides access to additional character encodings.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="provider" /> is <see langword="null" />.</exception>
		// Token: 0x060027A7 RID: 10151 RVA: 0x00090720 File Offset: 0x0008E920
		[SecurityCritical]
		public static void RegisterProvider(EncodingProvider provider)
		{
			EncodingProvider.AddProvider(provider);
		}

		/// <summary>Returns the encoding associated with the specified code page identifier.</summary>
		/// <param name="codepage">The code page identifier of the preferred encoding. Possible values are listed in the Code Page column of the table that appears in the <see cref="T:System.Text.Encoding" /> class topic.  
		///  -or-  
		///  0 (zero), to use the default encoding.</param>
		/// <returns>The encoding that is associated with the specified code page.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="codepage" /> is less than zero or greater than 65535.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="codepage" /> is not supported by the underlying platform.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="codepage" /> is not supported by the underlying platform.</exception>
		// Token: 0x060027A8 RID: 10152 RVA: 0x00090728 File Offset: 0x0008E928
		[SecuritySafeCritical]
		public static Encoding GetEncoding(int codepage)
		{
			Encoding encoding = EncodingProvider.GetEncodingFromProvider(codepage);
			if (encoding != null)
			{
				return encoding;
			}
			if (codepage < 0 || codepage > 65535)
			{
				throw new ArgumentOutOfRangeException("codepage", Environment.GetResourceString("Valid values are between {0} and {1}, inclusive.", new object[] { 0, 65535 }));
			}
			object internalSyncObject = Encoding.InternalSyncObject;
			lock (internalSyncObject)
			{
				if (Encoding.encodings != null)
				{
					Encoding.encodings.TryGetValue(codepage, out encoding);
				}
				if (encoding == null)
				{
					if (Encoding.encodings == null)
					{
						Encoding.encodings = new Dictionary<int, Encoding>();
					}
					if (Encoding.encodings.TryGetValue(codepage, out encoding))
					{
						return encoding;
					}
					if (codepage <= 1201)
					{
						if (codepage <= 3)
						{
							if (codepage == 0)
							{
								encoding = Encoding.Default;
								goto IL_0233;
							}
							if (codepage - 1 > 2)
							{
								goto IL_01B0;
							}
						}
						else if (codepage != 42)
						{
							if (codepage == 1200)
							{
								encoding = Encoding.Unicode;
								goto IL_0233;
							}
							if (codepage != 1201)
							{
								goto IL_01B0;
							}
							encoding = Encoding.BigEndianUnicode;
							goto IL_0233;
						}
						throw new ArgumentException(Environment.GetResourceString("{0} is not a supported code page.", new object[] { codepage }), "codepage");
					}
					if (codepage <= 20127)
					{
						if (codepage == 12000)
						{
							encoding = Encoding.UTF32;
							goto IL_0233;
						}
						if (codepage == 12001)
						{
							encoding = new UTF32Encoding(true, true);
							goto IL_0233;
						}
						if (codepage == 20127)
						{
							encoding = Encoding.ASCII;
							goto IL_0233;
						}
					}
					else
					{
						if (codepage == 28591)
						{
							encoding = Encoding.Latin1;
							goto IL_0233;
						}
						if (codepage == 65000)
						{
							encoding = Encoding.UTF7;
							goto IL_0233;
						}
						if (codepage == 65001)
						{
							encoding = Encoding.UTF8;
							goto IL_0233;
						}
					}
					IL_01B0:
					if (EncodingTable.GetCodePageDataItem(codepage) == null)
					{
						throw new NotSupportedException(Environment.GetResourceString("No data is available for encoding {0}. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.", new object[] { codepage }));
					}
					if (codepage != 12000)
					{
						if (codepage != 12001)
						{
							encoding = (Encoding)EncodingHelper.InvokeI18N("GetEncoding", new object[] { codepage });
							if (encoding == null)
							{
								throw new NotSupportedException(string.Format("Encoding {0} data could not be found. Make sure you have correct international codeset assembly installed and enabled.", codepage));
							}
						}
						else
						{
							encoding = new UTF32Encoding(true, true);
						}
					}
					else
					{
						encoding = Encoding.UTF32;
					}
					IL_0233:
					Encoding.encodings.Add(codepage, encoding);
				}
			}
			return encoding;
		}

		/// <summary>Returns the encoding associated with the specified code page identifier. Parameters specify an error handler for characters that cannot be encoded and byte sequences that cannot be decoded.</summary>
		/// <param name="codepage">The code page identifier of the preferred encoding. Possible values are listed in the Code Page column of the table that appears in the <see cref="T:System.Text.Encoding" /> class topic.  
		///  -or-  
		///  0 (zero), to use the default encoding.</param>
		/// <param name="encoderFallback">An object that provides an error-handling procedure when a character cannot be encoded with the current encoding.</param>
		/// <param name="decoderFallback">An object that provides an error-handling procedure when a byte sequence cannot be decoded with the current encoding.</param>
		/// <returns>The encoding that is associated with the specified code page.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="codepage" /> is less than zero or greater than 65535.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="codepage" /> is not supported by the underlying platform.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="codepage" /> is not supported by the underlying platform.</exception>
		// Token: 0x060027A9 RID: 10153 RVA: 0x000909A4 File Offset: 0x0008EBA4
		public static Encoding GetEncoding(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
		{
			Encoding encoding = EncodingProvider.GetEncodingFromProvider(codepage, encoderFallback, decoderFallback);
			if (encoding != null)
			{
				return encoding;
			}
			encoding = Encoding.GetEncoding(codepage);
			Encoding encoding2 = (Encoding)encoding.Clone();
			encoding2.EncoderFallback = encoderFallback;
			encoding2.DecoderFallback = decoderFallback;
			return encoding2;
		}

		/// <summary>Returns the encoding associated with the specified code page name.</summary>
		/// <param name="name">The code page name of the preferred encoding. Any value returned by the <see cref="P:System.Text.Encoding.WebName" /> property is valid. Possible values are listed in the Name column of the table that appears in the <see cref="T:System.Text.Encoding" /> class topic.</param>
		/// <returns>The encoding  associated with the specified code page.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is not a valid code page name.  
		/// -or-  
		/// The code page indicated by <paramref name="name" /> is not supported by the underlying platform.</exception>
		// Token: 0x060027AA RID: 10154 RVA: 0x000909E0 File Offset: 0x0008EBE0
		public static Encoding GetEncoding(string name)
		{
			Encoding encodingFromProvider = EncodingProvider.GetEncodingFromProvider(name);
			if (encodingFromProvider != null)
			{
				return encodingFromProvider;
			}
			return Encoding.GetEncoding(EncodingTable.GetCodePageFromName(name));
		}

		/// <summary>Returns the encoding associated with the specified code page name. Parameters specify an error handler for characters that cannot be encoded and byte sequences that cannot be decoded.</summary>
		/// <param name="name">The code page name of the preferred encoding. Any value returned by the <see cref="P:System.Text.Encoding.WebName" /> property is valid. Possible values are listed in the Name column of the table that appears in the <see cref="T:System.Text.Encoding" /> class topic.</param>
		/// <param name="encoderFallback">An object that provides an error-handling procedure when a character cannot be encoded with the current encoding.</param>
		/// <param name="decoderFallback">An object that provides an error-handling procedure when a byte sequence cannot be decoded with the current encoding.</param>
		/// <returns>The encoding that is associated with the specified code page.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is not a valid code page name.  
		/// -or-  
		/// The code page indicated by <paramref name="name" /> is not supported by the underlying platform.</exception>
		// Token: 0x060027AB RID: 10155 RVA: 0x00090A04 File Offset: 0x0008EC04
		public static Encoding GetEncoding(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
		{
			Encoding encodingFromProvider = EncodingProvider.GetEncodingFromProvider(name, encoderFallback, decoderFallback);
			if (encodingFromProvider != null)
			{
				return encodingFromProvider;
			}
			return Encoding.GetEncoding(EncodingTable.GetCodePageFromName(name), encoderFallback, decoderFallback);
		}

		/// <summary>Returns an array that contains all encodings.</summary>
		/// <returns>An array that contains all encodings.</returns>
		// Token: 0x060027AC RID: 10156 RVA: 0x00090A2C File Offset: 0x0008EC2C
		public static EncodingInfo[] GetEncodings()
		{
			return EncodingTable.GetEncodings();
		}

		/// <summary>When overridden in a derived class, returns a sequence of bytes that specifies the encoding used.</summary>
		/// <returns>A byte array containing a sequence of bytes that specifies the encoding used.  
		///  -or-  
		///  A byte array of length zero, if a preamble is not required.</returns>
		// Token: 0x060027AD RID: 10157 RVA: 0x00090A33 File Offset: 0x0008EC33
		public virtual byte[] GetPreamble()
		{
			return EmptyArray<byte>.Value;
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060027AE RID: 10158 RVA: 0x00090A3A File Offset: 0x0008EC3A
		public virtual ReadOnlySpan<byte> Preamble
		{
			get
			{
				return this.GetPreamble();
			}
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x00090A48 File Offset: 0x0008EC48
		private void GetDataItem()
		{
			if (this.dataItem == null)
			{
				this.dataItem = EncodingTable.GetCodePageDataItem(this.m_codePage);
				if (this.dataItem == null)
				{
					throw new NotSupportedException(Environment.GetResourceString("No data is available for encoding {0}. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.", new object[] { this.m_codePage }));
				}
			}
		}

		/// <summary>When overridden in a derived class, gets a name for the current encoding that can be used with mail agent body tags.</summary>
		/// <returns>A name for the current <see cref="T:System.Text.Encoding" /> that can be used with mail agent body tags.  
		///  -or-  
		///  An empty string (""), if the current <see cref="T:System.Text.Encoding" /> cannot be used.</returns>
		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060027B0 RID: 10160 RVA: 0x00090A9A File Offset: 0x0008EC9A
		public virtual string BodyName
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.BodyName;
			}
		}

		/// <summary>When overridden in a derived class, gets the human-readable description of the current encoding.</summary>
		/// <returns>The human-readable description of the current <see cref="T:System.Text.Encoding" />.</returns>
		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060027B1 RID: 10161 RVA: 0x00090AB5 File Offset: 0x0008ECB5
		public virtual string EncodingName
		{
			get
			{
				return Environment.GetResourceStringEncodingName(this.m_codePage);
			}
		}

		/// <summary>When overridden in a derived class, gets a name for the current encoding that can be used with mail agent header tags.</summary>
		/// <returns>A name for the current <see cref="T:System.Text.Encoding" /> to use with mail agent header tags.  
		///  -or-  
		///  An empty string (""), if the current <see cref="T:System.Text.Encoding" /> cannot be used.</returns>
		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060027B2 RID: 10162 RVA: 0x00090AC2 File Offset: 0x0008ECC2
		public virtual string HeaderName
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.HeaderName;
			}
		}

		/// <summary>When overridden in a derived class, gets the name registered with the Internet Assigned Numbers Authority (IANA) for the current encoding.</summary>
		/// <returns>The IANA name for the current <see cref="T:System.Text.Encoding" />.</returns>
		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060027B3 RID: 10163 RVA: 0x00090ADD File Offset: 0x0008ECDD
		public virtual string WebName
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.WebName;
			}
		}

		/// <summary>When overridden in a derived class, gets the Windows operating system code page that most closely corresponds to the current encoding.</summary>
		/// <returns>The Windows operating system code page that most closely corresponds to the current <see cref="T:System.Text.Encoding" />.</returns>
		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060027B4 RID: 10164 RVA: 0x00090AF8 File Offset: 0x0008ECF8
		public virtual int WindowsCodePage
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return this.dataItem.UIFamilyCodePage;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding can be used by browser clients for displaying content.</summary>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Text.Encoding" /> can be used by browser clients for displaying content; otherwise, <see langword="false" />.</returns>
		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060027B5 RID: 10165 RVA: 0x00090B13 File Offset: 0x0008ED13
		public virtual bool IsBrowserDisplay
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 2U) > 0U;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding can be used by browser clients for saving content.</summary>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Text.Encoding" /> can be used by browser clients for saving content; otherwise, <see langword="false" />.</returns>
		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060027B6 RID: 10166 RVA: 0x00090B33 File Offset: 0x0008ED33
		public virtual bool IsBrowserSave
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 512U) > 0U;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding can be used by mail and news clients for displaying content.</summary>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Text.Encoding" /> can be used by mail and news clients for displaying content; otherwise, <see langword="false" />.</returns>
		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060027B7 RID: 10167 RVA: 0x00090B57 File Offset: 0x0008ED57
		public virtual bool IsMailNewsDisplay
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 1U) > 0U;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding can be used by mail and news clients for saving content.</summary>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Text.Encoding" /> can be used by mail and news clients for saving content; otherwise, <see langword="false" />.</returns>
		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060027B8 RID: 10168 RVA: 0x00090B77 File Offset: 0x0008ED77
		public virtual bool IsMailNewsSave
		{
			get
			{
				if (this.dataItem == null)
				{
					this.GetDataItem();
				}
				return (this.dataItem.Flags & 256U) > 0U;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding uses single-byte code points.</summary>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Text.Encoding" /> uses single-byte code points; otherwise, <see langword="false" />.</returns>
		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x060027B9 RID: 10169 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[ComVisible(false)]
		public virtual bool IsSingleByte
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Text.EncoderFallback" /> object for the current <see cref="T:System.Text.Encoding" /> object.</summary>
		/// <returns>The encoder fallback object for the current <see cref="T:System.Text.Encoding" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value in a set operation is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">A value cannot be assigned in a set operation because the current <see cref="T:System.Text.Encoding" /> object is read-only.</exception>
		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x060027BA RID: 10170 RVA: 0x00090B9B File Offset: 0x0008ED9B
		// (set) Token: 0x060027BB RID: 10171 RVA: 0x00090BA3 File Offset: 0x0008EDA3
		[ComVisible(false)]
		public EncoderFallback EncoderFallback
		{
			get
			{
				return this.encoderFallback;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Instance is read-only."));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.encoderFallback = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Text.DecoderFallback" /> object for the current <see cref="T:System.Text.Encoding" /> object.</summary>
		/// <returns>The decoder fallback object for the current <see cref="T:System.Text.Encoding" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value in a set operation is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">A value cannot be assigned in a set operation because the current <see cref="T:System.Text.Encoding" /> object is read-only.</exception>
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060027BC RID: 10172 RVA: 0x00090BD2 File Offset: 0x0008EDD2
		// (set) Token: 0x060027BD RID: 10173 RVA: 0x00090BDA File Offset: 0x0008EDDA
		[ComVisible(false)]
		public DecoderFallback DecoderFallback
		{
			get
			{
				return this.decoderFallback;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Instance is read-only."));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.decoderFallback = value;
			}
		}

		/// <summary>When overridden in a derived class, creates a shallow copy of the current <see cref="T:System.Text.Encoding" /> object.</summary>
		/// <returns>A copy of the current <see cref="T:System.Text.Encoding" /> object.</returns>
		// Token: 0x060027BE RID: 10174 RVA: 0x00090C09 File Offset: 0x0008EE09
		[ComVisible(false)]
		public virtual object Clone()
		{
			Encoding encoding = (Encoding)base.MemberwiseClone();
			encoding.m_isReadOnly = false;
			return encoding;
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding is read-only.</summary>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Text.Encoding" /> is read-only; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060027BF RID: 10175 RVA: 0x00090C1D File Offset: 0x0008EE1D
		[ComVisible(false)]
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		/// <summary>Gets an encoding for the ASCII (7-bit) character set.</summary>
		/// <returns>An  encoding for the ASCII (7-bit) character set.</returns>
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060027C0 RID: 10176 RVA: 0x00090C25 File Offset: 0x0008EE25
		public static Encoding ASCII
		{
			get
			{
				if (Encoding.asciiEncoding == null)
				{
					Encoding.asciiEncoding = new ASCIIEncoding();
				}
				return Encoding.asciiEncoding;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060027C1 RID: 10177 RVA: 0x00090C43 File Offset: 0x0008EE43
		private static Encoding Latin1
		{
			get
			{
				if (Encoding.latin1Encoding == null)
				{
					Encoding.latin1Encoding = new Latin1Encoding();
				}
				return Encoding.latin1Encoding;
			}
		}

		/// <summary>When overridden in a derived class, calculates the number of bytes produced by encoding all the characters in the specified character array.</summary>
		/// <param name="chars">The character array containing the characters to encode.</param>
		/// <returns>The number of bytes produced by encoding all the characters in the specified character array.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027C2 RID: 10178 RVA: 0x00090C61 File Offset: 0x0008EE61
		public virtual int GetByteCount(char[] chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("Array cannot be null."));
			}
			return this.GetByteCount(chars, 0, chars.Length);
		}

		/// <summary>When overridden in a derived class, calculates the number of bytes produced by encoding the characters in the specified string.</summary>
		/// <param name="s">The string containing the set of characters to encode.</param>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027C3 RID: 10179 RVA: 0x00090C88 File Offset: 0x0008EE88
		public virtual int GetByteCount(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			char[] array = s.ToCharArray();
			return this.GetByteCount(array, 0, array.Length);
		}

		/// <summary>When overridden in a derived class, calculates the number of bytes produced by encoding a set of characters from the specified character array.</summary>
		/// <param name="chars">The character array containing the set of characters to encode.</param>
		/// <param name="index">The index of the first character to encode.</param>
		/// <param name="count">The number of characters to encode.</param>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.  
		/// -or-  
		/// <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="chars" />.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027C4 RID: 10180
		public abstract int GetByteCount(char[] chars, int index, int count);

		// Token: 0x060027C5 RID: 10181 RVA: 0x00090CB5 File Offset: 0x0008EEB5
		public int GetByteCount(string str, int index, int count)
		{
			return this.GetByteCount(str.ToCharArray(), index, count);
		}

		/// <summary>When overridden in a derived class, calculates the number of bytes produced by encoding a set of characters starting at the specified character pointer.</summary>
		/// <param name="chars">A pointer to the first character to encode.</param>
		/// <param name="count">The number of characters to encode.</param>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027C6 RID: 10182 RVA: 0x00090CC8 File Offset: 0x0008EEC8
		[ComVisible(false)]
		[SecurityCritical]
		[CLSCompliant(false)]
		public unsafe virtual int GetByteCount(char* chars, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("Array cannot be null."));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("Non-negative number required."));
			}
			char[] array = new char[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = chars[i];
			}
			return this.GetByteCount(array, 0, count);
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x00090D2E File Offset: 0x0008EF2E
		[SecurityCritical]
		internal unsafe virtual int GetByteCount(char* chars, int count, EncoderNLS encoder)
		{
			return this.GetByteCount(chars, count);
		}

		/// <summary>When overridden in a derived class, encodes all the characters in the specified character array into a sequence of bytes.</summary>
		/// <param name="chars">The character array containing the characters to encode.</param>
		/// <returns>A byte array containing the results of encoding the specified set of characters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027C8 RID: 10184 RVA: 0x00090D38 File Offset: 0x0008EF38
		public virtual byte[] GetBytes(char[] chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("Array cannot be null."));
			}
			return this.GetBytes(chars, 0, chars.Length);
		}

		/// <summary>When overridden in a derived class, encodes a set of characters from the specified character array into a sequence of bytes.</summary>
		/// <param name="chars">The character array containing the set of characters to encode.</param>
		/// <param name="index">The index of the first character to encode.</param>
		/// <param name="count">The number of characters to encode.</param>
		/// <returns>A byte array containing the results of encoding the specified set of characters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.  
		/// -or-  
		/// <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="chars" />.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027C9 RID: 10185 RVA: 0x00090D60 File Offset: 0x0008EF60
		public virtual byte[] GetBytes(char[] chars, int index, int count)
		{
			byte[] array = new byte[this.GetByteCount(chars, index, count)];
			this.GetBytes(chars, index, count, array, 0);
			return array;
		}

		/// <summary>When overridden in a derived class, encodes a set of characters from the specified character array into the specified byte array.</summary>
		/// <param name="chars">The character array containing the set of characters to encode.</param>
		/// <param name="charIndex">The index of the first character to encode.</param>
		/// <param name="charCount">The number of characters to encode.</param>
		/// <param name="bytes">The byte array to contain the resulting sequence of bytes.</param>
		/// <param name="byteIndex">The index at which to start writing the resulting sequence of bytes.</param>
		/// <returns>The actual number of bytes written into <paramref name="bytes" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charIndex" /> or <paramref name="charCount" /> or <paramref name="byteIndex" /> is less than zero.  
		/// -or-  
		/// <paramref name="charIndex" /> and <paramref name="charCount" /> do not denote a valid range in <paramref name="chars" />.  
		/// -or-  
		/// <paramref name="byteIndex" /> is not a valid index in <paramref name="bytes" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="bytes" /> does not have enough capacity from <paramref name="byteIndex" /> to the end of the array to accommodate the resulting bytes.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027CA RID: 10186
		public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex);

		/// <summary>When overridden in a derived class, encodes all the characters in the specified string into a sequence of bytes.</summary>
		/// <param name="s">The string containing the characters to encode.</param>
		/// <returns>A byte array containing the results of encoding the specified set of characters.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027CB RID: 10187 RVA: 0x00090D8C File Offset: 0x0008EF8C
		public virtual byte[] GetBytes(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s", Environment.GetResourceString("String reference not set to an instance of a String."));
			}
			byte[] array = new byte[this.GetByteCount(s)];
			this.GetBytes(s, 0, s.Length, array, 0);
			return array;
		}

		/// <summary>When overridden in a derived class, encodes a set of characters from the specified string into the specified byte array.</summary>
		/// <param name="s">The string containing the set of characters to encode.</param>
		/// <param name="charIndex">The index of the first character to encode.</param>
		/// <param name="charCount">The number of characters to encode.</param>
		/// <param name="bytes">The byte array to contain the resulting sequence of bytes.</param>
		/// <param name="byteIndex">The index at which to start writing the resulting sequence of bytes.</param>
		/// <returns>The actual number of bytes written into <paramref name="bytes" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charIndex" /> or <paramref name="charCount" /> or <paramref name="byteIndex" /> is less than zero.  
		/// -or-  
		/// <paramref name="charIndex" /> and <paramref name="charCount" /> do not denote a valid range in <paramref name="chars" />.  
		/// -or-  
		/// <paramref name="byteIndex" /> is not a valid index in <paramref name="bytes" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="bytes" /> does not have enough capacity from <paramref name="byteIndex" /> to the end of the array to accommodate the resulting bytes.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027CC RID: 10188 RVA: 0x00090DD0 File Offset: 0x0008EFD0
		public virtual int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return this.GetBytes(s.ToCharArray(), charIndex, charCount, bytes, byteIndex);
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x00090DF2 File Offset: 0x0008EFF2
		[SecurityCritical]
		internal unsafe virtual int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS encoder)
		{
			return this.GetBytes(chars, charCount, bytes, byteCount);
		}

		/// <summary>When overridden in a derived class, encodes a set of characters starting at the specified character pointer into a sequence of bytes that are stored starting at the specified byte pointer.</summary>
		/// <param name="chars">A pointer to the first character to encode.</param>
		/// <param name="charCount">The number of characters to encode.</param>
		/// <param name="bytes">A pointer to the location at which to start writing the resulting sequence of bytes.</param>
		/// <param name="byteCount">The maximum number of bytes to write.</param>
		/// <returns>The actual number of bytes written at the location indicated by the <paramref name="bytes" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charCount" /> or <paramref name="byteCount" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="byteCount" /> is less than the resulting number of bytes.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027CE RID: 10190 RVA: 0x00090E00 File Offset: 0x0008F000
		[ComVisible(false)]
		[SecurityCritical]
		[CLSCompliant(false)]
		public unsafe virtual int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", Environment.GetResourceString("Array cannot be null."));
			}
			if (charCount < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((charCount < 0) ? "charCount" : "byteCount", Environment.GetResourceString("Non-negative number required."));
			}
			char[] array = new char[charCount];
			for (int i = 0; i < charCount; i++)
			{
				array[i] = chars[i];
			}
			byte[] array2 = new byte[byteCount];
			int bytes2 = this.GetBytes(array, 0, charCount, array2, 0);
			if (bytes2 < byteCount)
			{
				byteCount = bytes2;
			}
			for (int i = 0; i < byteCount; i++)
			{
				bytes[i] = array2[i];
			}
			return byteCount;
		}

		/// <summary>When overridden in a derived class, calculates the number of characters produced by decoding all the bytes in the specified byte array.</summary>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		// Token: 0x060027CF RID: 10191 RVA: 0x00090EB0 File Offset: 0x0008F0B0
		public virtual int GetCharCount(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("Array cannot be null."));
			}
			return this.GetCharCount(bytes, 0, bytes.Length);
		}

		/// <summary>When overridden in a derived class, calculates the number of characters produced by decoding a sequence of bytes from the specified byte array.</summary>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
		/// <param name="index">The index of the first byte to decode.</param>
		/// <param name="count">The number of bytes to decode.</param>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.  
		/// -or-  
		/// <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		// Token: 0x060027D0 RID: 10192
		public abstract int GetCharCount(byte[] bytes, int index, int count);

		/// <summary>When overridden in a derived class, calculates the number of characters produced by decoding a sequence of bytes starting at the specified byte pointer.</summary>
		/// <param name="bytes">A pointer to the first byte to decode.</param>
		/// <param name="count">The number of bytes to decode.</param>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		// Token: 0x060027D1 RID: 10193 RVA: 0x00090ED8 File Offset: 0x0008F0D8
		[SecurityCritical]
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe virtual int GetCharCount(byte* bytes, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("Array cannot be null."));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("Non-negative number required."));
			}
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = bytes[i];
			}
			return this.GetCharCount(array, 0, count);
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x00090F3B File Offset: 0x0008F13B
		[SecurityCritical]
		internal unsafe virtual int GetCharCount(byte* bytes, int count, DecoderNLS decoder)
		{
			return this.GetCharCount(bytes, count);
		}

		/// <summary>When overridden in a derived class, decodes all the bytes in the specified byte array into a set of characters.</summary>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
		/// <returns>A character array containing the results of decoding the specified sequence of bytes.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		// Token: 0x060027D3 RID: 10195 RVA: 0x00090F45 File Offset: 0x0008F145
		public virtual char[] GetChars(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("Array cannot be null."));
			}
			return this.GetChars(bytes, 0, bytes.Length);
		}

		/// <summary>When overridden in a derived class, decodes a sequence of bytes from the specified byte array into a set of characters.</summary>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
		/// <param name="index">The index of the first byte to decode.</param>
		/// <param name="count">The number of bytes to decode.</param>
		/// <returns>A character array containing the results of decoding the specified sequence of bytes.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.  
		/// -or-  
		/// <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		// Token: 0x060027D4 RID: 10196 RVA: 0x00090F6C File Offset: 0x0008F16C
		public virtual char[] GetChars(byte[] bytes, int index, int count)
		{
			char[] array = new char[this.GetCharCount(bytes, index, count)];
			this.GetChars(bytes, index, count, array, 0);
			return array;
		}

		/// <summary>When overridden in a derived class, decodes a sequence of bytes from the specified byte array into the specified character array.</summary>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
		/// <param name="byteIndex">The index of the first byte to decode.</param>
		/// <param name="byteCount">The number of bytes to decode.</param>
		/// <param name="chars">The character array to contain the resulting set of characters.</param>
		/// <param name="charIndex">The index at which to start writing the resulting set of characters.</param>
		/// <returns>The actual number of characters written into <paramref name="chars" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="chars" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteIndex" /> or <paramref name="byteCount" /> or <paramref name="charIndex" /> is less than zero.  
		/// -or-  
		/// <paramref name="byteindex" /> and <paramref name="byteCount" /> do not denote a valid range in <paramref name="bytes" />.  
		/// -or-  
		/// <paramref name="charIndex" /> is not a valid index in <paramref name="chars" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="chars" /> does not have enough capacity from <paramref name="charIndex" /> to the end of the array to accommodate the resulting characters.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		// Token: 0x060027D5 RID: 10197
		public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);

		/// <summary>When overridden in a derived class, decodes a sequence of bytes starting at the specified byte pointer into a set of characters that are stored starting at the specified character pointer.</summary>
		/// <param name="bytes">A pointer to the first byte to decode.</param>
		/// <param name="byteCount">The number of bytes to decode.</param>
		/// <param name="chars">A pointer to the location at which to start writing the resulting set of characters.</param>
		/// <param name="charCount">The maximum number of characters to write.</param>
		/// <returns>The actual number of characters written at the location indicated by the <paramref name="chars" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="chars" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteCount" /> or <paramref name="charCount" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="charCount" /> is less than the resulting number of characters.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		// Token: 0x060027D6 RID: 10198 RVA: 0x00090F98 File Offset: 0x0008F198
		[ComVisible(false)]
		[CLSCompliant(false)]
		[SecurityCritical]
		public unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
		{
			if (chars == null || bytes == null)
			{
				throw new ArgumentNullException((chars == null) ? "chars" : "bytes", Environment.GetResourceString("Array cannot be null."));
			}
			if (byteCount < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((byteCount < 0) ? "byteCount" : "charCount", Environment.GetResourceString("Non-negative number required."));
			}
			byte[] array = new byte[byteCount];
			for (int i = 0; i < byteCount; i++)
			{
				array[i] = bytes[i];
			}
			char[] array2 = new char[charCount];
			int chars2 = this.GetChars(array, 0, byteCount, array2, 0);
			if (chars2 < charCount)
			{
				charCount = chars2;
			}
			for (int i = 0; i < charCount; i++)
			{
				chars[i] = array2[i];
			}
			return charCount;
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x00091048 File Offset: 0x0008F248
		[SecurityCritical]
		internal unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount, DecoderNLS decoder)
		{
			return this.GetChars(bytes, byteCount, chars, charCount);
		}

		/// <summary>When overridden in a derived class, decodes a specified number of bytes starting at a specified address into a string.</summary>
		/// <param name="bytes">A pointer to a byte array.</param>
		/// <param name="byteCount">The number of bytes to decode.</param>
		/// <returns>A string that contains the results of decoding the specified sequence of bytes.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is a null pointer.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteCount" /> is less than zero.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A   fallback occurred (see Character Encoding in .NET for a complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		// Token: 0x060027D8 RID: 10200 RVA: 0x00091055 File Offset: 0x0008F255
		[SecurityCritical]
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe string GetString(byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("Array cannot be null."));
			}
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("Non-negative number required."));
			}
			return string.CreateStringFromEncoding(bytes, byteCount, this);
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x00091094 File Offset: 0x0008F294
		public unsafe virtual int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars)
		{
			fixed (byte* nonNullPinnableReference = MemoryMarshal.GetNonNullPinnableReference<byte>(bytes))
			{
				byte* ptr = nonNullPinnableReference;
				fixed (char* nonNullPinnableReference2 = MemoryMarshal.GetNonNullPinnableReference<char>(chars))
				{
					char* ptr2 = nonNullPinnableReference2;
					return this.GetChars(ptr, bytes.Length, ptr2, chars.Length);
				}
			}
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000910CC File Offset: 0x0008F2CC
		public unsafe string GetString(ReadOnlySpan<byte> bytes)
		{
			fixed (byte* nonNullPinnableReference = MemoryMarshal.GetNonNullPinnableReference<byte>(bytes))
			{
				byte* ptr = nonNullPinnableReference;
				return this.GetString(ptr, bytes.Length);
			}
		}

		/// <summary>When overridden in a derived class, gets the code page identifier of the current <see cref="T:System.Text.Encoding" />.</summary>
		/// <returns>The code page identifier of the current <see cref="T:System.Text.Encoding" />.</returns>
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060027DB RID: 10203 RVA: 0x000910F1 File Offset: 0x0008F2F1
		public virtual int CodePage
		{
			get
			{
				return this.m_codePage;
			}
		}

		/// <summary>Gets a value indicating whether the current encoding is always normalized, using the default normalization form.</summary>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Text.Encoding" /> is always normalized; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x060027DC RID: 10204 RVA: 0x000910F9 File Offset: 0x0008F2F9
		[ComVisible(false)]
		public bool IsAlwaysNormalized()
		{
			return this.IsAlwaysNormalized(NormalizationForm.FormC);
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding is always normalized, using the specified normalization form.</summary>
		/// <param name="form">One of the <see cref="T:System.Text.NormalizationForm" /> values.</param>
		/// <returns>
		///   <see langword="true" /> if the current <see cref="T:System.Text.Encoding" /> object is always normalized using the specified <see cref="T:System.Text.NormalizationForm" /> value; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x060027DD RID: 10205 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[ComVisible(false)]
		public virtual bool IsAlwaysNormalized(NormalizationForm form)
		{
			return false;
		}

		/// <summary>When overridden in a derived class, obtains a decoder that converts an encoded sequence of bytes into a sequence of characters.</summary>
		/// <returns>A <see cref="T:System.Text.Decoder" /> that converts an encoded sequence of bytes into a sequence of characters.</returns>
		// Token: 0x060027DE RID: 10206 RVA: 0x00091102 File Offset: 0x0008F302
		public virtual Decoder GetDecoder()
		{
			return new Encoding.DefaultDecoder(this);
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x0009110A File Offset: 0x0008F30A
		[SecurityCritical]
		private static Encoding CreateDefaultEncoding()
		{
			Encoding encoding = EncodingHelper.GetDefaultEncoding();
			encoding.m_isReadOnly = true;
			return encoding;
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x00091118 File Offset: 0x0008F318
		internal void setReadOnly(bool value = true)
		{
			this.m_isReadOnly = value;
		}

		/// <summary>Gets the default encoding for this .NET implementation.</summary>
		/// <returns>The default encoding for this .NET implementation.</returns>
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060027E1 RID: 10209 RVA: 0x00091121 File Offset: 0x0008F321
		public static Encoding Default
		{
			[SecuritySafeCritical]
			get
			{
				if (Encoding.defaultEncoding == null)
				{
					Encoding.defaultEncoding = Encoding.CreateDefaultEncoding();
				}
				return Encoding.defaultEncoding;
			}
		}

		/// <summary>When overridden in a derived class, obtains an encoder that converts a sequence of Unicode characters into an encoded sequence of bytes.</summary>
		/// <returns>A <see cref="T:System.Text.Encoder" /> that converts a sequence of Unicode characters into an encoded sequence of bytes.</returns>
		// Token: 0x060027E2 RID: 10210 RVA: 0x0009113F File Offset: 0x0008F33F
		public virtual Encoder GetEncoder()
		{
			return new Encoding.DefaultEncoder(this);
		}

		/// <summary>When overridden in a derived class, calculates the maximum number of bytes produced by encoding the specified number of characters.</summary>
		/// <param name="charCount">The number of characters to encode.</param>
		/// <returns>The maximum number of bytes produced by encoding the specified number of characters.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charCount" /> is less than zero.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		// Token: 0x060027E3 RID: 10211
		public abstract int GetMaxByteCount(int charCount);

		/// <summary>When overridden in a derived class, calculates the maximum number of characters produced by decoding the specified number of bytes.</summary>
		/// <param name="byteCount">The number of bytes to decode.</param>
		/// <returns>The maximum number of characters produced by decoding the specified number of bytes.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteCount" /> is less than zero.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		// Token: 0x060027E4 RID: 10212
		public abstract int GetMaxCharCount(int byteCount);

		/// <summary>When overridden in a derived class, decodes all the bytes in the specified byte array into a string.</summary>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
		/// <returns>A string that contains the results of decoding the specified sequence of bytes.</returns>
		/// <exception cref="T:System.ArgumentException">The byte array contains invalid Unicode code points.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		// Token: 0x060027E5 RID: 10213 RVA: 0x00091147 File Offset: 0x0008F347
		public virtual string GetString(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("Array cannot be null."));
			}
			return this.GetString(bytes, 0, bytes.Length);
		}

		/// <summary>When overridden in a derived class, decodes a sequence of bytes from the specified byte array into a string.</summary>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
		/// <param name="index">The index of the first byte to decode.</param>
		/// <param name="count">The number of bytes to decode.</param>
		/// <returns>A string that contains the results of decoding the specified sequence of bytes.</returns>
		/// <exception cref="T:System.ArgumentException">The byte array contains invalid Unicode code points.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.  
		/// -or-  
		/// <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
		///  -and-  
		///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		// Token: 0x060027E6 RID: 10214 RVA: 0x0009116C File Offset: 0x0008F36C
		public virtual string GetString(byte[] bytes, int index, int count)
		{
			return new string(this.GetChars(bytes, index, count));
		}

		/// <summary>Gets an encoding for the UTF-16 format using the little endian byte order.</summary>
		/// <returns>An encoding for the UTF-16 format using the little endian byte order.</returns>
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060027E7 RID: 10215 RVA: 0x0009117C File Offset: 0x0008F37C
		public static Encoding Unicode
		{
			get
			{
				if (Encoding.unicodeEncoding == null)
				{
					Encoding.unicodeEncoding = new UnicodeEncoding(false, true);
				}
				return Encoding.unicodeEncoding;
			}
		}

		/// <summary>Gets an encoding for the UTF-16 format that uses the big endian byte order.</summary>
		/// <returns>An encoding object for the UTF-16 format that uses the big endian byte order.</returns>
		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060027E8 RID: 10216 RVA: 0x0009119C File Offset: 0x0008F39C
		public static Encoding BigEndianUnicode
		{
			get
			{
				if (Encoding.bigEndianUnicode == null)
				{
					Encoding.bigEndianUnicode = new UnicodeEncoding(true, true);
				}
				return Encoding.bigEndianUnicode;
			}
		}

		/// <summary>Gets an encoding for the UTF-7 format.</summary>
		/// <returns>An encoding for the UTF-7 format.</returns>
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060027E9 RID: 10217 RVA: 0x000911BC File Offset: 0x0008F3BC
		public static Encoding UTF7
		{
			get
			{
				if (Encoding.utf7Encoding == null)
				{
					Encoding.utf7Encoding = new UTF7Encoding();
				}
				return Encoding.utf7Encoding;
			}
		}

		/// <summary>Gets an encoding for the UTF-8 format.</summary>
		/// <returns>An encoding for the UTF-8 format.</returns>
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060027EA RID: 10218 RVA: 0x000911DA File Offset: 0x0008F3DA
		public static Encoding UTF8
		{
			get
			{
				if (Encoding.utf8Encoding == null)
				{
					Encoding.utf8Encoding = new UTF8Encoding(true);
				}
				return Encoding.utf8Encoding;
			}
		}

		/// <summary>Gets an encoding for the UTF-32 format using the little endian byte order.</summary>
		/// <returns>An  encoding object for the UTF-32 format using the little endian byte order.</returns>
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060027EB RID: 10219 RVA: 0x000911F9 File Offset: 0x0008F3F9
		public static Encoding UTF32
		{
			get
			{
				if (Encoding.utf32Encoding == null)
				{
					Encoding.utf32Encoding = new UTF32Encoding(false, true);
				}
				return Encoding.utf32Encoding;
			}
		}

		/// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current instance.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to compare with the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="value" /> is an instance of <see cref="T:System.Text.Encoding" /> and is equal to the current instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x060027EC RID: 10220 RVA: 0x0009121C File Offset: 0x0008F41C
		public override bool Equals(object value)
		{
			Encoding encoding = value as Encoding;
			return encoding != null && (this.m_codePage == encoding.m_codePage && this.EncoderFallback.Equals(encoding.EncoderFallback)) && this.DecoderFallback.Equals(encoding.DecoderFallback);
		}

		/// <summary>Returns the hash code for the current instance.</summary>
		/// <returns>The hash code for the current instance.</returns>
		// Token: 0x060027ED RID: 10221 RVA: 0x00091269 File Offset: 0x0008F469
		public override int GetHashCode()
		{
			return this.m_codePage + this.EncoderFallback.GetHashCode() + this.DecoderFallback.GetHashCode();
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x00091289 File Offset: 0x0008F489
		internal virtual char[] GetBestFitUnicodeToBytesData()
		{
			return EmptyArray<char>.Value;
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x00091289 File Offset: 0x0008F489
		internal virtual char[] GetBestFitBytesToUnicodeData()
		{
			return EmptyArray<char>.Value;
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x00091290 File Offset: 0x0008F490
		internal void ThrowBytesOverflow()
		{
			throw new ArgumentException(Environment.GetResourceString("The output byte buffer is too small to contain the encoded data, encoding '{0}' fallback '{1}'.", new object[]
			{
				this.EncodingName,
				this.EncoderFallback.GetType()
			}), "bytes");
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000912C3 File Offset: 0x0008F4C3
		[SecurityCritical]
		internal void ThrowBytesOverflow(EncoderNLS encoder, bool nothingEncoded)
		{
			if (encoder == null || encoder._throwOnOverflow || nothingEncoded)
			{
				if (encoder != null && encoder.InternalHasFallbackBuffer)
				{
					encoder.FallbackBuffer.InternalReset();
				}
				this.ThrowBytesOverflow();
			}
			encoder.ClearMustFlush();
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000912F7 File Offset: 0x0008F4F7
		internal void ThrowCharsOverflow()
		{
			throw new ArgumentException(Environment.GetResourceString("The output char buffer is too small to contain the decoded characters, encoding '{0}' fallback '{1}'.", new object[]
			{
				this.EncodingName,
				this.DecoderFallback.GetType()
			}), "chars");
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x0009132A File Offset: 0x0008F52A
		[SecurityCritical]
		internal void ThrowCharsOverflow(DecoderNLS decoder, bool nothingDecoded)
		{
			if (decoder == null || decoder._throwOnOverflow || nothingDecoded)
			{
				if (decoder != null && decoder.InternalHasFallbackBuffer)
				{
					decoder.FallbackBuffer.InternalReset();
				}
				this.ThrowCharsOverflow();
			}
			decoder.ClearMustFlush();
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x00091360 File Offset: 0x0008F560
		public unsafe virtual int GetCharCount(ReadOnlySpan<byte> bytes)
		{
			fixed (byte* nonNullPinnableReference = MemoryMarshal.GetNonNullPinnableReference<byte>(bytes))
			{
				byte* ptr = nonNullPinnableReference;
				return this.GetCharCount(ptr, bytes.Length);
			}
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x00091388 File Offset: 0x0008F588
		public unsafe virtual int GetByteCount(ReadOnlySpan<char> chars)
		{
			fixed (char* nonNullPinnableReference = MemoryMarshal.GetNonNullPinnableReference<char>(chars))
			{
				char* ptr = nonNullPinnableReference;
				return this.GetByteCount(ptr, chars.Length);
			}
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x000913B0 File Offset: 0x0008F5B0
		public unsafe virtual int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
		{
			fixed (char* nonNullPinnableReference = MemoryMarshal.GetNonNullPinnableReference<char>(chars))
			{
				char* ptr = nonNullPinnableReference;
				fixed (byte* nonNullPinnableReference2 = MemoryMarshal.GetNonNullPinnableReference<byte>(bytes))
				{
					byte* ptr2 = nonNullPinnableReference2;
					return this.GetBytes(ptr, chars.Length, ptr2, bytes.Length);
				}
			}
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x000913E8 File Offset: 0x0008F5E8
		public unsafe byte[] GetBytes(string s, int index, int count)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s", "String reference not set to an instance of a String.");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (index > s.Length - count)
			{
				throw new ArgumentOutOfRangeException("index", "Index and count must refer to a location within the string.");
			}
			char* ptr = s;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			int byteCount = this.GetByteCount(ptr + index, count);
			if (byteCount == 0)
			{
				return Array.Empty<byte>();
			}
			byte[] array = new byte[byteCount];
			fixed (byte* ptr2 = &array[0])
			{
				byte* ptr3 = ptr2;
				this.GetBytes(ptr + index, count, ptr3, byteCount);
			}
			return array;
		}

		// Token: 0x04001E14 RID: 7700
		private static volatile Encoding defaultEncoding;

		// Token: 0x04001E15 RID: 7701
		private static volatile Encoding unicodeEncoding;

		// Token: 0x04001E16 RID: 7702
		private static volatile Encoding bigEndianUnicode;

		// Token: 0x04001E17 RID: 7703
		private static volatile Encoding utf7Encoding;

		// Token: 0x04001E18 RID: 7704
		private static volatile Encoding utf8Encoding;

		// Token: 0x04001E19 RID: 7705
		private static volatile Encoding utf32Encoding;

		// Token: 0x04001E1A RID: 7706
		private static volatile Encoding asciiEncoding;

		// Token: 0x04001E1B RID: 7707
		private static volatile Encoding latin1Encoding;

		// Token: 0x04001E1C RID: 7708
		private static volatile Dictionary<int, Encoding> encodings;

		// Token: 0x04001E1D RID: 7709
		private const int MIMECONTF_MAILNEWS = 1;

		// Token: 0x04001E1E RID: 7710
		private const int MIMECONTF_BROWSER = 2;

		// Token: 0x04001E1F RID: 7711
		private const int MIMECONTF_SAVABLE_MAILNEWS = 256;

		// Token: 0x04001E20 RID: 7712
		private const int MIMECONTF_SAVABLE_BROWSER = 512;

		// Token: 0x04001E21 RID: 7713
		private const int CodePageDefault = 0;

		// Token: 0x04001E22 RID: 7714
		private const int CodePageNoOEM = 1;

		// Token: 0x04001E23 RID: 7715
		private const int CodePageNoMac = 2;

		// Token: 0x04001E24 RID: 7716
		private const int CodePageNoThread = 3;

		// Token: 0x04001E25 RID: 7717
		private const int CodePageNoSymbol = 42;

		// Token: 0x04001E26 RID: 7718
		private const int CodePageUnicode = 1200;

		// Token: 0x04001E27 RID: 7719
		private const int CodePageBigEndian = 1201;

		// Token: 0x04001E28 RID: 7720
		private const int CodePageWindows1252 = 1252;

		// Token: 0x04001E29 RID: 7721
		private const int CodePageMacGB2312 = 10008;

		// Token: 0x04001E2A RID: 7722
		private const int CodePageGB2312 = 20936;

		// Token: 0x04001E2B RID: 7723
		private const int CodePageMacKorean = 10003;

		// Token: 0x04001E2C RID: 7724
		private const int CodePageDLLKorean = 20949;

		// Token: 0x04001E2D RID: 7725
		private const int ISO2022JP = 50220;

		// Token: 0x04001E2E RID: 7726
		private const int ISO2022JPESC = 50221;

		// Token: 0x04001E2F RID: 7727
		private const int ISO2022JPSISO = 50222;

		// Token: 0x04001E30 RID: 7728
		private const int ISOKorean = 50225;

		// Token: 0x04001E31 RID: 7729
		private const int ISOSimplifiedCN = 50227;

		// Token: 0x04001E32 RID: 7730
		private const int EUCJP = 51932;

		// Token: 0x04001E33 RID: 7731
		private const int ChineseHZ = 52936;

		// Token: 0x04001E34 RID: 7732
		private const int DuplicateEUCCN = 51936;

		// Token: 0x04001E35 RID: 7733
		private const int EUCCN = 936;

		// Token: 0x04001E36 RID: 7734
		private const int EUCKR = 51949;

		// Token: 0x04001E37 RID: 7735
		internal const int CodePageASCII = 20127;

		// Token: 0x04001E38 RID: 7736
		internal const int ISO_8859_1 = 28591;

		// Token: 0x04001E39 RID: 7737
		private const int ISCIIAssemese = 57006;

		// Token: 0x04001E3A RID: 7738
		private const int ISCIIBengali = 57003;

		// Token: 0x04001E3B RID: 7739
		private const int ISCIIDevanagari = 57002;

		// Token: 0x04001E3C RID: 7740
		private const int ISCIIGujarathi = 57010;

		// Token: 0x04001E3D RID: 7741
		private const int ISCIIKannada = 57008;

		// Token: 0x04001E3E RID: 7742
		private const int ISCIIMalayalam = 57009;

		// Token: 0x04001E3F RID: 7743
		private const int ISCIIOriya = 57007;

		// Token: 0x04001E40 RID: 7744
		private const int ISCIIPanjabi = 57011;

		// Token: 0x04001E41 RID: 7745
		private const int ISCIITamil = 57004;

		// Token: 0x04001E42 RID: 7746
		private const int ISCIITelugu = 57005;

		// Token: 0x04001E43 RID: 7747
		private const int GB18030 = 54936;

		// Token: 0x04001E44 RID: 7748
		private const int ISO_8859_8I = 38598;

		// Token: 0x04001E45 RID: 7749
		private const int ISO_8859_8_Visual = 28598;

		// Token: 0x04001E46 RID: 7750
		private const int ENC50229 = 50229;

		// Token: 0x04001E47 RID: 7751
		private const int CodePageUTF7 = 65000;

		// Token: 0x04001E48 RID: 7752
		private const int CodePageUTF8 = 65001;

		// Token: 0x04001E49 RID: 7753
		private const int CodePageUTF32 = 12000;

		// Token: 0x04001E4A RID: 7754
		private const int CodePageUTF32BE = 12001;

		// Token: 0x04001E4B RID: 7755
		internal int m_codePage;

		// Token: 0x04001E4C RID: 7756
		internal CodePageDataItem dataItem;

		// Token: 0x04001E4D RID: 7757
		[NonSerialized]
		internal bool m_deserializedFromEverett;

		// Token: 0x04001E4E RID: 7758
		[OptionalField(VersionAdded = 2)]
		private bool m_isReadOnly;

		// Token: 0x04001E4F RID: 7759
		[OptionalField(VersionAdded = 2)]
		internal EncoderFallback encoderFallback;

		// Token: 0x04001E50 RID: 7760
		[OptionalField(VersionAdded = 2)]
		internal DecoderFallback decoderFallback;

		// Token: 0x04001E51 RID: 7761
		private static object s_InternalSyncObject;

		// Token: 0x020003BD RID: 957
		[Serializable]
		internal class DefaultEncoder : Encoder, ISerializable, IObjectReference
		{
			// Token: 0x060027F8 RID: 10232 RVA: 0x00091499 File Offset: 0x0008F699
			public DefaultEncoder(Encoding encoding)
			{
				this.m_encoding = encoding;
				this.m_hasInitializedEncoding = true;
			}

			// Token: 0x060027F9 RID: 10233 RVA: 0x000914B0 File Offset: 0x0008F6B0
			internal DefaultEncoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
				try
				{
					this._fallback = (EncoderFallback)info.GetValue("_fallback", typeof(EncoderFallback));
					this.charLeftOver = (char)info.GetValue("charLeftOver", typeof(char));
				}
				catch (SerializationException)
				{
				}
			}

			// Token: 0x060027FA RID: 10234 RVA: 0x00091548 File Offset: 0x0008F748
			[SecurityCritical]
			public object GetRealObject(StreamingContext context)
			{
				if (this.m_hasInitializedEncoding)
				{
					return this;
				}
				Encoder encoder = this.m_encoding.GetEncoder();
				if (this._fallback != null)
				{
					encoder._fallback = this._fallback;
				}
				if (this.charLeftOver != '\0')
				{
					EncoderNLS encoderNLS = encoder as EncoderNLS;
					if (encoderNLS != null)
					{
						encoderNLS._charLeftOver = this.charLeftOver;
					}
				}
				return encoder;
			}

			// Token: 0x060027FB RID: 10235 RVA: 0x0009159E File Offset: 0x0008F79E
			[SecurityCritical]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
			}

			// Token: 0x060027FC RID: 10236 RVA: 0x000915BF File Offset: 0x0008F7BF
			public override int GetByteCount(char[] chars, int index, int count, bool flush)
			{
				return this.m_encoding.GetByteCount(chars, index, count);
			}

			// Token: 0x060027FD RID: 10237 RVA: 0x000915CF File Offset: 0x0008F7CF
			[SecurityCritical]
			public unsafe override int GetByteCount(char* chars, int count, bool flush)
			{
				return this.m_encoding.GetByteCount(chars, count);
			}

			// Token: 0x060027FE RID: 10238 RVA: 0x000915DE File Offset: 0x0008F7DE
			public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
			{
				return this.m_encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
			}

			// Token: 0x060027FF RID: 10239 RVA: 0x000915F2 File Offset: 0x0008F7F2
			[SecurityCritical]
			public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, bool flush)
			{
				return this.m_encoding.GetBytes(chars, charCount, bytes, byteCount);
			}

			// Token: 0x04001E52 RID: 7762
			private Encoding m_encoding;

			// Token: 0x04001E53 RID: 7763
			[NonSerialized]
			private bool m_hasInitializedEncoding;

			// Token: 0x04001E54 RID: 7764
			[NonSerialized]
			internal char charLeftOver;
		}

		// Token: 0x020003BE RID: 958
		[Serializable]
		internal class DefaultDecoder : Decoder, ISerializable, IObjectReference
		{
			// Token: 0x06002800 RID: 10240 RVA: 0x00091604 File Offset: 0x0008F804
			public DefaultDecoder(Encoding encoding)
			{
				this.m_encoding = encoding;
				this.m_hasInitializedEncoding = true;
			}

			// Token: 0x06002801 RID: 10241 RVA: 0x0009161C File Offset: 0x0008F81C
			internal DefaultDecoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
				try
				{
					this._fallback = (DecoderFallback)info.GetValue("_fallback", typeof(DecoderFallback));
				}
				catch (SerializationException)
				{
					this._fallback = null;
				}
			}

			// Token: 0x06002802 RID: 10242 RVA: 0x0009169C File Offset: 0x0008F89C
			[SecurityCritical]
			public object GetRealObject(StreamingContext context)
			{
				if (this.m_hasInitializedEncoding)
				{
					return this;
				}
				Decoder decoder = this.m_encoding.GetDecoder();
				if (this._fallback != null)
				{
					decoder._fallback = this._fallback;
				}
				return decoder;
			}

			// Token: 0x06002803 RID: 10243 RVA: 0x000916D4 File Offset: 0x0008F8D4
			[SecurityCritical]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
			}

			// Token: 0x06002804 RID: 10244 RVA: 0x0008628A File Offset: 0x0008448A
			public override int GetCharCount(byte[] bytes, int index, int count)
			{
				return this.GetCharCount(bytes, index, count, false);
			}

			// Token: 0x06002805 RID: 10245 RVA: 0x000916F5 File Offset: 0x0008F8F5
			public override int GetCharCount(byte[] bytes, int index, int count, bool flush)
			{
				return this.m_encoding.GetCharCount(bytes, index, count);
			}

			// Token: 0x06002806 RID: 10246 RVA: 0x00091705 File Offset: 0x0008F905
			[SecurityCritical]
			public unsafe override int GetCharCount(byte* bytes, int count, bool flush)
			{
				return this.m_encoding.GetCharCount(bytes, count);
			}

			// Token: 0x06002807 RID: 10247 RVA: 0x00086362 File Offset: 0x00084562
			public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
			{
				return this.GetChars(bytes, byteIndex, byteCount, chars, charIndex, false);
			}

			// Token: 0x06002808 RID: 10248 RVA: 0x00091714 File Offset: 0x0008F914
			public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, bool flush)
			{
				return this.m_encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
			}

			// Token: 0x06002809 RID: 10249 RVA: 0x00091728 File Offset: 0x0008F928
			[SecurityCritical]
			public unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount, bool flush)
			{
				return this.m_encoding.GetChars(bytes, byteCount, chars, charCount);
			}

			// Token: 0x04001E55 RID: 7765
			private Encoding m_encoding;

			// Token: 0x04001E56 RID: 7766
			[NonSerialized]
			private bool m_hasInitializedEncoding;
		}

		// Token: 0x020003BF RID: 959
		internal class EncodingCharBuffer
		{
			// Token: 0x0600280A RID: 10250 RVA: 0x0009173C File Offset: 0x0008F93C
			[SecurityCritical]
			internal unsafe EncodingCharBuffer(Encoding enc, DecoderNLS decoder, char* charStart, int charCount, byte* byteStart, int byteCount)
			{
				this.enc = enc;
				this.decoder = decoder;
				this.chars = charStart;
				this.charStart = charStart;
				this.charEnd = charStart + charCount;
				this.byteStart = byteStart;
				this.bytes = byteStart;
				this.byteEnd = byteStart + byteCount;
				if (this.decoder == null)
				{
					this.fallbackBuffer = enc.DecoderFallback.CreateFallbackBuffer();
				}
				else
				{
					this.fallbackBuffer = this.decoder.FallbackBuffer;
				}
				this.fallbackBuffer.InternalInitialize(this.bytes, this.charEnd);
			}

			// Token: 0x0600280B RID: 10251 RVA: 0x000917D8 File Offset: 0x0008F9D8
			[SecurityCritical]
			internal unsafe bool AddChar(char ch, int numBytes)
			{
				if (this.chars != null)
				{
					if (this.chars >= this.charEnd)
					{
						this.bytes -= numBytes;
						this.enc.ThrowCharsOverflow(this.decoder, this.bytes == this.byteStart);
						return false;
					}
					char* ptr = this.chars;
					this.chars = ptr + 1;
					*ptr = ch;
				}
				this.charCountResult++;
				return true;
			}

			// Token: 0x0600280C RID: 10252 RVA: 0x00091851 File Offset: 0x0008FA51
			[SecurityCritical]
			internal bool AddChar(char ch)
			{
				return this.AddChar(ch, 1);
			}

			// Token: 0x0600280D RID: 10253 RVA: 0x0009185C File Offset: 0x0008FA5C
			[SecurityCritical]
			internal bool AddChar(char ch1, char ch2, int numBytes)
			{
				if (this.chars >= this.charEnd - 1)
				{
					this.bytes -= numBytes;
					this.enc.ThrowCharsOverflow(this.decoder, this.bytes == this.byteStart);
					return false;
				}
				return this.AddChar(ch1, numBytes) && this.AddChar(ch2, numBytes);
			}

			// Token: 0x0600280E RID: 10254 RVA: 0x000918BF File Offset: 0x0008FABF
			[SecurityCritical]
			internal void AdjustBytes(int count)
			{
				this.bytes += count;
			}

			// Token: 0x170004E4 RID: 1252
			// (get) Token: 0x0600280F RID: 10255 RVA: 0x000918CF File Offset: 0x0008FACF
			internal bool MoreData
			{
				[SecurityCritical]
				get
				{
					return this.bytes < this.byteEnd;
				}
			}

			// Token: 0x06002810 RID: 10256 RVA: 0x000918DF File Offset: 0x0008FADF
			[SecurityCritical]
			internal bool EvenMoreData(int count)
			{
				return this.bytes == this.byteEnd - count;
			}

			// Token: 0x06002811 RID: 10257 RVA: 0x000918F4 File Offset: 0x0008FAF4
			[SecurityCritical]
			internal unsafe byte GetNextByte()
			{
				if (this.bytes >= this.byteEnd)
				{
					return 0;
				}
				byte* ptr = this.bytes;
				this.bytes = ptr + 1;
				return *ptr;
			}

			// Token: 0x170004E5 RID: 1253
			// (get) Token: 0x06002812 RID: 10258 RVA: 0x00091923 File Offset: 0x0008FB23
			internal int BytesUsed
			{
				[SecurityCritical]
				get
				{
					return (int)((long)(this.bytes - this.byteStart));
				}
			}

			// Token: 0x06002813 RID: 10259 RVA: 0x00091938 File Offset: 0x0008FB38
			[SecurityCritical]
			internal bool Fallback(byte fallbackByte)
			{
				byte[] array = new byte[] { fallbackByte };
				return this.Fallback(array);
			}

			// Token: 0x06002814 RID: 10260 RVA: 0x00091958 File Offset: 0x0008FB58
			[SecurityCritical]
			internal bool Fallback(byte byte1, byte byte2)
			{
				byte[] array = new byte[] { byte1, byte2 };
				return this.Fallback(array);
			}

			// Token: 0x06002815 RID: 10261 RVA: 0x0009197C File Offset: 0x0008FB7C
			[SecurityCritical]
			internal bool Fallback(byte byte1, byte byte2, byte byte3, byte byte4)
			{
				byte[] array = new byte[] { byte1, byte2, byte3, byte4 };
				return this.Fallback(array);
			}

			// Token: 0x06002816 RID: 10262 RVA: 0x000919A8 File Offset: 0x0008FBA8
			[SecurityCritical]
			internal unsafe bool Fallback(byte[] byteBuffer)
			{
				if (this.chars != null)
				{
					char* ptr = this.chars;
					if (!this.fallbackBuffer.InternalFallback(byteBuffer, this.bytes, ref this.chars))
					{
						this.bytes -= byteBuffer.Length;
						this.fallbackBuffer.InternalReset();
						this.enc.ThrowCharsOverflow(this.decoder, this.chars == this.charStart);
						return false;
					}
					this.charCountResult += (int)((long)(this.chars - ptr));
				}
				else
				{
					this.charCountResult += this.fallbackBuffer.InternalFallback(byteBuffer, this.bytes);
				}
				return true;
			}

			// Token: 0x170004E6 RID: 1254
			// (get) Token: 0x06002817 RID: 10263 RVA: 0x00091A57 File Offset: 0x0008FC57
			internal int Count
			{
				get
				{
					return this.charCountResult;
				}
			}

			// Token: 0x04001E57 RID: 7767
			[SecurityCritical]
			private unsafe char* chars;

			// Token: 0x04001E58 RID: 7768
			[SecurityCritical]
			private unsafe char* charStart;

			// Token: 0x04001E59 RID: 7769
			[SecurityCritical]
			private unsafe char* charEnd;

			// Token: 0x04001E5A RID: 7770
			private int charCountResult;

			// Token: 0x04001E5B RID: 7771
			private Encoding enc;

			// Token: 0x04001E5C RID: 7772
			private DecoderNLS decoder;

			// Token: 0x04001E5D RID: 7773
			[SecurityCritical]
			private unsafe byte* byteStart;

			// Token: 0x04001E5E RID: 7774
			[SecurityCritical]
			private unsafe byte* byteEnd;

			// Token: 0x04001E5F RID: 7775
			[SecurityCritical]
			private unsafe byte* bytes;

			// Token: 0x04001E60 RID: 7776
			private DecoderFallbackBuffer fallbackBuffer;
		}

		// Token: 0x020003C0 RID: 960
		internal class EncodingByteBuffer
		{
			// Token: 0x06002818 RID: 10264 RVA: 0x00091A60 File Offset: 0x0008FC60
			[SecurityCritical]
			internal unsafe EncodingByteBuffer(Encoding inEncoding, EncoderNLS inEncoder, byte* inByteStart, int inByteCount, char* inCharStart, int inCharCount)
			{
				this.enc = inEncoding;
				this.encoder = inEncoder;
				this.charStart = inCharStart;
				this.chars = inCharStart;
				this.charEnd = inCharStart + inCharCount;
				this.bytes = inByteStart;
				this.byteStart = inByteStart;
				this.byteEnd = inByteStart + inByteCount;
				if (this.encoder == null)
				{
					this.fallbackBuffer = this.enc.EncoderFallback.CreateFallbackBuffer();
				}
				else
				{
					this.fallbackBuffer = this.encoder.FallbackBuffer;
					if (this.encoder._throwOnOverflow && this.encoder.InternalHasFallbackBuffer && this.fallbackBuffer.Remaining > 0)
					{
						throw new ArgumentException(Environment.GetResourceString("Must complete Convert() operation or call Encoder.Reset() before calling GetBytes() or GetByteCount(). Encoder '{0}' fallback '{1}'.", new object[]
						{
							this.encoder.Encoding.EncodingName,
							this.encoder.Fallback.GetType()
						}));
					}
				}
				this.fallbackBuffer.InternalInitialize(this.chars, this.charEnd, this.encoder, this.bytes != null);
			}

			// Token: 0x06002819 RID: 10265 RVA: 0x00091B78 File Offset: 0x0008FD78
			[SecurityCritical]
			internal unsafe bool AddByte(byte b, int moreBytesExpected)
			{
				if (this.bytes != null)
				{
					if (this.bytes >= this.byteEnd - moreBytesExpected)
					{
						this.MovePrevious(true);
						return false;
					}
					byte* ptr = this.bytes;
					this.bytes = ptr + 1;
					*ptr = b;
				}
				this.byteCountResult++;
				return true;
			}

			// Token: 0x0600281A RID: 10266 RVA: 0x00091BCA File Offset: 0x0008FDCA
			[SecurityCritical]
			internal bool AddByte(byte b1)
			{
				return this.AddByte(b1, 0);
			}

			// Token: 0x0600281B RID: 10267 RVA: 0x00091BD4 File Offset: 0x0008FDD4
			[SecurityCritical]
			internal bool AddByte(byte b1, byte b2)
			{
				return this.AddByte(b1, b2, 0);
			}

			// Token: 0x0600281C RID: 10268 RVA: 0x00091BDF File Offset: 0x0008FDDF
			[SecurityCritical]
			internal bool AddByte(byte b1, byte b2, int moreBytesExpected)
			{
				return this.AddByte(b1, 1 + moreBytesExpected) && this.AddByte(b2, moreBytesExpected);
			}

			// Token: 0x0600281D RID: 10269 RVA: 0x00091BF7 File Offset: 0x0008FDF7
			[SecurityCritical]
			internal bool AddByte(byte b1, byte b2, byte b3)
			{
				return this.AddByte(b1, b2, b3, 0);
			}

			// Token: 0x0600281E RID: 10270 RVA: 0x00091C03 File Offset: 0x0008FE03
			[SecurityCritical]
			internal bool AddByte(byte b1, byte b2, byte b3, int moreBytesExpected)
			{
				return this.AddByte(b1, 2 + moreBytesExpected) && this.AddByte(b2, 1 + moreBytesExpected) && this.AddByte(b3, moreBytesExpected);
			}

			// Token: 0x0600281F RID: 10271 RVA: 0x00091C2A File Offset: 0x0008FE2A
			[SecurityCritical]
			internal bool AddByte(byte b1, byte b2, byte b3, byte b4)
			{
				return this.AddByte(b1, 3) && this.AddByte(b2, 2) && this.AddByte(b3, 1) && this.AddByte(b4, 0);
			}

			// Token: 0x06002820 RID: 10272 RVA: 0x00091C58 File Offset: 0x0008FE58
			[SecurityCritical]
			internal void MovePrevious(bool bThrow)
			{
				if (this.fallbackBuffer.bFallingBack)
				{
					this.fallbackBuffer.MovePrevious();
				}
				else if (this.chars != this.charStart)
				{
					this.chars--;
				}
				if (bThrow)
				{
					this.enc.ThrowBytesOverflow(this.encoder, this.bytes == this.byteStart);
				}
			}

			// Token: 0x06002821 RID: 10273 RVA: 0x00091CBE File Offset: 0x0008FEBE
			[SecurityCritical]
			internal bool Fallback(char charFallback)
			{
				return this.fallbackBuffer.InternalFallback(charFallback, ref this.chars);
			}

			// Token: 0x170004E7 RID: 1255
			// (get) Token: 0x06002822 RID: 10274 RVA: 0x00091CD2 File Offset: 0x0008FED2
			internal bool MoreData
			{
				[SecurityCritical]
				get
				{
					return this.fallbackBuffer.Remaining > 0 || this.chars < this.charEnd;
				}
			}

			// Token: 0x06002823 RID: 10275 RVA: 0x00091CF4 File Offset: 0x0008FEF4
			[SecurityCritical]
			internal unsafe char GetNextChar()
			{
				char c = this.fallbackBuffer.InternalGetNextChar();
				if (c == '\0' && this.chars < this.charEnd)
				{
					char* ptr = this.chars;
					this.chars = ptr + 1;
					c = *ptr;
				}
				return c;
			}

			// Token: 0x170004E8 RID: 1256
			// (get) Token: 0x06002824 RID: 10276 RVA: 0x00091D32 File Offset: 0x0008FF32
			internal int CharsUsed
			{
				[SecurityCritical]
				get
				{
					return (int)((long)(this.chars - this.charStart));
				}
			}

			// Token: 0x170004E9 RID: 1257
			// (get) Token: 0x06002825 RID: 10277 RVA: 0x00091D45 File Offset: 0x0008FF45
			internal int Count
			{
				get
				{
					return this.byteCountResult;
				}
			}

			// Token: 0x04001E61 RID: 7777
			[SecurityCritical]
			private unsafe byte* bytes;

			// Token: 0x04001E62 RID: 7778
			[SecurityCritical]
			private unsafe byte* byteStart;

			// Token: 0x04001E63 RID: 7779
			[SecurityCritical]
			private unsafe byte* byteEnd;

			// Token: 0x04001E64 RID: 7780
			[SecurityCritical]
			private unsafe char* chars;

			// Token: 0x04001E65 RID: 7781
			[SecurityCritical]
			private unsafe char* charStart;

			// Token: 0x04001E66 RID: 7782
			[SecurityCritical]
			private unsafe char* charEnd;

			// Token: 0x04001E67 RID: 7783
			private int byteCountResult;

			// Token: 0x04001E68 RID: 7784
			private Encoding enc;

			// Token: 0x04001E69 RID: 7785
			private EncoderNLS encoder;

			// Token: 0x04001E6A RID: 7786
			internal EncoderFallbackBuffer fallbackBuffer;
		}
	}
}
