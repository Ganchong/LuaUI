
public abstract class AbstractRandom
{

	/* static fields */
	protected static readonly float FLOAT_MAX = 1.0f - 1.192092896e-07f;
	protected static readonly float FLOAT_AM = 1.0f / 2147483563.0f;
	protected static readonly double DOUBLE_MAX = 1.0d - 1.192092896e-07f;
	protected static readonly double DOUBLE_AM = 1.0d / 2147483563.0d;

	/* fields */
	
	protected int seed = 0;

	/* constructors */
	
	public AbstractRandom ()
	{
		seed = (int)TimeKit.getMillisTime();
	}
	
	public AbstractRandom (int seed)
	{
		this.seed = seed;
	}

	/* properties */

	public int getSeed ()
	{
		return seed;
	}
	
	public void setSeed (int seed)
	{
		this.seed = seed;
	}
	/* methods */
	
	public abstract int randomInt ();
	
	public float randomFloat ()
	{
		int r = randomInt ();
		float tmp = FLOAT_AM * r;
		return (tmp > FLOAT_MAX) ? FLOAT_MAX : tmp;
	}
	
	public int randomValue (int v1, int v2)
	{
		if (v2 > v1) {
			if (v2 == v1 + 1)
				return v1;
			return randomInt () % (v2 - v1) + v1;
		} else if (v1 > v2) {
			if (v1 == v2 + 1)
				return v2;
			return randomInt () % (v1 - v2) + v2;
		} else
			return v1;
	}

	public float randomValue (float v1, float v2)
	{
		if (v2 > v1)
			return randomFloat () * (v2 - v1) + v1;
		else if (v1 > v2)
			return randomFloat () * (v1 - v2) + v2;
		else
			return v1;
	}

	/* common methods */
	public string toString ()
	{
		return this.GetType().ToString() + "[ seed=" + seed + "]";
	}

}