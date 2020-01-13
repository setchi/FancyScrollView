/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

#ifndef GALLERY_METABALL_HLSL_INCLUDED
#define GALLERY_METABALL_HLSL_INCLUDED

#define CELL_COUNT 5 // CeilToInt(1f / cellInterval)
#define DATA_COUNT 7 // CELL_COUNT + 2(objects)

// xy = cell position, z = data index, w = scale
float4 _CellState[DATA_COUNT];

float f(float2 v)
{
    return 1. / (v.x * v.x + v.y * v.y + .0001);
}

float4 metaball(float2 st)
{
    float scale = 4600;
    float d = 0;

    [unroll]
    for (int i = 0; i < DATA_COUNT; i++)
    {
        d += f(st - _CellState[i].xy) * _CellState[i].w;
    }

    d *= scale;
    d = abs(d - 0.5);

    float3 color = 1;
    color = lerp(color, float3(0.16, 0.07, 0.31), smoothstep(d - 0.04, d - 0.04 + 0.002, 0));
    color = lerp(color, float3(0.16, 0.80, 0.80), smoothstep(d - 0.02, d - 0.02 + 0.002, 0));
    return float4(color, 1);
}

#endif
