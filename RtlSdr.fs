module Hamstr.RtlSdr

open System
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

(*
    Interface definitions for rtl-srd

    From the original copyright notice: 

     * rtl-sdr, turns your Realtek RTL2832 based DVB dongle into a SDR receiver
     * Copyright (C) 2012-2013 by Steve Markgraf <steve@steve-m.de>
     * Copyright (C) 2012 by Dimitri Stolnikov <horiz0n@gmx.net>
     *
     * This program is free software: you can redistribute it and/or modify
     * it under the terms of the GNU General Public License as published by
     * the Free Software Foundation, either version 2 of the License, or
     * (at your option) any later version.
     *
     * This program is distributed in the hope that it will be useful,
     * but WITHOUT ANY WARRANTY; without even the implied warranty of
     * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     * GNU General Public License for more details.
     *
     * You should have received a copy of the GNU General Public License
     * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*)

[<Literal>]
let PlatformCallingConvention = CallingConvention.Cdecl

type rtlsdr_dev_t = nativeint
type uint8_t = uint8
type uint16_t = uint16
type uint32_t = uint32

(*
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern typedef struct rtlsdr_dev rtlsdr_dev_t
*)

/// Return the number of compatible devices detected
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern uint32 rtlsdr_get_device_count()

[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern char* rtlsdr_get_device_name(uint32 index)

/// <summary>Get USB device strings.
/// NOTE: The string arguments must provide space for up to 256 bytes.
/// </summary>
/// <param name="index">The device index</param>
/// <param name="manufact">Manufacturer name, may be NULL</param>
/// <param name="product">Product name, may be NULL</param>
/// <param name="serial">Serial number, may be NULL</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_get_device_usb_strings(uint32 index, char *manufact, char *product, char *serial)

/// <summary>Get device index by USB serial string descriptor.</summary>
///
/// <param name="serial">Serial string of the device</param>
/// <returns>device index of first device where the name matched</returns>
/// <returns>-1 if name is NULL</returns>
/// <returns>-2 if no devices were found at all</returns>
/// <returns>-3 if devices were found, but none with matching name</returns>
 
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_get_index_by_serial(char *serial)

[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_open(rtlsdr_dev_t * *dev, uint32 index)

[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_close(rtlsdr_dev_t *dev)

// configuration functions
///
/// Set crystal oscillator frequencies used for the RTL2832 and the tuner IC.
///
/// Usually both ICs use the same clock. Changing the clock may make sense if
/// you are applying an external clock to the tuner or to compensate the
/// frequency (and samplerate) error caused by the original (cheap) crystal.
///
/// NOTE: Call this function only if you fully understand the implications.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="rtl_freq">Frequency value used to clock the RTL2832 in Hz</param>
/// <param name="tuner_freq">Frequency value used to clock the tuner IC in Hz</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_set_xtal_freq(rtlsdr_dev_t *dev, uint32_t rtl_freq, uint32_t tuner_freq)

/// Get crystal oscillator frequencies used for the RTL2832 and the tuner IC.
///
/// Usually both ICs use the same clock.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="rtl_freq">Frequency value used to clock the RTL2832 in Hz</param>
/// <param name="tuner_freq">Frequency value used to clock the tuner IC in Hz</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_get_xtal_freq(rtlsdr_dev_t *dev, uint32_t *rtl_freq, uint32_t *tuner_freq);

/// Get USB device strings.
///
/// NOTE: The string arguments must provide space for up to 256 bytes.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="manufact">Manufacturer name, may be NULL</param>
/// <param name="product">Product name, may be NULL</param>
/// <param name="serial">Serial number, may be NULL</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_get_usb_strings(rtlsdr_dev_t *dev, char *manufact, char *product, char *serial);

/// Write the device EEPROM
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="data">Buffer of data to be written</param>
/// <param name="offset">Address where the data should be written</param>
/// <param name="len">Length of the data</param>
/// <returns>0 on success</returns>
/// <returns>-1 if device handle is invalid</returns>
/// <returns>-2 if EEPROM size is exceeded</returns>
/// <returns>-3 if no EEPROM was found</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_write_eeprom(rtlsdr_dev_t *dev, uint8_t *data, uint8_t offset, uint16_t len);

/// Read the device EEPROM
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="data">Buffer where the data should be written</param>
/// <param name="offset">Address where the data should be read from</param>
/// <param name="len">Length of the data</param>
/// <returns>0 on success</returns>
/// <returns>-1 if device handle is invalid</returns>
/// <returns>-2 if EEPROM size is exceeded</returns>
/// <returns>-3 if no EEPROM was found</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_read_eeprom(rtlsdr_dev_t *dev, uint8_t *data, uint8_t offset, uint16_t len);

[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_set_center_freq(rtlsdr_dev_t *dev, uint32_t freq);

/// Get actual frequency the device is tuned to.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <returns>0 on error, frequency in Hz otherwise</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern uint32_t rtlsdr_get_center_freq(rtlsdr_dev_t *dev);

/// Set the frequency correction value for the device.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="ppm">Correction value in parts per million (ppm)</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_set_freq_correction(rtlsdr_dev_t *dev, int ppm);

/// Get actual frequency correction value of the device.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <returns>correction value in parts per million (ppm)</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_get_freq_correction(rtlsdr_dev_t *dev);

(*
enum rtlsdr_tuner {
    RTLSDR_TUNER_UNKNOWN = 0,
    RTLSDR_TUNER_E4000,
    RTLSDR_TUNER_FC0012,
    RTLSDR_TUNER_FC0013,
    RTLSDR_TUNER_FC2580,
    RTLSDR_TUNER_R820T,
    RTLSDR_TUNER_R828D
};

/// Get the tuner type.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <returns>RTLSDR_TUNER_UNKNOWN on error, tuner type otherwise</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern enum rtlsdr_tuner rtlsdr_get_tuner_type(rtlsdr_dev_t *dev);
*)
/// Get a list of gains supported by the tuner.
///
/// NOTE: The gains argument must be preallocated by the caller. If NULL is
/// being given instead, the number of available gain values will be returned.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="gains">Array of gain values. In tenths of a dB, 115 means 11.5 dB.</param>
/// <returns><= 0 on error, number of available (returned) gain values otherwise</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_get_tuner_gains(rtlsdr_dev_t *dev, int *gains);

/// Set the gain for the device.
/// Manual gain mode must be enabled for this to work.
///
/// Valid gain values (in tenths of a dB) for the E4000 tuner:
/// -10, 15, 40, 65, 90, 115, 140, 165, 190,
/// 215, 240, 290, 340, 420, 430, 450, 470, 490
///
/// Valid gain values may be queried with \ref rtlsdr_get_tuner_gains function.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="gain">In tenths of a dB, 115 means 11.5 dB.</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_set_tuner_gain(rtlsdr_dev_t *dev, int gain);

/// Get actual gain the device is configured to.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <returns>0 on error, gain in tenths of a dB, 115 means 11.5 dB.</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_get_tuner_gain(rtlsdr_dev_t *dev);

///* Set the intermediate frequency gain for the device.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="stage">Intermediate frequency gain stage number (1 to 6 for E4000)</param>
/// <param name="gain">In tenths of a dB, -30 means -3.0 dB.</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_set_tuner_if_gain(rtlsdr_dev_t *dev, int stage, int gain);

/// Set the gain mode (automatic/manual) for the device.
/// Manual gain mode must be enabled for the gain setter function to work.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="manual">Gain mode, 1 means manual gain mode shall be enabled.</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_set_tuner_gain_mode(rtlsdr_dev_t *dev, int manual);

/// this will select the baseband filters according to the requested sample rate */
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_set_sample_rate(rtlsdr_dev_t *dev, uint32_t rate);

/// Get actual sample rate the device is configured to.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <returns>0 on error, sample rate in Hz otherwise</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern uint32_t rtlsdr_get_sample_rate(rtlsdr_dev_t *dev);

/// Enable test mode that returns an 8 bit counter instead of the samples.
/// The counter is generated inside the RTL2832.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="test">Mode, 1 means enabled, 0 disabled</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_set_testmode(rtlsdr_dev_t *dev, int on);

/// Enable or disable the internal digital AGC of the RTL2832.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="digital">aGC mode, 1 means enabled, 0 disabled</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_set_agc_mode(rtlsdr_dev_t *dev, int on);

/// Enable or disable the direct sampling mode. When enabled, the IF mode
/// of the RTL2832 is activated, and rtlsdr_set_center_freq() will control
/// the IF-frequency of the DDC, which can be used to tune from 0 to 28.8 MHz
/// (xtal frequency of the RTL2832).
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="on">0 means disabled, 1 I-ADC input enabled, 2 Q-ADC input enabled</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_set_direct_sampling(rtlsdr_dev_t *dev, int on);

/// Get state of the direct sampling mode
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <returns>-1 on error, 0 means disabled, 1 I-ADC input enabled</returns>
///	    2 Q-ADC input enabled
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_get_direct_sampling(rtlsdr_dev_t *dev);

/// Enable or disable offset tuning for zero-IF tuners, which allows to avoid
/// problems caused by the DC offset of the ADCs and 1/f noise.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="on">0 means disabled, 1 enabled</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_set_offset_tuning(rtlsdr_dev_t *dev, int on);

/// Get state of the offset tuning mode
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <returns>-1 on error, 0 means disabled, 1 enabled</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_get_offset_tuning(rtlsdr_dev_t *dev);

// streaming functions

[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_reset_buffer(rtlsdr_dev_t *dev);

[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_read_sync(rtlsdr_dev_t *dev, void *buf, int len, int *n_read);

// typedef void( *rtlsdr_read_async_cb_t)(unsigned char *buf, uint32_t len, void *ctx);

/// Read samples from the device asynchronously. This function will block until
/// it is being canceled using rtlsdr_cancel_async()
///
/// NOTE: This function is deprecated and is subject for removal.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="cb">Callback function to return received samples</param>
/// <param name="ctx">User specific context to pass via the callback function</param>
/// <returns>0 on success</returns>
(*
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_wait_async(rtlsdr_dev_t *dev, rtlsdr_read_async_cb_t cb, void *ctx);


/// Read samples from the device asynchronously. This function will block until
/// it is being canceled using rtlsdr_cancel_async()
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <param name="cb">Callback function to return received samples</param>
/// <param name="ctx">User specific context to pass via the callback function</param>
/// <param name="buf_num">Optional buffer count, buf_num * buf_len = overall buffer size</param>
///      set to 0 for default buffer count (32)
/// <param name="buf_len">Optional buffer length, must be multiple of 512,</param>
///      set to 0 for default buffer length (16 * 32 * 512)
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_read_async(rtlsdr_dev_t *dev, rtlsdr_read_async_cb_t cb, void *ctx, uint32_t buf_num, uint32_t buf_len);
*)

/// Cancel all pending asynchronous operations on the device.
///
/// <param name="dev">The device handle given by rtlsdr_open()</param>
/// <returns>0 on success</returns>
[<DllImport("librtlsdr", CallingConvention=PlatformCallingConvention)>]
extern int rtlsdr_cancel_async(rtlsdr_dev_t *dev);
