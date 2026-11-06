using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	/// <summary>Provides access to information on a drive.</summary>
	// Token: 0x02000B5C RID: 2908
	[ComVisible(true)]
	[Serializable]
	public sealed class DriveInfo : ISerializable
	{
		// Token: 0x0600695C RID: 26972 RVA: 0x00167FAF File Offset: 0x001661AF
		private DriveInfo(string path, string fstype)
		{
			this.drive_format = fstype;
			this.path = path;
		}

		/// <summary>Provides access to information on the specified drive.</summary>
		/// <param name="driveName">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z'. A null value is not valid.</param>
		/// <exception cref="T:System.ArgumentNullException">The drive letter cannot be <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The first letter of <paramref name="driveName" /> is not an uppercase or lowercase letter from 'a' to 'z'.  
		///  -or-  
		///  <paramref name="driveName" /> does not refer to a valid drive.</exception>
		// Token: 0x0600695D RID: 26973 RVA: 0x00167FC8 File Offset: 0x001661C8
		public DriveInfo(string driveName)
		{
			if (!Environment.IsUnix)
			{
				if (driveName == null || driveName.Length == 0)
				{
					throw new ArgumentException("The drive name is null or empty", "driveName");
				}
				if (driveName.Length >= 2 && driveName[1] != ':')
				{
					throw new ArgumentException("Invalid drive name", "driveName");
				}
				driveName = char.ToUpperInvariant(driveName[0]).ToString() + ":\\";
			}
			DriveInfo[] drives = DriveInfo.GetDrives();
			Array.Sort<DriveInfo>(drives, (DriveInfo di1, DriveInfo di2) => string.Compare(di2.path, di1.path, true));
			foreach (DriveInfo driveInfo in drives)
			{
				if (driveName.StartsWith(driveInfo.path, StringComparison.OrdinalIgnoreCase))
				{
					this.path = driveInfo.path;
					this.drive_format = driveInfo.drive_format;
					return;
				}
			}
			throw new ArgumentException("The drive name does not exist", "driveName");
		}

		// Token: 0x0600695E RID: 26974 RVA: 0x001680B8 File Offset: 0x001662B8
		private static void GetDiskFreeSpace(string path, out ulong availableFreeSpace, out ulong totalSize, out ulong totalFreeSpace)
		{
			MonoIOError monoIOError;
			if (!DriveInfo.GetDiskFreeSpaceInternal(path, out availableFreeSpace, out totalSize, out totalFreeSpace, out monoIOError))
			{
				throw MonoIO.GetException(path, monoIOError);
			}
		}

		/// <summary>Indicates the amount of available free space on a drive, in bytes.</summary>
		/// <returns>The amount of free space available on the drive, in bytes.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x0600695F RID: 26975 RVA: 0x001680DC File Offset: 0x001662DC
		public long AvailableFreeSpace
		{
			get
			{
				ulong num;
				ulong num2;
				ulong num3;
				DriveInfo.GetDiskFreeSpace(this.path, out num, out num2, out num3);
				if (num <= 9223372036854775807UL)
				{
					return (long)num;
				}
				return long.MaxValue;
			}
		}

		/// <summary>Gets the total amount of free space available on a drive, in bytes.</summary>
		/// <returns>The total free space available on a drive, in bytes.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
		/// <exception cref="T:System.IO.DriveNotFoundException">The drive is not mapped or does not exist.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06006960 RID: 26976 RVA: 0x00168114 File Offset: 0x00166314
		public long TotalFreeSpace
		{
			get
			{
				ulong num;
				ulong num2;
				ulong num3;
				DriveInfo.GetDiskFreeSpace(this.path, out num, out num2, out num3);
				if (num3 <= 9223372036854775807UL)
				{
					return (long)num3;
				}
				return long.MaxValue;
			}
		}

		/// <summary>Gets the total size of storage space on a drive, in bytes.</summary>
		/// <returns>The total size of the drive, in bytes.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
		/// <exception cref="T:System.IO.DriveNotFoundException">The drive is not mapped or does not exist.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06006961 RID: 26977 RVA: 0x0016814C File Offset: 0x0016634C
		public long TotalSize
		{
			get
			{
				ulong num;
				ulong num2;
				ulong num3;
				DriveInfo.GetDiskFreeSpace(this.path, out num, out num2, out num3);
				if (num2 <= 9223372036854775807UL)
				{
					return (long)num2;
				}
				return long.MaxValue;
			}
		}

		/// <summary>Gets or sets the volume label of a drive.</summary>
		/// <returns>The volume label.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
		/// <exception cref="T:System.IO.DriveNotFoundException">The drive is not mapped or does not exist.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The volume label is being set on a network or CD-ROM drive.  
		///  -or-  
		///  Access to the drive information is denied.</exception>
		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06006962 RID: 26978 RVA: 0x00168181 File Offset: 0x00166381
		// (set) Token: 0x06006963 RID: 26979 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("Currently get only works on Mono/Unix; set not implemented")]
		public string VolumeLabel
		{
			get
			{
				return this.path;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets the name of the file system, such as NTFS or FAT32.</summary>
		/// <returns>The name of the file system on the specified drive.</returns>
		/// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
		/// <exception cref="T:System.IO.DriveNotFoundException">The drive does not exist or is not mapped.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06006964 RID: 26980 RVA: 0x00168189 File Offset: 0x00166389
		public string DriveFormat
		{
			get
			{
				return this.drive_format;
			}
		}

		/// <summary>Gets the drive type, such as CD-ROM, removable, network, or fixed.</summary>
		/// <returns>One of the enumeration values that specifies a drive type.</returns>
		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06006965 RID: 26981 RVA: 0x00168191 File Offset: 0x00166391
		public DriveType DriveType
		{
			get
			{
				return (DriveType)DriveInfo.GetDriveTypeInternal(this.path);
			}
		}

		/// <summary>Gets the name of a drive, such as C:\.</summary>
		/// <returns>The name of the drive.</returns>
		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06006966 RID: 26982 RVA: 0x00168181 File Offset: 0x00166381
		public string Name
		{
			get
			{
				return this.path;
			}
		}

		/// <summary>Gets the root directory of a drive.</summary>
		/// <returns>An object that contains the root directory of the drive.</returns>
		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06006967 RID: 26983 RVA: 0x0016819E File Offset: 0x0016639E
		public DirectoryInfo RootDirectory
		{
			get
			{
				return new DirectoryInfo(this.path);
			}
		}

		/// <summary>Gets a value that indicates whether a drive is ready.</summary>
		/// <returns>
		///   <see langword="true" /> if the drive is ready; <see langword="false" /> if the drive is not ready.</returns>
		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06006968 RID: 26984 RVA: 0x001681AB File Offset: 0x001663AB
		public bool IsReady
		{
			get
			{
				return Directory.Exists(this.Name);
			}
		}

		/// <summary>Retrieves the drive names of all logical drives on a computer.</summary>
		/// <returns>An array of type <see cref="T:System.IO.DriveInfo" /> that represents the logical drives on a computer.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
		// Token: 0x06006969 RID: 26985 RVA: 0x001681B8 File Offset: 0x001663B8
		[MonoTODO("In windows, alldrives are 'Fixed'")]
		public static DriveInfo[] GetDrives()
		{
			string[] logicalDrives = Environment.GetLogicalDrives();
			DriveInfo[] array = new DriveInfo[logicalDrives.Length];
			int num = 0;
			foreach (string text in logicalDrives)
			{
				array[num++] = new DriveInfo(text, DriveInfo.GetDriveFormat(text));
			}
			return array;
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data needed to serialize the target object.</summary>
		/// <param name="info">The object to populate with data.</param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
		// Token: 0x0600696A RID: 26986 RVA: 0x000479FC File Offset: 0x00045BFC
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns a drive name as a string.</summary>
		/// <returns>The name of the drive.</returns>
		// Token: 0x0600696B RID: 26987 RVA: 0x001681FF File Offset: 0x001663FF
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x0600696C RID: 26988
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool GetDiskFreeSpaceInternal(char* pathName, int pathName_length, out ulong freeBytesAvail, out ulong totalNumberOfBytes, out ulong totalNumberOfFreeBytes, out MonoIOError error);

		// Token: 0x0600696D RID: 26989 RVA: 0x00168208 File Offset: 0x00166408
		private unsafe static bool GetDiskFreeSpaceInternal(string pathName, out ulong freeBytesAvail, out ulong totalNumberOfBytes, out ulong totalNumberOfFreeBytes, out MonoIOError error)
		{
			char* ptr = pathName;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return DriveInfo.GetDiskFreeSpaceInternal(ptr, (pathName != null) ? pathName.Length : 0, out freeBytesAvail, out totalNumberOfBytes, out totalNumberOfFreeBytes, out error);
		}

		// Token: 0x0600696E RID: 26990
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern uint GetDriveTypeInternal(char* rootPathName, int rootPathName_length);

		// Token: 0x0600696F RID: 26991 RVA: 0x0016823C File Offset: 0x0016643C
		private unsafe static uint GetDriveTypeInternal(string rootPathName)
		{
			char* ptr = rootPathName;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return DriveInfo.GetDriveTypeInternal(ptr, (rootPathName != null) ? rootPathName.Length : 0);
		}

		// Token: 0x06006970 RID: 26992
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern string GetDriveFormatInternal(char* rootPathName, int rootPathName_length);

		// Token: 0x06006971 RID: 26993 RVA: 0x0016826C File Offset: 0x0016646C
		private unsafe static string GetDriveFormat(string rootPathName)
		{
			char* ptr = rootPathName;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return DriveInfo.GetDriveFormatInternal(ptr, (rootPathName != null) ? rootPathName.Length : 0);
		}

		// Token: 0x04003D2A RID: 15658
		private string drive_format;

		// Token: 0x04003D2B RID: 15659
		private string path;
	}
}
