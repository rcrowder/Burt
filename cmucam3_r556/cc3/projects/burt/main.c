#include <math.h>
#include <stdbool.h>
#include <stdio.h>
#include <time.h>
#include <stdlib.h>
#include <ctype.h>
#include <cc3.h>
#include <cc3_ilp.h>
#include <cc3_color_info.h>
#include <cc3_histogram.h>
#include <cc3_frame_diff.h>

#define SERVO_MIN 0
#define SERVO_MID 128
#define SERVO_MAX 255

void frame_diff (cc3_frame_diff_pkt_t * pkt);
void get_histogram (cc3_histogram_pkt_t * h_pkt);
void get_mean (cc3_color_info_pkt_t * s_pkt);
void led_test (void);

int main (void)
{
	int i, val;
	cc3_histogram_pkt_t my_hist;
	cc3_color_info_pkt_t s_pkt;
	cc3_frame_diff_pkt_t fd_pkt;

	cc3_timer_wait_ms (500);

	cc3_gpio_set_mode (0, CC3_GPIO_MODE_SERVO);
	cc3_gpio_set_mode (1, CC3_GPIO_MODE_SERVO);
	cc3_gpio_set_mode (2, CC3_GPIO_MODE_SERVO);
	cc3_gpio_set_mode (3, CC3_GPIO_MODE_SERVO);

	// configure uarts
	cc3_uart_init (0, 
		CC3_UART_RATE_115200, 
		CC3_UART_MODE_8N1,
		CC3_UART_BINMODE_TEXT);

	cc3_uart_init (1, 
		CC3_UART_RATE_38400,//CC3_UART_RATE_115200, 
		CC3_UART_MODE_8N1,
		CC3_UART_BINMODE_BINARY);

	// Make it so that stdout and stdin are not buffered
	val = setvbuf (stdout, NULL, _IONBF, 0);
	val = setvbuf (stdin, NULL, _IONBF, 0);

	printf( "Calling camera init\n" );
	cc3_camera_init ();
	cc3_camera_set_colorspace (CC3_COLORSPACE_MONOCHROME);//_RGB/_YCRCB/_HSV/_MONOCHROME
	cc3_camera_set_resolution (CC3_CAMERA_RESOLUTION_LOW);// (88*2) 176, 144
	cc3_pixbuf_frame_set_coi (CC3_CHANNEL_ALL);//for full 'colour_info'

	// Are these better settings for edge detection?
	//cc3_camera_set_colorspace (CC3_COLORSPACE_YCRCB);?All switches handled?
	//cc3_camera_set_resolution (CC3_CAMERA_RESOLUTION_HIGH);
	//cc3_pixbuf_frame_set_subsample(CC3_SUBSAMPLE_RANDOM, 2, 2);
	//cc3_pixbuf_frame_set_coi (CC3_CHANNEL_ALL);//_Y?

	printf( "Camera init done\n%d x %d\n", 
		cc3_g_pixbuf_frame.raw_width, cc3_g_pixbuf_frame.raw_height );

	// frame difference
	fd_pkt.coi = CC3_CHANNEL_ALL;
	fd_pkt.template_width = 16;//8;
	fd_pkt.template_height = 16;//8;
	fd_pkt.total_x = cc3_g_pixbuf_frame.width;
    fd_pkt.total_y = cc3_g_pixbuf_frame.height;
    fd_pkt.load_frame = 1;  // load a new frame

	fd_pkt.previous_template = malloc (fd_pkt.template_width * fd_pkt.template_height * sizeof (uint32_t));
	if (fd_pkt.previous_template == NULL)
		printf ("Malloc FD startup error!\r");

	cc3_camera_set_auto_white_balance (true);
	cc3_camera_set_auto_exposure (true);

	// The LED test masks the stabilization delays (~2000ms)
	printf ("Waiting for image to stabilize\n");
	led_test ();

	cc3_camera_set_auto_white_balance (false);
	cc3_camera_set_auto_exposure (false);

	printf ("Hello World...\n");

	printf ("\nPush button on camera back to continue\n");
	uint32_t now = cc3_timer_get_current_ms ();
	while (!cc3_button_get_state ())
		;

	cc3_led_set_state (0, true);

	cc3_pixbuf_load ();

	my_hist.channel = CC3_CHANNEL_ALL;
	my_hist.bins = 24;
	my_hist.hist = malloc (my_hist.bins * sizeof (uint32_t));

	while (true) {
		
        printf ("<3 EE\n   0x%02X", cc3_timer_get_current_ms());

		// Grab an image and take a frame difference of it
		cc3_pixbuf_load ();
        frame_diff (&fd_pkt);

		// Rewind and take a histogram of it
		cc3_pixbuf_rewind ();
		get_histogram (&my_hist);

		// Rewind and get some stats
		cc3_pixbuf_rewind ();
		get_mean(&s_pkt);

		printf( "min = [%d,%d,%d] mean = [%d,%d,%d] max = [%d,%d,%d] deviation = [%d,%d,%d] ",
				s_pkt.min.channel[0],s_pkt.min.channel[1],s_pkt.min.channel[2],
				s_pkt.mean.channel[0],s_pkt.mean.channel[1],s_pkt.mean.channel[2],
				s_pkt.max.channel[0],s_pkt.max.channel[1],s_pkt.max.channel[2],
				s_pkt.deviation.channel[0],s_pkt.deviation.channel[1],s_pkt.deviation.channel[2]
				);

		printf ("hist[%d] = ", my_hist.bins);
		for (i = 0; i < my_hist.bins; i++)
		{
			printf ("%d ", my_hist.hist[i]);

			// sample non-blocking serial routine
			if (!cc3_uart_has_data (1))
			{
				cc3_gpio_set_servo_position (0, SERVO_MID);
				cc3_gpio_set_servo_position (1, SERVO_MID);
			}
		}

		printf ("\n");
		//cc3_timer_wait_ms(400);
	}

	return 0;
}


