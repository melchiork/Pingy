using System;

namespace Pingy
{
    public class Config
    {
        public Config(string address)
        {
            Address = address;
        }

        public TimeSpan PingTimeout { get; } = TimeSpan.FromSeconds(5);

        public TimeSpan HttpGetTimeout { get; } = TimeSpan.FromSeconds(10);

        public TimeSpan Interval { get; } = TimeSpan.FromSeconds(5);

        public string Address { get; }
    }
}