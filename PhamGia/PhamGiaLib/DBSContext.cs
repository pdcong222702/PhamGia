using PhamGia.Common.Interface;
using PhamGia.Data;
using PhamGia.PhamGiaLib.impl;
using System.Data.SqlClient;
using System.Data;
using PhamGia.Common;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

namespace PhamGia.PhamGiaLib
{
    public class DBSContext : IBDSContext
    {
        public IDataProvider _dataProvider { get; }

        private readonly ISerilogProvider _serilogProvider;

        private ICommon _common { get; }

        public IErrorHandle ErrorHandler { get; }

        private IConfiguration _configuration { get; }
        private string ConnectionStringPM;
        public DBSContext(IDataProvider dataProvider, ICommon common, IConfiguration configuration, IErrorHandle errorHandler, ISerilogProvider serilogProvider)
        {

            this._common = common;
            this._dataProvider = dataProvider;
            this.ErrorHandler = errorHandler;
            this._serilogProvider = serilogProvider;
            this.ConnectionStringPM = "Data Source=LAPTOP-OK982IIR\\SQLEXPRESS;Database=PhamGia_SNS1;User Id=sa;Password=1";
            this._configuration = configuration;

        }
        public ResponseMessage GetBDS()
        {
            ResponseMessage cResponse = new ResponseMessage();

            try
            {
                cResponse.Data = this._dataProvider.GetDatasetFromSp_2(DataServiceConfig.SPPG_GETALLBDS, this.ConnectionStringPM);
                if (cResponse.Data == null)
                {
                    cResponse.Code = "404";
                }
            }
            catch (Exception e)
            {
                this.ErrorHandler.WriteToFile(e);
                throw;
            }
            return cResponse;
        }

        public ResponseMessage Login(User user)
        {
            ResponseMessage cResponse = new ResponseMessage();
            var sp_Params = new[]
            {
                // 2 params
                new SqlParameter()
                {
                    ParameterName = "@Username",
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.VarChar,
                    Size = 255,
                    Value = user.Username
                },
                new SqlParameter()
                {
                    ParameterName = "@Password",
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.VarChar,
                    Size = 255,
                    Value = user.Password
                },

            };
            cResponse = _dataProvider.GetResponseFromExecutedSp(DataServiceConfig.SPPG_LOGIN, sp_Params, this.ConnectionStringPM);


            return cResponse;
        }

        public ResponseMessage AddBDS(Property prop)
        {
            IDbDataParameter[] listParam =
            {

                new SqlParameter()
                {
                   ParameterName = "Coornidate",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                   Value = prop.Coordinate
                },
                new SqlParameter()
                {
                   ParameterName = "PropertyCode",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                   Value = prop.PropertyCode
                },
                new SqlParameter()
                {
                   ParameterName = "CustomerName",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                   Value= prop.CustomerName
                },
                new SqlParameter()
                {
                   ParameterName = "CustomerPhone",
                   SqlDbType = SqlDbType.NChar,
                   Direction = ParameterDirection.Input,
                   Value = prop.CustomerPhone
                },
                new SqlParameter()
                {
                   ParameterName = "Brand",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                   Value = prop.Brand
                },
                new SqlParameter()
                {
                   ParameterName = "District",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                   Value = prop.District
                },
                new SqlParameter()
                {
                   ParameterName = "Ward",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                   Value = prop.Ward
                },
                new SqlParameter()
                {
                   ParameterName = "AddressDetail",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                   Value = prop.AddressDetail
                },
                new SqlParameter()
                {
                   ParameterName = "MapLink",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                   Value = prop.MapLink
                },
                new SqlParameter()
                {
                   ParameterName = "Direction",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                   Value = prop.Direction
                },
                new SqlParameter()
                {
                   ParameterName = "Acreage",
                   SqlDbType = SqlDbType.Float,
                   Direction = ParameterDirection.Input,
                   Value = prop.Acreage
                },
                  new SqlParameter()
                {
                   ParameterName = "PriceByAcreage",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                   Value = prop.PriceByAcreage
                }
                   ,
                new SqlParameter()
                {
                    ParameterName = "MainStreet",
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.NVarChar,
                    Value = prop.MainStreet
                },
                  new SqlParameter()
                {
                   ParameterName = "Description",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                    Value = prop.Description
                },

                  new SqlParameter()
                {
                   ParameterName = "Extensions",
                   SqlDbType = SqlDbType.NVarChar,
                   Direction = ParameterDirection.Input,
                    Value = prop.Extensions
                },
                  new SqlParameter()
                {
                   ParameterName = "Street",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.Street,
                   Direction = ParameterDirection.Input,
                },
                    new SqlParameter()
                {
                   ParameterName = "Interior",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.Interior,
                   Direction = ParameterDirection.Input,
                },


                new SqlParameter()
                {
                   ParameterName = "Price",
                   SqlDbType = SqlDbType.Float,
                   Value = prop.Price,
                   Direction = ParameterDirection.Input,
                },
                new SqlParameter()
                {
                   ParameterName = "CheckPrice",
                   SqlDbType = SqlDbType.Float,
                   Value = prop.CheckPrice,
                   Direction = ParameterDirection.Input,
                },
                new SqlParameter()
                {
                   ParameterName = "PriceUnit",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.PriceUnit,
                   Direction = ParameterDirection.Input,
                },
                new SqlParameter()
                {
                   ParameterName = "Requried",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.Requried,
                   Direction = ParameterDirection.Input,
                },
                new SqlParameter()
                {
                   ParameterName = "ImageContact",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.ImageContact,
                   Direction = ParameterDirection.Input,
                },
                new SqlParameter()
                {
                   ParameterName = "ImageSign",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.ImageSign,
                   Direction = ParameterDirection.Input,
                },
                new SqlParameter()
                {
                   ParameterName = "MNV",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.MNV,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "CreatedDate",
                   SqlDbType = SqlDbType.DateTime,
                   Value = prop.CreatedDate,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "MovedDate",
                   SqlDbType = SqlDbType.DateTime,
                   Value = prop.MovedDate,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "Status",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.Status,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "Associate",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.Associate,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "IsTax",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.IsTax,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "UserUpdated",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.UserUpdated,
                   Direction = ParameterDirection.Input,
                }

                ,
                new SqlParameter()
                {
                   ParameterName = "Show",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.Show,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "PayBank",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.PayBank,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "Information",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.Information,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "BrandAcreage",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.BrandAcreage,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "UserLogin",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.UserLogin,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "Date",
                   SqlDbType = SqlDbType.DateTime,
                   Value = prop.Date,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "StatusTwo",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.StatusTwo,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "BrandHouse",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.BrandHouse,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "HienTrang",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.HienTrang,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "Week",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.Week,
                   Direction = ParameterDirection.Input,
                }
                ,
                new SqlParameter()
                {
                   ParameterName = "Month",
                   SqlDbType = SqlDbType.NVarChar,
                   Value = prop.Month,
                   Direction = ParameterDirection.Input,
                }
            };
            return _dataProvider.GetResponseFromExecutedSp(DataServiceConfig.SPPG_ADDBDS, listParam, this.ConnectionStringPM);
        }

