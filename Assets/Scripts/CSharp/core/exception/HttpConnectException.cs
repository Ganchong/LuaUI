using System;

public class HttpConnectException:Exception
{
	/* static fields */
	public const int INNER_EXCEPTION=1,TIME_OUT=2;
	
	/* fields */
	int errorCode;
	
	/* methods */
	public HttpConnectException (string message, int errorCode, Exception innerException):base(message,innerException)
	{
		this.errorCode = errorCode;
	}

	public HttpConnectException (string message, int errorCode):this(message,errorCode,null)
	{
	}
	
	public int getErrorCode ()
	{
		return errorCode;
	}

	private HttpConnectException ()
	{
		
	}
}

