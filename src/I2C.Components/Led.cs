namespace I2C.Components;

public class Led{

    public Led(int id, int brightness)
    {
        Id = id;
        Brightness = brightness;
    }

    public int Id { get; set; }

    public int Brightness { get; set; }
}