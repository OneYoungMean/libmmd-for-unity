using System.IO;
using LibMMD.Model;

namespace LibMMD.Reader
{
    public abstract class ModelReader
    {      
        /// <summary>
        /// 这里是一个父类的方法,但是他是在子类里被调用的,看起来好像很牛逼的样子(减少了引用)实际上我觉得还是挺奇怪的
        /// </summary>
        /// <param name="path"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public MmdModel Read(string path, ModelReadConfig config)
        {
            var fileStream = new FileStream(path, FileMode.Open);//OYM：打开文件
            var bufferedStream = new BufferedStream(fileStream);//OYM：缓存到这个类可以减少在不用时候产生性能的开销,等等那这里有个毛用啊
            var binaryReader = new BinaryReader(bufferedStream);//OYM：读取成二进制
            return Read(binaryReader, config);
        }

        /*备份一下
         * //OYM：这里补充一下using的知识点,using三种用法前两个就不说了,第三个的用法就是,在接下来的代码里面调用它
         * 反正我觉得这样写很傻逼
         *         public MmdModel Read(string path, ModelReadConfig config)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                using (var bufferedStream = new BufferedStream(fileStream)) {
                    using (var binaryReader = new BinaryReader(bufferedStream))
                    {
                        return Read(binaryReader, config);
                    }
                }
            }
        }
         */

        public abstract MmdModel Read(BinaryReader reader, ModelReadConfig config);
        
        public static MmdModel LoadMmdModel(string path, ModelReadConfig config)
        {
            var fileExt = new FileInfo(path).Extension.ToLower();//OYM：extension是扩展名,tolower是换成小写
            if (".pmd".Equals(fileExt))//OYM：是pmd
            {
                return new PmdReader().Read(path, config);
            }
            if (".pmx".Equals(fileExt))//OYM：是pmx
            {
                return new PmxReader().Read(path, config);//OYM：主要看这个吧,pmd的出bug就不管了
            }
            throw new MmdFileParseException("File " + path +
                                            " is not a MMD model file. File name should ends with \"pmd\" or \"pmx\".");//OYM：抛出错误
        }

    }
}