using System;

namespace WeatherConsole.Rendering
{
    internal static class AsciiWeatherArt
    {
        public static string Get(string condition)
        {
            if (string.IsNullOrWhiteSpace(condition)) return Unknown();
            var c = condition.Trim().ToLowerInvariant();

            if (c.Contains("thunderstorm") && c.Contains("hail")) return ThunderHail();
            if (c.Contains("thunderstorm")) return Thunder();
            if (c.Contains("partly")) return PartlyCloudy();
            if (c.Contains("overcast")) return Overcast();
            if (c.Contains("cloud")) return Cloudy();
            if (c.Contains("freezing rain")) return FreezingRain();
            if (c.Contains("freezing drizzle")) return FreezingDrizzle();
            if (c.Contains("drizzle")) return Drizzle();
            if (c.Contains("rain showers")) return Showers();
            if (c.Contains("rain")) return Rain();
            if (c.Contains("snow grains")) return SnowGrains();
            if (c.Contains("snow showers")) return SnowShowers();
            if (c.Contains("snow")) return Snow();
            if (c.Contains("fog")) return Fog();
            if (c.Contains("clear sky") || c.Contains("mainly clear") || c == "clear" || c.Contains("clear")) return Clear();

            return Unknown(condition);
        }

        private static string Clear() => @"  \   /  
   .-.   
― (   ) ―
   `-’   
  /   \  
   Sunny ";

        private static string PartlyCloudy() => @"  \  /      
 _ /"".-.    
   \_(   ).  
   /(___(__) 
   Partly cloudy";

        private static string Cloudy() => @"             
     .--.    
  .-(    ).  
 (___.__)__) 
    Cloudy";

        private static string Overcast() => @"             
     .--.    
  .-(    ).  
 (___.__)__) 
   Overcast";

        private static string Drizzle() => @"     .-.      
    (   ).    
   (___(__)   
    ‘ ‘ ‘ ‘   
     Drizzle";

        private static string FreezingDrizzle() => @"     .-.      
    (   ).    
   (___(__)   
   * ‘ * ‘    
 Freez. Drizzle";

        private static string Rain() => @"     .-.      
    (   ).    
   (___(__)   
   ‘ ‘ ‘ ‘    
    Rain";

        private static string Showers() => @"     .-.      
    (   ).    
   (___(__)   
  ‚‘‚‘‚‘‚‘    
  Showers";

        private static string FreezingRain() => @"     .-.      
    (   ).    
   (___(__)   
   * ‘ * ‘    
 Freezing Rain";

        private static string Snow() => @"     .-.      
    (   ).    
   (___(__)   
   * * * *    
     Snow";

        private static string SnowShowers() => @"     .-.      
    (   ).    
   (___(__)   
  * * * *     
 Snow Showers";

        private static string SnowGrains() => @"     .-.      
    (   ).    
   (___(__)   
    *  *      
  Snow Grains";

        private static string Fog() => @" _ - _ - _ - _ 
  Fog";

        private static string Thunder() => @"     .-.      
    (   ).    
   (___(__)   
   ⚡⚡⚡⚡     
 Thunder";

        private static string ThunderHail() => @"     .-.      
    (   ).    
   (___(__)   
   ⚡⚡ o o    
 Thndr + Hail";

        private static string Unknown(string? raw = null) => @"   ???       
 Unknown" + (string.IsNullOrEmpty(raw) ? string.Empty : $" ({raw})");
    }
}
