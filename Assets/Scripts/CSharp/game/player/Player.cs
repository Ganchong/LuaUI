using System;
using System.Collections.Generic;
/// <summary>
///  玩家
/// </summary>
public class Player:IEventSender
{
	/* static fields */
	private static Player instance;

	/* fields */
	/** sid */
	int sid;
	/** pid */
	long pid;
	/** 名称 */
	string name;
	/** 头像 */
	string headStyle;
	/** 经验值 */
	long exp;
	/** 等级 */
	int level;
	/** vip等级 */
	int vipLevel;
	/** 钻石 */
	int gold;
	/** 游戏币 */
	int money;
	/** 充值数 */
	int charge;

	/* static methods */
	/** 单例 */
	public static Player Instance
	{
		set{
			instance = value;
		}
		get{
			return instance;
		}
	}
	/** 序列化 */
	public static void load(ByteBuffer data)
	{
		if(instance==null) instance = new Player ();
		instance.bytesRead (data);
	}

	/** 序列化 */
	public void bytesRead(ByteBuffer data)
	{
		sid = data.readUnsignedShort ();
		name = data.readUTF ();
		headStyle = data.readUTF();
		exp = data.readLong ();
		level = data.readUnsignedByte ();
		gold = data.readInt ();
		money = data.readInt ();
		vipLevel = data.readUnsignedByte ();
		charge = data.readInt ();
	}

	public string HeadStyle {
		get {
			return this.headStyle;
		}
		set {
			headStyle = value;
		}
	}

	public int Gold {
		get {
			return this.gold;
		}
		set {
			gold = value;
		}
	}

	public int Money {
		get {
			return this.money;
		}
		set {
			money = value;
		}
	}

	public string Name {
		get {
			return this.name;
		}
	}

	public long Exp {
		get {
			return this.exp;
		}
		set {
			exp = value;
		}
	}

	public int Level {
		get {
			level=level<1?1:level;
			return this.level;
		}
		set
		{
			if (level != value) 
			{
				level = value;
			}
		}
	}
	public int VipLevel {
		get {
			return this.vipLevel;
		}
		set {
			vipLevel = value;
		}
	}
	public int Charge {
		get {
			return this.charge;
		}
		set {
			charge = value;
		}
	}
	public long Pid {
		get {
			return this.pid;
		}
		set {
			pid = value;
		}
	}
}