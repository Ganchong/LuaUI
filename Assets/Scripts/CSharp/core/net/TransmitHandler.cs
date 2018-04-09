/**
 * Coypright 2013 by 刘耀鑫<xiney@youkia.com>.
 */

/**
 * @author 刘耀鑫
 */
public interface TransmitHandler
{

	/* static fields */

	/* fields */

	/* methods */
	/**
	 * 消息传送方法， 参数connect为连接， 参数data是传送的消息，
	 */
	void transmit (Connect connect, ByteBuffer data);
}
