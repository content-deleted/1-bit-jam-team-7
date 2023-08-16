shader_type canvas_item;

uniform vec4 primaryColor : source_color = vec4(0,0.4,0,1);

uniform vec4 secondaryColor : source_color =  vec4(0.3,0.6,0.3,1);

uniform float threshold = 0.5;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_linear;

void fragment() {
		vec4 baseColor = textureLod(screen_texture, SCREEN_UV, 0.0);

        COLOR = ((baseColor.r + baseColor.g + baseColor.b) / 3.0) > threshold ? primaryColor : secondaryColor;
}
