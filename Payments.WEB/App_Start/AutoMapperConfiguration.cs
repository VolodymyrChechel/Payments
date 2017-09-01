using System;
using AutoMapper;
using Payments.BLL.Util;
using Payments.WEB.Util;

namespace Payments.WEB
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new BLLUserProfile());
            });
        }

    }
}