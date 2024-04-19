using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Services.BackgroundServices;

// create new db context
public class DeleteExpiredRefreshTokensService : IHostedService, IDisposable
{
    private readonly UserManager<ApplicationUser> _userManager;
    private Timer? _timer;
    public DeleteExpiredRefreshTokensService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(10));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private async void DoWork(object? state)
    {
        var users = _userManager.Users.Where(x => x.RefreshTokenExpirationDateTime <= DateTime.UtcNow).ToList();

        if (users.Count > 0)
        {
            foreach (var user in users)
            {
                user.RefreshToken = null;

                user.RefreshTokenExpirationDateTime = null;

                await _userManager.UpdateAsync(user);
            }
        }
    }
}
