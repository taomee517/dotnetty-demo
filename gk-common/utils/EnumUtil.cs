using System;

namespace gk_common.utils
{
    public class EnumUtil
    {
        public static T ToEnum<T>(int intValue)
        {
            return (T) Enum.ToObject(typeof(T), intValue);
        }
    }
}