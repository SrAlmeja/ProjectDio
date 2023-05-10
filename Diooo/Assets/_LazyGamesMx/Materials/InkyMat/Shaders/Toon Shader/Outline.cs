/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;

public class Outline : MonoBehaviour
{
    [Serializable]
    [PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.AfterStack, "Outline")]
    public sealed class PostProcessOutline : PostProcessEffectSetting
    {
        public FloatParameter thickness = new FloatParameter { value = 1f };
        public FloatParameter deptMin = new FloatParameter { value = 0f };
        public FloatParameter deptMax = new FloatParameter { value = 1f };

    }
}

public class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline>
{
    public override void Render(PostProcessRenderContext context)
    {
        PropertySheet sheet = context.propertySheets.Get(Shader.Find("Hidden/Outline"));
        sheet.properties.SetFloat("_Thickness", settings.thickness);
        sheet.properties.SetFloat("_MinDepth", settings.thickness);
        sheet.properties.SetFloat("_MaxDepth", settings.thickness);
        
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        
        
    }
}
*/
