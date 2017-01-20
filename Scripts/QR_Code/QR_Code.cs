using UnityEngine;
using System.Collections;
using ZXing;
using ZXing.QrCode;

public class QR_Code: MonoBehaviour 
{
	public Texture2D encoded;
	public string Lastresult;
	void Start () 
	{
		encoded = new Texture2D(256, 256);
		Lastresult = "http://www.baidu.com"; //自己的地址 ，测试用的谷歌
	} 
	private static Color32[] Encode(string textForEncoding, int width, int height)
	{
		var writer = new BarcodeWriter
		{
			Format = BarcodeFormat.QR_CODE,Options = new QrCodeEncodingOptions
			{
				Height = height,Width = width
			}
		};
		return writer.Write(textForEncoding);
	} 
	void Update () 
	{
		var textForEncoding = Lastresult;
		if (textForEncoding != null)
		{
			var color32 = Encode(textForEncoding, encoded.width, encoded.height);
			encoded.SetPixels32(color32);encoded.Apply();
		}
	} 
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(100, 100,256,256), encoded);
	} 

}