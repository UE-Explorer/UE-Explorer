#version 450

layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec3 vNormal;
layout (location = 2) in vec4 vColor;
layout (location = 3) in vec2 vTexCoord;

layout (location = 0) out vec4 outColor;
// layout (location = 1) out vec2 outTexCoord;

out gl_PerVertex 
{
    vec4 gl_Position;   
};

layout (push_constant) uniform constants
{
	vec4 Data;
	mat4 RenderMatrix;
} PushConstants;

void main() 
{
	gl_Position = PushConstants.RenderMatrix * vec4(vPosition, 1.0f);
	outColor = vColor;
	// outTexCoord = vTexCoord;
}
