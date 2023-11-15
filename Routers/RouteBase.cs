namespace MinimimApi.Routers {
    public class RouterBase
    {
        public string? ResourceName;
        protected ILogger? Logger;
        public virtual void AddRoutes(WebApplication app)
        {
        }
    }
}