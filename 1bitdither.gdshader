shader_type canvas_item;

uniform vec4 primaryColor : source_color = vec4(0,0.4,0,1);

uniform vec4 secondaryColor : source_color =  vec4(0.3,0.6,0.3,1);

uniform float threshold = 0.5;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

uniform sampler2D u_dither_tex : filter_nearest, repeat_enable;

uniform float u_dither_size = 2;

uniform vec2 dither_range = vec2(0.4, 0.6);

float luminance (vec4 c) {
	return (c.r * 0.3) + (c.g * 0.59) + (c.b * 0.11);
}

void fragment() {
		vec4 baseColor = textureLod(screen_texture, SCREEN_UV, 0.0);
		
		float lum = luminance(baseColor);
		
		vec2 screen_size = vec2(textureSize(screen_texture, 0)) / float(u_dither_size);
		
		ivec2 noise_size = textureSize(u_dither_tex, 0);
		vec2 inv_noise_size = vec2(1.0 / float(noise_size.x), 1.0 / float(noise_size.y));
		vec2 noise_uv = SCREEN_UV * inv_noise_size * vec2(float(screen_size.x), float(screen_size.y));
		// vec2 noise_uv = inv_noise_size * vec2(float(1000), float(500)) * SCREEN_UV / 2.0;
		vec4 dither = texture(u_dither_tex, noise_uv) * (lum > dither_range.x+(dither_range.y - dither_range.x)/ 2.0 ? 2.0 : 1.0) ;
		
		vec4 final = lum > dither_range.x && lum < dither_range.y ? dither : baseColor;
		final = luminance(final) > threshold ? primaryColor : secondaryColor;
        COLOR = final;
}
