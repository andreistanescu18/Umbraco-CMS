﻿using System;
using System.Collections.Generic;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence.Querying;

namespace Umbraco.Core.Persistence.Repositories
{
    public interface IMediaTypeRepository : IRepositoryQueryable<int, IMediaType>, IReadRepository<Guid, IMediaType>
    {
        /// <summary>
        /// Gets all entities of the specified <see cref="PropertyType"/> query
        /// </summary>
        /// <param name="query"></param>
        /// <returns>An enumerable list of <see cref="IContentType"/> objects</returns>
        IEnumerable<IMediaType> GetByQuery(IQuery<PropertyType> query);

        /// <summary>
        /// Creates a folder for content types
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="name"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        EntityContainer CreateContainer(int parentId, string name, int userId);

        /// <summary>
        /// Deletes a folder - this will move all contained content types into their parent
        /// </summary>
        /// <param name="folderId"></param>        
        void DeleteContainer(int folderId);

        IEnumerable<MoveEventInfo<IMediaType>> Move(IMediaType toMove, int parentId);
    }
}