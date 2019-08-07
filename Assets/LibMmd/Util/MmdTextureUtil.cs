using LibMMD.Material;

namespace LibMMD.Util
{
    public static class MmdTextureUtil
    {
        /// <summary>
        /// ����Ϊʲô��д����....ֻ׼��10����...
        /// </summary>
        private static readonly string[] GlobalToonNames =
        {
            "toon0.bmp",
            "toon01.bmp",
            "toon02.bmp",
            "toon03.bmp",
            "toon04.bmp",
            "toon05.bmp",
            "toon06.bmp",
            "toon07.bmp",
            "toon08.bmp",
            "toon09.bmp",
            "toon10.bmp"
        };
        /// <summary>
        /// ��ȡȫ�� ��toon
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public static MmdTexture GetGlobalToon(int index, string rootPath)
        {
            if (index >= 0 && index < GlobalToonNames.Length - 1)
            {
                return new MmdTexture(GlobalToonNames[index + 1]);
            }
            return null;
        }
    }
}