﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using PowerPointLabs.PictureSlidesLab.Model;
using PowerPointLabs.PictureSlidesLab.ModelFactory.Options.Interface;

namespace PowerPointLabs.PictureSlidesLab.ModelFactory.Options
{
    [Export(typeof(IStyleOptions))]
    [ExportMetadata("StyleOrder", 10)]
    class TriangleStyleOptions : BaseStyleOptions
    {
        public override List<StyleOption> GetOptionsForVariation()
        {
            var result = GetOptionsWithSuitableFontColor();
            foreach (var styleOption in result)
            {
                styleOption.IsUseTriangleStyle = true;
                styleOption.TextBoxPosition = 4; // left
                styleOption.TriangleTransparency = 25;
            }
            UpdateStyleName(
                result,
                TextCollection.PictureSlidesLabText.StyleNameTriangle);
            return result;
        }

        public override StyleOption GetDefaultOptionForPreview()
        {
            return new StyleOption()
            {
                StyleName = TextCollection.PictureSlidesLabText.StyleNameTriangle,
                IsUseTriangleStyle = true,
                TriangleColor = "#007FFF", // blue
                TextBoxPosition = 4, // left
                TriangleTransparency = 25
            };
        }
    }
}
