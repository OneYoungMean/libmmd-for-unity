using System;
using System.IO;
using System.Text;
using LibMMD.Reader;
using UnityEngine;

namespace LibMMD.Util
{
    public static class MmdReaderUtil
    {
        /// <summary>
        /// 负责读取二进制流里面的详细数据,int负责调节长度,encoding负责读取的格式
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="length"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadStringFixedLength(BinaryReader reader, int length, Encoding encoding)
        {
            if (length < 0)
            {
                throw new MmdFileParseException("pmx string length is negative");
            }
            if (length == 0)
            {
                return "";
            }
            var bytes = reader.ReadBytes(length);
            var str = encoding.GetString(bytes);
            var end = str.IndexOf("\0", StringComparison.Ordinal);
            if (end >= 0)
            {
                str = str.Substring(0, end);
            }
            return str;
        }
        /// <summary>
        /// 读取开头一个int32负责字节长度,然后读取剩下的字节
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadSizedString(BinaryReader reader, Encoding encoding)
        {
            var length = reader.ReadInt32();
            return ReadStringFixedLength(reader, length, encoding);
        }
        /// <summary>
        /// 获取额外UV的一个v4
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Vector4 ReadVector4(BinaryReader reader)
        {
            var ret = new Vector4();
            ret[0] = MathUtil.NanToZero(reader.ReadSingle());//OYM：读取一个single,判断一下是不是nan,然后赋值
            ret[1] = MathUtil.NanToZero(reader.ReadSingle());
            ret[2] = MathUtil.NanToZero(reader.ReadSingle());
            ret[3] = MathUtil.NanToZero(reader.ReadSingle());
            return ret;
        }

        public static Quaternion ReadQuaternion(BinaryReader reader)
        {
            var ret = new Quaternion();
            ret.x = MathUtil.NanToZero(reader.ReadSingle());
            ret.y = MathUtil.NanToZero(reader.ReadSingle());
            ret.z = MathUtil.NanToZero(reader.ReadSingle());
            ret.w = MathUtil.NanToZero(reader.ReadSingle());
            return ret;
        }
        /// <summary>
        /// 就是负责读取一个单精度数值的坐标
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Vector3 ReadVector3(BinaryReader reader)
        {
            var ret = new Vector3();
            ret[0] = MathUtil.NanToZero(reader.ReadSingle());
            ret[1] = MathUtil.NanToZero(reader.ReadSingle());
            ret[2] = MathUtil.NanToZero(reader.ReadSingle());
            return ret;
        }
        
        public static Vector2 ReadVector2(BinaryReader reader)
        {
            var ret = new Vector2();
            ret[0] = MathUtil.NanToZero(reader.ReadSingle());
            ret[1] = MathUtil.NanToZero(reader.ReadSingle());
            return ret;
        }
        /// <summary>
        /// 字节信息属性的读取
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int ReadIndex(BinaryReader reader, int size)
        {
            switch (size)
            {
                case 1:
                    return reader.ReadSByte();
                case 2:
                    return reader.ReadUInt16();
                case 4:
                    return reader.ReadInt32();
                default:
                    throw new MmdFileParseException("invalid index size: " + size);
            }
        }
        /// <summary>
        /// 返回一个表示颜色的四元数
        /// </summary>
        /// <param name="reader"></param>
        /// 这里可以写吗
        /// <param name="readA"></param>
        /// <returns></returns>
        public static Color ReadColor(BinaryReader reader, bool readA)
        {

            var ret = new Color
            {
                r = reader.ReadSingle(),
                g = reader.ReadSingle(),
                b = reader.ReadSingle(),
                a = readA ? reader.ReadSingle() : 1.0f
            };
            return ret;
        }
        
        public static bool Eof(BinaryReader binaryReader)
        {
            var bs = binaryReader.BaseStream;
            return (bs.Position == bs.Length);
        }

    }
}