/*
 * Copyright 2006-2007  Anthony Rowe and Adam Goode
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

#include <stdlib.h>
#include <string.h>
#include <ctype.h>
#include <sys/stat.h>
#include <sys/unistd.h>
#include <sys/time.h>
#include <sys/times.h>

#include "LPC2100.h"
#include "devices.h"
#include "serial.h"

#include <errno.h>
#undef errno
extern int errno;

register char *stack_ptr asm ("sp");
// The above line can be used to check the stack pointer
// uart0_write_hex(stack_ptr);

/* prototypes */
int kill(int pid, int sig);
void _exit(int status);
void abort(void);
int _system(const char *s);
int _gettimeofday (struct timeval *tp, struct timezone *tzp);
int _kill(int pid, int sig);
int _getpid(void);
int _times(struct tms *buf);
int _raise(int sig);
void *_sbrk(int nbytes);

/* implementation */
int kill(int pid __attribute__((unused)),
	 int sig __attribute__((unused)))
{
  errno = EINVAL;
  return(-1);
}

void _exit(int status __attribute__((unused)))
{
  // XXX: should call cc3_power_down
  while(1);
}

void abort(void)
{
  _exit(1);
}

int _system(const char *s)
{
  if (s == NULL) {
    return 0; /* no shell */
  } else {
    errno = EINVAL;
    return -1;
  }
}

int _gettimeofday (struct timeval *tp __attribute__((unused)),
		   struct timezone *tzp __attribute__((unused))) {
  return -1;
}

int _getpid()
{
  return 1;
}

int _times(struct tms *buf)
{
  clock_t ticks
    = REG(TIMER0_TC) / (1000 / CLOCKS_PER_SEC); // REG in milliseconds
  buf->tms_utime = ticks;
  buf->tms_stime = 0;
  buf->tms_cutime = 0;
  buf->tms_cstime = 0;
  return ticks;
}

int _raise(int sig __attribute__((unused)))
{
  return 1;
}

/* exciting memory management! */

extern char _end[];             /* end is set in the linker command 	*/
								/* file and is the end of statically 	*/
								/* allocated data (thus start of heap).	*/

extern char _heap_end[];        /* heap_end is also set in the linker   */
                                /* and represents the physical end of   */
                                /* ram (and the ultimate limit of the   */
                                /* heap).                               */

static void *heap_ptr;			/* Points to current end of the heap.	*/

void *_sbrk(int nbytes)
{
  char *base;		/*  errno should be set to  ENOMEM on error	*/
  uart0_write("in _sbrk\r\n");

  uart0_write(" nbytes = ");
  uart0_write_hex((unsigned int) nbytes);

  uart0_write(" heap_ptr = ");
  uart0_write_hex((unsigned int) heap_ptr);

  if (!heap_ptr) {	/*  Initialize if first time through.		*/
    heap_ptr = _end;
  }

  uart0_write(" heap_ptr = ");
  uart0_write_hex((unsigned int) heap_ptr);

  base = heap_ptr;	/*  Point to end of heap.			*/

  uart0_write(" base = ");
  uart0_write_hex((unsigned int) base);

  if (base + nbytes >= (char *) _heap_end) {
    uart0_write(" ENOMEM!\r\n");
    errno = ENOMEM;
    return (void *) -1;
  }

  heap_ptr = (char *)heap_ptr + nbytes;	        /*  Increase heap */

  uart0_write(" heap_ptr = ");
  uart0_write_hex((unsigned int) heap_ptr);

  uart0_write(" returning\r\n");
  return base;		/*  Return pointer to start of new heap area.	*/
}
