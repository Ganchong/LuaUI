using UnityEngine;
using System.Collections;
using System;

public class RandomKit
{
	private static RandomYK random = new RandomYK ();
	public static readonly string[] SPLIT_ARGS1 = {"#"};
	public static readonly string[] SPLIT_ARGS2 = {","};
	public static readonly string[] SPLIT_ARGS3 = {":"};
	public static readonly string[] SPLIT_ARGS4 = {"~"};

	public static  int randomValue (int startIndex, int endIndex)
	{
		return random.randomValue (startIndex, endIndex);
	}

	public static float randomValue (float startIndex, float endIndex)
	{
		return random.randomValue (startIndex, endIndex);
	}

	public static int getRandomNumber (string args)
	{
		return getRandomNumber (args, random);
	}

	public static int[] getRandomNumbers (string args)
	{
		return getRandomNumbers (args, random);
	}

	public static int[] getRandomNumbers (string args, RandomYK random)
	{
		string[] strs = args.Split (SPLIT_ARGS1, System.StringSplitOptions.None);
		int[] ints = new int[strs.Length];
		for (int i=0; i<ints.Length; i++) {
			ints [i] = getRandomNumber (strs [i], random);
		}
		return ints;
	}
	/**
	 * 数值抽取器(从枚举值,范围随机值,定值 中随机抽出一个值) 举例如下 枚举值(支持单个出现概率)：1,2,4 或
	 * 1:10,2:30,4:30 范围值： 1~10 定值：12 支持混合使用 如：1~10,44~89,2~5 又如 2~10:40
	 * 支持概率后缀 代表该值被抽取出来的几率 值越高被抽出的概率越大 并不限定后缀值的范围 如：1:20,1~4:30,4:500
	 */
	public static int getRandomNumber (string args, RandomYK random)
	{
		if (random == null) {
			random = RandomKit.random;
		}
		string[] tmp = null;
		if (args.IndexOf (SPLIT_ARGS2 [0]) > 0) {
			string[] values = args.Split (SPLIT_ARGS2, System.StringSplitOptions.None);
			int[] numbers = new int[values.Length];
			int[] ranCount = new int[values.Length];
			string valuesStr = null, ranStr;
			int sum = 0;
			// 结果索引
			if (args.IndexOf (SPLIT_ARGS3 [0]) > 0) {
				for (int i=0; i<values.Length; i++) {
					int endIndex = values [i].IndexOf (SPLIT_ARGS3 [0]);
					if (endIndex <= 0) {
						throw new ArgumentException (
							"err VariableAward Sample args:" + args);
					}
					valuesStr = values [i].Substring (0, endIndex);
					ranStr = values [i].Substring (endIndex + 1,
						values [i].Length - (endIndex+1));
					ranCount [i] = int.Parse (ranStr);
					sum += ranCount [i];
					if (valuesStr.IndexOf (SPLIT_ARGS4 [0]) > 0) {
						tmp = valuesStr.Split (SPLIT_ARGS4, System.StringSplitOptions.None);
						numbers [i] = random.randomValue (int
							.Parse (tmp [0]), int.Parse (tmp [1]) + 1);
					} else {
						numbers [i] = int.Parse (valuesStr);
					}
				}
				int ranNum = random.randomValue (0, sum);
				int start = 0, end = 0;
				for (int i=0; i<ranCount.Length; i++) {
					start = end;
					end = start + ranCount [i];
					if (ranNum < end && ranNum >= start) {
						return numbers [i];
					}
				}
			} else {
				for (int i=0; i<values.Length; i++) {
					if (values [i].IndexOf (SPLIT_ARGS4 [0]) > 0) {
						tmp = values [i].Split (SPLIT_ARGS4, System.StringSplitOptions.None);
						numbers [i] = random.randomValue (int
							.Parse (tmp [0]), int.Parse (tmp [1]) + 1);
					} else {
						numbers [i] = int.Parse (values [i]);
					}
				}
				return numbers [random.randomValue (0, numbers.Length)];
			}
		}
		if (args.IndexOf (SPLIT_ARGS4 [0]) > 0) {
			tmp = args.Split (SPLIT_ARGS4, System.StringSplitOptions.None);
			return random.randomValue (int.Parse (tmp [0]), int
				.Parse (tmp [1]) + 1);
		}
		int n = 0;
		try {
			n = int.Parse (args);
		} catch (FormatException e) {
			throw new ArgumentException (
				"err VariableAward Sample args:" + args, e);
		}
		return n;
	}
}
