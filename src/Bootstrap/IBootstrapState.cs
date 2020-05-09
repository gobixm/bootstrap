using System;

namespace Gobi.Bootstrap
{
    public interface IBootstrapState : IDisposable
    {
        string Progress { get; set; }
    }
}