/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

#ifndef GALLERY_VORONOI_HLSL_INCLUDED
#define GALLERY_VORONOI_HLSL_INCLUDED

#define CELL_COUNT  7 // CeilToInt(1f / cellInterval)
#define DATA_COUNT 11 // CELL_COUNT + 4(four corners)

// xy = cell position, z = data index, w = select animation
float4 _CellState[DATA_COUNT];

float3 hue_to_rgb(float h)
{
    h = frac(h) * 6 - 2;
    return saturate(float3(abs(h - 1) - 1, 2 - abs(h), 2 - abs(h - 2)));
}

float hash(float2 st)
{
    float3 p3  = frac(float3(st.xyx) * .1031);
    p3 += dot(p3, p3.yzx + 19.19);
    return frac((p3.x + p3.y) * p3.z);
}

float noise(float2 st)
{
    float2 i = floor(st);
    float2 f = frac(st);

    float a = hash(i);
    float b = hash(i + float2(1.0, 0.0));
    float c = hash(i + float2(0.0, 1.0));
    float d = hash(i + float2(1.0, 1.0));

    float2 u = f * f * (3.0 - 2.0 * f);
    return lerp(a, b, u.x) +
        (c - a)* u.y * (1.0 - u.x) +
        (d - b) * u.x * u.y;
}

float linework(float2 st)
{
    float a = atan2(st.y, st.x);
    float d = noise(float2(a * 120, 0)) + smoothstep(300, 50, length(st));
    return 1. - saturate(d);
}

float4 voronoi(float2 st)
{
    float cellIndex = 0, dist = 1e+9;
    float2 cellPos = 1e+5;

    [unroll]
    for (int i = 0; i < DATA_COUNT; i++)
    {
        float2 p = _CellState[i].xy;
        float2 q = st - p;
        float d = q.x * q.x + q.y * q.y;
        if (d < dist)
        {
            dist = d; cellPos = p; cellIndex = i;
        }
    }

    dist = 1e+5;

    [unroll]
    for (int j = 0; j < DATA_COUNT; j++)
    {
        if (cellIndex == j) continue;

        float2 p = _CellState[j].xy;
        float d = dot(st - (cellPos + p) * 0.5, normalize(cellPos - p));
        dist = min(dist, d);
    }

    float3 color = 1;
    float dataIndex = _CellState[cellIndex].z;
    color = hue_to_rgb(dataIndex * 0.1) + 0.1;
    color = lerp(color, 0, linework(st - cellPos) * _CellState[cellIndex].w);
    color = lerp(color, hue_to_rgb(cellIndex * 0.1) * 0.6, step(CELL_COUNT, cellIndex));

    float border = smoothstep(0, 13, dist);
    color = lerp(0.1, color, smoothstep(0.8 - 0.07, 0.8, border));
    color = lerp(1.0, color, smoothstep(0.5 - 0.07, 0.5, border));
    return float4(color, 1);
}

#endif
