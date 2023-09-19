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
#include "rom/gpio.h"
#include "sdkconfig.h"
#include "driver/i2c.h"
#include <time.h>
#include <unistd.h>

int Leds[] = {2, 4, 5, 18, 19, 23, 13, 12, 14, 27};

uint8_t i2c_data[2];

#define ELEMENTCOUNT(x) (sizeof(x) / sizeof(x[0]))

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
    if (err != ESP_OK)
    {
        return err;
    }
    return i2c_driver_install(i2c_slave_port, conf_slave.mode, 512, 512, 0);
}

static void configure_led(int pin)
{
    gpio_pad_select_gpio(pin);
    gpio_set_direction(pin, GPIO_MODE_OUTPUT);
}

static void configure_leds()
{
    for (int i = 0; i < ELEMENTCOUNT(Leds); i++)
    {
        configure_led(Leds[i]);
        gpio_set_level(Leds[i], 1);
    }
}

void i2c_slave_task(void *arg)
{
    while (1)
    {
        i2c_slave_read_buffer(0, i2c_data, 2, portMAX_DELAY);
        printf("%u %hhx \n", i2c_data[0], i2c_data[1]);
        gpio_set_level(Leds[i2c_data[0]], i2c_data[1]);
        i2c_data[0] = 0;
        i2c_data[1] = 0;
    }
}

void app_main()
{
    configure_leds();
    i2c_slave_init();
    xTaskCreate(i2c_slave_task, "i2c_slave_task", 4096, NULL, 1, NULL);
}
