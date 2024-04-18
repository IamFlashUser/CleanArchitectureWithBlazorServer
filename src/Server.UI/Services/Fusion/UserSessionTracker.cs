﻿using System.Collections.Immutable;
using Stl.Fusion;

namespace CleanArchitecture.Blazor.Server.UI.Services.Fusion;

public class UserSessionTracker : IUserSessionTracker
{

    private volatile ImmutableDictionary<string, ImmutableHashSet<string>> _pageUserSessions = ImmutableDictionary<string, ImmutableHashSet<string>>.Empty;

    public async Task AddUserSession(string pageComponent, string userName, CancellationToken cancellationToken = default)
    {
        if (_pageUserSessions.TryGetValue(pageComponent, out var existingUsers))
        {
            if (!existingUsers.Contains(userName))
            {
                var updatedUsers = existingUsers.Add(userName);
                _pageUserSessions = _pageUserSessions.SetItem(pageComponent, updatedUsers);
            }
        }
        else
        {
            _pageUserSessions = _pageUserSessions.Add(pageComponent, ImmutableHashSet.Create(userName));
        }
        using (Computed.Invalidate())
        {
            _ = await GetUserSessions(cancellationToken);
        }
    }

    public virtual  Task<(string PageComponent, string[] UserSessions)[]> GetUserSessions(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_pageUserSessions.Select(kvp => (kvp.Key, kvp.Value.ToArray())).ToArray());
    }

    public async Task RemoveUserSession(string pageComponent, string userName, CancellationToken cancellationToken = default)
    {
        if (_pageUserSessions.TryGetValue(pageComponent, out var users) && users.Contains(userName))
        {
            var updatedUsers = users.Remove(userName);
            if (updatedUsers.IsEmpty)
            {
                _pageUserSessions = _pageUserSessions.Remove(pageComponent);
            }
            else
            {
                _pageUserSessions = _pageUserSessions.SetItem(pageComponent, updatedUsers);
            }
        }

        using (Computed.Invalidate())
        {
            _ = await GetUserSessions(cancellationToken);
        }
    }
}