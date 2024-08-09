﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.AuditTrails.Caching;

public static class AuditTrailsCacheKey
{
    public const string GetAllCacheKey = "all-audittrails";
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(30);
    private static readonly object _tokenLock = new();
    private static CancellationTokenSource _tokenSource = new(RefreshInterval);


    public static MemoryCacheEntryOptions MemoryCacheEntryOptions =>
        new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(GetOrCreateTokenSource().Token));

    public static string GetPaginationCacheKey(string parameters)
    {
        return $"AuditTrailsWithPaginationQuery,{parameters}";
    }

    public static CancellationTokenSource GetOrCreateTokenSource()
    {
        lock (_tokenLock)
        {
            if (_tokenSource.IsCancellationRequested)
            {
                _tokenSource.Dispose();
                _tokenSource = new CancellationTokenSource(RefreshInterval);
            }
            return _tokenSource;
        }
    }

    public static void Refresh()
    {
        lock (_tokenLock)
        {
            if (!_tokenSource.IsCancellationRequested)
            {
                _tokenSource.Cancel();
                _tokenSource.Dispose();
                _tokenSource = new CancellationTokenSource(RefreshInterval);
            }
        }
    }
}