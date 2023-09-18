using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using System.Device.I2c;
using System.Threading.Tasks;

namespace I2C
{
    public class Channel : IChannel
    {
        private I2cDevice _device;

        public Channel(IOptions<ChannelConfiguration> configuration) : this(configuration.Value.Bus, configuration.Value.Address)
        {
        }

        public Channel(int bus, byte address)
        {
            _device = I2cDevice.Create(new I2cConnectionSettings(bus, address));
        }

        public void Send(byte[] data)
        {
            try
            {
                _device.Write(data);
            }
            catch (System.Exception)
            {
                Task.Delay(500).Wait();
                _device.Write(data);
            }
        }
    }
}
