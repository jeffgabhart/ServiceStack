//
// http://code.google.com/p/servicestack/wiki/TypeSerializer
// ServiceStack.Text: .NET C# POCO Type Text Serializer.
//
// Authors:
//   Demis Bellot (demis.bellot@gmail.com)
//
// Copyright 2010 Liquidbit Ltd.
//
// Licensed under the same terms of ServiceStack: new BSD license.
//

using System;
using System.Globalization;
using System.Xml;

namespace ServiceStack.Text.Jsv
{
	public static class DateTimeSerializer
	{
		public const string ShortDateTimeFormat = "yyyy-MM-dd";					//11
		public const string DefaultDateTimeFormat = "dd/MM/yyyy HH:mm:ss";		//20
		public const string DefaultDateTimeFormatWithFraction = "dd/MM/yyyy HH:mm:ss.fff";	//24
		public const string XsdDateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";	//29
		public const string XsdDateTimeFormat3F = "yyyy-MM-ddTHH:mm:ss.fffZ";	//25
		public const string XsdDateTimeFormatSeconds = "yyyy-MM-ddTHH:mm:ssZ";	//21

		public static string ToDateTimeString(DateTime dateTime)
		{
			return dateTime.ToUniversalTime().ToString(XsdDateTimeFormat);
		}

		public static DateTime ParseDateTime(string dateTimeStr)
		{
			return DateTime.ParseExact(dateTimeStr, XsdDateTimeFormat, null);
		}

		public static string ToXsdDateTimeString(DateTime dateTime)
		{
			return XmlConvert.ToString(dateTime.ToUniversalTime(), XmlDateTimeSerializationMode.Utc);
		}

		public static DateTime ParseXsdDateTime(string dateTimeStr)
		{
			return XmlConvert.ToDateTime(dateTimeStr, XmlDateTimeSerializationMode.Utc);
		}

		public static string ToShortestXsdDateTimeString(DateTime dateTime)
		{
			var timeOfDay = dateTime.TimeOfDay;
			
			if (timeOfDay.Ticks == 0)
				return dateTime.ToString(ShortDateTimeFormat);

			if (timeOfDay.Milliseconds == 0)
				return dateTime.ToUniversalTime().ToString(XsdDateTimeFormatSeconds);

			return ToXsdDateTimeString(dateTime);
		}

		public static DateTime ParseShortestXsdDateTime(string dateTimeStr)
		{
			if (string.IsNullOrEmpty(dateTimeStr)) 
				return DateTime.MinValue;

			if (dateTimeStr.Length == DefaultDateTimeFormat.Length
				|| dateTimeStr.Length == DefaultDateTimeFormatWithFraction.Length)
				return DateTime.Parse(dateTimeStr, CultureInfo.InvariantCulture);

			if (dateTimeStr.Length <= XsdDateTimeFormat.Length
			    || dateTimeStr.Length >= XsdDateTimeFormat3F.Length)
				return XmlConvert.ToDateTime(dateTimeStr, XmlDateTimeSerializationMode.Local);

			if (dateTimeStr.Length == XsdDateTimeFormatSeconds.Length)
				return DateTime.ParseExact(dateTimeStr, XsdDateTimeFormatSeconds, null,
				                           DateTimeStyles.AdjustToUniversal);

			return new DateTime(
				int.Parse(dateTimeStr.Substring(0, 4)),
				int.Parse(dateTimeStr.Substring(5, 2)),
				int.Parse(dateTimeStr.Substring(8, 2)),
				0, 0, 0,
				DateTimeKind.Local);
		}
		
	}
}