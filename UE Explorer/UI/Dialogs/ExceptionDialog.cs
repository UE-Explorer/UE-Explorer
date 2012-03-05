using System;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.Text;
using System.IO;

namespace UEExplorer.UI.Dialogs
{
	public partial class ExceptionDialog : Form
	{
		public ExceptionDialog()
		{
			InitializeComponent();
		}

		public static void Show( string error, Exception exception )
		{
			ExceptionDialog exceptionDialog = new ExceptionDialog();
			exceptionDialog.ExceptionStack.Text = "Thrown by:" + exception.TargetSite.Name + "\r\n" + exception.StackTrace + exception.InnerException ?? exception.InnerException.StackTrace;
			exceptionDialog.ExceptionStack.Text = exceptionDialog.ExceptionStack.Text.Replace( @"C:\Users\Eliot\Documents\Visual Studio 2010\Projects\", "" );
			exceptionDialog.ExceptionMessage.Text = exception.Message.Replace( @"C:\Users\Eliot\Documents\Visual Studio 2010\Projects\", "" );
			exceptionDialog.ExceptionError.Text = error;
			if( exceptionDialog.ShowDialog() == DialogResult.OK )
			{
				exceptionDialog.SendReport();
			}
		}

		public void SendReport()
		{
			SendDialog sendDialog = new SendDialog();
			if( sendDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK )
			{
				var logData = " exception:\r\n<code>" 
					+ ExceptionMessage.Text + "</code>\r\n\r\nStack:\r\n<code>" 
					+ ExceptionStack.Text;
 
				var postData = "data[reports][log]=" + HttpUtility.UrlEncode(logData)
					+ "&data[reports][title]=" + HttpUtility.UrlEncode(ExceptionError.Text)
					+ "&data[reports][description]=" + HttpUtility.UrlEncode(sendDialog.InfoText.Text)
					+ "&data[reports][reporter_email]=" + HttpUtility.UrlEncode(sendDialog.Email.Text);
				
				try
				{
					Post( "http://eliot.pwc-networks.com/report/report/", postData );  
					MessageBox.Show( "Thanks for reporting this exception occurrance!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information );
				}
				catch
				{
					MessageBox.Show( "Failed to send this report. Please try again later!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}
			}
		}

		public void Post( string url, string data )
		{
			var buffer = Encoding.UTF8.GetBytes( data );
			var webReq = (HttpWebRequest)WebRequest.Create( url );
			webReq.Method = "POST";
			webReq.ContentType = "application/x-www-form-urlencoded";
			webReq.ContentLength = buffer.Length;

			using( var postStream = webReq.GetRequestStream())
			{
				postStream.Write( buffer, 0, buffer.Length );
			}

			var response = webReq.GetResponse();
			//using( var responseReader = new StreamReader( response.GetResponseStream() ) )
			//{
			//    MessageBox.Show( responseReader.ReadToEnd() );
			//}
		}
	}
}