        public ResponseMessage FilterBDS(string direction, string acreage, string mainStreet, string price, string priceByAcreage
            , string brand, string status, string isTax, string brandAcreage, string priceUnit,string ward)
        {
            ResponseMessage cResponse = new ResponseMessage();
            try
            {
                IDbDataParameter[] listParam =
                {
                    new SqlParameter()
                    {
                       ParameterName = "Direction",
                       SqlDbType = SqlDbType.NVarChar,
                       Value = direction,
                       Direction = ParameterDirection.Input,
                    }
                    ,
                    new SqlParameter()
                    {
                       ParameterName = "Acreage",
                       SqlDbType = SqlDbType.NVarChar,
                       Value = acreage,
                       Direction = ParameterDirection.Input,
                    }
                    ,
                    new SqlParameter()
                    {
                       ParameterName = "MainStreet",
                       SqlDbType = SqlDbType.NVarChar,
                       Value = mainStreet,
                       Direction = ParameterDirection.Input,
                    }
                    ,
                    new SqlParameter()
                    {
                       ParameterName = "Price",
                       SqlDbType = SqlDbType.NVarChar,
                       Value = price,
                       Direction = ParameterDirection.Input,
                    }

                    ,
                    new SqlParameter()
                    {
                       ParameterName = "PriceByAcreage",
                       SqlDbType = SqlDbType.NVarChar,
                       Value = priceByAcreage,
                       Direction = ParameterDirection.Input,
                    }
                    ,
                    new SqlParameter()
                    {
                       ParameterName = "Brand",
                       SqlDbType = SqlDbType.NVarChar,
                       Value = brand,
                       Direction = ParameterDirection.Input,
                    }
                    ,
                    new SqlParameter()
                    {
                       ParameterName = "Status",
                       SqlDbType = SqlDbType.NVarChar,
                       Value = status,
                       Direction = ParameterDirection.Input,
                    }
                    ,
                    new SqlParameter()
                    {
                       ParameterName = "IsTax",
                       SqlDbType = SqlDbType.NVarChar,
                       Value = isTax,
                       Direction = ParameterDirection.Input,
                    }
                    ,
                    new SqlParameter()
                    {
                       ParameterName = "BrandAcreage",
                       SqlDbType = SqlDbType.NVarChar,
                       Value = brandAcreage,
                       Direction = ParameterDirection.Input,
                    }
                    ,
                    new SqlParameter()
                    {
                       ParameterName = "PriceUnit",
                       SqlDbType = SqlDbType.NVarChar,
                       Value = priceUnit,
                       Direction = ParameterDirection.Input,
                    }
                    ,
                    new SqlParameter()
                    {
                       ParameterName = "Ward",
                       SqlDbType = SqlDbType.NVarChar,
                       Value = ward,
                       Direction = ParameterDirection.Input,
                    }
                };
                cResponse.Data = this._dataProvider.GetDatasetFromSp(DataServiceConfig.SPPG_FILTERBDS, listParam, this.ConnectionStringPM);
                if (cResponse.Data == null)
                {
                    cResponse.Code = "404";
                }
            }
            catch (Exception e)
            {
                this.ErrorHandler.WriteToFile(e);
                throw;
            }
            return cResponse;
        }

        public ResponseMessage DeleteBDS()
        {
            ResponseMessage cResponse = new ResponseMessage();

            try
            {
                cResponse.Data = this._dataProvider.GetDatasetFromSp_2(DataServiceConfig.SPPG_DELBDS, this.ConnectionStringPM);
                if (cResponse.Data == null)
                {
                    cResponse.Code = "404";
                }
            }
            catch (Exception e)
            {
                this.ErrorHandler.WriteToFile(e);
                throw;
            }
            return cResponse;
        }
    }
}
