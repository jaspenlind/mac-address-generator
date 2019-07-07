using System;
using StructureMap;

namespace MacAddressGenerator
{
    public class IOC
    {
        private static readonly Lazy<IContainer> ContainerBuilder = new Lazy<IContainer>(() => new Container());

        public static IContainer Current => ContainerBuilder.Value;
    }
}
