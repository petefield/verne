using I2C.Components;



Console.WriteLine("Hello I2C!");

var channel = new I2C.Channel(1, 0x28);

var strip = new LedStrip(11, channel, autoRefresh: true);
bool state = true; 
int i = 0;

while(true){
            
    while ( i < strip.Length  && i >=0)
    {
        Console.WriteLine(i);

        strip[i] = state ? (byte)1 : (byte)0;
        Task.Delay(60).Wait();
        var inc =  (state ? 1 : -1);
        Console.WriteLine(inc);

        i += inc;
    }
                
    Console.WriteLine("swtich");
    state = !state;
}

