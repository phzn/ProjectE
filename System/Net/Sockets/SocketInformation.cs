using System;
using System.Runtime.Serialization;

namespace System.Net.Sockets
{
	/// <summary>Encapsulates the information that is necessary to duplicate a <see cref="T:System.Net.Sockets.Socket" />.</summary>
	// Token: 0x020007B4 RID: 1972
	[Serializable]
	public struct SocketInformation
	{
		/// <summary>Gets or sets the protocol information for a <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An array of type <see cref="T:System.Byte" />.</returns>
		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x06003EC7 RID: 16071 RVA: 0x000D6AC0 File Offset: 0x000D4CC0
		// (set) Token: 0x06003EC8 RID: 16072 RVA: 0x000D6AC8 File Offset: 0x000D4CC8
		public byte[] ProtocolInformation
		{
			get
			{
				return this.protocolInformation;
			}
			set
			{
				this.protocolInformation = value;
			}
		}

		/// <summary>Gets or sets the options for a <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.SocketInformationOptions" /> instance.</returns>
		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06003EC9 RID: 16073 RVA: 0x000D6AD1 File Offset: 0x000D4CD1
		// (set) Token: 0x06003ECA RID: 16074 RVA: 0x000D6AD9 File Offset: 0x000D4CD9
		public SocketInformationOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}

		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06003ECB RID: 16075 RVA: 0x000D6AE2 File Offset: 0x000D4CE2
		// (set) Token: 0x06003ECC RID: 16076 RVA: 0x000D6AEF File Offset: 0x000D4CEF
		internal bool IsNonBlocking
		{
			get
			{
				return (this.options & SocketInformationOptions.NonBlocking) > (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.NonBlocking;
					return;
				}
				this.options &= ~SocketInformationOptions.NonBlocking;
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06003ECD RID: 16077 RVA: 0x000D6B12 File Offset: 0x000D4D12
		// (set) Token: 0x06003ECE RID: 16078 RVA: 0x000D6B1F File Offset: 0x000D4D1F
		internal bool IsConnected
		{
			get
			{
				return (this.options & SocketInformationOptions.Connected) > (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.Connected;
					return;
				}
				this.options &= ~SocketInformationOptions.Connected;
			}
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06003ECF RID: 16079 RVA: 0x000D6B42 File Offset: 0x000D4D42
		// (set) Token: 0x06003ED0 RID: 16080 RVA: 0x000D6B4F File Offset: 0x000D4D4F
		internal bool IsListening
		{
			get
			{
				return (this.options & SocketInformationOptions.Listening) > (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.Listening;
					return;
				}
				this.options &= ~SocketInformationOptions.Listening;
			}
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06003ED1 RID: 16081 RVA: 0x000D6B72 File Offset: 0x000D4D72
		// (set) Token: 0x06003ED2 RID: 16082 RVA: 0x000D6B7F File Offset: 0x000D4D7F
		internal bool UseOnlyOverlappedIO
		{
			get
			{
				return (this.options & SocketInformationOptions.UseOnlyOverlappedIO) > (SocketInformationOptions)0;
			}
			set
			{
				if (value)
				{
					this.options |= SocketInformationOptions.UseOnlyOverlappedIO;
					return;
				}
				this.options &= ~SocketInformationOptions.UseOnlyOverlappedIO;
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x06003ED3 RID: 16083 RVA: 0x000D6BA2 File Offset: 0x000D4DA2
		// (set) Token: 0x06003ED4 RID: 16084 RVA: 0x000D6BAA File Offset: 0x000D4DAA
		internal EndPoint RemoteEndPoint
		{
			get
			{
				return this.remoteEndPoint;
			}
			set
			{
				this.remoteEndPoint = value;
			}
		}

		// Token: 0x0400256E RID: 9582
		private byte[] protocolInformation;

		// Token: 0x0400256F RID: 9583
		private SocketInformationOptions options;

		// Token: 0x04002570 RID: 9584
		[OptionalField]
		private EndPoint remoteEndPoint;
	}
}
