﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.PowerPoint;

namespace FunctionalTestInterface
{
    public interface IPowerPointOperations
    {
        # region PowerPoint Application API

        void MaximizeWindow();
        void EnterFunctionalTest();
        void ExitFunctionalTest();
        bool IsInFunctionalTest();
        List<ISlideData> FetchPresentationData(string pathToPresentation);
        List<ISlideData> FetchCurrentPresentationData();
        void ClosePresentation();
        void ActivatePresentation();
        int PointsToScreenPixelsX(float x);
        int PointsToScreenPixelsY(float y);
        Boolean IsOffice2010();
        Boolean IsOffice2013();

        # endregion

        # region Slide-related API

        Slide GetCurrentSlide();
        Slide SelectSlide(int index);
        Slide SelectSlide(string slideName);
        Slide[] GetAllSlides();

        # endregion

        # region Shape-related API

        Selection GetCurrentSelection();
        ShapeRange SelectShape(string shapeName);
        ShapeRange SelectShapes(IEnumerable<string> shapeNames);
        ShapeRange SelectShapesByPrefix(string prefix);
        FileInfo ExportSelectedShapes();
        string SelectAllTextInShape(string shapeName);
        string SelectTextInShape(string shapeName, int startIndex, int endIndex);

        # endregion
    }
}