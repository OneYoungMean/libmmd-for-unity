using System.IO;
using LibMMD.Model;

namespace LibMMD.Reader
{
    public abstract class ModelReader
    {      
        /// <summary>
        /// ������һ������ķ���,���������������ﱻ���õ�,�����������ţ�Ƶ�����(����������)ʵ�����Ҿ��û���ͦ��ֵ�
        /// </summary>
        /// <param name="path"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public MmdModel Read(string path, ModelReadConfig config)
        {
            var fileStream = new FileStream(path, FileMode.Open);//OYM�����ļ�
            var bufferedStream = new BufferedStream(fileStream);//OYM�����浽�������Լ����ڲ���ʱ��������ܵĿ���,�ȵ��������и�ë�ð�
            var binaryReader = new BinaryReader(bufferedStream);//OYM����ȡ�ɶ�����
            return Read(binaryReader, config);
        }

        /*����һ��
         * //OYM�����ﲹ��һ��using��֪ʶ��,using�����÷�ǰ�����Ͳ�˵��,���������÷�����,�ڽ������Ĵ������������
         * �����Ҿ�������д��ɵ��
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
            var fileExt = new FileInfo(path).Extension.ToLower();//OYM��extension����չ��,tolower�ǻ���Сд
            if (".pmd".Equals(fileExt))//OYM����pmd
            {
                return new PmdReader().Read(path, config);
            }
            if (".pmx".Equals(fileExt))//OYM����pmx
            {
                return new PmxReader().Read(path, config);//OYM����Ҫ�������,pmd�ĳ�bug�Ͳ�����
            }
            throw new MmdFileParseException("File " + path +
                                            " is not a MMD model file. File name should ends with \"pmd\" or \"pmx\".");//OYM���׳�����
        }

    }
}