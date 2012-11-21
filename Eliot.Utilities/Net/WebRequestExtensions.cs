using System.IO;
using System.Net;
using System.Text;

namespace Eliot.Utilities.Net
{
	/// <summary>
	/// 
	/// </summary>
	public static class WebRequestExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="webReq"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string Post( this WebRequest webReq, string data )
		{
			var buffer = Encoding.UTF8.GetBytes( data );
			webReq.Proxy = null;
			//webReq.Proxy = WebRequest.DefaultWebProxy;
			webReq.Method = "POST";
			webReq.ContentType = "application/x-www-form-urlencoded";
			webReq.ContentLength = buffer.Length;

			using( var postStream = webReq.GetRequestStream())
			{
				postStream.Write( buffer, 0, buffer.Length );
				
			}	

			string result;
			using( var response = webReq.GetResponse() )
			{ 
				using( var responseReader = new StreamReader( response.GetResponseStream() ) )
				{
					result = responseReader.ReadToEnd();
				}
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="webReq"></param>
		/// <returns></returns>
		public static MemoryStream Get( this WebRequest webReq )
		{
			//webReq.Proxy = WebRequest.DefaultWebProxy;
			webReq.Proxy = null;
			var buffer = new MemoryStream();
			using( var response = webReq.GetResponse() )
			{
				using( var responseStream = response.GetResponseStream() )
				{
					responseStream.CopyTo( buffer );
				}
			}
			return buffer;
		}
	}
}
