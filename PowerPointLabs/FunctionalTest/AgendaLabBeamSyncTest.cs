﻿using System;
using FunctionalTest.util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunctionalTest
{
    [TestClass]
    public class AgendaLabBeamSyncTest : BaseFunctionalTest
    {
        protected override string GetTestingSlideName()
        {
            return "AgendaSlidesBeamBeforeSync.pptx";
        }

        [TestMethod]
        public void FT_AgendaLabBeamSyncTest()
        {
            // TODO: Is there really no way to programmatically select multiple slides at once?
            // TODO: Ideally, I want to select the slides 5,6,7,8 together and apply Synchronise Agenda on them

            ClickOnSlideThumbnailsPanel();
            PpOperations.SelectSlide(5);
            PplFeatures.SynchronizeAgenda();

            PpOperations.SelectSlide(6);
            PplFeatures.SynchronizeAgenda();

            ClickOnSlideThumbnailsPanel();
            PpOperations.SelectSlide(8);
            PplFeatures.SynchronizeAgenda();

            var actualSlides = PpOperations.FetchCurrentPresentationData();
            var expectedSlides = PpOperations.FetchPresentationData(
                PathUtil.GetDocTestPresentationPath("AgendaSlidesBeamAfterSync.pptx"));
            PresentationUtil.AssertEqual(expectedSlides, actualSlides);
        }

        // Click Thumbnails Panel to make selectedSlide focused.
        // When focused, add the agenda beam to the selectedSlide (doesn't have agenda) & sync.
        // When unfocused, only sync (so selectedSlide (doesn't have agenda) remains the same).
        private static void ClickOnSlideThumbnailsPanel()
        {
            var pptPanel = NativeUtil.FindWindow("PPTFrameClass", null);
            var mdiPanel = NativeUtil.FindWindowEx(pptPanel, IntPtr.Zero, "MDIClient", null);
            var mdiPanel2 = NativeUtil.FindWindowEx(mdiPanel, IntPtr.Zero, "mdiClass", null);
            if (PpOperations.IsOffice2010())
            {
                var thumbnailsPanel = NativeUtil.FindWindowEx(mdiPanel2, IntPtr.Zero, "paneClassDC", "Thumbnails");
                NativeUtil.SendMessage(thumbnailsPanel, 0x0201 /*left button down*/, IntPtr.Zero, IntPtr.Zero);
            } 
            else if (PpOperations.IsOffice2013())
            {
                NativeUtil.SendMessage(mdiPanel2, 0x0201 /*left button down*/, IntPtr.Zero, IntPtr.Zero);
            }
        }
    }
}