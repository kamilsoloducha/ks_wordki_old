﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Wordki.Api.Domain2;
using Wordki.Api.Repositories.EntityFrameworkRepositories;
using Wordki.Utils.HttpContext;

namespace Wordki.Api.Featuers.Card.GetCount
{
    public class GetCountQueryHandler : IRequestHandler<GetCountQuery, int>
    {
        private readonly WordkiDbContext2 dbContext;
        private readonly IHttpContextProvider contextProvider;

        public GetCountQueryHandler(WordkiDbContext2 dbContext, IHttpContextProvider contextProvider)
        {
            this.dbContext = dbContext;
            this.contextProvider = contextProvider;
        }

        public async Task<int> Handle(GetCountQuery request, CancellationToken cancellationToken)
        {
            var userId = contextProvider.GetUserId();
            return await dbContext.Cards.CountAsync(c => c.Group.Owner.Id == userId);
        }
    }
}
