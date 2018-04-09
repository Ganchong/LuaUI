
public class TextValidity
{

	/* fields */
	char[] charRangeSet;
	string[] invalidTexts;
	

	/* properties */
	public char[] getCharRangeSet ()
	{
		return charRangeSet;
	}

	public void setCharRangeSet (char[] rangeSet)
	{
		charRangeSet = rangeSet;
	}

	public string[] getInvalidTexts ()
	{
		return invalidTexts;
	}

	public void setInvalidTexts (string[] strings)
	{
		invalidTexts = strings;
	}
	/* methods */
	public string valid (string str)
	{
		return valid (str, false);
	}

	public string valid (string str, bool caseless)
	{
		if (str == null || str.Length < 1)
			return null;
		char c = TextKit.valid (str, charRangeSet);
		if (c > 0)
			return c.ToString ();
		string[] strs = invalidTexts;
		if (strs == null)
			return null;
		if (caseless) {
			str = str.ToLower ();
			for (int i=0,n=strs.Length; i<n; i++) {
				if (str.IndexOf (strs [i].ToLower ()) >= 0)
					return strs [i];
			}
		} else {
			for (int i=0,n=strs.Length; i<n; i++) {
				if (str.IndexOf (strs [i]) >= 0)
					return strs [i];
			}
		}
		return null;
	}

}
