using System;

public class ActionEvent
{

	public object source;
	public int type;
	public int action;
	public object parameter;
	/** 前台临时缓存 */
	public object temp;
	
	/** 前台临时缓存 */
	public object getTemp()
	{
		return temp;
	}
	/** 前台临时缓存 */
	public void setTemp(object obj)
	{
		this.temp=obj;
	}

	/* constructors */
	public ActionEvent (int action)
	{
		this.action = action;
	}
	public ActionEvent (object source, int action)
	{
		this.source = source;
		this.action = action;
	}
	public ActionEvent (object source, int type, int action)
	{
		this.source = source;
		this.type = type;
		this.action = action;
	}
	public ActionEvent (object source, int type, int action, object parameter)
	{
		this.source = source;
		this.type = type;
		this.action = action;
		this.parameter = parameter;
	}
	public Boolean isOK()
	{
		return this.type==DataAccessHandler.ACCESS_OK;
	}

	/* common methods */
	public override string ToString ()
	{
		return base.ToString () + "[source=" + source + ", type=" + type + ", action="
			+ action + ", parameter=" + parameter + "] ";
	}

}
