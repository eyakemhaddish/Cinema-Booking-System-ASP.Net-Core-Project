using Microsoft.Extensions.Localization;

namespace FromScratch_.net_5.Models
{
  
        public class SharedResources
        {
            private readonly IStringLocalizer _localizer;

            public SharedResources(IStringLocalizer<SharedResources> localizer)
            {
                _localizer = localizer;
            }

            public string GetString(string key)
            {
                return _localizer[key];
            }

            public string GetString(string key, params object[] args)
            {
                return _localizer[key, args];
            }
        }
    
}
