﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BFF.Web.Config
{
    public static class AlterUpstream
    {
        public static string AlterUpstreamSwaggerJson(HttpContext context, string swaggerJson)
        {
            var swagger = JObject.Parse(swaggerJson);
            // ... alter upstream json
            return swagger.ToString(Formatting.Indented);
        }
    }
}
