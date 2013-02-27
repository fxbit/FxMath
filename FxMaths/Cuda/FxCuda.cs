﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ManagedCuda;
using System.Reflection;
using System.IO;

namespace FxMaths.Cuda
{
    public class FxCuda
    {
        public CudaContext ctx;
        public CudaUtils Utils;

        public FxCuda(Boolean initUtils=false)
        {
            //Init Cuda context
            ctx = new CudaContext(CudaContext.GetMaxGflopsDeviceId());

            // init utils
            if (initUtils)
                Utils = new CudaUtils(this);
        }

        /// <summary>
        /// Load the kernel and select automaticaly the 
        /// name of the file based on architecture.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        public CudaKernel LoadPTX(String filename, String path, String kernelName)
        {
            string resName;
            if (IntPtr.Size == 8)
                resName = filename+"x64.ptx";
            else
                resName = filename + ".ptx";

            return ctx.LoadKernelPTX(path + "/" + resName, kernelName);
        }

        /// <summary>
        /// Clean the internal memory of the cuda ctx
        /// </summary>
        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
