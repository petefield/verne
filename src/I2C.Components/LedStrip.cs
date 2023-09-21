namespace I2C.Components;

public class LedStrip : ILedStrip
{
    byte[] _leds;
    IChannel _channel;
    bool _autoRefresh;

    public LedStrip(IChannel channel) : this(11, channel, true) { }

    public LedStrip(int length, IChannel channel, bool autoRefresh = false)
    {
        _leds = new byte[length];
        _channel = channel; // new Channel(1, address);
        _autoRefresh = autoRefresh;
    }

    public int this[int index]
    {
        get => _leds[index];
        set
        {
            _leds[index] = (byte)value;
            if (_autoRefresh) Refresh();
        }
    }

    public int Length => _leds.Length;

    public IEnumerable<Led> Leds => _leds.Select((index, brightness) => new Led(index, brightness));

    public void SetAll(byte value)
    {
        Array.Fill(_leds, value);
        if (_autoRefresh) Refresh();
    }

    public void Refresh()
    {
        var data = _leds.SelectMany((led, index) => new[] { (byte)index, led }).ToArray();
        _channel.Send(data);
    }
}