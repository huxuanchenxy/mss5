using Dapper.FluentMap.Mapping;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MSS.API.Common.Utility
{
    public class ImportExcelHelper
    {
        public List<ClassView> classes { get; }
        public ImportExcelHelper(string fullName=null, string assemblyName=null)
        {
            if (!string.IsNullOrWhiteSpace(fullName) && !string.IsNullOrWhiteSpace(assemblyName))
            {
                classes = ListProperty(fullName, assemblyName);
            }
        }
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullName">命名空间.类型名</param>
        /// <param name="assemblyName">程序集</param>
        /// <returns></returns>
        private object CreateInstance(string fullName, string assemblyName)
        {
            string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
            Type o = Type.GetType(path);//加载类型
            object obj = Activator.CreateInstance(o, true);//根据类型创建实例
            return obj;
            //return (T)obj;//类型转换并返回
        }

        /// <summary>
        /// 类转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private DataTable TToDataTable()
        {
            DataTable dt = new DataTable();

            //PropertyInfo[] properties = typeof(T).GetProperties();
            //foreach (PropertyInfo property in properties)
            //{
            //    if (property.GetCustomAttributesData().Count > 0)
            //    {
            //        string propertyName = property.Name;
            //        Type propertyType = property.PropertyType;
            //        if ((propertyType.IsGenericType) && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            //        {
            //            propertyType = propertyType.GetGenericArguments()[0];
            //        }

            //        dt.Columns.Add(propertyName, propertyType);
            //    }
            //}
            foreach (var item in classes)
            {
                dt.Columns.Add(item.PropertyName, item.PropertyType);
            }
            return dt;
        }

        /// <summary>
        /// 获取类的属性列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<ClassView> ListProperty(string fullName, string assemblyName)
        {
            List<ClassView> ret = new List<ClassView>();
            string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
            Type o = Type.GetType(path);//加载类型
            PropertyInfo[] properties = o.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.GetCustomAttributesData().Count>0)
                {
                    ClassView cv = new ClassView();
                    var tmp = property.GetCustomAttribute<InfoAttribute>();
                    cv.Name = tmp.Name;
                    cv.Order = tmp.Order;
                    cv.Visible = tmp.Visiable;
                    cv.PropertyName = tmp.MapName;
                    Type propertyType = property.PropertyType;
                    if ((propertyType.IsGenericType) && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                    }
                    cv.PropertyType = propertyType;
                    ret.Add(cv);
                }
            }
            return ret.OrderBy(a=>a.Order).ToList();
        }

        public DataTable GetData(IFormFile file,string[] config,string[] required,int userID,ref ImportExcelLog log, ref string errMsg)
        {
            DataTable ret = TToDataTable();
            ISheet sheet = null;
            IWorkbook workbook = null;
            if (file.Length > 0)
            {
                //利用IFormFile里面的OpenReadStream()方法直接读取文件流
                Stream stream = file.OpenReadStream();
                string fileType = Path.GetExtension(file.FileName);
                log.FileName = Path.GetFileNameWithoutExtension(file.FileName);

                #region 判断excel版本
                //2007以上版本excel
                if (fileType == ".xlsx")
                {
                    workbook = new XSSFWorkbook(stream);
                }
                //2007以下版本excel
                else if (fileType == ".xls")
                {
                    workbook = new HSSFWorkbook(stream);
                }
                else
                {
                    errMsg = "传入的不是Excel文件";
                    return ret;
                }
                #endregion
                for (int sheetNo = 0; sheetNo < workbook.NumberOfSheets; sheetNo++)
                {
                    sheet = workbook.GetSheetAt(sheetNo);
                    if (sheet.GetRow(0) != null)
                    {
                        DataRow dataRow = null;
                        log.RecordNum = sheet.LastRowNum;
                        log.Field = "";
                        for (int i = 0; i < config.Length; i++)
                        {
                            log.Field += sheet.GetRow(0).GetCell(i).ToString().Trim();
                            if (i!= config.Length-1)
                            {
                                log.Field += ",";
                            }
                        }
                        //遍历行(首行为列名)
                        for (int j = 1; j <= sheet.LastRowNum; j++)
                        {
                            
                            IRow row = sheet.GetRow(j);
                            dataRow = ret.NewRow();
                            //遍历列
                            for (int i = 0; i < config.Length; i++)
                            {
                                int colNum =Convert.ToInt32(config[i]);
                                if (colNum == -1) continue;
                                ICell cellData = row.GetCell(i);
                                string str = cellData.ToString().Trim();
                                if (required[i]=="1" && string.IsNullOrWhiteSpace(str))
                                {
                                    errMsg = "第"+j+"行第"+i+"列为必填项";
                                    return ret;
                                }
                                dataRow[colNum] = str;
                            }
                            ret.Rows.Add(dataRow);
                        }
                    }
                }
            }

            return ret;
        }

        public List<string> GetExcelField(IFormFile file, ref string errMsg)
        {
            List<string> ret = new List<string>();
            ISheet sheet = null;
            IWorkbook workbook = null;
            if (file.Length > 0)
            {
                //利用IFormFile里面的OpenReadStream()方法直接读取文件流
                Stream stream = file.OpenReadStream();
                string fileType = Path.GetExtension(file.FileName);

                #region 判断excel版本
                //2007以上版本excel
                if (fileType == ".xlsx")
                {
                    workbook = new XSSFWorkbook(stream);
                }
                //2007以下版本excel
                else if (fileType == ".xls")
                {
                    workbook = new HSSFWorkbook(stream);
                }
                else
                {
                    errMsg = "传入的不是Excel文件";
                    return ret;
                }
                #endregion
                sheet = workbook.GetSheetAt(0);
                if (sheet.GetRow(0) != null)
                {
                    int i = 0;
                    while (true)
                    {
                        var cell = sheet.GetRow(0).GetCell(i);
                        if (cell==null) break;
                        else ret.Add(cell.ToString().Trim());
                        i++;
                    }
                }
            }

            return ret;
        }

        public void ToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }
            using (ShareFolderHelper helper = new ShareFolderHelper("test", "yfzx.2019", FilePath.CSVPATH + table.TableName + ".csv"))
            {
                File.WriteAllText(FilePath.CSVPATH + table.TableName + ".csv", sb.ToString());
            }
        }
    }
    public class ImportExcelLog
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public string Field { get; set; }
        public int RecordNum { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedName { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ImportExcelLogMap : EntityMap<ImportExcelLog>
    {
        public ImportExcelLogMap()
        {
            Map(o => o.FileName).ToColumn("file_name");
            Map(o => o.RecordNum).ToColumn("record_num");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedName).ToColumn("user_name");
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class InfoAttribute : Attribute
    {
        private string _name;
        private string _mapName;
        private int _order;
        private bool _visiable;
        public InfoAttribute(string name,string mapName,int order,bool visiable = true)
        {
            _name = name;
            _mapName = mapName;
            _order = order;
            _visiable = visiable;
        }
        public string Name
        {
            get { return _name; }
        }
        public string MapName
        {
            get { return _mapName; }
        }
        public int Order
        {
            get { return _order; }
        }
        public bool Visiable
        {
            get { return _visiable; }
        }
    }

    public class ClassView
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public bool Visible { get; set; }
        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
    }
}
