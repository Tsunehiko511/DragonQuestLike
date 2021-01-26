using UnityEngine;

[ExecuteInEditMode]
public class Grayscale : MonoBehaviour
{
	[SerializeField] [Range(-1, 1)] public float m_offset = 0;

	private Material m_material;

	private void Awake()
	{
		var shader = Shader.Find("Hidden/Grayscale Post Effect");
		m_material = new Material(shader);

	}
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		m_material.SetFloat("_Offset", m_offset);

		Graphics.Blit(src, dest, m_material);
	}
}