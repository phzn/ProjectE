using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E63 RID: 3683
	public sealed class RuntimeMjpegAviWriter : IDisposable
	{
		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x06006DFF RID: 28159 RVA: 0x002C3797 File Offset: 0x002C1997
		public int VideoFrameCount
		{
			get
			{
				return this._videoFrameCount;
			}
		}

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06006E00 RID: 28160 RVA: 0x002C379F File Offset: 0x002C199F
		public long AudioByteCount
		{
			get
			{
				return this._audioByteCount;
			}
		}

		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06006E01 RID: 28161 RVA: 0x002C37A7 File Offset: 0x002C19A7
		public long LengthBytes
		{
			get
			{
				if (this._stream == null)
				{
					return 0L;
				}
				return this._stream.Length;
			}
		}

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x06006E02 RID: 28162 RVA: 0x002C37BF File Offset: 0x002C19BF
		public double DurationSeconds
		{
			get
			{
				if (this._frameRate <= 0)
				{
					return 0.0;
				}
				return (double)this._videoFrameCount / (double)this._frameRate;
			}
		}

		// Token: 0x06006E03 RID: 28163 RVA: 0x002C37E4 File Offset: 0x002C19E4
		public RuntimeMjpegAviWriter(string path, int width, int height, int frameRate, bool includeAudio, int audioSampleRate = 48000, int audioChannels = 2)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentException("Output path is empty.", "path");
			}
			if (width < 16 || height < 16)
			{
				throw new ArgumentOutOfRangeException("width");
			}
			if (frameRate < 1 || frameRate > 120)
			{
				throw new ArgumentOutOfRangeException("frameRate");
			}
			this._width = width;
			this._height = height;
			this._frameRate = frameRate;
			this._includeAudio = includeAudio;
			this._audioSampleRate = Math.Max(8000, audioSampleRate);
			this._audioChannels = Math.Max(1, Math.Min(2, audioChannels));
			this._audioBlockAlign = this._audioChannels * 2;
			string directoryName = Path.GetDirectoryName(path);
			if (!string.IsNullOrEmpty(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			this._stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
			this._writer = new BinaryWriter(this._stream, Encoding.ASCII, true);
			this.WriteHeader();
		}

		// Token: 0x06006E04 RID: 28164 RVA: 0x002C38E0 File Offset: 0x002C1AE0
		public void WriteVideoFrame(byte[] jpeg)
		{
			this.EnsureWritable();
			if (jpeg == null || jpeg.Length < 4)
			{
				throw new ArgumentException("JPEG frame is empty.", "jpeg");
			}
			if (jpeg[0] != 255 || jpeg[1] != 216)
			{
				throw new InvalidDataException("The supplied video frame is not a JPEG image.");
			}
			this.WriteDataChunk(RuntimeMjpegAviWriter.FourCc("00dc"), 16U, jpeg, jpeg.Length);
			this._videoFrameCount++;
			if (jpeg.Length > this._largestVideoFrame)
			{
				this._largestVideoFrame = jpeg.Length;
			}
		}

		// Token: 0x06006E05 RID: 28165 RVA: 0x002C3964 File Offset: 0x002C1B64
		public void WriteAudioPcm(byte[] pcm)
		{
			this.EnsureWritable();
			if (!this._includeAudio || pcm == null || pcm.Length == 0)
			{
				return;
			}
			int num = pcm.Length - pcm.Length % this._audioBlockAlign;
			if (num <= 0)
			{
				return;
			}
			this.WriteDataChunk(RuntimeMjpegAviWriter.FourCc("01wb"), 0U, pcm, num);
			this._audioByteCount += (long)num;
		}

		// Token: 0x06006E06 RID: 28166 RVA: 0x002C39C0 File Offset: 0x002C1BC0
		public void FinalizeFile()
		{
			if (this._finalized || this._aborted)
			{
				return;
			}
			long position = this._stream.Position;
			this.PatchUInt32(this._moviListSizePosition, RuntimeMjpegAviWriter.CheckedUInt(position - (this._moviListSizePosition + 4L)));
			this.WriteFourCc("idx1");
			this._writer.Write(RuntimeMjpegAviWriter.CheckedUInt((long)this._index.Count * 16L));
			for (int i = 0; i < this._index.Count; i++)
			{
				RuntimeMjpegAviWriter.IndexEntry indexEntry = this._index[i];
				this._writer.Write(indexEntry.chunkId);
				this._writer.Write(indexEntry.flags);
				this._writer.Write(indexEntry.offset);
				this._writer.Write(indexEntry.size);
			}
			long position2 = this._stream.Position;
			uint num = 0U;
			if (this._videoFrameCount > 0)
			{
				double num2 = Math.Max(0.001, this.DurationSeconds);
				num = RuntimeMjpegAviWriter.CheckedUInt((long)Math.Ceiling((double)position2 / num2));
			}
			this.PatchUInt32(this._riffSizePosition, RuntimeMjpegAviWriter.CheckedUInt(position2 - 8L));
			this.PatchUInt32(this._mainMaxBytesPerSecondPosition, num);
			this.PatchUInt32(this._mainTotalFramesPosition, (uint)this._videoFrameCount);
			this.PatchUInt32(this._mainSuggestedBufferPosition, (uint)this._largestVideoFrame);
			this.PatchUInt32(this._videoLengthPosition, (uint)this._videoFrameCount);
			this.PatchUInt32(this._videoSuggestedBufferPosition, (uint)this._largestVideoFrame);
			if (this._includeAudio)
			{
				this.PatchUInt32(this._audioLengthPosition, RuntimeMjpegAviWriter.CheckedUInt(this._audioByteCount / (long)this._audioBlockAlign));
			}
			this._writer.Flush();
			this._stream.Flush(true);
			this._finalized = true;
			this._writer.Dispose();
			this._stream.Dispose();
		}

		// Token: 0x06006E07 RID: 28167 RVA: 0x002C3BA0 File Offset: 0x002C1DA0
		public void Abort()
		{
			if (this._finalized || this._aborted)
			{
				return;
			}
			this._aborted = true;
			try
			{
				this._writer.Dispose();
			}
			finally
			{
				this._stream.Dispose();
			}
		}

		// Token: 0x06006E08 RID: 28168 RVA: 0x002C3BF0 File Offset: 0x002C1DF0
		public void Dispose()
		{
			if (!this._finalized && !this._aborted)
			{
				this.FinalizeFile();
			}
		}

		// Token: 0x06006E09 RID: 28169 RVA: 0x002C3C08 File Offset: 0x002C1E08
		private void WriteHeader()
		{
			this.WriteFourCc("RIFF");
			this._riffSizePosition = this._stream.Position;
			this._writer.Write(0U);
			this.WriteFourCc("AVI ");
			long num = this.BeginList("hdrl");
			this.WriteMainHeader();
			this.WriteVideoStreamHeader();
			if (this._includeAudio)
			{
				this.WriteAudioStreamHeader();
			}
			this.EndList(num);
			this.WriteFourCc("LIST");
			this._moviListSizePosition = this._stream.Position;
			this._writer.Write(0U);
			this._moviListTypePosition = this._stream.Position;
			this.WriteFourCc("movi");
		}

		// Token: 0x06006E0A RID: 28170 RVA: 0x002C3CBC File Offset: 0x002C1EBC
		private void WriteMainHeader()
		{
			this.WriteFourCc("avih");
			this._writer.Write(56U);
			this._writer.Write((uint)Math.Round(1000000.0 / (double)this._frameRate));
			this._mainMaxBytesPerSecondPosition = this._stream.Position;
			this._writer.Write(0U);
			this._writer.Write(0U);
			this._writer.Write(16U);
			this._mainTotalFramesPosition = this._stream.Position;
			this._writer.Write(0U);
			this._writer.Write(0U);
			this._writer.Write(this._includeAudio ? 2U : 1U);
			this._mainSuggestedBufferPosition = this._stream.Position;
			this._writer.Write(0U);
			this._writer.Write((uint)this._width);
			this._writer.Write((uint)this._height);
			this._writer.Write(0U);
			this._writer.Write(0U);
			this._writer.Write(0U);
			this._writer.Write(0U);
		}

		// Token: 0x06006E0B RID: 28171 RVA: 0x002C3DE8 File Offset: 0x002C1FE8
		private void WriteVideoStreamHeader()
		{
			long num = this.BeginList("strl");
			this.WriteFourCc("strh");
			this._writer.Write(56U);
			this.WriteFourCc("vids");
			this.WriteFourCc("MJPG");
			this._writer.Write(0U);
			this._writer.Write(0);
			this._writer.Write(0);
			this._writer.Write(0U);
			this._writer.Write(1U);
			this._writer.Write((uint)this._frameRate);
			this._writer.Write(0U);
			this._videoLengthPosition = this._stream.Position;
			this._writer.Write(0U);
			this._videoSuggestedBufferPosition = this._stream.Position;
			this._writer.Write(0U);
			this._writer.Write(10000U);
			this._writer.Write(0U);
			this._writer.Write(0);
			this._writer.Write(0);
			this._writer.Write((short)this._width);
			this._writer.Write((short)this._height);
			this.WriteFourCc("strf");
			this._writer.Write(40U);
			this._writer.Write(40U);
			this._writer.Write(this._width);
			this._writer.Write(this._height);
			this._writer.Write(1);
			this._writer.Write(24);
			this.WriteFourCc("MJPG");
			this._writer.Write(RuntimeMjpegAviWriter.CheckedUInt((long)this._width * (long)this._height * 3L));
			this._writer.Write(0);
			this._writer.Write(0);
			this._writer.Write(0U);
			this._writer.Write(0U);
			this.EndList(num);
		}

		// Token: 0x06006E0C RID: 28172 RVA: 0x002C3FE0 File Offset: 0x002C21E0
		private void WriteAudioStreamHeader()
		{
			long num = this.BeginList("strl");
			uint num2 = (uint)(this._audioSampleRate * this._audioBlockAlign);
			this.WriteFourCc("strh");
			this._writer.Write(56U);
			this.WriteFourCc("auds");
			this._writer.Write(0U);
			this._writer.Write(0U);
			this._writer.Write(0);
			this._writer.Write(0);
			this._writer.Write(0U);
			this._writer.Write((uint)this._audioBlockAlign);
			this._writer.Write(num2);
			this._writer.Write(0U);
			this._audioLengthPosition = this._stream.Position;
			this._writer.Write(0U);
			this._writer.Write(32768U);
			this._writer.Write(uint.MaxValue);
			this._writer.Write((uint)this._audioBlockAlign);
			this._writer.Write(0);
			this._writer.Write(0);
			this._writer.Write(0);
			this._writer.Write(0);
			this.WriteFourCc("strf");
			this._writer.Write(16U);
			this._writer.Write(1);
			this._writer.Write((ushort)this._audioChannels);
			this._writer.Write((uint)this._audioSampleRate);
			this._writer.Write(num2);
			this._writer.Write((ushort)this._audioBlockAlign);
			this._writer.Write(16);
			this.EndList(num);
		}

		// Token: 0x06006E0D RID: 28173 RVA: 0x002C4184 File Offset: 0x002C2384
		private void WriteDataChunk(uint chunkId, uint flags, byte[] data, int count)
		{
			long position = this._stream.Position;
			this._writer.Write(chunkId);
			this._writer.Write((uint)count);
			this._writer.Write(data, 0, count);
			if ((count & 1) != 0)
			{
				this._writer.Write(0);
			}
			this._index.Add(new RuntimeMjpegAviWriter.IndexEntry
			{
				chunkId = chunkId,
				flags = flags,
				offset = RuntimeMjpegAviWriter.CheckedUInt(position - this._moviListTypePosition),
				size = (uint)count
			});
		}

		// Token: 0x06006E0E RID: 28174 RVA: 0x002C4218 File Offset: 0x002C2418
		private long BeginList(string listType)
		{
			this.WriteFourCc("LIST");
			long position = this._stream.Position;
			this._writer.Write(0U);
			this.WriteFourCc(listType);
			return position;
		}

		// Token: 0x06006E0F RID: 28175 RVA: 0x002C4243 File Offset: 0x002C2443
		private void EndList(long sizePosition)
		{
			this.PatchUInt32(sizePosition, RuntimeMjpegAviWriter.CheckedUInt(this._stream.Position - (sizePosition + 4L)));
		}

		// Token: 0x06006E10 RID: 28176 RVA: 0x002C4264 File Offset: 0x002C2464
		private void PatchUInt32(long position, uint value)
		{
			long position2 = this._stream.Position;
			this._stream.Position = position;
			this._writer.Write(value);
			this._stream.Position = position2;
		}

		// Token: 0x06006E11 RID: 28177 RVA: 0x002C42A1 File Offset: 0x002C24A1
		private void WriteFourCc(string value)
		{
			this._writer.Write(RuntimeMjpegAviWriter.FourCc(value));
		}

		// Token: 0x06006E12 RID: 28178 RVA: 0x002C42B4 File Offset: 0x002C24B4
		private static uint FourCc(string value)
		{
			if (value == null || value.Length != 4)
			{
				throw new ArgumentException("A FOURCC must contain exactly four ASCII characters.", "value");
			}
			return (uint)((int)((byte)value[0]) | ((int)((byte)value[1]) << 8) | ((int)((byte)value[2]) << 16) | ((int)((byte)value[3]) << 24));
		}

		// Token: 0x06006E13 RID: 28179 RVA: 0x002C4308 File Offset: 0x002C2508
		private static uint CheckedUInt(long value)
		{
			if (value < 0L || value > (long)((ulong)(-1)))
			{
				throw new InvalidOperationException("AVI 1.0 file exceeded its 4 GB address space.");
			}
			return (uint)value;
		}

		// Token: 0x06006E14 RID: 28180 RVA: 0x002C4321 File Offset: 0x002C2521
		private void EnsureWritable()
		{
			if (this._finalized)
			{
				throw new ObjectDisposedException("RuntimeMjpegAviWriter");
			}
			if (this._aborted)
			{
				throw new InvalidOperationException("The AVI writer was aborted.");
			}
		}

		// Token: 0x04007092 RID: 28818
		private const uint AviHasIndex = 16U;

		// Token: 0x04007093 RID: 28819
		private const uint AviKeyFrame = 16U;

		// Token: 0x04007094 RID: 28820
		private readonly FileStream _stream;

		// Token: 0x04007095 RID: 28821
		private readonly BinaryWriter _writer;

		// Token: 0x04007096 RID: 28822
		private readonly List<RuntimeMjpegAviWriter.IndexEntry> _index = new List<RuntimeMjpegAviWriter.IndexEntry>(4096);

		// Token: 0x04007097 RID: 28823
		private readonly int _width;

		// Token: 0x04007098 RID: 28824
		private readonly int _height;

		// Token: 0x04007099 RID: 28825
		private readonly int _frameRate;

		// Token: 0x0400709A RID: 28826
		private readonly int _audioSampleRate;

		// Token: 0x0400709B RID: 28827
		private readonly int _audioChannels;

		// Token: 0x0400709C RID: 28828
		private readonly int _audioBlockAlign;

		// Token: 0x0400709D RID: 28829
		private readonly bool _includeAudio;

		// Token: 0x0400709E RID: 28830
		private long _riffSizePosition;

		// Token: 0x0400709F RID: 28831
		private long _mainMaxBytesPerSecondPosition;

		// Token: 0x040070A0 RID: 28832
		private long _mainTotalFramesPosition;

		// Token: 0x040070A1 RID: 28833
		private long _mainSuggestedBufferPosition;

		// Token: 0x040070A2 RID: 28834
		private long _videoLengthPosition;

		// Token: 0x040070A3 RID: 28835
		private long _videoSuggestedBufferPosition;

		// Token: 0x040070A4 RID: 28836
		private long _audioLengthPosition;

		// Token: 0x040070A5 RID: 28837
		private long _moviListSizePosition;

		// Token: 0x040070A6 RID: 28838
		private long _moviListTypePosition;

		// Token: 0x040070A7 RID: 28839
		private int _videoFrameCount;

		// Token: 0x040070A8 RID: 28840
		private long _audioByteCount;

		// Token: 0x040070A9 RID: 28841
		private int _largestVideoFrame;

		// Token: 0x040070AA RID: 28842
		private bool _finalized;

		// Token: 0x040070AB RID: 28843
		private bool _aborted;

		// Token: 0x02000E64 RID: 3684
		private struct IndexEntry
		{
			// Token: 0x040070AC RID: 28844
			public uint chunkId;

			// Token: 0x040070AD RID: 28845
			public uint flags;

			// Token: 0x040070AE RID: 28846
			public uint offset;

			// Token: 0x040070AF RID: 28847
			public uint size;
		}
	}
}
