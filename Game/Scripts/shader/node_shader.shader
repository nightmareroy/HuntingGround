Shader "Custom/UnlitTexture"
{
    Properties{
        _MainTex("texture", 2D) = "white"{}
		_HaveBorder("have_border", Range(0,1))=0
    }

    SubShader{
        LOD 100

        Pass{
            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _MainTex_ST;
			float _HaveBorder;

            struct vIn{
                half4 vertex:POSITION;
            };

            struct vOut{
                half4 pos:SV_POSITION;
                float2 uv:TEXCOORD0;
				half4 vertex:POSITION1;
            };

            vOut vert(vIn v){
                vOut o;
				half4 t_vertex=half4(v.vertex.x*0.9,v.vertex.y,v.vertex.z*0.9,v.vertex.w);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				half4 worldPos=mul(_Object2World, v.vertex);
                o.uv = worldPos.xz;
				o.vertex=v.vertex;
                return o;
            }

            fixed4 frag(vOut i):COLOR{
                fixed4 tex = tex2D(_MainTex, i.uv);


				//if(abs(i.vertex.z)>0.43||abs(i.vertex.x*1.732-i.vertex.z)>0.86||abs(i.vertex.x*-1.732-i.vertex.z)>0.86)
				/*if(i.vertex.x>0)
				{
					return fixed4(0,0,0,1);
				}
				else
				{
					return tex;
				}*/
				return tex;
            }
            ENDCG
        }
    }
}



