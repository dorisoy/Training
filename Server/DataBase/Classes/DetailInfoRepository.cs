using Dapper;
using Dapper.Contrib.Extensions;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using Server.DataBase.Interfaces;
using System.Threading.Tasks;

namespace Server.DataBase.Classes
{

    class DetailInfoRepository : IDetailInfoRepository
    {

        readonly IRepository repository;


        public DetailInfoRepository(IRepository repository)
        {
            this.repository = repository;
        }


        public Task<ISTrainingPartResponse<DetailInfo>> GetDetailInfoAsync(int id, DetailInfoType type)
        {
            return repository.QueryAsync(con =>
            {
                return con.QueryFirstOrDefaultAsync<DetailInfo>($"SELECT * FROM detailInfo WHERE {type} = @peopleId", new { peopleId = id });
            });
        }


        public Task<ISTrainingPartResponse<bool>> AddOrUpdateAsync(DetailInfo detailInfo, DetailInfoType type)
        {
            return repository.QueryAsync(async con =>
            {
                if (detailInfo.Id > 0)
                    return await con.UpdateAsync(detailInfo);
                else return await con.InsertAsync(detailInfo) > 0;
            });
        }
    }
}
