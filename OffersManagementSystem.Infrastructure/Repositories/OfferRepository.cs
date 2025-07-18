using Dapper;
using OffersManagementSystem.Application.IData;
using OffersManagementSystem.Application.IRepositories;
using OffersManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersManagementSystem.Infrastructure.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly IAppDbDapper<Offer> _dapper;

        public OfferRepository(IAppDbDapper<Offer> dapper)
        {
            this._dapper = dapper;
        }
        public async Task<int> CreateOfferAsync(Offer offer)
        {
            var sql = @"INSERT INTO [dbo].[Offers]
           ([Serial]
           ,[OfferAddress]
           ,[OfferDate]
           ,[OfferDayes]
           ,[Total]
           ,[Details]
           ,[FilePath])
     VALUES
           (@Serial
           ,@OfferAddress
           ,@OfferDate
           ,@OfferDayes
           ,@Total
           ,@Details
           ,@FilePath)";

            var parameters = new
            {
                @Serial = offer.Serial,
                @OfferAddress = offer.OfferAddress,
                @OfferDate = offer.OfferDate,
                @OfferDayes = offer.OfferDayes,
                @Total = offer.Total,
                @Details = offer.Details,
                @FilePath = offer.FilePath
            };

            var result = await _dapper.ExecuteAsync(sql, parameters);

            return result;
        }

        public async Task<int> DeleteOfferAsync(int id)
        {
            var sql = "Delete from [dbo].[Offers] where id = @id";

            var parameters = new { @id = id };

            var result = await _dapper.ExecuteAsync(sql, parameters);

            return result;
        }

        public async Task<IEnumerable<Offer>> GetAllOffersAsync(string? serial, string? offerAddress, DateOnly? fromDate, DateOnly? toDate, bool? active, decimal? totalFrom, decimal? totalTo)
        {
            var sql = $"\r\nselect " +
                      $"\r\n\t\t\t[Id]" +
                      $"\r\n\t\t   ,[Serial]" +
                      $"\r\n           ,[OfferAddress]" +
                      $"\r\n           ,[OfferDate]" +
                      $"\r\n           ,[OfferDayes]" +
                      $"\r\n           ,[Total]" +
                      $"\r\n           ,[Details]" +
                      $"\r\n           ,[FilePath]" +
                      $"\r\nFrom [dbo].[Offers]" +
                      $"\r\nWhere 1=1";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(serial))
            {
                sql += " and Serial = @Serial";
                parameters.Add("@Serial", serial);
            }

            if (!string.IsNullOrEmpty(offerAddress))
            {
                sql += " and OfferAddress = @OfferAddress";
                parameters.Add("@OfferAddress", offerAddress);
            }

            if(active.HasValue)
            {
                var today = DateTime.Now.ToString("yyyy-MM-dd");
                if (active.Value == true)
                {
                    sql += " and DATEADD(Day, [OfferDayes], [OfferDate]) > @today";
                    parameters.Add("@today", today);
                }
                else
                {
                    sql += " and DATEADD(Day, [OfferDayes], [OfferDate]) <= @today";
                    parameters.Add("@today", today);
                }

            }

            if (fromDate.HasValue)
            {
                sql += " and OfferDate >= @FromDate";
                parameters.Add("@FromDate", fromDate.Value);
            }

            if (toDate.HasValue)
            {
                sql += " and OfferDate <= @ToDate";
                parameters.Add("@ToDate", toDate.Value);
            }

            if (totalFrom != null)
            {
                sql += " and Total >= @TotalFrom";
                parameters.Add("@TotalFrom", totalFrom);
            }

            if (totalTo != null)
            {
                sql += " and Total <= @TotalTo";
                parameters.Add("@TotalTo", totalTo);
            }

            sql += " order by OfferDate desc";

            return await _dapper.QueryAsync(sql, parameters);
        }

        public async Task<Offer> GetOfferByIdAsync(long id)
        {
            var sql = $"select " +
                      $"\r\n\t\t\t[Id]" +
                      $"\r\n\t\t   ,[Serial]" +
                      $"\r\n           ,[OfferAddress]" +
                      $"\r\n           ,[OfferDate]" +
                      $"\r\n           ,[OfferDayes]" +
                      $"\r\n           ,[Total]" +
                      $"\r\n           ,[Details]" +
                      $"\r\n           ,[FilePath]" +
                      $"\r\nFrom [dbo].[Offers]" +
                      $"\r\nWhere Id = @Id";

            var parameters = new { @Id = id };

            return await _dapper.QueryFirstOrDefaultAsync(sql, parameters);
        }

        public async Task<int> UpdateOfferAsync(Offer offer)
        {
            var sql = $"Update [dbo].[Offers]" +
                      $"\r\nSET" +
                      $"\r\n\t\t\t[Serial] = @Serial" +
                      $"\r\n           ,[OfferAddress] = @OfferAddress" +
                      $"\r\n           ,[OfferDate] = @OfferDate" +
                      $"\r\n           ,[OfferDayes] = @OfferDayes" +
                      $"\r\n           ,[Total] = @Total" +
                      $"\r\n           ,[Details] = @Details" +
                      $"\r\n           ,[FilePath] = @FilePath" +
                      $"\r\nWhere Id = @Id";

            var parameters = new
            {
                @Id = offer.Id,
                @Serial = offer.Serial,
                @OfferAddress = offer.OfferAddress,
                @OfferDate = offer.OfferDate,
                @OfferDayes = offer.OfferDayes,
                @Total = offer.Total,
                @Details = offer.Details,
                @FilePath = offer.FilePath
            };

            return await _dapper.ExecuteAsync(sql, parameters);
        }
    }
}
