using System;

namespace System.Net.Sockets
{
	/// <summary>Contains <see cref="T:System.Net.IPAddress" /> values used to join and drop multicast groups.</summary>
	// Token: 0x020007AA RID: 1962
	public class MulticastOption
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.MulticastOption" /> class with the specified IP multicast group address and local IP address associated with a network interface.</summary>
		/// <param name="group">The group <see cref="T:System.Net.IPAddress" />.</param>
		/// <param name="mcint">The local <see cref="T:System.Net.IPAddress" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="group" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="mcint" /> is <see langword="null" />.</exception>
		// Token: 0x06003EAB RID: 16043 RVA: 0x000D6810 File Offset: 0x000D4A10
		public MulticastOption(IPAddress group, IPAddress mcint)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			if (mcint == null)
			{
				throw new ArgumentNullException("mcint");
			}
			this.Group = group;
			this.LocalAddress = mcint;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.MulticastOption" /> class with the specified IP multicast group address and interface index.</summary>
		/// <param name="group">The <see cref="T:System.Net.IPAddress" /> of the multicast group.</param>
		/// <param name="interfaceIndex">The index of the interface that is used to send and receive multicast packets.</param>
		// Token: 0x06003EAC RID: 16044 RVA: 0x000D6842 File Offset: 0x000D4A42
		public MulticastOption(IPAddress group, int interfaceIndex)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			if (interfaceIndex < 0 || interfaceIndex > 16777215)
			{
				throw new ArgumentOutOfRangeException("interfaceIndex");
			}
			this.Group = group;
			this.ifIndex = interfaceIndex;
		}

		/// <summary>Initializes a new version of the <see cref="T:System.Net.Sockets.MulticastOption" /> class for the specified IP multicast group.</summary>
		/// <param name="group">The <see cref="T:System.Net.IPAddress" /> of the multicast group.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="group" /> is <see langword="null" />.</exception>
		// Token: 0x06003EAD RID: 16045 RVA: 0x000D687D File Offset: 0x000D4A7D
		public MulticastOption(IPAddress group)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			this.Group = group;
			this.LocalAddress = IPAddress.Any;
		}

		/// <summary>Gets or sets the IP address of a multicast group.</summary>
		/// <returns>An <see cref="T:System.Net.IPAddress" /> that contains the Internet address of a multicast group.</returns>
		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06003EAE RID: 16046 RVA: 0x000D68A5 File Offset: 0x000D4AA5
		// (set) Token: 0x06003EAF RID: 16047 RVA: 0x000D68AD File Offset: 0x000D4AAD
		public IPAddress Group
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
			}
		}

		/// <summary>Gets or sets the local address associated with a multicast group.</summary>
		/// <returns>An <see cref="T:System.Net.IPAddress" /> that contains the local address associated with a multicast group.</returns>
		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x06003EB0 RID: 16048 RVA: 0x000D68B6 File Offset: 0x000D4AB6
		// (set) Token: 0x06003EB1 RID: 16049 RVA: 0x000D68BE File Offset: 0x000D4ABE
		public IPAddress LocalAddress
		{
			get
			{
				return this.localAddress;
			}
			set
			{
				this.ifIndex = 0;
				this.localAddress = value;
			}
		}

		/// <summary>Gets or sets the index of the interface that is used to send and receive multicast packets.</summary>
		/// <returns>An integer that represents the index of a <see cref="T:System.Net.NetworkInformation.NetworkInterface" /> array element.</returns>
		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x06003EB2 RID: 16050 RVA: 0x000D68CE File Offset: 0x000D4ACE
		// (set) Token: 0x06003EB3 RID: 16051 RVA: 0x000D68D6 File Offset: 0x000D4AD6
		public int InterfaceIndex
		{
			get
			{
				return this.ifIndex;
			}
			set
			{
				if (value < 0 || value > 16777215)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.localAddress = null;
				this.ifIndex = value;
			}
		}

		// Token: 0x040024DD RID: 9437
		private IPAddress group;

		// Token: 0x040024DE RID: 9438
		private IPAddress localAddress;

		// Token: 0x040024DF RID: 9439
		private int ifIndex;
	}
}
