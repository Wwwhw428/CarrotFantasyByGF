using System.IO;
using System.Text;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.DataTable
{
    /// <summary>
    /// 场景配置表。
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class DRScene : DataRowBase
    {
        private int _id = 0;

        /// <summary>
        /// 获取场景编号。
        /// </summary>
        public override int Id => _id;

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取背景音乐编号。
        /// </summary>
        public int BackgroundMusicId
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            var columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (var i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            var index = 0;
            index++;
            _id = int.Parse(columnStrings[index++]);
            index++;
            AssetName = columnStrings[index++];
            BackgroundMusicId = int.Parse(columnStrings[index]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (var memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (var binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    _id = binaryReader.Read7BitEncodedInt32();
                    AssetName = binaryReader.ReadString();
                    BackgroundMusicId = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}