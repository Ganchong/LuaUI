using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UGUIFrame
{
	/// <summary>
	/// 材质管理器
	/// </summary>
	public class MaterialManager{

		/// <summary>
		/// 获取shader的自定义注入方法
		/// </summary>
		public static Func<string,Shader> CustomGetShaderFunc;

		/// <summary>
		/// 获取Shader
		/// </summary>
		public static Shader GetShader(string ShaderName) {

			if (CustomGetShaderFunc != null) {
				return CustomGetShaderFunc(ShaderName);
			}

			Shader shader = Shader.Find(ShaderName);
			if (shader != null) {
				return shader;
			}
			return null;
		}

		public static Material CreateMaterial(string MatName, string ShaderName) {
			return CreateMaterial(MatName, GetShader(ShaderName));
		}

		public static Material CreateMaterial(string MatName, Shader shader) {
			if (shader != null) {
				Material m = new Material(shader);
				m.name = MatName;
				return m;
			}
			return null;
		}
	}
}
