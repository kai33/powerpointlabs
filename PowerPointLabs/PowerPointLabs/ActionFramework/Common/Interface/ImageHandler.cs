﻿using System;
using System.Drawing;

namespace PowerPointLabs.ActionFramework.Common.Interface
{
    /// <summary>
    /// Handler that handles GetImage call
    /// </summary>
    public abstract class ImageHandler
    {
        public Bitmap Get(string ribbonId)
        {
            try
            {
                return GetImage(ribbonId);
            }
            catch (Exception e)
            {
                PowerPointLabsGlobals.LogException(e, ribbonId);
                Views.ErrorDialogWrapper.ShowDialog("PowerPointLabs", e.Message, e);
                return null;
            }
        }

        protected abstract Bitmap GetImage(string ribbonId);
    }
}
