using System;
using System.Collections;
using UnityEngine;

/**
 * 时间器
 */
public class Timer
{
	/* delegate */
	/** 委托 */
	public delegate void TimerHandle ();

	/* fields */
	/** 当前执行的次数,通过次数差来计算是否满足执行条件 */
	public int count=0;
	/** 重复执行的次数,0无限 */
	int repeatCount;
	/** 每间隔多长时间执行一次 */
	long intervalTime;
	/** 开始时间 */
	long startTime;
	/** 下次触发时间 */
	long nextTime;
	/** 是否运行中 */
	bool running;
	/** 方法回调 */
	TimerHandle onTimer;
	/** 定时器自动移除标识 */
	bool dispose;

	/* constructors */
	/** 构造方法（间隔时间，重复次数） */
	public Timer (long intervalTime, int repeatCount)
	{
		this.intervalTime = intervalTime;
		this.repeatCount = repeatCount;
	}
	/** 添加定时器处理 */
	public void addOnTimer (TimerHandle onTimer)
	{ 
		this.onTimer = onTimer;
	}
	/** 定时处理 */
	public void update (long nowTime)
	{
		if (!running) return;
		if (nowTime < nextTime) return;
		count++;
		if (onTimer != null) onTimer ();
		nextTime = nowTime+intervalTime;
		if (repeatCount > 0 && count >= repeatCount) 
			stop ();
	}
	/** 重置 */
	public void reset ()
	{
		startTime = TimeKit.getMillisTime ();
		nextTime = startTime + intervalTime;
		count = 0;
	}
	/** 开始 */
	public void start (bool firstCall)
	{
		count = 0;
		running = true;
		startTime = TimeKit.getMillisTime ();
		if (firstCall) nextTime = startTime;
		else  nextTime = startTime + intervalTime;
		dispose = false;
	}
	public void start ()
	{
		start (false);
	}
	/** 停止 */
	public void stop ()
	{ 
		running = false;
		startTime = 0;
		count = 0;
		dispose = true;
	}
	/** 是否停止 */
	public bool isDispose ()
	{
		return dispose;
	}
}