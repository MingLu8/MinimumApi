namespace MinimimApi.Routers {
    public class RouterBase
    {
        public string ResourceName;
        protected ILogger Logger;

        public RouterBase(string resouceName, ILogger logger)
        {
            ResourceName = resouceName;
            Logger = logger;
        }
        
        public virtual void AddRoutes(WebApplication app)
        {
        }
    }
}