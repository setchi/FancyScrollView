float2 _Resolution;

float2 ui_coord(float2 uv)
{
    uv -= 0.5;
    uv *= _Resolution;
    return uv;
}
