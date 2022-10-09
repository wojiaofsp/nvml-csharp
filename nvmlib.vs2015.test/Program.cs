using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nvidia.Nvml;
using WUtilitiesV01.Services;

namespace nvmlib.vs2015.test
{
    class Program
    {
        private static string GetStringFromSByteArray(sbyte[] data)
        {
            if (data == null)
                throw new SystemException("Data can't be null");

            byte[] busIdData = Array.ConvertAll(data, (a) => (byte)a);
            return Encoding.Default.GetString(busIdData).Replace("\0", "");
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Initialiling nvml library..");
            try
            {
                NvGpu.NvmlInitV2();


                Console.WriteLine("Retrieving device count in this machine ...");
                var deviceCount = NvGpu.NvmlDeviceGetCountV2();
                Console.WriteLine($"有{deviceCount}个显卡");
                Console.WriteLine("Listing devices ...");
                while (true)
                {

                    for (int i = 0; i < deviceCount; i++)
                    {
                        var device = NvGpu.NvmlDeviceGetHandleByIndex((uint)i);
                        if (device == IntPtr.Zero)
                        {
                            throw new SystemException($"Something got wrong retrieving device {i}. Do you have a nvidia card?");
                        }

                        var deviceName = NvGpu.NvmlDeviceGetName(device);
                        Console.WriteLine($"The device name from index {i} is {deviceName}");
                        var info = NvGpu.NvmlDeviceGetPciInfoV3(device);
                        var busId = GetStringFromSByteArray(info.busId);
                        Console.WriteLine($"BusId is {busId}");

                        var um = NvGpu.nvmlDeviceGetUtilizationRates(device);
                        Console.WriteLine($"GPU:{um.gpu}% Mem:{um.memory}%");

                        //Console.WriteLine("Trying to change compute mode. This may fail if device not support the new mode.");
                        //var computeMode = NvGpu.NvmlDeviceGetComputeMode(device);
                        //Console.WriteLine($"Current compute mode is {computeMode}");
                        //NvGpu.NvmlDeviceSetComputeMode(device, NvmlComputeMode.NVML_COMPUTEMODE_EXCLUSIVE_THREAD);
                        //NvGpu.NvmlDeviceSetComputeMode(device, computeMode);
                    }
                    Thread.Sleep(1500);
                }
                NvGpu.NvmlShutdown();
            }
            catch (SystemException se)
            {
                Console.WriteLine(se.ToString());
                NvGpu.NvmlShutdown();
            }

        }
    }
}
