using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;

namespace MSS.API.Common
{
    public static class UploadFileCommonHelper
    {
        /// <summary>
        /// �������ݿ��ѯ�����ȡ��Ӧ���ϴ���ʾ�ṹ
        /// </summary>
        /// <param name="ufs">���ݿ��ѯȨ�޽��</param>
        /// <returns>ǰ������Ҫ���ϴ���ʾ�ṹ</returns>
        public static List<object> ListShow(List<UploadFileModel> ufs)
        {
            List<object> objs = new List<object>();
            IEnumerable<IGrouping<int, UploadFileModel>> groupAction = ufs.GroupBy(a => a.Type);
            foreach (IGrouping<int, UploadFileModel> group in groupAction)
            {
                List<object> tmp = new List<object>();
                int type = 0;
                string typeName = "";
                foreach (UploadFileModel item in group)
                {
                    type = item.Type;
                    typeName = item.TypeName;
                    tmp.Add(new
                    {
                        type = item.Type,
                        typeName = item.TypeName,
                        id = item.ID,
                        name = item.FileName,
                        url = item.FilePath,
                        status = "success"
                    });
                }
                if (type != 0)
                {
                    objs.Add(new
                    {
                        type = type,
                        typeName = typeName,
                        list = tmp
                    });
                }
            }
            return objs;
        }

        /// <summary>
        /// �������ݿ��ѯ�����ȡ��Ӧ���ϴ�������ʾ�ṹ
        /// </summary>
        /// <param name="ufs">���ݿ��ѯȨ�޽��</param>
        /// <returns>ǰ������Ҫ���ϴ�������ʾ�ṹ</returns>
        public static List<object> CascaderShow(List<UploadFileModel> ufs)
        {
            List<object> objs = new List<object>();
            IEnumerable<IGrouping<int, UploadFileModel>> groupAction = ufs.GroupBy(a => a.Type);
            foreach (IGrouping<int, UploadFileModel> group in groupAction)
            {
                List<object> tmp = new List<object>();
                int type = 0;
                string typeName = "";
                foreach (UploadFileModel item in group)
                {
                    type = item.Type;
                    typeName = item.TypeName;
                    tmp.Add(new
                    {
                        type = item.Type,
                        typeName = item.TypeName,
                        value = item.ID,
                        label = item.FileName,
                        url = item.FilePath
                    });
                }
                if (type != 0)
                {
                    objs.Add(new
                    {
                        value = type,
                        label = typeName,
                        children = tmp
                    });
                }
            }
            return objs;
        }

        public static List<object> TimeLineShow(List<UploadFileModel> ehs)
        {
            List<object> objs = new List<object>();
            IEnumerable<IGrouping<int, UploadFileModel>> groupTypes = ehs.GroupBy(a => a.Type);
            // Ĭ����ʾʱ����ڵ������
            int stage = 1;
            string tag = "��������";
            int i = 0;
            foreach (var groupType in groupTypes)
            {
                if (i != 0)
                {
                    // ��ͬ�����У�������ǵ�һ������ʾ�ڵ��tag
                    stage = 0;
                    tag = "";
                }
                List<object> children = new List<object>();
                string content = "";
                // ��ͬ����
                foreach (UploadFileModel e in groupType)
                {
                    children.Add(new
                    {
                        id = e.ID,
                        name = e.FileName,
                        url = e.FilePath
                    });
                    content = e.TypeName;
                }
                objs.Add(new
                {
                    detailType = 0,
                    tag,
                    stage,
                    content,
                    children
                });
                i++;
            }
            return objs;
        }

    }
}
