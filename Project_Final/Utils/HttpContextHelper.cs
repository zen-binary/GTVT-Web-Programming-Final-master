namespace Project_Final.Utils
{
    public static class HttpContextHelper
    {
        private static IServiceProvider serviceProvider;

        public static void Configure(IServiceProvider serviceProvider)
        {
            HttpContextHelper.serviceProvider = serviceProvider;
        }

        public static IHttpContextAccessor HttpContextAccessor
        {
            get
            {
                return serviceProvider?.GetService<IHttpContextAccessor>();
            }
        }
    }
}
