using StructureMap;

namespace MacAddressGenerator
{
    public class StructureMapConfig
    {
        private readonly IContainer container;

        public StructureMapConfig(IContainer container)
        {
            this.container = container;
        }

        public void Bootstrap()
        {
            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Program));
                    _.WithDefaultConventions();
                });
            });
        }
    }
}
