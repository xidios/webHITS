using System;

namespace Backend2.Services
{
    public class GreetingService : IGreetingService
    {
        public String GetGreeting(String name)
        {
            if (String.IsNullOrEmpty(name) || name.ToLowerInvariant().Contains("admin"))
            {
                throw new ArgumentException();
            }

            return $"Hello, {name}!";
        }
    }
}
