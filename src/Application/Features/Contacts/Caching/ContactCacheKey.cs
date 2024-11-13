﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This file is part of the CleanArchitecture.Blazor project.
//     Licensed to the .NET Foundation under the MIT license.
//     See the LICENSE file in the project root for more information.
//
//     Author: neozhu
//     Created Date: 2024-11-12
//     Last Modified: 2024-11-12
//     Description: 
//       Defines static methods and properties for managing cache keys and expiration 
//       settings for Contact-related data. This includes creating unique cache keys for 
//       various contact queries (such as getting all contacts, contacts by ID, etc.), 
//       managing the cache expiration tokens to control cache validity, and providing a 
//       mechanism to refresh cached data in a thread-safe manner.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CleanArchitecture.Blazor.Application.Features.Contacts.Caching;
/// <summary>
/// Static class for managing cache keys and expiration for Contact-related data.
/// </summary>
public static class ContactCacheKey
{
    public const string GetAllCacheKey = "all-Contacts";
    public static string GetPaginationCacheKey(string parameters) {
        return $"ContactCacheKey:ContactsWithPaginationQuery,{parameters}";
    }
    public static string GetExportCacheKey(string parameters) {
        return $"ContactCacheKey:ExportCacheKey,{parameters}";
    }
    public static string GetByNameCacheKey(string parameters) {
        return $"ContactCacheKey:GetByNameCacheKey,{parameters}";
    }
    public static string GetByIdCacheKey(string parameters) {
        return $"ContactCacheKey:GetByIdCacheKey,{parameters}";
    }
    public static IEnumerable<string>? Tags => new string[] { "contact" };
    public static void Refresh()
    {
        FusionCacheFactory.RemoveByTags(Tags);
    }
}

