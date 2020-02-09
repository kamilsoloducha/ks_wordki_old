﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Wordki.Utils.Dapper;
using Wordki.Utils.Queries;

namespace Wordki.Queries.GetWordsFromGroup
{
    public class GetWordsFromGroupQueryHandler : IQueryManyHandler<GetWordsFromGroupQuery, GetWordsFromGroupDto>
    {
        private readonly IDbConnectionProvider dbConnection;

        public GetWordsFromGroupQueryHandler(IDbConnectionProvider dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<IEnumerable<GetWordsFromGroupDto>> HandleAsync(GetWordsFromGroupQuery query)
        {
            var param = new
            {
                groupId = query.GroupId
            };
            using(var connection = await dbConnection.Connect())
            {
                return await connection.GetAsync<GetWordsFromGroupDto>(sql, param);
            }
        }

        private readonly string sql = @$"
SELECT
w.id                as WordId,
w.language1         as Langauge1,
w.language2         as Language2,
w.drawer            as Drawer,
w.nextRepeat        as NextRepeat,
w.creationDate      as CreationDate,
count(r.id)         as RepeatsCount,
max(r.date)         as LastRepeat
FROM words w
LEFT JOIN repeats r ON r.wordId = w.id
JOIN groups g on g.id = w.groupId
WHERE g.id = @groupId
GROUP BY w.id";
    }
}