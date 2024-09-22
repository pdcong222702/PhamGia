using PhamGia.Core.DataTableObject.Attributes;

namespace PhamGia.Data
{
    public class Property
    {
        [DataNames("id")]
        public int ID { get; set; }

        [DataNames("property_code")]
        public string PropertyCode { get; set; }

        [DataNames("customer_name")]
        public string CustomerName { get; set; }

        [DataNames("customer_phone")]
        public string CustomerPhone { get; set; }

        [DataNames("district")]
        public string District { get; set; }

        [DataNames("ward")]
        public string Ward { get; set; }

        [DataNames("address_detail")]
        public string AddressDetail { get; set; }

        [DataNames("map_link")]
        public string MapLink { get; set; }

        [DataNames("direction")]
        public string Direction { get; set; }

        [DataNames("acreage")]
        public double? Acreage { get; set; }

        [DataNames("brand_acreage")]
        public string BrandAcreage { get; set; }

        [DataNames("main_street")]
        public string MainStreet { get; set; }

        [DataNames("description")]
        public string Description { get; set; }

        [DataNames("extensions")]
        public string Extensions { get; set; }

        [DataNames("street")]
        public string Street { get; set; }

        [DataNames("interior")]
        public string Interior { get; set; }

        [DataNames("price")]
        public double? Price { get; set; }

        [DataNames("check_price")]
        public double? CheckPrice { get; set; }

        [DataNames("price_unit")]
        public string PriceUnit { get; set; }

        [DataNames("price_by_acreage")]
        public string PriceByAcreage { get; set; }

        [DataNames("coordinate")]
        public string Coordinate { get; set; }

        [DataNames("requried")]
        public string Requried { get; set; }

        [DataNames("mnv")]
        public string MNV { get; set; }

        [DataNames("image_contact")]
        public string ImageContact { get; set; }

        [DataNames("image_sign")]
        public string ImageSign { get; set; }
        [DataNames("UserLogin")]
        public string UserLogin { get; set; }

        [DataNames("created_date")]
        public DateTime? CreatedDate { get; set; }
        [DataNames("date")]
        public DateTime? Date { get; set; }

        [DataNames("moved_date")]
        public DateTime? MovedDate { get; set; }

        [DataNames("status")]
        public string Status { get; set; }

        [DataNames("associate")]
        public string Associate { get; set; }

        [DataNames("is_tax")]
        public string IsTax { get; set; }

        [DataNames("user_updated")]
        public string UserUpdated { get; set; }

        [DataNames("brand")]
        public string Brand { get; set; }

        [DataNames("status_two")]
        public string StatusTwo { get; set; }
        [DataNames("brand_house")]
        public string BrandHouse {  get; set; }

        [DataNames("hien_trang")]
        public string HienTrang {  get; set; }


        [DataNames("pay_bank")]
        public string PayBank { get; set; }

        [DataNames("infomation")]
        public string Information { get; set; }

        [DataNames("show")]
        public string Show {  get; set; }

        [DataNames("week")]
        public string Week {  get; set; }

        [DataNames("month")]
        public string Month {  get; set; }

        [DataNames("facebook_link")]
        public string FacebookLink {  get; set; }
    }
}
