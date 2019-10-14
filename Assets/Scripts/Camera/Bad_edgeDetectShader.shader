Shader "Hidden/edgeShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ratio("_ratio", float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			sampler2D _MainTex;
			float _offset;
			float _ratio;
			float get_tresh(float4 col, float2 dir, float2 uv) {
				int n = 5;
				float2 d = dir / n;
				for (int i = 0; i < n * 4; i++) {
					float4 offset = tex2D(_MainTex, uv + d);
					float4 offset2 = tex2D(_MainTex, uv - d);
					float l1 = length(col - offset);
					float l2 = length(col - offset2);
					if (l1 > 0.2 || l2 > 0.2) {
						if (l1 > 0.2 && l2 > 0.2) {
							return -1;
						}
						if (i < n) {
							return 1;
						}
					}
					d = d + dir;
				}
				return 0;
			}

            float4 frag (v2f input) : SV_Target
            {
                float4 col = tex2D(_MainTex, input.uv);

				float2 right = float2(_offset * (_ratio), 0);
				float2 up = float2(0, _offset);

				float4 offset_r = tex2D(_MainTex, input.uv + right);
				float4 offset_d = tex2D(_MainTex, input.uv - right);


				bool edge = false;
				float response = get_tresh(col, right, input.uv);
				if (response == 1) {
					edge = true;
				}
				else if (response == -1) {
					return col;
				}
				response = get_tresh(col, up, input.uv);
				if (response == 1) {
					edge = true;
				}
				else if (response == -1) {
					return col;
				}
				///corners
				response = get_tresh(col, right + up, input.uv);
				if (response == 1) {
					edge = true;
				}
				response = get_tresh(col, -right + up, input.uv);
				if (response == 1) {
					edge = true;
				}

				if (edge) {
					return 0;
				}

				//return col * (1 - (col - offset));
				//return col * (1 - length(col -  offset_r + col - offset_d));
				return col;
            }
            ENDCG
        }
    }
}
