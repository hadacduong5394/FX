using FX.Context;
using System;

namespace FX.Data.DBFactory
{
    public interface IDbFactory : IDisposable
    {
        ContextConnection Init();
    }
}