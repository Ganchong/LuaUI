using System;
using System.Collections.Generic;

/**
 * 时间器管理器
 */
public class TimerManager : Singleton<TimerManager>
{
	/* static fields */

	/* fields */
	/** 定时器列表 */
	List<Timer> timerList = new List<Timer> ();

	/* methods */
	/** 更新 */
	public void update ()
	{
		long nowTime = TimeKit.getMillisTime ();
		for (int i=timerList.Count-1; i>=0; i--) 
		{
			if (timerList [i].isDispose ()) {
				timerList.RemoveAt (i);
			} else {
				timerList [i].update (nowTime);
			}
		}
	}
	/** 移出定时器 */
	public void removeTimer (Timer timer)
	{
		if (timer == null) return;
		if (timerList.Contains (timer)) 
		{
			timer.stop ();
			timerList.Remove (timer);
		}
	}
	/** delay timer间隔时间 count循环次数 */
	public Timer getTimer (long delay, int count)
	{
		Timer t = new Timer (delay, count);
		timerList.Add (t);
		return t;
	}
	/** delay timer间隔时间 一直循环 */
	public Timer getTimer (long delay)
	{
		Timer t = new Timer (delay, 0);
		timerList.Add (t);
		return t;
	}
	/** 清空 */
	public void clearAllTimer ()
	{
		timerList.Clear ();
	}
	public void addTimer(Timer timer){
		if(!timerList.Contains(timer))
			timerList.Add(timer);
	}
}