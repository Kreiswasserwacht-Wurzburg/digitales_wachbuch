using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wasserwacht.DigitalGuardBook.Common.Data;

[assembly: HostingStartup(typeof(Wasserwacht.DigitalGuardBook.Ui.Areas.Identity.IdentityHostingStartup))]
namespace Wasserwacht.DigitalGuardBook.Ui.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}