void frame_diff (cc3_frame_diff_pkt_t * pkt)
{
	uint8_t old_coi = cc3_g_pixbuf_frame.coi;
	cc3_pixbuf_frame_set_coi (pkt->coi);

	cc3_image_t img;
	img.channels = 1;
	img.width = cc3_g_pixbuf_frame.width;
	img.height = 1; // 1 row for scanline processing
	img.pix = malloc (img.width);
	if (img.pix == NULL) {
		return;
	}

	if (cc3_frame_diff_scanline_start (pkt) != 0) {
		while (cc3_pixbuf_read_rows (img.pix, 1)) {
			cc3_frame_diff_scanline (&img, pkt);
		}
		cc3_frame_diff_scanline_finish (pkt);
	}
	else
		printf ("frame diff start error\r");

	cc3_pixbuf_frame_set_coi (old_coi);
	free (img.pix);
}


void get_mean (cc3_color_info_pkt_t * s_pkt)
{
	cc3_image_t img;
	img.channels = 1;
	img.width = cc3_g_pixbuf_frame.width;
	img.height = 1;
	img.pix = malloc (3 * img.width);

	if (cc3_color_info_scanline_start (s_pkt) != 0) {
		while (cc3_pixbuf_read_rows (img.pix, 1)) {
			cc3_color_info_scanline (&img, s_pkt);
		}
		cc3_color_info_scanline_finish (s_pkt);
	}
	free (img.pix);
}


void get_histogram (cc3_histogram_pkt_t * h_pkt)
{
	cc3_image_t img;
	img.channels = 3;
	img.width = cc3_g_pixbuf_frame.width;
	img.height = 1;
	img.pix = cc3_malloc_rows (1);
	if (img.pix == NULL) {
		return;
	}

	if (cc3_histogram_scanline_start (h_pkt) != 0) {
		while (cc3_pixbuf_read_rows (img.pix, 1)) {
			// This does the HSV conversion 
			// cc3_rgb2hsv_row(img.pix,img.width);
			cc3_histogram_scanline (&img, h_pkt);
		}
	}
	cc3_histogram_scanline_finish (h_pkt);

	free (img.pix);
	return;
}


void led_test (void)
{
	for (int i = 0; i < 8; i++)
	{
		if (i & 1) {
			cc3_led_set_state(0, true);
		} else {
			cc3_led_set_state(0, false);
		}

		if (i & 2) {
			cc3_led_set_state(1, true);
		} else {
			cc3_led_set_state(1, false);
		}

		if (i & 4) {
			cc3_led_set_state(2, true);
		} else {
			cc3_led_set_state(2, false);
		}

		cc3_timer_wait_ms(200);
	}

	cc3_led_set_state (0, true);
	cc3_led_set_state (1, true);
	cc3_led_set_state (2, true);
	cc3_timer_wait_ms(400);

	cc3_led_set_state (0, false);
	cc3_led_set_state (1, false);
	cc3_led_set_state (2, false);
	cc3_timer_wait_ms(400);

}
