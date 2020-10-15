using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using static MSS.API.Common.FilePath;
using System.Text;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace MSS.API.Common.Utility
{
    public class PDFHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder">所保存的文件夹名称</param>
        /// <returns>所保存的相对路径</returns>

        /// <summary>
        /// 上传一个pdf文件
        /// </summary>
        /// <param name="file">上传的文件列表</param>
        /// <param name="type">上传的文件类型，即与哪张表相关联</param>
        /// <returns>存入数据库的路径</returns>
        public string GetSavePDFPath(List<IFormFile> file, int type,int systemResource)
        {
            //string folder = Enum.GetName(typeof(FileType), type) + "/";
            string basepath = (BASEFILE + SHAREFILE).Replace('/', '\\');
            createDirectory(basepath);
            basepath = (BASEFILE + SHAREFILE + systemResource + "/").Replace('/', '\\');
            createDirectory(basepath);
            basepath = (basepath + type+"/").Replace('/', '\\');
            createDirectory(basepath);
            if (file.Count > 0)
            {
                foreach (IFormFile item in file)
                {
                    string fileName = item.FileName;
                    string ext = fileName.Substring(fileName.LastIndexOf("."));
                    string fileNameNew = Guid.NewGuid().ToString();
                    return SHAREFILE + systemResource + "/" + type + "/" + fileNameNew + ext;
                }
            }
            return "";
        }

        private void createDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        /// <summary>
        /// 删除一个已上传的pdf文件
        /// </summary>
        /// <param name="path"></param>
        public void DeletePDF(string path)
        {
            string myPath = (BASEFILE + path).Replace('/', '\\');
            File.Delete(myPath);
        }
        public void SavePDF(IFormFile file,string path)
        {
            string filePath = (BASEFILE + path).Replace('/', '\\');
            SaveFile(file, filePath);
        }

        public void SaveFile(IFormFile file, string path)
        {
            using (FileStream fs = File.Create(path))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }
    }

    public class UploadHelper
    {
        public string ListToTreeJson(List<UploadFileRelation> ufets)
        {
            List<object> objs = new List<object>();
            IEnumerable<IGrouping<int, UploadFileRelation>> groups = ufets.GroupBy(a => a.Type);
            foreach (var group in groups.OrderBy(a => a.Key))
            {
                string label = "";
                List<object> children = new List<object>();
                foreach (var item in group)
                {
                    label = item.TName;
                    children.Add(new
                    {
                        value = item.FilePath,
                        label = item.FileName
                    });
                }
                objs.Add(new
                {
                    value = group.Key,
                    label = label,
                    children = children
                });
            }
            return JsonConvert.SerializeObject(objs);
        }

        public class UploadFileRelation
        {
            public int ID { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public int EntityID { get; set; }
            public int Type { get; set; }
            public string TName { get; set; }
        }

    }

}
