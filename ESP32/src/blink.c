/* Blink Example

   This example code is in the Public Domain (or CC0 licensed, at your option.)

   Unless required by applicable law or agreed to in writing, this
   software is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
   CONDITIONS OF ANY KIND, either express or implied.
*/
#include <stdio.h>
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "driver/gpio.h"
#include "sdkconfig.h"
#include "driver/i2c.h"
#include <time.h>
#include <unistd.h>

uint8_t byte = 0;
int Leds[] = { 2, 4, 5, 18, 19, 23, 13, 12, 14, 27};

/* Can use project configuration menu (idf.py menuconfig) to choose the GPIO to blink,
   or you can edit the following line and set a number here.
*/
#define ELEMENTCOUNT(x)  (sizeof(x) / sizeof(x[0]))

static esp_err_t i2c_slave_init(void)
{
    int i2c_slave_port = 0;
    i2c_config_t conf_slave = {
        .sda_io_num = 21,
        .sda_pullup_en = GPIO_PULLUP_ENABLE,
        .scl_io_num = 22,
        .scl_pullup_en = GPIO_PULLUP_ENABLE,
        .mode = I2C_MODE_SLAVE,
        .slave.addr_10bit_en = 0,
        .slave.slave_addr = 0x28,
    };
    esp_err_t err = i2c_param_config(i2c_slave_port, &conf_slave);
    if (err != ESP_OK) {
        return err;
    }
    return i2c_driver_install(i2c_slave_port, conf_slave.mode, 512, 512, 0);
}

static void configure_led(int pin){
    gpio_pad_select_gpio(pin);
    gpio_set_direction(pin, GPIO_MODE_OUTPUT);
}

static void configure_leds(){
    int s = ELEMENTCOUNT(Leds);

    for (int i = 0; i < s ; i++) {
        configure_led(Leds[i]);
        gpio_set_level(Leds[i], 1);
    }
}

void app_main()
{
   uint8_t *data = (uint8_t *)malloc(2);
   configure_leds();
   i2c_slave_init();

   while (1)
   {
        i2c_slave_read_buffer(0, data, 2, portMAX_DELAY);
        printf("%u %hhx \n", data[0], data[1]);
        gpio_set_level( Leds[data[0]], data[1] );
        data[0] = 0;
        data[1] = 0;
        usleep(10000);
    }
}
