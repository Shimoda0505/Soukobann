Shader "HIKALAB/Custom/Toon"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _RampTex("Ramp", 2D) = "white"{}
    }
    SubShader
    {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            //�J�X�^�����C�e�B���O�̐錾��`   
            //Lighting���\�b�h�̖��O--Lighting[���\�b�h��]--
            #pragma surface surf ToonRamp
            //shader model
            #pragma target 3.0

            
            sampler2D _MainTex;
            sampler2D _RampTex;

            struct Input 
            {
                //UV���W
                float2 uv_MainTex;
            };

            fixed4 _Color;


            //�J�X�^�����C�e�B���O�̃��\�b�h
            fixed4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten)
            {
                //�����������Ă鎞�̓��ς��擾(dot���悭�g���Ă���)
                half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
                //Ramp�e�N�X�`����UV���W�̎擾
                fixed3 ramp = tex2D(_RampTex, fixed2(d, 0.5)).rgb;
                //Ramp�e�N�X�`����UV�l����J���[���擾
                fixed4 c;
                c.rgb = s.Albedo * _LightColor0.rgb * ramp;
                c.a = 0;
                return c;
            }

            void surf(Input IN, inout SurfaceOutput o) {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
    }
        FallBack "Diffuse"
}
