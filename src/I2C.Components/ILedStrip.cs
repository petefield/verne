namespace I2C.Components;

public interface ILedStrip
{
    int this[int index] { get; set; }

    int Length { get; }

    void Refresh();

    void SetAll(byte value);

    IEnumerable<Led> Leds { get; }
}