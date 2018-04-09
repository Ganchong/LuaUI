using System;

public class TimerEvent
{
	/** static fields */
	
	/** fields */
	private int count = 0;
	private int maxCount = 0;
	private string action;
	private TimerListener listener;
	private long interval;
	private int loopCount = 0;
	private int delay = 0;
	private long delayInterval;
	
	
	/** methods */
	public TimerEvent (TimerListener listener, string action, long interval):this(listener,action,0,interval,0)
	{
		
	}
	
	public TimerEvent (TimerListener listener, string action, long interval, int loopCount):this(listener,action,0,interval,loopCount)
	{
		
	}
	
	public TimerEvent (TimerListener listener, string action, long delayInterval, long interval, int loopCount)
	{
		if (interval <= 0 || delay < 0 || loopCount < 0) {
			throw new ArgumentException (this + "argument is invalid");
		}
		this.listener = listener;
		this.action = action;
		this.interval = interval;
		this.delayInterval = delayInterval;
		this.loopCount = loopCount;
	}
	
	public string getAction ()
	{
		return action;
	}
	
	public TimerListener getListener ()
	{
		return listener;	
	}
	
	public void init (float frame)
	{
		maxCount = (int)(Math.Ceiling (interval / frame));
		delay = (int)(Math.Ceiling (delayInterval / frame));
	}
	
	public void updateCount ()
	{
		if (delay > 0)
			delay--;
		else		
			count++;
	}
	
	public bool checkTime ()
	{
		return count >= maxCount;
	}
	
	public void clearCount ()
	{
		count = 0;
		delay = 0;
	}

	public bool isLoopOver ()
	{
		if (loopCount == 0)
			return false;
		loopCount --;
		if (loopCount == 0) {
			return  true;
		}
		return false;
	}
	
	private TimerEvent()
	{
		
	}
}
