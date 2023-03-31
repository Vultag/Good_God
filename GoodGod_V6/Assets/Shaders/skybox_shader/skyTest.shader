Shader "Shader Graphs/Stylized_Skybox"
{
    Properties
    {
        Vector1_2CC6A532("Sun Radius", Range(0, 2)) = 1
        Vector1_166C34A9("Moon Radius", Range(0, 2)) = 0.5
        Vector1_6F3D4927("Moon Offset", Range(-1, 1)) = 0.1
        Color_BE31CDF2("Day Sky Color - Top", Color) = (0.254902, 0.5283097, 1, 0)
        Color_68FD0CD8("Day Sky Color - Bottom", Color) = (0.254717, 1, 0.9453618, 0)
        Color_D230EFA1("Night Sky Color - Top", Color) = (0.1711811, 0.1388394, 0.3773585, 0)
        Color_83CF459("Night Sky Color - Bottom", Color) = (0.1439124, 0.2019927, 0.4622642, 0)
        Color_1EB49FED("Horizon Color", Color) = (0.8679245, 0.7247315, 0.3111428, 0)
        Vector1_3B7A233E("Horizon Width", Float) = 3
        Vector1_D80769F7("Horizon Brightness", Float) = 2
        _test_rotate("test_rotate", Float) = 0
        _etoiles_intensite("etoiles_intensite", Float) = 0
        _etoiles_densite("etoiles_densite", Float) = 0
        _etoiles_powa("etoiles_powa", Float) = 0
        _etoiles_rotations("etoiles_rotations", Float) = 0
        _etoiles_rotations_1("etoiles_rotations (1)", Float) = 0
        _Vector2("Vector2", Vector) = (0, 0, 0, 0)
        [NoScaleOffset]_nuageJour("nuageJour", CUBE) = "" {}
        [NoScaleOffset]_NuagesNuit("NuagesNuit", CUBE) = "" {}
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                // LightMode: <None>
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        ZClip False
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        #pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 ObjectSpaceViewDirection;
             float3 WorldSpaceViewDirection;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float Vector1_2CC6A532;
        float Vector1_166C34A9;
        float Vector1_6F3D4927;
        float4 Color_BE31CDF2;
        float4 Color_68FD0CD8;
        float4 Color_D230EFA1;
        float4 Color_83CF459;
        float4 Color_1EB49FED;
        float Vector1_3B7A233E;
        float Vector1_D80769F7;
        float _test_rotate;
        float _etoiles_intensite;
        float _etoiles_densite;
        float _etoiles_powa;
        float _etoiles_rotations_1;
        float _etoiles_rotations;
        float2 _Vector2;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        float4 _WorldSpaceLightPos0;
        TEXTURECUBE(_nuageJour);
        SAMPLER(sampler_nuageJour);
        TEXTURECUBE(_NuagesNuit);
        SAMPLER(sampler_NuagesNuit);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Distance_float3(float3 A, float3 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Negate_float4(float4 In, out float4 Out)
        {
            Out = -1 * In;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Normalize_float3(float3 In, out float3 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Absolute_float(float In, out float Out)
        {
            Out = abs(In);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float2 Unity_Voronoi_RandomVector_LegacySine_float (float2 UV, float offset)
        {
            Hash_LegacySine_2_2_float(UV, UV);
            return float2(sin(UV.y * offset), cos(UV.x * offset)) * 0.5 + 0.5;
        }
        
        void Unity_Voronoi_LegacySine_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
        {
            float2 g = floor(UV * CellDensity);
            float2 f = frac(UV * CellDensity);
            float t = 8.0;
            float3 res = float3(8.0, 0.0, 0.0);
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    float2 lattice = float2(x, y);
                    float2 offset = Unity_Voronoi_RandomVector_LegacySine_float(lattice + g, AngleOffset);
                    float d = distance(lattice + offset, f);
                    if (d < res.x)
                    {
                        res = float3(d, offset.x, offset.y);
                        Out = res.x;
                        Cells = res.y;
                    }
                }
            }
        }
        
        void Unity_Power_float(float A, float B, out float Out)
        {
            Out = pow(A, B);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Rotate_About_Axis_Degrees_float(float3 In, float3 Axis, float Rotation, out float3 Out)
        {
            Rotation = radians(Rotation);
        
            float s = sin(Rotation);
            float c = cos(Rotation);
            float one_minus_c = 1.0 - c;
        
            Axis = normalize(Axis);
        
            float3x3 rot_mat = { one_minus_c * Axis.x * Axis.x + c,            one_minus_c * Axis.x * Axis.y - Axis.z * s,     one_minus_c * Axis.z * Axis.x + Axis.y * s,
                                      one_minus_c * Axis.x * Axis.y + Axis.z * s,   one_minus_c * Axis.y * Axis.y + c,              one_minus_c * Axis.y * Axis.z - Axis.x * s,
                                      one_minus_c * Axis.z * Axis.x - Axis.y * s,   one_minus_c * Axis.y * Axis.z + Axis.x * s,     one_minus_c * Axis.z * Axis.z + c
                                    };
        
            Out = mul(rot_mat,  In);
        }
        
        void Unity_Saturate_float4(float4 In, out float4 Out)
        {
            Out = saturate(In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_c2d18d08d4344485ad00bdcc0f425b63_Out_0 = IN.uv0;
            float _Split_e5f4931c52992f888b864b285d2eb7a7_R_1 = _UV_c2d18d08d4344485ad00bdcc0f425b63_Out_0[0];
            float _Split_e5f4931c52992f888b864b285d2eb7a7_G_2 = _UV_c2d18d08d4344485ad00bdcc0f425b63_Out_0[1];
            float _Split_e5f4931c52992f888b864b285d2eb7a7_B_3 = _UV_c2d18d08d4344485ad00bdcc0f425b63_Out_0[2];
            float _Split_e5f4931c52992f888b864b285d2eb7a7_A_4 = _UV_c2d18d08d4344485ad00bdcc0f425b63_Out_0[3];
            float3 _Vector3_ac18fbaae3095a86ba63d4a7089b57e1_Out_0 = float3(_Split_e5f4931c52992f888b864b285d2eb7a7_R_1, _Split_e5f4931c52992f888b864b285d2eb7a7_G_2, _Split_e5f4931c52992f888b864b285d2eb7a7_B_3);
            float4 _Property_0024955546785f8dabe6ddf42235b27f_Out_0 = _WorldSpaceLightPos0;
            float _Distance_ff895df4a75529858f705b22d9790e6f_Out_2;
            Unity_Distance_float3(_Vector3_ac18fbaae3095a86ba63d4a7089b57e1_Out_0, (_Property_0024955546785f8dabe6ddf42235b27f_Out_0.xyz), _Distance_ff895df4a75529858f705b22d9790e6f_Out_2);
            float _Property_0fe64435ba56ff879feaba2cf0de3653_Out_0 = Vector1_2CC6A532;
            float _Divide_0587d465a294fd89939e408897defbd1_Out_2;
            Unity_Divide_float(_Distance_ff895df4a75529858f705b22d9790e6f_Out_2, _Property_0fe64435ba56ff879feaba2cf0de3653_Out_0, _Divide_0587d465a294fd89939e408897defbd1_Out_2);
            float _OneMinus_0363875b7dd6248bba46e34770a6c6fd_Out_1;
            Unity_OneMinus_float(_Divide_0587d465a294fd89939e408897defbd1_Out_2, _OneMinus_0363875b7dd6248bba46e34770a6c6fd_Out_1);
            float _Multiply_75b21e80be4d4a8abdec6167a9a879ea_Out_2;
            Unity_Multiply_float_float(_OneMinus_0363875b7dd6248bba46e34770a6c6fd_Out_1, 50, _Multiply_75b21e80be4d4a8abdec6167a9a879ea_Out_2);
            float _Saturate_c4be325dac914182ab77534042191372_Out_1;
            Unity_Saturate_float(_Multiply_75b21e80be4d4a8abdec6167a9a879ea_Out_2, _Saturate_c4be325dac914182ab77534042191372_Out_1);
            float4 _UV_e936376873578889a784c059cac9327f_Out_0 = IN.uv0;
            float _Split_d6224afcec234a87a815fbd7c3b993be_R_1 = _UV_e936376873578889a784c059cac9327f_Out_0[0];
            float _Split_d6224afcec234a87a815fbd7c3b993be_G_2 = _UV_e936376873578889a784c059cac9327f_Out_0[1];
            float _Split_d6224afcec234a87a815fbd7c3b993be_B_3 = _UV_e936376873578889a784c059cac9327f_Out_0[2];
            float _Split_d6224afcec234a87a815fbd7c3b993be_A_4 = _UV_e936376873578889a784c059cac9327f_Out_0[3];
            float3 _Vector3_eec7a6eff655d081ac3a491a93b235ff_Out_0 = float3(_Split_d6224afcec234a87a815fbd7c3b993be_R_1, _Split_d6224afcec234a87a815fbd7c3b993be_G_2, _Split_d6224afcec234a87a815fbd7c3b993be_B_3);
            float4 _Property_2c852725881ee48c8a26f7a67d2f1af8_Out_0 = _WorldSpaceLightPos0;
            float4 _Negate_ce031150af1a89899d1ef252e40707ff_Out_1;
            Unity_Negate_float4(_Property_2c852725881ee48c8a26f7a67d2f1af8_Out_0, _Negate_ce031150af1a89899d1ef252e40707ff_Out_1);
            float _Distance_257b9818ac8541888148100201a66567_Out_2;
            Unity_Distance_float3(_Vector3_eec7a6eff655d081ac3a491a93b235ff_Out_0, (_Negate_ce031150af1a89899d1ef252e40707ff_Out_1.xyz), _Distance_257b9818ac8541888148100201a66567_Out_2);
            float _Property_0feaaad14730b686a4e93b150fbbd4c8_Out_0 = Vector1_166C34A9;
            float _Divide_b197c98807fa1181801202e86d510586_Out_2;
            Unity_Divide_float(_Distance_257b9818ac8541888148100201a66567_Out_2, _Property_0feaaad14730b686a4e93b150fbbd4c8_Out_0, _Divide_b197c98807fa1181801202e86d510586_Out_2);
            float _OneMinus_625be93cef0d048db0514d5b13ff864c_Out_1;
            Unity_OneMinus_float(_Divide_b197c98807fa1181801202e86d510586_Out_2, _OneMinus_625be93cef0d048db0514d5b13ff864c_Out_1);
            float _Multiply_da7ff55506a80a808c50a04c7f15dc60_Out_2;
            Unity_Multiply_float_float(_OneMinus_625be93cef0d048db0514d5b13ff864c_Out_1, 50, _Multiply_da7ff55506a80a808c50a04c7f15dc60_Out_2);
            float _Saturate_db3ab469449ce689a769d01ff2d036e9_Out_1;
            Unity_Saturate_float(_Multiply_da7ff55506a80a808c50a04c7f15dc60_Out_2, _Saturate_db3ab469449ce689a769d01ff2d036e9_Out_1);
            float4 _UV_6754638cc40dbd8e99cc1687affef5f2_Out_0 = IN.uv0;
            float _Split_3cafbe28397b318cba1225bf0e6f8880_R_1 = _UV_6754638cc40dbd8e99cc1687affef5f2_Out_0[0];
            float _Split_3cafbe28397b318cba1225bf0e6f8880_G_2 = _UV_6754638cc40dbd8e99cc1687affef5f2_Out_0[1];
            float _Split_3cafbe28397b318cba1225bf0e6f8880_B_3 = _UV_6754638cc40dbd8e99cc1687affef5f2_Out_0[2];
            float _Split_3cafbe28397b318cba1225bf0e6f8880_A_4 = _UV_6754638cc40dbd8e99cc1687affef5f2_Out_0[3];
            float _Property_5a81183edb8e818d876f56e4c20ad73c_Out_0 = Vector1_6F3D4927;
            float _Add_0402b6fd1f4fad81a7706cc7fedf8ba8_Out_2;
            Unity_Add_float(_Split_3cafbe28397b318cba1225bf0e6f8880_R_1, _Property_5a81183edb8e818d876f56e4c20ad73c_Out_0, _Add_0402b6fd1f4fad81a7706cc7fedf8ba8_Out_2);
            float3 _Vector3_1397d5af1f57228db629a77eeccdfcd1_Out_0 = float3(_Add_0402b6fd1f4fad81a7706cc7fedf8ba8_Out_2, _Split_3cafbe28397b318cba1225bf0e6f8880_G_2, _Split_3cafbe28397b318cba1225bf0e6f8880_B_3);
            float3 _Normalize_f34bc19600a8a383956053b20d3bfdc0_Out_1;
            Unity_Normalize_float3(_Vector3_1397d5af1f57228db629a77eeccdfcd1_Out_0, _Normalize_f34bc19600a8a383956053b20d3bfdc0_Out_1);
            float4 _Property_6bcf6f441c043b8189591940db1682fa_Out_0 = _WorldSpaceLightPos0;
            float4 _Negate_5fe44f8bcb08318483e796e4c1b30944_Out_1;
            Unity_Negate_float4(_Property_6bcf6f441c043b8189591940db1682fa_Out_0, _Negate_5fe44f8bcb08318483e796e4c1b30944_Out_1);
            float _Distance_bfb26dfd6a57e7858842c6992040534d_Out_2;
            Unity_Distance_float3(_Normalize_f34bc19600a8a383956053b20d3bfdc0_Out_1, (_Negate_5fe44f8bcb08318483e796e4c1b30944_Out_1.xyz), _Distance_bfb26dfd6a57e7858842c6992040534d_Out_2);
            float _Property_6fd2a3003ecb238fbb8d1a382079f268_Out_0 = Vector1_166C34A9;
            float _Divide_e6c93f950c35f981bcfb3c3944b34171_Out_2;
            Unity_Divide_float(_Distance_bfb26dfd6a57e7858842c6992040534d_Out_2, _Property_6fd2a3003ecb238fbb8d1a382079f268_Out_0, _Divide_e6c93f950c35f981bcfb3c3944b34171_Out_2);
            float _OneMinus_4b48cc99607d348795d464ca806441b1_Out_1;
            Unity_OneMinus_float(_Divide_e6c93f950c35f981bcfb3c3944b34171_Out_2, _OneMinus_4b48cc99607d348795d464ca806441b1_Out_1);
            float _Multiply_5dbdfc53361dc482ae6537bc67fb289e_Out_2;
            Unity_Multiply_float_float(_OneMinus_4b48cc99607d348795d464ca806441b1_Out_1, 50, _Multiply_5dbdfc53361dc482ae6537bc67fb289e_Out_2);
            float _Saturate_0fc96c5211198d8599b0059ddcbde563_Out_1;
            Unity_Saturate_float(_Multiply_5dbdfc53361dc482ae6537bc67fb289e_Out_2, _Saturate_0fc96c5211198d8599b0059ddcbde563_Out_1);
            float _Subtract_d77979008ac9658d888fd0e83a2e069b_Out_2;
            Unity_Subtract_float(_Saturate_db3ab469449ce689a769d01ff2d036e9_Out_1, _Saturate_0fc96c5211198d8599b0059ddcbde563_Out_1, _Subtract_d77979008ac9658d888fd0e83a2e069b_Out_2);
            float _Saturate_3c36af90f61c128b8f1ad55dca7ff322_Out_1;
            Unity_Saturate_float(_Subtract_d77979008ac9658d888fd0e83a2e069b_Out_2, _Saturate_3c36af90f61c128b8f1ad55dca7ff322_Out_1);
            float _Add_e7ac88e4962b11828b5e87b11b5436fd_Out_2;
            Unity_Add_float(_Saturate_c4be325dac914182ab77534042191372_Out_1, _Saturate_3c36af90f61c128b8f1ad55dca7ff322_Out_1, _Add_e7ac88e4962b11828b5e87b11b5436fd_Out_2);
            float _Split_90ab6a881a5c7488a68aebfcb8d951ea_R_1 = IN.AbsoluteWorldSpacePosition[0];
            float _Split_90ab6a881a5c7488a68aebfcb8d951ea_G_2 = IN.AbsoluteWorldSpacePosition[1];
            float _Split_90ab6a881a5c7488a68aebfcb8d951ea_B_3 = IN.AbsoluteWorldSpacePosition[2];
            float _Split_90ab6a881a5c7488a68aebfcb8d951ea_A_4 = 0;
            float _Saturate_ba8da1149d014786aae546b8dfa1c99b_Out_1;
            Unity_Saturate_float(_Split_90ab6a881a5c7488a68aebfcb8d951ea_G_2, _Saturate_ba8da1149d014786aae546b8dfa1c99b_Out_1);
            float _Multiply_5c1b8e5b5fb9e289a5a0db7f732c7215_Out_2;
            Unity_Multiply_float_float(10, _Saturate_ba8da1149d014786aae546b8dfa1c99b_Out_1, _Multiply_5c1b8e5b5fb9e289a5a0db7f732c7215_Out_2);
            float _Multiply_38b3ce781b4649b4b1178af45e44aeb9_Out_2;
            Unity_Multiply_float_float(_Add_e7ac88e4962b11828b5e87b11b5436fd_Out_2, _Multiply_5c1b8e5b5fb9e289a5a0db7f732c7215_Out_2, _Multiply_38b3ce781b4649b4b1178af45e44aeb9_Out_2);
            float4 _Property_449a7bd62271b58595e19c65e5edd0cf_Out_0 = _WorldSpaceLightPos0;
            float _Split_4c1596dbed67828bba6c8b8fa23c0690_R_1 = _Property_449a7bd62271b58595e19c65e5edd0cf_Out_0[0];
            float _Split_4c1596dbed67828bba6c8b8fa23c0690_G_2 = _Property_449a7bd62271b58595e19c65e5edd0cf_Out_0[1];
            float _Split_4c1596dbed67828bba6c8b8fa23c0690_B_3 = _Property_449a7bd62271b58595e19c65e5edd0cf_Out_0[2];
            float _Split_4c1596dbed67828bba6c8b8fa23c0690_A_4 = _Property_449a7bd62271b58595e19c65e5edd0cf_Out_0[3];
            float _Saturate_2e92d86ef607bf86997f0ad5ecec839b_Out_1;
            Unity_Saturate_float(_Split_4c1596dbed67828bba6c8b8fa23c0690_G_2, _Saturate_2e92d86ef607bf86997f0ad5ecec839b_Out_1);
            float _OneMinus_9b7918c50c25d38ab6ce3a9e3e7380ff_Out_1;
            Unity_OneMinus_float(_Saturate_2e92d86ef607bf86997f0ad5ecec839b_Out_1, _OneMinus_9b7918c50c25d38ab6ce3a9e3e7380ff_Out_1);
            float _Add_0e5748443be65d8ca545ade667d9cb81_Out_2;
            Unity_Add_float(_Split_4c1596dbed67828bba6c8b8fa23c0690_G_2, 0.5, _Add_0e5748443be65d8ca545ade667d9cb81_Out_2);
            float _Multiply_1415b3f0c08fa784af8c430a80f7777b_Out_2;
            Unity_Multiply_float_float(_OneMinus_9b7918c50c25d38ab6ce3a9e3e7380ff_Out_1, _Add_0e5748443be65d8ca545ade667d9cb81_Out_2, _Multiply_1415b3f0c08fa784af8c430a80f7777b_Out_2);
            float4 _UV_9d087471db9ffe84b5a1c7b7becb55e6_Out_0 = IN.uv0;
            float _Split_1b8c0959e329b385967d3d932dac9901_R_1 = _UV_9d087471db9ffe84b5a1c7b7becb55e6_Out_0[0];
            float _Split_1b8c0959e329b385967d3d932dac9901_G_2 = _UV_9d087471db9ffe84b5a1c7b7becb55e6_Out_0[1];
            float _Split_1b8c0959e329b385967d3d932dac9901_B_3 = _UV_9d087471db9ffe84b5a1c7b7becb55e6_Out_0[2];
            float _Split_1b8c0959e329b385967d3d932dac9901_A_4 = _UV_9d087471db9ffe84b5a1c7b7becb55e6_Out_0[3];
            float _Property_a593d65b892d4f8d8a57e7bae635b513_Out_0 = Vector1_3B7A233E;
            float _Multiply_78651d206fc51e83ae75fd215e6f7c1f_Out_2;
            Unity_Multiply_float_float(_Split_1b8c0959e329b385967d3d932dac9901_G_2, _Property_a593d65b892d4f8d8a57e7bae635b513_Out_0, _Multiply_78651d206fc51e83ae75fd215e6f7c1f_Out_2);
            float _Absolute_fcd0d3d2a50a5b82b1b195c030e1f887_Out_1;
            Unity_Absolute_float(_Multiply_78651d206fc51e83ae75fd215e6f7c1f_Out_2, _Absolute_fcd0d3d2a50a5b82b1b195c030e1f887_Out_1);
            float _OneMinus_47a3c0d16657e880b27eeb5a6cb27342_Out_1;
            Unity_OneMinus_float(_Absolute_fcd0d3d2a50a5b82b1b195c030e1f887_Out_1, _OneMinus_47a3c0d16657e880b27eeb5a6cb27342_Out_1);
            float _Saturate_2c2df04886cb7585958ba86fb28b7ca0_Out_1;
            Unity_Saturate_float(_OneMinus_47a3c0d16657e880b27eeb5a6cb27342_Out_1, _Saturate_2c2df04886cb7585958ba86fb28b7ca0_Out_1);
            float _Property_beb1e8cc9dae3486951f5e0e21663d89_Out_0 = Vector1_D80769F7;
            float _Add_7e6ba6ac8f954c89bf788a8b0f8ee011_Out_2;
            Unity_Add_float(_Split_1b8c0959e329b385967d3d932dac9901_G_2, _Property_beb1e8cc9dae3486951f5e0e21663d89_Out_0, _Add_7e6ba6ac8f954c89bf788a8b0f8ee011_Out_2);
            float _Multiply_8dd509e1d84dce8d843ccbf5cc46aa72_Out_2;
            Unity_Multiply_float_float(_Saturate_2c2df04886cb7585958ba86fb28b7ca0_Out_1, _Add_7e6ba6ac8f954c89bf788a8b0f8ee011_Out_2, _Multiply_8dd509e1d84dce8d843ccbf5cc46aa72_Out_2);
            float _Multiply_59bb5eb1228a6c88b9785edcb4ee802a_Out_2;
            Unity_Multiply_float_float(_Multiply_1415b3f0c08fa784af8c430a80f7777b_Out_2, _Multiply_8dd509e1d84dce8d843ccbf5cc46aa72_Out_2, _Multiply_59bb5eb1228a6c88b9785edcb4ee802a_Out_2);
            float _Saturate_3eebdc0a9ddf808db1b9136fde7711cb_Out_1;
            Unity_Saturate_float(_Multiply_59bb5eb1228a6c88b9785edcb4ee802a_Out_2, _Saturate_3eebdc0a9ddf808db1b9136fde7711cb_Out_1);
            float4 _Property_759cf5dffca2ad8bb5cf3adca8685fbc_Out_0 = Color_1EB49FED;
            float4 _Multiply_4df21b54919e8b8aa5be3e130cbd7f83_Out_2;
            Unity_Multiply_float4_float4((_Saturate_3eebdc0a9ddf808db1b9136fde7711cb_Out_1.xxxx), _Property_759cf5dffca2ad8bb5cf3adca8685fbc_Out_0, _Multiply_4df21b54919e8b8aa5be3e130cbd7f83_Out_2);
            float _OneMinus_0cddf1e404bc3a83af7197c852ff6981_Out_1;
            Unity_OneMinus_float(_Saturate_db3ab469449ce689a769d01ff2d036e9_Out_1, _OneMinus_0cddf1e404bc3a83af7197c852ff6981_Out_1);
            float4 _Property_2bf3de1a71d24b3398a6cd81d58e793c_Out_0 = _WorldSpaceLightPos0;
            float _Split_ec432a49e18b466e8faed4ee9eff6dcd_R_1 = _Property_2bf3de1a71d24b3398a6cd81d58e793c_Out_0[0];
            float _Split_ec432a49e18b466e8faed4ee9eff6dcd_G_2 = _Property_2bf3de1a71d24b3398a6cd81d58e793c_Out_0[1];
            float _Split_ec432a49e18b466e8faed4ee9eff6dcd_B_3 = _Property_2bf3de1a71d24b3398a6cd81d58e793c_Out_0[2];
            float _Split_ec432a49e18b466e8faed4ee9eff6dcd_A_4 = _Property_2bf3de1a71d24b3398a6cd81d58e793c_Out_0[3];
            float _Multiply_5073197e5f6141fca0c241c8dbdbd38b_Out_2;
            Unity_Multiply_float_float(_Split_ec432a49e18b466e8faed4ee9eff6dcd_G_2, 5, _Multiply_5073197e5f6141fca0c241c8dbdbd38b_Out_2);
            float _Saturate_94abf88d7985487a86d5553af9108b51_Out_1;
            Unity_Saturate_float(_Multiply_5073197e5f6141fca0c241c8dbdbd38b_Out_2, _Saturate_94abf88d7985487a86d5553af9108b51_Out_1);
            float _OneMinus_5e2154c8eca34e60bad8e2a41f8ca3b1_Out_1;
            Unity_OneMinus_float(_Saturate_94abf88d7985487a86d5553af9108b51_Out_1, _OneMinus_5e2154c8eca34e60bad8e2a41f8ca3b1_Out_1);
            float3 _Normalize_102866732eac4470b5a5548826e2896d_Out_1;
            Unity_Normalize_float3(IN.WorldSpacePosition, _Normalize_102866732eac4470b5a5548826e2896d_Out_1);
            float _Split_9ed555aa0a8b4a9093d977291669120a_R_1 = IN.WorldSpacePosition[0];
            float _Split_9ed555aa0a8b4a9093d977291669120a_G_2 = IN.WorldSpacePosition[1];
            float _Split_9ed555aa0a8b4a9093d977291669120a_B_3 = IN.WorldSpacePosition[2];
            float _Split_9ed555aa0a8b4a9093d977291669120a_A_4 = 0;
            float2 _Vector2_a3077e429ece419ca2d94bd1ba3ba29f_Out_0 = float2(_Split_9ed555aa0a8b4a9093d977291669120a_R_1, _Split_9ed555aa0a8b4a9093d977291669120a_B_3);
            float2 _Divide_d993fd29798a4d829af311ec5e0fc4ac_Out_2;
            Unity_Divide_float2(_Vector2_a3077e429ece419ca2d94bd1ba3ba29f_Out_0, (_Split_9ed555aa0a8b4a9093d977291669120a_G_2.xx), _Divide_d993fd29798a4d829af311ec5e0fc4ac_Out_2);
            float2 _TilingAndOffset_5178df25d09b4f74b0a9b1d835dbdd09_Out_3;
            Unity_TilingAndOffset_float((_Normalize_102866732eac4470b5a5548826e2896d_Out_1.xy), float2 (1, 1), _Divide_d993fd29798a4d829af311ec5e0fc4ac_Out_2, _TilingAndOffset_5178df25d09b4f74b0a9b1d835dbdd09_Out_3);
            float _Property_9184c92d6f0b4d4b9b0a66dc68ee79f8_Out_0 = _etoiles_rotations_1;
            float _Property_f596ae4aafa44338892e6e4f7ad574a3_Out_0 = _etoiles_densite;
            float _Voronoi_a2516c10487f4466b69bafd33f483916_Out_3;
            float _Voronoi_a2516c10487f4466b69bafd33f483916_Cells_4;
            Unity_Voronoi_LegacySine_float(_TilingAndOffset_5178df25d09b4f74b0a9b1d835dbdd09_Out_3, _Property_9184c92d6f0b4d4b9b0a66dc68ee79f8_Out_0, _Property_f596ae4aafa44338892e6e4f7ad574a3_Out_0, _Voronoi_a2516c10487f4466b69bafd33f483916_Out_3, _Voronoi_a2516c10487f4466b69bafd33f483916_Cells_4);
            float _Saturate_c8ccf88436fc48e59e98f3dbacaa15b4_Out_1;
            Unity_Saturate_float(_Voronoi_a2516c10487f4466b69bafd33f483916_Out_3, _Saturate_c8ccf88436fc48e59e98f3dbacaa15b4_Out_1);
            float _OneMinus_4c4814502b184131838346c18857c9d6_Out_1;
            Unity_OneMinus_float(_Saturate_c8ccf88436fc48e59e98f3dbacaa15b4_Out_1, _OneMinus_4c4814502b184131838346c18857c9d6_Out_1);
            float _Property_e1c10d6949ef4384926fb5909626a70a_Out_0 = _etoiles_powa;
            float _Power_8355d5640d4e460c8114708722d9b1f4_Out_2;
            Unity_Power_float(_OneMinus_4c4814502b184131838346c18857c9d6_Out_1, _Property_e1c10d6949ef4384926fb5909626a70a_Out_0, _Power_8355d5640d4e460c8114708722d9b1f4_Out_2);
            float _Saturate_cecbc392b0d2463daa1c77fa3cb411c2_Out_1;
            Unity_Saturate_float(_Power_8355d5640d4e460c8114708722d9b1f4_Out_2, _Saturate_cecbc392b0d2463daa1c77fa3cb411c2_Out_1);
            float _Property_e97f030b15d84284b6f440ceb957a22c_Out_0 = _etoiles_intensite;
            float _Multiply_d9abcf011f9049a4a0c14d4b0b653c84_Out_2;
            Unity_Multiply_float_float(_Saturate_cecbc392b0d2463daa1c77fa3cb411c2_Out_1, _Property_e97f030b15d84284b6f440ceb957a22c_Out_0, _Multiply_d9abcf011f9049a4a0c14d4b0b653c84_Out_2);
            float _Multiply_86666c5a2dd34ba7bc7be5a423e6725c_Out_2;
            Unity_Multiply_float_float(_OneMinus_5e2154c8eca34e60bad8e2a41f8ca3b1_Out_1, _Multiply_d9abcf011f9049a4a0c14d4b0b653c84_Out_2, _Multiply_86666c5a2dd34ba7bc7be5a423e6725c_Out_2);
            float _Multiply_d90ade0e128c4c8f97ffc0aa93abfa95_Out_2;
            Unity_Multiply_float_float(_OneMinus_0cddf1e404bc3a83af7197c852ff6981_Out_1, _Multiply_86666c5a2dd34ba7bc7be5a423e6725c_Out_2, _Multiply_d90ade0e128c4c8f97ffc0aa93abfa95_Out_2);
            float _Multiply_e1794edd9caa3a829aab6edba6d11eeb_Out_2;
            Unity_Multiply_float_float(_Multiply_d90ade0e128c4c8f97ffc0aa93abfa95_Out_2, _Multiply_5c1b8e5b5fb9e289a5a0db7f732c7215_Out_2, _Multiply_e1794edd9caa3a829aab6edba6d11eeb_Out_2);
            float4 _Property_663923b1ab40ab899f15d6982444d339_Out_0 = Color_83CF459;
            float4 _Property_0cd7139856da4e89a7ec36232817d185_Out_0 = Color_D230EFA1;
            float4 _UV_0bb570b9da8e5e8fa558678aaa952c29_Out_0 = IN.uv0;
            float _Split_25141b555a0be18ab9be0d13c0a8a43b_R_1 = _UV_0bb570b9da8e5e8fa558678aaa952c29_Out_0[0];
            float _Split_25141b555a0be18ab9be0d13c0a8a43b_G_2 = _UV_0bb570b9da8e5e8fa558678aaa952c29_Out_0[1];
            float _Split_25141b555a0be18ab9be0d13c0a8a43b_B_3 = _UV_0bb570b9da8e5e8fa558678aaa952c29_Out_0[2];
            float _Split_25141b555a0be18ab9be0d13c0a8a43b_A_4 = _UV_0bb570b9da8e5e8fa558678aaa952c29_Out_0[3];
            float _Saturate_bfabc1f11c42b489bee718e18c61afe5_Out_1;
            Unity_Saturate_float(_Split_25141b555a0be18ab9be0d13c0a8a43b_G_2, _Saturate_bfabc1f11c42b489bee718e18c61afe5_Out_1);
            float4 _Lerp_927359091c0f3187be61d805ebfa921e_Out_3;
            Unity_Lerp_float4(_Property_663923b1ab40ab899f15d6982444d339_Out_0, _Property_0cd7139856da4e89a7ec36232817d185_Out_0, (_Saturate_bfabc1f11c42b489bee718e18c61afe5_Out_1.xxxx), _Lerp_927359091c0f3187be61d805ebfa921e_Out_3);
            float4 _Property_b295c6cd5b7eb4828c59a8d3791336c6_Out_0 = Color_68FD0CD8;
            float4 _Property_df133a02ae17e6828fd6c02b52a3ece8_Out_0 = Color_BE31CDF2;
            float4 _UV_63f2eedaa7d7a186838a7d27ff29877b_Out_0 = IN.uv0;
            float _Split_8ab9d2401a966d8d9b19fa59d83c3785_R_1 = _UV_63f2eedaa7d7a186838a7d27ff29877b_Out_0[0];
            float _Split_8ab9d2401a966d8d9b19fa59d83c3785_G_2 = _UV_63f2eedaa7d7a186838a7d27ff29877b_Out_0[1];
            float _Split_8ab9d2401a966d8d9b19fa59d83c3785_B_3 = _UV_63f2eedaa7d7a186838a7d27ff29877b_Out_0[2];
            float _Split_8ab9d2401a966d8d9b19fa59d83c3785_A_4 = _UV_63f2eedaa7d7a186838a7d27ff29877b_Out_0[3];
            float _Saturate_5a4aa969c530cb89b3f45769171e89cc_Out_1;
            Unity_Saturate_float(_Split_8ab9d2401a966d8d9b19fa59d83c3785_G_2, _Saturate_5a4aa969c530cb89b3f45769171e89cc_Out_1);
            float4 _Lerp_b0cc5cebd9f98e85942af72261fe877a_Out_3;
            Unity_Lerp_float4(_Property_b295c6cd5b7eb4828c59a8d3791336c6_Out_0, _Property_df133a02ae17e6828fd6c02b52a3ece8_Out_0, (_Saturate_5a4aa969c530cb89b3f45769171e89cc_Out_1.xxxx), _Lerp_b0cc5cebd9f98e85942af72261fe877a_Out_3);
            float4 _Property_a16eceb332aa738f983a9252f2eb2a65_Out_0 = _WorldSpaceLightPos0;
            float _Split_ef8cbd1ee3991d8497c3a12f8238a404_R_1 = _Property_a16eceb332aa738f983a9252f2eb2a65_Out_0[0];
            float _Split_ef8cbd1ee3991d8497c3a12f8238a404_G_2 = _Property_a16eceb332aa738f983a9252f2eb2a65_Out_0[1];
            float _Split_ef8cbd1ee3991d8497c3a12f8238a404_B_3 = _Property_a16eceb332aa738f983a9252f2eb2a65_Out_0[2];
            float _Split_ef8cbd1ee3991d8497c3a12f8238a404_A_4 = _Property_a16eceb332aa738f983a9252f2eb2a65_Out_0[3];
            float _Saturate_2501276a5ef6c586bcc6332699716cb3_Out_1;
            Unity_Saturate_float(_Split_ef8cbd1ee3991d8497c3a12f8238a404_G_2, _Saturate_2501276a5ef6c586bcc6332699716cb3_Out_1);
            float4 _Lerp_de8806d8b42fec858f0290bb682ea8ef_Out_3;
            Unity_Lerp_float4(_Lerp_927359091c0f3187be61d805ebfa921e_Out_3, _Lerp_b0cc5cebd9f98e85942af72261fe877a_Out_3, (_Saturate_2501276a5ef6c586bcc6332699716cb3_Out_1.xxxx), _Lerp_de8806d8b42fec858f0290bb682ea8ef_Out_3);
            float4 _Add_af1fdd6f0ea0ba8796643c403a93eed1_Out_2;
            Unity_Add_float4((_Multiply_e1794edd9caa3a829aab6edba6d11eeb_Out_2.xxxx), _Lerp_de8806d8b42fec858f0290bb682ea8ef_Out_3, _Add_af1fdd6f0ea0ba8796643c403a93eed1_Out_2);
            float4 _Add_aebf5f14a960928d877890408fcec2f5_Out_2;
            Unity_Add_float4(_Multiply_4df21b54919e8b8aa5be3e130cbd7f83_Out_2, _Add_af1fdd6f0ea0ba8796643c403a93eed1_Out_2, _Add_aebf5f14a960928d877890408fcec2f5_Out_2);
            float4 _Property_fee7bae45ed045ea9d47a6cd5bce27f3_Out_0 = _WorldSpaceLightPos0;
            float4 _Negate_677526b0b8c1409d8500e85e57860856_Out_1;
            Unity_Negate_float4(_Property_fee7bae45ed045ea9d47a6cd5bce27f3_Out_0, _Negate_677526b0b8c1409d8500e85e57860856_Out_1);
            float _Split_e54d32944af342939614296bb3882552_R_1 = _Negate_677526b0b8c1409d8500e85e57860856_Out_1[0];
            float _Split_e54d32944af342939614296bb3882552_G_2 = _Negate_677526b0b8c1409d8500e85e57860856_Out_1[1];
            float _Split_e54d32944af342939614296bb3882552_B_3 = _Negate_677526b0b8c1409d8500e85e57860856_Out_1[2];
            float _Split_e54d32944af342939614296bb3882552_A_4 = _Negate_677526b0b8c1409d8500e85e57860856_Out_1[3];
            float _Multiply_492c22c902ee4688afa2c4922c35479f_Out_2;
            Unity_Multiply_float_float(_Split_e54d32944af342939614296bb3882552_G_2, 5, _Multiply_492c22c902ee4688afa2c4922c35479f_Out_2);
            float _Saturate_5308504522434d118e5f260f191d71f3_Out_1;
            Unity_Saturate_float(_Multiply_492c22c902ee4688afa2c4922c35479f_Out_2, _Saturate_5308504522434d118e5f260f191d71f3_Out_1);
            float _OneMinus_ac438ce7d2f84670838c0c3ffbe6af40_Out_1;
            Unity_OneMinus_float(_Saturate_5308504522434d118e5f260f191d71f3_Out_1, _OneMinus_ac438ce7d2f84670838c0c3ffbe6af40_Out_1);
            UnityTextureCube _Property_5671c5fd8fee45d1a9f593037d0829f7_Out_0 = UnityBuildTextureCubeStruct(_nuageJour);
            float _Property_61bf2955f660441cb83bbfeb43cc13be_Out_0 = _test_rotate;
            float _Multiply_ac392346f509442a9a4ad630d61191a9_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_61bf2955f660441cb83bbfeb43cc13be_Out_0, _Multiply_ac392346f509442a9a4ad630d61191a9_Out_2);
            float3 _RotateAboutAxis_033389c5653f4fa6b0b570f3e85178cb_Out_3;
            Unity_Rotate_About_Axis_Degrees_float(IN.WorldSpaceNormal, float3 (0, 1, 0), _Multiply_ac392346f509442a9a4ad630d61191a9_Out_2, _RotateAboutAxis_033389c5653f4fa6b0b570f3e85178cb_Out_3);
            float4 _SampleReflectedCubemap_1a970f2d39af42998ef1960dc5f96326_Out_0 = SAMPLE_TEXTURECUBE_LOD(_Property_5671c5fd8fee45d1a9f593037d0829f7_Out_0.tex, _Property_5671c5fd8fee45d1a9f593037d0829f7_Out_0.samplerstate, reflect(-IN.ObjectSpaceViewDirection, _RotateAboutAxis_033389c5653f4fa6b0b570f3e85178cb_Out_3), 0);
            float4 _Saturate_2eab6b72e92b4b7797627c9b0c3b1c7c_Out_1;
            Unity_Saturate_float4(_SampleReflectedCubemap_1a970f2d39af42998ef1960dc5f96326_Out_0, _Saturate_2eab6b72e92b4b7797627c9b0c3b1c7c_Out_1);
            float4 _Multiply_1a10b6b7a3f44d5ca32b2eb4e37c09cf_Out_2;
            Unity_Multiply_float4_float4((_OneMinus_ac438ce7d2f84670838c0c3ffbe6af40_Out_1.xxxx), _Saturate_2eab6b72e92b4b7797627c9b0c3b1c7c_Out_1, _Multiply_1a10b6b7a3f44d5ca32b2eb4e37c09cf_Out_2);
            float4 _Property_fa1fa549c49942e99cd5dc4cda656720_Out_0 = _WorldSpaceLightPos0;
            float _Split_d89b5fce1cd44e05aa505df30c1117a8_R_1 = _Property_fa1fa549c49942e99cd5dc4cda656720_Out_0[0];
            float _Split_d89b5fce1cd44e05aa505df30c1117a8_G_2 = _Property_fa1fa549c49942e99cd5dc4cda656720_Out_0[1];
            float _Split_d89b5fce1cd44e05aa505df30c1117a8_B_3 = _Property_fa1fa549c49942e99cd5dc4cda656720_Out_0[2];
            float _Split_d89b5fce1cd44e05aa505df30c1117a8_A_4 = _Property_fa1fa549c49942e99cd5dc4cda656720_Out_0[3];
            float _Multiply_80f6e1615e394f428a4521a5f04da13f_Out_2;
            Unity_Multiply_float_float(_Split_d89b5fce1cd44e05aa505df30c1117a8_G_2, 5, _Multiply_80f6e1615e394f428a4521a5f04da13f_Out_2);
            float _Saturate_65009cf085f942eba890548ccb7449a5_Out_1;
            Unity_Saturate_float(_Multiply_80f6e1615e394f428a4521a5f04da13f_Out_2, _Saturate_65009cf085f942eba890548ccb7449a5_Out_1);
            float _OneMinus_c3d1302e3d914e96a5388e43e2a98e76_Out_1;
            Unity_OneMinus_float(_Saturate_65009cf085f942eba890548ccb7449a5_Out_1, _OneMinus_c3d1302e3d914e96a5388e43e2a98e76_Out_1);
            UnityTextureCube _Property_21752d27a8e949f9bbe6d81b0df7b885_Out_0 = UnityBuildTextureCubeStruct(_NuagesNuit);
            float4 _SampleReflectedCubemap_14c463d4a4c24054aa713fb7c05a55da_Out_0 = SAMPLE_TEXTURECUBE_LOD(_Property_21752d27a8e949f9bbe6d81b0df7b885_Out_0.tex, _Property_21752d27a8e949f9bbe6d81b0df7b885_Out_0.samplerstate, reflect(-IN.ObjectSpaceViewDirection, _RotateAboutAxis_033389c5653f4fa6b0b570f3e85178cb_Out_3), 0);
            float4 _Saturate_2ef4afa901ad491b9ef6973d67c2b14f_Out_1;
            Unity_Saturate_float4(_SampleReflectedCubemap_14c463d4a4c24054aa713fb7c05a55da_Out_0, _Saturate_2ef4afa901ad491b9ef6973d67c2b14f_Out_1);
            float4 _Multiply_782d24bb9913401b93214677290fcc21_Out_2;
            Unity_Multiply_float4_float4((_OneMinus_c3d1302e3d914e96a5388e43e2a98e76_Out_1.xxxx), _Saturate_2ef4afa901ad491b9ef6973d67c2b14f_Out_1, _Multiply_782d24bb9913401b93214677290fcc21_Out_2);
            float4 _Add_1c7edc79043a46c78fda19f88574e8c3_Out_2;
            Unity_Add_float4(_Multiply_1a10b6b7a3f44d5ca32b2eb4e37c09cf_Out_2, _Multiply_782d24bb9913401b93214677290fcc21_Out_2, _Add_1c7edc79043a46c78fda19f88574e8c3_Out_2);
            float4 _Add_c00abf15fbfa4660abadf59765bc2902_Out_2;
            Unity_Add_float4(_Add_aebf5f14a960928d877890408fcec2f5_Out_2, _Add_1c7edc79043a46c78fda19f88574e8c3_Out_2, _Add_c00abf15fbfa4660abadf59765bc2902_Out_2);
            float4 _Add_c1ddb53ed299457589a80262aef9e4a2_Out_2;
            Unity_Add_float4((_Multiply_38b3ce781b4649b4b1178af45e44aeb9_Out_2.xxxx), _Add_c00abf15fbfa4660abadf59765bc2902_Out_2, _Add_c1ddb53ed299457589a80262aef9e4a2_Out_2);
            surface.BaseColor = (_Add_c1ddb53ed299457589a80262aef9e4a2_Out_2.xyz);
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
        
        
            output.WorldSpaceViewDirection = GetWorldSpaceNormalizeViewDir(input.positionWS);
            output.ObjectSpaceViewDirection = TransformWorldToObjectDir(output.WorldSpaceViewDirection);
            output.WorldSpacePosition = input.positionWS;
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormalsOnly"
            Tags
            {
                "LightMode" = "DepthNormalsOnly"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        #define _SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float Vector1_2CC6A532;
        float Vector1_166C34A9;
        float Vector1_6F3D4927;
        float4 Color_BE31CDF2;
        float4 Color_68FD0CD8;
        float4 Color_D230EFA1;
        float4 Color_83CF459;
        float4 Color_1EB49FED;
        float Vector1_3B7A233E;
        float Vector1_D80769F7;
        float _test_rotate;
        float _etoiles_intensite;
        float _etoiles_densite;
        float _etoiles_powa;
        float _etoiles_rotations_1;
        float _etoiles_rotations;
        float2 _Vector2;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        float4 _WorldSpaceLightPos0;
        TEXTURECUBE(_nuageJour);
        SAMPLER(sampler_nuageJour);
        TEXTURECUBE(_NuagesNuit);
        SAMPLER(sampler_NuagesNuit);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float Vector1_2CC6A532;
        float Vector1_166C34A9;
        float Vector1_6F3D4927;
        float4 Color_BE31CDF2;
        float4 Color_68FD0CD8;
        float4 Color_D230EFA1;
        float4 Color_83CF459;
        float4 Color_1EB49FED;
        float Vector1_3B7A233E;
        float Vector1_D80769F7;
        float _test_rotate;
        float _etoiles_intensite;
        float _etoiles_densite;
        float _etoiles_powa;
        float _etoiles_rotations_1;
        float _etoiles_rotations;
        float2 _Vector2;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        float4 _WorldSpaceLightPos0;
        TEXTURECUBE(_nuageJour);
        SAMPLER(sampler_nuageJour);
        TEXTURECUBE(_NuagesNuit);
        SAMPLER(sampler_NuagesNuit);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float Vector1_2CC6A532;
        float Vector1_166C34A9;
        float Vector1_6F3D4927;
        float4 Color_BE31CDF2;
        float4 Color_68FD0CD8;
        float4 Color_D230EFA1;
        float4 Color_83CF459;
        float4 Color_1EB49FED;
        float Vector1_3B7A233E;
        float Vector1_D80769F7;
        float _test_rotate;
        float _etoiles_intensite;
        float _etoiles_densite;
        float _etoiles_powa;
        float _etoiles_rotations_1;
        float _etoiles_rotations;
        float2 _Vector2;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        float4 _WorldSpaceLightPos0;
        TEXTURECUBE(_nuageJour);
        SAMPLER(sampler_nuageJour);
        TEXTURECUBE(_NuagesNuit);
        SAMPLER(sampler_NuagesNuit);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float Vector1_2CC6A532;
        float Vector1_166C34A9;
        float Vector1_6F3D4927;
        float4 Color_BE31CDF2;
        float4 Color_68FD0CD8;
        float4 Color_D230EFA1;
        float4 Color_83CF459;
        float4 Color_1EB49FED;
        float Vector1_3B7A233E;
        float Vector1_D80769F7;
        float _test_rotate;
        float _etoiles_intensite;
        float _etoiles_densite;
        float _etoiles_powa;
        float _etoiles_rotations_1;
        float _etoiles_rotations;
        float2 _Vector2;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        float4 _WorldSpaceLightPos0;
        TEXTURECUBE(_nuageJour);
        SAMPLER(sampler_nuageJour);
        TEXTURECUBE(_NuagesNuit);
        SAMPLER(sampler_NuagesNuit);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                // LightMode: <None>
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma target 3.5 DOTS_INSTANCING_ON
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpaceNormal;
             float3 ObjectSpaceViewDirection;
             float3 WorldSpaceViewDirection;
             float3 WorldSpacePosition;
             float3 AbsoluteWorldSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float Vector1_2CC6A532;
        float Vector1_166C34A9;
        float Vector1_6F3D4927;
        float4 Color_BE31CDF2;
        float4 Color_68FD0CD8;
        float4 Color_D230EFA1;
        float4 Color_83CF459;
        float4 Color_1EB49FED;
        float Vector1_3B7A233E;
        float Vector1_D80769F7;
        float _test_rotate;
        float _etoiles_intensite;
        float _etoiles_densite;
        float _etoiles_powa;
        float _etoiles_rotations_1;
        float _etoiles_rotations;
        float2 _Vector2;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        float4 _WorldSpaceLightPos0;
        TEXTURECUBE(_nuageJour);
        SAMPLER(sampler_nuageJour);
        TEXTURECUBE(_NuagesNuit);
        SAMPLER(sampler_NuagesNuit);
        
        // Graph Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Distance_float3(float3 A, float3 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Negate_float4(float4 In, out float4 Out)
        {
            Out = -1 * In;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Normalize_float3(float3 In, out float3 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Absolute_float(float In, out float Out)
        {
            Out = abs(In);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        float2 Unity_Voronoi_RandomVector_LegacySine_float (float2 UV, float offset)
        {
            Hash_LegacySine_2_2_float(UV, UV);
            return float2(sin(UV.y * offset), cos(UV.x * offset)) * 0.5 + 0.5;
        }
        
        void Unity_Voronoi_LegacySine_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
        {
            float2 g = floor(UV * CellDensity);
            float2 f = frac(UV * CellDensity);
            float t = 8.0;
            float3 res = float3(8.0, 0.0, 0.0);
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    float2 lattice = float2(x, y);
                    float2 offset = Unity_Voronoi_RandomVector_LegacySine_float(lattice + g, AngleOffset);
                    float d = distance(lattice + offset, f);
                    if (d < res.x)
                    {
                        res = float3(d, offset.x, offset.y);
                        Out = res.x;
                        Cells = res.y;
                    }
                }
            }
        }
        
        void Unity_Power_float(float A, float B, out float Out)
        {
            Out = pow(A, B);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Rotate_About_Axis_Degrees_float(float3 In, float3 Axis, float Rotation, out float3 Out)
        {
            Rotation = radians(Rotation);
        
            float s = sin(Rotation);
            float c = cos(Rotation);
            float one_minus_c = 1.0 - c;
        
            Axis = normalize(Axis);
        
            float3x3 rot_mat = { one_minus_c * Axis.x * Axis.x + c,            one_minus_c * Axis.x * Axis.y - Axis.z * s,     one_minus_c * Axis.z * Axis.x + Axis.y * s,
                                      one_minus_c * Axis.x * Axis.y + Axis.z * s,   one_minus_c * Axis.y * Axis.y + c,              one_minus_c * Axis.y * Axis.z - Axis.x * s,
                                      one_minus_c * Axis.z * Axis.x - Axis.y * s,   one_minus_c * Axis.y * Axis.z + Axis.x * s,     one_minus_c * Axis.z * Axis.z + c
                                    };
        
            Out = mul(rot_mat,  In);
        }
        
        void Unity_Saturate_float4(float4 In, out float4 Out)
        {
            Out = saturate(In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_c2d18d08d4344485ad00bdcc0f425b63_Out_0 = IN.uv0;
            float _Split_e5f4931c52992f888b864b285d2eb7a7_R_1 = _UV_c2d18d08d4344485ad00bdcc0f425b63_Out_0[0];
            float _Split_e5f4931c52992f888b864b285d2eb7a7_G_2 = _UV_c2d18d08d4344485ad00bdcc0f425b63_Out_0[1];
            float _Split_e5f4931c52992f888b864b285d2eb7a7_B_3 = _UV_c2d18d08d4344485ad00bdcc0f425b63_Out_0[2];
            float _Split_e5f4931c52992f888b864b285d2eb7a7_A_4 = _UV_c2d18d08d4344485ad00bdcc0f425b63_Out_0[3];
            float3 _Vector3_ac18fbaae3095a86ba63d4a7089b57e1_Out_0 = float3(_Split_e5f4931c52992f888b864b285d2eb7a7_R_1, _Split_e5f4931c52992f888b864b285d2eb7a7_G_2, _Split_e5f4931c52992f888b864b285d2eb7a7_B_3);
            float4 _Property_0024955546785f8dabe6ddf42235b27f_Out_0 = _WorldSpaceLightPos0;
            float _Distance_ff895df4a75529858f705b22d9790e6f_Out_2;
            Unity_Distance_float3(_Vector3_ac18fbaae3095a86ba63d4a7089b57e1_Out_0, (_Property_0024955546785f8dabe6ddf42235b27f_Out_0.xyz), _Distance_ff895df4a75529858f705b22d9790e6f_Out_2);
            float _Property_0fe64435ba56ff879feaba2cf0de3653_Out_0 = Vector1_2CC6A532;
            float _Divide_0587d465a294fd89939e408897defbd1_Out_2;
            Unity_Divide_float(_Distance_ff895df4a75529858f705b22d9790e6f_Out_2, _Property_0fe64435ba56ff879feaba2cf0de3653_Out_0, _Divide_0587d465a294fd89939e408897defbd1_Out_2);
            float _OneMinus_0363875b7dd6248bba46e34770a6c6fd_Out_1;
            Unity_OneMinus_float(_Divide_0587d465a294fd89939e408897defbd1_Out_2, _OneMinus_0363875b7dd6248bba46e34770a6c6fd_Out_1);
            float _Multiply_75b21e80be4d4a8abdec6167a9a879ea_Out_2;
            Unity_Multiply_float_float(_OneMinus_0363875b7dd6248bba46e34770a6c6fd_Out_1, 50, _Multiply_75b21e80be4d4a8abdec6167a9a879ea_Out_2);
            float _Saturate_c4be325dac914182ab77534042191372_Out_1;
            Unity_Saturate_float(_Multiply_75b21e80be4d4a8abdec6167a9a879ea_Out_2, _Saturate_c4be325dac914182ab77534042191372_Out_1);
            float4 _UV_e936376873578889a784c059cac9327f_Out_0 = IN.uv0;
            float _Split_d6224afcec234a87a815fbd7c3b993be_R_1 = _UV_e936376873578889a784c059cac9327f_Out_0[0];
            float _Split_d6224afcec234a87a815fbd7c3b993be_G_2 = _UV_e936376873578889a784c059cac9327f_Out_0[1];
            float _Split_d6224afcec234a87a815fbd7c3b993be_B_3 = _UV_e936376873578889a784c059cac9327f_Out_0[2];
            float _Split_d6224afcec234a87a815fbd7c3b993be_A_4 = _UV_e936376873578889a784c059cac9327f_Out_0[3];
            float3 _Vector3_eec7a6eff655d081ac3a491a93b235ff_Out_0 = float3(_Split_d6224afcec234a87a815fbd7c3b993be_R_1, _Split_d6224afcec234a87a815fbd7c3b993be_G_2, _Split_d6224afcec234a87a815fbd7c3b993be_B_3);
            float4 _Property_2c852725881ee48c8a26f7a67d2f1af8_Out_0 = _WorldSpaceLightPos0;
            float4 _Negate_ce031150af1a89899d1ef252e40707ff_Out_1;
            Unity_Negate_float4(_Property_2c852725881ee48c8a26f7a67d2f1af8_Out_0, _Negate_ce031150af1a89899d1ef252e40707ff_Out_1);
            float _Distance_257b9818ac8541888148100201a66567_Out_2;
            Unity_Distance_float3(_Vector3_eec7a6eff655d081ac3a491a93b235ff_Out_0, (_Negate_ce031150af1a89899d1ef252e40707ff_Out_1.xyz), _Distance_257b9818ac8541888148100201a66567_Out_2);
            float _Property_0feaaad14730b686a4e93b150fbbd4c8_Out_0 = Vector1_166C34A9;
            float _Divide_b197c98807fa1181801202e86d510586_Out_2;
            Unity_Divide_float(_Distance_257b9818ac8541888148100201a66567_Out_2, _Property_0feaaad14730b686a4e93b150fbbd4c8_Out_0, _Divide_b197c98807fa1181801202e86d510586_Out_2);
            float _OneMinus_625be93cef0d048db0514d5b13ff864c_Out_1;
            Unity_OneMinus_float(_Divide_b197c98807fa1181801202e86d510586_Out_2, _OneMinus_625be93cef0d048db0514d5b13ff864c_Out_1);
            float _Multiply_da7ff55506a80a808c50a04c7f15dc60_Out_2;
            Unity_Multiply_float_float(_OneMinus_625be93cef0d048db0514d5b13ff864c_Out_1, 50, _Multiply_da7ff55506a80a808c50a04c7f15dc60_Out_2);
            float _Saturate_db3ab469449ce689a769d01ff2d036e9_Out_1;
            Unity_Saturate_float(_Multiply_da7ff55506a80a808c50a04c7f15dc60_Out_2, _Saturate_db3ab469449ce689a769d01ff2d036e9_Out_1);
            float4 _UV_6754638cc40dbd8e99cc1687affef5f2_Out_0 = IN.uv0;
            float _Split_3cafbe28397b318cba1225bf0e6f8880_R_1 = _UV_6754638cc40dbd8e99cc1687affef5f2_Out_0[0];
            float _Split_3cafbe28397b318cba1225bf0e6f8880_G_2 = _UV_6754638cc40dbd8e99cc1687affef5f2_Out_0[1];
            float _Split_3cafbe28397b318cba1225bf0e6f8880_B_3 = _UV_6754638cc40dbd8e99cc1687affef5f2_Out_0[2];
            float _Split_3cafbe28397b318cba1225bf0e6f8880_A_4 = _UV_6754638cc40dbd8e99cc1687affef5f2_Out_0[3];
            float _Property_5a81183edb8e818d876f56e4c20ad73c_Out_0 = Vector1_6F3D4927;
            float _Add_0402b6fd1f4fad81a7706cc7fedf8ba8_Out_2;
            Unity_Add_float(_Split_3cafbe28397b318cba1225bf0e6f8880_R_1, _Property_5a81183edb8e818d876f56e4c20ad73c_Out_0, _Add_0402b6fd1f4fad81a7706cc7fedf8ba8_Out_2);
            float3 _Vector3_1397d5af1f57228db629a77eeccdfcd1_Out_0 = float3(_Add_0402b6fd1f4fad81a7706cc7fedf8ba8_Out_2, _Split_3cafbe28397b318cba1225bf0e6f8880_G_2, _Split_3cafbe28397b318cba1225bf0e6f8880_B_3);
            float3 _Normalize_f34bc19600a8a383956053b20d3bfdc0_Out_1;
            Unity_Normalize_float3(_Vector3_1397d5af1f57228db629a77eeccdfcd1_Out_0, _Normalize_f34bc19600a8a383956053b20d3bfdc0_Out_1);
            float4 _Property_6bcf6f441c043b8189591940db1682fa_Out_0 = _WorldSpaceLightPos0;
            float4 _Negate_5fe44f8bcb08318483e796e4c1b30944_Out_1;
            Unity_Negate_float4(_Property_6bcf6f441c043b8189591940db1682fa_Out_0, _Negate_5fe44f8bcb08318483e796e4c1b30944_Out_1);
            float _Distance_bfb26dfd6a57e7858842c6992040534d_Out_2;
            Unity_Distance_float3(_Normalize_f34bc19600a8a383956053b20d3bfdc0_Out_1, (_Negate_5fe44f8bcb08318483e796e4c1b30944_Out_1.xyz), _Distance_bfb26dfd6a57e7858842c6992040534d_Out_2);
            float _Property_6fd2a3003ecb238fbb8d1a382079f268_Out_0 = Vector1_166C34A9;
            float _Divide_e6c93f950c35f981bcfb3c3944b34171_Out_2;
            Unity_Divide_float(_Distance_bfb26dfd6a57e7858842c6992040534d_Out_2, _Property_6fd2a3003ecb238fbb8d1a382079f268_Out_0, _Divide_e6c93f950c35f981bcfb3c3944b34171_Out_2);
            float _OneMinus_4b48cc99607d348795d464ca806441b1_Out_1;
            Unity_OneMinus_float(_Divide_e6c93f950c35f981bcfb3c3944b34171_Out_2, _OneMinus_4b48cc99607d348795d464ca806441b1_Out_1);
            float _Multiply_5dbdfc53361dc482ae6537bc67fb289e_Out_2;
            Unity_Multiply_float_float(_OneMinus_4b48cc99607d348795d464ca806441b1_Out_1, 50, _Multiply_5dbdfc53361dc482ae6537bc67fb289e_Out_2);
            float _Saturate_0fc96c5211198d8599b0059ddcbde563_Out_1;
            Unity_Saturate_float(_Multiply_5dbdfc53361dc482ae6537bc67fb289e_Out_2, _Saturate_0fc96c5211198d8599b0059ddcbde563_Out_1);
            float _Subtract_d77979008ac9658d888fd0e83a2e069b_Out_2;
            Unity_Subtract_float(_Saturate_db3ab469449ce689a769d01ff2d036e9_Out_1, _Saturate_0fc96c5211198d8599b0059ddcbde563_Out_1, _Subtract_d77979008ac9658d888fd0e83a2e069b_Out_2);
            float _Saturate_3c36af90f61c128b8f1ad55dca7ff322_Out_1;
            Unity_Saturate_float(_Subtract_d77979008ac9658d888fd0e83a2e069b_Out_2, _Saturate_3c36af90f61c128b8f1ad55dca7ff322_Out_1);
            float _Add_e7ac88e4962b11828b5e87b11b5436fd_Out_2;
            Unity_Add_float(_Saturate_c4be325dac914182ab77534042191372_Out_1, _Saturate_3c36af90f61c128b8f1ad55dca7ff322_Out_1, _Add_e7ac88e4962b11828b5e87b11b5436fd_Out_2);
            float _Split_90ab6a881a5c7488a68aebfcb8d951ea_R_1 = IN.AbsoluteWorldSpacePosition[0];
            float _Split_90ab6a881a5c7488a68aebfcb8d951ea_G_2 = IN.AbsoluteWorldSpacePosition[1];
            float _Split_90ab6a881a5c7488a68aebfcb8d951ea_B_3 = IN.AbsoluteWorldSpacePosition[2];
            float _Split_90ab6a881a5c7488a68aebfcb8d951ea_A_4 = 0;
            float _Saturate_ba8da1149d014786aae546b8dfa1c99b_Out_1;
            Unity_Saturate_float(_Split_90ab6a881a5c7488a68aebfcb8d951ea_G_2, _Saturate_ba8da1149d014786aae546b8dfa1c99b_Out_1);
            float _Multiply_5c1b8e5b5fb9e289a5a0db7f732c7215_Out_2;
            Unity_Multiply_float_float(10, _Saturate_ba8da1149d014786aae546b8dfa1c99b_Out_1, _Multiply_5c1b8e5b5fb9e289a5a0db7f732c7215_Out_2);
            float _Multiply_38b3ce781b4649b4b1178af45e44aeb9_Out_2;
            Unity_Multiply_float_float(_Add_e7ac88e4962b11828b5e87b11b5436fd_Out_2, _Multiply_5c1b8e5b5fb9e289a5a0db7f732c7215_Out_2, _Multiply_38b3ce781b4649b4b1178af45e44aeb9_Out_2);
            float4 _Property_449a7bd62271b58595e19c65e5edd0cf_Out_0 = _WorldSpaceLightPos0;
            float _Split_4c1596dbed67828bba6c8b8fa23c0690_R_1 = _Property_449a7bd62271b58595e19c65e5edd0cf_Out_0[0];
            float _Split_4c1596dbed67828bba6c8b8fa23c0690_G_2 = _Property_449a7bd62271b58595e19c65e5edd0cf_Out_0[1];
            float _Split_4c1596dbed67828bba6c8b8fa23c0690_B_3 = _Property_449a7bd62271b58595e19c65e5edd0cf_Out_0[2];
            float _Split_4c1596dbed67828bba6c8b8fa23c0690_A_4 = _Property_449a7bd62271b58595e19c65e5edd0cf_Out_0[3];
            float _Saturate_2e92d86ef607bf86997f0ad5ecec839b_Out_1;
            Unity_Saturate_float(_Split_4c1596dbed67828bba6c8b8fa23c0690_G_2, _Saturate_2e92d86ef607bf86997f0ad5ecec839b_Out_1);
            float _OneMinus_9b7918c50c25d38ab6ce3a9e3e7380ff_Out_1;
            Unity_OneMinus_float(_Saturate_2e92d86ef607bf86997f0ad5ecec839b_Out_1, _OneMinus_9b7918c50c25d38ab6ce3a9e3e7380ff_Out_1);
            float _Add_0e5748443be65d8ca545ade667d9cb81_Out_2;
            Unity_Add_float(_Split_4c1596dbed67828bba6c8b8fa23c0690_G_2, 0.5, _Add_0e5748443be65d8ca545ade667d9cb81_Out_2);
            float _Multiply_1415b3f0c08fa784af8c430a80f7777b_Out_2;
            Unity_Multiply_float_float(_OneMinus_9b7918c50c25d38ab6ce3a9e3e7380ff_Out_1, _Add_0e5748443be65d8ca545ade667d9cb81_Out_2, _Multiply_1415b3f0c08fa784af8c430a80f7777b_Out_2);
            float4 _UV_9d087471db9ffe84b5a1c7b7becb55e6_Out_0 = IN.uv0;
            float _Split_1b8c0959e329b385967d3d932dac9901_R_1 = _UV_9d087471db9ffe84b5a1c7b7becb55e6_Out_0[0];
            float _Split_1b8c0959e329b385967d3d932dac9901_G_2 = _UV_9d087471db9ffe84b5a1c7b7becb55e6_Out_0[1];
            float _Split_1b8c0959e329b385967d3d932dac9901_B_3 = _UV_9d087471db9ffe84b5a1c7b7becb55e6_Out_0[2];
            float _Split_1b8c0959e329b385967d3d932dac9901_A_4 = _UV_9d087471db9ffe84b5a1c7b7becb55e6_Out_0[3];
            float _Property_a593d65b892d4f8d8a57e7bae635b513_Out_0 = Vector1_3B7A233E;
            float _Multiply_78651d206fc51e83ae75fd215e6f7c1f_Out_2;
            Unity_Multiply_float_float(_Split_1b8c0959e329b385967d3d932dac9901_G_2, _Property_a593d65b892d4f8d8a57e7bae635b513_Out_0, _Multiply_78651d206fc51e83ae75fd215e6f7c1f_Out_2);
            float _Absolute_fcd0d3d2a50a5b82b1b195c030e1f887_Out_1;
            Unity_Absolute_float(_Multiply_78651d206fc51e83ae75fd215e6f7c1f_Out_2, _Absolute_fcd0d3d2a50a5b82b1b195c030e1f887_Out_1);
            float _OneMinus_47a3c0d16657e880b27eeb5a6cb27342_Out_1;
            Unity_OneMinus_float(_Absolute_fcd0d3d2a50a5b82b1b195c030e1f887_Out_1, _OneMinus_47a3c0d16657e880b27eeb5a6cb27342_Out_1);
            float _Saturate_2c2df04886cb7585958ba86fb28b7ca0_Out_1;
            Unity_Saturate_float(_OneMinus_47a3c0d16657e880b27eeb5a6cb27342_Out_1, _Saturate_2c2df04886cb7585958ba86fb28b7ca0_Out_1);
            float _Property_beb1e8cc9dae3486951f5e0e21663d89_Out_0 = Vector1_D80769F7;
            float _Add_7e6ba6ac8f954c89bf788a8b0f8ee011_Out_2;
            Unity_Add_float(_Split_1b8c0959e329b385967d3d932dac9901_G_2, _Property_beb1e8cc9dae3486951f5e0e21663d89_Out_0, _Add_7e6ba6ac8f954c89bf788a8b0f8ee011_Out_2);
            float _Multiply_8dd509e1d84dce8d843ccbf5cc46aa72_Out_2;
            Unity_Multiply_float_float(_Saturate_2c2df04886cb7585958ba86fb28b7ca0_Out_1, _Add_7e6ba6ac8f954c89bf788a8b0f8ee011_Out_2, _Multiply_8dd509e1d84dce8d843ccbf5cc46aa72_Out_2);
            float _Multiply_59bb5eb1228a6c88b9785edcb4ee802a_Out_2;
            Unity_Multiply_float_float(_Multiply_1415b3f0c08fa784af8c430a80f7777b_Out_2, _Multiply_8dd509e1d84dce8d843ccbf5cc46aa72_Out_2, _Multiply_59bb5eb1228a6c88b9785edcb4ee802a_Out_2);
            float _Saturate_3eebdc0a9ddf808db1b9136fde7711cb_Out_1;
            Unity_Saturate_float(_Multiply_59bb5eb1228a6c88b9785edcb4ee802a_Out_2, _Saturate_3eebdc0a9ddf808db1b9136fde7711cb_Out_1);
            float4 _Property_759cf5dffca2ad8bb5cf3adca8685fbc_Out_0 = Color_1EB49FED;
            float4 _Multiply_4df21b54919e8b8aa5be3e130cbd7f83_Out_2;
            Unity_Multiply_float4_float4((_Saturate_3eebdc0a9ddf808db1b9136fde7711cb_Out_1.xxxx), _Property_759cf5dffca2ad8bb5cf3adca8685fbc_Out_0, _Multiply_4df21b54919e8b8aa5be3e130cbd7f83_Out_2);
            float _OneMinus_0cddf1e404bc3a83af7197c852ff6981_Out_1;
            Unity_OneMinus_float(_Saturate_db3ab469449ce689a769d01ff2d036e9_Out_1, _OneMinus_0cddf1e404bc3a83af7197c852ff6981_Out_1);
            float4 _Property_2bf3de1a71d24b3398a6cd81d58e793c_Out_0 = _WorldSpaceLightPos0;
            float _Split_ec432a49e18b466e8faed4ee9eff6dcd_R_1 = _Property_2bf3de1a71d24b3398a6cd81d58e793c_Out_0[0];
            float _Split_ec432a49e18b466e8faed4ee9eff6dcd_G_2 = _Property_2bf3de1a71d24b3398a6cd81d58e793c_Out_0[1];
            float _Split_ec432a49e18b466e8faed4ee9eff6dcd_B_3 = _Property_2bf3de1a71d24b3398a6cd81d58e793c_Out_0[2];
            float _Split_ec432a49e18b466e8faed4ee9eff6dcd_A_4 = _Property_2bf3de1a71d24b3398a6cd81d58e793c_Out_0[3];
            float _Multiply_5073197e5f6141fca0c241c8dbdbd38b_Out_2;
            Unity_Multiply_float_float(_Split_ec432a49e18b466e8faed4ee9eff6dcd_G_2, 5, _Multiply_5073197e5f6141fca0c241c8dbdbd38b_Out_2);
            float _Saturate_94abf88d7985487a86d5553af9108b51_Out_1;
            Unity_Saturate_float(_Multiply_5073197e5f6141fca0c241c8dbdbd38b_Out_2, _Saturate_94abf88d7985487a86d5553af9108b51_Out_1);
            float _OneMinus_5e2154c8eca34e60bad8e2a41f8ca3b1_Out_1;
            Unity_OneMinus_float(_Saturate_94abf88d7985487a86d5553af9108b51_Out_1, _OneMinus_5e2154c8eca34e60bad8e2a41f8ca3b1_Out_1);
            float3 _Normalize_102866732eac4470b5a5548826e2896d_Out_1;
            Unity_Normalize_float3(IN.WorldSpacePosition, _Normalize_102866732eac4470b5a5548826e2896d_Out_1);
            float _Split_9ed555aa0a8b4a9093d977291669120a_R_1 = IN.WorldSpacePosition[0];
            float _Split_9ed555aa0a8b4a9093d977291669120a_G_2 = IN.WorldSpacePosition[1];
            float _Split_9ed555aa0a8b4a9093d977291669120a_B_3 = IN.WorldSpacePosition[2];
            float _Split_9ed555aa0a8b4a9093d977291669120a_A_4 = 0;
            float2 _Vector2_a3077e429ece419ca2d94bd1ba3ba29f_Out_0 = float2(_Split_9ed555aa0a8b4a9093d977291669120a_R_1, _Split_9ed555aa0a8b4a9093d977291669120a_B_3);
            float2 _Divide_d993fd29798a4d829af311ec5e0fc4ac_Out_2;
            Unity_Divide_float2(_Vector2_a3077e429ece419ca2d94bd1ba3ba29f_Out_0, (_Split_9ed555aa0a8b4a9093d977291669120a_G_2.xx), _Divide_d993fd29798a4d829af311ec5e0fc4ac_Out_2);
            float2 _TilingAndOffset_5178df25d09b4f74b0a9b1d835dbdd09_Out_3;
            Unity_TilingAndOffset_float((_Normalize_102866732eac4470b5a5548826e2896d_Out_1.xy), float2 (1, 1), _Divide_d993fd29798a4d829af311ec5e0fc4ac_Out_2, _TilingAndOffset_5178df25d09b4f74b0a9b1d835dbdd09_Out_3);
            float _Property_9184c92d6f0b4d4b9b0a66dc68ee79f8_Out_0 = _etoiles_rotations_1;
            float _Property_f596ae4aafa44338892e6e4f7ad574a3_Out_0 = _etoiles_densite;
            float _Voronoi_a2516c10487f4466b69bafd33f483916_Out_3;
            float _Voronoi_a2516c10487f4466b69bafd33f483916_Cells_4;
            Unity_Voronoi_LegacySine_float(_TilingAndOffset_5178df25d09b4f74b0a9b1d835dbdd09_Out_3, _Property_9184c92d6f0b4d4b9b0a66dc68ee79f8_Out_0, _Property_f596ae4aafa44338892e6e4f7ad574a3_Out_0, _Voronoi_a2516c10487f4466b69bafd33f483916_Out_3, _Voronoi_a2516c10487f4466b69bafd33f483916_Cells_4);
            float _Saturate_c8ccf88436fc48e59e98f3dbacaa15b4_Out_1;
            Unity_Saturate_float(_Voronoi_a2516c10487f4466b69bafd33f483916_Out_3, _Saturate_c8ccf88436fc48e59e98f3dbacaa15b4_Out_1);
            float _OneMinus_4c4814502b184131838346c18857c9d6_Out_1;
            Unity_OneMinus_float(_Saturate_c8ccf88436fc48e59e98f3dbacaa15b4_Out_1, _OneMinus_4c4814502b184131838346c18857c9d6_Out_1);
            float _Property_e1c10d6949ef4384926fb5909626a70a_Out_0 = _etoiles_powa;
            float _Power_8355d5640d4e460c8114708722d9b1f4_Out_2;
            Unity_Power_float(_OneMinus_4c4814502b184131838346c18857c9d6_Out_1, _Property_e1c10d6949ef4384926fb5909626a70a_Out_0, _Power_8355d5640d4e460c8114708722d9b1f4_Out_2);
            float _Saturate_cecbc392b0d2463daa1c77fa3cb411c2_Out_1;
            Unity_Saturate_float(_Power_8355d5640d4e460c8114708722d9b1f4_Out_2, _Saturate_cecbc392b0d2463daa1c77fa3cb411c2_Out_1);
            float _Property_e97f030b15d84284b6f440ceb957a22c_Out_0 = _etoiles_intensite;
            float _Multiply_d9abcf011f9049a4a0c14d4b0b653c84_Out_2;
            Unity_Multiply_float_float(_Saturate_cecbc392b0d2463daa1c77fa3cb411c2_Out_1, _Property_e97f030b15d84284b6f440ceb957a22c_Out_0, _Multiply_d9abcf011f9049a4a0c14d4b0b653c84_Out_2);
            float _Multiply_86666c5a2dd34ba7bc7be5a423e6725c_Out_2;
            Unity_Multiply_float_float(_OneMinus_5e2154c8eca34e60bad8e2a41f8ca3b1_Out_1, _Multiply_d9abcf011f9049a4a0c14d4b0b653c84_Out_2, _Multiply_86666c5a2dd34ba7bc7be5a423e6725c_Out_2);
            float _Multiply_d90ade0e128c4c8f97ffc0aa93abfa95_Out_2;
            Unity_Multiply_float_float(_OneMinus_0cddf1e404bc3a83af7197c852ff6981_Out_1, _Multiply_86666c5a2dd34ba7bc7be5a423e6725c_Out_2, _Multiply_d90ade0e128c4c8f97ffc0aa93abfa95_Out_2);
            float _Multiply_e1794edd9caa3a829aab6edba6d11eeb_Out_2;
            Unity_Multiply_float_float(_Multiply_d90ade0e128c4c8f97ffc0aa93abfa95_Out_2, _Multiply_5c1b8e5b5fb9e289a5a0db7f732c7215_Out_2, _Multiply_e1794edd9caa3a829aab6edba6d11eeb_Out_2);
            float4 _Property_663923b1ab40ab899f15d6982444d339_Out_0 = Color_83CF459;
            float4 _Property_0cd7139856da4e89a7ec36232817d185_Out_0 = Color_D230EFA1;
            float4 _UV_0bb570b9da8e5e8fa558678aaa952c29_Out_0 = IN.uv0;
            float _Split_25141b555a0be18ab9be0d13c0a8a43b_R_1 = _UV_0bb570b9da8e5e8fa558678aaa952c29_Out_0[0];
            float _Split_25141b555a0be18ab9be0d13c0a8a43b_G_2 = _UV_0bb570b9da8e5e8fa558678aaa952c29_Out_0[1];
            float _Split_25141b555a0be18ab9be0d13c0a8a43b_B_3 = _UV_0bb570b9da8e5e8fa558678aaa952c29_Out_0[2];
            float _Split_25141b555a0be18ab9be0d13c0a8a43b_A_4 = _UV_0bb570b9da8e5e8fa558678aaa952c29_Out_0[3];
            float _Saturate_bfabc1f11c42b489bee718e18c61afe5_Out_1;
            Unity_Saturate_float(_Split_25141b555a0be18ab9be0d13c0a8a43b_G_2, _Saturate_bfabc1f11c42b489bee718e18c61afe5_Out_1);
            float4 _Lerp_927359091c0f3187be61d805ebfa921e_Out_3;
            Unity_Lerp_float4(_Property_663923b1ab40ab899f15d6982444d339_Out_0, _Property_0cd7139856da4e89a7ec36232817d185_Out_0, (_Saturate_bfabc1f11c42b489bee718e18c61afe5_Out_1.xxxx), _Lerp_927359091c0f3187be61d805ebfa921e_Out_3);
            float4 _Property_b295c6cd5b7eb4828c59a8d3791336c6_Out_0 = Color_68FD0CD8;
            float4 _Property_df133a02ae17e6828fd6c02b52a3ece8_Out_0 = Color_BE31CDF2;
            float4 _UV_63f2eedaa7d7a186838a7d27ff29877b_Out_0 = IN.uv0;
            float _Split_8ab9d2401a966d8d9b19fa59d83c3785_R_1 = _UV_63f2eedaa7d7a186838a7d27ff29877b_Out_0[0];
            float _Split_8ab9d2401a966d8d9b19fa59d83c3785_G_2 = _UV_63f2eedaa7d7a186838a7d27ff29877b_Out_0[1];
            float _Split_8ab9d2401a966d8d9b19fa59d83c3785_B_3 = _UV_63f2eedaa7d7a186838a7d27ff29877b_Out_0[2];
            float _Split_8ab9d2401a966d8d9b19fa59d83c3785_A_4 = _UV_63f2eedaa7d7a186838a7d27ff29877b_Out_0[3];
            float _Saturate_5a4aa969c530cb89b3f45769171e89cc_Out_1;
            Unity_Saturate_float(_Split_8ab9d2401a966d8d9b19fa59d83c3785_G_2, _Saturate_5a4aa969c530cb89b3f45769171e89cc_Out_1);
            float4 _Lerp_b0cc5cebd9f98e85942af72261fe877a_Out_3;
            Unity_Lerp_float4(_Property_b295c6cd5b7eb4828c59a8d3791336c6_Out_0, _Property_df133a02ae17e6828fd6c02b52a3ece8_Out_0, (_Saturate_5a4aa969c530cb89b3f45769171e89cc_Out_1.xxxx), _Lerp_b0cc5cebd9f98e85942af72261fe877a_Out_3);
            float4 _Property_a16eceb332aa738f983a9252f2eb2a65_Out_0 = _WorldSpaceLightPos0;
            float _Split_ef8cbd1ee3991d8497c3a12f8238a404_R_1 = _Property_a16eceb332aa738f983a9252f2eb2a65_Out_0[0];
            float _Split_ef8cbd1ee3991d8497c3a12f8238a404_G_2 = _Property_a16eceb332aa738f983a9252f2eb2a65_Out_0[1];
            float _Split_ef8cbd1ee3991d8497c3a12f8238a404_B_3 = _Property_a16eceb332aa738f983a9252f2eb2a65_Out_0[2];
            float _Split_ef8cbd1ee3991d8497c3a12f8238a404_A_4 = _Property_a16eceb332aa738f983a9252f2eb2a65_Out_0[3];
            float _Saturate_2501276a5ef6c586bcc6332699716cb3_Out_1;
            Unity_Saturate_float(_Split_ef8cbd1ee3991d8497c3a12f8238a404_G_2, _Saturate_2501276a5ef6c586bcc6332699716cb3_Out_1);
            float4 _Lerp_de8806d8b42fec858f0290bb682ea8ef_Out_3;
            Unity_Lerp_float4(_Lerp_927359091c0f3187be61d805ebfa921e_Out_3, _Lerp_b0cc5cebd9f98e85942af72261fe877a_Out_3, (_Saturate_2501276a5ef6c586bcc6332699716cb3_Out_1.xxxx), _Lerp_de8806d8b42fec858f0290bb682ea8ef_Out_3);
            float4 _Add_af1fdd6f0ea0ba8796643c403a93eed1_Out_2;
            Unity_Add_float4((_Multiply_e1794edd9caa3a829aab6edba6d11eeb_Out_2.xxxx), _Lerp_de8806d8b42fec858f0290bb682ea8ef_Out_3, _Add_af1fdd6f0ea0ba8796643c403a93eed1_Out_2);
            float4 _Add_aebf5f14a960928d877890408fcec2f5_Out_2;
            Unity_Add_float4(_Multiply_4df21b54919e8b8aa5be3e130cbd7f83_Out_2, _Add_af1fdd6f0ea0ba8796643c403a93eed1_Out_2, _Add_aebf5f14a960928d877890408fcec2f5_Out_2);
            float4 _Property_fee7bae45ed045ea9d47a6cd5bce27f3_Out_0 = _WorldSpaceLightPos0;
            float4 _Negate_677526b0b8c1409d8500e85e57860856_Out_1;
            Unity_Negate_float4(_Property_fee7bae45ed045ea9d47a6cd5bce27f3_Out_0, _Negate_677526b0b8c1409d8500e85e57860856_Out_1);
            float _Split_e54d32944af342939614296bb3882552_R_1 = _Negate_677526b0b8c1409d8500e85e57860856_Out_1[0];
            float _Split_e54d32944af342939614296bb3882552_G_2 = _Negate_677526b0b8c1409d8500e85e57860856_Out_1[1];
            float _Split_e54d32944af342939614296bb3882552_B_3 = _Negate_677526b0b8c1409d8500e85e57860856_Out_1[2];
            float _Split_e54d32944af342939614296bb3882552_A_4 = _Negate_677526b0b8c1409d8500e85e57860856_Out_1[3];
            float _Multiply_492c22c902ee4688afa2c4922c35479f_Out_2;
            Unity_Multiply_float_float(_Split_e54d32944af342939614296bb3882552_G_2, 5, _Multiply_492c22c902ee4688afa2c4922c35479f_Out_2);
            float _Saturate_5308504522434d118e5f260f191d71f3_Out_1;
            Unity_Saturate_float(_Multiply_492c22c902ee4688afa2c4922c35479f_Out_2, _Saturate_5308504522434d118e5f260f191d71f3_Out_1);
            float _OneMinus_ac438ce7d2f84670838c0c3ffbe6af40_Out_1;
            Unity_OneMinus_float(_Saturate_5308504522434d118e5f260f191d71f3_Out_1, _OneMinus_ac438ce7d2f84670838c0c3ffbe6af40_Out_1);
            UnityTextureCube _Property_5671c5fd8fee45d1a9f593037d0829f7_Out_0 = UnityBuildTextureCubeStruct(_nuageJour);
            float _Property_61bf2955f660441cb83bbfeb43cc13be_Out_0 = _test_rotate;
            float _Multiply_ac392346f509442a9a4ad630d61191a9_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_61bf2955f660441cb83bbfeb43cc13be_Out_0, _Multiply_ac392346f509442a9a4ad630d61191a9_Out_2);
            float3 _RotateAboutAxis_033389c5653f4fa6b0b570f3e85178cb_Out_3;
            Unity_Rotate_About_Axis_Degrees_float(IN.WorldSpaceNormal, float3 (0, 1, 0), _Multiply_ac392346f509442a9a4ad630d61191a9_Out_2, _RotateAboutAxis_033389c5653f4fa6b0b570f3e85178cb_Out_3);
            float4 _SampleReflectedCubemap_1a970f2d39af42998ef1960dc5f96326_Out_0 = SAMPLE_TEXTURECUBE_LOD(_Property_5671c5fd8fee45d1a9f593037d0829f7_Out_0.tex, _Property_5671c5fd8fee45d1a9f593037d0829f7_Out_0.samplerstate, reflect(-IN.ObjectSpaceViewDirection, _RotateAboutAxis_033389c5653f4fa6b0b570f3e85178cb_Out_3), 0);
            float4 _Saturate_2eab6b72e92b4b7797627c9b0c3b1c7c_Out_1;
            Unity_Saturate_float4(_SampleReflectedCubemap_1a970f2d39af42998ef1960dc5f96326_Out_0, _Saturate_2eab6b72e92b4b7797627c9b0c3b1c7c_Out_1);
            float4 _Multiply_1a10b6b7a3f44d5ca32b2eb4e37c09cf_Out_2;
            Unity_Multiply_float4_float4((_OneMinus_ac438ce7d2f84670838c0c3ffbe6af40_Out_1.xxxx), _Saturate_2eab6b72e92b4b7797627c9b0c3b1c7c_Out_1, _Multiply_1a10b6b7a3f44d5ca32b2eb4e37c09cf_Out_2);
            float4 _Property_fa1fa549c49942e99cd5dc4cda656720_Out_0 = _WorldSpaceLightPos0;
            float _Split_d89b5fce1cd44e05aa505df30c1117a8_R_1 = _Property_fa1fa549c49942e99cd5dc4cda656720_Out_0[0];
            float _Split_d89b5fce1cd44e05aa505df30c1117a8_G_2 = _Property_fa1fa549c49942e99cd5dc4cda656720_Out_0[1];
            float _Split_d89b5fce1cd44e05aa505df30c1117a8_B_3 = _Property_fa1fa549c49942e99cd5dc4cda656720_Out_0[2];
            float _Split_d89b5fce1cd44e05aa505df30c1117a8_A_4 = _Property_fa1fa549c49942e99cd5dc4cda656720_Out_0[3];
            float _Multiply_80f6e1615e394f428a4521a5f04da13f_Out_2;
            Unity_Multiply_float_float(_Split_d89b5fce1cd44e05aa505df30c1117a8_G_2, 5, _Multiply_80f6e1615e394f428a4521a5f04da13f_Out_2);
            float _Saturate_65009cf085f942eba890548ccb7449a5_Out_1;
            Unity_Saturate_float(_Multiply_80f6e1615e394f428a4521a5f04da13f_Out_2, _Saturate_65009cf085f942eba890548ccb7449a5_Out_1);
            float _OneMinus_c3d1302e3d914e96a5388e43e2a98e76_Out_1;
            Unity_OneMinus_float(_Saturate_65009cf085f942eba890548ccb7449a5_Out_1, _OneMinus_c3d1302e3d914e96a5388e43e2a98e76_Out_1);
            UnityTextureCube _Property_21752d27a8e949f9bbe6d81b0df7b885_Out_0 = UnityBuildTextureCubeStruct(_NuagesNuit);
            float4 _SampleReflectedCubemap_14c463d4a4c24054aa713fb7c05a55da_Out_0 = SAMPLE_TEXTURECUBE_LOD(_Property_21752d27a8e949f9bbe6d81b0df7b885_Out_0.tex, _Property_21752d27a8e949f9bbe6d81b0df7b885_Out_0.samplerstate, reflect(-IN.ObjectSpaceViewDirection, _RotateAboutAxis_033389c5653f4fa6b0b570f3e85178cb_Out_3), 0);
            float4 _Saturate_2ef4afa901ad491b9ef6973d67c2b14f_Out_1;
            Unity_Saturate_float4(_SampleReflectedCubemap_14c463d4a4c24054aa713fb7c05a55da_Out_0, _Saturate_2ef4afa901ad491b9ef6973d67c2b14f_Out_1);
            float4 _Multiply_782d24bb9913401b93214677290fcc21_Out_2;
            Unity_Multiply_float4_float4((_OneMinus_c3d1302e3d914e96a5388e43e2a98e76_Out_1.xxxx), _Saturate_2ef4afa901ad491b9ef6973d67c2b14f_Out_1, _Multiply_782d24bb9913401b93214677290fcc21_Out_2);
            float4 _Add_1c7edc79043a46c78fda19f88574e8c3_Out_2;
            Unity_Add_float4(_Multiply_1a10b6b7a3f44d5ca32b2eb4e37c09cf_Out_2, _Multiply_782d24bb9913401b93214677290fcc21_Out_2, _Add_1c7edc79043a46c78fda19f88574e8c3_Out_2);
            float4 _Add_c00abf15fbfa4660abadf59765bc2902_Out_2;
            Unity_Add_float4(_Add_aebf5f14a960928d877890408fcec2f5_Out_2, _Add_1c7edc79043a46c78fda19f88574e8c3_Out_2, _Add_c00abf15fbfa4660abadf59765bc2902_Out_2);
            float4 _Add_c1ddb53ed299457589a80262aef9e4a2_Out_2;
            Unity_Add_float4((_Multiply_38b3ce781b4649b4b1178af45e44aeb9_Out_2.xxxx), _Add_c00abf15fbfa4660abadf59765bc2902_Out_2, _Add_c1ddb53ed299457589a80262aef9e4a2_Out_2);
            surface.BaseColor = (_Add_c1ddb53ed299457589a80262aef9e4a2_Out_2.xyz);
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
            // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            float3 unnormalizedNormalWS = input.normalWS;
            const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        
        
            output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;      // we want a unit length Normal Vector node in shader graph
        
        
            output.WorldSpaceViewDirection = GetWorldSpaceNormalizeViewDir(input.positionWS);
            output.ObjectSpaceViewDirection = TransformWorldToObjectDir(output.WorldSpaceViewDirection);
            output.WorldSpacePosition = input.positionWS;
            output.AbsoluteWorldSpacePosition = GetAbsolutePositionWS(input.positionWS);
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormalsOnly"
            Tags
            {
                "LightMode" = "DepthNormalsOnly"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma target 3.5 DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 tangentWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float Vector1_2CC6A532;
        float Vector1_166C34A9;
        float Vector1_6F3D4927;
        float4 Color_BE31CDF2;
        float4 Color_68FD0CD8;
        float4 Color_D230EFA1;
        float4 Color_83CF459;
        float4 Color_1EB49FED;
        float Vector1_3B7A233E;
        float Vector1_D80769F7;
        float _test_rotate;
        float _etoiles_intensite;
        float _etoiles_densite;
        float _etoiles_powa;
        float _etoiles_rotations_1;
        float _etoiles_rotations;
        float2 _Vector2;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        float4 _WorldSpaceLightPos0;
        TEXTURECUBE(_nuageJour);
        SAMPLER(sampler_nuageJour);
        TEXTURECUBE(_NuagesNuit);
        SAMPLER(sampler_NuagesNuit);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma target 3.5 DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float Vector1_2CC6A532;
        float Vector1_166C34A9;
        float Vector1_6F3D4927;
        float4 Color_BE31CDF2;
        float4 Color_68FD0CD8;
        float4 Color_D230EFA1;
        float4 Color_83CF459;
        float4 Color_1EB49FED;
        float Vector1_3B7A233E;
        float Vector1_D80769F7;
        float _test_rotate;
        float _etoiles_intensite;
        float _etoiles_densite;
        float _etoiles_powa;
        float _etoiles_rotations_1;
        float _etoiles_rotations;
        float2 _Vector2;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        float4 _WorldSpaceLightPos0;
        TEXTURECUBE(_nuageJour);
        SAMPLER(sampler_nuageJour);
        TEXTURECUBE(_NuagesNuit);
        SAMPLER(sampler_NuagesNuit);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma target 3.5 DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float Vector1_2CC6A532;
        float Vector1_166C34A9;
        float Vector1_6F3D4927;
        float4 Color_BE31CDF2;
        float4 Color_68FD0CD8;
        float4 Color_D230EFA1;
        float4 Color_83CF459;
        float4 Color_1EB49FED;
        float Vector1_3B7A233E;
        float Vector1_D80769F7;
        float _test_rotate;
        float _etoiles_intensite;
        float _etoiles_densite;
        float _etoiles_powa;
        float _etoiles_rotations_1;
        float _etoiles_rotations;
        float2 _Vector2;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        float4 _WorldSpaceLightPos0;
        TEXTURECUBE(_nuageJour);
        SAMPLER(sampler_nuageJour);
        TEXTURECUBE(_NuagesNuit);
        SAMPLER(sampler_NuagesNuit);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma target 3.5 DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float Vector1_2CC6A532;
        float Vector1_166C34A9;
        float Vector1_6F3D4927;
        float4 Color_BE31CDF2;
        float4 Color_68FD0CD8;
        float4 Color_D230EFA1;
        float4 Color_83CF459;
        float4 Color_1EB49FED;
        float Vector1_3B7A233E;
        float Vector1_D80769F7;
        float _test_rotate;
        float _etoiles_intensite;
        float _etoiles_densite;
        float _etoiles_powa;
        float _etoiles_rotations_1;
        float _etoiles_rotations;
        float2 _Vector2;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        float4 _WorldSpaceLightPos0;
        TEXTURECUBE(_nuageJour);
        SAMPLER(sampler_nuageJour);
        TEXTURECUBE(_NuagesNuit);
        SAMPLER(sampler_NuagesNuit);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}