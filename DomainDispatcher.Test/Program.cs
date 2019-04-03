using System;

namespace DomainDispatcher.Test
{
    using Microsoft.Extensions.DependencyInjection;

    class Program
    {
        static void Main(string[] args)
        {
            var s = new ServiceCollection();

            s.AddDomainDispatcher();


        }
    }
}
