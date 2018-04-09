public class RandomYK:AbstractRandom
{

	/* static fields */
	private static readonly int A = 16807;
	private static readonly int M = 2147483647;
	private static readonly int Q = 127773;
	private static readonly int R = 2836;
	private static readonly int MASK = 123459876;

	/* constructors */
	public RandomYK ():base()
	{
	}

	public RandomYK (int seed):base (seed)
	{
	}

	public override int randomInt ()
	{
		int r = seed;
		r ^= MASK;
		int k = r / Q;
		r = A * (r - k * Q) - R * k;
		if (r < 0)
			r += M;
		seed = r;
		return r;
	}

}