
Shader "Custom/UnlitTextureMix" // Код полностью скопирован с методички, т.к. не удалось настроить VS на показ
// подсказок и ошибок (а писать в слепую на языке, который ты впервые видишь - выстрел в ногу).
// Но то, что код скопирован, не дает гарантии в его правильности, потому что ничего не работает, текстуры
// даже просто не прокидываются в свои поля.
{
	//свойства шейдера
	Properties
	{
		_Tex1("Texture1", 2D) = "white" {} // текстура1
		_Tex2("Texture2", 2D) = "white" {} // текстура2
		_MixValue("Mix Value", Range(0,1)) = 0.5 // параметр смешивания текстур
		_Color("Main Color", COLOR) = (1,1,1,1) // цвет окрашивания
		_Height("Height", Range(0,20)) = 0.5 // сила изгиба
	}

	//сабшейдер
	SubShader
	{
		Tags{ "RenderType" = "Opaque" } // тег, означающий, что шейдер непрозрачный
		LOD 100 // минимальный уровень детализации

		Pass
		{
		CGPROGRAM

		sampler2D _Tex1; // текстура1
		float4 _Tex1_ST;
		sampler2D _Tex2; // текстура2
		float4 _Tex2_ST;
		float _MixValue; // параметр смешивания
		float4 _Color; // цвет, которым будет окрашиваться изображение
		// структура, которая помогает преобразовать данные вершины в данные
	
		struct v2f
		{
		float2 uv : TEXCOORD0; // UV-координаты вершины
		float4 vertex : SV_POSITION; // координаты вершины
		};
		//здесь происходит обработка вершин
		v2f vert(appdata_full v)
		{
		v2f result;
		v.vertex.xyz += v.normal * _Height * v.texcoord.x * v.texcoord.x; // Поменять формулу и будет сделана домашка

		result.vertex = UnityObjectToClipPos(v.vertex);
		result.uv = TRANSFORM_TEX(v.texcoord, _Tex1);
		return result;
		}
		//здесь происходит обработка пикселей, цвет пикселей умножается на цвет
		
		fixed4 frag(v2f i) : SV_Target
		{
		fixed4 color;
		color = tex2D(_Tex1, i.uv) * _MixValue;
		color += tex2D(_Tex2, i.uv) * (1 - _MixValue);
		color = color * _Color;
		return color;
		}
		ENDCG
		}

	}
}
