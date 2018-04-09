using System;
using System.Runtime.InteropServices;

/// <summary>
/// 快速类型转换
/// </summary>
public static class FastBitConverter
{
	/* static methods */
	/** 浮点转整数 */
	public static int FloatToInt(float value)
	{
		ConverterHelperFloat helper=new ConverterHelperFloat();
		helper.Afloat=value;
		return helper.Aint;
	}
	/** 整数转浮点 */
	public static float IntToFloat(int value)
	{
		ConverterHelperFloat helper=new ConverterHelperFloat();
		helper.Aint=value;
		return helper.Afloat;
	}

    [StructLayout(LayoutKind.Explicit)]
    private struct ConverterHelperFloat
    {
        [FieldOffset(0)]
        public int Aint;

        [FieldOffset(0)]
        public float Afloat;
    }
}