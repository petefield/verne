using I2C.Components;



Console.WriteLine("Hello I2C!");

var channel = new I2C.Channel(1, 0x28);

var strip = new LedStrip(11, channel, autoRefresh: true);
bool state = true; 

while(true){

    for (int i = 0; i < strip.Length; i++)
    {
           Console.WriteLine(i);

        strip[i] = state ? (byte)1 : (byte)0;
        Task.Delay(1000).Wait();
    }
                
    Console.WriteLine("swtich");
    state = !state;
}

