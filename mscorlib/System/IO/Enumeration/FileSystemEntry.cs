using System;

namespace System.IO.Enumeration
{
	// Token: 0x02000B79 RID: 2937
	[Obsolete("Types with embedded references are not supported in this version of your compiler.", true)]
	public ref struct FileSystemEntry
	{
		// Token: 0x06006AFA RID: 27386 RVA: 0x0016E127 File Offset: 0x0016C327
		internal unsafe static void Initialize(ref FileSystemEntry entry, Interop.NtDll.FILE_FULL_DIR_INFORMATION* info, ReadOnlySpan<char> directory, ReadOnlySpan<char> rootDirectory, ReadOnlySpan<char> originalRootDirectory)
		{
			entry._info = info;
			entry.Directory = directory;
			entry.RootDirectory = rootDirectory;
			entry.OriginalRootDirectory = originalRootDirectory;
		}

		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06006AFB RID: 27387 RVA: 0x0016E146 File Offset: 0x0016C346
		// (set) Token: 0x06006AFC RID: 27388 RVA: 0x0016E14E File Offset: 0x0016C34E
		public ReadOnlySpan<char> Directory { readonly get; private set; }

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06006AFD RID: 27389 RVA: 0x0016E157 File Offset: 0x0016C357
		// (set) Token: 0x06006AFE RID: 27390 RVA: 0x0016E15F File Offset: 0x0016C35F
		public ReadOnlySpan<char> RootDirectory { readonly get; private set; }

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06006AFF RID: 27391 RVA: 0x0016E168 File Offset: 0x0016C368
		// (set) Token: 0x06006B00 RID: 27392 RVA: 0x0016E170 File Offset: 0x0016C370
		public ReadOnlySpan<char> OriginalRootDirectory { readonly get; private set; }

		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06006B01 RID: 27393 RVA: 0x0016E179 File Offset: 0x0016C379
		public unsafe ReadOnlySpan<char> FileName
		{
			get
			{
				return this._info->FileName;
			}
		}

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06006B02 RID: 27394 RVA: 0x0016E186 File Offset: 0x0016C386
		public unsafe FileAttributes Attributes
		{
			get
			{
				return this._info->FileAttributes;
			}
		}

		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06006B03 RID: 27395 RVA: 0x0016E193 File Offset: 0x0016C393
		public unsafe long Length
		{
			get
			{
				return this._info->EndOfFile;
			}
		}

		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06006B04 RID: 27396 RVA: 0x0016E1A0 File Offset: 0x0016C3A0
		public unsafe DateTimeOffset CreationTimeUtc
		{
			get
			{
				return this._info->CreationTime.ToDateTimeOffset();
			}
		}

		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06006B05 RID: 27397 RVA: 0x0016E1B2 File Offset: 0x0016C3B2
		public unsafe DateTimeOffset LastAccessTimeUtc
		{
			get
			{
				return this._info->LastAccessTime.ToDateTimeOffset();
			}
		}

		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06006B06 RID: 27398 RVA: 0x0016E1C4 File Offset: 0x0016C3C4
		public unsafe DateTimeOffset LastWriteTimeUtc
		{
			get
			{
				return this._info->LastWriteTime.ToDateTimeOffset();
			}
		}

		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06006B07 RID: 27399 RVA: 0x0016E1D6 File Offset: 0x0016C3D6
		public bool IsDirectory
		{
			get
			{
				return (this.Attributes & FileAttributes.Directory) > (FileAttributes)0;
			}
		}

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06006B08 RID: 27400 RVA: 0x0016E1E4 File Offset: 0x0016C3E4
		public bool IsHidden
		{
			get
			{
				return (this.Attributes & FileAttributes.Hidden) > (FileAttributes)0;
			}
		}

		// Token: 0x06006B09 RID: 27401 RVA: 0x0016E1F1 File Offset: 0x0016C3F1
		public FileSystemInfo ToFileSystemInfo()
		{
			return FileSystemInfo.Create(Path.Join(this.Directory, this.FileName), ref this);
		}

		// Token: 0x06006B0A RID: 27402 RVA: 0x0016E20A File Offset: 0x0016C40A
		public string ToFullPath()
		{
			return Path.Join(this.Directory, this.FileName);
		}

		// Token: 0x06006B0B RID: 27403 RVA: 0x0016E220 File Offset: 0x0016C420
		public string ToSpecifiedFullPath()
		{
			ReadOnlySpan<char> readOnlySpan = this.Directory.Slice(this.RootDirectory.Length);
			if (PathInternal.EndsInDirectorySeparator(this.OriginalRootDirectory) && PathInternal.StartsWithDirectorySeparator(readOnlySpan))
			{
				readOnlySpan = readOnlySpan.Slice(1);
			}
			return Path.Join(this.OriginalRootDirectory, readOnlySpan, this.FileName);
		}

		// Token: 0x04003DA8 RID: 15784
		internal unsafe Interop.NtDll.FILE_FULL_DIR_INFORMATION* _info;
	}
}
