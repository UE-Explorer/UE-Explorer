using System;

namespace Eliot.Utilities
{
	public static class StringExtensions
	{
		public static string Left( this string s, int index )
		{
			return s.Substring( 0, index );
		}

		public static string Mid( this string s, int index )
		{
			return s.Substring( index, s.Length - index );
		}

		public static string Right( this string s, int index )
		{
			return s.Substring( 0, s.Length - index );
		}

		public static char FromInt( this Char s, int i )
		{
			return (Char)i;
		}

		public static int FromChar( this Char s )
		{
			return s;
		}	
	}
}

