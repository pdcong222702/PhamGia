using Microsoft.JSInterop;
using PhamGia.Data;
using Radzen;

namespace PhamGia.PhamGiaLib.impl
{
    public interface IBDSContext
    {    
        ResponseMessage Login(User user);
        ResponseMessage GetBDS();

        ResponseMessage AddBDS(Property prop);

        ResponseMessage FilterBDS(string direction,string acreage, string mainStreet,string price,string priceByAcreage
            ,string brand, string status, string isTax,string brandAcreage, string priceUnit,string ward);

        ResponseMessage DeleteBDS();
    }
}
