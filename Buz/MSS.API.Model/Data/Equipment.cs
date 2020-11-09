using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Dapper.FluentMap.Mapping;
using MSS.API.Common.Utility;

namespace MSS.API.Model.Data
{
    public class Equipment
    {
        [Info("设备编码","eqp_code", 0)]
        public string Code { get; set; }
        [Info("设备名称","eqp_name", 1)]
        public string Name { get; set; }
        [Info("设备类型ID","eqp_type", 2)]
        public int Type { get; set; }
        public string TName { get; set; }
        [Info("设备资产编号", "eqp_asset_number", 3)]
        public string AssetNo { get; set; }
        [Info("设备规格型号", "eqp_model", 4)]
        public string Model { get; set; }
        [Info("子系统ID", "sub_system", 5)]
        public int SubSystem { get; set; }
        public string SubSystemName { get; set; }
        [Info("管辖班组ID","team", 6)]
        public int Team { get; set; }
        [Info("管辖班组级联路径","team_path", 7,false)]
        public string TeamPath { get; set; }
        [Info("所属公司ID","top_org", 8)]
        public int TopOrg { get; set; }
        [Info("所属线路ID","line", 9)]
        public int Line { get; set; }
        public string LineName { get; set; }
        public string TeamName { get; set; }
        [Info("设备条码", "bar_code", 10)]
        public string BarCode { get; set; }
        [Info("设备概述", "discription", 11)]
        public string Desc { get; set; }
        [Info("供应商ID", "supplier", 12)]
        public int? Supplier { get; set; }
        public string SupplierName { get; set; }
        [Info("制造商ID", "manufacturer", 13)]
        public int? Manufacturer { get; set; }
        public string ManufacturerName { get; set; }
        [Info("设备序列号", "serial_number", 14)]
        public string SerialNo { get; set; }
        [Info("额定电压", "rated_voltage", 15)]
        public double? RatedVoltage { get; set; }
        [Info("额定电流", "rated_current", 16)]
        public double? RatedCurrent { get; set; }
        [Info("额定功率", "rated_power", 17)]
        public double? RatedPower { get; set; }
        [Info("安装位置ID", "location", 18)]
        public int Location { get; set; }
        /// <summary>
        /// 哪张表的位置ID
        /// </summary>
        [Info("安装站区ID", "location_by", 19)]
        public int LocationBy { get; set; }
        [Info("安装站区级联路径", "location_path", 20,false)]
        public string LocationPath { get; set; }
        public string LocationName { get; set; }
        [Info("上线日期", "online_date", 21)]
        public DateTime Online { get; set; }
        [Info("使用期限", "life", 22)]
        public int? Life { get; set; }
        [Info("中修频率", "medium_repair", 23)]
        public int MediumRepair { get; set; }
        [Info("大修频率", "large_repair", 24)]
        public int LargeRepair { get; set; }
        [Info("再次上线日期", "online_again", 25)]
        public DateTime? OnlineAgain { get; set; }
        public int ID { get; set; }
        [Info("创建人ID", "created_by", 27, false)]
        public int CreatedBy { get; set; }
        [Info("创建时间", "created_time", 26,false)]
        public DateTime CreatedTime { get; set; }
        [Info("更信人ID", "updated_by", 29, false)]
        public int UpdatedBy { get; set; }
        [Info("更新时间", "updated_time", 28, false)]
        public DateTime UpdatedTime { get; set; }
        [Info("逻辑删除标志", "is_del", 30, false)]
        public int IsDel { get; set; }
        [Info("下一个中修日期", "next_medium_repair_date", 31, false)]
        public DateTime NextMediumRepairDate { get; set; }
        [Info("下一个大修日期", "next_large_repair_date", 32, false)]
        public DateTime NextLargeRepairDate { get; set; }
        public string CreatedName { get; set; }
        public string UpdatedName { get; set; }
        /// <summary>
        /// 上传图片的id，用逗号隔开
        /// </summary>
        public string FileIDs { get; set; }
    }

    public class EquipmentMap : EntityMap<Equipment>
    {
        public EquipmentMap()
        {
            Map(o => o.Code).ToColumn("eqp_code");
            Map(o => o.Name).ToColumn("eqp_name");
            Map(o => o.Type).ToColumn("eqp_type");
            Map(o => o.TName).ToColumn("type_name");

            Map(o => o.AssetNo).ToColumn("eqp_asset_number");
            Map(o => o.Model).ToColumn("eqp_model");
            Map(o => o.SubSystem).ToColumn("sub_system");
            Map(o => o.SubSystemName).ToColumn("sub_code_name");
            Map(o => o.Team).ToColumn("team");
            Map(o => o.TeamPath).ToColumn("team_path");
            Map(o => o.TopOrg).ToColumn("top_org");
            Map(o => o.LineName).ToColumn("line_name");
            Map(o => o.TeamName).ToColumn("name");
            Map(o => o.BarCode).ToColumn("bar_code");
            Map(o => o.Desc).ToColumn("discription");

            Map(o => o.Supplier).ToColumn("supplier");
            Map(o => o.SupplierName).ToColumn("sname");
            Map(o => o.Manufacturer).ToColumn("manufacturer");
            Map(o => o.ManufacturerName).ToColumn("mname");
            Map(o => o.SerialNo).ToColumn("serial_number");

            Map(o => o.RatedVoltage).ToColumn("rated_voltage");
            Map(o => o.RatedCurrent).ToColumn("rated_current");
            Map(o => o.RatedPower).ToColumn("rated_power");

            Map(o => o.Location).ToColumn("location");
            Map(o => o.LocationPath).ToColumn("location_path");
            Map(o => o.LocationBy).ToColumn("location_by");
            Map(o => o.LocationName).ToColumn("AreaName");
            Map(o => o.Online).ToColumn("online_date");
            Map(o => o.Life).ToColumn("life");

            Map(o => o.MediumRepair).ToColumn("medium_repair");
            Map(o => o.LargeRepair).ToColumn("large_repair");
            Map(o => o.OnlineAgain).ToColumn("online_again");

            Map(o => o.CreatedBy).ToColumn("created_by");
            Map(o => o.CreatedName).ToColumn("created_name");
            Map(o => o.CreatedTime).ToColumn("created_time");
            Map(o => o.UpdatedBy).ToColumn("updated_by");
            Map(o => o.UpdatedName).ToColumn("updated_name");
            Map(o => o.UpdatedTime).ToColumn("updated_time");
            Map(o => o.IsDel).ToColumn("is_del");

            Map(o => o.NextMediumRepairDate).ToColumn("next_medium_repair_date");
            Map(o => o.NextLargeRepairDate).ToColumn("next_large_repair_date");
        }
    }

    public class EqpQueryParm:BaseQueryParm
    {
        public int? SearchSubSystem { get; set; }
        public int? SearchType { get; set; }
        public string SearchCode { get; set; }
        public int? SearchLocation { get; set; }

        public int? SearchLocationBy { get; set; }
        public int? SearchTopOrg { get; set; }
        public int? SearchLine { get; set; }
        // 根据location_path字段查询
        public string LocationPath { get; set; }
    }

    public class EqpQueryByIDParm : BaseQueryParm
    {
        public List<int> IDs { get; set; }
    }

    public class AllArea
    {
        public int ID { get; set; }
        public string AreaName { get; set; }
        public int Tablename { get; set; }
    }

    public class EqpView
    {
        public List<Equipment> rows { get; set; }
        public int total { get; set; }
    }
}
