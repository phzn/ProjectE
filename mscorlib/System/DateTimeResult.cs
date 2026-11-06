using System;
using System.Globalization;

namespace System
{
	// Token: 0x0200012C RID: 300
	internal ref struct DateTimeResult
	{
		// Token: 0x06000B8E RID: 2958 RVA: 0x0002F598 File Offset: 0x0002D798
		internal void Init(ReadOnlySpan<char> originalDateTimeString)
		{
			this.originalDateTimeString = originalDateTimeString;
			this.Year = -1;
			this.Month = -1;
			this.Day = -1;
			this.fraction = -1.0;
			this.era = -1;
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0002F5CC File Offset: 0x0002D7CC
		internal void SetDate(int year, int month, int day)
		{
			this.Year = year;
			this.Month = month;
			this.Day = day;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0002F5E3 File Offset: 0x0002D7E3
		internal void SetBadFormatSpecifierFailure()
		{
			this.SetBadFormatSpecifierFailure(ReadOnlySpan<char>.Empty);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
		internal void SetBadFormatSpecifierFailure(ReadOnlySpan<char> failedFormatSpecifier)
		{
			this.failure = ParseFailureKind.FormatWithFormatSpecifier;
			this.failureMessageID = "Format specifier was invalid.";
			this.failedFormatSpecifier = failedFormatSpecifier;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0002F60B File Offset: 0x0002D80B
		internal void SetBadDateTimeFailure()
		{
			this.failure = ParseFailureKind.FormatWithOriginalDateTime;
			this.failureMessageID = "String was not recognized as a valid DateTime.";
			this.failureMessageFormatArgument = null;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002F626 File Offset: 0x0002D826
		internal void SetFailure(ParseFailureKind failure, string failureMessageID)
		{
			this.failure = failure;
			this.failureMessageID = failureMessageID;
			this.failureMessageFormatArgument = null;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0002F63D File Offset: 0x0002D83D
		internal void SetFailure(ParseFailureKind failure, string failureMessageID, object failureMessageFormatArgument)
		{
			this.failure = failure;
			this.failureMessageID = failureMessageID;
			this.failureMessageFormatArgument = failureMessageFormatArgument;
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0002F654 File Offset: 0x0002D854
		internal void SetFailure(ParseFailureKind failure, string failureMessageID, object failureMessageFormatArgument, string failureArgumentName)
		{
			this.failure = failure;
			this.failureMessageID = failureMessageID;
			this.failureMessageFormatArgument = failureMessageFormatArgument;
			this.failureArgumentName = failureArgumentName;
		}

		// Token: 0x04001177 RID: 4471
		internal int Year;

		// Token: 0x04001178 RID: 4472
		internal int Month;

		// Token: 0x04001179 RID: 4473
		internal int Day;

		// Token: 0x0400117A RID: 4474
		internal int Hour;

		// Token: 0x0400117B RID: 4475
		internal int Minute;

		// Token: 0x0400117C RID: 4476
		internal int Second;

		// Token: 0x0400117D RID: 4477
		internal double fraction;

		// Token: 0x0400117E RID: 4478
		internal int era;

		// Token: 0x0400117F RID: 4479
		internal ParseFlags flags;

		// Token: 0x04001180 RID: 4480
		internal TimeSpan timeZoneOffset;

		// Token: 0x04001181 RID: 4481
		internal Calendar calendar;

		// Token: 0x04001182 RID: 4482
		internal DateTime parsedDate;

		// Token: 0x04001183 RID: 4483
		internal ParseFailureKind failure;

		// Token: 0x04001184 RID: 4484
		internal string failureMessageID;

		// Token: 0x04001185 RID: 4485
		internal object failureMessageFormatArgument;

		// Token: 0x04001186 RID: 4486
		internal string failureArgumentName;

		// Token: 0x04001187 RID: 4487
		internal ReadOnlySpan<char> originalDateTimeString;

		// Token: 0x04001188 RID: 4488
		internal ReadOnlySpan<char> failedFormatSpecifier;
	}
}
