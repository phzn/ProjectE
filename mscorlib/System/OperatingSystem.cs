using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	/// <summary>Represents information about an operating system, such as the version and platform identifier. This class cannot be inherited.</summary>
	// Token: 0x020001CC RID: 460
	[Serializable]
	public sealed class OperatingSystem : ISerializable, ICloneable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.OperatingSystem" /> class, using the specified platform identifier value and version object.</summary>
		/// <param name="platform">One of the <see cref="T:System.PlatformID" /> values that indicates the operating system platform.</param>
		/// <param name="version">A <see cref="T:System.Version" /> object that indicates the version of the operating system.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="version" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="platform" /> is not a <see cref="T:System.PlatformID" /> enumeration value.</exception>
		// Token: 0x060013BC RID: 5052 RVA: 0x0004E838 File Offset: 0x0004CA38
		public OperatingSystem(PlatformID platform, Version version)
			: this(platform, version, null)
		{
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0004E844 File Offset: 0x0004CA44
		internal OperatingSystem(PlatformID platform, Version version, string servicePack)
		{
			if (platform < PlatformID.Win32S || platform > PlatformID.MacOSX)
			{
				throw new ArgumentOutOfRangeException("platform", platform, SR.Format("Illegal enum value: {0}.", platform));
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this._platform = platform;
			this._version = version;
			this._servicePack = servicePack;
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data necessary to deserialize this instance.</summary>
		/// <param name="info">The object to populate with serialization information.</param>
		/// <param name="context">The place to store and retrieve serialized data. Reserved for future use.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is <see langword="null" />.</exception>
		// Token: 0x060013BE RID: 5054 RVA: 0x0001B98F File Offset: 0x00019B8F
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new PlatformNotSupportedException();
		}

		/// <summary>Gets a <see cref="T:System.PlatformID" /> enumeration value that identifies the operating system platform.</summary>
		/// <returns>One of the <see cref="T:System.PlatformID" /> values.</returns>
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x0004E8A9 File Offset: 0x0004CAA9
		public PlatformID Platform
		{
			get
			{
				return this._platform;
			}
		}

		/// <summary>Gets the service pack version represented by this <see cref="T:System.OperatingSystem" /> object.</summary>
		/// <returns>The service pack version, if service packs are supported and at least one is installed; otherwise, an empty string ("").</returns>
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0004E8B1 File Offset: 0x0004CAB1
		public string ServicePack
		{
			get
			{
				return this._servicePack ?? string.Empty;
			}
		}

		/// <summary>Gets a <see cref="T:System.Version" /> object that identifies the operating system.</summary>
		/// <returns>A <see cref="T:System.Version" /> object that describes the major version, minor version, build, and revision numbers for the operating system.</returns>
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x0004E8C2 File Offset: 0x0004CAC2
		public Version Version
		{
			get
			{
				return this._version;
			}
		}

		/// <summary>Creates an <see cref="T:System.OperatingSystem" /> object that is identical to this instance.</summary>
		/// <returns>An <see cref="T:System.OperatingSystem" /> object that is a copy of this instance.</returns>
		// Token: 0x060013C2 RID: 5058 RVA: 0x0004E8CA File Offset: 0x0004CACA
		public object Clone()
		{
			return new OperatingSystem(this._platform, this._version, this._servicePack);
		}

		/// <summary>Converts the value of this <see cref="T:System.OperatingSystem" /> object to its equivalent string representation.</summary>
		/// <returns>The string representation of the values returned by the <see cref="P:System.OperatingSystem.Platform" />, <see cref="P:System.OperatingSystem.Version" />, and <see cref="P:System.OperatingSystem.ServicePack" /> properties.</returns>
		// Token: 0x060013C3 RID: 5059 RVA: 0x0004E8E3 File Offset: 0x0004CAE3
		public override string ToString()
		{
			return this.VersionString;
		}

		/// <summary>Gets the concatenated string representation of the platform identifier, version, and service pack that are currently installed on the operating system.</summary>
		/// <returns>The string representation of the values returned by the <see cref="P:System.OperatingSystem.Platform" />, <see cref="P:System.OperatingSystem.Version" />, and <see cref="P:System.OperatingSystem.ServicePack" /> properties.</returns>
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x0004E8EC File Offset: 0x0004CAEC
		public string VersionString
		{
			get
			{
				if (this._versionString == null)
				{
					string text;
					switch (this._platform)
					{
					case PlatformID.Win32S:
						text = "Microsoft Win32S ";
						break;
					case PlatformID.Win32Windows:
						text = ((this._version.Major > 4 || (this._version.Major == 4 && this._version.Minor > 0)) ? "Microsoft Windows 98 " : "Microsoft Windows 95 ");
						break;
					case PlatformID.Win32NT:
						text = "Microsoft Windows NT ";
						break;
					case PlatformID.WinCE:
						text = "Microsoft Windows CE ";
						break;
					case PlatformID.Unix:
						text = "Unix ";
						break;
					case PlatformID.Xbox:
						text = "Xbox ";
						break;
					case PlatformID.MacOSX:
						text = "Mac OS X ";
						break;
					default:
						text = "<unknown> ";
						break;
					}
					this._versionString = (string.IsNullOrEmpty(this._servicePack) ? (text + this._version.ToString()) : (text + this._version.ToString(3) + " " + this._servicePack));
				}
				return this._versionString;
			}
		}

		// Token: 0x04001455 RID: 5205
		private readonly Version _version;

		// Token: 0x04001456 RID: 5206
		private readonly PlatformID _platform;

		// Token: 0x04001457 RID: 5207
		private readonly string _servicePack;

		// Token: 0x04001458 RID: 5208
		private string _versionString;
	}
}
