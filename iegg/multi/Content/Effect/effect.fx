Texture xTexture;
Texture xNormal;
sampler NormalSampler = sampler_state { texture = <xNormal>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR;  AddressV = mirror; };
sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressV = mirror; };


float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;

float3 xLightPos;
float xLightPower;
float xAmbient;


float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

float4x4 WorldInverseTranspose;

float3 DiffuseLightDirection = float3(1, 0, 0);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;



struct VertexToPixel
{
	float4 Position     : POSITION;
	float2 TexCoords    : TEXCOORD0;
	float3 Normal        : TEXCOORD1;
	float3 Position3D    : TEXCOORD2;
};


struct PixelToFrame
{
	float4 Color        : COLOR0;
};


VertexToPixel SimplestVertexShader(float4 inPos : POSITION, float3 inNormal : NORMAL0, float2 inTexCoords : TEXCOORD0)
{
	VertexToPixel Output = (VertexToPixel)0;


//	float4x4 gameMatrix = mul(xWorld, xView);
	//gameMatrix = mul(gameMatrix, xProjection);

	float4 worldPosition = mul(inPos, xWorld);
	float4 viewPosition = mul(worldPosition, xView);
	//float4 baseColor2 = tex2D(TextureSampler, inTexCoords);
	//float4 baseColor = tex2D(TextureSampler2, inTexCoords);
	Output.Position = mul(viewPosition, xProjection);
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
	Output.TexCoords = inTexCoords;
	Output.Position3D = mul(inPos, xWorld);

	return Output;
}

float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
	float3 lightDir = normalize(pos3D - lightPos);
	return dot(-lightDir, normal);
}

PixelToFrame OurFirstPixelShader(VertexToPixel PSIn)
{
	PixelToFrame Output = (PixelToFrame)0;

	float4 normalVec = tex2D(NormalSampler, PSIn.TexCoords);

	float diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, tex2D(NormalSampler, PSIn.TexCoords).xyz);
	diffuseLightingFactor = saturate(diffuseLightingFactor);
	diffuseLightingFactor *= xLightPower;

	PSIn.TexCoords.y--;


	float4 baseColor2 = tex2D(TextureSampler, PSIn.TexCoords);

	Output.Color = baseColor2*(diffuseLightingFactor + xAmbient);

	return Output;
}


technique Simplest
{
	pass Pass0
	{
		VertexShader = compile vs_4_0_level_9_3 SimplestVertexShader();
		PixelShader = compile ps_4_0_level_9_3 OurFirstPixelShader();
	}
}
